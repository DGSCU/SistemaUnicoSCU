<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmImportSintesiProgetti.aspx.vb" Inherits="Futuro.WfrmImportSintesiProgetti" %>
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
        <legend>URL Sintesi Progetti</legend>
        <div id="TabellaImportSintesiProgetti" class="wrapper" style="width: 100%">
            <div class="headers">
                <h2>
                    <asp:Label ID="Label5" runat="server" Text="Importazione URL Sintesi Progetti"></asp:Label>
                </h2>
            </div>
            <div class="RigaVuota">
                &nbsp;</div>
           
         
            <div class="row">
                <asp:Label ID="Label2" runat="server" Width="330px" Text="1) Scaricare il file Sintesi Progetti   .csv"></asp:Label>
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
          <%--      <h3>
		            Il campo StatoRiattivazione deve contenere:
                </h3>
                    <%Call StatiRiattivazione()%>
                <br />
                
                 <h3>
		            Il campo ModalitàServizio deve contenere:
                </h3>
                    <%Call TipoModalitàServizio()%>
                <br />
                
                <h3>
                    Campo NumeroVolontariConConsenso:
                </h3>
                <ul>
                    <li><strong>da inserire solo per sedi di progetto che riprendono il servizio dopo l'interruzione temporanea. E' il numero di volontari che hanno dato il loro consenso e che quindi tornano in servizio attivo. </strong></li>
                </ul>--%>
                <h3>
                    Campo URL:
                </h3>
                <ul>
                    <strong>
                    URL sta per Uniform Resource Locator. Chiamato colloquialmente indirizzo web, è un riferimento a una risorsa web come un sito, una pagina o un file specifico.<br />
                    Le tre parti più importanti di un URL<br />
                    <li>il Protocollo</li>
                    <li>il dominio</li>
                    <li>il path</li><br /><br />
                        I protocolli accettati sono:<br />
                    https://<br />
                    http://<br />
                    andando quindi a completare la Url con il dominio (per esempio www.miosito.it oppure miosito.it) seguito, volendo, dalla pagina specifica e o dal file specifico (path) si ottiene una Url accettata dal sistema. <br /><br />Esempi di Url:<br />
                     <asp:Label ID="Label1" runat="server"  Text="https://miosito.it/home.aspx/miofile.pdf <br /> http://miosito.it/home.aspx"></asp:Label>
                    <%--esempio:  ....https://miosito.it/home.aspx/miofile.pdf.... oppure ....ftp://miosito.it/home.aspx/miofile.pdf.....--%>
                   </strong>
                </ul>
              <%-- <h3>
                    Campo NuovaSedeSvolgimentoNonAccreditata:
                </h3>
                <ul>
                    <li><strong>da indicare solo per sedi di progetto con stato riattivazione ORDINARIA o RIMODULATA e nel caso il<br />
                    servizio venga svolto presso altra sede non accreditata. Deve essere specificato:<br />
                    - Codice (se ente accreditato), Nome dell'ente e Tipologia (pubblico/privato)<br />
                    - Indirizzo completo della sede<br />
                    Nel caso sia necessario indicare più sedi devono essere separate da carattere # (cancelletto)<br />
                    esempio:<br />
                    ASSOCIAZIONE DEL SOLE (Privato) - Via Roma 191, Salerno#SU12345 COMUNE DI NAPOLI (Pubblico) - Via Milano 321, Napoli</strong></li>
                </ul>--%>
               <%-- <h3>
		            Il campo CodiceAttivitàRimodulazione deve contenere:
                </h3>
                    <%Call CodiceAttivitàRimodulazione()%>
                <br />
                 <h3>
                    Campo SpecificaAltro:
                </h3>
                <ul>
                    <li><strong>Si deve indicare solo se CodiceAttivitàRimodulazione corrisponde ad ALTRO. Descrivere brevemente le attività che non sono riconducibili a quelle dell’elenco di cui all’All. 1 alla circolare 4 aprile 2020</strong></li>
                </ul>
                 <h3>
		            Il campo DataRipresaServizio:
                </h3>
                <ul>
                    <li><strong>da indicare solo per sedi di progetto che riprendono il servizio dopo l'interruzione temporanea. E' possibile indicare solo una tra le seguenti date. (NOTA. Indicando la data del 16/04/2020 come data di ripresa del servizio si dichiara che la sede di progetto non era interrotta).</strong></li>
                </ul>
                    <%Call DataRipresaServizio()%>
                <br />--%>

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
