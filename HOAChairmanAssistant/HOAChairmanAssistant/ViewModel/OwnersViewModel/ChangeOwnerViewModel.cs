using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HOAChairmanAssistant.DataLayer.EF;
using HOAChairmanAssistant.Helpers;
using HOAChairmanAssistant.Helpers.GlobalData;
using HOAChairmanAssistant.Helpers.Navigation;
using HOAChairmanAssistant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HOAChairmanAssistant.ViewModel
{
    public class ChangeOwnerViewModel : ViewModelBase
    {
        #region Private Fields 

        private IFrameNavigationService _navigationService;
        private HOAChairmanAssistantContext context = new HOAChairmanAssistantContext();
        private House aboutHouse { get; set; }
        private OwnerStatus ownerStatus;
        private Owner owner;
        private PhoneNumber phoneNumber;
        private Flat selectedFlat { get; set; }
        private string surname;
        private string name;
        private string patronymic;
        private string additionalInfo;
        private string mobilePhone;
        private int debt;
        private string homePhone;
        private string additionalPhone;
        private bool isVisibleProgressBar;
        private bool isOpenDialog;
        private bool isAdded = false;
        private string message;

        #endregion

        #region Public Fields

        public House AboutHouse
        {
            get
            {
                return aboutHouse;
            }
            set
            {
                if (aboutHouse == value)
                {
                    return;
                }
                aboutHouse = value;
                RaisePropertyChanged();
            }
        }

        public Flat SelectedFlat
        {
            get
            {
                return selectedFlat;
            }
            set
            {
                if (selectedFlat == value)
                {
                    return;
                }
                selectedFlat = value;
                RaisePropertyChanged();
            }
        }

        public OwnerStatus OwnerStatus
        {
            get
            {
                return ownerStatus;
            }
            set
            {
                if (ownerStatus == value)
                {
                    return;
                }
                ownerStatus = value;
                RaisePropertyChanged();
            }
        }

        public string Surname
        {
            get
            {
                return surname;
            }
            set
            {
                if (surname == value)
                {
                    return;
                }

                surname = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name == value)
                {
                    return;
                }

                name = value;
                RaisePropertyChanged();
            }
        }

        public int Debt
        {
            get
            {
                return debt;
            }
            set
            {
                if (debt == value)
                {
                    return;
                }

                debt = value;
                RaisePropertyChanged();
            }
        }
        public string Patronymic
        {
            get
            {
                return patronymic;
            }
            set
            {
                if (patronymic == value)
                {
                    return;
                }
                patronymic = value;
                RaisePropertyChanged();
            }
        }

        public string AdditionalInfo
        {
            get
            {
                return additionalInfo;
            }
            set
            {
                if (additionalInfo == value)
                {
                    return;
                }
                additionalInfo = value;
                RaisePropertyChanged();
            }
        }

        public string MobilePhone
        {
            get
            {
                return mobilePhone;
            }
            set
            {
                if (mobilePhone == value)
                {
                    return;
                }
                mobilePhone = value;
                RaisePropertyChanged();
            }
        }

        public string HomePhone
        {
            get
            {
                return homePhone;
            }
            set
            {
                if (homePhone == value)
                {
                    return;
                }
                homePhone = value;
                RaisePropertyChanged();
            }
        }

        public string AdditionalPhone
        {
            get
            {
                return additionalPhone;
            }
            set
            {
                if (additionalPhone == value)
                {
                    return;
                }
                additionalPhone = value;
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
        #endregion

        #region Commands

        private RelayCommandParametr closeDialogCommand;
        public RelayCommandParametr CloseDialogCommand
        {
            get
            {
                return closeDialogCommand
                       ?? (closeDialogCommand = new RelayCommandParametr(
                           (obj) =>
                           {
                               if (isAdded == true)
                               {
                                   IsOpenDialog = false;
                                   _navigationService.NavigateTo("AboutHouse", AboutHouse);
                               }
                               else
                               {
                                   IsOpenDialog = false;
                               }
                           }));
            }
        }

        private RelayCommandParametr loadedCommand;
        public RelayCommandParametr LoadedCommand
        {
            get
            {
                return loadedCommand
                    ?? (loadedCommand = new RelayCommandParametr(
                    obj =>
                    {
                        owner = new Owner();
                        owner = context.Owners.Where(x => x.FlatId == GlobalData.OwnerForCheckFlatId).FirstOrDefault();
                        phoneNumber = new PhoneNumber();
                        phoneNumber = context.PhoneNumbers.Where(x => x.PhoneNumberId == owner.PhoneNumberId).FirstOrDefault();
                        Surname = owner.Surname;
                        Name = owner.Name;
                        Patronymic = owner.Patronymic;
                        AdditionalInfo = owner.AdditionalInfo;
                        OwnerStatus = owner.OwnerStatusId;
                        MobilePhone = phoneNumber.MobilePhone;
                        HomePhone = phoneNumber.HomePhone;
                        AdditionalPhone = phoneNumber.AdditionalPhone;
                        Debt = owner.CurrentDebt;
                    }));
            }
        }

        private RelayCommandParametr updateOwnerCommand;
        public RelayCommandParametr UpdateOwnerCommand
        {
            get
            {
                return updateOwnerCommand
                    ?? (updateOwnerCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        IsVisibleProgressBar = true;
                        ThreadPool.QueueUserWorkItem(
                        (o) =>
                        {
                            if (!String.IsNullOrWhiteSpace(Surname)
                                && !String.IsNullOrWhiteSpace(Name))
                            {
                                var user = SimpleIoc.Default.GetInstance<MainWindowViewModel>().User;

                                owner.Surname = Surname;
                                owner.Name = Name;
                                owner.Patronymic = Patronymic;
                                owner.AdditionalInfo = AdditionalInfo;
                                owner.OwnerStatusId = OwnerStatus;
                                phoneNumber.MobilePhone = MobilePhone;
                                phoneNumber.HomePhone = HomePhone;
                                phoneNumber.AdditionalPhone = AdditionalPhone;
                                owner.CurrentDebt = Debt;

                                context.SaveChanges();
                                IsVisibleProgressBar = false;
                                Message = "Успешно изменено!";
                                IsOpenDialog = true;
                                isAdded = true;
                            }
                            else
                            {
                                IsVisibleProgressBar = false;
                                Message = "Некорректно введены данные.";
                                IsOpenDialog = true;
                            }
                        });
                    },
                    (x1) => Surname?.Length > 0 && Name?.Length > 0 && MobilePhone?.Length > 0));
            }
        }

        private RelayCommandParametr deleteOwnerCommand;
        public RelayCommandParametr DeleteOwnerCommand
        {
            get
            {
                return deleteOwnerCommand
                    ?? (deleteOwnerCommand = new RelayCommandParametr(
                    (o) =>
                    {
                        Owner ownerForDelete = context.Owners.Find(owner.OwnerId);
                        PhoneNumber phoneNumberForDelete = context.PhoneNumbers.Find(phoneNumber.PhoneNumberId);
                        if (ownerForDelete != null && phoneNumberForDelete != null)
                        {
                            context.Owners.Remove(ownerForDelete);
                            context.PhoneNumbers.Remove(phoneNumberForDelete);
                        }
                        GlobalData.OwnerForCheckFlatId = 0;
                        GlobalData.OwnerFlatId = 0;
                        context.SaveChanges();
                        IsVisibleProgressBar = false;
                        Message = "Успешно удалено!";
                        IsOpenDialog = true;
                        isAdded = true;
                    },
                    x => owner != null && phoneNumber != null));
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
                               _navigationService.NavigateTo("AboutHouse", AboutHouse);
                           }));
            }
        }
        #endregion
        #region ctor
        public ChangeOwnerViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            AboutHouse = navigationService.Parameter as House;
            SelectedFlat = navigationService.Parameter2 as Flat;
            if (!IsVisibleProgressBar)
            {
                Surname = String.Empty;
                Name = String.Empty;
                Patronymic = String.Empty;
                AdditionalInfo = String.Empty;
                MobilePhone = String.Empty;
                HomePhone = String.Empty;
                AdditionalPhone = String.Empty;
            }
        }
        #endregion
    }
}
