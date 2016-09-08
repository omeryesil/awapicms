using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace AWAPI.App_Code
{
    public class Misc
    {
        #region MESSAGE
        public enum MessageType
        {
            ERROR,
            INFO,
            WARNING
        }

        public static void WriteMessage(System.Web.UI.WebControls.Literal lit, string message)
        {
            WriteMessage(MessageType.INFO, lit, message);
        }

        public static void WriteMessage(MessageType msgType, System.Web.UI.WebControls.Literal lit, string message)
        {
            System.Text.StringBuilder msg = new System.Text.StringBuilder();

            string className = "msgWarning";
            string title = "";
            switch (msgType)
            {
                case MessageType.INFO:
                    className = "msgInfo";
                    title = "";
                    break;
                case MessageType.ERROR:
                    className = "msgError";
                    title = "<b>ERROR : </b>";
                    break;
            }

            msg.Append("<span id='msgContainer' class='" + className + "'>");
            msg.Append(title);
            msg.Append(message);
            msg.Append("</span>");

            lit.Text = msg.ToString();
        }
        #endregion


        /// <summary>
        /// Removes the query from the url (http://awapi.com/default.aspx)
        /// </summary>
        /// <returns></returns>
        public static string GetUrlOnly()
        {
            if (String.IsNullOrEmpty(HttpContext.Current.Request.Url.Query))
                return HttpContext.Current.Request.Url.ToString();
            return HttpContext.Current.Request.Url.ToString().Replace(HttpContext.Current.Request.Url.Query, "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="size">1616, 2424, 3232... default is 1616</param>
        /// <returns></returns>
        public static string GetContentIcon(string contentType, string size)
        {
            contentType = contentType.ToLower().Trim();
            if (String.IsNullOrEmpty(size))
                size = "1616";


            string imageUrl = "/admin/img/fileassociation/" + size + "/icon-text.png";

            if (contentType.IndexOf("image") >= 0)
                imageUrl = "~/admin/img/fileassociation/" + size + "/icon-image.png";
            if (contentType.IndexOf("video") >= 0)
                imageUrl = "~/admin/img/fileassociation/" + size + "/icon-video.png";
            else if (contentType.IndexOf("excel") >= 0)
                imageUrl = "~/admin/img/fileassociation/" + size + "/icon-ms-excel-2003.png";
            else if (contentType.IndexOf("msword") >= 0)
                imageUrl = "~/admin/img/fileassociation/" + size + "/icon-ms-word-2003.png";
            else if (contentType.IndexOf("pdf") >= 0)
                imageUrl = "~/admin/img/fileassociation/" + size + "/icon-adobe-pdf.png";


            return imageUrl;
        }

    }
}
