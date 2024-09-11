using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.DB.Entites
{
    public class Employee:ApplicationUser
    {
        public string Role { get; set; }
        public string WorksFor { get; set; }


        [ForeignKey("VendorOwner")]
        public string? VendorOwnerId { get; set; }
        public VendorOwner VendorOwner { get; set; }


        [ForeignKey("Vendor")]
        public string? VendorId { get; set; }
        public Vendor Vendor { get; set; }

        [ForeignKey("Admin ")]
        public string? AdminId { get; set; }
        public Admin Admin { get; set; }

    }
}
