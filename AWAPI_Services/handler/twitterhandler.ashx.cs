using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Xml;
using System.Drawing.Imaging;
using AWAPI_BusinessLibrary.library;
using System.ServiceModel.Syndication;

using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
//using Twitterizer.Framework;
using LinqToTwitter;


namespace AWAPI_Services.handler
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class twitterhandler : IHttpHandler
    {
        AWAPI_BusinessLibrary.library.SiteLibrary _siteLib = new SiteLibrary();
        SyndicationFeed _feed = new SyndicationFeed();

        #region Properties

        /// <summary>
        /// atom, rss, json
        /// </summary>
        string ReturnType
        {
            get
            {
                if (HttpContext.Current.Request["type"] == null)
                    return "rss";
                return HttpContext.Current.Request["type"].ToLower().Trim();
            }
        }
        #endregion

        #region URI Paremeters

        /// <summary>
        /// GetStatus, GetStatusList
        /// </summary>
        string MethodName
        {
            get
            {
                if (HttpContext.Current.Request["method"] == null)
                    return "";
                return HttpContext.Current.Request["method"].ToLower().Trim();
            }
        }


        /// <summary>
        /// Site Id is required 
        /// </summary>
        long SiteId
        {
            get
            {
                if (HttpContext.Current.Request["siteid"] == null)
                    return 0;
                return Convert.ToInt64(HttpContext.Current.Request["siteid"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        int PageSize
        {
            get
            {
                if (HttpContext.Current.Request["pagesize"] == null)
                    return 0;
                return Convert.ToInt32(HttpContext.Current.Request["pagesize"]);
            }
        }

        #endregion


        public void ProcessRequest(HttpContext context)
        {
            if (SiteId <= 0)
                return;

            AWAPI_Data.Data.awSite site = _siteLib.Get(SiteId);
            if (site == null || !site.isEnabled ||
                String.IsNullOrEmpty(site.twitterUsername) || String.IsNullOrEmpty(site.twitterPassword))
                return;

            string username = site.twitterUsername;
            string password = new AWAPI_Common.library.SecurityLibrary().DecodeString(site.twitterPassword);

            _feed = new SyndicationFeed("AWAPI CMS Twitter Feed", "", null);
            _feed.Authors.Add(new SyndicationPerson(username));
            _feed.Categories.Add(new SyndicationCategory("tweets"));
            _feed.AttributeExtensions.Add(new XmlQualifiedName("pagesize"), PageSize.ToString());

            switch (MethodName)
            { 
                case "getstatus":
                    break;
                case "getstatuslist":
                    GetStatusList(username, password, PageSize);
                    break;                   
            }

            //TRANSFORM THE OUTPUT -----------------------------------------------------------------
            string output = "";
            XmlWriter writer = XmlWriter.Create(context.Response.Output);
            if (_feed != null)
            {
                switch (ReturnType)
                {
                    case "atom":
                        context.Response.ContentType = "application/atom+xml";
                        _feed.Description = new TextSyndicationContent("AWAPI Twitter in Atom 1.0 Feed Format");
                        Atom10FeedFormatter atom = new Atom10FeedFormatter(_feed);
                        atom.WriteTo(writer);
                        break;
                    case "json":
                        context.Response.ContentType = "application/json";
                        Rss20FeedFormatter rssFeed = new Rss20FeedFormatter(_feed);
                        if (rssFeed != null)
                        {
                            output = Newtonsoft.Json.JsonConvert.SerializeObject(rssFeed);
                        }
                        break;
                    default:    //rss
                        _feed.Description = new TextSyndicationContent("AWAPI Twitter in RSS 2.0 Feed Format");
                        context.Response.ContentType = "application/rss+xml";
                        Rss20FeedFormatter rss = new Rss20FeedFormatter(_feed);
                        rss.WriteTo(writer);
                        break;
                }
            }

            if (output != "")
                context.Response.Write(output);

            writer.Close();

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Returns status list
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="pageSize"></param>

        void GetStatusList(string username, string password, int pageSize)
        {

            var twitterCtx = new TwitterContext();
            var publicTweets =
                from tweet in twitterCtx.Status
                where tweet.Type == StatusType.User
                    && tweet.ScreenName == username
                select tweet;


            //GET TWEETS --------------------------------------------------------------------------
            TimeSpan thisSpan = new TimeSpan();
            int n = 0;

            System.Collections.Generic.IList<SyndicationItem> list = new System.Collections.Generic.List<SyndicationItem>();
            foreach (LinqToTwitter.Status status in publicTweets)
            {
                string timeBetween = "";
                thisSpan = DateTime.Now.Subtract(status.CreatedAt);

                if (thisSpan.Days > 0)
                    timeBetween = thisSpan.Days.ToString() + " days ago";
                else if (thisSpan.Hours > 0)
                    timeBetween = thisSpan.Hours.ToString() + " hours ago";
                else if (thisSpan.Minutes > 0)
                    timeBetween = thisSpan.Minutes.ToString() + " minutes ago";
                else if (thisSpan.Seconds > 0)
                    timeBetween = thisSpan.Seconds.ToString() + " seconds ago";

                Uri uri = new Uri("http://twitter.com/" + status.ScreenName + "/status/" + status.UserID);
                SyndicationItem item = new SyndicationItem(
                                status.Text, timeBetween, uri, "", status.CreatedAt);

                list.Add(item);
                n++;
                if (pageSize > 0 && n >= pageSize)
                    break;
            }
            _feed.Items = list;
        
        }
    }
}
