<%@ Page Title="Verifica Contratti Volontari" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmVerificaContrattoVolontari.aspx.vb" Inherits="Futuro.WfrmVerificaContrattoVolontari" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Verifica Contratti Volontari</legend>
        <asp:label id="lblmsg"  runat="server" CssClass="msgErrore"></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Verifica Contratti Volontari"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblCognome" CssClass="label" AssociatedControlID="txtCognome" runat="server" Text="Cognome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCognome" autofocus="true" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblNome" CssClass="label" AssociatedControlID="txtNome" runat="server" Text="Nome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtNome" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblCodVolontario" CssClass="label" AssociatedControlID="txtCodVolontario" runat="server" Text="Codice Volontario"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodVolontario" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblCodiceFiscale" CssClass="label" AssociatedControlID="txtCodiceFiscale" runat="server" Text="Codice Fiscale"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodiceFiscale" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblNomeEnte" CssClass="label" AssociatedControlID="txtNomeEnte" runat="server" Text="Nome Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:85%">       
                    <asp:TextBox ID="txtNomeEnte" CssClass="textbox" runat="server" Width="96%"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblCodiceEnte" CssClass="label" AssociatedControlID="txtCodiceEnte" runat="server" Text="Codice Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodiceEnte" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblStatoContratto" CssClass="label" AssociatedControlID="ddlStatoContratto" runat="server" Text="Stato Contratto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="ddlStatoContratto" runat="server" CssClass="ddlClass">
                        <asp:ListItem Value="0">Da Caricare</asp:ListItem>
						<asp:ListItem Value="1">Caricato</asp:ListItem>
						<asp:ListItem Value="2">Approvato</asp:ListItem>
						<asp:ListItem Value="3">Respinto</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblScaduti" CssClass="label" AssociatedControlID="ddlTerminiScaduti" runat="server" Text="Termini Scaduti"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="ddlTerminiScaduti" runat="server" CssClass="ddlClass">
                        <asp:ListItem Value="2">Selezionare</asp:ListItem>
						<asp:ListItem Value="1">Non Scaduto</asp:ListItem>
						<asp:ListItem Value="0">Scaduto</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />
            </div>
        </div>
        <br />
        <h3>
            <asp:label id="lblTitotoloElencoContratti" runat="server" Text="Elenco Contratti Volontari"></asp:label>
        </h3>
        <asp:datagrid id="dtgElencoContratti" runat="server" Width="100%" ToolTip="Elenco associazioni esistenti" CssClass="table" CellPadding="2"  AllowPaging="True" PageSize="5" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
                <asp:TemplateColumn HeaderText="Download"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgbtnDownload" CommandName="Download" ToolTip="Download" AlternateText="Download" runat="server" ImageURL="images/giu_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn DataField="CodiceVolontario" HeaderText="Codice Volontario">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Nominativo" HeaderText="Nomivativo">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Ente" HeaderText="Ente">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="DataInizioServizio" HeaderText="Data Inizio Servizio">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="DataFineServizio" HeaderText="Data Fine Servizio">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="StatoEntit&#224;" HeaderText="Stato Volontario">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="StatoContrattoVolontario" HeaderText="Stato Contratto">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="RiferimentoContrattoVolontario" HeaderText="RiferimentoContrattoVolontario">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="DataProrogaContratto" HeaderText="Data Proroga">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				</asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Proroga"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgbtnProroga" CommandName="Proroga" ToolTip="Proroga Contratto" AlternateText="Proroga Contratto" runat="server" ImageURL="images/proroga_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Approva"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgbtnApprova" CommandName="Approva" ToolTip="Approvazione Contratto" AlternateText="Approvazione Contratto" runat="server" ImageURL="images/valida_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Respingi"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgbtnRespingi" CommandName="Cancella" ToolTip="Contratto Respinto" AlternateText="Contratto Respinto" runat="server" ImageURL="images/canc_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="IDEntit&#224;"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="TerminiScadenza"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="NomeFileContratto"></asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
        <div id="divDownload" class="wrapper" style="width:100%; border-style:none" runat="server" visible="false">
            <div class="RigaVuota" >&nbsp;</div>
             <div class="row">
                <div class="collable" style="width:30%">      
                    <asp:Label ID="lblTestoUP" CssClass="label" AssociatedControlID="hlScarica" runat="server" Text="Scaricare il file  cliccando sul nome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:70%">       
                    <asp:hyperlink id="hlScarica" runat="server" CssClass="colHyperLink" Target="_blank"></asp:hyperlink>         
                </div>
            </div>
        </div>
        <div class="RigaPulsanti" style="text-align:right">
            <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
        </div>
    </fieldset>
</asp:Content>
