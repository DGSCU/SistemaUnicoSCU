<%@ Page Title="Ricerca Sede per Verifica Segnalata DGSCN" Language="vb" AutoEventWireup="false" CodeBehind="ver_RicercaSedeSegnalataUNSC.aspx.vb"
    Inherits="Futuro.ver_RicercaSedeSegnalataUNSC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head id="Head1" runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" lang="it-it" />
    <title></title>
    <style type="text/css">
        .style1
        {
            height: 12px;
        }
    </style>
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
                <asp:Label ID="lblTitolo" runat="server" Text="Ricerca Sede di Progetto"></asp:Label></h2>
        </div>
    </div>
    <div class="rowGroup" style="height: auto">
        <div class="RigaVuota">
            &nbsp;</div>
    </div>
    <div class="wrapper" style="width: 100%">
        <div class="row" id="div2" runat="server">
            <div class="collable" style="width: 100%">
                <asp:Label ID="LblProgetto" CssClass="label" runat="server" Text="Progetto" />
            </div>
        </div>
        <div class="row" id="div1" runat="server">
            <div class="collable" style="width: 15%">
                <asp:Label ID="LblSede" CssClass="label" runat="server" Text="Sede" AssociatedControlID="TxtSede" />
            </div>
            <div class="colOggetti" style="width: 85%">
                <asp:TextBox ID="TxtSede" runat="server" CssClass="textbox"  ></asp:TextBox>
            </div>
        </div>
        <div class="row" id="div3" runat="server">
            <div class="collable" style="width: 15%">
                <asp:Label ID="LblCodSedeAtt" CssClass="label" runat="server" Text="Cod. Sede" AssociatedControlID="TxtCodSede" />
            </div>
            <div class="colOggetti" style="width: 35%">
                <asp:TextBox ID="TxtCodSede" runat="server" CssClass="textbox"></asp:TextBox>
            </div>
            <div class="collable" style="width: 15%">
                <asp:Label ID="LblComune" CssClass="label" runat="server" Text="Comune" AssociatedControlID="TxtComune" />
            </div>
            <div class="colOggetti" style="width: 35%">
                <asp:TextBox ID="TxtComune" runat="server" CssClass="textbox" ></asp:TextBox>
            </div>
        </div>
        <div class="row" id="div4" runat="server">
            <div class="collable" style="width: 15%">
                <asp:Label ID="LblProvincia" CssClass="label" runat="server" Text="Provincia" AssociatedControlID="TxtProvincia" />
            </div>
            <div class="colOggetti" style="width: 35%">
                <asp:TextBox ID="TxtProvincia" runat="server" CssClass="textbox"></asp:TextBox>
            </div>
            <div class="collable" style="width: 15%">
                <asp:Label ID="LblRegione" CssClass="label" runat="server" Text="Regione" AssociatedControlID="TxtRegione" />
            </div>
            <div class="colOggetti" style="width: 35%">
                <asp:TextBox ID="TxtRegione" runat="server" CssClass="textbox"></asp:TextBox>
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
      



              <asp:DataGrid ID="dgSedi" runat="server" Width="100%" 
            ToolTip="Elenco Sedi di Progetti" CellPadding="2" Font-Size="Small" 
            CssClass="table" AllowSorting="True"
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
                        <asp:ImageButton ID="imgSede" CommandName="seleziona" ToolTip="Seleziona Sede" AlternateText="Seleziona Sede" runat="server" ImageURL="images/sedi_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>

  
                <asp:BoundColumn DataField="Ente" HeaderText="Ente"></asp:BoundColumn>
                <asp:BoundColumn DataField="Sede" HeaderText="Sede">
                    <HeaderStyle Width="60%"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Comune" HeaderText="Comune">
                    <HeaderStyle Width="15%"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="idente" HeaderText="idente">
                    <HeaderStyle Width="0%"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="identesedeattuazione" HeaderText="identesedeattuazione">
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
