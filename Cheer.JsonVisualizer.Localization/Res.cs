using System.Drawing;
using System.Globalization;
using System.Resources;

namespace Cheer.JsonVisualizer.Localization
{
    public static partial class Res
    {
        private static ResourceManager resourceManager;

        public static ResourceManager ResourceManager
        {
            get
            {
                if(object.ReferenceEquals(resourceManager, null))
                {
                    var rm = new ResourceManager(typeof(Res).FullName, typeof(Res).Assembly);
                    resourceManager = rm;
                }
                return resourceManager;
            }
        }
        public static CultureInfo Culture
        {
            get;
            set;
        }

        private const string DefaultFontResourceName = "Font.Default";

        /// <summary>
        /// Gets the default font for the user interface.
        /// </summary>
        public static Font DefaultFont
        {
            get
            {
                Font font = (Font)ResourceManager.GetObject(DefaultFontResourceName, Culture);
                return font == null ? null : (Font)font.Clone();
            }
        }
    }
}