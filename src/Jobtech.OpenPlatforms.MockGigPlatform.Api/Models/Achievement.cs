using System;

namespace Jobtech.OpenPlatforms.MockGigPlatform.Api.Models
{
    public class Achievement
    {
        public DateTimeOffset TimeStamp { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AchievementScore Score { get; set; }
        public string BadgeIconUri { get; set; }


        public class AchievementScore
        {
            public decimal Value { get; set; }
            public string Label { get; set; }
        }
    }
}
