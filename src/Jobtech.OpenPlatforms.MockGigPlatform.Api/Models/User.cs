using System.Collections.Generic;

namespace Jobtech.OpenPlatforms.MockGigPlatform.Api.Models
{
    public class User
    {
        public string Id { get;  set; }
        public string UserEmail { get;  set; }
        public IEnumerable<Interaction> Interactions { get; set; } = new List<Interaction>();
        public IEnumerable<Achievement> Achievements { get; set; } = new List<Achievement>();

    }

}
