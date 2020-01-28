namespace Jobtech.OpenPlatforms.MockGigPlatform.Api.Models
{
    public class UserDataRequest
    {
        public UserDataRequest(string myGigDataToken, string username, string requestId)
        {
            MyGigDataToken = myGigDataToken;
            Username = username;
            RequestId = requestId;
        }

        public string MyGigDataToken { get; }
        public string Username { get; }
        public string RequestId { get; }
    }
}
