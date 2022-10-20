<%@ Page Title="Ricerca Risorse da Valutare" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRisorsedaAccreditare.aspx.vb" Inherits="Futuro.WfrmRisorsedaAccreditare" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function MostraRichieste() {            
            return true;
        };
</script></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Ricerca Risorse da Valutare</legend>
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Risorse da Valutare"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDenominazione" CssClass="label" AssociatedControlID="txtdenomina" runat="server" Text="Denominazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtdenomina" CssClass="textbox" runat="server" MaxLength="200"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodiceEnte" CssClass="label" AssociatedControlID="txtCodiceEnte" runat="server" Text="Cod. Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodiceEnte" CssClass="textbox" runat="server" MaxLength="7"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCognome" CssClass="label" AssociatedControlID="txtCognome" runat="server" Text="Cognome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCognome" CssClass="textbox" runat="server" MaxLength="255"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblNome" CssClass="label" AssociatedControlID="txtNome" runat="server" Text="Nome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtNome" CssClass="textbox" runat="server" MaxLength="255"></asp:TextBox>         
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
                    <asp:Label ID="lblTipoRicerca" CssClass="label" AssociatedControlID="ddlTipoRicerca" runat="server" Text="Tipo Ricerca"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlTipoRicerca" runat="server" CssClass="ddlClass"></asp:DropDownList>           
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblRiferimentoFase" CssClass="label" AssociatedControlID="txtIDFaseEnte" runat="server" Text="Rif. Fase"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtIDFaseEnte" CssClass="textbox" runat="server" MaxLength="10"></asp:TextBox>                
                </div>
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
                <asp:Button ID="CmdEsportaRisorse" runat="server" CssClass="Pulsante" Text="Esporta Risorse CSV" Visible="False" />
                <asp:Button ID="CmdEsportaServizi" runat="server" CssClass="Pulsante" Text="Esporta Servizi CSV" Visible="False" />
		        <br />
	            <asp:HyperLink ID="ApriCSV1Risorse" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca risorse" CssClass="linkStampa" Text="DOWNLOAD RISORSE CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>&nbsp;
                <asp:HyperLink ID="ApriCSV1Servizi" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca servizi" CssClass="linkStampa" Text="DOWNLOAD SERVIZI CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>&nbsp;
            </div>
        </div>
        <br />
        <br />
        <br />
        <h3>
            <asp:label id="lblRisorse" runat="server" Text="Risultato Risorse" AssociatedControlID="dtgRisultatoRicerca"></asp:label>
        </h3>
        <asp:datagrid id="dtgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco Risorse" CssClass="table" CellPadding="2"  AllowPaging="True" PageSize="8" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
				<asp:ButtonColumn Visible="False"></asp:ButtonColumn>
				<asp:BoundColumn Visible="false" DataField="Immo"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="IDEnte" HeaderText="IDEnte"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="identepersonale" HeaderText="identepersonale"></asp:BoundColumn>
				<asp:BoundColumn DataField="Nominativo" HeaderText="Nominativo"></asp:BoundColumn>
				<asp:BoundColumn DataField="Ruolo" HeaderText="Ruolo">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Codiceregione" HeaderText="Cod. Ente">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="denominazione" HeaderText="Ente">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Data" HeaderText="Data Inserimento">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="IdEnteFase" HeaderText="Rif. Fase"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Conferma"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgBtnConferma" CommandName="Conferma" ToolTip="Conferma" AlternateText="Conferma" runat="server" ImageURL="images/valida_small.png" CausesValidation="false" OnClientClick="return confirm('Procedere con l\'operazione?');"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Respingi"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgBtnRespingi" CommandName="Respingi" ToolTip="Respingi" AlternateText="Respingi" runat="server" ImageURL="images/canc_small.png" CausesValidation="false" OnClientClick="return confirm('Procedere con l\'operazione?');"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="idruolo" HeaderText="idruolo"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="CodFis" HeaderText="CodFis"></asp:BoundColumn>
			    <asp:TemplateColumn HeaderText="Respinto"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgBtnEsito" CommandName="InfoRespinto" ToolTip="Info Respinto" Visible = '<%#IIF(EVAL("CodFis").Tostring().Equals(""), False, True)%>'
                        AlternateText="InfoRespinto" runat="server" ImageURL="images/info_small.png" CausesValidation="false" 
                        OnClientClick="return MostraRichieste();"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>

			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
        <br />
        <h3>
            <asp:label id="lblServizi" runat="server" Text="Risultato Servizi" AssociatedControlID="dgServizi"></asp:label>
        </h3>
        <asp:datagrid id="dgServizi" runat="server" Width="100%" ToolTip="Elenco Servizi" CssClass="table" CellPadding="2" AllowSorting="True" AllowPaging="true" PageSize="5" AutoGenerateColumns="False" UseAccessibleHeader="True">
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
			<Columns>
				<asp:ButtonColumn Text="" HeaderText="Dettaglio" CommandName="Select" Visible="false">
					<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<FooterStyle HorizontalAlign="Center"></FooterStyle>
				</asp:ButtonColumn>
				<asp:BoundColumn Visible="False" DataField="IDsistema" HeaderText="IDSistema">
					<HeaderStyle Width="6%"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="denominazioneentesecondario" HeaderText="Ente Richiedente"></asp:BoundColumn>
				<asp:BoundColumn DataField="CodReg" HeaderText="Codice Ente Richiedente"></asp:BoundColumn>
				<asp:BoundColumn DataField="Sistema" HeaderText="Servizi Acquisiti">
					<HeaderStyle Font-Size="XX-Small" Font-Names="verdana" HorizontalAlign="Center" Width="50%" VerticalAlign="Middle"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Denominazione" HeaderText="Ente Fornitore"></asp:BoundColumn>
				<asp:BoundColumn DataField="Codiceregione" HeaderText="Codice Ente Fornitore"></asp:BoundColumn>
				<asp:BoundColumn DataField="Stato" HeaderText="Stato"></asp:BoundColumn>
				<asp:ButtonColumn Visible="False" Text="&lt;img src=images/mini_conferma.jpg Width=20 Height=20 onclick=Messaggio() title='Conferma' border=0&gt;"
					HeaderText="Conferma" CommandName="Conferma">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:ButtonColumn>
				<asp:ButtonColumn Visible="False" Text="&lt;img src=images/canc.jpg onclick=Messaggio() Width=20 Height=20 title='Conferma' border=0&gt;"
					HeaderText="Respingi" CommandName="Respingi">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:ButtonColumn>
				<asp:BoundColumn Visible="False" DataField="IdenteAcquisizione" HeaderText="IdenteAcquisizioneServizi"></asp:BoundColumn>
				<asp:BoundColumn DataField="IdEnteFase" HeaderText="Rif.Fase"></asp:BoundColumn>
                 <asp:TemplateColumn HeaderText="Conferma"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgBtnConferma" CommandName="Conferma" ToolTip="Conferma" AlternateText="Conferma" runat="server" ImageURL="images/valida_small.png" CausesValidation="false" OnClientClick="return confirm('Procedere con l\'operazione?');"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Respingi"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgBtnRespingi" CommandName="Respingi" ToolTip="Respingi" AlternateText="Respingi" runat="server" ImageURL="images/canc_small.png" CausesValidation="false" OnClientClick="return confirm('Procedere con l\'operazione?');"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
        <asp:HiddenField ID="txtdenomina1" runat="server" />
        <asp:HiddenField ID="txtCognome1" runat="server" />
        <asp:HiddenField ID="Txtnome1" runat="server" />
    </fieldset>
</asp:Content>
