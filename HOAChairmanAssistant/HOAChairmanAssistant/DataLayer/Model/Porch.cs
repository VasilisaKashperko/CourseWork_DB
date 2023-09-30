using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOAChairmanAssistant.Model
{
    public class Porch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PorchId { get; set; }

        [Required]
        public int PorchNumber { get; set; }

        [Required]
        public int HouseId { get; set; }
        [ForeignKey("HouseId")]
        public House House { get; set; }

        public Porch(int porchId, int porchNumber, House house)
        {
            PorchId = porchId;
            PorchNumber = porchNumber;
            HouseId = house.HouseId;
        }

        public Porch()
        {

        }
    }
}
