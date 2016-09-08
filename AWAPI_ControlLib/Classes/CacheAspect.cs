using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using PostSharp.Laos;

namespace AWAPI_ControlLib.Classes
{
    [Serializable]
    public class CacheAspect : OnMethodInvocationAspect
    {
        public const string CACHE_PATH = "~/_cache/";
        //const string CACHE_NAME = "AWAPI_Cache";

        public enum TIME_PERIOD
        {
            Second,
            Minute,
            Hour,
            Day
        }

        private string _time = "";
        //private string _controlId = "";
        private string _webConfigKeyNameForCacheDuration = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">
        /// value + time period
        /// time period can be 
        ///     s -> second
        ///     m -> minute
        ///     h -> hour
        ///     d -> day
        /// example: 10m
        ///         10s
        ///         1h
        ///         1d
        /// </param>
        /// 
        public CacheAspect(int time, TIME_PERIOD period)
        {
            switch (period)
            {
                case TIME_PERIOD.Second:
                    _time = time + "s";
                    break;
                case TIME_PERIOD.Minute:
                    _time = time + "m";
                    break;
                case TIME_PERIOD.Hour:
                    _time = time + "h";
                    break;
                case TIME_PERIOD.Day:
                    _time = time + "d";
                    break;
                default:
                    _time = "10m";
                    break;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="webConfigKeyName"> 
        /// Key name in the appSettings section in the Web.Config file
        /// the value can be: 
        ///     s -> second
        ///     m -> minute
        ///     h -> hour
        ///     d -> day
        ///     
        /// Example value:
        ///         10s
        ///         1h
        ///         1d 
        /// </param>
        public CacheAspect(string webConfigKeyName)
        {
            _webConfigKeyNameForCacheDuration = webConfigKeyName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventArgs"></param>
        public override void OnInvocation(MethodInvocationEventArgs eventArgs)
        {
            object objRtn = null;
            string controlId = "";

            //check for caching
            string cacheKey = CreateKeyNameAndSetMethodParameters(eventArgs, ref controlId);
            objRtn = GetFromCache(cacheKey);

            if (objRtn == null)
            {
                //creates key, and get _controlId id and _time
                objRtn = eventArgs.Delegate.DynamicInvoke(eventArgs.GetArguments());

                if (!String.IsNullOrEmpty(controlId))   //controlId is required to cache
                    if (_time != "")                    //time is required to cache
                        AddToCache(cacheKey, controlId, _time, objRtn);
            }

            eventArgs.ReturnValue = objRtn;
        }

        /// <summary>
        /// Creates Key name based on the method name and it's parameters 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="controlId"></param>
        /// <returns></returns>
        private string CreateKeyNameAndSetMethodParameters(MethodInvocationEventArgs method, ref string controlId)
        {
            string prms = "";

            int no = 0;
            _time = "";
            controlId = "";

            ParameterInfo[] prmsInfo = method.Delegate.Method.GetParameters();
            if (prmsInfo != null && prmsInfo.Length > 0)
            {
                object[] objParamsValues = method.GetArguments();
                foreach (ParameterInfo prm in prmsInfo)
                {
                    if (objParamsValues[no] == null)
                        prms += "null|";
                    else
                    {
                        prms += objParamsValues[no].ToString() + "|";

                        if (prm.Name.ToLower() == "cacheduration" && objParamsValues[no].ToString().Trim() != "")
                            _time = objParamsValues[no].ToString();
                        else if (prm.Name.ToLower() == "controlid" && objParamsValues[no].ToString().Trim() != "")
                            controlId = objParamsValues[no].ToString();
                    }
                    no++;
                }
            }
            return method.Delegate.Method.Name + "|" + prms;
        }


        /// <summary>
        /// Returns cached data
        /// </summary>
        /// <returns></returns>
        private object GetFromCache(string key)
        {
            System.Collections.Hashtable cacheTable = (System.Collections.Hashtable)HttpContext.Current.Cache[key];

            if (cacheTable == null || cacheTable[key] == null)
                return null;

            if (cacheTable[key] != null && cacheTable[key].ToString().IndexOf("savedtofile:") >= 0)
            {
                string filePath = cacheTable[key].ToString().Replace("savedtofile:", "");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.StreamReader rdr = new System.IO.StreamReader(filePath);
                    string rtn = rdr.ReadToEnd();
                    rdr.Close();
                    return rtn;
                }
                else
                {
                    cacheTable.Remove(key);
                    return null;
                }
            }

            return cacheTable[key];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objToStore"></param>
        private void AddToCache(string key, string controlId, string cacheDuration, object objData)
        {
            object objToStore = null;

            System.Collections.Hashtable cacheTable = (System.Collections.Hashtable)HttpContext.Current.Cache[key];
            if (cacheTable == null)
                cacheTable = new System.Collections.Hashtable();

            //System.Collections.Hashtable cacheTable = (System.Collections.Hashtable)HttpContext.Current.Cache[CACHE_NAME];
            //if (cacheTable == null)
            //    cacheTable = new System.Collections.Hashtable();

            //long len = Utilities.GetObjectSize(objData);
            //if (len < 512)
            //    objToStore = objData;
            //else
            //{
            string fileName = controlId; // Utilities.GetUniqueId().ToString() + "_" + _controlId;
            if (System.Web.HttpContext.Current != null)
                fileName = HttpContext.Current.Request.Path.ToLower().Replace("/", "_") +
                        HttpContext.Current.Request.QueryString.ToString().ToLower().Replace("/", "_").Replace(" ", "").Replace("&", "").Replace("?", "") + 
                        "_" + fileName + "_" + _time;

                //fileName = Regex.Replace(HttpContext.Current.Request.Url.LocalPath, @"[^\w\.@-]", "") +
                //    "_" + fileName + "_" + _time;

            string dir = System.Web.HttpContext.Current.Server.MapPath(CACHE_PATH);
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);

            string filePath = dir + fileName;
            
            System.IO.File.WriteAllText(filePath, objData.ToString());
            //sw = new System.IO.StreamWriter(fileName);
            //sw.Write(objData);
            //sw.Close();

            string value = "savedtofile:" + filePath;

            objToStore = value;
            //}

            if (cacheTable[key] != null)
                cacheTable[key] = objToStore;
            else
                cacheTable.Add(key, objToStore);

            //SET EXPIRY TIME
            //if value is set from the webconfig
            DateTime tmExpiration = DateTime.MaxValue;
            string time = cacheDuration;

            if (cacheDuration == "" && _webConfigKeyNameForCacheDuration != "" &&
                System.Configuration.ConfigurationManager.AppSettings[_webConfigKeyNameForCacheDuration] != null &&
                System.Configuration.ConfigurationManager.AppSettings[_webConfigKeyNameForCacheDuration] != "")
                time = System.Configuration.ConfigurationManager.AppSettings[_webConfigKeyNameForCacheDuration].ToString().ToLower();

            if (time != "")
            {
                int count = Int32.Parse(time.Replace("s", "").Replace("m", "").Replace("h", "").Replace("d", ""));
                string term = time.Replace(count.ToString(), "").ToLower();
                switch (term)
                {
                    case "s":
                        tmExpiration = DateTime.Now.AddSeconds(count);
                        break;
                    case "m":
                        tmExpiration = DateTime.Now.AddMinutes(count);
                        break;
                    case "h":
                        tmExpiration = DateTime.Now.AddHours(count);
                        break;
                    case "d":
                        tmExpiration = DateTime.Now.AddDays(count);
                        break;
                }
            }
            HttpRuntime.Cache.Add(key,
                                cacheTable,
                                null,
                                tmExpiration,
                                System.Web.Caching.Cache.NoSlidingExpiration,
                                System.Web.Caching.CacheItemPriority.Normal,
                                new System.Web.Caching.CacheItemRemovedCallback(OnRemoveCache));
        }

        private void OnRemoveCache(string cacheValue, object value, System.Web.Caching.CacheItemRemovedReason removeReason)
        {
            System.Collections.Hashtable cacheTable = (System.Collections.Hashtable)value;
            if (cacheTable == null)
                return;

            foreach (string chValue in cacheTable.Values)
            {
                if (chValue != null && chValue.IndexOf("savedtofile:") >= 0)
                {
                    string filePath = chValue.Replace("savedtofile:", "");
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }
            }
        }


    }
}
