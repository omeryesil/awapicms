using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace AWAPI.Admin.frames
{
    public partial class contestentries : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.ContestLibrary _contestLib = new AWAPI_BusinessLibrary.library.ContestLibrary();

        #region PROPERTIES
        Int64 ContestId
        {
            get
            {
                if (Request["contestId"] == null)
                    return 0;
                return Convert.ToInt64(Request["contestId"]);
            }

        }

        Int64 GroupId
        {
            get
            {
                if (Request["GroupId"] == null)
                    return 0;
                return Convert.ToInt64(Request["GroupId"]);
            }

        }

        SortDirection _sortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                {
                    ViewState["sortDirection"] = SortDirection.Ascending;
                    return SortDirection.Ascending;
                }
                SortDirection direction = (SortDirection)ViewState["sortDirection"];
                if (direction == SortDirection.Ascending)
                    direction = SortDirection.Descending;
                else
                    direction = SortDirection.Ascending;

                ViewState["sortDirection"] = direction;
                return direction;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            _msg.Text = "";

            if (!IsPostBack)
            {
                if (ContestId > 0 || GroupId > 0)
                {
                    if (PopulateHeader())
                        PopulateList();
                }
            }
        }

        bool PopulateHeader()
        {
            if (GroupId > 0)
            {
                AWAPI_Data.Data.awContestGroup group = _contestLib.GetContestGroup(GroupId);
                if (group == null)
                    return false;

                lblTitle.Text = "Contest Group Entries - " + group.title;
                _hplExportExcel.NavigateUrl = "~/admin/reports/handlers/excel.ashx?report=" +
                                      AWAPI.admin.reports.handlers.excel.Reports.ContestGroupEntries + "&groupid=" + GroupId.ToString();


            }
            else
            {
                AWAPI_Data.CustomEntities.ContestExtended contest = _contestLib.GetContest(ContestId, false);
                if (contest == null)
                    return false;

                lblTitle.Text = "Contest Entries - " + contest.title;
                _hplExportExcel.NavigateUrl = "~/admin/reports/handlers/excel.ashx?report=" +
                                      AWAPI.admin.reports.handlers.excel.Reports.ContestEntries + "&contestid=" + ContestId.ToString();
            }

            return true;
        }

        void PopulateList()
        {
            PopulateList("");
        }

        void PopulateList(string sortName)
        {
            _todaysEntries.Text = "0";
            _totalEntries.Text = "0";

            _list.DataSource = null;
            IList<AWAPI_Data.CustomEntities.ContestEntryExtended> lst = null;
            if (GroupId > 0)
                lst = _contestLib.GetContestEntryListByGroupId(GroupId, _keywordSearch.Text.Trim().ToLower(), null);
            else
                lst = _contestLib.GetContestEntryList(ContestId, null, _keywordSearch.Text.Trim().ToLower(), null);

            if (lst != null)
            {
                switch (sortName.ToLower())
                {
                    case "email":
                        if (_sortDirection == SortDirection.Ascending)
                            _list.DataSource = lst.OrderBy(l => l.email).ToArray();
                        else
                            _list.DataSource = lst.OrderByDescending(l => l.email).ToArray();
                        break;

                    case "firstname":
                        if (_sortDirection == SortDirection.Ascending)
                            _list.DataSource = lst.OrderBy(l => l.firstName).ToArray();
                        else
                            _list.DataSource = lst.OrderByDescending(l => l.firstName).ToArray();
                        break;

                    case "lastname":
                        if (_sortDirection == SortDirection.Ascending)
                            _list.DataSource = lst.OrderBy(l => l.lastName).ToArray();
                        else
                            _list.DataSource = lst.OrderByDescending(l => l.lastName).ToArray();
                        break;

                    case "createdate":
                        if (_sortDirection == SortDirection.Ascending)
                            _list.DataSource = lst.OrderBy(l => l.createDate).ToArray();
                        else
                            _list.DataSource = lst.OrderByDescending(l => l.createDate).ToArray();
                        break;

                    default:
                        _list.DataSource = lst;
                        break;

                }

                if (lst != null)
                {
                    _totalEntries.Text = lst.Count.ToString();

                    DateTime dtTodayStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    var list2 = from l in lst
                                where l.createDate > dtTodayStart && l.createDate <= DateTime.Now
                                select l;
                    if (list2 != null)
                        _todaysEntries.Text = list2.Count().ToString();

                }
            }
            _list.DataBind();
        }



        protected void _list_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            _list.PageIndex = e.NewPageIndex;
            PopulateList();
        }

        protected void _list_Sorting(object sender, GridViewSortEventArgs e)
        {
            PopulateList(e.SortExpression);
        }

        protected void lbtnFilter_Click(object sender, EventArgs e)
        {
            PopulateList();
        }

        protected void lbtnClear_Click(object sender, EventArgs e)
        {
            _keywordSearch.Text = "";
            PopulateList();
        }



    }
}
