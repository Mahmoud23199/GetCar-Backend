using GetCar.BL.CustomResponse;
using GetCar.BL.DTO.AuthDtos;
using GetCar.BL.Services;
using GetCar.DB.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GetCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IEmailSender _emailSender;

        public EmployeeController(UserManager<ApplicationUser> userManager, IConfiguration configuration, ITokenService tokenService, IEmailSender emailSender)
        {
            _userManager = userManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _emailSender = emailSender;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserDto model, EmployeeRole employeeRole)
        {
            var currentUser = await _userManager.GetUserAsync(User); // Get current user

            if (currentUser == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Please login first with an Admin, Vendor, or SubVendor account"
                });
            }
            if (employeeRole == 0)
            {
                ModelState.AddModelError("Role", "Role Not Found.");
                return BadRequest(ModelState);
            }

            var currentUserRole = await _userManager.GetRolesAsync(currentUser);

            if (!currentUserRole.Any(role => role == "Admin" ||
                                             role == VendorRole.Vendor.ToString() ||
                                             role == VendorRole.SubVendor.ToString()))
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "You don't have permission to create an Employee"
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email is already taken.");
                return BadRequest(ModelState);
            }

            Employee applicationUser = new Employee
            {
                FristName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Address = model.Address,
                Email = model.Email,
                UserName = model.Email,
                CreatedAt = DateTime.Now,
                EmailConfirmed = true,
                Role=employeeRole.ToString(),
            };

            // Set the relationship based on the current user's role
            if (currentUserRole.Contains("Admin"))
            {
                applicationUser.AdminId = currentUser.Id;
                applicationUser.WorksFor = "Admin";
            }
            else if (currentUserRole.Contains(VendorRole.Vendor.ToString()))
            {
                applicationUser.VendorOwnerId = currentUser.Id;
                applicationUser.WorksFor = "VendorOwner";

            }
            else if (currentUserRole.Contains(VendorRole.SubVendor.ToString()))
            {
                applicationUser.VendorId = currentUser.Id;
                applicationUser.WorksFor = "Vendor";

            }

            var result = await _userManager.CreateAsync(applicationUser, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(applicationUser, employeeRole.ToString());

                // Generate email confirmation token
               //  var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
               // var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token, email = applicationUser.Email }, Request.Scheme);

                // Send email with the confirmation link
               // await _emailSender.SendEmailAsync(applicationUser.Email, "Confirm your email", $"Please Confirm your Account by Clicking this link or Copy and Paste a Link into a Browser: {confirmationLink}");

                return Ok(new ApiResponse
                {
                    Data = applicationUser,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Created Successfully"
                });
            }
            else
            {
               // _logger.LogError("User creation failed for {Email}: {Errors}", model.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                });
            }
        }

    }
}
