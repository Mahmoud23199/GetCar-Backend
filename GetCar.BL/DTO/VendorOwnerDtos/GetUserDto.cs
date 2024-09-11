using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.VendorOwnerDtos
{
    public class GetUserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public DateTime CreationDate { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }

    }
}
