<%@ Page Title="Visualizzazione Presenze Mensili" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmVisualizzaPresenzeMeseVol.aspx.vb" Inherits="Futuro.WfrmVisualizzaPresenzeMeseVol" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <fieldset class="ContornoPagina">
        <legend>Dettaglio Presenze Mensili</legend>
        <p>
            
         <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="True" />&nbsp;&nbsp;
            <asp:Label ID="lblAnnoMese"  runat="server" Text="" 
                style="text-align: center; font-weight: 700; font-size: medium"></asp:Label>
 </p>
 <p>
   <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
 </p>
 <asp:datagrid id="DtgDettaglioPresenze" runat="server" CssClass="table" Width="100%" ToolTip="Dettaglio Presenze Mensili" CellPadding="2" AllowSorting="True" AutoGenerateColumns="false" UseAccessibleHeader="True">
			<SelectedItemStyle BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
				<asp:BoundColumn DataField="CodiceVolontario" HeaderText="Codice Volontario" >
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"  Width="10%"></HeaderStyle>
				    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
                <asp:BoundColumn DataField="Nominativo" HeaderText="Nominativo">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"  Width="10%"></HeaderStyle>
				    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="CodiceFiscale" HeaderText="Codice Fiscale">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%"></HeaderStyle>
				    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="DataInizioServizio" HeaderText="Data Inizio Servizio">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" ></HeaderStyle>
				    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
                <asp:BoundColumn DataField="DataFineServizio" HeaderText="Data Fine Servizio">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" ></HeaderStyle>
				    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
                <asp:BoundColumn DataField="GiorniTotaliIndicati" HeaderText="Giorni Totali Indicati">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
                 <asp:BoundColumn DataField="GiorniPrevisti" HeaderText="Giorni Previsti">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
                <asp:BoundColumn DataField="EsistenzaFoglioPresenza" HeaderText="Esistenza Foglio Presenza">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Dettaglio" Visible="false"> 
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgSelect" CommandName="Select"  ToolTip="Seleziona Mese" AlternateText="Seleziona Mese" runat="server" ImageURL="images/lenteIngrandimento_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
			</Columns>
		</asp:datagrid>   
        </fieldset>      
        <div class="RigaPulsanti" style="text-align:right">
            <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
        </div>
</asp:Content>
