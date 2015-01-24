using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.IO;
using NetGore.NPCChat.Conditionals;

namespace NetGore.EditorTools.NPCChat
{
    public delegate void EditorNPCChatConditionalCollectionItemHandler(EditorNPCChatConditionalCollectionItem sender);

    public class EditorNPCChatConditionalCollectionItem : NPCChatConditionalCollectionItemBase
    {
        readonly List<NPCChatConditionalParameter> _parameters = new List<NPCChatConditionalParameter>();
        NPCChatConditionalBase _conditional;
        bool _not;

        public event EditorNPCChatConditionalCollectionItemHandler OnChange;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorNPCChatConditionalCollectionItem"/> class.
        /// </summary>
        /// <param name="source">The source NPCChatConditionalCollectionBase to copy the values from. If null,
        /// no values are copied.</param>
        public EditorNPCChatConditionalCollectionItem(NPCChatConditionalCollectionItemBase source)
        {
            if (source == null)
                return;

            var stream = new BitStream(BitStreamMode.Write, 256);

            using (var writer = new BinaryValueWriter(stream))
            {
                source.Write(writer);
            }

            stream.Mode = BitStreamMode.Read;

            IValueReader reader = new BinaryValueReader(stream);

            Read(reader);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorNPCChatConditionalCollectionItem"/> class.
        /// </summary>
        /// <param name="r">The IValueReader to read from.</param>
        public EditorNPCChatConditionalCollectionItem(IValueReader r)
        {
            Read(r);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorNPCChatConditionalCollectionItem"/> class.
        /// </summary>
        public EditorNPCChatConditionalCollectionItem()
        {
            _not = false;
            SetConditional(NPCChatConditionalBase.Conditionals.First());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorNPCChatConditionalCollectionItem"/> class.
        /// </summary>
        /// <param name="conditional">The conditional to use.</param>
        public EditorNPCChatConditionalCollectionItem(NPCChatConditionalBase conditional)
        {
            if (conditional == null)
                throw new ArgumentNullException("conditional");

            _not = false;
            SetConditional(conditional);
        }

        /// <summary>
        /// When overridden in the derived class, gets the NPCChatConditionalBase.
        /// </summary>
        public override NPCChatConditionalBase Conditional
        {
            get { return _conditional; }
        }

        /// <summary>
        /// When overridden in the derived class, gets a boolean that, if true, the result of this conditional
        /// when evaluating will be flipped. That is, True becomes False and vise versa. If false, the
        /// evaluated value is unchanged.
        /// </summary>
        public override bool Not
        {
            get { return _not; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the collection of parameters to use when evaluating
        /// the conditional.
        /// </summary>
        public override NPCChatConditionalParameter[] Parameters
        {
            get { return _parameters.ToArray(); }
        }

        /// <summary>
        /// Copies the values of this EditorNPCChatConditionalCollectionItem to another EditorNPCChatConditionalCollectionItem.
        /// </summary>
        /// <param name="dest">The EditorNPCChatConditionalCollectionItem to copy the values into.</param>
        public void CopyValuesTo(EditorNPCChatConditionalCollectionItem dest)
        {
            var stream = new BitStream(BitStreamMode.Write, 256);

            using (var writer = new BinaryValueWriter(stream))
            {
                Write(writer);
            }

            stream.Mode = BitStreamMode.Read;

            IValueReader reader = new BinaryValueReader(stream);

            dest.Read(reader);
        }

        /// <summary>
        /// Sets the Conditional.
        /// </summary>
        /// <param name="conditional">The new NPCChatConditionalBase.</param>
        public void SetConditional(NPCChatConditionalBase conditional)
        {
            if (conditional == null)
                throw new ArgumentNullException("conditional");

            if (_conditional == conditional)
                return;

            // Set the new conditional
            _conditional = conditional;

            // Re-create all the parameters to the appropriate type for the new conditional
            var newParameters = new NPCChatConditionalParameter[Conditional.ParameterCount];

            for (var i = 0; i < Conditional.ParameterCount; i++)
            {
                var neededType = Conditional.GetParameter(i);
                if (i < _parameters.Count && _parameters[i].ValueType == neededType)
                {
                    // The type matches the old type, so just reuse it
                    newParameters[i] = _parameters[i];
                }
                else
                {
                    // Different type or out of range, so make a new one
                    newParameters[i] = NPCChatConditionalParameter.CreateParameter(neededType);
                }
            }

            // Set the new parameters
            _parameters.Clear();
            _parameters.AddRange(newParameters);

            if (OnChange != null)
                OnChange(this);
        }

        /// <summary>
        /// Sets the Not property's value.
        /// </summary>
        /// <param name="value">The new value.</param>
        public void SetNot(bool value)
        {
            _not = value;

            if (OnChange != null)
                OnChange(this);
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="conditional">The conditional.</param>
        /// <param name="not">The Not value.</param>
        /// <param name="parameters">The parameters.</param>
        protected override void SetReadValues(NPCChatConditionalBase conditional, bool not,
                                              NPCChatConditionalParameter[] parameters)
        {
            _conditional = conditional;
            _not = not;
            _parameters.Clear();
            _parameters.AddRange(parameters);

            if (OnChange != null)
                OnChange(this);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Not)
                sb.Append("NOT ");
            sb.Append(Conditional.Name);
            sb.Append("(");
            if (_parameters.Count > 0)
            {
                foreach (var item in _parameters)
                {
                    var itemStr = item.Value.ToString();
                    if (itemStr.Length > 30)
                        itemStr = itemStr.Substring(0, 25) + "...";
                    sb.Append(itemStr);
                    sb.Append(", ");
                }
                sb.Length -= 2;
            }
            sb.Append(")");
            return sb.ToString();
        }

        /// <summary>
        /// Tries the set one of the Parameters.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if set successfully; otherwise false.</returns>
        public bool TrySetParameter(int index, NPCChatConditionalParameter value)
        {
            if (index < 0)
                return false;
            if (index >= _parameters.Count)
                return false;
            if (value == null)
                return false;

            _parameters[index] = value;

            if (OnChange != null)
                OnChange(this);

            return true;
        }
    }
}