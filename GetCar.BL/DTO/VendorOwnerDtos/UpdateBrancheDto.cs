using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.VendorOwnerDtos
{
    public class UpdateBrancheDto
    {
        [Required]
        public string BranchName { get; set; }
        [Required]
        public string ManagerName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }


    }
}
