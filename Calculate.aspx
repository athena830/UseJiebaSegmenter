<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Calculate.aspx.cs" Inherits="UseJiebaSegmenter.Calculate" MasterPageFile="~/Master.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
            <asp:Label ID="Label1" runat="server" Text=""></asp:Label><br />
            <asp:Button ID="Button1" runat="server" CssClass="btn btn-success" Text="FB 計算分數" OnClick="Button1_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button2" runat="server" CssClass="btn btn-primary" Text="RSS 計算分數" OnClick="Button2_Click" /><br />
</asp:Content>
