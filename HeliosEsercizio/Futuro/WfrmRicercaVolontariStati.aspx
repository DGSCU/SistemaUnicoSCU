<%@ Page Title="Ricerca Volontari Stai" Language="vb" MaintainScrollPositionOnPostback="false" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaVolontariStati.aspx.vb" Inherits="Futuro.WfrmRicercaVolontariStati" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
           <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
        <style type="text/css">
            .ui-datepicker {
                font-size: 11px;
            }
        </style>
          
       <script  type="text/javascript">


     

           $(function () {
               var IdData = CostruisciId('txtDataInizServ');
               var sharpIdData = "#" + IdData
               $("" + sharpIdData + "").datepicker();
           });

           $(function () {
               var IdData = CostruisciId('txtalladata');
               var sharpIdData = "#" + IdData
               $("" + sharpIdData + "").datepicker();
           });

           function VerificaStato() {
               var IdDdlStatiSelezionati = CostruisciId("cboStatiAttestatiSel");
               if (document.getElementById(IdDdlStatiSelezionati).value == "0") {
                   var messaggio = "Selezionare il nuovo stato da applicare ai volontari selezionati.";
                   AggiornaErrore(messaggio, CostruisciId("msgErrore"));
                   document.getElementById(CostruisciId("msgErrore" )).focus();
                   return false;
               }
               return true;

           }

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="firefox">
<fieldset class="ContornoPaginaScroll" >
<legend >Ricerca Volontari</legend>
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Volontari"></asp:Label>
      </h2>
      </div>
      <div class="rowGroup" style="height:auto">
        <div class="RigaVuota">
      &nbsp;</div>
               <div class="row" style="height:auto">
                <asp:Label ID="msgErrore" CssClass="msgErrore" runat="server"></asp:Label>
                <asp:Label ID="msgInfo" CssClass="msgInfo" runat="server"></asp:Label>
                <asp:Label ID="msgConferma" CssClass="msgConferma" runat="server"></asp:Label>
        </div>
        <div class="RigaVuota">
      &nbsp;</div>
    </div>

        <div class="row" >
    
           <div class="collable"  style="width:15%">
           <asp:Label ID="lblDescEnte" AssociatedControlID="txtDescEnte" CssClass="label"  runat="server" Text="Denominazione Ente"></asp:Label>
       </div>
       <div class="colOggetti" style="width:85%">
           <asp:TextBox ID="txtDescEnte" CssClass="textbox" Width="96%"  runat="server"></asp:TextBox>
       </div>
    
    </div>
         <div class="row" >
    
       
       <div class="collable" style="width:15%">
           <asp:Label ID="lblCodEnte" AssociatedControlID="txtCodEnte"   runat="server" Text="Cod. Ente"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:TextBox ID="txtCodEnte" CssClass="textbox"  runat="server"></asp:TextBox>
       </div>
          <div class="collable" style="width:15%">
           <asp:Label ID="LblCodVolontario" AssociatedControlID="txtCodVolontario"  runat="server" Text="Cod. Volontario"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:TextBox ID="txtCodVolontario" CssClass="textbox"   runat="server"></asp:TextBox>
       </div>
    
    </div>

    <div class="row" >
       <div class="collable" style="width:15%">      
           <asp:Label ID="IdLblCognome" AssociatedControlID="txtCognome" runat="server" Text="Cognome"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">       
           <asp:TextBox ID="txtCognome" CssClass="textbox" runat="server"></asp:TextBox>         
       </div>
       <div class="collable" style="width:15%">
            <asp:Label ID="IdLblNome" AssociatedControlID="txtNome" runat="server" Text="Nome"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:TextBox ID="txtNome" CssClass="textbox" runat="server"></asp:TextBox>  
       </div>
    </div>

    <div class="row" >
        <div class="collable" style="width:15%">
            <asp:Label ID="IdLblProgetto" AssociatedControlID="txtProgetto" runat="server" Text="Progetto"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
             <asp:TextBox ID="txtProgetto" CssClass="textbox" runat="server"></asp:TextBox>  
       </div>
           <div class="collable" style="width:15%">
            <asp:Label ID="IdLblCodProgetto" AssociatedControlID="txtCodProgetto" runat="server" Text="Cod. Progetto"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">
            <asp:TextBox ID="txtCodProgetto" CssClass="textbox" runat="server"></asp:TextBox>
       </div>
     
    </div>

    <div class="row" >
        <div class="collable" style="width:15%">
            <asp:Label ID="LblRegione" AssociatedControlID="txtRegione" runat="server" Text="Regione"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
             <asp:TextBox ID="txtRegione" CssClass="textbox" runat="server"></asp:TextBox>  
       </div>
           <div class="collable" style="width:15%">
            <asp:Label ID="LblProvincia" AssociatedControlID="txtProvincia" runat="server" Text="Provincia"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">
            <asp:TextBox ID="txtProvincia" CssClass="textbox" runat="server"></asp:TextBox>
       </div>
     
    </div>

    <div class="row" >
        <div class="collable" style="width:15%">
            <asp:Label ID="LblComune" AssociatedControlID="txtComune" runat="server" Text="Comune"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
             <asp:TextBox ID="txtComune" CssClass="textbox" runat="server"></asp:TextBox>  
       </div>
              <div class="collable" style="width:15%">
            <asp:Label ID="LblSesso" AssociatedControlID="cboSesso" runat="server" Text="Sesso"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">
            <asp:DropDownList ID="cboSesso" Width="100px" CssClass="ddlClass" runat="server"></asp:DropDownList>
       </div>
    
     </div>

    <div class="row" >
        <div class="collable" style="width:15%">
            <asp:Label ID="LblStato" AssociatedControlID="cboStato" runat="server" Text="Stato Volontario"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
              <asp:DropDownList ID="cboStato" CssClass="ddlClass" runat="server"></asp:DropDownList> 
       </div>
        <div class="collable" style="width:15%">
            <asp:Label ID="lblcboStatiAttestato" AssociatedControlID="cboStatiAttestato" runat="server" Text="Stato Attestato"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
              <asp:DropDownList ID="cboStatiAttestato" CssClass="ddlClass" runat="server"></asp:DropDownList> 
       </div>        
    </div>
    <div class="row" >
        <div class="collable" style="width:15%">
            <asp:Label ID="LblDataInizServ" AssociatedControlID="txtDataInizServ"  runat="server" Text="Data di Inizio Servizio (gg/mm/aaaa)"></asp:Label>
       </div>
        <div class="colOggetti" style="width:15%">    
             <asp:TextBox ID="txtDataInizServ" CssClass="textboxData" MaxLength="10" runat="server"></asp:TextBox>  
       </div>
        <div class="colOggetti" style="width:20%">&nbsp;   
        </div>
        <div class="collable" style="width:15%">
            <asp:Label ID="LblAlladata" AssociatedControlID="txtalladata"  runat="server" Text="In servizio alla data del (gg/mm/aaaa)"></asp:Label>
       </div>
        <div class="colOggetti" style="width:15%">    
             <asp:TextBox ID="txtalladata" CssClass="textboxData" MaxLength="10" runat="server"></asp:TextBox>  
       </div>
    
     </div>

    <div class="RigaPulsanti">
        <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
        <asp:Button 
            ID="cmdChiudi" CssClass="Pulsante" runat="server"
              Text="Chiudi"  />
   </div>
    </div>

  <fieldset class="ContornoPaginaScroll"  id="fldsetDataGrid" runat="server" visible="false">
