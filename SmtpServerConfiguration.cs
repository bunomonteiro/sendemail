﻿namespace SendEmail
{
  public class SmtpServerConfiguration
  {
    public bool? EnableSsl { get; set; }
    public string Host { get; set; }
    public string Password { get; set; }
    public int? Port { get; set; }
    public bool? UseDefaultCredentials { get; set; }
    public string Username { get; set; }
  }
}