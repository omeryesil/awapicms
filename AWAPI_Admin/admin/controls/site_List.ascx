<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="site_List.ascx.cs" Inherits="AWAPI.Admin.controls.site_List" %>
<asp:GridView ID="gwSiteList" runat="server" AutoGenerateColumns="False" BorderStyle="None"
    BorderWidth="0px" CellPadding="2" CellSpacing="2" Width="100%"
    GridLines="None" OnRowCommand="gwSiteList_RowCommand">
    <HeaderStyle HorizontalAlign="Left" />
    <Columns>
        <asp:TemplateField HeaderText="title">
            <ItemTemplate>
                <asp:Label ID="lblTitle" runat="server" Text='<%# Bind("title") %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle Width="100%" />
        </asp:TemplateField>
        <asp:BoundField DataField="siteId" HeaderText="Id" />
        <asp:TemplateField>
            <ItemTemplate>
                <asp:LinkButton ID='lbtnSelectSite' runat='server' Text='select' CommandName="select_site"
                    BorderStyle="None" CommandArgument='<%# Bind("siteId") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
