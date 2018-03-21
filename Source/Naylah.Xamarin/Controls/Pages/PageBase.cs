using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.Xamarin.Controls.Pages
{
    public class PageBase : Page, IPageBase
    {
        public static BindableProperty IsLoadingProperty =
         BindableProperty.Create(nameof(IsLoading), typeof(bool), typeof(PageBase), default(bool));

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public bool? HandleBack { get; set; } = false;

        protected override bool OnBackButtonPressed()
        {
            return HandleBack != null ? HandleBack.Value : false;
        }
    }

    public class ContentPageBase : ContentPage, IPageBase
    {
        public ContentPageBase()
        {
            OnCreate();
        }

        public virtual void OnCreate()
        {
        }

        public static BindableProperty IsLoadingProperty =
         BindableProperty.Create(nameof(IsLoading), typeof(bool), typeof(ContentPage), default(bool));

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public bool? HandleBack { get; set; } = false;

        protected override bool OnBackButtonPressed()
        {
            return HandleBack != null ? HandleBack.Value : false;
        }
    }

    public class TabbedPageBase : TabbedPage, IPageBase
    {
        public static BindableProperty IsLoadingProperty =
         BindableProperty.Create(nameof(IsLoading), typeof(bool), typeof(TabbedPageBase), default(bool));

        public TabbedPageBase()
        {
            OnCreate();
        }

        public virtual void OnCreate()
        {
        }

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public bool? HandleBack { get; set; } = false;

        protected override bool OnBackButtonPressed()
        {
            return HandleBack != null ? HandleBack.Value : false;
        }
    }

    public interface IPageBase
    {
        bool? HandleBack { get; set; }

        bool IsLoading { get; set; }
    }
}