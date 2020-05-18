namespace Jobtech.OpenPlatforms.MockGigPlatform.Api.Models
{
    public class UserDataRequest
    {
        public UserDataRequest(string platformToken, string userEmail, string requestId)
        {
            PlatformToken = platformToken;
            UserEmail = userEmail;
            RequestId = requestId;
        }

        public string PlatformToken { get; }
        public string UserEmail { get; }
        public string RequestId { get; }
    }
}
