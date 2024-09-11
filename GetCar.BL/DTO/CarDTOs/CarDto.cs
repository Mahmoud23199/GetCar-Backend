using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.CarDTOs
{
    public class CarDto
    {
        public int CarID { get; set; }
        public string Name { get; set; }
        public string? VendorID { get; set; }
        public string? VendorOwnerID { get; set; }

        public string VendorName { get; set; }
        public int? CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string PickupLocation { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int? Year { get; set; }
        public string Description { get; set; }
        public string ?BrancheName { get; set; }
        public decimal PricePerDay { get; set; }
        public bool Availability { get; set; }
        public bool DailyAvailability { get; set; }

        public string Liter { get; set; }
        public string Doors { get; set; }
        public string Type { get; set; }
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
        public bool ?WithDriver { get; set; }



        public IEnumerable<string> ImageURLs { get; set; }
        public IEnumerable<GetDropoffLocationDto>? DropoffLocation { get; set; }

    }
}
