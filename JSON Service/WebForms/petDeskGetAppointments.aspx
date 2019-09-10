<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="petDeskGetAppointments.aspx.cs" Inherits="SolutionsWeb.WebForms.petDeskGetAppointments" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PetDesk Get Appointments</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblinfo" runat="server"  ForeColor="black" Font-Bold="true" >Gathering appointments from PetDesk... please wait.</asp:Label>
        <br />
        <asp:Label ID="lberror" runat="server"  ForeColor="red" Font-Bold="true" ></asp:Label>
    </div>
</form>
</body>
</html>
