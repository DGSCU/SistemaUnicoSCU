<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRisultatoImportCorsoOlp.aspx.vb" Inherits="Futuro.WfrmRisultatoImportCorsoOlp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">

  
<legend>Risultati Importazione Corso OLP</legend>
<div class="wrapper" style="width:100%">
 <div class="headers" >
    <h2  style="text-align:center">
   
       <asp:Label ID="Label5" runat="server"  Text="Importazione Corso OLP"></asp:Label>
  
   </h2>
    </div>

<div id="codiciAssegnati" class="rowGroup" style="height:auto" runat="server" visible="false">
<fieldset class="ContornoPagina">
<legend>Codici Assegnati</legend>
<div class="wrapper" style="width:100%;border:0px">
  <div class="row" style="height:auto">
  <h3><asp:Label runat="server" ID="labelCodiceEnte" CssClass="labelDati" Text="Operazione di inserimento dei dati effettuata con successo."></asp:Label></h3>
</div>
  <div class="row" style="height:auto;text-align:center">
<h1><asp:label id="lblCodRif"  CssClass="bold" runat="server" Visible="False"></asp:label></h1>
  </div>
    <div class="row" style="height:auto">
  <h3><asp:Label runat="server" ID="label1" CssClass="labelDati" Text="Si ricorda che per completare la procedura l'ente deve inviare il previsto FORMULARIO tramite PEC all'indirizzo giovanieserviziocivile@pec.governo.it
								"></asp:Label></h3>
</div>
    <div class="row" style="height:auto;text-align:center">
<h1>&nbsp;</h1>
  </div>
  </div>
  </fieldset>
</div>
<p style="text-align:center">

 
 
</p>
   
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
   
  
    <p class="RigaPulsanti">
    
        <asp:Button
          ID="CmdConferma" runat="server" CssClass="Pulsante" Text="Conferma" />&nbsp;&nbsp;<asp:Button 
            ID="CmdChiudi" CssClass="Pulsante" runat="server"
              Text="Chiudi"  />
       
   </p>
    </div>
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
