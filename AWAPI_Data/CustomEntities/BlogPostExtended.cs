using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWAPI_Data.CustomEntities
{
    public class BlogPostExtended
    {
        private long _blogPostId;
        private long _blogId;
        private long? _authorUserId;
        private long _userId;
        private long _siteId;
        private AWAPI_Data.Data.awBlog _blog;

        private string _username;
        private string _userFirstName;
        private string _userLastName;
        private string _userEmail;
        private string _userImageUrl;

        private string _title;
        private string _description;
        private string _summary;

        private bool _isPublished;
        private bool _isCommentable;

        private int _recommended;

        private string _imageUrl;
        private string _blogPostPage;
        private string _geoTag;

        private DateTime? _pubDate;
        private DateTime? _pubEndDate;
        private DateTime? _lastBuildDate;
        private DateTime? _createDate;

        private IEnumerable<AWAPI_Data.Data.awBlogTag> _tags;

        private int _numberOfComments = 0;


        public long blogPostId
        {
            get { return _blogPostId; }
            set { _blogPostId = value; }
        }


        public long blogId
        {
            get { return _blogId; }
            set { _blogId = value; }
        }

        public long? authorUserId
        {
            get { return _authorUserId; }
            set { _authorUserId = value; }
        }

        public long userId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public long siteId
        {
            get { return _siteId; }
            set { _siteId = value; }
        }

        public AWAPI_Data.Data.awBlog Blog
        {
            get { return _blog; }
            set { _blog = value; }
        }


        public string username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string userFirstName
        {
            get { return _userFirstName; }
            set { _userFirstName = value; }
        }

        public string userLastName
        {
            get { return _userLastName; }
            set { _userLastName = value; }
        }

        public string userEmail
        {
            get { return _userEmail; }
            set { _userEmail = value; }
        }

        public string userImageUrl
        {
            get { return _userImageUrl; }
            set { _userImageUrl = value; }
        }

        public string title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string summary
        {
            get { return _summary; }
            set { _summary = value; }
        }


        public string description
        {
            get { return _description; }
            set { _description = value; }
        }


        public bool isPublished
        {
            get { return _isPublished; }
            set { _isPublished = value; }
        }

        public bool isCommentable
        {
            get { return _isCommentable; }
            set { _isCommentable = value; }
        }

        public int recommended
        {
            get { return _recommended; }
            set { _recommended = value; }
        }


        public string geoTag
        {
            get { return _geoTag; }
            set { _geoTag = value; }
        }

        public string imageurl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; }
        }

        public string blogPostPage
        {
            get { return _blogPostPage; }
            set { _blogPostPage = value; }
        }

        public DateTime? pubDate
        {
            get { return _pubDate; }
            set { _pubDate = value; }
        }


        public DateTime? pubEndDate
        {
            get { return _pubEndDate; }
            set { _pubEndDate = value; }
        }


        public DateTime? lastBuildDate
        {
            get { return _lastBuildDate; }
            set { _lastBuildDate = value; }
        }


        public DateTime? createDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        public int numberOfComments
        {
            get { return _numberOfComments; }
            set { _numberOfComments = value; }
        }

        public IEnumerable<AWAPI_Data.Data.awBlogTag> tags
        {
            get { return _tags; }
            set { _tags = value; }
        }
    }
}
