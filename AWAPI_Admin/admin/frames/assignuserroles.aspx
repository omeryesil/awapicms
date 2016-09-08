<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="assignuserroles.aspx.cs"
    Inherits="AWAPI.Admin.frames.RoleList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AWAPI - User Roles</title>
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
    <div id="main" style="min-height: 300px;overflow:auto;">
        <div id="header">
            <h3>
                <asp:Label ID='_pageTitle' runat='server' Text='User Roles' /></h3>
        </div>
        <div id="middle" style="min-height: 250px;">
            <div id='top-bar' style="padding: 4px;">
                <asp:ImageButton ID="btnSavenAssignRole_" runat="server" ImageUrl="../img/button/save-icon.png"
                    AlternateText="Save" OnClick="btnSavenAssignRole__Click" />
                <asp:Literal ID="_msg" runat='server' />
            </div>
            <div>
                <asp:UpdatePanel ID="UpdatePanel1" runat='server'>
                    <ContentTemplate>
                        <asp:GridView ID='_roleList' runat='server' AutoGenerateColumns="false" GridLines="None"
                            HeaderStyle-HorizontalAlign="left" OnRowDataBound="_roleList_RowDataBound" CellPadding="1"
                            CellSpacing="1">
                            <Columns>
                                <asp:TemplateField HeaderText="" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID='_assigned' runat='server' />
                                    </ItemTemplate>
                                    <ItemStyle Width="40px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="title" HeaderText="Role" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID='_roleId' runat='server' Text='<%# Bind("roleId") %>' Visible="false" />
                                        <asp:HyperLink ID='hplInfo' runat='server' ToolTip='<%# Bind("description") %>' ImageUrl="../img/info-icon.gif" />
                                        &nbsp;&nbsp;
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="module" HeaderText="Module" />
                                <asp:BoundField DataField="canRead" HeaderText="Read" ItemStyle-Width="60px">
                                    <ItemStyle Width="60px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="canAdd" HeaderText="Add" ItemStyle-Width="60px">
                                    <ItemStyle Width="60px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="canUpdate" HeaderText="Update" ItemStyle-Width="60px">
                                    <ItemStyle Width="60px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="canUpdateStatus" HeaderText="UpdateStatus" ItemStyle-Width="60px">
                                    <ItemStyle Width="60px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="canDelete" HeaderText="Delete" ItemStyle-Width="60px">
                                    <ItemStyle Width="60px" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
