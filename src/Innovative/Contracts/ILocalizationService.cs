using System.Collections.Generic;
using PSY.Innovative.Models;

namespace PSY.Innovative.Contracts
{
    public interface ILocalizationService
    {
        Language CurrentLanguage { get; set; }
        IEnumerable<Language> SupportedLanguages { get; }

        string GetString(string key);

        /// <summary>
        /// Appends a the current language to the file name (just before the extension).
        /// Example: "Readme.txt" -> "Readme_de.txt"
        /// </summary>
        /// <param name="fileName"> The file name to append the language</param>
        /// <param name="useDefaultLanguage"> If true it uses the default language (en).</param>
        /// <returns>The file name with the language added.</returns>
        string GetLocalizedFileName(string fileName, bool useDefaultLanguage = false);
    }
}