using System.ComponentModel.DataAnnotations;

namespace Jobtech.OpenPlatforms.MockGigPlatform.Api.Models
{
    public class UserTokenRequestModel
    {
        [Required]
        public string MyGigDataToken { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
