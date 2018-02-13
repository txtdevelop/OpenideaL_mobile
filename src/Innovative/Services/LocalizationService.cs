using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using MvvmCross.Platform;
using Plugin.Settings.Abstractions;
using PSY.Innovative.Contracts;
using PSY.Innovative.Models;
using PSY.Innovative.Resources;
using PSY.Innovative.Utils;

namespace PSY.Innovative.Services
{
    public class LocalizationService : ILocalizationService
    {
        private const string DefaultLanguageCode = "en";
        private readonly ILocalizationInfo _localizationInfo;
        private readonly ISettings _settings;
        //private CultureInfo _currentCultureInfo;
        private Language _currentLanguage;
        private ResourceManager _resmgr;
        private string _resourceId;

        public LocalizationService(ILocalizationInfo localizationInfo, ISettings settings)
        {
            _localizationInfo = localizationInfo;
            _settings = settings;

            SupportedLanguages = new List<Language>
            {
                new Language {CultureInfo = new CultureInfo("en")},
                new Language {CultureInfo = new CultureInfo("de")},
                new Language {CultureInfo = new CultureInfo("it")},
                new Language {CultureInfo = new CultureInfo("fr")},
            };

            InitializeCurrentLanguage();
        }

        public IEnumerable<Language> SupportedLanguages { get; private set; }

        public Language CurrentLanguage
        {
            get { return _currentLanguage; }
            set
            {
                _currentLanguage = value;
                AppResources.Culture = _currentLanguage.CultureInfo;
                SaveCurrentLanguage();
            }
        }

        public string GetString(string key)
        {
            var translation = _resmgr.GetString(key, _currentLanguage.CultureInfo);

            if (translation == null)
            {
#if DEBUG
                // throw new ArgumentException(
                Mvx.Exception($"Key '{key}' was not found in resources '{_resourceId}' for culture '{_currentLanguage.CultureInfo.Name}'.", "Text");
                translation = key;
#else
                translation = key; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }

        /// <summary>
        /// Appends a the current language to the file name (just before the extension).
        /// Example: "Readme.txt" -> "Readme_de.txt"
        /// </summary>
        /// <param name="fileName"> The file name to append the language</param>
        /// <param name="useDefaultLanguage"> If true it uses the default language (en).</param>
        /// <returns>The file name with the language added.</returns>
        public string GetLocalizedFileName(string fileName, bool useDefaultLanguage = false)
        {
            var language = DefaultLanguageCode;
            if (!useDefaultLanguage)
            {
                language = CurrentLanguage.CultureInfo.Name.ToLower();
                if (language.Contains("-"))
                    language = language.Substring(language.IndexOf("-", StringComparison.Ordinal) + 1, 2);
            }
            return $"{Path.ChangeExtension(fileName, null)}_{language}{Path.GetExtension(fileName)}";
        }

        private void InitializeCurrentLanguage()
        {
            var defaultDeviceLanguage = _localizationInfo.GetCurrentCultureInfo();
            var settingsLanguage = _settings.GetValueOrDefault(SettingsKeys.Language, "en");

            if (string.IsNullOrEmpty(settingsLanguage))
            {
                settingsLanguage = defaultDeviceLanguage.Name;
            }

            var currentCultureInfo = new CultureInfo(settingsLanguage);
            _resourceId = typeof (AppResources).FullName;
            _resmgr = new ResourceManager(_resourceId, typeof (AppResources).GetTypeInfo().Assembly);

            // first check language and region
            var language = (SupportedLanguages.SingleOrDefault(l => l.CultureInfo.Name.Equals(currentCultureInfo.Name, StringComparison.OrdinalIgnoreCase)) 
                ?? SupportedLanguages.FirstOrDefault(l => l.CultureInfo.TwoLetterISOLanguageName.Equals(currentCultureInfo.TwoLetterISOLanguageName, StringComparison.OrdinalIgnoreCase))) 
                ?? SupportedLanguages.Single(l => l.CultureInfo.Name == DefaultLanguageCode);
            CurrentLanguage = language;
        }

        private void SaveCurrentLanguage()
        {
            _settings.AddOrUpdateValue(SettingsKeys.Language, _currentLanguage.CultureInfo.Name);
        }
    }
}