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


namespace AWAPI_ControlLib.Classes
{
    public class BaseControl : WebControl
    {

        public enum EXMLType
        {
            RSS,
            Atom,
            JSON        //if JSON, cannot be transformed
        }


        #region PROPERTIES
        [Category("Settings")]
        [Description("Culture code in two letters")]
        public string Culture
        {
            get
            {
                if (ViewState["culture"] != null)
                    return ViewState["culture"].ToString();

                string sessionName = System.Configuration.ConfigurationManager.AppSettings["AWAPI_CultureCode_SessionName"];
                if (String.IsNullOrEmpty(sessionName) || 
                    HttpContext.Current == null || HttpContext.Current.Session == null || HttpContext.Current.Session[sessionName] == null)
                    return "";

                //it might come as en-us,, we need only en
                string[] parts = HttpContext.Current.Session[sessionName].ToString().Split('-');
                return parts[0];
            }
            set
            {
                ViewState["culture"] = value;
            }
        }


        [Category("Cache")]
        [Description("EXMLType.RSS, EXMLType.Atom or EXMLType.JSON")]
        public EXMLType XMLType
        {
            get
            {
                if (ViewState["XMLType"] == null)
                    return EXMLType.RSS;
                return (EXMLType)ViewState["XMLType"];
            }
            set
            {
                ViewState["XMLType"] = value;
            }
        }

        [Category("Cache")]
        [Description("Cache duration; 10m, 1h, 1d")]
        public string CacheDuration
        {
            get
            {
                if (AWAPI_ControlLib.Classes.Configuration.ForceToDisableCache || ViewState["CacheDuration"] == null)
                    return "";

                return ViewState["CacheDuration"].ToString();
            }
            set
            {
                ViewState["CacheDuration"] = value;
            }
        }

        /// <summary>
        /// If uriPrm name exists in this string then it won't be added to the server call URL
        /// If * all the parameters will be ignored
        /// examle: ContentId|where|order
        /// </summary>
        [Browsable(true)]
        [Description("If Uri parameter is defined in the value, then this parameter will be ignored. Parameters are seperated by pipe (|)")]
        public string IgnoredUriParameters
        {
            get
            {
                if (ViewState["IgnoredUriParameters"] == null)
                    return "";
                return ViewState["IgnoredUriParameters"].ToString();
            }
            set
            {
                ViewState["IgnoredUriParameters"] = value;
            }
        }

