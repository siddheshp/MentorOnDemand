using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MOD.AuthLibrary.Models;
using MOD.AuthLibrary.Repositories;

namespace MOD.AdminService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        IAdminRepository repository;
        public AdminController(IAdminRepository repository)
        {
            this.repository = repository;
        }
        // GET: api/Admin
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Admin/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Admin
        [HttpPost]
        public IActionResult Post([FromBody] Skill model)
        {
            if (ModelState.IsValid)
            {
                repository.AddSkill(model);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Admin/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
