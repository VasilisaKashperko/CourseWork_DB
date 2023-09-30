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

namespace HOAChairmanAssistant.ViewModel.ContactsViewModel
{
    internal class AddContactViewModel : ViewModelBase
    {
        #region Private Fields 

        private IFrameNavigationService _navigationService;
        private HOAChairmanAssistantContext context = new HOAChairmanAssistantContext();
        private User user;

        private string surname;
        private string name;
        private string patronymic;
        private string position;
        private string mobilePhone;
        private string homePhone;
        private string additionalPhone;
        private bool isVisibleProgressBar;
        private bool isOpenDialog;
        private bool isAdded = false;
        private string message;


        #endregion

        #region Public Fields
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

        public string Position
        {
            get
            {
                return position;
            }
            set
            {
                if (position == value)
                {
                    return;
                }
                position = value;
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

        #region Command
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
                                   _navigationService.NavigateTo("Contacts");
                               }
                               else
                               {
                                   IsOpenDialog = false;
                               }
                           }));
            }
        }

        private RelayCommand _loadedpageCommand;
        public RelayCommand LoadedPageCommand
        {
            get
            {
                return _loadedpageCommand
                ?? (_loadedpageCommand = new RelayCommand(
                () =>
                {
                    user = SimpleIoc.Default.GetInstance<MainWindowViewModel>().User;
                }));
            }
        }

        private RelayCommand _backCommand;
        public RelayCommand BackCommand
        {
            get
            {
                return _backCommand
                ?? (_backCommand = new RelayCommand(
                () =>
                {
                    _navigationService.NavigateTo("Contacts");
                }));
            }
        }

        private RelayCommandParametr addContactCommand;
        public RelayCommandParametr AddContactCommand
        {
            get
            {
                return addContactCommand
                    ?? (addContactCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        IsVisibleProgressBar = true;
                        ThreadPool.QueueUserWorkItem(
                        (o) =>
                        {
                            if (!String.IsNullOrWhiteSpace(Surname)
                                && !String.IsNullOrWhiteSpace(Name))
                            {
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
                                int lastIdC = context.Contacts
                                            .Select(x2 => x2.ContactId)
                                            .OrderByDescending(x2 => x2)
                                            .FirstOrDefault();
                                int newIdC = lastIdC + 1;
                                Contact contact = new Contact()
                                {
                                    ContactId = newIdC,
                                    Surname = surname,
                                    Name = name,
                                    Patronymic = patronymic,
                                    Position = position,
                                    PhoneNumberId = phoneNumber.PhoneNumberId,
                                    UserId = user.UserId,
                                };

                                context.PhoneNumbers.Add(phoneNumber);
                                context.Contacts.Add(contact);
                                context.SaveChanges();
                                Surname = Name = Patronymic = Position = MobilePhone = HomePhone = AdditionalPhone = string.Empty;
                                IsVisibleProgressBar = false;
                                Message = "Номер сохранен!";
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
        #endregion

        #region ctor
        public AddContactViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        #endregion
    }
}
