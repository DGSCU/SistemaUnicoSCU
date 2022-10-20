<%@ Page Title="Importazione Valutazione Progetti" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmImportValutazioneProgetti.aspx.vb" Inherits="Futuro.WfrmImportValutazioneProgetti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        function NascondiElaboraNazionale() {
            document.all.MainContent_lblMessaggioNazionale.style.visibility = 'visible';
            document.all.MainContent_lblMessaggioNazionale.style.fontSize = '1.6em'
            document.all.MainContent_lblMessaggioNazionale.style.fontWeight = 'bold'
            document.all.MainContent_lblMessaggioNazionale.style.color = '#3a4f63'
            document.all.MainContent_lblMessaggioNazionale.innerText = 'ATTENDERE........';
            document.all.MainContent_CmdElaboraProgettiNazionale.style.visibility = 'hidden';
        }

        function NascondiElaboraEstero() {
            document.all.MainContent_lblMessaggioEstero.style.visibility = 'visible';
            document.all.MainContent_lblMessaggioEstero.style.fontSize = '1.6em'
            document.all.MainContent_lblMessaggioEstero.style.fontWeight = 'bold'
            document.all.MainContent_lblMessaggioEstero.style.color = '#3a4f63'
            document.all.MainContent_lblMessaggioEstero.innerText = 'ATTENDERE........';
            document.all.MainContent_CmdElaboraProgettiEstero.style.visibility = 'hidden';
        }

        function NascondiElaboraRegionale() {
            document.all.MainContent_lblMessaggioRegionale.style.visibility = 'visible';
            document.all.MainContent_lblMessaggioRegionale.style.fontSize = '1.6em'
            document.all.MainContent_lblMessaggioRegionale.style.fontWeight = 'bold'
            document.all.MainContent_lblMessaggioRegionale.style.color = '#3a4f63'
            document.all.MainContent_lblMessaggioRegionale.innerText = 'ATTENDERE........';
            document.all.MainContent_CmdElaboraProgettiRegionale.style.visibility = 'hidden';
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Importazione Valutazione Progetti</legend>
        <div class="wrapper" style="width:100%">
            <div class="headers" >
                <h2><asp:Label ID="lblTitolo" runat="server"  Text="Importazione Valutazione Progetti"></asp:Label></h2>
            </div>
            <div class="RigaVuota"><asp:Label ID="lblMessaggioErrore" CssClass="msgErrore"
                            runat="server" Text=""></asp:Label></div>
            <asp:PlaceHolder ID="phNazionaleEstero" runat="server">
            <h3 style="font-weight:bold">Caricamento File Valutazione Progetti NAZIONALE</h3>
            <div class="RigaVuota">&nbsp;</div>
            <p>
                Selezionare il file CSV&nbsp;&nbsp;&nbsp;
                <asp:FileUpload ID="txtSelFileProgettiNazionale" runat="server" ToolTip="Caricamento File Valutazione Progetti NAZIONALE" style="width:40%" />&nbsp;
                <asp:Button ID="CmdElaboraProgettiNazionale" OnClientClick="NascondiElaboraNazionale()" CssClass="Pulsante" runat="server" Text="Elabora" />
                <asp:Label ID="lblMessaggioNazionale" CssClass="bold" runat="server" Text="" AssociatedControlID="txtSelFileProgettiNazionale"></asp:Label>
            </p>
            <div class="RigaVuota">&nbsp;</div> 
            <div class="RigaVuota">&nbsp;</div>
            <div class="RigaVuota">&nbsp;</div>
            <h3 style="font-weight:bold">Caricamento File Valutazione Progetti ESTERO</h3>
            <div class="RigaVuota">&nbsp;</div>
            <p>
                Selezionare il file CSV&nbsp;&nbsp;&nbsp;
                <asp:FileUpload ID="txtSelFileProgettiEstero" runat="server" ToolTip="Caricamento File Valutazione Progetti ESTERO" style="width:40%" />&nbsp;
                <asp:Button ID="CmdElaboraProgettiEstero" OnClientClick="NascondiElaboraEstero()" CssClass="Pulsante" runat="server" Text="Elabora" />
                <asp:Label ID="lblMessaggioEstero" CssClass="bold" runat="server" Text="" AssociatedControlID="txtSelFileProgettiEstero"></asp:Label>
            </p>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phRegionale" runat="server">
            <div class="RigaVuota">&nbsp;</div>
            <div class="RigaVuota">&nbsp;</div>
            <div class="RigaVuota">&nbsp;</div>
            <h3 style="font-weight:bold">Caricamento File Valutazione Progetti REGIONALE</h3>
            <div class="RigaVuota">&nbsp;</div>
            <p>
                Selezionare il file CSV&nbsp;&nbsp;&nbsp;
                <asp:FileUpload ID="txtSelFileProgettiRegionale" runat="server" ToolTip="Caricamento File Valutazione Progetti Regionale" style="width:40%" />&nbsp;
                <asp:Button ID="CmdElaboraProgettiRegionale" OnClientClick="NascondiElaboraRegionale()" CssClass="Pulsante" runat="server" Text="Elabora" />
                <asp:Label ID="lblMessaggioRegionale" CssClass="bold" runat="server" Text="" AssociatedControlID="txtSelFileProgettiRegionale"></asp:Label>
            </p>
            </asp:PlaceHolder>
            <br />
            <br />
            <strong>Note</strong>
            <span>
            <br />
				Tutti i valori inseriti che contengo il carattere <strong>;</strong> devono essere racchiusi tra due virgolette <strong>" "</strong>.
            </span>
            <br />
            <br />
            Il file deve avere le seguenti specifiche:
            <ol>
                <li>Il formato deve essere CSV delimitato dal carattere ";"</li>
                <li>Non devono essere presenti colonne iniziali vuote;</li>
                <li>Non devono essere presenti righe di intestazione. La prima riga deve corrispondere al primo progetto;</li>
                <li>Non devono esistere righe vuote tra un progetto e l'altro;</li>
                <li>I parametri non valutati non devono contenere valori;</li>
                <li>La colonna corrispondente al paramentro <strong>ACC</strong> deve contenere i valori <strong>POSITIVO/POSITIVO LIMITATO</strong>.</li>
                <li>La colonna corrispondente al paramentro <strong>TOT</strong>, in caso di valori decimali, indicare il punteggio con la virgola <strong>","</strong> e non con il punto <strong>"."</strong> (es. 5,2 anziche' 5.2).</li>
                <%--<asp:PlaceHolder ID="phNote" runat="server">
                <li>La colonna corrispondente al paramentro <strong>REG</strong>, in caso di valori decimali, indicare il punteggio con la virgola <strong>","</strong> e non con il punto <strong>"."</strong> (es. 5,2 anziche' 5.2).</li>
                <li>La colonna corrispondente al paramentro <strong>TOT</strong>, in caso di valori decimali, indicare il punteggio con la virgola <strong>","</strong> e non con il punto <strong>"."</strong> (es. 5,2 anziche' 5.2).</li>
                </asp:PlaceHolder>--%>
            </ol>
            <div class="RigaVuota">&nbsp;</div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
            </div>
        </div>
    </fieldset>
</asp:Content>
