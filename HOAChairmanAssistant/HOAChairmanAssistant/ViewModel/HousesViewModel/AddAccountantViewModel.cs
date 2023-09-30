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
    public class AddAccountantViewModel : ViewModelBase
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
        private string login;
        private int role;
        private string password;
        private bool isRegistrated = false;

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

        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                if (login == value)
                {
                    return;
                }
                login = value;
                RaisePropertyChanged();
            }
        }

        public int Role
        {
            get
            {
                return role;
            }
            set
            {
                if (role == value)
                {
                    return;
                }
                role = value;
                RaisePropertyChanged();
            }
        }

        public bool IsRegistrated
        {
            get
            {
                return isRegistrated;
            }
            set
            {
                if (isRegistrated == value)
                {
                    return;
                }
                isRegistrated = value;
                RaisePropertyChanged();
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (password == value)
                {
                    return;
                }
                password = value;
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

        private RelayCommand _closeDialogCommand;
        public RelayCommand CloseDialogCommand
        {
            get
            {
                return _closeDialogCommand
                    ?? (_closeDialogCommand = new RelayCommand(
                    () =>
                    {
                        if (isAdded == true)
                        {
                            _navigationService.NavigateTo("Houses");
                            IsOpenDialog = false;
                        }
                        else
                        {
                            IsOpenDialog = false;
                        }
                    }));
            }
        }

        private RelayCommandParametr _registerCommand;
        public RelayCommandParametr RegisterCommand
        {
            get
            {
                return _registerCommand
                    ?? (_registerCommand = new RelayCommandParametr(
                    (x) =>
                    {
                        IsVisibleProgressBar = true;
                        ThreadPool.QueueUserWorkItem(
                        o =>
                        {
                            if (context.Users.FirstOrDefault(x1 => x1.Login == login) != null)
                            {
                                IsVisibleProgressBar = false;
                                Message = "Пользователь с таким логином уже зарегистрирован.";
                                IsOpenDialog = true;
                            }
                            else if (Login != null && Password != null && Name != null && Surname != null)
                            {
                                string hashPass = User.getHash(Password);
                                int lastId = context.Users
                                            .Select(x2 => x2.UserId)
                                            .OrderByDescending(x2 => x2)
                                            .FirstOrDefault();
                                int newId = lastId + 1;
                                User user = new User
                                {
                                    UserId = newId,
                                    Surname = surname,
                                    Name = name,
                                    Patronymic = patronymic,
                                    Login = login,
                                    Password = hashPass,
                                    Role = (UserRole)1,
                                    AccountantId = GlobalData.UserId
                                };
                                context.Users.Add(user);
                                context.SaveChanges();
                                isAdded = true;
                                IsRegistrated = true;
                                IsOpenDialog = true;
                                IsVisibleProgressBar = false;
                                Message = "Спасибо за регистрацию!";
                            }
                            else
                            {
                                IsVisibleProgressBar = false;
                                Message = "Некорректно введены данные.";
                                IsOpenDialog = true;
                            }
                        }
                    );
                    },
                    (x1) =>
                    Name?.Length > 0 && Login?.Length > 0 && Surname?.Length > 0 && Patronymic?.Length > 0 && Password?.Length > 0));
            }
        }

        private RelayCommand _housesPageCommand;
        public RelayCommand HousesPageCommand
        {
            get
            {
                return _housesPageCommand
                    ?? (_housesPageCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Houses");
                    }));
            }
        }
        #endregion

        #region ctor
        public AddAccountantViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            if (!IsVisibleProgressBar)
            {
                Surname = String.Empty;
                Name = String.Empty;
                Patronymic = String.Empty;
            }
        }
        #endregion
    }
}
