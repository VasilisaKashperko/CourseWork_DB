using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOAChairmanAssistant.Model
{
    public class Flat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FlatId { get; set; }

        [Required]
        public int FlatNumber { get; set; }

        [Required]
        public int PorchId { get; set; }
        [ForeignKey("PorchId")]
        public Porch Porch { get; set; }

        public Flat()
        {

        }

        public Flat(int flatId, int flatNumber, Porch porch)
        {
            FlatId = flatId;
            FlatNumber = flatNumber;
            PorchId = porch.PorchId;
        }
    }
}
