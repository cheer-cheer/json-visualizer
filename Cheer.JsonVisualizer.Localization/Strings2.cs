namespace Cheer.JsonVisualizer.Localization
{
    /// <summary>
    /// Localizable strings.
    /// </summary>
    public class Strings2
        : Strings
    {
        protected Strings2()
        {

        }
        
        public static string Format(string key, params object[] args)
        {
            var culture = Culture;
            var format = ResourceManager.GetString(key, culture);

            return format == null ? null : string.Format(culture, format, args);
        }
    }
}
