using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using HOAChairmanAssistant.Model;

namespace HOAChairmanAssistant.Helpers.Navigation
{
    public interface IFrameNavigationService : INavigationService
    {
        object Parameter { get; }
        object Parameter2 { get; }

        void NavigateTo(string v, House aboutHouse, Flat flat);
    }
}
