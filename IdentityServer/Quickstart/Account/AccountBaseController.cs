using System;
using System.Text;
using System.Threading.Tasks;
using IdentityServer.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace IdentityServerHost.Quickstart.UI
{
    public class AccountBaseController : Controller
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly SignInManager<ApplicationUser> _signInManager;
        protected readonly IIdentityServerInteractionService _interaction;
        protected readonly IClientStore _clientStore;
        protected readonly IAuthenticationSchemeProvider _schemeProvider;
        protected readonly IEventService _events;
        private readonly ILogger<AccountBaseController> _logger;

        public AccountBaseController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,ILogger<AccountBaseController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _logger = logger;
        }
        
        public async Task SendConfirmationEmail(ApplicationUser user, string returnUrl)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Action("ConfirmEmail","AccountConfirmation", new {code, id = user.Id,returnUrl});
            // send email here 
            // or grab the the url from the console and visit it 
            _logger.LogInformation($"email confirmation url :{callbackUrl}");
        }
    }
}