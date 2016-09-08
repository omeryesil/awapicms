using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Net.Mail;

using AWAPI_Data.Data;
using AWAPI_Common.library;

namespace AWAPI_BusinessLibrary.library
{
    public class EmailTemplateLib
    {
        EmailTemplateContextDataContext _context = new EmailTemplateContextDataContext();


        /// <summary>
        /// Returns poll based on the culturecode
        /// </summary>
        /// <param name="pollId"></param>
        /// <param name="cultureCode"></param>
        /// <returns></returns>
        public AWAPI_Data.Data.awEmailTemplate Get(long? emailTemplateId)
        {
            if (emailTemplateId == null || emailTemplateId <= 0)
                return null;

            var list = from l in _context.awEmailTemplates
                       where l.emailTemplateId.Equals(emailTemplateId)
                       select l;

            return list.FirstOrDefault<awEmailTemplate>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<awEmailTemplate> GetList(long siteId)
        {
            if (siteId <= 0)
                return null;

            var list = from l in _context.awEmailTemplates
                       where l.siteId.Equals(siteId)
                       orderby l.title
                       select l;

            if (list == null)
                return null;

            return list.ToList<awEmailTemplate>();
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailTemplateId"></param>
        public void Delete(long emailTemplateId)
        {
            if (emailTemplateId <= 0)
                return;

            var list = from l in _context.awEmailTemplates
                        where l.emailTemplateId.Equals(emailTemplateId)
                        select l;
            _context.awEmailTemplates.DeleteAllOnSubmit(list);
            _context.SubmitChanges();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="emailFrom"></param>
        /// <param name="emailSubject"></param>
        /// <param name="emailBody"></param>
        /// <returns></returns>
        public long Add(long siteId, long userId,
                        string title, string description, string emailFrom, string emailSubject, string emailBody)
        {
            long id = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            awEmailTemplate template = new awEmailTemplate();

            template.emailTemplateId = id;
            template.siteId = siteId;
            template.title = title;
            template.description = description;
            template.emailFrom = emailFrom;
            template.emailSubject = emailSubject;
            template.emailBody = emailBody;

            template.userId = userId;
            template.lastBuildDate = DateTime.Now;
            template.createDate = DateTime.Now;

            _context.awEmailTemplates.InsertOnSubmit(template);
            _context.SubmitChanges();

            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailTemplateId"></param>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="emailFrom"></param>
        /// <param name="emailSubject"></param>
        /// <param name="emailBody"></param>
        /// <returns></returns>
        public bool Update(long emailTemplateId, long userId,
                        string title, string description, string emailFrom, string emailSubject, string emailBody)
        {
            awEmailTemplate template = _context.awEmailTemplates.
                                    FirstOrDefault(st => st.emailTemplateId.Equals(emailTemplateId));

            if (template == null)
                return false;

            template.title = title;
            template.description = description;
            template.emailFrom = emailFrom;
            template.emailSubject = emailSubject;
            template.emailBody = emailBody;

            template.userId = userId;
            template.lastBuildDate = DateTime.Now;
            _context.SubmitChanges();

            return true;
        }


        /// <summary>
        /// args: 
        /// example: 
        /// Hello {Name}, we contacted you on {Date}, etc....
        /// 
        /// {Name} and {Date} can be replaced by the args. 
        /// We set these values from by args values. Value and the parameter name 
        /// are seperated with pipe '|'
        /// Name|Omer Yesil, date|02/03/2005
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="templatePath">example: /en/mail/contestregister.html</param>
        /// <param name="args">commenter|This is a test</param>
        public bool Send(long emailTemplateId, string to, params string[] args)
        {
            awEmailTemplate t = Get(emailTemplateId);
            if (t == null || 
                String.IsNullOrEmpty(t.emailFrom) || 
                String.IsNullOrEmpty(t.emailSubject) || 
                String.IsNullOrEmpty(t.emailBody))
                return false;

            string subject = t.emailSubject;
            string body = t.emailBody;

            if (args != null && args.Length > 0)
            {
                foreach (string arg in args)
                {
                    if (String.IsNullOrEmpty(arg))
                        continue;

                    string[] parts = arg.Split('|');
                    string valueName = "";
                    string value = "";
                    if (parts.Length > 0)
                    {
                        valueName = parts[0];
                        if (parts.Length > 1)
                            value = parts[1];

                        subject = Regex.Replace(subject, "{" + valueName + "}", value, RegexOptions.IgnoreCase);
                        body = Regex.Replace(body, "{" + valueName + "}", value, RegexOptions.IgnoreCase);
                    }
                }
            }

            SmtpClient client = new SmtpClient(ConfigurationLibrary.Config.smtpServer);
            MailMessage mm = new MailMessage(t.emailFrom, to);

            mm.IsBodyHtml = true;
            mm.Subject = subject;
            mm.SubjectEncoding = System.Text.Encoding.UTF8;
            mm.Body = body;
            mm.BodyEncoding = System.Text.Encoding.UTF8;

            try
            {
                client.ServicePoint.MaxIdleTime=1;
                client.Send(mm);
            }
            catch (Exception ex)
            {
                string log = System.Web.HttpContext.Current.Server.MapPath("~/log.txt");
                System.IO.TextWriter tw = new System.IO.StreamWriter(log);
                tw.WriteLine("MAIL ERROR ---------------------");
                tw.WriteLine("SMTP:" + ConfigurationLibrary.Config.smtpServer);
                tw.WriteLine("ERROR:" + ex.Message);
                if (ex.InnerException != null)
                    tw.WriteLine("DETAIL:" + ex.InnerException.Message);
                tw.Close();


                throw ex;
            }
            finally
            {
                mm.Dispose();
            }

            return true;
        }

    }
}
