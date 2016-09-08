using System;
using System.Web;
using System.Data;

namespace AWAPI_ControlLib.Classes
{
    public class Utilities
    {
        /// <summary>
        /// Returns a unique 64 bit Id from GUID
        /// </summary>
        /// <returns></returns>
        public static long GetUniqueId()
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

        ///// <summary>
        ///// Returns an object's size
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static long GetObjectSize(object obj)
        //{
        //    long len = 0;
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream();

        //    System.IO.StreamWriter sw = new System.IO.StreamWriter(ms);
        //    sw.Write(obj);
        //    len = ms.Length;
        //    sw.Close();

        //    ms.Dispose();
        //    sw.Dispose();

        //    return len;
        //}


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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtrs"></param>
        /// <param name="startRow"></param>
        /// <param name="endRow"></param>
        /// <returns></returns>
        public static DataTable DataRowsToDataTable(DataRow[] dtrs, int startRow, int endRow)
        {
            DataTable dt = new DataTable();

            if (dtrs == null || dtrs.Length == 0)
                dt = null;
            else
            {
                dt = dtrs[0].Table.Clone();
                {
                    bool startEnabled = startRow > 0 ? true : false;

                    int nCurrentRow = 0;
                    foreach (DataRow dr in dtrs)
                    {
                        if (endRow > 0 && nCurrentRow >= endRow)
                            break;

                        if (startRow > 0)
                        {
                            if (nCurrentRow >= startRow)
                                dt.ImportRow(dr);
                        }
                        else
                            dt.ImportRow(dr);

                        nCurrentRow++;
                    }
                }
            }
            return dt;
        }

    }
}
