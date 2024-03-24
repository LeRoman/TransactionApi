using ClosedXML.Excel;
using System.Data;
using TransactionApi.Entities;
using TransactionApi.Interfaces;

namespace TransactionApi.Services
{
    public class ExportService : IExportService
    {
        private readonly ITransactionService _transactionService;

        public ExportService(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public byte[] GenerateExcel(string fileName, IEnumerable<Transaction> transactions)
        {
            DataTable dataTable = new DataTable("Transacions");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Id"),
                new DataColumn("Name"),
                new DataColumn("Email"),
                new DataColumn("Amount"),
                new DataColumn("TransactionDate"),
                new DataColumn("Location"),
                new DataColumn("TimeZone")
            });

            foreach (var transaction in transactions)
            {
                dataTable.Rows.Add(transaction.Id, transaction.Name, transaction.Email, transaction.Amount,
                    transaction.TransactionDate, transaction.Location, transaction.TimeZone);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add(dataTable);
                workSheet.Worksheet.Columns().AdjustToContents();
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
