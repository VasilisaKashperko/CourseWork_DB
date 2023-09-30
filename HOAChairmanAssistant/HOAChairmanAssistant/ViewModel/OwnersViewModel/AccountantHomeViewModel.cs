using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HOAChairmanAssistant.DataLayer.EF;
using HOAChairmanAssistant.Helpers;
using HOAChairmanAssistant.Helpers.GlobalData;
using HOAChairmanAssistant.Helpers.Navigation;
using HOAChairmanAssistant.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace HOAChairmanAssistant.ViewModel
{
    public class AccountantHomeViewModel : ViewModelBase
    {
        #region Private Fields

        private HOAChairmanAssistantContext context = new HOAChairmanAssistantContext();
        private IFrameNavigationService _navigationService;
        private ObservableCollection<House> houses = new ObservableCollection<House>();
        private string searchField;
        private string street;
        private string userName;
        private bool isVisibleProgressBar;
        private bool isOpenDialog;
        private bool isAdded = false;
        private string message;

        #endregion

        #region Public Fields

        public ObservableCollection<House> Houses
        {
            get { return houses; }
            set
            {
                if (houses == value)
                {
                    return;
                }
                houses = value;
                RaisePropertyChanged();
            }
        }

        public string UserName
        {
            get { return userName; }
            set
            {
                userName = GlobalData.UserName;
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
        /// <summary>
        /// Message for the dialog  
        /// </summary>
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                if (message == value)
                {
                    return;
                }
                message = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAdded
        {
            get
            {
                return isAdded;
            }
            set
            {
                if (isAdded == value)
                {
                    return;
                }
                isAdded = value;
                RaisePropertyChanged();
            }
        }

        public string Street
        {
            get { return street; }
            set
            {
                if (street == value)
                {
                    return;
                }
                street = context.Addresses.Select(c => c.Street).FirstOrDefault();
                RaisePropertyChanged();
            }
        }

        public bool IsVisibleProgressBar
        {
            get
            {
                return isVisibleProgressBar;
            }
            set
            {
                if (isVisibleProgressBar == value)
                {
                    return;
                }
                isVisibleProgressBar = value;
                RaisePropertyChanged();
            }
        }

        public string SearchField
        {
            get
            {
                return searchField;
            }
            set
            {
                if (searchField == value)
                {
                    return;
                }

                searchField = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands

        private RelayCommandParametr loadedCommand;
        public RelayCommandParametr LoadedCommand
        {
            get
            {
                return loadedCommand
                    ?? (loadedCommand = new RelayCommandParametr(
                    obj =>
                    {
                        var accUser = context.Users.FirstOrDefault(y => y.UserId == GlobalData.UserId);
                        var userAcc = context.Users.Where(h => h.UserId == accUser.AccountantId).FirstOrDefault();
                        if (userAcc != null)
                        {
                            Houses = new ObservableCollection<House>(context.Houses.Where(u => u.UserId == userAcc.UserId).ToList());
                        }
                        else
                        {
                            Houses = new ObservableCollection<House>(context.Houses.Where(y => y.UserId == GlobalData.UserId).ToList());
                        }
                    }));
            }
        }

        private RelayCommand closeDialogCommand;
        public RelayCommand CloseDialogCommand
        {
            get
            {
                return closeDialogCommand
                    ?? (closeDialogCommand = new RelayCommand(
                    () =>
                    {
                        if (isAdded == true)
                        {
                            IsOpenDialog = false;
                            GlobalData.AccountantName = "";
                            GlobalData.AccountantSurname = "";
                            _navigationService.NavigateTo("Houses");
                        }
                        else
                        {
                            IsOpenDialog = false;
                        }
                    }));
            }
        }

        private RelayCommandParametr _aboutHouseCommand;
        public RelayCommandParametr AboutHouseCommand
        {
            get
            {
                return _aboutHouseCommand
                       ?? (_aboutHouseCommand = new RelayCommandParametr(
                           (obj) =>
                           {
                               _navigationService.NavigateTo("AboutHouse", obj);
                           }));
            }
        }

        #endregion

        #region ctor
        public AccountantHomeViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        #endregion
    }
}
