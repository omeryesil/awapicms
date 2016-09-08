<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="noPermission.aspx.cs" Inherits="AWAPI.admin.noPermission" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>AWAPI - Permission Error</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h3>Permission Error</h3>
        You do not have permission to see the page, click <asp:HyperLink runat='server' Text='here' NavigateUrl="~/admin/default.aspx" /> to continue.
    </div>
    </form>
</body>
</html>
