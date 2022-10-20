<%@ Page Title="Gestione Rimborsi Volontari" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmGestioneRimborsoVolontari.aspx.vb" Inherits="Futuro.WfrmGestioneRimborsoVolontari" %>
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

            var DataRiferimento = CostruisciId('txtdataRiferimento');
            var readonly = '<%=txtdataRiferimento.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataRiferimento = "#" + DataRiferimento
                $("" + sharpDataRiferimento + "").datepicker();
            }

        });

        function infoPrefissi() {
            window.open("WfrmInfoPrefissiDocumentiRimborsi.aspx", "Elenco", "height=450,width=600,dependent=no,scrollbars=yes,status=no");
        }

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Gestione Rimborsi Volontari</legend>
        <asp:label id="lblmess"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Gestione Rimborsi Volontari"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblStatoVolontario" CssClass="label" AssociatedControlID="lblStato" runat="server" Text="Stato Volontario"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblStato" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodiceVolontarioLabel" CssClass="label" AssociatedControlID="lblCodiceVolontario" runat="server" Text="Codice Volontario"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblCodiceVolontario" CssClass="bold" runat="server"></asp:label>         
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
                    <asp:Label ID="lblDataInizioLabel" CssClass="label" AssociatedControlID="lblDataInizio" runat="server" Text="Data Inizio Servizio"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblDataInizio" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDataFineLabel" CssClass="label" AssociatedControlID="lbldataFine" runat="server" Text="Data Fine Servizio"></asp:Label>   
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
				<asp:BoundColumn DataField="img" Visible="false">
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
            <asp:label id="lblTitotloElencoVolontari" runat="server" CssClass="bold" Text="Elenco Richieste Rimborsi"></asp:label>
        </p>
        <asp:datagrid id="dgRisultatoRicercaRimborsi" runat="server" CssClass="table"  Width="100%" ToolTip="Elenco Rimborsi"  CellPadding="2"  AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" PageSize="3" UseAccessibleHeader="True">
			<SelectedItemStyle BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>    
                <asp:TemplateColumn HeaderText="Seleziona"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImageButton1" CommandName="Modifica" ToolTip="Seleziona Rimborso" AlternateText="Seleziona Risorsa" runat="server" ImageURL="images/Icona_Volontario_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn DataField="dataRiferimento" HeaderText="Data Riferimento" DataFormatString="{0:d}">
					<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Causale" HeaderText="Causale">
					<HeaderStyle HorizontalAlign="Center" Width="30%" VerticalAlign="Middle"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Importo" HeaderText="Importo">
					<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="importoConfermato" HeaderText="Importo Confermato">
					<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Note" HeaderText="Note">
					<HeaderStyle HorizontalAlign="Center" Width="20%" VerticalAlign="Middle"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Stato" HeaderText="Stato">
					<HeaderStyle HorizontalAlign="Center" Width="8%" VerticalAlign="Middle"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="identit&#224;" HeaderText="identit&#224;"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Conferma"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImageButton2" CommandName="Conferma" ToolTip="Accetta Richiesta" AlternateText="Accetta Richiesta" runat="server" ImageURL="images/valida_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Respingi"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImageButton3" CommandName="Respingi" ToolTip="Rifiuta Richiesta" AlternateText="Rifiuta Richiesta" runat="server" ImageURL="images/canc_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="idRimborso" HeaderText="idRimborso"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="idcausale" HeaderText="idCausale"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="idAttivit&#224;Rimborso" HeaderText="idAttivit&#224;Rimborso"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Scarica">
                    <ItemTemplate>
                         <asp:ImageButton ID="ImageButton4" CommandName="Scarica" ToolTip="Scarica File" AlternateText="Scarica File" runat="server" ImageURL="~/Images/giu_small.png" CausesValidation="false"></asp:ImageButton>    
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Elimina">
                    <ItemTemplate>
                         <asp:ImageButton ID="ImageButton5" CommandName="Elimina" ToolTip="Elimina Rimborso" AlternateText="Elimina Rimborso" runat="server" ImageURL="~/Images/canc_small.png" CausesValidation="false"></asp:ImageButton>    
                    </ItemTemplate>
                </asp:TemplateColumn>  
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
        <br />
         <p>
            <asp:HyperLink ID="hlDw"  runat="server" Visible="false" onclick="target='_blank';"></asp:HyperLink>
        </p>
        <div class="wrapper" style="width:100%">
            <div class="RigaVuota" style="text-align:center">
                <asp:label id="Label1" runat="server" CssClass="bold" Text="Inserimento Rimborso"></asp:label>   
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDataRiferimento" CssClass="label" AssociatedControlID="txtdataRiferimento" runat="server" Text="(*)Data Riferimento"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:textbox id="txtdataRiferimento" runat="server" Width="75px" CssClass="textbox" MaxLength="10"></asp:textbox>
                </div>
                <div class="collable" style="width:15%">
                    <asp:label id="lblCausale" CssClass="label" runat="server" AssociatedControlID="ddlCausale" Text="(*)Causale"></asp:label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:dropdownlist id="ddlCausale" runat="server"  CssClass="ddlClass"></asp:dropdownlist> 
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">  
                    <asp:Label ID="lblImporto" CssClass="label" AssociatedControlID="txtImporto" runat="server" Text="(*)Importo"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:textbox id="txtImporto" runat="server" Width="80px" CssClass="textbox" MaxLength="10"></asp:textbox>
                    <asp:Label ID="lblImportoEuro" CssClass="label" runat="server" Text="€"></asp:Label>
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblImportoConfermato" CssClass="label" AssociatedControlID="txtImportoConf" runat="server" Text="Importo Confermato"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:textbox id="txtImportoConf" runat="server" Width="80px" CssClass="textbox" MaxLength="10" BackColor="Gainsboro" ReadOnly="true"></asp:textbox>
                    <asp:Label ID="lblImportoConfermatoEuro" CssClass="label" runat="server" Text="€"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:label id="lblNote" runat="server" CssClass="label" AssociatedControlID="txtNote" Text="Note"></asp:label>
                </div>
                <div class="colOggetti" style="width:85%">
                    <asp:textbox id="txtNote" runat="server" TextMode="MultiLine" 
                        CssClass="textbox" Width="96%" Height="58px" ></asp:textbox>
                </div>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div>
                <div style="vertical-align:middle;">
                    <p>
                        <asp:ImageButton  id="imgPrefissiDocumenti"  runat="server" style="cursor:hand;margin-top:5px" ImageUrl="images/info_small.png" OnClientClick="javascript:infoPrefissi()"  ToolTip="Prefissi Documenti" AlternateText="Prefissi Documenti"/>
                        <asp:label ID="LblSel" CssClass="label"  AssociatedControlID="txtSelFile" runat="server" Text="Selezionare il file" ></asp:label>
                        <input ID="txtSelFile" type="file" style="width:47%"   runat="server"  />
                    </p>
                </div>
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdModifica" runat="server" CssClass="Pulsante" Text="Salva" Visible="False" />&nbsp;
                <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" Text="Salva" />&nbsp;
                <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
            </div>
        </div>
        <asp:HiddenField ID="txtIdentita" runat="server" /> 
        <asp:HiddenField ID="txtidattivita" runat="server" />
        <asp:HiddenField ID="txtidentitaRimborso" runat="server" />
    </fieldset>
</asp:Content>
