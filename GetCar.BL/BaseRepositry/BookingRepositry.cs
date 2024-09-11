using GetCar.BL.CustomResponse;
using GetCar.BL.DTO.BookingDtos;
using GetCar.BL.GenericRepositry;
using GetCar.BL.Services;
using GetCar.DB.ApplicationDbContext;
using GetCar.DB.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.BaseRepositry
{
    public class BookingRepositry : IBookingRepositry
    {
        private readonly GetCarDbContext _context;
        private readonly ISaveFileService _saveFileService;
        private readonly ICustomerService _customerService;
        private readonly IGenericRepositry<Category> _categoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingRepositry(GetCarDbContext context, ICustomerService customerService, ISaveFileService saveFileService, IGenericRepositry<Category> categoryRepository, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _customerService = customerService;
            _saveFileService = saveFileService;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
        }
        public async Task<BookingResponseDto> BookCarAsync(BookCarDto model)
        {
            var car = await _context.Cars
                .Where(c=>c.CarID==model.CarId && c.Availability==true)
                .FirstOrDefaultAsync();
            if (car == null)
            {
                throw new DirectoryNotFoundException($"Car With Id:{model.CarId} not found or not Availabil");
            }
            if (model.StartDate >= model.EndDate)
            {
                throw new InvalidOperationException("Invalid AvailablityDate. PickupDate must be earlier than DropoffDate.");
            }
            if (model.StartDate < DateTime.Now || model.EndDate < DateTime.Now)
            {
                throw new InvalidOperationException("Pickup date cannot be in the past. ");
            }

            var calendarEntries =await _context.Calendars
            .Where(cal => cal.CarID == model.CarId &&
                          cal.Date >= model.StartDate &&
                          cal.Date <= model.EndDate && cal.IsAvailable).ToListAsync();

            if (!calendarEntries.Any()) 
            {
                throw new DirectoryNotFoundException($"Car With Id:{model.CarId} is busy");
            }

            var isCustomer =await _customerService.IsCustomerAsync(model.CustomerId);
            if(!isCustomer)
                throw new InvalidOperationException("Invalid Customer");

           
            if(model.DriverId.HasValue && model.DriverId != 0)
            {
                var driver =await _context.Drivers.FindAsync(model.DriverId);
                if (driver == null)
                    throw new DirectoryNotFoundException($"Driver With Id:{model.DriverId} not found");
            }
            //-----------------------------------------------
            //
            // Payment
            //
            //-----------------------------------------------

            foreach (var entry in calendarEntries)
            {
                entry.IsAvailable = false;
            }
            await _context.SaveChangesAsync();

            var booking = new Booking
            {
                BookingNumber = GenerateBookingNumber(),
                CarId = model.CarId,
                CustomerId = model.CustomerId,
                DriverId = model.DriverId == 0 ? null : model.DriverId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Status = BookingStatus.Pending,
                TotalPrice = model.TotalPrice
            };
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            return new BookingResponseDto
            {
                BookingId = booking.BookingId,
                BookingNumber = booking.BookingNumber,
                Status = booking.Status.ToString(),
                StartDate = booking.StartDate,
                EndDate = booking.EndDate
            };
        }

        public async Task<BookingResponseDto> CancelBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings.Include(b => b.Car)
                     .ThenInclude(c => c.Calendars)
                     .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null) throw new KeyNotFoundException("Booking not found.");

            booking.Status = BookingStatus.Cancelled;
            await _context.SaveChangesAsync();

            // Mark the calendar dates as available again
            var calendarEntries = booking.Car.Calendars
                .Where(c => c.Date >= booking.StartDate && c.Date <= booking.EndDate)
                .ToList();

            foreach (var cal in calendarEntries)
            {
                cal.IsAvailable = true;
            }

            await _context.SaveChangesAsync();

            return new BookingResponseDto
            {
                BookingId = booking.BookingId,
                BookingNumber = booking.BookingNumber,
                Status = booking.Status.ToString(),
                StartDate = booking.StartDate,
                EndDate = booking.EndDate
            };
        }

        public async Task<BookingResponseDto> ConfirmBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null) throw new KeyNotFoundException("Booking not found.");

            booking.Status = BookingStatus.Confirmed;
            await _context.SaveChangesAsync();

            return new BookingResponseDto
            {
                BookingId = booking.BookingId,
                BookingNumber = booking.BookingNumber,
                Status = booking.Status.ToString(),
                StartDate = booking.StartDate,
                EndDate = booking.EndDate
            };
        }

        public string GenerateBookingNumber()
        {
            string prefix = "#";
            string day = DateTime.Now.Day.ToString();
            string randomPart = new Random().Next(1000, 9999).ToString(); 

            return $"{prefix}-{day}{randomPart}";
        }

        public async Task<InvoiceResponseDto> GenerateInvoiceAsync(int bookingId)
        {
            var booking = await _context.Bookings.Include(b => b.Car).FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null)
            {
                throw new KeyNotFoundException($"Booking with Id: {bookingId} not found.");
            }

            // Create the invoice
            var invoice = new Invoice
            {
               
                //BookingId = bookingId,
                //AmountDue = booking.TotalPrice,
                //IssuedDate = DateTime.Now,
                //IsPaid = false
            };

            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();

            return new InvoiceResponseDto
            {
                //InvoiceId = invoice.InvoiceId,
                //InvoiceNumber = invoice.InvoiceNumber,
                //Amount = invoice.Amount,
                //IssuedDate = invoice.IssuedDate,
                //IsPaid = invoice.IsPaid
            };
        }

        public async Task MarkInvoiceAsPaidAsync(int invoiceId)
        {
            var invoice = await _context.Invoices.FindAsync(invoiceId);
            if (invoice == null)
            {
                throw new KeyNotFoundException($"Invoice with Id: {invoiceId} not found.");
            }

            //invoice.IsPaid = true;
            //invoice.PaidDate = DateTime.Now;
            //await _context.SaveChangesAsync();
        }

        public async Task<PaymentResponseDto> ProcessPaymentAsync(ProcessPaymentDto model)
        {
            var invoice = await _context.Invoices.FirstOrDefaultAsync(i => i.InvoiceId == model.InvoiceId);
            if (invoice == null)
            {
                throw new KeyNotFoundException($"Invoice with Id: {model.InvoiceId} not found.");
            }

            //if (invoice.IsPaid)
            //{
            //    throw new InvalidOperationException("Invoice has already been paid.");
            //}

            //// Create the payment entry
            //var payment = new Payment
            //{
            //    PaymentNumber = GeneratePaymentNumber(),
            //    InvoiceId = model.InvoiceId,
            //    Amount = invoice.Amount,
            //    PaymentDate = DateTime.Now,
            //    Status = PaymentStatus.Pending
            //};

            //await _context.Payments.AddAsync(payment);
            //await _context.SaveChangesAsync();

            // Simulate Payment Processing (this would usually involve a payment gateway API)
            //payment.Status = PaymentStatus.Completed;
            //await _context.SaveChangesAsync();

            // Mark invoice as paid
            //await _invoiceService.MarkInvoiceAsPaidAsync(invoice.InvoiceId);

            return new PaymentResponseDto
            {
                //PaymentId = payment.PaymentId,
                //PaymentNumber = payment.PaymentNumber,
                //Status = payment.Status.ToString(),
                //PaymentDate = payment.PaymentDate,
                //Amount = payment.Amount
            };
        }

        // Handle refund
        public async Task RefundPaymentAsync(int paymentId)
        {
            //var payment = await _context.Payments.Include(p => p.Invoice).FirstOrDefaultAsync(p => p.PaymentId == paymentId);
            //if (payment == null || payment.Status != PaymentStatus.Completed)
            //{
            //    throw new InvalidOperationException("Payment is not found or not eligible for refund.");
            //}

            //payment.Status = PaymentStatus.Refunded;
            //await _context.SaveChangesAsync();

        }

    }
}

