using CsvHelper;
using System.Globalization;

namespace TransactionApi.Services
{
    public class CsvService
    {
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
                        Amount = Convert.ToDecimal(csv.GetField("amount").Trim('$')),
                        TransactionDate = csv.GetField("transaction_date"),
                        Location = csv.GetField("client_location")
                    };
                    records.Add(record);
                }
                transactionList = records;
            }
            return transactionList;
        }
    }
}
