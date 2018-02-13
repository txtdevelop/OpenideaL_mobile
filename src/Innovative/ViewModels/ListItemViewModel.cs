using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace PSY.Innovative.ViewModels
{
    public class ListItemViewModel : MvxViewModel
    {
        public Action<ListItemViewModel> SelectItemAction { get; set;}
        public Action<ListItemViewModel> ClickItemAction { get; set;}

        private bool _isSelected;
        public ICommand ClickItemCommand { get; }

        public ListItemViewModel(Action<ListItemViewModel> selectItemAction = null, Action<ListItemViewModel> clickItemAction = null)
        {
            SelectItemAction = selectItemAction;
            ClickItemAction = clickItemAction;

            ClickItemCommand = new MvxCommand(ClickItem);
        }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public string ItemIconSource { get; set; }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected == value)
                    return;

                _isSelected = value;

                SelectItemAction?.Invoke(this);
            }
        }

        public bool IsSelectingAllowed { get; set; }

        public void SetSelected(bool isSelected)
        {
            _isSelected = isSelected;
            RaisePropertyChanged(nameof(IsSelected));
        }
        
        private void ClickItem()
        {
            ClickItemAction?.Invoke(this);
        }

        public string Icon => ItemIconSource;
    }
}
