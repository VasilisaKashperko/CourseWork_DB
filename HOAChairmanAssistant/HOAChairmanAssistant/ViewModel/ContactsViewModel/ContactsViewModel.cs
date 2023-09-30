using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HOAChairmanAssistant.DataLayer.EF;
using HOAChairmanAssistant.Helpers;
using HOAChairmanAssistant.Helpers.GlobalData;
using HOAChairmanAssistant.Helpers.Navigation;
using HOAChairmanAssistant.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOAChairmanAssistant.ViewModel.ContactsViewModel
{
    internal class ContactsViewModel : ViewModelBase
    {
        #region Private Fields 

        private IFrameNavigationService _navigationService;
        private HOAChairmanAssistantContext context = new HOAChairmanAssistantContext();
        private ObservableCollection<ContactData> contacts = new ObservableCollection<ContactData>();

        private string surname;
        private string name;
        private string patronymic;
        private string position;
        private string mobilePhone;
        private string homePhone;
        private string additionalPhone;
        private ContactData selectedContact;


        #endregion

        #region Public Fields

        public ObservableCollection<ContactData> Contacts
        {
            get { return contacts; }
            set
            {
                if (contacts == value)
                {
                    return;
                }
                contacts = value;
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

        public ContactData SelectedContact
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

        #endregion

        #region Command

        private RelayCommand _loadedpageCommand;
        public RelayCommand LoadedPageCommand
        {
            get
            {
                return _loadedpageCommand
                ?? (_loadedpageCommand = new RelayCommand(
                () =>
                {
                    var contactsDataQuery = (
                                        from contact in context.Contacts
                                        join phoneNumber in context.PhoneNumbers
                                        on contact.PhoneNumberId equals phoneNumber.PhoneNumberId
                                        select new
                                        {
                                            Surname = contact.Surname,
                                            Name = contact.Name,
                                            Patronymic = contact.Patronymic,
                                            Position = contact.Position,
                                            PhoneId = phoneNumber.PhoneNumberId,
                                            MobilePhone = phoneNumber.MobilePhone
                                        }).ToList();
                    foreach (var x in contactsDataQuery)
                    {
                        Contacts.Add(new ContactData(x.Surname, x.Name, x.Patronymic, x.Position, x.MobilePhone, x.PhoneId));
                    }
                }));
            }
        }

        private RelayCommandParametr _addContactCommand;
        public RelayCommandParametr AddContactCommand
        {
            get
            {
                return _addContactCommand
                       ?? (_addContactCommand = new RelayCommandParametr(
                           (obj) =>
                           {
                               _navigationService.NavigateTo("AddContact");
                           }
                    )
                );
            }
        }

        private RelayCommandParametr _updateContactCommand;
        public RelayCommandParametr UpdateContactCommand
        {
            get
            {
                return _updateContactCommand
                       ?? (_updateContactCommand = new RelayCommandParametr(
                           (obj) =>
                           {
                               Contact contact = context.Contacts.Find(selectedContact.PhoneId);
                               _navigationService.NavigateTo("UpdateContact", contact);
                           },
                           x => SelectedContact != null));
            }
        }

        #endregion

        #region ctor
        public ContactsViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        #endregion
    }
}
