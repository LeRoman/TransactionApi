using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransactionApi.Services;

namespace TransactionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private CsvService _csvHandler;
        private readonly TransactionContext _context;
        private readonly IConfiguration _configuration;

        public TransactionController(TransactionContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration= configuration;
        }
        // GET: api/<TransactionController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TransactionController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TransactionController>
        [HttpPost("upload")]
        public ActionResult Upload(IFormFile file)
        {
            _csvHandler = new CsvService(_configuration);

            var sad = _csvHandler.ReadFile(file);


            return Ok();
        }

        // PUT api/<TransactionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TransactionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
