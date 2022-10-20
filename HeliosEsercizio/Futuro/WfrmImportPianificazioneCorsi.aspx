<%@ Page Title="Importazione Pianificazione Formazione Corsi" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmImportPianificazioneCorsi.aspx.vb" Inherits="Futuro.WfrmImportPianificazioneCorsi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <script type="text/javascript">
//<![CDATA[

         function NascondiUnicaTrance() {
             document.all.MainContent_lblMessaggioErroreUnicaTrance.style.visibility = 'visible';
             document.all.MainContent_lblMessaggioErroreUnicaTrance.style.fontSize = '1.6em'
             document.all.MainContent_lblMessaggioErroreUnicaTrance.style.fontWeight = 'bold'
             document.all.MainContent_lblMessaggioErroreUnicaTrance.style.color = '#3a4f63'
             document.all.MainContent_lblMessaggioErroreUnicaTrance.innerText = 'ATTENDERE........';
             document.all.MainContent_CmdElaboraUnicaTrance.style.visibility = 'hidden';
         }

         function NascondiDoppiaTrance() {
             document.all.MainContent_lblMessaggioErroreDoppiaTrance.style.visibility = 'visible';
             document.all.MainContent_lblMessaggioErroreDoppiaTrance.style.fontSize = '1.6em'
             document.all.MainContent_lblMessaggioErroreDoppiaTrance.style.fontWeight = 'bold'
             document.all.MainContent_lblMessaggioErroreDoppiaTrance.style.color = '#3a4f63'
             document.all.MainContent_lblMessaggioErroreDoppiaTrance.innerText = 'ATTENDERE........';
             document.all.MainContent_CmdElaboraDoppiaTrance.style.visibility = 'hidden';
         }
//]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Importazione Pianificazione Formazione Corsi</legend>
        <div class="wrapper" style="width:100%">
            <div class="headers" >
                <h2><asp:Label ID="lblTitolo" runat="server"  Text="Importazione Pianificazione Formazione Corsi"></asp:Label></h2>
            </div>
            <div class="RigaVuota">&nbsp;</div>
            <h3 style="font-weight:bold">Pianificazione Formazione con modalità di erogazione UNICA TRANCHE</h3>
            <p> Avviare la ricerca dei Progetti da Pianificare cliccando qui&nbsp;&nbsp;&nbsp;
                <asp:HyperLink ID="hplRicercaProgettiUnicaTrance" NavigateUrl="~/WfrmRicercaProgettiFormazione.aspx?TipoFormazioneGenerale=1&VediEnte=1" runat="server">Ricerca Progetti Da Pianificare</asp:HyperLink> <br />
                Eseguire l'Upload del file completo&nbsp;&nbsp;&nbsp;
                <asp:FileUpload ID="txtSelFileUnicaTrance" runat="server" ToolTip="File di Upload Unica Trance" style="width:40%" />&nbsp;
                <asp:Button ID="CmdElaboraUnicaTrance" OnClientClick="NascondiUnicaTrance()" CssClass="Pulsante" runat="server" Text="Elabora" />
                <asp:Label ID="lblMessaggioErroreUnicaTrance" CssClass="msgErrore" runat="server" Text="" AssociatedControlID="txtSelFileUnicaTrance"></asp:Label>
            </p>
            <br />
            <h3 style="font-weight:bold">Pianificazione Formazione con modalità di erogazione DUE TRANCHE 80% - 20%</h3>
            <p> Avviare la ricerca dei Progetti da Pianificare cliccando qui &nbsp;&nbsp;&nbsp;
                <asp:HyperLink ID="hplRicercaProgettiDoppiaTrance" NavigateUrl="~/WfrmRicercaProgettiFormazione.aspx?TipoFormazioneGenerale=2&VediEnte=1" runat="server">Ricerca Progetti Da Pianificare</asp:HyperLink> <br />
                Eseguire l'Upload del file completo&nbsp;&nbsp;&nbsp;
                <asp:FileUpload ID="txtSelFileDoppiaTrance" runat="server" ToolTip="File di Upload Doppia Trance" style="width:40%"/>&nbsp;
                <asp:Button ID="CmdElaboraDoppiaTrance" OnClientClick="NascondiDoppiaTrance()" CssClass="Pulsante" runat="server" Text="Elabora" />
                <asp:Label ID="lblMessaggioErroreDoppiaTrance" CssClass="msgErrore" runat="server" Text="" AssociatedControlID="txtSelFileDoppiaTrance"></asp:Label>
            </p>
            <div class="RigaVuota">&nbsp;</div>
            <strong>Note</strong>
            <br />
            <p>
				Tutti i valori inseriti che contengo il carattere <strong>;</strong> devono essere racchiusi tra due virgolette <strong>" "</strong>
            </p>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
            </div>
        </div> 
        
        
        
    </fieldset>
</asp:Content>
