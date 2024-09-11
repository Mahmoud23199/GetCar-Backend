using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.AuthDtos
{
    public class ResendConfirmEmailDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

}
