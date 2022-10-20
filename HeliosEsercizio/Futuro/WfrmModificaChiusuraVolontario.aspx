<%@ Page Title="Modifica tipologia chiusura del volontario " Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmModificaChiusuraVolontario.aspx.vb" Inherits="Futuro.WfrmModificaChiusuraVolontario" %>
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
         /* <![CDATA[ */
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
            var operazione = '<%= Request.Params("Op")%>';
            if(operazione!="esclusione"){
                var IdDataChiusura = CostruisciId('txtDataChiusura');
                var sharpDataChiusura  = "#" +IdDataChiusura;
                $(""+sharpDataChiusura+"").datepicker();
          }
      });
  /* ]]> */
		</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina" >
    <legend>Modifica tipologia chiusura del volontario </legend>
    <div class="wrapper"  style="width:100%;border:0px">
     <div class= "rowGroup" style="height:auto"  >
    
            <asp:label id="lblmessaggiosopra"  runat="server" CssClass="msgErrore" ></asp:label>
             <asp:label id="LblConfermaOperazione"  runat="server" CssClass="msgConferma" ></asp:label>
             <input id="txtDataInizioServizio" type="hidden" name="txtDataInizioServizio" runat="server"/>
			    <input id="chkConferma" type="hidden" name="chkConferma" runat="server"/> 
                <input id="CheckFormazione" type="hidden" name="CheckFormazione" runat="server" /> 
                <input id="CheckRinuncia" type="hidden" name="CheckRinuncia" runat="server"/>
                    <div class="RigaVuota" style="height:auto">&nbsp;</div>
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


    <asp:datagrid id="dtgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco Sedi" 
		    AllowPaging="false" CellPadding="1" Caption="Elenco Sedi"  CaptionAlign="Top" CssClass="table" Font-Size="Small" AllowSorting="false" AutoGenerateColumns="False" UseAccessibleHeader="True">
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
	    <HeaderStyle></HeaderStyle>
        <Columns>
			<asp:BoundColumn DataField="SedeAttuazione" HeaderText="Sede">
				<ItemStyle Width="25%" />
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Progetto" HeaderText="Progetto">
                    <ItemStyle Width="25%" />
			</asp:BoundColumn>
                        <asp:BoundColumn DataField="DataInizioAttività" HeaderText="Data Inizio Progetto">
				<ItemStyle Width="10%" />
			</asp:BoundColumn>
            	<asp:BoundColumn DataField="DataFineAttività" HeaderText="Data Fine Progetto">
					<ItemStyle Width="10%" />
			</asp:BoundColumn>
			<asp:BoundColumn DataField="DataInizio" HeaderText="Data Inizio Servizio Volontario">
				<ItemStyle Width="10%" />
			</asp:BoundColumn>
			<asp:BoundColumn DataField="DataFine" HeaderText="Data Fine Servizio Volontario">
				<ItemStyle Width="10%" />
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
              <asp:TextBox ID="txtDataChiusura" CssClass="textbox" Width="30%"  MaxLength="10" runat="server"></asp:TextBox>
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

    <div class="RigaPulsanti">
        <asp:Button
          ID="cmdConferma" runat="server" CssClass="Pulsante" Text="Conferma"  />&nbsp;
          <asp:Button 
            ID="imgChiudi" CssClass="Pulsante" runat="server"
              Text="Chiudi"  />

   </div>
    </div>
</fieldset>

</asp:Content>
