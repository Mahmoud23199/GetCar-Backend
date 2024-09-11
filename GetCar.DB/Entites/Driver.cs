using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.DB.Entites
{
    public class Driver
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string IDNumber { get; set; }
        public string Phone { get; set; }
        public string IdFace { get; set; }
        public string IdBack { get; set; }
        public string LicenseNumber { get; set; }
        public bool Age { get; set; }
        public string DriverInfo { get; set; }
        public string LicenseFace { get; set; }
        public string LicenseBack { get; set; }

        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
