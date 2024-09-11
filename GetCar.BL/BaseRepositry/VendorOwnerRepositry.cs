using GetCar.BL.CustomResponse;
using GetCar.BL.DTO.VendorOwnerDtos;
using GetCar.DB.ApplicationDbContext;
using GetCar.DB.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.BaseRepositry
{
    public class VendorOwnerRepositry : IVendorOwnerRepositry
    {
        private readonly GetCarDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public VendorOwnerRepositry(GetCarDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IEnumerable<GetVendorOwnerDto>> GetVendorOwner()
        {
            var data = await _context.VendorOwners.Include(i => i.Vendors).ThenInclude(v => v.Cars)
                .Select(i => new GetVendorOwnerDto
                {
                    Id = i.Id,
                    Branches = i.Vendors.Count() + 1,
                    CompanyLogo = i.CompanyLogo,
                    Notes = i.Notes,
                    AvilableCars = i.Cars.Count() + i.Vendors.SelectMany(v => v.Cars).Count(),
                    Phone=i.PhoneNumber,

                }).ToListAsync();

            return data;
        }
        public async Task<IEnumerable<GetVendorOwnerDto>> GetTopVendorOwner()
        {
            var data = await _context.VendorOwners.Include(i => i.Vendors).ThenInclude(v => v.Cars)
                .Select(i => new GetVendorOwnerDto
                {
                    Id = i.Id,
                    Branches = i.Vendors.Count() + 1,
                    CompanyLogo = i.CompanyLogo,
                    Notes = i.Notes,
                    AvilableCars = i.Cars.Count() + i.Vendors.SelectMany(v => v.Cars).Count(),
                    Phone=i.PhoneNumber

                }).OrderByDescending(i => i.Branches)
                 .ThenByDescending(i => i.AvilableCars).Take(10).ToListAsync();

            return data;
        }

        public async Task<GetVendorOwnerDto> GetVendorOwnerById(string id)
        {
            var data = await _context.VendorOwners.Include(i => i.Vendors).ThenInclude(v => v.Cars)
              .Where(i => i.Id == id).Select(i => new GetVendorOwnerDto
              {
                  Id = i.Id,
                  Branches = i.Vendors.Count() + 1,
                  CompanyLogo = i.CompanyLogo,
                  Notes = i.Notes,
                  AvilableCars = i.Cars.Count() + i.Vendors.SelectMany(v => v.Cars).Count(),
                  Phone=i.PhoneNumber

              }).FirstOrDefaultAsync();

            return data;

        }

        public async Task<GetVendorBranchesDto> GetVendorBranches(ApplicationUser user)
        {
           
            var currentUserRole = await _userManager.GetRolesAsync(user);

            if (currentUserRole.Any(role => role == VendorRole.Vendor.ToString())|| currentUserRole.Any(role => role == "Admin")) // role VendorOwner
            {

                var data = await _context.VendorOwners
                .Include(i => i.Vendors)
                .ThenInclude(v => v.Cars)
                .Where(i => i.Id == user.Id)
                .Select(i => new GetVendorBranchesDto
                {
                    Id = i.Id,
                    MainBrancheName = i.CompanyName,
                    MainCity = i.Address,
                    MainGovernorate = i.Governorate,
                    MainManagerName = i.Manager,
                    MainAvailabilCars = i.Cars.Where(c => c.Availability == true).Count(),
                    MainCarsNumber = i.Cars.Count(),
                    MainBookedCars=i.Cars.Where(c => c.Bookings.Any(b =>
                          b.Status == BookingStatus.Confirmed ||
                          b.Status == BookingStatus.InProgress &&
                          b.StartDate <= DateTime.Now &&
                          b.EndDate >= DateTime.Now)).Count(),
                    Phone=i.PhoneNumber,
                    VendorBranches = i.Vendors.Select(v => new MainVendorBrancheDto
                    {
                        Id = v.Id,
                        BrancheName = v.BrancheName,
                        City = v.City,
                        Governorate = v.Governorate,
                        ManagerName = v.ManagerName,
                        AvailabilCars = v.Cars.Where(c => c.Availability == true).Count(),
                        CarsNumber = v.Cars.Count(),
                        BookedCars= v.Cars.Where(v => v.Bookings.Any(b =>
                          b.Status == BookingStatus.Confirmed ||
                          b.Status == BookingStatus.InProgress &&
                          b.StartDate <= DateTime.Now &&
                          b.EndDate >= DateTime.Now)).Count(),
                        Phone=v.PhoneNumber
                    }).ToList()
                }).FirstOrDefaultAsync(); 

                return data; 
            }
            else
            {
                throw new Exception("You don't have permission to get Vendor data");
            }
        }

        public async Task UpdateVendorBranche(ApplicationUser currentUser, string id, UpdateBrancheDto modle)
        {
            var currentUserRoles = await _userManager.GetRolesAsync(currentUser);

            if (currentUserRoles.Contains(VendorRole.Vendor.ToString()))
            {
                var vendorOwner = (VendorOwner)currentUser;

                if (vendorOwner.Id == id )
                {
                    var vendorOwn=await _context.VendorOwners.FindAsync(id);

                    if (vendorOwn == null) 
                          throw new NotFoundException($"branch with id:{id} not found.");
                    // VendorOwner: update branch
                    await UpdateOwnerBrancheAsync(id, modle);
                }
                else if(_context.Vendors.Where(v => v.VendorOwnerId == id)!=null)
                {
                    var vendor = await _context.Vendors.FindAsync(id);

                    if (vendor == null)
                        throw new NotFoundException($"branch with id:{id} not found.");
                    // SubVendor: update branch
                    await UpdateSubBrancheAsync(id, modle);

                }
                else
                {
                    throw new UnauthorizedAccessException("You don't have permission to update this branch.");
                }
            }
            else if (currentUserRoles.Contains(VendorRole.SubVendor.ToString()))
            {
                var subVendor = (Vendor)currentUser;

                if (subVendor.Id == id)
                {
                    var vendor = await _context.Vendors.FindAsync(id);
                    if (vendor == null)
                        throw new NotFoundException($"branch with id:{id} not found.");
                    // SubVendor: update branch
                    await UpdateSubBrancheAsync(id, modle);
                }
                else
                {
                    throw new UnauthorizedAccessException("You can only update your own branch.");
                }
            }
            else if (currentUserRoles.Any(role => role == EmployeeRole.Finance.ToString()
                                               || role == EmployeeRole.Manager.ToString()
                                               || role == EmployeeRole.IT.ToString()))
            {
               throw new UnauthorizedAccessException("You don't have permission to update this branch.");

                //var employee = (Employee)currentUser;

                //if (_context.Employees.Any(v => v.VendorId == id) ||_context.Employees.Any(vo=>vo.VendorOwnerId==id))
                //{
                //    var vendorOwn = await _context.VendorOwners.FindAsync(id);
                //    var vendor = await _context.Vendors.FindAsync(id);

                //    if(vendorOwn != null)
                //    {
                //        // EmployeeVendorOwner:update branch
                //        await UpdateOwnerBrancheAsync(id, modle);
                //    }
                //    else if (vendor != null)
                //    {
                //        // EmployeeSubVendor:update branch
                //        await UpdateSubBrancheAsync(id, modle);
                //    }
                //    else
                //        throw new NotFoundException($"branch with id:{id} not found.");
                //}

                //else
                //{
                //    throw new UnauthorizedAccessException("You don't have permission to update this branch.");
                //}
            }
            else if (currentUserRoles.Contains("Customer"))
            {
                throw new UnauthorizedAccessException("Customers don't have permission to update branches.");
            }
            else
            {
                throw new UnauthorizedAccessException("You don't have permission to update this branch.");
            }
        }

        public async Task UpdateOwnerBrancheAsync(string id, UpdateBrancheDto modle)
        {

            var Branch = await _context.VendorOwners.FindAsync(id);
            if (Branch != null)
            {
                Branch.CompanyName = modle.BranchName;
                Branch.PhoneNumber = modle.Phone;
                Branch.Address = modle.Address;
                Branch.Manager = modle.ManagerName;

                await _context.SaveChangesAsync();
            }

        }
        public async Task UpdateSubBrancheAsync(string id, UpdateBrancheDto modle)
         {
            var Branch = await _context.Vendors.FindAsync(id);
            if (Branch != null)
            {
                Branch.BrancheName = modle.BranchName;
                Branch.PhoneNumber = modle.Phone;
                Branch.Address = modle.Address;
                Branch.ManagerName = modle.ManagerName;

                await _context.SaveChangesAsync();
            }
         }

        public async Task<IEnumerable<GetUserDto>> GetUsersByVendorOrOwnerAsync(ApplicationUser currentUser)
        {
            var currentUserRoles = await _userManager.GetRolesAsync(currentUser);

            if (currentUserRoles.Contains(VendorRole.Vendor.ToString()))
            {
                var vendorOwner = (VendorOwner)currentUser;

                var users = await _context.VendorOwners
                                .Include(i => i.Vendors)
                                .Include(i => i.Employees)
                                .Where(v => v.Id == vendorOwner.Id) // Filter for the current vendorOwner
                                .SelectMany(v => v.Vendors.Select(vendor => new GetUserDto
                                {
                                    Id = vendor.Id,
                                    FirstName = vendor.FristName,
                                    CreationDate = vendor.CreatedAt,
                                    Role = "Vendor",
                                    Status = vendor.EmailConfirmed.ToString()
                                })
                                .Concat(v.Employees.Select(employee => new GetUserDto
                                {
                                    Id = employee.Id,
                                    FirstName = employee.FristName,
                                    CreationDate = employee.CreatedAt,
                                    Role = employee.Role,
                                    Status = employee.EmailConfirmed.ToString()
                                })))
                                .ToListAsync();

                return users;
            }
            else if (currentUserRoles.Contains(VendorRole.SubVendor.ToString()))
            {
                var vendor = (Vendor)currentUser;

                var employees = await _context.Vendors
                                   .Include(i => i.Employees)
                                   .Where(v => v.Id == vendor.Id) // Filter for the current vendor
                                   .SelectMany(v => v.Employees.Select(employee => new GetUserDto
                                   {
                                       Id = employee.Id,
                                       FirstName = employee.FristName,
                                       CreationDate = employee.CreatedAt,
                                       Role = employee.Role,
                                       Status = employee.EmailConfirmed.ToString()
                                   }))
                                   .ToListAsync();

                return employees;
            }
            else if (currentUserRoles.Contains("Admin"))
            {
                var admin = (Admin)currentUser;

                var employees = await _context.Admins
                                   .Include(i => i.Employees)
                .Where(v => v.Id == admin.Id) // Filter for the current admin
                                   .SelectMany(v => v.Employees.Select(employee => new GetUserDto
                                   {
                                       Id = employee.Id,
                                       FirstName = employee.FristName,
                                       CreationDate = employee.CreatedAt,
                                       Role = employee.Role,
                                       Status = employee.EmailConfirmed.ToString()
                                   }))
                                   .ToListAsync();

                return employees;

            }
            else
            {
                throw new UnauthorizedAccessException("You don't have permission to access users.");
            }
        }

    }
}
