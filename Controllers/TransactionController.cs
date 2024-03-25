using Microsoft.AspNetCore.Mvc;
using TransactionApi.Entities;
using TransactionApi.Interfaces;

namespace TransactionApi.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    [Produces("application/json")]
    public class TransactionController : ControllerBase
    {
        private ICsvImportService _csvHandler;
        private ITransactionService _transactionService;
        private readonly IExportService _exportService;
        private readonly IConfiguration _configuration;

        public TransactionController(IConfiguration configuration, ICsvImportService csvservice, ITransactionService transactionService, IExportService exportService)
        {
            _configuration= configuration;
            _csvHandler = csvservice;
            _transactionService=transactionService;
            _exportService=exportService;
        }

        /// <summary>
        /// Returns transactions made in January 2024
        /// </summary>
        [HttpGet("january-2024")]
        public IEnumerable<Transaction> GetTransactionsForJanuary2024()
        {
            return _transactionService.GetTransactionsJanuary2024();
        }

        /// <summary>
        /// Returns transactions made in 2023
        /// </summary>
        [HttpGet("transactions/2023")]
        public IEnumerable<Transaction> GetTransactionsFor2023()
        {
            return _transactionService.GetTransactions2023();
        }

        /// <summary>
        /// Returns transactions made in 2023 (in user timezone)
        /// </summary>
        [HttpGet("2023-in-user-timezone")]
        public IEnumerable<Transaction> GetTransactionsFor2023InUserTimeZone()
        {
            return _transactionService.GetTransactions2023InUserTimeZone();
        }

        /// <summary>
        /// Returns transactions made in January 2024 (in user timezone)
        /// </summary>
        [HttpGet("january-2024-in-user-timezone")]
        public IEnumerable<Transaction> GetTransactionsForJanuary2024InUserTimeZone()
        {
            return _transactionService.GetTransactionsJanuary2024InUserTimeZone();
        }

        /// <summary>
        /// Imports csv file
        /// </summary>
        /// <response code="200">If imported successfully</response>
        /// <response code="400">If data is invalid</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("import-csv-file")]
        public ActionResult Upload(IFormFile file)
        {
            try
            {
                _csvHandler.ReadFile(file);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        /// <summary>
        /// Gets all transactions in excel file form
        /// </summary>
        [HttpGet("export-to-excel")]
        public ActionResult ExportToExcel()
        {
            var transactionslist =_transactionService.GetAllTransactions();
            var file = _exportService.GenerateExcel("transactions",transactionslist);
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "transactions.xlsx");
        }
    }
    


}
