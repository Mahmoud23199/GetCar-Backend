using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.VendorOwnerDtos
{
    public class GetVendorOwnerDto
    {
        public string Id { get; set; }
        public int Branches { get; set; }
        public int AvilableCars { get; set; }
        public string CompanyLogo { get; set; }
        public string Notes { get; set; }
        public string Phone { get; set; }

    }
}
