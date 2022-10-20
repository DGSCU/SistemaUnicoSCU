<%@ Page Title="Ricerca Progetto per Verifica Segnalata DGSCN" Language="vb" AutoEventWireup="false" CodeBehind="ver_RicercaProgettoSegnalataUNSC.aspx.vb"
    Inherits="Futuro.ver_RicercaProgettoSegnalataUNSC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head id="Head1" runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" lang="it-it" />
    <title></title>
</head>
<body>
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
    <form id="Form1" method="post" runat="server">
    <div class="firefox" style="width: 98%;">
        <center>
        </center>
    </div>
    <div class="rowGroup" style="height: auto">
        <asp:Label ID="lblmessaggio" runat="server" CssClass="msgErrore"></asp:Label>
        <div class="RigaVuota">
            &nbsp;</div>
    </div>
    <div class="wrapper" style="width: 100%">
        <div class="headers">
            <h2>
                <asp:Label ID="lblTitolo" runat="server" Text=" Ricerca Progetto"></asp:Label></h2>
        </div>
    </div>
    <div class="rowGroup" style="height: auto">
        <div class="RigaVuota">
            &nbsp;</div>
    </div>
    <div class="wrapper" style="width: 100%">
        <div class="row" id="div2" runat="server">
        </div>
        <div class="row" id="div1" runat="server">
            <div class="collable" style="width: 15%">
                <asp:Label ID="LblTitolooo" CssClass="label" runat="server" Text="Titolo" AssociatedControlID="TxtTitolo" />
            </div>
            <div class="colOggetti" style="width: 85%">
                <asp:TextBox ID="TxtTitolo" runat="server" CssClass="textbox"></asp:TextBox>
            </div>
        </div>
        <div class="row" id="div3" runat="server">
            <div class="collable" style="width: 15%; height: 13px;">
                <asp:Label ID="LblCodiceProgetto" CssClass="label" runat="server" Text="Cod. Progetto"
                    AssociatedControlID="TxtCodProgetto" />
            </div>
            <div class="colOggetti" style="width: 35%">
                <asp:TextBox ID="TxtCodProgetto" runat="server" CssClass="textbox"></asp:TextBox>
            </div>
            <div class="collable" style="width: 15%">
                <asp:Label ID="LblCircolare" CssClass="label" runat="server" Text="Circolare" AssociatedControlID="ddCircolare" />
            </div>
            <div class="colOggetti" style="width: 35%">
                <asp:DropDownList ID="ddCircolare" runat="server" CssClass="ddlClass" AutoPostBack="True">
                </asp:DropDownList>
            </div>
        </div>
        <div class="row" id="div4" runat="server">
            <div class="collable" style="width: 15%">
                <asp:Label ID="LblSettore" CssClass="label" runat="server" Text="Settore" AssociatedControlID="ddlSettore" />
            </div>
            <div class="colOggetti" style="width: 35%">
                <asp:DropDownList ID="ddlSettore" runat="server" CssClass="ddlClass" AutoPostBack="True">
                </asp:DropDownList>
            </div>
            <div class="collable" style="width: 15%">
                <asp:Label ID="LblArea" CssClass="label" runat="server" Text="Area" AssociatedControlID="ddlArea" />
            </div>
            <div class="colOggetti" style="width: 35%">
                <asp:DropDownList ID="ddlArea" runat="server" CssClass="ddlClass" AutoPostBack="True"
                    Enabled="False">
                </asp:DropDownList>
            </div>
        </div>
        <div class="row" id="div5" runat="server">
            <div class="collable" style="width: 15%">
                <asp:Label ID="Label2" CssClass="label" runat="server" Text="Competenza Progetto"
                    AssociatedControlID="ddlCompetenzaProgetto" />
            </div>
            <div class="colOggetti" style="width: 35%">
                <asp:DropDownList ID="ddlCompetenzaProgetto" runat="server" CssClass="ddlClass" 
                    AutoPostBack="True" >
                </asp:DropDownList>
            </div>
        </div>
        <div class="RigaPulsanti">
            <asp:Button ID="CmdRicerca" CssClass="Pulsante" runat="server" Text="Ricerca"></asp:Button>
            <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"></asp:Button>
        </div>
        <div class="rowGroup" style="height: auto">
            <div class="RigaVuota">
                &nbsp;</div>
        </div>
        <asp:DataGrid ID="dgProgetti" runat="server" Width="100%" ToolTip="Elenco Progetti"
            AllowPaging="false" CellPadding="2" Font-Size="Small" CssClass="table" AllowSorting="True"
            AutoGenerateColumns="False" UseAccessibleHeader="True">
            <FooterStyle></FooterStyle>
            <SelectedItemStyle Font-Bold="true" BackColor="White"></SelectedItemStyle>
            <EditItemStyle></EditItemStyle>
            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
            <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle></HeaderStyle>
            <Columns>
                <asp:TemplateColumn> 
                    <ItemTemplate>
                        <asp:ImageButton ID="imgpRG" CommandName="seleziona" ToolTip="Seleziona Progetto" AlternateText="Seleziona Progetto" runat="server" ImageURL="images/Icona_Progetto_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>


                <asp:BoundColumn DataField="Progetto" HeaderText="Progetto">
                    <HeaderStyle Width="60%"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="settore" HeaderText="Settore/Area">
                    <HeaderStyle Width="15%"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="circolare" HeaderText="Circolare">
                    <HeaderStyle Width="15%"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="numerovolontari" HeaderText="N&#176; Volontari">
                    <HeaderStyle Width="5%"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="idattivita" HeaderText="idattivita">
                    <HeaderStyle Width="0%"></HeaderStyle>
                </asp:BoundColumn>
            </Columns>
            <PagerStyle NextPageText="Successiva" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                PrevPageText="Precedente" HorizontalAlign="Center" ForeColor="Navy" BackColor="#99CCFF"
                Mode="NumericPages"></PagerStyle>
        </asp:DataGrid>
    </div>
    </form>
</body>
</html>
