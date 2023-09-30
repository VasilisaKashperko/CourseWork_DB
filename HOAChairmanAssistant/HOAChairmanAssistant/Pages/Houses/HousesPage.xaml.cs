using GalaSoft.MvvmLight.Ioc;
using HOAChairmanAssistant.Helpers.GlobalData;
using HOAChairmanAssistant.Helpers.Navigation;
using HOAChairmanAssistant.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HOAChairmanAssistant.Pages.Houses
{
    /// <summary>
    /// Логика взаимодействия для HousesPage.xaml
    /// </summary>
    public partial class HousesPage : Page
    {
        public HousesPage()
        {
            InitializeComponent();
            DataContext = new HousesViewModel(SimpleIoc.Default.GetInstance<IFrameNavigationService>());
        }
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)sender;
            if (e.Delta < 0)
            {
                scrollViewer.LineDown();
            }
            else
            {
                scrollViewer.LineUp();
            }
            e.Handled = true;
        }

        private void userNameLabel_Loaded(object sender, RoutedEventArgs e)
        {
            userNameLabel.Text = GlobalData.UserName;
        }

        private void accNameLabel_Loaded(object sender, RoutedEventArgs e)
        {
            accNameLabel.Text = GlobalData.AccountantName;
        }

        private void accSurnameLabel_Loaded(object sender, RoutedEventArgs e)
        {
            accSurnameLabel.Text = GlobalData.AccountantSurname;
        }

        private void Button_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Refresh();
        }
    }
}
