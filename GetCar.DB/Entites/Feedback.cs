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
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }
        public decimal Rating { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedAt { get; set; }


        [ForeignKey("Customer")]
        public string ?CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [ForeignKey("Booking")]
        public int? BookingID { get; set; }
        public virtual Booking Booking { get; set; }

        [ForeignKey("Car")]
        public int? CarId { get; set; }
        public virtual Car Car { get; set; }

    }

}
