using CsvHelper;
using Microsoft.Data.SqlClient;
using System.Globalization;
using TransactionApi.Entities;
using TransactionApi.Interfaces;
using GeoTimeZone;
using NodaTime;
using TransactionApi.MappingProfiles;
using TransactionApi.DbQueries;

namespace TransactionApi.Services
{
    public class CsvImportService : ICsvImportService
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;

        public CsvImportService(IConfiguration configuration)
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
            var timezone = TimeHelper.GetTimeZoneByLocation(transaction.Location);

            transaction.TimeZone = timezone;

            var offsetHours = TimeHelper.GetOffsetHoursByLocation(transaction.Location);

            transaction.TransactionDateUTC = TimeHelper.GetUtcDateTime(transaction.TransactionDate, offsetHours);

            return transaction;
        }
    }
}
