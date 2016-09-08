using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Xml;
using System.Drawing.Imaging;
using AWAPI_BusinessLibrary.library;
using System.ServiceModel.Syndication;

using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;


namespace AWAPI_Services.handler
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class weatherhandler : IHttpHandler
    {
        SyndicationFeed _feed = new SyndicationFeed();


        #region PROPERTIES

        public string WeatherServiceUrl
        {
            get
            {
                if (String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["weatherServiceUrl"]))
                    return "";
                return System.Configuration.ConfigurationManager.AppSettings["weatherServiceUrl"];
            }
        }


        public string WeatherServiceProvider
        {
            get
            {
                if (String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["WeatherServiceProvider"]))
                    return "";
                return System.Configuration.ConfigurationManager.AppSettings["WeatherServiceProvider"];
            }

        }
        #endregion

        #region URI PARAMTER
        public long SiteId
        {
            get
            {
                if (HttpContext.Current.Request["siteId"] == null)
                    return 0;
                return Convert.ToInt64(HttpContext.Current.Request["siteId"]);
            }
        }

        public string City
        {
            get
            {
                if (HttpContext.Current.Request["city"] == null)
                    return "toronto,on,canada";
                return HttpContext.Current.Request["city"];
            }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            AWAPI_Data.Data.awSite site = null;
            if (SiteId != 0)
                site = new AWAPI_BusinessLibrary.library.SiteLibrary().Get(SiteId);

            if (site == null)
                return;


            _feed = new SyndicationFeed("AWAPI CMS Feed", "", null);
            _feed.Authors.Add(new SyndicationPerson(""));
            _feed.Categories.Add(new SyndicationCategory("weatherforecast"));
            _feed.AttributeExtensions.Add(new XmlQualifiedName("site"), site.title);
            _feed.AttributeExtensions.Add(new XmlQualifiedName("sitelink"), site.link);

            if (City != "" && WeatherServiceUrl != "")
            {
                switch (WeatherServiceProvider.ToLower())
                {
                    case "google":
                        GetFromGoogle(WeatherServiceUrl + City);
                        break;
                    default:
                        break;
                }
            }

            XmlWriter writer = XmlWriter.Create(context.Response.Output);
            context.Response.ContentType = "application/rss+xml";
            Rss20FeedFormatter rss = new Rss20FeedFormatter(_feed);
            rss.WriteTo(writer);
            writer.Close();


        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void GetFromGoogle(string serviceUrl)
        {
            string url = serviceUrl;
            string root = "/xml_api_reply/weather/";
            XPathDocument docNav = new XPathDocument(url);
            XPathNavigator nav = docNav.CreateNavigator();

            //Set forecast information (will be added in the RSS header as elements
            AddSyndFeedHeader(GetNodeValue(nav, root + "forecast_information/city", "data"),
                            GetNodeValue(nav, root + "forecast_information/postal_code", "data"),
                            GetNodeValue(nav, root + "forecast_information/latitude_e6", "data"),
                            GetNodeValue(nav, root + "forecast_information/longitude_e6", "data"),
                            GetNodeValue(nav, root + "forecast_information/forecast_date", "data"),
                            GetNodeValue(nav, root + "forecast_information/current_date_time", "data"),
                            GetNodeValue(nav, root + "forecast_information/unit_system", "data"));

            //Set current conditions
            AddSyncFeedItem_CurrentCondition(GetNodeValue(nav, root + "current_conditions/condition", "data"),
                             GetNodeValue(nav, root + "current_conditions/temp_f", "data"),
                             GetNodeValue(nav, root + "current_conditions/temp_c", "data"),
                             GetNodeValue(nav, root + "current_conditions/icon", "data"),
                             GetNodeValue(nav, root + "current_conditions/wind_condition", "data"),
                             GetNodeValue(nav, root + "current_conditions/humidity", "data"));

            //Add forcast data
            XPathNodeIterator it = nav.Select(root + "forecast_conditions");
            if (it != null)
            {
                System.Collections.Generic.List<SyndicationItem> items = new System.Collections.Generic.List<SyndicationItem>();
                while (it.MoveNext())
                {
                    string day = GetNodeValue(it.Current, "day_of_week", "data");
                    string lowF = GetNodeValue(it.Current, "low", "data");
                    string highF = GetNodeValue(it.Current, "high", "data");
                    string imageUrl = GetNodeValue(it.Current, "icon", "data");
                    string condition = GetNodeValue(it.Current, "condition", "data");

                    int lowC = -99;
                    int highC = -99;

                    if (!String.IsNullOrEmpty(lowF)) lowC = FahrenheitToCelsius(Convert.ToDouble(lowF));
                    if (!String.IsNullOrEmpty(highF)) highC = FahrenheitToCelsius(Convert.ToDouble(highF));

                    SyndicationItem itm = AddSyncFeedItem_ForeCast(day, condition, lowF, lowC.ToString(), highF, highC.ToString(), imageUrl);
                    items.Add(itm);
                }
                _feed.Items = items;
            }

        }

        string GetNodeValue(XPathNavigator nav, string path, string attribute)
        {
            if (nav == null)
                return "";
            XmlNamespaceManager ns = new XmlNamespaceManager(nav.NameTable);
            XPathNodeIterator it = nav.Select(path);
            if (it == null)
                return "";

            it.MoveNext();

            return it.Current.GetAttribute(attribute, ns.DefaultNamespace);
        }

        void AddSyndFeedHeader(string city, string postalCode, string latitude, string longitude, string forecastDate, string currentDateTime, string unitSystem)
        {
            _feed.ElementExtensions.Add("city", null, city);
            _feed.ElementExtensions.Add("postalcode", null, postalCode);
            _feed.ElementExtensions.Add("latitude", null, latitude);
            _feed.ElementExtensions.Add("longitude", null, longitude);
            _feed.ElementExtensions.Add("forecastDate", null, forecastDate);
            _feed.ElementExtensions.Add("currentDateTime", null, currentDateTime);
            _feed.ElementExtensions.Add("unitSystem", null, unitSystem);
        }

        void AddSyncFeedItem_CurrentCondition(string condition, string tempF, string tempC, string imageUrl, string windCondition, string humidty)
        {
            _feed.ElementExtensions.Add("current_condition", null, condition);
            _feed.ElementExtensions.Add("current_temp_f", null, tempF);
            _feed.ElementExtensions.Add("current_temp_c", null, tempC);
            _feed.ElementExtensions.Add("current_imageurl", null, imageUrl);
            _feed.ElementExtensions.Add("current_windcondition", null, windCondition);
            _feed.ElementExtensions.Add("current_humidty", null, humidty);
        }

        SyndicationItem AddSyncFeedItem_ForeCast(string day, string condition, string lowF, string lowC, string highF, string highC, string imageUrl)
        {
            SyndicationItem item = new SyndicationItem(
                        day,
                        condition,
                        null,
                        null,
                        DateTime.Now);

            item.ElementExtensions.Add("condition", null, condition);
            item.ElementExtensions.Add("low_f", null, lowF);
            item.ElementExtensions.Add("low_c", null, lowC);
            item.ElementExtensions.Add("high_f", null, highF);
            item.ElementExtensions.Add("high_c", null, highC);
            item.ElementExtensions.Add("imageurl", null, imageUrl);

            return item;

        }


        #region HELPER METHODS 

        public int CelciusToFahrenheit(double cel)
        {
            double result = (((0.9 / 0.5) * cel) + 32);
            return Convert.ToInt32(result);

        }

        public int FahrenheitToCelsius(double farh)
        {
            double result = ((0.5 / 0.9) * (farh - 32));
            return Convert.ToInt32(result);
        }
        #endregion



    }
}
