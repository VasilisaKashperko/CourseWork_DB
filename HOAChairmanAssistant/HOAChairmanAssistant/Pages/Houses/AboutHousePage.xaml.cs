using GalaSoft.MvvmLight.Ioc;
using HOAChairmanAssistant.DataLayer.EF;
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
    /// Логика взаимодействия для AboutHousePage.xaml
    /// </summary>
    public partial class AboutHousePage : Page
    {
        private HOAChairmanAssistantContext context = new HOAChairmanAssistantContext();
        public AboutHousePage()
        {
            DataContext = new AboutHouseViewModel(SimpleIoc.Default.GetInstance<IFrameNavigationService>());
            InitializeComponent();
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

        private void DeleteButton_Loaded(object sender, RoutedEventArgs e)
        {
            var userch = context.Users.Where(z => z.UserId == GlobalData.UserId).FirstOrDefault();
            var userAcc = context.Users.Where(h => h.UserId == userch.AccountantId).FirstOrDefault();
            if (userAcc != null)
            {
                GlobalData.IsAccountant = true;
                DeleteButton.Visibility = Visibility.Hidden;
                AddInfo.Visibility = Visibility.Hidden;
                ForCh.Visibility = Visibility.Visible;
                ForAcc.Visibility = Visibility.Hidden;
            }
            else
            {
                GlobalData.IsAccountant = false;
                DeleteButton.Visibility = Visibility.Visible;
                AddInfo.Visibility = Visibility.Visible;
                ForAcc.Visibility = Visibility.Visible;
                ForCh.Visibility = Visibility.Hidden;
            }
        }
    }
}
