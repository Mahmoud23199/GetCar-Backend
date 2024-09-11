using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.VendorDtos
{
    public class RegisterVendorDto
    {
        [Required]
        public string ManagerName { get; set; }
        [Required]
        public string BrancheName { get; set; }
        [Required]
        public string Governorate { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string ContactInfo { get; set; }

        [Required]
        public IFormFile? locationLicense { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

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
