using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.CarDTOs
{
    public class DropoffLocationDto
    {
        public string Address { get; set; }
        public string City { get; set; }
        //  public string? DropoffLocation { get; set; } // Optional

    }
    public class GetDropoffLocationDto
    {
        public string Address { get; set; }
        public string City { get; set; }
        public int Id { get; set; }

    }
}
