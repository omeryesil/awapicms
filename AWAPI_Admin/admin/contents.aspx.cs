using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AWAPI_Data.Data;

namespace AWAPI.Admin
{
    public partial class PageContents : AWAPI.App_Code.AdminBasePage
    {
        #region consts
        const string FORMAT_SELECTED_START = "<b><font color='#0000ff'>";
        const string FORMAT_SELECTED_END = "</font></b>";
        const string FORMAT_CONTENT_SAVED = " (<i>saved</i>)";
        const string FORMAT_DISABLED_START = "<font color='#bbbbbb'><i>";
        const string FORMAT_DISABLED_END = "</i></font>";
        //const string FORMAT_ENABLED_IMAGE = "<img  src='img/legend/enabled-legend.gif'/>&nbsp;";
        //const string FORMAT_DISABLED_IMAGE = "<img src='img/legend/disabled-legend.gif'/>&nbsp;";
        //const string FORMAT_PUBLISHED_IMAGE = "<img style='border:none'  src='img/legend/published-legend.gif'/>&nbsp;";
        //const string FORMAT_NOT_PUBLISHED_IMAGE = "<img style='border:none'  src='img/legend/published-disabled-legend.gif'/>&nbsp;";
        const string FORMAT_CONTENTTYPE_SYSTEM = "<img style='border:none'  src='img/legend/system-legend.gif'/>&nbsp;";
        const string FORMAT_CONTENTTYPE_CONTENT = "<img style='border:none'  src='img/legend/blog-legend.gif'/>&nbsp;";
        const string FORMAT_CONTENTTYPE_BLOG = "<img style='border:none'  src='img/legend/blog-legend.gif'/>&nbsp;";

        #endregion

        #region members
        AWAPI_BusinessLibrary.library.ContentLibrary _contentLib = new AWAPI_BusinessLibrary.library.ContentLibrary();
        AWAPI_BusinessLibrary.library.ContentCustomFieldLibrary _customLib = new AWAPI_BusinessLibrary.library.ContentCustomFieldLibrary();
        IList<awContent> _contentList;
        System.Collections.Generic.IList<AWAPI_Data.CustomEntities.ContentCustomFieldValueExtended> _valueList = null;

        public long _parentContentId
        {
            get
            {
                if (ViewState["parentcontentid"] != null)
                    return Convert.ToInt64(ViewState["parentcontentid"]);

                if (Request["contentid"] != null)
                    return Convert.ToInt64(Request["contentid"]);

                return 0;
            }
            set
            {
                ViewState["parentcontentid"] = value;
            }
        }

        #endregion

        public PageContents()
        {
            ModuleName = "content";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateContentList();
                PopulateLanguges();
                PopulateContentParentList(0);
                PopulateTags();
                ResetControls();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //if (App_Code.SessionInfo.CurrentSite == null || App_Code.SessionInfo.CurrentSite.siteId <= 0)
            //{
            //    AdminMaster.WriteIframeMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, "You need to select a poll before managing groups. <br/>" +
            //                "Please click <a href='javascript:void(0);' onclick=\"$.fn.colorbox({href:'/admin/frames/selectsite.aspx', open:true, width:'470px', height:'450px', iframe:true});\">here</a> to select a poll.");
            //    btnSaveContent_.Visible = false;
            //    return;
            //}

            PopulateBreadCrumb();

            RegisterCustomScript();
            AddScriptToTagList();
        }

        void AddScriptToTagList()
        {
            foreach (ListItem li in _tagList.Items)
            {
                li.Attributes.Add("name", "tagitem");
                li.Attributes.Add("tag", li.Text);
                li.Attributes.Add("onclick", "setElementFromCheckBoxList('tagitem', '" + _tagList.ClientID + "', '" + _contentTags.ClientID + "')");
            }
        }

