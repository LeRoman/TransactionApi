using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransactionApi.DatabaseContext;
using TransactionApi.Interfaces;

namespace TransactionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private ICsvService _csvHandler;
        private readonly IConfiguration _configuration;

        public TransactionController(TransactionContext context,IConfiguration configuration, ICsvService csvservice)
        {
            _configuration= configuration;
            _csvHandler = csvservice;
        }
        // GET: api/<TransactionController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TransactionController>/5
        [HttpGet("{id}")]
        public string Get(string id)
        {

            return "value";
        }

        // POST api/<TransactionController>
        [HttpPost("upload")]
        public ActionResult Upload(IFormFile file)
        {
            try
            {
                _csvHandler.ReadFile(file);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            
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
