using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Infra.Contracts
{
    public interface IEmail
    {
        void Send(string urlSenderRmailService, MailAddress[] receivers, string subject, string body, MailAddress from = null, Attachment attachment = null, 
            bool? sendNow = false, int campaingSubtypeCode = 0, Dictionary<string, string> hashTagsReplacement = null);


        void Send(string urlSenderRmailService, MailAddress[] receivers, MailAddress[] blindReceivers, string subject, string body, MailAddress from = null,
            Attachment attachment = null, bool? sendNow = false, int campaingSubtypeCode = 0, Dictionary<string, string> hashTagsReplacement = null);
    }
}
