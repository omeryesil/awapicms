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
    public class bloghandler : BaseHandler, IHttpHandler
    {
        BlogLibrary _blogLib = new BlogLibrary();
        ContentCustomFieldLibrary _customFieldLib = new ContentCustomFieldLibrary();
        TagLibrary _tagLib = new TagLibrary();


        #region URI Paremeters

        /// <summary>
        /// 
        /// </summary>
        long BlogId
        {
            get
            {
                if (HttpContext.Current.Request["blogid"] == null)
                    return 0;
                return Convert.ToInt64(HttpContext.Current.Request["blogid"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        long BlogCategoryId
        {
            get
            {
                if (HttpContext.Current.Request["blogcatid"] == null)
                    return 0;
                return Convert.ToInt64(HttpContext.Current.Request["blogcatid"]);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        long PostId
        {
            get
            {
                if (HttpContext.Current.Request["postid"] == null)
                    return 0;
                return Convert.ToInt64(HttpContext.Current.Request["postid"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        long TagId
        {
            get
            {
                if (HttpContext.Current.Request["tagid"] == null)
                    return 0;
                return Convert.ToInt64(HttpContext.Current.Request["tagid"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        long CurrentPostId
        {
            get
            {
                if (HttpContext.Current.Request["currpostid"] == null)
                    return 0;
                return Convert.ToInt64(HttpContext.Current.Request["currpostid"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        string Archive
        {
            get
            {
                if (HttpContext.Current.Request["archive"] == null)
                    return "";
                return HttpContext.Current.Request["archive"];
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
        string Search
        {
            get
            {
                if (HttpContext.Current.Request["Search"] == null)
                    return "";
                return HttpContext.Current.Request["Search"];
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

            _feed = new SyndicationFeed(_site.title + " - Blog Feed", "", new Uri(link));
            _feed.Authors.Add(new SyndicationPerson("omer@awapi.com"));

            _feed.AttributeExtensions.Add(new XmlQualifiedName("site"), _site.title);
            _feed.AttributeExtensions.Add(new XmlQualifiedName("sitelink"), _site.link);
            _feed.AttributeExtensions.Add(new XmlQualifiedName("defaultculture"), _site.cultureCode);

            _feed.AttributeExtensions.Add(new XmlQualifiedName("currentpostid"), CurrentPostId.ToString());
            _feed.AttributeExtensions.Add(new XmlQualifiedName("servertime"), DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));

            if (BlogId > 0) _feed.AttributeExtensions.Add(new XmlQualifiedName("blogid"), BlogId.ToString());
            if (BlogCategoryId > 0) _feed.AttributeExtensions.Add(new XmlQualifiedName("blogcategoryid"), BlogCategoryId.ToString());
            if (PostId > 0) _feed.AttributeExtensions.Add(new XmlQualifiedName("postid"), PostId.ToString());

            AddParamsToFeed();

            context.Response.ContentType = "text/xml";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;

            switch (MethodName)
            {
                case "getblog":
                    _feed.Title = new TextSyndicationContent(_site.title + " - Blog");
                    _feed.Categories.Add(new SyndicationCategory("blogs"));
                    GetBlog(SiteId, BlogId);
                    break;

                case "getbloglist":
                    _feed.Title = new TextSyndicationContent(_site.title + " - Blog List");
                    _feed.Categories.Add(new SyndicationCategory("blogs"));
                    GetBlogList(SiteId);
                    break;

                case "getcategorylist":
                    _feed.Title = new TextSyndicationContent(_site.title + " - Blog Category List");
                    _feed.Categories.Add(new SyndicationCategory("blogs"));
                    GetCategoryList(BlogId);
                    break;

                case "getpost":
                    _feed.Title = new TextSyndicationContent(_site.title + " - Blog Post");
                    _feed.Categories.Add(new SyndicationCategory("posts"));
                    GetBlogPost(SiteId, PostId);
                    break;

                case "getpostfilelist":
                    _feed.Title = new TextSyndicationContent(_site.title + " - Blog Post File List");
                    _feed.Categories.Add(new SyndicationCategory("files"));
                    GetBlogPostFileList(SiteId, PostId);
                    break;

                case "getpostlist":
                    _feed.Title = new TextSyndicationContent(_site.title + " - Post List");
                    _feed.Categories.Add(new SyndicationCategory("posts"));
                    GetBlogPostList(SiteId, BlogId, BlogCategoryId, TagId, Search, PageIndex, PageSize);
                    break;

                case "getarchivedpostlist":
                    _feed.Title = new TextSyndicationContent(_site.title + " - Archived Post List");
                    _feed.Categories.Add(new SyndicationCategory("posts"));
                    GetArchivedBlogPostList(SiteId, BlogId, Archive, PageIndex, PageSize);
                    break;

                case "getarchivelist":
                    _feed.Title = new TextSyndicationContent(_site.title + " - Post Archive List");
                    _feed.Categories.Add(new SyndicationCategory("posts"));
                    GetBlogPostArchiveList(BlogId);
                    break;

                case "getcommentlist":
                    _feed.Title = new TextSyndicationContent(_site.title + " - Comment List");
                    _feed.Categories.Add(new SyndicationCategory("comments"));
                    GetBlogCommentList(SiteId, BlogId, PostId);
                    break;

                case "gettaglist":
                    _feed.Title = new TextSyndicationContent(_site.title + " - Tag List");
                    _feed.Categories.Add(new SyndicationCategory("tags"));
                    GetBlogTagList(BlogId, PostId);
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

        #region CATEGORY
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        void GetCategoryList(long blogId)
        {
            IList<awBlogCategory> categoryList = _blogLib.GetBlogCategoryList(blogId, true);
            if (categoryList == null || categoryList.Count() == 0)
                return;

            //create syndication items for each blog item
            List<SyndicationItem> items = new List<SyndicationItem>();

            foreach (awBlogCategory cat in categoryList.ToList())
            {
                SyndicationItem contentItem = CreateBlogCategorySyndicationItem(cat);
                items.Add(contentItem);
            }

            _feed.Items = items;

        }
        #endregion

        #region BLOG
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="blogid"></param>
        void GetBlog(long siteId, long blogid)
        {
            awBlog blog = _blogLib.GetBlog(blogid);
            if (blog == null || blog.siteId != siteId || !blog.isEnabled)
                return;

            List<SyndicationItem> items = new List<SyndicationItem>();
            SyndicationItem blogitem = CreateBlogSyndicationItem(blog);
            items.Add(blogitem);

            _feed.Items = items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        void GetBlogList(long siteId)
        {
            StringBuilder where = new StringBuilder();
            where.Append(" isEnabled = 1 ");

            if (Where.Trim() != "")
                where.Append(" AND ( " + Where + "  ) ");

            IList<awBlog> blogList = _blogLib.GetBlogList(siteId, where.ToString());
            if (blogList == null || blogList.Count() == 0)
                return;

            AddPageInfoToFeedHeader(blogList.Count, PageIndex, PageSize, Search);

            //create syndication items for each blog item
            List<SyndicationItem> items = new List<SyndicationItem>();
            int rowNo = 0;
            int startRowNo = PageIndex * PageSize;
            foreach (awBlog content in blogList.ToList())
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

                SyndicationItem contentItem = CreateBlogSyndicationItem(content);

                items.Add(contentItem);
                rowNo++;
            }

            _feed.Items = items;

        }
        #endregion

        #region BLOG POST
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="blogPostCommentId"></param>
        void GetBlogPost(long siteId, long blogPostId)
        {
            DateTime now = DateTime.Now;
            AWAPI_Data.CustomEntities.BlogPostExtended blogPost = _blogLib.GetBlogPost(blogPostId);
            if (blogPost == null || blogPost.siteId != siteId ||
                !blogPost.Blog.isEnabled || !blogPost.isPublished)
                return;

            if (blogPost.pubDate != null && blogPost.pubDate > now ||
                blogPost.pubEndDate != null && blogPost.pubEndDate < now)
                return;

            _feed.AttributeExtensions.Add(new XmlQualifiedName("blogtitle"), blogPost.Blog.title);

            GetPostPageBaseUrl(blogPost.Blog.blogPostPage);

            List<SyndicationItem> items = new List<SyndicationItem>();
            string postPageBaseUrl = GetPostPageBaseUrl(blogPost.Blog.blogPostPage);
            SyndicationItem blogitem = CreateBlogPostSyndicationItem(blogPost, postPageBaseUrl);
            items.Add(blogitem);

            _feed.Items = items;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="blogId"></param>
        void GetBlogPostArchiveList(long blogId)
        {
            IList<AWAPI_Data.CustomEntities.BlogPostArchive> archiveList = _blogLib.GetPostArchiveList(blogId, true);

            //create syndication items for each blog item
            List<SyndicationItem> items = new List<SyndicationItem>();
            foreach (AWAPI_Data.CustomEntities.BlogPostArchive archiveMonth in archiveList.ToList())
            {
                SyndicationItem item = CreateBlogPostArchiveSyndicationItem(archiveMonth);
                items.Add(item);
            }

            _feed.Items = items;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="blogId"></param>
        void GetBlogPostList(long siteId, long blogId, long blogCategoryId, long tagId, string search, int pageIndex, int pageSize)
        {
            IList<AWAPI_Data.CustomEntities.BlogPostExtended> postList = null;
            if (tagId > 0)
                postList = _blogLib.GetBlogPostListByTagId(tagId, true, search);
            else if (blogCategoryId > 0)
                postList = _blogLib.GetPostListByCategoryId(blogCategoryId, true, search);
            else if (blogId > 0)
                postList = _blogLib.GetPostListByBlogId(blogId, true, search);
            else
                postList = _blogLib.GetBlogPostList(siteId, true, search);


            if (postList == null || postList.Count() == 0)
                return;

            AddPageInfoToFeedHeader(postList.Count, pageIndex, pageSize, search);

            //create syndication items for each blog item
            string postPageBaseUrl = GetPostPageBaseUrl(postList[0].Blog.blogPostPage);
            List<SyndicationItem> items = new List<SyndicationItem>();
            int rowNo = 0;
            int startRowNo = pageIndex * pageSize;
            foreach (AWAPI_Data.CustomEntities.BlogPostExtended post in postList.ToList())
            {
                //Check if paging is enabled.
                if (PageSize > 0)
                {
                    if (rowNo < startRowNo)
                    {
                        rowNo++;
                        continue;
                    }
                    else if (rowNo >= startRowNo + pageSize)
                        break;
                }

                SyndicationItem contentItem = CreateBlogPostSyndicationItem(post, postPageBaseUrl);

                items.Add(contentItem);
                rowNo++;
            }

            _feed.Items = items;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="blogId"></param>
        void GetBlogPostFileList(long siteId, long blogPostId)
        {
            AWAPI_Data.CustomEntities.BlogPostExtended blogPost = _blogLib.GetBlogPost(blogPostId);

            //check if the blog post is available
            DateTime now = DateTime.Now;
            if (blogPost == null || blogPost.siteId != siteId || 
                !blogPost.Blog.isEnabled || !blogPost.isPublished)
                return;

            if (blogPost.pubDate != null && blogPost.pubDate > now ||
                blogPost.pubEndDate != null && blogPost.pubEndDate < now)
                return;
            
            IList<AWAPI_Data.Data.awFile_Blog> fileList = _blogLib.GetBlogPostFileList(blogPostId);
            if (fileList == null || fileList.Count == 0)
                return;

            //create syndication items for each blog item
            string postPageBaseUrl = GetPostPageBaseUrl(blogPost.Blog.blogPostPage);
            List<SyndicationItem> items = new List<SyndicationItem>();
            foreach (AWAPI_Data.Data.awFile_Blog file in fileList)
            {
                SyndicationItem item = CreateBlogPostFileSyndicationItem(blogPost, file, postPageBaseUrl);
                items.Add(item);
            }
            _feed.Items = items;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="blogId"></param>
        /// <param name="archiveTime">month-year. Example: 4-2010</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        void GetArchivedBlogPostList(long siteId, long blogId, string archiveTime, int pageIndex, int pageSize)
        {
            string[] parts = archiveTime.Split('-');
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            if (parts.Length == 2)
            {
                year = Convert.ToInt32(parts[0]);
                month = Convert.ToInt32(parts[1]);
            }

            IList<AWAPI_Data.CustomEntities.BlogPostExtended> postList = _blogLib.GetPostListByArchiveDate(blogId, true, year, month);

            if (postList == null || postList.Count() == 0)
                return;

            AddPageInfoToFeedHeader(postList.Count, pageIndex, pageSize, "");

            //create syndication items for each blog item
            string postPageBaseUrl = GetPostPageBaseUrl(postList[0].Blog.blogPostPage);
            List<SyndicationItem> items = new List<SyndicationItem>();
            int rowNo = 0;
            int startRowNo = pageIndex * pageSize;
            foreach (AWAPI_Data.CustomEntities.BlogPostExtended post in postList.ToList())
            {
                //Check if paging is enabled.
                if (PageSize > 0)
                {
                    if (rowNo < startRowNo)
                    {
                        rowNo++;
                        continue;
                    }
                    else if (rowNo >= startRowNo + pageSize)
                        break;
                }

                SyndicationItem contentItem = CreateBlogPostSyndicationItem(post, postPageBaseUrl);

                items.Add(contentItem);
                rowNo++;
            }

            _feed.Items = items;
        }


        #endregion

        #region BLOG POST COMMENT
        /// <summary>
        /// If post id is set then comments under a post will be returned
        /// else if blogid is not set then comments under a blog will be returned
        /// else all the comments under the site will be returned
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="postId"></param>
        void GetBlogCommentList(long siteId, long blogId, long postId)
        {
            IList<AWAPI_Data.Data.awBlogPostComment> commentList = null;

            if (postId > 0)
                commentList = _blogLib.GetBlogPostCommentListByPostId(postId, true);
            else if (blogId > 0)
                commentList = _blogLib.GetBlogPostCommentListByBlogId(blogId, true);
            else
                commentList = _blogLib.GetBlogPostCommentListBySiteId(siteId, true);

            if (commentList == null || commentList.Count() == 0)
                return;

            AddPageInfoToFeedHeader(commentList.Count, PageIndex, PageSize, "");

            //create syndication items for each blog item
            List<SyndicationItem> items = new List<SyndicationItem>();
            int rowNo = 0;
            int startRowNo = PageIndex * PageSize;

            foreach (AWAPI_Data.Data.awBlogPostComment comment in commentList.ToList())
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

                SyndicationItem syndItem = CreateBlogPostCommentSyndicationItem(comment);

                items.Add(syndItem);
                rowNo++;
            }

            _feed.Items = items;

        }

        #endregion

        #region BLOG TAG LIST
        /// <summary>
        /// If postId is empty, it will return the most assigned tags..
        /// else it will return the post's tags
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="postId"></param>
        void GetBlogTagList(long blogId, long postId)
        {
            int size = 20;
            if (PageSize > 0)
                size = PageSize;

            IList<AWAPI_Data.CustomEntities.BlogTagExtended> tagList = null;
            tagList = _blogLib.GetBlogTagListWithPostCount(blogId, postId, true, size);

            if (tagList == null || tagList.Count() == 0)
                return;

            //number of posts will be 100%
            //and each tag will have its rate based on number of posts.. for example
            //if tag a has 20 posts it's ratio will be 0.2
            int maxNumberOfPosts = 0;

            foreach (AWAPI_Data.CustomEntities.BlogTagExtended tg in tagList)
                if (tg.numberOfPosts > maxNumberOfPosts)
                    maxNumberOfPosts = tg.numberOfPosts;

            AddPageInfoToFeedHeader(tagList.Count, 0, PageSize, "");

            //create syndication items for each blog item
            List<SyndicationItem> items = new List<SyndicationItem>();

            foreach (AWAPI_Data.CustomEntities.BlogTagExtended tag in tagList.ToList())
            {
                double tmpValue = 100;
                if (maxNumberOfPosts > 0)
                    tmpValue = tag.numberOfPosts * 100 / maxNumberOfPosts;

                int fontsizefactor = Convert.ToInt32(System.Math.Round(tmpValue));

                SyndicationItem syndItem = CreateBlogTagSyndicationItem(tag, fontsizefactor);

                items.Add(syndItem);
            }

            _feed.Items = items;

        }
        #endregion

        #region CREATE SYNDICATION

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        SyndicationItem CreateBlogCategorySyndicationItem(awBlogCategory cat)
        {
            Uri uri = null;

            SyndicationItem item = new SyndicationItem(
                                cat.title,
                                cat.description,
                                uri,
                                cat.blogCategoryId.ToString(),
                                cat.lastBuildDate.Value);

            item.ElementExtensions.Add("blogcategoryid", null, cat.blogCategoryId);
            if (cat.createDate != null) item.ElementExtensions.Add("createdate", null, cat.createDate);
            return item;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        SyndicationItem CreateBlogSyndicationItem(awBlog blog)
        {
            Uri uri = null;

            
            //SyndicationItem item = new SyndicationItem(
            //                    AWAPI_Common.library.MiscLibrary.AddCDATA(AWAPI_Common.library.MiscLibrary.CropSentence(blog.title, MaximumTitleLength)),
            //                    AWAPI_Common.library.MiscLibrary.AddCDATA(AWAPI_Common.library.MiscLibrary.CropSentence(blog.description, MaximumDescLength)),
            //                    uri,
            //                    blog.blogId.ToString(),
            //                    blog.lastBuildDate.Value);

            SyndicationItem item = new SyndicationItem();
            item.Title = new TextSyndicationContent(AWAPI_Common.library.MiscLibrary.CropSentence(blog.title, MaximumTitleLength), TextSyndicationContentKind.Html);
            item.Content = new TextSyndicationContent(AWAPI_Common.library.MiscLibrary.CropSentence(blog.description, MaximumDescLength), TextSyndicationContentKind.Html);
            item.LastUpdatedTime = blog.lastBuildDate.Value;
            item.BaseUri = uri;
            item.Id = blog.blogId.ToString();



            item.ElementExtensions.Add("blogid", null, blog.blogId);
            item.ElementExtensions.Add("alias", null, blog.alias);
            if (blog.createDate != null) item.ElementExtensions.Add("createdate", null, blog.createDate);
            if (blog.imageurl != null) item.ElementExtensions.Add("imageurl", null, blog.imageurl);

            item.ElementExtensions.Add("siteid", null, blog.siteId);
            item.ElementExtensions.Add("userid", null, blog.userId);

            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        SyndicationItem CreateBlogPostSyndicationItem(AWAPI_Data.CustomEntities.BlogPostExtended blogPost, string pageUrlBase)
        {
            Uri uri = null;
            if (!String.IsNullOrEmpty(pageUrlBase))
                uri = new Uri(pageUrlBase + "?blogid=" + BlogId + "&postid=" + blogPost.blogPostId.ToString());

            string summary = String.IsNullOrEmpty(blogPost.summary) ?
                    AWAPI_Common.library.MiscLibrary.EncodeHtml(AWAPI_Common.library.MiscLibrary.CropSentence(blogPost.description, 200)) :
                    blogPost.summary;

            SyndicationItem item = new SyndicationItem();
            item.Title = new TextSyndicationContent(AWAPI_Common.library.MiscLibrary.CropSentence(blogPost.title, MaximumTitleLength), TextSyndicationContentKind.Html);
            item.Content = new TextSyndicationContent(AWAPI_Common.library.MiscLibrary.CropSentence(blogPost.description, MaximumDescLength), TextSyndicationContentKind.Html);
            item.LastUpdatedTime = blogPost.lastBuildDate.Value;
            item.BaseUri = uri;
            item.Id = blogPost.blogPostId.ToString();
            
                //new SyndicationItem(
                //                AWAPI_Common.library.MiscLibrary.AddCDATA(AWAPI_Common.library.MiscLibrary.CropSentence(blogPost.title, MaximumTitleLength)),
                //                AWAPI_Common.library.MiscLibrary.AddCDATA(AWAPI_Common.library.MiscLibrary.CropSentence(blogPost.description, MaximumDescLength)),
                //                uri,
                //                blogPost.blogPostId.ToString(),
                //                blogPost.lastBuildDate.Value);
            
            
            //new TextSyndicationContent("<b>Item Content</b>",TextSyndicationContentKind.Html


            item.ElementExtensions.Add("blogalias", null, blogPost.Blog.alias);
            item.ElementExtensions.Add("blogpostid", null, blogPost.blogPostId);
            item.ElementExtensions.Add("pubDate", null, blogPost.pubDate.Value);
            if (blogPost.geoTag != null) item.ElementExtensions.Add("geotag", null, blogPost.geoTag);
            item.ElementExtensions.Add("recommended", null, blogPost.recommended);
            item.ElementExtensions.Add("iscommentable", null, blogPost.isCommentable);
            item.ElementExtensions.Add("summary", null, summary);

            item.ElementExtensions.Add("userid", null, blogPost.userId);
            item.ElementExtensions.Add("username", null, blogPost.username);
            item.ElementExtensions.Add("useremail", null, blogPost.userEmail);
            item.ElementExtensions.Add("userfirstname", null, blogPost.userFirstName);
            item.ElementExtensions.Add("userlastname", null, blogPost.userLastName);
            if (blogPost.userImageUrl != null) item.ElementExtensions.Add("userimageurl", null, blogPost.userImageUrl);
            item.ElementExtensions.Add("numberofcomments", null, blogPost.numberOfComments);

            if (blogPost.createDate != null) item.ElementExtensions.Add("createdate", null, blogPost.createDate);
            if (blogPost.imageurl != null) item.ElementExtensions.Add("imageurl", null, blogPost.imageurl);

            if (blogPost.tags != null && blogPost.tags.Count() > 0)
            {
                IList<awBlogTag> tagList = blogPost.tags.ToList<awBlogTag>();
                foreach (awBlogTag tg in tagList)
                {
                    SyndicationCategory cat = new SyndicationCategory();
                    cat.AttributeExtensions.Add(new XmlQualifiedName("name"), "tags");
                    cat.AttributeExtensions.Add(new XmlQualifiedName("tagid"), tg.blogTagId.ToString());
                    cat.AttributeExtensions.Add(new XmlQualifiedName("tag"), tg.title.ToString());
                    item.Categories.Add(cat);
                }
            }

            return item;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        SyndicationItem CreateBlogPostFileSyndicationItem(AWAPI_Data.CustomEntities.BlogPostExtended blogPost, AWAPI_Data.Data.awFile_Blog file, string pageUrlBase)
        {
            Uri uri = null;
            if (!String.IsNullOrEmpty(pageUrlBase))
                uri = new Uri(pageUrlBase + "?blogid=" + BlogId + "&postid=" + blogPost.blogPostId.ToString());

            //SyndicationItem item = new SyndicationItem(
            //                    AWAPI_Common.library.MiscLibrary.AddCDATA(AWAPI_Common.library.MiscLibrary.CropSentence(file.title, MaximumTitleLength)),
            //                    AWAPI_Common.library.MiscLibrary.AddCDATA(AWAPI_Common.library.MiscLibrary.CropSentence(file.description, MaximumDescLength)),
            //                    uri,
            //                    file.fileId.ToString(),
            //                    file.lastBuildDate.Value);

            SyndicationItem item = new SyndicationItem();
            item.Title = new TextSyndicationContent(AWAPI_Common.library.MiscLibrary.CropSentence(file.title, MaximumTitleLength), TextSyndicationContentKind.Html);
            item.Content = new TextSyndicationContent(AWAPI_Common.library.MiscLibrary.CropSentence(file.description, MaximumDescLength), TextSyndicationContentKind.Html);
            item.LastUpdatedTime = file.lastBuildDate.Value;
            item.BaseUri = uri;
            item.Id = file.fileId.ToString();

            item.ElementExtensions.Add("fileurl", null, AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileServiceUrl + "?id=" + file.fileId.ToString());
            item.ElementExtensions.Add("contenttype", null, file.contentType);
            if (file.isOnLocal)
                item.ElementExtensions.Add("fileoriginalurl", null, AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileServiceUrl + "?id=" + file.fileId.ToString());
            else
                item.ElementExtensions.Add("fileoriginalurl", null, file.path);

            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="archive"></param>
        /// <returns></returns>
        SyndicationItem CreateBlogPostArchiveSyndicationItem(AWAPI_Data.CustomEntities.BlogPostArchive archive)
        {
            Uri uri = null;
            string monthName = System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(archive.month);

            SyndicationItem item = new SyndicationItem(
                    String.Format("{0} {1} ({2})", monthName, archive.year, archive.numberOfPosts),
                    "",
                    uri,
                    String.Format("{0}-{1}", archive.year, archive.month),
                    DateTime.Now);

            item.ElementExtensions.Add("month", null, archive.month);
            item.ElementExtensions.Add("monthname", null, monthName);
            item.ElementExtensions.Add("year", null, archive.year);

            return item;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        SyndicationItem CreateBlogPostCommentSyndicationItem(AWAPI_Data.Data.awBlogPostComment comment)
        {
            Uri uri = null;
            string pageUrlBase = GetPostPageBaseUrl(comment.awBlogPost.awBlog.blogPostPage);
            if (!String.IsNullOrEmpty(pageUrlBase))
                uri = new Uri(pageUrlBase + "?blogid=" + BlogId + "&postid=" + comment.blogPostId.ToString());

            //SyndicationItem item = new SyndicationItem(
            //                    AWAPI_Common.library.MiscLibrary.AddCDATA(AWAPI_Common.library.MiscLibrary.CropSentence(comment.title, MaximumTitleLength)),
            //                    AWAPI_Common.library.MiscLibrary.AddCDATA(AWAPI_Common.library.MiscLibrary.CropSentence(comment.description, MaximumDescLength)),
            //                    uri,
            //                    comment.blogCommentID.ToString(),
            //                    comment.lastBuildDate.Value);

            SyndicationItem item = new SyndicationItem();
            item.Title = new TextSyndicationContent(AWAPI_Common.library.MiscLibrary.CropSentence(comment.title, MaximumTitleLength), TextSyndicationContentKind.Html);
            item.Content = new TextSyndicationContent(AWAPI_Common.library.MiscLibrary.CropSentence(comment.description, MaximumDescLength), TextSyndicationContentKind.Html);
            item.LastUpdatedTime = comment.lastBuildDate.Value;
            item.BaseUri = uri;
            item.Id = comment.blogCommentID.ToString();

            item.ElementExtensions.Add("blogpostcommentid", null, comment.blogCommentID);
            item.ElementExtensions.Add("blogpostid", null, comment.blogPostId);
            item.ElementExtensions.Add("blogposttitle", null, comment.awBlogPost.title);
            item.ElementExtensions.Add("blogalias", null, comment.awBlogPost.awBlog.alias);

            //            if (tag.email != null) item.ElementExtensions.Add("email", null, tag.email);
            if (comment.userId != null) item.ElementExtensions.Add("userid", null, comment.userId);
            if (comment.firstName != null) item.ElementExtensions.Add("userfirstname", null, comment.firstName);
            if (comment.lastName != null) item.ElementExtensions.Add("userlastname", null, comment.lastName);
            if (comment.userName != null) item.ElementExtensions.Add("username", null, comment.userName);

            if (comment.createDate != null) item.ElementExtensions.Add("createdate", null, comment.createDate);

            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="percentage"></param>
        /// <returns></returns>
        SyndicationItem CreateBlogTagSyndicationItem(AWAPI_Data.CustomEntities.BlogTagExtended tag, int fontsizefactor)
        {
            Uri uri = null;

            SyndicationItem item = new SyndicationItem(
                                AWAPI_Common.library.MiscLibrary.CropSentence(tag.title, MaximumTitleLength),
                                "",
                                uri,
                                tag.blogTagId.ToString(), tag.lastBuildDate.Value);

            item.ElementExtensions.Add("tagid", null, tag.blogTagId);
            item.ElementExtensions.Add("numberofposts", null, tag.numberOfPosts);
            item.ElementExtensions.Add("fontsizefactor", null, fontsizefactor);
            if (tag.createDate != null) item.ElementExtensions.Add("createdate", null, tag.createDate);

            return item;
        }

        #endregion

        #region helper methods

        void AddPageInfoToFeedHeader(int numberOfRecords, int pageIndex, int pageSize, string search)
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

            _feed.AttributeExtensions.Add(new XmlQualifiedName("search"), search);
        }

        /// <summary>
        /// Sets _blogPageUrl value
        /// </summary>
        string GetPostPageBaseUrl(string blogPostPage)
        {
            string rtn = "";

            if (!String.IsNullOrEmpty(_site.link))
            {
                rtn = _site.link;
                if (!rtn.EndsWith("/")) rtn += "/";

                if (!String.IsNullOrEmpty(blogPostPage))
                {
                    if (blogPostPage.StartsWith("/"))
                        rtn += blogPostPage.Substring(1, blogPostPage.Length - 1);
                    else
                        rtn += blogPostPage;
                }
            }
            return rtn;
        }
        #endregion


    }
}
