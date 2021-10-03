using System.Collections.Generic;

namespace SendEmail
{
  public class EmailConfiguration
  {
    public List<string> Attachments { get; set; }
    public string Body { get; set; }
    public PersonConfiguration From { get; set; }
    public bool? IsBodyHtml { get; set; }
    public SmtpServerConfiguration Smtp { get; set; }
    public string Subject { get; set; }
    public List<PersonConfiguration> To { get; set; }

    public EmailConfiguration()
    {
      this.From = new();
      this.To = new List<PersonConfiguration>();
      this.Attachments = new List<string>();
      this.Smtp = new();
    }
  }
}