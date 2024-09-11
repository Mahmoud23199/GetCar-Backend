using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.DB.Entites
{
    public class Image
    {
        [Key]
        public int ImageId { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }

        [ForeignKey("Car")]
        public int CarID { get; set; }
        public Car Car { get; set; }
    }

}
