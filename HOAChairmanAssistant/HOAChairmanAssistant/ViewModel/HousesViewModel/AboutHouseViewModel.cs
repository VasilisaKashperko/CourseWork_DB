using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HOAChairmanAssistant.DataLayer.EF;
using HOAChairmanAssistant.Helpers;
using HOAChairmanAssistant.Helpers.Converters;
using HOAChairmanAssistant.Helpers.GlobalData;
using HOAChairmanAssistant.Helpers.Navigation;
using HOAChairmanAssistant.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;


namespace HOAChairmanAssistant.ViewModel
{
    public class AboutHouseViewModel : ViewModelBase
    {
        #region Private Fields
        private IFrameNavigationService _navigationService;
        private HOAChairmanAssistantContext context = new HOAChairmanAssistantContext();
        private ObservableCollection<Porch> porches = new ObservableCollection<Porch>();
        private ObservableCollection<Flat> flats = new ObservableCollection<Flat>();
        private ObservableCollection<Owner> owners = new ObservableCollection<Owner>();
        private ObservableCollection<OwnerData> ownersData = new ObservableCollection<OwnerData>();
        private bool isFavorite;
        private OwnerData selectedFlat;
        private User user;
        private House aboutHouse { get; set; }
        private Porch aboutPorch { get; set; }
        private bool isOpenDialog;
        private bool isVisibleProgressBar;
        private string message;
        private int porchNumber;
        private string searchField;
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
        public Porch AboutPorch
        {
            get
            {
                return aboutPorch;
            }
            set
            {
                if (aboutPorch == value)
                {
                    return;
                }
                aboutPorch = value;
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

        public OwnerData SelectedFlat
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
                Owner ownerForCheck = context.Owners.Where(x => x.FlatId == selectedFlat.FlatId).FirstOrDefault();
                GlobalData.OwnerForCheckFlatId = ownerForCheck.FlatId;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Porch> Porches
        {
            get { return porches; }
            set
            {
                if (porches == value)
                {
                    return;
                }
                porches = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Flat> Flats
        {
            get { return flats; }
            set
            {
                if (flats == value)
                {
                    return;
                }
                flats = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<OwnerData> OwnersData
        {
            get { return ownersData; }
            set
            {
                if (ownersData == value)
                {
                    return;
                }
                ownersData = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Owner> Owners
        {
            get { return owners; }
            set
            {
                if (owners == value)
                {
                    return;
                }
                owners = value;
                RaisePropertyChanged();
            }
        }

        public int PorchNumber
        {
            get
            {
                return porchNumber;
            }
            set
            {
                if (porchNumber == value)
                {
                    return;
                }
                porchNumber = value;
                RaisePropertyChanged();
            }
        }

        //For DataGrid



        //

        public bool IsFavorite
        {
            get
            {
                return isFavorite;
            }
            set
            {
                if (isFavorite == value)
                {
                    return;
                }
                isFavorite = value;
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
        #endregion

        #region Commands

        private RelayCommand _housesPageCommand;
        public RelayCommand HousesPageCommand
        {
            get
            {
                return _housesPageCommand
                    ?? (_housesPageCommand = new RelayCommand(
                    () =>
                    {
                        var userch = context.Users.Where(z => z.UserId == GlobalData.UserId).FirstOrDefault();
                        var userAcc = context.Users.Where(h => h.UserId == userch.AccountantId).FirstOrDefault();
                        if (userAcc != null)
                        {
                            _navigationService.NavigateTo("AccountantHomePage");
                        }
                        else
                        {
                            _navigationService.NavigateTo("Houses");
                        }
                    }));
            }
        }

        private RelayCommandParametr _deleteHouseCommand;
        public RelayCommandParametr DeleteHouseCommand
        {
            get
            {
                return _deleteHouseCommand
                       ?? (_deleteHouseCommand = new RelayCommandParametr(
                           (obj) =>
                           {
                               var house = context.Houses.Find(AboutHouse.HouseId);
                               context.Houses.Remove(house);
                               context.SaveChanges();
                               _navigationService.NavigateTo("Houses");
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
                    Porches = new ObservableCollection<Porch>(context.Porches.Where(x => x.HouseId == aboutHouse.HouseId).ToList());
                    House house = context.Houses.AsNoTracking().Where(x => x.UserId == user.UserId && x.HouseId == aboutHouse.HouseId).FirstOrDefault();
                }));
            }
        }

        private RelayCommandParametr searchCommand;
        public RelayCommandParametr SearchCommand
        {
            get
            {
                return searchCommand
                    ?? (searchCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        IsVisibleProgressBar = true;
                        if (String.IsNullOrWhiteSpace(searchField))
                        {
                            OwnersData.Clear();
                            foreach (Porch p in Porches)
                            {
                                var ownersDataQuery = (
                                                 from flat in context.Flats
                                                 where flat.PorchId == p.PorchId
                                                 join owner in context.Owners on flat.FlatId equals owner.FlatId into q1
                                                 from owner in q1.DefaultIfEmpty()
                                                 join phoneNumber in context.PhoneNumbers on owner.PhoneNumberId equals phoneNumber.PhoneNumberId into q2
                                                 from phoneNumber in q2.DefaultIfEmpty()
                                                 select new
                                                 {
                                                     FlatId = flat.FlatId,
                                                     FlatNumber = flat.FlatNumber,
                                                     Surname = (owner == null ? string.Empty : owner.Surname),
                                                     Name = (owner == null ? string.Empty : owner.Name),
                                                     Patronymic = (owner == null ? string.Empty : owner.Patronymic),
                                                     MobilePhone = (phoneNumber == null ? string.Empty : phoneNumber.MobilePhone),
                                                     OwnerStatusId = (owner == null ? OwnerStatus.Undefined : owner.OwnerStatusId),
                                                     CurrentDebt = (owner == null ? 0 : owner.CurrentDebt)
                                                 }).ToList();
                                foreach (var x in ownersDataQuery)
                                {
                                    OwnersData.Add(new OwnerData(x.FlatId, x.FlatNumber, x.Surname, x.Name, x.Patronymic, x.MobilePhone, x.CurrentDebt, x.OwnerStatusId));
                                }
                            }
                        }
                        else if (!String.IsNullOrWhiteSpace(searchField) && int.TryParse(searchField, out int result) == true)
                        {
                            OwnersData.Clear();
                            foreach (Porch p in Porches)
                            {
                                var ownersDataQuery = (
                                                 from flat in context.Flats
                                                 where flat.FlatNumber == result && flat.PorchId == p.PorchId
                                                 join owner in context.Owners on flat.FlatId equals owner.FlatId into q1
                                                 from owner in q1.DefaultIfEmpty()
                                                 join phoneNumber in context.PhoneNumbers on owner.PhoneNumberId equals phoneNumber.PhoneNumberId into q2
                                                 from phoneNumber in q2.DefaultIfEmpty()
                                                 select new
                                                 {
                                                     FlatId = flat.FlatId,
                                                     FlatNumber = flat.FlatNumber,
                                                     Surname = (owner == null ? string.Empty : owner.Surname),
                                                     Name = (owner == null ? string.Empty : owner.Name),
                                                     Patronymic = (owner == null ? string.Empty : owner.Patronymic),
                                                     MobilePhone = (phoneNumber == null ? string.Empty : phoneNumber.MobilePhone),
                                                     OwnerStatusId = (owner == null ? OwnerStatus.Undefined : owner.OwnerStatusId),
                                                     CurrentDebt = (owner == null ? 0 : owner.CurrentDebt)
                                                 }).ToList();
                                foreach (var x in ownersDataQuery)
                                {
                                    OwnersData.Add(new OwnerData(x.FlatId, x.FlatNumber, x.Surname, x.Name, x.Patronymic, x.MobilePhone, x.CurrentDebt, x.OwnerStatusId));
                                }
                            }
                        }
                        else if (!String.IsNullOrWhiteSpace(searchField))
                        {

                            OwnersData.Clear();
                            foreach (Porch p in Porches)
                            {
                                var ownersDataQuery = (
                                                 from flat in context.Flats
                                                 where flat.PorchId == p.PorchId
                                                 join owner in context.Owners on flat.FlatId equals owner.FlatId into q1
                                                 from owner in q1.DefaultIfEmpty()
                                                 join phoneNumber in context.PhoneNumbers on owner.PhoneNumberId equals phoneNumber.PhoneNumberId into q2
                                                 from phoneNumber in q2.DefaultIfEmpty()
                                                 select new
                                                 {
                                                     FlatId = flat.FlatId,
                                                     FlatNumber = flat.FlatNumber,
                                                     Surname = (owner == null ? string.Empty : owner.Surname),
                                                     Name = (owner == null ? string.Empty : owner.Name),
                                                     Patronymic = (owner == null ? string.Empty : owner.Patronymic),
                                                     MobilePhone = (phoneNumber == null ? string.Empty : phoneNumber.MobilePhone),
                                                     OwnerStatusId = (owner == null ? OwnerStatus.Undefined : owner.OwnerStatusId),
                                                     CurrentDebt = (owner == null ? 0 : owner.CurrentDebt)
                                                 }).ToList();
                                var search = searchField.ToLower();
                                var ownersDataQuerySearchedSurname = ownersDataQuery.Where(x => x.Surname.ToLower().Contains(search));
                                foreach (var x in ownersDataQuerySearchedSurname)
                                {
                                    OwnersData.Add(new OwnerData(x.FlatId, x.FlatNumber, x.Surname, x.Name, x.Patronymic, x.MobilePhone, x.CurrentDebt, x.OwnerStatusId));
                                }
                            }
                        }
                        SearchField = null;
                            IsVisibleProgressBar = false;
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
                        IsOpenDialog = false;
                    }));
            }
        }

        private RelayCommandParametr _addOwnerCommand;
        public RelayCommandParametr AddOwnerCommand
        {
            get
            {
                return _addOwnerCommand
                       ?? (_addOwnerCommand = new RelayCommandParametr(
                           (obj) =>
                           {
                           Flat flat = context.Flats.Find(selectedFlat.FlatId);
                           _navigationService.NavigateTo("AddOwnerPage", AboutHouse, flat);
                           },
                           x => selectedFlat != null && selectedFlat.FlatId != GlobalData.OwnerFlatId && selectedFlat.FlatId != GlobalData.OwnerForCheckFlatId));
            }
        }

        private RelayCommandParametr _changeOwnerCommand;
        public RelayCommandParametr ChangeOwnerCommand
        {
            get
            {
                return _changeOwnerCommand
                       ?? (_changeOwnerCommand = new RelayCommandParametr(
                           (obj) =>
                           {
                               Flat flat = context.Flats.Find(selectedFlat.FlatId);
                                   _navigationService.NavigateTo("ChangeOwnerPage", AboutHouse, flat);
                           },
                           x => selectedFlat != null && selectedFlat.FlatId == GlobalData.OwnerForCheckFlatId));
            }
        }

        private RelayCommandParametr _changeOwnerdebtCommand;
        public RelayCommandParametr ChangeOwnerDebtCommand
        {
            get
            {
                return _changeOwnerdebtCommand
                       ?? (_changeOwnerdebtCommand = new RelayCommandParametr(
                           (obj) =>
                           {
                               Flat flat = context.Flats.Find(selectedFlat.FlatId);
                               _navigationService.NavigateTo("AccountantChangeDebt", AboutHouse, flat);
                           },
                           x => selectedFlat != null && selectedFlat.FlatId == GlobalData.OwnerForCheckFlatId));
            }
        }

        private RelayCommandParametr _changeDataCommand;
        public RelayCommandParametr ChangeDataCommand
        {
            get
            {
                return _changeDataCommand
                       ?? (_changeDataCommand = new RelayCommandParametr(
                           (obj) =>
                           {   
                               AboutPorch = obj as Porch;
                               Porch porch = Porches.Where(x => x.PorchId == aboutPorch.PorchId).FirstOrDefault();
                               OwnersData.Clear();
                           var ownersDataQuery = (
                                            from flat in context.Flats
                                            where flat.PorchId == porch.PorchId
                                            join owner in context.Owners on flat.FlatId equals owner.FlatId into q1
                                            from owner in q1.DefaultIfEmpty()
                                            join phoneNumber in context.PhoneNumbers on owner.PhoneNumberId equals phoneNumber.PhoneNumberId into q2
                                            from phoneNumber in q2.DefaultIfEmpty()
                                            select new {
                                            FlatId = flat.FlatId,
                                            FlatNumber = flat.FlatNumber,
                                            Surname = (owner == null? string.Empty : owner.Surname),
                                            Name = (owner == null ? string.Empty : owner.Name),
                                            Patronymic = (owner == null ? string.Empty : owner.Patronymic),
                                            MobilePhone = (phoneNumber == null ? string.Empty : phoneNumber.MobilePhone),
                                            OwnerStatusId = (owner == null ? OwnerStatus.Undefined: owner.OwnerStatusId),
                                            CurrentDebt = (owner == null ? 0 : owner.CurrentDebt)
                                            }).ToList();
                               foreach (var x in ownersDataQuery)
                               {
                                   OwnersData.Add(new OwnerData(x.FlatId, x.FlatNumber, x.Surname, x.Name, x.Patronymic, x.MobilePhone, x.CurrentDebt, x.OwnerStatusId));
                               }
                           }));
            }
        }

        #endregion

        #region ctor
        public AboutHouseViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            AboutHouse = navigationService.Parameter as House;
        }
        #endregion
    }
}
