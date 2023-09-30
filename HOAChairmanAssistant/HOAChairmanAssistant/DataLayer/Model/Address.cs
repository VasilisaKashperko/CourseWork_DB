using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOAChairmanAssistant.Model
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AddressId { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        public string District { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public int HouseNumber { get; set; }

        public string HousingNumber { get; set; }

        public Address(int addressId, string country, string city, string district, string street, int houseNumber, string housingNumber)
        {
            AddressId = addressId;
            Country = country;
            City = city;
            District = district;
            Street = street;
            HouseNumber = houseNumber;
            HousingNumber = housingNumber;
        }
    }
}
