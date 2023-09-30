using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using HOAChairmanAssistant.DataLayer.EF;
using HOAChairmanAssistant.Helpers.GlobalData;
using HOAChairmanAssistant.Helpers.MessageWindow;
using HOAChairmanAssistant.Helpers.Navigation;
using HOAChairmanAssistant.Model;

namespace HOAChairmanAssistant.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Private Fields
        private User user;
        private HOAChairmanAssistantContext context = new HOAChairmanAssistantContext();
        private bool isAccountant = false;
        private bool isChairman = false;
        private bool isOpenDialog;
        private IFrameNavigationService _navigationService;
        #endregion

        #region Public Fields

        public User User
        {
            get
            {
                return user;
            }
            set
            {
                if (user == value)
                {
                    return;
                }
                user = value;
                RaisePropertyChanged();
            }
        }

        public bool IsOpenDialog
        {
            get
            {
                return isOpenDialog;
            }
            set
            {
                if (isOpenDialog == value)
                {
                    return;
                }
                isOpenDialog = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAccountant
        {
            get
            {
                return isAccountant;
            }
            set
            {
                if (isAccountant == value)
                {
                    return;
                }
                isAccountant = value;
                RaisePropertyChanged();
            }
        }

        public bool IsChairman
        {
            get
            {
                return isChairman;
            }
            set
            {
                if (isChairman == value)
                {
                    return;
                }
                isChairman = value;
                RaisePropertyChanged();
            }
        }

        public string Message { get; internal set; }
        #endregion

        #region Commands
        private RelayCommand _loginpageCommand;
        public RelayCommand LoginPageCommand
        {
            get
            {
                return _loginpageCommand
                    ?? (_loginpageCommand = new RelayCommand(
                    () =>
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            _navigationService.NavigateTo("Login");
                        });
                        
                    }));
            }
        }

        private RelayCommand _housesCommand;
        public RelayCommand HousesCommand
        {
            get
            {
                return _housesCommand
                    ?? (_housesCommand = new RelayCommand(
                    () =>
                    {
                        if (GlobalData.UserId != 0)
                        {
                            _navigationService.NavigateTo("Houses");
                            var user = context.Users.Where(x => x.UserId == GlobalData.UserId).FirstOrDefault();
                            var userAcc = context.Users.Where(y => y.UserId == user.AccountantId).FirstOrDefault();
                            if (userAcc != null)
                            {
                                isAccountant = true;
                            }
                            else
                            {
                                isAccountant = false;
                            }
                            var userChairman = context.Users.Where(y => y.UserId == user.AccountantId).FirstOrDefault();
                            if (userChairman == null)
                            {
                                isChairman = true;
                            }
                            else
                            {
                                isChairman = false;
                            }
                        }
                    }));
            }
        }

        private RelayCommand _informationCommand;
        public RelayCommand InformationCommand
        {
            get
            {
                return _informationCommand
                    ?? (_informationCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Information");
                    }));
            }
        }

        private RelayCommand _contactsCommand;
        public RelayCommand ContactsCommand
        {
            get
            {
                return _contactsCommand
                    ?? (_contactsCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Contacts");
                    }));
            }
        }

        private RelayCommand _settingsCommand;
        public RelayCommand SettingsCommand
        {
            get
            {
                return _settingsCommand
                    ?? (_settingsCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Settings");
                    }));
            }
        }

        private RelayCommand closeDialodCommand;
        public RelayCommand CloseDialodCommand
        {
            get
            {
                return closeDialodCommand
                    ?? (closeDialodCommand = new RelayCommand(
                    () =>
                    {
                        IsOpenDialog = false;
                    }));
            }
        }

        #endregion

        #region ctor

        public MainWindowViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #endregion
    }
}
