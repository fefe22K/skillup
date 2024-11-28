using Microsoft.EntityFrameworkCore;

namespace skill_up.Models;

public class EmailNotificacao
{
    public string? Nome {get;set;}
        public int CursoId {get;set;} 
        public DateOnly DataValidade {get;set;}
        public string? Email {get;set;} 
}
