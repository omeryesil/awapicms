using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Data;
using System.Web;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
using System.Reflection;
using System.ComponentModel;

namespace AWAPI_Common.library
{
    public class MiscLibrary
    {

        /// <summary>
        /// Returns a unique 64 bit Id from GUID
        /// </summary>
        /// <returns></returns>
        public static long CreateUniqueId()
        {
            byte[] bytes = System.Guid.NewGuid().ToByteArray();
            long ret = 0;
            long b = (bytes[0] ^ bytes[15]);
            ret += b;
            b = (bytes[1] ^ bytes[14]); b = b << 8;
            ret += b;
            b = (bytes[2] ^ bytes[13]); b = b << 16;
            ret += b;
            b = (bytes[3] ^ bytes[12]); b = b << 24;
            ret += b;
            b = (bytes[4] ^ bytes[11]); b = b << 32;
            ret += b;
            b = (bytes[5] ^ bytes[10]); b = b << 40;
            ret += b;
            b = (bytes[6] ^ bytes[9]); b = b << 48;
            ret += b;
            b = (bytes[7] ^ bytes[8]) & 0x7F; b = b << 56; // (to avoid the sign change) 
            ret += b;

            return ret;
        }

        /// <summary>
        /// Surronds string with CDATA
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string EncodeHtml(string source)
        {
            return System.Web.HttpUtility.HtmlEncode(source);
            //return System.Text.Encoding.
            //return "<![CDATA[" + source + "]]>";
        }

        /// <summary>
        /// Removes special characters with - from alias name
        /// </summary>
        /// <param name="source"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string FormatAliasName(string source)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]");
            string alias = reg.Replace(source.ToLower(), "-");

            return alias;
        }



        /// <summary>
        /// -1: Expired, 0: Not published yet, 1: between (true)
        /// </summary>
        /// <param name="now"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>
        /// -1: Expired
        /// 0: Not published yet
        /// 1: between (true)
        /// </returns>
        public static int IsDateBetween(DateTime now, DateTime? startDate, DateTime? endDate)
        {
            //Not published yet
            if (startDate != null && now < startDate)
                return 0;

            //Expired...
            if (endDate != null && now > endDate)
                return -1;

            //Between
            return 1;
        }


        #region string methods
        public static DateTime? ConvertStringToDate(string dtInString)
        {
            return ConvertStringToDate(dtInString, false);
        }


        /// <summary>
        /// if time (HH:mm) isn't 
        /// </summary>
        /// <param name="dtInString"></param>
        /// <param name="addLastHour"></param>
        /// <returns></returns>
        public static DateTime? ConvertStringToDate(string dtInString, bool isEndDate)
        {
            return ConvertStringToDate(dtInString, "MM/dd/yyyy", isEndDate);
        }

        ///// <summary>
        ///// Converts date in string to DateTime format
        ///// format: MM/dd/yyyy or 
        /////         
        /////         
        ///// </summary>
        ///// <param name="dtInString"></param>
        ///// <returns></returns>

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtInString"></param>
        /// <param name="format">example: MM/dd/yyyy</param>
        /// <returns></returns>
        public static DateTime? ConvertStringToDate(string dtInString, string format, bool isEndDate)
        {
            DateTime? dtRtn = null;
            dtInString = dtInString.Trim();

            if (dtInString.Length <= 0)
                return null;

            int timeSeperator = dtInString.Split(':').Length;
            switch (timeSeperator)
            {
                //case 1: //doesn't have hour in it
                //    dtRtn = DateTime.ParseExact(dtInString, format, null);
                //    break;
                case 2:  //has hour and minute
                    dtRtn = DateTime.ParseExact(dtInString, format + " HH:mm", null);
                    break;
                case 3://has hour, minute and second
                    dtRtn = DateTime.ParseExact(dtInString, format + " HH:mm:ss", null);
                    break;
                default:
                    dtRtn = DateTime.ParseExact(dtInString, format, null);
                    if (isEndDate)
                        dtRtn = new DateTime(dtRtn.Value.Year, dtRtn.Value.Month, dtRtn.Value.Day,
                                              23, 59, 59);
                    break;
            }

            return dtRtn;
        }


        /// <summary>
        /// Converts DateTime to string in MM/dd/yyyy HH:mm format
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateToString(object obj)
        {
            string rtn = "";

            if (obj == null || obj.ToString().Trim().Length == 0)
                return "";

            DateTime? t = (DateTime?)obj;
            rtn = t.Value.ToString("MM/dd/yyyy HH:mm");
            return rtn;
        }

        public static string ReplaceInsensitive(string strValue, string strToReplace, string strByReplace)
        {
            int index = strValue.IndexOf(strToReplace, 0, StringComparison.CurrentCultureIgnoreCase);
            if (index == -1)
                return strValue;
            else
            {
                strToReplace = strValue.Substring(index, strToReplace.Length);
                return strValue.Replace(strToReplace, strByReplace);
            }
        }

        public static string CropSentence(string sourceText, int length)
        {
            if (length <= 0) return sourceText;

            string sourceWithOutHTML = System.Text.RegularExpressions.Regex.
                                        Replace(sourceText, @"<(.|\n)*?>", string.Empty);

            if (sourceWithOutHTML.Trim().Length == 0)
                return sourceText;

            StringBuilder cropped = new StringBuilder("");
            string[] words = sourceWithOutHTML.Split(' ');

            foreach (string word in words)
            {
                if (cropped.Length + word.Length >= length)
                    return cropped.ToString();
                cropped.Append(word);
                cropped.Append(" ");
            }
            return cropped.ToString();
        }
        #endregion



        #region FILE


        #endregion

        #region COOKIE
        public static string CookieGetValue(string cookieName)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] != null &&
                HttpContext.Current.Request.Cookies[cookieName].Value.ToString().Trim().Length > 0)
                return HttpContext.Current.Request.Cookies[cookieName].Value.ToString();
            return "";
        }

        public static void CookieSetValue(string cookieName, string value, int numberOfDays)
        {
            HttpContext context = HttpContext.Current;
            context.Response.Cookies[cookieName].Value = value;
            context.Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(numberOfDays);
        }

        public static void CookieRemove(string cookieName)
        {
            HttpContext context = HttpContext.Current;
            context.Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(-1);
        }
        #endregion
    }
}
