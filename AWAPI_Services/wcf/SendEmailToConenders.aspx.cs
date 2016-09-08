using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;

namespace AWAPI_Services.wcf
{
    public partial class test : System.Web.UI.Page
    {
        public string XML_Path
        {
            get
            {
                return Server.MapPath("~/App_Data/contestentries.xml");
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        AWAPI_BusinessLibrary.library.BlogLibrary _blogLibrary = new AWAPI_BusinessLibrary.library.BlogLibrary();

        public void AddBlogComment(long postId)
        {
            long commentId = _blogLibrary.AddBlogPostComment(postId, null, "title", "description",
                                            "oyesilo@yahoo.ca", "userName", "firstName", "lastName",
                                            Convert.ToInt32(AWAPI_BusinessLibrary.library.BlogLibrary.Comment_Status.Pending));
        }

        /// <summary>
        /// Prepare an xml file and insert the records...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _btnPrepareData_Click(object sender, EventArgs e)
        {
            AWAPI_BusinessLibrary.library.ContestLibrary lib = new AWAPI_BusinessLibrary.library.ContestLibrary();
            IList<AWAPI_Data.CustomEntities.ContestEntryExtended> list = lib.GetContestEntryListByGroupId(Convert.ToInt64(_groupId.Text), "", null);
            if (list == null || list.Count == 0)
            {
                Response.Write("<font color='#ff0000'>No entries has been found</font><br/>");
                return;
            }

            //filter based on start and end dates
            DateTime? dtStart = AWAPI_Common.library.MiscLibrary.ConvertStringToDate(_startDate.Text);
            DateTime? dtEnd = AWAPI_Common.library.MiscLibrary.ConvertStringToDate(_endDate.Text);
            if (dtStart == null || dtEnd == null)
            {
                Response.Write("<font color='#ff0000'>Start and End dates are required</font><br/>");
                return;
            }
            var tmpList = from l in list
                          where l.createDate >= dtStart.Value && l.createDate <= dtEnd.Value
                          orderby l.email
                          select l;
            if (tmpList == null || tmpList.Count() == 0)
            {
                Response.Write("<font color='#ff0000'>No entries has been found in the date range</font><br/>");
                return;
            }
            list = tmpList.ToList();

            //Write the results to the xml
            System.Data.DataTable dt = new System.Data.DataTable("contestentries");
            dt.Columns.Add("email");
            dt.Columns.Add("firstname");
            dt.Columns.Add("lastname");
            dt.Columns.Add("createdate");
            dt.Columns.Add("sent");

            System.IO.TextWriter tw = new System.IO.StreamWriter("c:\\emaillist.txt");
            System.Collections.ArrayList emails = new System.Collections.ArrayList();
            foreach (AWAPI_Data.CustomEntities.ContestEntryExtended ent in list)
            {
                string email = ent.email.ToLower().Trim();
                if (isEmail(email))
                {
                    if (emails.BinarySearch(email) >= 0)
                        continue;

                    System.Data.DataRow dr = dt.NewRow();
                    dr["email"] = email;
                    dr["firstname"] = ent.firstName;
                    dr["lastname"] = ent.lastName;
                    dr["sent"] = "0";

                    dt.Rows.Add(dr);
                    emails.Add(email);

                    tw.Write(email + ",");
                }
            }
            dt.WriteXml(XML_Path);
            tw.Close();
            _prepareDataResult.Text = "Number of records : " + dt.Rows.Count.ToString() + "<br/>" +
                        "Files location: " + XML_Path;

            _resultList.DataSource = dt;
            _resultList.DataBind();


        }

        private static bool isEmail(string inputEmail)
        {
            if (inputEmail == null ||
                inputEmail.Trim() == "")
                return false;

            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }


        protected void _sendEmail_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(XML_Path);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                Response.Write("<font color='#ff0000'>Xml file is empty.</font><br/>");
                return;
            }

            DataRow[] drs = ds.Tables[0].Select("sent=0");
            if (drs == null || drs.Length == 0)
            {
                Response.Write("<font color='#ff0000'>All the records already were sent.</font><br/>");
                return;
            }

            AWAPI_BusinessLibrary.library.EmailTemplateLib emailLib = new AWAPI_BusinessLibrary.library.EmailTemplateLib();
            long emailTemplateId = Convert.ToInt64(_emailTemplateId.Text);

            int n = 0;
            int max = Convert.ToInt32(_numberOfEmailsToSend.Text);
            foreach (DataRow dr in drs)
            {
                string email = dr["email"].ToString();
                email = "oyesilo@yahoo.ca";
                emailLib.Send(emailTemplateId, email,
                                "firstname|" + dr["firstname"].ToString(),
                                "lastname|" + dr["lastname"].ToString(),
                                "startdate|25th of February 2010",
                                "enddate|1st of March 2010");
                dr["sent"] = "1";

                n++;
                if (n >= max)
                    break;
            }

            ds.WriteXml(XML_Path);
            //backup
            ds.WriteXml(XML_Path + DateTime.Now.ToString("yyyy_MM_dd_HH_mm.xml"));
            ds.Dispose();

            _resultList.DataSource = ds;
            _resultList.DataBind();        
        }



        protected void _resultList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            _resultList.PageIndex = e.NewPageIndex;

            DataSet ds = new DataSet();
            ds.ReadXml(XML_Path);

            _resultList.DataSource = ds;
            _resultList.DataBind();
        }


    }
}
