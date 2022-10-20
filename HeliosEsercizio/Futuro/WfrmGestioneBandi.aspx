<%@ Page Title="Gestione Bandi" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmGestioneBandi.aspx.vb" Inherits="Futuro.WfrmGestioneBandi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
      <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css"/>
        <script src="Scripts/JHelper.js" type="text/javascript"></script>
        <style type="text/css">
            .ui-datepicker {
                font-size: 11px;
            }

        </style>
         <script type="text/javascript" >
             function CheckImportoValido(valore, descrizione) {
             if(document.getElementById(CostruisciId(valore)).value != ""){
                 var numeroValido = ValidaNumero(CostruisciId(valore), descrizione, CostruisciId("lblErrore"));
                 
                 if (numeroValido == true) {
                     var auff = parseFloat(document.getElementById(CostruisciId(valore)).value);
                     document.getElementById(CostruisciId(valore)).value = auff.toFixed(2);
                 }
                 else {
                     document.getElementById(CostruisciId(valore)).value = ""
                 }
                 }
            };

            function CheckInteroValido(valore, descrizione) {
                 ValidaNumeroIntero(CostruisciId(valore), descrizione, CostruisciId("lblErrore"));
            };
         </script>

        <script type="text/javascript" >
//           /* <![CDATA[ */
            $(function () {
                var clientId = "#" + CostruisciId('txtInizio');
                var dataAbilitata = '<%= txtInizio.Enabled  %>';
                if(dataAbilitata== 'True'){
                    $("" + clientId + "").datepicker();
                 }
            });
               $(function () {
                var clientId = "#" + CostruisciId('txtfine');
                    var dataAbilitata = '<%= txtfine.Enabled  %>';
                if(dataAbilitata== 'True'){
                    $("" + clientId + "").datepicker();
                    }
            });
                    $(function () {
                var clientId = "#" + CostruisciId('TxtDataInizioVolontari');
                  var dataAbilitata = '<%= TxtDataInizioVolontari.Enabled  %>';
                if(dataAbilitata== 'True'){
                    $("" + clientId + "").datepicker();
                    }
            });
               $(function () {
                var clientId = "#" + CostruisciId('TxtDataFineVolontari');
                      var dataAbilitata = '<%= TxtDataFineVolontari.Enabled  %>';
               if(dataAbilitata== 'True'){
                    $("" + clientId + "").datepicker();
                    }
            });
           $(function () {
                var clientId = "#" + CostruisciId('TxtDataScadGrad');
                       var dataAbilitata = '<%= TxtDataScadGrad.Enabled  %>';
               if(dataAbilitata== 'True'){
                    $("" + clientId + "").datepicker();
                    }
            });
//                 /* ]]> */
        </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
    <legend>Gestione Bando</legend>

    <div class="wrapper" style="width:100%;border:0px">
        <div class="headers" >
            <h2>
            <asp:Label ID="lblTitolo" runat="server"  Text="Gestione Bandi: "></asp:Label>
            <asp:Label ID="lblAzione" runat="server"  Text="&nbsp;"></asp:Label>
            </h2>
        </div>
         <div class="row" style="height:auto">
            <asp:label  id="lblErrore" runat="server" CssClass="msgErrore"></asp:label>
             <asp:label id="lblConferma" runat="server" CssClass="msgConferma"></asp:label>
              <asp:label id="lblInfo" runat="server" CssClass="msgInfo" ></asp:label>
        </div>
        <div class="row" >
         <div class="collable" style="width:80%" id="div1">
         <fieldset class="fieldsetrasparente">
         <div class="wrapper" style="width:100%;border:0px">
