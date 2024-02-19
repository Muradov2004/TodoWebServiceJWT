using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Mail;
using System.Net;
using TodoWebService.Data;

namespace TodoWebService.BackgroundServices;

public class EmailBackgroundServices : BackgroundService
{
    private readonly IServiceProvider _services;

    public EmailBackgroundServices(IServiceProvider services)
    {
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();


                    var tasks = dbContext.TodoItems.Where(t => t.DeadLine.Date == DateTime.Today.AddDays(1)).ToList();

                    foreach (var task in tasks)
                    {

                        string subject = "Task Deadline Reminder";
                        string message = $"The deadline for task '{task.Text}' is tomorrow. Please complete it on time.";

                        await SendEmailAsync(dbContext.Users.FirstOrDefault(u => u.Id == task.UserId)!.Email!, subject, message);
                    }
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        try
        {
            var client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,

                Credentials = new NetworkCredential(
                "ami65143@gmail.com",
                "aekuvdvvjfgoofpw")
            };

            var mailMessage = new MailMessage
            {
                Subject = subject,
                Body = message,
                From = new MailAddress("ami65143@gmail.com", "Mail")
            };
            mailMessage.To.Add(new MailAddress(email));

            client.Send(mailMessage);
            await Console.Out.WriteLineAsync("gonderildiiiiiiiiiii");


        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email: {ex.Message}");
        }
    }
}
