using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.Xamarin.Controls.Customizations
{
    public class ImageGalleryView : ScrollView
    {
        readonly StackLayout _imageStack;
        int _selectedIndex;
        public event EventHandler<object> ItemClick;

        public ImageGalleryView()
        {
            this.Orientation = ScrollOrientation.Horizontal;

            _imageStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            this.Content = _imageStack;
        }

        public IList<View> Children
        {
            get
            {
                return _imageStack.Children;
            }
        }


        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IList),
                typeof(ImageGalleryView),
                null,
                propertyChanging: (bindableObject, oldValue, newValue) =>
                {
                    ((ImageGalleryView)bindableObject).ItemsSourceChanging();
                },
                propertyChanged: (bindableObject, oldValue, newValue) =>
                {
                    ((ImageGalleryView)bindableObject).ItemsSourceChanged();
                }
            );

        public IList ItemsSource
        {
            get
            {
                return (IList)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }


        void ItemsSourceChanging()
        {
            if (ItemsSource == null) return;
            _selectedIndex = ItemsSource.IndexOf(SelectedItem);
        }

        void ItemsSourceChanged()
        {
            _imageStack.Children.Clear();
            if (ItemsSource == null) return;

            foreach (var item in ItemsSource)
            {
                var view = (View)ItemTemplate.CreateContent();
                var bindableObject = view as BindableObject;
                if (bindableObject != null)
                    bindableObject.BindingContext = item;

                var tap = new TapGestureRecognizer();
                tap.Tapped += (s, e) => {
                    ItemClick.Invoke(s, item);
                };
                view.GestureRecognizers.Add(tap);

                _imageStack.Children.Add(view);
            }

            if (_selectedIndex >= 0) SelectedIndex = _selectedIndex;
        }

        public DataTemplate ItemTemplate
        {
            get;
            set;
        }

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create<ImageGalleryView, object>(
                view => view.SelectedItem,
                null,
                BindingMode.TwoWay,
                propertyChanged: (bindable, oldValue, newValue) => {
                    ((ImageGalleryView)bindable).UpdateSelectedIndex();
                }
            );

        public object SelectedItem
        {
            get
            {
                return GetValue(SelectedItemProperty);
            }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        void UpdateSelectedIndex()
        {
            if (SelectedItem == BindingContext)
                return;

            SelectedIndex = Children
                .Select(c => c.BindingContext)
                .ToList()
                .IndexOf(SelectedItem);

        }

        public static readonly BindableProperty SelectedIndexProperty =
            BindableProperty.Create<ImageGalleryView, int>(
                carousel => carousel.SelectedIndex,
                0,
                BindingMode.TwoWay,
                propertyChanged: (bindable, oldValue, newValue) => {
                    ((ImageGalleryView)bindable).UpdateSelectedItem();
                }
            );

        public int SelectedIndex
        {
            get
            {
                return (int)GetValue(SelectedIndexProperty);
            }
            set
            {
                SetValue(SelectedIndexProperty, value);
            }
        }

        void UpdateSelectedItem()
        {
            SelectedItem = SelectedIndex > -1 ? Children[SelectedIndex].BindingContext : null;
        }
    }
}
