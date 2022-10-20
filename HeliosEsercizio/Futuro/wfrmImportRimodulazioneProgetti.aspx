<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="wfrmImportRimodulazioneProgetti.aspx.vb" Inherits="Futuro.wfrmImportRimodulazioneProgetti" %>
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
        <legend>Importazione File Rimodulazione Progetti COVID</legend>
        <div id="TabellaImportAssenzeVolontari" class="wrapper" style="width: 100%">
            <div class="headers">
                <h2>
                    <asp:Label ID="Label5" runat="server" Text="Import File Rimodulazione Progetti COVID"></asp:Label>
                </h2>
            </div>
            <div class="RigaVuota">
                &nbsp;</div>
           
         
            <div class="row">
                <asp:Label ID="Label2" runat="server" Width="330px" Text="1) Scaricare il file Rimodulazione Progetti   .csv"></asp:Label>
                <!--<asp:HyperLink ID="HplFileAssenze" NavigateUrl="~/download/Master/assenzevolontari.csv"
                    runat="server">File assenzevolontari.csv</asp:HyperLink>-->
            &nbsp;&nbsp;
             <asp:Button ID="imgEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV"
            Visible="true" ToolTip="Esporta nel formato CSV" />
            <asp:hyperlink id="hlVolontari" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" 
        Text="DOWNLOAD CSV" runat="server" 
        ForeColor="#003399" Visible="False"></asp:hyperlink>

            </div>
            <br />
            <br />
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
		            Il campo StatoAttivazione deve contenere:
                </h3>
                    <%Call StatiRiattivazione()%>
                <br />
                
                 <h3>
		            Il campo ModalitàServizio deve contenere:
                </h3>
                    <%Call TipoModalitàServizio()%>
                <br />
                
                                <h3>
		            Il campo CodiceAttivitàRimodulazione deve contenere:
                </h3>
                    <%Call CodiceAttivitàRimodulazione()%>
                <br />
                 <h3>
                    Campo SpecificaAltro:
                </h3>
                <ul>
                    <li><strong>Si deve indicare solo se CodiceAttivitàRimodulazione corrisponde ad ALTRO. </strong></li>
                </ul>
                 <h3>
		            Il campo DataValidità:
                </h3>
                <ul>
                    <li><strong>indica la data prevista per l'attivazione della misura.</strong></li>
                </ul>
                    <%--<%Call DataRipresaServizio()%>--%>
                 <h3>
		            Il campo TipoEmergenza deve contenere:
                </h3>
                <ul>
                    <li><strong>COVID</strong></li>
                </ul>
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
