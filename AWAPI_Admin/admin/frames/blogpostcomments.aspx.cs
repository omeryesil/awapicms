using System;
using System.Collections;
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
    public partial class blogpostcomments : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.BlogLibrary _blogLib = new AWAPI_BusinessLibrary.library.BlogLibrary();

        Int64 PostId
        {
            get
            {
                if (Request["postid"] == null)
                    return 0;
                return Convert.ToInt64(Request["postid"]);
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _msg.Text = "";

            if (!IsPostBack)
            {
                ResetControls();
                PopulateComments();

                if (Request["commentid"] != null)
                    PopulateComment(Convert.ToInt64(Request["commentid"]));
            }
        }

        void PopulateComments()
        {
            _commentList.DataSource = null;

            var list = _blogLib.GetBlogPostCommentListByPostId(PostId);

            if (_searchStatus.SelectedValue == "")
                _commentList.DataSource = list;
            else
            {
                int status = Convert.ToInt32(_searchStatus.SelectedValue);
                var list2 = from c in list
                            where c.status.Equals(status)
                            select c;
                if (list2 != null)
                    _commentList.DataSource = list2.ToList();
            }
            _commentList.DataBind();

        }

        void ResetControls()
        {
            _id.Text = "";
            _userId.Text = App_Code.SessionInfo.CurrentUser.userId.ToString();
            _userName.Text = App_Code.SessionInfo.CurrentUser.username;
            _firstName.Text = App_Code.SessionInfo.CurrentUser.firstName;
            _lastName.Text = App_Code.SessionInfo.CurrentUser.lastName;
            _email.Text = App_Code.SessionInfo.CurrentUser.email;
            _title.Text = "";
            _description.Text = "";

            ShowHideControls(false);
        }

        void ShowHideControls(bool show)
        {
            ShowHideControl(btnDeleteBlogPostComment_, show);
        }

        protected void btnNewCustomField_Click(object sender, ImageClickEventArgs e)
        {
            ResetControls();
        }

        protected void btnSaveCustomField_Click(object sender, ImageClickEventArgs e)
        {
            AWAPI_Data.Data.awBlogPostComment comment = new AWAPI_Data.Data.awBlogPostComment();
            Int64 id = 0;

            try
            {
                comment.title = _title.Text;
                comment.description = _description.Text;
                comment.status = Convert.ToInt32(_status.SelectedValue);
                comment.firstName = _firstName.Text;
                comment.lastName = _lastName.Text;
                comment.email = _email.Text;
                if (!String.IsNullOrEmpty(_userId.Text))
                    comment.userId = Convert.ToInt64(_userId.Text);
                comment.userName = _userName.Text;

                if (_id.Text.Trim() == "")  //add new 
                {
                    id = _blogLib.AddBlogPostComment(PostId,
                                            comment.userId,
                                            comment.title,
                                            comment.description,
                                            comment.email,
                                            comment.userName,
                                            comment.firstName,
                                            comment.lastName,
                                            comment.status);
                }
                else  //Update  
                {
                    id = Convert.ToInt64(_id.Text);
                    _blogLib.UpdateBlogPostComment(id,
                            comment.title,
                            comment.description,
                            comment.email,
                            comment.userName,
                            comment.firstName,
                            comment.lastName,
                            comment.status);
                }
                _id.Text = id.ToString();

                PopulateComments();
                ShowHideControls(true);

                App_Code.Misc.WriteMessage(_msg, "Comment has been saved.");
            }
            catch (Exception ex)
            {
                App_Code.Misc.WriteMessage(App_Code.Misc.MessageType.ERROR, _msg, ex.Message);
            }

        }

        protected void _name0_TextChanged(object sender, EventArgs e)
        {

        }

        protected void _commentList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            _commentList.PageIndex = e.NewPageIndex;
            PopulateComments();
        }

        protected void btnDeleteComment_Click(object sender, ImageClickEventArgs e)
        {
            _blogLib.DeleteBlogPostComment(Convert.ToInt64(_id.Text));
            ResetControls();
            PopulateComments();

            App_Code.Misc.WriteMessage(_msg, "Comment has been deleted.");
        }

        protected void _commentList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "editcomment":
                    PopulateComment(Convert.ToInt64(e.CommandArgument.ToString()));
                    break;

            }
        }

        void PopulateComment(long commentId)
        {
            ResetControls();
            AWAPI_Data.Data.awBlogPostComment comment = _blogLib.GetBlogPostComment(commentId);
            if (comment == null)
                return;

            _id.Text = commentId.ToString();
            _title.Text = comment.title;
            _description.Text = comment.description;
            _status.SelectedValue = comment.status.ToString();
            _firstName.Text = comment.firstName;
            _lastName.Text = comment.lastName;
            _email.Text = comment.email;
            _userId.Text = comment.userId.ToString();
            _userName.Text = comment.userName;

            ShowHideControls(true);
        }

        protected void _searchStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateComments();
        }

        protected void _commentList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                int status = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "status").ToString());

                switch (status)
                { 
                    case 0:
                        lblStatus.CssClass = "statusPending";
                        lblStatus.Text = "pending";
                        break;
                    case 1:
                        lblStatus.CssClass = "statusApproved";
                        lblStatus.Text = "approved";
                        break;
                    case 2:
                        lblStatus.CssClass = "statusRejected";
                        lblStatus.Text = "rejected";
                        break;
                }




            
            }
        }

    }
}
