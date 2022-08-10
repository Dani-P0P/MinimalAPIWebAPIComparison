using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Minimal_WebApiComparison.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly DataContext _context;

        public PersonController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>>> Persons()
        {
            return Ok(await _context.Persons.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if(person == null)
                return NotFound("Person not found.");
            return Ok(person);
        }

        [HttpPost]
        public async Task<ActionResult<List<Person>>> AddPerson(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
            return Ok(await _context.Persons.ToListAsync());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Person>>> UpdatePerson(Person person, int id)
        {
            var dbPerson = await _context.Persons.FindAsync(id);
            if (dbPerson == null)
                return NotFound("Person not found.");

            dbPerson.FirstName = person.FirstName;
            dbPerson.LastName = person.LastName;
            dbPerson.BirthDate = person.BirthDate;

            await _context.SaveChangesAsync();
            return Ok(await _context.Persons.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Person>>> DeletePerson(int id)
        {
            var dbPerson = await _context.Persons.FindAsync(id);
            if (dbPerson == null)
                return NotFound("Person not found.");
            _context.Persons.Remove(dbPerson);
            await _context.SaveChangesAsync();
            return Ok(await _context.Persons.ToListAsync());
        }
    }
}
