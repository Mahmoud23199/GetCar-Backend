using GetCar.BL.DTO.CarDTOs;
using GetCar.DB.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.BaseRepositry
{
    public interface ICarService
    {
        Task<IEnumerable<CarDto>> GetAllCarsAsync();
        Task<CarDto> GetCarByIdAsync(int carId);
        Task<CarDto> CreateCarAsync(CreateCarDto createCarDto);
        Task CreateDropoffCarAsync(int id ,List<DropoffLocationDto> dropoffLocations);
        Task<bool> UpdateCarAsync(int id ,UpdateCarDto updateCarDto);
        Task<bool> DeleteCarAsync(int carId);
        Task DeleteDropoffCarAsync(int id);
        Task UpdateCarAvailabilityAsync(int id,bool status, ApplicationUser currentUser);

        Task<IEnumerable<Car>> SearchAvailableCarsAsync(CarSearchCriteriaDto searchCriteriaDto);

    }
}
