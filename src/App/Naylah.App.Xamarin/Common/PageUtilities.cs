﻿using Naylah.App.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Naylah.App.Common
{
    public static class PageUtilities
    {
        public static void InvokeViewAndViewModelAction<T>(object view, Action<T> action) where T : class
        {
            T viewAsT = view as T;
            if (viewAsT != null)
                action(viewAsT);

            var element = view as BindableObject;
            if (element != null)
            {
                var viewModelAsT = element.BindingContext as T;
                if (viewModelAsT != null)
                {
                    action(viewModelAsT);
                }
            }
        }

        public static void DestroyPage(Page page)
        {
            try
            {
                DestroyChildren(page);

                InvokeViewAndViewModelAction<IDestructible>(page, v => v.Destroy());

                page.Behaviors?.Clear();
                page.BindingContext = null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Cannot destroy {page}.", ex);
            }
        }

        private static void DestroyChildren(Page page)
        {
            if (page is MasterDetailPage)
            {
                DestroyPage(((MasterDetailPage)page).Master);
                DestroyPage(((MasterDetailPage)page).Detail);
            }
            else if (page is TabbedPage)
            {
                var tabbedPage = (TabbedPage)page;
                foreach (var item in tabbedPage.Children.Reverse())
                {
                    DestroyPage(item);
                }
            }
            else if (page is CarouselPage)
            {
                var carouselPage = (CarouselPage)page;
                foreach (var item in carouselPage.Children.Reverse())
                {
                    DestroyPage(item);
                }
            }
            else if (page is NavigationPage)
            {
                var navigationPage = (NavigationPage)page;
                foreach (var item in navigationPage.Navigation.NavigationStack.Reverse())
                {
                    DestroyPage(item);
                }
            }
        }

        public static void DestroyWithModalStack(Page page, IList<Page> modalStack)
        {
            foreach (var childPage in modalStack.Reverse())
            {
                DestroyPage(childPage);
            }
            DestroyPage(page);
        }

        //public static Task<bool> CanNavigateAsync(object page, object parameters)
        //{
        //    var confirmNavigationItem = page as IConfirmNavigationAsync;
        //    if (confirmNavigationItem != null)
        //        return confirmNavigationItem.CanNavigateAsync(parameters);

        // var bindableObject = page as BindableObject; if (bindableObject != null) { var
        // confirmNavigationBindingContext = bindableObject.BindingContext as
        // IConfirmNavigationAsync; if (confirmNavigationBindingContext != null) return
        // confirmNavigationBindingContext.CanNavigateAsync(parameters); }

        //    return Task.FromResult(CanNavigate(page, parameters));
        //}

        //public static bool CanNavigate(object page, object parameters)
        //{
        //    var confirmNavigationItem = page as IConfirmNavigation;
        //    if (confirmNavigationItem != null)
        //        return confirmNavigationItem.CanNavigate(parameters);

        // var bindableObject = page as BindableObject; if (bindableObject != null) { var
        // confirmNavigationBindingContext = bindableObject.BindingContext as IConfirmNavigation; if
        // (confirmNavigationBindingContext != null) return
        // confirmNavigationBindingContext.CanNavigate(parameters); }

        //    return true;
        //}

        public static void OnNavigatedFrom(object page, object parameters)
        {
            if (page != null)
                InvokeViewAndViewModelAction<INavigable>(page, v => v.OnNavigatedFromAsync(parameters));
        }

        public static void OnNavigatingTo(object page, object parameters, NavigationMode navigationMode = NavigationMode.New)
        {
            if (page != null)
                InvokeViewAndViewModelAction<INavigable>(page, v => v.OnNavigatingToAsync(parameters, navigationMode));
        }

        public static void OnNavigatedTo(object page, object parameters, NavigationMode navigationMode = NavigationMode.New)
        {
            if (page != null)
                InvokeViewAndViewModelAction<INavigable>(page, v => v.OnNavigatedToAsync(parameters, navigationMode));
        }

        public static Page GetOnNavigatedToTarget(Page page, Page mainPage, bool useModalNavigation)
        {
            Page target = null;

            if (useModalNavigation)
            {
                var previousPage = GetPreviousPage(page, page.Navigation.ModalStack);

                //MainPage is not included in the navigation stack, so if we can't find the previous page above
                //let's assume they are going back to the MainPage
                target = GetOnNavigatedToTargetFromChild(previousPage ?? mainPage);
            }
            else
            {
                target = GetPreviousPage(page, page.Navigation.NavigationStack);
                if (target != null)
                    target = GetOnNavigatedToTargetFromChild(target);
                else
                    target = GetOnNavigatedToTarget(page, mainPage, true);
            }

            return target;
        }

        public static Page GetOnNavigatedToTargetFromChild(Page target)
        {
            Page child = null;

            if (target is MasterDetailPage)
                child = ((MasterDetailPage)target).Detail;
            else if (target is TabbedPage)
                child = ((TabbedPage)target).CurrentPage;
            else if (target is CarouselPage)
                child = ((CarouselPage)target).CurrentPage;
            else if (target is NavigationPage)
                child = target.Navigation.NavigationStack.Last();

            if (child != null)
                target = GetOnNavigatedToTargetFromChild(child);

            return target;
        }

        public static Page GetPreviousPage(Page currentPage, System.Collections.Generic.IReadOnlyList<Page> navStack)
        {
            Page previousPage = null;

            int currentPageIndex = GetCurrentPageIndex(currentPage, navStack);
            int previousPageIndex = currentPageIndex - 1;
            if (navStack.Count >= 0 && previousPageIndex >= 0)
                previousPage = navStack[previousPageIndex];

            return previousPage;
        }

        public static int GetCurrentPageIndex(Page currentPage, System.Collections.Generic.IReadOnlyList<Page> navStack)
        {
            int stackCount = navStack.Count;
            for (int x = 0; x < stackCount; x++)
            {
                var view = navStack[x];
                if (view == currentPage)
                    return x;
            }

            return stackCount - 1;
        }

        public static Page GetCurrentPage(Page mainPage)
        {
            var page = mainPage;

            var lastModal = page.Navigation.ModalStack.LastOrDefault();
            if (lastModal != null)
                page = lastModal;

            return GetOnNavigatedToTargetFromChild(page);
        }

        public static void HandleSystemGoBack(Page previousPage, Page currentPage)
        {
            var parameters = new object();
            //parameters.GetNavigationParametersInternal().Add(KnownInternalParameters.NavigationMode, NavigationMode.Back);
            OnNavigatedFrom(previousPage, parameters);
            OnNavigatedTo(GetOnNavigatedToTargetFromChild(currentPage), parameters);
            DestroyPage(previousPage);
        }

        internal static bool HasDirectNavigationPageParent(Page page)
        {
            return page?.Parent != null && page?.Parent is NavigationPage;
        }

        internal static bool HasNavigationPageParent(Page page)
        {
            if (page?.Parent != null)
            {
                if (page.Parent is NavigationPage)
                {
                    return true;
                }
                else if (page.Parent is TabbedPage || page.Parent is CarouselPage)
                {
                    return page.Parent.Parent != null && page.Parent.Parent is NavigationPage;
                }
            }

            return false;
        }

        internal static bool IsSameOrSubclassOf<T>(Type potentialDescendant)
        {
            if (potentialDescendant == null)
                return false;

            Type potentialBase = typeof(T);

            return potentialDescendant.GetTypeInfo().IsSubclassOf(potentialBase)
                   || potentialDescendant == potentialBase;
        }

        public static bool? HandleGoBackForPage(Page page)
        {
            var p = page as IPageBackHandler;

            if (p != null)
            {
                return p.HandleBack?.Invoke();
            }

            return false;
        }
    }
}