<%@ Page Title="Modifica Verifica su Segnalazione" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="WfrmGestioneVerModSegnalazione.aspx.vb" Inherits="Futuro.WfrmGestioneVerModSegnalazione" %>

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
             $(function () {
                var clientId = "#" + CostruisciId('txtDataProtocollo');
                    $("" + clientId + "").datepicker();

                 /* ]]> */
    </script>
 <%--   <script type="text/javascript">
        function fncmdAssegna() {

            window.open('Ver_PopupAssegnaPerSegnalazioni.aspx?IdVerifica=<%=idVerifica%>&idSegnalazione=<%=Request.QueryString("idsegnalazione")%>', 'SEGNA', 'height=200,width=720,dependent=no,scrollbars=no,status=no,resizable=no');
            return false;
       }
    </script>--%>
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
//            var CodiceFasc = $('#<%= txtNumeroFascicolo.ClientID %>').val();
//            myWin = window.open("WfrmSIGEDElencoFascicoli.aspx?Processo=MONITORAGGIO&objForm=frmMain&objNumero=txtNumeroFascicolo&objCodice=TxtCodiceFasc&objDescFasc=txtDescFasc&IdFascicolo=" + CodiceFasc, "Visualizza", "width=800,height=350,dependent=no,scrollbars=yes,status=no");
//            return false;


            var CodiceFasc = $('#<%= txtNumeroFascicolo.ClientID %>').val();



            var IdtxtNumeroFascicolo = CostruisciId("txtNumeroFascicolo");
            var IdTxtCodiceFasc = CostruisciId("TxtCodiceFasc");
            var IdtxtDescFasc = CostruisciId("txtDescFasc");


            var winOption = PopUpOption();
            formId = document.forms[0];
            myWin = window.open("WfrmSIGEDElencoFascicoli.aspx?Processo=MONITORAGGIO&objForm=" + formId + "&objNumero=" + IdtxtNumeroFascicolo + "&objCodice=" + IdTxtCodiceFasc + "&objDescFasc=" + IdtxtDescFasc + "&IdFascicolo=" + CodiceFasc, "Visualizza", "width=800,height=350,dependent=no,scrollbars=yes,status=no");
            return false;

        }
    </script>
    <script type="text/javascript">

        var prefissoIdClient = "MainContent_";

        function CostruisciId(IdServer) {
            var IdClient = prefissoIdClient + IdServer;
            return IdClient;
        };

  
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
            var CodiceFasc = $('#<%= TxtCodiceFasc.ClientID %>').val();


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
    </script>
    <script type="text/javascript">
 


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

   <%-- </script>
    <script type="text/javascript">--%>


          function  fncmdSelProtocollo2() {
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
            }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="firefox">
        <fieldset class="ContornoPaginaScroll">
            <legend>Modifica Verifica su segnalazione</legend>
            <div class="wrapper" style="width: 100%">
                <div class="headers">
                    <h2>
                        <asp:Label ID="lblTitolo" runat="server" Text="Modifica Verifica su segnalazione"></asp:Label></h2>
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
                            <div class="colOggetti" style="width: 55%">
                                <asp:TextBox ID="txtNumeroFascicolo" runat="server" CssClass="textbox"  Style="width: 60%" MaxLength="50"/>
                            </div>
                            <div class="colOggetti" style="width: 30%">
                                <asp:ImageButton ID="cmdSelFascicolo" runat="server" Width="30px" ImageUrl="images/cartella_small.png"  AlternateText="Seleziona Codice Fascicolo"  ToolTip="Seleziona Codice Fascicolo"  OnClientClick="return fncmdSelFascicolo();"/>
                                <asp:ImageButton ID="cmdSelProtocollo0" runat="server" Width="30px" ImageUrl="images/busta_small.png"  AlternateText="Elenco Documenti"  ToolTip="Elenco Documenti" OnClientClick="return fncmdSelProtocollo0();"/>
                                <asp:ImageButton ID="cmdFascCanc" runat="server" Width="30px" ImageUrl="images/canc_small.png" AlternateText="Rimuovi Fascicolo/Protocolli"  ToolTip="Rimuovi Fascicolo/Protocolli"/>
                            </div>
                           
                        </div>
                        <div class="row" id="div2" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblDescFasc" CssClass="label" AssociatedControlID="txtDescFasc" runat="server"
                                    Text="Descr. Fascicolo:"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 85%">
                                <asp:TextBox ID="txtDescFasc" runat="server" CssClass="textbox" ReadOnly="True" Width="100%">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblNumProtocollo" CssClass="label" AssociatedControlID="txtNumeroProtocollo"
                                    runat="server" Text="Num. Prot. Segnalazione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 25%">
                                <asp:TextBox ID="txtNumeroProtocollo" runat="server" CssClass="textbox" MaxLength="50"
                                    Style="width: 60%" ></asp:TextBox>
                               
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblDataProtocollo" CssClass="label" AssociatedControlID="txtDataProtocollo"
                                    runat="server" Text="Data Prot. Segnalazione:"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 20%">
                                <asp:TextBox ID="txtDataProtocollo" runat="server" CssClass="textboxData">
                                </asp:TextBox>
                            </div>
                            <div class="collable" style="width: 20%">
                                <asp:ImageButton ID="cmdSelProtocollo" runat="server" Width="30px" ImageUrl="images/busta_small.png" ToolTip="Seleziona Protocollo"  AlternateText="Seleziona Protocollo"  OnClientClick="return fncmdSelProtocollo();"/>
                                <asp:ImageButton ID="cmdAllegati" runat="server" Width="30px" ImageUrl="images/documento_small.png" ToolTip="Visualizza Allegati"  AlternateText="Visualizza Allegati"   OnClientClick="return fncmdAllegati();"/>
                            </div>
                        </div>
                        <div class="row" id="div3" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label1" CssClass="label" AssociatedControlID="ddEsitoSegnalazione"
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
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label2" CssClass="label" AssociatedControlID="ddlCompetenza" runat="server"
                                    Text="Competenza:"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:DropDownList ID="ddlCompetenza" runat="server" AutoPostBack="True" CssClass="ddlClass">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row" id="div4" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label3" CssClass="label" AssociatedControlID="ddFonte" runat="server"
                                    Text="Fonte:"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:DropDownList ID="ddFonte" runat="server" AutoPostBack="False" CssClass="ddlClass">
                                    <asp:ListItem Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="0">Interno</asp:ListItem>
                                    <asp:ListItem Value="1">IGF</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="collable" style="width: 10%">
                                <asp:Label ID="Label4" CssClass="label" AssociatedControlID="txtDataSegnalazione"
                                    runat="server" Text="Data segnalazione:"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 10%">
                                <asp:TextBox ID="txtDataSegnalazione" runat="server" CssClass="textboxData">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row" id="div5" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label5" CssClass="label" AssociatedControlID="txtOggetto" runat="server"
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
                       <div class="wrapper" id="Div15" style="border-style: none;
                        border-color: inherit; border-width: 0px; width: 100%; margin-top: 0px;">
                    <div class="row" id="div6" runat="server">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label6" CssClass="label" AssociatedControlID="txtLetteraNProt" runat="server"
                                Text="Numero Protocollo:"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 25%">
                            <asp:TextBox ID="txtLetteraNProt" runat="server" CssClass="textbox" MaxLength="50" Style="width: 70%" ReadOnly="True"/>
                        </div>
                        <div class="colOggetti" style="width: 10%">
                            <asp:ImageButton Style="cursor: hand" ID="cmdSelProtocollo1" runat="server" Width="30px" ImageUrl="images/busta_small.png" ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo" OnClientClick="return fncmdSelProtocollo1();"/>
                            <asp:ImageButton Style="cursor: hand" ID="cmdAllegati1" runat="server" Width="30px" ImageUrl="images/documento_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati"  OnClientClick="return fncmdAllegati1();"/>
                        </div>
                        <div class="collable" style="width: 20%">
                            <asp:Label ID="Label7" CssClass="label" AssociatedControlID="txtLetteraDataProt"
                                runat="server" Text="Data Protocollo:"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 20%">
                            <asp:TextBox ID="txtLetteraDataProt" runat="server" CssClass="textboxData" ReadOnly="True">   </asp:TextBox>
                        </div>
                         <div class="colOggetti" style="width: 10%">
                             <asp:ImageButton ID="CmdLetteraInterlocutoria" runat="server" 
                                 ImageUrl="images/stampa_small.png" Width="30px" 
                                 AlternateText="Lettera Interlocutoria" ToolTip="Lettera Interlocutoria"/>
                        </div>
                    </div>
                    </div> 
                </fieldset>
                <fieldset class="ContornoPagina">
                    <legend>Risposta alla lettera interlocutoria</legend>
                       <div class="wrapper" id="Div16" style="border-style: none;
                        border-color: inherit; border-width: 0px; width: 100%; margin-top: 0px;">
                    <div class="row" id="div7" runat="server">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label8" CssClass="label" AssociatedControlID="txtRispostaNProt" runat="server"
                                Text="Numero Protocollo:"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 20%">
                            <asp:TextBox ID="txtRispostaNProt" runat="server" CssClass="textbox" MaxLength="50" Style="width: 70%" ReadOnly="True" />
                        </div>
                        <div class="colOggetti" style="width: 10%">
                            <asp:ImageButton Style="cursor: hand" ID="cmdSelProtocollo2" runat="server"  Width="30px" ImageUrl="images/busta_small.png" ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo" OnClientClick="return fncmdSelProtocollo2();"/>
                            <asp:ImageButton Style="cursor: hand" ID="cmdAllegati2" runat="server" Width="30px" ImageUrl="images/documento_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" OnClientClick="return fncmdAllegati2();"/>
                        </div>
                        <div class="collable" style="width: 20%">
                            <asp:Label ID="Label9" CssClass="label" AssociatedControlID="txtRispostaDataProt"
                                runat="server" Text="Data Protocollo:"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 20%">
                            <asp:TextBox ID="txtRispostaDataProt" runat="server" CssClass="textboxData" ReadOnly="True">   </asp:TextBox>
                        </div>
                    </div>
                    <div class="row" id="div8" runat="server">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label10" CssClass="label" AssociatedControlID="txtNote" runat="server"
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
                <div class="row" id="div9" runat="server">
                    <div class="collable">
                        <asp:LinkButton ID="lBttRicercaAttivitaEnteSedeAttuazione" runat="server" Text="Ricerca Sede di Progetto"></asp:LinkButton>
                    </div>
                </div>
                <div id="dvAssegnazione" visible="false" runat="server" >
                    <fieldset class="ContornoPagina">
                    <legend>Assegnazione</legend>
                    <div class="row" id="div10" runat="server">
                        <div class="collable" style="width: 20%">
                            <asp:Label ID="Lblverificatore" CssClass="label" runat="server" AssociatedControlID="ddlVerificatori" >Verificatore</asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 40%"> 
                            <asp:DropDownList ID="ddlVerificatori" runat="server" AutoPostBack="True" CssClass="ddlClass"/>
                        </div>
                        <div class="collable" style="width: 40%">
                            <asp:Label ID="LblCirc" CssClass="label" runat="server"  AssociatedControlID="LblCirc"></asp:Label>
                        </div>
                        
                     </div>
                    <div class="row" id="div11" runat="server">
                        <div class="collable" style="width: 20%">
                            <asp:Label ID="lblDataInizioASS" CssClass="label" runat="server" AssociatedControlID="txtDataInizioASS">Data Inizio Prevista Verifica</asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 30%">
                            <asp:TextBox ID="txtDataInizioASS" CssClass="textbox" runat="server" MaxLength="10"></asp:TextBox>
                        </div>
                        <div class="collable" style="width: 20%">
                            <asp:Label ID="lblDataFineASS" CssClass="label" runat="server"  AssociatedControlID="TxtDataFineASS">Data Fine Prevista Verifica</asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 30%">
                            <asp:TextBox ID="TxtDataFineASS" CssClass="textbox" runat="server" MaxLength="10"></asp:TextBox>
                        </div>            
                    </div>
                    <div class="row" id="div12" runat="server">
            
                        <div class="collable" style="width: 45%">
                            <asp:Label ID="LblAssegna" CssClass="label" runat="server" AssociatedControlID="lblaccredita"></asp:Label>
                        </div>
                        <div class="collable" style="width: 45%">
                            <asp:Label ID="lblaccredita" CssClass="label" runat="server"></asp:Label>
                        </div>
                    </div>
                </fieldset>
                 </div>
                <div class="RigaPulsanti">
                    <asp:Button ID="cmdConferma" CssClass="Pulsante" runat="server" Text="Salva">
                    </asp:Button>

                    <asp:Button ID="cmdApprovazione" runat="server" CssClass="Pulsante" Text="Approvazione" />
                    <asp:Button ID="imgChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
                </div>
                <br />
                <div class="wrapper" id="Div13" style="width: 100%; border: 0px">
                    <div class="row" id="div14" runat="server">
                        <div class="collable" >
                            <asp:Label ID="lblMsgSede" CssClass="msgErroreGrid" Visible="false" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    <asp:DataGrid ID="dgRisultatoRicerca" runat="server" ToolTip="Elenco Sede di Progetto"
                        Width="100%" AllowPaging="True" CssClass="table" AllowSorting="True" AutoGenerateColumns="False"
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
                            <asp:ButtonColumn Visible="False" Text="&lt;img src=images/statoente.jpg Width=20 Height=20 title='Visualizza dettagli' border=0&gt;"
                                CommandName="Select"></asp:ButtonColumn>
                            <asp:ButtonColumn Visible="False" Text="&lt;img src=images/mini_conferma.jpg Width=20 Height=20 title='Accetta Progetto' border=0&gt;"
                                CommandName="accettazione"></asp:ButtonColumn>
                            <asp:ButtonColumn Visible="False" Text="&lt;img src=images/xp3.gif Width=25 Height=25 title='Valutazione Qualit&#224; Progetto' border=0&gt;"
                                CommandName="valutazione"></asp:ButtonColumn>
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
                            <asp:BoundColumn DataField="IdRegioneCompetenza" Visible="False">
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center"
                            CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                </div>
            </div>
        </fieldset>
    </div>
</asp:Content>
