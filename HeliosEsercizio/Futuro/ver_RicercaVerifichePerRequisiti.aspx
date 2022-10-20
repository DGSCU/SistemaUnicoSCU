<%@ Page Title="Ricerca Verifiche" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="ver_RicercaVerifichePerRequisiti.aspx.vb" Inherits="Futuro.ver_RicercaVerifichePerRequisiti" %>

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
                var clientId = "#" + CostruisciId('txtDataDalInizio');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('txtDataAlInizio');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('txtDataAlPrevista');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('txtDataDalPrevista');
                    $("" + clientId + "").datepicker();
            });

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
      /* <![CDATA[ */
        var pathPlus = "images/plus_xsmall.png";
        var pathMinus = "images/minus_xsmall.png";
        var titlePathMinus = "Riduci";
        var titlePathPlus = "Espandi";
        var inserimento = '1';
        var inserimento1 = '';

      
        /* ]]> */  
    </script>
    <script type="text/javascript">
       /* <![CDATA[ */
        $(function () {
            var tornaSu = 0;
            if (inserimento1 != "") {
                $('#CaratteristicheOrganizzativeContent').css('display', 'none');
                $('#CaratteristicheOrganizzative').find("img").first().attr("src", pathPlus);
                $('#CaratteristicheOrganizzative').find("img").first().attr("title", titlePathPlus);
            }
            else {
                $('#CaratteristicheOrganizzativeContent').css('display', 'yes');
                $('#CaratteristicheOrganizzative').find("img").first().attr("src", pathMinus);
                $('#CaratteristicheOrganizzative').find("img").first().attr("title", titlePathMinus);
            }

            $('#TornaSuCaratteristicheOrganizzative').click(function () {
                tornaSu = 1;
            });
            $('#CaratteristicheOrganizzative').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#CaratteristicheOrganizzativeContent').is(':visible');
                    $('#CaratteristicheOrganizzativeContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    if (!isVisible) {
                        $(this).find("img").first().attr("src", pathMinus);
                        $(this).find("img").first().attr("title", titlePathMinus);
                        title = "Espandi"
                    }
                    else {
                        $(this).find("img").first().attr("src", pathPlus);
                        $(this).find("img").first().attr("title", titlePathPlus);
                    }
                }
                tornaSu = 0;
                return false;
            });
        });
       


        $(function () {
            var tornaSu = 0;
            if (inserimento != "") {
                $('#CaratteristicheProgettiContent').css('display', 'none');
                $('#CaratteristicheProgetti').find("img").first().attr("src", pathPlus);
                $('#CaratteristicheProgetti').find("img").first().attr("title", titlePathPlus);
            }
            else {
                $('#CaratteristicheProgettiContent').css('display', 'yes');
                $('#CaratteristicheProgetti').find("img").first().attr("src", pathMinus);
                $('#CaratteristicheProgetti').find("img").first().attr("title", titlePathMinus);
            }

            $('#TornaSuCaratteristicheProgetti').click(function () {
                tornaSu = 1;
            });
            $('#CaratteristicheProgetti').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#CaratteristicheProgettiContent').is(':visible');
                    $('#CaratteristicheProgettiContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    if (!isVisible) {
                        $(this).find("img").first().attr("src", pathMinus);
                        $(this).find("img").first().attr("title", titlePathMinus);
                        title = "Espandi"
                    }
                    else {
                        $(this).find("img").first().attr("src", pathPlus);
                        $(this).find("img").first().attr("title", titlePathPlus);
                    }
                }
                tornaSu = 0;
                return false;
            });
        });
       

        $(function () {
            var tornaSu = 0;
            if (inserimento != "") {
                $('#CaratteristicheEntiContent').css('display', 'none');
                $('#CaratteristicheEnti').find("img").first().attr("src", pathPlus);
                $('#CaratteristicheEnti').find("img").first().attr("title", titlePathPlus);
            }
            else {
                $('#CaratteristicheEntiContent').css('display', 'yes');
                $('#CaratteristicheEnti').find("img").first().attr("src", pathMinus);
                $('#CaratteristicheEnti').find("img").first().attr("title", titlePathMinus);
            }

            $('#TornaSuCaratteristicheEnti').click(function () {
                tornaSu = 1;
            });
            $('#CaratteristicheEnti').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#CaratteristicheEntiContent').is(':visible');
                    $('#CaratteristicheEntiContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    if (!isVisible) {
                        $(this).find("img").first().attr("src", pathMinus);
                        $(this).find("img").first().attr("title", titlePathMinus);
                        title = "Espandi"
                    }
                    else {
                        $(this).find("img").first().attr("src", pathPlus);
                        $(this).find("img").first().attr("title", titlePathPlus);
                    }
                }
                tornaSu = 0;
                return false;
            });
        });
       


  
  



                 /* ]]> */
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="firefox">
        <fieldset class="ContornoPaginaScroll">
            <legend>Ricerca/Modifica Verifiche</legend>
            <div class="wrapper" style="width: 100%">
                <div class="headers">
                    <h2>
                        <asp:Label ID="lblTitolo" runat="server" Text="Ricerca/Modifica Verifiche"></asp:Label></h2>
                </div>
                <div class="rowGroup" style="height: auto">
                    <asp:Label ID="lblErrore" runat="server" CssClass="msgErrore"></asp:Label>
                    <div class="RigaVuota">
                        &nbsp;</div>
                </div>
                <fieldset id="flsCaratteristicheOrganizzative" class="ContornoPagina">
                    <legend id="CaratteristicheOrganizzative">
                        <img src="images/plus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor: pointer;" />&nbsp;&nbsp;<img
                            id="TornaSuCaratteristicheOrganizzative" style="cursor: pointer;" onclick="TornaAdInizioPagina()"
                            src="images/tornasu_xsmall.png" title="Torna ad inizio pagina" alt="Torna ad inizio pagina" />&nbsp;&nbsp;Caratteristiche
                        Verifiche</legend>
                    <div class="wrapper" id="CaratteristicheOrganizzativeContent" style="width: 100%;
                        border: 0px">
                        <div class="row" id="divCaratteristicheVerifiche" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label25" CssClass="label" AssociatedControlID="txtCodiceFascicolo"
                                    runat="server" Text="Numero Fascicolo"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:TextBox ID="txtCodiceFascicolo" runat="server" CssClass="textbox"></asp:TextBox>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label23" CssClass="label" AssociatedControlID="ddlProgrammazione"
                                    runat="server" Text="Programmazione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:DropDownList ID="ddlProgrammazione" runat="server" CssClass="ddlClass" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label27" CssClass="label" AssociatedControlID="ddlStatoVerifica" runat="server"
                                    Text="Stato Verifica"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:DropDownList ID="ddlStatoVerifica" runat="server" CssClass="ddlClass">
                                </asp:DropDownList>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblTipoVerifica" CssClass="label" AssociatedControlID="ddlTipologiaVerifica"
                                    runat="server" Text="Tipologia Verifica"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:DropDownList ID="ddlTipologiaVerifica" runat="server" CssClass="ddlClass">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="Div1" class="row" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label5" CssClass="label" AssociatedControlID="ddlCompetenza" runat="server"
                                    Text="Competenza"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:DropDownList ID="ddlCompetenza" runat="server" CssClass="ddlClass" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label26" CssClass="label" AssociatedControlID="ddlVerificatoreInterno"
                                    runat="server" Text="Verificatore Interno"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:DropDownList ID="ddlVerificatoreInterno" runat="server" AutoPostBack="False"
                                    CssClass="ddlClass">
                                </asp:DropDownList>
                            </div>
                            <div class="collable" style="width: 15%">
                            </div>
                            <div class="colOggetti" style="width: 35%">
                            </div>
                        </div>
                        <div id="Div3" class="row" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label28" CssClass="label" AssociatedControlID="txtDataDalPrevista"
                                    runat="server" Text="Data prevista Verifica Dal"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="txtDataDalPrevista" runat="server" CssClass="textboxData">
                                </asp:TextBox>
                            </div>
                            <div class="collable" style="width: 5%">
                                <asp:Label ID="Label32" CssClass="label" AssociatedControlID="txtDataAlPrevista"
                                    runat="server" Text="  Al"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="txtDataAlPrevista" runat="server" CssClass="textboxData">
                                </asp:TextBox>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label30" CssClass="label" AssociatedControlID="txtDataDalInizio" runat="server"
                                    Text="Data Inizio Verifica Dal"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="txtDataDalInizio" runat="server" CssClass="textboxData">
                                </asp:TextBox>
                            </div>
                            <div class="collable" style="width: 5%">
                                <asp:Label ID="Label34" CssClass="label" AssociatedControlID="txtDataAlInizio" runat="server"
                                    Text="  Al"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="txtDataAlInizio" runat="server" CssClass="textboxData">
                                </asp:TextBox>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <fieldset id="flsCaratteristicheEnti" class="ContornoPagina">
                    <legend id="CaratteristicheEnti">
                        <img src="images/plus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor: pointer;" />&nbsp;&nbsp;<img
                            id="TornaSuCaratteristicheEnti" style="cursor: pointer;" onclick="TornaAdInizioPagina()"
                            src="images/tornasu_xsmall.png" title="Torna ad inizio pagina" alt="Torna ad inizio pagina" />&nbsp;&nbsp;Caratteristiche
                        Enti</legend>
                    <div class="wrapper" id="CaratteristicheEntiContent" style="width: 100%; border: 0px">
                        <div class="row" id="div8" runat="server">
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
                        <div id="Div9" class="row" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label9" CssClass="label" AssociatedControlID="ddlClasse" runat="server"
                                    Text="Classe/Sezione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:DropDownList ID="ddlClasse" runat="server" AutoPostBack="False" CssClass="ddlClass">
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
                    </div>
                </fieldset>
                <fieldset id="flsCaratteristicheProgetti" class="ContornoPagina">
                    <legend id="CaratteristicheProgetti">
                        <img src="images/plus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor: pointer;" />&nbsp;&nbsp;<img
                            id="TornaSuCaratteristicheProgetti" style="cursor: pointer;" onclick="TornaAdInizioPagina()"
                            src="images/tornasu_xsmall.png" title="Torna ad inizio pagina" alt="Torna ad inizio pagina" />&nbsp;&nbsp;Caratteristiche
                        Progetti</legend>
                    <div class="wrapper" id="CaratteristicheProgettiContent" style="width: 100%; border: 0px">
                        <div class="row" id="div5" runat="server">
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
                        <div id="Div6" class="row" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label2" CssClass="label" AssociatedControlID="DdlBando" runat="server"
                                    Text="Circolare"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:DropDownList ID="DdlBando" runat="server" AutoPostBack="False" CssClass="ddlClass">
                                </asp:DropDownList>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label3" CssClass="label" AssociatedControlID="ddlMaccCodAmAtt" runat="server"
                                    Text="Settore"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:DropDownList ID="ddlMaccCodAmAtt" runat="server" CssClass="ddlClass">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="Div7" class="row" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label4" CssClass="label" AssociatedControlID="ddlCodAmAtt" runat="server"
                                    Text="Area Intervento"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:DropDownList ID="ddlCodAmAtt" runat="server" AutoPostBack="False" CssClass="ddlClass">
                                </asp:DropDownList>
                            </div>
                        </div>
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
                            <div class="colOggetti" style="width: 35%">
                                <asp:TextBox ID="TxtRegione" runat="server" CssClass="textbox"></asp:TextBox>
                            </div>
                        </div>
                        <div id="Div12" class="row" runat="server">
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
                <div class="RigaPulsanti">
                   <asp:Button ID="cmdEsporta" runat="server" ToolTip="Esporta" 
                        CssClass="Pulsante"  Text="Esporta CSV" Visible="False" 
                        UseSubmitBehavior="False" />
                    <asp:Button ID="cmdRicerca" CssClass="Pulsante" runat="server" Text="Ricerca"></asp:Button>
                    <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"></asp:Button>
                 
                    <br />
                    <asp:HyperLink ID="ApriCSV1" runat="server" AccessKey="S" CssClass="linkStampa" ForeColor="#003399"
                        Text="DOWNLOAD CSV" ToolTip="Link per la stampa del risultato della ricerca"
                        Visible="False"></asp:HyperLink>
                </div>
            </div>
            <br />
                   
                <asp:DataGrid ID="dgRisultatoRicerca" Caption="Elenco Progetti" runat="server" Width="100%"
                    AllowPaging="true" PageSize="10" CssClass="table" AllowSorting="True" AutoGenerateColumns="False"
                    UseAccessibleHeader="True" Visible="true">
                    <FooterStyle></FooterStyle>
                    <SelectedItemStyle BackColor="White" Font-Bold="true"></SelectedItemStyle>
                    <EditItemStyle></EditItemStyle>
                    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                    <HeaderStyle></HeaderStyle>
                    <Columns>
                       <asp:TemplateColumn> 
                            <ItemTemplate>
                                <asp:ImageButton ID="IMGseg" CommandName="seleziona" ToolTip="Seleziona Verifica" AlternateText="Seleziona Verifica" runat="server" ImageURL="images/Icona_Progetto_small.png" CausesValidation="false"></asp:ImageButton>  
					        </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="Idverifica" HeaderText="Idverifica">
                            <HeaderStyle></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Codicefascicolo" HeaderText="Numero Fascicolo">
                            <HeaderStyle></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="statoverifiche" HeaderText="Stato Verifica">
                            <HeaderStyle></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TipoVerifica" HeaderText="Tipo Verifica">
                            <HeaderStyle></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="programmazione" HeaderText="Programmazione">
                            <HeaderStyle></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="DataPrevistaVerifica" HeaderText="Data Inizio Prevista Verifica"
                            DataFormatString="{0:dd/MM/yyyy}">
                            <HeaderStyle></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="DataFinePrevistaVerifica" HeaderText="Data Fine Prevista Verifica"
                            DataFormatString="{0:dd/MM/yyyy}">
                            <HeaderStyle></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="DataFineVerifica" HeaderText="Data Chiusura Verifica"
                            DataFormatString="{0:dd/MM/yyyy}">
                            <HeaderStyle></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="nominativo" HeaderText="Verificatore">
                            <HeaderStyle></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Denominazione" HeaderText="Ente Proponente">
                            <HeaderStyle></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Titolo" HeaderText="Progetto">
                            <HeaderStyle></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="DataInizioAttivit&#224;" HeaderText="Data Inizio Progetto"
                            DataFormatString="{0:dd/MM/yyyy}">
                            <HeaderStyle></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="datafineattivit&#224;" HeaderText="Data Fine Progetto"
                            DataFormatString="{0:dd/MM/yyyy}">
                            <HeaderStyle></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="idprogrammazione"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="idente"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center"
                        CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
               
         
        </fieldset>
    </div>
</asp:Content>
