using GetCar.DB.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.DB.Entites
{
    public class Customer:ApplicationUser
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string? Image { get; set; }

        public string? City { get; set; }

        public ICollection<Driver> Drivers { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }


    }
}
