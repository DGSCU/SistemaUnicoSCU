<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="WfrmVerVersioni.aspx.vb" Inherits="Futuro.WfrmVerVersioni" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset id="GestioneProgetti" class="ContornoPagina" runat="server">
        <legend>Versione Requisiti Verifiche</legend>
        <div class="wrapper" style="width: 100%; border: 0px">
            <div class="headers">
                <h2>
                    <asp:Label ID="lblTitoloPagina" runat="server" Text="Versione Requisiti Verifiche"></asp:Label>
                </h2>
            </div>

               <div class="RigaVuota">
                &nbsp;
            </div>
            <asp:DataGrid ID="dtgElencoVersioni" runat="server" Width="100%" ToolTip="Elenco versini verifiche"
                AllowPaging="True" CellPadding="2" Font-Size="Small" CssClass="table" AllowSorting="True"
                AutoGenerateColumns="False" UseAccessibleHeader="True">
                <FooterStyle></FooterStyle>
                <SelectedItemStyle Font-Bold="true" BackColor="White"></SelectedItemStyle>
                <EditItemStyle></EditItemStyle>
                <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                <HeaderStyle></HeaderStyle>
                <Columns>
                    <asp:ButtonColumn Text="&lt;img src=images/Icona_Progetto_small.png Width=20 Height=20 title='Seleziona Versione' border=0&gt;"
                        CommandName="Requisiti">
                        <HeaderStyle Width="1%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:ButtonColumn>
                    <asp:BoundColumn Visible="False" DataField="IDVersioneVerifiche" HeaderText="IDVersioneVerifiche">
                    </asp:BoundColumn>
                    <asp:BoundColumn Visible="False" DataField="id"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Descrizione" HeaderText="Versione">
                        <HeaderStyle Width="60%"></HeaderStyle>
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Stato" HeaderText="Stato">
                        <HeaderStyle Width="40%"></HeaderStyle>
                    </asp:BoundColumn>
                    <asp:ButtonColumn Text="&lt;img src=images/vincoli_small.png Width=20 Height=20 title='Associa Bando/Requisito' border=0&gt;"
                        HeaderText="Associa Bando/Versione" CommandName="Bando">
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
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"></asp:Button>&nbsp;
            </div>
        </div>
       
    </fieldset>
</asp:Content>
