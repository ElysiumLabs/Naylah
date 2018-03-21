using Naylah.Xamarin.Controls.Pages;
using Naylah.Xamarin.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.Xamarin.Controls.Customizations
{

    [ContentProperty(nameof(Content))]
    public class ContentLoader : ContentView
    {

        private IPageBase _parentPage;

        public IPageBase ParentPage
        {
            get { return _parentPage; }
            set
            {
                _parentPage = value;
                if (ParentPage != null)
                {
                    holdingHandleBackPage = ParentPage.HandleBack;
                }
            }
        }


        private bool? holdingHandleBackPage;

        private Grid GridContent;

        public new static BindableProperty ContentProperty =
            BindableProperty.Create(nameof(Content), typeof(View), typeof(ContentLoader), default(View), propertyChanged: ContentPropertyChanged);

        private static void ContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var contentLoader = bindable as ContentLoader;
            var oldView = oldValue as View;
            var newView = newValue as View;

            contentLoader.UpdateContent(oldView, newView);

        }


        public new View Content
        {
            get { return (View)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static BindableProperty LoadingContentProperty =
            BindableProperty.Create(nameof(LoadingContent), typeof(View), typeof(ContentLoader), default(View), propertyChanged: LoadingContentPropertyChanged);

        private static void LoadingContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var contentLoader = bindable as ContentLoader;
            var oldView = oldValue as View;
            var newView = newValue as View;

            contentLoader.UpdateContent(oldView, newView);

        }

        public View LoadingContent
        {
            get { return (View)GetValue(LoadingContentProperty); }
            set { SetValue(LoadingContentProperty, value); }
        }

        public static BindableProperty IsLoadingProperty =
            BindableProperty.Create(nameof(IsLoading), typeof(bool), typeof(ContentLoader), default(bool));

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public static BindableProperty HideNavigationBarProperty =
            BindableProperty.Create(nameof(HideNavigationBar), typeof(bool), typeof(ContentLoader), default(bool));

        public bool HideNavigationBar
        {
            get { return (bool)GetValue(HideNavigationBarProperty); }
            set { SetValue(HideNavigationBarProperty, value); }
        }

        public static BindableProperty HandlePageBackProperty =
            BindableProperty.Create(nameof(HandlePageBack), typeof(bool), typeof(ContentLoader), default(bool));

        public bool HandlePageBack
        {
            get { return (bool)GetValue(HandlePageBackProperty); }
            set { SetValue(HandlePageBackProperty, value); }
        }



        private void UpdateContent(View oldView, View newView)
        {
            if (oldView != null)
            {
                GridContent.Children.Remove(oldView);
            }

            GridContent.Children.Add(newView);

            AdjustContent();
        }

        public ContentLoader(IPageBase parentPage) : this()
        {
            ParentPage = parentPage;
        }

        public ContentLoader()
        {
            PropertyChanged += ContentLoader_PropertyChanged;

            GridContent = new Grid();

            base.Content = GridContent;

            if ((Parent as IPageBase) != null)
            {
                holdingHandleBackPage = (Parent as IPageBase).HandleBack;
            }

            //CreateDefaultLoading();
        }

        private void ContentLoader_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (
                //(e.PropertyName == nameof(LoadingContent)
                //||
                //(e.PropertyName == nameof(Content))
                //||
                (e.PropertyName == nameof(IsLoading))
                //)
                )
            {
                AdjustContent();
            }

        }

        private void AdjustContent()
        {

            try
            {
                PropertyChanged -= ContentLoader_PropertyChanged;

                if (LoadingContent != null)
                {
                    LoadingContent.IsVisible = IsLoading;
                }

                if (Content != null)
                {
                    Content.IsVisible = !IsLoading;
                }

                if (ParentPage == null)
                {
                    TryGetParentAsPage();
                }

                if ((HandlePageBack) && (ParentPage != null))
                {
                    ParentPage.HandleBack = (IsLoading) ? true : holdingHandleBackPage;
                }

                if ((HideNavigationBar) && (ParentPage != null))
                {
                    NavigationPage.SetHasNavigationBar((BindableObject)ParentPage, !IsLoading);
                }


            }
            catch (Exception)
            {
            }
            finally
            {
                PropertyChanged += ContentLoader_PropertyChanged;
            }


        }

        private void TryGetParentAsPage()
        {
            try
            {
                ParentPage = Parent as IPageBase;
            }
            catch (Exception)
            {
            }
        }






        //public void CreateDefaultLoading()
        //{
        //    defaultLoadingContent = new StackLayout()
        //    {
        //        VerticalOptions = LayoutOptions.Center,
        //        HorizontalOptions = LayoutOptions.Center,
        //    };

        //    activityIndicator = new ActivityIndicator();

        //    defaultLoadingContent.Children.Add(activityIndicator);

        //    LoadingContent = defaultLoadingContent;

        //}
    }



}
