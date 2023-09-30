using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace HOAChairmanAssistant.ViewModel
{
    public interface IFrameNavigationService : INavigationService
    {
        object Parameter { get; }
    }
    public class MainViewModel : ViewModelBase
    {
        private IFrameNavigationService _navigationService;

        private RelayCommand _infoCommand;
        public RelayCommand InfoCommand
        {
            get
            {
                return _infoCommand
                    ?? (_infoCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("InformationPage");
                    }));
            }
        }
    }
}