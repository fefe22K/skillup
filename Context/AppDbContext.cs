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

    public DbSet<ApplicationUser> AspNetUsers{get;set;}

    public DbSet<skill_up.Models.OrgaoEmissor> OrgaoEmissor { get; set; } = default!;
    public object Treinamentos { get; internal set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<FuncionarioCurso>()
        .HasOne(fc => fc.Funcionario)
        .WithMany(f => f.FuncionarioCursos)
        .HasForeignKey(fc => fc.FuncionarioId);
}

}