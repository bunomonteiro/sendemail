# sendemail
Send email by command line on **Windows**, **GNU Linux** or **MAC OS**

### Parameters

List of supported parameters

```
  -P, --port               Sets the port used for SMTP transactions.

  -H, --host               Sets the name or IP address of the host used for SMTP transactions.

  -S, --ssl                Specify whether the SMTP uses Secure Sockets Layer (SSL) to encrypt the connection.

  --default-credentials    Sets a boolean value that controls whether the DefaultCredentials are sent with requests

  -u, --username           Sets the user name associated with the credentials.

  -p, --password           Sets the password for the user name associated with the credentials.

  -f, --from.address       Sets the "from" address for this email message.

  --from.name              Sets the "from" display name for this email message.

  -t, --to.address         Sets the list of "to" address for this email message.

  --to.name                Sets the list of "To" display names for this email message.

  -s, --subject            Sets the subject line for this email message.

  -b, --body               Sets the message body.

  -h, --html               Sets a value indicating whether the mail message body is in HTML.

  -a, --attachments        It is a list of paths that defines the collection of attachments used to store data attached
                           to this email message.

  --help                   Display this help screen.

  --version                Display version information.
```

### Configuration

You can set default values in the ```./appsettings.json``` file

```json
{
  "Email": {
    "From": {
      "Address": "from@email.com",
      "Name": "From Name"
    },
    "To": [
      {
        "Address": "to_1@email.com"
      },
      {
        "Address": "to_2@email.com"
      }
    ],
    "Subject": "Subject",
    "Body": "Email body supports <strong>html</strong>",
    "IsBodyHtml": true,
    "Attachments": ["/usr/file_1.pdf", "c:/temp/file_2.pdf"],
    "Smtp": {
      "Port": 25,
      "Host": "localhost",
      "EnableSsl": false,
      "UseDefaultCredentials": false,
      "Username": "username",
      "Password": "password"
    }
  }
}

```

License
----

The **sendemail** source code is issued under [MIT license][MIT], a permissive free license, which means you can modify it as you please, and incorporate it into your own commercial or non-commercial software.

**Free Software, oh yeah!**

[MIT]: <https://raw.githubusercontent.com/bunomonteiro/sendemail/main/LICENSE>
