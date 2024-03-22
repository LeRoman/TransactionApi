using Microsoft.AspNetCore.Mvc;
using TransactionApi.Entities;

namespace TransactionApi.Interfaces
{
    public interface IExportService
    {
        public byte[] GenerateExcel(string fileName, IEnumerable<Transaction> transactions);
    }
}
