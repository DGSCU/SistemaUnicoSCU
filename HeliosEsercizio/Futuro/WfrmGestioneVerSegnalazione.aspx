<%@ Page Title="Inserimento Verifiche su Segnalazione" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"  CodeBehind="WfrmGestioneVerSegnalazione.aspx.vb" Inherits="Futuro.WfrmGestioneVerSegnalazione" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
    <script type="text/javascript">
            /* <![CDATA[ */
            $(function () {
                var clientId = "#" + CostruisciId('txtDataProtocollo');
                    $("" + clientId + "").datepicker();
            });
                $(function () {
                var clientId = "#" + CostruisciId('txtDataSegnalazione');
                    $("" + clientId + "").datepicker();
            });
                $(function () {
                var clientId = "#" + CostruisciId('txtLetteraDataProt'); 
                    $("" + clientId + "").datepicker();
            });
                $(function () {
                var clientId = "#" + CostruisciId('txtRispostaDataProt');
                    $("" + clientId + "").datepicker();
            });


                    /* ]]> */
    </script>


           <script  type="text/javascript">
         /* <![CDATA[ */
        var formId;
        var prefissoIdClient = "MainContent_";

	    
	    function ImpostaFormId() {
	        formId = document.forms[0].id;
	        return formId;
	    }
        function CostruisciId(IdServer) {
           var IdClient = prefissoIdClient + IdServer
           return IdClient
        }
          
                    /* ]]> */
    </script>


    <script type="text/javascript">


        function caricaIDComune() {
            try {
                frmMain.txtIDComuneNascita.value = AppoID.value;
            }
            catch (e) {
            }
        }
    </script>
    <script type="text/javascript">
        function fncmdSelProtocollo0() {

            var CodiceFasc = $('#<%= txtNumeroFascicolo.ClientID %>').val();
            if ($('#<%= txtNumeroFascicolo.ClientID %>').val() == '') {
                alert("Specificare il numero fascicolo!");

            } else {
                myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?NumeroFascicolo=" + CodiceFasc, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");
            }
            return false;
        }
    </script>
    <script type="text/javascript">

        function fncmdSelFascicolo() {
            var CodiceFasc = $('#<%= txtNumeroFascicolo.ClientID %>').val();



            var IdtxtNumeroFascicolo = CostruisciId("txtNumeroFascicolo");
            var IdTxtCodiceFasc = CostruisciId("TxtCodiceFasc");
            var IdtxtDescFasc = CostruisciId("txtDescFasc");


            var winOption = PopUpOption();
            formId = document.forms[0];
            myWin = window.open("WfrmSIGEDElencoFascicoli.aspx?Processo=MONITORAGGIO&objForm=" + formId + "&objNumero="+IdtxtNumeroFascicolo+"&objCodice="+IdTxtCodiceFasc+"&objDescFasc="+IdtxtDescFasc+"&IdFascicolo=" + CodiceFasc, "Visualizza", "width=800,height=350,dependent=no,scrollbars=yes,status=no");
            return false;
        }

    </script>
    <script type="text/javascript">
        function fncmdAllegati() {
            var CodiceFasc = $('#<%= txtNumeroFascicolo.ClientID %>').val();
            if ($('#<%= txtNumeroFascicolo.ClientID %>').val() == '') {
                alert("Specificare il numero fascicolo!");

            } else {
                var Nprot = $('#<%= txtNumeroProtocollo.ClientID %>').val();
                var Dataprot = $('#<%= txtDataProtocollo.ClientID %>').val();
                if ($('#<%= txtNumeroProtocollo.ClientID %>').val() == '') {
                    alert("Indicare il numero protocollo!");
                } else if ($('#<%= txtDataProtocollo.ClientID %>').val() == '') {
                    alert("Indicare la data protocollo!");
                } else {
                    myWin = window.open("WfrmSIGEDElencoAllegati.aspx?NumeroProtocollo=" + Nprot + "&DataProtocollo=" + Dataprot + "&CodiceFascicolo=" + CodiceFasc, "Visualizza", "width=800,height=350,dependent=no,scrollbars=yes,status=no");

                }
            }
        }
    </script>
    <script type="text/javascript">
        function fncmdSelProtocollo() {
         

             

            var CodiceFasc = document.getElementById(CostruisciId("TxtCodiceFasc")).value;

            TxtProt = CostruisciId("txtNumeroProtocollo");
            TxtData = CostruisciId("txtDataProtocollo");
            formId = document.forms[0];
            if ($('#<%= txtNumeroFascicolo.ClientID %>').val() == '') {
                alert("Specificare il numero fascicolo!");

            } else {
                myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?objForm="+formId+"&TxtProt="+TxtProt+"&TxtData="+TxtData+"&NumeroFascicolo=" + CodiceFasc, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");
               // myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?objForm=frmMain&TxtProt=txtNumeroProtocollo&TxtData=txtDataProtocollo&NumeroFascicolo=" + CodiceFasc, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");

            }
            return false;
        }
 
        function fncmdSelProtocollo1() {
            var CodiceFasc = document.getElementById(CostruisciId("TxtCodiceFasc")).value;
            TxtProt = CostruisciId("txtLetteraNProt");
            TxtData = CostruisciId("txtLetteraDataProt");

        

          
            formId = document.forms[0];
            if ($('#<%= txtNumeroFascicolo.ClientID %>').val() == '') {
                alert("Specificare il numero fascicolo!");

            } else {

                myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?objForm=" + formId + "&TxtProt=" + TxtProt + "&TxtData=" + TxtData + "&NumeroFascicolo=" + CodiceFasc, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");
              
            }
            return false;
        }
    </script>
    <script type="text/javascript">
        function fncmdSelProtocollo2() {
            var CodiceFasc = document.getElementById(CostruisciId("TxtCodiceFasc")).value;
            TxtProt = CostruisciId("txtRispostaNProt");
            TxtData = CostruisciId("txtRispostaDataProt");




            formId = document.forms[0];
            if ($('#<%= txtNumeroFascicolo.ClientID %>').val() == '') {
                alert("Specificare il numero fascicolo!");

            } else {

                myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?objForm=" + formId + "&TxtProt=" + TxtProt + "&TxtData=" + TxtData + "&NumeroFascicolo=" + CodiceFasc, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");

            }
            return false;
        }
    </script>
    <script type="text/javascript">
        function fncmdAllegati1() {
            var CodiceFasc = $('#<%= TxtCodiceFasc.ClientID %>').val();
            if ($('#<%= txtNumeroFascicolo.ClientID %>').val() == '') {
                alert("Specificare il codice fascicolo!");

            } else {

                var Nprot = $('#<%= txtLetteraNProt.ClientID %>').val();
                var Dataprot = $('#<%= txtLetteraDataProt.ClientID %>').val();
                if ($('#<%= txtLetteraNProt.ClientID %>').val() == '') {
                    alert("Indicare il numero protocollo!");
                } else if ($('#<%= txtLetteraDataProt.ClientID %>').val() == '') {
                    alert("Indicare la data protocollo!");
                } else {

                    myWin = window.open("WfrmSIGEDElencoAllegati.aspx?NumeroProtocollo=" + Nprot + "&DataProtocollo=" + Dataprot + "&CodiceFascicolo=" + CodiceFasc, "Visualizza", "width=800,height=350,dependent=no,scrollbars=yes,status=no");

                }
            }
            return false;
        }
    </script>
    <script type="text/javascript">
        function fncmdAllegati2() {

            var CodiceFasc = $('#<%= TxtCodiceFasc.ClientID %>').val();
            if ($('#<%= txtNumeroFascicolo.ClientID %>').val() == '') {
                alert("Specificare il codice fascicolo!");

            } else {

                var Nprot = $('#<%= txtRispostaNProt.ClientID %>').val();
                var Dataprot = $('#<%= txtRispostaDataProt.ClientID %>').val();
                if ($('#<%= txtRispostaNProt.ClientID %>').val() == '') {
                    alert("Indicare il numero protocollo!");
                } else if ($('#<%= txtRispostaDataProt.ClientID %>').val() == '') {
                    alert("Indicare la data protocollo!");
                } else {


                    myWin = window.open("WfrmSIGEDElencoAllegati.aspx?NumeroProtocollo=" + Nprot + "&DataProtocollo=" + Dataprot + "&CodiceFascicolo=" + CodiceFasc, "Visualizza", "width=800,height=350,dependent=no,scrollbars=yes,status=no");


                }
            } return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="firefox">
        <fieldset class="ContornoPaginaScroll">
            <legend>Inserimento Verifica su segnalazione</legend>
            <div class="wrapper" style="width: 100%; border: 0">
                <div class="headers">
                    <h2>
                        <asp:Label ID="lblTitolo" runat="server" Text="Inserimento Verifica su segnalazione"></asp:Label></h2>
                </div>
                <div class="rowGroup" style="height: auto">
                    <asp:Label ID="lblErrore" runat="server" CssClass="msgErrore"></asp:Label>
                    <asp:Label ID="lblmessaggiosopra" runat="server" CssClass="msgConferma"></asp:Label>
                    <div class="RigaVuota">
                        &nbsp;<asp:HiddenField ID="TxtCodiceFasc" runat="server" />
                    </div>
                </div>
                <fieldset class="ContornoPagina"> 
                    <legend>Informazioni Generali</legend>
                    <div class="wrapper" id="CaratteristicheOrganizzativeContent" style="border-style: none;
                        border-color: inherit; border-width: 0px; width: 100%; margin-top: 0px;">
                        <div class="row" id="div1" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblNumFascicolo" CssClass="label" AssociatedControlID="txtNumeroFascicolo"
                                    runat="server" Text="Numero Fascicolo:"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 60%">
                                <asp:TextBox ID="txtNumeroFascicolo" runat="server" CssClass="textbox" ></asp:TextBox>
                            </div>
                            <div class="collable" style="width:20%">
                                <asp:ImageButton Style="cursor: hand" ID="cmdSelFascicolo" runat="server" 
                                    ImageUrl="images/cartella_small.png" OnClientClick="return fncmdSelFascicolo();" Width="30px"  AlternateText="Seleziona Codice Fascicolo" ToolTip="Seleziona Codice Fascicolo"></asp:ImageButton>
                                <asp:ImageButton Style="cursor: hand" ID="cmdSelProtocollo0" runat="server" ImageUrl="images/busta_small.png" ToolTip="Elenco Documenti"    AlternateText="Elenco Documenti"
                                    OnClientClick="return fncmdSelProtocollo0();" Width="30px"></asp:ImageButton>
                                <asp:ImageButton Style="cursor: hand" ID="cmdFascCanc" runat="server" ImageUrl="images/canc_small.png" AlternateText="Rimuovi Fascicolo/Protocolli"
                                    ToolTip="Rimuovi Fascicolo/Protocolli" Width="30px"></asp:ImageButton>
                            </div>
                         </div>
                         <div class="row" id="div2" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblTipologia" CssClass="label" AssociatedControlID="txtDescFasc" runat="server"
                                    Text="Descr. Fascicolo:"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 85%">
                                <asp:TextBox ID="txtDescFasc" runat="server" CssClass="textboxData" 
                                    MaxLength="200" Width="100%"></asp:TextBox>
                            </div>
                        </div>
                         <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblNumProtocollo" CssClass="label" AssociatedControlID="txtNumeroProtocollo"
                                    runat="server" Text="Num. Prot. Segnalazione:"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 25%">
                                <asp:TextBox ID="txtNumeroProtocollo" runat="server" CssClass="textbox" ></asp:TextBox>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblDataProtocollo" CssClass="label" AssociatedControlID="txtDataProtocollo"
                                    runat="server" Text="Data Prot. Segnalazione:"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 25%">
                                <asp:TextBox ID="txtDataProtocollo" runat="server" CssClass="textboxData"> </asp:TextBox>
                            </div>
                            <div class="collable" style="width: 20%">
                                 <asp:ImageButton Style="cursor: hand" ID="cmdSelProtocollo" runat="server" ImageUrl="images/busta_small.png"
                                    ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo"
                                    OnClientClick="return fncmdSelProtocollo();" Width="30px"></asp:ImageButton>
                                    <asp:ImageButton  Style="cursor: hand" ID="cmdAllegati" runat="server" ImageUrl="images/documento_small.png"
                                        ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati"  OnClientClick="return fncmdAllegati();" 
                                    Width="30px"></asp:ImageButton>
                            </div>
                        </div>
                        
                        <div class="row" id="div3" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblEsitoSegnalazione" CssClass="label" AssociatedControlID="ddEsitoSegnalazione"
                                    runat="server" Text="Esito Segnalazione:"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:DropDownList ID="ddEsitoSegnalazione" runat="server" AutoPostBack="False" CssClass="ddlClass">
                                    <asp:ListItem Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="0">Registrata</asp:ListItem>
                                    <asp:ListItem Value="1">Infondata</asp:ListItem>
                                    <asp:ListItem Value="2">Trasmessa altro Servizio</asp:ListItem>
                                    <asp:ListItem Value="3">Archiviata</asp:ListItem>
                                    <asp:ListItem Value="4">Accettata</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="collable" style="width: 10%">
                                <asp:Label ID="LblCompetenza" CssClass="label" AssociatedControlID="ddlCompetenza"
                                    runat="server" Text="Competenza:"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:DropDownList ID="ddlCompetenza" runat="server" AutoPostBack="true" CssClass="ddlClass">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row" id="div4" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblFonte" CssClass="label" AssociatedControlID="ddFonte" runat="server" Text="Fonte:"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:DropDownList ID="ddFonte" runat="server" AutoPostBack="False" CssClass="ddlClass">
                                    <asp:ListItem Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="0">Interno</asp:ListItem>
                                    <asp:ListItem Value="1">IGF</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="collable" style="width: 10%">
                                <asp:Label ID="LblDataSegnalazione" CssClass="label" AssociatedControlID="txtDataSegnalazione"
                                    runat="server" Text="Data segnalazione:"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 10%">
                                <asp:TextBox ID="txtDataSegnalazione" runat="server" CssClass="textboxData">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row" id="div5" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblOggetto" CssClass="label" AssociatedControlID="txtOggetto" runat="server"
                                    Text="Oggetto:"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 85%">
                                <asp:TextBox ID="txtOggetto" runat="server" CssClass="textbox" MaxLength="200" Width="100%"
                                    TextMode="MultiLine">
                                </asp:TextBox>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <fieldset class="ContornoPagina">
                    <legend>Lettera interlocutoria</legend>
                    <div class="wrapper" id="Div9" style="border-style: none; border-color: inherit;
                        border-width: 0px; width: 100%; margin-top: 0px;">
                        <div class="row" id="div6" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblNumProtLettInterlocutoria" CssClass="label" AssociatedControlID="txtLetteraNProt"
                                    runat="server" Text="Numero Protocollo:"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:TextBox ID="txtLetteraNProt" runat="server" CssClass="textbox" MaxLength="50"
                                    Style="width: 60%" ReadOnly="True">  </asp:TextBox>
                                <asp:ImageButton Style="cursor: hand" ID="cmdSelProtocollo1" runat="server" 
                                    ImageUrl="images/busta_small.png" Visible="false" 
                                    ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo"
                                    OnClientClick="return fncmdSelProtocollo1();" Width="30px"></asp:ImageButton>
                                    <asp:ImageButton Style="cursor: hand" ID="cmdAllegati1" runat="server" 
                                    ImageUrl="images/documento_small.png" Visible="false" 
                                        ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati"  OnClientClick="return fncmdAllegati1();" 
                                    Width="30px"></asp:ImageButton>
                            </div>
                            <div class="collable" style="width: 10%">
                                <asp:Label ID="LblDataProtLettInterlocutoria" CssClass="label" AssociatedControlID="txtLetteraDataProt"
                                    runat="server" Text="Data Protocollo:"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="txtLetteraDataProt" runat="server" CssClass="textboxData" ReadOnly="True" Enabled="false"
                                    Style="width: 60%">   </asp:TextBox>
                                &nbsp;&nbsp;<asp:ImageButton ID="CmdLetteraInterlocutoria" runat="server" 
                                    ImageUrl="images//stampa_small.png" Visible="false" AlternateText="Lettera Trasmissione Interlocutoria"
                                    ToolTip="Lettera Trasmissione Interlocutoria" Width="30px"></asp:ImageButton>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <fieldset class="ContornoPagina">
                    <legend>Risposta alla lettera interlocutoria</legend>
                    <div class="wrapper" id="Div11" style="border-style: none; border-color: inherit;
                        border-width: 0px; width: 100%; margin-top: 0px;">
                        <div class="row" id="div7" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblNumProtRispostaLettInterlocutoria" CssClass="label" AssociatedControlID="txtRispostaNProt"
                                    runat="server" Text="Numero Protocollo:"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:TextBox ID="txtRispostaNProt" runat="server" CssClass="textbox" MaxLength="50"
                                    Style="width: 60%" ReadOnly="True">  </asp:TextBox>
                                <asp:ImageButton Style="z-index: 0; cursor: hand" ID="cmdSelProtocollo2" 
                                    runat="server" Visible="false" AlternateText="Seleziona Protocollo" 
                                    ImageUrl="images//busta_small.png" ToolTip="Seleziona Protocollo" 
                                    OnClientClick="return fncmdSelProtocollo2();" ImageAlign="Top">
                                </asp:ImageButton><asp:ImageButton Style="cursor: hand" ID="cmdAllegati2" 
                                    runat="server" Visible="false" ImageUrl="images/documento_small.png" 
                                    ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" OnClientClick="return fncmdAllegati2();" 
                                    Width="30px">
                                </asp:ImageButton>
                            </div>
                            <div class="collable" style="width: 10%">
                                <asp:Label ID="LblDataProtRispostaLettInterlocutoria" CssClass="label" AssociatedControlID="txtRispostaDataProt"
                                    runat="server" Text="Data Protocollo:"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 10%">
                                <asp:TextBox ID="txtRispostaDataProt" runat="server" CssClass="textboxData" ReadOnly="True" Enabled="false" >   </asp:TextBox>
                            </div>
                        </div>
                        <div class="row" id="div8" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblNote" CssClass="label" AssociatedControlID="txtNote" runat="server"
                                    Text="Note:"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 85%">
                                <asp:TextBox ID="txtNote" runat="server" CssClass="textbox" MaxLength="1000" Width="100%"
                                    TextMode="MultiLine">
                                </asp:TextBox>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <div class="row" id="div10" runat="server">
                    <div class="collable">
                        <asp:LinkButton ID="lBttRicercaAttivitaEnteSedeAttuazione" runat="server" Text="Ricerca Sede di Progetto"></asp:LinkButton>
                    </div>
                </div>
                <div class="RigaPulsanti">
                    <asp:Button ID="cmdConferma" CssClass="Pulsante" runat="server" Text="Salva">
                    </asp:Button>
                    <asp:Button ID="imgChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
                </div>
                <div class="wrapper" id="Div13" style="width: 100%; border: 0px">
                    <div class="row" id="div14" runat="server">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="lblMsgSede" CssClass="label" Visible="false" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    
                    <asp:DataGrid ID="dgRisultatoRicerca" runat="server" Width="100%" AllowPaging="True"
                        PageSize="5" CssClass="table" AllowSorting="True" AutoGenerateColumns="False"
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
                            <asp:ButtonColumn Visible="False" CommandName="Select"></asp:ButtonColumn>
                            <asp:ButtonColumn Visible="False" CommandName="accettazione"></asp:ButtonColumn>
                            <asp:ButtonColumn Visible="False" CommandName="valutazione"></asp:ButtonColumn>
                            <asp:BoundColumn DataField="CodiceEnte" HeaderText="Codice Ente"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Denominazione" HeaderText="Ente Proponente"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="idattivit&#224;entesedeattuazione" HeaderText="idattivit&#224;entesedeattuazione">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="codiceProgetto" HeaderText="Codice Progetto"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Titolo" HeaderText="Titolo">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ambito" HeaderText="Settore / Ambito"></asp:BoundColumn>
                            <asp:BoundColumn DataField="datafineattivit&#224;" HeaderText="Data Fine Progetto"
                                DataFormatString="{0:dd/MM/yyyy}">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="IDEnteSedeAttuazione" Visible="false" HeaderText="Codice Sede Attuazione">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="EnteFiglio" Visible="false" HeaderText="Ente Figlio">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Unico" HeaderText="Sede Attuazione"></asp:BoundColumn>
                            <asp:BoundColumn DataField="NumeroVolontari" HeaderText="Num. Volontari"></asp:BoundColumn>
                            <asp:BoundColumn DataField="comune1" HeaderText="Comune"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Provincia" Visible="False" HeaderText="Provincia"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Regione" HeaderText="Regione"></asp:BoundColumn>
                            <asp:BoundColumn DataField="IDAttività" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="idente" Visible="False"></asp:BoundColumn>


                        </Columns>
                         <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center"
                            CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                </div>
            </div>
        </fieldset>
    </div>
</asp:Content>
