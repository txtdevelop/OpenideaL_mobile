namespace PSY.Innovative.Views
{
    public partial class LicencesView
    {
        public LicencesView()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            //has to be bind manually
            LicencesWebView.BindingContext = BindingContext;
        }
    }
}