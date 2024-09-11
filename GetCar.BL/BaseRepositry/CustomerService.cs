using GetCar.BL.DTO.CommonDtos;
using GetCar.BL.DTO.CustomerDtos;
using GetCar.DB.ApplicationDbContext;
using GetCar.DB.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.BaseRepositry
{
    public class CustomerService:ICustomerService
    {
        private readonly GetCarDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CustomerService(GetCarDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<ApplicationUser> GetCustomerByAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                throw new Exception("Invalid CustomerId. The specified Customer does not exist.");
            }
            if (user.EmailConfirmed == false)
            {
                throw new Exception("Invalid User. The specified User does not Confirme his email.");
            }
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Customer"))
            {
                return user;
            }
            else
            {
                throw new Exception("Invalid CustomerId. The specified User does not have Customer Role.");
            }

        }

        public async Task<bool> IsCustomerAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                throw new Exception("Invalid CustomerId. The specified Customer does not exist.");
            }
            if (user.EmailConfirmed == false)
            {
                throw new Exception("Invalid User. The specified User does not Confirme his email.");
            }
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Customer"))
            {
                return true;
            }
            else
            {
                throw new Exception("Invalid CustomerId. The specified User does not have Customer Role.");
            }
        }

        public async Task<IEnumerable<FeedbackDto>> GetAllFeedBack()
        {
            var item = await _context.Feedbacks
                     .Include(i => i.Car)
                     .Include(i => i.Customer)
                     .Where(i => i.CustomerId != null)
                     .ToListAsync();

            var result = item.Select(i => new FeedbackDto
            {
                feedbackId = i.FeedbackId,
                comments = i.Comments,
                rating = i.Rating,
                CreatedAt = i.CreatedAt,
                FristName = i.Customer?.FristName, 
                LastName = i.Customer?.LastName,
                Address = i.Customer?.Address,
                City = i.Customer?.City,
                Image = i.Customer?.Image,
                PhoneNumber = i.Customer?.PhoneNumber,
                CarID = i.Car?.CarID, 
                CarName = i.Car?.Name,
                CarDescription = i.Car?.Description,
            }).ToList();

            return result;
        }

        public async Task<Driver> AddDriverAsync(Driver driver)
        {
            await _context.Drivers.AddAsync(driver);
            await _context.SaveChangesAsync();
            return driver;
        }

        public async Task<IEnumerable<Driver>> GetAllDriversAsync()
        {
            return await _context.Drivers.ToListAsync();
        }

        public async Task<IEnumerable<Driver>> GetCustomerDriversAsync(string customerId)
        {
            return await _context.Drivers
                .Where(d => d.CustomerId == customerId)
                .ToListAsync();
        }
    }
}
