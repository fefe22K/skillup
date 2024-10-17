using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using skill_up.Controllers;
using skill_up.Models;



namespace skill_up.Context;
 
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions options): base(options){}
    public DbSet<Curso> Cursos{get;set;}
    public DbSet<FuncionarioCurso> FuncionarioCursos{get;set;}
}