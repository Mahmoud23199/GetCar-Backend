using GetCar.BL.BaseRepositry;
using GetCar.BL.CustomResponse;
using GetCar.BL.DTO.AuthDtos;
using GetCar.BL.DTO.CustomerDtos;
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
    public class CustomerController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IEmailSender _emailSender;
        private readonly ISaveFileService _saveFileService;
        private readonly ICustomerService _customerService;

        public CustomerController(UserManager<ApplicationUser> userManager, ICustomerService customerService, ISaveFileService saveFileService ,IConfiguration configuration, ITokenService tokenService, IEmailSender emailSender)
        {
            _userManager = userManager;
            _customerService = customerService;
            _configuration = configuration;
            _tokenService = tokenService;
            _emailSender = emailSender;
            _saveFileService=saveFileService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterCustomerDto model)
        {
            if (!ModelState.IsValid)
            {
                var response = new ApiResponse
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = ModelState.Values.SelectMany(i => i.Errors).Select(e => e.ErrorMessage).ToList()
                };

                return BadRequest(response);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                ModelState.AddModelError("Email", "Email is already taken.");

                return BadRequest(ModelState);
            }

            // Create a new user with Identity

            ApplicationUser applicationUser = new Customer
            {

                FullName = model.Name,

                PhoneNumber = model.PhoneNumber,

                Email = model.Email,

                UserName = model.Email,// we will change it later

                CreatedAt = DateTime.Now,

            };

            var result = await _userManager.CreateAsync(applicationUser, model.Password);
            if (result.Succeeded)
            {
                //if (userRole == 0)
                //{
                //    userRole = UserRole.Customer;
                //}
                // Add the user to  role
                await _userManager.AddToRoleAsync(applicationUser, "Customer");

                // Generate email confirmation token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Customer", new { token, email = applicationUser.Email }, Request.Scheme);

                // Send email with the confirmation link
                await _emailSender.SendEmailAsync(applicationUser.Email, "Confirm your email", $"Please Confirm your Account by Clicking this link or Copy and Paste a Link into a Browser: {confirmationLink}");


                // Return the created user and the token as a response
                return Ok(new ApiResponse
                {
                    Data = applicationUser,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Created Success",
                });
            }
            else
            {
                // If the user creation failed, return a bad request response with the errors
                return BadRequest(new ApiResponse
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                });
            }

        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest(new ApiResponse { StatusCode = StatusCodes.Status400BadRequest, Message = "Invalid Email" });


            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {

                return Ok(new { Message = "Email confirmed successfully!" });
            }
            return BadRequest(new { Message = "Error confirming email." });
        }


        [HttpPost("AddDriver")]
        public async Task<IActionResult> AddDriver(CreateDriverDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = ModelState.Values.SelectMany(i => i.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }
            try
            {
                var isCustomer = await _customerService.IsCustomerAsync(model.CustomerId);
                if (isCustomer)
                {
                    var driver = new Driver
                    {
                        Age = model.Age,
                        City = model.City,
                        CustomerId = model.CustomerId,
                        DriverInfo = model.DriverInfo,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        IdBack = await _saveFileService.SaveFileAsync(model.IdBack),
                        IdFace = await _saveFileService.SaveFileAsync(model.IdFace),
                        LicenseBack= await _saveFileService.SaveFileAsync(model.LicenseBack),
                        LicenseFace= await _saveFileService.SaveFileAsync(model.LicenseFace),
                        LicenseNumber = model.LicenseNumber,
                        Phone = model.Phone,
                        IDNumber = model.IDNumber,
                        LastName = model.LastName,
                        
                    };
                    var addedDriver = await _customerService.AddDriverAsync(driver);
                    return Ok(new ApiResponse
                    {
                        Data = new DriverDto
                        {
                            Id = driver.Id,
                            IdFace = driver.IdFace,
                            IdBack = driver.IdBack,
                            IDNumber = driver.IDNumber,
                            LicenseFace = driver.LicenseFace,
                            LicenseBack = driver.LicenseBack,
                            FirstName=driver.FirstName,
                            LastName = driver.LastName,
                            DriverInfo= driver.DriverInfo,
                            Age = driver.Age,
                            City = driver.City,
                            CustomerId = driver.CustomerId,
                            Email = driver.Email,
                            LicenseNumber = driver.LicenseNumber,
                            Phone = driver.Phone,
                            
                        },
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Created Success",
                    });
                }
                else
                {
                    return NotFound(new ApiResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }

        }

        // Get all drivers
        [HttpGet("GetAllDrivers")]
        public async Task<IActionResult> GetAllDrivers()
        {
            var drivers = await _customerService.GetAllDriversAsync();

            return Ok(new ApiResponse
            {
                Data = drivers.Select(i=>new DriverDto
                {
                    Id = i.Id,
                    IdFace = i.IdFace,
                    IdBack = i.IdBack,
                    IDNumber = i.IDNumber,
                    LicenseFace = i.LicenseFace,
                    LicenseBack = i.LicenseBack,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    DriverInfo = i.DriverInfo,
                    Age = i.Age,
                    City = i.City,
                    CustomerId = i.CustomerId,
                    Email = i.Email,
                    LicenseNumber = i.LicenseNumber,
                    Phone = i.Phone,
                }).ToList(),
                StatusCode = StatusCodes.Status200OK,
                Message = "Success",
            });
        }

        // Get all drivers for a specific customer
        [HttpGet("GetCustomerDrivers/{customerId}")]
        public async Task<IActionResult> GetCustomerDrivers(string customerId)
        {
            var drivers = await _customerService.GetCustomerDriversAsync(customerId);
            if (drivers == null || !drivers.Any())
            {
                return NotFound("No drivers found for this customer");
            }

            return Ok(new ApiResponse
            {
                Data = drivers.Select(i => new DriverDto
                {
                    Id = i.Id,
                    IdFace = i.IdFace,
                    IdBack = i.IdBack,
                    IDNumber = i.IDNumber,
                    LicenseFace = i.LicenseFace,
                    LicenseBack = i.LicenseBack,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    DriverInfo = i.DriverInfo,
                    Age = i.Age,
                    City = i.City,
                    CustomerId = i.CustomerId,
                    Email = i.Email,
                    LicenseNumber = i.LicenseNumber,
                    Phone = i.Phone,
                }).ToList(),
                StatusCode = StatusCodes.Status200OK,
                Message = "Success",
            });
        }

    }
}
