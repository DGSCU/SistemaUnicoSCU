<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmImportAccettazioneProgetti.aspx.vb" Inherits="Futuro.WfrmImportAccettazioneProgetti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script language="javascript" type="text/javascript">
//<![CDATA[


    function Nascondi() {
        document.all.MainContent_lblMessaggioErrore.style.visibility = 'visible';
        document.all.MainContent_lblMessaggioErrore.style.fontSize = '1.6em'
        document.all.MainContent_lblMessaggioErrore.style.fontWeight = 'bold'
        document.all.MainContent_lblMessaggioErrore.style.color = '#3a4f63'
        document.all.MainContent_lblMessaggioErrore.innerText = 'ATTENDERE........';
        document.all.MainContent_CmdElabora.style.visibility = 'hidden';
    }
 
//]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
        <legend>Importazione File Accettazione Progetti</legend>
        <div id="TabellaImportAccettazioneProgetti" class="wrapper" style="width: 100%">
            <div class="headers">
                <h2>
                    <asp:Label ID="Label5" runat="server" Text="Import File Accettazione Progetti"></asp:Label>
                </h2>
            </div>
            <div class="RigaVuota">
                &nbsp;</div>
           
         
            <div class="row">
                <asp:Label ID="lblTipoImport" runat="server"  Text="Scaricare il file cliccando qui"></asp:Label>&nbsp;&nbsp;&nbsp;
                <asp:HyperLink ID="hplFile" NavigateUrl="" runat="server" Text="file"></asp:HyperLink> <br />

            </div>
           
            <div class="row">
                <asp:Label ID="Label3" runat="server" Width="330px" Text="2) Eseguire l'Upload del file completato"></asp:Label>
                <br />
                <br />
                <p>
                    <asp:Label ID="lblUpLoad" runat="server" AssociatedControlID="txtSelFile" Text="Seleziona File: "></asp:Label><asp:FileUpload
                        ID="txtSelFile" runat="server" />
                    &nbsp;<asp:Button ID="CmdElabora" OnClientClick="Nascondi()" CssClass="Pulsante"
                        runat="server" Text="Elabora" /><asp:Label ID="lblMessaggioErrore" CssClass="msgErrore"
                            runat="server" Text=""></asp:Label>
                </p>
            </div>
            <p>
                &nbsp;</p>
            <fieldset>
                <strong>Note:</strong>
                <h3>
		            Il campo TipoEsitoPositivo deve contenere:
                </h3>
                    AMMISSIBILE<br />
                    AMMISSIBILE CON LIMITAZIONI
                <br />
               
            </fieldset>
            <div class="RigaPulsanti">
<asp:HiddenField ID="hf_IdEnte" runat="server"/>
                <asp:Button ID="CmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />&nbsp;
            </div>
        </div>
    </fieldset>
    <p>
        &nbsp;</p>
</asp:Content>
