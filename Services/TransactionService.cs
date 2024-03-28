using Microsoft.Data.SqlClient;
using NodaTime;
using TransactionApi.DbQueries;
using TransactionApi.Entities;
using TransactionApi.Interfaces;

namespace TransactionApi.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly string? _connectionString;

        public TransactionService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public IEnumerable<Transaction> GetTransactionsByDateInterval(DateTime dateFrom, DateTime dateTo)
        {
            var transactionListClientsTime = new List<Transaction>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand getCommand = SqlQueries.GetByDateInterval(connection);
                getCommand.Parameters.Add(new SqlParameter("@dateFrom", dateFrom));
                getCommand.Parameters.Add(new SqlParameter("@dateTo", dateTo));
                var sqlReader = getCommand.ExecuteReader();
                while (sqlReader.Read())
                {
                    transactionListClientsTime.Add(GetTransactionFromRow(sqlReader));
                }
                sqlReader.Close();
            }

            return transactionListClientsTime;
        }

        public IEnumerable<Transaction> GetTransactionsByDateIntervalInUserTimezone(DateTime dateFrom, DateTime dateTo, string location)
        {
            var offsetHours = TimeHelper.GetOffsetHoursByLocation(location);

            // bring  dateTime parameters to Utc time
            var dateFromInUTC = TimeHelper.GetUtcDateTime(dateFrom, offsetHours);
            var dateToInUTC = TimeHelper.GetUtcDateTime(dateTo, offsetHours);

            var transactionListClientsTime = new List<Transaction>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand getCommand = SqlQueries.GetByDateIntervalInUserTimezone(connection);
                getCommand.Parameters.Add(new SqlParameter("@dateFrom", dateFromInUTC));
                getCommand.Parameters.Add(new SqlParameter("@dateTo", dateToInUTC));
                var sqlReader = getCommand.ExecuteReader();
                while (sqlReader.Read())
                {
                    transactionListClientsTime.Add(GetTransactionFromRow(sqlReader));
                }
                sqlReader.Close();
            }

            return transactionListClientsTime;
        }

        public IEnumerable<Transaction> GetAllTransactions()
        {
            var transactionList = new List<Transaction>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand getCommand = SqlQueries.GetAllTransactionsQuery(connection);
                var sqlReader = getCommand.ExecuteReader();
                while (sqlReader.Read())
                {
                    transactionList.Add(GetTransactionFromRow(sqlReader));
                }
                sqlReader.Close();
            }
            return transactionList;
        }
       
        private Transaction GetTransactionFromRow(SqlDataReader sqlReader)
        {
            return new Transaction
            {
                Id = sqlReader.GetString(0),
                Name = sqlReader.GetString(1),
                Email = sqlReader.GetString(2),
                Amount = sqlReader.GetDouble(3),
                TransactionDate = sqlReader.GetDateTime(4),
                Location = sqlReader.GetString(6),
                TimeZone = sqlReader.GetString(7),
            };
        }
       
    }
}
