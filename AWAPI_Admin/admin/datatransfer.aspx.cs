using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AWAPI_BusinessLibrary.library;
using System.Collections;
namespace AWAPI.admin
{

    public partial class PageDataTransfer : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.EnvironmentLibrary _envLib = new AWAPI_BusinessLibrary.library.EnvironmentLibrary();
        BlogLibrary _blogLib = new BlogLibrary();
        ContentLibrary _contentLib = new ContentLibrary();
        PollLibrary _pollLib = new PollLibrary();
        public const string SERVICEURL_CONTENT = "wcf/ContentService.svc";
        public const string SERVICEURL_FILE = "wcf/FileService.svc";


        public PageDataTransfer()
        {
            ModuleName = "datatransfer";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                PopulateList();
        }

        void ResetControls()
        {
            _serviceUrl.Text = "";
        }

        void PopulateList()
        {

            _environmentList.DataValueField = "environmentid";
            _environmentList.DataTextField = "title";
            _environmentList.DataSource = _envLib.GetList(App_Code.SessionInfo.CurrentSite.siteId);
            _environmentList.DataBind();

            _environmentList.Items.Insert(0, new ListItem("-Please select an environment", "0"));
            _environmentList.SelectedIndex = 0;

        }

        void Populate(long environmentId)
        {
            ResetControls();

            AWAPI_Data.Data.awEnvironment env = _envLib.Get(environmentId);
            if (env == null)
                return;

            _serviceUrl.Text = env.serviceUrl;

        }

