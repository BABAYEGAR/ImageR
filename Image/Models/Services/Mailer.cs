using System;
using System.Globalization;
using System.IO;
using Image.Models.Entities;
using MimeKit;

namespace Image.Models.Services
{
    public class Mailer
    {
        public void SendNewUserEmail(string path, AppUser appUser,Role role,AppUserAccessKey accessKey)
        {
            //From Address
            var FromAddress = "support@camerack.com";
            var FromAdressTitle = "Camerack Studio";
            //To Address
            var toVendor = appUser.Email;
            //var toCustomer = email;
            var ToAdressTitle = "Camerack Studio";
            var Subject = "Activate Account.";
            //var BodyContent = message;

            //Smtp Server
            var smtpServer = new AppConfig().EmailServer;
            //Smtp Port Number
            var smtpPortNumber = new AppConfig().Port;

            var mimeMessageVendor = new MimeMessage();
            mimeMessageVendor.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));
            mimeMessageVendor.To.Add(new MailboxAddress(ToAdressTitle, toVendor));
            mimeMessageVendor.Subject = Subject;
            BodyBuilder bodyBuilder = new BodyBuilder();
            using (var data = File.OpenText(path))
            {
                if (data.BaseStream != null)
                {
                    //manage content

                    bodyBuilder.HtmlBody = data.ReadToEnd();
                    var body = bodyBuilder.HtmlBody;

                    var replace = body.Replace("NAME", appUser.Name);
                    replace = replace.Replace("URL", "http://studio.camerack.com/Account/AccountActivationLink?accessCode=" + accessKey.AccountActivationAccessCode);
                    replace = replace.Replace("ROLE", role.Name);
                    replace = replace.Replace("DATE", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    bodyBuilder.HtmlBody = replace;
                    mimeMessageVendor.Body = bodyBuilder.ToMessageBody();
                }
            }
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(smtpServer, smtpPortNumber);
                // Note: only needed if the SMTP server requires authentication
                // Error 5.5.1 Authentication 
                client.Authenticate(new AppConfig().Email, new AppConfig().Password);
                client.Send(mimeMessageVendor);
                client.Disconnect(true);
            }
        }
        public void SendForgotPasswordResetLink(string path, AppUser appUser,AppUserAccessKey accessKey)
        {
            //From Address
            var FromAddress = "support@camerack.com";
            var FromAdressTitle = "Camerack Studio";
            //To Address
            var toVendor = appUser.Email;
            //var toCustomer = email;
            var ToAdressTitle = "Camerack Studio";
            var Subject = "Password Reset.";
            //var BodyContent = message;

            //Smtp Server
            var smtpServer = new AppConfig().EmailServer;
            //Smtp Port Number
            var smtpPortNumber = new AppConfig().Port;

            var mimeMessageVendor = new MimeMessage();
            mimeMessageVendor.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));
            mimeMessageVendor.To.Add(new MailboxAddress(ToAdressTitle, toVendor));
            mimeMessageVendor.Subject = Subject;
            BodyBuilder bodyBuilder = new BodyBuilder();
            using (StreamReader data = File.OpenText(path))
            {
                if (data.BaseStream != null)
                {
                    //manage content

                    bodyBuilder.HtmlBody = data.ReadToEnd();
                    var body = bodyBuilder.HtmlBody;

                    var replace = body.Replace("NAME", appUser.Name);
                    replace = replace.Replace("DATE", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    replace = replace.Replace("URL", "http://studio.camerack.com/Account/ForgotPassword?accessCode="+ accessKey.PasswordAccessCode);
                    bodyBuilder.HtmlBody = replace;
                    mimeMessageVendor.Body = bodyBuilder.ToMessageBody();
                }
            }
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(smtpServer, smtpPortNumber, true);
                // Note: only needed if the SMTP server requires authentication
                // Error 5.5.1 Authentication 
                client.Authenticate(new AppConfig().Email, new AppConfig().Password);
                client.Send(mimeMessageVendor);
                client.Disconnect(true);
            }
        }
        public void SendCompetitionEmail(string path, AppUser appUser, Competition competition)
        {
            //From Address
            var FromAddress = "support@camerack.com";
            var FromAdressTitle = "Camerack Studio";
            //To Address
            var toVendor = appUser.Email;
            //var toCustomer = email;
            var ToAdressTitle = "Camerack Studio";
            var Subject = "Competition.";
            //var BodyContent = message;

            //Smtp Server
            var smtpServer = new AppConfig().EmailServer;
            //Smtp Port Number
            var smtpPortNumber = new AppConfig().Port;

            var mimeMessageVendor = new MimeMessage();
            mimeMessageVendor.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));
            mimeMessageVendor.To.Add(new MailboxAddress(ToAdressTitle, toVendor));
            mimeMessageVendor.Subject = Subject;
            BodyBuilder bodyBuilder = new BodyBuilder();
            using (StreamReader data = File.OpenText(path))
            {
                if (data.BaseStream != null)
                {
                    //manage content

                    bodyBuilder.HtmlBody = data.ReadToEnd();
                    var body = bodyBuilder.HtmlBody;

                    var replace = body.Replace("NAME", appUser.Name);
                    replace = replace.Replace("TITLE", competition.Name);
                    replace = replace.Replace("THEME", competition.Theme);
                    replace = replace.Replace("PRICE", competition.Price);
                    replace = replace.Replace("DESCRIPTION", competition.Description);
                    replace = replace.Replace("STARTDATE",competition.StartDate.ToString(CultureInfo.InvariantCulture));
                    replace = replace.Replace("ENDDATE", competition.EndDate.ToString(CultureInfo.InvariantCulture));
                    replace = replace.Replace("URL", "http://studio.camerack.com/Account/Login");
                    bodyBuilder.HtmlBody = replace;
                    mimeMessageVendor.Body = bodyBuilder.ToMessageBody();
                }
            }
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(smtpServer, smtpPortNumber, true);
                // Note: only needed if the SMTP server requires authentication
                // Error 5.5.1 Authentication 
                client.Authenticate(new AppConfig().Email, new AppConfig().Password);
                client.Send(mimeMessageVendor);
                client.Disconnect(true);
            }
        }
    }
}