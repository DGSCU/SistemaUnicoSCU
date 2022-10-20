<%@ Page Title="Inserimento Assenze Volontari" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="assenzevolontari.aspx.vb" Inherits="Futuro.assenzevolontari" MaintainScrollPositionOnPostBack="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css"> .ui-datepicker {font-size: 11px;}</style>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
   
    <script type="text/javascript">

        var prefissoIdClient = "MainContent_";

        function CostruisciId(IdServer) {
            var IdClient = prefissoIdClient + IdServer
            return IdClient
        }


        function ElencoDocumenti() {

            var prefissoCodiceFascicolo = CostruisciId("TxtCodiceFascicolo");
            var prefissoIdFascicolo = CostruisciId("TxtIdFascicolo");

            var codiceFascicolo = document.getElementById(prefissoCodiceFascicolo);
            var idFascicolo = document.getElementById(prefissoIdFascicolo);

            if (codiceFascicolo.value == '') {
	            alert("Specificare il numero fascicolo!");
	            codiceFascicolo.focus();
	        } else {
	            myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?NumeroFascicolo=" + idFascicolo.value, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");
	        }

	        return false;
	    }

	    function Protocollazione() {
            
            var prefissoCognome = CostruisciId("lblCognome");
            var prefissoCodiceFascicolo = CostruisciId("TxtCodiceFascicolo");
            var prefissoIdFascicolo = CostruisciId("TxtIdFascicolo");
            var prefissoDescrFascicolo = CostruisciId("txtDescFasc");

            var cognome = document.getElementById(prefissoCognome);
            var codiceFascicolo = document.getElementById(prefissoCodiceFascicolo);
            var idFascicolo = document.getElementById(prefissoIdFascicolo);
            var descrFascicolo = document.getElementById(prefissoDescrFascicolo);

            myWin = window.open("WfrmSIGEDProtocollazione.aspx?CorrCodUnivoco=" + cognome.innerText + "&Processo=VOLONTARI&VArUpdate=0&Fascicolo=" + codiceFascicolo.value + "&NumeroFascicolo=" + idFascicolo.value + "&DescFascicolo=" + descrFascicolo.value + "&CodEnte=<%= Session("txtCodEnte")%>", "Visualizza","width=800,height=400,dependent=no,scrollbars=yes,status=no");

	        return false;

	    }


         $(function () {
            var DataInizio = CostruisciId("txtDataInizio");
            var readonlyInizio = '<%=txtDataInizio.ReadOnly%>';
            if (readonlyInizio == "False") {
                var sharpDataInizio = "#" + DataInizio
                $("" + sharpDataInizio + "").datepicker();
            }

            var DataFine = CostruisciId("txtDataFine");
            var readonlyFine = '<%=txtDataFine.ReadOnly%>';
            if (readonlyFine == "False") {
                var sharpDataFine = "#" + DataFine
                $("" + sharpDataFine + "").datepicker();
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Inserimento Assenze Volontari</legend>
        <asp:label id="lblmess"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Gestione Assenze Volontari"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblStatoVolontario" CssClass="label" AssociatedControlID="lblStato" runat="server" Text="Stato Volontario"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblStato" CssClass="bold" runat="server"></asp:label>         
                </div>
            </div>
            <div class="row" id="divProtocollo" runat="server" Visible="False">
                <div class="collable" style="width:15%">
                    <asp:label id="lblCodiceFascicolo" CssClass="label" runat="server" AssociatedControlID="TxtCodiceFascicolo" Text="Numero Fascicolo"></asp:label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:textbox id="TxtCodiceFascicolo" runat="server" autofocus="true" CssClass="textbox" ReadOnly="True" Width="95%"></asp:textbox>
                </div>
                <div class="colOggetti" style="width:50%">
                    <asp:imagebutton id="cmdSelProtocollo0" AlternateText="Elenco Documenti" ToolTip="Elenco Documenti" style="CURSOR: hand" runat="server" ImageUrl="images/busta_small.png" OnClientClick="ElencoDocumenti()"></asp:imagebutton>
                    <asp:imagebutton ID="cmdNuovoFasciocloDLA" AlternateText="Allega File" ToolTip="Allega File" style="CURSOR: hand" runat="server" ImageUrl="images/documento_small.png"  OnClientClick="Protocollazione()"></asp:imagebutton>
                    <asp:HiddenField ID="TxtIdFascicolo" runat="server" />
                </div>
            </div>
            <div class="row" id="divFascicolo" runat="server" Visible="false">
                <div class="collable" style="width:15%">
                    <asp:label id="LblDescrFascicolo" runat="server" CssClass="label" AssociatedControlID="txtDescFasc" Text="Descr. Fascicolo"></asp:label>
                </div>
                <div class="colOggetti" style="width:85%">
                    <asp:textbox id="txtDescFasc" runat="server" ReadOnly="True" TextMode="MultiLine" CssClass="textbox"></asp:textbox>
                </div>
            </div>
        </div>
        <div class="wrapper" style="width:100%">
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblCodiceVolontario" CssClass="label" AssociatedControlID="lblCognome" runat="server" Text="Codice Volontario"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblCognome" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblNominativo" CssClass="label" AssociatedControlID="lblNome" runat="server" Text="Nominativo"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblNome" CssClass="bold" runat="server"></asp:label>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblSessoLabel" CssClass="label" AssociatedControlID="lblsesso" runat="server" Text="Sesso"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblsesso" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDataNascitaLabel" CssClass="label" AssociatedControlID="lbldataNascita" runat="server" Text="Data Nascita"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lbldataNascita" CssClass="bold" runat="server"></asp:label>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblComuneNascitaLabel" CssClass="label" AssociatedControlID="lblComuneNascita" runat="server" Text="Comune Nascita"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblComuneNascita" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblComuneResidenzaLabel" CssClass="label" AssociatedControlID="lblComuneResidenza" runat="server" Text="Comune Residenza"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblComuneResidenza" CssClass="bold" runat="server"></asp:label>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblCodiceFiscale" CssClass="label" AssociatedControlID="lblCodFis" runat="server" Text="Codice Fiscale"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblCodFis" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblProgettoLabel" CssClass="label" AssociatedControlID="lblProgetto" runat="server" Text="Progetto"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblProgetto" CssClass="bold" runat="server"></asp:label>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDataInizioLabel" CssClass="label" AssociatedControlID="lblDataInizio" runat="server" Text="Data Inizio"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblDataInizio" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDataFineLabel" CssClass="label" AssociatedControlID="lbldataFine" runat="server" Text="Data Fine"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lbldataFine" CssClass="bold" runat="server"></asp:label>         
                </div>
            </div>
        </div>
        <br />
    <p style="text-align:center">
        <asp:label id="Label2" runat="server" CssClass="bold" Text="Sedi Progetto"></asp:label>
    </p>
    <asp:datagrid id="dgRisultatoRicercaSedi" runat="server" CssClass="table" Width="100%" ToolTip="Elenco sedi Attuazione" CellPadding="2" AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" PageSize="3" UseAccessibleHeader="True">
		<SelectedItemStyle BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
		<Columns>
			<asp:BoundColumn DataField="sedefisica" HeaderText="Sede Fisica">
				<HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="sedeAttuazione" HeaderText="Sede Att.">
				<HeaderStyle HorizontalAlign="Center" Width="16%" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="indirizzo" HeaderText="Indirizzo">
				<HeaderStyle HorizontalAlign="Center" Width="20%" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="comune" HeaderText="Comune ">
				<HeaderStyle Width="16%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn Visible="False" HeaderText="N&#176; Vol. Richiesti">
				<HeaderStyle HorizontalAlign="Center" Width="8%" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn Visible="False" HeaderText="N&#176; Vol. Assegnati">
				<HeaderStyle Width="8%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Right"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="identit&#224;" HeaderText="identit&#224;"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="idattivit&#224;entesedeattuazione" HeaderText="idattivit&#224;entesedeattuazione"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="datafineattivit&#224;entit&#224;" HeaderText="datafineattivit&#224;entit&#224;"
				DataFormatString="{0:d}"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="statodefault" HeaderText="statodefault"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="note" HeaderText="note"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="percentualeutilizzo" HeaderText="percentualeutilizzo"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="idtipologiaposto" HeaderText="idtipologiaposto"></asp:BoundColumn>
		</Columns>
		<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
    <br />
    <p style="text-align:center">
        <asp:label id="lblTitotloElencoVolontari" runat="server" CssClass="bold" Text="Elenco Assenze"></asp:label>
    </p>
    <asp:datagrid id="dgRisultatoRicercaAssenze" runat="server" CssClass="table"  Width="100%" ToolTip="Elenco Assenze"  CellPadding="2"  AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" PageSize="3" UseAccessibleHeader="True">
        <SelectedItemStyle BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
		<Columns>
            <asp:TemplateColumn> 
                <ItemTemplate>
					<asp:ImageButton ID="ImageButton1" CommandName="Modifica" ToolTip="Seleziona Assenza" AlternateText="Seleziona Assenza" runat="server" ImageURL="images/valida_small.png" CausesValidation="false"></asp:ImageButton>                         
                </ItemTemplate>
            </asp:TemplateColumn>
			<asp:BoundColumn Visible="False" DataField="identit&#224;assenza" HeaderText="identit&#224;assenza"
				DataFormatString="{0:d}">
				<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="anno" HeaderText="Anno">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Mese" HeaderText="Mese">
				<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Giorni" HeaderText="N&#176;Giorni"></asp:BoundColumn>
			<asp:BoundColumn DataField="DataConferma" HeaderText="Data di Conferma">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Causale" HeaderText="Causale">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Stato" HeaderText="Stato"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="Note" HeaderText="Note"></asp:BoundColumn>
			<asp:ButtonColumn Visible="False" Text="&lt;img src=images/mini_conferma.jpg Width=20 Height=20 title='Conferma Assenza' border=0&gt;"
				HeaderText="Conferma" CommandName="Conferma">
				<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:ButtonColumn>
            <asp:BoundColumn DataField="DataInizioGreen" HeaderText="Data Inizio" DataFormatString="{0:d}">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
            <asp:BoundColumn DataField="DataFineGreen" HeaderText="Data Fine"  DataFormatString="{0:d}">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
             <asp:TemplateColumn> 
                <ItemTemplate>
					<asp:ImageButton ID="ImageButton2" CommandName="Respingi" ToolTip="Rifiuta Assenza" AlternateText="Rifiuta Assenza" runat="server" ImageURL="images/canc_small.png" CausesValidation="false"></asp:ImageButton>                         
                </ItemTemplate>
            </asp:TemplateColumn>
			<asp:BoundColumn Visible="False" DataField="IdCausale" HeaderText="IdCausale"></asp:BoundColumn>
		</Columns>
		<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
    <br />
    <div class="wrapper" style="width:100%">
        <div class="RigaVuota" style="text-align:center">
            <asp:label id="lblInserimentoAssenze" runat="server" CssClass="bold" Text="Inserimento Assenze"></asp:label>
            <asp:HiddenField ID="txtidentitaassenza" runat="server" />
        </div>
        <div class="RigaVuota" >&nbsp;</div>
        <div class="row" >
            <div class="collable" style="width:15%">      
                <asp:Label ID="lblAnno" CssClass="label" AssociatedControlID="ddlAnno" runat="server" Text="(*)Anno"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">       
                <asp:dropdownlist id="ddlAnno" runat="server" CssClass="ddlClass"></asp:dropdownlist>         
            </div>
            <div class="collable" style="width:15%">
                <asp:label id="lblMese" CssClass="label" runat="server" AssociatedControlID="ddlMesi" Text="(*)Mese"></asp:label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:dropdownlist id="ddlMesi" runat="server"  CssClass="ddlClass">
                    <asp:ListItem Value="0" Selected="True">Selezionare</asp:ListItem>
					<asp:ListItem Value="1">Gennaio</asp:ListItem>
					<asp:ListItem Value="2">Febbraio</asp:ListItem>
					<asp:ListItem Value="3">Marzo</asp:ListItem>
					<asp:ListItem Value="4">Aprile</asp:ListItem>
					<asp:ListItem Value="5">Maggio</asp:ListItem>
					<asp:ListItem Value="6">Giugno</asp:ListItem>
					<asp:ListItem Value="7">Luglio</asp:ListItem>
					<asp:ListItem Value="8">Agosto</asp:ListItem>
					<asp:ListItem Value="9">Settembre</asp:ListItem>
					<asp:ListItem Value="10">Ottobre</asp:ListItem>
					<asp:ListItem Value="11">Novembre</asp:ListItem>
					<asp:ListItem Value="12">Dicembre</asp:ListItem>
                </asp:dropdownlist> 
            </div>
        </div>
         <div class="row" >
            <div class="collable" style="width:15%">      
                <asp:Label ID="lblCausale" CssClass="label" AssociatedControlID="ddlCausale" runat="server" Text="(*)Causale"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">       
                <asp:dropdownlist id="ddlCausale" runat="server" CssClass="ddlClass" 
                    AutoPostBack="True"></asp:dropdownlist>         
            </div>
            <div class="collable" style="width:15%">
                <asp:label id="lblGiorni" CssClass="label" runat="server" AssociatedControlID="txtNumGiorni" Text="(*)N° Giorni"></asp:label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:textbox id="txtNumGiorni" runat="server" CssClass="textbox" MaxLength="2" ></asp:textbox>
            </div>
        </div>

        <div class="row" id="DIVgreenpass" runat="server" visible="false">
                <div class="collable" style="width:15%">      
                    &nbsp;<asp:Label ID="Label1" CssClass="label" AssociatedControlID="txtDataInizio" runat="server" Text="Data Inizio Sospensione "></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtDataInizio" CssClass="textbox" runat="server" Width="75px" MaxLength="10"></asp:TextBox>         
                    </div> 
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="Label3" CssClass="label" AssociatedControlID="txtDataFine" runat="server" Text="Data Fine Sospensione"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtDataFine" CssClass="textbox" runat="server" Width="75px" MaxLength="10"></asp:TextBox>         
                    </div>
        </div>

        <div class="row">
            <div class="collable" style="width:15%">
                <asp:label id="lblNote" runat="server" CssClass="label" AssociatedControlID="txtNote" Text="Note"></asp:label>
            </div>
            <div class="colOggetti" style="width:85%">
                <asp:textbox id="txtNote" runat="server" TextMode="MultiLine" CssClass="textbox" Width="96%"></asp:textbox>
            </div>
        </div>
        <div class="RigaPulsanti">
            <asp:Button ID="cmdModifica" runat="server" CssClass="Pulsante" Text="Salva" Visible="False" />&nbsp;
            <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" Text="Salva" />&nbsp;
            <asp:Button ID="CmdAnnulla" CssClass="Pulsante" runat="server" Text="Annulla" Visible="false" />&nbsp;
            <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
        </div>
    </div>
    </fieldset>
</asp:Content>
