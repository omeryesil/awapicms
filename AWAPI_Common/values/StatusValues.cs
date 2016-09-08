using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWAPI_Common.values
{
    public class StatusValues
    {
        public const int SITE_DISABLED = 0;
        public const int SITE_ENABLED = 1;
        public const int SITE_SUSPENDED = 20;
        public const int SITE_DELETED = 90;

        public const int USER_DISABLED = 0;
        public const int USER_ENABLED = 1;
        public const int USER_SUSPENDED = 20;
        public const int USER_DELETED = 90;

        public const int SITEUSER_DISABLED = 0;
        public const int SITEUSER_ENABLED = 1;
        public const int SITEUSER_SUSPENDED = 20;
        public const int SITEUSER_DELETED = 90;

        public const int CONTENT_DRAFT = 0;
        public const int CONTENT_PUBLISHED = 1;
        public const int CONTENT_LOCKED = 20;
        public const int CONTENT_WAITING_TO_APPROVE = 30;
        public const int CONTENT_DELETED = 90;
    }
}
