using Microsoft.Data.SqlClient;
using NodaTime;
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

        public IEnumerable<Transaction> GetAllTransactions()
        {
            var transactionList = new List<Transaction>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand getCommand = SqlQueries.GetAll(connection);
                var sqlReader = getCommand.ExecuteReader();
                while (sqlReader.Read())
                {
                    transactionList.Add(GetTransactionFromRow(sqlReader));
                }
                sqlReader.Close();
            }
            return transactionList
                .Select(x => setToClientDateTime(x));
        }

        public IEnumerable<Transaction> GetTransactions2023()
        {
            var transactionList = new List<Transaction>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand getCommand = SqlQueries.Get2023(connection);
                var sqlReader = getCommand.ExecuteReader();
                while (sqlReader.Read())
                {
                    transactionList.Add(GetTransactionFromRow(sqlReader));
                }
                sqlReader.Close();
            }
            return transactionList
                .Select(x => setToClientDateTime(x));
        }

        public IEnumerable<Transaction> GetTransactions2023InUserTimeZone()
        {
            var transactionList = new List<Transaction>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand getCommand = SqlQueries.Get2023(connection);
                var sqlReader = getCommand.ExecuteReader();
                while (sqlReader.Read())
                {
                    transactionList.Add(GetTransactionFromRow(sqlReader));
                }
                sqlReader.Close();
            }
            return transactionList
                .Select(x => setToApiUserDateTime(x))
                .Where(y => y.TransactionDate.Year == 2023);
        }

        public IEnumerable<Transaction> GetTransactionsJanuary2024InUserTimeZone()
        {
            var transactionList = new List<Transaction>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand getCommand = SqlQueries.GetJanuary2024(connection);
                var sqlReader = getCommand.ExecuteReader();
                while (sqlReader.Read())
                {
                    transactionList.Add(GetTransactionFromRow(sqlReader));
                }
                sqlReader.Close();
            }

            return transactionList
                .Select(x => setToApiUserDateTime(x))
                .Where(x => x.TransactionDate.Year == 2024)
                .Where(x => x.TransactionDate.Month == 1);
        }

        public IEnumerable<Transaction> GetTransactionsJanuary2024()
        {
            var transactionList = new List<Transaction>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand getCommand = SqlQueries.GetJanuary2024(connection);
                var sqlReader = getCommand.ExecuteReader();
                while (sqlReader.Read())
                {
                    transactionList.Add(GetTransactionFromRow(sqlReader));
                }
                sqlReader.Close();
            }

            return transactionList
                .Select(x => setToClientDateTime(x));

        }

        private Transaction GetTransactionFromRow(SqlDataReader sqlReader)
        {
            var tt = sqlReader.GetString(5);
            return new Transaction
            {
                Id = sqlReader.GetString(0),
                Name = sqlReader.GetString(1),
                Email = sqlReader.GetString(2),
                Amount = sqlReader.GetDouble(3),
                TransactionDate = sqlReader.GetDateTime(4),
                Location = sqlReader.GetString(5),
                TimeZone = sqlReader.GetString(6),
            };
        }
        Transaction setToClientDateTime(Transaction transaction)
        {
            int hours = DateTimeZoneProviders.Tzdb.GetZoneOrNull(transaction.TimeZone).MaxOffset.ToTimeSpan().Hours;
            transaction.TransactionDate = transaction.TransactionDate.AddHours(hours);
            return transaction;
        }

        Transaction setToApiUserDateTime(Transaction transaction)
        {
            int hours = DateTimeZoneProviders.Tzdb.GetSystemDefault().MaxOffset.ToTimeSpan().Hours;
            int minHours = DateTimeZoneProviders.Tzdb.GetSystemDefault().MinOffset.ToTimeSpan().Hours;
            transaction.TransactionDate = transaction.TransactionDate.AddHours(hours);
            return transaction;
        }

    }
}
