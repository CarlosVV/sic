namespace Nagnoi.SiC.Application.Core {
    
    #region References

    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Domain.Core.Model;
    using Domain.Core.Repositories;
    using Domain.Core.Services;
    using Infrastructure.Core.Caching;
    using Infrastructure.Core.Dependencies;
    using Infrastructure.Core.Helpers;            
    using Infrastructure.Core.Log;
    using Infrastructure.Core.Validations;    
    using System.Net.Mail;
    using System.Net;
    using System;

    #endregion

    public class EmailSenderService: IEmailSenderService {

        #region Private Members

        private readonly ILogger logger = null;
        private readonly ISettingService settingService = null;
        private readonly IMessageTemplateService messageTemplateService = null;
        #endregion

        #region Constructors

        public EmailSenderService() : this(IoC.Resolve<ILogger>(), IoC.Resolve<ISettingService>(), IoC.Resolve<IMessageTemplateService>()) { }

        public EmailSenderService(ILogger logger, ISettingService settingService, IMessageTemplateService messageTemplateService) {
            this.logger = logger;
            this.settingService = settingService;
            this.messageTemplateService = messageTemplateService;
        }

        #endregion

        #region Public Methods

        public void SendEmail(string subject, string body, System.Net.Mail.MailAddress from, IEnumerable<System.Net.Mail.MailAddress> to, IEnumerable<System.Net.Mail.AlternateView> embeddedResources, IEnumerable<string> bcc, IEnumerable<string> cc, IEnumerable<System.Net.Mail.Attachment> attachments) {
            MailMessage message = new MailMessage();

            message.From = from;

            if (to != null) {
                foreach (MailAddress address in to) {
                    if (address != null) {
                        message.To.Add(address);
                    }
                }
            }

            if (embeddedResources != null) {
                foreach (AlternateView view in embeddedResources) {
                    if (view != null) {
                        message.AlternateViews.Add(view);
                    }
                }
            }

            if (bcc != null) {
                foreach (string address in bcc) {
                    if (!string.IsNullOrWhiteSpace(address)) {
                        message.Bcc.Add(address.Trim());
                    }
                }
            }

            if (cc != null) {
                foreach (string address in cc) {
                    if (!string.IsNullOrWhiteSpace(address)) {
                        message.CC.Add(address.Trim());
                    }
                }
            }

            if (attachments != null) {
                foreach (Attachment attachment in attachments) {
                    message.Attachments.Add(attachment);
                }
            }

            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient();            

            smtpClient.UseDefaultCredentials = this.settingService.GetSettingValueBoolean("EmailService.UseDefaultCredentials");
            smtpClient.Host = this.settingService.GetSettingValueByName("EmailService.Host");
            smtpClient.Port = this.settingService.GetSettingValueInteger("EmailService.Port");
            smtpClient.EnableSsl = this.settingService.GetSettingValueBoolean("EmailService.UseSSL");

            if (this.settingService.GetSettingValueBoolean("EmailService.UseDefaultCredentials")) {
                smtpClient.Credentials = CredentialCache.DefaultNetworkCredentials;
            } else {
                smtpClient.Credentials = new NetworkCredential(this.settingService.GetSettingValueByName("EmailService.UserName"), this.settingService.GetSettingValueByName("EmailService.Password"));
            }

            try {
                smtpClient.Send(message);
            } catch (SmtpException ex) {
                this.logger.Fatal(ex.Message, ex);
            } catch (Exception ex) {
                this.logger.Fatal(ex.Message, ex);
            }
        }

        #endregion
    }
}
