using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Threading;
using System.Drawing;
using GalaSoft.MvvmLight.Command;
using HOAChairmanAssistant.Helpers.Navigation;
using HOAChairmanAssistant.DataLayer.EF;
using HOAChairmanAssistant.Helpers;
using HOAChairmanAssistant.Model;
using GalaSoft.MvvmLight.Ioc;
using System.Linq;
using HOAChairmanAssistant.Helpers.GlobalData;

namespace HOAChairmanAssistant.ViewModel
{
    class AddHouseViewModel : ViewModelBase
    {
        #region Private Fields 

        private IFrameNavigationService _navigationService;
        private HOAChairmanAssistantContext context = new HOAChairmanAssistantContext();
        private string country;
        private string city;
        private string district;
        private string street;
        private string houseName;
        private int houseNumber;
        private string housingNumber;
        private int numberOfFlats;
        private int numberOfPorches;
        private bool isVisibleProgressBar;
        private bool isOpenDialog;
        private bool isAdded = false;
        private string message;

        #endregion

        #region Public Fields

        public string Country
        {
            get
            {
                return country;
            }
            set
            {
                if (country == value)
                {
                    return;
                }

                country = value;
                RaisePropertyChanged();
            }
        }

        public string City
        {
            get
            {
                return city;
            }
            set
            {
                if (city == value)
                {
                    return;
                }

                city = value;
                RaisePropertyChanged();
            }
        }

        public string District
        {
            get
            {
                return district;
            }
            set
            {
                if (district == value)
                {
                    return;
                }
                district = value;
                RaisePropertyChanged();
            }
        }

        public string Street
        {
            get
            {
                return street;
            }
            set
            {
                if (street == value)
                {
                    return;
                }
                street = value;
                RaisePropertyChanged();
            }
        }

        public string HouseName
        {
            get
            {
                return houseName;
            }
            set
            {
                if (houseName == value)
                {
                    return;
                }
                houseName = value;
                RaisePropertyChanged();
            }
        }

        public int HouseNumber
        {
            get
            {
                return houseNumber;
            }
            set
            {
                if (houseNumber == value)
                {
                    return;
                }
                houseNumber = value;
                RaisePropertyChanged();
            }
        }

        public string HousingNumber
        {
            get
            {
                return housingNumber;
            }
            set
            {
                if (housingNumber == value)
                {
                    return;
                }
                housingNumber = value;
                RaisePropertyChanged();
            }
        }

        public int NumberOfFlats
        {
            get
            {
                return numberOfFlats;
            }
            set
            {
                if (numberOfFlats == value)
                {
                    return;
                }
                numberOfFlats = value;
                GlobalData.NumberOfFlats = value;
                RaisePropertyChanged();
            }
        }

