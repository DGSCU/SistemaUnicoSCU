<%@ Page Title="Dettagli volontario" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="dettaglivolontario.aspx.vb" Inherits="Futuro.dettaglivolontario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
 <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .ui-datepicker {
                font-size: 11px;
            }
        </style>
          
       <script  type="text/javascript">

           var formId;
        var prefissoIdClient = "MainContent_";

	    function ImpostaFormId() {
	        formId = document.forms[0].id;
	        return formId;
	    }
        function CostruisciId(IdServer) {
           var IdClient = prefissoIdClient + IdServer
           return IdClient
        }

        $(function () {
            var IdDataChiusura = CostruisciId('txtDataChiusura');
            var sharpDataChiusura = "#" + IdDataChiusura
            var operazione = '<%=Request.QueryString("Op")%>'
  
            if (operazione != "rinuncia") {
                $("" + sharpDataChiusura + "").datepicker();
            }
        });

	</script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
<legend >Volontario</legend>
<div class="wrapper" style="width:100%">
                <input id="txtDataInizioServizio" type="hidden" name="txtDataInizioServizio" runat="server"/>
				<input id="chkConferma" type="hidden" name="chkConferma" runat="server"/> 
                <input id="CheckFormazione" type="hidden" name="CheckFormazione" runat="server" /> 
                <input id="CheckRinuncia" type="hidden" name="CheckRinuncia" runat="server"/>
         <div class="rowGroup" style="height:auto">
            <div class="RigaVuota"> &nbsp;</div>
		    <asp:label id="lblmessaggiosopra"  runat="server" CssClass="msgErrore"></asp:label>
            <asp:label id="LblConfermaOperazione"  runat="server" CssClass="msgConferma" ></asp:label>
             <div class="RigaVuota">  &nbsp; </div>
		</div>
       
        <div class="row" >
    
       <div class="collable" style="width:15%" >
           <asp:Label ID="lblNominativo" AssociatedControlID="txtNominativo"  runat="server" Text="Nominativo"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="txtNominativo" CssClass="labelDati"  Enabled="false"   runat="server"></asp:Label>
       </div>
       <div class="collable" style="width:15%">
           <asp:Label ID="lblCodiceFiscale" CssClass="label" AssociatedControlID="txtCodiceFiscale" runat="server" Text="Codice Fiscale"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:Label ID="txtCodiceFiscale" CssClass="labelDati" runat="server"></asp:Label>
       </div>
       </div>
       <div class="row" >
       <div class="collable" style="width:15%"  >
           <asp:Label ID="lblSesso" CssClass="label" AssociatedControlID="txtSesso" runat="server" Text="Sesso"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%"  >
              <asp:Label ID="txtSesso" CssClass="labelDati"  runat="server"></asp:Label>
       </div>
    
       <div class="collable" style="width:15%"  >
           <asp:Label ID="lblDataNascita" CssClass="label" AssociatedControlID="txtDataNascita"  runat="server" Text="Data di Nascita"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="txtDataNascita" CssClass="labelDati" runat="server"></asp:Label>
       </div>
       </div>
       <div class="row" >
       <div class="collable" style="width:15%" >
           <asp:Label ID="lblComuneNascita" CssClass="label" AssociatedControlID="txtComuneNascita" runat="server" Text="Comune di Nascita"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:Label ID="txtComuneNascita"  CssClass="labelDati" runat="server"></asp:Label>
       </div>
      
       
       <div class="collable" style="width:15%" >
           <asp:Label ID="lblComuneResidenza" CssClass="label" AssociatedControlID="txtComuneResidenza" runat="server" Text="Comune di Residenza"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%" >
              <asp:Label ID="txtComuneResidenza" CssClass="labelDati" Enabled="false" runat="server"></asp:Label>
       </div>
    </div>
    <div class="row" >
       <div class="collable" style="width:15%" >
           <asp:Label ID="LblDataIniServ" CssClass="label" AssociatedControlID="txtDataIniServ" runat="server" Text="Data Inizio Servizio"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:Label ID="txtDataIniServ"  CssClass="labelDati" runat="server"></asp:Label>
       </div>
           <div class="collable" style="width:15%" >
           <asp:Label ID="LblDataFineServ" CssClass="label" AssociatedControlID="txtDataFineServ" runat="server" Text="Data Fine Servizio"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:Label ID="txtDataFineServ"  CssClass="labelDati" runat="server"></asp:Label>
       </div>
    </div>
 <h3>
