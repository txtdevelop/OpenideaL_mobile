using Xamarin.Forms;

namespace PSY.Innovative.Utils
{
    public static class ViewUtils
    {
        public static Point PositionOnScreen(View parameterView)
        {
            VisualElement view = parameterView;

            var position = new Point();

            while (view.Parent is VisualElement)
            {
                position.X += view.X;
                position.Y += view.Y;
                view = (VisualElement)view.Parent;
            }
            return position;
        }
    }
}