        void RegisterCustomScript()
        {
            System.Text.StringBuilder script = new System.Text.StringBuilder();

            //if (_contentId.Text != "")
            //    script.Append("parent.location.hash = '" + _contentId.Text + "'");

            script.Append("$(document).ready(function(){ \n");
            script.Append("    $(\"a[rel='managecustomfields']\").colorbox({width:'960', height:'565', iframe:true}); \n");
            script.Append("    $(\"a[rel='selectfile']\").colorbox({width:'960', height:'620', iframe:true}); \n");
            script.Append("    $(\"a[rel='managecontentform']\").colorbox({width:'960', height:'620', iframe:true}); \n");
            script.Append("    aliasNameTrigger('" + _alias.ClientID + "', '" + _title.ClientID + "'); \n");
            script.Append("}); \n");

            if (script.Length > 0)
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "", script.ToString(), true);

        }

        void PopulateLanguges()
        {
            AWAPI_BusinessLibrary.library.CultureLibrary lib = new AWAPI_BusinessLibrary.library.CultureLibrary();
            _culture.DataSource = lib.GetListBySiteId(App_Code.SessionInfo.CurrentSite.siteId);
            _culture.DataValueField = "cultureCode";
            _culture.DataTextField = "title";
            _culture.DataBind();

            _culture.Items.Insert(0, new ListItem("None", ""));
            if (!String.IsNullOrEmpty(App_Code.SessionInfo.CurrentSite.cultureCode) &&
                _culture.Items.FindByValue(App_Code.SessionInfo.CurrentSite.cultureCode) != null)
            {
                _culture.SelectedValue = App_Code.SessionInfo.CurrentSite.cultureCode;
                _culture.SelectedItem.Text = _culture.SelectedItem.Text + " (default)";
            }
            else
                _culture.SelectedIndex = 0;
        }

        void FillContentList(bool updateFromDB)
        {
            if (updateFromDB || _contentList == null)
                _contentList = _contentLib.GetList(App_Code.SessionInfo.CurrentSite.siteId, "", "");
        }

        void ResetControls()
        {
            _contentId.Text = "";
            _alias.Text = "";
            _title.Text = "";
            _description.Value = "";
            _contentEventStartDate.Text = "";
            _contentEventEndDate.Text = "";
            _contentImageUrl.Text = "";
            _contentImage.ImageUrl = "";
            _contentPublishStartDate.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm"); ;
            _contentPublishEndDate.Text = "";
            _contentSortOrder.Text = "1";
            _contentIsCommentable.Checked = false;
            _contentImage.Attributes.Add("style", "display:none;");
            //_contentType.Text = TypeValues.ContentTypes.content.ToString();
            _lblBreadCrumbCurrentContent.Text = "";

            //reset custom fields 
            customFieldsList.DataSource = null;
            customFieldsList.DataBind();

            //reset tag list 
            foreach (ListItem li in _tagList.Items)
                li.Selected = false;


            ShowHideContentButtons(false, "");
        }

        void ShowHideContentButtons(bool show, string contentType)
        {
            ShowHideControl(_methodsTitle, show);

            ShowHideControl(_customFieldLink, show);
            ShowHideControl(_customFieldLinkText, show);

            ShowHideControl(btnDeleteContent_, show);
            ShowHideControl(lblDeleteContent_, show);

            ShowHideControl(hplContentForm_, show);
            ShowHideControl(lblContentForm_, show);

            //ShowHideControl(btnSaveContent_, show);
            //ShowHideControl(lblSaveContent_, show);

            _culture.Enabled = show;

            _methodGet.Visible = show;
            _methodGetList.Visible = show;
            _methodType.Visible = show;

            //show hide controls based on the blog type --------------            
            _alias.Enabled = true;
            _title.Enabled = true;

            _contentParentList.Enabled = true;
            _contentPublishStartDate.Enabled = true;

            _contentPublishEndDate.Enabled = true;
            _contentEventStartDate.Enabled = true;

            _contentEventEndDate.Enabled = true;
            _contentTags.Enabled = true;

            _description.Visible = true;
            _contentIsCommentable.Enabled = true;

            cbStatusContent_.Enabled = true;
            _contentSortOrder.Enabled = true;

            _contentImageUrl.Enabled = true;
            _expandTags.Visible = true;

            if (show)
            {
                _customFieldLink.NavigateUrl = "~/admin/frames/managecustomfields.aspx?contentid=" + _contentId.Text;
                hplContentForm_.NavigateUrl = "~/admin/frames/managecontentform.aspx?contentid=" + _contentId.Text;

            }
            else
            {
                _methodGet.NavigateUrl = "";
                _methodGetList.NavigateUrl = "";
            }
        }

