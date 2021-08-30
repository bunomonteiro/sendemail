namespace SendEmail
{
  public class SmtpServerConfiguration
  {
    public int? Port { get; set; }
    public string Host { get; set; }
    public bool? EnableSsl { get; set; }
    public bool? UseDefaultCredentials { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
  }
}
