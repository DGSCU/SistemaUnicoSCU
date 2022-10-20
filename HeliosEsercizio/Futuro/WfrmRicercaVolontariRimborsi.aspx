<%@ Page Title="Ricerca Volontari Per Rimborsi" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaVolontariRimborsi.aspx.vb" Inherits="Futuro.WfrmRicercaVolontariRimborsi" %>
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
        
            //DataDal
            var DataDal = CostruisciId('txtdatadal');
            var readonly = '<%=txtdatadal.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataAvvio = "#" + DataDal
                $("" + sharpDataAvvio + "").datepicker();
            }

            //DataAl
            var DataAl = CostruisciId('txtdataal');
            var readonly = '<%=txtdataal.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataAvvio = "#" + DataAl
                $("" + sharpDataAvvio + "").datepicker();
            }

        });

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Ricerca Volontari Per Rimborsi</legend>
        <asp:label id="lblMessaggiErrore"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
                <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Volontari per Rimborsi"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" id="divTipoUtente" runat="server">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblEnte" CssClass="label" AssociatedControlID="txtEnte" runat="server" Text="Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtEnte" CssClass="textbox" autofocus="true" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodEnte" CssClass="label" AssociatedControlID="txtCodEnte" runat="server" Text="Cod. Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodEnte" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCognome" CssClass="label" AssociatedControlID="txtCognome" runat="server" Text="Cognome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCognome" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblNome" CssClass="label" AssociatedControlID="txtNome" runat="server" Text="Nome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtNome" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblProgetto" CssClass="label" AssociatedControlID="txtProgetto" runat="server" Text="Progetto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtProgetto" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodProgetto" CssClass="label" AssociatedControlID="txtCodProgetto" runat="server" Text="Cod. Progetto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodProgetto" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row" id="divDataRiferimento" runat="server">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lbldal" CssClass="label" AssociatedControlID="txtdatadal" runat="server" Text="Data Riferimento Dal"></asp:Label>
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtdatadal" Width="75px"  CssClass="textbox" MaxLength="10" runat="server"></asp:TextBox>         
                </div> 
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblAl" CssClass="label" AssociatedControlID="txtdataal" runat="server" Text="Data Riferimento Al"></asp:Label>
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtdataal" Width="75px"  CssClass="textbox" MaxLength="10" runat="server"></asp:TextBox>         
                </div>    
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
		            <asp:Label ID="lblCodVolontario" CssClass="label" AssociatedControlID="TxtCodVolontario" runat="server" Text="Cod. Volontario"></asp:Label>
	            </div>
	            <div class="colOggetti" style="width:35%">       
		            <asp:TextBox ID="TxtCodVolontario" CssClass="textbox" runat="server"></asp:TextBox>         
	            </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblStato" CssClass="label" AssociatedControlID="ddlstato" runat="server" Text="Stato"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="ddlstato" runat="server" CssClass="ddlClass"></asp:DropDownList>
               </div>
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
                <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
		        <br />
	            <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
              &nbsp;
            </div>
        </div>
        <br />
        <h3>
            <asp:label id="lblmessaggio" runat="server" AssociatedControlID="dgVolontari"></asp:label>
        </h3>
        <asp:datagrid id="dgVolontari" runat="server" Width="100%" ToolTip="Elenco Volontari" CssClass="table" CellPadding="2"  AllowPaging="True" PageSize="10" AllowSorting="True" AutoGenerateColumns="False"  Visible="False" UseAccessibleHeader="True">
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
				<asp:BoundColumn Visible="False" DataField="IdEntit&#224;" HeaderText="IdEntit&#224;"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="IdAttivit&#224;" HeaderText="IdAttivit&#224;"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="IdEnte" HeaderText="IdEnte"></asp:BoundColumn>
                 <asp:TemplateColumn HeaderText="Selez."> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImageButton1" CommandName="Select" ToolTip="Inserisci Rimborso" AlternateText="Inserisci Rimborso" runat="server" ImageURL="images/Icona_Volontario_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="Codicevolontario" HeaderText="Codice Volontario"></asp:BoundColumn>
				<asp:BoundColumn DataField="nominativo" HeaderText="Nominativo">
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="CodiceFiscale" HeaderText="Cod. Fiscale"></asp:BoundColumn>
				<asp:BoundColumn DataField="datanascita" HeaderText="Data Nascita" DataFormatString="{0:d}">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
					<ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="ComuneNascita" HeaderText="Comune Nascita">
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Progetto" HeaderText="Progetto"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="SedeAttuazione" HeaderText="Sede Attuazione"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="Ente" HeaderText="Ente"></asp:BoundColumn>
				<asp:BoundColumn DataField="TotRimborsi" HeaderText="tot. Rimborsi">
					<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="NConfermati" HeaderText="Rimborsi Confermati">
					<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Nrespinti" HeaderText="Rimborsi Respinti">
					<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="nProposti" HeaderText="Rimborsi Proposti">
					<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
    </fieldset>
</asp:Content>
