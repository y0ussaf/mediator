using System.ComponentModel.DataAnnotations;

namespace IdentityServerHost.Quickstart.UI
{
    public class SendEmailConfirmationFormVm
    {
        public string ReturnUrl { get; set; }
        [Required]
        public string Email { get; set; }
    }
}