<legend >Gestione Ricerca Volontari</legend>
<div class="wrapper" style="width:100%;border:0px">
    <div class="RigaVuota">
    &nbsp;
    </div>
    <div class="row">
        <div class="colOggetti" style="width:50%">
            <asp:checkbox id="checkSelDesel" runat="server" Text="Seleziona tutto"  AutoPostBack="true"></asp:checkbox>
         </div>
         <div class="collable" style="width:15%">
            <asp:Label ID="lblStati" AssociatedControlID="cboStatiAttestatiSel" runat="server" Text="Stati Attestato"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
              <asp:DropDownList ID="cboStatiAttestatiSel" CssClass="ddlClass" runat="server"></asp:DropDownList> 
       </div>
     </div>
           <div class="RigaPulsanti">
        <asp:Button ID="imgConferma" runat="server" CssClass="Pulsante" Text="Aggiorna Stato" OnClientClick="return VerificaStato();" />&nbsp;
        </div>
    </div>
</fieldset>
    <asp:datagrid id="dtgRisultatoRicerca" runat="server" Width="100%"  ToolTip="Elenco Volontari" 
			    AllowPaging="True" CellPadding="2" Font-Size="Small"  CssClass="table" 
                AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True"
                Caption="Risultato Ricerca Volontari"
                >
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
	    <HeaderStyle></HeaderStyle>
	    <Columns>
         <asp:TemplateColumn  HeaderText="Vol." ItemStyle-Width="5%"> 
                        <ItemTemplate >
				        <asp:ImageButton  ID="linkVolontario" style="cursor:pointer;" CommandName="SelezionaVolontario"  AlternateText="Seleziona Nominativo" runat="server" ImageUrl="images/Icona_Volontario_small.png" ></asp:ImageButton>      
                        </ItemTemplate>
                        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText = "Sel./Des."  > 
				<ItemTemplate>
					<asp:CheckBox  id="chkSelVol" ToolTip="Seleziona/Deseleziona Volontario"    runat="server"  Text ="Sel./Des."></asp:CheckBox>
				</ItemTemplate>
                <HeaderStyle Width="5%"></HeaderStyle>
			</asp:TemplateColumn>
		    <asp:BoundColumn DataField="codicevolontario" HeaderText="Codice Volontario">
		    </asp:BoundColumn>
		    <asp:BoundColumn Visible="False" DataField="IdAttivit&#224;" HeaderText="IdAttivit&#224;"></asp:BoundColumn>
		    <asp:BoundColumn Visible="False" DataField="IDEntit&#224;" HeaderText="IDEntit&#224;"></asp:BoundColumn>
		    <asp:BoundColumn DataField="Nominativo" HeaderText="Nominativo"></asp:BoundColumn>
		    <asp:BoundColumn DataField="Ente" HeaderText="Ente">
		    </asp:BoundColumn>
		    <asp:BoundColumn DataField="DataInizio" HeaderText="Data Inizio Servizio">
			    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
		    </asp:BoundColumn>
		    <asp:BoundColumn DataField="DataFine" HeaderText="Data Fine Servizio">
			    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
		    </asp:BoundColumn>
		    <asp:BoundColumn Visible="False" DataField="ComProv" HeaderText="Comune Progetto">
		    </asp:BoundColumn>
		    <asp:BoundColumn DataField="AltreInfo" HeaderText="Altre Informazioni">
		    </asp:BoundColumn>
		    <asp:BoundColumn DataField="Stato" HeaderText="Stato volontario">
		    </asp:BoundColumn>
		    <asp:BoundColumn Visible="False" DataField="IdEnte" HeaderText="IdEnte"></asp:BoundColumn>
		    <asp:BoundColumn Visible="False" DataField="Ente1" HeaderText="Ente"></asp:BoundColumn>
		    <asp:BoundColumn DataField="CLP" HeaderText="Stato attestato">
		    </asp:BoundColumn>
		    <asp:BoundColumn Visible="False" DataField="cf" HeaderText="cf"></asp:BoundColumn>
		    <asp:BoundColumn DataField="DataAttestato" HeaderText="Data Attestato">
		    </asp:BoundColumn>
               <asp:TemplateColumn  HeaderText="Crono." ItemStyle-Width="5%"> 
                        <ItemTemplate >
				        <asp:ImageButton  ID="linkStoricoVolontario" style="cursor:pointer;" CommandName="StoricoVolontario" ToolTip="Storico Volontario"  AlternateText="Apri Storico Volontario" runat="server" ImageUrl="images/cronologia_small.png" ></asp:ImageButton>      
                        </ItemTemplate>
                        </asp:TemplateColumn>
		    <asp:BoundColumn Visible="False" DataField="CodiceRegione" HeaderText="Codice Ente"></asp:BoundColumn>
	    </Columns>
	    <PagerStyle NextPageText="&gt;&gt;"   
		    PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
    </asp:datagrid>

<div class="RigaVuota">
&nbsp;

</div>


</fieldset>
</div>
</asp:Content>

