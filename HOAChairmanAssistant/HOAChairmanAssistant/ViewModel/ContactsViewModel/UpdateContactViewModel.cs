using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
    internal class UpdateContactViewModel : ViewModelBase
    {
        #region Private Fields 

        private Contact selectedContact { get; set; }
        private IFrameNavigationService _navigationService;
        private HOAChairmanAssistantContext context = new HOAChairmanAssistantContext();
        private Contact contact;
        private PhoneNumber phoneNumber;


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

        public Contact SelectedContact
        {
            get
            {
                return selectedContact;
            }
            set
            {
                if (selectedContact == value)
                {
                    return;
                }
                selectedContact = value;
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

        private RelayCommand _loadedpageCommand;
        public RelayCommand LoadedPageCommand
        {
            get
            {
                return _loadedpageCommand
                ?? (_loadedpageCommand = new RelayCommand(
                () =>
                {
                    contact = context.Contacts.Where(x => x.PhoneNumberId == selectedContact.PhoneNumberId).FirstOrDefault();
                    phoneNumber = context.PhoneNumbers.Where(x => x.PhoneNumberId == contact.PhoneNumberId).FirstOrDefault();
                    Surname = contact.Surname;
                    Name = contact.Name;
                    Patronymic = contact.Patronymic;
                    Position = contact.Position;
                    MobilePhone = phoneNumber.MobilePhone;
                    HomePhone = phoneNumber.HomePhone;
                    AdditionalPhone = phoneNumber.AdditionalPhone;
                }));
            }
        }

        private RelayCommandParametr updateContactCommand;
        public RelayCommandParametr UpdateContactCommand
        {
            get
            {
                return updateContactCommand
                    ?? (updateContactCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        IsVisibleProgressBar = true;
                        ThreadPool.QueueUserWorkItem(
                        (o) =>
                        {
                            if (!String.IsNullOrWhiteSpace(Surname)
                                && !String.IsNullOrWhiteSpace(Name))
                            {

                                contact.Surname = Surname;
                                contact.Name = Name;
                                contact.Patronymic = Patronymic;
                                contact.Position = Position;
                                phoneNumber.MobilePhone = MobilePhone;
                                phoneNumber.HomePhone = HomePhone;
                                phoneNumber.AdditionalPhone = AdditionalPhone;

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

        private RelayCommandParametr deleteContactCommand;
        public RelayCommandParametr DeleteContactCommand
        {
            get
            {
                return deleteContactCommand
                    ?? (deleteContactCommand = new RelayCommandParametr(
                    (o) =>
                    {
                        Contact contactForDelete = context.Contacts.Find(contact.PhoneNumberId);
                        PhoneNumber phoneNumberForDelete = context.PhoneNumbers.Find(phoneNumber.PhoneNumberId);
                        if (contactForDelete != null && phoneNumberForDelete != null)
                        {
                            context.Contacts.Remove(contactForDelete);
                            context.PhoneNumbers.Remove(phoneNumberForDelete);
                        }

                        context.SaveChanges();
                        IsVisibleProgressBar = false;
                        Message = "Успешно удалено!";
                        IsOpenDialog = true;
                        isAdded = true;
                    },
                    x => contact != null && phoneNumber != null));
            }
        }

        #endregion

        #region ctor
        public UpdateContactViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            SelectedContact = navigationService.Parameter as Contact;
            if (!IsVisibleProgressBar)
            {
                Surname = String.Empty;
                Name = String.Empty;
                Patronymic = String.Empty;
                Position = String.Empty;
                MobilePhone = String.Empty;
                HomePhone = String.Empty;
                AdditionalPhone = String.Empty;
            }
        }
        #endregion
    }
}

