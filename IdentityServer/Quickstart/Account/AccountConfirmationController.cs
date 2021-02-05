using System;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IdentityServerHost.Quickstart.UI
{
    public class AccountConfirmationController : AccountBaseController
    {
        public AccountConfirmationController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            ILogger<AccountConfirmationController> logger

            ) : base(userManager, signInManager, interaction, clientStore, schemeProvider, events,logger)
        {
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendNewEmailConfirmation(string email,string returnUrl)
        {
            var user = await _userManager.FindByEmailAsync(email);
            await SendConfirmationEmail(user,returnUrl);
            return View("EmailConfirmationSent",new EmailConfirmationSentVm()
            {
                ReturnUrl = returnUrl,
                Email = user.Email
            });
        }
        
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string id,string code,string returnUrl)
        {
            code = Encoding.UTF8.GetString(Base64Url.Decode(code));
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                // give the user the ability to generate another email confirmation token
                return View("EmailConfirmationFailed",
                    new SendEmailConfirmationFormVm {ReturnUrl = returnUrl, Email = user.Email});
            }

            TempData["isEmailHasBeenJustConfirmed"] = true;
            return RedirectToAction("Login","Account",new {returnUrl});
        }

    }
}