        protected void _environmentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Populate(Convert.ToInt64(_environmentList.SelectedValue));
        }


        protected void _module_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (_module.SelectedValue.ToLower().Trim())
            {
                case "contents":
                    _mvRemoteServer.SetActiveView(_viewRemoteServerContent);
                    break;
                case "resourcegroups":
                    break;
                case "resources":
                    break;
            }
        }

        protected void btnConnect_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                switch (_module.SelectedValue.ToLower().Trim())
                {
                    case "contents":
                        LoadCurrentContentList();
                        LoadRemoteContentList();
                        PopulateRemoteContents();
                        PopulateCurrentContents();
                        break;
                    case "resourcegroups":
                        break;
                    case "resources":
                        break;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                    errorMessage += "<br/>" +
                            ex.InnerException.Message;
                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, errorMessage);

            }
        }

        #region

        string GetFullUrl(string baseUrl, string service)
        {
            string url = baseUrl.Trim();
            if (!url.EndsWith("/"))
                url += "/";
            url += service.Trim();
            return url;
        }

        IList<AWAPI_Data.Data.awContent> _currentServerContentListSource;
        ArrayList _currentServerContentList = new ArrayList();

        AWAPI_Data.CustomEntities.ContentExtended[] _remoteServerContentListSource;
        ArrayList _remoteServerContentList = new ArrayList();

        public void LoadRemoteContentList()
        {
            long siteId = App_Code.SessionInfo.CurrentSite.siteId;
            string url = GetFullUrl(_serviceUrl.Text, SERVICEURL_CONTENT);
            AWAPI_ContentService.ContentServiceClient client = new AWAPI.AWAPI_ContentService.ContentServiceClient("basicHttp_ContentService", url);
            _remoteServerContentListSource = client.GetList(siteId, "", "lineage, sortorder");

        }

        public void LoadCurrentContentList()
        {
            long siteId = App_Code.SessionInfo.CurrentSite.siteId;
            _currentServerContentListSource = _contentLib.GetList(siteId, "", "");
        }


        #region REMOTE CONTENT

        void PopulateRemoteContents()
        {
            _gvRemoteServerList.DataSource = null;

            if (_remoteServerContentListSource != null)
                AddRemoteChildContent(0);

            _gvRemoteServerList.DataSource = _remoteServerContentList;
            _gvRemoteServerList.DataBind();

        }

        void AddRemoteChildContent(long parentContentId)
        {
            var tmp = from l in _remoteServerContentListSource
                      where l.parentContentId == parentContentId
                      orderby l.sortOrder
                      select l;

            if (tmp != null)
            {
                foreach (AWAPI_Data.CustomEntities.ContentExtended content in tmp)
                {
                    _remoteServerContentList.Add(content);
                    AddRemoteChildContent(content.contentId);
                }
            }
        }
        #endregion

        #region CURRENT CONTENT

        void PopulateCurrentContents()
        {
            _gvCurrentServerList.DataSource = null;

            if (_currentServerContentListSource != null)
                AddCurrentChildContent(0);

            _gvCurrentServerList.DataSource = _currentServerContentList;
            _gvCurrentServerList.DataBind();

        }

        void AddCurrentChildContent(long parentContentId)
        {
            var tmp = from l in _currentServerContentListSource
                      where l.parentContentId == parentContentId
                      orderby l.sortOrder
                      select l;

            if (tmp != null)
            {
                foreach (AWAPI_Data.Data.awContent content in tmp)
                {
                    _currentServerContentList.Add(content);
                    AddCurrentChildContent(content.contentId);
                }
            }
        }
        #endregion



        #endregion

        protected void _remoteServerList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                long id = (long)DataBinder.Eval(e.Row.DataItem, "contentId");
                AWAPI_Data.CustomEntities.ContentExtended contentOnRemote = (from l in _remoteServerContentListSource
                                                                             where l.contentId.Equals(id)
                                                                             select l).FirstOrDefault<AWAPI_Data.CustomEntities.ContentExtended>();

                LinkButton lbtnAlias = (LinkButton)e.Row.FindControl("lbtnContent_Alias");
                lbtnAlias.ForeColor = System.Drawing.Color.Gray;

                //FIND THE DIFFERENCES -------------------------------------------------------------------
                if (_currentServerContentList == null)
                    lbtnAlias.ForeColor = System.Drawing.Color.Red;
                else
                {
                    AWAPI_Data.Data.awContent currentContent = (from l in _currentServerContentListSource
                                                                where l.contentId.Equals(id)
                                                                select l).FirstOrDefault<AWAPI_Data.Data.awContent>();
                    if (currentContent == null)
                        lbtnAlias.ForeColor = System.Drawing.Color.Green;
                    else if (contentOnRemote.alias != currentContent.alias ||
                        contentOnRemote.contentType != currentContent.contentType ||
                        contentOnRemote.description != currentContent.description ||
                        contentOnRemote.eventEndDate != currentContent.eventEndDate ||
                        contentOnRemote.eventStartDate != currentContent.eventStartDate ||
                        contentOnRemote.imageurl != currentContent.imageurl ||
                        contentOnRemote.isCommentable != currentContent.isCommentable ||
                        contentOnRemote.isEnabled != currentContent.isEnabled ||
                        contentOnRemote.lineage != currentContent.lineage ||
                        contentOnRemote.link != currentContent.link ||
                        contentOnRemote.parentContentId != currentContent.parentContentId ||
                        contentOnRemote.pubDate != currentContent.pubDate ||
                        contentOnRemote.pubEndDate != currentContent.pubEndDate ||
                        contentOnRemote.sortOrder != currentContent.sortOrder ||
                        contentOnRemote.title != currentContent.title)
                        lbtnAlias.ForeColor = System.Drawing.Color.Red;
                }


                if (!String.IsNullOrEmpty(contentOnRemote.lineage))
                {
                    string[] lineageParts = contentOnRemote.lineage.Split('|');
                    string space = "";
                    for (int n = 0; n < lineageParts.Length - 1; n++)
                        space += "&nbsp;&nbsp;";

                    lbtnAlias.Text = space + contentOnRemote.alias;


                }
            }
        }

        protected void _gvCurrentServerList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                long id = (long)DataBinder.Eval(e.Row.DataItem, "contentId");
                AWAPI_Data.Data.awContent current = (from l in _currentServerContentListSource
                                                     where l.contentId.Equals(id)
                                                     select l).FirstOrDefault<AWAPI_Data.Data.awContent>();

                LinkButton lbtnAlias = (LinkButton)e.Row.FindControl("lbtnContent_Alias");
                lbtnAlias.ForeColor = System.Drawing.Color.Gray;

                //FIND THE DIFFERENCES -------------------------------------------------------------------
                if (_currentServerContentList == null)
                    lbtnAlias.ForeColor = System.Drawing.Color.Red;
                else
                {
                    AWAPI_Data.CustomEntities.ContentExtended rmt = (from l in _remoteServerContentListSource
                                                                     where l.contentId.Equals(id)
                                                                     select l).FirstOrDefault<AWAPI_Data.CustomEntities.ContentExtended>();
                    if (rmt == null)
                        lbtnAlias.ForeColor = System.Drawing.Color.Green;
                    else if (current.alias != rmt.alias ||
                        current.contentType != rmt.contentType ||
                        current.description != rmt.description ||
                        current.eventEndDate != rmt.eventEndDate ||
                        current.eventStartDate != rmt.eventStartDate ||
                        current.imageurl != rmt.imageurl ||
                        current.isCommentable != rmt.isCommentable ||
                        current.isEnabled != rmt.isEnabled ||
                        current.lineage != rmt.lineage ||
                        current.link != rmt.link ||
                        current.parentContentId != rmt.parentContentId ||
                        current.pubDate != rmt.pubDate ||
                        current.pubEndDate != rmt.pubEndDate ||
                        current.sortOrder != rmt.sortOrder ||
                        current.title != rmt.title)
                        lbtnAlias.ForeColor = System.Drawing.Color.Red;
                }


                if (!String.IsNullOrEmpty(current.lineage))
                {
                    string[] lineageParts = current.lineage.Split('|');
                    string space = "";
                    for (int n = 0; n < lineageParts.Length - 1; n++)
                        space += "&nbsp;&nbsp;";

                    lbtnAlias.Text = space + current.alias;


                }
            }
        }

        protected void _gvRemoteServerList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "copycontent":
                    CopyContents(Convert.ToInt64(e.CommandArgument.ToString()));
                    break;


            }
        }

        void CopyContents(long sourceContentId)
        {
            LoadRemoteContentList();

            //Update the selected content
            CopyContentFromRemoteToCurrentServer(sourceContentId);

            //Update the childs 
            var childs = from l in _remoteServerContentListSource
                         where l.lineage.IndexOf(sourceContentId.ToString()) > 0
                         orderby l.lineage, l.sortOrder
                         select l;
            if (childs != null && childs.Count() > 0)
            {
                IList<AWAPI_Data.CustomEntities.ContentExtended> list = childs.ToList();
                foreach (AWAPI_Data.CustomEntities.ContentExtended rmt in list)
                    CopyContentFromRemoteToCurrentServer(rmt.contentId);
            }

            LoadCurrentContentList();
            LoadRemoteContentList();

            PopulateRemoteContents();
            PopulateCurrentContents();

        }

        void CopyContentFromRemoteToCurrentServer(long sourceContentId)
        {
            //GET FROM THE REMOTE 
            AWAPI_Data.CustomEntities.ContentExtended rmt = (from l in _remoteServerContentListSource
                                                             where l.contentId.Equals(sourceContentId)
                                                             select l).FirstOrDefault();

            //GET THE CURRENT
            bool addNew = false;
            AWAPI_Data.Data.awContent cur = _contentLib.Get(sourceContentId);
            if (cur == null)
            {
                addNew = true;
                cur = new AWAPI_Data.Data.awContent();
            }

            cur.contentId = rmt.contentId;
            cur.alias = rmt.alias;
            cur.siteId = rmt.siteId;
            cur.description = rmt.description;
            cur.title = rmt.title;
            cur.isEnabled = rmt.isEnabled;
            cur.sortOrder = rmt.sortOrder;
            cur.link = rmt.link;
            cur.imageurl = rmt.imageurl;
            cur.isCommentable = rmt.isCommentable;
            cur.lineage = rmt.lineage;
            cur.parentContentId = rmt.parentContentId;
            cur.pubDate = rmt.pubDate;
            cur.pubEndDate = rmt.pubEndDate;
            cur.eventStartDate = rmt.eventStartDate;
            cur.eventEndDate = rmt.eventEndDate;
            cur.contentType = rmt.contentType;

            if (addNew)
                _contentLib.Add(cur.contentId, cur.alias, cur.title, cur.description, cur.contentType, cur.siteId,
                            App_Code.SessionInfo.CurrentUser.userId, cur.parentContentId, cur.eventStartDate, cur.eventEndDate,
                            cur.link, cur.imageurl, cur.sortOrder, cur.isEnabled, cur.isCommentable, cur.pubDate,
                            cur.pubEndDate);
            else
                _contentLib.Update(cur.contentId, cur.alias, cur.title, cur.description, cur.contentType, App_Code.SessionInfo.CurrentUser.userId,
                                    cur.parentContentId, cur.eventStartDate, cur.eventEndDate, cur.link, cur.imageurl, cur.sortOrder, cur.isEnabled,
                                    cur.isCommentable, cur.pubDate, cur.pubEndDate);

            CopyContentCustomFieldsFromRemoteToCurrentServer(cur.contentId, App_Code.SessionInfo.CurrentSite.cultureCode);

        }

        void CopyContentCustomFieldsFromRemoteToCurrentServer(long contentId, string cultureCode)
        {
            string url = GetFullUrl(_serviceUrl.Text, SERVICEURL_CONTENT);
            AWAPI_ContentService.ContentServiceClient client = new AWAPI.AWAPI_ContentService.ContentServiceClient("basicHttp_ContentService", url);
            AWAPI_Data.CustomEntities.ContentCustomField[] customFields = client.GetFieldList(contentId);
            AWAPI_Data.CustomEntities.ContentCustomFieldValueExtended[] values = client.GetFieldValueList(contentId, cultureCode);

            //Add Fields
            if (customFields == null || customFields.Length == 0)
                return;

            ContentCustomFieldLibrary lib = new ContentCustomFieldLibrary();
            foreach (AWAPI_Data.CustomEntities.ContentCustomField cf in customFields)
            {
                AWAPI_Data.Data.awContentCustomField fld = lib.GetField(cf.customFieldId);
                if (cf.fieldContentId != contentId)
                    break;

                if (fld == null)
                    fld = new AWAPI_Data.Data.awContentCustomField();

                fld.contentId = contentId;
                fld.customFieldId = cf.customFieldId;
                fld.title = cf.title;
                fld.description = cf.description;
                fld.applyToSubContents = cf.applyToSubContents;
                fld.fieldType = cf.fieldType;
                fld.maximumLength = cf.maximumLength;
                fld.maximumValue = cf.maximumValue;
                fld.minimumValue = cf.minimumValue;
                fld.defaultValue = cf.defaultValue;
                fld.regularExpression = cf.regularExpression;
                fld.sortOrder = cf.sortOrder;
                fld.isEnabled = cf.isEnabled;
                fld.lastBuildDate = DateTime.Now;

                //lib.SaveField(true, fld);
            }

            //Add Values
            if (values != null && values.Length > 0)
                foreach (AWAPI_Data.CustomEntities.ContentCustomFieldValueExtended value in values)
                {
                    lib.UpdateFieldValue(value.contentId, value.customFieldId,
                                        App_Code.SessionInfo.CurrentUser.userId,
                                        value.fieldValue, "");
                }
        }


        protected void _gvCurrentServerList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "deletecontent":
                    _contentLib.Delete(Convert.ToInt64(e.CommandArgument.ToString()));
                    LoadCurrentContentList();
                    LoadRemoteContentList();

                    PopulateCurrentContents();
                    PopulateRemoteContents();
                    break;



            }
        }


    }
}
