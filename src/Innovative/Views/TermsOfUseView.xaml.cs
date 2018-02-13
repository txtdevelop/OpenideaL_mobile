namespace PSY.Innovative.Views
{
    public partial class TermsOfUseView
    {
        public TermsOfUseView()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            //has to be bind manually
            TermsOfUseWebView.BindingContext = BindingContext;
        }
    }
}