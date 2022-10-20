<%@ Page Title="Risultato Importazione File di Graduatorie Volontari" Language="vb"
    AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRisultatoImportIbanUNSC.aspx.vb"
    Inherits="Futuro.WfrmRisultatoImportIbanUNSC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" language="javascript">

        function ApriSuggerimento(idcomune, indirizzo) {
            window.open("wfrmSuggerimenti.aspx?idcomune=" + idcomune + "&strIndirizzo=" + indirizzo, "Suggerimenti", "height=500,width=500,dependent=no,scrollbars=yes,status=no,resizable=yes");
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
<legend >Import Iban Volontari CSV</legend>
    <div class="headers">
        <h2>
            <asp:Label ID="Label5" runat="server" Text="Importazione Codici Iban"></asp:Label>
        </h2>
    </div>
    <p>
        <asp:Label ID="lblEsito" CssClass="bold" runat="server" Visible="False">Label</asp:Label>
    </p>
    <p>
        <asp:Label ID="lblDenominazioneEnte" runat="server" CssClass="bold">Denominazione Ente</asp:Label>
    </p>
    <p>
        <asp:Label ID="lblCodiceEnte" runat="server" CssClass="bold">Codice Ente</asp:Label>
    </p>
    <p>
        <asp:Label ID="lblNoteEnte" runat="server" CssClass="msgErrore">Note</asp:Label>
    </p>
    <p>
        <asp:Label ID="lblTotali" CssClass="bold" runat="server"></asp:Label>
    </p>
      <div id="AvvisoConferma" runat="server" visible="false">
     <ul style="border-color:Red;border-style:ridge;margin:1% 20% 1% 20%;text-align:left;" runat="server" id="avviso">
        <dt style="margin-left:5px;font-size:1.6em;font-weight:bold;color:Red">Attenzione!!!</dt>
        <dt style="margin-left:5px;font-size:1.2em;font-weight:bold" runat="server" id="testoavviso"></dt>
       <%-- <li style="margin-left:5px;font-size:1.4em;font-weight:bold;">Il bando dei volontari funzionerà normalmente. Il concorso “Realizzazione di uno spot televisivo e di un cortometraggio di promozione del Servizio Civile Nazionale” funzionerà regolarmente.</li>--%>

    </ul>
    </div>
    <p>
        <asp:HyperLink ID="hlDownLoad" CssClass="linkStampa" runat="server">Scarica il file completo nel formato CSV</asp:HyperLink>
         </p>
    <p>
        <asp:hyperlink id="hlErrore" runat="server" CssClass="linkStampa" Visible="False">Scaricare il file degli errori</asp:hyperlink>
    </p>
    <p class="RigaPulsanti">
        <asp:Button ID="CmdConferma" runat="server" CssClass="Pulsante" Text="Conferma" />&nbsp;&nbsp;<asp:Button
            ID="CmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
    </p>
    <p>
        <asp:DataGrid ID="dtgCSV" runat="server" Width="100%" CssClass="table" AutoGenerateColumns="True"
            AllowSorting="True" CellPadding="3" PageSize="5">
            <FooterStyle></FooterStyle>
            <SelectedItemStyle BackColor="White"></SelectedItemStyle>
            <EditItemStyle></EditItemStyle>
            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
            <ItemStyle CssClass="tr"></ItemStyle>
            <HeaderStyle CssClass="linkPageGrid"></HeaderStyle>
        </asp:DataGrid>
    </p>
   
        </fieldset>
</asp:Content>
