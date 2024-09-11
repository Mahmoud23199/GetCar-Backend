using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.AuthDtos
{
    public class RegisterUserDto
    {
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "Email")]

        public string Email { get; set; }

        [Required]

        [Display(Name = "Phone Number")]

        public string PhoneNumber { get; set; }

        

        [Display(Name = "Gender")]

        public string? Gender { get; set; }

       

        [Display(Name = "Date of Birth")]

        public DateTime? DateOfBirth { get; set; }


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
