using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWAPI_Data.CustomEntities
{
    public class BlogTagExtended
    {
        private long _blogTagId;
        private long _blogId;
        private string _title;
        private DateTime? _lastBuildDate;
        private DateTime _createDate;
        private int _numberOfPosts = 0;
  

        public long blogTagId
        {
            get { return _blogTagId; }
            set { _blogTagId = value; }
        }

        public long blogId
        {
            get { return _blogId; }
            set { _blogId = value; }
        }


        public string title
        {
            get { return _title; }
            set { _title = value; }
        }


        public DateTime? lastBuildDate
        {
            get { return _lastBuildDate; }
            set { _lastBuildDate = value; }
        }


        public DateTime createDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }


        public int numberOfPosts
        {
            get { return _numberOfPosts; }
            set { _numberOfPosts = value; }
        }

    }
}
