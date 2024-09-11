using GetCar.BL.BaseRepositry;
using GetCar.BL.CustomResponse;
using GetCar.BL.DTO.AuthDtos;
using GetCar.BL.DTO.VendorOwnerDtos;
using GetCar.BL.Services;
using GetCar.DB.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GetCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorOwnerController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IEmailSender _emailSender;
        private readonly ISaveFileService _saveFileService;
        private readonly IVendorOwnerRepositry _vendorOwnerRepositry;

        public VendorOwnerController(UserManager<ApplicationUser> userManager, IConfiguration configuration, ITokenService tokenService, IEmailSender emailSender, ISaveFileService saveFileService, IVendorOwnerRepositry vendorOwnerRepositry)
        {
            _userManager = userManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _emailSender = emailSender;
            _saveFileService=saveFileService;
            _vendorOwnerRepositry = vendorOwnerRepositry;
        }

        [HttpPost("Vendor/Register")]
        public async Task<IActionResult> Register([FromForm] RegisterVendorOwnerDto model)
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

            ApplicationUser applicationUser = new VendorOwner
            {
                
                Address = model.Address,
                Email = model.Email,
                UserName = model.Email,
                CreatedAt = DateTime.Now,
                CompanyName=model.CompanyName,
                Manager=model.Manager,
                PhoneNumber=model.PhoneNumber,
                City=model.City,
                Governorate=model.Governorate,
                PostalCode=model.PostalCode,
                IdFace=await _saveFileService.SaveFileAsync(model.IdFace),//wait to convert,decode it from base64
                IdBack= await _saveFileService.SaveFileAsync(model.IdBack),
                CompanyLogo= await _saveFileService.SaveFileAsync(model.CompanyLogo),
                BusinessLicense= await _saveFileService.SaveFileAsync(model.BusinessLicense),
                InsuranceCertificates= await _saveFileService.SaveFileAsync(model.InsuranceCertificates),
                TaxIdNumber= await _saveFileService.SaveFileAsync(model.TaxIdNumber),
                Notes=model.Notes,  
            };

            var result = await _userManager.CreateAsync(applicationUser, model.Password);
            if (result.Succeeded)
            {
                
                 var userRole = VendorRole.Vendor;//vendorOwnerRole
                
                // Add the user to  role
                await _userManager.AddToRoleAsync(applicationUser, userRole.ToString());

                // Generate email confirmation token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "VendorOwner", new { token, email = applicationUser.Email }, Request.Scheme);

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

        [HttpPost("ResendConfirmEmail")]
        public async Task<IActionResult> ResendConfirmEmail([FromBody] ResendConfirmEmailDto model)
        {

            if (!ModelState.IsValid)
            {
                var response = new ApiResponse
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                };
                return BadRequest(response);
            }


            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(new ApiResponse
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid email."
                });
            }
            else if (user.EmailConfirmed)
            {
                return BadRequest(new ApiResponse
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Email is already confirmed."
                });
            }

            // Generate a new email confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Generate the confirmation link
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "VendorOwner", new { token, email = user.Email }, Request.Scheme);

            // Send the confirmation email
            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your account by clicking this link: {confirmationLink}");

            // Return success response
            return Ok(new ApiResponse
            {
                Data = null,
                StatusCode = StatusCodes.Status200OK,
                Message = "Confirmation link has been resent to your email."
            });
        }

        [HttpGet("Vendors")]
        public async Task<IActionResult> GetVendorsOwner()
        {
            var data =await _vendorOwnerRepositry.GetVendorOwner();
            
            return Ok(new ApiResponse
            {
                Data = data ,
                StatusCode = StatusCodes.Status200OK,
                Message = "Success",
            });

        }
        [HttpGet("Top10Vendor")]
        public async Task<IActionResult> GetTopVendorsOwner()
        {
            var data = await _vendorOwnerRepositry.GetTopVendorOwner();

            return Ok(new ApiResponse
            {
                Data = data,
                StatusCode = StatusCodes.Status200OK,
                Message = "Success",
            });

        }
        [HttpGet("TopVendor/{id}")]
        public async Task<IActionResult> GetVendorOwner(string id)
        {
            var data = await _vendorOwnerRepositry.GetVendorOwnerById(id);
           
            if (data == null)
            {
                return NotFound();
            }

            return Ok(new ApiResponse
            {
                Data = data,
                StatusCode = StatusCodes.Status200OK,
                Message = "Success",
            });

        }

        [HttpGet("GetVendorBranches")]
        public async Task<IActionResult> GetVendorBranches()
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
            try
            {
                var data = await _vendorOwnerRepositry.GetVendorBranches(currentUser);
                return Ok(new ApiResponse
                {
                    Data = data,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                });
            }
            catch (Exception ex) {

                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = ex.Message,
                });
            }
        

        }

        [HttpPut("UpdateBranche/{id}")]
        public async Task<IActionResult> VendorBranches(string id,UpdateBrancheDto modle)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Please login first with Your account"
                });
            }
            try
            {
                await _vendorOwnerRepositry.UpdateVendorBranche(currentUser,id,modle);
                return Ok(new ApiResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                });

            }
            catch(Exception ex) {

                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });


            }

           
        }

        /// <summary>
        /// Get All Users For Current[Admin,Vendor,SubVendor]
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Please login first with Your account"
                });
            }
            try
            {
                var data = await _vendorOwnerRepositry.GetUsersByVendorOrOwnerAsync(currentUser);
                
                return Ok(new ApiResponse
                {
                    Data=data,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                });

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


    }
}  
