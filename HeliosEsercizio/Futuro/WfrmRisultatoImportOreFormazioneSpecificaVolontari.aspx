<%@ Page Title="Importazione Ore Formazione Specifiva" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRisultatoImportOreFormazioneSpecificaVolontari.aspx.vb" Inherits="Futuro.WfrmRisultatoImportOreFormazioneSpecificaVolontari" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="firefox" >
        <fieldset class="ContornoPaginaScroll">
            <legend>Importazione Ore Formazione Specifica</legend>
            <div class="wrapper" >
            <div class="headers">
                <h2><asp:Label ID="lblTitolo" runat="server"  Text="Importazione Ore Formazione Specifica"></asp:Label></h2>
            </div>
            <div class="rowGroup" style="height:auto">
            <div class="row" style="height:auto">
            <asp:Label ID="lblConferma"  runat="server" CssClass="msgConferma"></asp:Label>
             <asp:Label ID="lblErrore"  runat="server" CssClass="msgErrore"></asp:Label>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            </div>
            <div class="wrapper" style="width:100%; border-style:none">
            <div class="row" style="height:auto">
                <div class="collable" style="width:100%">
                <asp:Label ID="lblTotali" CssClass="msgInfo" AssociatedControlID="hlDownLoad" runat="server"></asp:Label>
            </div>
            </div>
            <asp:DataGrid ID="dtgCSV" runat="server" Width="100%" ToolTip="Elenco Volontari"  Caption="Elenco Volontari" 
                   AllowPaging="false" PageSize="5"  CssClass="table"
                   AutoGenerateColumns="true" UseAccessibleHeader="True" Visible="true">
		    <FooterStyle HorizontalAlign="Center"></FooterStyle>
	        <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	        <EditItemStyle></EditItemStyle>
	        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	        <ItemStyle CssClass="tr" HorizontalAlign ="Center"></ItemStyle>
	        <HeaderStyle></HeaderStyle>
            <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Justify" CssClass="linkPageGrid"  
                    Mode="NumericPages"></PagerStyle>
			</asp:DataGrid>
            <div class="RigaVuota" >&nbsp;</div>
              <div id="AvvisoConferma" runat="server" visible="false">
     <ul style="border-color:Red;border-style:ridge;margin:1% 20% 1% 20%;text-align:left;" runat="server" id="avviso">
        <dt style="margin-left:5px;font-size:1.6em;font-weight:bold;color:Red">Attenzione!!!</dt>
        <dt style="margin-left:5px;font-size:1.2em;font-weight:bold" runat="server" id="testoavviso"></dt>
       <%-- <li style="margin-left:5px;font-size:1.4em;font-weight:bold;">Il bando dei volontari funzionerà normalmente. Il concorso “Realizzazione di uno spot televisivo e di un cortometraggio di promozione del Servizio Civile Nazionale” funzionerà regolarmente.</li>--%>

    </ul>
    </div>
            <asp:HyperLink ID="hlDownLoad" CssClass="linkStampa" runat="server" Text="Scarica il file completo nel formato CSV"></asp:HyperLink>
            <div class="RigaPulsanti">
                <asp:Button ID="CmdConferma" runat="server" CssClass="Pulsante" Text="Conferma" />&nbsp;
                <asp:Button ID="CmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
            </div>
        </div>
        </div>
        </fieldset>
    </div>
</asp:Content>