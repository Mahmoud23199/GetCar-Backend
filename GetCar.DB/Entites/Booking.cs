using GetCar.DB.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.DB.Entites
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
        public string BookingNumber { get; set; }

        [ForeignKey("Car")]
        public int CarId { get; set; }
        public Car Car { get; set; }

        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("Payment")]
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }


        [ForeignKey("Driver")]
        public int? DriverId { get; set; }
        public Driver Driver { get; set; }


        public Invoice Invoice { get; set; }

        public ICollection<Feedback> Feedbacks { get; set; }
    }
    public enum BookingStatus
    {
        Pending = 1, Confirmed = 2, Cancelled = 3,InProgress=4
    }
}
