using GetCar.BL.DTO.CommonDtos;
using GetCar.BL.DTO.CustomerDtos;
using GetCar.DB.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.BaseRepositry
{
    public interface ICustomerService
    {
        Task<bool> IsCustomerAsync(string id);
        Task<ApplicationUser> GetCustomerByAsync(string id);
        Task<IEnumerable<FeedbackDto>> GetAllFeedBack();

        Task<Driver> AddDriverAsync(Driver driver);
        Task<IEnumerable<Driver>> GetAllDriversAsync();
        Task<IEnumerable<Driver>> GetCustomerDriversAsync(string customerId);
    }
}
