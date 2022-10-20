<%@ Page Title="Ricollocazione Volontari" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="RicollocaVolontario.aspx.vb" Inherits="Futuro.RicollocaVolontario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <style type="text/css"> .ui-datepicker {font-size: 11px;}</style>

    <script  type="text/javascript">

        var prefissoIdClient = "MainContent_";

        function CostruisciId(IdServer) {
            var IdClient = prefissoIdClient + IdServer
            return IdClient
        }

        $(function () {
            var DataAvvio = CostruisciId('txtdataAvvio');
            var readonly = '<%=txtdataAvvio.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataAvvio = "#" + DataAvvio
                $("" + sharpDataAvvio + "").datepicker();
            }
        });

	</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Ricollocazione Volontari</legend>
        <asp:label id="lblMessaggi"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricollocazione Volontari"></asp:Label></h2>
            </div>
            <div class="RigaVuota">&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblStatoVolontario" CssClass="label" AssociatedControlID="lblStato" runat="server" Text="Stato Volontario"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblStato" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDataChiusura" CssClass="label" AssociatedControlID="txtdatachiusuraEV" runat="server" Text="Data Chiusura"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">        
                    <asp:label id="txtdatachiusuraEV" CssClass="bold" runat="server"></asp:label>      
                </div>
            </div>
        </div>
        <div class="wrapper" style="width:100%">
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblCognomeLabel" CssClass="label" AssociatedControlID="lblCognome" runat="server" Text="Cognome"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblCognome" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblNomeLabel" CssClass="label" AssociatedControlID="lblNome" runat="server" Text="Nome"></asp:Label>   
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
                    <asp:Label ID="lblDataInizioLabel" CssClass="label" AssociatedControlID="lblDataInizio" runat="server" Text="Data Inizio Progetto"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblDataInizio" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDataFineLabel" CssClass="label" AssociatedControlID="lbldataFine" runat="server" Text="Data Fine Progetto"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lbldataFine" CssClass="bold" runat="server"></asp:label>         
                </div>
            </div>
        </div>
        <br />
        <h3>
            <asp:label id="lblSedeProgettoAttuale" runat="server" Text="Sede Progetto Attuale"></asp:label>
        </h3>
        <asp:datagrid id="dgRisultatoRicercaSedi" runat="server" CssClass="table" Width="100%" ToolTip="Elenco sedi Attuazione" CellPadding="2" AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" PageSize="3" UseAccessibleHeader="True">
			<SelectedItemStyle BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
                <asp:BoundColumn DataField="img" Visible="False">
			        <HeaderStyle Width="2%"></HeaderStyle>
		        </asp:BoundColumn>
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
				<asp:BoundColumn DataField="Percentuale" HeaderText="Percentuale ">
					<HeaderStyle Width="16%"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="DataInizio" HeaderText="Data Inizio ">
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
				<asp:BoundColumn Visible="False" DataField="IDAttivit&#224;Entit&#224;" HeaderText="IDAttivit&#224;Entit&#224;"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="idente" HeaderText="idente"></asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
        <div class="RigaPulsanti" style="text-align:right">
            <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />
        </div>
        <br />
        <h3>
            <asp:label id="lblTitotloElencoVolontari" runat="server" Text="Sede Progetto Collocamento" Visible="False"></asp:label>
        </h3>
        <asp:datagrid id="dtgElencoSedi" runat="server" CssClass="table"  Width="100%" ToolTip="Elenco Sedi"  CellPadding="2"  AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" PageSize="3" UseAccessibleHeader="True">
			<SelectedItemStyle BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
                <asp:BoundColumn DataField="img" Visible="False">
					<HeaderStyle Width="2%"></HeaderStyle>
				</asp:BoundColumn>
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
				<asp:TemplateColumn Visible="False">
					<HeaderStyle Width="120px"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<HeaderTemplate>
						<b>Percentuale Utilizzo</b>
					</HeaderTemplate>
					<ItemTemplate>
						<font size="2" face="verdana" color="navy"><b>%</b></font>
						<asp:TextBox onkeypress="javascript: CheckValue()" onblur="javascript: ControllaValore(this.id,this.value,this.name);"
							ID="txtPercentualeUtilizzo" Width="30" Runat="server" MaxLength="3">100</asp:TextBox>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn Visible="False">
					<ItemStyle HorizontalAlign="Left"></ItemStyle>
					<HeaderTemplate>
						<b>Note</b>
					</HeaderTemplate>
					<ItemTemplate>
						<asp:TextBox ID="txtNote" Runat="server" Width="100" MaxLength="100"></asp:TextBox>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="identit&#224;" HeaderText="identit&#224;"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="idattivit&#224;entesedeattuazione" HeaderText="idattivit&#224;entesedeattuazione"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="datafineattivit&#224;entit&#224;" HeaderText="datafineattivit&#224;entit&#224;"
					DataFormatString="{0:d}"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="statodefault" HeaderText="statodefault"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="note" HeaderText="note"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="percentualeutilizzo" HeaderText="percentualeutilizzo"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="idtipologiaposto" HeaderText="idtipologiaposto"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="idente" HeaderText="idente"></asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
        
            <div class="RigaVuota">&nbsp;</div>
            <div class="row">
	            <div class="collable" style="width:50%">
		            <asp:label id="lbldataAvvio"  AssociatedControlID="txtdataAvvio" runat="server" CssClass="label" Text="Data di trasferimento (gg/mm/aaaa)"></asp:label>&nbsp;
		            <asp:textbox id="txtdataAvvio" runat="server" Width="75px"  CssClass="textbox" MaxLength="10"></asp:textbox>
	            </div>
            </div>
            <div class="RigaPulsanti" style="text-align:right">
	            <asp:Button ID="cmdConferma" runat="server" CssClass="Pulsante" Text="Conferma" />
	            <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
            </div>
        
        <asp:HiddenField id="txtidentitaSbentrante" runat="server" />
        <asp:HiddenField id="txtIdentitaVecchia" runat="server" />
        <asp:HiddenField id="txtidattivita" runat="server" />
        <asp:HiddenField id="txtdatalimite" runat="server"/>
    </fieldset>

</asp:Content>
