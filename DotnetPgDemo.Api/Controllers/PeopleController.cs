using DotnetPgDemo.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetPgDemo.Api.Controllers
{
    [Route("api/[controller]")] // localhost:5xxx/api/people
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PeopleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePerson(Person person)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _context.People.Add(person);
                await _context.SaveChangesAsync();
                return Ok(person); // 200 OK with created person
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new 
                { 
                    error = "An error occurred while creating the person",
                    message = ex.Message
                });
            }
        }   
    }
}
