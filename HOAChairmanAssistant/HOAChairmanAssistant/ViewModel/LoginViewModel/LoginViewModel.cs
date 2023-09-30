using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using HOAChairmanAssistant.DataLayer.EF;
using HOAChairmanAssistant.Helpers;
using HOAChairmanAssistant.Helpers.GlobalData;
using HOAChairmanAssistant.Helpers.MessageWindow;
using HOAChairmanAssistant.Helpers.Navigation;
using HOAChairmanAssistant.Model;
using System.Linq;
using System.Threading;

namespace HOAChairmanAssistant.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        #region Private Fields

        private IFrameNavigationService _navigationService;
        private HOAChairmanAssistantContext context = new HOAChairmanAssistantContext();
        private string login;
        private string password;

        private bool isVisibleProgressBar;

        private bool isOpenDialog;

        private string message;
        #endregion

        #region Public Fields

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
        /// <summary>
        /// Is Open Dialog 
        /// </summary>
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

        #endregion

        #region Commands
        private RelayCommand _registerCommand;
        public RelayCommand RegisterCommand
        {
            get
            {
                return _registerCommand
                    ?? (_registerCommand = new RelayCommand(
                    () =>
                    {

                        _navigationService.NavigateTo("Registration");

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
                        _navigationService.NavigateTo("Login");
                    }));
            }
        }

        private RelayCommandParametr _loginCommand;
        public RelayCommandParametr LoginCommand
        {
            get
            {
                return _loginCommand
                    ?? (_loginCommand = new RelayCommandParametr(
                    (x) =>
                    {
                        IsVisibleProgressBar = true;
                        ThreadPool.QueueUserWorkItem(
                        o =>
                        {
                            string tmpPassword = User.getHash(password);
                            if (context.Users.FirstOrDefault(x1 => x1.Login == login && x1.Password == tmpPassword || x1.Password == password) != null)
                            {
                                User user = context.Users.FirstOrDefault(x1 => x1.Login == login);
                                GlobalData.UserName = user.Name.ToString();
                                GlobalData.UserId = user.UserId;
                                var user1 = context.Users.FirstOrDefault(y => y.AccountantId == GlobalData.UserId);
                                if (user1 != null)
                                {
                                    GlobalData.AccountantSurname = user1.Surname.ToString();
                                    GlobalData.AccountantName = user1.Name.ToString();
                                }
                                else
                                {
                                    GlobalData.AccountantSurname = "";
                                    GlobalData.AccountantName = "";
                                }
                                var userch = context.Users.Where(z => z.UserId == GlobalData.UserId).FirstOrDefault();
                                var userAcc = context.Users.Where(h => h.UserId == userch.AccountantId).FirstOrDefault();
                                if (userAcc != null)
                                {
                                    GlobalData.IsAccountant = true;
                                }
                                else
                                {
                                    GlobalData.IsAccountant = false;
                                }
                                var userChairman = context.Users.Where(g => g.UserId == userch.AccountantId).FirstOrDefault();
                                if (userChairman == null)
                                {
                                    GlobalData.IsChairman = true;
                                }
                                else
                                {
                                    GlobalData.IsChairman = false;
                                }
                                context.SaveChanges();
                                DispatcherHelper.CheckBeginInvokeOnUI(
                                    () =>
                                    {
                                        Messenger.Default.Send<OpenWindowMessage>(
                                        new OpenWindowMessage() { Type = WindowType.kMain, Argument = user });
                                    }
                                );
                            }
                            else
                            {
                                IsVisibleProgressBar = false;
                                Message = "Неверный логин или пароль!";
                                IsOpenDialog = true;
                            }
                        }
                );
                    },
                    (x) =>
                    Login?.Length > 0 && Password?.Length > 0));
            }
        }


        #endregion

        #region ctor

        public LoginViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #endregion

    }
}