        [Description("")]
        [DefaultValue(0)]
        public long SiteId
        {
            get
            {
                if (ViewState["siteid"] == null)
                    if (Classes.Configuration.SiteId > 0)
                        return Classes.Configuration.SiteId;
                    else
                        return 0;
                return Convert.ToInt64(ViewState["siteid"]);
            }
            set
            { ViewState["siteid"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string AccessKey
        {
            get
            {
                return Classes.Configuration.AccessKey;
            }        
        }



        [Description("Additional where statement")]
        public string Where
        {
            get
            {
                if (ViewState["where"] != null)
                    return ViewState["where"].ToString();

                string value = GetPropertyValue("where", "", "");
                return value;
            }
            set
            {
                ViewState["where"] = value;
            }
        }

        [Browsable(true)]
        [Description("Default is sortOrder")]
        public string SortBy
        {
            get
            {
                if (ViewState["SortBy"] != null)
                    return ViewState["SortBy"].ToString();

                string value = GetPropertyValue("sortby", "", "");
                return value;
            }
            set
            {
                ViewState["SortBy"] = value;
            }
        }

        /// <summary>
        /// </summary>
        [Description("")]
        public int MaxTitleLength
        {
            get
            {
                if (ViewState["MaxTitleLength"] == null)
                    return 0;
                return Convert.ToInt32(ViewState["MaxTitleLength"]);
            }
            set
            {
                ViewState["MaxTitleLength"] = value;
            }
        }

        /// <summary>
        /// </summary>
        [Description("")]
        public int MaxDescriptionLength
        {
            get
            {
                if (ViewState["MaxDescriptionLength"] == null)
                    return 0;
                return Convert.ToInt32(ViewState["MaxDescriptionLength"]);
            }
            set
            {
                ViewState["MaxDescriptionLength"] = value;
            }
        }

        /// <summary>
        /// </summary>
        [Description("Additional parameters which will be added to the xml. Format: prm1=value1|prm2=value2")]
        [Bindable(true)]
        public string Parameters
        {
            get
            {
                if (ViewState["Parameters"] == null)
                    return "";
                return ViewState["Parameters"].ToString();
            }
            set
            {
                ViewState["Parameters"] = value;
            }
        }


        /// <summary>
        /// First uriPrm is the property name, second is the uri uriPrm
        /// ContentId=catId|description=desc
        /// </summary>
        [Browsable(true)]
        [Description("Matches content's field values with Uri parameters. Example: ContentId=catId|Description=uriDescription ")]
        public string UriEquivalents
        {
            get
            {
                if (ViewState["UriEquivalents"] == null)
                    return "";
                return ViewState["UriEquivalents"].ToString();
            }
            set
            {
                ViewState["UriEquivalents"] = value;
            }
        }

        [Description("")]
        [DefaultValue(0)]
        public int PageIndex
        {
            get
            {
                if (ViewState["PageIndex"] == null)
                {
                    if (!IsInIgnoreList("PageIndex") && HttpContext.Current.Request["PageIndex"] != null)
                        return Convert.ToInt32(HttpContext.Current.Request["PageIndex"]);
                    else
                        return 0;
                }
                else
                    return Convert.ToInt32(ViewState["PageIndex"].ToString());
            }
            set
            {
                ViewState["PageIndex"] = value;
            }
        }

        [Description("")]
        [DefaultValue(0)]
        public int PageSize
        {
            get
            {
                if (ViewState["pagesize"] == null)
                    return -1;
                return Convert.ToInt32(ViewState["pagesize"]);
            }
            set
            { ViewState["pagesize"] = value; }
        }
        #endregion


        /// <summary>
        /// UriEquivalents:   
        ///     ContentId=catId|Lineage=names meas=> get ContentId from the URI catId
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>
        /// if not defined returns ""
        /// if defined but URI value is empty or not found returns "none"
        /// </returns>
        public string GetURIMatchesValue(string propertyName)
        {
            if (UriEquivalents.Trim().Length == 0)
                return "";
            string parameterName = "";
            string[] parts = UriEquivalents.Split('|');
            bool found = false;

            foreach (string part in parts)
            {
                string[] propertyAndValue = part.Split('=');
                if (propertyAndValue.Length < 2)
                    continue;

                if (propertyName.ToLower().Trim() == propertyAndValue[0].Trim().ToLower())
                {
                    found = true;
                    parameterName = propertyAndValue[1];
                    break;
                }
            }

            if (found == false)
                return "";

            if (parameterName.Trim().Length == 0)
                return "none";

            if (HttpContext.Current.Request[parameterName] != null)
                return HttpContext.Current.Request[parameterName].ToString();

            return "none";
        }

        public bool IsInIgnoreList(string uriPrm)
        {
            //Ignore all
            if (IgnoredUriParameters.Trim() == "*")
                return true;
            
            if (IgnoredUriParameters.Trim() == "")
                return false;

            uriPrm = uriPrm.ToLower();
            string[] prms = IgnoredUriParameters.ToLower().Split('|');
            foreach (string prm in prms)
                if (prm == uriPrm)
                    return true;

            return false;
        }

        public int GetPropertyValue(string propertyName, int property, int defaultValue)
        {
            int rtn = 0;
            string value = "";

            value = this.GetURIMatchesValue(propertyName);

            if (value == "")
                rtn = property;
            else if (value == "none")
                rtn = defaultValue;
            else
                rtn = Int32.Parse(value);

            return rtn;
        }

        public string GetPropertyValue(string propertyName, string property, string defaultValue)
        {
            string rtn = "";
            string value = "";

            value = this.GetURIMatchesValue(propertyName);

            if (value == "")
                rtn = property;
            else if (value == "none")
                rtn = defaultValue;
            else
                rtn = value;

            return rtn;
        }

        public long GetPropertyValue(string propertyName, long property, long defaultValue)
        {
            long rtn = 0;
            string value = "";

            value = this.GetURIMatchesValue(propertyName);

            if (value == "")
                rtn = property;
            else if (value == "none")
                rtn = defaultValue;
            else
                rtn = Int64.Parse(value);

            return rtn;
        }


        public string GetXSLUrl(string path)
        {
            string strRtn = path;

            if (path.IndexOf("http:") < 0)
            {
                int dynamicPathStart = path.IndexOf("{");
                int dynamicPathEnd = path.IndexOf("}");
                if (dynamicPathStart >= 0 && dynamicPathEnd > 0)
                {
                    string dynamicAppKeyName = path.Substring(dynamicPathStart + 1, dynamicPathEnd - dynamicPathStart - 1);
                    string appKeyValue = System.Configuration.ConfigurationManager.AppSettings[dynamicAppKeyName];
                    if (!String.IsNullOrEmpty(appKeyValue))
                        path = path.Replace("{" + dynamicAppKeyName + "}", appKeyValue);
                }


                string baseUrl = new Classes.XslFunctions().GetDomainUrl();
                if (baseUrl.EndsWith("/"))
                    baseUrl = baseUrl.Remove(baseUrl.Length - 1);

                strRtn = baseUrl + ResolveUrl(path);
            }
            return strRtn;
        }


    }
}
