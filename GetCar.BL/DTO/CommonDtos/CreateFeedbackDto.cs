using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.CommonDtos
{
    public class CreateFeedbackDto
    {
        [Required]
        public decimal rating { get; set; }
        [Required]
        public string comments { get; set; }

        public int carId { get; set; }
       // public int BookingID { get; set; }


    }
}
