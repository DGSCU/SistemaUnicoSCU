<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="ver_Sanzione.aspx.vb" Inherits="Futuro.ver_Sanzione" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function fncmdApplica() {
            if (document.Form1.ddlSanzione.value == "0") {
                alert("E' necessario indicare la sanzione.");
                document.Form1.ddlSanzione.focus;
                return false;
            }
        }
    </script>
    <script type="text/javascript" src="stampa.js"></script>
    <%-- <script language="javascript" for="imgStampa" event="onclick">--%>
    <script type="text/javascript">
        function fncmdStampa() {
           
                document.all.StampaPagina.style.visibility = 'hidden';
                window.print()
                window.close()
            }
        }
    </script>
    <script type="text/javascript">
        function Irregolarita() {
            if (document.Form1.hddControllo.value == "1") {
                var msg;
                msg = confirm("Si sta sanzionado una Sede senza irregolarità. Si vuole continuare?");
                if (msg == false) {
                    document.Form1.hddControllo.value = '0';
                    return false;
                }
                else {
                    document.Form1.hddControllo.value = '2';
                }
             }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       

    <div class="firefox">
        <fieldset class="ContornoPaginaScroll">
            <legend>Verifica</legend>
      
            <div class="wrapper" style="width: 100%">
                <div class="headers">
                    <h2>
                        <asp:Label ID="lblTitolo" runat="server" Text="Verifica"></asp:Label>
                    </h2>
                </div>
                <div class="RigaVuota">
                    &nbsp;</div>
                <div class="row" id="div1" runat="server" style="width: 100%">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label1" CssClass="label" AssociatedControlID="lblProgrammazione" runat="server"
                            Text="Programmazione:"></asp:Label>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblProgrammazione" CssClass="labelDati" runat="server"></asp:Label>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label3" CssClass="label" AssociatedControlID="lblIspettore" runat="server"
                            Text="Verificatore:"></asp:Label>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblIspettore" CssClass="labelDati" runat="server"></asp:Label>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label4" CssClass="label" AssociatedControlID="lblStatoVerifica" runat="server"
                            Text="Stato Verifica:"></asp:Label>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblStatoVerifica" CssClass="labelDati" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row" id="div18" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label26" CssClass="label" AssociatedControlID="lblEnte" runat="server"
                            Text="Ente Proponente:"></asp:Label>
                    </div>
                    <div class="collable" style="width: 75%">
                        <asp:Label ID="lblEnte" CssClass="labelDati" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row" id="div19" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label28" CssClass="label" AssociatedControlID="LblDatiEnte" runat="server"
                            Text="Dati Ente:"></asp:Label>
                    </div>
                    <div class="collable" style="width: 75%">
                        <asp:Label ID="LblDatiEnte" CssClass="labelDati" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row" id="div2" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label5" CssClass="label" AssociatedControlID="lblProgetto" runat="server"
                            Text="Progetto:"></asp:Label>
                    </div>
                    <div class="collable" style="width: 75%">
                        <asp:Label ID="lblProgetto" CssClass="labelDati" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row" id="div3" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label2" CssClass="label" AssociatedControlID="lblDataInizioProgetto"
                            runat="server" Text="Data Inizio Progetto:"></asp:Label>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblDataInizioProgetto" CssClass="labelDati" runat="server"></asp:Label>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label6" CssClass="label" AssociatedControlID="lblDataFineProgetto"
                            runat="server" Text="Data Fine Progetto:"></asp:Label>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblDataFineProgetto" CssClass="labelDati" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row" id="div4" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label7" CssClass="label" AssociatedControlID="lblDataAssegnazione"
                            runat="server" Text="Data Assegnazione:"></asp:Label>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblDataAssegnazione" CssClass="labelDati" runat="server"></asp:Label>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label8" CssClass="label" AssociatedControlID="lblDataApprovazione"
                            runat="server" Text="Data Approvazione:"></asp:Label>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblDataApprovazione" CssClass="labelDati" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row" id="div5" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label9" CssClass="label" AssociatedControlID="lblDataPrevistaVerifica"
                            runat="server" Text="Data Inizio Prevista Verifica:"></asp:Label>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblDataPrevistaVerifica" CssClass="labelDati" runat="server"></asp:Label>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label10" CssClass="label" AssociatedControlID="lblDataFinePrevistaVerifica"
                            runat="server" Text="Data Fine Prevista Verifica:"></asp:Label>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblDataFinePrevistaVerifica" CssClass="labelDati" runat="server"></asp:Label>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label11" CssClass="label" AssociatedControlID="lblTipologiaVerifica"
                            runat="server" Text="Tipologia Verifica:"></asp:Label>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblTipologiaVerifica" CssClass="labelDati" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row" id="div6" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label12" CssClass="label" AssociatedControlID="LblCompetenzaProg"
                            runat="server" Text="Competenza:"></asp:Label>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="LblCompetenzaProg" CssClass="labelDati" runat="server"></asp:Label>
                    </div>
                </div>
               
                <fieldset id="flsInformazioniGenerali" class="ContornoPagina">
                    <legend id="CaratteristicheOrganizzative">Informazioni Sanzione </legend>
                    <div class="wrapper" id="InformazioniGeneraliContent" style="width: 100%; border: 0px">
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label13" CssClass="label" AssociatedControlID="LblDataProtTrasmSanzione"
                                    runat="server" Text="Data Prot. Trasmissione Sanzione:"></asp:Label>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblDataProtTrasmSanzione" CssClass="labelDati" runat="server"></asp:Label>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label14" CssClass="label" AssociatedControlID="LblNumProtTrasmSanzione"
                                    runat="server" Text="N° Prot. Trasmissione Sanzione:"></asp:Label>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblNumProtTrasmSanzione" CssClass="labelDati" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label15" CssClass="label" AssociatedControlID="LblDataProtEsecSanzione"
                                    runat="server" Text="Data Prot. Esecuzione Sanzione:"></asp:Label>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblDataProtEsecSanzione" CssClass="labelDati" runat="server"></asp:Label>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label17" CssClass="label" AssociatedControlID="LblNumProtEsecSanzione"
                                    runat="server" Text="N° Prot. Esecuzione Sanzione:"></asp:Label>
                            </div>
                            <div class="collable" style="width: 15%; height: 13px;">
                                <asp:Label ID="LblNumProtEsecSanzione" CssClass="labelDati" runat="server"></asp:Label>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label16" CssClass="label" AssociatedControlID="LblDataEsecSanzione"
                                    runat="server" Text="Data Esec. Sanzione:"></asp:Label>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblDataEsecSanzione" CssClass="labelDati" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label18" CssClass="label" AssociatedControlID="LblCompetenza" runat="server"
                                    Text="Competenza:"></asp:Label>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblCompetenza" CssClass="labelDati" runat="server"></asp:Label>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label20" CssClass="label" AssociatedControlID="LblUfficio" runat="server"
                                    Text="Ufficio USCN:"></asp:Label>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblUfficio" CssClass="labelDati" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </fieldset>

                <fieldset class="ContornoPaginaScroll">
                    <legend>Sanzione</legend>
                    <asp:Label ID="lblErrore" runat="server" CssClass="msgErrore"></asp:Label>
                    <asp:Label ID="lblmessaggio" runat="server" CssClass="msgConferma"/>
     
                    <div class="wrapper" id="Div20" style="width: 100%; border: 0px">
                             <div class="row" id="div9" runat="server">
                            <div class="collable" style="width: 10%">
                                <asp:Label ID="Label22" CssClass="label" runat="server" AssociatedControlID="ddlSanzione"
                                    Text="Sanzioni:"></asp:Label>
                            </div>
                            <div class="collable" style="width: 60%">
                                <asp:DropDownList ID="ddlSanzione" runat="server" Width="462px" AutoPostBack="true"/>
                            </div>
                            <div class="collable" style="width: 20%">
                                <asp:Button ID="cmdApplica" runat="server" CssClass="Pulsante" Text="Applica" />
                            </div>
                        </div>
                        <div class="row" >
                            <div class="collable" style="width:50%" id="left">
                                <asp:Label ID="Label19" CssClass="label" runat="server" Text="Progetto:"></asp:Label>
                                <asp:DataGrid ID="dgProgetti" runat="server" Width="98%" CssClass="table" AllowSorting="false"
                                    AutoGenerateColumns="False" UseAccessibleHeader="True" Visible="true">
                                    <FooterStyle></FooterStyle>
                                    <SelectedItemStyle BackColor="White"></SelectedItemStyle>
                                    <EditItemStyle></EditItemStyle>
                                    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                                    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                                    <HeaderStyle></HeaderStyle>
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <HeaderStyle HorizontalAlign="Center" Width="0%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkProg" ToolTip="" AutoPostBack="False" runat="server"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn Visible="False" DataField="CodiceProgetto" HeaderText="Codice Progetto">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Titolo" HeaderText="Progetto"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="idattivita"></asp:BoundColumn>
                                        <asp:ButtonColumn Text="&lt;img src=images/lenteIngrandimento_small.png Width=20 Height=20 title=Dettaglio border=0&gt;"
                                            CommandName="Select">
                                            <HeaderStyle Width="0%"></HeaderStyle>
                                        </asp:ButtonColumn>
                                    </Columns>
                                   	<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
                                </asp:DataGrid>
                                <asp:Label ID="Label23" CssClass="label" runat="server" Text="Ente Capofila:"></asp:Label>
                                 <asp:DataGrid ID="dgEnteCapofila" runat="server" Width="98%" CssClass="table" AllowSorting="false"
                                    AutoGenerateColumns="False" UseAccessibleHeader="True" Visible="true">
                                    <FooterStyle></FooterStyle>
                                    <SelectedItemStyle BackColor="White"></SelectedItemStyle>
                                    <EditItemStyle></EditItemStyle>
                                    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                                    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                                    <HeaderStyle></HeaderStyle>
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <HeaderStyle HorizontalAlign="Center" Width="0%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkEnteCap" ToolTip="" AutoPostBack="False" runat="server"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="Denominazione" HeaderText="Ente"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="idente"></asp:BoundColumn>
                                        <asp:ButtonColumn Text="&lt;img src=images/lenteIngrandimento_small.png Width=20 Height=20 title=Dettaglio border=0&gt;"
                                            CommandName="Select">
                                            <HeaderStyle Width="0%"></HeaderStyle>
                                        </asp:ButtonColumn>
                                    </Columns>
                                   	<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
                                </asp:DataGrid>
                                <asp:Label ID="Label24" CssClass="label" runat="server" Text="Ente Dipendente:"></asp:Label>
                                 <asp:DataGrid ID="dgEnteDipendente" runat="server" Width="98%" CssClass="table" AllowSorting="false"
                                    AutoGenerateColumns="False" UseAccessibleHeader="True" Visible="true">
                                    <FooterStyle></FooterStyle>
                                    <SelectedItemStyle BackColor="White"></SelectedItemStyle>
                                    <EditItemStyle></EditItemStyle>
                                    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                                    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                                    <HeaderStyle></HeaderStyle>
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <HeaderStyle HorizontalAlign="Center" Width="0%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkEnteDip" ToolTip="" AutoPostBack="False" runat="server"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="entefiglio" HeaderText="Ente"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="identefiglio" HeaderText="identefiglio">
                                        </asp:BoundColumn>
                                        <asp:ButtonColumn Text="&lt;img src=images/lenteIngrandimento_small.png Width=20 Height=20 title=Dettaglio border=0&gt;"
                                            CommandName="Select">
                                            <HeaderStyle Width="0%"></HeaderStyle>
                                        </asp:ButtonColumn>
                                    </Columns>
                                  	<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
                                </asp:DataGrid>
                                 <asp:Label ID="Label25" CssClass="label" runat="server" Text="Sedi attuazione:"></asp:Label>
                                 <asp:DataGrid ID="dgSediAttuazione" runat="server" Width="98%" CssClass="table" AllowSorting="false"
                                    AutoGenerateColumns="False" UseAccessibleHeader="True" Visible="true">
                                    <FooterStyle></FooterStyle>
                                    <SelectedItemStyle BackColor="White"></SelectedItemStyle>
                                    <EditItemStyle></EditItemStyle>
                                    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                                    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                                    <HeaderStyle></HeaderStyle>
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <HeaderStyle HorizontalAlign="Center" Width="0%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSediAtt" ToolTip="" AutoPostBack="False" runat="server"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="denominazione" HeaderText="Sede Attuazione"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="identesedeattuazione" HeaderText="identesedeattuazione">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="indirizzo" HeaderText="Indirizzo"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="comune" HeaderText="Comune"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Irregolarit&#224;" HeaderText="Irr."></asp:BoundColumn>
                                        <asp:ButtonColumn Text="&lt;img src=images/lenteIngrandimento_small.png Width=20 Height=20 title=Dettaglio border=0&gt;"
                                            CommandName="Select">
                                            <HeaderStyle Width="0%"></HeaderStyle>
                                        </asp:ButtonColumn>
                                    </Columns>
                                   	<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
                                </asp:DataGrid>
                                <asp:DataGrid ID="dgSedi" runat="server" Visible="False" Width="98%" CssClass="table"
                                    AllowSorting="false" AutoGenerateColumns="False" UseAccessibleHeader="True">
                                    <FooterStyle></FooterStyle>
                                    <SelectedItemStyle BackColor="White"></SelectedItemStyle>
                                    <EditItemStyle></EditItemStyle>
                                    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                                    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                                    <HeaderStyle></HeaderStyle>
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <HeaderStyle HorizontalAlign="Center" Width="0%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSedi" ToolTip="" AutoPostBack="False" runat="server"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn Visible="False" DataField="identesede"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="DenEnteSede" HeaderText="Sede"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="indirizzo" HeaderText="Indirizzo"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="comune" HeaderText="Comune"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="telefono" HeaderText="Telefono"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="fax" HeaderText="Fax"></asp:BoundColumn>
                                        <asp:ButtonColumn Text="&lt;img src=images/lenteIngrandimento_small.png Width=20 Height=20 title=Dettaglio border=0&gt;"
                                            CommandName="Select">
                                            <HeaderStyle Width="0%"></HeaderStyle>
                                                     </asp:ButtonColumn>
                                    </Columns>
                                   	<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
                                </asp:DataGrid>
                                <asp:DataGrid ID="dgSediProgetto" runat="server" Visible="False" Width="98%" CssClass="table"
                                    AllowSorting="false" AutoGenerateColumns="False" UseAccessibleHeader="True">
                                    <FooterStyle></FooterStyle>
                                    <SelectedItemStyle BackColor="White"></SelectedItemStyle>
                                    <EditItemStyle></EditItemStyle>
                                    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                                    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                                    <HeaderStyle></HeaderStyle>
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <HeaderStyle HorizontalAlign="Center" Width="0%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSediProg" ToolTip="" AutoPostBack="False" runat="server"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="DenomSede" HeaderText="Sede"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Progetto" HeaderText="Progetto"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="NumeroVolontari" HeaderText="N&#176;.Vol."></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Irregolarit&#224;" HeaderText="Irr."></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="IDAttivit&#224;EnteSedeAttuazione" HeaderText="IDAttivit&#224;EnteSedeAttuazione">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="IDEnteSedeAttuazione" HeaderText="IDEnteSedeAttuazione">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="idAttivit&#224;" HeaderText="idAttivit&#224;">
                                        </asp:BoundColumn>
                                        <asp:ButtonColumn Visible="False" Text="&lt;img src=images/lenteIngrandimento_small.png Width=20 Height=20 title=Dettaglio border=0&gt;"
                                            CommandName="Select">
                                            <HeaderStyle Width="0%"></HeaderStyle>
                                        </asp:ButtonColumn>
                                    </Columns>
                                  	<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
                                </asp:DataGrid>
                            </div>
                            <div class="collable" style="width:50%" id="right">
                                <asp:Label ID="Label21" CssClass="label" runat="server" Text="Sanzioni Applicate:"></asp:Label>
                                <asp:DataGrid ID="dgSanzione" runat="server" Width="98%" CssClass="table" AllowSorting="false"
                                    AutoGenerateColumns="False" UseAccessibleHeader="True" Visible="true">
                                    <FooterStyle></FooterStyle>
                                    <SelectedItemStyle BackColor="White"></SelectedItemStyle>
                                    <EditItemStyle></EditItemStyle>
                                    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                                    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                                    <HeaderStyle></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="Sanzione" HeaderText="Sanzione"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Tipo" HeaderText="Tipo"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Soggetto" HeaderText="Soggetto"></asp:BoundColumn>
                                        <asp:ButtonColumn Text="&lt;img src=images/canc_small.png Width=20 Height=20 title=Elimina Sanzione' border=0&gt;"
                                            CommandName="Delete"></asp:ButtonColumn>
                                        <asp:BoundColumn Visible="False" DataField="IdAttivita" HeaderText="IdAttivita">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="IdEnte" HeaderText="IdEnte"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="IdEnteSede" HeaderText="IdEnteSede">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="IdEnteSedeAttuazione" HeaderText="IdEnteSedeAttuazione">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="IDAttivit&#224;EnteSedeAttuazione" HeaderText="IDAttivit&#224;EnteSedeAttuazione">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="IdSanzione" HeaderText="IdSanzione">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Stato" HeaderText="Stato"></asp:BoundColumn>
                                        <asp:ButtonColumn Text="&lt;img src=images/cronologia_small.png Width=20 Height=20 title=Annulla Sanzione' border=0&gt;"
                                            CommandName="Annulla"></asp:ButtonColumn>
                                    </Columns>
                                 	<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
                                </asp:DataGrid>
                                
                            </div>
                            
                        </div>


<%--
                        <div class="row" id="div7" runat="server">
                            <div class="collable" style="width: 50%">
                                
                            </div>
                            <div class="collable" style="width: 50%">
                                
                            </div>
                        </div>
                        <div class="row" id="div8" runat="server">
                            <div class="collable" style="width: 50%; height: 44px;">
                                
                            </div>
                            <div class="collable" style="width: 50%">
                                
                            </div>
                        </div>
                         <div class="RigaVuota">
                        &nbsp;</div>
                        <div class="row" id="div10" runat="server">
                            <div class="collable" style="width: 50%">
                                
                            </div>
                            <div class="collable" style="width: 50%">
                            </div>
                        </div>
                        <div class="row" id="div11" runat="server">
                            <div class="collable" style="width: 50%; height: 44px;">
                               
                            </div>
                            <div class="collable" style="width: 50%">
                            </div>
                        </div>
                         <div class="RigaVuota">
                        &nbsp;</div>
                        <div class="row" id="div12" runat="server">
                            <div class="collable" style="width: 50%">
                                
                            </div>
                            <div class="collable" style="width: 50%">
                            </div>
                        </div>
                        <div class="row" id="div13" runat="server">
                            <div class="collable" style="width: 50%; height: 44px;">
                               
                            </div>
                            <div class="collable" style="width: 50%">
                            </div>
                        </div>
                         <div class="RigaVuota">
                        &nbsp;</div>
                        <div class="row" id="div14" runat="server">
                            <div class="collable" style="width: 50%">
                               
                            </div>
                            <div class="collable" style="width: 50%">
                            </div>
                        </div>
                        <div class="row" id="div15" runat="server">
                            <div class="collable" style="width: 50%; height: 44px;">
                                
                            </div>
                            <div class="collable" style="width: 50%">
                            </div>
                        </div>
                        <div class="row" id="div16" runat="server">
                            <div class="collable" style="width: 50%; height: 44px;">
                                
                           
                            </div>
                            <div class="collable" style="width: 50%">
                            </div>
                        </div>
                        <div class="row" id="div17" runat="server">
                            <div class="collable" style="width: 50%; height: 44px;"">
                                
                            </div>
                            <div class="collable" style="width: 50%">
                            </div>
                        </div>--%>
                         <div class="RigaVuota">
                        &nbsp;</div>
                        <div class="RigaPulsanti">
                            <asp:Button ID="cmdSalva" runat="server" CssClass="Pulsante" Text="Salva" />
                            <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
                        </div>
                    </div>
                </fieldset>
            </div>
        </fieldset>
    </div>
  
    <input style="z-index: 101; position: absolute; width: 32px; height: 22px; top: 1864px;
        left: 56px" id="hddControllo" value="0" size="1" type="hidden" runat="server" />
                    </div>
            </div>
</asp:Content>
