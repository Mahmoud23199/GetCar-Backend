using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.VendorOwnerDtos
{
    public class GetVendorBranchesDto
    {
        public string Id { get; set; }
        public string MainManagerName { get; set; }
        public string MainBrancheName { get; set; }
        public string MainGovernorate { get; set; }
        public string MainCity { get; set; }
        public int MainCarsNumber { get; set; }
        public int MainAvailabilCars { get; set; }
        public int MainBookedCars { get; set; }
        public string Phone { get; set; }

        public List<MainVendorBrancheDto> VendorBranches { get; set; }
        
    }
    public class MainVendorBrancheDto
    {
        public string Id { get; set; }
        public string ManagerName { get; set; }
        public string BrancheName { get; set; }
        public string Governorate { get; set; }
        public string City { get; set; }
        public int CarsNumber { get; set; }
        public int AvailabilCars { get; set; }
        public int BookedCars { get; set; }
        public string Phone { get; set; }

    }
}
