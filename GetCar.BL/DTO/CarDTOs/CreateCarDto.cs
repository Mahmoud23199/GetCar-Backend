using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.CarDTOs
{
    public class CreateCarDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string? VendorID { get; set; }
       
        
        public string? VendorOwnerID { get; set; }

        [Required]
        public int? CategoryID { get; set; }

        [Required]
        public string PickupLocation { get; set; }

        [Required]
        [MaxLength(50)]
        public string Make { get; set; }

        [Required]
        [MaxLength(50)]
        public string Model { get; set; }

        public int Year { get; set; }

        [MaxLength(255)]
        [Required]
        public string Description { get; set; }

        [Required]
        public decimal PricePerDay { get; set; }
        [Required]
        public string Liter { get; set; }
        [Required]
        public string Doors { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string People { get; set; }

        public bool? AirConditionServices { get; set; }
        public bool? ToddlerSeatServices { get; set; }
        public bool? NavigationSystemServices { get; set; }
        public string? CancellationPolicy { get; set; }
        public string? ProtectionTitle { get; set; }
        public string? ProtectionDescription { get; set; }

        public bool? Electricmirrors { get; set; }
        public bool? Cruisecontrol { get; set; }
        public bool? Foglights { get; set; }
        public bool? Power { get; set; }
        public bool? Roofbox { get; set; }
        public bool? GPS { get; set; }
        public bool? Remotecontrol { get; set; }
        public bool? Audioinput { get; set; }
        public bool? CDplayer { get; set; }
        public bool? Bluetooth { get; set; }
        public bool? USBInput { get; set; }
        public bool? Sensors { get; set; }
        public bool? EBDbrakes { get; set; }
        public bool? Airbag { get; set; }
        public bool? ABSBrakes { get; set; }
        [Required]
        public bool WithDriver { get; set; }

        [Required]
        public IFormFile Image1 { get; set; }
        public string ImageDescription1 { get; set; }
        public IFormFile? Image2 { get; set; }
        public string? ImageDescription2 { get; set; }
        public IFormFile? Image3 { get; set; }
        public string? ImageDescription3 { get; set; }
        public IFormFile? Image4 { get; set; }
        public string? ImageDescription4 { get; set; }
        public IFormFile? Image5 { get; set; }
        public string? ImageDescription5 { get; set; }
        [Required]
        public bool Availability { get; set; } = true;
        //public DateTime AvailableStartDate { get; set; }= DateTime.Now;
        //public DateTime AvailableEndDate { get; set; }



    }
}
