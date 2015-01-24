using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using NetGore.Db.ClassCreator.Properties;

namespace NetGore.Db.ClassCreator
{
    public abstract class DbClassGenerator : IDisposable
    {
        /// <summary>
        /// Name of the class used to store the column metadata.
        /// </summary>
        public const string ColumnMetadataClassName = "ColumnMetadata";

        /// <summary>
        /// Name of the CopyValuesFrom method in the generated code.
        /// </summary>
        public const string CopyValuesFromMethodName = "CopyValuesFrom";

        /// <summary>
        /// Name of the CopyValues method in the generated code.
        /// </summary>
        public const string CopyValuesMethodName = "CopyValues";

        /// <summary>
        /// Name of the dataReader when used in arguments in the generated code.
        /// </summary>
        public const string DataReaderName = "dataReader";

        /// <summary>
        /// Name of the _dbColumns field in the generated code.
        /// </summary>
        public const string DbColumnsField = "_dbColumns";

        /// <summary>
        /// Name of the _dbColumnsKeys field in the generated code.
        /// </summary>
        public const string DbColumnsKeysField = "_dbColumnsKeys";

        /// <summary>
        /// Name of the _dbColumnsNonKeys field in the generated code.
        /// </summary>
        public const string DbColumnsNonKeysField = "_dbColumnsNonKey";

        /// <summary>
        /// Name of the TryCopyValues method in the generated code.
        /// </summary>
        public const string TryCopyValuesMethodName = "TryCopyValues";

        /// <summary>
        /// Name given to all extension method's first parameter.
        /// </summary>
        const string _extensionParamName = "source";

        readonly List<ColumnCollection> _columnCollections = new List<ColumnCollection>();
        readonly List<CustomTypeMapping> _customTypes = new List<CustomTypeMapping>();

        /// <summary>
        /// Dictionary of the DataReader Read method names for a given Type.
        /// </summary>
        readonly Dictionary<Type, string> _dataReaderReadMethods = new Dictionary<Type, string>();

        readonly Dictionary<string, IEnumerable<DbColumnInfo>> _dbTables = new Dictionary<string, IEnumerable<DbColumnInfo>>();

        /// <summary>
        /// Using directives to add.
        /// </summary>
        readonly List<string> _usings = new List<string>();

        DbConnection _dbConnction;
        bool _isDbContentLoaded = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbClassGenerator"/> class.
        /// </summary>
        protected DbClassGenerator()
        {
            Formatter = new CSharpCodeFormatter();
            SetDataReaderReadMethod(typeof(float), "GetFloat");
        }

        /// <summary>
        /// Gets the DbConnection used to connect to the database.
        /// </summary>
        public DbConnection DbConnection
        {
            get { return _dbConnction; }
        }

        /// <summary>
        /// Gets or sets the CodeFormatter to use. Default is the CSharpCodeFormatter.
        /// </summary>
        public CodeFormatter Formatter { get; set; }

        /// <summary>
        /// Gets or sets an IEnumerable of the name of the tables to generate. If this value is null or empty,
        /// all tables will be generated. Otherwise, only tables defined in this collection will be generated.
        /// </summary>
        public IEnumerable<string> TablesToGenerate { get; set; }

        /// <summary>
        /// Adds a column to the column collection.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="keyType">Type of the key.</param>
        /// <param name="valueType">Type of the value.</param>
        /// <param name="tables">The tables.</param>
        /// <param name="columns">The columns.</param>
        public void AddColumnCollection(string name, Type keyType, Type valueType, IEnumerable<string> tables,
                                        IEnumerable<ColumnCollectionItem> columns)
        {
            if (!(keyType.IsEnum))
                throw new ArgumentException("Only Enums are supported for the keyType at the present.", "keyType");

            var columnCollection = new ColumnCollection(name, keyType, valueType, tables, columns);
            _columnCollections.Add(columnCollection);
        }

        /// <summary>
        /// Adds a column to the column collection.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="keyType">Type of the key.</param>
        /// <param name="valueType">Type of the value.</param>
        /// <param name="table">The table.</param>
        /// <param name="columns">The columns.</param>
        public void AddColumnCollection(string name, Type keyType, Type valueType, string table,
                                        IEnumerable<ColumnCollectionItem> columns)
        {
            if (!(keyType.IsEnum))
                throw new ArgumentException("Only Enums are supported for the keyType at the present.", "keyType");

            var columnCollection = new ColumnCollection(name, keyType, valueType, new string[] { table }, columns);
            _columnCollections.Add(columnCollection);
        }

        public void AddCustomType(Type type, string table, params string[] columns)
        {
            AddCustomType(type, new string[] { table }, columns);
        }

        public void AddCustomType(string type, string table, params string[] columns)
        {
            AddCustomType(type, new string[] { table }, columns);
        }

        public void AddCustomType(Type type, IEnumerable<string> tables, params string[] columns)
        {
            AddCustomType(Formatter.GetTypeString(type), tables, columns);
        }

        public void AddCustomType(string type, IEnumerable<string> tables, params string[] columns)
        {
            _customTypes.Add(new CustomTypeMapping(tables, columns, type));
        }

        /// <summary>
        /// Adds a Using directive to the generated code.
        /// </summary>
        /// <param name="namespaceName">Namespace to use.</param>
        public void AddUsing(string namespaceName)
        {
            if (!_usings.Contains(namespaceName, StringComparer.OrdinalIgnoreCase))
                _usings.Add(namespaceName);
        }

