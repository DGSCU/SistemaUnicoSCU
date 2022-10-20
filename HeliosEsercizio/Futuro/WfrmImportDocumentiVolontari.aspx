<%@ Page Title="Importazione Documenti Volontari" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmImportDocumentiVolontari.aspx.vb" Inherits="Futuro.WfrmImportDocumentiVolontari" %>
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

        function toggleSettoriAree() {
            var x = document.getElementById("divSettoriAree");
            var y = document.getElementById("btnSettoriAree");
            if (x.style.display === "none") {
                x.style.display = "block";
                y.innerHTML = "Nascondi elenco Settori/Aree"
            } else {
                x.style.display = "none";
                y.innerHTML = "Visualizza elenco Settori/Aree"
            }
        }

    //]]>
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

<fieldset class="ContornoPagina">
        <legend><asp:Label ID="lblTitoloLegend" runat="server" Text=""/></legend> 
             
        <div id="DivImportCV" runat="server">
                <p> 
                    <asp:FileUpload ID="FileCV" runat="server" ToolTip="File zip dei Curricula" style="width:40%" />&nbsp;
                    <asp:Button ID="btnCaricaCV"  CssClass="Pulsante" runat="server" Text="Elabora" />
                </p>
                <asp:Label runat="server" ID="txtErroreCV" CssClass="msgErrore"></asp:Label>

                <fieldset>
                    <strong>Note:</strong>
                    <h3>Caricare un file in formato Zip (la cui dimensione non deve superare i 50Mb) contenente files con le seguenti caratteristiche:</h3>
                    <ul>
                    <li>I Files devono essere di tipo Pdf o Pdf.P7m</li>
                    <li>I Codici Volontario associati ai files devono essere presenti nell'ente</li>
                    <li>I Files devono essere tutti distinti</li>
                    <li>La Nomenclatura dei files deve essere del tipo: PREFISSO_CODICEVOLONTARIO_[eventuale descrizione aggiuntiva libera].PDF</li>
                    <li>I Prefissi disponibili sono nella tabella seguente</li>
                    <asp:datagrid id="dgElencoPrefissi" runat="server" Width="100%"  ToolTip="Elenco Prefissi Documenti Volontario" 
                    Caption="Elenco Prefissi Documenti Volontario" AllowPaging="false" CellPadding="2" Font-Size="Small"  CssClass="table"  
                    AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
                    <FooterStyle></FooterStyle>
                    <SelectedItemStyle Font-Bold="true" BackColor="White"></SelectedItemStyle>
                    <EditItemStyle></EditItemStyle>
                    <AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
                    <ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
                    <HeaderStyle></HeaderStyle>
                    <Columns>
                    <asp:BoundColumn DataField="Prefisso" HeaderText="Prefisso">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="TipologiaDocumento" HeaderText="Tipologia Documento">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Modalit&#224;Invio" HeaderText="Modalit&#224; Invio">
                    </asp:BoundColumn>
                    </Columns>                              
                    </asp:datagrid>
                    </ul>
                </fieldset>
            </div>

                <%--POPUP Messaggi--%>
            <asp:ModalPopupExtender runat="server" ID="modalCVResult" TargetControlID="hfCVResult" 
                PopupControlID="pnlCVResult" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
                BehaviorID="pnlCVResult" Enabled="True" CancelControlID="btnCloseCVResult"> 
            </asp:ModalPopupExtender>
	        <asp:Panel ID="pnlCVResult" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
                <asp:HiddenField runat="server" ID="hfCVResult" />
                <p class="msgInfo">Elaborazione File Importazione Documenti Volontari</p>
                <asp:Label runat="server" ID="lblElaborazioneCV"></asp:Label>
                <asp:Literal runat="server" ID="lstErroriCV"></asp:Literal>
                <div style="margin-top: 1em;"></div>
                <asp:Button ID="btnCloseCVResult" runat="server" CssClass="Pulsante" Text="Chiudi"/>
	        </asp:Panel>
       
        <div class="RigaPulsanti">
        <asp:Button 
            ID="CmdChiudi" CssClass="Pulsante" runat="server"
              Text="Chiudi"  />&nbsp;
       </div>

       <%-- <p>&nbsp;</p>--%>
    </fieldset>

</asp:Content>
