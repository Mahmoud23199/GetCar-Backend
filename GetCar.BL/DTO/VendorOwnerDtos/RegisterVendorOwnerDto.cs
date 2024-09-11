using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.VendorOwnerDtos
{
    public class RegisterVendorOwnerDto
    {
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string Manager { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Governorate { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public IFormFile IdFace { get; set; } // Photo
        [Required]
        public IFormFile IdBack { get; set; } // Photo
        [Required]
        public IFormFile CompanyLogo { get; set; } // Photo
        public string Notes { get; set; }
        [Required]
        public IFormFile BusinessLicense { get; set; } // Photo or PDF
        [Required]
        public IFormFile InsuranceCertificates { get; set; } // Photo or PDF
        [Required]
        public IFormFile TaxIdNumber { get; set; } // Photo or PDF

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]

        [Display(Name = "Phone Number")]

        public string PhoneNumber { get; set; }


        [Required]

        [Display(Name = "Address")]

        public string Address { get; set; }


        [Required]

        [Display(Name = "Password")]

        public string Password { get; set; }


        [Required]

        [Display(Name = "Confirm Password")]

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]

        public string ConfirmPassword { get; set; }
    }
}
