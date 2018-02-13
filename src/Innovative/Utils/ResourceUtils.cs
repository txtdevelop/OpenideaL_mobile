using System;
using System.IO;
using System.Reflection;
using PSY.Innovative.Contracts;
using PSY.Innovative.Helpers;

namespace PSY.Innovative.Utils
{
    /// <summary>
    /// Common resource related functions. The resource must be located in the same assembly as this class,
    /// thats why the class is internal. 
    /// </summary>
    internal class ResourceUtils
    {
        /// <summary>
        /// Returns a localized resource stream. First it looks for the stream with the current language. If such a stream dosn't exist
        /// or the language is not supported the default language (englisch) is returned.
        /// </summary>
        /// <param name="localizationService">The localization service</param>
        /// <param name="resourceName">The resource file name.</param>
        /// <returns></returns>
        internal static Stream GetLocalizedResourceStream(ILocalizationService localizationService, String resourceName)
        {
            try
            {
                var assembly = typeof(ResourceUtils).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream(localizationService.GetLocalizedFileName(resourceName));
                if (stream == null)
                    stream = assembly.GetManifestResourceStream(localizationService.GetLocalizedFileName(resourceName, true));
                // last chance, try undecorated name
                if (stream == null)
                    stream = assembly.GetManifestResourceStream(resourceName);
                return stream;
            }
            catch (Exception ex)
            {
                Logger.Exception(nameof(GetLocalizedResourceStream), ex);
                return null;
            }
        }
    }
}
