using System;
using System.Collections.Generic;

namespace Jobtech.OpenPlatforms.MockGigPlatform.Api.Models
{
    public class Interaction
    {
        public TimePeriod Period { get; set; }
        public string Id { get; set; }
        public Person Client { get; set; }
        public string Type { get; set; }
        public InteractionOutcome Outcome { get; set; }
        public AdditionalInfo AdditionalData { get; set; }

        public class Image
        {
            public string Uri { get; set; }
            public string Caption { get; set; }
        }

        public class TimePeriod
        {
            public DateTimeOffset? Start { get; set; }
            public DateTimeOffset? End { get; set; }
        }

        public class Person
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string PhotoUri { get; set; }
        }

        public class AdditionalInfo
        {
            public string Title { get; set; }
            public string Location { get; set; }
            public string Description { get; set; }
            public IEnumerable<Image> Images { get; set; }
            public decimal NoOfHours { get; set; }
            public Money Income { get; set; }
        }

        public class InteractionOutcome
        {
            public InteractionReview Review { get; set; }
            public IEnumerable<Rating> Ratings { get; set; } = new List<Rating>();
            //public InteractionRatings Ratings { get; set; }
            public bool? Success { get; set; }
        }

        public class Money
        {
            public static Money From(decimal amount, string currency)
                => new Money { Amount = amount, Currency = currency };

            public decimal Amount { get; set; }
            public string Currency { get; set; }
        }

        public class InteractionReview
        {
            public string Title { get; set; }
            public string Text { get; set; }
        }

        [Obsolete("Remove, unless adding additional info, like Average, to this")]
        public class InteractionRatings
        {
            // public int TotalCount { get { return DetailedRatings.Count(); } }
            // public double AverageRating { get { return DetailedRatings.Average(a => a.Value); } }

            public IEnumerable<Rating> DetailedRatings { get; set; } = new List<Rating>();
        }

        public class Rating
        {
            public double Min { get; set; }
            public double Max { get; set; }
            public string Name { get; set; }
            public double Value { get; set; }
        }
    }
}