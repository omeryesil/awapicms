using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace AWAPI_Services.wcf
{
    [DataContract]
    public class AwapiFaultException
    {
        private string _errorMessage;

        [DataMember]
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

    }
}
