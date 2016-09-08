<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="blogpostcomments.aspx.cs"
    Inherits="AWAPI.Admin.frames.blogpostcomments" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AWAPI - Manage Post Comments</title>
    <link href="../css/iframe.css" rel="stylesheet" type="text/css" />
    <style>
        .statusPending
        {
            color: #CF5C00;
        }
        .statusApproved
        {
            color: #308F00;
        }
        .statusRejected
        {
            color: #FF0000;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID='scriptManager1' runat='server'>
    </asp:ScriptManager>
    <div id="main" style="min-height: 500px;">
        <div id="header">
            <h3>
                Manage Post Comments</h3>
        </div>
        <div id="middle" style="width: 95%; min-height: 450px; height: 450px;">
            <div id='left-column' style="width: 400px; min-height: 450px; overflow: auto;">
                <asp:UpdatePanel ID="UpdatePanel2" runat='server'>
                    <ContentTemplate>
                        <asp:DropDownList ID="_searchStatus" runat='server' CssClass="textMedium" AutoPostBack="True"
                            OnSelectedIndexChanged="_searchStatus_SelectedIndexChanged">
                            <asp:ListItem Text="All" Value="" />
                            <asp:ListItem Text="Pending" Value="0" />
                            <asp:ListItem Text="Approved" Value="1" />
                            <asp:ListItem Text="Rejected" Value="2" />
                        </asp:DropDownList>
                        <br />
                        <asp:GridView ID='_commentList' runat='server' AutoGenerateColumns="false" BorderStyle="None"
                            CellPadding="3" CellSpacing="3" GridLines="None" ShowHeader="true" Width="100%"
                            AllowPaging="True" OnPageIndexChanging="_commentList_PageIndexChanging" OnRowCommand="_commentList_RowCommand"
                            OnRowDataBound="_commentList_RowDataBound">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Left" />
                            <Columns>
                                <asp:TemplateField HeaderText="Title" ItemStyle-Width="100%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID='lbtnTitle' runat='server' Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "title")) == ""? "None": DataBinder.Eval(Container.DataItem, "title") %>'
                                            CommandName="editcomment"
                                            CommandArgument='<%# Bind("blogCommentID") %>' ToolTip='<%# Bind("description") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="username" HeaderText="Commenter" />
                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="100%">
                                    <ItemTemplate>
                                        <asp:Label ID='lblStatus' runat='server' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="pubDate" HeaderText="Date" ItemStyle-Wrap="false">
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id='center-column' style="padding: 1px 14px 0 12px;">
                <asp:UpdatePanel ID="UpdatePanel1" runat='server'>
                    <ContentTemplate>
                        <div id='top-bar'>
                            <asp:ImageButton ID="btnNewBlogPostComment_" runat="server" ImageUrl="../img/button/new-icon.png"
                                AlternateText="Add New Comment" CausesValidation="False" OnClick="btnNewCustomField_Click" />
                            <asp:ImageButton ID="btnSaveBlogPostComment_" runat="server" ImageUrl="../img/button/save-icon.png"
                                AlternateText="Save" ValidationGroup="_savecomment" OnClick="btnSaveCustomField_Click" />
                            <asp:ImageButton ID="btnDeleteBlogPostComment_" runat="server" AlternateText="Delete Comment"
                                ImageUrl="../img/button/delete-icon.png" OnClientClick="return confirm('Are you sure you want to delete the comment?');"
                                OnClick="btnDeleteComment_Click" />
                            <asp:Literal ID="_msg" runat='server' />
                        </div>
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        Id:
                                    </td>
                                    <td>
                                        <asp:Literal ID="_id" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Status:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID='_status' runat='server' CssClass="textMedium">
                                            <asp:ListItem Text="Pending" Value="0" />
                                            <asp:ListItem Text="Approved" Value="1" />
                                            <asp:ListItem Text="Rejected" Value="2" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        User Id:
                                    </td>
                                    <td>
                                        <asp:TextBox ID='_userId' runat='server' />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        UserName:
                                    </td>
                                    <td>
                                        <asp:TextBox ID='_userName' runat='server' />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID='_firstName' runat='server' />
                                        &nbsp;<asp:TextBox ID='_lastName' runat='server' OnTextChanged="_name0_TextChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Email:
                                    </td>
                                    <td>
                                        <asp:TextBox ID='_email' runat='server' CssClass="textLong" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Title:
                                    </td>
                                    <td>
                                        <asp:TextBox ID='_title' runat='server' CssClass="textLong" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_title"
                                            Display="Dynamic" ErrorMessage="required" ValidationGroup="_savecomment"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Comment:
                                    </td>
                                    <td>
                                        <asp:TextBox ID='_description' runat='server' CssClass="textLong" TextMode="MultiLine"
                                            Style="height: 170px;" />
                                        <br />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="_description"
                                            Display="Dynamic" ErrorMessage="required" ValidationGroup="_savecomment"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
