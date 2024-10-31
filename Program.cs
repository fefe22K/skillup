using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using skill_up.Context;
using skill_up.Models;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHostedService<EmailService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Cadastros de cursos e funcionários ",
        Description = "Uma Api para cadastro de cursos e funcionários wn ASP.net",
        TermsOfService = new Uri("https://example.com/license"),
        Contact = new OpenApiContact
        {
            Name = "Suporte",
            Url = new Uri("https://example.com/contact")
        },
         License = new OpenApiLicense
        {
            Name = "Licença de uso",
            Url = new Uri("https://example.com/license")
        }
    });
    var xmlNome = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,xmlNome));
});
 

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("azure")));


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = false,
         ValidateAudience = false,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"])),
         ClockSkew = TimeSpan.Zero
     };
 }
);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

builder.Services.AddCors(options => { options.AddPolicy(MyAllowSpecificOrigins, policy => { policy.WithOrigins("*") .AllowAnyHeader() .AllowAnyMethod(); }); }); 








var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseDefaultFiles(); 
app.UseStaticFiles(); 
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
