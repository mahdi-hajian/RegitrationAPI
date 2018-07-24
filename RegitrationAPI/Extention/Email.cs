using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
 
namespace RegitrationAPI.Extention
{
    public static class Email
    {
        private static readonly string EmailSender = "AgentMahdihajian@gmail.com";
        private static readonly string PasswordEmailSender = "";

        #region Send Email
        public async static void SendEmail(string EmailTo, string body, string subject)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(EmailSender);
                mail.To.Add(EmailTo);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(EmailSender, PasswordEmailSender);
                SmtpServer.EnableSsl = true;

                await SmtpServer.SendMailAsync(mail);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region SendEmailAfterRegistration
        //متد ارسال ایمیل بعد از ثبت نام
        /// <summary>
        /// send email after registration
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="code"></param>
        /// <param name="firstName"></param>
        public static void SendEmailAfterRegistration(string email, string userName, string password, string code, string firstName, string userId)
        {
            try
            {
                string strRootRelativePathName =
                "App_Data/LocalizedEmailTemplates/UserEmailVerification.htm";

                //ایجاد یک رشته و مراحل تبدیل مراحل نسبی به فیزیکی
                string strPathName =
                    Path.GetFullPath(strRootRelativePathName);

                //استفاده از متد رید کلاس فایل برای خواندن مسیر فیزیکی
                string strEmailBody = File.ReadAllText(strPathName);
                //جایگزینی مقادیر موجود در فایل خوانده شده با مقادیر داده شده
                strEmailBody = strEmailBody
                                .Replace("[USER_NAME]", userName)
                                .Replace("[PASSWORD]", password)
                                .Replace("[Code]", code)
                                .Replace("[UserId]", userId)
                                .Replace("[FIRST_NAME]", firstName);

                SendEmail(email, strEmailBody, "تایید ایمیل");
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region SendComfirmEmailAgain
        /// متد ارسال کلید تائید به کاربراگر هنگام ثبت نام عمل تائیدیه انجام نشد و اقدام به لاگین نمود

        /// <summary>
        /// if user want to send confirm email again
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="code"></param>
        /// <param name="firstName"></param>
        public static void SendComfirmEmailAgain(string email, string userName, string code, string firstName, string userId)
        {
            SendEmailAfterRegistration(email, userName, "Password", code, firstName, userId);
        }
        #endregion

        #region SendEmailForgotPassword
        //متد ارسال ایمیل برای فراموشی رمز عبور
        /// <summary>
        /// send email for forget password
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="userName"></param>
        /// <param name="code"></param>
        public static void SendEmailForgotPassword(string email, string firstName, string userName, string code, string userId)
        {
            //استفاده از قالب موجود در ای پی پی دیتا
            string strRootRelativePathName =
                "App_Data/LocalizedEmailTemplates/ForgotPasswordUserEmail.htm";

            //ایجاد یک رشته و مراحل تبدیل مراحل نسبی به فیزیکی
            string strPathName =
                Path.GetFullPath(strRootRelativePathName);

            //استفاده از متد رید کلاس فایل برای خواندن مسیر فیزیکی
            string strEmailBody = File.ReadAllText(strPathName);

            //جایگزینی مقادیر موجود در فایل خوانده شده با مقادیر داده شده
            strEmailBody = strEmailBody
                            .Replace("[FIRST_NAME]", firstName)
                            .Replace("[USER_NAME]", userName)
                            .Replace("[USER_ID]", userId)
                            .Replace("[CODE]", code);

            SendEmail(email, strEmailBody, "بازیابی گذرواژه");
        }
        #endregion

        #region Change Email
        
        public static void ChangeEmail(string email, string userName, string code, string userId)
        {
            //استفاده از قالب موجود در ای پی پی دیتا
            string strRootRelativePathName =
                "App_Data/LocalizedEmailTemplates/ChangeEmail.htm";

            //ایجاد یک رشته و مراحل تبدیل مراحل نسبی به فیزیکی
            string strPathName =
                Path.GetFullPath(strRootRelativePathName);

            //استفاده از متد رید کلاس فایل برای خواندن مسیر فیزیکی
            string strEmailBody = File.ReadAllText(strPathName);

            //جایگزینی مقادیر موجود در فایل خوانده شده با مقادیر داده شده
            strEmailBody = strEmailBody
                            .Replace("[USERID]", userId)
                            .Replace("[EMAIL]", email)
                            .Replace("[USER_NAME]", userName)
                            .Replace("[CODE]", code);

            SendEmail(email, strEmailBody, "تغییر ایمیل");
        }
        #endregion

        #region MyRegion
        //متد ارسال خبرنامه
        public static void SendNewsLetter(string userId, string email, string category, string name, string content)
        {
            //استفاده از قالب موجود در ای پی پی دیتا
            string strRootRelativePathName =
                "App_Data/LocalizedEmailTemplates/News.htm";

            //ایجاد یک رشته و مراحل تبدیل مراحل نسبی به فیزیکی
            string strPathName =
                Path.GetFullPath(strRootRelativePathName);

            //استفاده از متد رید کلاس فایل برای خواندن مسیر فیزیکی
            string strEmailBody = File.ReadAllText(strPathName);

            //جایگزینی مقادیر موجود در فایل خوانده شده با مقادیر داده شده
            strEmailBody = strEmailBody
                            .Replace("[USER_NAME]", email)
                            .Replace("[CAT]", category)
                            .Replace("[NAME]", name)
                            .Replace("[CONTENT]", content);
            //ایجاد یک شی از میل آدرس با 3 پارامتر
            System.Net.Mail.MailAddress oMailAddress =
                new System.Net.Mail.MailAddress(email, userId, System.Text.Encoding.UTF8);
            //استفاده از متد سند کلاس میل مسیج
            // MailMessage.Send
            // (oMailAddress, "خبرنامه!", strEmailBody, System.Net.Mail.MailPriority.High);
        }


        //متد ارسال تماس با ما
        public static void SendContact(string name, string email, string subject, string message)
        {
            //استفاده از قالب موجود در ای پی پی دیتا
            string strRootRelativePathName =
                "App_Data/LocalizedEmailTemplates/Contact.htm";

            //ایجاد یک رشته و مراحل تبدیل مراحل نسبی به فیزیکی
            string strPathName =
                Path.GetFullPath(strRootRelativePathName);

            //استفاده از متد رید کلاس فایل برای خواندن مسیر فیزیکی
            string strEmailBody = File.ReadAllText(strPathName);

            //جایگزینی مقادیر موجود در فایل خوانده شده با مقادیر داده شده
            strEmailBody = strEmailBody
                            .Replace("[NAME]", name)
                            .Replace("[MAIL]", email)
                            .Replace("[SUBJECT]", subject)
                            .Replace("[MESSAGE]", message);
            //ایجاد یک شی از میل آدرس با 3 پارامتر
            System.Net.Mail.MailAddress oMailAddress =
                new System.Net.Mail.MailAddress(email, email, System.Text.Encoding.UTF8);
            //استفاده از متد سند کلاس میل مسیج
            // MailMessage.Send
            // (oMailAddress, "تماس با ما!", strEmailBody, System.Net.Mail.MailPriority.High);
        }
        #endregion

    }
}
