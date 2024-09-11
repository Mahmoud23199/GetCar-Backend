﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.AuthDtos
{
    public class RegisterCustomerDto
    {
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Display(Name = "Email")]
            [Required]
            public string Email { get; set; }

            [Required]

            [Display(Name = "Phone Number")]

            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "Password")]
            public string Password { get; set; }


            [Required]
            [Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]

            public string ConfirmPassword { get; set; }
    }
}
