using Naylah.Xamarin.Converters;
using Naylah.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.Xamarin.Cells
{
    public class InfoDetailCell : ViewCell
    {
        public InfoDetailCell()
        {
            var icon = new Image
            {
                Aspect = Aspect.AspectFit
            };
            icon.SetBinding(Image.SourceProperty, Binding.Create<InfoDetail>(obj => obj.Image));
            icon.SetBinding(Image.IsVisibleProperty, Binding.Create<InfoDetail>(obj => obj.Image, BindingMode.Default, new NullToVisibilityConverter()));

            var title = new Label
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.StartAndExpand
            };
            title.SetBinding(Label.TextProperty, Binding.Create<InfoDetail>(obj => obj.Title));
            title.SetBinding(Label.IsVisibleProperty, Binding.Create<InfoDetail>(obj => obj.Title, BindingMode.Default, new NullToVisibilityConverter()));

            var description = new Label
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                FontSize = 14
            };
            description.SetBinding(Label.TextProperty, Binding.Create<InfoDetail>(obj => obj.Description));
            description.SetBinding(Label.IsVisibleProperty, Binding.Create<InfoDetail>(obj => obj.Description, BindingMode.Default, new NullToVisibilityConverter()));

            View = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Spacing = 4,
                Padding = 16,
                Children =
                {
                   title,
                   description,
                   icon
                }
            };
        }
    }
}
