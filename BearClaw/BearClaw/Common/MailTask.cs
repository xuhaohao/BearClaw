using BearClaw.Models;
using BearClaw.Strategy;
using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BearClaw.Common
{
    public class MailTask
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(MailTask));

        private static object lockObj = new object();

        private static List<Jobs> _bag = new List<Jobs>();

        public static void Put(int id ,List<Jobs> jobs, Action<List<Jobs>> sendResult) {
            if (jobs != null && jobs.Count > 0) {
                _bag.AddRange(jobs);
            }
            log.DebugFormat("MailTask.Put(id={0})   _bag.Count = {1}", id, _bag.Count);
            if ( id % MyStrategy.List.Count == 0)
            {
                Release(sendResult);
            }
        }

        public static void Release(Action<List<Jobs>> sendResult)
        {
            log.DebugFormat("MailTask.Release.    _bag.Count = {0}", _bag.Count);
            if (_bag.Count > 0)
            {
                SendMail(sendResult);
            }
        }


        private static void SendMail(Action<List<Jobs>> sendResult)
        {
            if (_bag != null && _bag.Count > 0)
            {
                var mailId = Guid.NewGuid().ToString();
                var content = new StringBuilder();
                log.DebugFormat(content.ToString());
                content.Append("<!DOCTYPE HTML><html><body>");
                content.AppendFormat("恭喜发财,发现{0}家公司:<br/>", _bag.Count);
                content.Append("<table border='1'><tr><th>序号</th><th>公司名称</th><th>来源</th></tr>");
                for (int i = 0; i < _bag.Count; i++)
                {
                    var job = _bag[i];
                    content.Append("<tr>");
                    content.Append("<td>").Append(i + 1).Append("</td>");
                    content.Append("<td>").Append("<a href='").Append(job.Url).Append("'>").Append(job.Name).Append("</a></td>");
                    content.Append("<td>").Append(job.Ext1??"").Append("</td>");
                    content.Append("</tr>");
                }
                content.Append("</table><br/><br/>");
                content.Append(mailId).Append("<br/>");
                content.Append(DateTime.Now.ToString()).Append("<br/>");
                content.Append(Dns.GetHostName()).Append("<br/>");
                content.Append("</body><html>");
                
                var subject = string.Format("发现{0}家公司", _bag.Count);
                SendMail(mailId , subject, content.ToString(), sendResult);
            }
        }



        private static void SendMail(string mailId,string subject , string content, Action<List<Jobs>> sendResult)
        {
            var eMailServer = Db.GetFirstParam("email", "send_server").Value;
            var userName = Db.GetFirstParam("email", "send_username").Value;
            var pwd = Db.GetFirstParam("email", "send_password").Value;
            //var subject = Db.GetFirstParam("email", "send_subject").Value;
            var list = Db.GetParams("email", "receive_username");

            var mailMessage = new MailMessage();
            foreach (var param_receive_username in list)
            {
                if (param_receive_username.Value != null && param_receive_username.Value.Trim() != "")
                {
                    //mailMessage.To.Add(@"370081393@qq.com");
                    mailMessage.To.Add(param_receive_username.Value.Trim());
                }
            }

            mailMessage.From = new MailAddress(userName, "mimi");
            mailMessage.Subject = subject;
            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.Body = content;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;
            mailMessage.Priority = MailPriority.High;

            var smtp = new SmtpClient(eMailServer, 25) { Credentials = new NetworkCredential(userName, pwd) };
            smtp.EnableSsl = true;
            object userState = mailMessage;

            try
            {
                smtp.SendCompleted += (s, e) =>
                {
                    if (e.Error == null)
                    {
                        sendResult(_bag);
                        log.DebugFormat("邮件[{0}]发送成功", mailId);
                        _bag.Clear();
                    }
                    else {
                        sendResult(new List<Jobs>());
                        log.Error(string.Format("邮件[{0}]发送失败,舍弃:", mailId), e.Error);
                    }
                };
                smtp.SendAsync(mailMessage, userState);
                log.DebugFormat("邮件[{0}]发送开始,内容:{1}", mailId , content);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("邮件[{0}]发送失败:", mailId), ex);
            }
        }
    }
}
