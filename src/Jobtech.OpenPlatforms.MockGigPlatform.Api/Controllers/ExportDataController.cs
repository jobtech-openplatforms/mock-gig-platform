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

            if (string.IsNullOrEmpty(_gigDataServiceConfig?.MyGigDataToken))
            {
                Serilog.Log.Fatal("Missing app setting for MyGigDataToken. Token {MyGigDataToken}", _gigDataServiceConfig?.MyGigDataToken);
                throw new Exception("Internal server error. ");
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> GetUserData()
        {
            UserDataRequest userDataRequest = new UserDataRequest(Request.Headers["myGigDataToken"], Request.Headers["username"], Request.Headers["requestId"]);
            return await GetUserData(userDataRequest);
        }

        [HttpPost("")]
        public async Task<IActionResult> GetUserData(UserDataRequest userDataRequest)
        {
            Serilog.Log.Information("Request {userDataRequest}", userDataRequest);

            // Check the MyGigDataToken for validity
            if (userDataRequest?.MyGigDataToken == null || userDataRequest.MyGigDataToken != _gigDataServiceConfig.MyGigDataToken)
            {
                Serilog.Log.Error("Incorrect MyGigDataToken. Token {MyGigDataToken}", _gigDataServiceConfig?.MyGigDataToken);
                
                // There are multiple registrations for the main mock platform 
                // service now, so disabling this requirement
                //throw new Exception("MyGigDataToken mismatch.");
            }

            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var user = await session
                    .Query<User>()
                    .Where(u => u.UserName == userDataRequest.Username)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound(new
                    {
                        message = $"Unable to find that user. Request Username: '{userDataRequest.Username}'"
                    });
                }
                Serilog.Log.Information("User", user);

                UserDataResponse response = new UserDataResponse {
                    Id = user.Id,
                    RequestId = userDataRequest.RequestId,
                    UserName = user.UserName,
                    Interactions = user.Interactions,
                    Achievements = user.Achievements
                };

                return Ok(response);
            }
        }
    }
}