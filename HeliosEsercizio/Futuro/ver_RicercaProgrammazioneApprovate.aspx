<%@ Page Title="Ricerca Programmazioni Approvaew" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="ver_RicercaProgrammazioneApprovate.aspx.vb" Inherits="Futuro.ver_RicercaProgrammazioneApprovate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">


  <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>





        <script type="text/javascript">
           /* <![CDATA[ */
            $(function () {
                var clientId = "#" + CostruisciId('TxtDataProtPresentazione');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('TxtDataProtApprovazione');
                    $("" + clientId + "").datepicker();
            });
             
                 /* ]]> */
       </script>

         <script type="text/javascript">
    /* <![CDATA[ */
        var prefissoIdClient = "MainContent_";

        function CostruisciId(IdServer) {
            var IdClient = prefissoIdClient + IdServer;
            return IdClient;
        };

   
          /* ]]> */

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="firefox">
        <fieldset class="ContornoPaginaScroll">
            <legend>Ricerca Programmazioni</legend>
            <div class="wrapper" style="width: 100%">
                <div class="headers">
                    <h2>
                        <asp:Label ID="lblTitolo" runat="server" Text="Ricerca Programmazioni"></asp:Label></h2>
                </div>
                <div class="RigaVuota">
                    &nbsp;</div>
                <div class="row" id="div1" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblProgr" CssClass="label" AssociatedControlID="ddlProgr" runat="server"
                            Text="Programmazione"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 75%">
                        <div class="colOggetti" style="width: 100%">
                            <asp:DropDownList ID="ddlProgr" runat="server" CssClass="ddlClass">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div  class="row" id="div4" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label5" CssClass="label" AssociatedControlID="ddlCompetenza" runat="server"
                            Text="Competenza"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 75%">
                        <asp:DropDownList ID="ddlCompetenza" runat="server" CssClass="ddlClass" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblNumFascicolo" CssClass="label" runat="server" Text="Num.Fascicolo"
                            AssociatedControlID="TxtNumFascicolo"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 75%">
                        <asp:TextBox ID="TxtNumFascicolo" CssClass="textbox" runat="server" />
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="LblDataProtPresentazione" CssClass="label" AssociatedControlID="TxtDataProtPresentazione"
                            runat="server" Text="Data Presentazione"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 15%">
                        <asp:TextBox ID="TxtDataProtPresentazione" CssClass="textboxData" runat="server"
                             />
                    </div>
                    <div class="colOggetti" style="width: 15%">
                        <asp:Label ID="LblDataProtApprovazione" CssClass="label" AssociatedControlID="TxtDataProtApprovazione"
                            runat="server" Text="Data Approvazione"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 15%">
                        <asp:TextBox ID="TxtDataProtApprovazione" CssClass="textboxData" 
                            runat="server" />
                    </div>
                </div>
                <div class="RigaVuota">
                    &nbsp;</div>
                <div class="RigaPulsanti">
                    <asp:Button ID="CmdRicerca" runat="server" Text="Ricerca" CssClass="Pulsante">
                    </asp:Button>
                    <asp:Button ID="cmdChiudi" runat="server" Text="Chiudi" CssClass="Pulsante"></asp:Button>
                    <asp:Button ID="imgStampa" runat="server" CssClass="Pulsante" 
                        Text="Esporta CSV" Visible="False" />
                </div>
                <p>
                        <asp:HyperLink ID="ApriCSV1" runat="server" AccessKey="S" CssClass="linkStampa" ForeColor="#003399"
                            Text="DOWNLOAD CSV" ToolTip="Link per la stampa del risultato della ricerca"
                            Visible="False"></asp:HyperLink>
                </p>

                <asp:Label ID="lblMessaggio" runat="server" CssClass="msgConferma"></asp:Label>
                <div class="RigaVuota"> &nbsp;</div>
                <asp:DataGrid ID="dgRisultatoRicerca" runat="server" Width="100%" AllowPaging="True"
                    CssClass="table" AllowSorting="True" AutoGenerateColumns="False" 
                    UseAccessibleHeader="True">
                    <FooterStyle></FooterStyle>
                    <SelectedItemStyle BackColor="White" Font-Bold="true"></SelectedItemStyle>
                    <EditItemStyle></EditItemStyle>
                    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                    <HeaderStyle></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="IDProgrammazione" HeaderText="IDProgrammazione">
                        </asp:BoundColumn>

                        <asp:TemplateColumn > 
                            <ItemTemplate>
                                <asp:ImageButton ID="imgProg" CommandName="Select" ToolTip="Seleziona Programmazione" AlternateText="Seleziona Programmazione" runat="server" ImageURL="images/Icona_Progetto_small.png" CausesValidation="false"></asp:ImageButton>                         
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="CodiceFascicolo" HeaderText="Numero Fascicolo"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Descrizione" HeaderText="Programmazione"></asp:BoundColumn>
                        <asp:BoundColumn DataField="DataProtPresentazione" HeaderText="Data Prentazione">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="DataProtApprovazione" HeaderText="Data Approvazione">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="totVer" HeaderText="N&#176; Verifiche"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Competenza" HeaderText="Competenza">
                        </asp:BoundColumn>
                    </Columns>
   <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center"
                        CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </div>
        </fieldset>
    </div>
</asp:Content>
