<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="Ver_GestioneStampa.aspx.vb" Inherits="Futuro.Ver_GestioneStampa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
    <script event="onclick" for="chkSelDesel">			
				for (var i=0;i<document.Form1.elements.length;i++)
				{
					var e = document.Form1.elements[i];
					if (e.type == 'checkbox'){					
						if (e.disabled==false)
						{
							e.checked = document.Form1.chkSelDesel.checked;
						}
					}				
				}			
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset id="GestioneProgetti" class="ContornoPagina" runat="server">
        <legend>Stampa Multipla Lettera d'Incarico</legend>
        <div class="wrapper" style="width: 100%; border: 0px">
            <div class="headers">
                <h2>
                    <asp:Label ID="Label4" runat="server" Text="Stampa Multipla Lettera d'Incarico"></asp:Label>
                </h2>
            </div>
            <div class="RigaVuota">
                &nbsp;
            </div>
            <div class="row">
                <div class="collable">
                    <asp:Label ID="Label3" AssociatedControlID="ddlVerificatoreInterno" runat="server"
                        Text="Verificatore Interno"></asp:Label>
                </div>
                <div class="colOggetti">
                    <asp:DropDownList ID="ddlVerificatoreInterno" runat="server" CssClass="ddlClass" AutoPostBack="false">
                    </asp:DropDownList>
                </div>
                <div class="collable">
                    <asp:Label ID="Label1" AssociatedControlID="ddlTipologiaVerifica" runat="server"
                        Text="Tipologia Verifica"></asp:Label>
                </div>
                <div class="colOggetti">
                    <asp:DropDownList ID="ddlTipologiaVerifica" CssClass="ddlClass" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="collable">
                    <asp:Label ID="Label2" AssociatedControlID="ddlProgrammazione" runat="server" Text="Programmazioni"></asp:Label>
                </div>
                <div class="colOggetti">
                    <asp:DropDownList ID="ddlProgrammazione" runat="server" AutoPostBack="false" CssClass="ddlClass">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdRicerca" CssClass="Pulsante" runat="server" Text="Esegui Ricerca">
                </asp:Button>
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Esci"></asp:Button>
            </div>
            <div class="rowGroup" style="height: auto">
                <asp:Label ID="lblmessaggio" runat="server" CssClass="msgErrore"></asp:Label>
                <div class="RigaVuota">
                    &nbsp;</div>
            </div>

                 <div class="row">
                <div class="collable">
                   <asp:CheckBox ID="chkSelDesel" runat="server" Font-Names="Arial" Font-Size="X-Small"
                    Font-Bold="True" ForeColor="Navy" Visible="False" Text="Seleziona tutto" 
                        AutoPostBack="True">  </asp:CheckBox>
                </div>

                    <div class="collable">
                    <asp:Label ID="LblVerde" AssociatedControlID="ddlTipologiaVerifica" runat="server" Visible="False"
                        Text="Stampe Prodotte"></asp:Label>
                </div>
                <div class="colOggetti">
                  <asp:Image ID="ImgVerde" runat="server" Width="20px" ImageUrl="images/QuestApprovato.jpg"
                    Height="20px" Visible="False"></asp:Image>
                </div>
            </div>

                  <asp:DataGrid ID="dgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco Verifiche"
                    AllowPaging="false" CellPadding="2" Font-Size="Small" CssClass="table" AllowSorting="True"
                    AutoGenerateColumns="False" UseAccessibleHeader="True">
                    <FooterStyle></FooterStyle>
                    <SelectedItemStyle Font-Bold="true" BackColor="White"></SelectedItemStyle>
                    <EditItemStyle></EditItemStyle>
                    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                    <HeaderStyle></HeaderStyle>
                        <Columns>
                        <asp:TemplateColumn HeaderText="Sel.">
                            <HeaderStyle Width="3px"></HeaderStyle>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelProg" ToolTip="Seleziona Verifica" AutoPostBack="False" runat="server">
                                </asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="CodiceFascicolo" HeaderText="Numero Fascicolo">
                            <HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TipoVerifica" HeaderText="Tipologia Verifica">
                            <HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Programmazione" HeaderText="Denominazione Programmazione">
                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Denominazione" HeaderText="Ente Proponente">
                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="False" HeaderText="selezionato">
                            <HeaderStyle HorizontalAlign="Center" Width="1px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Titolo" HeaderText=" Progetto">
                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="SedeIn" HeaderText="Sede">
                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="IndirizzoCiv" HeaderText="Indirizzo e N&#176; Civico">
                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ComuneAbb" HeaderText="Comune">
                            <HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Regione" HeaderText="Regione">
                            <HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="IdVerificheAssociate" HeaderText="IdVerificheAssociate">
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="DataStampa" HeaderText="DataStampa">
                        </asp:BoundColumn>
                    </Columns>
                    <PagerStyle NextPageText="Successiva" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
                        PrevPageText="Precedente" HorizontalAlign="Center" ForeColor="Navy" BackColor="#99CCFF"
                        Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>

                   <div class="RigaPulsanti">
                <asp:Button ID="imgConferma" CssClass="Pulsante" runat="server" Text="Conferma" Visible="False">
                </asp:Button>
               
            </div>
        </div>
    </fieldset>
  
</asp:Content>
