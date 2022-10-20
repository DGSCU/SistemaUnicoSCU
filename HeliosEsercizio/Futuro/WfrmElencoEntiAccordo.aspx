<%@ Page Title="Elenco Enti in Accordo" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmElencoEntiAccordo.aspx.vb" Inherits="Futuro.WfrmElencoEntiAccordo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend> Elenco Enti in Accordo</legend>
        <asp:label id="lblMessaggi"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
                <h2><asp:Label ID="lblTitolo" runat="server"  Text="Elenco Enti in Accordo"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDenominazione" CssClass="label" AssociatedControlID="txtDenominazione" runat="server" Text="Denominazione Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtDenominazione" CssClass="textbox" runat="server" MaxLength="200"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodiceFiscale" CssClass="label" AssociatedControlID="txtcodicefiscale" runat="server" Text="Codice Fiscale"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtcodicefiscale" CssClass="textbox" runat="server" MaxLength="50"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblRifFase" CssClass="label" AssociatedControlID="txtIdEnteFase" runat="server" Text="Rif. Fase"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtIdEnteFase" CssClass="textbox" runat="server" MaxLength="10"></asp:TextBox>         
                </div>
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
                <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" 
                    Text="Esporta CSV" />
            </div>
        </div>
	            <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
        <br />
        <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco Enti" CssClass="table" CellPadding="2"  AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" PageSize="5" UseAccessibleHeader="True" >
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
                 <asp:TemplateColumn> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgEnte" CommandName="SelezionaEnte" ToolTip="Seleziona Ente" AlternateText="Seleziona Ente" runat="server" ImageURL="images/ente-mini.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn DataField="Codiceregione" HeaderText="Cod.Ente">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Denominazione" HeaderText="Denominazione">
					<HeaderStyle HorizontalAlign="Center" Width="400px"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="CodiceFiscale" HeaderText="Codice Fiscale">
					<HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="ClasseAttribuita" HeaderText="Sezione Attribuita">
					<HeaderStyle HorizontalAlign="Center" Width="20px"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="ClasseRichiesta" HeaderText="Sezione Richiesta">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				</asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Nr.Sedi"> 
                    <ItemTemplate>
					    <asp:LinkButton id="lnkElencoEnti" CommandName="ElencoEnti" ToolTip="Elenco Enti" Text='<%# Eval("Nsedi")%>' runat="server"></asp:LinkButton>                   
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn DataField="statorelazione" HeaderText="Stato Relazione">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="statoaccreditamento" HeaderText="Stato Iscr.">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="idente" HeaderText="idEnte">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="EntePadre" HeaderText="EntePadre"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="identepadre" HeaderText="identepadre"></asp:BoundColumn>
				<asp:BoundColumn DataField="IdEnteFase" HeaderText="Rif.Fase"></asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
    </fieldset>
</asp:Content>
