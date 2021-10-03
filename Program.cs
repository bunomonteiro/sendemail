using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

using CommandLine;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

namespace SendEmail
{
  public class Program
  {
    private static IConfigurationRoot Configuration;
    private static IServiceProvider ServiceProvider;

    protected Program()
    {
    }

    /// <example>
    /// args = new string[] { "--param_1", "value_1", "--param_2", "value_2", "..." };
    /// </example>
    public static void Main(string[] args)
    {
      Parser.Default.ParseArguments<Options>(args).WithParsed(email =>
      {
        try
        {
          Prepare();

          SendEmail(email);
        } catch(Exception ex)
        {
          Log.Error(ex.Message);
        }
      });
    }

    private static void Prepare()
    {
      Log.Logger = new LoggerConfiguration()
           .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
           .MinimumLevel.Debug()
           .Enrich.FromLogContext()
           .CreateLogger();

      ServiceCollection serviceCollection = new ServiceCollection();
      ConfigureServices(serviceCollection);
      ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    private static void SendEmail(EmailConfiguration options)
    {
#if DEBUG
      Log.Information("Attachments: " + string.Join(",", options.Attachments));
      Log.Information("Subject: " + string.Join(",", options.Subject));
      Log.Information("Body: " + string.Join(",", options.Body));
      Log.Information("IsBodyHtml: " + string.Join(",", options.IsBodyHtml));

      Log.Information("From.Address: " + string.Join(",", options.From.Address));
      Log.Information("From.Name: " + string.Join(",", options.From.Name));

      Log.Information("To.Address: " + string.Join(",", options.To.Select(t => t.Address)));
      Log.Information("To.Name: " + string.Join(",", options.To.Select(t => t.Name)));

      Log.Information("Smtp.Port: " + string.Join(",", options.Smtp.Port));
      Log.Information("Smtp.Host: " + string.Join(",", options.Smtp.Host));
      Log.Information("Smtp.Username: " + string.Join(",", options.Smtp.Username));
      Log.Information("Smtp.Password: " + string.Join(",", options.Smtp.Password));
      Log.Information("Smtp.EnableSsl: " + string.Join(",", options.Smtp.EnableSsl));
      Log.Information("Smtp.UseDefaultCredentials: " + string.Join(",", options.Smtp.UseDefaultCredentials));
#endif
      try
      {
        Log.Information("Preparing email...");

        MailMessage message = new();
        SmtpClient smtp = new();

        var configuration = Configuration.GetSection("Email").Get<EmailConfiguration>();

        foreach(var path in options.Attachments.Any() ? options.Attachments : configuration.Attachments)
        {
          if(!File.Exists(path))
          {
            continue;
          }

          message.Attachments.Add(new Attachment(path));
        }

        message.From = new MailAddress(options.From.Address ?? configuration.From.Address, options.From.Address ?? configuration.From.Name);
        foreach(var to in options.To.Any() ? options.To.Select(t => new MailAddress(t.Address, t.Name)) : configuration.To.Select(t => new MailAddress(t.Address, t.Name)))
        {
          message.To.Add(to);
        }
        message.Subject = options.Subject ?? configuration.Subject;
        message.Body = options.Body ?? configuration.Body;
        message.IsBodyHtml = options.IsBodyHtml.HasValue ? options.IsBodyHtml.Value : configuration.IsBodyHtml.Value;
        message.HeadersEncoding = Encoding.UTF8;
        message.SubjectEncoding = Encoding.UTF8;
        message.BodyEncoding = Encoding.UTF8;
        smtp.Port = options.Smtp.Port.HasValue ? options.Smtp.Port.Value : configuration.Smtp.Port.Value;
        smtp.Host = options.Smtp.Host ?? configuration.Smtp.Host;
        smtp.EnableSsl = options.Smtp.EnableSsl.HasValue ? options.Smtp.EnableSsl.Value : configuration.Smtp.EnableSsl.Value;
        smtp.UseDefaultCredentials = options.Smtp.UseDefaultCredentials.HasValue ? options.Smtp.UseDefaultCredentials.Value : configuration.Smtp.UseDefaultCredentials.Value;
        smtp.Credentials = new NetworkCredential(options.Smtp.Password ?? configuration.Smtp.Username, options.Smtp.Password ?? configuration.Smtp.Password);
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

        Log.Information("Sending email...");

        smtp.SendMailAsync(message).Wait();

        Log.Information("Email successfully sent!");
      } catch(Exception ex)
      {
        Log.Error(ex.Message);
      }
    }

    private static void ConfigureServices(IServiceCollection serviceCollection)
    {
      serviceCollection.AddLogging();

      // Build configuration
      Configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", true, true)
          .Build();

      // Add access to generic IConfigurationRoot
      serviceCollection.AddSingleton(Configuration);
      serviceCollection.AddTransient<EmailConfiguration>();
    }
  }
}