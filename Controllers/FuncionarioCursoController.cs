using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using skill_up.Context;
using skill_up.Models;

namespace skill_up.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionarioCursoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FuncionarioCursoController(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Retorna um curso de um funcionario
        /// </summary>
        /// <returns></returns>
        // GET: api/FuncionarioCurso
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FuncionarioCurso>>> GetFuncionarioCursos()
        {
          if (_context.FuncionarioCursos == null)
          {
              return NotFound();
          }
            return await _context.FuncionarioCursos.ToListAsync();
        }

        /// <summary>
        /// Retorna um curso de um funcionario especifico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/FuncionarioCurso/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FuncionarioCurso>> GetFuncionarioCurso(int id)
        {
          if (_context.FuncionarioCursos == null)
          {
              return NotFound();
          }
            var funcionarioCurso = await _context.FuncionarioCursos.FindAsync(id);

            if (funcionarioCurso == null)
            {
                return NotFound();
            }

            return funcionarioCurso;
        }
           
           /// <summary>
           /// Edita o curso do funcionario
           /// </summary>
           /// <param name="id"></param>
           /// <param name="funcionarioCurso"></param>
           /// <returns></returns>
        // PUT: api/FuncionarioCurso/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFuncionarioCurso(int id, FuncionarioCurso funcionarioCurso)
        {
            if (id != funcionarioCurso.FuncCursoId)
            {
                return BadRequest();
            }

            _context.Entry(funcionarioCurso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FuncionarioCursoExists(id))
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

        /// <summary>
        /// Cadastra um curso novo de um funcionario
        /// </summary>
        /// <param name="funcionarioCurso"></param>
        /// <returns></returns>
        // POST: api/FuncionarioCurso
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FuncionarioCurso>> PostFuncionarioCurso(FuncionarioCurso funcionarioCurso)
        {
          if (_context.FuncionarioCursos == null)
          {
              return Problem("Entity set 'AppDbContext.FuncionarioCursos'  is null.");
          }
            _context.FuncionarioCursos.Add(funcionarioCurso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFuncionarioCurso", new { id = funcionarioCurso.FuncCursoId }, funcionarioCurso);
        }

        /// <summary>
        /// Deleta um curso de um funcionario desejado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/FuncionarioCurso/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuncionarioCurso(int id)
        {
            if (_context.FuncionarioCursos == null)
            {
                return NotFound();
            }
            var funcionarioCurso = await _context.FuncionarioCursos.FindAsync(id);
            if (funcionarioCurso == null)
            {
                return NotFound();
            }

            _context.FuncionarioCursos.Remove(funcionarioCurso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FuncionarioCursoExists(int id)
        {
            return (_context.FuncionarioCursos?.Any(e => e.FuncCursoId == id)).GetValueOrDefault();
        }
    }
}
