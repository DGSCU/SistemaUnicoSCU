<%@ Page Title="Ricerca Volontari Per Stampa Attestati" MaintainScrollPositionOnPostback="true"  Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaVolontariStampa.aspx.vb" Inherits="Futuro.WfrmRicercaVolontariStampa" %>
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
 /* <![CDATA[ */
           function OpenContaVolontari(IdVolontario, checked) {
               window.open('contavolontari.aspx?strIdVol=' + IdVolontario + '&strCheckStato=' + checked, 'contavolontario', 'width=250,height=250,toolbar=no,location=no,menubar=no,scrollbars=no');
           };


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

           /* ]]> */
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
              <asp:DropDownList ID="cboStatiAttestato" CssClass="ddlClass" runat="server">
              <asp:ListItem Value="2">Richiedibile</asp:ListItem>
								<asp:ListItem Value="3">In Stampa</asp:ListItem>
              </asp:DropDownList> 
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
  <fieldset class="ContornoPaginaScroll"  id="fldSetGestioneVolontariSelezionati" runat="server" visible="false">
<legend >Gestione Volontari Selezionati</legend>
<div class="wrapper" style="width:100%;border:0px">
    <div class="RigaVuota">
    &nbsp;
    </div>

    <div class="row">

         <div class="collable" style="width:15%">
            <asp:Label ID="lblStati" AssociatedControlID="ddlStampa" runat="server" Text="Stampa"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
              <asp:DropDownList ID="ddlStampa" CssClass="ddlClass" runat="server">
              <asp:ListItem Value="0" Selected="True">Originale</asp:ListItem>
			    <asp:ListItem Value="1">Duplicato</asp:ListItem>
              </asp:DropDownList> 
       </div>
         <div class="colOggetti" style="width:25%">  
       <asp:RadioButton ID="optAttestato" GroupName="attestati" runat="server"  Text="Attestato" Checked="true"/>
       </div>
         <div class="colOggetti" style="width:25%">  
       <asp:RadioButton ID="optLettera" GroupName="attestati" runat="server" Text="Lettera" />
       </div>
     </div>
           <div class="RigaPulsanti">
        <asp:Button ID="imgStampa" runat="server" CssClass="Pulsante" Text="Stampa" ToolTip="Stampa"  />&nbsp;
          <asp:Button ID="imgAnnullaSelezione" runat="server" CssClass="Pulsante" Text="Cancella Selezione" ToolTip="Cancella Selezione"  />&nbsp;
                   <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" />&nbsp;
                <br />
         <asp:HyperLink ID="ApriCSV1"   AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" 
        Text="DOWNLOAD CSV" runat="server" 
        ForeColor="#003399" Visible="False"></asp:HyperLink>
        </div>
    </div>
</fieldset>
  <fieldset class="ContornoPaginaScroll" style="border-color:transparent" >
  <div class="wrapper" style="width:100%;border:0px;height:auto" runat="server" visible="false" id="DivSelezionaTutto">
        <div class="row" style="height:auto">
        <div class="colOggetti" style="width:50%">
            <asp:checkbox id="checkSelDesel" runat="server" Text="Seleziona tutto"  AutoPostBack="true"></asp:checkbox>
         </div>
         </div>
</div>
    <asp:DataGrid id="dtgRisultatoRicerca" runat="server" Width="100%"  ToolTip="Elenco Volontari" 
			    AllowPaging="True" CellPadding="2" Font-Size="Small"  CssClass="table" 
                AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True"
                Caption="Risultato Ricerca Volontari"
                >
	    <FooterStyle></FooterStyle>
	
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
	    <HeaderStyle></HeaderStyle>
	    <Columns>
         <asp:TemplateColumn  HeaderText="Vol." ItemStyle-Width="5%"> 
                        <ItemTemplate >
				        <asp:ImageButton  ID="linkVolontario" style="cursor:pointer;" CommandName="SelezionaVolontario" ToolTip="Link dettoglio volontario"  AlternateText="Seleziona Volontario" runat="server" ImageUrl="images/Icona_Volontario_small.png" ></asp:ImageButton>      
                        </ItemTemplate>
                        </asp:TemplateColumn>
      
	        <asp:BoundColumn  HeaderText="Sel./Des."   DataField ="chkSelVol" ></asp:BoundColumn>
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
               <asp:TemplateColumn  HeaderText="Crono." ItemStyle-Width="5%"> 
                        <ItemTemplate >
				        <asp:ImageButton  ID="linkStoricoVolontario" style="cursor:pointer;" CommandName="StoricoVolontario" ToolTip="Storico Volontario"  AlternateText="Apri Storico Volontario" runat="server" ImageUrl="images/cronologia_small.png" ></asp:ImageButton>      
                        </ItemTemplate>
                        </asp:TemplateColumn>
		    <asp:BoundColumn Visible="False" DataField="CodiceRegione" HeaderText="Codice Ente"></asp:BoundColumn>
            		<asp:BoundColumn Visible="False" DataField="comune" HeaderText="comune"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="provincia" HeaderText="provincia"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="datanascita" HeaderText="datanascita"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="progetto" HeaderText="progetto"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="note" HeaderText="note"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="indirizzo" HeaderText="indirizzo"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="localita" HeaderText="localita"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="Nome" HeaderText="Nome"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="Cognome" HeaderText="Cognome"></asp:BoundColumn>
	    </Columns>
	    <PagerStyle NextPageText="&gt;&gt;"   
		    PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
    </asp:DataGrid>
    </fieldset>
<div class="RigaVuota">
&nbsp;

</div>


</fieldset>
</div>
</asp:Content>

