<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteNew.Master" CodeBehind="Login.aspx.vb" Inherits="Futuro.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>Errore nell'accesso al Sistema</h2>
	<h3><asp:Label ID="txtMessaggio" runat="server" CssClass="labelDati"  Text=""></asp:Label></h3>
	<a class="btn btn-primary" runat="server" id="lnkRitorna">Torna alla pagina di accesso</a>
</asp:Content>
