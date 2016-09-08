<%@ Page Language="C#" MasterPageFile="~/admin/admin.Master" AutoEventWireup="true"
    CodeBehind="blogs.aspx.cs" Inherits="AWAPI.Admin.PageBlogs" Title="AWAPI - Blogs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolder" runat="server">
    <div id='left-column'>
        <div class='title'>
            <div style="float: left">
                <h3>
                    Blog List
                </h3>
            </div>
            <div style="float: right">
                <asp:HyperLink ID='hplAddBlog' runat='server' Text='New Blog' ImageUrl="img/button/1616/new-icon.png"
                    NavigateUrl='blogdetail.aspx' ToolTip="Add New Blog" />
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel2" runat='server'>
            <ContentTemplate>
                <div class="nav">
                    <asp:GridView ID="blogList" runat='server' AutoGenerateColumns="False" Style="width: 100%"
                        GridLines="None" OnRowCommand="blogList_RowCommand" CellPadding="2" 
                        CellSpacing="2" onrowdatabound="blogList_RowDataBound">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="100%" HeaderText="Blog Name" HeaderStyle-HorizontalAlign="left">
                                <ItemTemplate>
                                    <asp:LinkButton ID='lbtnSelectBlog' runat='server' Text='<%# Bind("title") %>' CommandName="selectblog"
                                        CommandArgument='<%# Eval ("blogid") %>' ToolTip="Show Blog Posts" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID='hplEditBlog' runat='server' Text='edit' ToolTip="Edit Blog" NavigateUrl='<%# "blogdetail.aspx?blogid=" + Eval ("blogid") %>' />
                                    &nbsp;&nbsp;
                                    <asp:HyperLink ID='rssLink' runat='server' Target="_blank" Text='edit' ToolTip="Edit Blog"
                                        NavigateUrl='<%# AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.blogServiceUrl + "?method=getpostlist&siteid=" + Eval ("siteid") + "&blogid=" + Eval ("blogid")  %>'
                                        ImageUrl="img/button/1616/rss-icon.png" />
                                </ItemTemplate>
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
                        <asp:Label ID='_blogTitle' runat='server' Text='Blog Posts' /></h1>
                    <asp:Label ID='_blogId' runat='server' Visible="false" />
                    <div class="breadcrumbs">
                    </div>
                </div>
                <div class="select-bar">
                    <div style="float: left">
                        <ul>
                            <li>
                                <asp:ImageButton ID="btnNewBlogPost_" runat="server" ImageUrl="img/button/new-icon.png"
                                    AlternateText="New Post" CausesValidation="false" ToolTip="New Blog Post" OnClick="btnNewContent_Click" /><br />
                                <asp:Label ID='lblNewBlogPost_' runat='server' Text='New Post' />
                            </li>
                        </ul>
                    </div>
                </div>
                <div style="clear: both;">
                    <div>
                        Category:
                        <asp:DropDownList ID='_filterByCategory' runat='server' AutoPostBack="true" 
                            onselectedindexchanged="_filterByCategory_SelectedIndexChanged" />
                        &nbsp;&nbsp; Tags:
                        <asp:DropDownList ID='_filterByTags' runat='server' AutoPostBack="true" 
                            onselectedindexchanged="_filterByTags_SelectedIndexChanged" />
                        &nbsp; Keyword:
                        <asp:TextBox ID='_filterByText' runat='server' />
                        &nbsp;<asp:LinkButton ID='lbtnFilter' runat='server' Text='Filter' OnClick="lbtnFilter_Click" />
                        &nbsp;
                        <asp:LinkButton ID="lbtnClear" runat="server" OnClick="lbtnClear_Click" Text="Clear" />
                    </div>
                    <div>
                        <asp:GridView ID='gwArticles' runat='server' AllowPaging="True" AutoGenerateColumns="False"
                            CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="gwArticles_PageIndexChanging"
                            PageSize="15" Width="100%" OnRowCommand="gwArticles_RowCommand" 
                            HorizontalAlign="Left" onrowdatabound="gwArticles_RowDataBound">
                            <RowStyle BackColor="#EFF3FB" />
                            <Columns>
                                <asp:TemplateField HeaderText="Post Title">
                                    <ItemStyle Wrap="false" Width="100%" HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID='rowLblContentId' runat='server' Text='<%# Bind("blogPostId")%>' Visible="false" />
                                        <asp:HyperLink ID='rowHplTitle' runat='server' NavigateUrl='<%# "blogpostdetail.aspx?blogid=" + Eval("blogid") + "&postid=" + Eval("blogPostId") %>'
                                            Text='<%# Bind("title") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Published">
                                    <ItemStyle Wrap="false" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="rowCbPublished" runat="server" Checked='<%# Bind("isPublished") %>'
                                            AutoPostBack="true" OnCheckedChanged="rowCbPublished_CheckedChanged" />
                                        <asp:Label ID='rowLblPublishMsg' runat='server' Text='<%# Convert.ToBoolean(Eval("isPublished"))?"Published":"Draft" %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="isCommentable" HeaderText="Commentable" />
                                <asp:BoundField DataField="username" HeaderText="Author" />
                                <asp:BoundField DataField="pubDate" DataFormatString="{0:dd MMMM yyyy}" HeaderText="Date"
                                    HtmlEncode="False">
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="false" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID='btnDeleteBlogPost_' runat='server' CommandArgument='<%# Bind("blogPostId") %>'
                                            CommandName="deletepost" AlternateText="Delete" ImageUrl="img/button/1616/delete-icon.png"
                                            OnClientClick='return confirm("Are you sure you want to delete the post?");' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" 
                                HorizontalAlign="Left" VerticalAlign="Top" />
                            <EditRowStyle BackColor="#2461BF" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
