<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmCOMPSommeRestituite.aspx.vb" Inherits="Futuro.WfrmCOMPSommeRestituite" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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

        <script  type="text/javascript">
            $(function () {
                var IdData = CostruisciId('TxtDataRiferimento');
                var sharpIdData = "#" + IdData
                $("" + sharpIdData + "").datepicker();
            });
           
            $(function () {
                var IdData = CostruisciId('TxtDataDomanda');
                var sharpIdData = "#" + IdData
                var IdVolontario = '<%= Request.Params("IdVol")%>';
                if (IdVolontario == "") {
                    $("" + sharpIdData + "").datepicker();
                }
            });

        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <fieldset class="ContornoPagina">
        <legend><span id="MainContent_lblTitolo">Somme Restituite</span></legend>
        <asp:label id="lblmess"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="GESTIONE SOMME RESTITUITE"></asp:Label></h2>
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
                    <asp:Label ID="lblCodiceFiscale" CssClass="label" AssociatedControlID="lblCodFis" runat="server" Text="Codice Fiscale"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblCodFis" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblIban" CssClass="label" AssociatedControlID="lblIbanVol" runat="server" Text="IBAN"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblIbanVol" CssClass="bold" runat="server"></asp:label>         
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
        <asp:label id="lblTitotloElencoRestituzione" runat="server" CssClass="bold" Text="Elenco Somme Restituite"></asp:label>
    </p>
    <asp:datagrid id="dgRisultatoRicercaRestituzioni" runat="server" CssClass="table"  
            Width="100%" ToolTip="Elenco Restituzioni"  CellPadding="2"  
            AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" PageSize="5" 
            UseAccessibleHeader="True">
        <SelectedItemStyle BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
		<Columns>
            <asp:TemplateColumn> 
                <ItemTemplate>
					<asp:ImageButton ID="CmdModifica" CommandName="Modifica" ToolTip="Seleziona Restituzione" AlternateText="Seleziona Restituzione" runat="server" ImageURL="images/valida_small.png" CausesValidation="false"></asp:ImageButton>                         
                </ItemTemplate>
            </asp:TemplateColumn>
			<asp:BoundColumn Visible="False" DataField="idSommaRestituita" HeaderText="idSommaRestituita"
				DataFormatString="{0:d}">
				<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
            <asp:BoundColumn Visible="False" DataField="idEntità" HeaderText="idEntità"
				DataFormatString="{0:d}">
				<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Importo" HeaderText="Importo">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Descrizione" HeaderText="Descrizione">
				<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="DataRiferimento" HeaderText="Data Riferimento" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
			<asp:BoundColumn DataField="UserNameCreazioneRecord" HeaderText="USERNAME">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>    
			<asp:BoundColumn DataField="DataCreazioneRecord" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data Ultima Modifica">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:ButtonColumn Visible="False" Text="&lt;img src=images/mini_conferma.jpg Width=20 Height=20 title='Conferma Restituiti' border=0&gt;"
				HeaderText="Conferma" CommandName="Conferma">
				<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:ButtonColumn>
            <asp:TemplateColumn> 
                <ItemTemplate>
					<asp:ImageButton ID="CmdElimina" CommandName="Elimina" ToolTip="Elimina Restituzione" AlternateText="Elimina Restituzione" runat="server" ImageURL="images/canc_small.png" CausesValidation="false"></asp:ImageButton>                         
                </ItemTemplate>
            </asp:TemplateColumn>
			
		</Columns>
		<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
    <br />
    <div class="wrapper" style="width:100%">
        <div class="RigaVuota" style="text-align:center">
            <asp:label id="lblInserimentoRestituzione" runat="server" CssClass="bold" Text="Dettaglio Somma Restituita"></asp:label>
        </div>
        <div class="RigaVuota" >&nbsp;</div>
        <div class="row" >
            <div class="collable" style="width:15%">      
                <asp:Label ID="lblImporto" CssClass="label" AssociatedControlID="txtImporto" runat="server" Text="(*)Importo"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">       
                <asp:TextBox ID="txtImporto" runat="server" MaxLength="10"></asp:TextBox>
            </div>
            <div class="collable" style="width:15%">
                <asp:label id="lblDescrizione" CssClass="label" runat="server" AssociatedControlID="txtDescrizione" Text="(*)Descrizione"></asp:label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:textbox id="txtDescrizione" runat="server" TextMode="MultiLine" CssClass="textbox" Width="96%"></asp:textbox>
            </div>
        </div>
         <div class="row" >
            <div class="collable" style="width:15%">      
                <asp:Label ID="lblDataRiferimento" CssClass="label" AssociatedControlID="txtDataRiferimento" 
                    runat="server" Text="(*)DataRiferimento"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">       
                <asp:TextBox ID="TxtDataRiferimento" runat="server" MaxLength="10" Enabled="True" CssClass="textboxData"></asp:TextBox>
            </div>

<asp:HiddenField ID="HdnIdSommaRestituita" runat="server" />
        </div>
        
      
        <div class="RigaPulsanti">
            <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" Text="Inserisci" />&nbsp;
             <asp:Button ID="cmdModifica" CssClass="Pulsante" Visible="false" runat="server" Text="Modifica" />&nbsp;
              <asp:Button ID="CmdAnnulla" runat="server" CssClass="Pulsante" Text="Annulla" />&nbsp;
            <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
        </div>
    </div>
    </fieldset>
</asp:Content>
