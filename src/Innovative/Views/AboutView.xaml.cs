namespace PSY.Innovative.Views
{
    public partial class AboutView
    {
        public AboutView()
        {
            InitializeComponent();
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            //has to be bind manually
            AboutWebView.BindingContext = BindingContext;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //TestLabel.FontSize = 36;
            //await Task.Run(async () =>
            //    {
            //        for (int i = 0; i < 20; i++)
            //        {						
            //            await Task.Delay(500).ConfigureAwait(false);

            //            if (i == 20)
            //            {
            //                //i = 0;
            //            }

            //            Device.BeginInvokeOnMainThread(() =>
            //                TestLabel.Text = i.ToString());
            //        }
            //    }).ConfigureAwait(false);

            //Device.BeginInvokeOnMainThread (() =>
            //    TestLabel.FontSize = 24);
        }
    }
}