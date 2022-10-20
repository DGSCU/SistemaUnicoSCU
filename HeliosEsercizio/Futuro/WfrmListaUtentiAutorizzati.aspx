<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmListaUtentiAutorizzati.aspx.vb" Inherits="Futuro.WfrmListaUtentiAutorizzati" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
 <legend>Riepilogo utenze attive per profilo</legend>
	
 <div class="wrapper" style="width:100%">
		<%--<div class="headers" >
			  <h2><asp:Label ID="lblTitoloPrincipale" runat="server"  Text="Lista Utenti Autorizzati"></asp:Label></h2>
		 </div>
			<br/>--%>
            <div class="RigaPulsanti">
                <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV"/>
		        <br />
	            <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
              &nbsp;
            </div>
        <div class ="row" id="DivGriglia" runat="server">
        <asp:datagrid id="dtgRisultatoRicerca" runat="server" Width="100%"  Caption="Elenco Utenti Autorizzati" ToolTip="Elenco Utenti Autorizzati" CssClass="table" CellPadding="2"  AllowPaging="False" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr" ></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
				<asp:BoundColumn DataField="Nominativo" HeaderText="Nominativo"></asp:BoundColumn>
                <asp:BoundColumn DataField="CF_Persona" HeaderText="Codice Fiscale"></asp:BoundColumn>
                <asp:BoundColumn DataField="TipoUtente" HeaderText="Ruolo"  ></asp:BoundColumn>
				<asp:BoundColumn DataField="CF_Ente" HeaderText="Codice Fiscale Ente"  ></asp:BoundColumn>
                <asp:BoundColumn DataField="CodiceEnte" HeaderText="Cod. Ente" ></asp:BoundColumn>
                <asp:BoundColumn DataField="DenominazioneEnte" HeaderText="Denominazione Ente" ></asp:BoundColumn>
				<asp:BoundColumn DataField="Dettagli" HeaderText="Dettagli" ></asp:BoundColumn>
			</Columns>
		</asp:datagrid>
        </div>
	</div>
</fieldset>


</asp:Content>
