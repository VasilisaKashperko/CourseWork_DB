using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOAChairmanAssistant.Helpers.GlobalData
{
    public class ContactData
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Position { get; set; }
        public string MobilePhone { get; set; }
        public int PhoneId { get; set; }

        public ContactData(string surname, string name, string patronymic, string position, string mobilePhone, int phoneId)
        {
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            Position = position;
            MobilePhone = mobilePhone;
            PhoneId = phoneId;
        }
    }
}
