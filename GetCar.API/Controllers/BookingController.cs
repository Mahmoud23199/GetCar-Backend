using GetCar.BL.BaseRepositry;
using GetCar.BL.CustomResponse;
using GetCar.BL.DTO.BookingDtos;
using GetCar.DB.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GetCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepositry _bookingService;

        public BookingController(IBookingRepositry bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookCarDto bookingDto)
        {
            try
            {
                var result = await _bookingService.BookCarAsync(bookingDto);

                return Ok(new ApiResponse
                {
                    Data = result,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Created Success",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = ex.Message
                });
            }
        }

        //[HttpPut("{id}/confirm")]
        //public async Task<IActionResult> ConfirmBooking(int id)
        //{
        //    try
        //    {
        //        var result = await _bookingService.ConfirmBookingAsync(id);
        //        return Ok(new ApiResponse
        //        {
        //            Data = result,
        //            StatusCode = StatusCodes.Status200OK,
        //            Message = "Success",
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ApiResponse
        //        {
        //            StatusCode = StatusCodes.Status400BadRequest,
        //            Errors = ex.Message
        //        });
        //    }
        //}

        //[HttpPut("{id}/cancel")]
        //public async Task<IActionResult> CancelBooking(int id)
        //{
        //    try
        //    {
        //        var result = await _bookingService.CancelBookingAsync(id);
        //        return Ok(new ApiResponse
        //        {
        //            Data = result,
        //            StatusCode = StatusCodes.Status200OK,
        //            Message = "Success",
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ApiResponse
        //        {
        //            StatusCode = StatusCodes.Status400BadRequest,
        //            Errors = ex.Message
        //        });
        //    }
           
        //}
    }
}
