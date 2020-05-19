using System;
using System.Linq;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.MockGigPlatform.Api.Config;
using Jobtech.OpenPlatforms.MockGigPlatform.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Jobtech.OpenPlatforms.MockGigPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportDataController : ControllerBase
    {
        private readonly IDocumentStore _documentStore;
        private readonly GigDataServiceConfig _gigDataServiceConfig;

        public ExportDataController(IDocumentStoreHolder documentStore, IOptions<GigDataServiceConfig> gigDataServiceConfig)
        {
            this._documentStore = documentStore.Store;
            _gigDataServiceConfig = gigDataServiceConfig.Value;

            if (string.IsNullOrEmpty(_gigDataServiceConfig?.PlatformToken))
            {
                Serilog.Log.Fatal("Missing app setting for PlatformToken. Token {PlatformToken}", _gigDataServiceConfig?.PlatformToken);
                throw new Exception("Internal server error. ");
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> GetUserData()
        {
            UserDataRequest userDataRequest = new UserDataRequest(Request.Headers["platformToken"], Request.Headers["userEmail"], Request.Headers["requestId"]);
            return await GetUserData(userDataRequest);
        }

        [HttpPost("")]
        public async Task<IActionResult> GetUserData(UserDataRequest userDataRequest)
        {
            Serilog.Log.Information("Request {userDataRequest}", userDataRequest);

            // Check the PlatformToken for validity
            if (userDataRequest?.PlatformToken == null || userDataRequest.PlatformToken != _gigDataServiceConfig.PlatformToken)
            {
                Serilog.Log.Error("Incorrect PlatformToken. Token {PlatformToken}", _gigDataServiceConfig?.PlatformToken);
                
                // There are multiple registrations for the main mock platform 
                // service now, so disabling this requirement
                //throw new Exception("MyGigDataToken mismatch.");
            }

            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var user = await session
                    .Query<User>()
                    .Where(u => u.UserEmail == userDataRequest.UserEmail)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound(new
                    {
                        message = $"Unable to find that user. Request UserEmail: '{userDataRequest.UserEmail}'"
                    });
                }
                Serilog.Log.Information("User", user);

                UserDataResponse response = new UserDataResponse {
                    Id = user.Id,
                    RequestId = userDataRequest.RequestId,
                    UserEmail = user.UserEmail,
                    Interactions = user.Interactions,
                    Achievements = user.Achievements
                };

                return Ok(response);
            }
        }
    }
}