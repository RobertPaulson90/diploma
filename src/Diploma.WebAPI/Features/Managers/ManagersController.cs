using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.WebAPI.Features.Managers
{
    [Produces("application/json")]
    [Route("api/managers")]
    public class ManagersController : Controller
    {
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // GET: api/managers
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET: api/managers/5
        [HttpGet("{id}", Name = "GetManagers")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/managers
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/managers/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}
