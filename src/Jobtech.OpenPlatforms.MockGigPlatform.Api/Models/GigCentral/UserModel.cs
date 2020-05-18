namespace Jobtech.OpenPlatforms.MockGigPlatform.Api.Models.GigCentral
{
    public class UserModel
    {
        public string Name { get; set; }
        public string DataVersion { get; set; }
        public string Photo { get; set; }
        public string Email { get; set; } // Unique identifier
    }

    public static class UserModelExtensions
    {
        public static UserModel ToGigCentralUserModel(this User user)
            => new UserModel { DataVersion =  "1.0", Email = user.UserEmail, Name = user.UserEmail, Photo = ""};
    }
}
