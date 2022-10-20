<%@ Page Title="Associa Verifiche" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="ver_RicercaAssociaVerifiche.aspx.vb" Inherits="Futuro.ver_RicercaAssociaVerifiche" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
   
     <script type="text/javascript">
                 function fncmdSalva(){
    
			        var msg;
			        msg=confirm("Attenzione. L'operazione che si sta eseguendo raggruppa le sedi per una singola verifica. Continuare?");
			        if (msg == false) {
			            return false;
			        }
			        return true; 
			}
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="firefox">
        <asp:HiddenField ID="txtIdVerificaPadre" runat="server" />
        <fieldset class="ContornoPaginaScroll">
            <legend>Ricerca Sedi attuazione</legend>
            <div class="wrapper" style="width: 100%">
                <div class="headers">
                    <h2>
                        <asp:Label ID="lblTitolo" runat="server" Text="Ricerca Sedi attuazione"></asp:Label></h2>
                </div>
                <fieldset class="ContornoPagina">
                    <div class="wrapper" id="Div1" style="width: 100%; border: 0px">
                        <div class="row" id="div2" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="lblProgetto" CssClass="label" AssociatedControlID="txtProgetto" runat="server"
                                    Text="Progetto"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 85%">
                                <asp:TextBox ID="txtProgetto" runat="server" CssClass="textbox" ReadOnly="True" Width="90%"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row" id="div3" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblVerificatore" CssClass="label" AssociatedControlID="TxtVerificatore"
                                    runat="server" Text="Verificatore"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 85%">
                                <asp:TextBox ID="TxtVerificatore" runat="server" CssClass="textbox" ReadOnly="True"
                                    Width="90%"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row" id="div4" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblTipoVerifica" CssClass="label" AssociatedControlID="TxtTipoVerifica"
                                    runat="server" Text="Tipologia Verifica"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:TextBox ID="TxtTipoVerifica" runat="server" CssClass="textbox" ReadOnly="True"
                                    Width="90%"></asp:TextBox>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblStatoVerifica" CssClass="label" AssociatedControlID="TxtStatoVerifica"
                                    runat="server" Text="Stato Verifica"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:TextBox ID="TxtStatoVerifica" runat="server" CssClass="textbox" ReadOnly="True"
                                    Width="90%"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row" id="div5" runat="server" visible="false">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label1" CssClass="label" AssociatedControlID="TxtDescrEnte" runat="server"
                                    Text="Descr. Ente"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:TextBox ID="TxtDescrEnte" runat="server" CssClass="textbox" ReadOnly="True"
                                    Width="90%"></asp:TextBox>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label2" CssClass="label" AssociatedControlID="TxtCodEnte" runat="server"
                                    Text="Cod. Ente"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:TextBox ID="TxtCodEnte" runat="server" CssClass="textbox" ReadOnly="True" Width="90%"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row" id="div6" runat="server" visible="false">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label5" CssClass="label" AssociatedControlID="TxtComune" runat="server"
                                    Text="Comune"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:TextBox ID="TxtComune" runat="server" CssClass="textbox" ReadOnly="True" Width="90%"></asp:TextBox>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label6" CssClass="label" AssociatedControlID="TxtProvincia" runat="server"
                                    Text="Provincia"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:TextBox ID="TxtProvincia" runat="server" CssClass="textbox" ReadOnly="True"
                                    Width="90%"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row" id="div7" runat="server" visible="false">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label7" CssClass="label" AssociatedControlID="TxtRegione" runat="server"
                                    Text="Regione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:TextBox ID="TxtRegione" runat="server" CssClass="textbox" ReadOnly="True" Width="90%"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row" id="div8" runat="server" visible="false">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label8" CssClass="label" AssociatedControlID="ddlSettore" runat="server"
                                    Text="Settore"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:DropDownList ID="ddlSettore" runat="server" CssClass="ddlClass" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label9" CssClass="label" AssociatedControlID="ddlArea" runat="server"
                                    Text="Area Int."></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:DropDownList ID="ddlArea" runat="server" CssClass="ddlClass">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <div class="RigaPulsanti">
                   
                    <asp:Button ID="CmdRicerca" CssClass="Pulsante" runat="server" Text="Ricerca"></asp:Button>
                    <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"></asp:Button>
                          <asp:Button ID="imgStampa" runat="server" ToolTip="Esporta" CssClass="Pulsante"
                        Text="Esporta CSV" Visible="False" />
                    <br />
                    


                    <asp:HyperLink ID="ApriCSV1" runat="server" AccessKey="S" CssClass="linkStampa" ForeColor="#003399"
                        Text="DOWNLOAD CSV" ToolTip="Link per la stampa del risultato della ricerca"
                        Visible="False"></asp:HyperLink>


                </div>
            </div>
            <fieldset class="ContornoPaginaScroll">
                <legend>Dettaglio Sedi attuazione</legend>
                <div class="firefox">
                    <div class="rowGroup" style="height: auto">
                        <asp:Label ID="lblmessaggi" runat="server" CssClass="msgErrore"></asp:Label>
                        <div class="RigaVuota">
                            &nbsp;</div>
                    </div>
                    <div class="row" id="div9" runat="server">
                        <div class="colOggetti" style="width: 80%">
                            <asp:CheckBox ID="chkSelDesel" runat="server"   Visible="False" 
                                Text="Seleziona tutto" AutoPostBack="True">
                            </asp:CheckBox>
                        </div>
                        <div class="colOggetti" style="width: 20%">
                            <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" Text="Salva" Visible="False" OnClientClick ="fncmdSalva();">
                            </asp:Button>
                        </div>
                    </div>
                
                    <asp:DataGrid ID="dgRisultatoRicerca" runat="server" ToolTip="Elenco Progetti" runat="server"
                        Width="100%" AllowPaging="True" CssClass="table" AllowSorting="True"
                        AutoGenerateColumns="False" UseAccessibleHeader="True">
                        <FooterStyle></FooterStyle>
                        <SelectedItemStyle BackColor="White" Font-Bold="true"></SelectedItemStyle>
                        <EditItemStyle></EditItemStyle>
                        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                        <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="idverifica" HeaderText="idverifica">
                            </asp:BoundColumn>
 <%--                           <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    <b>Sel</b>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSela" runat="server" AutoPostBack="false" ToolTip="Assegna Verificatore IGF">
                                    </asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>--%>
                               <asp:TemplateColumn HeaderText="Sel">
								<ItemTemplate>
                                    <asp:CheckBox id="chkSel" toolTip="Verifica"  runat="server"  Text="&nbsp;"></asp:CheckBox>    
								</ItemTemplate>
							       <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                                       Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" 
                                       VerticalAlign="Middle" />
							</asp:TemplateColumn>

                            <asp:BoundColumn DataField="EnteProponente" HeaderText="Ente"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Progetto" HeaderText="Progetto">
                                <HeaderStyle Width="2%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="datafineattivit&#224;" HeaderText="Data Fine Progetto"
                                DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                            <asp:BoundColumn DataField="DataPrevistaVerifica" HeaderText="Data Inizio Prevista Verifica"
                                DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                            <asp:BoundColumn DataField="DataFinePrevistaVerifica" HeaderText="Data Fine Prevista Verifica"
                                DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Programmazione" HeaderText="Programmazione"></asp:BoundColumn>
                            <asp:BoundColumn DataField="IDEnteSedeAttuazione" HeaderText="Cod Sede"></asp:BoundColumn>
                            <asp:BoundColumn DataField="comune" HeaderText="Comune"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Regione" HeaderText="Regione"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center"
                            CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                </div>
            </fieldset>
        </fieldset>
    </div>
 
  
</asp:Content>
