using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using user_service.Models;
using user_service.Models.AccountViewModels;
using user_service.Services;
using user_service.Data;

namespace user_service.Controllers
{

    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IHostingEnvironment _environment;
        private readonly JWTSettings _options;
        private readonly string _uploadPath;
        private readonly int JwtTokenExpireTime = 60;

        public AccountController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            IHostingEnvironment environment,
            IOptions<JWTSettings> optionsAccessor)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _environment = environment;
            _options = optionsAccessor.Value;
            _uploadPath = Path.Combine(_environment.WebRootPath, "images\\upload\\users\\");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            var x = model;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    return new JsonResult(new Dictionary<string, object>
                    {
                        { "access_token", GetAccessToken(model.Email) },
                        { "id_token", GetIdToken(user) }
                    });
                }
                if (result.RequiresTwoFactor)
                {
                    return BadRequest(new { succeeded = false, errors = new string[] { "Require two factor." } });
                }
                if (result.IsLockedOut)
                {
                    return BadRequest(new { succeeded = false, errors = new string[] { "User account locked out." } });
                }
                else
                {
                    return BadRequest(new { succeeded = false, errors = new string[] { "Invalid login attempt." } });
                }
            }

            return BadRequest(new { succeeded = false, errors = GetValidationErrors() });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            // Model is valid
            if (ModelState.IsValid)
            {
                // Ensure only create user if Kong consumer was created
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        var kongService = new KongService();
                        var createConsumer = await kongService.CreateConsumer(user.Id);

                        if (createConsumer.IsSuccessStatusCode)
                        {
                            transaction.Commit();
                            return Ok(new { succeeded = true });
                        }
                        else
                        {
                            return BadRequest(new { succeeded = false, errors = new string[] { "Cannot create Kong consumer." } });
                        }
                    }

                    return BadRequest(result);
                }
            }

            return BadRequest(new { succeeded = false, errors = GetValidationErrors() });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, [FromBody]EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            string kongAction = "";

            if (user.LockoutEnd == DateTime.MaxValue && model.IsEnabled)
            {
                // Unlock user
                user.LockoutEnd = null;
                kongAction = "create";
            }
            else if (user.LockoutEnd != DateTime.MaxValue && !model.IsEnabled)
            {
                // Lock user
                user.LockoutEnd = DateTime.MaxValue;
                kongAction = "delete";
            }

            Utils.CopyProperties(model, user);

            if (ModelState.IsValid)
            {
                // Ensure only update user if Kong consumer was created or deleted
                using (var transaction = _context.Database.BeginTransaction())
                {
                    // Catch update database concurrency error
                    try
                    {
                        // if (newAvatar != null && newAvatar.Length > 0)
                        // {
                        //     // Todo: Validate upload files, resize images

                        //     // Catch save thumbnail error
                        //     try
                        //     {
                        //         var fileName = DateTime.Now.Ticks.ToString() + Path.GetExtension(newAvatar.FileName);

                        //         using (var stream = new FileStream(Path.Combine(_uploadPath, fileName), FileMode.Create))
                        //         {
                        //             // Copy new avatar
                        //             await newAvatar.CopyToAsync(stream);

                        //             // Delete old avatar
                        //             //System.IO.File.Delete(Path.Combine(_uploadPath, user.Avatar));

                        //             // Update new avatar
                        //             //user.Avatar = fileName;
                        //         }
                        //     }
                        //     catch
                        //     {
                        //         ModelState.AddModelError("Avatar", "Cannot save Avatar!");
                        //         return BadRequest(new { succeeded = false, errors = GetValidationErrors() });
                        //     }
                        // }

                        // Update User
                        await _userManager.UpdateAsync(user);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(user.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    var kongService = new KongService();
                    HttpResponseMessage action = null;

                    if (kongAction == "create")
                    {
                        action = await kongService.CreateConsumer(id);
                    }
                    else if (kongAction == "delete")
                    {
                        action = await kongService.DeleteConsumer(id);
                    }

                    if (action != null && !action.IsSuccessStatusCode)
                    {
                        return BadRequest(new { succeeded = false, errors = new string[] { $"Cannot {kongAction} Kong consumer." } });
                    }

                    transaction.Commit();
                    return Ok(new { succeeded = true });
                }
            }

            return BadRequest(new { succeeded = false, errors = GetValidationErrors() });
        }

        #region Helper   

        private bool UserExists(string id)
        {
            return _userManager.FindByIdAsync(id) != null;
        }

        private string GetIdToken(IdentityUser user)
        {
            var payload = new Dictionary<string, object>
            {
                { "id", user.Id },
                { "sub", user.Email },
                { "email", user.Email },
                { "emailConfirmed", user.EmailConfirmed },
            };
            return GetToken(payload);
        }

        private string GetAccessToken(string Email)
        {
            var payload = new Dictionary<string, object>
            {
                { "sub", Email },
                { "email", Email }
            };
            return GetToken(payload);
        }

        private string GetToken(Dictionary<string, object> payload)
        {
            var secret = _options.SecretKey;

            payload.Add("iss", _options.Issuer);
            payload.Add("aud", _options.Audience);
            payload.Add("nbf", Utils.ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("iat", Utils.ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("exp", Utils.ConvertToUnixTimestamp(DateTime.Now.AddMinutes(JwtTokenExpireTime)));

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, secret);
        }

        private List<string> GetValidationErrors()
        {
            var errors = new List<string>();
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }

            return errors;
        }

        #endregion HelperAction

    }
}
