using System;
using System.Collections.Concurrent;
using System.Windows.Input;
using Xamarin.Forms;

namespace PSY.Innovative.Controls.ListItem
{
    public partial class ListItemView : Grid
    {
        private View _parameterView;
        private BoxView _separatorLine;

        public static readonly BindableProperty IsSelectedProperty =
            BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(ListItemView), false, propertyChanged:
                (bindable, value, newValue) =>
                {
                    var control = (ListItemView)bindable;

                    //Logger.Trace($"Updating is selected for {control.Title.ToUpper()} from {value.ToString().ToUpper()} to {newValue.ToString().ToUpper()}");

                    if (value != newValue)
                    {
                        control.UpdateIcon();
                    }


                }, defaultBindingMode: BindingMode.TwoWay);

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly BindableProperty IsSelectingAllowedProperty =
            BindableProperty.Create(nameof(IsSelectingAllowed), typeof(bool), typeof(ListItemView), true, BindingMode.TwoWay);

        public bool IsSelectingAllowed
        {
            get {return (bool)GetValue(IsSelectingAllowedProperty);}
            set {SetValue(IsSelectingAllowedProperty, value);}
        }

        public ListItemView()
        {
            InitializeComponent();

            _separatorLine = new BoxView()
            {
                BackgroundColor = Color.FromRgb(240, 240, 240),
                HeightRequest = 1,

            };

            Children.Add(_separatorLine, 0, 1);
            SetColumnSpan(_separatorLine, 3);


            //default binding
            //SetBinding(IsSelectedProperty, new Binding(nameof(IsChecked)));

            DescriptionLabel.IsVisible = false;
            //IconContainer.IsVisible = false;
            ArrowImage.IsVisible = false;
            Icon.IsVisible = false;
            Icon.WidthRequest = 0;
            HasSeparator = true;


            IconType = IconType.SmallIcon;

            if (Device.RuntimePlatform == Device.iOS)
            {
                BackgroundColor = Color.White;
            }

        }

        public bool HasSeparator
        {
            get { return _separatorLine.IsVisible; }
            set { _separatorLine.IsVisible = value; }
        }

        public bool HasArrow
        {
            set { ArrowImage.IsVisible = value; }
        }

        private void OnItemClickGestureRecognizerTapped(object sender, EventArgs e)
        {
            if (ClickItemCommand != null)
            {
                if (ClickItemCommand.CanExecute(null))
                    ClickItemCommand.Execute(null);
            }           
            else OnItemIconGestureRecognizerTapped(sender, e);
        }

        private void OnItemIconGestureRecognizerTapped(object sender, EventArgs e)
        {
            if (IsSelectable)
            {
                IsSelected = !IsSelected;
            }
        }

        //public static readonly BindableProperty ClickNextCommandProperty =
        //    BindableProperty.Create(nameof(ClickNextCommand), typeof(ICommand), typeof(ListItemView), propertyChanged:
        //       (bindable, value, newValue) =>
        //       {
        //           var control = (ListItemView)bindable;

        //           control.ArrowImage.IsVisible = newValue != null;

        //           if (newValue != null)
        //           {
        //               ((ListItemView)bindable).RegisterItemTapGestureRecognizer();
        //           }
        //       });

        //public ICommand ClickNextCommand
        //{
        //    get { return (ICommand)GetValue(ClickNextCommandProperty); }
        //    set { SetValue(ClickNextCommandProperty, value); }
        //}

        public static readonly BindableProperty ClickItemCommandProperty =
           BindableProperty.Create(nameof(ClickItemCommand), typeof(ICommand), typeof(ListItemView), propertyChanged:
               (bindable, value, newValue) =>
               {
                   if (newValue != null)
                   {
                       ((ListItemView)bindable).RegisterItemTapGestureRecognizer();
                   }
               });

        private TapGestureRecognizer _itemTapGestureReconizer;
        public void RegisterItemTapGestureRecognizer()
        {
            if (_itemTapGestureReconizer == null)
            {
                _itemTapGestureReconizer = new TapGestureRecognizer();
                _itemTapGestureReconizer.Tapped += OnItemClickGestureRecognizerTapped;
                GestureRecognizers.Add(_itemTapGestureReconizer);
            }
        }

        public ICommand ClickItemCommand
        {
            get { return (ICommand)GetValue(ClickItemCommandProperty); }
            set { SetValue(ClickItemCommandProperty, value); }
        }

