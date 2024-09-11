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
    [Microsoft.EntityFrameworkCore.Index(nameof(Date), Name = "IX_Calendar_Date")]
    public class Calendar
    {
        [Key]
        public int CalendarID { get; set; }
        public DateTime Date { get; set; }
        public bool IsAvailable { get; set; }

        [ForeignKey("Car")]
        public int CarID { get; set; }
        public Car Car { get; set; }
    }

}
