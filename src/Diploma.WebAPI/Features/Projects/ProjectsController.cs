using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.WebAPI.Features.Projects
{
    [Produces("application/json")]
    [Route("api/projects")]
    public class ProjectsController : Controller
    {
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // GET: api/projects
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET: api/projects/5
        [HttpGet("{id}", Name = "GetProjects")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/projects
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/projects/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}
