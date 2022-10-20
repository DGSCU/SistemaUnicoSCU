<%@ Page Title="Gestione Risorse Progetto" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WebGestioneSediProgettoOlp.aspx.vb" Inherits="Futuro.WebGestioneSediProgettoOlp" %>
  <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
      <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css"/>
        <script src="Scripts/JHelper.js" type="text/javascript"></script>

<script type="text/javascript">

    var idErrore = CostruisciId("lblmessaggio");
    function VerificaNumeroIntero(idCampo, descrizioneCampo) {
        VerificaNumeroIntero(CostruisciId(idCampo), descrizioneCampo, idErrore);
    };

 </script>
 <script type="text/javascript">

     var width = screen.width / 1.4;
     var height = screen.height / 1.4;
     var x = screen.width / 2 - width;
     var y = screen.height / 2 - height;

     function simulateClick() {
         var ricerca = (document.getElementById('cmdRicerca'));
         if (document.all) {
             ricerca.click();
         }
         else {
             var evObj = document.createEvent('MouseEvents');
             evObj.initMouseEvent('click', true, true, window, 1, 12, 345, 7, 220, false, false, true, false, 0, null);
             ricerca.dispatchEvent(evObj);
         }
     };

</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <fieldset class="ContornoPagina">
<legend >Gestione Risorse</legend>
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:Label ID="lblIntProgetto" runat="server"  Text="Ricerca Risorse Per Il progetto: '"></asp:Label>
      <asp:label  id="lblProgetto" style="vertical-align:middle"  Text="&nbsp;" runat="server"></asp:label>
      <asp:label  id="lblApiceFineProgetto" style="vertical-align:middle" Text="'" runat="server"></asp:label>
      </h2>
    </div>
  
    <div class="rowGroup" style="height:auto">
        <div class="RigaVuota">
      &nbsp;</div>
         <asp:label id="lblmessaggio"  runat="server" CssClass="msgErrore" ></asp:label>
         <asp:HiddenField ID="txtModifica" runat="server" />
         <asp:HiddenField ID="txtidbando" runat="server" />
        <div class="RigaVuota">
      &nbsp;</div>
    </div>
    
    <div class="row" >
    
        <div class="colOggetti" style="width:15%">
            <asp:Label ID="lblSedeFisica" CssClass="label" AssociatedControlID="txtSedefisica"  runat="server" Text="Codice Ente"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%" >
            <asp:TextBox ID="txtSedefisica" autofocus="true" CssClass="textbox" runat="server"></asp:TextBox>
        </div>
          <div class="collable" style="width:15%">
           <asp:Label ID="lblCodice" CssClass="label" AssociatedControlID="txtCodice" runat="server" Text="Codice Sede"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:TextBox  ID="txtCodice" CssClass="textbox"  onchange="javascript: VerificaNumeroIntero('txtCodiceSede','Codice Sede');" runat="server"></asp:TextBox>       
       </div>
    </div>

    <div class="row" >  
        <div class="colOggetti" style="width:15%">
            <asp:Label ID="lblSedeAttuazione" CssClass="label" AssociatedControlID="txtSedeAttuaz"  runat="server" Text="Sede Attuazione"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%" >
            <asp:TextBox ID="txtSedeAttuaz" CssClass="textbox" style="color" runat="server"></asp:TextBox>
        </div>
    </div>

    <div class="RigaPulsanti"> 
        <asp:Button id="cmdRicerca" CssClass="Pulsante"  runat="server" Text="Ricerca" ></asp:Button>
        <asp:Button id="imgControllaProvincie" CssClass="Pulsante"  runat="server" Visible="false" Text="Risorse per Provincia" ></asp:Button>
        <asp:Button id="imgCheckOLP" CssClass="Pulsante"  runat="server" Visible="false" Text="OLP" ></asp:Button>
         <asp:Button id="cmdChiudi" CssClass="Pulsante"  runat="server" Text="Chiudi" ></asp:Button>
        &nbsp;
    </div>

