using Microsoft.Data.SqlClient;
using TransactionApi.Entities;

namespace TransactionApi.DbQueries
{
    public static class SqlQueries
    {
        public static SqlCommand AddToDbQuery(Transaction transaction, SqlConnection connection)
        {
            var commandText = "INSERT INTO Transactions (Id,Name,Email,Amount,TransactionDate, TransactionDateUTC, Location,TimeZone)" +
               " VALUES (@Id,@Name,@Email,@Amount, @TransactionDate,@TransactionDateUtc, @Location, @TimeZone)";
            var command = new SqlCommand(commandText, connection);
            command.Parameters.AddRange(GetSqlParameters(transaction));

            return command;
        }

        public static SqlCommand GetFromDbQuery(Transaction transaction, SqlConnection connection)
        {
            var commandText = "SELECT * FROM Transactions WHERE Id=@Id";
            var command = new SqlCommand(commandText, connection);
            command.Parameters.AddRange(GetSqlParameters(transaction));

            return command;
        }

        internal static SqlCommand GetByDateInterval(SqlConnection connection)
        {
            var commandText = "SELECT * FROM Transactions WHERE TransactionDate >=@dateFrom AND TransactionDate<=@dateTo";
            var command = new SqlCommand(commandText, connection);
            return command;
        }

        internal static SqlCommand GetByDateIntervalInUserTimezone(SqlConnection connection)
        {
            var commandText = "SELECT * FROM Transactions WHERE TransactionDateUTC >=@dateFrom AND TransactionDateUTC<=@dateTo";
            var command = new SqlCommand(commandText, connection);
            return command;
        }



        internal static SqlCommand GetAllTransactionsQuery(SqlConnection connection)
        {
            var commandText = "SELECT * FROM Transactions";
            return new SqlCommand(commandText, connection);
        }

        internal static SqlCommand UpdateDbQuery(Transaction transaction, SqlConnection connection)
        {
            var commandText = "UPDATE Transactions SET Name=@Name, Email=@Email, Amount=@Amount," +
                "TransactionDate=@TransactionDate,TransactionDateUTC=@TransactionDateUTC, Location=@Location, TimeZone=@TimeZone" +
                " WHERE @Id=Id";
            var command = new SqlCommand(commandText, connection);
            command.Parameters.AddRange(GetSqlParameters(transaction));

            return command;
        }

        static SqlParameter[] GetSqlParameters(Transaction transaction)
        {
            var idParam = new SqlParameter("@Id", transaction.Id);
            var nameParam = new SqlParameter("@Name", transaction.Name);
            var emailParam = new SqlParameter("@Email", transaction.Email);
            var amountParam = new SqlParameter("@Amount", transaction.Amount);
            var dateParam = new SqlParameter("@TransactionDate", transaction.TransactionDate);
            var dateParamUTC = new SqlParameter("@TransactionDateUtc", transaction.TransactionDateUTC);
            var locationParam = new SqlParameter("@Location", transaction.Location);
            var timeZoneParam = new SqlParameter("@TimeZone", transaction.TimeZone);

            return new[] { idParam, nameParam, emailParam, amountParam, dateParam, dateParamUTC, locationParam, timeZoneParam };
        }
    }
}
