using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.CustomerDtos
{
    public class DriverDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string IDNumber { get; set; }
        public string Phone { get; set; }
        public string IdFace { get; set; }
        public string IdBack { get; set; }
        public string LicenseFace { get; set; }
        public string LicenseBack { get; set; }
        public string LicenseNumber { get; set; }
        public bool Age { get; set; }
        public string DriverInfo { get; set; }

        public string CustomerName { get; set; }
        public string CustomerId { get; set; }


    }
}
