using GetCar.BL.CustomResponse;
using GetCar.BL.DTO.AuthDtos;
using GetCar.BL.Services;
using GetCar.DB.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using IEmailSender = GetCar.BL.Services.IEmailSender;

namespace GetCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IEmailSender _emailSender;

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration, ITokenService tokenService,IEmailSender emailSender)
        {
            _userManager = userManager;
            _configuration= configuration;
            _tokenService= tokenService;
            _emailSender= emailSender;
        }

        [HttpPost("Register/Admin")]
        public async Task <IActionResult> Register(RegisterUserDto model)
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

            if (!currentUserRole.Any(role => role =="Admin")) // role VendorOwner
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "You don't have permission to create an Admin"
                });

            }

            if (!ModelState.IsValid)
            {
                var response = new ApiResponse {
                 Data=null,
                 StatusCode= StatusCodes.Status400BadRequest,
                 Errors= ModelState.Values.SelectMany(i => i.Errors).Select(e => e.ErrorMessage).ToList()
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

            ApplicationUser applicationUser = new Admin
            {

                FristName = model.FirstName,

                LastName = model.LastName,

                PhoneNumber = model.PhoneNumber,
               
                Gender = model.Gender,

                DateOfBirth = model.DateOfBirth,

                Address = model.Address,

                Email = model.Email,

                UserName = model.Email,// we will change it later

                CreatedAt= DateTime.Now,
                
            };

            var result = await _userManager.CreateAsync(applicationUser, model.Password);
            if (result.Succeeded)
            {
                // Add the user to  role
                await _userManager.AddToRoleAsync(applicationUser,"Admin");

                // Generate email confirmation token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token, email = applicationUser.Email }, Request.Scheme);

                // Send email with the confirmation link
                await _emailSender.SendEmailAsync(applicationUser.Email, "Confirm your email", $"Please Confirm your Account by Clicking this link or Copy and Paste a Link into a Browser: {confirmationLink}");


                // Return the created user and the token as a response
                return Ok( new ApiResponse{
                    Data=applicationUser,
                    StatusCode= StatusCodes.Status200OK,
                    Message="Created Success",
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

       
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    if(!user.EmailConfirmed) 
                    {
                        var Confirmeresponse = new ApiResponse
                        {
                            Data = null,
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = "Confirm your Email first!"
                        };
                        return BadRequest(Confirmeresponse);
                    }

                    var roles = await _userManager.GetRolesAsync(user);
                    var token = _tokenService.GenerateToken(user, roles);

                    return Ok(new ApiResponse
                    {
                        Data=new
                        {
                            token = token,
                            expires = DateTime.Now.AddDays(1)
                        },
                        StatusCode= StatusCodes.Status200OK,
                        Message= "login successfully"
                    });
                }

                var response1 = new ApiResponse
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message =  "Invalid email or password."
                };
                return BadRequest(response1);
            }

            var response = new ApiResponse
            {
                Data = null,
                StatusCode = StatusCodes.Status400BadRequest,
                Errors = ModelState.Values.SelectMany(i => i.Errors).Select(e => e.ErrorMessage).ToList()
            };
            return BadRequest(response);
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
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token, email = user.Email }, Request.Scheme);

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

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = ModelState.Values.SelectMany(i => i.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            // Get the current authenticated user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            if (!user.EmailConfirmed)
            {
                var Confirmeresponse = new ApiResponse
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Confirm your Email first!"
                };
                return BadRequest(Confirmeresponse);
            }

            // Change the user’s password
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new ApiResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Password changed successfully."
                });
            }

            return BadRequest(new ApiResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Errors = result.Errors.Select(e => e.Description).ToList()
            });
        }


        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = ModelState.Values.SelectMany(i => i.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("No user found with this email address.");
            }
            if (!user.EmailConfirmed)
            {
                var Confirmeresponse = new ApiResponse
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Confirm your Email first!"
                };
                return BadRequest(Confirmeresponse);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            //----------
            Random random = new Random();
            int randomNumber = random.Next(100, 1000); // Generates a number between 100 and 999
            //----
            var resetLink = Url.Action(nameof(ResetPassword), "Auth", new { token, email = user.Email,newPassword=$"GetCar{randomNumber}@" }, Request.Scheme);

            // Send resetLink via email (using SendGrid or other email service)
            await _emailSender.SendEmailAsync(user.Email, "Password Reset Request",
                $"Please reset your password by clicking this link: {resetLink}");

            return Ok(new ApiResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Password reset link has been sent to your email."
            });
        }


        [HttpGet("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string token, string email, string newPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = ModelState.Values.SelectMany(i => i.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Invalid email address.");
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    Message = $"Your Password is {newPassword} "
                });
            }

            return BadRequest(new 
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Errors = result.Errors.Select(e => e.Description).ToList()
            });
        }



    }






}
