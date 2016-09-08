<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.Master" AutoEventWireup="true"
    CodeBehind="datatransfer.aspx.cs" Inherits="AWAPI.admin.PageDataTransfer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolder" runat="server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div id='center-column' style="width: 97%">
                <div class="top-bar" style="border-bottom: solid 1px #dcdcdc">
                    <h1>
                        Import Data</h1>
                </div>
                <div style="margin: 10px; padding: 3px;">
                    Environment:
                    <asp:DropDownList ID='_environmentList' runat='server' AutoPostBack="true" OnSelectedIndexChanged="_environmentList_SelectedIndexChanged" />
                    Module:
                    <asp:DropDownList ID='_module' runat='server' AutoPostBack="True" OnSelectedIndexChanged="_module_SelectedIndexChanged">
                        <asp:ListItem Text="Contensts" Value="contents" />
                        <asp:ListItem Text="Resource Groups" Value="resourceGroups" />
                        <asp:ListItem Text="Resources" Value="resources" />
                    </asp:DropDownList>
                    Service Url:
                    <asp:Label ID="_serviceUrl" runat='server' Text="" /><br />
                </div>
                <div class="select-bar">
                    <div style="float: left">
                        <ul>
                            <li>
                                <asp:ImageButton ID="btnConnect" runat="server" ImageUrl="img/button/new-icon.png"
                                    AlternateText="New Post" CausesValidation="false" ToolTip="Conntect To Remote"
                                    OnClick="btnConnect_Click" /><br />
                                <asp:Label ID='lblNewBlogPost_' runat='server' Text='Connect' />
                            </li>
                            <%-- <li class="seperator"></li>--%>
                        </ul>
                    </div>
                </div>
                <div style="overflow: scroll; height: 440px;">
                    <div style="float: left; width: 48%; padding: 2px;">
                        <asp:MultiView ID="_mvRemoteServer" runat='server' ActiveViewIndex="0">
                            <asp:View ID="_viewRemoteServerContent" runat='server'>
                                <h3>
                                    Remote Server</h3>
                                <asp:GridView ID="_gvRemoteServerList" runat='server' AutoGenerateColumns="False"
                                    OnRowDataBound="_remoteServerList_RowDataBound" Font-Names="courier new" GridLines="None"
                                    PageSize="30" OnRowCommand="_gvRemoteServerList_RowCommand" CellPadding="4" ForeColor="#333333">
                                    <RowStyle BackColor="#EFF3FB" />
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="100%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnContent_Alias" runat='server' Text='<%# Bind("alias") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="100%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnContent_CopyToRight" runat='server' Text='copy>>' OnClientClick='return confirm("Data will be copied from the remote server. Are you sure?")'
                                                    CommandName="copycontent" CommandArgument='<%# Bind("contentid") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </asp:View>
                        </asp:MultiView>
                    </div>
                    <div style="float: left; width: 48%; padding: 2px;">
                        <asp:MultiView ID="_mvCurrentServer" runat='server' ActiveViewIndex="0">
                            <asp:View ID="_viewCurrentServerContent" runat='server'>
                                <h3>
                                    Current Server</h3>
                                <asp:GridView ID="_gvCurrentServerList" runat='server' AutoGenerateColumns="False"
                                    Font-Names="courier new" GridLines="None" PageSize="30" CellPadding="4" ForeColor="#333333"
                                    OnRowDataBound="_gvCurrentServerList_RowDataBound" Width="100%" 
                                    onrowcommand="_gvCurrentServerList_RowCommand">
                                    <RowStyle BackColor="#EFF3FB" />
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="100%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnContent_Alias" runat='server' Text='<%# Bind("alias") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="100%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="100%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnContent_Delete" runat='server' Text='delete' OnClientClick='return confirm("Are you sure you want to delete the content?");'
                                                    CommandName="deletecontent" CommandArgument='<%# Bind("contentid") %>'  />
                                            </ItemTemplate>
                                        </asp:TemplateField>                                        
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </asp:View>
                        </asp:MultiView>
                    </div>
                </div>
                <div style='clear: both'>
                    &nbsp;</div>
                <div>
                    <font color='red'>Different</font><br />
                    <font color='green'>Does not exist</font><br />
                    <font color='gray'>Same</font><br />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
