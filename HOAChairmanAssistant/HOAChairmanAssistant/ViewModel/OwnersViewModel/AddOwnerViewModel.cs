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
    public class AddOwnerViewModel : ViewModelBase
    {
        #region Private Fields 

        private IFrameNavigationService _navigationService;
        private HOAChairmanAssistantContext context = new HOAChairmanAssistantContext();
        private House aboutHouse { get; set; }
        private OwnerStatus ownerStatus;
        private Flat selectedFlat { get; set; }
        private string surname;
        private string name;
        private string patronymic;
        private string additionalInfo;
        private string mobilePhone;
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

        private RelayCommandParametr addOwnerCommand;
        public RelayCommandParametr AddOwnerCommand
        {
            get
            {
                return addOwnerCommand
                    ?? (addOwnerCommand = new RelayCommandParametr(
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
                                int lastId = context.PhoneNumbers
                                            .Select(x2 => x2.PhoneNumberId)
                                            .OrderByDescending(x2 => x2)
                                            .FirstOrDefault();
                                int newId = lastId + 1;
                                PhoneNumber phoneNumber = new PhoneNumber()
                                {
                                    PhoneNumberId = newId,
                                    MobilePhone = mobilePhone,
                                    HomePhone = homePhone,
                                    AdditionalPhone = additionalPhone
                                };
                                int lastIdO = context.Owners
                                        .Select(x2 => x2.OwnerId)
                                        .OrderByDescending(x2 => x2)
                                        .FirstOrDefault();
                                int newIdO = lastIdO + 1;
                                Owner owner = new Owner()
                                {
                                    OwnerId = newIdO,
                                    Surname = surname,
                                    Name = name,
                                    Patronymic = patronymic,
                                    AdditionalInfo = additionalInfo,
                                    PhoneNumberId = phoneNumber.PhoneNumberId,
                                    OwnerStatusId = ownerStatus,
                                    FlatId = selectedFlat.FlatId
                                };
                                GlobalData.OwnerFlatId = owner.FlatId;
                                context.PhoneNumbers.Add(phoneNumber);
                                context.Owners.Add(owner);
                                context.SaveChanges();
                                Surname = Name = Patronymic = AdditionalInfo = MobilePhone = HomePhone = AdditionalPhone = string.Empty;
                                IsVisibleProgressBar = false;
                                Message = "Успешно добавлено!";
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
        public AddOwnerViewModel(IFrameNavigationService navigationService)
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
