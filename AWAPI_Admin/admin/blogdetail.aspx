<%@ Page Language="C#" MasterPageFile="~/admin/admin.Master" AutoEventWireup="true"
    CodeBehind="blogdetail.aspx.cs" Inherits="AWAPI.Admin.PageBlogDetail" Title="AWAPI - Manage Blog" %>

<%@ Register Assembly="Moxiecode.TinyMCE" Namespace="Moxiecode.TinyMCE.Web" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--   <script type="text/javascript" src="includes/tiny_mce/tiny_mce.js"></script>--%>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function setImageFromIFrame(url) {
            document.getElementById("<%= _blogImageUrl.ClientID %>").value = url;
            var image = document.getElementById("<%= _blogImage.ClientID %>");
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
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class='title'>
                    <div style="float: left">
                        <h3>
                            Blog List
                        </h3>
                    </div>
                    <div style="float: right">
                        <asp:ImageButton ID='btnRefreshTree' runat='server' AlternateText="Refresh Content List"
                            ImageUrl="img/bg-refresh.png" OnClick="btnRefreshTree_Click" />
                    </div>
                </div>
                <div class="nav">
                    <asp:GridView ID="blogList" runat="server" AutoGenerateColumns="False" GridLines="None"
                        OnRowCommand="blogList_RowCommand" Style="width: 100%" CellPadding="2" CellSpacing="2"
                        OnRowDataBound="blogList_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Blog Name" ItemStyle-Width="100%" HeaderStyle-HorizontalAlign="left">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnSelectBlog" runat="server" CommandArgument='<%# Eval ("blogId") %>'
                                        CommandName="editblog" Text='<%# Eval ("title") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Posts" HeaderStyle-HorizontalAlign="left">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hplBlogPosts" runat="server" NavigateUrl='<%# "blogs.aspx?blogid=" + DataBinder.Eval(Container.DataItem, "blogId") %>'
                                        Text='posts' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
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
                        Manage Blog</h1>
                    <div class="breadcrumbs">
                        <asp:HyperLink runat='server' NavigateUrl="~/admin/blogs.aspx" Text="Blogs" />
                        &gt; Manage Blog
                    </div>
                </div>
                <div class="select-bar">
                    <div style="float: left">
                        <ul>
                            <li>
                                <asp:ImageButton ID="btnNewBlog_" runat="server" ImageUrl="img/button/new-icon.png"
                                    AlternateText="New Blog" CausesValidation="false" ToolTip="New Blog" OnClick="btnNew_Click" /><br />
                                <asp:Label ID='lblNewBlog_' runat='server' Text='New Blog' />
                            </li>
                            <li class="seperator"></li>
                            <li>
                                <asp:ImageButton ID="btnSaveBlog_" runat="server" ImageUrl="img/button/save-icon.png"
                                    AlternateText="Save Content" ValidationGroup="save" ToolTip="Save Content" OnClick="btnSave_Click" /><br />
                                <asp:Label ID='lblSaveBlog_' runat='server' Text='Save' />
                            </li>
                            <li>
                                <asp:HyperLink ID="_categoriesLink" rel="manageblogcategories" runat="server" ToolTip="Manage Blog Categories"
                                    ImageUrl="img/button/folder-icon.png" NavigateUrl="~/admin/frames/manageblogcategories.aspx"></asp:HyperLink><br />
                                <asp:Label ID='_categoriesLinkText' runat='server' Text='Categories' />
                            </li>
                            <li>
                                <asp:HyperLink ID="_tagsLink" rel="managetags" runat="server" Text="Manage Tags"
                                    ImageUrl="img/button/tag-icon.gif" NavigateUrl="~/admin/frames/manageblogtags.aspx"
                                    ToolTip="Manage Tags" />
                                <br />
                                <asp:Label ID='_tagsLinkText' runat='server' Text='Tags' />
                            </li>
                            <li>
                                <asp:ImageButton ID="btnDeleteBlog_" runat="server" AlternateText="Delete Content"
                                    ImageUrl="img/button/delete-icon.png" ToolTip="Delete Content" OnClientClick="return confirm('Are you sure you want to delete the content?');"
                                    ValidationGroup="save" OnClick="btnDelete_Click" /><br />
                                <asp:Label ID='lblDeleteBlog_' runat='server' Text='Delete' />
                            </li>
                        </ul>
                    </div>
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
                                        <asp:Label ID="_blogId" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap" valign="top">
                                        Enabled:
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:CheckBox ID='_blogIsEnabled' runat='server' />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap" valign="top">
                                        Blog Alias:
                                    </td>
                                    <td nowrap="nowrap" valign="top" width="100%">
                                        <asp:TextBox ID="_alias" runat="server" CssClass="textLong" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="_alias"
                                            ErrorMessage="required" ValidationGroup="save"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID='revContentName' runat='server' ControlToValidate="_alias"
                                            ErrorMessage="You can't use special characters" ValidationGroup="save" Display="Dynamic"
                                            ValidationExpression="^([0-9a-zA-Z\&amp;\-\.\'+'\'_']*|)" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap" valign="top">
                                        Blog Title:
                                    </td>
                                    <td nowrap="nowrap" valign="top" width="100%">
                                        <asp:TextBox ID="_blogTitle" runat="server" CssClass="textLong" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_blogTitle"
                                            ErrorMessage="required" ValidationGroup="save"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap" valign="top">
                                        Image:
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:TextBox ID="_blogImageUrl" runat="server" class="textLong" Width="300px"></asp:TextBox>
                                        <a rel="selectfile" href='frames/selectfile.aspx?type=image&callback=setImageFromIFrame'>
                                            <img class="toolBarButton" src="img/button/1616/Folder-Open-icon.png" />
                                        </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap" valign="top">
                                        Description:
                                    </td>
                                    <td nowrap="nowrap">
                                        <cc1:tinymcetextarea id="_blogDescription" installpath="includes/tiny_mce" theme="advanced"
                                            plugins="spellchecker,safari,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template"
                                            theme_advanced_buttons1="bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,formatselect,fontselect,fontsizeselect,|,forecolor,backcolor,|,preview,fullscreen"
                                            theme_advanced_buttons2="pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,|,link,unlink,anchor,image,cleanup,code,|,removeformat,|,sub,sup,|,charmap,emotions,iespell,media,advhr"
                                            theme_advanced_buttons3="" theme_advanced_buttons4="" theme_advanced_toolbar_location="top"
                                            theme_advanced_toolbar_align="left" theme_advanced_path_location="bottom" convertfonttospan="false"
                                            runat="server" width="100%" rows="17" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap" valign="top">
                                        BlogPost Page:
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:TextBox ID="_blogPostPage" runat="server" class="textLong" Width="300px" /><i>ex:
                                            /blog/postdetail.aspx</i>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap" valign="top">
                                        Comment Notifier:
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:CheckBox ID="_enableCommentEmailNotifier" runat='server' AutoPostBack="true"
                                            OnCheckedChanged="_enableCommentEmailNotifier_CheckedChanged" />
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnlCommentEmailNotifier" runat='server' Visible="false">
                                <table cellpadding="3" cellspacing="2" class="style1">
                                    <tr>
                                        <td align="right" nowrap="nowrap" valign="top">
                                            Email Recipes:
                                        </td>
                                        <td nowrap="nowrap" valign="top">
                                            <asp:TextBox ID="_commentEmailTo" TextMode="MultiLine" Rows="5" runat="server" CssClass="textLong"
                                                Width="100%" />
                                            <i>comma seperated</i>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" nowrap="nowrap" valign="top">
                                            Email Template:
                                        </td>
                                        <td nowrap="nowrap">
                                            <asp:DropDownList ID="_emailTemplate" runat='server' />
                                            <br />
                                            <i>Awapilable Comment Template Fields:<br />
                                                <ul>
                                                    <li>{commenter} :Name of the commenter</li>
                                                    <li>{commenteremail} : Commenter's email address</li>
                                                    <li>{comment} : Comment</li>
                                                    <li>{moderatelink} : Link to moderate the comment through the admin module</li>
                                                    <li>{approvelink} : Link to directly approve the comment</li>
                                                    <li>{rejectlink} : Link to diresctly reject the comment</li>
                                                    <li>{date} : Comment date</li>
                                                </ul>
                                            </i>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td valign="top" align="right" style="text-align: right;">
                            <br />
                            <asp:Image ID='_blogImage' runat='server' Width="150px" />
                        </td>
                    </tr>
                </table>
                <div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
