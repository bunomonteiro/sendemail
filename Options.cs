using System.Collections.Generic;
using System.Linq;

using CommandLine;

namespace SendEmail
{
  public class Options
  {
    #region Smtp
    [Option('P', "port", Required = false, HelpText = "Sets the port used for SMTP transactions.")]
    public int? Port { get; set; }
    [Option('H', "host", Required = false, HelpText = "Sets the name or IP address of the host used for SMTP transactions.")]
    public string Host { get; set; }
    [Option('S', "ssl", Required = false, HelpText = "Specify whether the SMTP uses Secure Sockets Layer (SSL) to encrypt the connection.")]
    public bool? EnableSsl { get; set; }
    [Option("default-credentials", Required = false, HelpText = "Sets a boolean value that controls whether the DefaultCredentials are sent with requests")]
    public bool? UseDefaultCredentials { get; set; }
    [Option('u', "username", Required = false, HelpText = "Sets the user name associated with the credentials.")]
    public string Username { get; set; }
    [Option('p', "password", Required = false, HelpText = "Sets the password for the user name associated with the credentials.")]
    public string Password { get; set; }
    #endregion

    #region From
    [Option('f', "from.address", Required = false, HelpText = "Sets the \"from\" address for this email message.")]
    public string FromAddress { get; set; }

    [Option("from.name", Required = false, HelpText = "Sets the \"from\" display name for this email message.")]
    public string FromName { get; set; }
    #endregion

    #region To
    [Option('t', "to.address", Required = false, HelpText = "Sets the list of \"to\" address for this email message.")]
    public IEnumerable<string> ToAddress { get; set; }

    [Option("to.name", Required = false, HelpText = "Sets the list of \"To\" display names for this email message.")]
    public IEnumerable<string> ToName { get; set; }
    #endregion

    [Option('s', "subject", Required = false, HelpText = "Sets the subject line for this email message.")]
    public string Subject { get; set; }

    [Option('b', "body", Required = false, HelpText = "Sets the message body.")]
    public string Body { get; set; }

    [Option('h', "html", Required = false, HelpText = "Sets a value indicating whether the mail message body is in HTML.")]
    public bool? IsBodyHtml { get; set; }

    [Option('a', "attachments", Required = false, HelpText = "It is a list of paths that defines the collection of attachments used to store data attached to this email message.")]
    public IEnumerable<string> Attachments { get; set; }


    public Options()
    {
      this.ToAddress = new List<string>();
      this.ToName = new List<string>();
      this.Attachments = new List<string>();
    }

    public static implicit operator EmailConfiguration(Options options)
    {
      var configuration = new EmailConfiguration
      {
        Smtp = new SmtpServerConfiguration
        {
          Host = options.Host,
          Port = options.Port,
          Username = options.Username,
          Password = options.Password,
          EnableSsl = options.EnableSsl,
          UseDefaultCredentials = options.UseDefaultCredentials
        },
        From = new PersonConfiguration
        {
          Address = options.FromAddress,
          Name = options.FromName
        },
        Subject = options.Subject,
        Body = options.Body,
        IsBodyHtml = options.IsBodyHtml,
        Attachments = options.Attachments?.ToList()
      };

      if(options.ToAddress.Any())
      {
        for(int i = 0; i < options.ToAddress.Count(); i++)
        {
          configuration.To.Add(new PersonConfiguration
          {
            Address = options.ToAddress.ElementAt(i),
            Name = options.ToName.Any() && options.ToName.Count() > i ? options.ToName.ElementAt(i) : null
          });
        }
      }

      return configuration;
    }

    public static implicit operator Options(EmailConfiguration configuration)
    {
      return new Options
      {
        Host = configuration.Smtp.Host,
        Port = configuration.Smtp.Port,
        EnableSsl = configuration.Smtp.EnableSsl,
        UseDefaultCredentials = configuration.Smtp.UseDefaultCredentials,
        Username = configuration.Smtp.Username,
        Password = configuration.Smtp.Password,

        FromAddress = configuration.From.Address,
        FromName = configuration.From.Name,

        ToAddress = configuration.To.Select(t => t.Address).ToList(),
        ToName = configuration.To.Select(t => t.Name).ToList(),

        Subject = configuration.Subject,
        Body = configuration.Body,
        IsBodyHtml = configuration.IsBodyHtml,
        Attachments = configuration.Attachments
      };
    }
  }
}
