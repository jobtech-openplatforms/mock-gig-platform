using System.Collections.Generic;
using System.Linq;
using Jobtech.OpenPlatforms.MockGigPlatform.Api.Config;
using Jobtech.OpenPlatforms.MockGigPlatform.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Jobtech.OpenPlatforms.MockGigPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IDocumentStore _documentStore;

        public UserController(IDocumentStoreHolder documentStore)
            => this._documentStore = documentStore.Store;

        [HttpGet("")]
        public ActionResult<IEnumerable<User>> Get()
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                return session
                    .Query<User>()
                    .ToList()
                    ;
            }
        }

        [HttpPost("add")]
        public ActionResult<User> Add([FromBody] User request)
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                if (session
                    .Query<User>()
                    .Where(u => u.UserName == request.UserName)
                    .Any())
                    return BadRequest(new { message = "A user with that UserName already exists." })
                    ;

                session.Store(request);
                session.SaveChanges();

                return request;
            }   
        }
    }
}