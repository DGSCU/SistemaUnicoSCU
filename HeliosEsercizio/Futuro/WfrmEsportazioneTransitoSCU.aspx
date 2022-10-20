<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmEsportazioneTransitoSCU.aspx.vb" Inherits="Futuro.WfrmEsportazioneTransitoSCU" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina" id="FidsetPagina">
    <legend> Esportazione dati</legend>  
    <div class="wrapper" style="width:100%">
        <div class="headers" >
            <h2><asp:Label ID="lblTitolo" runat="server"  Text="Esportazione dati per transito SCU"></asp:Label></h2>
        </div>
        <br />
        <div id="esporta" class="rowGroup" style="height:auto" runat="server" visible="true">                               
            <div class="wrapper" style="width:100%;border:0px">
                <div class="row" style="height:auto">
                    <h3>Questa funzione consente di esportare i dati dell'Ente in formato csv.<br>I 
                        tracciati forniti dal sistema sono esattamente quelli previsti dalle procedure 
                        di importazione dati disponibili sul Sistema e pertanto possono essere utilizzati per agevolare le procedure di inserimento dati per l'accreditamento all'albo di Servizio Civile Universale.</h3>
                       <br />
                    <h3>I dati previsti per l'esportazione sono i seguenti:</h3>
                    <ul>
                        <li>ENTI DI ACCOGLIENZA</li>
                        <li>SEDI DI ATTUAZIONE</li>
                        <li>RISORSE (OLP - FORMATORI - SELETTORI - ESPERTI MONITORAGGIO)</li>
                    </ul>
              
                </div>
                <br />
                        
                <div class="RigaPulsanti" style="height:auto;text-align:center">
                    <asp:Button ID="CmdElabora" CssClass="Pulsante" runat="server" 
                        ToolTip="Esportazione Dati" Text="Esportazione Dati" Visible="False" />
                </div>
                    <div class="row">
                    <asp:Label ID="lblMessaggioErrore" CssClass="msgErrore" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="lblMessaggio" CssClass="msgConferma" runat="server" Text="" Visible="false"></asp:Label>
                </div>        <br />
                <fieldset class="ContornoPagina" id="dwFile" runat="server" visible="false">
                    <legend>Download File</legend>
                    
                    <div class="colHyperLink" style="width:100%;" >
                        <asp:hyperlink  id="hlScaricaEnti"  runat="server" Target="_blank"/>
                    </div>
                    <div class="colHyperLink" style="width:100%;" >
                        <asp:hyperlink  id="hlScaricaSedi"  runat="server" Target="_blank"/>
                    </div>
                    <div class="colHyperLink" style="width:100%;" >
                        <asp:hyperlink  id="hlScaricaRisorse"  runat="server"  Target="_blank"/>
                    </div>
                </fieldset>
            </div>
            <div class="RigaVuota">&nbsp;</div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
            </div>
        </div>
    </div>
</fieldset>
 </asp:Content>