</div>
<asp:DataGrid id="dgRisultatoRicerca" Caption="Risultato Ricerca Risorse Progetto"   runat="server" Width="100%" ToolTip="Risorse Progetto" 
                   AllowPaging="true" PageSize="10"  CssClass="table" AllowSorting="True" 
                   AutoGenerateColumns="False" UseAccessibleHeader="True" Visible="true">
	<FooterStyle></FooterStyle>
	<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	<EditItemStyle></EditItemStyle>
	<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	<ItemStyle CssClass="tr" HorizontalAlign ="Center"></ItemStyle>
	<HeaderStyle></HeaderStyle>
    <Columns>
        <asp:TemplateColumn  Visible="false"> 
            <ItemTemplate >
				<asp:Image ID="ImageButton1"  AlternateText="Sede" ToolTip="Sede Progetto" runat="server" ImageUrl="images/sedi_small.png" />  
                </ItemTemplate>
        <ItemStyle Width="0%" />
        </asp:TemplateColumn>
		<asp:BoundColumn DataField="sedeFisica" HeaderText="Sede">
        <ItemStyle Width="25%" />
		</asp:BoundColumn>
		<asp:BoundColumn DataField="Indirizzo" HeaderText="Indirizzo">
        <ItemStyle Width="15%" />
		</asp:BoundColumn>
		<asp:BoundColumn DataField="Comune" HeaderText="Comune ">
        <ItemStyle Width="15%" />
		</asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="telefono" HeaderText="telefono">
         <ItemStyle Width="0%" />
		</asp:BoundColumn>
		<asp:BoundColumn Visible="false" DataField="sedeattuazione" HeaderText="Sede Attuazione">
        <ItemStyle Width="0%" />
		</asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="nProgAtt" HeaderText="N&#176; Progetti Attivi">
       <ItemStyle Width="3%" />
		</asp:BoundColumn>
		<asp:BoundColumn Visible="false" DataField="idEntesede" HeaderText="idSede">
        <ItemStyle Width="0%" />
        </asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="idEntesedeattuazione" HeaderText="idSedeAtt"></asp:BoundColumn>
		<asp:TemplateColumn Visible="False" HeaderText="vol">
			<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			<HeaderTemplate>
				<B>Nr Volontari</B>
			</HeaderTemplate>
			<ItemTemplate>
				<asp:TextBox id="Textbox1" runat="server" Width="30" BackColor="Gainsboro" ReadOnly="True" tooltip="N&#176; Volonatri"
					AutoPostBack="False"></asp:TextBox>
			</ItemTemplate>
            <ItemStyle Width="3%" />
		</asp:TemplateColumn>
         <asp:TemplateColumn  Visible="False" HeaderText="Aggiungi"> 
            <ItemTemplate >
			    <asp:ImageButton  Visible="False" ID="ImageButton2"  style="cursor:pointer;" CommandName="Rimuovi" AlternateText="Aggiungi Sede" ToolTip="Aggiungi Sede" runat="server" ImageURL="images/canc_small.png" CausesValidation="false"></asp:ImageButton>      
            </ItemTemplate>
            <ItemStyle Width="3%" />
        </asp:TemplateColumn>
		<asp:BoundColumn DataField="NumeroPosti" HeaderText="N&#176; Volontari">
        <ItemStyle Width="3%" />
		</asp:BoundColumn>
        <asp:BoundColumn Visible="false"  HeaderText="N&#186; Volontari">
        <ItemStyle Width="3%" />
		</asp:BoundColumn>
		<asp:BoundColumn DataField="nolpins" HeaderText="N&#186; OLP Inseriti">
        <ItemStyle Width="5%" />
		</asp:BoundColumn>
		<asp:BoundColumn DataField="NOlpRic"  HeaderText="N&#186; OLP Richiesti">
         <ItemStyle Width="3%" />
		</asp:BoundColumn>
         <asp:TemplateColumn HeaderText="Visualizza Olp"> 
            <ItemTemplate >
			    <asp:ImageButton  ID="AggiungiModificaOlp"  style="cursor:pointer;" CommandName="AggiungiModificaOlp" AlternateText="Aggiungi Olp" ToolTip='Aggiungi Olp' runat="server" ImageURL="images/valida_small.png" CausesValidation="false"  Visible='<%# IIF(Eval("VISIBILE").ToString().Equals("NO"), False, True) %>'></asp:ImageButton>      
            </ItemTemplate>
             <ItemStyle Width="3%" />
        </asp:TemplateColumn>
         <asp:TemplateColumn Visible="false" HeaderText="Rimuovi"> 
            <ItemTemplate >
			    <asp:ImageButton ID="ImageButton3"  style="cursor:pointer;" CommandName="Rimuovi" AlternateText="Rimuovi Risorsa" ToolTip='Rimuovi Risorsa' runat="server" ImageURL="images/canc_small.png" CausesValidation="false"></asp:ImageButton>      
            </ItemTemplate>
        </asp:TemplateColumn>
		<asp:BoundColumn Visible="False" HeaderText="NVol"></asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="idatt" HeaderText="idAttivitaES"></asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="attiva" HeaderText="attiva"></asp:BoundColumn>
		<asp:BoundColumn DataField="nrla" HeaderText="NRLEA">
        <ItemStyle Width="3%" />
		</asp:BoundColumn>
          <asp:TemplateColumn HeaderText="Visualizza Rlea"> 
            <ItemTemplate   >
			    <asp:ImageButton DataField="aggrla" ID="AggiungiModificaRlea"  style="cursor:pointer;" CommandName="AggiungiModificaRlea" AlternateText="Aggiungi Rlea" ToolTip='Aggiungi Rlea' runat="server" ImageURL="images/valida_small.png" CausesValidation="false" Visible='<%# IIF(Eval("VISIBILE").ToString().Equals("NO"), False, True) %>'></asp:ImageButton>      
            </ItemTemplate>
             <ItemStyle Width="3%" />
        </asp:TemplateColumn>
		<asp:BoundColumn DataField="NTutor" HeaderText="NTutor">
		</asp:BoundColumn>
         <asp:TemplateColumn HeaderText="Visualizza Tutor"> 
            <ItemTemplate   >
			    <asp:ImageButton DataField="Aggtutor" ID="AggiungiModificaTutor"  style="cursor:pointer;" CommandName="AggiungiModificaTutor" AlternateText="Aggiungi Tutor" ToolTip='Aggiungi Tutor' runat="server" ImageURL="images/valida_small.png" CausesValidation="false"></asp:ImageButton>      
            </ItemTemplate>
            <ItemStyle Width="3%" />
        </asp:TemplateColumn>
        <asp:BoundColumn Visible="False" DataField="idattivitàEnteSedeAttuazione" HeaderText="idttivitàEnteSedeAttuazione"></asp:BoundColumn>
        <asp:BoundColumn Visible="false" DataField="identecapofila" HeaderText="identecapofila"></asp:BoundColumn>
        	<asp:BoundColumn Visible="false"  DataField="statoentesede" HeaderText="Stato"></asp:BoundColumn>
	</Columns>
  
		<PagerStyle NextPageText="&gt;&gt;"  
PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
</asp:DataGrid>
</fieldset>
</asp:Content>
