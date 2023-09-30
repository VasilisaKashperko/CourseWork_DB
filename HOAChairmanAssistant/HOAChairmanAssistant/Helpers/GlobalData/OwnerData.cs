using HOAChairmanAssistant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOAChairmanAssistant.Helpers.GlobalData
{
    public class OwnerData
    {
        public int FlatId { get; set; }
        public int FlatNumber { get; set; }
        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }
        public string MobilePhone { get; set; }

        public int CurrentDebt { get; set; }

        public OwnerStatus OwnerStatusId { get; set; }

        public OwnerData(int i, int f, string s, string n, string p, string m, int d, OwnerStatus os)
        {
            FlatId = i;
            FlatNumber = f;
            Surname = s;
            Name = n;
            MobilePhone = m;
            Patronymic = p;
            OwnerStatusId = os;
            CurrentDebt = d;
        }
    }
}
