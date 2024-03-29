﻿using Microsoft.Extensions.Options;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Services.Concrete
{
    public class MailManager : IMailService
    {
        private readonly SmtpSettings _smtpSettings;
        public MailManager(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;   
        }
        public IResult Send(EmailSendDto emailSendDto)
        {
            MailMessage message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail),
                To = { new MailAddress(emailSendDto.Email) },
                Subject = emailSendDto.Subject,
                IsBodyHtml = true,
                Body = emailSendDto.Message
            };
            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Server,
                Port = _smtpSettings.Port,
                EnableSsl = true,//microsoft guvenlik
                UseDefaultCredentials = false,//kendi eposta bilgilerinin verilmesi
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            smtpClient.Send(message);

            return new Result(ResultStatus.Success, $"E-postanız başarıyla gönderilmiştir. ");
        }

        public IResult SendContactEmail(EmailSendDto emailSendDto)
        {
            MailMessage message = new MailMessage
            {
                From=new MailAddress(_smtpSettings.SenderEmail),
                To = {new MailAddress("de.murat7@gmail.com")},
                Subject=emailSendDto.Subject,
                IsBodyHtml=true,
                Body=$"Gönderen Kişi: {emailSendDto.Name}, Gönderen E-posta: {emailSendDto.Email}<br/>{emailSendDto.Message} "
            };
            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Server,
                Port =_smtpSettings.Port,
                EnableSsl=true,//microsoft guvenlik
                UseDefaultCredentials=false,//kendi eposta bilgilerinin verilmesi
                Credentials=new NetworkCredential(_smtpSettings.Username,_smtpSettings.Password),
                DeliveryMethod=SmtpDeliveryMethod.Network
            };
            smtpClient.Send(message);

            return new Result(ResultStatus.Success, $"E-postanız başarıyla gönderilmiştir. ");
        }
    }
}
