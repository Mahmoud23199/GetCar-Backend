using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.DB.Entites
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public decimal AmountPaid { get; set; }
        public PaymentStatus Status { get; set; }


        [ForeignKey("Booking")]
        public int BookingId { get; set; }
        public Booking Booking { get; set; }


    }
    public enum PaymentStatus
    {
        Pending=1,
        Completed=2,
        Failed=3,
        Refunded=4
    }

}
