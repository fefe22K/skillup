using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace skill_up.Models;

public class FuncionarioCurso
{

    [Key]
    public int FuncCursoId { get; set; }
    public string? FuncionarioId { get; set; }
    public int CursoId { get; set; }
    public DateOnly DataValidade { get; set; }

    [JsonIgnore]
    public virtual ApplicationUser? Funcionario { get; set; }
    [JsonIgnore]
    public virtual Curso? Curso { get; set; }

   
}
