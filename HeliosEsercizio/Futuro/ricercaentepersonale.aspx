<%@ Page Title="Ricerca Risorse Ente" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ricercaentepersonale.aspx.vb" Inherits="Futuro.ricercaentepersonale"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Ricerca Risorse Ente</legend>
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitoloPrincipale" runat="server"  Text="Ricerca Risorse Ente"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
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
                    <asp:Label ID="lblRuolo" CssClass="label" AssociatedControlID="ddlRuolo" runat="server" Text="Ruolo"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlRuolo" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblPosizione" CssClass="label" AssociatedControlID="txtPosizione" runat="server" Text="Posizione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtPosizione" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
             <div class="row" >
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblTitolo" CssClass="label" AssociatedControlID="txtTitolo" runat="server" Text="Titolo"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtTitolo" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="Label1" CssClass="lblStato" AssociatedControlID="ddlStato" runat="server" Text="Stato"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlStato" runat="server" CssClass="ddlClass">
                        <asp:ListItem Value="0" Selected="True">Selezionare</asp:ListItem>
						<asp:ListItem Value="1">Attiva</asp:ListItem>
						<asp:ListItem Value="2">Cancellata</asp:ListItem>
                    </asp:DropDownList>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblStatoAccreditamento" CssClass="label" AssociatedControlID="ddlStatoAccr" runat="server" Text="Stato Accr."></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlStatoAccr" runat="server" CssClass="ddlClass">
                        <asp:ListItem Value="10" Selected="True">Selezionare</asp:ListItem>
						<asp:ListItem Value="-1">Chiuso</asp:ListItem>
						<asp:ListItem Value="0">Da Valutare</asp:ListItem>
						<asp:ListItem Value="1">Accreditato</asp:ListItem>
                    </asp:DropDownList>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodiceFiscale" CssClass="label" AssociatedControlID="txtCodFis" runat="server" Text="Codice Fiscale"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodFis" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row" id="divFiltroFase" runat="server" visible="false">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblFiltroFase" CssClass="label" AssociatedControlID="txtFiltroFase" runat="server" Text="Rif. Fase"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtFiltroFase" CssClass="textbox" runat="server"></asp:TextBox>
                    <asp:Label ID="lblErroreFiltroFase" CssClass="msgErrore" Text="Inserire un valore numerico." Visible="false" runat="server" />
                </div>
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />&nbsp;
                <asp:Button ID="CmdEsportaPersonale" runat="server" CssClass="Pulsante" Text="Esporta Risorse CSV" Visible="False" />
                <asp:Button ID="CmdEsportaServizi" runat="server" CssClass="Pulsante" Text="Esporta Servizi CSV" Visible="False" />
		        <br />
	            <asp:HyperLink ID="ApriCSV1Personale" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca risorse" CssClass="linkStampa" Text="DOWNLOAD RISORSE CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>&nbsp;
                <asp:HyperLink ID="ApriCSV1Servizi" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca servizi" CssClass="linkStampa" Text="DOWNLOAD SERVIZI CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>&nbsp;
            </div>
        </div>
        <br />
        <br />
        <br />
        <h3>
            <asp:label id="lblPersonale" runat="server" Text="" AssociatedControlID="dtgRisultatoRicerca"></asp:label>
        </h3>
        <asp:datagrid id="dtgRisultatoRicerca" runat="server" Width="100%"  Caption="RISORSE ENTE" ToolTip="Elenco Risorse Ente" CssClass="table" CellPadding="2"  AllowPaging="True" PageSize="10" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
                <asp:TemplateColumn HeaderText="Selez."> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgVolontario" CommandName="Select" ToolTip="Seleziona Nominativo" AlternateText="Seleziona Nominativo" runat="server" ImageURL="images/Icona_Volontario_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="IDEntePersonale" HeaderText="IDPersonale"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="IDEnte" HeaderText="IDEnte"></asp:BoundColumn>
				<asp:BoundColumn DataField="Nominativo" HeaderText="Nominativo"></asp:BoundColumn>
				<asp:BoundColumn DataField="Ruolo" HeaderText="Ruolo">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Posizione" HeaderText="Posizione">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Accreditato" HeaderText="Stato Accr.">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Tipologia" HeaderText="Tipologia">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="Approvato" HeaderText="Approvato">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="IDPersonaleAcquisito" HeaderText="Acquisito"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="Denominazione" HeaderText="Denominazione"></asp:BoundColumn>
				<asp:BoundColumn DataField="Stato" HeaderText="Stato">
					<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="UsernameInseritore" HeaderText="UsernameInseritore"></asp:BoundColumn>
				<asp:BoundColumn ItemStyle-HorizontalAlign="Center" Visible="True" DataField="DataInseritore" HeaderText="Data Inserimento" DataFormatString="{0:d}"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="IdRuolo" HeaderText="IDRuolo"></asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
        <br />
        
            <asp:label id="lblServizi" runat="server" Text="" AssociatedControlID="dgServizi"></asp:label>
        <br />
        <asp:datagrid id="dgServizi" runat="server" Width="100%" Caption="SERVIZI" ToolTip="Elenco Servizi" CssClass="table" CellPadding="2" AllowSorting="True" AllowPaging="true" PageSize="5" AutoGenerateColumns="False" UseAccessibleHeader="True" DataKeyField="IDEnteAcquisizione">
			<SelectedItemStyle  BackColor="White" ></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
			<Columns>
                <asp:TemplateColumn HeaderText="Dettaglio"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgServizio" CommandName="Seleziona" ToolTip="Seleziona Servizio" AlternateText="Seleziona Servizio" runat="server" ImageURL="images/lenteIngrandimento_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="IDsistema" HeaderText="IDSistema">
					<HeaderStyle Width="6%"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Sistema" HeaderText="Servizi Acquisiti"></asp:BoundColumn>
				<asp:BoundColumn DataField="Denominazione" HeaderText="Ente"></asp:BoundColumn>
				<asp:BoundColumn DataField="Codiceregione" HeaderText="Codice Ente"></asp:BoundColumn>
				<asp:BoundColumn DataField="Stato" HeaderText="Stato"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Elimina/Annulla">
                    <ItemTemplate>
                         <asp:ImageButton ID="ImgElimina" CommandName="Cancella" ToolTip="Cancella" AlternateText="Cancella" runat="server" ImageURL="~/Images/canc_small.png" CausesValidation="false"></asp:ImageButton>   
                         <asp:ImageButton ID="ImgAnnulla" CommandName="Annulla" ToolTip="Annulla Cancellazione" AlternateText="Annulla Cancellazione" runat="server" ImageURL="~/Images/cronologia_small.png" CausesValidation="false"></asp:ImageButton> 
                    </ItemTemplate>
                </asp:TemplateColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
    </fieldset>
</asp:Content>
