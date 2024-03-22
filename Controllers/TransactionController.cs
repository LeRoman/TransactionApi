using GeoTimeZone;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransactionApi.DatabaseContext;
using TransactionApi.Interfaces;
using NodaTime;
using NodaTime.TimeZones;
using TransactionApi.Entities;

namespace TransactionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private ICsvService _csvHandler;
        private ITransactionService _transactionService;
        private readonly IConfiguration _configuration;

        public TransactionController(TransactionContext context,IConfiguration configuration, ICsvService csvservice, ITransactionService transactionService)
        {
            _configuration= configuration;
            _csvHandler = csvservice;
            _transactionService=transactionService;
        }
        

        [HttpGet("transactions/january-2024")]
        public IEnumerable<Transaction> GetTransactionsForJanuary2024()
        {
            return _transactionService.GetTransactionsJanuary2024();
        }


        [HttpGet("transactions/2023")]
        public IEnumerable<Transaction> GetTransactionsFor2023()
        {
            return _transactionService.GetTransactions2023();
        }

        [HttpGet("transactions/2023-in-user-timezone")]
        public IEnumerable<Transaction> GetTransactionsFor2023InUserTimeZone()
        {
            return _transactionService.GetTransactions2023InUserTimeZone();
        }

        [HttpGet("transactions/january-2024-in-user-timezone")]
        public IEnumerable<Transaction> GetTransactionsForJanuary2024InUserTimeZone()
        {
            return _transactionService.GetTransactionsJanuary2024InUserTimeZone();
        }


        [HttpPost("upload")]
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
    }
}
