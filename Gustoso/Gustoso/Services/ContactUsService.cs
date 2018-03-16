using Gustoso.Common.DTO.Communication;
using Gustoso.Common.Interfaces;
using Gustoso.Common.IServices;
using Gustoso.Common.Models;
using Gustoso.Context;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Gustoso.Services
{
    public class ContactUsService: IContactUsService
    {
        private readonly MSContext _db;
        private readonly string _email;
        private readonly string _password;
        private readonly string _host;
        private static object _smtpLocker = new object();


        public ContactUsService(IConfigurationRoot root, MSContext context)
        {
            _db = context;
            var config = root.GetSection("SmtpConfig");
            _email = config["email"];
            _password = config["password"];
            _host = config["host"];
        }

        public async Task<Response<string>> addMessage(IContactUsDTO obj)
        {
            var response = new Response<string>();
            var objToDB = new ContactUs()
            {
                clientName = obj.clientName,
                clientEmail = obj.clientEmail,
                clientSubject = obj.clientSubject,
                clientMessage = obj.clientMessage
            };
            await _db.ContactUs.AddAsync(objToDB);
            await _db.SaveChangesAsync();
            await sendToEmail(obj);
            response.Data = "Message is success saved!";
            return response;
        }

        public async Task sendToEmail(IContactUsDTO obj)
        {
            string emailBody = "";
            using (StreamReader sr = new StreamReader("EmailBodyHTML.txt"))
            {
                emailBody = await sr.ReadToEndAsync();
                emailBody = emailBody.Replace("clientName", obj.clientName);
                emailBody = emailBody.Replace("clientEmail", obj.clientEmail);
                emailBody = emailBody.Replace("clientSubject", obj.clientSubject);
                emailBody = emailBody.Replace("clientMessage", obj.clientMessage);
            }
            using (MailMessage Message = new MailMessage()
            {
                Subject = $"{obj.clientName} звертається до вас з сервісу Gustoso Backery",
                Body = emailBody,
                BodyEncoding = Encoding.GetEncoding("utf-8"),
                From = new MailAddress(_email),
                IsBodyHtml = true,
            })
            {
                Message.To.Add(new MailAddress(obj.clientEmail));
                lock (_smtpLocker)
                {
                    using (SmtpClient Smtp = new SmtpClient()
                    {
                        Host = _host,
                        Credentials = new System.Net.NetworkCredential(_email, _password),
                        EnableSsl = true,
                        Timeout = 20_000,
                    })
                    {
                        Smtp.SendCompleted += SendCompleted;
                        Smtp.Send(Message);
                        Smtp.Dispose();
                    }
                }
            }
        }

        private static void SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            var responseSend = e;
        }
    }
}
