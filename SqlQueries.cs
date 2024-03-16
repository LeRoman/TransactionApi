using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using TransactionApi.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TransactionApi
{
    public static class SqlQueries
    {
        public static SqlCommand AddToDbQuery(Transaction transaction)
        {
            //var result = $"INSERT INTO Transactions (Id,Name,Email,Amount,TransactionDate,Location)" +
            //    $" VALUES ('{transaction.Id}','{transaction.Name}','{transaction.Email}',{transaction.Amount.}," +
            //    $"'{transaction.TransactionDate}','{transaction.Location}')";
            //return result;
            SqlCommand command = new SqlCommand();

            // создаем параметр для имени
            var idParam = new SqlParameter("@Id", transaction.Id);
            var nameParam = new SqlParameter("@Name", transaction.Name);
            var emailParam = new SqlParameter("@Email", transaction.Email);
            var amountParam = new SqlParameter("@Amount", transaction.Amount);
            var dateParam = new SqlParameter("@TransactionDate", transaction.TransactionDate);
            var locationParam = new SqlParameter("@Location", transaction.Location);
            // добавляем параметр к команде
            command.Parameters.AddRange(new[] { idParam, nameParam, emailParam, amountParam, dateParam, locationParam });
            command.CommandText= "INSERT INTO Transactions (Id,Name,Email,Amount,TransactionDate,Location)" +
               " VALUES (@Id,@Name,@Email,@Amount, @TransactionDate, @Location)";
            return command;
        }
    }
}
