using PSY.Innovative.ViewModels;
using Xamarin.Forms;

namespace PSY.Innovative.Views
{
    public partial class MasterDetailView 
    {
        public MasterDetailView()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            SetBinding(TitleProperty, new Binding("Title"));

            var vm = BindingContext as BaseViewModel;
            if (vm != null)
                vm.Loaded();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var vm = BindingContext as BaseViewModel;
            if (vm == null) return;
            
            vm.Resumed();
        }

        
    }
}