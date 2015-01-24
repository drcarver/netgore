using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.IO;

namespace DemoGame.Client
{
    public delegate void UseSkillHandler(SkillType skillType);

    public class SkillsForm : Form, IRestorableSettings
    {
        static readonly Vector2 _iconSize = new Vector2(32, 32);
        readonly int _lineSpacing;
        public event UseSkillHandler OnUseSkill;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillsForm"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="parent">The parent.</param>
        public SkillsForm(Vector2 position, Control parent)
            : base(parent.GUIManager, "Skills", position, new Vector2(150, 100), parent)
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            // Find the spacing to use between lines
            _lineSpacing = (int)Math.Max(Font.LineSpacing, _iconSize.Y);

            // Create all the skills
            var allSkillTypes = SkillTypeHelper.Values;
            Vector2 offset = Vector2.Zero;
            foreach (SkillType skillType in allSkillTypes)
            {
                CreateSkillEntry(offset, skillType);
                offset += new Vector2(0, _lineSpacing);
            }
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        void CreateSkillEntry(Vector2 position, SkillType skillType)
        {
            SkillInfo skillInfo = SkillInfo.GetSkillInfo(skillType);

            PictureBox pb = new SkillPictureBox(this, skillInfo, position);
            pb.OnClick += SkillPicture_OnClick;

            SkillLabel skillLabel = new SkillLabel(this, skillInfo, position + new Vector2(_iconSize.X + 4, 0));
            skillLabel.OnClick += SkillLabel_OnClick;
        }

        void SkillLabel_OnClick(object sender, MouseClickEventArgs e)
        {
            if (OnUseSkill != null)
            {
                SkillLabel source = (SkillLabel)sender;
                OnUseSkill(source.SkillInfo.SkillType);
            }
        }

        void SkillPicture_OnClick(object sender, MouseClickEventArgs e)
        {
            if (OnUseSkill != null)
            {
                SkillPictureBox source = (SkillPictureBox)sender;
                OnUseSkill(source.SkillInfo.SkillType);
            }
        }

        #region IRestorableSettings Members

        /// <summary>
        /// Loads the values supplied by the <paramref name="items"/> to reconstruct the settings.
        /// </summary>
        /// <param name="items">NodeItems containing the values to restore.</param>
        public void Load(IDictionary<string, string> items)
        {
            Position = new Vector2(items.AsFloat("X", Position.X), items.AsFloat("Y", Position.Y));
            IsVisible = items.AsBool("IsVisible", IsVisible);
        }

        /// <summary>
        /// Returns the key and value pairs needed to restore the settings.
        /// </summary>
        /// <returns>The key and value pairs needed to restore the settings.</returns>
        public IEnumerable<NodeItem> Save()
        {
            return new NodeItem[]
            { new NodeItem("X", Position.X), new NodeItem("Y", Position.Y), new NodeItem("IsVisible", IsVisible) };
        }

        #endregion

        class SkillLabel : Label
        {
            public SkillLabel(Control parent, SkillInfo skillInfo, Vector2 position) : base(skillInfo.Name, position, parent)
            {
                SkillInfo = skillInfo;
            }

            public SkillInfo SkillInfo { get; private set; }
        }

        class SkillPictureBox : PictureBox
        {
            public SkillPictureBox(Control parent, SkillInfo skillInfo, Vector2 position)
                : base(new Grh(GrhInfo.GetData(skillInfo.Icon)), position, parent)
            {
                SkillInfo = skillInfo;
                Size = _iconSize;
            }

            public SkillInfo SkillInfo { get; private set; }
        }
    }
}