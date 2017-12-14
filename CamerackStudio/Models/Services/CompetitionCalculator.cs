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
                rating = 10;
            }
            if (competitionDate.AddDays(1).Date == uploadDate.Date)
            {
                rating = 9;
            }
            if (competitionDate.AddDays(2).Date == uploadDate.Date)
            {
                rating = 8;
            }
            if (competitionDate.AddDays(3).Date == uploadDate.Date)
            {
                rating = 7;
            }
            if (competitionDate.AddDays(4).Date == uploadDate.Date)
            {
                rating = 6;
            }
            if (competitionDate.AddDays(5).Date == uploadDate.Date)
            {
                rating = 5;
            }
            if (competitionDate.AddDays(6).Date == uploadDate.Date)
            {
                rating = 4;
            }
            if (competitionDate.AddDays(7).Date == uploadDate.Date)
            {
                rating = 3;
            }
            if (competitionDate.AddDays(8).Date == uploadDate.Date)
            {
                rating = 2;
            }
            if (competitionDate.AddDays(9).Date == uploadDate.Date)
            {
                rating = 1;
            }
            return rating;
        }

        public long CalculateUserAcceptanceRating(long usersCount,long uploadLikes)
        {
            long rating = 0;
            if (uploadLikes > 0)
            {
                rating = (uploadLikes / usersCount) * 40;
            }
           
            return rating;
        }
    }
}
