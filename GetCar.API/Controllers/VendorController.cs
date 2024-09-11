using GetCar.BL.CustomResponse;
using GetCar.BL.DTO.AuthDtos;
using GetCar.BL.DTO.VendorDtos;
using GetCar.BL.Services;
using GetCar.DB.Entites;
using GetCar.DB.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GetCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IEmailSender _emailSender;
        private readonly ISaveFileService _saveFileService;
        public VendorController(UserManager<ApplicationUser> userManager, IConfiguration configuration, ITokenService tokenService, IEmailSender emailSender, ISaveFileService saveFileService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _emailSender = emailSender;
            _saveFileService = saveFileService;
        }

        [HttpPost("Subvendor/Register")]
        public async Task<IActionResult> Register([FromForm]RegisterVendorDto model,string? vendorOwnerId)
        {
            var currentUser = await _userManager.GetUserAsync(User); // Get current user

            if (currentUser == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Please login first with an VendorOwner account"
                });
            }

            var currentUserRole = await _userManager.GetRolesAsync(currentUser);

            if (currentUserRole.Any( role => role == VendorRole.Vendor.ToString())) // role VendorOwner
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email is already taken.");
                    return BadRequest(ModelState);
                }

                Vendor applicationUser = new Vendor
                {
                    ManagerName = model.ManagerName,
                    BrancheName = model.BrancheName,
                    City = model.City,
                    ContactInfo = model.ContactInfo,
                    Governorate = model.Governorate,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    Email = model.Email,
                    UserName = model.Email,
                    CreatedAt = DateTime.Now,
                    EmailConfirmed = true,
                    VendorOwnerId = currentUser.Id,
                    locationLicense = await _saveFileService.SaveFileAsync(model.locationLicense)

                };


                var result = await _userManager.CreateAsync(applicationUser, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(applicationUser, VendorRole.SubVendor.ToString());//subVendor

                    // Generate email confirmation token
                    //  var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                    // var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token, email = applicationUser.Email }, Request.Scheme);

                    // Send email with the confirmation link
                    // await _emailSender.SendEmailAsync(applicationUser.Email, "Confirm your email", $"Please Confirm your Account by Clicking this link or Copy and Paste a Link into a Browser: {confirmationLink}");

                    return Ok(new ApiResponse
                    {
                      
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
            else if (currentUserRole.Any(role =>role=="Admin"))
            {

                var existingUser = await _userManager.FindByEmailAsync(model.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email is already taken.");
                    return BadRequest(ModelState);
                }
                if (vendorOwnerId == null)
                {
                    ModelState.AddModelError("vendorOwnerId", "vendorOwnerId is Required.");
                    return BadRequest(ModelState);
                }

                Vendor applicationUser = new Vendor
                {
                    ManagerName = model.ManagerName,
                    BrancheName = model.BrancheName,
                    City = model.City,
                    ContactInfo = model.ContactInfo,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    Email = model.Email,
                    UserName = model.Email,
                    CreatedAt = DateTime.Now,
                    EmailConfirmed = true,
                    VendorOwnerId = vendorOwnerId,
                    locationLicense = await _saveFileService.SaveFileAsync(model.locationLicense)

                };


                var result = await _userManager.CreateAsync(applicationUser, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(applicationUser, VendorRole.SubVendor.ToString());//subVendor

                    // Generate email confirmation token
                    //  var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                    // var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token, email = applicationUser.Email }, Request.Scheme);

                    // Send email with the confirmation link
                    // await _emailSender.SendEmailAsync(applicationUser.Email, "Confirm your email", $"Please Confirm your Account by Clicking this link or Copy and Paste a Link into a Browser: {confirmationLink}");

                    return Ok(new ApiResponse
                    {
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
            else
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "You don't have permission to create an Vendor"
                });
            }
      
        }

    }
}
