<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmVisualizzaDettaglioCorsoOLP.aspx.vb" Inherits="Futuro.WfrmVisualizzaDettaglioCorsoOLP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
        <legend>DETTAGLIO CORSO OLP</legend>
        <fieldset>
         <div class="RigaVuota" >&nbsp;<asp:label id="lblmessaggio"  runat="server" CssClass="msgErrore" ></asp:label>
            </div>
        <div class="wrapper" style="width:100%">
           <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="CORSO OLP"></asp:Label></h2>
            </div>
           <div class="RigaVuota" >&nbsp;</div>
             
             <div class="row" >
             <div class="collable" style="width:15%"> 
             <asp:Label ID="lblEnte" runat="server" Text="Ente"></asp:Label>
             </div>  
             <div class="colOggetti"  style="width:35%">
             <asp:Label ID="LblEnteDato" Font-Bold="true" runat="server" Text=""></asp:Label>
             </div>
              <div class="collable" style="width:15%"> 
              <asp:Label ID="lblTitoloProg" runat="server" Text="Denominazione Ente"></asp:Label>
             </div>  
             <div class="colOggetti"  style="width:35%">
                 <asp:Label ID="LblTitoloDato" runat="server" Font-Bold="true" Text=""></asp:Label>
             </div>
             </div>
             <div class="row" >
             <div class="collable" style="width:15%"> 
             <asp:Label ID="lblcodrif" runat="server" Text="Codice Riferimento"></asp:Label>
             </div>  
             <div class="colOggetti"  style="width:35%">
             <asp:Label ID="LblCorsoRiferimento" runat="server" Font-Bold="true" Text=""></asp:Label>
             </div>
              <div class="collable" style="width:15%"> 
              <asp:Label ID="Label3" runat="server" Text="Stato Richiesta"></asp:Label>
             </div>  
             <div class="colOggetti"  style="width:35%">
                 <asp:Label ID="lblStatoCorso" runat="server" Font-Bold="true" Text=""></asp:Label>
             </div>
            </div>
            
            <div class="RigaPulsanti" style="text-align:right">
            
                <asp:Button ID="CmdAccetta" runat="server" CssClass="Pulsante" Text="Approva" />&nbsp;
                <asp:Button ID="CmdRespingi" runat="server" CssClass="Pulsante" Visible="true" Text="Respingi" />&nbsp;
                <asp:Button ID="CmdStampa" runat="server" CssClass="Pulsante" Visible="false" Text="ZIP PDF" />&nbsp;
                                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;

             </div>
             </div>
          </fieldset>  
          <p><asp:HyperLink ID="HyperLink1" Visible="false" runat="server">Download File Zip Corso OLP</asp:HyperLink></p> 
        <asp:datagrid id="dgRisultatoRicerca" runat="server" Caption="Dettaglio Corso OLP"  CssClass="table" CellPadding="2" PageSize = "10"  AllowPaging="true" 	
        AllowSorting="True"  AutoGenerateColumns="False" UseAccessibleHeader="True"  Width="100%">
		<FooterStyle></FooterStyle>
		<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
	    <Columns>
        <asp:BoundColumn Visible="False" DataField="IdCorsoFormazioneOLPDettaglio" HeaderText="IdEnte"></asp:BoundColumn>    
        <asp:BoundColumn Visible="False" DataField="IdCorsoFormazioneOLP" HeaderText="IdCorsoFormazioneOLP"></asp:BoundColumn>
         <asp:TemplateColumn HeaderText="OLP"> 
             <ItemTemplate >
                 <asp:ImageButton ID="CmdImgDettCorsoOLP" AlternateText="Seleziona" CommandName="DettCorso" ImageUrl="~/Images/Icona_Volontario_small.png" runat="server" />
             </ItemTemplate>
        </asp:TemplateColumn>
         <asp:BoundColumn DataField="Cognome"   HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Cognome"></asp:BoundColumn>
         <asp:BoundColumn DataField="Nome"   HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Nome"></asp:BoundColumn>
          <asp:BoundColumn DataField="EnteRiferimento"   HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Ente Appartenenza"></asp:BoundColumn>
         <asp:BoundColumn DataField="LuogoSvolgimento"    HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Luogo di Svogimento"></asp:BoundColumn>
        <asp:BoundColumn DataField="DataSvolgimentoCorso"   HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Periodo Svolgimento Corso"></asp:BoundColumn>
        <asp:BoundColumn DataField="NumeroOre" HeaderStyle-CssClass="linkPageGridRidotto"  HeaderText="Numero Ore"></asp:BoundColumn>
       <asp:BoundColumn DataField="StatoRichiesta"    HeaderStyle-CssClass="linkPageGridRidotto" Visible="false" HeaderText="Stato Ric."></asp:BoundColumn>
      <asp:TemplateColumn HeaderText="Pdf"> 
             <ItemTemplate >
                 <asp:ImageButton ID="CmdPdf" AlternateText="Genera Pdf" CommandName="SelezionatoPdf" ImageUrl="~/Images/pdf.png" Width="30px" Height="30px" Visible='<%# IIF(Eval("StatoRichiesta") <> "Approvata" or Eval("NumeroOre") < 8 , False, True)%>'  runat="server" />
             </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
	<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGridRidotto"  Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
    </fieldset>
</asp:Content>
