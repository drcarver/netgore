using System;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Factory for DynamicEntity creation and serialization.
    /// </summary>
    public static class DynamicEntityFactory
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public const string TypeNameStringKey = "DynamicEntityType";
        static readonly TypeFactory _typeCollection;

        /// <summary>
        /// Static DynamicEntityFactory constructor.
        /// </summary>
        static DynamicEntityFactory()
        {
            // NOTE: It would be nice if the DynamicEntity constructors were required on the Client, but still not the Server (since the server doesn't need to construct all DynamicEntities)
            var filter = new TypeFilterCreator { IsClass = true, IsAbstract = false, Subclass = typeof(DynamicEntity) };
            _typeCollection = new TypeFactory(filter.GetFilter(), OnLoadTypeHandler, false);
            _typeCollection.BeginLoading();
        }

        /// <summary>
        /// Handles when a new type has been loaded into a FactoryTypeCollection.
        /// </summary>
        /// <param name="factoryTypeCollection">FactoryTypeCollection that the event occured on.</param>
        /// <param name="loadedType">Type that was loaded.</param>
        /// <param name="name">Name of the Type.</param>
        static void OnLoadTypeHandler(TypeFactory factoryTypeCollection, Type loadedType, string name)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat("Loaded DynamicEntity `{0}` from Type `{1}`.", name, loadedType);
        }

        /// <summary>
        /// Reads and constructs a DynamicEntity from a stream.
        /// </summary>
        /// <param name="reader">XmlReader to read the DynamicEntity from.</param>
        /// <returns>The DynamicEntity created from the <paramref name="reader"/>.</returns>
        public static DynamicEntity Read(IValueReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            string typeName = reader.ReadString(TypeNameStringKey);

            DynamicEntity dEntity = (DynamicEntity)_typeCollection.GetTypeInstance(typeName);

            dEntity.ReadAll(reader);

            return dEntity;
        }

        /// <summary>
        /// Writes a DynamicEntity to a stream.
        /// </summary>
        /// <param name="writer">IValueWriter to write the DynamicEntity to.</param>
        /// <param name="dEntity">DynamicEntity to write to the stream.</param>
        public static void Write(IValueWriter writer, DynamicEntity dEntity)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            if (dEntity == null)
                throw new ArgumentNullException("dEntity");

            Type type = dEntity.GetType();
            string typeName = _typeCollection[type];

            writer.Write(TypeNameStringKey, typeName);
            dEntity.WriteAll(writer);
        }
    }
}