using CsvHelper;
using Microsoft.Data.SqlClient;
using System.Globalization;
using TransactionApi.Entities;
using TransactionApi.Interfaces;
using GeoTimeZone;
using NodaTime;

namespace TransactionApi.Services
{
    public class CsvService : ICsvService
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;

        public CsvService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void ReadFile(IFormFile file)
        {

            using (var streamReader = new StreamReader(file.OpenReadStream()))
            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();

                csvReader.Context.RegisterClassMap<TransactionMap>();

                while (csvReader.Read())
                {
                    var record = csvReader.GetRecord<Transaction>();
                    record = SetTimeToUtcAndTimeZone(record);

                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        SqlCommand getCommand = SqlQueries.GetFromDbQuery(record, connection);
                        var sqlReader = getCommand.ExecuteReader();

                        if (sqlReader.HasRows)
                        {
                            SqlCommand updateCommand = SqlQueries.UpdateDbQuery(record, connection);
                            sqlReader.Close();
                            updateCommand.ExecuteNonQuery();
                        }

                        else
                        {
                            sqlReader.Close();
                            SqlCommand addCommand = SqlQueries.AddToDbQuery(record, connection);
                            addCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        

        private Transaction SetTimeToUtcAndTimeZone(Transaction transaction)
        {
            var coordinates = transaction.Location.Split(',');
            var latitude = double.Parse(coordinates[0], CultureInfo.InvariantCulture);
            var longtitude = double.Parse(coordinates[1], CultureInfo.InvariantCulture);
            var timezone = TimeZoneLookup.GetTimeZone(latitude, longtitude).Result;

            
            var offsetHours = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timezone)
                .MaxOffset.ToTimeSpan().Hours;
            transaction.TransactionDate = new DateTimeOffset(transaction.TransactionDate, new TimeSpan(offsetHours, 0, 0)).UtcDateTime;

            transaction.TimeZone = timezone;

            return transaction;
        }
    }
}
