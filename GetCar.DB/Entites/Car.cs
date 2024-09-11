using GetCar.DB.Entites;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GetCar.DB.Entites
{
    [Microsoft.EntityFrameworkCore.Index(nameof(PickupLocation), Name = "IX_Car_PickupLocation")]
    public class Car
    {
        [Key]
        public int CarID { get; set; }
        public string Name { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public string PickupLocation { get; set; }
        public decimal PricePerDay { get; set; }
        public bool Availability { get; set; }
        public string Liter { get; set; }
        public string Doors { get; set; }
        public string Type { get; set; }
        public string People { get; set; }
        public string ?Specifications { get; set; }
        public string ?Transmission { get; set; }
        public string ?Mileage { get; set; }
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
        public bool WithDriver { get; set; }


        [ForeignKey("Vendor")]
        public string? VendorID { get; set; }
        public Vendor Vendor { get; set; }

        [ForeignKey("VendorOwner")]
        public string? VendorOwnerID { get; set; }
        public VendorOwner VendorOwner { get; set; }

        [ForeignKey("Category")]
        public int? CategoryID { get; set; }
        public Category Category { get; set; }

      
        public ICollection<DropoffLocation>? DropoffLocation { get; set; }
        public ICollection<Image>? Images { get; set; }
        public ICollection<Video>? Videos { get; set; }
        public ICollection<Calendar>? Calendars { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }

    }

}