<div class="wrapper" style="width:100%;border:0px">
<div class="rowGroup" style="height:auto">
        <div class="row" >
        <div class="collable" style="width:15%">
        <asp:label  id="lblStatoBando" AssociatedControlID="lblStato" runat="server" CssClass="label" Font-Size="Small"  Text="Stato"></asp:label>
        </div>
        <div class="collable" style="width:85%">
            <asp:Label ID="lblStato" CssClass="labelDati"  runat="server"></asp:Label>
        </div>
        </div>
        </div>
         <div class="row">
        <div class="collable" style="width:15%">
        <asp:label  id="llbBando" AssociatedControlID="txtBando" runat="server" CssClass="label" Font-Size="Small" Text="<strong>(*)</strong>Bando"></asp:label>
        </div>
        <div class="collable" style="width:85%">
            <asp:TextBox ID="txtBando" CssClass="textbox" Width="96%"  runat="server" TextMode="MultiLine" Rows="5"></asp:TextBox>
        </div>
        </div>
           <br />
        <div class="row">
        <div class="collable" style="width:15%">
        <asp:label  id="lblBandoBreve" AssociatedControlID="txtBandoBreve" runat="server" Font-Size="Small" CssClass="label"  Text="<strong>(*)</strong>Descrizione Abbreviata Bando"></asp:label>
        </div>
        <div class="collable" style="width:85%">
            <asp:TextBox ID="txtBandoBreve" Width="96%" CssClass="textbox" runat="server" TextMode="MultiLine" Rows="4" MaxLength="254"></asp:TextBox>
        </div>
        </div>
        <br />
           <div class="row">
        <div class="collable" style="width:15%">
        <asp:label  id="lbltxtriferimento" AssociatedControlID="txtriferimento" runat="server" Font-Size="Small" CssClass="label"  Text="<strong>(*)</strong>Num. Rif."></asp:label>
        </div>
        <div class="collable" style="width:35%">
            <asp:TextBox ID="txtriferimento" CssClass="textbox" runat="server" ></asp:TextBox>
        </div>
        <div class="collable" style="width:15%">
        <asp:label  id="Label1" AssociatedControlID="TxtAnnoRiferimento" runat="server" CssClass="label" Font-Size="Small" Text="<strong>(*)</strong>Anno Rif."></asp:label>
        </div>
        <div class="collable" style="width:35%">
            <asp:TextBox ID="TxtAnnoRiferimento" CssClass="textbox" runat="server" MaxLength="6" ></asp:TextBox>
        </div>

        </div>
        <div class="row">
             <div class="collable" style="width:15%">
        <asp:label  id="lblImportoStanziato" AssociatedControlID="txtImportoStanziato" runat="server" Font-Size="Small" CssClass="label"  Text="<strong>(*)</strong>Importo Stanziato (€)"></asp:label>
        </div>
        <div class="collable" style="width:35%">
            <asp:TextBox ID="txtImportoStanziato" CssClass="textbox" class="currency" runat="server" onchange="javascript:CheckImportoValido('txtImportoStanziato', 'Importo Stanziato')" ></asp:TextBox>
        </div>
        </div>
        <div class="row">
        <div class="collable" style="width:15%">
        <asp:label  id="lbltxtInizio" AssociatedControlID="txtInizio" runat="server" CssClass="label" Font-Size="Small"  Text="<strong>(*)</strong>Data Inizio"></asp:label>
        </div>
        <div class="collable" style="width:35%">
            <asp:TextBox ID="txtInizio" CssClass="textbox" runat="server" ></asp:TextBox>
        </div>
        <div class="collable" style="width:15%">
        <asp:label  id="l" AssociatedControlID="txtfine" runat="server" CssClass="label"  Font-Size="Small" Text="<strong>(*)</strong>Data Fine"></asp:label>
        </div>
        <div class="collable" style="width:35%">
            <asp:TextBox ID="txtfine" CssClass="textbox" runat="server" ></asp:TextBox>
        </div>

        </div>
          <div class="row">
        <div class="collable" style="width:15%">
        <asp:label  id="lblTxtDataInizioVolontari" AssociatedControlID="TxtDataInizioVolontari" Font-Size="Small" runat="server" CssClass="label"  Text="Data Inizio Vol."></asp:label>
        </div>
        <div class="collable" style="width:35%">
            <asp:TextBox ID="TxtDataInizioVolontari" CssClass="textbox" runat="server" ></asp:TextBox>
        </div>
        <div class="collable" style="width:15%">
        <asp:label  id="lblTxtDataFineVolontari" AssociatedControlID="TxtDataFineVolontari" Font-Size="Small" runat="server" CssClass="label"  Text="Data Fine Vol."></asp:label>
        </div>
        <div class="collable" style="width:35%">
            <asp:TextBox ID="TxtDataFineVolontari" CssClass="textbox" runat="server" ></asp:TextBox>
        </div>

        </div>
               <div class="row">
        <div class="collable" style="width:15%">
        <asp:label  id="lblTxtDataScadGrad" AssociatedControlID="TxtDataScadGrad" runat="server" Font-Size="Small" CssClass="label"  Text="Data Scadenza Grad."></asp:label>
        </div>
        <div class="collable" style="width:35%">
            <asp:TextBox ID="TxtDataScadGrad" CssClass="textbox" runat="server" ></asp:TextBox>
        </div>
        </div>
        </div>
        <br />
        <div class="wrapper" style="width:98%">
        <br />
            <div class="row">
        <div class="collable" style="width:25%">
        <asp:label  id="lblNMaxVolontariProgettoItalia" AssociatedControlID="TxtNMaxVolontariProgettoItalia" Font-Size="Small" runat="server" CssClass="label"  Text="<strong>(*)</strong>N&#186; Max Volontari Progetto Italia"></asp:label>
        </div>
        <div class="collable" style="width:25%">
            <asp:TextBox ID="TxtNMaxVolontariProgettoItalia" onchange="javascript:CheckInteroValido('TxtNMaxVolontariProgettoItalia', 'N&#186; Max Volontari Progetto Italia')" CssClass="textbox" runat="server" Text="0"></asp:TextBox>
        </div>
        <div class="collable" style="width:25%">
        <asp:label  id="lblNMaxVolontariProgettoEstero" AssociatedControlID="TxtNMaxVolontariProgettoEstero" Font-Size="Small" runat="server" CssClass="label"  Text="<strong>(*)</strong>N&#186; Max Volontari Progetto Estero"></asp:label>
        </div>
        <div class="collable" style="width:25%">
            <asp:TextBox ID="TxtNMaxVolontariProgettoEstero" onchange="javascript:CheckInteroValido('TxtNMaxVolontariProgettoEstero', 'N&#186; Max Volontari Progetto Estero')" CssClass="textbox" runat="server" Text="0" ></asp:TextBox>
        </div>

        </div>
            <div class="row">
                    <div class="collable" style="width:25%">
        <asp:label  id="lblNMinVolontariProgettoItalia" AssociatedControlID="TxtNMinVolontariProgettoItalia" Font-Size="Small" runat="server" CssClass="label"  Text="<strong>(*)</strong>N&#186; Min Volontari Progetto Italia"></asp:label>
        </div>
                    <div class="collable" style="width:25%">
            <asp:TextBox ID="TxtNMinVolontariProgettoItalia" onchange="javascript:CheckInteroValido('TxtNMinVolontariProgettoItalia', 'N&#186; Min Volontari Progetto Italia')" CssClass="textbox" runat="server" Text="0"></asp:TextBox>
        </div>
                    <div class="collable" style="width:25%">
        <asp:label  id="lblNMinVolontariProgettoEstero" AssociatedControlID="TxtNMinVolontariProgettoEstero" Font-Size="Small" runat="server" CssClass="label"  Text="<strong>(*)</strong>N&#186; Min Volontari Progetto Estero"></asp:label>
        </div>
                    <div class="collable" style="width:25%">
            <asp:TextBox ID="TxtNMinVolontariProgettoEstero" onchange="javascript:CheckInteroValido('TxtNMinVolontariProgettoEstero', 'N&#186; Min Volontari Progetto Estero')" CssClass="textbox" runat="server" Text="0"></asp:TextBox>
        </div>
            </div>
            <div class="row">
                <div class="collable" style="width:25%">
                <asp:label  id="lblNMinVolontariSedeItalia" AssociatedControlID="TxtNMinVolontariSedeItalia" Font-Size="Small" runat="server" CssClass="label"  Text="<strong>(*)</strong>N&#186; Min Volontari Sede Italia"></asp:label>
                </div>
                <div class="collable" style="width:25%">
                    <asp:TextBox ID="TxtNMinVolontariSedeItalia" onchange="javascript:CheckInteroValido('TxtNMinVolontariSedeItalia', 'N&#186; Min Volontari Sede Italia')" CssClass="textbox" runat="server" Text="0"></asp:TextBox>
                </div>
                <div class="collable" style="width:25%">
                <asp:label  id="lblNMinVolontariSedeEstero" AssociatedControlID="TxtNMinVolontariSedeEstero" Font-Size="Small" runat="server" CssClass="label"  Text="<strong>(*)</strong>N&#186; Min Volontari Sede Estero"></asp:label>
                </div>
                <div class="collable" style="width:25%">
                    <asp:TextBox ID="TxtNMinVolontariSedeEstero" onchange="javascript:CheckInteroValido('TxtNMinVolontariSedeEstero', 'N&#186; Min Volontari Sede Estero')" CssClass="textbox" runat="server" Text="0"></asp:TextBox>
                </div>
            </div>
            <div class="row">
            <div class="collable" style="width:25%">
             <asp:label  id="lblNMaxVolontariProgettoCoprogettato" AssociatedControlID="TxtNMaxVolontariProgettoCoprogettato" Font-Size="Small" runat="server" CssClass="label"  Text="<strong>(*)</strong>N&#186; Max Volontari Progetto Coprogettato"></asp:label>
            
            </div>
            <div class="collable" style="width:25%">
                    <asp:TextBox ID="TxtNMaxVolontariProgettoCoprogettato" onchange="javascript:CheckInteroValido('TxtNMaxVolontariProgettoCoprogettato', 'N&#186; Max Volontari Progetto Coprogettato')" CssClass="textbox" runat="server" Text="0"></asp:TextBox>
            </div>
            <div class="collable" style="width:25%">
            
            
            </div>
            <div class="collable" style="width:25%">
            
            
            
            </div>
            
            </div>

    </div>
    <br />
       <div class="wrapper" style="width:100%;border:0px;">
            <div class="row">
            <div class="collable" style="width:25%">
            <asp:checkbox id="ChkAssAutomatica" runat="server" Font-Size="Small" AutoPostBack="True" Text="Ass. Automatica"></asp:checkbox>
            </div>
              <div class="collable" style="width:25%">
            <asp:checkbox id="ChkAbilita" runat="server" AutoPostBack="True"  Font-Size="Small" Text="Ente Abilitato"></asp:checkbox>
            </div>
         <div class="collable" style="width:20%">
            <asp:label  id="Label3" AssociatedControlID="TxtGruppo" runat="server" CssClass="label" Font-Size="Small" Text="<strong>(*)</strong>Gruppo"></asp:label>
        </div>
            <div class="collable" style="width:20%">
              <asp:TextBox ID="TxtGruppo" CssClass="textbox" runat="server" MaxLength="5" ></asp:TextBox>
        </div>
          <div class="collable" style="width:5%">
              <asp:ImageButton ID="cmdCalcola" runat="server" AlternateText="Calcola progressivo gruppo" ToolTip="Progressivo Cruppo"  ImageUrl="Images/Icona_Progetto_small.png"/>
        </div>
        </div>
       </div>
       <asp:datagrid id="dgTipiProgetto" CssClass="table" runat="server" CellPadding="2" ToolTip="Elenco Tipi Progetto"  PageSize = "20"   AllowPaging="true" 
            AllowSorting="True" AutoGenerateColumns="False"  UseAccessibleHeader="True"  Visible="true" ShowFooter="false"  Width="98%">
		<FooterStyle></FooterStyle>
		<SelectedItemStyle ></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle ></HeaderStyle>
        <Columns>
            <asp:TemplateColumn HeaderText="Sel.">
				<ItemTemplate >
					<asp:CheckBox id="check1" runat="server" AutoPostBack="False" Text="Sel"  TextAlign="Left" ToolTip="Seleziona Tipo progetto"></asp:CheckBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn Visible="False" DataField="Idtipoprogetto" HeaderText="Idtipoprogetto">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Descrizione" HeaderText="Tipi Progetto">
			</asp:BoundColumn>
            <asp:TemplateColumn HeaderText="Profili" > 
                <ItemTemplate>
					<asp:ImageButton  CommandName="Profili" AlternateText="Visualizza Profili" ToolTip="Profili" runat="server" ImageURL="Images/lenteIngrandimento_small.png"></asp:ImageButton>      
                    </ItemTemplate>
            </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Ambiti attivita" > 
                <ItemTemplate>
					<asp:ImageButton  CommandName="AmbitiAttivita" AlternateText="Ambiti attivita" ToolTip="Ambiti" runat="server" ImageURL="Images/lenteIngrandimento_small.png"></asp:ImageButton>      
                    </ItemTemplate>
            </asp:TemplateColumn>
            </Columns>
        <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
            </asp:datagrid>
            </div>
            </fieldset>
  </div>
      <%--  Elenco Regioni--%>
       <div class="collable" style="width:19%" id="divElencoRegioni">
       <div class="row">
          <div class="collable" style="width:100%;margin-right:2px">
           
           &nbsp;<asp:checkbox id="chkSelDesel"  runat="server" Font-Bold="True" 
								  TextAlign="Left" Text="Seleziona tutto" Font-Size="Small" AutoPostBack="True"></asp:checkbox>
          
       </div>
       </div>
       <asp:datagrid id="dgRegioneCompetenza" CssClass="table" runat="server" CellPadding="2" Caption="Regioni Competenza"  AllowPaging="false" 
                    AllowSorting="True" AutoGenerateColumns="False"  UseAccessibleHeader="True" Visible="true" ShowFooter="false"  Width="100%">
			    <FooterStyle></FooterStyle>
		        <SelectedItemStyle ></SelectedItemStyle>
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		        <HeaderStyle ></HeaderStyle>
                <Columns>
                <asp:TemplateColumn HeaderText="Sel">
					<ItemStyle HorizontalAlign="Center"  ></ItemStyle>
						<ItemTemplate>
    						<asp:CheckBox id="check2" DataField="idRegioneCompetenza" Text="Sel" TextAlign="Left"  runat="server"  AutoPostBack="false" ToolTip="Seleziona Regione Competenza"></asp:CheckBox>
						</ItemTemplate>
				</asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="idRegioneCompetenza" HeaderText="idRegioneCompetenza"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="codiceregionecompetenza" HeaderText="Codice Regione">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Descrizione" HeaderText="Regione Competenza">
				</asp:BoundColumn>
                    </Columns>
                    </asp:datagrid>
       </div> 

       </div>
        </div>

    <br />
        <div class="wrapper" style="width:100%;border:0px;">
            <div class="RigaPulsanti">
            <asp:Button ID="cmdImpVirtuali" CssClass="Pulsante" runat="server" Visible="false" Text="Assegna Importi"/>
            <asp:Button ID="cmdPubblica" CssClass="Pulsante" runat="server" Text="Pubblica Bando"   />
            <asp:Button ID="cmdCancella" CssClass="Pulsante" runat="server" Text="Cancella Bando"  />
            <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" ToolTip="Salva" Text="Salva" />
            <asp:Button ID="CmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
         
            </div>
            </div>

    </fieldset>
    <asp:HiddenField ID="lblIdBando" runat="server" />
    <asp:HiddenField ID="lblRicerca" runat="server" />
    <asp:HiddenField ID="lblpage" runat="server" />
    <asp:HiddenField ID="Hdd_chkFlag" runat="server" />
     <asp:HiddenField ID="Hdd_dtfinevol" runat="server" />
     <asp:HiddenField ID="Hdd_dtiniziovol" runat="server" />
     <asp:HiddenField ID="Hdd_Rif" runat="server" />
     <asp:HiddenField ID="hf_DisabilitaCalendar" Value="0" runat="server" />

</asp:Content>