        /// <summary>
        /// Adds a Using directive to the generated code.
        /// </summary>
        /// <param name="namespaceNames">Namespaces to use.</param>
        public void AddUsing(IEnumerable<string> namespaceNames)
        {
            foreach (var n in namespaceNames)
            {
                AddUsing(n);
            }
        }

        protected virtual IEnumerable<GeneratedTableCode> CreateCode(string tableName, IEnumerable<DbColumnInfo> columns,
                                                                     string classNamespace, string interfaceNamespace)
        {
            columns = columns.OrderBy(x => x.Name);

            var cd = new DbClassData(tableName, columns, Formatter, _dataReaderReadMethods, _columnCollections, _customTypes);

            yield return
                new GeneratedTableCode(tableName, cd.ClassName, WrapCodeFile(CreateCodeForClass(cd), classNamespace, false),
                                       GeneratedCodeType.Class);
            yield return
                new GeneratedTableCode(tableName, cd.InterfaceName,
                                       WrapCodeFile(CreateCodeForInterface(cd), interfaceNamespace, true),
                                       GeneratedCodeType.Interface);

            yield return
                new GeneratedTableCode(tableName, cd.ExtensionClassName,
                                       WrapCodeFile(CreateCodeForExtensions(cd), classNamespace, false),
                                       GeneratedCodeType.ClassDbExtensions);
        }

