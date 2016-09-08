using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWAPI_Data.CustomEntities
{
    public class BlogPostArchive
    {
        private long _siteId;
        private long _blogId;
        private int _month;
        private int _year;
        private int _numberOfPosts;

        public long siteId
        {
            get { return _siteId; }
            set { _siteId = value; }
        }

        public long blogId
        {
            get { return _blogId; }
            set { _blogId = value; }
        }

        public int month
        {
            get { return _month; }
            set { _month = value; }
        }

        public int year
        {
            get { return _year; }
            set { _year = value; }
        }

        public int numberOfPosts
        {
            get { return _numberOfPosts; }
            set { _numberOfPosts = value; }
        }


    }
}
