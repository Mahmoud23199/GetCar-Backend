using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.DB.Entites
{
    public class DropoffLocation
    {
        [ForeignKey("Car")]
        public int CarId { get; set; }
        public Car Car { get; set; }
        public string Address {  get; set; }
        public string City { get; set; }

        [Key]
        public int DropoffLocationId { get; set; }
     
    }
}
