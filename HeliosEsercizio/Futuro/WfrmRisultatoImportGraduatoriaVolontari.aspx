<%@ Page Title="Risultato Importazione File di Graduatorie Volontari" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRisultatoImportGraduatoriaVolontari.aspx.vb" Inherits="Futuro.WfrmRisultatoImportGraduatoriaVolontari" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">


<script type="text/javascript" language="javascript">

    function ApriSuggerimento(idcomune, indirizzo) {
        window.open("wfrmSuggerimenti.aspx?idcomune=" + idcomune + "&strIndirizzo=" + indirizzo, "Suggerimenti", "height=500,width=500,dependent=no,scrollbars=yes,status=no,resizable=yes");
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
<legend>Risultati Importazione Graduatorie Volontari</legend>


    <div class="headers">
    <h2>
   
       <asp:Label ID="Label5" runat="server"  Text="Import Graduatoria Volontari"></asp:Label>
  
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
   
  
    <p class="RigaPulsanti" style="text-align:right">
    
        <asp:Button
          ID="CmdConferma" runat="server" CssClass="Pulsante" Text="Conferma" />&nbsp;&nbsp;<asp:Button 
            ID="CmdChiudi" CssClass="Pulsante" runat="server"
              Text="Chiudi"  />
       
   </p>
    
     <p >
    <asp:datagrid id="dtgCSV" runat="server" width="100%"  CssClass="table" 
											 AutoGenerateColumns="True" AllowSorting="True" 
											CellPadding="3" PageSize="5">
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
