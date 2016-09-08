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
    public class contesthandler : BaseHandler, IHttpHandler
    {
        ContestLibrary _contestLib = new ContestLibrary();

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

            _feed = new SyndicationFeed("AWAPI CMS Feed", "", null);
            _feed.Authors.Add(new SyndicationPerson(""));
            _feed.Categories.Add(new SyndicationCategory("contests"));
            _feed.AttributeExtensions.Add(new XmlQualifiedName("site"), _site.title);
            _feed.AttributeExtensions.Add(new XmlQualifiedName("sitelink"), _site.link);
            _feed.AttributeExtensions.Add(new XmlQualifiedName("defaultculture"), _site.cultureCode);
            _feed.AttributeExtensions.Add(new XmlQualifiedName("pagesize"), PageSize.ToString());
            _feed.AttributeExtensions.Add(new XmlQualifiedName("pageindex"), PageIndex.ToString());
            _feed.AttributeExtensions.Add(new XmlQualifiedName("servertime"), DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
            AddParamsToFeed();

            context.Response.ContentType = "text/xml";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;

            switch (MethodName)
            {
                case "getentrylist":
                    GetEntryList(SiteId);
                    break;

                case "getentrydailysummary":
                    GetEntryDailySummary(SiteId);
                    break;

                default:
                    break;
            }

            WriteFeed();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="siteTagId"></param>
        /// <param name="Lineage"></param>
        /// <param name="deep"></param>
        void GetEntryList(long siteId)
        {
            long userId = HttpContext.Current.Request["userId"] == null ? 0 : Convert.ToInt64(HttpContext.Current.Request["userId"]);
            long contestGroupId = HttpContext.Current.Request["groupId"] == null ? 0 : Convert.ToInt64(HttpContext.Current.Request["groupId"]);
            long contestId = HttpContext.Current.Request["contestId"] == null ? 0 : Convert.ToInt64(HttpContext.Current.Request["contestId"]);
            string tmpDate = HttpContext.Current.Request["date"] == null ? "" : HttpContext.Current.Request["date"].ToString();
            DateTime? dt = null;
            if (!String.IsNullOrEmpty(tmpDate))
                dt = DateTime.ParseExact(tmpDate, "yyyy-MM-dd", null);


            IList<AWAPI_Data.CustomEntities.ContestEntryExtended> list = null;
            if (contestGroupId > 0)
                list = _contestLib.GetContestEntryListByGroupId(contestGroupId, "", dt);
            else if (contestId > 0)
                list = _contestLib.GetContestEntryList(contestId, userId, "", dt);

            if (list == null || list.Count == 0)
                return;

            ////IF userId is set, then get this user's contest entries...
            //if (userId > 0)
            //{
            //    var tmpList = from l in list
            //                  where (l.userId != null && l.userId.Equals(userId) && l.awUser_Contest.isEnabled)
            //                  select l;
            //    if (tmpList == null || tmpList.Count() == 0)
            //        return;

            //    list = tmpList.ToList();
            //}

            //Filter for files
            //var tmpList2 = from l in list
            //               where (l.fileId == null ||
            //                     (l.fileId != null && l.awFile_Contest.isEnabled))
            //               select l;

            //if (tmpList2 == null || tmpList2.Count() == 0)
            //    return;

            //list = tmpList2.ToList();

            //add contest header info
            //DateTime? dtContestStartDate = list[0].awContest.pubDate;
            //DateTime? dtContestEndDate = list[0].awContest.pubEndDate;
            _feed.AttributeExtensions.Add(new XmlQualifiedName("groupid"), contestGroupId.ToString());
            _feed.AttributeExtensions.Add(new XmlQualifiedName("contestid"), contestId.ToString());
            _feed.AttributeExtensions.Add(new XmlQualifiedName("userid"), userId.ToString());
            //_feed.AttributeExtensions.Add(new XmlQualifiedName("conteststartdate"), dtContestStartDate == null ? "" : dtContestStartDate.Value.ToString("yyyy-dd-MM HH:mm:ss"));
            //_feed.AttributeExtensions.Add(new XmlQualifiedName("contestenddate"), dtContestEndDate == null ? "" : dtContestEndDate.Value.ToString("yyyy-dd-MM HH:mm:ss"));

            List<SyndicationItem> items = new List<SyndicationItem>();
            int rowNo = 0;
            int startRowNo = PageIndex * PageSize;
            foreach (AWAPI_Data.CustomEntities.ContestEntryExtended entry in list.ToList())
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

                SyndicationItem contentItem = CreateContestEntrySyndicationItem(entry);


                items.Add(contentItem);
                rowNo++;
            }

            _feed.Items = items;

        }

        /// <summary>
        /// Resturns summary of contest,,
        /// contestId is require
        /// </summary>
        /// <param name="siteId"></param>
        void GetEntryDailySummary(long siteId)
        {
            long userId = HttpContext.Current.Request["userId"] == null ? 0 : Convert.ToInt64(HttpContext.Current.Request["userId"]);
            long contestId = HttpContext.Current.Request["contestId"] == null ? 0 : Convert.ToInt64(HttpContext.Current.Request["contestId"]);

            if (contestId <= 0)
                return;

            IList<AWAPI_Data.CustomEntities.ContestEntryDailyReport> list = _contestLib.GetContestEntryDailyReport(contestId, userId);


            //add contest header info
            _feed.AttributeExtensions.Add(new XmlQualifiedName("contestid"), contestId.ToString());
            _feed.AttributeExtensions.Add(new XmlQualifiedName("userid"), userId.ToString());

            List<SyndicationItem> items = new List<SyndicationItem>();
            foreach (AWAPI_Data.CustomEntities.ContestEntryDailyReport entry in list.ToList())
            {
                Uri uri = null;
                SyndicationItem item = new SyndicationItem(
                                    AWAPI_Common.library.MiscLibrary.EncodeHtml(entry.contestTitle),
                                    "",
                                    uri, 
                                    entry.contestId.ToString(),
                                    entry.contestPubDate.Value);

                item.Title = new TextSyndicationContent(entry.contestTitle, TextSyndicationContentKind.Html);


                item.ElementExtensions.Add("contestid", null, entry.contestId);
                item.ElementExtensions.Add("contesttitle", null, entry.contestTitle);
                item.ElementExtensions.Add("contestpubdate", null, entry.contestPubDate);
                item.ElementExtensions.Add("entrydate", null, entry.entryDay);
                item.ElementExtensions.Add("numberofentries", null, entry.numberOfEntries);

                if (entry.firstFileId != null) item.ElementExtensions.Add("firstfileid", null, entry.firstFileId);
                if (entry.firstFileUrl != null) item.ElementExtensions.Add("firstfileurl", null, entry.firstFileUrl);
                if (entry.numberOfEntries != null) item.ElementExtensions.Add("number", null, entry.numberOfEntries);


                items.Add(item);
            }

            _feed.Items = items;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        SyndicationItem CreateContestEntrySyndicationItem(AWAPI_Data.CustomEntities.ContestEntryExtended entry)
        {
            Uri uri = null;

            //Calculate contest's entry day
            //DateTime? dtContestStartDate = entry.awContest.pubDate;
            string dayDifference = "0";
            //if (dtContestStartDate != null)
            //{
            //    TimeSpan difference = entry.createDate.Subtract(dtContestStartDate.Value);
            //    dayDifference = difference.Days.ToString();
            //}
            //Create syndication item
            //SyndicationItem item = new SyndicationItem(
            //                    AWAPI_Common.library.MiscLibrary.AddCDATA(entry.title),
            //                    AWAPI_Common.library.MiscLibrary.AddCDATA(entry.description),
            //                    uri,
            //                    entry.contestEntryId.ToString(),
            //                    entry.createDate);

            SyndicationItem item = new SyndicationItem();
            item.Title = new TextSyndicationContent(entry.title, TextSyndicationContentKind.Html);
            item.Content = new TextSyndicationContent(entry.description, TextSyndicationContentKind.Html);
            item.LastUpdatedTime = entry.createDate;
            item.BaseUri = uri;
            item.Id = entry.contestEntryId.ToString();



            item.ElementExtensions.Add("pubdate", null, entry.createDate);
            item.ElementExtensions.Add("daydifference", null, entry.daysPassed);
            if (entry.userId != null) item.ElementExtensions.Add("userid", null, entry.userId);
            if (entry.email != null) item.ElementExtensions.Add("email", null, entry.email);
            if (entry.firstName != null) item.ElementExtensions.Add("firstname", null, entry.firstName);
            if (entry.lastName != null) item.ElementExtensions.Add("lastname", null, entry.lastName);
            if (entry.tel != null) item.ElementExtensions.Add("tel", null, entry.tel);
            if (entry.telType != null) item.ElementExtensions.Add("teltype", null, entry.telType);
            if (entry.address != null) item.ElementExtensions.Add("address", null, entry.address);
            if (entry.city != null) item.ElementExtensions.Add("city", null, entry.city);
            if (entry.postalCode != null) item.ElementExtensions.Add("postalcode", null, entry.postalCode);
            if (entry.country != null) item.ElementExtensions.Add("country", null, entry.country);

            if (entry.fileId != null)
            {
                item.ElementExtensions.Add("fileid", null, entry.fileId);
                item.ElementExtensions.Add("fileurl", null, entry.fileUrl);
                item.ElementExtensions.Add("contenttype", null, entry.fileContentType);
                item.ElementExtensions.Add("thumbnail", null, entry.fileThumbUrl);
            }
            return item;
        }



    }
}
