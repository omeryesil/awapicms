using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace AWAPI.admin.reports.handlers
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class excel : IHttpHandler
    {
        string ReportName
        {
            get
            {
                if (HttpContext.Current.Request["report"] == null)
                    return "";
                return HttpContext.Current.Request["report"].Trim();
            }
        }

        public enum Reports
        {
            ContestEntries,
            ContestGroupEntries
        }




        public void ProcessRequest(HttpContext context)
        {
            string content = "";
            string fileName = "";

            switch (ReportName.ToLower())
            {
                case "contestentries":
                    fileName = "contestentries";
                    content = GetContestEnties();
                    break;
                case "contestgroupentries":
                    fileName = "contestgroupentries";
                    content = GetContestGroupEnties();
                    break;

            }
            fileName += "_" + DateTime.Now.ToString("yyyy_MM_dd_HH:mm") + ".csv";


            context.Response.ContentType = "text/csv";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.AddHeader("content-disposition", "attachement; filename=" + fileName);
            context.Response.Write(content);


        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region Get Reports

        /// <summary>
        /// contestId must be set in the url
        /// </summary>
        /// <returns></returns>
        string GetContestEnties()
        {
            if (HttpContext.Current.Request["contestid"] == null)
                return "";
            AWAPI_BusinessLibrary.library.ContestLibrary lib = new AWAPI_BusinessLibrary.library.ContestLibrary();
            long contestId = Convert.ToInt64(HttpContext.Current.Request["contestid"]);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();


            //GET CONTEST IFNO
            AWAPI_Data.CustomEntities.ContestExtended contest = lib.GetContest(contestId, false);
            if (contest == null)
                return "";
            sb.Append("Contest entries report\n");
            sb.AppendFormat("Report date:,{0}\n\n", DateTime.Now);

            sb.AppendFormat("ContestId:,{0}\n", contest.contestId.ToString());
            sb.AppendFormat("Name:,{0}\n", contest.title);
            sb.AppendFormat("Description:,{0}\n", contest.description.Replace(",", " ").Replace("\n", " ")).Replace("\"", " "); ;
            sb.AppendFormat("StartDate:,{0}\n", contest.pubDate);
            sb.AppendFormat("EndDate:,{0}\n", contest.pubEndDate);

            sb.Append("\n");
            IList<AWAPI_Data.CustomEntities.ContestEntryExtended> list = lib.GetContestEntryList(contestId);
            if (list == null || list.Count == 0)
                return "";

            sb.Append("entryId,isEnabled,cultureCode,email,firstName,lastName,title, " +
                        "fileUrl,phone,phoneType,address,city,province," +
                        "postalCode,country,createDate,description\n");
            foreach (AWAPI_Data.CustomEntities.ContestEntryExtended c in list)
            {

                c.email = c.email == null ? "" : c.email.Replace(",", " ").Replace("\"", " ");
                c.firstName = c.firstName == null ? "" : c.firstName.Replace(",", " ").Replace("\"", " "); ;
                c.lastName = c.lastName == null ? "" : c.lastName.Replace(",", " ").Replace("\"", " "); ;
                c.title = c.title == null ? "" : c.title.Replace(",", " ").Replace("\"", " "); ;
                c.fileUrl = c.fileUrl == null ? "" : c.fileUrl.Replace(",", " ").Replace("\"", " "); ;
                c.tel = c.tel == null ? "" : c.tel.Replace(",", " ").Replace("\"", " "); ;
                c.telType = c.telType == null ? "" : c.telType.Replace(",", " ").Replace("\"", " "); ;
                c.address = c.address == null ? "" : c.address.Replace(",", " ").Replace("\"", " "); ;
                c.city = c.city == null ? "" : c.city.Replace(",", " ").Replace("\"", " "); ;
                c.province = c.province == null ? "" : c.province.Replace(",", " ").Replace("\"", " "); ;
                c.postalCode = c.postalCode == null ? "" : c.postalCode.Replace(",", " ").Replace("\"", " "); ;
                c.country = c.country == null ? "" : c.country.Replace(",", " ").Replace("\"", " "); ;
                c.description = c.description == null ? "" : c.description.Replace(",", " ").Replace("\n", " ").Replace("\r", "").Replace("\"", " "); ;

                sb.AppendFormat("{0},{1},{2},{3},{4},{5}," +
                            "{6},{7},{8},{9},{10},{11},{12}," +
                            "{13},{14},{15},{16}",
                            c.contestEntryId, c.isEnabled, c.cultureCode, c.email.Replace(",", " "), 
                            c.firstName.Replace(",", " "), 
                            c.lastName.Replace(",", " "), 
                            c.title.Replace(",", " "),
                            c.fileUrl.Replace(",", " "), 
                            c.tel.Replace(",", " "), 
                            c.telType.Replace(",", " "), 
                            c.address.Replace(",", " "), 
                            c.city.Replace(",", " "), 
                            c.province.Replace(",", " "),
                            c.postalCode.Replace(",", " "), 
                            c.country.Replace(",", " "), 
                            c.createDate, 
                            c.description.Replace(",", " ").Replace("\n", " ").Replace("\r", "") + "\n");
            }

            return sb.ToString();
        }

        /// <summary>
        /// groupid must be set in the url
        /// </summary>
        /// <returns></returns>
        string GetContestGroupEnties()
        {
            if (HttpContext.Current.Request["groupid"] == null)
                return "";
            AWAPI_BusinessLibrary.library.ContestLibrary lib = new AWAPI_BusinessLibrary.library.ContestLibrary();
            long contestGroupId = Convert.ToInt64(HttpContext.Current.Request["groupid"]);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();


            //GET CONTEST IFNO
            AWAPI_Data.Data.awContestGroup group = lib.GetContestGroup(contestGroupId);
            if (group == null)
                return "";
            sb.Append("Contest group entries report\n");
            sb.AppendFormat("Report date:,{0}\n\n", DateTime.Now);

            sb.AppendFormat("ContestId:,{0}\n", group.contestGroupId.ToString());
            sb.AppendFormat("Name:,{0}\n", group.title);
            sb.AppendFormat("Description:,{0}\n", group.description.Replace(",", " ").Replace("\n", " "));
            sb.AppendFormat("StartDate:,{0}\n", group.pubDate);
            sb.AppendFormat("EndDate:,{0}\n", group.pubEndDate);

            sb.Append("\n");
            IList<AWAPI_Data.CustomEntities.ContestEntryExtended> list = lib.GetContestEntryListByGroupId(contestGroupId, "", null);
            if (list == null || list.Count == 0)
                return "";

            sb.Append("entryId,cultureCode,email,firstName,lastName,title, " +
                        "fileUrl,phone,phoneType,address,city,province," +
                        "postalCode,country,createDate,description\n");
            foreach (AWAPI_Data.CustomEntities.ContestEntryExtended c in list)
            {
                sb.AppendFormat("{0},{1},{2},{3},{4},{5}," +
                            "{6},{7},{8},{9},{10},{11},{12}," +
                            "{13},{14},{15}",
                            c.contestEntryId, c.cultureCode, c.email.Replace(",", " "), 
                            c.firstName.Replace(",", " ").Replace("\"", " "),
                            c.lastName.Replace(",", " ").Replace("\"", " "),
                            c.title.Replace(",", " ").Replace("\"", " "),
                            c.fileUrl.Replace(",", " ").Replace("\"", " "),
                            c.tel.Replace(",", " ").Replace("\"", " "),
                            c.telType.Replace(",", " ").Replace("\"", " "),
                            c.address.Replace(",", " ").Replace("\"", " "),
                            c.city.Replace(",", " ").Replace("\"", " "),
                            c.province.Replace(",", " ").Replace("\"", " "),
                            c.postalCode.Replace(",", " ").Replace("\"", " "),
                            c.country.Replace(",", " ").Replace("\"", " "), 
                            c.createDate,
                            c.description.Replace(",", " ").Replace("\n", " ").Replace("\r", "") + "\n").Replace("\"", " ");
            }

            return sb.ToString();
        }

        #endregion

    }
}
