using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.CustomerDtos
{
    public class CreateDriverDto
    {
        [Required]
        public string CustomerId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string IDNumber { get; set; }
        public string Phone { get; set; }
        public IFormFile IdFace { get; set; }
        public IFormFile IdBack { get; set; }
        public string LicenseNumber { get; set; }
        public IFormFile LicenseFace { get; set; }
        public IFormFile LicenseBack { get; set; }

        public bool Age { get; set; }
        public string DriverInfo { get; set; }
    }
}
