using System.IO;
using System.Reflection;

namespace Temporal
{
    public class Resources
    {
        private const string TemporalEmbeddedAssetsPrefix = "Temporal.assets.";

        public static string GetCss()
        {
            var css = GetStuntmanResource("time-machine.css");

            return css;
        }

        private static string GetStuntmanResource(string resourceName)
        {
            var resource = string.Empty;

            using (var stream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream(TemporalEmbeddedAssetsPrefix + resourceName))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    resource = streamReader.ReadToEnd();
                }
            }

            return resource;
        }
    }
}
