using GetCar.BL.DTO.BookingDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.BaseRepositry
{
    public interface IBookingRepositry
    {
       string GenerateBookingNumber();
       Task<BookingResponseDto> BookCarAsync(BookCarDto model);
       Task<BookingResponseDto> ConfirmBookingAsync(int bookingId);
       Task<BookingResponseDto> CancelBookingAsync(int bookingId);
    }
}
