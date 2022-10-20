<%@ Page Title="Inserimento Sanzioni Volontari" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="SanzioniVolontari.aspx.vb" Inherits="Futuro.SanzioniVolontari" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Inserimento Sanzioni Volontari</legend>
        <asp:label id="lblmess"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Gestione Sanzioni Volontari"></asp:Label></h2>
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
            <asp:label id="lblTitotloElencoSanzioni" runat="server" CssClass="bold" Text="Elenco Sanzioni"></asp:label>
        </p>
        <asp:datagrid id="dgRisultatoRicercaAssenze" runat="server" CssClass="table"  Width="100%" ToolTip="Elenco Sanzioni"  CellPadding="2"  AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" PageSize="3" UseAccessibleHeader="True">
			<SelectedItemStyle BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
                <asp:TemplateColumn> 
                <ItemTemplate>
					<asp:ImageButton ID="ImageButton1" CommandName="Modifica" ToolTip="Seleziona Sanzione" AlternateText="Seleziona Sanzione" runat="server" ImageURL="images/valida_small.png" CausesValidation="false"></asp:ImageButton>                         
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
				<asp:BoundColumn Visible="False" DataField="DataConferma" HeaderText="Data di Conferma">
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
                <asp:TemplateColumn> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImageButton2" CommandName="Respingi" ToolTip="Rifiuta Sanzione" AlternateText="Rifiuta Sanzione" runat="server" ImageURL="images/canc_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="IdCausale" HeaderText="IdCausale"></asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
        <br />
        <div class="wrapper" style="width:100%">
            <div class="RigaVuota" style="text-align:center">
                <asp:label id="lblInserimentoSanzioni" runat="server" CssClass="bold" Text="Inserimento Sanzioni"></asp:label>
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
                    <asp:dropdownlist id="ddlCausale" runat="server" CssClass="ddlClass"></asp:dropdownlist>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:label id="lblGiorni" CssClass="label" runat="server" AssociatedControlID="txtNumGiorni" Text="(*)N° Giorni"></asp:label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:textbox id="txtNumGiorni" runat="server" CssClass="textbox" MaxLength="2" ></asp:textbox>
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
                <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
            </div>
            <asp:HiddenField id="txtcontrollomese" runat="server" />
            <asp:HiddenField id="Txttot" runat="server" />
            <asp:HiddenField id="Txtmodprece" runat="server" />
            <asp:HiddenField id="txtidentitaassenza" runat="server" />
        </div>
    </fieldset>
</asp:Content>
