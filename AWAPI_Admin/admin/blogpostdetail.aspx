<%@ Page Language="C#" MasterPageFile="~/admin/admin.Master" AutoEventWireup="true"
    CodeBehind="blogpostdetail.aspx.cs" Inherits="AWAPI.Admin.PageBlogPostDetail"
    Title="AWAPI - Manage Contents" %>

<%@ Register Assembly="Moxiecode.TinyMCE" Namespace="Moxiecode.TinyMCE.Web" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="includes/tiny_mce/tiny_mce.js"></script>

    <link href="includes/jscript/jquery-autocomplete/jquery.autocomplete.css" rel="stylesheet"
        type="text/css" />

    <script type="text/javascript" src="includes/jscript/jquery-autocomplete/lib/jquery.bgiframe.min.js"></script>

    <script type="text/javascript" src="includes/jscript/jquery-autocomplete/jquery.autocomplete.js"></script>

    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function setImageFromIFrame(url) {
            document.getElementById("<%= _imageUrl.ClientID %>").value = url;

            var image = document.getElementById("<%= _image.ClientID %>");
            image.style.display = '';

            if (url.indexOf("http://") == 0 || url.toIndexOf("https://") == 0)
                image.src = url;
            else
                image.src = url + "&size=150x150";

            image.value = url;
            $.fn.colorbox.close();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolder" runat="server">
    <div id='left-column'>
        <div class='title'>
            <div style="float: left">
                <h3>
                    Blog Post List
                </h3>
            </div>
            <div style="float: right">
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="nav">
                    <asp:TextBox ID='txbFilter' runat='server' />
                    <asp:LinkButton ID='lbtnFilter' runat='server' Text='Filter' OnClick="lbtnFilter_Click" />
                    &nbsp;<asp:LinkButton ID='lbtnClear' runat='server' Text='Clear' OnClick="lbtnClear_Click" />
                    <hr />
                    <asp:GridView ID="gwPostList" runat='server' AutoGenerateColumns="False" Style="width: 100%"
                        GridLines="None" ShowHeader="True" AllowPaging="True" CellPadding="2" CellSpacing="2"
                        PageSize="15" OnRowCommand="gwPostList_RowCommand" OnPageIndexChanging="gwPostList_PageIndexChanging"
                        OnRowDataBound="gwPostList_RowDataBound">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="100%" HeaderText="Post Title" ItemStyle-Wrap="true"
                                HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton ID='lbtnSelect' runat='server' Text='<%# Eval("title") %>' CommandName="selectblogpost"
                                        CommandArgument='<%# Eval ("blogpostid") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Wrap="True" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="pubDate" HeaderText="Date" DataFormatString="{0:MM/dd/yy}" />
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id='center-column'>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="top-bar">
                    <h1>
                        Manage Post</h1>
                    <div class="breadcrumbs">
                        <asp:HyperLink runat='server' NavigateUrl="~/admin/blogs.aspx" Text="Blogs" />
                        &gt;
                        <asp:HyperLink ID="_blogTitle" runat='server' />
                        &gt; Manage Post
                    </div>
                </div>
                <div class="select-bar">
                    <div style="float: left">
                        <ul>
                            <li>
                                <asp:ImageButton ID="btnNewBlogPost_" runat="server" ImageUrl="img/button/new-icon.png"
                                    AlternateText="New Post" CausesValidation="false" ToolTip="New Post" OnClick="btnNewContent_Click" /><br />
                                <asp:Label ID='lblNewBlogPost_' runat='server' Text='New Post' />
                            </li>
                            <%-- <li class="seperator"></li>--%>
                            <li>
                                <asp:ImageButton ID="btnSaveBlogPost_" runat="server" ImageUrl="img/button/save-icon.png"
                                    AlternateText="Save Content" ValidationGroup="saveContent" ToolTip="Save Content"
                                    OnClick="btnSaveBlogPostContent_Click" /><br />
                                <asp:Label ID='lblSaveBlogPost_' runat='server' Text='Save' />
                            </li>
                            <li>
                                <asp:HyperLink ID="hplPopupBlogPostComment_" rel="blogpostcomments" runat="server"
                                    ToolTip="Comments" ImageUrl="img/button/attach-icon.png" NavigateUrl="~/admin/frames/blogpostcomments.aspx"></asp:HyperLink><br />
                                <asp:Label ID='lblPopupBlogPostComment_' runat='server' Text='Comments' />
                            </li>
                            <li>
                                <asp:HyperLink ID="hplPopupBlogPostFile_" rel="blogpostfiles" runat="server"
                                    ToolTip="Post Files" ImageUrl="img/button/icon-image.png" NavigateUrl="~/admin/frames/manageblogfiles.aspx"></asp:HyperLink><br />
                                <asp:Label ID='lblPopupBlogPostFile_' runat='server' Text='Files' />
                            </li>                            
                            <li>
                                <asp:ImageButton ID="btnDeleteBlogPost_" runat="server" AlternateText="Delete Content"
                                    ImageUrl="img/button/delete-icon.png" ToolTip="Delete Content" OnClientClick="return confirm('Are you sure you want to delete the content?');"
                                    ValidationGroup="saveContent" OnClick="btnDelete_Click" /><br />
                                <asp:Label ID='lblDeleteBlogPost_' runat='server' Text='Delete' />
                            </li>
                        </ul>
                    </div>
                </div>
                <div>
                </div>
                <table>
                    <tr>
                        <td>
                            <table cellpadding="3" cellspacing="2" class="style1">
                                <tr>
                                    <td align="right" nowrap="nowrap" valign="top">
                                        Id:
                                    </td>
                                    <td nowrap="nowrap" valign="top">
                                        <asp:Literal ID="_id" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap" valign="top">
                                        Author:
                                    </td>
                                    <td nowrap="nowrap" valign="top" width="100%">
                                        <asp:DropDownList ID="_authorList" runat='server' />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap" valign="top">
                                        Title:
                                    </td>
                                    <td nowrap="nowrap" valign="top" width="100%">
                                        <asp:TextBox ID="_title" runat="server" CssClass="textLong" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_title"
                                            ErrorMessage="required" ValidationGroup="saveContent"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap" valign="top">
                                        Summary:
                                    </td>
                                    <td nowrap="nowrap" colspan="3">
                                        <asp:TextBox ID="_summary" runat="server" CssClass="textDescription" TextMode="MultiLine" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap">
                                        Published:
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:CheckBox ID="_isPublished" runat="server" />
                                        &nbsp;&nbsp;Commentable:<asp:CheckBox ID="_isCommentable" runat="server" />
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap">
                                        Image URL:
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:TextBox ID="_imageUrl" runat="server" class="textLong" Width="300px"></asp:TextBox>
                                        <a rel="selectfile" href='frames/selectfile.aspx?type=image&group=<%= HttpContext.Current.Server.UrlEncode(_blogTitle.Text.Replace("'", "")) %>&callback=setImageFromIFrame'>
                                            <img alt="" class="toolBarButton" src="img/button/1616/Folder-Open-icon.png" />
                                        </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap="nowrap">
                                        Publish Start Date:
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:TextBox ID="_pubDate" runat="server" CssClass="textMedium"></asp:TextBox>
                                        <span class="table_td_second_section" />End Date:
                                        <asp:TextBox ID="_pubEndDate" runat="server" CssClass="textMedium"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" PopupPosition="TopRight"
                                            TargetControlID="_pubDate" Format="MM/dd/yyyy" />
                                        <ajax:CalendarExtender ID="CalendarExtender2" runat="server" PopupPosition="TopRight"
                                            TargetControlID="_pubEndDate" Format="MM/dd/yyyy" />
                                        <br />
                                        <i>If you set the value, post will be available only between this days</i>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap" valign="top">
                                        Tags:
                                    </td>
                                    <td nowrap="nowrap" valign="top">
                                        <style>
                                            #tagList ul
                                            {
                                                list-style-type: none;
                                                padding: 3px;
                                                margin-top: 3px;
                                            }
                                            #tagList li
                                            {
                                                margin-bottom: 4px;
                                            }
                                        </style>
                                        <asp:TextBox ID="_tags" runat="server" class="textLong" TextMode="MultiLine" Rows="2" />&nbsp;&nbsp;<a
                                            href='javascript:void(0);' onclick='showHideDiv("tagList");'><img alt="" src="img/expand_collapse_icon.png" /></a><i>(Comma
                                                seperated)</i>
                                        <div id='tagList' class="collapsablePanelSmall" style="display: none;">
                                            <ul>
                                                <asp:Repeater ID="_tagList" runat="server">
                                                    <ItemTemplate>
                                                        <li><a href='javascript:void(0);' onclick='<%# "AddTag(\"" + DataBinder.Eval(Container.DataItem, "title") + "\", \"" + _tags.ClientID  + "\")" %>'>
                                                            <asp:Label ID="lblTagName" Text='<%# Bind("title") %>' runat="server" />
                                                        </a></li>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ul>
                                            <asp:Literal ID="_tagsAutoCompleteScript" runat='server' Visible="false" />
                                            <%--                                            <asp:CheckBoxList ID="_tagList" runat='server'>
                                            </asp:CheckBoxList>--%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap" valign="top">
                                        Categories:
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:TextBox ID="_categoryListInText" runat="server" ReadOnly="true" Enabled="false"
                                            class="textMedium" Width="300px" />&nbsp;&nbsp;<a href='javascript:void(0);' onclick='showHideDiv("blogCategories");'><img
                                                alt="" src="img/expand_collapse_icon.png" /></a>
                                        <div id='blogCategories' class="collapsablePanelSmall" style="display: none;">
                                            <asp:CheckBoxList ID="_categoryList" runat='server'>
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" align="right" style="text-align: right;">
                            <br />
                            <asp:Image ID='_image' runat='server' Width="150px" />
                        </td>
                    </tr>
                </table>
                <div>
                    <cc1:TinyMCETextArea ID="_description" InstallPath="includes/tiny_mce" theme="advanced"
                        plugins="spellchecker,safari,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template"
                        theme_advanced_buttons1="bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,formatselect,fontselect,fontsizeselect,|,forecolor,backcolor,|,preview,fullscreen"
                        theme_advanced_buttons2="pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,|,link,unlink,anchor,image,cleanup,code,|,removeformat,|,sub,sup,|,charmap,emotions,iespell,media,advhr"
                        theme_advanced_buttons3=""
                        theme_advanced_buttons4=""
                        theme_advanced_toolbar_location="top"
                        theme_advanced_toolbar_align="left" theme_advanced_path_location="bottom" ConvertFontToSpan="false"
                        runat="server" Width="100%" Rows="17" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSaveBlogPost_" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
