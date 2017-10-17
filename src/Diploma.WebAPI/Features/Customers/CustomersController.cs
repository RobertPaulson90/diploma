using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.WebAPI.Features.Customers
{
    [Produces("application/json")]
    [Route("api/customers")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomersController : Controller
    {
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // GET: api/customers
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "Hello" };
        }

        // GET: api/customers/5
        [HttpGet("{id}", Name = "GetCustomers")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/customers
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/customers/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}
