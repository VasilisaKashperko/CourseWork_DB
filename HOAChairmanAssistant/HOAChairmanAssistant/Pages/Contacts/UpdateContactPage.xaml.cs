using GalaSoft.MvvmLight.Ioc;
using HOAChairmanAssistant.Helpers.Navigation;
using HOAChairmanAssistant.ViewModel.ContactsViewModel;
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

namespace HOAChairmanAssistant.Pages.Contacts
{
    /// <summary>
    /// Логика взаимодействия для UpdateContactPage.xaml
    /// </summary>
    public partial class UpdateContactPage : Page
    {
        public UpdateContactPage()
        {
            InitializeComponent();
            DataContext = new UpdateContactViewModel(SimpleIoc.Default.GetInstance<IFrameNavigationService>());
        }
    }
}
