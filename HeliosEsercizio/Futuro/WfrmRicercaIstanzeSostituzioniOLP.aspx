<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaIstanzeSostituzioniOLP.aspx.vb" Inherits="Futuro.WfrmRicercaIstanzeSostituzioniOLP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <script src="Scripts/JHelper.js" type="text/javascript"></script>

    <style type="text/css"> .ui-datepicker {font-size: 11px;}</style>

    <script  type="text/javascript">

        var prefissoIdClient = "MainContent_";

        function CostruisciId(IdServer) {
            var IdClient = prefissoIdClient + IdServer;
            return IdClient;
        };

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
        <legend id="lgContornoPagina" runat="server">Ricerca istanze sostituzioni OLP</legend>
        <div class="wrapper" style="width: 100%">
            <div class="headers">
                <h2>
                    <asp:Label ID="lblTitolo" runat="server" Text="Istanze sostituzioni OLP"></asp:Label>
                </h2>
            </div>
            <asp:Label ID="lblMessaggio" runat="server" CssClass="msgInfo"></asp:Label>
            <div class="RigaVuota">&nbsp;</div>
                <div class="row" id="divRicercaEnte" runat="server" visible="false">
                    <div class="collable" style="width:15%">
                    &nbsp;<asp:Label ID="lblDenEnte" CssClass="label" AssociatedControlID="txtDenominazioneEnte" runat="server" Text="Denominazione Ente"/>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:TextBox ID="txtDenominazioneEnte" runat="server" CssClass="textbox" />
                    </div>
                    <div class="collable" style="width:15%">
                        <asp:Label ID="LblCodRegione" CssClass="label" AssociatedControlID="txtCodRegione" runat="server" Text="Cod. Ente"/>
                    </div>
                    <div class="colOggetti" style="width:35%">
                         <asp:TextBox ID="txtCodRegione" runat="server" CssClass="textbox" />
                    </div> 
                </div>
				<div class="row" >
					<div class="collable" style="width:15%">
					&nbsp;<asp:Label ID="lblTitoloProgetto" AssociatedControlID="txtTitoloProgetto" runat="server" Text="Titolo Progetto"></asp:Label>
					</div>
					<div class="colOggetti" style="width:35%" >
						<asp:TextBox ID="txtTitoloProgetto" runat="server"></asp:TextBox>
					</div>
					<div class="collable" style="width:15%">
						<asp:Label ID="lblCodiceProgetto"  AssociatedControlID="TxtCodProg" runat="server" Text="Codice Progetto"></asp:Label>
					</div>
					<div class="colOggetti" style="width:35%">
						<asp:TextBox ID="TxtCodProg" runat="server" ></asp:TextBox>
					</div>
				</div>					
                <div class="row">
                    <div class="collable" style="width:15%">
                    &nbsp;<asp:Label ID="lblNomeSostRicerca" CssClass="label" AssociatedControlID="txtNomeSostRicerca" runat="server" Text="Nome sostituito"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtNomeSostRicerca" CssClass="textbox" runat="server"></asp:TextBox>         
                    </div> 
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblCognomeSostRicerca" CssClass="label" AssociatedControlID="txtCognomeSostRicerca" runat="server" Text="Cognome sostituito"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtCognomeSostRicerca" CssClass="textbox" runat="server"></asp:TextBox>         
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width:15%">
                    &nbsp;<asp:Label ID="lblNomeSubRicerca" CssClass="label" AssociatedControlID="txtNomeSubRicerca" runat="server" Text="Nome subentrante"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtNomeSubRicerca" CssClass="textbox" runat="server"></asp:TextBox>         
                    </div> 
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblCognomeSubRicerca" CssClass="label" AssociatedControlID="txtCognomeSubRicerca" runat="server" Text="Cognome subentrante"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtCognomeSubRicerca" CssClass="textbox" runat="server"></asp:TextBox>         
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width:15%">      
                    &nbsp;<asp:Label ID="lblDataInizio" CssClass="label" AssociatedControlID="txtDataInizio" runat="server" Text="Data di protocollo inizio"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtDataInizio" CssClass="textbox" runat="server" Width="75px" MaxLength="10"></asp:TextBox>         
                    </div> 
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblDataFine" CssClass="label" AssociatedControlID="txtDataFine" runat="server" Text="Data di protocollo fine"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtDataFine" CssClass="textbox" runat="server" Width="75px" MaxLength="10"></asp:TextBox>         
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width:15%">      
                    &nbsp;<asp:Label ID="lblStatoIstanza" CssClass="label" AssociatedControlID="ddlStatoIstanza" runat="server" Text="Stato Istanza"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:DropDownList ID="ddlStatoIstanza" runat="server" Width="30%">
                        <asp:ListItem Value="0">TUTTI</asp:ListItem>
				        <asp:ListItem Value="1">Registrata</asp:ListItem>
				        <asp:ListItem Value="2">Presentata</asp:ListItem>
				        <asp:ListItem Value="3">Valutata</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            <div class="RigaPulsanti" style="text-align:right;">
                <asp:Button ID="btnRicerca" CssClass="Pulsante" runat="server" ToolTip="Ricerca" Text="Ricerca"  />
                <asp:Button ID="cmdInserisci" CssClass="Pulsante" runat="server" ToolTip="Inserisci istanza sostituzioni OLP" Text="Inserisci istanza sostituzioni OLP"  OnClick="cmdInserisci_Click"/>
            </div>
        </div>
        <br />
		<asp:datagrid id="dgIstanze" runat="server" Width="100%" 
				ToolTip="Elenco Istanze" CssClass="table"  AllowPaging="True" 
				AllowSorting="True" AutoGenerateColumns="False"
				UseAccessibleHeader="True">
			<EditItemStyle></EditItemStyle>
			<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
			<ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
			<HeaderStyle></HeaderStyle>
			<Columns>
				<asp:TemplateColumn HeaderText="Selez."> 
				<ItemTemplate>
					<asp:ImageButton ID="IdImgSelIstanza" style="cursor:pointer;" CommandName="Select" alt="Seleziona Istanza" ToolTip='Seleziona Istanza' runat="server" ImageURL="images/lenteIngrandimento_small.png" CausesValidation="false"></asp:ImageButton>      
					</ItemTemplate>
				</asp:TemplateColumn>
                <asp:BoundColumn DataField="IdEnte" HeaderText="IdEnte" Visible="false"></asp:BoundColumn>
                <asp:BoundColumn DataField="Competenza" HeaderText="Competenza" Visible="false"></asp:BoundColumn>
                <asp:BoundColumn DataField="CodiceEnte" HeaderText="Cod. Ente" Visible="false"></asp:BoundColumn>
                <asp:BoundColumn DataField="Denominazione" HeaderText="Ente" Visible="false"></asp:BoundColumn>
				<asp:BoundColumn DataField="ProtocolloOLP" HeaderText="Codice Istanza / Protocollo"></asp:BoundColumn>
				<asp:BoundColumn DataField="StatoIstanza" HeaderText="Stato"></asp:BoundColumn>
				<asp:BoundColumn DataField="DataCreazioneRecord" HeaderText="Data Creazione"></asp:BoundColumn>
				<asp:BoundColumn DataField="UsernameCreazioneRecord" HeaderText="Utente Creazione"></asp:BoundColumn>
				<asp:BoundColumn DataField="Numero" HeaderText="Numero sostituzioni">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="IdIstanzaSostituzioneOLP" HeaderText="Codice Istanza" Visible="false"></asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
    </fieldset>
</asp:Content>
