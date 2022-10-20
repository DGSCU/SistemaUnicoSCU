<%@ Page Title="Elenco Sedi" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="WfrmRicPrgVerifica.aspx.vb" Inherits="Futuro.WfrmRicPrgVerifica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
   <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
    <style type="text/css">
        .ui-datepicker
        {
            font-size: 11px;
        }
    </style>


   <script type="text/javascript">
           /* <![CDATA[ */
            $(function () {
                var clientId = "#" + CostruisciId('TxtInizioPog');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('TxtFinePog');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('TxtAlladata');
                    $("" + clientId + "").datepicker();
            });
              
		 						
	
 

                 /* ]]> */
    </script>

    	 <script type="text/javascript">
                 function fnStampa(){
                     StampaPagina();
			        return false;
                    }
		</script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="firefox">
        <fieldset class="ContornoPaginaScroll">
            <legend>Ricerca Progetti</legend>
              <div class="wrapper" id="Div15" style="width: 100%; border: 0px">
            <div class="headers">
                    <h2>
                        <asp:Label ID="lblTitolo" runat="server" Text="Ricerca Progetti"></asp:Label></h2>
                </div>
            <fieldset class="ContornoPagina">
                <legend>Caratteristiche Progetti</legend>
                <div class="wrapper" id="CaratteristicheProgetti" style="width: 100%; border: 0px">
                    <div class="row" id="divCaratteristicheProgetti" runat="server">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="codpog" CssClass="label" AssociatedControlID="TxtCodPog" runat="server"
                                Text="Codice Progetto"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 35%">
                            <asp:TextBox ID="TxtCodPog" runat="server" CssClass="textbox"></asp:TextBox>
                        </div>
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="nomepog" CssClass="label" AssociatedControlID="TxtDescPog" runat="server"
                                Text="Titolo Progetto"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 35%">
                            <asp:TextBox ID="TxtDescPog" runat="server" CssClass="textbox"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row" id="div1" runat="server">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label2" CssClass="label" AssociatedControlID="DdlBando" runat="server"
                                Text="Circolare"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 35%">
                            <asp:DropDownList ID="DdlBando" runat="server" CssClass="ddlClass" AutoPostBack="false">
                            </asp:DropDownList>
                        </div>
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label3" CssClass="label" AssociatedControlID="ddlMaccCodAmAtt" runat="server"
                                Text="Settore"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 35%">
                            <asp:DropDownList ID="ddlMaccCodAmAtt" runat="server" CssClass="ddlClass" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row" id="div2" runat="server">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label4" CssClass="label" AssociatedControlID="ddlCodAmAtt" runat="server"
                                Text="Area Intervento"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 35%">
                            <asp:DropDownList ID="ddlCodAmAtt" runat="server" CssClass="ddlClass" AutoPostBack="false">
                            </asp:DropDownList>
                        </div>
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label5" CssClass="label" AssociatedControlID="TxtNumVolontari" runat="server"
                                Text="N° Minimo Volontari"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 35%">
                            <asp:TextBox ID="TxtNumVolontari" runat="server" CssClass="textbox"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row" id="div3" runat="server">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label6" CssClass="label" AssociatedControlID="chkLTipoPosto" runat="server"
                                Text="Tipo Posto"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 35%">
                            <asp:CheckBoxList ID="chkLTipoPosto" runat="server" Font-Bold="True" Font-Names="Verdana"
                                Font-Size="XX-Small" ForeColor="Navy" RepeatDirection="Horizontal">
                                <asp:ListItem Value="V">V</asp:ListItem>
                                <asp:ListItem Value="VA">V/A</asp:ListItem>
                                <asp:ListItem Value="N">N</asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label7" CssClass="label" AssociatedControlID="ddlTipoProgetto" runat="server"
                                Text="Tipo Progetto"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 35%">
                            <asp:DropDownList ID="ddlTipoProgetto" runat="server" CssClass="ddlClass" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row" id="div4" runat="server">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label24" CssClass="label" AssociatedControlID="ddlCompetenzaProgetto"
                                runat="server" Text="Competenza"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 35%">
                            <asp:DropDownList ID="ddlCompetenzaProgetto" runat="server" CssClass="ddlClass" AutoPostBack="false">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </fieldset>
            <fieldset class="ContornoPagina">
                <legend>Caratteristiche Enti</legend>
                <div class="wrapper" id="Div7" style="width: 100%; border: 0px">
                    <div class="row" id="div5" runat="server">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label20" CssClass="label" AssociatedControlID="TxtCodEnte" runat="server"
                                Text="Cod. Ente"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 35%">
                            <asp:TextBox ID="TxtCodEnte" runat="server" CssClass="textbox"></asp:TextBox>
                        </div>
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label21" CssClass="label" AssociatedControlID="TxtDescrEnte" runat="server"
                                Text="Descr. Ente"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 35%">
                            <asp:TextBox ID="TxtDescrEnte" runat="server" CssClass="textbox"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row" id="div6" runat="server">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label9" CssClass="label" AssociatedControlID="ddlClasse" runat="server"
                                Text="Classe/Sezione"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 35%">
                            <asp:DropDownList ID="ddlClasse" runat="server" CssClass="ddlClass">
                            </asp:DropDownList>
                        </div>
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label10" CssClass="label" AssociatedControlID="ddlTipologia" runat="server"
                                Text="Tipologia"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 35%">
                            <asp:DropDownList ID="ddlTipologia" runat="server" CssClass="ddlClass">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row" id="div8" runat="server">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label11" CssClass="label" AssociatedControlID="ddlCompetenza" runat="server"
                                Text="Competenza"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 35%">
                            <asp:DropDownList ID="ddlCompetenza" runat="server" CssClass="ddlClass" >
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </fieldset>
            <fieldset class="ContornoPagina">
                <legend>Attuazione Geografica Progetti</legend>
                <div class="wrapper" id="Div9" style="width: 100%; border: 0px">
                    <div class="row" id="div10" runat="server">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label13" CssClass="label" AssociatedControlID="TxtComune" runat="server"
                                Text="Comune"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 35%">
                            <asp:TextBox ID="TxtComune" runat="server" CssClass="textbox"></asp:TextBox>
                        </div>
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label14" CssClass="label" AssociatedControlID="TxtProvincia" runat="server"
                                Text="Provincia"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 35%">
                            <asp:TextBox ID="TxtProvincia" runat="server" CssClass="textbox"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row" id="div11" runat="server">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label15" CssClass="label" AssociatedControlID="TxtRegione" runat="server"
                                Text="Regione"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 85%">
                            <asp:TextBox ID="TxtRegione" runat="server" CssClass="textbox"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </fieldset>
            <fieldset class="ContornoPagina">
                <legend>Caratteristiche Temporali</legend>
                <div class="wrapper" id="Div12" style="width: 100%; border: 0px">
                    <div id="Div13" class="row" runat="server">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label17" CssClass="label" AssociatedControlID="TxtInizioPog" runat="server"
                                Text="Data Inizio Progetti"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="TxtInizioPog" runat="server" CssClass="textboxData">
                            </asp:TextBox>
                        </div>
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label18" CssClass="label" AssociatedControlID="TxtFinePog" runat="server"
                                Text="Data Fine Progetti"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="TxtFinePog" runat="server" CssClass="textboxData">
                            </asp:TextBox>
                        </div>
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label19" CssClass="label" AssociatedControlID="TxtAlladata" runat="server"
                                Text="Progetti alla Data"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="TxtAlladata" runat="server" CssClass="textboxData">
                            </asp:TextBox>
                        </div>
                    </div>
                </div>
            </fieldset>
            </div> 
            <div class="wrapper" id="Div14" style="width: 100%; border: 0px">
                <div class="RigaPulsanti">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="LblCrea" CssClass="label" AssociatedControlID="cmdInsersci" runat="server"
                            Text="Crea Programmazione"  Visible="False"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 15%">
                        <asp:ImageButton ID="cmdInsersci" Style="cursor: hand" runat="server" ImageUrl="images/Sosti.gif"
                            BorderWidth="1px" ToolTip="Crea Programmazione" Visible="False" BorderStyle="Outset"
                            BorderColor="#6699FF"></asp:ImageButton>
                    </div>
                          <asp:Button ID="imgEsporta" runat="server" ToolTip="Esporta" CssClass="Pulsante"
                        Text="Esporta CSV" Visible="False" />
                    <asp:Button ID="cmdSalva" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                    <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />&nbsp;
              
                    <br />
                    &nbsp;
                </div>
            </div>
            <div class="wrapper" id="Div18" style="width: 100%; border: 0px">
                <div class="RigaPulsanti">
                    <div class="collable" style="width: 100%">
                        <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca"
                            CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399"
                            Visible="False"></asp:HyperLink>
                    </div>
                </div>
             <%--   <div class="wrapper" id="Div15" style="width: 100%; border: 0px">
                    <div id="Div16" class="row" runat="server">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="lblStampa" CssClass="label" AssociatedControlID="TxtInizioPog" runat="server"
                                Text="Stampa" Visible="False"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:ImageButton ID="imgStampa" ImageUrl="images/printHELIOS.jpg" 
                                runat="server" OnClientClick ="fnStampa();"
                                Alt="Stampa" Visible="False"></asp:ImageButton>
                        </div>
                    </div>
                    <div id="Div17" class="row" runat="server">
                        <div class="rowGroup" style="height: auto">
                            <asp:Label ID="lblMsgSede" CssClass="label" AssociatedControlID="" runat="server"
                                Text=""></asp:Label>
                        </div>
                    </div>
                </div>--%>
                 <asp:HiddenField  ID="txtbando1"  runat="server" />
                 <asp:HiddenField  ID="ddlCodAmAtt1" runat="server" />
                 <asp:HiddenField  ID="txtDenominazioneEnte1"  runat="server" />
                 <asp:HiddenField  ID="ddlStatoAttivita1"  runat="server" />
                 <asp:HiddenField  ID="ddlMaccCodAmAtt1"  runat="server" />
                 <asp:HiddenField  ID="txtTitoloProgetto1"  runat="server" />
                <br />
                <fieldset class="ContornoPaginaScroll">
                    <div class="firefox">
                        <asp:DataGrid ID="dgRisultatoRicerca" ToolTip="Elenco Progetti" runat="server" Width="100%"
                            AllowPaging="True" CssClass="table" AllowSorting="True" AutoGenerateColumns="False"
                            UseAccessibleHeader="True">
                            <FooterStyle></FooterStyle>
                            <SelectedItemStyle BackColor="White" Font-Bold="true"></SelectedItemStyle>
                            <EditItemStyle></EditItemStyle>
                            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                            <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                            <HeaderStyle></HeaderStyle>
                            <Columns>

                                 <asp:TemplateColumn> 
                                    <ItemTemplate>
                                        <asp:ImageButton ID="IMGVOL" CommandName="LinkVol" ToolTip="Elenco persone" AlternateText="Elenco persone" runat="server" ImageURL="images/Icona_Volontario_small.png" CausesValidation="false"></asp:ImageButton>  
								    </ItemTemplate>
                                 </asp:TemplateColumn>
                             
                                <asp:ButtonColumn Visible="False" ></asp:ButtonColumn>
                                <asp:ButtonColumn Visible="False" ></asp:ButtonColumn>
                                <asp:BoundColumn DataField="EnteProponente" HeaderText="Ente Proponente"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="idattivit&#224;entesedeattuazione" HeaderText="idattivit&#224;entesedeattuazione">
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Progetto" HeaderText="Progetto">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="ambito" HeaderText="Settore / Ambito"></asp:BoundColumn>
                                <asp:BoundColumn DataField="datainizioattivit&#224;" HeaderText="Data Inizio Progetto"
                                    DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                <asp:BoundColumn DataField="datafineattivit&#224;" HeaderText="Data Fine Progetto"
                                    DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="IDEnteSedeAttuazione" Visible="False" HeaderText="Codice Sede Attuazione">
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="EnteFiglio" Visible="False" HeaderText="Ente Figlio">
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Unico" HeaderText="Sede Attuazione"></asp:BoundColumn>
                                <asp:BoundColumn DataField="NumeroVolontari" HeaderText="Num. Volontari"></asp:BoundColumn>
                                <asp:BoundColumn DataField="comune" HeaderText="Comune"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Regione" HeaderText="Regione"></asp:BoundColumn>

                                <asp:TemplateColumn> 
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgProg" CommandName="Associa" ToolTip="Associa" AlternateText="Associa" runat="server" ImageURL="images/valida_small.png" CausesValidation="false"></asp:ImageButton>                         
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                
                                 <asp:BoundColumn DataField="IDAttività" Visible="False"></asp:BoundColumn>
                                 <asp:BoundColumn DataField="idente" Visible="False"></asp:BoundColumn>
                                 <asp:BoundColumn DataField="IDEnteSedeAttuazione" Visible="False">
                                 </asp:BoundColumn>
                                
                            </Columns>
                            <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center"
                                CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
                        </asp:DataGrid>
                    </div>
                </fieldset>
        </fieldset>
    </div>
</asp:Content>
