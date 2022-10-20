<%@ Page Title="Lista Progetti" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmGestioneGraduatoriaProg.aspx.vb" Inherits="Futuro.WfrmGestioneGraduatoriaProg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Lista Progetti</legend>
        <asp:label id="lblMessaggio"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
        <p style="text-align:center">
            <asp:label id="lbltitolo" runat="server" CssClass="bold" AssociatedControlID="dgRisultatoRicerca"></asp:label>
        </p>
        <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco Bandi" CssClass="table" CellPadding="2"  AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ForeColor="Black"></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
				<asp:ButtonColumn Text="" CommandName="Select" Visible="false"></asp:ButtonColumn>
				<asp:BoundColumn Visible="False" DataField="idbando" HeaderText="idbando"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="idente" HeaderText="idEnte">
					<HeaderStyle HorizontalAlign="Center" Width="110px"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Denominazione" HeaderText="Ente">
					<HeaderStyle HorizontalAlign="Center" Width="170px"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="idattivit&#224;" HeaderText="idattivit&#224;">
					<HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Titolo" HeaderText="Progetti">
					<HeaderStyle HorizontalAlign="Left" Width="110px"></HeaderStyle>
					<ItemStyle HorizontalAlign="Left"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Ambito" HeaderText="Settore \ Area Intervento">
					<HeaderStyle Width="170px"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Importo" HeaderText="Importo Progetto">
					<HeaderStyle HorizontalAlign="Right" Width="150px"></HeaderStyle>
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="idgraduatoriaprogetto" HeaderText="idgraduatoriaprogetto"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="Value" HeaderText="Value"></asp:BoundColumn>
				<asp:BoundColumn DataField="punteggiofinale" HeaderText="Punteggio">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="NumVolRic" HeaderText="Vol. Ric.">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn HeaderText="Tot. Vol.">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
				</asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
        <div style="text-align:right">
            <asp:Button ID="CmdConferma" runat="server" CssClass="Pulsante" Text="Conferma"/>
            <asp:Button ID="CmdAnnulla" CssClass="Pulsante" runat="server" Text="Annulla" />
            <asp:Button ID="CmdSalva" runat="server" CssClass="Pulsante" Text="Salva"/>
            <asp:Button ID="CmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
        </div>
    </fieldset>
</asp:Content>
