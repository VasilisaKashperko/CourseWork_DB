using GalaSoft.MvvmLight.Ioc;
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

namespace HOAChairmanAssistant.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            DataContext = new RegistrationViewModel(SimpleIoc.Default.GetInstance<IFrameNavigationService>());
            InitializeComponent();
        }
    }
}
