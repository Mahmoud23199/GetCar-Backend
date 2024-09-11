using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.CarDTOs
{
    public class UpdateCarDto
    {
        [MaxLength(100)]
        public string Name { get; set; }

        public string? VendorID { get; set; }
        public string? VendorOwnerID { get; set; }
        [Required]
        public int? CategoryID { get; set; }

        [MaxLength(50)]
        public string Make { get; set; }

        [MaxLength(50)]
        public string Model { get; set; }

        public int Year { get; set; }

        [MaxLength(255)]
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
        [Required]
        public bool Availability { get; set; }
        [Required]
        public bool? AirConditionServices { get; set; }
        [Required]
        public bool? ToddlerSeatServices { get; set; }
        [Required]
        public bool? NavigationSystemServices { get; set; }
        [Required]
        public string? CancellationPolicy { get; set; }
        [Required]
        public string? ProtectionTitle { get; set; }
        [Required]
        public string? ProtectionDescription { get; set; }
        [Required]
        public bool? Electricmirrors { get; set; }
        [Required]
        public bool? Cruisecontrol { get; set; }
        [Required]
        public bool? Foglights { get; set; }
        [Required]
        public bool? Power { get; set; }
        [Required]
        public bool? Roofbox { get; set; }
        [Required]
        public bool? GPS { get; set; }
        [Required]
        public bool? Remotecontrol { get; set; }
        [Required]
        public bool? Audioinput { get; set; }
        [Required]
        public bool? CDplayer { get; set; }
        [Required]
        public bool? Bluetooth { get; set; }
        [Required]
        public bool? USBInput { get; set; }
        [Required]
        public bool? Sensors { get; set; }
        [Required]
        public bool? EBDbrakes { get; set; }
        [Required]
        public bool? Airbag { get; set; }
        [Required]
        public bool? ABSBrakes { get; set; }
        [Required]
        public bool WithDriver { get; set; }
    }
}
