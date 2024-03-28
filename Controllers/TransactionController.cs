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
            _configuration = configuration;
            _csvHandler = csvservice;
            _transactionService = transactionService;
            _exportService = exportService;
        }

        /// <summary>
        /// Return transaction in requested time interval
        /// </summary>
        /// <param name="dateFrom" > Date interval start parameter</param>
        /// <param name="dateTo" >Date interval end parameter</param>
        /// <response code="200">Returns a JSON</response>
        [HttpGet("by-date-interval")]
        public IEnumerable<Transaction> GetByDateInterval(DateTime dateFrom, DateTime dateTo)
        {
            return _transactionService.GetTransactionsByDateInterval(dateFrom, dateTo);
        }

        /// <summary>
        /// Return transaction in requested time interval(in user timezone)
        /// </summary>
        /// <param name="dateFrom" > Date interval start parameter</param>
        /// <param name="dateTo" >Date interval end parameter</param>
        /// <param name="location" >user location parameter (coordinates)</param>
        /// <response code="200">Returns a JSON</response>
        [HttpGet("by-date-interval-in-user-timezone")]
        public IEnumerable<Transaction> GetByDateIntervalInUserTimezone(DateTime dateFrom, DateTime dateTo, string location)
        {
            return _transactionService.GetTransactionsByDateIntervalInUserTimezone(dateFrom, dateTo, location);
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
            var transactionslist = _transactionService.GetAllTransactions();
            var file = _exportService.GenerateExcel("transactions", transactionslist);
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "transactions.xlsx");
        }
    }



}
