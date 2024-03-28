using TransactionApi.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TransactionApi.Interfaces
{
    public interface ITransactionService
    {
        public IEnumerable<Transaction> GetAllTransactions();
        public IEnumerable<Transaction> GetTransactionsByDateInterval(DateTime dateFrom, DateTime dateTo);
        public IEnumerable<Transaction> GetTransactionsByDateIntervalInUserTimezone(DateTime dateFrom, DateTime dateTo, string location);
    }
}
