using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using skill_up.Context;
using skill_up.Models;

namespace skill_up.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrgaoEmissorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrgaoEmissorController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/OrgaoEmissor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrgaoEmissor>>> GetOrgaoEmissor()
        {
          if (_context.OrgaoEmissor == null)
          {
              return NotFound();
          }
            return await _context.OrgaoEmissor.ToListAsync();
        }

        // GET: api/OrgaoEmissor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrgaoEmissor>> GetOrgaoEmissor(int id)
        {
          if (_context.OrgaoEmissor == null)
          {
              return NotFound();
          }
            var orgaoEmissor = await _context.OrgaoEmissor.FindAsync(id);

            if (orgaoEmissor == null)
            {
                return NotFound();
            }

            return orgaoEmissor;
        }

        // PUT: api/OrgaoEmissor/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrgaoEmissor(int id, OrgaoEmissor orgaoEmissor)
        {
            if (id != orgaoEmissor.OrgaoEmissorId)
            {
                return BadRequest();
            }

            _context.Entry(orgaoEmissor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrgaoEmissorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/OrgaoEmissor
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrgaoEmissor>> PostOrgaoEmissor(OrgaoEmissor orgaoEmissor)
        {
          if (_context.OrgaoEmissor == null)
          {
              return Problem("Entity set 'AppDbContext.OrgaoEmissor'  is null.");
          }
            _context.OrgaoEmissor.Add(orgaoEmissor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrgaoEmissor", new { id = orgaoEmissor.OrgaoEmissorId }, orgaoEmissor);
        }

        // DELETE: api/OrgaoEmissor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrgaoEmissor(int id)
        {
            if (_context.OrgaoEmissor == null)
            {
                return NotFound();
            }
            var orgaoEmissor = await _context.OrgaoEmissor.FindAsync(id);
            if (orgaoEmissor == null)
            {
                return NotFound();
            }

            _context.OrgaoEmissor.Remove(orgaoEmissor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrgaoEmissorExists(int id)
        {
            return (_context.OrgaoEmissor?.Any(e => e.OrgaoEmissorId == id)).GetValueOrDefault();
        }
    }
}
