using CRA.Data;
using CRA.Data.Entities;
using CRA.Tasks.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Tasks.Tasks
{
    public class ProcessQueuedEmailTask : TaskBase
    {
        DataContext _dataContext;

        int _emailQueueBatchSize;

        SmtpOptions _smtpOptions;
        public ProcessQueuedEmailTask(int emailQueueBatchSize, SmtpOptions smtpOptions)
        {
            _emailQueueBatchSize = emailQueueBatchSize;
            _smtpOptions = smtpOptions;
            _dataContext = new DataContext();
        }

        public override void Run()
        {
            List<Email> emails = null;

            var emailStatusTypes = _dataContext.EmailStatusTypes.Where(p => p.Deleted == false).ToList();

            long createdStatusTypeId = emailStatusTypes.Where(p => p.Name == "Created").Single().EmailStatusTypeId;
            long processingStatusTypeId = emailStatusTypes.Where(p => p.Name == "Processing").Single().EmailStatusTypeId;
            long completedStatusTypeId = emailStatusTypes.Where(p => p.Name == "Completed").Single().EmailStatusTypeId;
            long errorStatusTypeId = emailStatusTypes.Where(p => p.Name == "Error").Single().EmailStatusTypeId;


            using (var transaction = _dataContext.Database.BeginTransaction(IsolationLevel.Serializable))
            {
                emails = _dataContext.Emails.Where(p => p.EmailStatusTypeId == createdStatusTypeId && p.Deleted == false).Take(_emailQueueBatchSize).ToList();
                foreach (var email in emails)
                {
                    email.EmailStatusTypeId = processingStatusTypeId;
                }

                _dataContext.SaveChanges();
                transaction.Commit();
            }

            //At this point, send the email and set the status to completed or error based on the processing result.
            var smtp = new SmtpClient
            {
                Host = _smtpOptions.HostName,
                Port = _smtpOptions.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            foreach (var email in emails)
            {

                using (var mailMessage = new MailMessage())
                {
                    try
                    {
                        mailMessage.From = new MailAddress(email.FromAddress, email.FromName);
                        mailMessage.Subject = email.Subject;
                        mailMessage.Body = email.Body;
                        mailMessage.IsBodyHtml = email.IsHtml;

                        foreach (var address in _dataContext.EmailTos.Where(p => p.EmailId == email.EmailId && p.Deleted == false).ToList())
                        {
                            mailMessage.To.Add(new MailAddress(address.Address, address.Name));
                        }

                        foreach (var address in _dataContext.EmailCcs.Where(p => p.EmailId == email.EmailId && p.Deleted == false).ToList())
                        {
                            mailMessage.CC.Add(new MailAddress(address.Address, address.Name));
                        }

                        foreach (var address in _dataContext.EmailBccs.Where(p => p.EmailId == email.EmailId && p.Deleted == false).ToList())
                        {
                            mailMessage.Bcc.Add(new MailAddress(address.Address, address.Name));
                        }

                        foreach (var address in _dataContext.EmailReplyTos.Where(p => p.EmailId == email.EmailId && p.Deleted == false).ToList())
                        {
                            mailMessage.ReplyToList.Add(new MailAddress(address.Address, address.Name));
                        }

                        foreach (var attachment in _dataContext.EmailAttachments.Where(p => p.EmailId == email.EmailId && p.Deleted == false).ToList())
                        {
                            using (var content = new MemoryStream(attachment.Content))
                            {
                                content.Position = 0;
                                var emailAttachment = new Attachment(content, attachment.Name);
                                mailMessage.Attachments.Add(emailAttachment);
                            }

                        }

                        smtp.Send(mailMessage);
                        email.EmailStatusTypeId = completedStatusTypeId;

                    }

                    catch (Exception exception)
                    {
                        email.ErrorMessage = exception.ToString();
                        email.EmailStatusTypeId = errorStatusTypeId;
                    }

                }

                _dataContext.SaveChanges();

            }

        }
    }
}
