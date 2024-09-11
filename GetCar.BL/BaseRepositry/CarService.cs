using GetCar.BL.CustomResponse;
using GetCar.BL.DTO.CarDTOs;
using GetCar.BL.GenericRepositry;
using GetCar.BL.Services;
using GetCar.DB.ApplicationDbContext;
using GetCar.DB.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.BaseRepositry
{
    public class CarService : ICarService
    {
        private readonly GetCarDbContext _context;
        private readonly ISaveFileService _saveFileService;
        private readonly IGenericRepositry<Category> _categoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CarService(GetCarDbContext context, ISaveFileService saveFileService, IGenericRepositry<Category> categoryRepository, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _saveFileService = saveFileService;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
        }

        public async Task<CarDto> CreateCarAsync(CreateCarDto createCarDto)
        {
            if (createCarDto.VendorOwnerID != null)
            {
                // Check if VendorOwnerID exists
                var vendorOwnerExists = await _context.Users.AnyAsync(u => u.Id == createCarDto.VendorOwnerID);
                if (!vendorOwnerExists)
                {
                    throw new Exception("Invalid VendorOwnerID. The specified Vendor Owner does not exist.");
                }
            }
            if (createCarDto.VendorID != null)
            {
                // Check if VendorID exists
                var vendorExists = await _context.Vendors.AnyAsync(v => v.Id == createCarDto.VendorID);
                if (!vendorExists)
                {
                    throw new Exception("Invalid VendorID. The specified Vendor does not exist.");
                }
            }

            // Check if CategoryID exists
            var categoryExists = await _categoryRepository.GetByIdAsync(i => i.CategoryID == createCarDto.CategoryID);
            if (categoryExists == null)
            {
                throw new Exception("Invalid CategoryID. The specified Category does not exist.");
            }


            //if (createCarDto.AvailableStartDate > createCarDto.AvailableEndDate)
            //{
            //    throw new Exception("Invalid AvailablityDate. The AvailableEndDate must bigger than AvailableStartDate.");
            //}

            var car = new Car
            {
                VendorOwnerID = createCarDto.VendorOwnerID,
                VendorID = createCarDto.VendorID,
                CategoryID = createCarDto.CategoryID,
                Name = createCarDto.Name,
                Type = createCarDto.Type,
                People = createCarDto.People,
                Liter = createCarDto.Liter,
                Doors = createCarDto.Doors,
                Make = createCarDto.Make,
                Model = createCarDto.Model,
                Year = createCarDto.Year,
                Description = createCarDto.Description,
                PricePerDay = createCarDto.PricePerDay,
                Availability = createCarDto.Availability,
                PickupLocation = createCarDto.PickupLocation,
                AirConditionServices = createCarDto.AirConditionServices,
                NavigationSystemServices = createCarDto.NavigationSystemServices,
                ToddlerSeatServices = createCarDto.ToddlerSeatServices,
                ProtectionTitle=createCarDto.ProtectionTitle,
                ProtectionDescription=createCarDto.ProtectionDescription,
                CancellationPolicy = createCarDto.CancellationPolicy,
                ABSBrakes = createCarDto.ABSBrakes,
                Airbag=createCarDto.Airbag,
                Audioinput= createCarDto.Audioinput,
                Bluetooth = createCarDto.Bluetooth,
                Cruisecontrol = createCarDto.Cruisecontrol,
                EBDbrakes = createCarDto.EBDbrakes,
                Electricmirrors = createCarDto.Electricmirrors,
                Foglights = createCarDto.Foglights,
                GPS = createCarDto.GPS,
                Power = createCarDto.Power,
                Remotecontrol = createCarDto.Remotecontrol,
                Sensors = createCarDto.Sensors,
                Roofbox = createCarDto.Roofbox,
                USBInput = createCarDto.USBInput,
                CDplayer = createCarDto.CDplayer,
                WithDriver = createCarDto.WithDriver
                
            };
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();

            // Initialize Calendar for  available Duration////----------------------------------------
            //var startDate = createCarDto.AvailableStartDate;
            //var endDate = createCarDto.AvailableEndDate;

            //var calendarEntries = new List<Calendar>();

            //for (var date = startDate; date <= endDate; date = date.AddDays(1))
            //{
            //    calendarEntries.Add(new Calendar
            //    {
            //        Date = date,
            //        IsAvailable = true,
            //        CarID = car.CarID
            //    });
            //}

            // Add all calendar entries in a single operation
            //await _context.Calendars.AddRangeAsync(calendarEntries);
            //await _context.SaveChangesAsync();


            if (createCarDto.Image1 != null)
            {

                var path = await _saveFileService.SaveFileAsync(createCarDto.Image1);
                var newImage = new Image
                {
                    CarID = car.CarID,
                    Description = createCarDto.ImageDescription1,
                    ImageURL = path

                };
                await _context.Images.AddAsync(newImage);
                await _context.SaveChangesAsync();

            }
            if (createCarDto.Image2 != null)
            {

                var path = await _saveFileService.SaveFileAsync(createCarDto.Image2);
                var newImage = new Image
                {
                    CarID = car.CarID,
                    Description = createCarDto.ImageDescription2,
                    ImageURL = path

                };
                await _context.Images.AddAsync(newImage);
                await _context.SaveChangesAsync();

            }
            if (createCarDto.Image3 != null)
            {

                var path = await _saveFileService.SaveFileAsync(createCarDto.Image3);
                var newImage = new Image
                {
                    CarID = car.CarID,
                    Description = createCarDto.ImageDescription3,
                    ImageURL = path

                };
                await _context.Images.AddAsync(newImage);
                await _context.SaveChangesAsync();

            }
            if (createCarDto.Image4 != null)
            {

                var path = await _saveFileService.SaveFileAsync(createCarDto.Image4);
                var newImage = new Image
                {
                    CarID = car.CarID,
                    Description = createCarDto.ImageDescription4,
                    ImageURL = path

                };
                await _context.Images.AddAsync(newImage);
                await _context.SaveChangesAsync();

            }
            if (createCarDto.Image5 != null)
            {

                var path = await _saveFileService.SaveFileAsync(createCarDto.Image5);
                var newImage = new Image
                {
                    CarID = car.CarID,
                    Description = createCarDto.ImageDescription5,
                    ImageURL = path

                };
                await _context.Images.AddAsync(newImage);
                await _context.SaveChangesAsync();

            }

            return await GetCarByIdAsync(car.CarID);
        }


        public async Task<bool> DeleteCarAsync(int carId)
        {
            var car = await _context.Cars.Include(c => c.Calendars).Include(c=>c.Bookings)
                                          .FirstOrDefaultAsync(c => c.CarID == carId);

            if (car == null)
                throw new InvalidOperationException("Invalid Car. The specified Car does not exist.");

            if (car.Availability == false)
                throw new InvalidOperationException("Invalid Delete. The specified Car is Busy.");

            var isBusy = car.Calendars.Any(calendar => calendar.Date > DateTime.Now);
         
            if (isBusy)
                throw new InvalidOperationException("Invalid Delete. The specified Car is Busy.");

            var isBooked=car.Bookings.Any(b => b.Status == BookingStatus.Confirmed ||
                                                           b.Status == BookingStatus.InProgress);
            if (isBooked)
                throw new InvalidOperationException("Invalid Delete. The specified Car is Busy.");


            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<CarDto>> GetAllCarsAsync()
        {
            var cars = await _context.Cars.Include(c => c.VendorOwner)
                .Include(c => c.Vendor)
                .Include(c => c.Category)
                .Include(c => c.Images)
                .Include(c => c.DropoffLocation)
                .Include(c=>c.Calendars)
                .ToListAsync();

            return cars.Select(c => new CarDto
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
                    Id = i.DropoffLocationId,
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
                DailyAvailability = c.Availability == true ? !c.Calendars.Where(i => i.CarID == c.CarID && i.Date.Date == DateTime.Now.Date)
            .Any(i => !i.IsAvailable) : false ,
                ProtectionDescription = c.ProtectionDescription,
                ProtectionTitle = c.ProtectionTitle,
                ToddlerSeatServices = c.ToddlerSeatServices,
                NavigationSystemServices = c.NavigationSystemServices,
                AirConditionServices = c.AirConditionServices,
                CancellationPolicy = c.CancellationPolicy,
                ABSBrakes = c.ABSBrakes,
                Airbag = c.Airbag,
                Audioinput = c.Audioinput,
                Bluetooth = c.Bluetooth,
                Cruisecontrol =c .Cruisecontrol,
                EBDbrakes = c.EBDbrakes,
                Electricmirrors = c.Electricmirrors,
                Foglights = c.Foglights,
                GPS = c.GPS,
                Power = c.Power,
                Remotecontrol = c.Remotecontrol,
                Sensors = c.Sensors,
                Roofbox = c.Roofbox,
                USBInput = c.USBInput,
                CDplayer = c.CDplayer,
                WithDriver=c.WithDriver
            }).ToList(); 
        }
        public async Task<CarDto> GetCarByIdAsync(int carId)
        {
            var car = await _context.Cars.Include(c => c.VendorOwner)
                .Include(c => c.Vendor)
                .Include(c => c.Category)
                .Include(c => c.Images)
                .Include(c => c.Calendars)
                .Include(c => c.DropoffLocation)
                .FirstOrDefaultAsync(c => c.CarID == carId);

            if (car == null)
                return null;

            return new CarDto
            {
                CarID = car.CarID,
                Name = car.Name,
                VendorID = car.VendorID,
                VendorOwnerID = car.VendorOwnerID,
                VendorName = car.VendorOwner?.CompanyName,
                BrancheName = car.Vendor?.BrancheName,
                CategoryID = car.CategoryID,
                CategoryName = car.Category?.CategoryName,
                PickupLocation = car.PickupLocation,
                DropoffLocation = car.DropoffLocation.Select(i => new GetDropoffLocationDto
                {
                    Id=i.DropoffLocationId,
                    Address = i.Address,
                    City = i.City
                }).ToList(),
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                Doors = car.Doors,
                Liter = car.Liter,
                People = car.People,
                Type = car.Type,
                Description = car.Description,
                PricePerDay = car.PricePerDay,
                Availability = car.Availability,
                ImageURLs = car.Images?.Select(i => i.ImageURL),
                DailyAvailability = car.Availability == true ? !car.Calendars.Where(i => i.CarID == car.CarID && i.Date.Date == DateTime.Now.Date)
                   .Any(i => !i.IsAvailable) : false,
                ProtectionDescription = car.ProtectionDescription,
                ProtectionTitle = car.ProtectionTitle,
                ToddlerSeatServices = car.ToddlerSeatServices,
                NavigationSystemServices = car.NavigationSystemServices,
                AirConditionServices = car.AirConditionServices,
                CancellationPolicy = car.CancellationPolicy,
                ABSBrakes = car.ABSBrakes,
                Airbag = car.Airbag,
                Audioinput = car.Audioinput,
                Bluetooth = car.Bluetooth,
                Cruisecontrol = car.Cruisecontrol,
                EBDbrakes = car.EBDbrakes,
                Electricmirrors = car.Electricmirrors,
                Foglights = car.Foglights,
                GPS = car.GPS,
                Power = car.Power,
                Remotecontrol = car.Remotecontrol,
                Sensors = car.Sensors,
                Roofbox = car.Roofbox,
                USBInput = car.USBInput,
                CDplayer = car.CDplayer,
                WithDriver=car.WithDriver
            };
        }

        public async Task<bool> UpdateCarAsync(int id, UpdateCarDto updateCarDto)
        {
            if (updateCarDto.VendorOwnerID != null)
            {
                // Check if VendorOwnerID exists
                var vendorOwnerExists = await _context.Users.AnyAsync(u => u.Id == updateCarDto.VendorOwnerID);
                if (!vendorOwnerExists)
                {
                    throw new Exception("Invalid VendorOwnerID. The specified Vendor Owner does not exist.");
                }
            }
            if (updateCarDto.VendorID != null)
            {
                // Check if VendorID exists
                var vendorExists = await _context.Vendors.AnyAsync(v => v.Id == updateCarDto.VendorID);
                if (!vendorExists)
                {
                    throw new Exception("Invalid VendorID. The specified Vendor does not exist.");
                }
            }

            // Check if CategoryID exists
            var categoryExists = await _categoryRepository.GetByIdAsync(i => i.CategoryID == updateCarDto.CategoryID);
            if (categoryExists == null)
            {
                throw new Exception("Invalid CategoryID. The specified Category does not exist.");
            }

            var car = await _context.Cars.FindAsync(id);

            if (car == null) return false;

            if (car.Availability == false)///////////////////if booking will update it after make booking service
            {
                throw new Exception("Invalid Update. The specified Car is Booking now.");
            }
            // Update properties
            car.Make = updateCarDto.Make;
            car.Name = updateCarDto.Name;
            car.Model = updateCarDto.Model;
            car.CategoryID = updateCarDto?.CategoryID;
            car.VendorOwnerID = updateCarDto?.VendorOwnerID;
            car.VendorID = updateCarDto?.VendorID;
            car.Description = updateCarDto.Description;
            car.PricePerDay = updateCarDto.PricePerDay;
            car.Doors = updateCarDto.Doors;
            car.People = updateCarDto.People;
            car.Type = updateCarDto.Type;
            car.Liter = updateCarDto.Liter;
            car.Availability = updateCarDto.Availability;
            car.Make = updateCarDto.Make;
            car.Model = updateCarDto.Model;
            car.Year = updateCarDto.Year;
            car.Description = updateCarDto.Description;
            car.CancellationPolicy = updateCarDto.CancellationPolicy;
            car.AirConditionServices = updateCarDto.AirConditionServices;
            car.NavigationSystemServices = updateCarDto.NavigationSystemServices;
            car.NavigationSystemServices= updateCarDto.NavigationSystemServices;
            car.ProtectionTitle = updateCarDto.ProtectionTitle;
            car.ProtectionDescription = updateCarDto.ProtectionDescription;
            car.ABSBrakes = updateCarDto.ABSBrakes;
            car.Airbag= updateCarDto.Airbag;
            car.GPS = updateCarDto.GPS;
            car.Sensors = updateCarDto.Sensors;
            car.EBDbrakes = updateCarDto.EBDbrakes;
            car.Cruisecontrol = updateCarDto.Cruisecontrol;
            car.CDplayer = updateCarDto.CDplayer;
            car.Audioinput = updateCarDto.Audioinput;
            car.Bluetooth= updateCarDto.Bluetooth;
            car.Electricmirrors = updateCarDto.Electricmirrors;
            car.Power= updateCarDto.Power;
            car.Roofbox= updateCarDto.Roofbox;
            car.Foglights= updateCarDto.Foglights;
            car.WithDriver = updateCarDto.WithDriver;

            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<IEnumerable<Car>> SearchAvailableCarsAsync(CarSearchCriteriaDto searchCriteria)
        {
            //for more search perform
            //  var query = _context.Cars
            //          .Include(c => c.VendorOwner)
            //          .Include(c => c.Category)
            //          .Include(c => c.Images)
            //          .Include(c => c.DropoffLocation)
            //          .Include(c => c.Vendor)
            //.Include(c => c.Calendars)
            //.Where(c => c.PickupLocation.Contains(searchCriteria.PickupLocation) && c.Calendars.Any(cal => cal.Date >= searchCriteria.PickupDate && cal.Date <= searchCriteria.DropoffDate) &&
            //      c.Calendars
            //          .Where(cal => cal.Date >= searchCriteria.PickupDate && cal.Date <= searchCriteria.DropoffDate)
            //          .Count() >= (searchCriteria.DropoffDate - searchCriteria.PickupDate).TotalDays + 1 &&
            //      c.Calendars
            //          .Where(cal => cal.Date >= searchCriteria.PickupDate && cal.Date <= searchCriteria.DropoffDate)
            //          .All(cal => cal.IsAvailable))
            //          .AsQueryable();
            //  return await query.ToListAsync();

            var availableCars = await _context.Cars.Include(c => c.VendorOwner)
                      .Include(c => c.Category)
                      .Include(c => c.Images)
                      .Include(c => c.DropoffLocation)
                      .Include(c => c.Vendor)
                      .Include(c => c.Calendars)
             .Where(car => car.PickupLocation.Contains(searchCriteria.PickupLocation) && car.Availability).ToListAsync();

            var th = availableCars.Where(car => !car.Calendars
                .Any(cal =>
                    cal.CarID == car.CarID &&
                    cal.Date >= searchCriteria.PickupDate &&
                    cal.Date <= searchCriteria.DropoffDate &&
                    !cal.IsAvailable))
            .ToList();

            return th;
        }

        public async Task CreateDropoffCarAsync(int id, List<DropoffLocationDto> dropoffLocations)
        {
            // Check if Car exists
            var car = await _context.Cars.FindAsync(id);
            if (car==null)
            {
                throw new Exception("Invalid Car. The specified Car does not exist.");
            }

            var dropoffEntities = dropoffLocations.Select(Dropoff => new DropoffLocation
            {
                Address = Dropoff.Address,
                City = Dropoff.City,
                CarId = id,
            }).ToList();

            await _context.DropoffLocation.AddRangeAsync(dropoffEntities);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteDropoffCarAsync(int id)
        {
            // Check if Car exists
            var dropoffLocation = await _context.DropoffLocation.FindAsync(id);
            if (dropoffLocation == null)
            {
                throw new Exception("Invalid dropoffLocation. The specified dropoffLocation does not exist.");
            }

            _context.DropoffLocation.Remove(dropoffLocation);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCarAvailabilityAsync(int id, bool status, ApplicationUser currentUser)
        {
            var car = await _context.Cars
                           .Include(c => c.Calendars)
                           .Include(c => c.Bookings)
                           .FirstOrDefaultAsync(c => c.CarID == id);

            if (car == null)
            {
                throw new KeyNotFoundException($"Car with id: {id} not found");
            }

            var currentUserRoles = await _userManager.GetRolesAsync(currentUser);

            if (currentUserRoles.Any(role => role == "Finance" || role == "Manager" || role == "IT"))
            {
                var employee = (Employee)currentUser;
                if (!(car.VendorID == employee.VendorId || car.VendorOwnerID == employee.VendorOwnerId))
                {
                    throw new UnauthorizedAccessException($"You don't have permission to update this car");
                }
            }
            else if (currentUserRoles.Any(role => role == "Customer"))
            {
                throw new UnauthorizedAccessException($"Customers don't have permission to update cars");
            }
            else if (!(car.VendorID == currentUser.Id || car.VendorOwnerID == currentUser.Id))
            {
                throw new UnauthorizedAccessException($"You don't have permission to update this car");
            }

            bool hasUnavailableCalendarEntries = car.Calendars.Any(c => c.Date.Day >= DateTime.Now.Day
                                                                      && c.Date.Month >= DateTime.Now.Month && !c.IsAvailable);

            
            bool hasActiveBookings = car.Bookings.Any(b => b.Status == BookingStatus.Confirmed ||
                                                           b.Status == BookingStatus.InProgress);

           
            if (hasUnavailableCalendarEntries || hasActiveBookings)
            {
                throw new InvalidOperationException("Cannot change car availability while it is booked or unavailable in the calendar.");
            }
         
            if (car.Availability == status)
            {
                 throw new InvalidOperationException($"Car Availability is already {status}");
            }

             car.Availability = status;

            await _context.SaveChangesAsync();
        }

    }
}