        /// <summary>
        /// Creates the code for the class.
        /// </summary>
        /// <param name="cd">Class data.</param>
        /// <returns>The code for the class.</returns>
        protected virtual string CreateCodeForClass(DbClassData cd)
        {
            var sb = new StringBuilder(8192);

            sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateCode.ClassSummary, cd.TableName)));
            sb.AppendLine(Formatter.GetClass(cd.ClassName, MemberVisibilityLevel.Public, false, new string[] { cd.InterfaceName }));
            sb.AppendLine(Formatter.OpenBrace);
            {
                // Other Fields/Properties
                {
                    // All fields
                    var fieldNamesCode = Formatter.GetStringArrayCode(cd.Columns.Select(x => x.Name));
                    sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.ColumnArrayField));
                    sb.AppendLine(Formatter.GetField(DbColumnsField, typeof(string[]), MemberVisibilityLevel.Private,
                                                     fieldNamesCode, true, true));

                    sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.ColumnIEnumerableProperty));
                    sb.AppendLine(Formatter.GetProperty("DbColumns", typeof(IEnumerable<string>), typeof(IEnumerable<string>),
                                                        MemberVisibilityLevel.Public, null, DbColumnsField, false, true));
                }

                {
                    // Key fields
                    var keyFieldNamesCode =
                        Formatter.GetStringArrayCode(
                            cd.Columns.Where(x => x.KeyType == DbColumnKeyType.Primary).Select(x => x.Name));
                    sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.KeyColumnArrayField));
                    sb.AppendLine(Formatter.GetField(DbColumnsKeysField, typeof(string[]), MemberVisibilityLevel.Private,
                                                     keyFieldNamesCode, true, true));

                    sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.KeyColumnIEnumerableProperty));
                    sb.AppendLine(Formatter.GetProperty("DbKeyColumns", typeof(IEnumerable<string>), typeof(IEnumerable<string>),
                                                        MemberVisibilityLevel.Public, null, DbColumnsKeysField, false, true));
                }

                {
                    // Non-key fields
                    var nonKeyFieldNamesCode =
                        Formatter.GetStringArrayCode(
                            cd.Columns.Where(x => x.KeyType != DbColumnKeyType.Primary).Select(x => x.Name));
                    sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.NonKeyColumnArrayField));
                    sb.AppendLine(Formatter.GetField(DbColumnsNonKeysField, typeof(string[]), MemberVisibilityLevel.Private,
                                                     nonKeyFieldNamesCode, true, true));

                    sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.NonKeyColumnIEnumerableProperty));
                    sb.AppendLine(Formatter.GetProperty("DbNonKeyColumns", typeof(IEnumerable<string>),
                                                        typeof(IEnumerable<string>), MemberVisibilityLevel.Public, null,
                                                        DbColumnsNonKeysField, false, true));
                }

                {
                    // Collection fields
                    foreach (var coll in cd.ColumnCollections)
                    {
                        var privateFieldName = cd.GetPrivateName(coll) + "Columns";
                        var publicPropertyName = cd.GetPublicName(coll) + "Columns";

                        var currColl = coll;
                        var columnsInCollection = cd.Columns.Where(x => cd.GetCollectionForColumn(x) == currColl);
                        if (columnsInCollection.Count() == 0)
                            continue;

                        // Getter
                        var columnNamesCode = Formatter.GetStringArrayCode(columnsInCollection.Select(x => x.Name));
                        sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateCode.ColumnCollectionField, coll.Name)));
                        sb.AppendLine(Formatter.GetField(privateFieldName, typeof(string[]), MemberVisibilityLevel.Private,
                                                         columnNamesCode, true, true));

                        // Setter
                        sb.AppendLine(
                            Formatter.GetXmlComment(string.Format(Comments.CreateCode.ColumnCollectionProperty, coll.Name)));
                        sb.AppendLine(Formatter.GetProperty(publicPropertyName, typeof(IEnumerable<string>),
                                                            typeof(IEnumerable<string>), MemberVisibilityLevel.Public, null,
                                                            privateFieldName, false, true));

                        // Collection
                        sb.AppendLine(
                            Formatter.GetXmlComment(string.Format(Comments.CreateCode.ColumnCollectionValueProperty, coll.Name)));
                        var ikvpType = Formatter.GetIEnumerableKeyValuePair(coll.KeyType, coll.ValueType);
                        sb.AppendLine(Formatter.GetProperty(coll.CollectionPropertyName, ikvpType, ikvpType,
                                                            MemberVisibilityLevel.Public, null, cd.GetPrivateName(coll), false,
                                                            false));
                    }
                }

                sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.TableName));
                sb.AppendLine(Formatter.GetConstField("TableName", typeof(string), MemberVisibilityLevel.Public,
                                                      "\"" + cd.TableName + "\""));

                sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.ColumnCount));
                sb.AppendLine(Formatter.GetConstField("ColumnCount", typeof(int), MemberVisibilityLevel.Public,
                                                      cd.Columns.Count().ToString()));

                // Properties for the interface implementation
                sb.AppendLine(CreateFields(cd));

                // DeepCopy implementation
                sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.DeepCopySummary, Comments.CreateCode.DeepCopyReturn));
                sb.AppendLine(Formatter.GetMethodHeader("DeepCopy", MemberVisibilityLevel.Public, null, cd.InterfaceName, false,
                                                        false));
                sb.AppendLine(
                    Formatter.GetMethodBody("return new " + cd.ClassName + Formatter.OpenParameterString + "this" +
                                            Formatter.CloseParameterString + Formatter.EndOfLine));

                // Constructor (empty)
                sb.AppendLine(CreateConstructor(cd, string.Empty, false));

                // Constructor (full)
                var fullConstructorBody = FullConstructorMemberBody(cd);
                sb.AppendLine(CreateConstructor(cd, fullConstructorBody, true));

                // Constructor (self-referencing interface)
                var sriConstructorParams = new MethodParameter[] { new MethodParameter("source", cd.InterfaceName) };
                sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateCode.ConstructorSummary, cd.ClassName), null,
                                                      new KeyValuePair<string, string>("source",
                                                                                       string.Format(
                                                                                           Comments.CreateCode.
                                                                                               ConstructorInterfaceParameter,
                                                                                           cd.InterfaceName))));
                sb.AppendLine(Formatter.GetConstructorHeader(cd.ClassName, MemberVisibilityLevel.Public, sriConstructorParams));
                sb.AppendLine(Formatter.GetMethodBody(Formatter.GetCallMethod(CopyValuesFromMethodName, "source")));

                // Methods
                sb.AppendLine(CreateMethodCopyValuesToDict(cd));
                sb.AppendLine(CreateMethodCopyValuesFrom(cd));
                sb.AppendLine(CreateMethodGetValue(cd));
                sb.AppendLine(CreateMethodSetValue(cd));
                sb.AppendLine(CreateMethodGetColumnData(cd));

                // ConstEnumDictionary class
                foreach (var coll in cd.ColumnCollections)
                {
                    sb.AppendLine(cd.GetConstEnumDictonaryCode(coll));
                }
            }
            sb.AppendLine(Formatter.CloseBrace);

            return sb.ToString();
        }

        protected virtual GeneratedTableCode CreateCodeForColumnMetadata(string codeNamespace)
        {
            var code = Resources.ColumnMetadataCode;
            var sb = new StringBuilder(code.Length + 200);

            sb.AppendLine(Formatter.GetUsing("System"));

            sb.AppendLine(Formatter.GetNamespace(codeNamespace));
            sb.AppendLine(Formatter.OpenBrace);
            {
                sb.AppendLine(code);
            }
            sb.AppendLine(Formatter.CloseBrace);

            return new GeneratedTableCode(ColumnMetadataClassName, ColumnMetadataClassName, sb.ToString(),
                                          GeneratedCodeType.ColumnMetadata);
        }

        protected virtual string CreateCodeForExtensions(DbClassData cd)
        {
            var sb = new StringBuilder(8192);

            sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateCode.ExtensionClassSummary, cd.ClassName)));
            sb.AppendLine(Formatter.GetClass(cd.ExtensionClassName, MemberVisibilityLevel.Public, true, null));
            sb.AppendLine(Formatter.OpenBrace);
            {
                // Extension methods
                sb.AppendLine(CreateMethodCopyValuesToDbParameterValues(cd));
                sb.AppendLine(CreateMethodReadValues(cd));
                sb.AppendLine(CreateMethodTryReadValues(cd));
                sb.AppendLine(CreateMethodTryCopyValuesToDbParameterValues(cd));
            }
            sb.AppendLine(Formatter.CloseBrace);

            return sb.ToString();
        }

        /// <summary>
        /// Creates the code for the interface.
        /// </summary>
        /// <param name="cd">Class data.</param>
        /// <returns>The class code for the interface.</returns>
        protected virtual string CreateCodeForInterface(DbClassData cd)
        {
            var sb = new StringBuilder(2048);

            sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateCode.InterfaceSummary, cd.TableName)));
            sb.AppendLine(Formatter.GetInterface(cd.InterfaceName, MemberVisibilityLevel.Public));
            sb.AppendLine(Formatter.OpenBrace);
            {
                // DeepCopy
                sb.AppendLine(Formatter.GetXmlComment(Comments.CreateCode.DeepCopySummary, Comments.CreateCode.DeepCopyReturn));
                sb.AppendLine(Formatter.GetInterfaceMethod("DeepCopy", cd.InterfaceName));

                // Columns
                var addedCollections = new List<ColumnCollection>();
                foreach (var column in cd.Columns)
                {
                    ColumnCollectionItem collItem;
                    var coll = cd.GetCollectionForColumn(column, out collItem);

                    if (coll == null)
                    {
                        // No collection - just add the property like normal
                        sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateCode.InterfaceGetProperty, column.Name)));
                        sb.AppendLine(Formatter.GetInterfaceProperty(cd.GetPublicName(column), cd.GetExternalType(column), false));
                    }
                    else if (!addedCollections.Contains(coll))
                    {
                        // Has a collection - only add the code if the collection hasn't been added yet
                        addedCollections.Add(coll);
                        var name = cd.GetPublicName(coll);
                        var keyParameter = new MethodParameter("key", coll.KeyType, Formatter);

                        // Getter
                        sb.AppendLine(
                            Formatter.GetXmlComment(string.Format(Comments.CreateCode.InterfaceCollectionGetter, coll.Name),
                                                    Comments.CreateCode.InterfaceCollectionReturns,
                                                    new KeyValuePair<string, string>("key",
                                                                                     Comments.CreateCode.
                                                                                         InterfaceCollectionParamKey)));
                        sb.AppendLine(Formatter.GetInterfaceMethod("Get" + name, coll.ValueType, keyParameter));

                        // Collection
                        sb.AppendLine(
                            Formatter.GetXmlComment(string.Format(Comments.CreateCode.ColumnCollectionValueProperty, coll.Name)));
                        sb.AppendLine(Formatter.GetInterfaceProperty(coll.CollectionPropertyName,
                                                                     Formatter.GetIEnumerableKeyValuePair(coll.KeyType,
                                                                                                          coll.ValueType), false));
                    }
                }
            }
            sb.AppendLine(Formatter.CloseBrace);

            return sb.ToString();
        }

        protected virtual string CreateConstructor(DbClassData cd, string code, bool addParameters)
        {
            MethodParameter[] parameters;
            if (addParameters)
                parameters = GetConstructorParameters(cd);
            else
                parameters = MethodParameter.Empty;

            var cParams = new List<KeyValuePair<string, string>>(Math.Max(1, parameters.Length));
            foreach (var p in parameters)
            {
                var kvp = new KeyValuePair<string, string>(p.Name, Comments.CreateConstructor.Parameter);
                cParams.Add(kvp);
            }

            var sb = new StringBuilder(2048);
            sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateCode.ConstructorSummary, cd.ClassName), null,
                                                  cParams.ToArray()));
            sb.AppendLine(Formatter.GetConstructorHeader(cd.ClassName, MemberVisibilityLevel.Public, parameters));
            sb.Append(Formatter.GetMethodBody(code));
            return sb.ToString();
        }

        protected virtual string CreateFields(DbClassData cd)
        {
            var sb = new StringBuilder(2048);

            // Column fields
            var addedCollections = new List<ColumnCollection>();
            foreach (var column in cd.Columns)
            {
                ColumnCollectionItem collItem;
                var coll = cd.GetCollectionForColumn(column, out collItem);

                if (coll == null)
                {
                    // Not part of a collection
                    var comment = string.Format(Comments.CreateFields.Field, column.Name);
                    sb.AppendLine(Formatter.GetXmlComment(comment));
                    sb.AppendLine(Formatter.GetField(cd.GetPrivateName(column), cd.GetInternalType(column),
                                                     MemberVisibilityLevel.Private));
                }
                else if (!addedCollections.Contains(coll))
                {
                    // Has a collection - only add the code if the collection hasn't been added yet
                    addedCollections.Add(coll);
                    sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateFields.CollectionField, coll.Name)));
                    var collType = DbClassData.GetCollectionTypeString(coll);
                    sb.AppendLine(Formatter.GetField(cd.GetPrivateName(coll), collType, MemberVisibilityLevel.Private,
                                                     "new " + collType + Formatter.OpenParameterString +
                                                     Formatter.CloseParameterString, true, false));
                }
            }

            // Properties for the fields
            addedCollections.Clear();
            foreach (var column in cd.Columns)
            {
                ColumnCollectionItem collItem;
                var coll = cd.GetCollectionForColumn(column, out collItem);

                if (coll == null)
                {
                    // Not part of a collection
                    var comment = string.Format(Comments.CreateFields.Property, column.Name, column.DatabaseType);

                    if (column.DefaultValue != null && !string.IsNullOrEmpty(column.DefaultValue.ToString()))
                        comment += string.Format(Comments.CreateFields.PropertyHasDefaultValue, column.DefaultValue);
                    else
                        comment += Comments.CreateFields.PropertyNoDefaultValue;

                    if (!string.IsNullOrEmpty(column.Comment))
                        comment += string.Format(Comments.CreateFields.PropertyDbComment, column.Comment);

                    sb.AppendLine(Formatter.GetXmlComment(comment));

                    sb.AppendLine(Formatter.GetProperty(cd.GetPublicName(column), cd.GetExternalType(column),
                                                        cd.GetInternalType(column), MemberVisibilityLevel.Public,
                                                        MemberVisibilityLevel.Public, cd.GetPrivateName(column), false, false));
                }
                else if (!addedCollections.Contains(coll))
                {
                    // Has a collection - only add the code if the collection hasn't been added yet
                    addedCollections.Add(coll);

                    var name = cd.GetPublicName(coll);
                    var keyParameter = new MethodParameter("key", coll.KeyType, Formatter);
                    var field = cd.GetPrivateName(coll) + Formatter.OpenIndexer + Formatter.GetCast(coll.KeyType) + "key" +
                                Formatter.CloseIndexer;

                    // Getter
                    sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateFields.PublicMethodGet, coll.Name),
                                                          Comments.CreateFields.PublicMethodGetReturns,
                                                          new KeyValuePair<string, string>("key",
                                                                                           Comments.CreateFields.
                                                                                               PublicMethodGetKeyParameter)));
                    sb.AppendLine(Formatter.GetMethodHeader("Get" + name, MemberVisibilityLevel.Public,
                                                            new MethodParameter[] { keyParameter }, coll.ValueType, false, false));
                    sb.AppendLine(
                        Formatter.GetMethodBody("return " + Formatter.GetCast(cd.GetExternalType(column)) + field +
                                                Formatter.EndOfLine));

                    // Setter
                    sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CreateFields.PublicMethodGet, coll.Name), null,
                                                          new KeyValuePair<string, string>("key",
                                                                                           Comments.CreateFields.
                                                                                               PublicMethodGetKeyParameter),
                                                          new KeyValuePair<string, string>("value",
                                                                                           Comments.CreateFields.
                                                                                               PublicMethodValueParameter)));
                    sb.AppendLine(Formatter.GetMethodHeader("Set" + name, MemberVisibilityLevel.Public,
                                                            new MethodParameter[]
                                                            {
                                                                keyParameter,
                                                                new MethodParameter("value", coll.ValueType, Formatter)
                                                            },
                                                            typeof(void), false, false));

                    sb.AppendLine(
                        Formatter.GetMethodBody(Formatter.GetSetValue(field, "value", true, false, cd.GetInternalType(column))));
                }
            }

            return sb.ToString();
        }

        protected virtual string CreateMethodCopyValuesFrom(DbClassData cd)
        {
            const string sourceName = "source";

            var parameters = new MethodParameter[] { new MethodParameter(sourceName, cd.InterfaceName) };

            var sb = new StringBuilder(2048);

            // Header
            sb.AppendLine(Formatter.GetXmlComment(string.Format(Comments.CopyValuesFrom.Summary, cd.ClassName), null,
                                                  new KeyValuePair<string, string>(sourceName,
                                                                                   string.Format(
                                                                                       Comments.CopyValuesFrom.SourceParameter,
                                                                                       cd.InterfaceName))));
            sb.AppendLine(Formatter.GetMethodHeader(CopyValuesFromMethodName, MemberVisibilityLevel.Public, parameters,
                                                    typeof(void), false, false));

            // Body
            var bodySB = new StringBuilder(2048);
            foreach (var column in cd.Columns)
            {
                bodySB.AppendLine(cd.GetColumnValueMutator(column, sourceName + "." + cd.GetColumnValueAccessor(column)));
            }
            sb.AppendLine(Formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        protected virtual string CreateMethodCopyValuesToDbParameterValues(DbClassData cd)
        {
            const string parameterName = "paramValues";

            var sb = new StringBuilder(2048);

            // Header
            sb.AppendLine(Formatter.GetXmlComment(Comments.CopyToDPV.Summary, null,
                                                  new KeyValuePair<string, string>(_extensionParamName,
                                                                                   Comments.CopyToDPV.ParameterSource),
                                                  new KeyValuePair<string, string>(parameterName,
                                                                                   Comments.CopyToDPV.ParameterDbParameterValues)));
            sb.AppendLine(Formatter.GetExtensionMethodHeader(CopyValuesMethodName,
                                                             new MethodParameter(_extensionParamName, cd.InterfaceName),
                                                             new MethodParameter[]
                                                             {
                                                                 new MethodParameter(parameterName, typeof(DbParameterValues),
                                                                                     Formatter)
                                                             }, typeof(void)));

            // Body
            var bodySB = new StringBuilder(2048);
            foreach (var column in cd.Columns)
            {
                var left = parameterName + "[\"@" + column.Name + "\"]";
                var right = _extensionParamName + "." + cd.GetColumnValueAccessor(column);
                var line = Formatter.GetSetValue(left, right, false, false, cd.GetInternalType(column));
                bodySB.AppendLine(line);
            }
            sb.AppendLine(Formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        protected virtual string CreateMethodCopyValuesToDict(DbClassData cd)
        {
            const string parameterName = "dic";
            const string sourceName = "source";

            var iParameters = new MethodParameter[]
            { new MethodParameter(parameterName, typeof(IDictionary<string, object>), Formatter) };
            var sParameters =
                new MethodParameter[] { new MethodParameter(sourceName, cd.InterfaceName) }.Concat(iParameters).ToArray();

            var sb = new StringBuilder(2048);

            // Instanced header
            sb.AppendLine(Formatter.GetXmlComment(Comments.CopyToDict.Summary, null,
                                                  new KeyValuePair<string, string>(parameterName,
                                                                                   Comments.CopyToDict.ParameterDict)));

            sb.AppendLine(Formatter.GetMethodHeader(CopyValuesMethodName, MemberVisibilityLevel.Public, iParameters, typeof(void),
                                                    false, false));

            // Instanced body
            sb.AppendLine(Formatter.GetMethodBody(Formatter.GetCallMethod(CopyValuesMethodName, "this", parameterName)));

            // Static hader
            sb.AppendLine(Formatter.GetXmlComment(Comments.CopyToDict.Summary, null,
                                                  new KeyValuePair<string, string>(sourceName, Comments.CopyToDict.ParameterSource),
                                                  new KeyValuePair<string, string>(parameterName,
                                                                                   Comments.CopyToDict.ParameterDict)));

            sb.AppendLine(Formatter.GetMethodHeader(CopyValuesMethodName, MemberVisibilityLevel.Public, sParameters, typeof(void),
                                                    false, true));

            // Static body
            var bodySB = new StringBuilder(2048);
            foreach (var column in cd.Columns)
            {
                var left = parameterName + "[\"@" + column.Name + "\"]";
                var right = sourceName + "." + cd.GetColumnValueAccessor(column);
                var line = Formatter.GetSetValue(left, right, false, false, cd.GetExternalType(column));
                bodySB.AppendLine(line);
            }
            sb.AppendLine(Formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        protected virtual string CreateMethodGetColumnData(DbClassData cd)
        {
            const string methodName = "GetColumnData";
            const string parameterName = "columnName";

            var parameters = new MethodParameter[] { new MethodParameter(parameterName, typeof(string), Formatter) };

            var sb = new StringBuilder(4096);

            // Header
            sb.AppendLine(Formatter.GetXmlComment(Comments.GetColumnData.Summary, Comments.GetColumnData.Returns,
                                                  new KeyValuePair<string, string>(parameterName,
                                                                                   Comments.GetColumnData.ColumnNameParameter)));
            sb.AppendLine(Formatter.GetMethodHeader(methodName, MemberVisibilityLevel.Public, parameters, ColumnMetadataClassName,
                                                    false, true));

            // Body
            var switches =
                cd.Columns.Select(
                    x => new KeyValuePair<string, string>("\"" + x.Name + "\"", CreateMethodGetColumnDataReturnString(x)));
            var defaultCode = "throw new ArgumentException(\"Field not found.\",\"" + parameterName + "\")" + Formatter.EndOfLine;
            sb.AppendLine(Formatter.GetMethodBody(Formatter.GetSwitch(parameterName, switches, defaultCode)));

            return sb.ToString();
        }

        protected string CreateMethodGetColumnDataReturnString(DbColumnInfo column)
        {
            var sb = new StringBuilder(256);

            sb.Append(Formatter.ReturnString);
            sb.Append(" ");
            sb.Append("new ");
            sb.Append(ColumnMetadataClassName);
            sb.Append(Formatter.OpenParameterString);
            sb.Append("\"" + column.Name + "\"" + Formatter.ParameterSpacer);
            sb.Append("\"" + column.Comment + "\"" + Formatter.ParameterSpacer);
            sb.Append("\"" + column.DatabaseType + "\"" + Formatter.ParameterSpacer);

            if (column.DefaultValue == null || (column.DefaultValue.ToString() == "" && !(column.DefaultValue is string)))
                sb.Append("null" + Formatter.ParameterSpacer);
            else if (column.DefaultValue is string)
                sb.Append("\"" + column.DefaultValue + "\"" + Formatter.ParameterSpacer);
            else
                sb.Append(column.DefaultValue + Formatter.ParameterSpacer);

            sb.Append("typeof(" + Formatter.GetTypeString(column.Type) + ")" + Formatter.ParameterSpacer);
            sb.Append(column.IsNullable.ToString().ToLower() + Formatter.ParameterSpacer);
            sb.Append((column.KeyType == DbColumnKeyType.Primary).ToString().ToLower() + Formatter.ParameterSpacer);
            sb.Append((column.KeyType == DbColumnKeyType.Foreign).ToString().ToLower());
            sb.Append(Formatter.CloseParameterString);
            sb.Append(Formatter.EndOfLine);

            return sb.ToString();
        }

        protected virtual string CreateMethodGetValue(DbClassData cd)
        {
            const string parameterName = "columnName";
            const string methodName = "GetValue";

            var parameters = new MethodParameter[] { new MethodParameter(parameterName, typeof(string), Formatter) };

            var sb = new StringBuilder(2048);

            // Header
            sb.AppendLine(Formatter.GetXmlComment(Comments.GetValue.Summary, Comments.GetValue.Returns,
                                                  new KeyValuePair<string, string>(parameterName,
                                                                                   Comments.GetValue.ColumnNameParameter)));
            sb.AppendLine(Formatter.GetMethodHeader(methodName, MemberVisibilityLevel.Public, parameters, typeof(object), false,
                                                    false));

            // Body
            var switches =
                cd.Columns.Select(
                    x =>
                    new KeyValuePair<string, string>("\"" + x.Name + "\"",
                                                     Formatter.ReturnString + " " + cd.GetColumnValueAccessor(x) +
                                                     Formatter.EndOfLine));
            var defaultCode = "throw new ArgumentException(\"Field not found.\",\"" + parameterName + "\")" + Formatter.EndOfLine;
            sb.AppendLine(Formatter.GetMethodBody(Formatter.GetSwitch(parameterName, switches, defaultCode)));

            return sb.ToString();
        }

        protected virtual string CreateMethodReadValues(DbClassData cd)
        {
            var sb = new StringBuilder(2048);

            // Header
            sb.AppendLine(Formatter.GetXmlComment(Comments.ReadValues.Summary, null,
                                                  new KeyValuePair<string, string>(_extensionParamName,
                                                                                   Comments.Extensions.ExtensionParameter),
                                                  new KeyValuePair<string, string>(DataReaderName,
                                                                                   Comments.ReadValues.ParameterDataReader)));

            sb.AppendLine(Formatter.GetExtensionMethodHeader("ReadValues", new MethodParameter(_extensionParamName, cd.ClassName),
                                                             new MethodParameter[]
                                                             {
                                                                 new MethodParameter(DataReaderName, typeof(IDataReader), Formatter)
                                                             }, typeof(void)));

            // Body
            var bodySB = new StringBuilder(2048);
            bodySB.AppendLine(Formatter.GetLocalField("i", typeof(int), null));
            bodySB.AppendLine();
            foreach (var column in cd.Columns)
            {
                bodySB.AppendLine(Formatter.GetSetValue("i", DataReaderName + ".GetOrdinal(\"" + column.Name + "\")", false, false));

                var right = cd.GetDataReaderAccessor(column, "i");
                bodySB.AppendLine(cd.GetColumnValueMutator(column, right, _extensionParamName));
                bodySB.AppendLine();
            }

            sb.AppendLine(Formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        protected virtual string CreateMethodSetValue(DbClassData cd)
        {
            const string parameterName = "columnName";
            const string valueName = "value";
            const string methodName = "SetValue";

            var parameters = new MethodParameter[]
            {
                new MethodParameter(parameterName, typeof(string), Formatter),
                new MethodParameter(valueName, typeof(object), Formatter)
            };

            var sb = new StringBuilder(2048);

            // Header
            sb.AppendLine(Formatter.GetXmlComment(Comments.SetValue.Summary, null,
                                                  new KeyValuePair<string, string>(parameterName,
                                                                                   Comments.SetValue.ColumnNameParameter),
                                                  new KeyValuePair<string, string>(valueName, Comments.SetValue.ValueParameter)));
            sb.AppendLine(Formatter.GetMethodHeader(methodName, MemberVisibilityLevel.Public, parameters, typeof(void), false,
                                                    false));

            // Body
            var switches =
                cd.Columns.Select(
                    x =>
                    new KeyValuePair<string, string>("\"" + x.Name + "\"",
                                                     cd.GetColumnValueMutator(x, valueName) + Environment.NewLine + "break" +
                                                     Formatter.EndOfLine));
            var defaultCode = "throw new ArgumentException(\"Field not found.\",\"" + parameterName + "\")" + Formatter.EndOfLine;
            sb.AppendLine(Formatter.GetMethodBody(Formatter.GetSwitch(parameterName, switches, defaultCode)));

            return sb.ToString();
        }

        protected virtual string CreateMethodTryCopyValuesToDbParameterValues(DbClassData cd)
        {
            const string parameterName = "paramValues";

            var sb = new StringBuilder(2048);

            // Header
            sb.AppendLine(Formatter.GetXmlComment(Comments.TryCopyValues.Summary, null,
                                                  new KeyValuePair<string, string>(_extensionParamName,
                                                                                   Comments.TryCopyValues.ParameterSource),
                                                  new KeyValuePair<string, string>(parameterName,
                                                                                   Comments.TryCopyValues.
                                                                                       ParameterDbParameterValues)));

            sb.AppendLine(Formatter.GetExtensionMethodHeader(TryCopyValuesMethodName,
                                                             new MethodParameter(_extensionParamName, cd.InterfaceName),
                                                             new MethodParameter[]
                                                             {
                                                                 new MethodParameter(parameterName, typeof(DbParameterValues),
                                                                                     Formatter)
                                                             }, typeof(void)));

            // Body
            var bodySB = new StringBuilder(2048);
            bodySB.AppendLine("for (int i = 0; i < " + parameterName + ".Count; i++)"); // NOTE: Language specific code
            bodySB.AppendLine(Formatter.OpenBrace);
            {
                bodySB.AppendLine(Formatter.GetSwitch(parameterName + ".GetParameterName(i)",
                                                      cd.Columns.Select(
                                                          x =>
                                                          new KeyValuePair<string, string>("\"@" + x.Name + "\"",
                                                                                           CreateMethodTryCopyValuesToDbParameterValuesSwitchString
                                                                                               (cd, x, parameterName))), null));
            }
            bodySB.Append(Formatter.CloseBrace);

            sb.AppendLine(Formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        protected string CreateMethodTryCopyValuesToDbParameterValuesSwitchString(DbClassData cd, DbColumnInfo column,
                                                                                  string parameterName)
        {
            var sb = new StringBuilder();
            sb.AppendLine(Formatter.GetSetValue(parameterName + Formatter.OpenIndexer + "i" + Formatter.CloseIndexer,
                                                _extensionParamName + "." + cd.GetColumnValueAccessor(column), false, false,
                                                cd.GetInternalType(column)));
            sb.AppendLine("break" + Formatter.EndOfLine);
            return sb.ToString();
        }

        protected virtual string CreateMethodTryReadValues(DbClassData cd)
        {
            var sb = new StringBuilder(2048);

            // Header
            sb.AppendLine(Formatter.GetXmlComment(Comments.TryReadValues.Summary, null,
                                                  new KeyValuePair<string, string>(_extensionParamName,
                                                                                   Comments.Extensions.ExtensionParameter),
                                                  new KeyValuePair<string, string>(DataReaderName,
                                                                                   Comments.TryReadValues.ParameterDataReader)));

            sb.AppendLine(Formatter.GetExtensionMethodHeader("TryReadValues",
                                                             new MethodParameter(_extensionParamName, cd.ClassName),
                                                             new MethodParameter[]
                                                             {
                                                                 new MethodParameter(DataReaderName, typeof(IDataReader), Formatter)
                                                             }, typeof(void)));

            // Body
            var bodySB = new StringBuilder(2048);
            bodySB.AppendLine("for (int i = 0; i < " + DataReaderName + ".FieldCount; i++)"); // NOTE: Hardcoded language code
            bodySB.AppendLine(Formatter.OpenBrace);
            {
                bodySB.AppendLine(Formatter.GetSwitch(DataReaderName + ".GetName(i)",
                                                      cd.Columns.Select(
                                                          x =>
                                                          new KeyValuePair<string, string>("\"" + x.Name + "\"",
                                                                                           CreateMethodTryReadValuesSwitchString(
                                                                                               cd, x))), null));
            }
            bodySB.Append(Formatter.CloseBrace);

            sb.AppendLine(Formatter.GetMethodBody(bodySB.ToString()));

            return sb.ToString();
        }

        protected string CreateMethodTryReadValuesSwitchString(DbClassData cd, DbColumnInfo column)
        {
            var sb = new StringBuilder();
            sb.AppendLine(cd.GetColumnValueMutator(column, cd.GetDataReaderAccessor(column, "i"), _extensionParamName));
            sb.AppendLine("break" + Formatter.EndOfLine);
            return sb.ToString();
        }

        protected virtual string FullConstructorMemberBody(DbClassData cd)
        {
            var sb = new StringBuilder(1024);
            foreach (var column in cd.Columns)
            {
                var right = cd.GetParameterName(column);
                sb.AppendLine(cd.GetColumnValueMutator(column, right));
            }
            return sb.ToString();
        }

        public virtual void Generate(string classNamespace, string interfaceNamespace, string classOutputDir,
                                     string interfaceOutputDir)
        {
            if (!classOutputDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
                classOutputDir += Path.DirectorySeparatorChar.ToString();

            if (!interfaceOutputDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
                interfaceOutputDir += Path.DirectorySeparatorChar.ToString();

            if (!Directory.Exists(classOutputDir))
                Directory.CreateDirectory(classOutputDir);

            if (!Directory.Exists(interfaceOutputDir))
                Directory.CreateDirectory(interfaceOutputDir);

            var items = Generate(classNamespace, interfaceNamespace);

            foreach (var item in items)
            {
                string filePath;
                if (item.CodeType == GeneratedCodeType.Interface)
                    filePath = interfaceOutputDir;
                else
                    filePath = classOutputDir;

                filePath += item.ClassName + "." + Formatter.FilenameSuffix;

                File.WriteAllText(filePath, item.Code);
            }
        }

        public virtual IEnumerable<GeneratedTableCode> Generate(string classNamespace, string interfaceNamespace)
        {
            LoadDbContent();

            bool isFilteringTables = (TablesToGenerate != null && TablesToGenerate.Count() > 0);

            foreach (var table in _dbTables)
            {
                if (isFilteringTables && !TablesToGenerate.Contains(table.Key, StringComparer.OrdinalIgnoreCase))
                    continue;

                var generatedCodes = CreateCode(table.Key, table.Value, classNamespace, interfaceNamespace);
                foreach (var item in generatedCodes)
                {
                    yield return item;
                }
            }

            yield return CreateCodeForColumnMetadata(classNamespace);
        }

        protected abstract IEnumerable<DbColumnInfo> GetColumns(string table);

        protected virtual MethodParameter[] GetConstructorParameters(DbClassData cd)
        {
            var columnsArray = cd.Columns.ToArray();
            var parameters = new MethodParameter[columnsArray.Length];
            for (var i = 0; i < columnsArray.Length; i++)
            {
                var column = columnsArray[i];
                var p = new MethodParameter(cd.GetParameterName(column), cd.GetExternalType(column));
                parameters[i] = p;
            }

            return parameters;
        }

        protected abstract IEnumerable<string> GetTables();

        /// <summary>
        /// Loads the content (table and column data) from the database and populates _dbTables. This only happens once
        /// per object instance.
        /// </summary>
        void LoadDbContent()
        {
            if (_isDbContentLoaded)
                return;

            _isDbContentLoaded = true;

            DbConnection.Open();

            var tables = GetTables();

            foreach (var table in tables)
            {
                var columns = GetColumns(table);
                _dbTables.Add(table, columns.ToArray());
            }

            DbConnection.Close();
        }

        /// <summary>
        /// Sets the name of the method to use for the DataReader to read a given type.
        /// </summary>
        /// <param name="type">The type to read.</param>
        /// <param name="methodName">Name of the DataReader method to use to read the <paramref name="type"/>.</param>
        public void SetDataReaderReadMethod(Type type, string methodName)
        {
            if (_dataReaderReadMethods.ContainsKey(type))
                _dataReaderReadMethods[type] = methodName;
            else
                _dataReaderReadMethods.Add(type, methodName);
        }

        protected void SetDbConnection(DbConnection dbConnection)
        {
            if (_dbConnction != null)
                throw new MethodAccessException("The DbConnection has already been set!");

            _dbConnction = dbConnection;
        }

        string WrapCodeFile(string code, string namespaceName, bool isInterface)
        {
            var sb = new StringBuilder(code.Length + 512);

            // Header
            IEnumerable<string> usings = new string[] { "System", "System.Linq" };
            if (!isInterface)
                usings = usings.Concat(_usings);

            foreach (var usingNamespace in usings)
            {
                sb.AppendLine(Formatter.GetUsing(usingNamespace));
            }

            // Namespace
            sb.AppendLine(Formatter.GetNamespace(namespaceName));
            sb.AppendLine(Formatter.OpenBrace);
            {
                // Code
                sb.AppendLine(code);
            }
            sb.AppendLine(Formatter.CloseBrace);

            return sb.ToString();
        }

        #region IDisposable Members

        public abstract void Dispose();

        #endregion
    }
}