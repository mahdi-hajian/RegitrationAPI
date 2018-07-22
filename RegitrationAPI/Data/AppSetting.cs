//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using Microsoft.AspNetCore.Http.Features;

//namespace RegitrationAPI.Data
//{
//    public class AppSetting
//    {
//        PersianCalendar PersianDate = new PersianCalendar();
//        public string GetPersianDate()
//        {
//            try
//            {
//                string NowaDate = PersianDate.GetYear(DateTime.Now).ToString() + "/" + PersianDate.GetMonth(DateTime.Now).ToString() + "/" + PersianDate.GetDayOfMonth(DateTime.Now).ToString();
//                NowaDate += ' ' + DateTime.Now.ToShortTimeString().Replace("ب.ظ", "PM").Replace("ق.ظ", "AM");
//                return NowaDate;
//            }
//            catch (Exception)
//            {
//                return "";
//            }

//        }
//        public string GetIPAddress()
//        {
//            return new HttpContext()
//        }
//        public string ConvertToMiladi(string ShamsiDate)
//        {
//            try
//            {
//                ShamsiDate = ShamsiDate.Replace('۰', '0');
//                ShamsiDate = ShamsiDate.Replace('۱', '1');
//                ShamsiDate = ShamsiDate.Replace('۲', '2');
//                ShamsiDate = ShamsiDate.Replace('۳', '3');
//                ShamsiDate = ShamsiDate.Replace('۴', '4');
//                ShamsiDate = ShamsiDate.Replace('۵', '5');
//                ShamsiDate = ShamsiDate.Replace('۶', '6');
//                ShamsiDate = ShamsiDate.Replace('۷', '7');
//                ShamsiDate = ShamsiDate.Replace('۸', '8');
//                ShamsiDate = ShamsiDate.Replace('۹', '9');
//                int year = int.Parse(ShamsiDate.Split('/')[0]);
//                int month = int.Parse(ShamsiDate.Split('/')[1]);
//                int day = int.Parse(ShamsiDate.Split('/')[2]);
//                PersianCalendar p = new PersianCalendar();
//                DateTime date = p.ToDateTime(year, month, day, 0, 0, 0, 0);
//                return date.ToShortDateString();
//            }
//            catch (Exception)
//            {
//                return null;
//            }

//        }
//        public string ConvertToSafeDate(string ShamsiDate)
//        {
//            try
//            {
//                ShamsiDate = ShamsiDate.Replace('۰', '0');
//                ShamsiDate = ShamsiDate.Replace('۱', '1');
//                ShamsiDate = ShamsiDate.Replace('۲', '2');
//                ShamsiDate = ShamsiDate.Replace('۳', '3');
//                ShamsiDate = ShamsiDate.Replace('۴', '4');
//                ShamsiDate = ShamsiDate.Replace('۵', '5');
//                ShamsiDate = ShamsiDate.Replace('۶', '6');
//                ShamsiDate = ShamsiDate.Replace('۷', '7');
//                ShamsiDate = ShamsiDate.Replace('۸', '8');
//                ShamsiDate = ShamsiDate.Replace('۹', '9');
//                return ShamsiDate;
//            }
//            catch (Exception)
//            {
//                return null;
//            }

//        }
//        public string ConvertToShamsi(DateTime MidaliDate)
//        {
//            try
//            {
//                PersianCalendar shamsi = new PersianCalendar();
//                string ysh = shamsi.GetYear(MidaliDate).ToString();
//                string msh = shamsi.GetMonth(MidaliDate).ToString();
//                string dsh = shamsi.GetDayOfMonth(MidaliDate).ToString();
//                return ysh + "/" + (msh.ToString().Length == 1 ? "0" + msh.ToString() : msh.ToString()) + "/" + (dsh.ToString().Length == 1 ? "0" + dsh.ToString() : dsh.ToString());
//            }
//            catch (Exception)
//            {
//            }
//            return MidaliDate.ToShortDateString();
//        }
//        public string SafeFarsiStr(string input)
//        {
//            return input.Replace("ی", "ي").Replace("ک", "ک");
//        }

//    }
//}
