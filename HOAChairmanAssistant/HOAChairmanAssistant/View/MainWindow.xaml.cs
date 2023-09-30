using HOAChairmanAssistant.Pages;
using HOAChairmanAssistant.ViewModel;
using System;
using System.Windows;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HOAChairmanAssistant.DataLayer.EF;
using GalaSoft.MvvmLight.Ioc;
using HOAChairmanAssistant.Helpers.Navigation;
using System.Threading;
using HOAChairmanAssistant.Pages.Houses;
using GalaSoft.MvvmLight.Messaging;
using HOAChairmanAssistant.Helpers.MessageWindow;
using GalaSoft.MvvmLight.Threading;
using HOAChairmanAssistant.Helpers.GlobalData;
using HOAChairmanAssistant.Pages.Accountant;

namespace HOAChairmanAssistant.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HOAChairmanAssistantContext context = new HOAChairmanAssistantContext();
        public MainWindow()
        {
            DataContext = new MainWindowViewModel(SimpleIoc.Default.GetInstance<IFrameNavigationService>());
            InitializeComponent();
        }
        private void ButtonShutDown_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MainFrame_Loaded(object sender, RoutedEventArgs e)
        {
            if (GlobalData.IsChairman == true)
            {
                MainFrame.NavigationService.Navigate(new HousesPage());
            }
            if(GlobalData.IsAccountant == true)
            {
                MainFrame.NavigationService.Navigate(new AccountantHomePage());
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string oracleConnectionString = "Data Source=localhost:1521/orcl1;User Id=C##Vasilisa;Password=Pa$$w0rd;";
            string mssqlConnectionString = "Data Source= VASILISIK;initial catalog=HOAChairmanAssistantDB;integrated security=True;";

            string oracleTableUsers = "USERS";
            string mssqlTableUsers = "dbo.Users";

            string oracleTableAddresses = "ADDRESSES";
            string mssqlTableAddresses = "dbo.Addresses";

            string oracleTableHouses = "HOUSES";
            string mssqlTableHouses = "dbo.Houses";

            string oracleTablePorches = "PORCHES";
            string mssqlTablePorches = "dbo.Porches";

            string oracleTableFlats = "FLATS";
            string mssqlTableFlats = "dbo.Flats";

            string oracleTableOwners = "OWNERS";
            string mssqlTableOwners = "dbo.Owners";

            string oracleTablePhoneNumbers = "PHONENUMBERS";
            string mssqlTablePhoneNumbers = "dbo.PhoneNumbers";

            string oracleTableContacts = "CONTACTS";
            string mssqlTableContacts = "dbo.Contacts";


            DataLayer.DataMigrator.MoveDataFromMSSQLToOracle(mssqlConnectionString, oracleConnectionString, mssqlTableUsers, oracleTableUsers);
            DataLayer.DataMigrator.MoveDataFromMSSQLToOracle(mssqlConnectionString, oracleConnectionString, mssqlTableAddresses, oracleTableAddresses);
            DataLayer.DataMigrator.MoveDataFromMSSQLToOracle(mssqlConnectionString, oracleConnectionString, mssqlTableHouses, oracleTableHouses);
            DataLayer.DataMigrator.MoveDataFromMSSQLToOracle(mssqlConnectionString, oracleConnectionString, mssqlTablePorches, oracleTablePorches);
            DataLayer.DataMigrator.MoveDataFromMSSQLToOracle(mssqlConnectionString, oracleConnectionString, mssqlTableFlats, oracleTableFlats);
            DataLayer.DataMigrator.MoveDataFromMSSQLToOracle(mssqlConnectionString, oracleConnectionString, mssqlTableOwners, oracleTableOwners);
            DataLayer.DataMigrator.MoveDataFromMSSQLToOracle(mssqlConnectionString, oracleConnectionString, mssqlTablePhoneNumbers, oracleTablePhoneNumbers);
            DataLayer.DataMigrator.MoveDataFromMSSQLToOracle(mssqlConnectionString, oracleConnectionString, mssqlTableContacts, oracleTableContacts);

            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (GlobalData.UserId != 0) {
                var userch = context.Users.Where(z => z.UserId == GlobalData.UserId).FirstOrDefault();
                var userAcc = context.Users.Where(h => h.UserId == userch.AccountantId).FirstOrDefault();
                if (userAcc != null)
                {
                    GlobalData.IsAccountant = true;
                    Contacts.Visibility = Visibility.Hidden;
                    Phones.Visibility = Visibility.Hidden;
                    Houses.Visibility = Visibility.Hidden;
                }
                else
                {
                    GlobalData.IsAccountant = false;
                    Contacts.Visibility = Visibility.Visible;
                    Phones.Visibility = Visibility.Visible;
                    Houses.Visibility = Visibility.Visible;
                }
                var userChairman = context.Users.Where(g => g.UserId == userch.AccountantId).FirstOrDefault();
                if (userChairman == null)
                {
                    GlobalData.IsChairman = true;
                    Contacts.Visibility = Visibility.Visible;
                    Phones.Visibility = Visibility.Visible;
                    Houses.Visibility = Visibility.Visible;
                }
                else
                {
                    GlobalData.IsChairman = false;
                    Contacts.Visibility = Visibility.Hidden;
                    Phones.Visibility = Visibility.Hidden;
                    Houses.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
