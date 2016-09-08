<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" MasterPageFile="~/admin/admin.Master"
    Inherits="AWAPI.Admin.PageDefault" Title="AWAPI - Dashboard" %>

<asp:Content runat='server' ContentPlaceHolderID="contentPlaceHolder">
    <style type="text/css">
        .column1
        {
            width: 49%;
            float: left;
        }
        .column2
        {
            width: 49%;
            float: left;
        }
        .panel
        {
            margin: 5px;
            width: 98%;
        }
        .panel h3
        {
            background-color: #3FB5FF;
            color: #FFFFFF;
            font-size: 11px;
            height: 21px;
            line-height: 20px;
            margin: 0;
            padding: 0 0 0 4px;
        }
        .panel .box
        {
            border: solid 1px #dcdcdc;
            padding: 5px;
        }
    </style>
    <div id='left-column'>
        <div class="panel" style="width: 98%; background-color: #efefef;">
            <div class="box" style="height: 140px;">
                <div>
                    <asp:Image runat='server' ID="_userImage" ImageUrl="img/no_image.jpg" align="left"
                        Style="margin-right: 5px;" Width="108" /><strong style="text-transform: uppercase;">
                            <asp:Label runat='server' ID='_userName' />
                        </strong>
                    <br />
                    <asp:Label runat='server' ID='_userEmail' />
                    <br />
                    <br />
                    <asp:HyperLink runat="server" NavigateUrl="~/admin/updateprofile.aspx" Text="Update profile" />
                    <br />
                </div>
            </div>
        </div>
        <div class="panel" style="width: 98%;">
            <div class="box">
                <h3>
                    Configuration</h3>
                <p>
                    <strong>Current Site Id</strong><br />
                    <asp:Label ID="_currentsiteId" runat='server'/>
                </p>
                <p>
                    <strong>File Location</strong><br />
                    <asp:Image runat='server' ID='_serverFileLocation' align="left" />&nbsp;&nbsp;<asp:Label
                        ID="_serverFileLocationTitle" runat='server' />
                </p>
                <p>
                    <strong>Blog Handler</strong><br />
                    <asp:HyperLink runat='server' ID='_serverBlogHandlerLink' Target="_blank" />
                </p>
                <p>
                    <strong>Content Handler</strong><br />
                    <asp:HyperLink runat='server' ID='_serverContentHandlerLink' Target="_blank" />
                </p>
                <p>
                    <strong>File Handler</strong><br />
                    <asp:HyperLink runat='server' ID='_serverFileHandlerLink' Target="_blank" />
                </p>
                <p>
                    <strong>Twitter Handler</strong><br />
                    <asp:HyperLink runat='server' ID='_serverTwitterHandlerLink' Target="_blank" />
                </p>
                <p>
                    <strong>Weather Handler</strong><br />
                    <asp:HyperLink runat='server' ID='_serverWeatherHandlerLink' Target="_blank" />
                </p>
            </div>
        </div>
    </div>
    <div id='center-column'>
        <div class="column1">
            <asp:Panel ID='pnlShareIt' runat='server' CssClass="panel">
                <h3>
                    ShareIt
                </h3>
                <div class="box">
                    <asp:GridView ID='_shareIt' runat='server' AutoGenerateColumns="false" BorderStyle="None"
                        CellPadding="3" CellSpacing="3" GridLines="None" ShowHeader="true" Width="100%">
                        <HeaderStyle VerticalAlign="Top" HorizontalAlign="Left" />
                        <Columns>
                            <asp:TemplateField HeaderText="Title" ItemStyle-Width="100%" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:HyperLink runat='server' ID='hplShareIt' Target="_blank" Text='<%# Bind("title") %>'
                                        ToolTip='<%# Bind("description") %>' NavigateUrl='<%# Bind("link") %>' /><br />
                                    <asp:Literal ID="litShareItDescription" runat='server' Text='<%# Bind("description") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="From" ItemStyle-Width="100%" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Label ID='lblFirstName' runat='server' Text='<%# DataBinder.Eval(Container.DataItem, "firstName") + " " + DataBinder.Eval(Container.DataItem, "lastName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <asp:Panel ID='pnlRecentImages' runat='server' CssClass="panel">
                <h3>
                    Recently Uploaded Image
                </h3>
                <div class="box" style='height: 200px;'>
                    <asp:Repeater ID="_recentImages" runat="server" OnItemDataBound="_recentImages_ItemDataBound">
                        <ItemTemplate>
                            <a href='<%# "files.aspx?fileid=" + DataBinder.Eval(Container.DataItem, "fileid")  %>'>
                                <asp:Image ID='_recentImage' runat='server' ImageAlign="left" ToolTip='<%# Bind("title") %>'
                                    Style='margin-right: 5px;' Width="150" />
                            </a>
                            <asp:Label ID='_recentImageTitle' runat='server' Text='<%# Bind("title") %>' /><br />
                            <asp:Label ID='_recentImageDescription' runat='server' Text='<%# Bind("description") %>' /><br />
                            <asp:Label ID='_recentImageDate' runat='server' Text='<%# Bind("createDate", "{0:MM/dd/yyyy}") %>' /><br />
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </asp:Panel>
            <asp:Panel ID='pnlUnApprovedFiles' runat='server' CssClass="panel">
                <h3>
                    Unapproved Files
                </h3>
                <div class="box" style='height: 170px; overflow: auto'>
                    <asp:GridView ID='_recentlyUploadedImages' runat='server' AutoGenerateColumns="false"
                        BorderStyle="None" CellPadding="3" CellSpacing="3" GridLines="None" ShowHeader="true"
                        Width="100%">
                        <HeaderStyle VerticalAlign="Top" HorizontalAlign="Left" />
                        <Columns>
                            <asp:TemplateField HeaderText="Title" ItemStyle-Width="100%" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:HyperLink runat='server' ID='hplUnApprovedFiles' Text='<%# Bind("title") %>'
                                        ToolTip='<%# Bind("description") %>' NavigateUrl='<%# "files.aspx?fileid=" + DataBinder.Eval(Container.DataItem, "fileId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="username" HeaderText="By" />
                            <asp:BoundField DataField="createDate" HeaderText="Date" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
        </div>
        <div class="column2">
            <asp:Panel ID='pnlPendingComments' runat='server' CssClass="panel">
                <h3>
                    Pending Comments
                </h3>
                <div class="box">
                    <asp:GridView ID='_pendingComments' runat='server' AutoGenerateColumns="false" BorderStyle="None"
                        CellPadding="3" CellSpacing="3" GridLines="None" ShowHeader="true" Width="100%">
                        <HeaderStyle VerticalAlign="Top" HorizontalAlign="Left" />
                        <Columns>
                            <asp:TemplateField HeaderText="Title" ItemStyle-Width="100%" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:HyperLink runat='server' ID='hplPeningComments' Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "title")) == ""? "None": DataBinder.Eval(Container.DataItem, "title") %>'
                                        ToolTip='<%# Bind("description") %>' NavigateUrl='<%# "blogpostdetail.aspx?postid=" + DataBinder.Eval(Container.DataItem, "blogpostid") + "&commentid=" + DataBinder.Eval(Container.DataItem, "blogCommentID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="pubDate" HeaderText="Date" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <asp:Panel ID='pnlRecentPosts' runat='server' CssClass="panel">
                <h3>
                    Recent Posts
                </h3>
                <div class="box">
                    <asp:GridView ID='_recentPosts' runat='server' AutoGenerateColumns="false" BorderStyle="None"
                        CellPadding="3" CellSpacing="3" GridLines="None" ShowHeader="true" Width="100%">
                        <HeaderStyle VerticalAlign="Top" HorizontalAlign="Left" />
                        <Columns>
                            <asp:TemplateField HeaderText="Title" ItemStyle-Width="100%" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:HyperLink runat='server' ID='hplRecentPosts' Text='<%# Bind("title") %>' NavigateUrl='<%# "blogpostdetail.aspx?postid=" + DataBinder.Eval(Container.DataItem, "blogpostid") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="pubDate" HeaderText="Date" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