        public int NumberOfPorches
        {
            get
            {
                return numberOfPorches;
            }
            set
            {
                if (numberOfPorches == value)
                {
                    return;
                }
                numberOfPorches = value;
                GlobalData.NumberOfPorches = value;
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

        private RelayCommand closeDialogCommand;
        public RelayCommand CloseDialogCommand
        {
            get
            {
                return closeDialogCommand
                    ?? (closeDialogCommand = new RelayCommand(
                    () =>
                    {
                        if (isAdded == true)
                        {
                            IsOpenDialog = false;
                            _navigationService.NavigateTo("Houses");
                        }
                        else
                        {
                            IsOpenDialog = false;
                        }
                    }));
            }
        }

        private RelayCommandParametr addHouseCommand;
        public RelayCommandParametr AddHouseCommand
        {
            get
            {
                return addHouseCommand
                    ?? (addHouseCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                            IsVisibleProgressBar = true;
                            ThreadPool.QueueUserWorkItem(
                            (o) =>
                            {
                                if (context.Houses.FirstOrDefault(x1 => x1.HouseName == houseName) != null)
                                {
                                    IsVisibleProgressBar = false;
                                    Message = "Дом с таким названием уже добавлен.";
                                    IsOpenDialog = true;
                                }
                                if (context.Houses.FirstOrDefault(x1 => x1.Address.HouseNumber == houseNumber) != null
                                    && context.Houses.FirstOrDefault(x1 => x1.Address.Street == street) != null
                                    && context.Houses.FirstOrDefault(x1 => x1.Address.City == city) != null)
                                {
                                    IsVisibleProgressBar = false;
                                    Message = "Дом с таким адресом уже добавлен.";
                                    IsOpenDialog = true;
                                }
                                if (!String.IsNullOrWhiteSpace(Country)
                                    && !String.IsNullOrWhiteSpace(City)
                                    && !String.IsNullOrWhiteSpace(Street)
                                    && HouseNumber != 0
                                    && NumberOfFlats != 0
                                    && NumberOfPorches != 0
                                    && !String.IsNullOrWhiteSpace(HouseName))
                                {
                                    var user = SimpleIoc.Default.GetInstance<MainWindowViewModel>().User;
                                    int lastId = context.Addresses
                                                .Select(x2 => x2.AddressId)
                                                .OrderByDescending(x2 => x2)
                                                .FirstOrDefault();
                                    int newId = lastId + 1;
                                    Address address = new Address(newId, Country, City, District, Street, HouseNumber, HousingNumber);
                                    int lastIdH = context.Houses
                                                .Select(x3 => x3.HouseId)
                                                .OrderByDescending(x3 => x3)
                                                .FirstOrDefault();
                                    int newIdH = lastIdH + 1;
                                    House house = new House()
                                    {
                                        HouseId = newIdH,
                                        HouseName = houseName,
                                        NumberOfFlats = numberOfFlats,
                                        NumberOfPorches = numberOfPorches,
                                        AddressId = address.AddressId,
                                        UserId = user.UserId
                                    };
                                    context.Addresses.Add(address);
                                    context.Houses.Add(house);
                                    context.SaveChanges();
                                    int amount = NumberOfFlats / NumberOfPorches;
                                    for (int i = 1; i <= NumberOfPorches; i++)
                                    {
                                        int lastIdP = context.Porches
                                                    .Select(x3 => x3.PorchId)
                                                    .OrderByDescending(x3 => x3)
                                                    .FirstOrDefault();
                                        int newIdP = lastIdP + 1;
                                        Porch porch = new Porch(newIdP, i, house);
                                        context.Porches.Add(porch);
                                        context.SaveChanges();
                                        int n = 1;
                                        if (i > 1)
                                        {
                                            n = 1 + amount * (i - 1);
                                        }

                                        for (int y = n; y <= amount * i; y++)
                                        {
                                            int lastIdF = context.Flats
                                                        .Select(x3 => x3.FlatId)
                                                        .OrderByDescending(x3 => x3)
                                                        .FirstOrDefault();
                                            int newIdF = lastIdF + 1;
                                            Flat flat = new Flat(newIdF, y, porch);
                                            context.Flats.Add(flat);
                                            context.SaveChanges();
                                        }
                                    }
                                    Country = City = District = Street = HouseName = HousingNumber = string.Empty;
                                    HouseNumber = NumberOfFlats = NumberOfPorches = 0;
                                    IsVisibleProgressBar = false;
                                    Message = "Дом успешно добавлен!";
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
                    (x1) => Street?.Length > 0 && Country?.Length > 0 && City?.Length > 0 && HouseName?.Length > 0 && HouseNumber != 0 && NumberOfFlats != 0 && NumberOfPorches != 0));
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

                               _navigationService.NavigateTo("AboutHouse", obj);
                           }));
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
        public AddHouseViewModel(IFrameNavigationService navigationService)
        {

            _navigationService = navigationService;
            if (!IsVisibleProgressBar)
            {
                Country = String.Empty;
                District = String.Empty;
                City = String.Empty;
                Street = String.Empty;
                HouseName = String.Empty;
                NumberOfFlats = 0;
                NumberOfPorches = 0;
                HouseNumber = 0;
                HousingNumber = String.Empty;
            }

        }
        #endregion
    }
}
