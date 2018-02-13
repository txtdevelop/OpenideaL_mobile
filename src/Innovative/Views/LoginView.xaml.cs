using PSY.Innovative.ViewModels;
using Xamarin.Forms;

namespace PSY.Innovative.Views
{
    public partial class LoginView
    {


        public LoginView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            //var vm = BindingContext as LoginViewModel;

            UsernameEntry.Completed += (s, e) => PasswordEntry.Focus();
            //PasswordEntry.Completed += (s, e) => vm.LoginClick.ExecKCute(null);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
