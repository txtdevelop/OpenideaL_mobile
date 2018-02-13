using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PSY.Innovative.Controls
{
    public class HideableToolbarItem : ToolbarItem
    {
        public static readonly BindableProperty IsVisibleProperty =
            BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(HideableToolbarItem), false, propertyChanged: OnIsVisibleChanged);

        public HideableToolbarItem()
        {
            InitVisibility();
        }

        public new ContentPage Parent { set; get; }
        private int Index { get; set; }

        public bool IsVisible
        {
            get { return (bool) GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        private async void InitVisibility()
        {
            await Task.Delay(10);
            OnIsVisibleChanged(this, false, IsVisible);
        }

        private static void OnIsVisibleChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var item = bindable as HideableToolbarItem;

            if (item.Parent == null)
                return;

            var items = item.Parent.ToolbarItems;

            var newValue = (bool)newvalue;

            var itemIndex = items.IndexOf(item);
            if (newValue && itemIndex < 0)
            {
                items.Insert(Math.Min(item.Index, items.Count), item);
            }
            else if (!newValue && itemIndex >= 0)
            {
                item.Index = itemIndex;
                items.Remove(item);
            }
        }
    }
}