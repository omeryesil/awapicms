using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.ServiceModel.Syndication;
using System.Web.Script.Serialization;
using AWAPI_BusinessLibrary.library;
using AWAPI_Data.Data;
using System.Reflection;


namespace AWAPI_Services.handler
{
    public class BaseHandler 
    {
        public SyndicationFeed _feed = new SyndicationFeed();

        #region Properties
        public awSite _site = null;

        /// <summary>
        /// atom, rss, json
        /// </summary>
        public string ReturnType
        {
            get
            {
                if (HttpContext.Current.Request["type"] == null)
                    return "rss";
                return HttpContext.Current.Request["type"].ToLower().Trim();
            }
        }

        #endregion

        #region URI PARAMETERS
        /// <summary>
        /// Site Id is required 
        /// </summary>
        public long SiteId
        {
            get
            {
                if (HttpContext.Current.Request["siteid"] == null)
                    return 0;
                return Convert.ToInt64(HttpContext.Current.Request["siteid"]);
            }
        }

        /// <summary>
        /// GetParent, Get, GetSub, GetSubs
        /// </summary>
        public string MethodName
        {
            get
            {
                if (HttpContext.Current.Request["method"] == null)
                    return "";
                return HttpContext.Current.Request["method"].ToLower().Trim();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (HttpContext.Current.Request["pageindex"] == null)
                    return 0;
                return Convert.ToInt32(HttpContext.Current.Request["pageindex"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int PageSize
        {
            get
            {
                if (HttpContext.Current.Request["pagesize"] == null)
                    return 0;
                return Convert.ToInt32(HttpContext.Current.Request["pagesize"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Parameters
        {
            get
            {
                if (HttpContext.Current.Request["prms"] == null)
                    return "";
                return HttpContext.Current.Request["prms"];
            }
        }


        #endregion


        public void WriteFeed()
        {
            string output = "";
            XmlWriter writer = XmlWriter.Create(HttpContext.Current.Response.Output);
            if (_feed != null)
            {
                switch (ReturnType)
                {
                    case "atom":
                        HttpContext.Current.Response.ContentType = "application/atom+xml";
                        _feed.Description = new TextSyndicationContent("AWAPI Content in Atom 1.0 Feed Format");
                        Atom10FeedFormatter atom = new Atom10FeedFormatter(_feed);
                        atom.WriteTo(writer);
                        break;
                    case "json":
                        HttpContext.Current.Response.ContentType = "application/json";
                        Rss20FeedFormatter rssFeed = new Rss20FeedFormatter(_feed);
                        if (rssFeed != null)
                        {
                            output = Newtonsoft.Json.JsonConvert.SerializeObject(rssFeed);
                            //JavaScriptSerializer ser = new JavaScriptSerializer();
                            //output = ser.Serialize(rssFeed);
                        }
                        break;
                    default:    //rss
                        _feed.Description = new TextSyndicationContent("AWAPI Content in RSS 2.0 Feed Format");
                        HttpContext.Current.Response.ContentType = "application/rss+xml";
                        Rss20FeedFormatter rss = new Rss20FeedFormatter(_feed);
                        rss.WriteTo(writer);
                        break;
                }
            }

            if (output != "")
                HttpContext.Current.Response.Write(output);

            writer.Close();
        }
    }
}