        public static readonly BindableProperty TitleProperty =
          BindableProperty.Create(nameof(Title), typeof(string), typeof(ListItemView), "", propertyChanged:
               (bindable, value, newValue) =>
               {
                   var control = (ListItemView)bindable;
                   control.TitleLabel.Text = (string)newValue;
               });

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly BindableProperty DescriptionProperty =
            BindableProperty.Create(nameof(Description), typeof(string), typeof(ListItemView), "", propertyChanged:
               (bindable, value, newValue) =>
               {
                   var control = (ListItemView)bindable;
                   var text = (string)newValue;

                   control.DescriptionLabel.Text = text;
                   control.DescriptionLabel.IsVisible = !string.IsNullOrEmpty(text);

               });

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly BindableProperty IconImageProperty =
            BindableProperty.Create(nameof(IconImage), typeof(string), typeof(ListItemView), "", propertyChanged:
               (bindable, value, newValue) =>
               {
                   var control = (ListItemView)bindable;
                   control.UpdateIcon();
               });

        public string IconImage
        {
            get { return (string)GetValue(IconImageProperty); }
            set { SetValue(IconImageProperty, value); }
        }

        public static readonly BindableProperty TitleTextColorProperty =
            BindableProperty.Create(nameof(TitleTextColor), typeof(Color), typeof(ListItemView), Styles.MainDarkTextColor, propertyChanged:
               (bindable, value, newValue) =>
               {
                   var control = (ListItemView)bindable;
                   control.TitleLabel.TextColor = (Color)newValue;
               });

        public Color TitleTextColor
        {
            get { return (Color)GetValue(TitleTextColorProperty); }
            set { SetValue(TitleTextColorProperty, value); }
        }

        public static readonly BindableProperty TitleTextFontAttributesProperty =
            BindableProperty.Create(nameof(TitleTextFontAttributes), typeof(FontAttributes), typeof(ListItemView), FontAttributes.None, propertyChanged:
               (bindable, value, newValue) =>
               {
                   var control = (ListItemView)bindable;
                   control.TitleLabel.FontAttributes = (FontAttributes)newValue;
               });

        public FontAttributes TitleTextFontAttributes
        {
            get { return (FontAttributes)GetValue(TitleTextFontAttributesProperty); }
            set { SetValue(TitleTextFontAttributesProperty, value); }
        }

        public IconType IconType { get; set; }

        public bool IsSelectable
        {
            get
            {
                return _selectionTapGestureReconizer != null && IsSelectingAllowed;
            }
            set
            {
                if (value)
                    RegisterSelectionTapGestureRecognizer();
            }
        }

        private TapGestureRecognizer _selectionTapGestureReconizer;
        public void RegisterSelectionTapGestureRecognizer()
        {
            if (_selectionTapGestureReconizer == null)
            {
                _selectionTapGestureReconizer = new TapGestureRecognizer();
                _selectionTapGestureReconizer.Tapped += OnItemIconGestureRecognizerTapped;
                IconContentView.GestureRecognizers.Add(_selectionTapGestureReconizer);
            }
        }

        public View ParameterView
        {
            get
            {
                return _parameterView;
            }
            set
            {
                if (value != null)
                {
                    _parameterView = value;

                    ItemGrid.Children.Add(value, 2, 0);
                }
            }
        }

        protected void UpdateIcon()
        {
            var fileSource = Icon.Source as FileImageSource;

            var newIcon = IsSelected ? "selected.png" : IconImage;
            if (fileSource != null && fileSource.File.Equals(newIcon))
            {
                //Logger.Trace($"No need to update image for {Title.ToUpper()}");
                return;
            }

            Icon.Source = GetOrCreateImage(newIcon);

            if (Icon.Source == null)
            {
                Icon.IsVisible = false;
                Icon.WidthRequest = 0;
                return;
            }

            if (Icon.IsVisible)
            {
                return;
            }

            switch (IconType)
            {
                case IconType.SmallIcon:
                    Icon.WidthRequest = 40;
                    Icon.HeightRequest = 40;
                    break;
                case IconType.MediumIcon:
                    Icon.WidthRequest = 50;
                    Icon.HeightRequest = 50;
                    break;
                case IconType.BigIcon:
                    Icon.WidthRequest = 60;
                    Icon.HeightRequest = 60;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Icon.IsVisible = true;
        }

        public static ImageSource GetOrCreateImage(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
                return default(ImageSource);

            if (!ImageSourceCache.ContainsKey(imageName))
            {
                ImageSourceCache[imageName] = ImageSource.FromFile(imageName);
            }

            return ImageSourceCache[imageName];
        }

        public static ConcurrentDictionary<string, ImageSource> ImageSourceCache = new ConcurrentDictionary<string, ImageSource>();
    }

    public enum IconType
    {
        SmallIcon,
        MediumIcon,
        BigIcon
    }
}