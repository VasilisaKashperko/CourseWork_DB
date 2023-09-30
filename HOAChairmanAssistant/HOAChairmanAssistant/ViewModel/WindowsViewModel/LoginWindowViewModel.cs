using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HOAChairmanAssistant.Helpers.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOAChairmanAssistant.ViewModel
{
    public class LoginWindowViewModel : ViewModelBase
    {
        #region Private Fields
        private IFrameNavigationService _navigationService;
        private RelayCommand _loadedpageCommand;
        #endregion

        #region Public Fields
        public RelayCommand LoadedPageCommand
        {
            get
            {
                return _loadedpageCommand
                    ?? (_loadedpageCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Login");
                    }));
            }
        }
        #endregion

        #region ctor
        public LoginWindowViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        #endregion
    }
}
