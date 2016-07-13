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
            <%--<asp:Button ID="Button1" runat="server" Text="切切切" OnClick="Button1_Click" /><br /><br/><br/>--%>
            <asp:Label ID="Label2" runat="server" Text=""></asp:Label><br />
            <asp:ListView runat="server" ID="ListView_result">
                <ItemTemplate>
                        <tr>
                            <td><%# Eval("Number") %></td>
                            <td><%# Eval("Sentence") %></td>
                            <td><%# Eval("Memo") %></td>
                            <td><%# Eval("Score") %></td>
                            <td>
                                <asp:Button ID="Btn_Yes" runat="server" Text="有" OnCommand="Btn_Command" CommandArgument="yes" CommandName='<%# Eval("ID")+"|"+Eval("Score") %>' />
                                <asp:Button ID="Btn_No" runat="server" Text="無" OnCommand="Btn_Command" CommandArgument="no" CommandName='<%# Eval("ID")+"|"+Eval("Score") %>'/></td>
                        </tr>
                </ItemTemplate>
                <LayoutTemplate>
                    <table border="1">
                        <tr>
                            <th>Number</th>
                            <th style="width:50%">Sentence</th>
                            <th>Memo</th>
                             <th>Score</th>
                            <th>有無變異點</th>
                       </tr>
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                    </table>
                </LayoutTemplate>
            </asp:ListView> 
        </div>
    </form>
</body>
</html>
