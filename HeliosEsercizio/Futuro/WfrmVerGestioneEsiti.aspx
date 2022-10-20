<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="WfrmVerGestioneEsiti.aspx.vb" Inherits="Futuro.WfrmVerGestioneEsiti" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset id="GestioneProgetti" class="ContornoPagina" runat="server">
        <legend>Elenco Esiti</legend>
        <div class="wrapper" style="width: 100%; border: 0px">
            <div class="headers">
                <h2>
                    <asp:Label ID="lblTitoloPagina" runat="server" Text="Elenco Esiti"></asp:Label>
                </h2>
            </div>
            <div class="RigaVuota">
                &nbsp;
            </div>

                     <asp:DataGrid ID="dtgElencoEsiti" runat="server" Width="100%" ToolTip="Elenco esiti requisiti"
                AllowPaging="True" CellPadding="2" Font-Size="Small" CssClass="table" AllowSorting="True"
                AutoGenerateColumns="False" UseAccessibleHeader="True">
                <FooterStyle></FooterStyle>
                <SelectedItemStyle Font-Bold="true" BackColor="White"></SelectedItemStyle>
                <EditItemStyle></EditItemStyle>
                <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                <HeaderStyle></HeaderStyle>
        				<Columns>
									<asp:ButtonColumn Text="&lt;img src=images/ente-mini.png Width=20 Height=20 title='Seleziona Versione' border=0&gt;"
										CommandName="ModRequisiti">
										<HeaderStyle Width="1%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:ButtonColumn>
									<asp:BoundColumn Visible="False" DataField="idesito" HeaderText="idesito"></asp:BoundColumn>
									<asp:BoundColumn DataField="esito" HeaderText="Esito">
										<HeaderStyle Width="10%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="stato" HeaderText="Stato">
										<HeaderStyle Width="5%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:ButtonColumn Text="&lt;img src=images/canc_small.png Width=20 Height=20 title='Disabilia' border=0&gt;"
										HeaderText="Disabilita" CommandName="Disabilita">
										<HeaderStyle Width="1%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:ButtonColumn>
								</Columns>
                <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid"
                    Mode="NumericPages"></PagerStyle>
            </asp:DataGrid>
            <div class="RigaVuota">
                &nbsp;
            </div>
            <div class="RigaPulsanti">
              <asp:Button ID="cmdNuovo" CssClass="Pulsante" runat="server" Text="Nuovo Esito"></asp:Button>&nbsp;
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"></asp:Button>&nbsp;
                   
            </div>






        </div>
    </fieldset>


     




</asp:Content>
