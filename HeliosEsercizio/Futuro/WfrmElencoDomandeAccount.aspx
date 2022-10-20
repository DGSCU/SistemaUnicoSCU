<%@ Page Title="Elenco Account" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmElencoDomandeAccount.aspx.vb" Inherits="Futuro.WfrmElencoDomandeAccount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Elenco Account</legend>
        <asp:label id="lblMessaggi"  runat="server" CssClass="msgErrore" AssociatedControlID="dgRisultatoRicerca"></asp:label>
        <br />
        <br />     
        <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco Domande Account" CssClass="table" CellPadding="2"  AllowPaging="True" PageSize="5" AllowSorting="True" AutoGenerateColumns="False"  Visible="False" UseAccessibleHeader="True">
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
				<asp:ButtonColumn Text="Sel" Visible="false">
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:ButtonColumn>
                <asp:TemplateColumn HeaderText="Sel"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgInoltra" CommandName="Inoltra" ToolTip="Inoltro Password" AlternateText="Inoltro Password" runat="server" ImageURL="images/valida_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn DataField="Codiceregione" HeaderText="Cod.Ente">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Denominazione" HeaderText="Denominazione"></asp:BoundColumn>
				<asp:BoundColumn DataField="codicefiscale" HeaderText="Cod.Fiscale">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="StatoEnte" HeaderText="Stato"></asp:BoundColumn>
				<asp:BoundColumn DataField="Email" HeaderText="Email"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" HeaderText="Data Richiesta"></asp:BoundColumn>
				<asp:BoundColumn DataField="NoteRichiestaRegistrazione" HeaderText="Richiedente"></asp:BoundColumn>
				<asp:BoundColumn DataField="telefonorichiestaregistrazione" HeaderText="Telefono"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="password" HeaderText="password"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="Username" HeaderText="Usermname"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="IdEnte" HeaderText="IdEnte"></asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
        <div class="RigaPulsanti" style="text-align:right">
            <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
            <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
		    <br />
	        <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>&nbsp;
        </div>
    </fieldset>
    <asp:HiddenField ID="txtstrsql" runat="server" />
</asp:Content>
