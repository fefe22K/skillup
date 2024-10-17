using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using skill_up.Models;


namespace skill_up.Models;
 
public class Curso
{
    [Key]
    public int CursoId{get;set;} 
    public string? Nome{get;set;}
    [MaxLength(14)]
    public string? Descricao{get;set;}
    public int OrgaoEmissorId{get;set;}
    [JsonIgnore]
    public virtual OrgaoEmissor? OrgaoEmissor {get;set;}
}

