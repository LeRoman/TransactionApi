using GeoTimeZone;
using NodaTime;
using System.Globalization;
using TransactionApi.Entities;

namespace TransactionApi
{
    public static class TimeHelper
    {
        public static string GetTimeZoneByLocation(string location)
        {
            var coordinates = location.Split(',');
            var latitude = double.Parse(coordinates[0], CultureInfo.InvariantCulture);
            var longtitude = double.Parse(coordinates[1], CultureInfo.InvariantCulture);
            var timezone = TimeZoneLookup.GetTimeZone(latitude, longtitude).Result;
            return timezone;
        }

        public static int GetOffsetHoursByLocation(string location)
        {
            var timezone = GetTimeZoneByLocation(location);
            return - DateTimeZoneProviders.Tzdb.GetZoneOrNull(timezone).MaxOffset.ToTimeSpan().Hours;
        }

        public static DateTime GetUtcDateTime(DateTime transactionDate, int offsetHours )
        {
            var utc= new DateTimeOffset(transactionDate, TimeSpan.FromHours(offsetHours)).UtcDateTime;

            return utc;
            
        }

    }
}
