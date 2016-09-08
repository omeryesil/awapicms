using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Linq.SqlClient;

using AWAPI_Data.Data;
using AWAPI_Common.library;

namespace AWAPI_BusinessLibrary.library
{
    public class BlogLibrary
    {
        BlogContextDataContext _context = new BlogContextDataContext();

        #region ENUMS
        public enum Comment_Status
        {
            Pending = 0,
            Approved = 1,
            Rejected = 2

        }
        #endregion

        #region BLOG ---
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public awBlog GetBlog(long blogId)
        {
            if (blogId <= 0)
                return null;
            var blog = _context.GetTable<awBlog>()
                        .Where(st => st.blogId.Equals(blogId));

            if (blog == null && blog.ToList().Count() == 0)
                return null;
            return blog.First();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<awBlog> GetBlogList(long siteId, string where)
        {
            StringBuilder sb = new StringBuilder(" SELECT * FROM awBlog ");
            StringBuilder sbWhere = new StringBuilder(" WHERE siteId=" + siteId);

            if (where.Trim().Length > 0)
                sbWhere.Append(" AND (" + where + ") ");

            sb.Append(sbWhere.ToString());
            sb.Append(" ORDER BY title");

            var contents = _context.ExecuteQuery<awBlog>(sb.ToString());
            if (contents == null)
                return null;

            return contents.ToList<awBlog>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogId"></param>
        public void DeleteBlog(long blogId)
        {
            if (blogId <= 0)
                return;

            //TAGS ---------------------------------------------------------------
            //Delete post tag relations
            var postTags = from c in _context.awBlogTagPosts
                           where c.awBlogTag.awBlog.blogId.Equals(blogId)
                           select c;
            _context.awBlogTagPosts.DeleteAllOnSubmit(postTags);

            //Delete tags
            var tags = from c in _context.awBlogTags
                       where c.blogId.Equals(blogId)
                       select c;
            _context.awBlogTags.DeleteAllOnSubmit(tags);

            //CATEGORIES ---------------------------------------------------------------
            //Delete post category relations
            var postCats = from c in _context.awBlogCategoryPosts
                           where c.awBlogCategory.awBlog.blogId.Equals(blogId)
                           select c;
            _context.awBlogCategoryPosts.DeleteAllOnSubmit(postCats);

            //Delete tags
            var cats = from c in _context.awBlogCategories
                       where c.blogId.Equals(blogId)
                       select c;
            _context.awBlogCategories.DeleteAllOnSubmit(cats);

            //DELETE POST COMMENTS ---------------------------------------------------------------
            var comments = from c in _context.awBlogPostComments
                           where c.awBlogPost.awBlog.blogId.Equals(blogId)
                           select c;
            _context.awBlogPostComments.DeleteAllOnSubmit(comments);

            //DELETE BLOG POST FILES -------------------------------------------------------------
            var blogPostFiles = from l in _context.awBlogPostFiles
                                where l.awBlogPost.blogId.Equals(blogId)
                                select l;
            _context.awBlogPostFiles.DeleteAllOnSubmit(blogPostFiles);

            //DELETE POSTS ---------------------------------------------------------------
            var posts = from p in _context.awBlogPosts
                        where p.blogId.Equals(blogId)
                        select p;
            _context.awBlogPosts.DeleteAllOnSubmit(posts);

            //DELETE BLOG  ---------------------------------------------------------------
            var blogs = from b in _context.awBlogs
                        where b.blogId.Equals(blogId)
                        select b;
            _context.awBlogs.DeleteAllOnSubmit(blogs);

            //SUBMIT CHANGES -------------------------------------------------------------
            _context.SubmitChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        public long AddBlog(long siteId, long userId,
                        string alias, string title, string description, string imageUrl,
                         bool isEnabled,
                        bool enableCommentEmailNotifier, string commentEmailTo, long? commentEmailTemplateId, string blogPostPage)
        {
            if (BlogAliasAlreadyExist(0, alias))
                throw new Exception("Alias was already defined for another blog.");

            long id = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            awBlog blog = new awBlog();

            blog.blogId = id;
            blog.title = title;
            blog.alias = alias;
            blog.description = description;
            blog.imageurl = imageUrl;

            blog.siteId = siteId;
            blog.userId = userId;
            blog.isEnabled = isEnabled;

            blog.blogPostPage = blogPostPage;

            blog.enableCommentEmailNotifier = enableCommentEmailNotifier;
            blog.commentEmailTo = commentEmailTo;
            blog.commentEmailTemplateId = commentEmailTemplateId;

            blog.lastBuildDate = DateTime.Now;
            blog.createDate = DateTime.Now;

            _context.awBlogs.InsertOnSubmit(blog);
            _context.SubmitChanges();

            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        public bool UpdateBlog(long blogId, string alias, string title, string description, bool isEnabled, string imageUrl,
                            bool enableCommentEmailNotifier, string commentEmailTo, long? commentEmailTemplateId, string blogPostPage)
        {
            awBlog blog = _context.awBlogs.FirstOrDefault(st => st.blogId.Equals(blogId));

            if (blog == null)
                return false;

            if (BlogAliasAlreadyExist(blogId, alias))
                throw new Exception("Alias was already defined for another blog.");

            blog.alias = alias;
            blog.title = title;
            blog.description = description;
            blog.imageurl = imageUrl;
            blog.isEnabled = isEnabled;
            blog.blogPostPage = blogPostPage;

            blog.enableCommentEmailNotifier = enableCommentEmailNotifier;
            blog.commentEmailTo = commentEmailTo;
            blog.commentEmailTemplateId = commentEmailTemplateId;

            blog.lastBuildDate = DateTime.Now;

            _context.SubmitChanges();

            return true;
        }

        /// <summary>
        /// Checks if blog alias is already defined under another blog
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public bool BlogAliasAlreadyExist(long blogId, string alias)
        {
            var list = from l in _context.awBlogs
                       where l.alias.Equals(alias.ToLower()) && l.blogId != blogId
                       select l;

            if (list == null || list.Count() == 0)
                return false;

            return true;
        }

        #endregion

        #region BLOG POSTS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogPostCommentId"></param>
        /// <returns></returns>
        public AWAPI_Data.CustomEntities.BlogPostExtended GetBlogPost(long blogPostId)
        {
            if (blogPostId <= 0)
                return null;
            var blogPost = from p in _context.awBlogPosts
                           where p.blogPostId.Equals(blogPostId)
                           select new AWAPI_Data.CustomEntities.BlogPostExtended
                           {
                               siteId = p.awBlog.siteId,
                               Blog = p.awBlog,
                               blogPostId = p.blogPostId,
                               blogId = p.blogId,
                               userId = p.userId,
                               authorUserId = p.authorUserId,
                               username = p.awUser_BlogPost.username,   //author username
                               userFirstName = p.awUser_BlogPost.firstName, //author firstname
                               userLastName = p.awUser_BlogPost.lastName,
                               userEmail = p.awUser_BlogPost.email,
                               userImageUrl = p.awUser_BlogPost.imageurl,
                               blogPostPage = p.awBlog.blogPostPage,
                               title = p.title,
                               summary = p.summary,
                               description = p.description,
                               isCommentable = p.isCommentable,
                               isPublished = p.isPublished,
                               imageurl = p.imageurl,
                               recommended = p.recommended,
                               geoTag = p.geoTag,
                               pubDate = p.pubDate,
                               pubEndDate = p.pubEndDate,
                               lastBuildDate = p.lastBuildDate,
                               createDate = p.createDate,
                               numberOfComments = (
                                    from c in p.awBlogPostComments
                                    where c.blogPostId.Equals(p.blogPostId) && c.status.Equals(1)
                                    select c).Count(),
                               tags = (from t in p.awBlogTagPosts
                                       where t.blogPostId.Equals(p.blogPostId)
                                       orderby t.awBlogTag.title
                                       select t.awBlogTag)

                           };


            if (blogPost == null && blogPost.ToList().Count() == 0)
                return null;
            return blogPost.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.BlogPostExtended> GetBlogPostList(long siteId)
        {
            return GetBlogPostList(siteId, false, "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="onlyPublished"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.BlogPostExtended> GetBlogPostList(long siteId, bool onlyPublished, string search)
        {
            DateTime now = DateTime.Now;
            //using a boolean is faster
            bool addSearch = !String.IsNullOrEmpty(search);
            search = "%" + search.Trim() + "%";

            var posts = from p in _context.awBlogPosts
                        where p.awBlog.siteId.Equals(siteId) &&
                        (
                            (!onlyPublished ||
                              (onlyPublished && p.awBlog.isEnabled && p.isPublished &&
                              (p.pubDate.Equals(null) || p.pubDate <= now) &&
                              (p.pubEndDate.Equals(null) || p.pubEndDate > now))
                             )
                             &&
                            (!addSearch ||
                               addSearch && (
                                    SqlMethods.Like(p.title, search) ||
                                    SqlMethods.Like(p.description, search) ||
                                    SqlMethods.Like(p.summary, search) ||
                                    SqlMethods.Like(p.awUser_BlogPost.username, search)
                               )
                            )
                        )
                        orderby p.pubDate descending
                        select new AWAPI_Data.CustomEntities.BlogPostExtended
                        {
                            siteId = p.awBlog.siteId,
                            Blog = p.awBlog,
                            blogPostId = p.blogPostId,
                            blogId = p.blogId,
                            userId = p.userId,
                            authorUserId = p.authorUserId,
                            username = p.awUser_BlogPost.username,
                            userFirstName = p.awUser_BlogPost.firstName,
                            userLastName = p.awUser_BlogPost.lastName,
                            userEmail = p.awUser_BlogPost.email,
                            userImageUrl = p.awUser_BlogPost.imageurl,
                            blogPostPage = p.awBlog.blogPostPage,
                            title = p.title,
                            summary = p.summary,
                            description = p.description,
                            isCommentable = p.isCommentable,
                            isPublished = p.isPublished,
                            imageurl = p.imageurl,
                            recommended = p.recommended,
                            geoTag = p.geoTag,
                            pubDate = p.pubDate,
                            pubEndDate = p.pubEndDate,
                            lastBuildDate = p.lastBuildDate,
                            createDate = p.createDate,
                            numberOfComments = (
                                    from c in p.awBlogPostComments
                                    where c.blogPostId.Equals(p.blogPostId) && c.status.Equals(1)
                                    select c).Count(),
                            tags = (from t in p.awBlogTagPosts
                                    where t.blogPostId.Equals(p.blogPostId)
                                    orderby t.awBlogTag.title
                                    select t.awBlogTag)
                        };

            if (posts == null || posts.Count() == 0)
                return null;

            return posts.ToList<AWAPI_Data.CustomEntities.BlogPostExtended>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.BlogPostExtended> GetBlogPostsByBlogId(long blogId)
        {
            return GetPostListByBlogId(blogId, false, "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="onlyPublished"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.BlogPostExtended> GetPostListByBlogId(long blogId, bool onlyPublished, string search)
        {
            DateTime now = DateTime.Now;

            //using a boolean is faster
            bool addSearch = !String.IsNullOrEmpty(search);
            search = "%" + search.Trim() + "%";

            var posts = from cp in _context.awBlogPosts
                        where cp.blogId.Equals(blogId) &&
                        (
                            ((onlyPublished && cp.awBlog.isEnabled && cp.isPublished &&
                               (cp.pubDate.Equals(null) || cp.pubDate <= now) &&
                               (cp.pubEndDate.Equals(null) || cp.pubEndDate > now)
                            ) || !onlyPublished)
                            &&
                            (!addSearch ||
                               addSearch && (
                                    SqlMethods.Like(cp.title, search) ||
                                    SqlMethods.Like(cp.description, search) ||
                                    SqlMethods.Like(cp.summary, search) ||
                                    SqlMethods.Like(cp.awUser_BlogPost.username, search)
                               )
                            )
                        )
                        orderby cp.pubDate descending
                        select new AWAPI_Data.CustomEntities.BlogPostExtended
                        {
                            siteId = cp.awBlog.siteId,
                            Blog = cp.awBlog,
                            blogPostId = cp.blogPostId,
                            blogId = cp.blogId,
                            userId = cp.userId,
                            authorUserId = cp.authorUserId,
                            username = cp.awUser_BlogPost.username,
                            userFirstName = cp.awUser_BlogPost.firstName,
                            userLastName = cp.awUser_BlogPost.lastName,
                            userEmail = cp.awUser_BlogPost.email,
                            userImageUrl = cp.awUser_BlogPost.imageurl,
                            blogPostPage = cp.awBlog.blogPostPage,
                            title = cp.title,
                            summary = cp.summary,
                            description = cp.description,
                            isCommentable = cp.isCommentable,
                            isPublished = cp.isPublished,
                            imageurl = cp.imageurl,
                            recommended = cp.recommended,
                            geoTag = cp.geoTag,
                            pubDate = cp.pubDate,
                            pubEndDate = cp.pubEndDate,
                            lastBuildDate = cp.lastBuildDate,
                            createDate = cp.createDate,
                            numberOfComments = (
                                    from c in cp.awBlogPostComments
                                    where c.blogPostId.Equals(cp.blogPostId) && c.status.Equals(1)
                                    select c).Count(),
                            tags = (from t in cp.awBlogTagPosts
                                    where t.blogPostId.Equals(cp.blogPostId)
                                    orderby t.awBlogTag.title
                                    select t.awBlogTag)
                        };
            if (posts == null || posts.Count() == 0)
                return null;

            return posts.ToList<AWAPI_Data.CustomEntities.BlogPostExtended>();
        }

        /// <summary>
        /// Returns list of the posts based on publishing date and publishing month
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="onlyPublished"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.BlogPostExtended> GetPostListByArchiveDate(long blogId, bool onlyPublished, int year, int month)
        {
            DateTime now = DateTime.Now;

            //using a boolean is faster

            var posts = from cp in _context.awBlogPosts
                        where cp.blogId.Equals(blogId) &&
                        (
                            ((onlyPublished && cp.awBlog.isEnabled && cp.isPublished &&
                              (cp.pubDate.Equals(null) || cp.pubDate <= now) &&
                              (cp.pubEndDate.Equals(null) || cp.pubEndDate > now)

                            ) || !onlyPublished) &&
                            cp.pubDate.Value.Month.Equals(month) &&
                            cp.pubDate.Value.Year.Equals(year)
                        )
                        orderby cp.pubDate descending
                        select new AWAPI_Data.CustomEntities.BlogPostExtended
                        {
                            siteId = cp.awBlog.siteId,
                            Blog = cp.awBlog,
                            blogPostId = cp.blogPostId,
                            blogId = cp.blogId,
                            userId = cp.userId,
                            authorUserId = cp.authorUserId,
                            username = cp.awUser_BlogPost.username,
                            userFirstName = cp.awUser_BlogPost.firstName,
                            userLastName = cp.awUser_BlogPost.lastName,
                            userEmail = cp.awUser_BlogPost.email,
                            userImageUrl = cp.awUser_BlogPost.imageurl,
                            blogPostPage = cp.awBlog.blogPostPage,
                            title = cp.title,
                            summary = cp.summary,
                            description = cp.description,
                            isCommentable = cp.isCommentable,
                            isPublished = cp.isPublished,
                            imageurl = cp.imageurl,
                            recommended = cp.recommended,
                            geoTag = cp.geoTag,
                            pubDate = cp.pubDate,
                            pubEndDate = cp.pubEndDate,
                            lastBuildDate = cp.lastBuildDate,
                            createDate = cp.createDate,
                            numberOfComments = (
                                    from c in cp.awBlogPostComments
                                    where c.blogPostId.Equals(cp.blogPostId) && c.status.Equals(1)
                                    select c).Count(),
                            tags = (from t in cp.awBlogTagPosts
                                    where t.blogPostId.Equals(cp.blogPostId)
                                    orderby t.awBlogTag.title
                                    select t.awBlogTag)
                        };
            if (posts == null || posts.Count() == 0)
                return null;

            return posts.ToList<AWAPI_Data.CustomEntities.BlogPostExtended>();
        }




        /// <summary>
        /// Groups the blog posts in 
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="onlyPublished"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.BlogPostArchive> GetPostArchiveList(long blogId, bool onlyPublished)
        {
            DateTime now = DateTime.Now;

            //using a boolean is faster
            var list = from p in _context.awBlogPosts
                       where p.blogId.Equals(blogId) &&
                       (
                           (
                            onlyPublished &&
                            p.awBlog.isEnabled && p.isPublished &&
                            (p.pubDate.Equals(null) || p.pubDate <= now) &&
                            (p.pubEndDate.Equals(null) || p.pubEndDate > now)
                           ) || !onlyPublished
                       )
                       group p by new
                       {
                           blogId = p.blogId,
                           monthName = "", //p.pubDate.Value.ToString("MMMM"),
                           month = p.pubDate.Value.Month,
                           year = p.pubDate.Value.Year
                       }
                           into d
                           orderby d.Key.year descending, d.Key.month descending
                           select new AWAPI_Data.CustomEntities.BlogPostArchive
                           {
                               blogId = d.Key.blogId,
                               month = d.Key.month,
                               year = d.Key.year,
                               numberOfPosts = d.Count(),
                           };

            if (list == null)
                return null;

            return list.ToList();

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogPostCommentId"></param>
        /// <param name="isPublished"></param>
        /// <returns></returns>
        public bool UpdateBlogPostPublishStatus(long blogPostId, bool isPublished)
        {
            awBlogPost post = _context.awBlogPosts.FirstOrDefault(st => st.blogPostId.Equals(blogPostId));

            if (post == null)
                return false;

            post.isPublished = isPublished;
            post.lastBuildDate = DateTime.Now;

            _context.SubmitChanges();

            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogPostCommentId"></param>
        public void DeleteBlogPost(long blogPostId)
        {
            if (blogPostId <= 0)
                return;

            //Delete tags
            var postTags = from c in _context.awBlogTagPosts
                           where c.blogPostId.Equals(blogPostId)
                           select c;
            _context.awBlogTagPosts.DeleteAllOnSubmit(postTags);


            //Delete tags
            var postCats = from c in _context.awBlogCategoryPosts
                           where c.blogPostId.Equals(blogPostId)
                           select c;
            _context.awBlogCategoryPosts.DeleteAllOnSubmit(postCats);


            //Delete posts
            var comments = from c in _context.awBlogPostComments
                           where c.blogPostId.Equals(blogPostId)
                           select c;
            _context.awBlogPostComments.DeleteAllOnSubmit(comments);

            //Delete blog post files 
            var blogPostFiles = from l in _context.awBlogPostFiles
                                where l.blogPostId.Equals(blogPostId)
                                select l;
            _context.awBlogPostFiles.DeleteAllOnSubmit(blogPostFiles);

            //Delete posts
            var post = from p in _context.awBlogPosts
                       where p.blogPostId.Equals(blogPostId)
                       select p;
            _context.awBlogPosts.DeleteAllOnSubmit(post);

            _context.SubmitChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="summary"></param>
        /// <param name="imageurl"></param>
        /// <param name="geoTag"></param>
        /// <param name="isPublished"></param>
        /// <param name="isCommentable"></param>
        /// <param name="pubDate"></param>
        /// <param name="pubEndDate"></param>
        /// <returns></returns>
        public long AddBlogPost(long blogId, long userId, long? authorUserId,
                        string title, string description, string summary,
                        string imageurl, string geoTag, bool isPublished, bool isCommentable,
                        DateTime? pubDate, DateTime? pubEndDate)
        {
            long id = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            awBlogPost blogPost = new awBlogPost();

            blogPost.blogPostId = id;
            blogPost.authorUserId = authorUserId;
            blogPost.title = title;
            blogPost.description = description;
            blogPost.summary = summary;
            blogPost.geoTag = geoTag;
            blogPost.recommended = 0;
            blogPost.userId = userId;
            blogPost.blogId = blogId;

            blogPost.imageurl = imageurl;
            blogPost.isPublished = isPublished;
            blogPost.isCommentable = isCommentable;

            blogPost.lastBuildDate = DateTime.Now;

            if (pubDate == null)
                blogPost.pubDate = DateTime.Now;
            else
                blogPost.pubDate = pubDate;
            blogPost.pubEndDate = pubEndDate;
            blogPost.createDate = DateTime.Now;

            _context.awBlogPosts.InsertOnSubmit(blogPost);
            _context.SubmitChanges();

            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogPostCommentId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="summary"></param>
        /// <param name="imageurl"></param>
        /// <param name="geoTag"></param>
        /// <param name="isPublished"></param>
        /// <param name="isCommentable"></param>
        /// <param name="pubDate"></param>
        /// <param name="pubEndDate"></param>
        /// <returns></returns>
        public bool UpdateBlogPost(long blogPostId, long userId, long? authorUserId, string title, string description, string summary,
                        string imageurl, string geoTag, bool isPublished, bool isCommentable,
                        DateTime? pubDate, DateTime? pubEndDate)
        {
            awBlogPost blogPost = _context.awBlogPosts.FirstOrDefault(st => st.blogPostId.Equals(blogPostId));

            if (blogPost == null)
                return false;

            blogPost.authorUserId = authorUserId;
            blogPost.userId = userId;
            blogPost.title = title;
            blogPost.description = description;
            blogPost.summary = summary;
            blogPost.geoTag = geoTag;
            blogPost.imageurl = imageurl;
            blogPost.isPublished = isPublished;
            blogPost.isCommentable = isCommentable;

            blogPost.lastBuildDate = DateTime.Now;

            if (pubDate == null)
                blogPost.pubDate = DateTime.Now;
            else
                blogPost.pubDate = pubDate;
            blogPost.pubEndDate = pubEndDate;

            _context.SubmitChanges();

            return true;
        }

        #endregion

        #region BLOG POST COMMENTS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogPostCommentId"></param>
        /// <returns></returns>
        public awBlogPostComment GetBlogPostComment(long blogPostCommentId)
        {
            if (blogPostCommentId <= 0)
                return null;
            var blogComment = _context.GetTable<awBlogPostComment>()
                        .Where(st => st.blogCommentID.Equals(blogPostCommentId));

            if (blogComment == null && blogComment.ToList().Count() == 0)
                return null;
            return blogComment.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<awBlogPostComment> GetBlogPostCommentListBySiteId(long siteId)
        {
            return GetBlogPostCommentListBySiteId(siteId, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="onlyPublished"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<awBlogPostComment> GetBlogPostCommentListBySiteId(long siteId, bool onlyPublished)
        {
            var comments = from p in _context.awBlogPostComments
                           where p.awBlogPost.awBlog.siteId.Equals(siteId) &&
                           (
                               (onlyPublished &&
                                   p.awBlogPost.awBlog.isEnabled &&
                                   p.awBlogPost.isPublished &&
                                   p.status.Equals(Comment_Status.Approved)) || //if onlyPublished true
                               !onlyPublished  //else 
                           )
                           orderby p.pubDate descending
                           select p;
            if (comments == null || comments.Count() == 0)
                return null;

            return comments.ToList<awBlogPostComment>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<awBlogPostComment> GetBlogPostCommentListByBlogId(long blogId)
        {
            return GetBlogPostCommentListByBlogId(blogId, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="onlyPublished"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<awBlogPostComment> GetBlogPostCommentListByBlogId(long blogId, bool onlyPublished)
        {
            var comments = from p in _context.awBlogPostComments
                           where p.awBlogPost.blogId.Equals(blogId) &&
                           (
                               (onlyPublished &&
                                   p.awBlogPost.awBlog.isEnabled &&
                                   p.awBlogPost.isPublished &&
                                   p.status.Equals(Comment_Status.Approved)) || //if onlyPublished true
                               !onlyPublished  //else 
                           )
                           orderby p.pubDate descending
                           select p;
            if (comments == null || comments.Count() == 0)
                return null;

            return comments.ToList<awBlogPostComment>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<awBlogPostComment> GetBlogPostCommentListByPostId(long postId)
        {
            return GetBlogPostCommentListByPostId(postId, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="onlyPublished"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<awBlogPostComment> GetBlogPostCommentListByPostId(long postId, bool onlyPublished)
        {
            var comments = from p in _context.awBlogPostComments
                           where p.blogPostId.Equals(postId) &&
                           (
                               (onlyPublished &&
                                   p.awBlogPost.awBlog.isEnabled &&
                                   p.awBlogPost.isPublished &&
                                   p.status.Equals(Comment_Status.Approved)) || //if onlyPublished true
                               !onlyPublished  //else 
                           )
                           orderby p.pubDate descending
                           select p;
            if (comments == null || comments.Count() == 0)
                return null;

            return comments.ToList<awBlogPostComment>();
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="blogPostCommentId"></param>
        ///// <param name="status"></param>
        ///// <returns></returns>
        //public bool UpdateBlogPostCommentStatus(long blogPostCommentId, Comment_Status status)
        //{
        //    return UpdateBlogPostCommentStatus(blogPostCommentId, (int)status);
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogPostCommentId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateBlogPostCommentStatus(long blogPostCommentId, int status)
        {
            awBlogPostComment comment = _context.awBlogPostComments.FirstOrDefault(st => st.blogCommentID.Equals(blogPostCommentId));

            if (comment == null)
                return false;

            comment.status = status;

            comment.lastBuildDate = DateTime.Now;

            _context.SubmitChanges();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogPostCommentId"></param>
        public void DeleteBlogPostComment(long blogPostCommentId)
        {
            if (blogPostCommentId <= 0)
                return;

            awBlogPostComment comment = GetBlogPostComment(blogPostCommentId);
            if (comment == null)
                return;

            _context.awBlogPostComments.DeleteOnSubmit(comment);
            _context.SubmitChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogPostId"></param>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="email"></param>
        /// <param name="userName"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public long AddBlogPostComment(long blogPostId, long? userId,
                        string title, string description, string email,
                        string userName, string firstName, string lastName, int status)
        {
            long id = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            awBlogPostComment comment = new awBlogPostComment();

            comment.blogCommentID = id;
            comment.blogPostId = blogPostId;
            if (!String.IsNullOrEmpty(title))
                comment.title = title;
            else
            {
                int len = 20;
                if (description.Trim().Length < len)
                    len = description.Length;
                comment.title = description.Substring(0, len);
            }

            comment.description = description;
            comment.userName = String.IsNullOrEmpty(userName) ? firstName.Trim() + "_" + lastName.Trim() : userName;
            comment.firstName = firstName;
            comment.lastName = lastName;
            comment.email = email;
            comment.userId = userId;
            comment.status = status;

            comment.lastBuildDate = DateTime.Now;
            comment.pubDate = DateTime.Now;
            comment.createDate = DateTime.Now;

            _context.awBlogPostComments.InsertOnSubmit(comment);
            _context.SubmitChanges();

            //Sent email if it is enabled
            if (comment.awBlogPost.awBlog.enableCommentEmailNotifier &&
                !String.IsNullOrEmpty(comment.awBlogPost.awBlog.commentEmailTo) &&
                comment.awBlogPost.awBlog.commentEmailTemplateId != null)
            {
                AWAPI_BusinessLibrary.library.AutomatedTaskLibrary taskLib = new AutomatedTaskLibrary();

                //CREATE TASK LINKS FOR Approve and Reject ----------------------------------
                long automatedTaskGroupId = AWAPI_Common.library.MiscLibrary.CreateUniqueId();

                //Create approve task
                Guid approveTaskGuid = Guid.NewGuid();
                string approveLink = ConfigurationLibrary.Config.automatedTaskServiceUrl + "?taskid=" + approveTaskGuid.ToString();
                taskLib.Add(comment.awBlogPost.awBlog.siteId, automatedTaskGroupId, approveTaskGuid,
                            "Approve Comment", "Approve blog post comment.",
                            true, "Comment has been approved.",
                            "AWAPI_BusinessLibrary.library.BlogLibrary", "UpdateBlogPostCommentStatus",
                            String.Format("int64:{0}|int:{1}", comment.blogCommentID, (int)Comment_Status.Approved), "");

                //Create reject task
                Guid rejectTaskGuid = Guid.NewGuid();
                string rejectLink = ConfigurationLibrary.Config.automatedTaskServiceUrl + "?taskid=" + rejectTaskGuid.ToString();
                taskLib.Add(comment.awBlogPost.awBlog.siteId, automatedTaskGroupId, rejectTaskGuid,
                            "Reject Comment", "Reject blog post comment.",
                            true, "Comment has been rejected.",
                            "AWAPI_BusinessLibrary.library.BlogLibrary", "UpdateBlogPostCommentStatus",
                            String.Format("int64:{0}|int:{1}", comment.blogCommentID, (int)Comment_Status.Rejected), "");


                //Create link to moderate the comment throug the admin panel ---------------------
                string baseUrl = System.Configuration.ConfigurationManager.AppSettings["AWAPI_Admin_Base_Url"];
                if (baseUrl != null && !baseUrl.EndsWith("/"))
                    baseUrl += "/";
                string moderatorLink = baseUrl + "admin/blogpostdetail.aspx" + "?postid=" + blogPostId + "&commentid=" + comment.blogCommentID;


                AWAPI_BusinessLibrary.library.EmailTemplateLib emailLib = new EmailTemplateLib();
                emailLib.Send(comment.awBlogPost.awBlog.commentEmailTemplateId.Value,
                                comment.awBlogPost.awBlog.commentEmailTo,
                                "commenter|" + comment.userName,
                                "commenteremail|" + comment.email,
                                "comment|" + comment.description,
                                "moderatelink|" + moderatorLink,
                                "approvelink|" + approveLink,
                                "rejectlink|" + rejectLink,
                                "date|" + DateTime.Now.ToString());

            }

            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogPostCommentId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateBlogPostComment(long blogPostCommentId,
                                    string title, string description, string email,
                                    string userName, string firstName, string lastName, int status)
        {
            awBlogPostComment comment = _context.awBlogPostComments.FirstOrDefault(st => st.blogCommentID.Equals(blogPostCommentId));

            if (comment == null)
                return false;

            comment.title = title;
            comment.description = description;
            comment.firstName = firstName;
            comment.lastName = lastName;
            comment.userName = userName;
            comment.email = email;
            comment.status = status;

            comment.lastBuildDate = DateTime.Now;
            _context.SubmitChanges();

            return true;
        }


        #endregion

        #region BLOG CATEGORIES

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogCategoryId"></param>
        /// <returns></returns>
        public awBlogCategory GetBlogCategory(long blogCategoryId)
        {
            if (blogCategoryId <= 0)
                return null;

            var blogTag = _context.GetTable<awBlogCategory>()
                        .Where(st => st.blogCategoryId.Equals(blogCategoryId));

            if (blogTag == null && blogTag.ToList().Count() == 0)
                return null;
            return blogTag.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="onlyEnabled"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<awBlogCategory> GetBlogCategoryList(long blogId, bool onlyEnabled)
        {
            var categories = from p in _context.awBlogCategories
                             where p.awBlog.blogId.Equals(blogId) &&
                             (
                                 (onlyEnabled &&
                                     p.awBlog.isEnabled && p.isEnabled.Equals(true)) || //if onlyPublished true
                                 !onlyEnabled  //else 
                             )
                             orderby p.title
                             select p;
            if (categories == null || categories.Count() == 0)
                return null;

            return categories.ToList<awBlogCategory>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        public long AddBlogCategory(long blogId, string title, string description, bool isEnabled)
        {
            long id = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            awBlogCategory category = new awBlogCategory();

            category.blogCategoryId = id;
            category.blogId = blogId;
            category.title = title;
            category.description = description;
            category.isEnabled = isEnabled;

            category.lastBuildDate = DateTime.Now;
            category.createDate = DateTime.Now;

            _context.awBlogCategories.InsertOnSubmit(category);
            _context.SubmitChanges();

            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogCategoryId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        public bool UpdateBlogCategory(long blogCategoryId, string title, string description, bool isEnabled)
        {
            awBlogCategory category = _context.awBlogCategories.
                            FirstOrDefault(st => st.blogCategoryId.Equals(blogCategoryId));

            if (category == null)
                return false;

            category.title = title;
            category.description = description;
            category.isEnabled = isEnabled;

            category.lastBuildDate = DateTime.Now;
            _context.SubmitChanges();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogCategoryId"></param>
        public void DeleteBlogCategory(long blogCategoryId)
        {
            if (blogCategoryId <= 0)
                return;

            //Delete Blog Post and Blog Category Relations
            var blogCategoryPostList = from l in _context.awBlogCategoryPosts
                                       where l.blogCategoryId.Equals(blogCategoryId)
                                       select l;

            _context.awBlogCategoryPosts.DeleteAllOnSubmit(blogCategoryPostList);

            //Delete the tag            
            awBlogCategory cat = GetBlogCategory(blogCategoryId);
            if (cat == null)
                return;

            _context.awBlogCategories.DeleteOnSubmit(cat);
            _context.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="onlyEnabled"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<awBlogCategory> GetBlogCategoryPostList(long postId, bool onlyEnabled)
        {
            var categories = from p in _context.awBlogCategoryPosts
                             where p.blogPostId.Equals(postId) &&
                             (
                                 (onlyEnabled &&
                                  p.awBlogCategory.isEnabled.Equals(true) &&
                                  p.awBlogPost.isPublished.Equals(true) &&
                                  p.awBlogCategory.awBlog.isEnabled.Equals(true)) ||
                                 !onlyEnabled  //else 
                             )
                             orderby p.awBlogCategory.title
                             select p.awBlogCategory;
            if (categories == null || categories.Count() == 0)
                return null;

            return categories.ToList<awBlogCategory>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="blogCategoryId"></param>
        /// <param name="onlyEnabled"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.BlogPostExtended> GetPostListByCategoryId(long blogCategoryId, bool onlyEnabled, string search)
        {
            DateTime now = DateTime.Now;
            //using a boolean is faster
            bool addSearch = !String.IsNullOrEmpty(search);
            search = "%" + search.Trim() + "%";

            var posts = from p in _context.awBlogCategoryPosts
                        where p.blogCategoryId.Equals(blogCategoryId) &&
                        (
                            ((onlyEnabled &&
                                p.awBlogCategory.awBlog.isEnabled.Equals(true) && p.awBlogCategory.isEnabled.Equals(true) &&
                                p.awBlogPost.isPublished.Equals(true) && p.awBlogCategory.awBlog.isEnabled.Equals(true) &&
                                (p.awBlogPost.pubDate.Equals(null) || p.awBlogPost.pubDate <= now) &&
                                (p.awBlogPost.pubEndDate.Equals(null) || p.awBlogPost.pubEndDate > now)

                                ) ||
                            !onlyEnabled)
                            &&
                            (!addSearch ||
                               addSearch && (
                                    SqlMethods.Like(p.awBlogPost.title, search) ||
                                    SqlMethods.Like(p.awBlogPost.description, search) ||
                                    SqlMethods.Like(p.awBlogPost.summary, search) ||
                                    SqlMethods.Like(p.awBlogPost.awUser_BlogPost.username, search)
                               )
                            )
                        )
                        orderby p.awBlogPost.pubDate descending
                        select new AWAPI_Data.CustomEntities.BlogPostExtended
                        {
                            siteId = p.awBlogPost.awBlog.siteId,
                            Blog = p.awBlogPost.awBlog,
                            blogPostId = p.awBlogPost.blogPostId,
                            blogId = p.awBlogPost.blogId,
                            userId = p.awBlogPost.userId,
                            authorUserId = p.awBlogPost.authorUserId,
                            username = p.awBlogPost.awUser_BlogPost.username,
                            userFirstName = p.awBlogPost.awUser_BlogPost.firstName,
                            userLastName = p.awBlogPost.awUser_BlogPost.lastName,
                            userEmail = p.awBlogPost.awUser_BlogPost.email,
                            userImageUrl = p.awBlogPost.awUser_BlogPost.imageurl,
                            blogPostPage = p.awBlogPost.awBlog.blogPostPage,
                            title = p.awBlogPost.title,
                            summary = p.awBlogPost.summary,
                            description = p.awBlogPost.description,
                            isCommentable = p.awBlogPost.isCommentable,
                            isPublished = p.awBlogPost.isPublished,
                            imageurl = p.awBlogPost.imageurl,
                            recommended = p.awBlogPost.recommended,
                            geoTag = p.awBlogPost.geoTag,
                            pubDate = p.awBlogPost.pubDate,
                            pubEndDate = p.awBlogPost.pubEndDate,
                            lastBuildDate = p.awBlogPost.lastBuildDate,
                            createDate = p.awBlogPost.createDate,
                            numberOfComments = (
                                    from c in p.awBlogPost.awBlogPostComments
                                    where c.blogPostId.Equals(p.awBlogPost.blogPostId) && c.status.Equals(1)
                                    select c).Count(),
                            tags = (from t in p.awBlogPost.awBlogTagPosts
                                    where t.blogPostId.Equals(p.awBlogPost.blogPostId)
                                    orderby t.awBlogTag.title
                                    select t.awBlogTag)
                        };
            if (posts == null || posts.Count() == 0)
                return null;

            return posts.ToList<AWAPI_Data.CustomEntities.BlogPostExtended>();
        }

        /// <summary>
        /// Creates blogpost and blog tag relation
        /// </summary>
        /// <param name="blogTagIds"></param>
        /// <param name="blogPostId"></param>
        public void AddPostToBlogCategories(System.Collections.ArrayList blogCatIds, long blogPostId)
        {
            //If not in the list delete
            var notExist = from c in _context.awBlogCategoryPosts
                           where c.blogPostId.Equals(blogPostId) &&
                                !blogCatIds.Cast<Int64>().Contains(c.blogCategoryId)
                           select c;
            if (notExist != null && notExist.Count() > 0)
                _context.awBlogCategoryPosts.DeleteAllOnSubmit(notExist);


            //if already added don't add, else add..
            foreach (long blogCatId in blogCatIds)
            {
                var list = from c in _context.awBlogCategoryPosts
                           where c.blogPostId.Equals(blogPostId) &&
                                c.blogCategoryId.Equals(blogCatId)
                           select c;
                if (list != null && list.Count() > 0)
                    continue;

                //add the record
                long id = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
                awBlogCategoryPost catPost = new awBlogCategoryPost();

                catPost.blogCategoryPostId = id;
                catPost.blogPostId = blogPostId;
                catPost.blogCategoryId = blogCatId;
                catPost.createDate = DateTime.Now;

                _context.awBlogCategoryPosts.InsertOnSubmit(catPost);
            }
            _context.SubmitChanges();

            return;
        }

        #endregion

        #region BLOG TAGS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogTagId"></param>
        /// <returns></returns>
        public awBlogTag GetBlogTag(long blogTagId)
        {
            if (blogTagId <= 0)
                return null;

            var blogTag = _context.GetTable<awBlogTag>()
                        .Where(st => st.blogTagId.Equals(blogTagId));

            if (blogTag == null && blogTag.ToList().Count() == 0)
                return null;
            return blogTag.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public awBlogTag GetBlogTag(long blogId, string tag)
        {
            if (string.IsNullOrEmpty(tag))
                return null;

            var blogTag = _context.GetTable<awBlogTag>()
                        .Where(t => t.blogId.Equals(blogId) &&
                                t.title.ToLower().Trim().Equals(tag.ToLower().Trim())
                               );

            if (blogTag == null && blogTag.ToList().Count() == 0)
                return null;
            return blogTag.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="onlyEnabled"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.BlogTagExtended> GetBlogTagList(long blogId, bool onlyEnabled, int pageSize)
        {
            IList<AWAPI_Data.CustomEntities.BlogTagExtended> list = null;
            var temp = from p in _context.awBlogTags
                       where p.awBlog.blogId.Equals(blogId) &&
                       (
                           (onlyEnabled && p.awBlog.isEnabled) || //if onlyPublished true
                           !onlyEnabled  //else 
                       )
                       orderby p.title
                       select new AWAPI_Data.CustomEntities.BlogTagExtended
                       {
                           blogTagId = p.blogTagId,
                           blogId = p.blogId,
                           title = p.title,
                           createDate = p.createDate,
                           lastBuildDate = p.lastBuildDate,
                           numberOfPosts = (
                                 from tp in p.awBlogTagPosts
                                 where tp.blogTagId.Equals(p.blogTagId) && tp.awBlogPost.isPublished
                                 select tp.blogPostId
                           ).Count()
                       };

            if (temp == null) return null;
            list = temp.ToList<AWAPI_Data.CustomEntities.BlogTagExtended>();

            if (pageSize > 0)
            {
                //take the most selected ones
                var tags = (from t in list
                            orderby t.numberOfPosts descending
                            select t).Take(pageSize);

                //sort them by name
                list = tags.ToList<AWAPI_Data.CustomEntities.BlogTagExtended>();
                var tgs2 = from l in list
                           orderby l.title
                           select l;
                list = tgs2.ToList<AWAPI_Data.CustomEntities.BlogTagExtended>();
            }

            return list;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="onlyEnabled"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.BlogTagExtended> GetBlogTagListWithPostCount(long blogId, long postId, bool onlyEnabled, int pageSize)
        {
            DateTime now = DateTime.Now;
            IList<AWAPI_Data.CustomEntities.BlogTagExtended> list = null;

            if (postId > 0)
            {
                var temp = from bt in _context.awBlogTagPosts
                           where bt.awBlogPost.blogPostId.Equals(postId) &&
                           (!onlyEnabled ||
                               (onlyEnabled && bt.awBlogPost.awBlog.isEnabled && bt.awBlogPost.isPublished &&
                                (bt.awBlogPost.pubDate.Equals(null) || bt.awBlogPost.pubDate <= now) &&
                                (bt.awBlogPost.pubEndDate.Equals(null) || bt.awBlogPost.pubEndDate > now)
                               )
                           )
                           orderby bt.awBlogTag.title
                           select new AWAPI_Data.CustomEntities.BlogTagExtended
                           {
                               blogTagId = bt.blogTagId,
                               blogId = bt.awBlogPost.blogId,
                               title = bt.awBlogTag.title,
                               createDate = bt.awBlogTag.createDate,
                               lastBuildDate = bt.awBlogTag.lastBuildDate,
                               numberOfPosts = (
                                     from tp in _context.awBlogTagPosts
                                     where tp.blogTagId.Equals(bt.blogTagId) && tp.awBlogPost.isPublished &&
                                           (tp.awBlogPost.pubDate.Equals(null) || tp.awBlogPost.pubDate <= now) &&
                                           (tp.awBlogPost.pubEndDate.Equals(null) || tp.awBlogPost.pubEndDate > now)
                                     select tp.blogPostId
                               ).Count()
                           };

                if (temp == null) return null;
                list = temp.ToList<AWAPI_Data.CustomEntities.BlogTagExtended>();
            }
            else
            {
                list = GetBlogTagList(blogId, onlyEnabled, pageSize);

                //var temp = from p in _context.awBlogTags
                //           where p.awBlog.blogId.Equals(blogId) &&
                //           (
                //               (onlyEnabled && p.awBlog.isEnabled) || //if onlyPublished true
                //               !onlyEnabled  //else 
                //           )
                //           orderby p.title
                //           select new AWAPI_Data.CustomEntities.BlogTagExtended
                //           {
                //               blogTagId = p.blogTagId,
                //               blogId = p.blogId,
                //               title = p.title,
                //               createDate = p.createDate,
                //               lastBuildDate = p.lastBuildDate,
                //               numberOfPosts = (
                //                     from tp in p.awBlogTagPosts
                //                     where tp.blogTagId.Equals(p.blogTagId) && tp.awBlogPost.isPublished
                //                     select tp.blogPostId
                //               ).Count()
                //           };

                //if (temp == null) return null;

            }

            if (list == null || list.Count == 0)
                return null;

            if (pageSize > 0)
            {
                //take the most selected ones
                var tags = (from t in list
                            orderby t.numberOfPosts descending
                            select t).Take(pageSize);

                //sort them by name
                list = tags.ToList<AWAPI_Data.CustomEntities.BlogTagExtended>();
                var tgs2 = from l in list
                           orderby l.title
                           select l;
                list = tgs2.ToList<AWAPI_Data.CustomEntities.BlogTagExtended>();
            }

            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public long AddBlogTag(long blogId, string title)
        {
            title = title.ToLower().Trim();

            if (GetBlogTag(blogId, title) != null)
                throw new Exception("Tag already exist for the blog");

            long id = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            awBlogTag category = new awBlogTag();

            category.blogTagId = id;
            category.blogId = blogId;
            category.title = title;

            category.lastBuildDate = DateTime.Now;
            category.createDate = DateTime.Now;

            _context.awBlogTags.InsertOnSubmit(category);
            _context.SubmitChanges();

            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogTagId"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool UpdateBlogTag(long blogTagId, string title)
        {
            title = title.ToLower().Trim();

            //if the tag title already exists with another id
            var another = from t in _context.awBlogTags
                          where !t.blogTagId.Equals(blogTagId) &&
                                t.title.Equals(title)
                          select t;
            if (another != null && another.Count() > 0)
                throw new Exception("Tag already exist for the blog, you need to give another title.");

            //update the tag
            awBlogTag tag = _context.awBlogTags.
                            FirstOrDefault(st => st.blogTagId.Equals(blogTagId));

            if (tag == null)
                return false;

            tag.title = title;
            tag.lastBuildDate = DateTime.Now;
            _context.SubmitChanges();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogTagId"></param>
        public void DeleteBlogTag(long blogTagId)
        {
            if (blogTagId <= 0)
                return;

            //Delete Blog Post and Blog Category Relations
            var blogTagPostList = from l in _context.awBlogTagPosts
                                  where l.blogTagId.Equals(blogTagId)
                                  select l;
            _context.awBlogTagPosts.DeleteAllOnSubmit(blogTagPostList);

            //Delete the tag            
            awBlogTag cat = GetBlogTag(blogTagId);
            if (cat == null)
                return;

            _context.awBlogTags.DeleteOnSubmit(cat);
            _context.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="onlyEnabled"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<awBlogTag> GetBlogPostTagList(long postId, bool onlyEnabled)
        {
            var tags = from p in _context.awBlogTagPosts
                       where p.blogPostId.Equals(postId) &&
                       (
                           (onlyEnabled &&
                            p.awBlogPost.isPublished.Equals(true) &&
                            p.awBlogTag.awBlog.isEnabled.Equals(true)) ||
                           !onlyEnabled  //else 
                       )
                       orderby p.awBlogTag.title
                       select p.awBlogTag;
            if (tags == null || tags.Count() == 0)
                return null;

            return tags.ToList<awBlogTag>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogTagId"></param>
        /// <param name="onlyEnabled"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.BlogPostExtended> GetPostsInBlogTag(long blogTagId, bool onlyEnabled)
        {
            return GetBlogPostListByTagId(blogTagId, onlyEnabled, "");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogTagId"></param>
        /// <param name="onlyEnabled"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.BlogPostExtended> GetBlogPostListByTagId(long blogTagId, bool onlyEnabled, string search)
        {
            //using a boolean is faster
            DateTime now = DateTime.Now;
            bool addSearch = !String.IsNullOrEmpty(search);
            search = "%" + search.Trim() + "%";

            var posts = from p in _context.awBlogTagPosts
                        where p.blogTagId.Equals(blogTagId) &&
                        (
                            (
                                (
                                    onlyEnabled &&
                                    p.awBlogTag.awBlog.isEnabled && p.awBlogPost.isPublished && p.awBlogTag.awBlog.isEnabled &&
                                    (p.awBlogPost.pubDate.Equals(null) || p.awBlogPost.pubDate <= now) &&
                                    (p.awBlogPost.pubEndDate.Equals(null) || p.awBlogPost.pubEndDate > now)
                                )
                                || !onlyEnabled
                             ) &&
                            //if Where statement is set
                            (!addSearch ||
                               addSearch && (
                                    SqlMethods.Like(p.awBlogPost.title, search) ||
                                    SqlMethods.Like(p.awBlogPost.description, search) ||
                                    SqlMethods.Like(p.awBlogPost.summary, search) ||
                                    SqlMethods.Like(p.awBlogPost.awUser_BlogPost.username, search)
                               )
                            )
                        )
                        orderby p.awBlogPost.pubDate descending
                        select new AWAPI_Data.CustomEntities.BlogPostExtended
                        {
                            siteId = p.awBlogPost.awBlog.siteId,
                            Blog = p.awBlogPost.awBlog,
                            blogPostId = p.awBlogPost.blogPostId,
                            blogId = p.awBlogPost.blogId,
                            userId = p.awBlogPost.userId,
                            authorUserId = p.awBlogPost.authorUserId,
                            username = p.awBlogPost.awUser_BlogPost.username,
                            userFirstName = p.awBlogPost.awUser_BlogPost.firstName,
                            userLastName = p.awBlogPost.awUser_BlogPost.lastName,
                            userEmail = p.awBlogPost.awUser_BlogPost.email,
                            userImageUrl = p.awBlogPost.awUser_BlogPost.imageurl,
                            blogPostPage = p.awBlogPost.awBlog.blogPostPage,
                            recommended = p.awBlogPost.recommended,
                            title = p.awBlogPost.title,
                            summary = p.awBlogPost.summary,
                            description = p.awBlogPost.description,
                            isCommentable = p.awBlogPost.isCommentable,
                            isPublished = p.awBlogPost.isPublished,
                            imageurl = p.awBlogPost.imageurl,
                            geoTag = p.awBlogPost.geoTag,
                            pubDate = p.awBlogPost.pubDate,
                            pubEndDate = p.awBlogPost.pubEndDate,
                            lastBuildDate = p.awBlogPost.lastBuildDate,
                            createDate = p.awBlogPost.createDate,
                            numberOfComments = (from c in p.awBlogPost.awBlogPostComments
                                                where c.blogPostId.Equals(p.blogPostId) && c.status.Equals(1)
                                                select c).Count(),
                            tags = (from t in p.awBlogPost.awBlogTagPosts
                                    where t.blogPostId.Equals(p.blogPostId)
                                    orderby t.awBlogTag.title
                                    select t.awBlogTag)
                        };


            if (posts == null || posts.Count() == 0)
                return null;

            return posts.ToList<AWAPI_Data.CustomEntities.BlogPostExtended>();
        }

        /// <summary>
        /// Creates blogpost and blog tag relation
        /// </summary>
        /// <param name="blogTagIds"></param>
        /// <param name="blogPostId"></param>
        public void AddPostToBlogTags(string[] tags, long blogPostId)
        {
            //Get blog Id
            AWAPI_Data.CustomEntities.BlogPostExtended post = GetBlogPost(blogPostId);
            if (post == null)
                throw new Exception("Blog Post doesn't exist.");

            //If not in the list delete
            var notExist = from c in _context.awBlogTagPosts
                           where c.blogPostId.Equals(blogPostId) &&
                                !tags.Contains(c.awBlogTag.title)
                           select c;
            if (notExist != null && notExist.Count() > 0)
                _context.awBlogTagPosts.DeleteAllOnSubmit(notExist);

            //If tag doesn't exist then create it
            foreach (string tag in tags)
            {
                if (String.IsNullOrEmpty(tag) || tag == ",")
                    continue;

                if (GetBlogTag(post.blogId, tag) == null)
                    AddBlogTag(post.blogId, tag);
            }

            //if the relation already added don't add, else add..
            foreach (string tag in tags)
            {
                if (String.IsNullOrEmpty(tag))
                    continue;

                var list = from c in _context.awBlogTagPosts
                           where c.blogPostId.Equals(blogPostId) &&
                                c.awBlogTag.title.Equals(tag)
                           select c;
                if (list != null && list.Count() > 0)
                    continue;

                //add the record
                awBlogTag blogTag = GetBlogTag(post.blogId, tag);
                if (blogTag != null)
                {
                    long id = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
                    awBlogTagPost catPost = new awBlogTagPost();

                    catPost.blogTagPostId = id;
                    catPost.blogPostId = blogPostId;
                    catPost.blogTagId = blogTag.blogTagId;

                    _context.awBlogTagPosts.InsertOnSubmit(catPost);
                }
            }
            _context.SubmitChanges();

            return;
        }

        #endregion

        #region BLOG POST FILE

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.Data.awFile_Blog> GetBlogPostFileList(long postId)
        {
            var list = from l in _context.awBlogPostFiles
                       where l.blogPostId.Equals(postId)
                       orderby l.sortOrder
                       select l.awFile_Blog;

            if (list == null)
                return null;
            return list.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public void DeleteFileFromBlogPost(long blogPostFileId)
        {
            var list = from l in _context.awBlogPostFiles
                       where l.blogPostFileId.Equals(blogPostFileId)
                       select l;
            if (list == null || list.Count() == 0)
                return;

            _context.awBlogPostFiles.DeleteAllOnSubmit(list);
            _context.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogPostId"></param>
        public void RemoveFileBlogPostRelationByFileId(long fileId)
        {
            var list = from l in _context.awBlogPostFiles
                       where l.fileId.Equals(fileId)
                       select l;
            if (list == null || list.Count() == 0)
                return;

            _context.awBlogPostFiles.DeleteAllOnSubmit(list);

            _context.SubmitChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogPostId"></param>
        /// <param name="blogPostFileId"></param>
        /// <param name="sortOrder"></param>
        public void SaveFileToBlogPost(long blogPostId, long fileId, int sortOrder)
        {
            awBlogPostFile file = new awBlogPostFile();

            var list = from l in _context.awBlogPostFiles
                       where l.blogPostId.Equals(blogPostId) &&
                            l.fileId.Equals(fileId)
                       select l;

            //adds new 
            if (list == null || list.Count() == 0)
            {
                file.blogPostFileId = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
                file.fileId = fileId;
                file.blogPostId = blogPostId;
                file.sortOrder = sortOrder;
                _context.awBlogPostFiles.InsertOnSubmit(file);
            }
            else
            {
                file = list.ToList()[0];
                file.sortOrder = sortOrder;
            }

            _context.SubmitChanges();
        }
        #endregion
    }
}
