using System.Windows.Input;
using Xamarin.Forms;

namespace PSY.Innovative.Controls
{
    public class CarouselViewControl : CarouselView.FormsPlugin.Abstractions.CarouselViewControl
    {
        public CarouselViewControl()
        {
            PositionSelected += (sender, eventArgs) =>
            {
                if (PositionSelectedCommand != null && PositionSelectedCommand.CanExecute(null))
                    PositionSelectedCommand.Execute(new SelectedPositionChangedEventArgs(Position));
            };
        }

        public static readonly BindableProperty PositionSelectedCommandProperty = BindableProperty.Create(nameof(PositionSelectedCommand), typeof(ICommand), typeof(CarouselViewControl), null);

        public ICommand PositionSelectedCommand
        {
            get { return (ICommand) GetValue(PositionSelectedCommandProperty); }
            set { SetValue(PositionSelectedCommandProperty, value); }
        }
    }
}
