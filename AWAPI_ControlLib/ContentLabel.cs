using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;


namespace AWAPI_ControlLib
{
    [DefaultProperty("Text")]
    [Description("Returns description value of the content. Contents are specified based on lineage")]
    [ToolboxData("<{0}:ContentLabel runat=server></{0}:ContentLabel>")]
    public class ContentLabel : Label, INamingContainer
    {
        #region Properties

        /// <summary>
        /// Sample : Resources|Errors
        /// </summary>
        public string Lineage
        {
            get
            {
                if (ViewState["Lineage"] == null)
                    return null;
                return ViewState["Lineage"].ToString();
            }
            set
            {
                ViewState["Lineage"] = value;
            }
        }

        /// <summary>
        /// Sample : Will be looked under Lineage,
        /// example: File_Not_Found, 
        /// so the content will be taken from Lineage + '|' + Lineage,
        /// example: Resources|Errors|File_Not_Found
        /// </summary>
        public string Alias
        {
            get
            {
                if (ViewState["Alias"] == null)
                    return null;
                return ViewState["Alias"].ToString();
            }
            set
            {
                ViewState["Alias"] = value;
            }
        }

        /// <summary>
        /// Field name to be returned.
        /// Default is 'description'
        /// example: title, pubDate, etc...
        /// </summary>
        public string FieldName
        {
            get
            {
                if (ViewState["FieldName"] == null)
                    return "description";
                return ViewState["FieldName"].ToString();
            }
            set
            {
                ViewState["FieldName"] = value;
            }
        }

        public string Default
        {
            get
            {
                if (ViewState["Default"] == null)
                    return "";
                return ViewState["Default"].ToString();
            }
            set
            {
                ViewState["Default"] = value;
            }
        }

        #endregion


        protected override void RenderContents(HtmlTextWriter output)
        {
            Content content = new Content();
            content.MethodName = Content.EMethods.GetList;
            content.IgnoredUriParameters = "*";
            content.Lineage = Lineage;

            string url = content.ServiceCall;

            if (HttpContext.Current.Request["showurl"] != null)
                output.Write("<br/><span style='font-family:courier new;font-weight:normal;font-size:11px;color:red;background-color:#ffffff;'>" + url + "<br/></span>");

            string value = GetResource(url, Lineage, Alias, FieldName);

            if (!String.IsNullOrEmpty(value))
                Text = value;
            else
                Text = Default;
            output.Write(Text);
        }

        public static string GetResource(string url, string lineage, string alias, string fieldName)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            System.Data.DataSet ds = new DataSet();

            //if it is in the session, get from it
            if (HttpContext.Current.Session[url] != null)
                ds = (DataSet)HttpContext.Current.Session[url];
            else
                ds.ReadXml(url);
            //put into the session
            HttpContext.Current.Session[url] = ds;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 2)
            {
                DataRow[] drs = ds.Tables[2].Select("alias='" + alias + "'");
                if (drs != null && drs.Length > 0)

                    sb = new StringBuilder(HttpUtility.HtmlDecode(drs[0][fieldName].ToString()));
            }
            return sb.ToString();
        }

    }
}
