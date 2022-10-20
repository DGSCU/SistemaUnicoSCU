<%@ Page Title="Importazione Presenze" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmImportPresenzeVolontari.aspx.vb" Inherits="Futuro.WfrmImportPresenzeVolontari" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script  type="text/javascript">

     function Nascondi() {
         document.all.MainContent_lblMessaggioErrore.style.visibility = 'visible';
         document.all.MainContent_lblMessaggioErrore.style.fontSize = '1.6em'
         document.all.MainContent_lblMessaggioErrore.style.fontWeight = 'bold'
         document.all.MainContent_lblMessaggioErrore.style.color = '#3a4f63'
         document.all.MainContent_lblMessaggioErrore.innerText = 'ATTENDERE........';
         document.all.MainContent_CmdElabora.style.visibility = 'hidden';
     }
 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Importazione Presenze Volontari</legend>
        <div id="TabellaImportAssenzeVolontari" class="wrapper" style="width: 100%">
            <div class="headers">
                <h2>
                    <asp:Label ID="Label5" runat="server" Text="Import Presenze Volontari"></asp:Label>
                </h2>
            </div>
            <div class="RigaVuota">
            <p>
            ATTENZIONE: questa funzione deve essere utilizzata solo se si intende inserire le presenze/assenze dei volontari utilizzando un file CSV anziché la procedura ordinaria di "Inserimento Presenze/Assenze Volontario". 

Non riguarda quindi il caricamento del foglio presenza mensile scannerizzato con le firme del volontario che deve essere inserito sul sistema come descritto nel paragrafo 2.4 del manuale utente.
</p>
             </div>
           <p></p>
         
            <div class="row">
                <asp:Label ID="Label2" runat="server" Width="330px" Text="1) Scaricare il file presenzevolontari.csv"></asp:Label>
                <asp:HyperLink ID="HplFileAssenze" NavigateUrl="~/download/Master/presenzevolontari.csv"
                    runat="server">File presenzevolontari.csv</asp:HyperLink>
            </div>
            <div class="row">
                <asp:Label ID="Label3" runat="server" Width="330px" Text="2) Eseguire l'Upload del file completo"></asp:Label>
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
            <p>&nbsp;</p>
                
            <fieldset>
                <strong>Note:</strong>
               
                <h3>
                    Il campo Causale deve contenere:
                </h3>
              <p>
              <%Call caricalista()%>
             </p>
         
                <h3>
                    Il campo Giorno deve essere scritto nel formato:
                </h3>
                <ul>
                    <li><strong>gg/mm/aaaa</strong></li>
                   
                </ul>
               
                <br />
                <br />
            </fieldset>
            <div class="RigaPulsanti">
                <asp:Button ID="CmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />&nbsp;
            </div>
        </div>
    </fieldset>
    <p>
        &nbsp;</p>
</asp:Content>
