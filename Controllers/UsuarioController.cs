using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using skill_up.Context;
using skill_up.Models;

namespace skill_up.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public UsuarioController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration, 
        AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _context = context;
    }

     /// <summary>
    /// Retorna todos os funcionarios existente
    /// </summary>
    /// <remarks> </remarks>
    [HttpGet]
    public async Task<ActionResult <IEnumerable<ApplicationUser>>> Get()
    {
         if (_context.AspNetUsers == null)
        {
            return NotFound();
        }
        var funcionario = await _context.AspNetUsers.ToListAsync();

        if (funcionario == null)
        {
            return NotFound();
        }

        return funcionario;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApplicationUser>> GetUsuario(string id)
    {
        if (_context.AspNetUsers == null)
        {
            return NotFound();
        }
        var funcionario = await _context.AspNetUsers.FindAsync(id);

        if (funcionario == null)
        {
            return NotFound();
        }

        return funcionario;
    }

      /// <summary>
    /// Cadastrar um novo funcionário
    /// </summary>
    ///<remarks> </remarks>
    [HttpPost("Criar")]
    public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
    {
        {
            var user = new ApplicationUser
            {
                UserName = model.Cpf,
                Cpf = model.Cpf,
                Nome = model.Nome,
                Email = model.Email,
                PhoneNumber = model.Telefone,
                
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {   string tipo = model.Tipo == "A"? "Admin":"Basic";
                
                await _userManager.AddToRoleAsync(user, tipo);
                var roles = await _userManager.GetRolesAsync(user);
                return BuildToken(model, roles, user);
            }
            else
            {
                return BadRequest("Usuário ou senha inválidos");
            }
        }
    }

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
   /// Logar um usuário existente
   /// </summary>
   ///<remarks> </remarks>
    [HttpPost("Login")]
    public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo userInfo)
    {
        var result = await _signInManager.PasswordSignInAsync(userInfo.Cpf, userInfo.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByNameAsync(userInfo.Cpf);
            var roles = await _userManager.GetRolesAsync(user);
            return BuildToken(userInfo, roles, user);
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Login inválido.");
            return BadRequest(ModelState);
        }
    }
         

    private UserToken BuildToken(UserInfo userInfo, IList<string> userRoles, ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Cpf),
            new Claim("meuValor", "oque voce quiser"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddHours(1);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        return new UserToken()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration,
            Roles = userRoles,
            Id = user.Id
        };
    }
        private bool FuncionarioCursoExists(int id)
    {
        return (_context.FuncionarioCursos?.Any(e => e.FuncCursoId == id)).GetValueOrDefault();
    }

     [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(string id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var usuario = await _context.AspNetUsers.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
             try{
            _context.AspNetUsers.Remove(usuario);
            await _context.SaveChangesAsync();
             }catch(DbUpdateException ex){
                Console.WriteLine("***************************"+ex.Message);
                if(ex.InnerException.Message.Contains("FK_FuncionarioCursos_AspNetUsers_FuncionarioId"))
                {
                    return BadRequest("Funcionario ja tem cursos vinculados então não pode ser excluido");
                }

             }

            return NoContent();
        }


}


