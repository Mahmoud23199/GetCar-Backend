using GetCar.BL.BaseRepositry;
using GetCar.BL.CustomResponse;
using GetCar.BL.DTO.CarDTOs;
using GetCar.DB.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GetCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly  UserManager<ApplicationUser>_userManager;


        public CarController(ICarService carService,UserManager<ApplicationUser> userManager)
        {
            _carService = carService;
            _userManager = userManager;
        }
        [HttpPost("Search")]
        public async Task<IActionResult> SearchCars([FromBody] CarSearchCriteriaDto searchCriteria)
        {
            if (searchCriteria.PickupDate >= searchCriteria.DropoffDate)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message= "Invalid AvailablityDate. PickupDate must be earlier than DropoffDate."
                });
            }
            if (searchCriteria.PickupDate< DateTime.Now|| searchCriteria.DropoffDate<DateTime.Now)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Pickup date cannot be in the past. "
                });
            }

            var cars = await _carService.SearchAvailableCarsAsync(searchCriteria);

            if (cars == null || !cars.Any())
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "No cars found for the given criteria."
                });
            }
            var result = cars.Select(c => new CarDto
            {
                CarID = c.CarID,
                Name = c.Name,
                VendorID = c.VendorID,
                VendorOwnerID = c.VendorOwnerID,
                VendorName = c.VendorOwner?.CompanyName,
                BrancheName = c.Vendor?.BrancheName,
                CategoryID = c.CategoryID,
                CategoryName = c.Category?.CategoryName,
                PickupLocation = c.PickupLocation,
                DropoffLocation = c.DropoffLocation.Select(i => new GetDropoffLocationDto
                {
                    Id=i.DropoffLocationId,
                    Address = i.Address,
                    City = i.City
                }).ToList(),
                Make = c.Make,
                Model = c.Model,
                Year = c.Year,
                Doors = c.Doors,
                Liter = c.Liter,
                People = c.People,
                Type = c.Type,
                Description = c.Description,
                PricePerDay = c.PricePerDay,
                Availability = c.Availability,
                ImageURLs = c.Images?.Select(i => i.ImageURL),
                ProtectionTitle = c.ProtectionTitle,
                ProtectionDescription = c.ProtectionDescription,
                NavigationSystemServices = c.NavigationSystemServices,
                AirConditionServices = c.AirConditionServices,
                ToddlerSeatServices = c.ToddlerSeatServices,
                CancellationPolicy = c.CancellationPolicy,
                WithDriver= c.WithDriver,
            });

            return Ok(new ApiResponse
            {
                Data = result,
                StatusCode = StatusCodes.Status200OK,
                Message = "Success",
            });
        }

        [HttpPost("CreateCar")]
        public async Task<IActionResult> CreateCar([FromForm] CreateCarDto createCarDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = ModelState.Values.SelectMany(i => i.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }
            if (createCarDto.VendorOwnerID == null && createCarDto.VendorID == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "VendorOwnerID and VendorID both cant be null You Must enter at least one"
                });
            }
            try
            {
                var car = await _carService.CreateCarAsync(createCarDto);
                return CreatedAtAction(nameof(GetCarById), new { id = car.CarID }, car);
                //return Ok(new ApiResponse
                //{
                //    Data = car,
                //    StatusCode = StatusCodes.Status200OK,
                //    Message = "Success",
                //});

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
        }

        [HttpGet("GetCar/{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
                return NotFound(new ApiResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Car With Id {id} Not Found"
                });

            return Ok(new ApiResponse
            {
                Data = car,
                StatusCode = StatusCodes.Status200OK,
                Message = "Success",
            });
        }

        [HttpGet("GetCars")]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _carService.GetAllCarsAsync();
            return Ok(new ApiResponse
            {
                Data = cars,
                StatusCode = StatusCodes.Status200OK,
                Message = "Created Success",
            });
        }

        [HttpPut("UpdateCar/{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromForm] UpdateCarDto updateCarDto)//if booking will update it after make booking service
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = ModelState.Values.SelectMany(i => i.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }
            if (updateCarDto.VendorOwnerID == null && updateCarDto.VendorID == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "VendorOwnerID and VendorID both cant be null You Must enter at least one"
                });
            } else if (updateCarDto.VendorOwnerID != null && updateCarDto.VendorID != null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Car Relted to only VendorOwnerId or VendorId not both"
                });
            }
            try
            {
                var success = await _carService.UpdateCarAsync(id, updateCarDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            return Ok(new ApiResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Success",
            });
        }

        [HttpDelete("DeleteCar/{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            try
            {
                var success = await _carService.DeleteCarAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            return Ok(new ApiResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Success",
            });
        }

        [HttpPost("AddedDropoffCar/{id}")]
        public async Task<IActionResult> CreateDropoffCar(int id, List<DropoffLocationDto> model)
        {
            try
            {
                await _carService.CreateDropoffCarAsync(id, model);
                return Ok(new ApiResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Created Success",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = ModelState.Values.SelectMany(i => i.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }
        }

        [HttpDelete("DeletedropoffLocation/{id}")]
        public async Task<IActionResult> DeleteDropoffLocation(int id)
        {
            try
            {
                 await _carService.DeleteDropoffCarAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            return Ok(new ApiResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "deleted Success",
            });
        }

        [HttpPut("Active-InActiveCar/{id}")]
        public async Task<IActionResult> ActiveCar(int id,[Required]bool status)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Please login first with your account"
                });
            }
           // var currentUserRole = await _userManager.GetRolesAsync(currentUser);

           // if (currentUserRole.Any(role => role == VendorRole.Vendor.ToString()))

            try
            {
                await _carService.UpdateCarAvailabilityAsync(id, status,currentUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            return Ok(new ApiResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Update Success",
            });
        }
    }
}
