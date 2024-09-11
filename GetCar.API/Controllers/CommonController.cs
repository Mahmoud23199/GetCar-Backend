using GetCar.BL.BaseRepositry;
using GetCar.BL.CustomResponse;
using GetCar.BL.DTO.CommonDtos;
using GetCar.BL.GenericRepositry;
using GetCar.BL.Services;
using GetCar.DB.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GetCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IGenericRepositry<Feedback> _genericRepositry;
        private readonly ICustomerService _customerService;
        private readonly ICarService _carService;


        public CommonController(ITokenService tokenService, IGenericRepositry<Feedback> genericRepositry, ICustomerService customerService, ICarService carService)
        {
            _tokenService = tokenService;
            _genericRepositry = genericRepositry;
            _customerService = customerService;
            _carService = carService;
        }

        [HttpPost("CreateFeedback/{customerId}")]
        public async Task<IActionResult>CreateFeedback(string customerId,CreateFeedbackDto model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = ModelState.Values.SelectMany(i => i.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }
            try
            {
                var IsCustomer = await _customerService.IsCustomerAsync(customerId);

            }catch(Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
            if (model.carId != 0)
            {
                var car = await _carService.GetCarByIdAsync(model.carId);
                if (car == null)
                {
                    return NotFound(new ApiResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"Car With Id:{model.carId} Not Found"
                    });
                }
            }
            // check Booking 
            // ..
            //
            var feedBack = new Feedback
            {
                Comments = model.comments,
                CustomerId = customerId,
                Rating = model.rating,
                CreatedAt = DateTime.Now,
                CarId= model.carId !=0? (int?)model.carId : null,
              // BookingID= model.BookingID != 0 ? (int?)model.BookingID : null,
            };

             await _genericRepositry.AddedAsync(feedBack);

            
                 return Ok(new ApiResponse
                 {
                     StatusCode = StatusCodes.Status200OK,
                     Message = "Created Success",
                 });
        }

        [HttpGet("GetFeedbacks")]
        public async Task<IActionResult> GetAllFeedBack()
        {

            var include = new string[] { "Customer","Car" };

            var review = await _customerService.GetAllFeedBack();
           

            if (review != null) 
            {
                return Ok(new ApiResponse
                {
                    Data= review,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                });
            }
            return BadRequest(new ApiResponse {
                StatusCode = StatusCodes.Status400BadRequest,
                Errors = ModelState.Values.SelectMany(i => i.Errors).Select(e => e.ErrorMessage).ToList()
            });
        }
    }
}
