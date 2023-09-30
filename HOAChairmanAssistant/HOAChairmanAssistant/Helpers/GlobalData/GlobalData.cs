using HOAChairmanAssistant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOAChairmanAssistant.Helpers.GlobalData
{
    public static class GlobalData
    {
        public static string UserName { get; set; }
        public static string AccountantName { get; set; }
        public static string AccountantSurname { get; set; }
        public static int UserId { get; set; }
        public static int NumberOfFlats { get; set; }
        public static int NumberOfPorches { get; set; }
        public static int OwnerFlatId { get; set; }
        public static int OwnerForCheckFlatId { get; set; }
        public static bool IsChairman { get; set; }
        public static bool IsAccountant { get; set; }

    }
}
