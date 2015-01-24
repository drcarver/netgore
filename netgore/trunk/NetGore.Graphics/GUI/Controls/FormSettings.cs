using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    public class FormSettings : TextControlSettings
    {
        static FormSettings _default = null;

        public FormSettings(Color foreColor, ControlBorder border) : base(foreColor, border)
        {
        }

        public static FormSettings Default
        {
            get
            {
                if (_default == null)
                    new FormSettings(Color.Black, null);
                return _default;
            }
        }
    }
}