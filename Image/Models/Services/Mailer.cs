using System;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using Image.Models.Entities;
using MimeKit;
using MailKit.Net.Smtp;

namespace Image.Models.Services
{
    public class Mailer
    {
        public void SendNewUserEmail(string path, AppUser appUser,Role role,AppUserAccessKey accessKey)
        {
            //From Address
            var FromAddress = "support@photostudio.com";
            var FromAdressTitle = "Welcome To PS";
            //To Address
            var toVendor = appUser.Email;
            //var toCustomer = email;
            var ToAdressTitle = "PS-LOGIN";
            var Subject = "PS-LOGIN CRED.";
            //var BodyContent = message;

            //Smtp Server
            var SmtpServer = "smtp.gmail.com";
            //Smtp Port Number
            var SmtpPortNumber = 587;

            var mimeMessageVendor = new MimeMessage();
            mimeMessageVendor.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));
            mimeMessageVendor.To.Add(new MailboxAddress(ToAdressTitle, toVendor));
            //mimeMessage.To.Add(new MailboxAddress(ToAdressTitle, toCustomer));
            mimeMessageVendor.Subject = Subject;
            BodyBuilder bodyBuilder = new BodyBuilder();
            using (StreamReader data = System.IO.File.OpenText(path))
            {
                if (data.BaseStream != null)
                {
                    //manage content

                    bodyBuilder.HtmlBody = data.ReadToEnd();
                    var body = bodyBuilder.HtmlBody;

                    var replace = body.Replace("NAME", appUser.Name);
                    replace = replace.Replace("CODE", accessKey.AccountActivationAccessCode);
                    replace = replace.Replace("ROLE", role.Name);
                    replace = replace.Replace("DATE", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    bodyBuilder.HtmlBody = replace;
                    mimeMessageVendor.Body = bodyBuilder.ToMessageBody();
                }
            }
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(SmtpServer, SmtpPortNumber, false);
                // Note: only needed if the SMTP server requires authentication
                // Error 5.5.1 Authentication 
                client.Authenticate("salxsaa@gmail.com", "Brigada95");
                client.Send(mimeMessageVendor);
                client.Disconnect(true);
            }
        }
        public void SendForgotPasswordResetLink(string path, AppUser appUser,AppUserAccessKey accessKey)
        {
            //From Address
            var FromAddress = "support@photostudio.com";
            var FromAdressTitle = "Welcome To PS";
            //To Address
            var toVendor = appUser.Email;
            //var toCustomer = email;
            var ToAdressTitle = "PS-LOGIN";
            var Subject = "PS-Reset Password.";
            //var BodyContent = message;

            //Smtp Server
            var SmtpServer = "smtp.gmail.com";
            //Smtp Port Number
            var SmtpPortNumber = 587;

            var mimeMessageVendor = new MimeMessage();
            mimeMessageVendor.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));
            mimeMessageVendor.To.Add(new MailboxAddress(ToAdressTitle, toVendor));
            //mimeMessage.To.Add(new MailboxAddress(ToAdressTitle, toCustomer));
            mimeMessageVendor.Subject = Subject;
            BodyBuilder bodyBuilder = new BodyBuilder();
            using (StreamReader data = System.IO.File.OpenText(path))
            {
                if (data.BaseStream != null)
                {
                    //manage content

                    bodyBuilder.HtmlBody = data.ReadToEnd();
                    var body = bodyBuilder.HtmlBody;

                    var replace = body.Replace("NAME", appUser.Name);
                    replace = replace.Replace("CODE", accessKey.AccountActivationAccessCode);
                    replace = replace.Replace("DATE", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    bodyBuilder.HtmlBody = replace;
                    mimeMessageVendor.Body = bodyBuilder.ToMessageBody();
                }
            }
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(SmtpServer, SmtpPortNumber, false);
                // Note: only needed if the SMTP server requires authentication
                // Error 5.5.1 Authentication 
                client.Authenticate("salxsaa@gmail.com", "Brigada95");
                client.Send(mimeMessageVendor);
                client.Disconnect(true);
            }
        }
    }
}