<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manageblogfiles.aspx.cs"
    Inherits="AWAPI.Admin.frames.manageblogfiles" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>AWAPI - Manage Tags</title>

    <script src="../includes/jscript/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../includes/jscript/tooltip/tooltip.js" type="text/javascript"></script>

    <link href="../css/iframe.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
    <style type="text/css">
        li
        {
            text-align: center;
            margin: 2px;
            padding: 2px;
            overflow: hidden;
            float: left;
            width: 100px;
            height: 100px;
            border: solid 1px #aaaaaa;
        }
        li:hover
        {
            background-color: #dcdcdc;
        }
        #screenshot
        {
            position: absolute;
            border: 1px solid #ccc;
            background: #333;
            padding: 5px;
            display: none;
            color: #fff;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID='scriptManager1' runat='server'>
    </asp:ScriptManager>
    <div id="main" style="min-height: 505px;">
        <div id="header">
            <h3>
                Manage Tags</h3>
        </div>
        <div id="middle" style="width: 750px; min-height: 460px; height: 460px;">
            <div id='left-column' style="width: 385px; min-height: 460px; overflow: auto;">
                <asp:UpdatePanel ID="UpdatePanel1" runat='server'>
                    <ContentTemplate>
                        Group Name:<br />
                        <asp:DropDownList ID="ddlGroupList" runat="server" CssClass="textMedium" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlGroupList_SelectedIndexChanged" />
                        <br />
                        Search:<br />
                        <asp:TextBox ID="txbFilter" runat="server" CssClass="textMedium" />
                        <asp:LinkButton ID="lbtnFilter" runat="server" Text="Filter" OnClick="lbtnFilter_Click" />
                        &nbsp;<asp:LinkButton ID="lbtnClear" runat="server" Text="Clear" OnClick="lbtnClear_Click" />
                        <br />
                        <div style="height: 380px; overflow-y: scroll;">
                            <asp:GridView ID="_fileList" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                CellPadding="2" CellSpacing="2" GridLines="None" OnRowCommand="_fileList_RowCommand"
                                PageSize="10" ShowHeader="True" OnRowDataBound="_fileList_RowDataBound" OnPageIndexChanging="_fileList_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Image runat='server' ID='_fileIcon' ImageUrl="~/admin/img/fileassociation/1616/icon-text.png" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="FileName" ItemStyle-Width="100%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtnSelect" CssClass='screenshot' rel='../includes/jscript/tooltip/cssg_screenshot.jpg'
                                                runat="server" CommandArgument='<%# Bind("fileid") %>' CommandName="addfiletopost"
                                                Text='<%# DataBinder.Eval(Container.DataItem, "title")==null || DataBinder.Eval(Container.DataItem, "title").ToString()==""?"undefined":DataBinder.Eval(Container.DataItem, "title") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id='center-column' style="width: 335px;">
                <asp:UpdatePanel ID="UpdatePanel3" runat='server'>
                    <ContentTemplate>
                        <div id='top-bar'>
                            <asp:Literal ID='_msg' runat='server' />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div style="clear: both;">
                </div>
                <asp:UpdatePanel ID="UpdatePanel2" runat='server'>
                    <ContentTemplate>
                        <asp:GridView ID="_blogPostFileList" runat='server' BorderStyle="None" AllowPaging='true'
                            CellPadding="3" CellSpacing="3" GridLines="None" PageSize="10" AutoGenerateColumns="false"
                            Width="100%" ShowHeader="false" 
                            OnRowCommand="_blogPostFileList_RowCommand" 
                            OnSelectedIndexChanging="_blogPostFileList_SelectedIndexChanging" 
                            onpageindexchanging="_blogPostFileList_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Title" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnSelectBlogPostFile" runat='server' Text='<%# Bind("title") %>'
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "fileId") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="100%" Wrap="false" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="contenttype" HeaderText="Type" />
                                <asp:TemplateField>
                                    <ItemStyle Wrap="false" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID='btnDeleteFileFromPost' runat='server' CommandArgument='<%# Bind("fileid") %>'
                                            CommandName="deletefile" AlternateText="Delete" ImageUrl="../img/button/1616/delete-icon.png"
                                            OnClientClick='return confirm("Are you sure you want to remove the file from the post?");' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
