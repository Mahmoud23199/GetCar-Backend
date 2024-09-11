using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.DB.Entites
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        [Required]
        public decimal AmountDue { get; set; }
        [Required]
        public decimal AmountPaid { get; set; }

        public DateTime? PaymentDate { get; set; }

        public InvoiceStatus? Status { get;}

        [ForeignKey("Booking")]
        public int BookingId { get; set; } 
        public Booking Booking { get; set; }
    }
    public enum InvoiceStatus
    {
        PartiallyPaid=1,Unpaid=2,Paid=3,Overdue=4
    }
}
