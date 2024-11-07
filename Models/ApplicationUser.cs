using Microsoft.AspNetCore.Identity;
namespace skill_up.Models;

public class ApplicationUser : IdentityUser
{
    public string? Nome {get;set;}
    public string? Cpf{get;set;}
    
    List<FuncionarioCurso>? FuncionarioCursos{get;set;}
}
