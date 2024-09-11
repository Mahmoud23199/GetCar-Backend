using GetCar.BL.DTO.CommonDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.CustomerDtos
{
    public class CustomerDto
    {
        public string FristName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string ?Image { get; set; }
        public bool? Age { get; set; }
        public string? City { get; set; }
        public List<FeedbackDto> Feedback { get; set; }
    }
}
