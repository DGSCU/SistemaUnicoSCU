<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRisultatoImportDisabili.aspx.vb" Inherits="Futuro.WfrmRisultatoImportDisabili" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" language="javascript">

        function ApriSuggerimento(idcomune, indirizzo) {
            window.open("wfrmSuggerimenti.aspx?idcomune=" + idcomune + "&strIndirizzo=" + indirizzo, "Suggerimenti", "height=500,width=500,dependent=no,scrollbars=yes,status=no,resizable=yes");
        }
    </script>
    <script language="javascript" type="text/javascript">
    //<![CDATA[


        function Nascondi() {
            document.all.MainContent_lblMessaggioErrore.style.visibility = 'visible';
            document.all.MainContent_lblMessaggioErrore.style.fontSize = '1.6em'
            document.all.MainContent_lblMessaggioErrore.style.fontWeight = 'bold'
            document.all.MainContent_lblMessaggioErrore.style.color = '#3a4f63'
            document.all.MainContent_lblMessaggioErrore.innerText = 'ATTENDERE........';
            document.all.MainContent_CmdConferma.style.visibility = 'hidden';
        }
 
    //]]>
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
<legend>Risultati importazione</legend>


    <div class="headers">
   
        <h2>
            <asp:Label ID="lblTipoImport" runat="server"  Text=""></asp:Label>
        </h2>
    </div>
    <p>
        <asp:label id="lblEsito"  CssClass="bold" runat="server" Visible="False">Label</asp:label>
    </p>
    <p>
        <asp:label id="lblTotali" CssClass="bold" runat="server" ></asp:label>
    </p>
      <div id="AvvisoConferma" runat="server" visible="false">
     <ul style="border-color:Red;border-style:ridge;margin:1% 20% 1% 20%;text-align:left;" runat="server" id="avviso">
        <dt style="margin-left:5px;font-size:1.6em;font-weight:bold;color:Red">Attenzione!!!</dt>
        <dt style="margin-left:5px;font-size:1.2em;font-weight:bold" runat="server" id="testoavviso"></dt>
       <%-- <li style="margin-left:5px;font-size:1.4em;font-weight:bold;">Il bando dei volontari funzionerà normalmente. Il concorso “Realizzazione di uno spot televisivo e di un cortometraggio di promozione del Servizio Civile Nazionale” funzionerà regolarmente.</li>--%>

    </ul>
    </div>
    <p>
        <asp:hyperlink id="hlDownLoad" CssClass="linkStampa" runat="server">Scarica il file completo nel formato CSV</asp:hyperlink>   
    </p>
    <p class="RigaPulsanti" style="text-align:right;">


        <asp:Button ID="CmdConferma"  OnClientClick="Nascondi()" CssClass="Pulsante" runat="server" Text="Conferma"/>&nbsp;&nbsp;
        <asp:Label ID="lblMessaggioErrore" CssClass="msgErrore" runat="server" Text=""></asp:Label>
        <asp:Button ID="CmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"/>
    </p>
    <p>
        <asp:datagrid id="dtgCSV" runat="server" width="100%"  CssClass="table" AutoGenerateColumns="True" AllowSorting="True" 	CellPadding="3" PageSize="5">
	        <FooterStyle ></FooterStyle>
	        <SelectedItemStyle BackColor="White"></SelectedItemStyle>
	        <EditItemStyle></EditItemStyle>
	        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	        <ItemStyle CssClass="tr"></ItemStyle>
	        <HeaderStyle CssClass="linkPageGrid"></HeaderStyle>
        </asp:datagrid>
  </p>
  <p>&nbsp;</p>
  </fieldset>
</asp:Content>
