using GetCar.BL.DTO.VendorOwnerDtos;
using GetCar.DB.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.BaseRepositry
{
    public interface IVendorOwnerRepositry
    {
        Task<IEnumerable<GetVendorOwnerDto>> GetVendorOwner();
        Task<IEnumerable<GetVendorOwnerDto>> GetTopVendorOwner();
        Task<GetVendorOwnerDto> GetVendorOwnerById(string id);
        Task<GetVendorBranchesDto> GetVendorBranches(ApplicationUser user);

        Task UpdateVendorBranche(ApplicationUser user,string id, UpdateBrancheDto modle);
        Task<IEnumerable<GetUserDto>> GetUsersByVendorOrOwnerAsync(ApplicationUser user);


    }
}
