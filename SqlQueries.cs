using Microsoft.Data.SqlClient;
using TransactionApi.Entities;

namespace TransactionApi
{
    public static class SqlQueries
    {
        public static SqlCommand AddToDbQuery(Transaction transaction, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand();

            command.Parameters.AddRange(GetSqlParameters(transaction));
            command.Connection = connection;
            command.CommandText = "INSERT INTO Transactions (Id,Name,Email,Amount,TransactionDate,Location,TimeZone)" +
               " VALUES (@Id,@Name,@Email,@Amount, @TransactionDate, @Location, @TimeZone)";

            return command;
        }

        public static SqlCommand GetFromDbQuery(Transaction transaction, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand();
            command.Parameters.AddRange(GetSqlParameters(transaction));
            command.Connection = connection;
            command.CommandText = "SELECT * FROM Transactions WHERE Id=@Id";

            return command;
        }

        internal static SqlCommand Get2023(SqlConnection connection)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = " SELECT * FROM Transactions WHERE YEAR(Transactiondate) = 2023";

            return command;
        }

        internal static SqlCommand GetAll(SqlConnection connection)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM Transactions";

            return command;
        }

        internal static SqlCommand GetJanuary2024(SqlConnection connection)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM Transactions WHERE Transactiondate <= '01.31.2024'  AND Transactiondate >= '01.01.2024' ";

            return command;
        }

        internal static SqlCommand UpdateDbQuery(Transaction transaction, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand();
            command.Parameters.AddRange(GetSqlParameters(transaction));
            command.Connection = connection;
            command.CommandText = "UPDATE Transactions SET Name=@Name, Email=@Email, Amount=@Amount," +
                "TransactionDate=@TransactionDate, Location=@Location, TimeZone=@TimeZone" +
               " WHERE @Id=Id";

            return command;
        }

        static SqlParameter[] GetSqlParameters(Transaction transaction)
        {
            var idParam = new SqlParameter("@Id", transaction.Id);
            var nameParam = new SqlParameter("@Name", transaction.Name);
            var emailParam = new SqlParameter("@Email", transaction.Email);
            var amountParam = new SqlParameter("@Amount", transaction.Amount);
            var dateParam = new SqlParameter("@TransactionDate", transaction.TransactionDate.ToUniversalTime());
            var locationParam = new SqlParameter("@Location", transaction.Location);
            var timeZoneParam = new SqlParameter("@TimeZone", transaction.TimeZone);

            return new[] { idParam, nameParam, emailParam, amountParam, dateParam, locationParam, timeZoneParam };
        }
    }
}
