using HOAChairmanAssistant.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HOAChairmanAssistant.Helpers.Navigation
{
    public class FrameNavigationService : IFrameNavigationService, INotifyPropertyChanged
    {
        #region Fields
        private readonly Dictionary<string, Uri> _pagesByKey;
        private readonly List<string> _historic;
        private string _currentPageKey;
        #endregion

        #region Properties                                              
        public string CurrentPageKey
        {
            get
            {
                return _currentPageKey;
            }

            private set
            {
                if (_currentPageKey == value)
                {
                    return;
                }

                _currentPageKey = value;
                OnPropertyChanged("CurrentPageKey");
            }
        }
        public object Parameter { get; private set; }
        public object Parameter2 { get; private set; }
        #endregion

        #region Ctors and Methods
        public FrameNavigationService()
        {
            _pagesByKey = new Dictionary<string, Uri>();
            _historic = new List<string>();
        }
        public void GoBack()
        {
            if (_historic.Count > 1)
            {
                _historic.RemoveAt(_historic.Count - 1);
                NavigateTo(_historic.Last(), null);
            }/*
            else
            {
                MessageBox.Show("You are at Main Page");
            }*/
        }
        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        public virtual void NavigateTo(string pageKey, object parameter)
        {
            lock (_pagesByKey)
            {
                if (!_pagesByKey.ContainsKey(pageKey))
                {
                    throw new ArgumentException(string.Format("No such page: {0} ", pageKey), "pageKey");
                }
                Frame frame = GetDescendantFromName(Application.Current.Windows[0], "MainFrame") as Frame;
                if (Application.Current.Windows.Count > 2)
                {
                    if (Application.Current.Windows[2].Name == "MainWindow")
                        frame = GetDescendantFromName(Application.Current.Windows[2], "MainFrame") as Frame;
                }


                if (frame != null)
                {
                    frame.Source = _pagesByKey[pageKey];
                }
                Parameter = parameter;
                _historic.Add(pageKey);
                CurrentPageKey = pageKey;
            }
        }

        public virtual void NavigateTo(string pageKey, object parameter, object parameter2)
        {
            lock (_pagesByKey)
            {
                if (!_pagesByKey.ContainsKey(pageKey))
                {
                    throw new ArgumentException(string.Format("No such page: {0} ", pageKey), "pageKey");
                }
                Frame frame = GetDescendantFromName(Application.Current.Windows[0], "MainFrame") as Frame;
                if (Application.Current.Windows.Count > 2)
                {
                    if (Application.Current.Windows[2].Name == "MainWindow")
                        frame = GetDescendantFromName(Application.Current.Windows[2], "MainFrame") as Frame;
                }


                if (frame != null)
                {
                    frame.Source = _pagesByKey[pageKey];
                }
                Parameter = parameter;
                Parameter2 = parameter2;
                _historic.Add(pageKey);
                CurrentPageKey = pageKey;
            }
        }

        public void NavigateTo(string pageKey, House aboutHouse, Flat flat)
        {
            lock (_pagesByKey)
            {
                if (!_pagesByKey.ContainsKey(pageKey))
                {
                    throw new ArgumentException(string.Format("No such page: {0} ", pageKey), "pageKey");
                }
                Frame frame = GetDescendantFromName(Application.Current.Windows[0], "MainFrame") as Frame;
                if (Application.Current.Windows.Count > 2)
                {
                    if (Application.Current.Windows[2].Name == "MainWindow")
                        frame = GetDescendantFromName(Application.Current.Windows[2], "MainFrame") as Frame;
                }


                if (frame != null)
                {
                    frame.Source = _pagesByKey[pageKey];
                }
                Parameter = aboutHouse;
                Parameter2 = flat;
                _historic.Add(pageKey);
                CurrentPageKey = pageKey;
            }
        }

        public void Configure(string key, Uri pageType)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(key))
                {
                    _pagesByKey[key] = pageType;
                }
                else
                {
                    _pagesByKey.Add(key, pageType);
                }
            }
        }

        private static FrameworkElement GetDescendantFromName(DependencyObject parent, string name)
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);

            if (count < 1)
            {
                return null;
            }

            for (var i = 0; i < count; i++)
            {
                var frameworkElement = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                if (frameworkElement != null)
                {
                    if (frameworkElement.Name == name)
                    {
                        return frameworkElement;
                    }

                    frameworkElement = GetDescendantFromName(frameworkElement, name);
                    if (frameworkElement != null)
                    {
                        return frameworkElement;
                    }
                }
            }
            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
