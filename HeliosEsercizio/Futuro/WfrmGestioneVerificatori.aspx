<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="WfrmGestioneVerificatori.aspx.vb" Inherits="Futuro.WfrmGestioneVerificatori" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset id="ContornoPaginaId" class="ContornoPagina" runat="server">
        <legend>
            <asp:Label ID="lblIntestazione1" runat="server" Text=" "></asp:Label>
        </legend>
        <div class="wrapper" style="width: 100%; border: 0px">
            <div class="headers">
                <h2>
                    <asp:Label ID="lblIntestazione" runat="server" Text=" "></asp:Label>
                </h2>
            </div>
            <div class="rowGroup" style="height: auto">
                <asp:Label ID="lblErrore" runat="server" CssClass="msgErrore"></asp:Label>
                <asp:Label ID="lblmessaggiosopra" runat="server" CssClass="msgConferma"></asp:Label>
                <div class="RigaVuota">
                    &nbsp;</div>
            </div>
            <div class="RigaVuota">
                &nbsp;
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label1" CssClass="label" runat="server" Text="*Cognome" AssociatedControlID="txtCognome"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:TextBox ID="txtCognome" CssClass="textbox" runat="server" />
                </div>
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label2" CssClass="label" runat="server" Text="*Nome" AssociatedControlID="txtNome"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:TextBox ID="txtNome" CssClass="textbox" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label3" AssociatedControlID="ddlCompetenza" runat="server" Text="*Competenza"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:DropDownList ID="ddlCompetenza" runat="server" AutoPostBack="True" CssClass="ddlClass">
                    </asp:DropDownList>
                </div>
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label4" AssociatedControlID="ddCircoscrizione" runat="server" Text="*Circoscrizione"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:DropDownList ID="ddCircoscrizione" runat="server" AutoPostBack="True" CssClass="ddlClass">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label5" CssClass="label" runat="server" Text="Riferimento" AssociatedControlID="txtRiferimento"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 85%">
                    <asp:TextBox ID="txtRiferimento" CssClass="textbox" runat="server" TextMode="MultiLine"
                        Width="96%" />
                </div>
            </div>
              <div class="RigaVuota">
                &nbsp;
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label6" AssociatedControlID="ddTipologia" runat="server" Text="*Tipologia"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 15%">
                    <asp:DropDownList ID="ddTipologia" runat="server" CssClass="ddlClass">
                        <asp:ListItem Selected="True"></asp:ListItem>
                        <asp:ListItem Value="0">Interno</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label7" AssociatedControlID="ddAbilitato" runat="server" Text="Abilitato"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 15%">
                    <asp:DropDownList ID="ddAbilitato" runat="server" AutoPostBack="True" CssClass="ddlClass">
                        <asp:ListItem Value="0">Si</asp:ListItem>
                        <asp:ListItem Value="1">No</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label8" AssociatedControlID="ddAbilitato" runat="server" Text="*Verificatore interno"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 25%">
                    <asp:DropDownList ID="ddVerificatoreInterno" runat="server" Enabled="False" CssClass="ddlClass">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label9" AssociatedControlID="ddlUsername" runat="server" Text="*Username"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 45%">
                    <asp:DropDownList ID="ddlUsername" runat="server" Enabled="False" CssClass="ddlClass"  Width="96%">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label10" CssClass="label" runat="server" Text="Email" AssociatedControlID="txtEmail"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 85%">
                    <asp:TextBox ID="txtEmail" CssClass="textbox" runat="server" Width="96%" />
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label11" CssClass="label" runat="server" Text="Tel. Cellulare" AssociatedControlID="txtTelCell"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:TextBox ID="txtTelCell" CssClass="textbox" runat="server" />
                </div>
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label12" CssClass="label" runat="server" Text="Tel. interno" AssociatedControlID="txtTelInterno"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:TextBox ID="txtTelInterno" CssClass="textbox" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label13" CssClass="label" runat="server" Text="Note" AssociatedControlID="txtNote"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 85%">
                    <asp:TextBox ID="txtNote" CssClass="textbox" runat="server" TextMode="MultiLine"
                        Width="96%" />
                </div>
            </div>
              <div class="RigaVuota">
                &nbsp;
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdConferma" CssClass="Pulsante" runat="server" Text="Salva">
                </asp:Button>
                <asp:Button ID="imgChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"></asp:Button>
            </div>
        </div>
    </fieldset>
</asp:Content>
