<%@ Page Title="Importazione Ore Formazione Specifica Volontari" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmImportOreFormazioneSpecificaVolontari.aspx.vb" Inherits="Futuro.WfrmImportOreFormazioneSpecificaVolontari" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Importazione Ore Formazione Specifica Volontari</legend>

        <div class="wrapper" style="width:100%">
            <div class="headers" >
                <h2><asp:Label ID="lblTitolo" runat="server"  Text="Importazione Ore Formazione Specifica Volontari"></asp:Label></h2>
            </div>
            <div class="row" style="height:auto">
                <asp:Label ID="lblMessaggio"  CssClass="msgConferma" runat="server"></asp:Label>
                <asp:Label ID="lblErrore" CssClass="msgErrore" runat="server"></asp:Label>
                <div class="RigaVuota">&nbsp;</div>
            </div>
            <div class="row">
             <div class="collable" style="width:25%">
                <asp:Label ID="lblRicercaVolontari" runat="server" AssociatedControlID="hplRicercaOre" Text="Avviare la ricerca delle ore volontari cliccando qui" CssClass="label"></asp:Label>
            </div>
             <div class="colHyperLink" style="width:75%">
                <asp:HyperLink ID="hplRicercaOre" NavigateUrl="~/WfrmRicercaOreFormazioneSpecificaVolontari.aspx?TipoFormazioneGenerale=1&VengoDa=''&VediEnte=1" runat="server" Text="Ricerca Ore Volontari" />
             </div>
            </div>
            <div class="row" >
             <div class="collable" style="width:25%">
                <asp:Label ID="lblUpload" runat="server" AssociatedControlID="txtSelFile" Text="Eseguire l'Upload del file completo" CssClass="label"></asp:Label>
            </div>
            <div class="colOggetti" style="width:45%">
            <asp:FileUpload ID="txtSelFile" Width="95%"  Height="20px" runat="server" ToolTip="File di Upload Ore Servizio Volontari Formazione Specifica"  />
            </div>
            <div class="collable" style="width:20%">
            <asp:Button ID="CmdElabora"  CssClass="Pulsante" Height="25px"  runat="server" Text="Elabora File" />
            </div>
               </div> 
            <div class="RigaVuota">&nbsp;
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
            </div>
        </div>  
        <asp:HiddenField ID="idAttivita" runat="server" />
    </fieldset>
</asp:Content>