<asp:label id="lblRisultato"  runat="server" AssociatedControlID="dtgRisultatoRicerca" CssClass="msgInfo"  Text="Sedi Assegnazione"></asp:label>
</h3>

    <asp:datagrid id="dtgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco Volontari" 
		    AllowPaging="false" CellPadding="2"  CssClass="table" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
	    <HeaderStyle></HeaderStyle>
        <Columns>
					    <asp:BoundColumn DataField="SedeAttuazione" HeaderText="Sede">
						    <HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						    <FooterStyle Wrap="False"></FooterStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn DataField="Progetto" HeaderText="Progetto">
						    <HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						    <FooterStyle Wrap="False"></FooterStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn DataField="DataInizio" HeaderText="Data Inizio">
						    <HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						    <FooterStyle Wrap="False"></FooterStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn DataField="DataFine" HeaderText="Data Fine">
						    <HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						    <FooterStyle Wrap="False"></FooterStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn DataField="DataFineAttività" HeaderText="Data Fine Progetto" Visible="false"> 
						    <HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						    <FooterStyle Wrap="False"></FooterStyle>
					    </asp:BoundColumn>
				    </Columns>
								
    </asp:datagrid>

    <div class="RigaVuota">&nbsp;
    </div>
    <div class="RigaVuota">&nbsp;
                            </div>

    <div class="row" >
    
       <div class="collable" style="width:15%" >
           <asp:Label ID="IdlblCausale" CssClass="label" AssociatedControlID="ddlCausale" runat="server" Text="Causale"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:dropdownlist id="ddlCausale" CssClass="ddlClass" runat="server"></asp:dropdownlist>
       </div>
       <div class="collable" style="width:15%">
           <asp:Label ID="IdlblDataChiusura" CssClass="label" AssociatedControlID="txtDataChiusura"  runat="server" Text="Data Chiusura (gg/mm/aaaa)"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:TextBox ID="txtDataChiusura" CssClass="textbox" Width="25%"  MaxLength="10" runat="server"></asp:TextBox>
       </div>
        </div>
    <div class="row" >
       <div class="collable" style="width:15%">      
           <asp:Label ID="IdLblNote" CssClass="label" AssociatedControlID="txtNote" runat="server" Text="Note"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">       
           <asp:TextBox ID="txtNote" CssClass="textbox" runat="server" MaxLength="500" Rows="4" TextMode="MultiLine"></asp:TextBox>         
       </div>

    </div>
       <div class="RigaVuota">&nbsp;
                            </div>
    <div class="row" >
       <div class="collable" style="width:15%">      
           <asp:Label ID="lblOreFormazione" CssClass="label" AssociatedControlID="txtoreformazione"  runat="server" Text="Cognome"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">       
           <asp:TextBox ID="txtoreformazione" CssClass="textbox" Width="55px"  runat="server"></asp:TextBox>
           <asp:image style="Z-INDEX: 0" id="imgalert" runat="server" ImageUrl="images\alert3.gif" Height="22px"
												Width="34px" Visible="False" AlternateText="Alert ore formazione" ToolTip="Alert ore formazione"></asp:image>         
       </div>
       <div class="collable" style="width:15%">
            <asp:Label ID="lblAssenze" AssociatedControlID="txtAssenze" CssClass="label" runat="server" Text="Nome"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:TextBox ID="txtAssenze" CssClass="textbox" Width="55px"  runat="server"></asp:TextBox>  
              <asp:image style="Z-INDEX: 0" id="Imgalert1" runat="server" ImageUrl="images\alert3.gif" Height="22px"
												Width="34px" Visible="False" AlternateText="Alert ore assenza" ToolTip="Alert ore assenza"></asp:image>
       </div>
    </div>
    <div class="RigaPulsanti">
        <asp:Button ID="cmdConferma" runat="server" CssClass="Pulsante" Text="Conferma"  />&nbsp;
          <asp:Button ID="ImgSostitu" runat="server" CssClass="Pulsante"  Visible="False" ToolTip="Effettua la Sostituzione" Text="Sostituisci"  />
          <asp:Button 
            ID="cmdChiudi" CssClass="Pulsante" runat="server"
              Text="Chiudi"  />&nbsp;

   </div>
</div>
</fieldset>
</asp:Content>
