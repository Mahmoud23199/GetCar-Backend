using GetCar.DB.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.DB.Entites
{
    public class VendorOwner:ApplicationUser
    {
        public string CompanyName { get; set; }
        public string Manager { get; set; }
        public string City { get; set; }
        public string Governorate { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string IdFace { get; set; }
        public string IdBack { get; set; }
        public string CompanyLogo { get; set; }
        public string Notes { get; set; }
        public string BusinessLicense { get; set; }
        public string InsuranceCertificates { get; set; }
        public string TaxIdNumber { get; set; }
        public string ContactInfo { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Descrpition { get; set; }
        public int Branches { get; set; }
        public int AvilableCars { get; set; }


        public ICollection<Vendor> Vendors { get; set; }
        public ICollection<Car> Cars { get; set; }
        public ICollection<Employee> Employees { get; set; }

    }

}
