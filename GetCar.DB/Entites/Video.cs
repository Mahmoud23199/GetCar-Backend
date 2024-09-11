using GetCar.DB.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.DB.Entites
{
    public class Video
    {
        public int VideoId { get; set; }
        public string VideoURL { get; set; }
        public string Description { get; set; }

        [ForeignKey("car")]
        public int CarId { get; set; }
        public Car Car { get; set; }
    }

}
