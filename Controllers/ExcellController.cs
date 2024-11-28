using Microsoft.AspNetCore.Mvc;
using skill_up.Context;
using skill_up.Models;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace skill_up.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcellController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExcellController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Nenhum arquivo foi enviado.");

            try
            {
                using (var stream = file.OpenReadStream())
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Primeira aba do Excel

                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++) // Supõe que a primeira linha é o cabeçalho
                    {
                        // Obtenha o Id do funcionário e o CursoId do Excel
                        var nome = worksheet.Cells[row, 1].Text;
                        var funcionario = _context.AspNetUsers.FirstOrDefault(p => p.Nome == nome);
                        var cursoNome = worksheet.Cells[row, 2].Text;
                        var cursoId = _context.Cursos.FirstOrDefault(p => p.Nome == cursoNome);

                        var data = worksheet.Cells[row, 3].Text;

                        string formato = "yyyy-MM-dd";
                        DateOnly dataConvertida = DateOnly.ParseExact(data, formato, CultureInfo.InvariantCulture);
                        
                        DateOnly dataFinal = DateOnly.Parse(data);
                        Console.WriteLine("*************************" + data+ " ***** "+ dataConvertida+"********"+dataFinal);
                    
                        var funcionarioCurso = new FuncionarioCurso
                        {
                            FuncionarioId = funcionario.Id,
                            CursoId = cursoId.CursoId,
                            DataValidade = dataFinal,
                             // Assume que 'Nome' é a propriedade para o nome do curso
                        };

                        // Salve os dados no banco de dados
                        await SalvarDadosNoBanco(funcionarioCurso);
                    }
                }

                return Ok("Dados processados e salvos com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar o arquivo: {ex.Message}");
            }
        }

        private async Task SalvarDadosNoBanco(FuncionarioCurso funcionarioCurso)
        {
            _context.FuncionarioCursos.Add(funcionarioCurso);
            await _context.SaveChangesAsync();
        }
    }
}
