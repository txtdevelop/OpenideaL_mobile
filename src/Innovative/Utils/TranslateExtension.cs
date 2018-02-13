using System;
using MvvmCross.Platform;
using PSY.Innovative.Contracts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSY.Innovative.Utils
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        private readonly ILocalizationService _localizationService;

        public TranslateExtension()
        {
            _localizationService = Mvx.Resolve<ILocalizationService>();
        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            return _localizationService.GetString(Text);
        }
    }
}