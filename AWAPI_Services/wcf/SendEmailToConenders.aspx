<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendEmailToConenders.aspx.cs"
    Inherits="AWAPI_Services.wcf.test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>
            Email sender</h1>
            Contest Group Id:<br />
        <asp:TextBox ID="_groupId" runat="server" Text="6257716035721343807" 
            Width="168px" /><br />
        Start Date:<br />
        <asp:TextBox ID="_startDate" runat="server" Text="02/25/2010 00:00:01" 
            Width="165px"/><i>MM/dd/yyyy HH:mm</i><br />
        End Date:<br />
        <asp:TextBox ID="_endDate" runat="server"  Text="03/01/2010 23:59:59" 
            Width="165px" /><i>MM/dd/yyyy HH:mm</i><br />
        <asp:Button ID="_btnPrepareData" runat='server' Text="Prepare Data" 
            onclick="_btnPrepareData_Click" />
            <asp:Label ID="_prepareDataResult" runat="server" />
        <hr />
        Email Template Id:<br />
        <asp:TextBox ID="_emailTemplateId" runat='server' Width="168px"  Text="7029348735959640713" /><br />
        &nbsp;<br />
        Number of emails to send:
        <br />
        <asp:TextBox ID="_numberOfEmailsToSend" runat="server" Text="25" Width="50px" /><br />
        <asp:Button ID="_sendEmail" runat='server' Text="Send Email" 
            onclick="_sendEmail_Click" />
        <br />
        <asp:GridView ID="_resultList" runat='server' AllowPaging='true' PageSize="20" 
            onpageindexchanging="_resultList_PageIndexChanging">
        </asp:GridView>
        
    </div>
    </form>
</body>
</html>
