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
    public class contenthandler : IHttpHandler
    {
        ContentLibrary _contentLib = new ContentLibrary();
        ContentCustomFieldLibrary _customFieldLib = new ContentCustomFieldLibrary();
        TagLibrary _tagLib = new TagLibrary();

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
        long ContentId
        {
            get
            {
                if (HttpContext.Current.Request["contentid"] == null)
                    return 0;
                return Convert.ToInt64(HttpContext.Current.Request["contentid"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        string Alias
        {
            get
            {
                if (HttpContext.Current.Request["alias"] == null)
                    return "";
                return HttpContext.Current.Request["alias"].ToLower().Trim();
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
        long CurrentContentId
        {
            get
            {
                if (HttpContext.Current.Request["currcontentid"] == null)
                    return 0;
                return Convert.ToInt64(HttpContext.Current.Request["currcontentid"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        int Deep
        {
            get
            {
                if (HttpContext.Current.Request["deep"] == null)
                    return 0;
                return Convert.ToInt32(HttpContext.Current.Request["deep"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        string Where
        {
            get
            {
                if (HttpContext.Current.Request["where"] == null)
                    return "";
                return HttpContext.Current.Request["where"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        string SortBy
        {
            get
            {
                if (HttpContext.Current.Request["sortby"] == null)
                    return "";
                return HttpContext.Current.Request["sortby"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        string Lineage
        {
            get
            {
                if (HttpContext.Current.Request["lineage"] == null)
                    return "";
                return HttpContext.Current.Request["lineage"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        int MaximumTitleLength
        {
            get
            {
                if (HttpContext.Current.Request["maxtitlelength"] == null)
                    return 0;
                return Convert.ToInt32(HttpContext.Current.Request["maxtitlelength"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        int MaximumDescLength
        {
            get
            {
                if (HttpContext.Current.Request["maxdesclength"] == null)
                    return 0;
                return Convert.ToInt32(HttpContext.Current.Request["maxdesclength"]);
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

        /// <summary>
        /// | seperated tags
        /// </summary>
        string Tags
        {
            get
            {
                if (HttpContext.Current.Request["tags"] == null)
                    return "";
                return HttpContext.Current.Request["tags"];
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

            string culture = CultureCode;
            if (String.IsNullOrEmpty(CultureCode))
                culture = _site.cultureCode;


            _feed = new SyndicationFeed("AWAPI CMS Feed", "", null);
            _feed.Authors.Add(new SyndicationPerson(""));
            _feed.Categories.Add(new SyndicationCategory("contents"));
            _feed.AttributeExtensions.Add(new XmlQualifiedName("site"), _site.title);
            _feed.AttributeExtensions.Add(new XmlQualifiedName("sitelink"), _site.link);
            _feed.AttributeExtensions.Add(new XmlQualifiedName("defaultculture"), _site.cultureCode);
            _feed.AttributeExtensions.Add(new XmlQualifiedName("culture"), culture);

            _feed.AttributeExtensions.Add(new XmlQualifiedName("currentcontentid"), CurrentContentId.ToString());
            _feed.AttributeExtensions.Add(new XmlQualifiedName("pagesize"), PageSize.ToString());
            _feed.AttributeExtensions.Add(new XmlQualifiedName("pageindex"), PageIndex.ToString());
            _feed.AttributeExtensions.Add(new XmlQualifiedName("servertime"), DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
            AddParamsToFeed();

            context.Response.ContentType = "text/xml";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;

            switch (MethodName)
            {
                case "get":
                    if (ContentId <= 0 && !String.IsNullOrEmpty(Alias))
                        GetByAlias(SiteId, Alias, Lineage, Deep, culture);
                    else
                        Get(SiteId, ContentId, Lineage, Deep, culture);
                    break;

                case "getlist":
                    GetList(SiteId, ContentId, Lineage, Deep, Tags, culture);
                    break;

                case "gettaglist":
                    GetTagList(SiteId);
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
                        _feed.Description = new TextSyndicationContent("AWAPI Content in Atom 1.0 Feed Format");
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
                        _feed.Description = new TextSyndicationContent("AWAPI Content in RSS 2.0 Feed Format");
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


        /// <summary>
        /// ContentId is required
        /// Also deep parameter can be used.. 
        /// if deep is 0 then looks for the siteTagId
        /// If deep is smaller than 0 then it will look for the parents
        /// if deep is greater than 0 then it will look for childs
        /// if Lineage is not empty, then first the deep will be found, then the 
        ///     Lineage will be checked...
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        void Get(long siteId, long contentId, string Lineage, int deep, string cultureCode)
        {
            awContent contentToReturn = GetContent(siteId, contentId, Lineage, deep, cultureCode);

            if (contentToReturn == null)
                return;

            //if (!_contentLib.IsParentsAvailable(true, contentToReturn))
            //    return;

            contentId = contentToReturn.contentId;
            List<SyndicationItem> items = new List<SyndicationItem>();
            SyndicationItem contentItem = CreateContentSyndicationItem(contentToReturn);
            AddCustomField(contentId, contentToReturn.parentContentId, cultureCode, ref contentItem);
            items.Add(contentItem);

            _feed.Items = items;
        }

        /// <summary>
        /// ContentId is required
        /// Also deep parameter can be used.. 
        /// if deep is 0 then looks for the siteTagId
        /// If deep is smaller than 0 then it will look for the parents
        /// if deep is greater than 0 then it will look for childs
        /// if Lineage is not empty, then first the deep will be found, then the 
        ///     Lineage will be checked...
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        void GetByAlias(long siteId, string alias, string lineage, int deep, string cultureCode)
        {
            string tmpLineage = alias;
            if (!String.IsNullOrEmpty(Lineage))
                if (tmpLineage.EndsWith("|"))
                    tmpLineage = Lineage + alias;
                else
                    tmpLineage = Lineage + "|" + alias;          

            awContent contentToReturn = GetContent(siteId, 0, tmpLineage, deep, cultureCode);

            if (contentToReturn == null)
                return;

            List<SyndicationItem> items = new List<SyndicationItem>();
            SyndicationItem contentItem = CreateContentSyndicationItem(contentToReturn);
            AddCustomField(contentToReturn.contentId, contentToReturn.parentContentId, cultureCode, ref contentItem);
            items.Add(contentItem);

            _feed.Items = items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="siteTagId"></param>
        /// <param name="Lineage"></param>
        /// <param name="deep"></param>
        void GetList(long siteId, long contentId, string Lineage, int deep, string tags, string cultureCode)
        {
            awContent parentContent = GetContent(siteId, contentId, Lineage, deep, cultureCode);
            if (parentContent == null)
                return;

            StringBuilder where = new StringBuilder();
            where.Append(" parentContentId=" + parentContent.contentId);

            if (Where.Trim() != "")
                where.Append(" AND ( " + Where + "  ) ");

            IList<awContent> contentList = _contentLib.GetList(true, SiteId, where.ToString(), SortBy, cultureCode).ToList();
            if (contentList == null || contentList.Count() == 0)
                return;

            //FILTER FOR TAGS 
            if (tags.Trim().Length > 0)
            {
                IList<awContent> taggedContents = _tagLib.GetTaggedContentList(tags);
                if (taggedContents == null || taggedContents.Count == 0)
                    return;

                ArrayList arrIds = new ArrayList();
                foreach (awContent c in taggedContents)
                    arrIds.Add(c.contentId);

                var taggeds = from t in contentList
                              where arrIds.Contains(t.contentId)
                              select t;
                if (taggeds == null || taggeds.ToList().Count == 0)
                    return;
                contentList = taggeds.ToList();
            }

            //create syndication items for each blog item
            AWAPI_BusinessLibrary.library.ContentCustomFieldLibrary custFieldLib = new ContentCustomFieldLibrary();
            List<SyndicationItem> items = new List<SyndicationItem>();
            int rowNo = 0;
            int startRowNo = PageIndex * PageSize;
            foreach (awContent content in contentList.ToList())
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

                SyndicationItem contentItem = CreateContentSyndicationItem(content);

                AddCustomField(content.contentId, content.parentContentId, cultureCode, ref contentItem);

                items.Add(contentItem);
                rowNo++;
            }

            _feed.Items = items;

        }

        /// <summary>
        /// Returns tag list
        /// </summary>
        /// <param name="siteId"></param>
        void GetTagList(long siteId)
        {
            var tags = _tagLib.GetList(siteId, "");

            List<SyndicationItem> items = new List<SyndicationItem>();
            int rowNo = 0;
            int startRowNo = PageIndex * PageSize;
            foreach (awSiteTag tag in tags.ToList())
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

                SyndicationItem
                    tagItem = new SyndicationItem(AWAPI_Common.library.MiscLibrary.EncodeHtml(tag.title),
                                                    "",
                                                    null,
                                                    tag.siteTagId.ToString(),
                                                    tag.createDate);

                //add item to the list
                items.Add(tagItem);
                rowNo++;
            }

            _feed.Items = items;

        }
        #region helper methods



        /// <summary>
        /// ContentId is required
        /// Also deep parameter can be used.. 
        /// if deep is 0 then looks for the siteTagId
        /// If deep is smaller than 0 then it will look for the parents
        /// if deep is greater than 0 then it will look for childs
        /// if Lineage is not empty, then first the deep will be found, then the 
        ///     Lineage will be checked...
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        awContent GetContent(long siteId, long contentId, string contentLineageInAlias, int deep, string cultureCode)
        {
            long contentIdToReturn = 0;

            if (Deep == 0)  // returns the current one
                contentIdToReturn = contentId;
            else
            {
                awContent content = _contentLib.Get(contentId, cultureCode, true);
                if (content == null)
                    return null;

                if (deep < 0) //look for parents 
                {
                    if (String.IsNullOrEmpty(content.lineage))
                        return null;

                    string[] parentIds = content.lineage.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    if (deep == -99 || (-1 * deep) >= parentIds.Length) //get the first one
                        contentIdToReturn = Convert.ToInt64(parentIds[0]);
                    else
                    {
                        contentIdToReturn = Convert.ToInt64(parentIds[parentIds.Length + Deep]);
                    }
                }
                else if (Deep > 0)  //look for childs
                {
                    //TODO: Complete this.
                }
            }

            awContent contentToReturn = null;
            if (String.IsNullOrEmpty(contentLineageInAlias))
                contentToReturn = _contentLib.Get(contentIdToReturn, cultureCode, true);
            else
                contentToReturn = _contentLib.GetByAliasLineage(siteId, contentIdToReturn, contentLineageInAlias, cultureCode);

            if (!_contentLib.IsParentsAvailable(true, contentToReturn))
                return null;
            return contentToReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        SyndicationItem CreateContentSyndicationItem(awContent content)
        {
            Uri uri = null;
            if (content.link != null && content.link.Trim() != "")
                uri = new Uri(content.link);

            string title = String.IsNullOrEmpty(content.title) ? content.alias : content.title;

            //SyndicationItem item = new SyndicationItem(
            //                    AWAPI_Common.library.MiscLibrary.AddCDATA(AWAPI_Common.library.MiscLibrary.CropSentence(title, MaximumTitleLength)),
            //                    AWAPI_Common.library.MiscLibrary.AddCDATA(AWAPI_Common.library.MiscLibrary.CropSentence(content.description, MaximumDescLength)),
            //                    uri,
            //                    content.contentId.ToString(),
            //                    content.lastBuildDate.Value);

            SyndicationItem item = new SyndicationItem();
            item.Title = new TextSyndicationContent(AWAPI_Common.library.MiscLibrary.CropSentence(title, MaximumTitleLength), TextSyndicationContentKind.Html);
            item.Content = new TextSyndicationContent(AWAPI_Common.library.MiscLibrary.CropSentence(content.description, MaximumDescLength), TextSyndicationContentKind.Html);
            item.LastUpdatedTime = content.lastBuildDate.Value;
            item.BaseUri = uri;
            item.Id = content.contentId.ToString();


            //item.ElementExtensions.Add("contentid", null, blog.siteTagId);
            if (content.alias != null) item.ElementExtensions.Add("alias", null, content.alias);
            if (content.contentType != null) item.ElementExtensions.Add("contenttype", null, content.contentType);
            if (content.createDate != null) item.ElementExtensions.Add("createdate", null, content.createDate);
            if (content.imageurl != null) item.ElementExtensions.Add("imageurl", null, content.imageurl);
            if (content.lineage != null) item.ElementExtensions.Add("lineage", null, content.lineage);
            if (content.parentContentId != null) item.ElementExtensions.Add("parentcontentid", null, content.parentContentId);
            if (content.pubDate != null) item.ElementExtensions.Add("pubdate", null, content.pubDate);
            if (content.pubEndDate != null) item.ElementExtensions.Add("pubenddate", null, content.pubEndDate);
            if (content.eventStartDate != null)
            {
                item.ElementExtensions.Add("eventstartdate", null, content.eventStartDate);
                item.ElementExtensions.Add("eventstartdate_dddd", null, content.eventStartDate.Value.ToString("dddd"));
                item.ElementExtensions.Add("eventstartdate_MMMM", null, content.eventStartDate.Value.ToString("MMMM"));
            }
            if (content.eventEndDate != null)
            {
                item.ElementExtensions.Add("eventenddate", null, content.eventEndDate);
                item.ElementExtensions.Add("eventenddate_dddd", null, content.eventEndDate.Value.ToString("dddd"));
                item.ElementExtensions.Add("eventenddate_MMMM", null, content.eventEndDate.Value.ToString("MMMM"));

            }

            item.ElementExtensions.Add("siteid", null, content.siteId);
            if (content.sortOrder != null) item.ElementExtensions.Add("sortorder", null, content.sortOrder);
            item.ElementExtensions.Add("userid", null, content.userId);

            return item;
        }

        void AddCustomField(long contentId, long? parentContentId, string cultureCode, ref SyndicationItem contentItem)
        {
            System.Collections.Generic.IEnumerable<AWAPI_Data.CustomEntities.ContentCustomField> cstFlds = _customFieldLib.GetFieldList(contentId, true);

            if (cstFlds != null && cstFlds.Count() > 0)
            {
                var result = from r in cstFlds
                             where r.applyToSubContents == false && r.fieldContentId.Equals(contentId) ||
                                   r.applyToSubContents && r.fieldContentId.Equals(parentContentId)
                             orderby r.sortOrder
                             select r;

                IList<AWAPI_Data.CustomEntities.ContentCustomFieldValueExtended> valueList = _customFieldLib.GetFieldValueList(contentId, cultureCode.ToLower());

                foreach (AWAPI_Data.CustomEntities.ContentCustomFieldValueExtended value in valueList)
                {
                    SyndicationElementExtension extElement = new SyndicationElementExtension(value.title.Trim(),
                                                            null,
                                                            AWAPI_Common.library.MiscLibrary.EncodeHtml(AWAPI_Common.library.MiscLibrary.CropSentence(value.fieldValue, MaximumDescLength)));
                    contentItem.ElementExtensions.Add(extElement);
                }
            }
        }



        #endregion


    }
}
