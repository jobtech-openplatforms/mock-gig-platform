using System.Collections.Generic;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.MockGigPlatform.Api.Models;
using Jobtech.OpenPlatforms.MockGigPlatform.Api.Models.GigCentral;

namespace Jobtech.OpenPlatforms.MockGigPlatform.Api.Handlers
{
    public interface IMockPlatformHttpClient
    {
        Task AddUserAsync(User user);
        Task<IEnumerable<UserModel>> GetUsersAsync();
    }
}