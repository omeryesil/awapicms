using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWAPI_Data.CustomEntities
{
    public class ContentExtended
    {
		private long _contentId;
		private string _alias;
		private string _title;
		private string _description;
		private long _siteId;
		private string _contentType;
		private bool _isEnabled;
		private bool _isCommentable;
		private System.Nullable<long> _parentContentId;
		private string _lineage;
		private System.Nullable<int> _sortOrder;
		private string _link;
		private string _imageurl;
		private long _userId;
		private System.Nullable<System.DateTime> _pubDate;
		private System.Nullable<System.DateTime> _pubEndDate;
		private System.Nullable<System.DateTime> _eventStartDate;
		private System.Nullable<System.DateTime> _eventEndDate;
		private System.Nullable<System.DateTime> _lastBuildDate;
		private System.DateTime _createDate;

        public long contentId
        {
            get { return _contentId; }
            set { _contentId = value; }
        }
        public string alias
        {
            get { return _alias; }
            set { _alias = value; }
        }
        public string title
        {
            get { return _title; }
            set { _title = value; }
        }
        public string description
        {
            get { return _description; }
            set { _description = value; }
        }
        public long siteId
        {
            get { return _siteId; }
            set { _siteId = value; }
        }
        public string contentType
        {
            get { return _contentType; }
            set { _contentType = value; }
        }
        public bool isEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }
        public bool isCommentable
        {
            get { return _isCommentable; }
            set { _isCommentable = value; }
        }
        public System.Nullable<long> parentContentId
        {
            get { return _parentContentId; }
            set { _parentContentId = value; }
        }
        public string lineage
        {
            get { return _lineage; }
            set { _lineage = value; }
        }
        public System.Nullable<int> sortOrder
        {
            get { return _sortOrder; }
            set { _sortOrder = value; }
        }

        public string link
        {
            get { return _link; }
            set { _link = value; }
        }
        public string imageurl
        {
            get { return _imageurl; }
            set { _imageurl = value; }
        }
        public long userId
        {
            get { return _userId; }
            set { _userId = value; }
        }
        public System.Nullable<System.DateTime> pubDate
        {
            get { return _pubDate; }
            set { _pubDate = value; }
        }
        public System.Nullable<System.DateTime> pubEndDate
        {
            get { return _pubEndDate; }
            set { _pubEndDate = value; }
        }
        public System.Nullable<System.DateTime> eventStartDate
        {
            get { return _eventStartDate; }
            set { _eventStartDate = value; }
        }
        public System.Nullable<System.DateTime> eventEndDate
        {
            get { return _eventEndDate; }
            set { _eventEndDate = value; }
        }
        public System.Nullable<System.DateTime> lastBuildDate
        {
            get { return _lastBuildDate; }
            set { _lastBuildDate = value; }
        }
        public System.DateTime createDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

    }
}
