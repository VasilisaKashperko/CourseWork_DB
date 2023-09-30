using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using HOAChairmanAssistant.Helpers.Navigation;
using HOAChairmanAssistant.ViewModel;
using System;

namespace HOAChairmanAssistant.Helpers.Locator
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<LoginWindowViewModel>();
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SetupNavigation();
        }

        private static void SetupNavigation()
        {
            var navigationService = new FrameNavigationService();
            navigationService.Configure("Login", new Uri("../Pages/Login/LoginPage.xaml", UriKind.Relative));
            navigationService.Configure("Registration", new Uri("../Pages/Login/RegistrationPage.xaml", UriKind.Relative));
            navigationService.Configure("Houses", new Uri("../Pages/Houses/HousesPage.xaml", UriKind.Relative));
            navigationService.Configure("AddHousePage", new Uri("../Pages/Houses/AddHousePage.xaml", UriKind.Relative));
            navigationService.Configure("Information", new Uri("../Pages/Information/InformationPage.xaml", UriKind.Relative));
            navigationService.Configure("Contacts", new Uri("../Pages/Contacts/ContactsPage.xaml", UriKind.Relative));
            navigationService.Configure("AddContact", new Uri("../Pages/Contacts/AddContactPage.xaml", UriKind.Relative));
            navigationService.Configure("UpdateContact", new Uri("../Pages/Contacts/UpdateContactPage.xaml", UriKind.Relative));
            navigationService.Configure("AboutHouse", new Uri("../Pages/Houses/AboutHousePage.xaml", UriKind.Relative));
            navigationService.Configure("AddOwnerPage", new Uri("../Pages/Owners/AddOwnerPage.xaml", UriKind.Relative));
            navigationService.Configure("ChangeOwnerPage", new Uri("../Pages/Owners/ChangeOwnerPage.xaml", UriKind.Relative));
            navigationService.Configure("AddAccountantPage", new Uri("../Pages/Houses/AddAccountantPage.xaml", UriKind.Relative));
            navigationService.Configure("AccountantHomePage", new Uri("../Pages/Accountant/AccountantHomePage.xaml", UriKind.Relative));
            navigationService.Configure("AccountantChangeDebt", new Uri("../Pages/Accountant/AccountantChangeDebt.xaml", UriKind.Relative));
            SimpleIoc.Default.Register<IFrameNavigationService>(() => navigationService);
        }

        public MainWindowViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }


        public LoginWindowViewModel LoginWindowViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginWindowViewModel>();
            }
        }

        public static void Cleanup()
        {
        }
    }
}
