using System;

namespace CamerackStudio.Models.Services
{
    public class CompetitionCalculator
    {
        public long? CalculateDiscount(long? discount, long? price)
        {
            long? amount = 0;
            long? disounted
                = discount * price / 100;
            amount = price - disounted;
            return amount;
        }
        public long CalculateTimeRating(DateTime competitionDate, DateTime uploadDate)
        {
            long rating = 0 ;
            if (competitionDate.AddDays(0).Date == uploadDate.Date)
            {
                rating = 20;
            }
            if (competitionDate.AddDays(1).Date == uploadDate.Date)
            {
                rating = 18;
            }
            if (competitionDate.AddDays(2).Date == uploadDate.Date)
            {
                rating = 16;
            }
            if (competitionDate.AddDays(3).Date == uploadDate.Date)
            {
                rating = 14;
            }
            if (competitionDate.AddDays(4).Date == uploadDate.Date)
            {
                rating = 12;
            }
            if (competitionDate.AddDays(5).Date == uploadDate.Date)
            {
                rating = 10;
            }
            if (competitionDate.AddDays(6).Date == uploadDate.Date)
            {
                rating = 8;
            }
            if (competitionDate.AddDays(7).Date == uploadDate.Date)
            {
                rating = 6;
            }
            if (competitionDate.AddDays(8).Date == uploadDate.Date)
            {
                rating = 5;
            }
            if (competitionDate.AddDays(9).Date == uploadDate.Date)
            {
                rating = 4;
            }
            if (competitionDate.AddDays(10).Date == uploadDate.Date)
            {
                rating = 3;
            }
            if (competitionDate.AddDays(11).Date == uploadDate.Date)
            {
                rating = 2;
            }
            if (competitionDate.AddDays(12).Date == uploadDate.Date)
            {
                rating = 1;
            }
            if (competitionDate.AddDays(13).Date == uploadDate.Date)
            {
                rating = 0;
            }
            if (competitionDate.AddDays(14).Date == uploadDate.Date)
            {
                rating = 0;
            }
            return rating;
        }

        public long CalculateUserAcceptanceRating(long usersCount,long uploadLikes)
        {
            long rating = 0;
            if (uploadLikes > 0)
            {
                rating = (uploadLikes / usersCount) * 45;
            }
           
            return rating;
        }
        public long CalculateTagsRating(long tagsCount)
        {
            long rating = 0;
            if (tagsCount > 0 && tagsCount <= 5)
            {
                rating = 5;
            }
            if (tagsCount > 5 && tagsCount <= 10)
            {
                rating = 10;
            }
            if (tagsCount > 10 && tagsCount <= 15)
            {
                rating = 15;
            }
            if (tagsCount > 15 && tagsCount <= 20)
            {
                rating = 20;
            }
            return rating;
        }
        public long CalculateDescriptionRating(string description,long? locationId, long? cameraId)
        {
            long rating = 0;
            if (description.Length <= 0)
            {
                rating = 0;
            }
            if (description.Length > 0 && description.Length <= 100)
            {
                rating = 3;
            }
            if (description.Length > 100 && description.Length <= 200)
            {
                rating = 6;
            }
            if (description.Length > 200 && description.Length <= 300)
            {
                rating = 9;
            }
            if (description.Length > 300)
            {
                rating = 15;
            }
            if (locationId > 0)
            {
                rating = rating + 5;
            }
            if (cameraId > 0)
            {
                rating = rating + 5;
            }
            return rating;
        }
    }
}
