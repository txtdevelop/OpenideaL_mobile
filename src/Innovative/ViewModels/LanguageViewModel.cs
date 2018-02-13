using System;
using System.Collections.Generic;
using System.Linq;
using PSY.Innovative.Resources;

namespace PSY.Innovative.ViewModels
{
    public class LanguageViewModel : BaseViewModel
    {
//        private readonly IEventAggregatorComponent _eventAggregator;
        private LanguageDetailViewModel _selectedLanguage;

        public LanguageViewModel(/*IEventAggregatorComponent eventAggregator*/)
        {
            Title = AppResources.Language;
            //_eventAggregator = eventAggregator;
        }

        private void SelectLanguage(ListItemViewModel listItemViewModel)
        {
            LanguageDetailViewModel languageDetailViewModel = (LanguageDetailViewModel) listItemViewModel;

            if (languageDetailViewModel == null) return;

            if (!languageDetailViewModel.IsSelected)
            {
                languageDetailViewModel.SetSelected( true);
                return;
            }

            LocalizationService.CurrentLanguage = languageDetailViewModel.Language;
            //_eventAggregator.Publish<Events.OnLanguageChangedEvent>();
            this.Close();
        }

        public List<LanguageDetailViewModel> Languages { get; private set; }

        public new Parameter ClassParameter => (Parameter)base.ClassParameter;

        public override void Start()
        {
            base.Start();

            Languages = LocalizationService.SupportedLanguages.Select(l => new LanguageDetailViewModel(l, SelectLanguage)).ToList();
            var currentLanguage = LocalizationService.CurrentLanguage;
            var language = Languages.Single(l => l.Language == currentLanguage);
            language.SetSelected(true);
        }

        public class Parameter : BaseViewModelParameter
        {
            public Parameter(bool isModal)
                : base(isModal)
            {
            }

            public Action OnClose { get; set; }
        }
    }
}