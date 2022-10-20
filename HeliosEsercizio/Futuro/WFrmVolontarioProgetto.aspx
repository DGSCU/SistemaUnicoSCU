<%@ Page Title="Associa Graduatoria Volontario" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WFrmVolontarioProgetto.aspx.vb" Inherits="Futuro.WFrmVolontarioProgetto" %>
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
            var DataInizio = CostruisciId('TxtDataInizio');
            var readonly = '<%=TxtDataInizio.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataInizio = "#" + DataInizio
                $("" + sharpDataInizio + "").datepicker();
            }
        });

        $(function () {
            var DataFine = CostruisciId('txtDataFine');
            var readonly = '<%=txtDataFine.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataFine = "#" + DataFine
                $("" + sharpDataFine + "").datepicker();
            }
        });

        function Conferma() {

            msg = confirm("Si vuole procedere con l'operazione?")
            if (msg == false) {
                return false;
            }

            var prefissoCodReg = CostruisciId("txtcodreg");
            var codReg = document.getElementById(prefissoCodReg);
            if (codReg.value == 'True') {
                alert('Attenzione! Si sta avviando al servizio il volontario presso altro ente. Sarà pertanto aggiornata anche la sede di primo giorno con la sede di servizio selezionata.')

            }
        }

    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Assegna Volontari</legend>
        <asp:label id="LblErrore"  runat="server" CssClass="msgErrore"></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitDatiVolontario" runat="server"  Text="Dati del Volontario"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:10%">
                    <asp:Label ID="lblCodiceFiscale" CssClass="label" AssociatedControlID="LblCF" runat="server" Text="Codice Fiscale"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:23%">       
                    <asp:label id="LblCF" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:10%">
                    <asp:Label ID="lblCognomeLabel" CssClass="label" AssociatedControlID="LblCognome" runat="server" Text="Cognome"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:23%">       
                    <asp:label id="LblCognome" CssClass="bold" runat="server"></asp:label>         
                </div>
                 <div class="collable" style="width:10%">
                    <asp:Label ID="lblNomeLabel" CssClass="label" AssociatedControlID="LblNome" runat="server" Text="Nome"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:23%">       
                    <asp:label id="LblNome" CssClass="bold" runat="server"></asp:label>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDataNascita" CssClass="label" AssociatedControlID="LblData" runat="server" Text="Data di Nascita"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="LblData" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblLuogoNascita" CssClass="label" AssociatedControlID="LblLuogo" runat="server" Text="Luogo di Nascita"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="LblLuogo" CssClass="bold" runat="server"></asp:label>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDataInizioServizio" CssClass="label" AssociatedControlID="TxtDataInizio" runat="server" Text="Data Inizio Servizio"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:textbox id="TxtDataInizio" runat="server" Width="75px"  CssClass="textbox" MaxLength="10"></asp:textbox>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblTipoDiPosto" CssClass="label" AssociatedControlID="CboTipoPosto" runat="server" Text="Tipo di Posto"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="CboTipoPosto" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDataFine" CssClass="label" AssociatedControlID="txtDataFine" runat="server" Text="Data Fine Servizio" Visible="false"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:textbox id="txtDataFine" runat="server" Width="75px"  CssClass="textbox" MaxLength="10" Visible="false"></asp:textbox>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblCodiceVolontario" CssClass="label" AssociatedControlID="txtCodiceVolontario" runat="server" Text="Codice Volontario" Visible="false"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:textbox id="txtCodiceVolontario" runat="server" CssClass="textbox" Visible="false"></asp:textbox>         
                </div>
            </div>
        </div>
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitRicercaProgetti" runat="server"  Text="Ricerca Progetti - Sedi"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblCodiceEnte" CssClass="label" AssociatedControlID="TxtCodiceEnte" runat="server" Text="Codice Ente"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:textbox id="TxtCodiceEnte" runat="server" CssClass="textbox"></asp:textbox>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDenominazione" CssClass="label" AssociatedControlID="TxtDescEnte" runat="server" Text="Denominazione"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:textbox id="TxtDescEnte" runat="server" CssClass="textbox"></asp:textbox>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblCodiceProgetto" CssClass="label" AssociatedControlID="TxtCodiceProgetto" runat="server" Text="Codice Progetto"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:textbox id="TxtCodiceProgetto" runat="server" CssClass="textbox"></asp:textbox>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblTitolo" CssClass="label" AssociatedControlID="TxtDescProgetto" runat="server" Text="Titolo"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:textbox id="TxtDescProgetto" runat="server" CssClass="textbox"></asp:textbox>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblCodiceSede" CssClass="label" AssociatedControlID="TxtCodiceSede" runat="server" Text="Codice Sede"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:textbox id="TxtCodiceSede" runat="server" CssClass="textbox"></asp:textbox>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblComune" CssClass="label" AssociatedControlID="TxtComune" runat="server" Text="Comune"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:textbox id="TxtComune" runat="server" CssClass="textbox"></asp:textbox>         
                </div>
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
            </div>
        </div>
        <br />
        <h3>
            <asp:label id="lblRicercaProgetti" runat="server" Text="Risultato Ricerca Progetti"></asp:label>
        </h3>
        <asp:datagrid id="DtgProgetti" runat="server" CssClass="table"  Width="100%" ToolTip="Elenco Progetti"  CellPadding="2"  AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" PageSize="5" UseAccessibleHeader="True">
			<SelectedItemStyle BackColor="White" Font-Bold="true"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
                <asp:TemplateColumn> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImageButton1" CommandName="Select" ToolTip="Seleziona Progetto" AlternateText="Seleziona Progetto" runat="server" ImageURL="images/valida_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="IdAttivita" HeaderText="IdAttivita"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="IdAttivitaEnteSedeAttuazione" HeaderText="IdAttivitaEnteSedeAttuazione"></asp:BoundColumn>
				<asp:BoundColumn DataField="CodiceEnte" HeaderText="Codice"></asp:BoundColumn>
				<asp:BoundColumn DataField="Titolo" HeaderText="Titolo"></asp:BoundColumn>
				<asp:BoundColumn DataField="DataInizioAttivita" HeaderText="Data Inizio" DataFormatString="{0:d}"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="DataFineAttivita" HeaderText="DataFineAttivita" DataFormatString="{0:d}"></asp:BoundColumn>
				<asp:BoundColumn DataField="Comune" HeaderText="Comune"></asp:BoundColumn>
				<asp:BoundColumn DataField="Provincia" HeaderText="Provincia"></asp:BoundColumn>
				<asp:BoundColumn DataField="Indirizzo" HeaderText="Indirizzo"></asp:BoundColumn>
				<asp:BoundColumn DataField="IdEnteSedeAttuazione" HeaderText="Sede"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="Posti" HeaderText="Posti"></asp:BoundColumn>
				<asp:BoundColumn DataField="PostiPrevistiSede" HeaderText="Posti Previsti Sede"></asp:BoundColumn>
				<asp:BoundColumn DataField="PostiOccupati" HeaderText="Posti Occupati"></asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
        <div class="RigaPulsanti" style="text-align:right">
	        <asp:Button ID="cmdAssocia" runat="server" CssClass="Pulsante" Text="Assegna" OnClientClick="return Conferma()"/>
	        <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
        </div>
        <asp:HiddenField ID="txtLOTUS" runat="server"/>
        <asp:HiddenField ID="HdyIdAttivita" runat="server" />
        <asp:HiddenField ID="HdyIdBando" runat="server" />
        <asp:HiddenField ID="txtcodreg" runat="server" />
    </fieldset>
</asp:Content>
