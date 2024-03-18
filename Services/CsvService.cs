using CsvHelper;
using Microsoft.Data.SqlClient;
using System.Globalization;
using TransactionApi.Entities;
using TransactionApi.Interfaces;

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
            var transactionList = new List<Transaction>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = new Transaction
                    {
                        Id = csv.GetField("transaction_id"),
                        Name = csv.GetField("name"),
                        Email = csv.GetField("email"),
                        Amount = Convert.ToDouble(csv.GetField("amount").Trim('$'), CultureInfo.InvariantCulture),
                        TransactionDate = csv.GetField("transaction_date"),
                        Location = csv.GetField("client_location")
                    };

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
                            SqlCommand addCommand = SqlQueries.AddToDbQuery(record, connection);
                            addCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}
