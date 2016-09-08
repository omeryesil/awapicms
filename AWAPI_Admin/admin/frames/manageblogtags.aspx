<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manageblogtags.aspx.cs"
    Inherits="AWAPI.Admin.frames.manageblogtags" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>AWAPI - Manage Tags</title>
    <link href="../css/iframe.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID='scriptManager1' runat='server'>
    </asp:ScriptManager>
    <div id="main" style="min-height: 400px;">
        <div id="header">
            <h3>
                Manage Tags</h3>
        </div>
        <div id="middle" style="width: 700px; min-height: 340px; height: 340px;">
            <div id='left-column' style="width: 200px; min-height: 300px; overflow: auto;">
                <asp:UpdatePanel ID="UpdatePanel1" runat='server'>
                    <ContentTemplate>
                        <asp:GridView ID='_tagList' runat='server' AutoGenerateColumns="False" BorderStyle="None"
                            AllowPaging='true' PageSize="15" CellPadding="3" CellSpacing="3" GridLines="None"
                            ShowHeader="false" OnRowCommand="_tagList_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="title">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="tagTitle" runat='server' Text='<%# Bind("title") %>' CommandName="edittag"
                                            CommandArgument='<%# Bind("blogTagId") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="100%" Wrap="True" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="numberOfPosts" HeaderText="Posts" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id='center-column' style="width: 450px;">
                <asp:UpdatePanel ID="UpdatePanel3" runat='server'>
                    <ContentTemplate>
                        <div id='top-bar'>
                            <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="../img/button/new-icon.png"
                                AlternateText="Add New Tag" CausesValidation="False" OnClick="btnAdd_Click" ToolTip="New" />
                            <asp:ImageButton ID="btnSave" runat="server" ImageUrl="../img/button/save-icon.png"
                                AlternateText="Save" ValidationGroup="saveTag" OnClick="btnSave_Click" ToolTip="Save" />
                            <asp:ImageButton ID="btnDelete" runat="server" AlternateText="Delete Tag" ImageUrl="../img/button/delete-icon.png"
                                OnClientClick="return confirm('Are you sure you want to delete the tag?');" ValidationGroup="saveTag"
                                OnClick="btnDelete_Click" ToolTip="Delete Tag" />
                            <asp:Literal ID='_msg' runat='server' />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div style="clear: both;">
                </div>
                <asp:UpdatePanel ID="UpdatePanel2" runat='server'>
                    <ContentTemplate>
                        <table align="right" cellpadding="3" cellspacing="4" class="style1">
                            <tr>
                                <td align="right" valign="top" nowrap="nowrap">
                                    Tag Id
                                </td>
                                <td width="100%">
                                    <asp:Literal ID="_tagId" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Title:
                                </td>
                                <td>
                                    <asp:TextBox ID="_title" runat="server" CssClass="textMedium"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_title"
                                        Display="Dynamic" ErrorMessage="required" ValidationGroup="saveTag"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                </td>
                                <td>
                                    </b>Posts:<br />
                                    <asp:GridView ID="_tagPosts" runat='server' BorderStyle="None" AllowPaging='true'
                                        CellPadding="3" CellSpacing="3" GridLines="None" PageSize="5" AutoGenerateColumns="false"
                                        Width="100%" ShowHeader="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="title" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="tagTitle" runat='server' Text='<%# Bind("title") %>' NavigateUrl='<%# "../blogpostdetail.aspx?blogid=" + DataBinder.Eval(Container.DataItem, "blogid") + "&postid=" + DataBinder.Eval(Container.DataItem, "blogpostid") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="100%" Wrap="false" HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
