<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="UseJiebaSegmenter._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text=""></asp:Label><br />
            <asp:Button ID="Button1" runat="server" Text="切切切" OnClick="Button1_Click" /><br />
            <asp:Label ID="Label2" runat="server" Text=""></asp:Label><br />
            <asp:ListView runat="server" ID="ListView_result">
                <ItemTemplate>
                        <tr>
                            <td><%# Eval("Number") %></td>
                            <td><%# Eval("Sentence") %></td>
                        </tr>
                </ItemTemplate>
                <LayoutTemplate>
                    <table border="1">
                        <tr>
                            <th>Number</th>
                            <th>Sentence</th>
                        </tr>
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                    </table>
                </LayoutTemplate>
            </asp:ListView> 
        </div>
    </form>
</body>
</html>
