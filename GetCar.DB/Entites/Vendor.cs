using GetCar.DB.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.DB.Entites
{
    public class Vendor: ApplicationUser
    {
        public string ManagerName { get; set; }
        public string BrancheName { get; set; }
        public string Governorate { get; set; }
        public string City { get; set; }
        public string ContactInfo { get; set; }
        public string? locationLicense { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public ICollection<Car> Cars { get; set; }
        public ICollection<Employee> Employees { get; set; }

        [ForeignKey("VendorOwner")]
        public string VendorOwnerId { get; set; }
        public VendorOwner VendorOwner {  get; set; }
    }

}
