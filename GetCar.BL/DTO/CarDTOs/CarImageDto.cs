using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.CarDTOs
{
    public class CarImageDto
    {
        [Required]
        public IFormFile Image { get; set; }
        public string ImageDescription { get; set; }
    }
}
