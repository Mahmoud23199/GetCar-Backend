using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.CommonDtos
{
    public class FeedbackDto
    {
        public int feedbackId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal rating { get; set; }
        public string comments { get; set; }
        public string? FristName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Image { get; set; }
        public string? City { get; set; }
        public int? CarID { get; set; }
        public string? CarName { get; set; } 
        public string? CarDescription { get; set; }
    }
}
