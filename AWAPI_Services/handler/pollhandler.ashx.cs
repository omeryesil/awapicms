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
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class pollhandler : IHttpHandler
    {
        PollLibrary _pollLib = new PollLibrary();
        SyndicationFeed _feed = new SyndicationFeed();

        string _blogPageUrl = "";

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

        awSite _site = null;

        #endregion

        #region URI Paremeters

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
        /// GetParent, Get, GetSub, GetSubs
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
        /// 
        /// </summary>
        long PollId
        {
            get
            {
                if (HttpContext.Current.Request["pollid"] == null)
                    return 0;
                return Convert.ToInt64(HttpContext.Current.Request["pollid"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        string CultureCode
        {
            get
            {
                if (HttpContext.Current.Request["culture"] == null)
                    return "";
                return HttpContext.Current.Request["culture"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        int PageIndex
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
        int PageSize
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
        string Parameters
        {
            get
            {
                if (HttpContext.Current.Request["prms"] == null)
                    return "";
                return HttpContext.Current.Request["prms"];
            }
        }

        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


        public void ProcessRequest(HttpContext context)
        {
            _site = null;
            if (SiteId != 0)
                _site = new AWAPI_BusinessLibrary.library.SiteLibrary().Get(SiteId);

            if (_site == null || !_site.isEnabled)
                return;

            string link = String.IsNullOrEmpty(_site.link) ? "http://awapi.com" : _site.link;

            _feed = new SyndicationFeed(_site.title + " - Poll Feed", "", new Uri(link));
            _feed.Authors.Add(new SyndicationPerson("omer@awapi.com"));

            _feed.AttributeExtensions.Add(new XmlQualifiedName("site"), _site.title);
            _feed.AttributeExtensions.Add(new XmlQualifiedName("sitelink"), _site.link);
            _feed.AttributeExtensions.Add(new XmlQualifiedName("defaultculture"), _site.cultureCode);
            _feed.AttributeExtensions.Add(new XmlQualifiedName("culture"), CultureCode);
            if (PollId > 0) _feed.AttributeExtensions.Add(new XmlQualifiedName("pollid"), PollId.ToString());

            AddParamsToFeed();

            context.Response.ContentType = "text/xml";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;

            switch (MethodName)
            {
                case "getpoll":
                    _feed.Title = new TextSyndicationContent(_site.title + " - Poll");
                    GetPoll(PollId, CultureCode);
                    break;

                case "getpolllist":
                    _feed.Title = new TextSyndicationContent(_site.title + " - Poll List");
                    GetPollList(SiteId, CultureCode, PageIndex, PageSize);
                    break;

                default:
                    break;
            }

            string output = "";
            XmlWriter writer = XmlWriter.Create(context.Response.Output);
            if (_feed != null)
            {
                switch (ReturnType)
                {
                    case "atom":
                        context.Response.ContentType = "application/atom+xml";
                        Atom10FeedFormatter atom = new Atom10FeedFormatter(_feed);
                        atom.WriteTo(writer);
                        break;

                    case "json":
                        context.Response.ContentType = "application/json";
                        Rss20FeedFormatter rssFeed = new Rss20FeedFormatter(_feed);
                        if (rssFeed != null)
                        {
                            output = Newtonsoft.Json.JsonConvert.SerializeObject(rssFeed);
                            //JavaScriptSerializer ser = new JavaScriptSerializer();
                            //output = ser.Serialize(rssFeed);
                        }
                        break;

                    default:    //rss
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

        void AddParamsToFeed()
        {
            if (Parameters.Trim().Length == 0)
                return;

            string[] prms = Parameters.Split('|');
            foreach (string prm in prms)
            {
                if (prm.Trim().Length == 0)
                    continue;
                string[] prmAndValue = prm.Split('=');
                string prmOnly = prmAndValue[0];
                string valueOnly = "";
                if (prmAndValue.Length > 1)
                    valueOnly = prmAndValue[1];
                _feed.AttributeExtensions.Add(new XmlQualifiedName(prmOnly), valueOnly);
            }
        }


        #region POLL
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogid"></param>
        /// <param name="cultureCode"></param>
        void GetPoll(long pollId, string cultureCode)
        {
            AWAPI_Data.CustomEntities.PollExtended poll = _pollLib.GetPoll(pollId, cultureCode, true);
            if (poll == null || poll.siteId != _site.siteId || !poll.isEnabled)
                return;

            List<SyndicationItem> items = new List<SyndicationItem>();
            SyndicationItem blogitem = CreatePollSyndicationItem(poll, cultureCode);
            items.Add(blogitem);

            _feed.Items = items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        void GetPollList(long siteId, string cultureCode, int pageIndex, int pageSize)
        {
            IList<AWAPI_Data.CustomEntities.PollExtended> pollList = _pollLib.GetPollList(siteId, "", cultureCode, true);

            //IList<awPoll> pollList = _pollLib.GetPollList(siteId, "", cultureCode, true);
            if (pollList == null || pollList.Count() == 0)
                return;

            AddPageInfoToFeedHeader(pollList.Count, pageIndex, pageSize);

            //create syndication items for each blog item
            List<SyndicationItem> items = new List<SyndicationItem>();
            int rowNo = 0;
            int startRowNo = PageIndex * PageSize;
            foreach (AWAPI_Data.CustomEntities.PollExtended poll in pollList.ToList())
            {
                //Check if paging is enabled.
                if (PageSize > 0)
                {
                    if (rowNo < startRowNo)
                    {
                        rowNo++;
                        continue;
                    }
                    else if (rowNo >= startRowNo + PageSize)
                        break;
                }
                SyndicationItem pollItem = CreatePollSyndicationItem(poll, cultureCode);

                items.Add(pollItem);
                rowNo++;
            }

            _feed.Items = items;
        }
        #endregion


        #region helper methods

        void AddPageInfoToFeedHeader(int numberOfRecords, int pageIndex, int pageSize)
        {
            int pageCount = 1;
            if (pageSize > 0)
            {
                pageCount = numberOfRecords / pageSize;
                if ((pageCount * pageSize) - pageCount > 0)
                    pageCount += 1;
            }

            _feed.AttributeExtensions.Add(new XmlQualifiedName("pagesize"), pageSize.ToString());
            _feed.AttributeExtensions.Add(new XmlQualifiedName("pageindex"), pageIndex.ToString());
            _feed.AttributeExtensions.Add(new XmlQualifiedName("numberofrecords"), numberOfRecords.ToString());
            _feed.AttributeExtensions.Add(new XmlQualifiedName("pagecount"), pageCount.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        SyndicationItem CreatePollSyndicationItem(AWAPI_Data.CustomEntities.PollExtended poll, string cultureCode)
        {
            Uri uri = null;
            int totalVotes = 0;

            //SyndicationItem item = new SyndicationItem(
            //                    String.IsNullOrEmpty(poll.title) ? "" : AWAPI_Common.library.MiscLibrary.AddCDATA(poll.title),
            //                    String.IsNullOrEmpty(poll.description) ? "" : AWAPI_Common.library.MiscLibrary.AddCDATA(poll.description),
            //                    uri,
            //                    poll.pollId.ToString(),
            //                    poll.lastBuildDate.Value);

            SyndicationItem item = new SyndicationItem();
            item.Title = String.IsNullOrEmpty(poll.title) ? null : new TextSyndicationContent(poll.title, TextSyndicationContentKind.Html);
            item.Content = String.IsNullOrEmpty(poll.description) ? null : new TextSyndicationContent(poll.description, TextSyndicationContentKind.Html);
            item.LastUpdatedTime = poll.lastBuildDate.Value;
            item.BaseUri = uri;
            item.Id = poll.pollId.ToString();

            item.ElementExtensions.Add("pollid", null, poll.pollId);
            if (!string.IsNullOrEmpty(poll.answeredQuestion))
                item.ElementExtensions.Add("answeredquestion", null, AWAPI_Common.library.MiscLibrary.EncodeHtml(poll.answeredQuestion));
            item.ElementExtensions.Add("ismultiplechoice", null, poll.isMultipleChoice);
            if (poll.pubDate != null) item.ElementExtensions.Add("pubdate", null, poll.pubDate);

            IList<AWAPI_Data.CustomEntities.PollChoiceExtended> choiceList = _pollLib.GetPollChoiceList(poll.pollId, CultureCode);

            if (choiceList != null && choiceList.Count > 0)
            {
                foreach (AWAPI_Data.CustomEntities.PollChoiceExtended choice in choiceList)
                    totalVotes += choice.numberOfVotes;

                foreach (AWAPI_Data.CustomEntities.PollChoiceExtended choice in choiceList)
                {
                    double percentage = 0;
                    if (totalVotes > 0)
                        percentage = choice.numberOfVotes * 100 / totalVotes;

                    SyndicationCategory cat = new SyndicationCategory();

                    cat.AttributeExtensions.Add(new XmlQualifiedName("name"), "choices");
                    cat.AttributeExtensions.Add(new XmlQualifiedName("pollid"), choice.pollId.ToString());
                    cat.AttributeExtensions.Add(new XmlQualifiedName("pollchoiceid"), choice.pollChoiceId.ToString());
                    cat.AttributeExtensions.Add(new XmlQualifiedName("title"), AWAPI_Common.library.MiscLibrary.EncodeHtml(choice.title));
                    cat.AttributeExtensions.Add(new XmlQualifiedName("description"), AWAPI_Common.library.MiscLibrary.EncodeHtml(choice.description));
                    cat.AttributeExtensions.Add(new XmlQualifiedName("numberofvotes"), choice.numberOfVotes.ToString());
                    cat.AttributeExtensions.Add(new XmlQualifiedName("voteratio"), Convert.ToInt32(percentage).ToString());
                    item.Categories.Add(cat);
                }
            }

            item.ElementExtensions.Add("totalvotes", null, totalVotes);
            return item;
        }

        #endregion


    }
}
