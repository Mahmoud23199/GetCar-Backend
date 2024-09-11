using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.BookingDtos
{
    public class BookCarDto
    {
        [Required]
        public int CarId { get; set; }
        [Required]
        public string CustomerId { get; set; }
        public int? DriverId { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}
