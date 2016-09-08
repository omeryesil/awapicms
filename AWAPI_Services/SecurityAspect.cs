using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PostSharp.Laos;
using System.Reflection;

namespace AWAPI_Services
{
    [Serializable]
    public class SecurityAspect : OnMethodInvocationAspect
    {
        
        
        long _siteId = 0;
        string _accessKey = "";

        public enum SecurityType
        { 
            Domain, 
            AccessKey        
        }

        SecurityType _securityType;

        public SecurityAspect(SecurityType type)
        {
            _securityType = type;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventArgs"></param>
        public override void OnInvocation(MethodInvocationEventArgs eventArgs)
        {
            GetSiteId(eventArgs);
            bool hasRight = false;


            if (_siteId != 0)
            {
                switch (_securityType.ToString().ToLower())
                {
                    case "domain":
                        hasRight = AWAPI_BusinessLibrary.library.SecurityLibrary.IsValidReferrer(_siteId);
                        break;
                    case "accesskey":
                        hasRight = AWAPI_BusinessLibrary.library.SecurityLibrary.IsValidAccessKey(_siteId, _accessKey);
                        break;
                }
            }
            
            if (!hasRight)
                throw new System.ServiceModel.FaultException("You don't have permission.");
            else 
            {
                //eventArgs.Proceed();
                object objRtn = eventArgs.Delegate.DynamicInvoke(eventArgs.GetArguments());
                eventArgs.ReturnValue = objRtn;
            }
        }

        /// <summary>
        /// Creates Key name based on the method name and it's parameters 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="controlId"></param>
        /// <returns></returns>
        private void GetSiteId(MethodInvocationEventArgs method)
        {
            int no = 0;
            _siteId = 0;
            _accessKey = "";

            ParameterInfo[] prmsInfo = method.Delegate.Method.GetParameters();
            if (prmsInfo != null && prmsInfo.Length > 0)
            {
                object[] objParamsValues = method.GetArguments();
                foreach (ParameterInfo prm in prmsInfo)
                {
                    if (prm.Name.ToLower() == "siteid" && objParamsValues[no].ToString().Trim() != "")
                            _siteId = Convert.ToInt64(objParamsValues[no].ToString());
                    else if (prm.Name.ToLower() == "accesskey" && objParamsValues[no].ToString().Trim() != "")
                            _accessKey = objParamsValues[no].ToString();
                    no++;
                }
            }
        }


    }
}
