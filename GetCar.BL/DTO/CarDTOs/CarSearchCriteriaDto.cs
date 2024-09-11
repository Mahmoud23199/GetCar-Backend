using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.CarDTOs
{
    public class CarSearchCriteriaDto
    {
        public string PickupLocation { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime DropoffDate { get; set; }
    }
}
