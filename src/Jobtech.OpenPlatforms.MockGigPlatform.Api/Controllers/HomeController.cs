using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.MockGigPlatform.Api.Config;
using Jobtech.OpenPlatforms.MockGigPlatform.Api.Handlers;
using Jobtech.OpenPlatforms.MockGigPlatform.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace Jobtech.OpenPlatforms.MockGigPlatform.Api.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly IDocumentStore _documentStore;
        private readonly IMockPlatformHttpClient _mockPlatformHttpClient;

        public HomeController(IDocumentStoreHolder documentStore, IMockPlatformHttpClient mockPlatformHttpClient)
        {
            this._documentStore = documentStore.Store;
            _mockPlatformHttpClient = mockPlatformHttpClient;
        }

        [HttpGet("")]
        [Produces("text/html")]
        public async Task<IActionResult> Index()
        {
            var model = new HomeIndexViewModel();
            model.GigCentralUsers = await _mockPlatformHttpClient.GetUsersAsync();
            model.MockPlatformUsers = await GetAsync();
            return View(model);
        }

        public class HomeIndexViewModel
        {
            public IEnumerable<Models.GigCentral.UserModel> GigCentralUsers { get; set; }
            public IEnumerable<User> MockPlatformUsers { get; set; }
        }

        [HttpGet("dummy")]
        [Produces("text/html")]
        public async Task<IActionResult> PopulateDatabases()
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            var hours = rnd.Next(5, 2000);
            var hours2 = rnd.Next(5, 2000);
            var pricePerHour = rnd.Next(40, 120);
            var users = new List<User>();

            var user = new User
            {
                UserEmail = $"calle+test{rnd.Next(1, 9999)}@roombler.com",
                //UserAccessToken = Guid.NewGuid().ToString(),
                Interactions = new List<Interaction>
                {
                    new Interaction
                    {
                            Id = rnd.Next(2,12345678).ToString(),
                        Period = new Interaction.TimePeriod
                        {
                            Start = DateTimeOffset.Now.AddMonths(-rnd.Next(1,20)),
                            End = DateTimeOffset.Now.AddDays(-rnd.Next(1,26)),
                        },
                        Type = "Web design",
                        Client = new Interaction.Person
                        {
                            Id = rnd.Next(2,12345678).ToString(),
                            Name = "John Client",
                            PhotoUri = ""
                        },
                        Outcome = new Interaction.InteractionOutcome
                        {
                            Review = new Interaction.InteractionReview
                            {
                                Title = "Great freelancer",
                                Text = "I am so happy with this designer. Will hire again."
                            },
                            Ratings = new List<Interaction.Rating>
                            {
                                new Interaction.Rating
                                {
                                    Name = "Delivery on time",
                                    Min = 1,
                                    Max = 5,
                                    Value = 5
                                },
                                new Interaction.Rating
                                {
                                    Name = "Overall satisfaction",
                                    Min = 1,
                                    Max = 5,
                                    Value = 5
                                },
                                new Interaction.Rating
                                {
                                    Name = "Quality of work",
                                    Min = 1,
                                    Max = 5,
                                    Value = 5
                                },
                                new Interaction.Rating
                                {
                                    Name = "Communication",
                                    Min = 1,
                                    Max = 5,
                                    Value = 5
                                }
                            },
                            Success = true
                        },
                        AdditionalData = new Interaction.AdditionalInfo
                        {
                            NoOfHours  = hours,
                            Income = new Interaction.Money
                            {
                                Amount = hours * pricePerHour,
                                Currency = "EUR"
                            },
                            Description = "Web design",
                            Title = "Please help with my Wordpress website",
                            Location = "",
                            Images = new List<Interaction.Image>
                            {
                                new Interaction.Image
                                {
                                    Uri = "https://placeimg.com/900/600/tech",
                                    Caption = "Sample Tech image"
                                },
                                new Interaction.Image
                                {
                                    Uri = "https://placeimg.com/900/600/tech",
                                    Caption = "Sample Tech image"
                                },
                                new Interaction.Image
                                {
                                    Uri =  "https://placeimg.com/900/600/tech",
                                    Caption = "Sample Tech image"
                                },
                                new Interaction.Image
                                {
                                    Uri = "https://placeimg.com/900/600/tech",
                                    Caption = "Sample Tech image"
                                }
                            }
                        }
                    },
                    new Interaction
                    {
                            Id = rnd.Next(2,12345678).ToString(),

                        Period = new Interaction.TimePeriod
                        {
                            Start = DateTimeOffset.Now.AddMonths(-rnd.Next(1,20)),
                            End = DateTimeOffset.Now.AddDays(-rnd.Next(1,26)),
                        },
                        Type = "Programming",
                        Client = new Interaction.Person
                        {
                            Id = rnd.Next(2,12345678).ToString(),
                            Name = "Jane Client",
                            PhotoUri = ""
                        },
                        Outcome = new Interaction.InteractionOutcome
                        {
                            Review = new Interaction.InteractionReview
                            {
                                Title = "Great programmer",
                                Text = "I am so happy with this programmer. Will hire again."
                            },
                            Ratings = new List<Interaction.Rating>
                            {
                                new Interaction.Rating
                                {
                                    Name = "Delivery on time",
                                    Min = 1,
                                    Max = 5,
                                    Value = 5
                                },
                                new Interaction.Rating
                                {
                                    Name = "Overall satisfaction",
                                    Min = 1,
                                    Max = 5,
                                    Value = 5
                                },
                                new Interaction.Rating
                                {
                                    Name = "Quality of work",
                                    Min = 1,
                                    Max = 5,
                                    Value = 5
                                },
                                new Interaction.Rating
                                {
                                    Name = "Communication",
                                    Min = 1,
                                    Max = 5,
                                    Value = 5
                                }
                            },
                            Success = true
                        },
                        AdditionalData = new Interaction.AdditionalInfo
                        {
                            NoOfHours  = hours2,
                            Income = new Interaction.Money
                            {
                                Amount = hours2 * pricePerHour,
                                Currency = "EUR"
                            },
                            Description = "Programming in C#",
                            Title = "Create Microservice and deploy to Azure.",
                            Location = "Somewhere over the rainbow",
                            Images = new List<Interaction.Image>(),
                        }
                    }
                },
                Achievements = new List<Achievement>
                {
                    new Achievement
                    {
                        Id = rnd.Next(2,12345678).ToString(),
                        Name = "Top Talent",
                        Description = "The user is currently part of the top talent program.",
                        TimeStamp = DateTimeOffset.Now.AddDays(rnd.Next(-220,-4)),
                        Type = "Badge",
                        BadgeIconUri = "",
                        Score = new Achievement.AchievementScore
                        {
                            Value = 100,
                            Label = "percent"
                        }
                    },
                    new Achievement
                    {
                        Id = rnd.Next(2,12345678).ToString(),
                        Name = "Rising star",
                        Description = "The user is currently part of the rising star program.",
                        TimeStamp = DateTimeOffset.Now.AddDays(rnd.Next(-220,-4)),
                        Type = "Badge"
                    },
                    new Achievement
                    {
                        Id = rnd.Next(2,12345678).ToString(),
                        Name = "Success Rate",
                        Description = "The user has a success rate above 70%.",
                        TimeStamp = DateTimeOffset.Now.AddDays(rnd.Next(-220,-4)),
                        Type = "Badge",
                        BadgeIconUri = "",
                        Score = new Achievement.AchievementScore
                        {
                            Value = 90,
                            Label = "percent"
                        }
                    },
                    new Achievement
                    {
                        Id = rnd.Next(2,12345678).ToString(),
                        Name = "Featured",
                        Description = "The user is currently featured on our platform.",
                        TimeStamp = DateTimeOffset.Now.AddDays(rnd.Next(-220,-4)),
                        Type = "Badge"
                    }
                }
            };

            users.Add(await AddAsync(user));
            await AddTestUserToGigCentralAsync(user);

            return Content($"<html><body><p>Alright, I'm done. user is \n<br/>\n </p><pre>{JsonConvert.SerializeObject(users, Formatting.Indented)}</pre></body></html>");

            //var user = JsonConvert.DeserializeObject<User>(@"{
            //            ""userName"": ""Calle Roombler Test"",
            //            ""userId"": ""calle+test@roombler.com"",
            //            ""interactions"": [
            //                {
            //                    ""timestamp"": ""2018-10-10T00:00:00"",
            //                    ""interactionId"": """ + rnd.Next(100, 9999999) + @""",
            //                    ""clientId"": ""customer" + rnd.Next(100, 9999999) + @""",
            //                    ""clientName"": ""John's Customer"",
            //                    ""serviceType"": ""Home-Gardening"",
            //                    ""location"": ""Strandgatan 1, Hebbelelle, Svedala"",
            //                    ""textContent"": null,
            //                    ""images"": [],
            //                    ""numberOfHours"": 18.5,
            //                    ""income"": {
            //                        ""amount"": 18500,
            //                        ""currency"": ""EUR""
            //                    },
            //                    ""ratings"": {
            //                        ""detailedRatings"": [
            //                            {
            //                                ""min"": 1,
            //                                ""max"": 5,
            //                                ""success"": 3,
            //                                ""name"": ""Pruning trees"",
            //                                ""value"": 5
            //                            },
            //                            {
            //                                ""min"": 1,
            //                                ""max"": 5,
            //                                ""success"": 3,
            //                                ""name"": ""Cutting grass"",
            //                                ""value"": 5
            //                            }
            //                        ]
            //                    }
            //                }
            //            ]
            //        }");
            //users.Add(await AddAsync(user));
            //await AddTestUserToGigCentralAsync(user);

            //for (int i = 0; i < 6; i++)
            //{
            //    user = JsonConvert.DeserializeObject<User>(@"{
            //            ""userName"": ""John Jane " + rnd.Next(100, 9999999) + @""",
            //            ""userId"": """ + rnd.Next(100, 9999999) + @"test@test.test"",
            //            ""interactions"": [
            //                {
            //                    ""timestamp"": ""2018-09-10T00:00:00"",
            //                    ""interactionId"": """ + rnd.Next(100, 9999999) + @""",
            //                    ""clientId"": ""customer" + rnd.Next(100, 9999999) + @""",
            //                    ""clientName"": ""John's Customer"",
            //                    ""serviceType"": ""Home-Gardening"",
            //                    ""location"": ""Strandgatan 1, Hebbelelle, Svedala"",
            //                    ""textContent"": null,
            //                    ""images"": [],
            //                    ""numberOfHours"": 18.5,
            //                    ""income"": {
            //                        ""amount"": " + rnd.Next(100, 9999999) + @",
            //                        ""currency"": ""EUR""
            //                    },
            //                    ""ratings"": {
            //                        ""detailedRatings"": [
            //                            {
            //                                ""min"": 1,
            //                                ""max"": 5,
            //                                ""success"": 3,
            //                                ""name"": ""Pruning trees"",
            //                                ""value"": 5
            //                            },
            //                            {
            //                                ""min"": 1,
            //                                ""max"": 5,
            //                                ""success"": 3,
            //                                ""name"": ""Cutting grass"",
            //                                ""value"": 5
            //                            }
            //                        ]
            //                    }
            //                }
            //            ]
            //        }");
            //    users.Add(await AddAsync(user));
            //    await AddTestUserToGigCentralAsync(user);
            //}

            //return Content($"<html><body><p>Alright, I'm done. user is \n<br/>\n </p><pre>{JsonConvert.SerializeObject(users, Formatting.Indented)}</pre></body></html>");
        }

        public async Task<User> AddAsync(User request)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                if (await session
                    .Query<User>()
                    .Where(u => u.UserEmail == request.UserEmail)
                    .AnyAsync())
                    return null // this is really ugly, but not putting in time to create result classes for C# for this mock project
                    ;

                await session.StoreAsync(request);
                await session.SaveChangesAsync();

                return request;
            }
        }

        public async Task<IEnumerable<User>> GetAsync()
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                return await session
                    .Query<User>()
                    .ToListAsync();
            }
        }

        public async Task AddTestUserToGigCentralAsync(User request) => await _mockPlatformHttpClient.AddUserAsync(request);
    }
}