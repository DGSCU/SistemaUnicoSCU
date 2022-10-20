<%@ Page Title="Importazione File Graduatorie Volontari" Language="vb" AutoEventWireup="false"
    MasterPageFile="~/Site.Master" CodeBehind="wfrmImportAssenze.aspx.vb" Inherits="Futuro.wfrmImportAssenze" %>

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
        <legend>Importazione Assenze Volontari</legend>
        <div id="TabellaImportAssenzeVolontari" class="wrapper" style="width: 100%">
            <div class="headers">
                <h2>
                    <asp:Label ID="Label5" runat="server" Text="Import Assenze Volontari"></asp:Label>
                </h2>
            </div>
            <div class="RigaVuota">
                &nbsp;</div>
           
         
            <div class="row">
                <asp:Label ID="Label2" runat="server" Width="330px" Text="1) Scaricare il file assenzevolontari.csv"></asp:Label>
                <asp:HyperLink ID="HplFileAssenze" NavigateUrl="~/download/Master/assenzevolontari.csv"
                    runat="server">File assenzevolontari.csv</asp:HyperLink>
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
            <p>
                &nbsp;</p>
            <fieldset>
                <strong>Note:</strong>
                <h3>
                    Tutti i valori inseriti che contengo il carattere ; devono essere racchiusi tra
                    due virgolette " "
                </h3>
                <h3>
                    Il campo Causale deve contenere:
                </h3>
                <ul>
                    <li><strong>1</strong> per <strong>Malattie Oltre 15 Giorni</strong></li>
                    <li><strong>2</strong> per <strong>Infortunio in Servizio</strong></li>
                    <li><strong>3</strong> per <strong>Permessi Oltre 20 Giorni</strong></li>
                    <li><strong>4</strong> per <strong>Maternità</strong></li>
                    <li><strong>5</strong> per <strong>Malattia COVID</strong></li>
                   <%-- <li><strong>5</strong> per <strong>Sospensione per mancanza green pass</strong></li>--%>
                  <%--<p><strong>NOTA:</strong> per inserire la Causale <strong>"Sospensione per mancanza green pass" si deve procedere manualmente sul Sistema andando nel Menù Gestione Assenze.</strong></p> --%>
                </ul>
                <h3>
                    Il campo Mese deve contenere:
                </h3>
                <ul>
                    <li><strong>1</strong> per <strong>Gennaio</strong></li>
                    <li><strong>2</strong> per <strong>Febbraio</strong></li>
                    <li><strong>3</strong> per <strong>Marzo</strong></li>
                    <li><strong>4</strong> <strong>ecc...</strong></li>
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
