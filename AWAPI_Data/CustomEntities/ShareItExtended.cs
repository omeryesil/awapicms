using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWAPI_Data.CustomEntities
{
    public class ShareItExtended
    {
        private long _shareItId;
        private long _userId;
        private string _title;
        private string _description;
        private string _link;
        private bool _shareWithEveryone;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _userImageUrl; 
        private DateTime _createDate;

        public long shareItId
        {
            get { return _shareItId; }
            set { _shareItId = value; }
        }

        public long userId
        {
            get { return _userId; }
            set { _userId = value; }
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

        public string link
        {
            get { return _link; }
            set { _link = value; }
        }

        public bool shareWithEveryone
        {
            get { return _shareWithEveryone; }
            set { _shareWithEveryone = value; }
        }

        public string firstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string lastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string userImageUrl
        {
            get { return _userImageUrl; }
            set { _userImageUrl = value; }
        }

        public DateTime createDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }


    }
}
