using System;
using System.Collections.Generic;
using System.Web;
using System.Net.Mail;
using System.Data;
using NPoco;
using Newtonsoft.Json;
using System.Linq;

/// <summary>
/// Summary description for mail
/// </summary>
public class mail
{
    public Boolean SendMail(List<String> toMailId, string mailSubject, string bodyText, string fileAttachment = "", List<String> ccMailId = null)
    {
        try
        {
            MailMessage mail = new MailMessage();
            List<Class_Settings> settingData = null;
            CrudSettings st = new CrudSettings();

            using (Database db = new Database("connString"))
            {
                settingData = JsonConvert.DeserializeObject<List<Class_Settings>>(st.SelectSettings()).ToList();
            }

            // if no mail data then use qsq's mail account
            if (settingData == null)
            {
                 settingData[0].from_mailid = "noreply@nngi.co.in";
 settingData[0].from_mailpwd = "krjc fqwy fltf xacb";
 //settingData[0].from_mailid = "test@qsqspl.com";
// settingData[0].from_mailpwd = "2Q4h%%SL";
 settingData[0].smtp_address = "smtp.gmail.com";
 settingData[0].smtp_port = 587;
 settingData[0].enable_adsl = "Y";
            }

            //TODO: UNCOMMENT WHEN GOING LIVE
            // add all the to mail ids
            foreach (string toId in toMailId)
            {
                mail.To.Add(toId);
            }

            //// add all the cc mail ids
            if (ccMailId != null)
            {
                foreach (string ccId in ccMailId)
                {
                    mail.CC.Add(ccId);
                }
            }

            // added for testing purpose
            // mail.CC.Add("ss_balki@qsqspl.com");

            // attach the file here

            //TODO: COMMENT WHEN GOING LIVE
            // mail.To.Add("ss_balki@nngi.in");
            // mail.CC.Add("ss.balki@gmail.com");
            mail.To.Clear();
            mail.CC.Clear();
            mail.To.Add("lija.shine@nngi.in");
            // rest of the mail part
            mail.From = new MailAddress(settingData[0].from_mailid);
            mail.Subject = mailSubject;
            mail.Body = bodyText;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();

            smtp.Port = settingData[0].smtp_port;
            smtp.Credentials = new System.Net.NetworkCredential(settingData[0].from_mailid, settingData[0].from_mailpwd);
            smtp.Host = settingData[0].smtp_address;
            smtp.EnableSsl = (settingData[0].enable_adsl == "Y" ? true : false);

            try
            {
                 smtp.Send(mail);
                return true;
            }
            catch (SmtpException smtpEx)
            {
                throw smtpEx;
                return false;
            }
            catch (Exception ex)
            {
                Exception ex2 = ex;
                string errorMessage = string.Empty;
                while (ex2 != null)
                {
                    errorMessage += ex2.ToString();
                    ex2 = ex2.InnerException;
                }
                throw ex2;
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
            return false;
        }
        return true;
    }


}