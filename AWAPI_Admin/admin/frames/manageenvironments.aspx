<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manageenvironments.aspx.cs"
    Inherits="AWAPI.Admin.frames.manageEnvironments" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AWAPI - Manage Post Environments</title>
    <link href="../css/iframe.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID='scriptManager1' runat='server'>
    </asp:ScriptManager>
    <div id="main" style="min-height: 500px;">
        <div id="header">
            <h3>
                Manage Enviroments</h3>
        </div>
        <div id="middle" style="width: 95%; min-height: 450px; height: 450px;">
            <div id='left-column' style="width: 250px; min-height: 450px; overflow: auto;">
                <asp:UpdatePanel ID="UpdatePanel2" runat='server'>
                    <ContentTemplate>
                        <asp:GridView ID='_enviromentList' runat='server' AutoGenerateColumns="False" BorderStyle="None"
                            CellPadding="3" CellSpacing="3" GridLines="None" Width="100%" AllowPaging="True"
                            OnPageIndexChanging="_enviromentList_PageIndexChanging" OnRowCommand="_enviromentList_RowCommand">
                            <HeaderStyle VerticalAlign="Top" HorizontalAlign="Left" />
                            <Columns>
                                <asp:TemplateField HeaderText="Title" ItemStyle-Width="100%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID='lbtnTitle' runat='server' Text='<%# Bind("title") %>' CommandName="editenviroment"
                                            CommandArgument='<%# Bind("environmentID") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="100%" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="serviceUrl" HeaderText="ServiceUrl" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id='center-column' style="padding: 1px 14px 0 12px;">
                <asp:UpdatePanel ID="UpdatePanel1" runat='server'>
                    <ContentTemplate>
                        <div id='top-bar'>
                            <asp:ImageButton ID="btnNewBlogPostEnvironment_" runat="server" ImageUrl="~/admin/img/button/new-icon.png"
                                AlternateText="Add New Environment" CausesValidation="False" OnClick="btnNew_Click" />
                            <asp:ImageButton ID="btnSaveBlogPostEnvironment_" runat="server" ImageUrl="~/admin/img/button/save-icon.png"
                                AlternateText="Save" ValidationGroup="_saveenviroment" OnClick="btnSave_Click" />
                            <asp:ImageButton ID="btnDeleteEnvironment_" runat="server" AlternateText="Delete Environment"
                                ImageUrl="~/admin/img/button/delete-icon.png" OnClientClick="return confirm('Are you sure you want to delete the enviroment?');"
                                OnClick="btnDelete_Click" />
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
                                        Title: required
                                    </td>
                                    <td>
                                        <asp:TextBox ID="_title" runat="server" CssClass="textMedium" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="_title"
                                            Display="Dynamic" ErrorMessage="required" ValidationGroup="_saveenviroment"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Service Url:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="_serviceUrl" runat="server" CssClass="textLong" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="_serviceUrl"
                                            Display="Dynamic" ErrorMessage="required" ValidationGroup="_saveenviroment"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Description:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="_description" runat="server" CssClass="textLong" Height="96px"
                                            TextMode="MultiLine" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Public Key:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="_publicKey" runat="server" CssClass="textLong" Style="height: 170px;"
                                            TextMode="MultiLine" />
                                        <br />
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
