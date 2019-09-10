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

        <table width="1200" border="1" cellspacing="10" cellpadding="10">
            <tbody>
            <tr>
                <td class="auto-style1">
                    <asp:GridView ID="dgvResults" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="10" CellSpacing="5" GridLines="Vertical">
                        <AlternatingRowStyle BackColor="#DCDCDC" />
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#0000A9" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#000065" />
                    </asp:GridView>
                </td>
            </tr>
            </tbody>
        </table>
    </div>
</form>
</body>
</html>
