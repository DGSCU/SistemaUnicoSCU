<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="WfrmRicercaVerificatore.aspx.vb" Inherits="Futuro.WfrmRicercaVerificatore" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset id="ContornoPaginaId" class="ContornoPagina" runat="server">
        <legend>Ricerca Verificatore  </legend>
        <div class="wrapper" style="width: 100%; border: 0px">
            <div class="headers">
                <h2>
                    <asp:Label ID="lblTitolo" runat="server" Text="Ricerca Verificatore "></asp:Label>
                </h2>
            </div>
            <div class="rowGroup" style="height: auto">
                <asp:Label ID="lblmessaggiosopra" runat="server" CssClass="msgErrore"></asp:Label>
                <div class="RigaVuota">
                    &nbsp;</div>
            </div>
            <div class="RigaVuota">
                &nbsp;
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label1" CssClass="label" runat="server" Text="Cognome" AssociatedControlID="txtCognome"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:TextBox ID="txtCognome" CssClass="textbox" runat="server" />
                </div>
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label2" CssClass="label" runat="server" Text="Nome" AssociatedControlID="txtNome"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:TextBox ID="txtNome" CssClass="textbox" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label3" AssociatedControlID="ddCircoscrizione" runat="server" Text="Circoscrizione"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:DropDownList ID="ddCircoscrizione" runat="server" CssClass="ddlClass">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="0">Si</asp:ListItem>
                        <asp:ListItem Value="1">No</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label4" AssociatedControlID="cmbAbilitato" runat="server" Text="Abilitato"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:DropDownList ID="cmbAbilitato" runat="server" CssClass="ddlClass">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="0">Si</asp:ListItem>
                        <asp:ListItem Value="1">No</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label5" AssociatedControlID="cmbTipologia" runat="server" Text="Tipologia"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:DropDownList ID="cmbTipologia" runat="server" CssClass="ddlClass">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="0">Interno</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label6" AssociatedControlID="cmbVerificatoreInterno" runat="server"
                        Text="Verificatore interno"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:DropDownList ID="cmbVerificatoreInterno" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label7" AssociatedControlID="ddlCompetenza" runat="server" Text="Competenza"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:DropDownList ID="ddlCompetenza" runat="server" CssClass="ddlClass">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdRicerca" CssClass="Pulsante" runat="server" Text="Ricerca"></asp:Button>
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"></asp:Button>
            </div>
            <div class="RigaVuota">
                &nbsp;
            </div>
            <asp:DataGrid ID="dgRicerca" runat="server" Width="100%" ToolTip="Elenco Utenze Learning"
                AllowPaging="True" CellPadding="2" Font-Size="Small" CssClass="table"
                AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
                <FooterStyle></FooterStyle>
                <SelectedItemStyle BackColor="White"></SelectedItemStyle>
                <EditItemStyle></EditItemStyle>
                <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                <ItemStyle CssClass="tr" HorizontalAlign="Left"></ItemStyle>
                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                    Font-Strikeout="False" Font-Underline="False" VerticalAlign="Middle"></HeaderStyle>
                <Columns>
                    <asp:ButtonColumn Text="&lt;img src=images/Icona_Volontario_small.png Width=20 Height=20 title='Seleziona Verificatori' border=0&gt;"
                        CommandName="Select">
                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" 
                            VerticalAlign="Middle" Wrap="False" />
                    </asp:ButtonColumn>
                    <asp:BoundColumn DataField="IdVerificatore" HeaderText="Codice Verificatore"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Nominativo" HeaderText="Nominativo"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Circoscrizione" HeaderText="Circoscrizione"></asp:BoundColumn>
                    <asp:BoundColumn DataField="DescrAbilitato" HeaderText="Abilitato"></asp:BoundColumn>
                    <asp:BoundColumn DataField="DescrTipologia" HeaderText="Tipologia"></asp:BoundColumn>
                    <asp:BoundColumn DataField="NominativoInt" HeaderText="NominativoInt"></asp:BoundColumn>
                    <asp:BoundColumn Visible="False" DataField="idverificatoreinterno"></asp:BoundColumn>
                    <asp:BoundColumn Visible="False" DataField="IDREGCOMPETENZA"></asp:BoundColumn>
                </Columns>
                <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center"
                    CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
            </asp:DataGrid>
        </div>
    </fieldset>
</asp:Content>
