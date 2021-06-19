using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;

namespace satCompent.ViewModel
{
    public class NavigationService : ViewModelBase, INavigationService
    {
        #region Field
        private readonly Dictionary<string, Uri> pagesByKey;
        private readonly List<string> historic;
        private string currentPageKey;
        #endregion

        #region Property
        public string CurrentPageKey
        {
            get { return currentPageKey; }
            set 
            {
                Set(() => CurrentPageKey, ref currentPageKey, value);
            }
        }
        public object Parameter { get; private set; }
        #endregion

        #region Constructor and Method
        public NavigationService()
        {
            pagesByKey = new Dictionary<string, Uri>();
            historic = new List<string>();
        }

        public void Configure(string key, Uri pageType)
        {
            lock (pagesByKey)
            {
                if (pagesByKey.ContainsKey(key))
                    pagesByKey[key] = pageType;
                else
                    pagesByKey.Add(key, pageType);
            }
        }

        public void GoBack()
        {
            if (historic.Count > 1)
            {
                historic.RemoveAt(historic.Count - 1);
                NavigateTo(historic.Last(), "Back");
            }
        }

        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, "Next");
        }

        public virtual void NavigateTo(string pageKey, object parameter)
        {
            lock (pagesByKey)
            {
                if (!pagesByKey.ContainsKey(pageKey))
                    throw new ArgumentException(string.Format("No Such Page: {0}", pageKey), "pageKey");

                var frame = GetDescendantFromName(Application.Current.MainWindow, "MainFrame") as Frame;
                if (frame != null)
                    frame.Source = pagesByKey[pageKey];
                Parameter = parameter;
                if ("Next".Equals(Convert.ToString(parameter)))
                    historic.Add(pageKey);
                CurrentPageKey = pageKey;
            }
        }

        private static FrameworkElement GetDescendantFromName(DependencyObject parent, string name)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            if (count < 1)
                return null;

            for (int i = 0; i < count; i++)
            {
                var frameworkElement = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                if (frameworkElement != null)
                {
                    if (frameworkElement.Name == name)
                        return frameworkElement;

                    frameworkElement = GetDescendantFromName(frameworkElement, name);
                    if (frameworkElement != null)
                        return frameworkElement;
                }
            }

            return null;
        }
        #endregion

    }
}
