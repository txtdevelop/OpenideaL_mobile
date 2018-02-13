namespace PSY.Innovative.Views
{
    public partial class PrivacyAgreementView
    {
        public PrivacyAgreementView()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            //has to be bind manually
            PrivacyAgreementWebView.BindingContext = BindingContext;
        }
    }
}