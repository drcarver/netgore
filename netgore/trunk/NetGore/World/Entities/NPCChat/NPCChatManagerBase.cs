using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.NPCChat
{
    /// <summary>
    /// Base class for managing the NPCChatDialogBases.
    /// </summary>
    public abstract class NPCChatManagerBase : IEnumerable<NPCChatDialogBase>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly bool _isReadonly;
        readonly DArray<NPCChatDialogBase> _npcChatDialogs = new DArray<NPCChatDialogBase>(32, false);

        /// <summary>
        /// NPCChatManagerBase constructor.
        /// </summary>
        /// <param name="isReadonly">If this manager is read-only.</param>
        protected NPCChatManagerBase(bool isReadonly)
        {
            _isReadonly = isReadonly;
            Load();
        }

        /// <summary>
        /// Gets the NPCChatDialogBase at the specified index.
        /// </summary>
        /// <param name="index">Index of the NPCChatDialogBase.</param>
        /// <returns>The NPCChatDialogBase at the specified index, or null if invalid.</returns>
        public NPCChatDialogBase this[int index]
        {
            get
            {
                // Check for a valid index
                if (!_npcChatDialogs.CanGet(index))
                {
                    const string errmsg = "Invalid NPC chat dialog index `{0}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, index);
                    Debug.Fail(string.Format(errmsg, index));
                    return null;
                }

                return _npcChatDialogs[index];
            }
            set
            {
                if (IsReadonly)
                    throw CreateReadonlyException();

                _npcChatDialogs[index] = value;
            }
        }

        /// <summary>
        /// Gets if this NPCChatManagerBase is read-only.
        /// </summary>
        public bool IsReadonly
        {
            get { return _isReadonly; }
        }

        /// <summary>
        /// When overridden in the derived class, creates a NPCChatDialogBase from the given IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        /// <returns>A NPCChatDialogBase created from the given IValueReader.</returns>
        protected abstract NPCChatDialogBase CreateDialog(IValueReader reader);

        /// <summary>
        /// Creates a MethodAccessException to use for when trying to access a method that is cannot be access when read-only. 
        /// </summary>
        /// <returns>A MethodAccessException to use for when trying to access a method that is cannot be
        /// access when read-only.</returns>
        protected static MethodAccessException CreateReadonlyException()
        {
            return new MethodAccessException("Cannot access this method when the NPCChatManagerBase is set to Read-Only.");
        }

        /// <summary>
        /// Gets the path for the data file.
        /// </summary>
        /// <param name="contentPath">ContentPaths to use.</param>
        /// <returns>The path for the data file.</returns>
        protected static string GetFilePath(ContentPaths contentPath)
        {
            return contentPath.Data.Join("npcchat.xml");
        }

        /// <summary>
        /// Loads the data from file.
        /// </summary>
        void Load()
        {
            _npcChatDialogs.Clear();

            var filePath = GetFilePath(ContentPaths.Dev);

            if (!File.Exists(filePath))
            {
                _npcChatDialogs.Trim();
                return;
            }

            var reader = new XmlValueReader(filePath, "ChatDialogs");
            var items = reader.ReadManyNodes("ChatDialogs", x => CreateDialog(x));

            for (var i = 0; i < items.Length; i++)
            {
                if (items[i] != null)
                    _npcChatDialogs[i] = items[i];
            }

            _npcChatDialogs.Trim();
        }

        /// <summary>
        /// Reorganizes the internal buffer to ensure the indices all match up. Only needed if IsReadonly is false
        /// and you don't manually update the indices.
        /// </summary>
        public void Reorganize()
        {
            var dialogs = _npcChatDialogs.ToArray();
            _npcChatDialogs.Clear();

            foreach (var dialog in dialogs)
            {
                _npcChatDialogs[dialog.Index] = dialog;
            }
        }

        /// <summary>
        /// Saves the NPCChatDialogBases in this NPCChatManagerBase to file.
        /// </summary>
        public void Save()
        {
            var filePath = GetFilePath(ContentPaths.Dev);
            var buildFilePath = GetFilePath(ContentPaths.Build);
            var tempFilePath = filePath + ".temp";

            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);

            var dialogs = _npcChatDialogs.Where(x => x != null);
            using (var writer = new XmlValueWriter(tempFilePath, "ChatDialogs"))
            {
                writer.WriteManyNodes("ChatDialogs", dialogs, ((w, item) => item.Write(w)));
            }

            if (File.Exists(filePath))
                File.Delete(filePath);

            File.Copy(tempFilePath, filePath);

            if (File.Exists(buildFilePath))
                File.Delete(buildFilePath);

            File.Copy(tempFilePath, buildFilePath);

            File.Delete(tempFilePath);
        }

        #region IEnumerable<NPCChatDialogBase> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<NPCChatDialogBase> GetEnumerator()
        {
            return ((IEnumerable<NPCChatDialogBase>)_npcChatDialogs).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}