using System;
using PSY.Innovative.Models;

namespace PSY.Innovative.ViewModels
{
    public class LanguageDetailViewModel : ListItemViewModel
    {
        public LanguageDetailViewModel(Language language, Action<ListItemViewModel> selectItemAction)
            : base(selectItemAction, null)
        {
            Language = language;
            Name = language.Name.ToUpper();
            if (Language.CultureInfo.Name.Contains("-"))
                ItemIconSource = "flag_" +
                       Language.CultureInfo.Name.ToLower().Substring(Language.CultureInfo.Name.IndexOf('-') + 1, 2) +
                       ".png";
            else
                ItemIconSource = "flag_" + Language.CultureInfo.Name.ToLower() + ".png";
        }

        public Language Language { get; private set; }
    }
}