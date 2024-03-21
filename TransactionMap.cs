using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using TransactionApi.Entities;
namespace TransactionApi
{
    public class TransactionMap : ClassMap<Transaction>
    {
        public TransactionMap()
        {
            Map(m => m.Id).Name("transaction_id");
            Map(m => m.Name).Name("name");
            Map(m => m.Email).Name("email");
            Map(m => m.Amount).Name("amount").Convert(args => Convert.ToDouble(args.Row.GetField("amount").Trim('$'), CultureInfo.InvariantCulture));
            Map(m => m.TransactionDate).Name("transaction_date").TypeConverterOption.Format("yyyy-MM-dd HH:mm:ss");   
            Map(m => m.Location).Name("client_location");
        }
    }

}