        void SetMethodLinks()
        {
            string siteId = App_Code.SessionInfo.CurrentSite.siteId.ToString();
            _methodGet.NavigateUrl = AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.contentServiceUrl + "?method=get&siteid=" + siteId + "&contentid=" + _contentId.Text + "&type=" + _methodType.SelectedValue;
            _methodGetList.NavigateUrl = AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.contentServiceUrl + "?method=getlist&deep=0&siteid=" + siteId + "&contentid=" + _contentId.Text + "&type=" + _methodType.SelectedValue;
        }

        #region CONTENT TREE
        string _selectedContentLineage = "";

        void PopulateContentList()
        {
            PopulateContentList(_parentContentId);
        }

        protected void _culture_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_contentId.Text != "")
                PopulateContent(Convert.ToInt64(_contentId.Text), _culture.SelectedValue);
        }



        void PopulateContentList(long parentContentId)
        {
            gwContentList.DataSource = null;
            twContent.Nodes.Clear();
            FillContentList(false);

            //PopulateContentList first level
            var parentContents = from c in _contentList
                                 where (parentContentId > 0 && c.parentContentId == parentContentId) ||
                                        (parentContentId <= 0 && (c.parentContentId == 0 || c.parentContentId == null))
                                 orderby c.pubDate descending
                                 orderby c.eventStartDate descending
                                 orderby c.sortOrder
                                 select c;

            _selectedContentLineage = "";
            foreach (AWAPI_Data.Data.awContent content in parentContents)
            {
                bool currentPublished = false;

                if (content.pubDate == null || content.pubDate <= DateTime.Now &&
                    content.pubEndDate == null || content.pubEndDate >= DateTime.Now)
                    currentPublished = true;

                TreeNode nd = new TreeNode();
                nd.ImageUrl = "~/admin/img/button/1616/folder-icon.png";
                nd.Text = FormatContentNode(content.contentId, content.alias, content.contentType, true, content.isEnabled, currentPublished);
                nd.Value = content.contentId.ToString();
                twContent.Nodes.Add(nd);

                if (content.contentId.ToString() == _contentId.Text)
                    _selectedContentLineage = content.lineage;

                bool hasChild = PopulateChildNodes(nd, content.contentId, content.isEnabled, currentPublished);
                if (!hasChild)
                    twContent.FindNode(nd.ValuePath).ImageUrl = "~/admin/img/button/1616/document-icon.png";

                if (_selectedContentLineage.Trim().Length > 0 && _selectedContentLineage.IndexOf(content.contentId.ToString() + "|") >= 0)
                    nd.Expand();
                else
                    nd.Collapse();
            }
        }

        bool PopulateChildNodes(TreeNode parentNode, long parentNodeId, bool parentEnabled, bool parentPublished)
        {
            if (parentNode == null)
                return false;

            var childContents = from c in _contentList
                                where c.parentContentId.Equals(parentNodeId)
                                orderby (c.pubDate) descending
                                orderby (c.eventStartDate) descending
                                orderby (c.sortOrder)
                                select c;

            if (childContents == null || childContents.Count() == 0)
                return false;

            foreach (awContent content in childContents.ToList())
            {
                bool currentPublished = false;

                if (content.pubDate == null || content.pubDate <= DateTime.Now &&
                    content.pubEndDate == null || content.pubEndDate >= DateTime.Now)
                    currentPublished = true;

                TreeNode nd = new TreeNode();
                nd.ImageUrl = "~/admin/img/button/1616/folder-icon.png";
                nd.Text = FormatContentNode(content.contentId, content.alias, content.contentType, parentEnabled, content.isEnabled, parentPublished && currentPublished);
                nd.Value = content.contentId.ToString();
                parentNode.ChildNodes.Add(nd);
                if (content.contentId.ToString() == _contentId.Text)
                    _selectedContentLineage = content.lineage;

                bool hasChild = PopulateChildNodes(nd, content.contentId, content.isEnabled, currentPublished);
                if (!hasChild)
                    twContent.FindNode(nd.ValuePath).ImageUrl = "~/admin/img/button/1616/document-icon.png";

                if (_selectedContentLineage.Trim().Length > 0 && _selectedContentLineage.IndexOf(content.contentId.ToString() + "|") >= 0)
                    nd.Expand();
                else
                    nd.Collapse();
            }
            return true;
        }

        string FormatContentNode(long contentId, string title, string contentType, bool parentEnabled, bool isEnabled, bool currentPublished)
        {
            string rtn = title;

            if (_contentId.Text == contentId.ToString())
                rtn = FORMAT_SELECTED_START + title + FORMAT_SELECTED_END;

            if (String.IsNullOrEmpty(rtn) || rtn.Trim() == "")
                rtn = "undefined";

            if (!isEnabled || !parentEnabled || !currentPublished)
                rtn = FORMAT_DISABLED_START + rtn + FORMAT_DISABLED_END;

            rtn = "&nbsp;&nbsp;" + rtn;
            //if (currentPublished)
            //    rtn = FORMAT_PUBLISHED_IMAGE + rtn;
            //else
            //    rtn = FORMAT_NOT_PUBLISHED_IMAGE + rtn;

            //if (parentEnabled && isEnabled)
            //    rtn = FORMAT_ENABLED_IMAGE + rtn;
            //else
            //    rtn = FORMAT_DISABLED_IMAGE + rtn;

            switch (contentType.Trim().ToLower())
            {
                case "system":
                    rtn = FORMAT_CONTENTTYPE_SYSTEM + rtn;
                    break;
                case "blog":
                    rtn = FORMAT_CONTENTTYPE_BLOG + rtn;
                    break;
                case "blogpost":
                    break;
                case "content":
                    //rtn = FORMAT_CONTENTTYPE_CONTENT + rtn;
                    break;
            }

            return rtn;

        }

        void HighlightContentNode(string command, TreeNode ndSource, TreeNodeCollection ndCollection)
        {
            if (ndSource.Value == "0")
                ndSource.Value = _contentId.Text;

            for (int row = 0; row < ndCollection.Count; row++)
            {
                TreeNode nd = ndCollection[row];
                nd.Text = nd.Text
                                .Replace(FORMAT_SELECTED_START, "")
                                .Replace(FORMAT_SELECTED_END, "")
                                .Replace(FORMAT_CONTENT_SAVED, "");

                if (nd.Value == ndSource.Value)
                {
                    nd.Select();
                    switch (command.ToLower())
                    {
                        case "selected":
                            nd.Text = FORMAT_SELECTED_START + nd.Text + FORMAT_SELECTED_END;
                            break;

                        case "deleted":
                            ndCollection.Remove(nd);
                            return;
                            break;

                        case "saved":
                            nd.Text = "&nbsp;&nbsp;" + FORMAT_SELECTED_START + ndSource.Text + FORMAT_SELECTED_END + FORMAT_CONTENT_SAVED;
                            nd.Value = ndSource.Value;
                            break;
                    }
                }
                HighlightContentNode(command, ndSource, nd.ChildNodes);
            }
        }

        protected void twContent_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (twContent.SelectedNode != null)
            {
                PopulateContent(Convert.ToInt64(twContent.SelectedValue));
                HighlightContentNode("selected", twContent.SelectedNode, twContent.Nodes);
            }
        }
        #endregion

        /// <summary>
        /// Populates breadcrumb
        /// </summary>
        /// <param name="contentId"></param>
        void PopulateBreadCrumb()
        {
            long contentId = 0;
            if (_contentId.Text != "")
                contentId = Convert.ToInt64(_contentId.Text);
            else
                contentId = _parentContentId;

            _rptBreadCrumb.DataSource = null;

            if (contentId > 0 && _contentList != null)
            {
                var list = from c in _contentList.ToList()
                           where c.contentId.Equals(contentId)
                           select c;
                if (list != null)
                {
                    awContent cont = list.FirstOrDefault();
                    string lineage = cont.lineage.Trim();
                    System.Collections.ArrayList ids = new System.Collections.ArrayList();

                    if (!String.IsNullOrEmpty(lineage))
                    {
                        string[] parentIds = lineage.Split('|');


                        for (int i = 0; i < parentIds.Length; i++)
                            if (!String.IsNullOrEmpty(parentIds[i]))
                                ids.Add(Convert.ToInt64(parentIds[i]));

                    }
                    //ids.Add(cont.contentId);

                    var parentContents = from c in _contentList.ToList()
                                         where ids.Cast<long>().Contains(c.contentId)
                                         select c;
                    _rptBreadCrumb.DataSource = parentContents;
                }
            }
            _rptBreadCrumb.DataBind();

            _lblBreadCrumbCurrentContent.Text = _alias.Text;

        }

        void PopulateContent(Int64 contentId)
        {
            PopulateContent(contentId, null);
        }

        void PopulateContent(Int64 contentId, string cultureCode)
        {
            ResetControls();
            if (String.IsNullOrEmpty(cultureCode) && _culture.Items.FindByValue(App_Code.SessionInfo.CurrentSite.cultureCode) != null)
                _culture.SelectedValue = App_Code.SessionInfo.CurrentSite.cultureCode;

            awContent content = _contentLib.Get(contentId, cultureCode);

            if (content == null)
                return;

            _contentId.Text = content.contentId.ToString();
            _alias.Text = content.alias;
            cbStatusContent_.Checked = content.isEnabled;
            _contentSortOrder.Text = content.sortOrder.ToString();
            if (content.eventStartDate != null) _contentEventStartDate.Text = content.eventStartDate.Value.ToString("MM/dd/yyyy HH:mm");
            if (content.eventEndDate != null) _contentEventEndDate.Text = content.eventEndDate.Value.ToString("MM/dd/yyyy HH:mm");
            if (content.pubDate != null) _contentPublishStartDate.Text = content.pubDate.Value.ToString("MM/dd/yyyy HH:mm");
            if (content.pubEndDate != null) _contentPublishEndDate.Text = content.pubEndDate.Value.ToString("MM/dd/yyyy HH:mm");
            if (content.parentContentId == null) content.parentContentId = 0;
            _contentIsCommentable.Checked = content.isCommentable;

            _title.Text = content.title;
            _description.Value = content.description;
            _contentImageUrl.Text = content.imageurl;
            //_contentType.Text = content.contentType;

            SetImage(content.imageurl);

            PopulateCustomFields(contentId, content.parentContentId);
            PopulateContentsTags(contentId);

            //POPULATE OTHER ---------------
            PopulateContentParentList(content.contentId);
            _contentParentList.SelectedValue = content.parentContentId.ToString();

            SetMethodLinks();
            ShowHideContentButtons(true, content.contentType);

        }

        void PopulateContentParentList(Int64 currentContentId)
        {
            _contentParentList.Items.Clear();
            _contentParentList.Items.Add(new ListItem("root", "0"));

            FillContentList(false);
            if (_contentList == null)
                return;

            var parentContents = from c in _contentList
                                 where (_parentContentId > 0 && c.parentContentId == _parentContentId) ||
                                        (_parentContentId <= 0 && (c.parentContentId == 0 || c.parentContentId == null))
                                 orderby c.title
                                 select c;


            foreach (awContent content in parentContents)
            {

                _contentParentList.Items.Add
                            (new ListItem(AddCharacterForLineage(content.lineage) + content.alias, content.contentId.ToString()));
                PopulateContentParentListChilds(content.contentId);
            }
        }

        void PopulateContentParentListChilds(Int64 parentContentId)
        {
            if (parentContentId <= 0)
                return;

            var childContents = from c in _contentList
                                where c.parentContentId.Equals(parentContentId)
                                orderby c.title
                                select c;
            foreach (awContent content in childContents)
            {
                _contentParentList.Items.Add
                        (new ListItem(AddCharacterForLineage(content.lineage) + content.alias, content.contentId.ToString()));
                PopulateContentParentListChilds(content.contentId);
            }

        }

        string AddCharacterForLineage(string lineage)
        {
            System.Text.StringBuilder space = new System.Text.StringBuilder("");
            if (lineage != null && lineage.Trim().Length > 0)
            {
                string[] parentContents = lineage.Split('|');
                for (int i = 1; i < parentContents.Length; i++)
                    space.Append("___");
            }
            else
                return "___";

            return space.ToString();

        }

        protected void btnDeleteContent_Click(object sender, ImageClickEventArgs e)
        {
            DeleteContent(Convert.ToInt64(_contentId.Text));
            AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.INFO, "Content has been deleted.");
        }

        void DeleteContent(long contentId)
        {
            AWAPI_BusinessLibrary.library.ContentLibrary contLib = new AWAPI_BusinessLibrary.library.ContentLibrary();
            contLib.Delete(contentId);

            ResetControls();
            PopulateContentList();
        }

        protected void btnNewContent_Click(object sender, EventArgs e)
        {
            PopulateLanguges();
            ResetControls();
        }

        protected void btnSaveContent_Click(object sender, ImageClickEventArgs e)
        {
            long contentId = _contentId.Text.Trim().Length == 0 ? 0 : Convert.ToInt64(_contentId.Text);
            bool newContent = false;

            awContent content = new awContent();
            content.alias = _alias.Text;
            content.title = _title.Text;
            content.description = _description.Value;
            content.link = "";
            content.imageurl = _contentImageUrl.Text;
            content.userId = App_Code.SessionInfo.CurrentUser.userId;
            content.contentType = AWAPI_Common.values.TypeValues.ContentTypes.content.ToString();
            content.parentContentId = Convert.ToInt64(_contentParentList.SelectedValue);

            if (contentId != 0 &&           //if content is selected
                !String.IsNullOrEmpty(_culture.SelectedValue) &&
                !String.IsNullOrEmpty(App_Code.SessionInfo.CurrentSite.cultureCode) &&
                App_Code.SessionInfo.CurrentSite.cultureCode != _culture.SelectedValue)
            {
                _contentLib.SaveContentLanguage(Convert.ToInt64(_contentId.Text),
                                            _culture.SelectedValue.ToLower(),
                                            content.title, content.description, content.link, content.imageurl, content.userId);
            }
            else
            {
                content.eventStartDate = AWAPI_Common.library.MiscLibrary.ConvertStringToDate(_contentEventStartDate.Text);
                content.eventEndDate = AWAPI_Common.library.MiscLibrary.ConvertStringToDate(_contentEventEndDate.Text, true);
                content.pubDate = AWAPI_Common.library.MiscLibrary.ConvertStringToDate(_contentPublishStartDate.Text);
                content.pubEndDate = AWAPI_Common.library.MiscLibrary.ConvertStringToDate(_contentPublishEndDate.Text, true);
                content.siteId = App_Code.SessionInfo.CurrentSite.siteId;

                if (_contentSortOrder.Text.Trim() != "")
                    content.sortOrder = Convert.ToInt32(_contentSortOrder.Text);
                content.isEnabled = cbStatusContent_.Checked;
                content.isCommentable = _contentIsCommentable.Checked;

                if (contentId == 0)
                {
                    newContent = true;
                    contentId = _contentLib.Add(content.alias, content.title, content.description,
                                content.contentType, content.siteId, content.userId, content.parentContentId,
                                content.eventStartDate, content.eventEndDate,
                                content.link, content.imageurl, content.sortOrder, content.isEnabled, content.isCommentable,
                                content.pubDate, content.pubEndDate);
                    _contentId.Text = contentId.ToString();
                }
                else
                {
                    contentId = Convert.ToInt64(_contentId.Text);
                    _contentLib.Update(contentId, content.alias, content.title, content.description,
                                content.contentType, content.userId, content.parentContentId,
                                content.eventStartDate, content.eventEndDate,
                                content.link, content.imageurl, content.sortOrder, content.isEnabled, content.isCommentable,
                                content.pubDate, content.pubEndDate);

                }
            }
            SaveContentsTags(contentId);
            SaveCustomFields(contentId);

            SetImage(content.imageurl);

            ShowHideContentButtons(true, content.contentType);

            PopulateContentList();
            PopulateContentParentList(content.contentId);
            PopulateContentsTags(contentId);

            if (newContent)
                PopulateCustomFields(contentId, content.parentContentId);

            _contentParentList.SelectedValue = content.parentContentId.ToString();

            AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.INFO, "Content has been saved.");

            HighlightContentNode("saved", new TreeNode(content.alias, content.contentId.ToString()), twContent.Nodes);
        }

        void SetImage(string imageUrl)
        {
            if (!String.IsNullOrEmpty(imageUrl))
            {
                _contentImage.Attributes.Add("style", "display:'';");
                if (imageUrl.ToLower().IndexOf(AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileServiceUrl.ToLower()) >= 0)
                    _contentImage.ImageUrl = imageUrl + "&size=150x150";
                else
                    _contentImage.ImageUrl = imageUrl;
            }

        }

        protected void _methodType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetMethodLinks();
        }

        protected void btnRefreshTree_Click(object sender, ImageClickEventArgs e)
        {
            PopulateContentList();
        }

        protected void btnNewChild_Click(object sender, EventArgs e)
        {
            PopulateLanguges();
            if (_contentId.Text != "")
            {
                long contentId = Convert.ToInt64(_contentId.Text);
                _contentParentList.SelectedValue = contentId.ToString();
                ResetControls();
            }
            else
                ResetControls();
        }

        #region TAGS ----------------------------

        void PopulateTags()
        {
            _tagList.Items.Clear();

            AWAPI_BusinessLibrary.library.TagLibrary tagLib = new AWAPI_BusinessLibrary.library.TagLibrary();

            var list = tagLib.GetList(App_Code.SessionInfo.CurrentSite.siteId, "");
            if (list == null)
                return;

            foreach (awSiteTag tag in list)
            {
                ListItem itm = new ListItem();
                itm.Value = tag.siteTagId.ToString();
                itm.Text = tag.title;
                itm.Attributes.Add("name", "tagitem");
                itm.Attributes.Add("tag", tag.title);

                //function setElementFromCheckBoxList (spanName, listId, targetId)
                itm.Attributes.Add("onclick", "setElementFromCheckBoxList('tagitem', '" + _tagList.ClientID + "', '" + _contentTags.ClientID + "')");

                _tagList.Items.Add(itm);
            }

        }

        void PopulateContentsTags(long contentId)
        {
            _contentTags.Text = "";

            AWAPI_BusinessLibrary.library.TagLibrary tagLib = new AWAPI_BusinessLibrary.library.TagLibrary();
            var tags = tagLib.GetContentTagList(contentId);

            foreach (awSiteTag tag in tags)
            {
                ListItem li = _tagList.Items.FindByValue(tag.siteTagId.ToString());
                if (li != null)
                {
                    li.Selected = true;
                    _contentTags.Text += li.Text + ";";
                }
            }
        }

        void SaveContentsTags(long contentId)
        {
            AWAPI_BusinessLibrary.library.TagLibrary tagLib = new AWAPI_BusinessLibrary.library.TagLibrary();

            //delete the current ones
            tagLib.DeleteTaggedContentByContentId(contentId);

            //add 
            foreach (ListItem li in _tagList.Items)
                if (li.Selected)
                    tagLib.TagContent(Convert.ToInt64(li.Value), contentId);
        }
        #endregion TAGS ----------------------------

        #region CUSTOM FIELDS -------------------------------------
        void PopulateCustomFields(Int64 contentId, Int64? parentContentId)
        {
            System.Collections.Generic.IEnumerable<AWAPI_Data.CustomEntities.ContentCustomField> cstFlds = _customLib.GetFieldList(contentId, true);
            _valueList = null;
            customFieldsList.DataSource = null;

            if (cstFlds != null && cstFlds.Count() > 0)
            {
                var result = from r in cstFlds
                             where r.applyToSubContents == false && r.fieldContentId.Equals(contentId) ||
                                    r.applyToSubContents && r.fieldContentId.Equals(parentContentId)
                             orderby r.sortOrder
                             select r;
                customFieldsList.DataSource = result;

                string cultureCode = _culture.SelectedValue.ToLower();
                if (String.IsNullOrEmpty(cultureCode) && !String.IsNullOrEmpty(App_Code.SessionInfo.CurrentSite.cultureCode))
                    cultureCode = App_Code.SessionInfo.CurrentSite.cultureCode.ToLower();

                _valueList = _customLib.GetFieldValueList(contentId, cultureCode);

            }
            customFieldsList.DataBind();
        }

        protected void customFieldsList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txb = (TextBox)e.Row.FindControl("_fieldValue");
                int maxLength = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "maximumLength"));
                Int64 fieldId = Convert.ToInt64(DataBinder.Eval(e.Row.DataItem, "customFieldId"));

                var value = from v in _valueList
                            where v.customFieldId.Equals(fieldId)
                            select v;
                if (value != null && value.Count() > 0)
                    txb.Text = value.First().fieldValue;

                txb.MaxLength = maxLength;
                txb.TextMode = TextBoxMode.SingleLine;

                if (maxLength <= 10)
                    txb.Width = new Unit(75, UnitType.Pixel);
                else if (maxLength <= 20)
                    txb.Width = new Unit(150, UnitType.Pixel);
                else if (maxLength <= 50)
                    txb.Width = new Unit(320, UnitType.Pixel);
                else if (maxLength <= 100)
                    txb.Width = new Unit(100, UnitType.Percentage);
                else if (maxLength <= 255)
                {
                    txb.Width = new Unit(100, UnitType.Percentage);
                    txb.Height = new Unit(70, UnitType.Pixel);
                    txb.TextMode = TextBoxMode.MultiLine;
                }
                else
                {
                    txb.Width = new Unit(100, UnitType.Percentage);
                    txb.Height = new Unit(150, UnitType.Pixel);
                    txb.TextMode = TextBoxMode.MultiLine;
                }
            }
        }

        void SaveCustomFields(long contentId)
        {
            string cultureCode = _culture.SelectedValue;
            if (String.IsNullOrEmpty(cultureCode))
                cultureCode = App_Code.SessionInfo.CurrentSite.cultureCode;

            foreach (GridViewRow row in customFieldsList.Rows)
            {
                Label fieldIdControl = (Label)row.FindControl("_fieldId");
                long fieldId = Convert.ToInt64(fieldIdControl.Text);

                TextBox valueControl = (TextBox)row.FindControl("_fieldValue");

                _customLib.UpdateFieldValue(contentId, fieldId, App_Code.SessionInfo.CurrentUser.userId,
                                            valueControl.Text, cultureCode);
            }

        }

        #endregion


        void PopulateContentsInGridView()
        {
            PopulateContentList();
        }

        protected void gwContentList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gwContentList.PageIndex = e.NewPageIndex;
            PopulateContentList();
        }

        protected void gwContentList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "selectcontent":
                    PopulateContent(Convert.ToInt64(e.CommandArgument.ToString()));
                    break;
                case "subcontent":
                    PopulateContentList(Convert.ToInt64(e.CommandArgument.ToString()));
                    PopulateContent(Convert.ToInt64(e.CommandArgument.ToString()));
                    break;
            }
        }

        protected void _rptBreadCrumb_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "selectcontent":
                    PopulateContent(Convert.ToInt64(e.CommandArgument.ToString()));
                    PopulateContentList();
                    break;
            }
        }

        protected void _lbtnBreadCrumbReset_Click(object sender, EventArgs e)
        {
            PopulateContentList(0);
            ResetControls();
        }

        protected void lbtnFilter_Click(object sender, EventArgs e)
        {
            PopulateContentList();
        }

        protected void lbtnClear_Click(object sender, EventArgs e)
        {
            txbFilter.Text = "";
            PopulateContentList();
        }


    }
}
