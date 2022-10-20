<%@ Page Title="Assegna Verificatore" Language="vb" AutoEventWireup="false" CodeBehind="Ver_AssegnaVerificatoriSupp.aspx.vb"
    Inherits="Futuro.Ver_AssegnaVerificatoriSupp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head id="Head1" runat="server">
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>

    <link href="Styles/Site.css" rel="stylesheet" type="text/css" lang="it-it" />
    <title>Assegna Verificatori</title>
</head>
<body>
  
      <script type="text/javascript">



           /* <![CDATA[ */
            $(function () {
                var clientId = "#" + ('TxtDataInizioPrevistaVer');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + ('TxtDataFinePrevistaVer');
                    $("" + clientId + "").datepicker();
            });
            

                 /* ]]> */
    </script>

    <form id="Form1" method="post" runat="server">
        <fieldset class="ContornoPagina">
            <legend >Dettagli Volontario</legend>
                <div class="wrapper" style="width:100%">
                    <div class="headers" >
                        <h2><asp:Label ID="Label5" runat="server"  Text="Elenco Veiricatori"></asp:Label></h2>
                    </div>
                        <asp:Label ID="lblmessaggio" runat="server" CssClass="msgErrore"></asp:Label>
                    <div class="RigaVuota">&nbsp;</div>
                    <div class="row" id="div2" runat="server" style="width: 100%">
                        <div class="collable" >
                            <asp:RadioButton ID="OptPrincipale" runat="server" Font-Size="XX-Small" Font-Names="Verdana"
                                Font-Bold="True" ForeColor="Navy" GroupName="OptTipologia" Text="Verificatore Principale"
                                AutoPostBack="True" Checked="True"></asp:RadioButton>
                        </div>
                        <div class="collable" >
                            <asp:RadioButton ID="OptSupporto" runat="server" Font-Size="XX-Small" Font-Names="Verdana"
                                Font-Bold="True" ForeColor="Navy" GroupName="OptTipologia" Text="Verificatore Supporto"
                                AutoPostBack="True"></asp:RadioButton>
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width: 25%">
                            <asp:Label ID="LblDataInizioPrevistaVer" CssClass="label" runat="server" Text="Data Inizio Prevista" AssociatedControlID="TxtDataInizioPrevistaVer" />
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="TxtDataInizioPrevistaVer" runat="server" CssClass="textboxData"/>
                        </div>
                        <div class="collable" style="width: 25%">
                            <asp:Label ID="LblDataFinePrevistaVer" CssClass="label" runat="server" Text="Data Fine Prevista" AssociatedControlID="TxtDataFinePrevistaVer"/>
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="TxtDataFinePrevistaVer" runat="server" CssClass="textboxData"/>
                        </div>
                    </div>
                    <asp:DataGrid ID="dgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco Progetti"
                    AllowPaging="false" CellPadding="2" Font-Size="Small" CssClass="table" AllowSorting="True"
                    AutoGenerateColumns="False" UseAccessibleHeader="True">
                    <FooterStyle></FooterStyle>
                    <SelectedItemStyle Font-Bold="true" BackColor="White"></SelectedItemStyle>
                    <EditItemStyle></EditItemStyle>
                    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                    <HeaderStyle></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="idverificatore" HeaderText="idverificatore"> </asp:BoundColumn>
                        <asp:TemplateColumn> 
                            <ItemTemplate>
                                <asp:ImageButton ID="IMGseg" CommandName="seleziona" ToolTip="Verificare" AlternateText="Verificare" runat="server" ImageURL="images/Icona_Progetto_small.png" CausesValidation="false"></asp:ImageButton>  
					        </ItemTemplate>
                        </asp:TemplateColumn>

               
                        <asp:BoundColumn DataField="Verificatore" HeaderText="Verificatore"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Tipologia" HeaderText="Tipologia"></asp:BoundColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                <b>Assegna Verificatore</b>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="check1" ToolTip="Assegna Verificatore" AutoPostBack="False" runat="server">
                                </asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle NextPageText="Successiva" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                        PrevPageText="Precedente" HorizontalAlign="Center" ForeColor="Navy" BackColor="#99CCFF"
                        Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                    <div class="RigaPulsanti">
                        <asp:Button ID="CmdAssegna" CssClass="Pulsante" runat="server" Text="Assegna verificatore"/>
                        <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"/>
                    </div>
                </div>
            </fieldset>
      <asp:HiddenField ID="txtIdVerificatore" runat="server" />
      <asp:HiddenField ID="txtIdVerifica" runat="server" />
    </form>
</body>
</html>
