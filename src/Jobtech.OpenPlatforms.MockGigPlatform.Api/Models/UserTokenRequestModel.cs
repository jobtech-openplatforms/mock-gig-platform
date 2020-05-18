using System.ComponentModel.DataAnnotations;

namespace Jobtech.OpenPlatforms.MockGigPlatform.Api.Models
{
    public class UserTokenRequestModel
    {
        [Required]
        public string PlatformToken { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
