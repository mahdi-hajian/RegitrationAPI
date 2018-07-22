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
        //متد ارسال ایمیل بعد از ثبت نام
        /// <summary>
        /// send email after registration
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="code"></param>
        /// <param name="firstName"></param>
        public static void SendEmailAfterRegistration(string Email, string userName, string password, string code, string firstName)
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
                                .Replace("[UserName]", userName)
                                .Replace("[Code]", code)
                                .Replace("[FIRST_NAME]", firstName);

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(EmailSender);
                mail.To.Add(Email);
                mail.Subject = "تایید ثبت نام";
                mail.Body = strEmailBody;
                mail.BodyEncoding = Encoding.UTF8;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(EmailSender, PasswordEmailSender);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception)
            {
            }
        }
        //#######################################################################


        /// متد ارسال کلید تائید به کاربراگر هنگام ثبت نام عمل تائیدیه انجام نشد و اقدام به لاگین نمود
        public static void Sendcode(string userId, string userName, string code, string firstName)
        {
            SendEmailAfterRegistration(userId, userName, "Account Password", code, firstName);
        }

        //#######################################################################


        //متد ارسال ایمیل برای فراموشی رمز عبور
        /// <summary>
        /// send email for forget password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="code"></param>
        public static void SendEmailForgotPassword
            (string userId, string userName, string code)
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
                            .Replace("[USER_ID]", userId)
                            .Replace("[USER_NAME]", userName)
                            .Replace("[CODE]", code);
            //ایجاد یک شی از میل آدرس با 3 پارامتر
            System.Net.Mail.MailAddress oMailAddress =
                new System.Net.Mail.MailAddress(userName, userId, System.Text.Encoding.UTF8);
            //استفاده از متد سند کلاس میل مسیج
            // MailMessage.Send
            // (oMailAddress, "بازیابی گذرواژه!", strEmailBody, System.Net.Mail.MailPriority.High);
        }

        //#######################################################################
        //متد ارسال خبرنامه
        public static void SendNewsLetter
            (string userId, string email, string category, string name, string content)
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

        //#######################################################################

        //متد ارسال تماس با ما
        public static void SendContact
            (string name, string email, string subject, string message)
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

        //#######################################################################
    }
}
