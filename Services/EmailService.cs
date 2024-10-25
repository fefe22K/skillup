using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using skill_up.Context;


public class EmailService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly AppDbContext _context;
    private readonly ILogger<EmailService> _logger;
    private readonly string _smtpServer = "smtp.gmail.com";
    private readonly int _smtpPort = 587;
    private readonly string _smtpUser = "heitormc18@gmail.com";
    private readonly string _smtpPass = "gvzo ndnn dasb ulob";  // Atualize com a senha de app gerada

    public EmailService(ILogger<EmailService> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();


                var result = context.AspNetUsers
                    .Join(context.FuncionarioCursos,
                          u => u.Id,
                          f => f.Id,
                          (u, f) => new { u, f }) // Join AspNetUsers and FuncionarioCursos
                    .Where(uf => uf.f.DataValidade <= DateTime.UtcNow.AddDays(15)) // Verifica se a data de validade é menor ou igual a 15 dias a partir de agora
                   
                    .Select(uf => new
                    {
                        Nome = uf.u.Nome,
                        CursoId = uf.f.CursoId,
                        DataValidade = uf.f.DataValidade,
                        Email = uf.u.Email
                    })
                    .ToListAsync(stoppingToken); // Usando ToListAsync para evitar bloqueios

                   

        
                foreach (var mail in  await result)
                {
                      string mensagem = $"{mail.Nome} seu curso {mail.CursoId} vence no dia {mail.DataValidade:dd/MM/yyyy}.";
                    Console.Write("************"+mail.Email+"***************");
                    await SendEmailAsync(mail.Email, "Curso prestes a vencer", mensagem);
                    _logger.LogInformation("Email enviado com sucesso para {Email} às {time}", mail.Email, DateTimeOffset.Now);
                }
            }
            // Aguarda 10 minutos (600.000 milissegundos) antes de executar novamente 
            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
        }
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            var smtpClient = new SmtpClient(_smtpServer)
            {
                Port = _smtpPort,
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUser),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);
            await smtpClient.SendMailAsync(mailMessage);
        }

        catch (Exception ex)
        {
            _logger.LogError($"Erro ao enviar email: {ex.Message}");
        }

    }

}