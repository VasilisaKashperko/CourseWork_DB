using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using HOAChairmanAssistant.Helpers.Locator;
using HOAChairmanAssistant.Helpers.MessageWindow;
using HOAChairmanAssistant.Model;
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
using System.Windows.Shapes;

namespace HOAChairmanAssistant.View
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            string oracleConnectionString = "Data Source=localhost:1521/orcl1;User Id=system;Password=Pa$$w0rd;";
            string mssqlConnectionString = "Data Source= VASILISIK;initial catalog=HOAChairmanAssistantDB;integrated security=True;";

            string oracleTableUsers = "C##Vasilisa.USERS";
            string mssqlTableUsers = "dbo.Users";

            string oracleTableAddresses = "C##Vasilisa.ADDRESSES";
            string mssqlTableAddresses = "dbo.Addresses";

            string oracleTableHouses = "C##Vasilisa.HOUSES";
            string mssqlTableHouses = "dbo.Houses";

            string oracleTablePorches = "C##Vasilisa.PORCHES";
            string mssqlTablePorches = "dbo.Porches";

            string oracleTableFlats = "C##Vasilisa.FLATS";
            string mssqlTableFlats = "dbo.Flats";

            string oracleTableOwners = "C##Vasilisa.OWNERS";
            string mssqlTableOwners = "dbo.Owners";

            string oracleTablePhoneNumbers = "C##Vasilisa.PHONENUMBERS";
            string mssqlTablePhoneNumbers = "dbo.PhoneNumbers";

            string oracleTableContacts = "C##Vasilisa.CONTACTS";
            string mssqlTableContacts = "dbo.Contacts";


            DataLayer.DataMigrator.MoveDataFromOracleToMSSQL(oracleConnectionString, mssqlConnectionString, oracleTableUsers, mssqlTableUsers);
            DataLayer.DataMigrator.MoveDataFromOracleToMSSQL(oracleConnectionString, mssqlConnectionString, oracleTableAddresses, mssqlTableAddresses);
            DataLayer.DataMigrator.MoveDataFromOracleToMSSQL(oracleConnectionString, mssqlConnectionString, oracleTableHouses, mssqlTableHouses);
            DataLayer.DataMigrator.MoveDataFromOracleToMSSQL(oracleConnectionString, mssqlConnectionString, oracleTablePorches, mssqlTablePorches);
            DataLayer.DataMigrator.MoveDataFromOracleToMSSQL(oracleConnectionString, mssqlConnectionString, oracleTableFlats, mssqlTableFlats);
            DataLayer.DataMigrator.MoveDataFromOracleToMSSQL(oracleConnectionString, mssqlConnectionString, oracleTableOwners, mssqlTableOwners);
            DataLayer.DataMigrator.MoveDataFromOracleToMSSQL(oracleConnectionString, mssqlConnectionString, oracleTablePhoneNumbers, mssqlTablePhoneNumbers);
            DataLayer.DataMigrator.MoveDataFromOracleToMSSQL(oracleConnectionString, mssqlConnectionString, oracleTableContacts, mssqlTableContacts);

            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            Messenger.Default.Register<OpenWindowMessage>(
              this,
              message =>
              {
                  if (message.Type == WindowType.kMain)
                  {
                      var modalWindowVM = SimpleIoc.Default.GetInstance<MainWindowViewModel>();
                      modalWindowVM.User = message.Argument;
                      modalWindowVM.IsAccountant = modalWindowVM.User.Role == UserRole.Accountant;
                      var mainWindow = new MainWindow();
                      mainWindow.Show();
                      this.Close();
                  }
              });
        }

        private void LoginWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
