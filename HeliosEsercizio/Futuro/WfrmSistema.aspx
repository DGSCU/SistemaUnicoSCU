<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmSistema.aspx.vb"  MasterPageFile="~/SiteNew.Master" Inherits="Futuro.WfrmSistema" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">


	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">




<h3>Scelta Sistema</h3>
<div class="row">
<div class ="col-6">
     <asp:ImageButton ID="ImgHeliosAccesso" AlternateText="Accesso  Helios" ToolTip="Scelta Sistema Helios" Height="100%" Width="100%" ImageUrl="~/Images/HELIOS_LOGO_BIG.jpg" runat="server"  /> 
     <asp:ImageButton ID="ImgHeliosAccessoGrey" AlternateText="Accesso  Helios" Visible="false" ToolTip="Scelta Sistema Helios" Height="100%" Width="100%" ImageUrl="~/Images/HELIOS_LOGO_BIG_ND.jpg" runat="server"  />
</div>
<div class ="col-6">
	<asp:ImageButton ID="ImgFuturoAccesso" ImageUrl="~/Images/FUTURO_LOGO_BIG.jpg" AlternateText="Accesso a Futuro" ToolTip="Scelta Sistema Futuro" Height="100%" Width="100%"  runat="server" />
    <asp:ImageButton ID="ImgFuturoAccessoGrey" 	ImageUrl="~/Images/FUTURO_LOGO_BIG_ND.jpg" AlternateText="Accesso a Futuro" Visible="false" ToolTip="Scelta Sistema Futuro" Height="100%" Width="100%"  runat="server" />
</div>
</div>

</asp:Content>