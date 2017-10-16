using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.WebAPI.Features.Programmers
{
    [Produces("application/json")]
    [Route("api/programmers")]
    public class ProgrammersController : Controller
    {
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // GET: api/programmers
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET: api/programmers/5
        [HttpGet("{id}", Name = "GetProgrammers")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/programmers
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/programmers/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}
