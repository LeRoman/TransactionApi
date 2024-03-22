using TransactionApi.Entities;

namespace TransactionApi.Interfaces
{
    public interface ITransactionService
    {
        IEnumerable<Transaction> GetTransactionsJanuary2024();
        IEnumerable<Transaction> GetTransactions2023();
        IEnumerable<Transaction> GetTransactionsJanuary2024InUserTimeZone();
        IEnumerable<Transaction> GetTransactions2023InUserTimeZone();
        public IEnumerable<Transaction> GetAllTransactions();

    }
}
