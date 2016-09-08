<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="contestusers.aspx.cs" Inherits="AWAPI.Admin.frames.contestusers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AWAPI - Manage Blog Categories</title>
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
    <div id="main" style="min-height: 505px;">
        <div id="header">
            <h3>
                <asp:Label ID="lblTitle" runat='server' Text="Contest Entries" /></h3>
        </div>
        <div id="middle">
            <div id='center-column' style="width: 100%;">
                <div id='top-bar'>
                    <%-- 
                    <asp:ImageButton ID="btnNew" runat="server" ImageUrl="../img/button/new-icon.png"
                        AlternateText="Add New Category" ToolTip="Add New Category" CausesValidation="False"
                        OnClick="btnNew_Click" />
                    <asp:ImageButton ID="btnSave" runat="server" ImageUrl="../img/button/save-icon.png"
                        AlternateText="Save" ToolTip="Save Category" ValidationGroup="save_" OnClick="btnSave_Click" />
                    <asp:ImageButton ID="btnDelete" runat="server" AlternateText="Delete" ImageUrl="../img/button/delete-icon.png"
                        ToolTip="Delete Category" OnClientClick="return confirm('Are you sure you want to delete the category?');"
                        Visible="False" ValidationGroup="save_" OnClick="btnDelete_Click" />
                        --%>
                    <asp:Literal ID="_msg" runat='server' />
                </div>
                <asp:TextBox ID='_keywordSearch' runat='server' />
                <asp:LinkButton ID='lbtnFilter' runat='server' Text='Filter' OnClick="lbtnFilter_Click" />
                &nbsp;<asp:LinkButton ID='lbtnClear' runat='server' Text='Clear' OnClick="lbtnClear_Click" />
                <hr />
                <div>
                    <div style="float: left">
                        <strong>Total entries : </strong>
                        <asp:Label ID='_totalEntries' Text="0" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;<strong>Today's
                            entries : </strong>
                        <asp:Label ID='_todaysEntries' Text="0" runat="server" />
                    </div>
                    <div style="float: right">
                        <asp:HyperLink ID="_hplExportExcel" runat='server' ImageUrl="../img/button/ms-excel.png"
                            Target="_blank" />
                    </div>
                </div>
                <div style="clear: both">
                </div>
                <hr />
                <div>
                    <div style="float: left; border-left: solid 1px #dcdcdc">
                        <asp:GridView ID='_list' runat='server' AutoGenerateColumns="False" BorderStyle="None"
                            CellPadding="3" CellSpacing="3" GridLines="None" AllowPaging="True" OnPageIndexChanging="_list_PageIndexChanging"
                            PageSize="10" AllowSorting='true' OnSorting="_list_Sorting">
                            <PagerSettings Mode="NumericFirstLast" />
                            <Columns>
                                <asp:TemplateField HeaderText="Users" HeaderStyle-HorizontalAlign="Left" SortExpression="email">
                                    <ItemTemplate>
                                        <strong>
                                            <asp:LinkButton ID="lbtnUserDetails" runat='server' Text='<%# Bind("email") %>' /></strong>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:GridView>
                    </div>
                    <div style="float: left; border-left: solid 1px #dcdcdc">
                        <h3>Files</h3>
                        <asp:GridView ID='_fileList' runat='server' AutoGenerateColumns="False" BorderStyle="None"
                            CellPadding="3" CellSpacing="3" GridLines="None" AllowPaging="True" OnPageIndexChanging="_list_PageIndexChanging"
                            PageSize="10" AllowSorting='true' OnSorting="_list_Sorting">
                            <PagerSettings Mode="NumericFirstLast" />
                            <Columns>
                                <asp:TemplateField HeaderText="Users" HeaderStyle-HorizontalAlign="Left" SortExpression="email">
                                    <ItemTemplate>
                                        <strong>
                                            <asp:LinkButton ID="lbtnUserDetails" runat='server' Text='<%# Bind("email") %>' /></strong>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
