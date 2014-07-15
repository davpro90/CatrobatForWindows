﻿using System;
using System.Linq;
using System.Reflection;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Catrobat.IDE.Core.Services;
using Catrobat.IDE.Core.ViewModels;
using Catrobat.IDE.WindowsPhone.Views;
using Catrobat.IDE.WindowsShared.Common;

namespace Catrobat.IDE.WindowsShared.Services
{
    public class NavigationServiceWindowsShared : INavigationService
    {
        private readonly Frame _frame;
        private int _removedBackEntryCount;

        public NavigationServiceWindowsShared(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateTo<T>()
        {
            NavigateTo(typeof(T));
        }

        public void NavigateTo(Type type)
        {
            NavigateBack();

            Type pageType = null;

            if (type.GetTypeInfo().BaseType == typeof(ViewModelBase))
            {
                var viewModelInstance = (ViewModelBase)ServiceLocator.GetInstance(type);

                if (viewModelInstance.SkipAndNavigateTo != null)
                    type = viewModelInstance.SkipAndNavigateTo;

                pageType = viewModelInstance.PresenterType;

                if (pageType == null)
                {
                    var viewModelName = type.GetTypeInfo().AssemblyQualifiedName.Split(',').First();
                    var viewName = viewModelName.Replace("Catrobat.IDE.Core.ViewModels", "Catrobat.IDE.WindowsPhone.Views");
                    viewName = viewName.Replace("ViewModel", "View");
                    pageType = Type.GetType(viewName);
                }
            }
            else if (type.GetTypeInfo().BaseType == typeof (ViewPageBase))
            {
                pageType = type;
            }

            if (pageType.GetTypeInfo().BaseType == typeof(ViewPageBase)) // this is not true for flyouts (UserControls)
                _frame.Navigate(pageType);
        }

        public void NavigateBack(object navigationObject)
        {
            var flyout = navigationObject as Flyout;
            if (flyout != null)
                flyout.Hide();
            else
            {
                _removedBackEntryCount++;
                NavigateBack();
            }
        }

        public void NavigateBackForPlatform(NavigationPlatform platform)
        {
            if (platform == NavigationPlatform.WindowsStore)
            {
                _removedBackEntryCount++;
                NavigateBack();
            }
        }

        private void NavigateBack()
        {
            while (_removedBackEntryCount > 0)
            {
                _removedBackEntryCount--;
                if (CanGoBack)
                    _frame.GoBack();
                else
                    Application.Current.Exit(); // TODO: remove this
                
            }
        }

        public void RemoveBackEntry()
        {
            _removedBackEntryCount++;
        }


        public void RemoveBackEntryForPlatform(NavigationPlatform platform)
        {
            //if (platform == NavigationPlatform.WindowsStore)
                _removedBackEntryCount++;
        }


        public bool CanGoBack
        {
            get { return _frame.CanGoBack; }
        }

        public void NavigateToWebPage(string uri)
        {
            Launcher.LaunchUriAsync(new Uri(uri));
        }
    }
}