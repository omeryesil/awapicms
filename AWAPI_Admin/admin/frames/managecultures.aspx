<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="managecultures.aspx.cs"
    Inherits="AWAPI.Admin.frames.managecultures" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AWAPI - Manage Cultures</title>
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
    <div id="main" style="min-height: 350px;">
        <div id="header">
            <h3>
                Manage Cultures</h3>
        </div>
        <div id="middle" style="width: 380px; min-height: 300px; height: 300px;">
            <div id='left-column' style="width: 100px; min-height: 300px; overflow: auto;">
                <strong>Cultures</strong><br />
                <asp:UpdatePanel ID="UpdatePanel1" runat='server'>
                    <ContentTemplate>
                        <asp:GridView ID='_list' runat='server' AutoGenerateColumns="False" BorderStyle="None"
                            CellPadding="3" CellSpacing="3" GridLines="None" ShowHeader="false" OnRowCommand="_list_RowCommand">
                            <Columns>
                                <asp:BoundField DataField='title' />
                                <asp:TemplateField HeaderText="title">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnAdd" runat='server' Text='Add' CommandName="addculture"
                                            CommandArgument='<%# Bind("cultureCode") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="100%" Wrap="True" HorizontalAlign="Left" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id='center-column' style="width: 200px; padding: 1px 14px 0 12px;">
                <strong>Site Cultures</strong><br />
                <asp:Literal ID='_msg' runat='server' />
                <asp:UpdatePanel ID="UpdatePanel2" runat='server'>
                    <ContentTemplate>
                        <asp:GridView ID='_culturesInSite' runat='server' AutoGenerateColumns="False" BorderStyle="None"
                            CellPadding="3" CellSpacing="3" GridLines="None" ShowHeader="false" OnRowCommand="_culturesInSite_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="title" />
                                <asp:TemplateField HeaderText="title">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnTitle" runat='server' Text='Remove' CommandName="removeculture"
                                            CommandArgument='<%# Bind("cultureCode") %>' OnClientClick='return confirm("Are you sure you want to remove the culture?");' />
                                    </ItemTemplate>
                                    <ItemStyle Width="100%" Wrap="True" HorizontalAlign="Left" />
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
