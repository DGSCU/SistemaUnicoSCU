<%@ Page Title="Importazione Ore Formazione" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRisultatoImportOreVolontari.aspx.vb" Inherits="Futuro.WfrmRisultatoImportOreVolontari" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="firefox" >
        <fieldset class="ContornoPaginaScroll" >
            <legend>Importazione Ore Formazione</legend>
            <div class="headers">
                <h2><asp:Label ID="lblTitolo" runat="server"  Text="Importazione Ore Formazione"></asp:Label></h2>
            </div>
            <br />
            <asp:Label ID="lblEsito" CssClass="bold" AssociatedControlID="dtgCSV" runat="server" Visible="false"></asp:Label>
            <br />
            <br />
            <div class="wrapper" style="width:100%; border-style:none">
            <div class="RigaVuota" >&nbsp;</div>
            <asp:DataGrid ID="dtgCSV" runat="server" Width="100%" CssClass="table" CellPadding="3" PageSize="5" AutoGenerateColumns="True">
				<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr"></ItemStyle>
		        <HeaderStyle></HeaderStyle>
			</asp:DataGrid>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="collable" style="width:100%">
                <asp:Label ID="lblTotali" CssClass="bold" AssociatedControlID="hlDownLoad" runat="server"></asp:Label>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
              <div id="AvvisoConferma" runat="server" visible="false">
     <ul style="border-color:Red;border-style:ridge;margin:1% 20% 1% 20%;text-align:left;" runat="server" id="avviso">
        <dt style="margin-left:5px;font-size:1.6em;font-weight:bold;color:Red">Attenzione!!!</dt>
        <dt style="margin-left:5px;font-size:1.2em;font-weight:bold" runat="server" id="testoavviso"></dt>
       <%-- <li style="margin-left:5px;font-size:1.4em;font-weight:bold;">Il bando dei volontari funzionerà normalmente. Il concorso “Realizzazione di uno spot televisivo e di un cortometraggio di promozione del Servizio Civile Nazionale” funzionerà regolarmente.</li>--%>

    </ul>
    </div>
            <asp:HyperLink ID="hlDownLoad" CssClass="linkStampa" runat="server">Scarica il file completo nel formato CSV</asp:HyperLink>
            <div class="RigaPulsanti">
                <asp:Button ID="CmdConferma" runat="server" CssClass="Pulsante" Text="Conferma" />&nbsp;
                <asp:Button ID="CmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
            </div>
        </div>
        </fieldset>
    </div>
</asp:Content>
