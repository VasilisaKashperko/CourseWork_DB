using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOAChairmanAssistant.Model
{
    public class PhoneNumber
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PhoneNumberId { get; set; }

        public string MobilePhone { get; set; }

        public string HomePhone { get; set; }

        public string AdditionalPhone { get; set; }

        public PhoneNumber()
        {

        }
    }
}
