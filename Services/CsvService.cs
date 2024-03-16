using CsvHelper;
using Microsoft.Data.SqlClient;
using System.Globalization;
using TransactionApi.Entities;

namespace TransactionApi.Services
{
    public class CsvService : ICsvService
    {
        private readonly IConfiguration _configuration;

        public CsvService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<Transaction> ReadFile(IFormFile file)
        {
            var transactionList = new List<Transaction>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {

                var records = new List<Transaction>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = new Transaction
                    {
                        Id = csv.GetField("transaction_id"),
                        Name = csv.GetField("name"),
                        Email = csv.GetField("email"),
                        Amount = Convert.ToDecimal(csv.GetField("amount").Trim('$'),CultureInfo.InvariantCulture),
                        TransactionDate = csv.GetField("transaction_date"),
                        Location = csv.GetField("client_location")
                    };
                    records.Add(record);

                }
                transactionList = records;
                for (int i = 0; i < transactionList.Count; i++)
                {
                    var nam = transactionList[i].Name;
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {

                        connection.Open();
                        SqlCommand command = SqlQueries.AddToDbQuery(transactionList[i]);
                        command.Connection = connection;
                        command.ExecuteNonQuery();
                    }
                }

            }
            return transactionList;
        }
    }
}
