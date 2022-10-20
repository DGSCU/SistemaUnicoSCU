<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmConsultazionePresentazioniProgrammi.aspx.vb" Inherits="Futuro.WfrmConsultazionePresentazioniProgrammi" %>
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
        <legend >Consultazione Protocolli Istanze Programmi</legend>
        <div class="wrapper" style="width:100%">
            <div class="headers">
                <h2>
                    <asp:Label ID="lblTitolo" runat="server"  Text="Consultazione Protocolli Istanze Programmi"></asp:Label>
                </h2>
            </div>
            <asp:Label ID="lblMessaggio" runat="server" CssClass="msgInfo"></asp:Label>
            <div class="RigaVuota">&nbsp;</div>
            <div class="row">
				<div class="collable" style="width:15%">
				&nbsp;<asp:Label ID="lblCodEnte" AssociatedControlID="txtCodEnte" runat="server" Text="Codice Ente"></asp:Label>
				</div>
				<div class="colOggetti" style="width:35%">
					<asp:textbox id="txtCodEnte" runat="server" MaxLength="100"></asp:textbox>
				</div>
				<div class="collable" style="width:15%">
                    <asp:Label ID="lblEnte" AssociatedControlID="txtDenominazioneEnte" runat="server" Text="Ente"></asp:Label>
                </div>
				<div class="colOggetti" style="width:35%">    
				    <asp:textbox id="txtDenominazioneEnte" runat="server" MaxLength="100"></asp:textbox>
                </div>
            </div>
			<div class="row" >
				<div class="collable" style="width:15%">
				&nbsp;<asp:Label ID="lblBando" AssociatedControlID="DdlBando" runat="server" Text="Avviso"></asp:Label>
                </div>
			    <div class="colOggetti" style="width:35%">
				    <asp:dropdownlist id="DdlBando" runat="server" AutoPostBack="False"></asp:dropdownlist>
			    </div>
            </div>
                <div class="row">
                    <div class="collable" style="width:15%">      
                    &nbsp;<asp:Label ID="lblDataInizio" CssClass="label" AssociatedControlID="txtDataInizio" runat="server" Text="Data presentazione inizio"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtDataInizio" CssClass="textbox" runat="server" Width="75px" MaxLength="10"></asp:TextBox>         
                    </div> 
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblDataFine" CssClass="label" AssociatedControlID="txtDataFine" runat="server" Text="Data presentazione fine"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtDataFine" CssClass="textbox" runat="server" Width="75px" MaxLength="10"></asp:TextBox>         
                    </div>
                </div>

            <div class="RigaPulsanti">
            
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
            </div>
        </div>
    </fieldset>
    <fieldset class="ContornoPagina">
        <legend>ELENCO PROTOCOLLI</legend>
        <div id="divRicerche" runat="server" visible="false">
            <asp:datagrid id="dtgElencoPresentazioni" CssClass="table" runat="server"  Caption="ELENCO PROTOCOLLI" Width="100%"  ToolTip="Elenco protocolli"  AutoGenerateColumns="False" UseAccessibleHeader="True"  AllowSorting="True"  CellPadding="3" AllowPaging="True">
	            <FooterStyle ></FooterStyle>
	            <SelectedItemStyle ></SelectedItemStyle>
	            <EditItemStyle ></EditItemStyle>
	            <AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
	            <ItemStyle CssClass="tr"></ItemStyle>
	            <HeaderStyle ></HeaderStyle>
	            <Columns>
				    <asp:BoundColumn DataField="Avviso" HeaderText="Avviso"><ItemStyle HorizontalAlign="Center"></ItemStyle></asp:BoundColumn>
				    <asp:BoundColumn DataField="Codice Ente" HeaderText="Codice Ente"><ItemStyle HorizontalAlign="Center"></ItemStyle></asp:BoundColumn>
				    <asp:BoundColumn DataField="Denominazione Ente" HeaderText="Denominazione Ente"><ItemStyle HorizontalAlign="Center"></ItemStyle></asp:BoundColumn>
				    <asp:BoundColumn DataField="Data presentazione" HeaderText="Data Presentazione"><ItemStyle HorizontalAlign="Center"></ItemStyle></asp:BoundColumn>
				    <asp:BoundColumn DataField="Data protocollazione presentazione" HeaderText="Data Protocollazione presentazione"><ItemStyle HorizontalAlign="Center"></ItemStyle></asp:BoundColumn>
				    <asp:BoundColumn DataField="N. protocollo presentazione" HeaderText="N. protocollo presentazione"><ItemStyle HorizontalAlign="Center"></ItemStyle></asp:BoundColumn>
				    <asp:BoundColumn DataField="Data annullamento" HeaderText="Data annullamento"><ItemStyle HorizontalAlign="Center"></ItemStyle></asp:BoundColumn>
				    <asp:BoundColumn DataField="Data protocollazione annullamento" HeaderText="Data protocollazione annullamento"><ItemStyle HorizontalAlign="Center"></ItemStyle></asp:BoundColumn>
				    <asp:BoundColumn DataField="N. protocollo annullamento" HeaderText="N. protocollo annullamento"><ItemStyle HorizontalAlign="Center"></ItemStyle></asp:BoundColumn>
	            </Columns>
		        <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
            </asp:datagrid>
        </div>
        <div id="divNoRicerche" runat="server" visible="true">
            <asp:Label ID="lblNoResult" CssClass="msgInfo" runat="server" Text="Nessuna ricerca effettuata"></asp:Label>
        </div>
</fieldset>
</asp:Content>
