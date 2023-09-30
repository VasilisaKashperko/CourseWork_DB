using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOAChairmanAssistant.Model
{
    public class Owner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OwnerId { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Name { get; set; }

        public string Patronymic { get; set; }

        public string AdditionalInfo { get; set; }

        public int CurrentDebt { get; set; }

        public int PhoneNumberId { get; set; }
        [ForeignKey("PhoneNumberId")]
        public PhoneNumber PhoneNumber { get; set; }

        [Required]
        public OwnerStatus OwnerStatusId { get; set; }

        [Required]
        public int FlatId { get; set; }
        [ForeignKey("FlatId")]
        public Flat Flat { get; set; }

        public Owner()
        {
        }
    }
}
