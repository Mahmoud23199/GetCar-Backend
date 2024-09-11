using GetCar.DB.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.BookingDtos
{
    public class BookingResponseDto
    {
        public int BookingId { get; set; }
        public string BookingNumber { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
