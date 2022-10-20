<%@ Page Title="Gestione Progetti Servizio Civile Universale" Language="vb" MaintainScrollPositionOnPostback="true"  AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="TabProgettiSCU.aspx.vb" Inherits="Futuro.TabProgettiSCU" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
        <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css"/>
        <script src="Scripts/JHelper.js" type="text/javascript"></script>
        <style type="text/css">
            .ui-datepicker {
                font-size: 11px;
            }

        </style>
        <%--<script type="text/javascript" >
           /* <![CDATA[ */
            $(function () {
                if (document.getElementById(CostruisciId('txtDataInizioPrevista')).disabled == false) {
                var clientId = "#" + CostruisciId('txtDataInizioPrevista');
                    $("" + clientId + "").datepicker();
                }
            });

                 /* ]]> */
        </script>--%>
    <script  type="text/javascript">
       /* <![CDATA[ */
        var pathPlus = "images/plus_xsmall.png";
        var pathMinus = "images/minus_xsmall.png";
        var titlePathMinus = "Riduci";
        var titlePathPlus = "Espandi";
        var inserimento = '<%=Request.Params("IdAttivita")%>';
        var isPostbackFormazioneSpecifica  = '<%= hdnIsPostbackFS.value %>' ;
        var isPostbackFormazioneGenerale = '<%= hdnIsPostbackFG.value %>' ;
        var isPostbackFormazioneProgetto = '<%= hdnIsPostbackProg.value %>' ;
         var isPostbackConoscenzeAcquisite = '<%= hdnIsPostbackCCA.value %>' ;
         var isPostbackCaratteristicheOrganizzative = '<%= hdnIsPostbackCO.value %>' ;
         var isPostbackEstero = '<%= hdnIsPostbackEstero.value %>' ;

         
        function CheckNumeroValido(valore, descrizione) {
            var clientIdErrore = CostruisciId("lblerrore")
            var numeroValido = ValidaNumero(CostruisciId(valore), descrizione, clientIdErrore);
            if (numeroValido == false) {
                TornaAdInizioPagina();
            }
        };

        function TotaleRichiesti() {
            var vittoAlloggio = parseInt(document.getElementById(CostruisciId("TxtVittoAlloggioRic")).value);
            var noVittoAlloggio = parseInt(document.getElementById(CostruisciId("TxtNoVittoAlloggioRic")).value);
            var soloVitto = parseInt(document.getElementById(CostruisciId("TxtVittoRic")).value);

            if (isNaN(vittoAlloggio) == true) {
                vittoAlloggio = 0;
            }
            if (isNaN(noVittoAlloggio) == true) {
                noVittoAlloggio = 0;
            }
            if (isNaN(soloVitto) == true) {
                soloVitto = 0;
            }

            document.getElementById(CostruisciId("TxtTotaleRic")).value = vittoAlloggio + noVittoAlloggio + soloVitto;
        };

        function AggiornaTotaleVolontariDaImpiegare() {
            var vittoAlloggio 
            var noVittoAlloggio
            var soloVitto
            if (document.getElementById(CostruisciId("txtNumeroPostiVittoAlloggio")).value == '') {
                vittoAlloggio = 0;
            }
            else{
                vittoAlloggio = parseInt(document.getElementById(CostruisciId("txtNumeroPostiVittoAlloggio")).value);
            }

            if (document.getElementById(CostruisciId("txtNumeroPostiSenzaVittoAlloggio")).value == '') {
                noVittoAlloggio = 0;
            }
            else{
                noVittoAlloggio = parseInt(document.getElementById(CostruisciId("txtNumeroPostiSenzaVittoAlloggio")).value);
            }

            if (document.getElementById(CostruisciId("txtNumeroPostiSoloVitto")).value == '') {
                soloVitto = 0;
            }
            else{
                soloVitto = parseInt(document.getElementById(CostruisciId("txtNumeroPostiSoloVitto")).value);
            }
            document.getElementById(CostruisciId("txtNumTotVolontaridaImpiegare")).value = vittoAlloggio + noVittoAlloggio + soloVitto;
        };

        function ControllaTipologiaProgetto() {
            if ('<%=request.querystring("Nazionale")%>' != '2') {
                if (TabStrip1.selectedIndex == 6) {
                    alert("Scheda non prevista per i Progetti in Italia.")
                    TabStrip1.selectedIndex = 0;
                    return false;
                }
            }
        };

        function ApriSigedElencoDocumenti() {
            var winOption = PopUpOption();
            var codiceFascicolo = CostruisciId("TxtCodiceFascicolo");
            var NumFascicolo = '<%= TxtIdFascicolo.value %>';

            if (codiceFascicolo == '') {
                alert("Specificare il numero fascicolo!");
                return false;
            } else {
                myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?NumeroFascicolo=" + NumFascicolo, "Visualizza", winOption);
            }
            
        };
           /* ]]> */
    </script>

    <script type="text/javascript">
        /* <![CDATA[ */
        $(function () {
            var tornaSu = 0;
            if( isPostbackCaratteristicheOrganizzative != "True"){
            if (inserimento != "") {
                $('#CaratteristicheOrganizzativeContent').css('display', 'none');
                $('#CaratteristicheOrganizzative').find("img").first().attr("src", pathPlus);
                $('#CaratteristicheOrganizzative').find("img").first().attr("title", titlePathPlus);
            }
            else {
                $('#CaratteristicheOrganizzativeContent').css('display', 'yes');
                $('#CaratteristicheOrganizzative').find("img").first().attr("src", pathMinus);
                $('#CaratteristicheOrganizzative').find("img").first().attr("title", titlePathMinus);
                }
               }
            else {
            if( isPostbackCaratteristicheOrganizzative = "True"){
             $('#CaratteristicheOrganizzativeContent').css('display', 'yes');
                $('#CaratteristicheOrganizzative').find("img").first().attr("src", pathMinus);
                $('#CaratteristicheOrganizzative').find("img").first().attr("title", titlePathMinus);
            }
            }
            $('#CaratteristicheOrganizzativeToggler').click(function () {
                var isVisible = $('#CaratteristicheOrganizzativeContent').is(':visible');
                if (!isVisible) {
                    $('#CaratteristicheOrganizzativeContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    $('#CaratteristicheOrganizzative').find("img").first().attr("src", pathMinus);
                    $('#CaratteristicheOrganizzative').find("img").first().attr("title", titlePathMinus);
                    $('#flsCaratteristicheOrganizzative').focus();
                }
                else {
                    $('#flsCaratteristicheOrganizzative').focus();
                }
                return false;
            });
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
            if( isPostbackConoscenzeAcquisite != "True"){
            if (inserimento != "") {
                $('#ConoscenzeAcquisiteContent').css('display', 'none');
                $('#ConoscenzeAcquisite').find("img").first().attr("src", pathPlus);
                $('#ConoscenzeAcquisite').find("img").first().attr("title", titlePathPlus);
            }
            else {
                $('#ConoscenzeAcquisiteContent').css('display', 'yes');
                $('#ConoscenzeAcquisite').find("img").first().attr("src", pathMinus);
                $('#ConoscenzeAcquisite').find("img").first().attr("title", titlePathMinus);
            }
               }
            else {
            if( isPostbackConoscenzeAcquisite = "True"){
             $('#ConoscenzeAcquisiteContent').css('display', 'yes');
                $('#ConoscenzeAcquisite').find("img").first().attr("src", pathMinus);
                $('#ConoscenzeAcquisite').find("img").first().attr("title", titlePathMinus);
            }
            }
            
            $('#ConoscenzeAcquisiteToggler').click(function () {
                var isVisible = $('#ConoscenzeAcquisiteContent').is(':visible');
                if (!isVisible) {
                    $('#ConoscenzeAcquisiteContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    $('#flsConoscenzeAcquisite').focus();
                    $('#ConoscenzeAcquisite').find("img").first().attr("src", pathMinus);
                    $('#ConoscenzeAcquisite').find("img").first().attr("title", titlePathMinus);
                }
                else {
                    $('#flsConoscenzeAcquisite').focus();
                }
                return false;
            });
            $('#TornaSuConoscenzeAcquisite').click(function () {
                tornaSu = 1;
            });
            $('#ConoscenzeAcquisite').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#ConoscenzeAcquisiteContent').is(':visible');
                    $('#ConoscenzeAcquisiteContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    if (!isVisible) {
                        $(this).find("img").first().attr("src", pathMinus);
                        $(this).find("img").first().attr("title", titlePathMinus);
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
           if( isPostbackFormazioneGenerale != "True"){
            if (inserimento != "") {
                $('#FormazioneGeneraleContent').css('display', 'none');
                $('#FormazioneGenerale').find("img").first().attr("src", pathPlus);
                $('#FormazioneGenerale').find("img").first().attr("title", titlePathPlus);
            }
            else {
                $('#FormazioneGeneraleContent').css('display', 'yes');
                $('#FormazioneGenerale').find("img").first().attr("src", pathMinus);
                $('#FormazioneGenerale').find("img").first().attr("title", titlePathMinus);
            }
            }
            else {
            if( isPostbackFormazioneGenerale = "True"){
             $('#FormazioneGeneraleContent').css('display', 'yes');
                $('#FormazioneGenerale').find("img").first().attr("src", pathMinus);
                $('#FormazioneGenerale').find("img").first().attr("title", titlePathMinus);
            }
            }
            $('#FormazioneGeneraleToggler').click(function () {
                var isVisible = $('#FormazioneGeneraleContent').is(':visible');
                if (!isVisible) {
                    $('#FormazioneGeneraleContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    $('#FormazioneGenerale').find("img").first().attr("src", pathMinus);
                    $('#FormazioneGenerale').find("img").first().attr("title", titlePathMinus);
                    $('#flsFormazioneGenerale').focus();
                }
                else {
                    $('#flsFormazioneGenerale').focus();
                }
                return false;
            });
            $('#TornaSuFormazioneGenerale').click(function () {
                tornaSu = 1;
            });
            $('#FormazioneGenerale').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#FormazioneGeneraleContent').is(':visible');
                    $('#FormazioneGeneraleContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    if (!isVisible) {
                        $(this).find("img").first().attr("src", pathMinus);
                        $(this).find("img").first().attr("title", titlePathMinus);
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
             if( isPostbackFormazioneSpecifica != "True"){
            if (inserimento != "") {
                $('#FormazioneSpecificaContent').css('display', 'none');
                $('#FormazioneSpecifica').find("img").first().attr("src", pathPlus);
                $('#FormazioneSpecifica').find("img").first().attr("title", titlePathPlus);
            }
            else {
                $('#FormazioneSpecificaContent').css('display', 'yes');
                $('#FormazioneSpecifica').find("img").first().attr("src", pathMinus);
                $('#FormazioneSpecifica').find("img").first().attr("title", titlePathMinus);
            }

            }
               else {
            if( isPostbackFormazioneSpecifica = "True"){
             $('#FormazioneSpecificaContent').css('display', 'yes');
                $('#FormazioneSpecifica').find("img").first().attr("src", pathMinus);
                $('#FormazioneSpecifica').find("img").first().attr("title", titlePathMinus);
            }
            }
            $('#FormazioneSpecificaToggler').click(function () {
                var isVisible = $('#FormazioneSpecificaContent').is(':visible');
                if (!isVisible) {
                    $('#FormazioneSpecificaContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    $('#FormazioneSpecifica').find("img").first().attr("src", pathMinus);
                    $('#FormazioneSpecifica').find("img").first().attr("title", titlePathMinus);
                    $('#flsFormazioneSpecifica').focus();
                }
                else {
                    $('#flsFormazioneSpecifica').focus();
                }
                return false;
            });
            $('#TornaSuFormazioneSpecifica').click(function () {
                tornaSu = 1;
            });
            $('#FormazioneSpecifica').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#FormazioneSpecificaContent').is(':visible');
                    $('#FormazioneSpecificaContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    if (!isVisible) {
                        $(this).find("img").first().attr("src", pathMinus);
                        $(this).find("img").first().attr("title", titlePathMinus);
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
                $('#AltriElementiContent').css('display', 'none');
                $('#AltriElementi').find("img").first().attr("src", pathPlus);
                $('#AltriElementi').find("img").first().attr("title", titlePathPlus);

            }
            else {
                $('#AltriElementiContent').css('display', 'yes');
                $('#AltriElementi').find("img").first().attr("src", pathMinus);
                $('#AltriElementi').find("img").first().attr("title", titlePathMinus);
            }
            
            $('#AltriElementiToggler').click(function () {
                var isVisible = $('#AltriElementiContent').is(':visible');
                if (!isVisible) {
                    $('#AltriElementiContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    $('#AltriElementi').find("img").first().attr("src", pathMinus);
                    $('#AltriElementi').find("img").first().attr("title", titlePathMinus);
                    $('#flsAltriElementi').focus();
                }
                else {
                    $('#flsAltriElementi').focus();
                }
                return false;
            });
            $('#TornaSuAltriElementi').click(function () {
                tornaSu = 1;
            });
            $('#AltriElementi').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#AltriElementiContent').is(':visible');
                    $('#AltriElementiContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    if (!isVisible) {
                        $(this).find("img").first().attr("src", pathMinus);
                        $(this).find("img").first().attr("title", titlePathMinus);
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
            $('#ProgettoContent').css('display', 'yes');
            $('#Progetto').find("img").first().attr("src", pathMinus);
            $('#Progetto').find("img").first().attr("title", titlePathMinus);
            $('#ProgettoToggler').click(function () {
                var isVisible = $('#ProgettoContent').is(':visible');
                if (!isVisible) {
                    $('#ProgettoContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    $('#Progetto').find("img").first().attr("src", pathMinus);
                    $('#Progetto').find("img").first().attr("title", titlePathMinus);
                    $('#flsProgetto').focus();
                }
                return false;
            });
            $('#TornaSuProgetto').click(function () {
                tornaSu = 1;
            });
            $('#Progetto').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#ProgettoContent').is(':visible');
                    $('#ProgettoContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    if (!isVisible) {
                        $(this).find("img").first().attr("src", pathMinus);
                        $(this).find("img").first().attr("title", titlePathMinus);
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

     <script type="text/javascript">
          /* <![CDATA[ */
         $(function () {
             var tornaSu = 0;
             if( isPostbackEstero != "True"){
             if(inserimento!=""){
                 $('#EsteroContent').css('display', 'none');
                 $('#Estero').find("img").first().attr("src", pathPlus)
                 $('#Estero').find("img").first().attr("title", titlePathPlus);
             }
             else{
                 $('#EsteroContent').css('display', 'yes');
                 $('#Estero').find("img").first().attr("src", pathMinus);
                 $('#Estero').find("img").first().attr("title", titlePathMinus);
                    }
               }
            else {
            if( isPostbackFormazioneGenerale = "True"){
             $('#EsteroContent').css('display', 'yes');
                $('#Estero').find("img").first().attr("src", pathMinus);
                $('#Estero').find("img").first().attr("title", titlePathMinus);
            }
            }
             $('#EsteroToggler').click(function () {
                 var isVisible = $('#EsteroContent').is(':visible');
                 if (!isVisible) {
                     $('#EsteroContent').slideToggle('fast');
                     $(this).toggleClass('slideSign');
                     $('#Estero').find("img").first().attr("src", pathMinus);
                     $('#Estero').find("img").first().attr("title", titlePathMinus);
                     $('#flsEstero').focus();
                 }
                 else {
                     $('#flsEstero').focus();
                 }
                 return false;
             });
             $('#TornaSuEstero').click(function () {
                 $("#GestioneProgetti").select();
                 tornaSu = 1;
             });
             $('#Estero').click(function () {
                 if (tornaSu == 0) {
                     var isVisible = $('#EsteroContent').is(':visible');
                     $('#EsteroContent').slideToggle('fast');
                     $(this).toggleClass('slideSign');
                     if (!isVisible) {
                         $(this).find("img").first().attr("src", pathMinus);
                         $(this).find("img").first().attr("title", titlePathMinus);
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

     <script  type="text/javascript">
         /* <![CDATA[ */
         function Salva() {
             var messaggioDiErrore = '';
             var validazioneOk = true;
             var IdMsgErrore = CostruisciId("lblerrore");
             AggiornaErrore("", IdMsgErrore);
             if (document.getElementById(CostruisciId("txtTitolo")).value == '') {
                 messaggioDiErrore = "Sezione Progetto - Inserire il Titolo.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
             if (document.getElementById(CostruisciId("ddlSettore")).value == 0) {
                 messaggioDiErrore = "Sezione Progetto - Selezionare il settore l'Area.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
            if (document.getElementById(CostruisciId("ddlDurata")).value == 0) {
                 messaggioDiErrore = "Sezione Progetto - Selezionare la durata del progetto.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
             <% If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") And Not Request.QueryString("IdAttivita") Is Nothing Then %>
             if (parseInt(document.getElementById(CostruisciId("txtNumeroPostiVittoAlloggio")).value) > 32767) {
                 messaggioDiErrore = "Sezione Progetto - Il valore inserito dei posti con vitto e alloggio  %E8 superiore al numero massimo consentito."
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
             if (parseInt(document.getElementById(CostruisciId("txtNumeroPostiSenzaVittoAlloggio")).value) > 32767) {
                 messaggioDiErrore = "Sezione Progetto - Il valore inserito dei posti senza vitto e alloggio  %E8 superiore al numero massimo consentito.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
             if (parseInt(document.getElementById(CostruisciId("txtNumeroPostiSoloVitto")).value) > 32767) {
                 messaggioDiErrore = "Sezione Progetto - Il valore inserito dei posti con vitto  %E8 superiore al numero massimo consentito.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 return false;
             }
            <% End If %>
             if (document.getElementById(CostruisciId("CheckMonteOreTipo")) != null) {
                if (document.getElementById(CostruisciId("CheckMonteOreTipo")).value == 'selezionare' ) {
                     messaggioDiErrore = "Sezione Progetto - Selezionare il tipo di Monte Ore";
                     ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                     validazioneOk = false;
                 }
                 if ((document.getElementById(CostruisciId("txtOreServizioSettimanali")).value == '') && (document.getElementById(CostruisciId("CheckMonteOreTipo")).value == '1' )) {
                     messaggioDiErrore = "Sezione Progetto - Inserire almeno 12 ore di Servizio Settimanale.";
                     ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                     validazioneOk = false;
                 }
                 if ((document.getElementById(CostruisciId("txtOreServizioSettimanali")).value == '') && (document.getElementById(CostruisciId("CheckMonteOreTipo")).value == '0')) {
                     messaggioDiErrore = "Sezione Progetto - Inserire 25 ore di Servizio Settimanale.";
                     ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                     validazioneOk = false;
                 }
                 else {
                     if (document.getElementById(CostruisciId("CheckMonteOreTipo")).value == '0') {
                         if (parseInt(document.getElementById(CostruisciId("txtOreServizioSettimanali")).value) != 25) {
                             messaggioDiErrore = "Sezione Progetto - Inserire 25 ore di Servizio Settimanale.";
                             ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                             validazioneOk = false;
                         }
                         else {
                             if (parseInt(document.getElementById(CostruisciId("txtOreServizioSettimanali")).value) > 255) {
                                 messaggioDiErrore = "Sezione Progetto - Il valore inserito e' superiore al numero massimo consentito.";
                                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                                 validazioneOk = false;
                             }
                         }
                     }
                     else {
                         if (parseInt(document.getElementById(CostruisciId("txtOreServizioSettimanali")).value) < 12) {
                             messaggioDiErrore = "Sezione Progetto - Inserire almeno 12 ore di Servizio Settimanale.";
                             ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                             validazioneOk = false;
                         }
                         else {
                             if (parseInt(document.getElementById(CostruisciId("txtOreServizioSettimanali")).value) > 25) {
                                 messaggioDiErrore = "Sezione Progetto - Il valore inserito  delle ore settimanali e' superiore al numero massimo consentito.";
                                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                                 validazioneOk = false;
                             }
                         }
                     }
                 }


                 if (document.getElementById(CostruisciId("CheckMonteOreTipo")).value == '1') {
                     if (document.getElementById(CostruisciId("txtMonteOreAnnuo")).value == '') {
                         messaggioDiErrore = "Sezione Progetto - Inserire un Monte ore Annuo minimo di 1400 ore.";
                         ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                         validazioneOk = false;
                     }
                     if (parseInt(document.getElementById(CostruisciId("txtMonteOreAnnuo")).value) > 32767) {
                         messaggioDiErrore = "Sezione Progetto - Il valore inserito del monte ore annuo %E8 superiore al numero massimo consentito.";
                         ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                         validazioneOk = false;
                     }
                     //if (parseInt(document.getElementById(CostruisciId("txtMonteOreAnnuo")).value) < 1400) {
                    //     messaggioDiErrore = "Sezione Progetto - Inserire un Monte ore Annuo minimo di 1400 ore.";
                     //    ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                     //    validazioneOk = false;
                    // }
                 }
             }
             if (document.getElementById(CostruisciId("txtNumGiorniServizio")).value == '') {
                 messaggioDiErrore = "Sezione Progetto - I giorni di Servizio Settimanali devono essere 5 oppure 6.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
             else
             if (document.getElementById(CostruisciId("txtNumGiorniServizio")).value != 5 && document.getElementById(CostruisciId("txtNumGiorniServizio")).value != 6) {
                 messaggioDiErrore = "Sezione Progetto - I giorni di Servizio Settimanali devono essere 5 oppure 6.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
        
            if (document.getElementById(CostruisciId("chkGiovaniMinoriOp")).value == '1') {
                if (document.getElementById(CostruisciId("txtNumeroGiovaniMinoriOpportunita")).value == '') {
                    messaggioDiErrore = "Sezione Progetto - Inserire il numero di giovani con minori opportunità.";
                    ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                    validazioneOk = false;
                }

                if (document.getElementById(CostruisciId("chkGiovaniMinoriOp")).value != '') {
                  if (parseInt(document.getElementById(CostruisciId("chkGiovaniMinoriOp")).value) == 0 ) {
                    messaggioDiErrore = "Sezione Progetto - Il numero di giovani con minori opportunità deve essere maggiore di 0.";
                    ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                    validazioneOk = false;
                    }
                }
             }
            
             //Criteri di Selezione
             if (document.getElementById(CostruisciId("ddlEventualiCriteri")).value == 0) {
                 messaggioDiErrore = "Sezione Caratteristiche Organizzative - Selezionare i Criteri di Selezione.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
             if ((document.getElementById(CostruisciId("ddlEventualiCriteri")).value == 3) && (document.getElementById(CostruisciId("txtCodEnteEventuali")).value == '')) {
                 messaggioDiErrore = "Sezione Caratteristiche Organizzative - Inserire il codice ente per i Criteri di Selezione.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
             //Criteri di Selezione
             if (document.getElementById(CostruisciId("ddlPianoMonitoraggio")).value == 0) {
                 messaggioDiErrore = "Sezione Caratteristiche Organizzative - Selezionare il Piano di Monitoriaggio.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
             if ((document.getElementById(CostruisciId("ddlPianoMonitoraggio")).value == 2) && (document.getElementById(CostruisciId("txtPianoMonitoraggio")).value == '')) {
                 messaggioDiErrore = "Sezione Caratteristiche Organizzative - Inserire il codice ente per il Piano di Monitoraggio.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
             if (document.getElementById(CostruisciId("txtOrePromozioneSensibilizzazione")).value == '') {
                 messaggioDiErrore = "Sezione Caratteristiche Organizzative - Le Ore di Promozione e Sensibilizzazione devono essere specificate.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
             //Caratteristiche Conoscenze Acquisite
             if (document.getElementById(CostruisciId("ddlCreditiFormativiRiconosciuti")).value == 10) {
                 messaggioDiErrore = "Sezione Caratteristiche Conoscenze Acquisite - Selezionare i Crediti Formativi Riconosciuti.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
             if (document.getElementById(CostruisciId("ddlEventualiTirociniRiconosciuti")).value == 10) {
                 messaggioDiErrore = "Sezione Caratteristiche Conoscenze Acquisite - Selezionare Gli Eventuali Tirocini Riconosciuti.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
             if (document.getElementById(CostruisciId("ddlCompetenzeAcquisibili")).value == 10) {
                 messaggioDiErrore = "Sezione Caratteristiche Conoscenze Acquisite - Selezionare le Competenze Acquisibili.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
             //Formazione Generale
             if (document.getElementById(CostruisciId("txtDurata")).value != '') {
                 if (parseInt(document.getElementById(CostruisciId("txtDurata")).value) > 32767) {
                     messaggioDiErrore = "Sezione Formazione Generale - Il valore inserito %E8 superiore al numero massimo consentito.";
                     ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                     validazioneOk = false;
                 }
                 if (parseInt(document.getElementById(CostruisciId("txtDurata")).value) < 30) {
                     messaggioDiErrore = "Sezione Formazione Generale - Il valore deve essere minimo di 30 ore.";
                     ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                     validazioneOk = false;
                 }
             }
             else {
                 messaggioDiErrore = "Sezione Formazione Generale - Inserire la durata della formazione(Il valore minimo &#232; di 30 ore).";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }

             //Formazione Generale
             if (document.getElementById(CostruisciId("ddlModalitaAttuazione")).value == 0) {
                 messaggioDiErrore = "Sezione Formazione Generale - Selezionare la Modalit&#224; di Attuazione della Formazione Generale.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
             if ((document.getElementById(CostruisciId("ddlModalitaAttuazione")).value == 2) && (document.getElementById(CostruisciId("txtModalitaAttuazione")).value == '')) {
                 messaggioDiErrore = "Sezione Formazione Generale - Inserire il codice ente per la Modalit&#224; di Attuazione.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
             if (document.getElementById(CostruisciId("ddlFormazioneGeneraleErogazione")).value == "selezionare")  {
                 messaggioDiErrore = "Sezione Formazione Generale - Selezionare una Modalit&#224; di Erogazione della Formazione Generale.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }

             if (document.getElementById(CostruisciId("ddlFormazioneGeneraleErogazione")).value ==  "percentuale" && document.getElementById(CostruisciId("ddlDurata")).value != 12)  {
                 messaggioDiErrore = "Sezione Formazione Generale - Per i progetti di durata inferiore a 12 mesi, l'erogazione della formazione generale deve essere ad Unica Tranche. ";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
             //Formazione Specifica

             if (document.getElementById(CostruisciId("txtDurataSpec")).value != '') {
                 if (parseInt(document.getElementById(CostruisciId("txtDurataSpec")).value) > 32767) {
                     messaggioDiErrore = "Sezione Formazione Specifica - Il valore inserito %E8 superiore al numero massimo consentito.";
                     ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                     validazioneOk = false;
                 }
                 if (parseInt(document.getElementById(CostruisciId("txtDurataSpec")).value) == 0) {
                     messaggioDiErrore = "Sezione Formazione Specifica - Il valore deve essere minimo di 1 ora.";
                     ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                     validazioneOk = false;
                 }
                 if (parseInt(document.getElementById(CostruisciId("txtDurataSpec")).value) < 50) {
                     messaggioDiErrore = "Sezione Formazione Specifica - Le ore di Formazione Specifica devono essere almeno 50.";
                     ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                     validazioneOk = false;
                 }
                 if (parseInt(document.getElementById(CostruisciId("txtDurataSpec")).value) + parseInt(document.getElementById(CostruisciId("txtDurata")).value) < 80) {
                     messaggioDiErrore = "Sezione Formazione Specifica - Il valore della durata della Formazione Generale sommato a quello della Formazione Specifica deve essere almeno di 80 ore.";
                     ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                     validazioneOk = false;
                 }
                 if (parseInt(document.getElementById(CostruisciId("txtDurataSpec")).value) + parseInt(document.getElementById(CostruisciId("txtDurata")).value) > 150) {
                     messaggioDiErrore = "Sezione Formazione Specifica - Il valore della durata della Formazione Generale sommato a quello della Formazione Specifica non deve superare le 150 ore.";
                     ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                     validazioneOk = false;
                 }
             }
             else {
                 messaggioDiErrore = "Sezione Formazione Specifica - Inserire la durata della Formazione Specifica.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }


             //Formazione Specifica

             if (document.getElementById(CostruisciId("ddlmodattuazione")).value == 0) {
                 messaggioDiErrore = "Sezione Formazione Specifica - Selezionare la Modalita&#224; di Attuazione della Formazione Specifica.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
             if ((document.getElementById(CostruisciId("ddlmodattuazione")).value == 2) && (document.getElementById(CostruisciId("txtmodattuazione")).value == '')) {
                 messaggioDiErrore = "Sezione Formazione Specifica - Inserire il codice ente per la Modalit&#224; di Attuazione della Formazione Specifica.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }

             if (document.getElementById(CostruisciId("ddlFormazioneSpecificaErogazione")).value == "selezionare")  {
                 messaggioDiErrore = "Sezione Formazione Specifica - Selezionare una Modalit&#224; di Erogazione della Formazione Specifica.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }

            if (document.getElementById(CostruisciId("ddlFormazioneSpecificaErogazione")).value ==  "percentuale" && document.getElementById(CostruisciId("ddlDurata")).value != 12)  {
                 messaggioDiErrore = "Sezione Formazione Specifica - Per i progetti di durata inferiore a 12 mesi, l'erogazione della formazione specifica deve essere ad Unica Tranche. ";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
             if ('<%=request.querystring("Nazionale")%>' == '6') { //Estero
                 //Particolari Condizioni di Rischio
                 if (document.getElementById(CostruisciId("ddlCondizioniRischio")).value == 10) {
                     messaggioDiErrore = "Sezione Estero - Selezionare le Particolari Condizioni di Rischio.";
                     ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                     validazioneOk = false;
                 }
                 //Livelli Misure di Sicurezza
                 if (document.getElementById(CostruisciId("ddlLivelliSicurezza")).value == 10) {
                     messaggioDiErrore = "Sezione Estero - Selezionare i Livelli Minimi di Sicurezza.";
                     ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                     validazioneOk = false;
                 }
                 //Particolari Condizioni di Disagio
                 if (document.getElementById(CostruisciId("ddlCondizioniDisagio")).value == 10) {
                     messaggioDiErrore = "Sezione Estero - Selezionare le Particolari Condizioni di Disagio.";
                     ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                     validazioneOk = false;
                 }
                 //Eventuale Assicurazione Integrativa
                 if (document.getElementById(CostruisciId("ddlEventualeAssicurazione")).value == 10) {
                     messaggioDiErrore = "Sezione Estero - Selezionare l'Eventuale Assicurazione Integrativa.";
                     ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                     validazioneOk = false;
                 }
             }
             if (validazioneOk == false) {
                 TornaAdInizioPagina();
             }
           return validazioneOk;
//             return true;//test
         };
              /* ]]> */

     </script>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset id="GestioneProgetti"  class="ContornoPagina" runat="server">
<legend >Progetto Sperimentale di Servizio Civile Universale</legend>
<div class="wrapper"  style="width:100%;border:0px">
  <div class="headers" >
    <h2>
      <asp:label  id="lblTitoloProgetto" style="vertical-align:middle" runat="server"></asp:label>
    </h2>
</div>
<div class="RigaVuota">&nbsp;</div>
<div class="row" style="height:auto">
<asp:Label ID="lblMessaggioConferma"  CssClass="msgConferma" runat="server"></asp:Label>
<asp:Label ID="lblerroreSettore" CssClass="msgErrore" runat="server"></asp:Label>
<asp:Label ID="lblareaerrore" CssClass="msgErrore" runat="server"></asp:Label>
<asp:Label ID="lblerrore" CssClass="msgErrore" runat="server"></asp:Label>


</div>
<div class="RigaVuota">&nbsp;</div>
 <div class="rowGroup" style="height:auto"> 
    <div class="row" style="height:auto">  
          <div class="colHyperLink" style="width:50%">
                <asp:LinkButton ID="imgSediProgetto" runat="server" style="cursor:pointer" Text="Gestione Sedi di Progetto"></asp:LinkButton>
            </div>
            <div class="colHyperLink" style="width:50%">
                <asp:LinkButton ID="imgRisorse" runat="server" style="cursor:pointer" Text="Gestione Risorse"></asp:LinkButton>
            </div>    
    </div>
    <div class="row" style="height:auto">  
              <div class="colHyperLink" style="width:50%">
                <asp:LinkButton ID="imgElencoDocumentiProg" Visible="False" Text="Documenti Progetto" style="cursor:pointer" runat="server"></asp:LinkButton>
            </div>  
            <div class="colHyperLink" style="width:50%">
                <asp:LinkButton ID="ImgStampa"  Text="Stampa Progetto" runat="server" style="cursor:pointer"></asp:LinkButton>
            </div>         
      
       </div>
    <div class="row" style="height:auto">                  

                <div class="colHyperLink" style="width:50%">
                <asp:LinkButton ID="imgVolontari"  Text="Visualizza Volontari" runat="server" style="cursor:pointer"></asp:LinkButton>
            </div> 
             <div class="colHyperLink" style="width:50%">
                <asp:LinkButton ID="imgDett" Text="Riepilogo Assenze Volontari" style="cursor:pointer" runat="server"></asp:LinkButton>
            </div>
       </div>
    <div class="row" style="height:auto">                  
        <div class="colHyperLink" style="width:50%">
            <asp:LinkButton ID="imgSediCertificate" runat="server" style="cursor:pointer" Text="Gestione Iscrizione Sedi" Visible="False"></asp:LinkButton>
        </div>
         <div class="colHyperLink" style="width:50%">
            <asp:LinkButton ID="imgDisabili" runat="server" style="cursor:pointer" Text="Lista soggetti interessati all'accompagnamento"></asp:LinkButton>
        </div>     
     </div>
    <div class="row" style="height:auto">                  
             <div class="colHyperLink" style="width:50%">
                <asp:LinkButton ID="imgSediAssegnazione"  Visible="False" Text="Gestione Sedi di Assegnazione" style="cursor:pointer" runat="server"></asp:LinkButton>
            </div>
                <div class="colHyperLink" style="width:50%">
                <asp:LinkButton ID="imgAssociaDocumentiProg"  Visible="false" Text="Associa Documenti Progetto" runat="server" style="cursor:pointer"></asp:LinkButton>
            </div> 
       </div>
        
    <div class="RigaVuota"  runat="server" id="RigaCodiceProgetto" visible="false"  style="height:auto" >
        <div class="collable" style="width:15%" >
        <asp:Label ID="LblCodProgetto" runat="server" CssClass="label" Text="Codice Progetto" AssociatedControlID="txtCodiceEnte"></asp:Label>
        </div>
        <div class="collable" style="width:35%" >
        <asp:textbox id="txtCodiceEnte" CssClass="textbox" autofocus="true" Runat="server" MaxLength="21" Enabled="False"></asp:textbox>
        </div>
        <div class="collable" style="width:15%" >
        <asp:Label ID="Label1" runat="server" CssClass="label" Text="Data Inizio Progetto" AssociatedControlID="txtDataInizioPrevista"></asp:Label>
        </div>
        <div class="collable" style="width:35%" >
        <asp:textbox id="txtDataInizioPrevista" CssClass="textboxData" width="35%"  MaxLength="10" Runat="server"></asp:textbox>
        </div>
    </div>
</div>
  <fieldset  class="ContornoPagina" >
  <legend>Sezioni</legend>
<div class="row" style="height:auto">  
    <div id="ProgettoToggler" class="colHyperLink" style="width:100%">
        <asp:LinkButton ID="link6" runat="server" style="cursor:pointer"  Text="Sezione Progetto" ></asp:LinkButton>
    </div>
     <div id="CaratteristicheOrganizzativeToggler" class="colHyperLink" style="width:100%">
                <asp:LinkButton ID="link1" runat="server" style="cursor:pointer"   Text="Sezione Caratteristiche Organizzative" ></asp:LinkButton>
                </div>
    <div id="ConoscenzeAcquisiteToggler" class="colHyperLink" style="width:100%">
                <asp:LinkButton ID="LinkButton1" runat="server" style="cursor:pointer" Text="Sezione Caratteristiche Conoscenze Acquisite"></asp:LinkButton>
                </div>
     <div id="FormazioneGeneraleToggler" class="colHyperLink" style="width:100%">
                <asp:LinkButton ID="LinkButton2" runat="server" style="cursor:pointer" Text="Sezione Formazione Generale"></asp:LinkButton>
                </div>
    <div id="FormazioneSpecificaToggler" class="colHyperLink" style="width:100%">
            <asp:LinkButton ID="LinkButton3" runat="server" style="cursor:pointer" Text="Sezione Formazione Specifica"></asp:LinkButton>
    </div>
    <div id="AltriElementiToggler" class="colHyperLink" style="width:100%">
            <asp:LinkButton ID="LinkButton4" runat="server" style="cursor:pointer" Text="Sezione Altri Elementi Formazione"></asp:LinkButton>
     </div>
    <div id="EsteroToggler" class="colHyperLink" style="width:100%">
            <asp:LinkButton ID="LinkButtonEstero" runat="server" style="cursor:pointer"  Text="Sezione Estero"></asp:LinkButton>
     </div>             
</div>   
</fieldset>
  <fieldset id="flsProgetto" class="ContornoPagina" >
  <legend  id="Progetto"><img style="cursor:pointer;" src="images/minus_xsmall.png" alt="Espandi/Riduci" />&nbsp;&nbsp;<img style="cursor:pointer;" id="TornaSuProgetto" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Progetto</legend> 
  
 <div class="wrapper" id="ProgettoContent" style="width:100%;border:0px" >
  <div id="IdDivRigaFascicolo" class="rowGroup"  runat="server" visible="false" style="height:auto">
   <div  class="row">
       <div class="collable" style="width:15%">
        <asp:Label ID="LblNumFascicolo" AssociatedControlID="TxtCodiceFascicolo" runat="server" Text="Numero Fascicolo" ></asp:Label>
   
       </div>
       <div class="colOggetti" style="width:80%">
        <asp:TextBox ID="TxtCodiceFascicolo" Width="96%" runat="server" CssClass="textbox"  ></asp:TextBox>

       </div>
       <div class="collable" style="width:5%">
               <asp:image  id="cmdSelProtocollo" runat="server" onclick="ApriSigedElencoDocumenti()" ImageUrl="images/busta_small.png" style="CURSOR: pointer;"
								 ToolTip="Elenco Documenti" AlternateText="Elenco Documenti"></asp:image>
    </div>
   </div>
   <div  class="row">
    <div class="collable" style="width:15%">
    <asp:label id="LblDescrFascicolo" runat="server" CssClass="label" Text="Descrizione" AssociatedControlID="txtDescFasc"></asp:label>
    </div>
     <div class="colOggetti" style="width:85%">
     <asp:textbox id="txtDescFasc" CssClass="textbox" runat="server" ReadOnly="True" TextMode="MultiLine"></asp:textbox>
     </div>
   </div>
</div>
<div class="RigaVuota">&nbsp;
</div>
   <div class="row">
    <div class="collable" style="width:15%">
    <asp:Label Id="LblTitolo" Text="Titolo" AssociatedControlID="txtTitolo" runat="server"  CssClass="label"></asp:Label>
    </div>
    <div class="colOggetti" style="width:85%">
    <asp:TextBox ID="txtTitolo" runat="server" CssClass="textbox" ></asp:TextBox>
    </div>
   </div>
      <div class="row">
      <div class="collable" style="width:15%">
        <asp:Label ID="lblddlSettore" AssociatedControlID="ddlSettore" Text="Settore Principale" runat="server" CssClass="label" />
   </div>
   <div class="colOggetti" style="width:35%">
        <asp:dropdownlist id="ddlSettore" runat="server" CssClass="ddlClass" AutoPostBack="True"></asp:dropdownlist>
    </div>
        <div class="collable" style="width:15%">
        <asp:Label ID="lblArea" AssociatedControlID="ddlArea" Text="Area" runat="server"  CssClass="label"/>
   </div>
    <div class="colOggetti" style="width:30%">
        <asp:dropdownlist id="ddlArea" runat="server" CssClass="ddlClass" Enabled="true"  AutoPostBack="True"></asp:dropdownlist>
    </div>
    
   </div>
     <div class="row" id="DivAltriAmbiti" visible="true" style="height:auto" runat="server">
        <div class="collable" style="width:15%">
            <asp:Label Id="lblAltriAmbiti" Text="Altri Ambiti" AssociatedControlID="txtArea"  runat="server"  CssClass="label"></asp:Label>
        </div>
        <div class="colOggetti" style="width:80%">
            <asp:TextBox ID="txtArea" ReadOnly="true"  Width="96%" runat="server" CssClass="textbox" TextMode="MultiLine" Rows="4" ></asp:TextBox>
            

        </div>
        <div class="collable" style="width:5%">
            <asp:ImageButton ID="imgArea" ImageUrl="images/lenteIngrandimento_small.png" runat="server" AlternateText="Seleziona Ambiti"  ToolTip="Ambiti" 	style="cursor:hand" />
        </div>
   </div>
   <div class="RigaVuota"  > &nbsp;
   </div>
   <div class="row" id="DivDurata" visible="true" style="height:auto" runat="server">
        <div class="collable" style="width:15%">
            <asp:Label Id="LblDurataProgetto" Text="Durata Progetto" AssociatedControlID="ddlDurata"  runat="server"  CssClass="label"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%">
            <asp:dropdownlist id="ddlDurata" runat="server" CssClass="ddlClass" AutoPostBack="True"></asp:dropdownlist>
        </div>
   </div>
    <div class="rowGroup" id="DivSelezionaAltriAmbiti" visible="false" style="height:auto" runat="server" >
   <div class="rowGroup"  >
        <div class="collable" style="width:45%">
            <asp:datagrid id="dtgAmbiti" runat="server" Width="100%"  ToolTip="Elenco Ambiti" Caption="Elenco Ambiti" 
						AllowPaging="True" CellPadding="2" Font-Size="Small"  CssClass="table"  PageSize="10" AllowSorting="false" AutoGenerateColumns="False" UseAccessibleHeader="True">
			    <FooterStyle></FooterStyle>
				<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
				<EditItemStyle></EditItemStyle>
				<AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
				<ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
				<HeaderStyle></HeaderStyle>
				<Columns>
                    <asp:TemplateColumn > 
                        <ItemTemplate >
						    <asp:ImageButton ID="SelezionaArea"  style="cursor:pointer;" CommandName="Select" AlternateText="Seleziona Ambito" ToolTip="Seleziona Ambito" runat="server" ImageURL="images/valida_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                  </asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="IdAmbito" HeaderText="IdAmbito"></asp:BoundColumn>
				<asp:BoundColumn DataField="Denominazione" HeaderText="Denominazione"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="IdAmbitoNascosto" HeaderText="IdAmbitoNascosto"></asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;"   
	                    PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
        </div>
          <div class="collable" style="width:10%">&nbsp;
          </div>
        <div class="colOggetti" style="width:45%">
            <asp:datagrid id="dtgAmbitiSelezionati"  runat="server" Width="100%"  ToolTip="Elenco Ambiti Selezionati" Caption="Elenco Ambiti Selezionati" 
						AllowPaging="false" CellPadding="2" Font-Size="Small"  CssClass="table"  AllowSorting="false" AutoGenerateColumns="False" UseAccessibleHeader="True">
			    <FooterStyle></FooterStyle>
				<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
				<EditItemStyle></EditItemStyle>
				<AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
				<ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
				<HeaderStyle></HeaderStyle>
				<Columns>
                <asp:TemplateColumn > 
                    <ItemTemplate >
						<asp:ImageButton ID="IdImgSelVolontari"  style="cursor:pointer;" CommandName="Select" ToolTip="Cancella Ambito Selezionato" AlternateText="Cancella Ambito Selezionato" runat="server" ImageURL="images/canc_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                  </asp:TemplateColumn>
					<asp:BoundColumn Visible="False" DataField="IdAmbito" HeaderText="IdAmbito"></asp:BoundColumn>
					<asp:BoundColumn DataField="Denominazione" HeaderText="Denominazione"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="IdAmbitoNascosto" HeaderText="IdAmbitoNascosto"></asp:BoundColumn>
				</Columns>
				<PagerStyle NextPageText="&gt;&gt;"   
					PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
			</asp:datagrid>
            
<div class="RigaVuota" >&nbsp;</div>
<div class="wrapper" style="width:100%;border:0px">
           <div class="RigaPulsanti" >
		<asp:Button id="imgConferma" runat="server" CssClass="Pulsante" Text="Conferma" ToolTip="Conferma Elenco Ambiti Selezionati"/>
        <asp:Button id="btnNascondiElencoAmbiti" runat="server"   CssClass="Pulsante" ToolTip="Nascondi Elenco Ambiti" Text="Nascondi Elenco" />
    </div>
    </div>
     </div>
        </div>


</div>

   <fieldset>
    <legend>Posti</legend>
     <div class="wrapper" style="width:100%;border:0px">
         <div class="row">
            <div class="collable" style="width:10%">
            <asp:Label ID="lblVittoEAlloggio" AssociatedControlID="txtNumeroPostiVittoAlloggio" Text="Vitto e Alloggio" CssClass="label" runat="server"></asp:Label>
            </div>
            <div class="colOggetti" style="width:10%">
            <asp:TextBox ID="txtNumeroPostiVittoAlloggio" Enabled="false" MaxLength="5" onchange="CheckNumeroValido('txtNumeroPostiVittoAlloggio','Posti Vitto e Alloggio')" onkeyup="AggiornaTotaleVolontariDaImpiegare()" runat="server" CssClass="textbox"  ></asp:TextBox>
            </div>
              <div class="collable" style="width:10%">
            <asp:Label ID="lblSenzaVittoEAlloggio" AssociatedControlID="txtNumeroPostiSenzaVittoAlloggio" Text="Senza Vitto e Alloggio" CssClass="label" runat="server"></asp:Label>
            </div>
            <div class="colOggetti" style="width:10%">
            <asp:TextBox ID="txtNumeroPostiSenzaVittoAlloggio" Enabled="false" MaxLength="5" onchange="CheckNumeroValido('txtNumeroPostiSenzaVittoAlloggio','Posti Senza Vitto e Alloggio')" onkeyup="AggiornaTotaleVolontariDaImpiegare()" runat="server" CssClass="textbox"  ></asp:TextBox>
            </div>
             <div class="collable" style="width:10%">
            <asp:Label ID="lblSoloVitto" AssociatedControlID="txtNumeroPostiSoloVitto" Text="Solo Vitto" CssClass="label" runat="server"></asp:Label>
            </div>
            <div class="colOggetti" style="width:10%">
            <asp:TextBox ID="txtNumeroPostiSoloVitto" Enabled="false" MaxLength="5" onchange="CheckNumeroValido('txtNumeroPostiSoloVitto','Posti Solo Vitto')" onkeyup="AggiornaTotaleVolontariDaImpiegare()" runat="server" CssClass="textbox"  ></asp:TextBox>
            </div>
            <div class="collable" style="width:10%">
            <asp:Label ID="lblTotale" AssociatedControlID="txtNumTotVolontaridaImpiegare" Text="Totale" CssClass="label" runat="server"></asp:Label>
            </div>
            <div class="colOggetti" style="width:10%">
            <asp:TextBox ID="txtNumTotVolontaridaImpiegare" Enabled="false" runat="server" CssClass="textbox"  ></asp:TextBox>
            </div>
        </div>
        <div  class="row">
            <div class="collable" style="width:30%">
                <asp:CheckBox id="chkGiovaniMinoriOp" Runat="server" Text="Giovani Minori Opportunità" AutoPostBack="true" ></asp:CheckBox>          
            </div>
            <div class="colOggetti" style="width:10%">
                <asp:TextBox ID="txtNumeroGiovaniMinoriOpportunita" Enabled="false" runat="server" CssClass="textbox"  ></asp:TextBox>
            </div>
        </div>
    </div>
   </fieldset>

   <% If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") And Not Request.QueryString("IdAttivita") Is Nothing Then %>
   <fieldset id="fieldSetPostiRichiesti" runat="server" style="height:auto">
    <legend>Posti Richiesti</legend>
     <div class="wrapper" style="width:100%;border:0px">
     <div class="row">
        <div class="collable" style="width:10%">
        <asp:Label ID="lblVittoAlloggioRic" AssociatedControlID="TxtVittoAlloggioRic" Text="Vitto e Alloggio" CssClass="label" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:10%">
        <asp:TextBox ID="TxtVittoAlloggioRic"  MaxLength="5"  CssClass="textbox"  runat="server"  onchange="CheckNumeroValido('TxtVittoAlloggioRic','Posti Richiesti Vitto e Alloggio')"
																				onblur="TotaleRichiesti()" ></asp:TextBox>
        </div>
          <div class="collable" style="width:10%">
        <asp:Label ID="lblNoVittoAlloggioRic" AssociatedControlID="TxtNoVittoAlloggioRic" Text="Senza Vitto e Alloggio" CssClass="label" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:10%">
        <asp:TextBox ID="TxtNoVittoAlloggioRic"  MaxLength="5"  runat="server" CssClass="textbox" onchange="CheckNumeroValido('TxtNoVittoAlloggioRic','Posti Richiesti Senza Vitto e Alloggio')"
																				onblur="TotaleRichiesti()" ></asp:TextBox>
        </div>
         <div class="collable" style="width:10%">
        <asp:Label ID="lblVittoRic" AssociatedControlID="TxtVittoRic" Text="Solo Vitto" CssClass="label" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:10%">
        <asp:TextBox ID="TxtVittoRic"  MaxLength="5"  runat="server" CssClass="textbox" onchange="CheckNumeroValido('TxtVittoRic','Posti Richiesti Solo Vitto')"
																				onblur="TotaleRichiesti()" ></asp:TextBox>
        </div>
        <div class="collable" style="width:10%">
        <asp:Label ID="lblTotaleRic" AssociatedControlID="TxtTotaleRic" Text="Totale" CssClass="label" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:10%">
        <asp:TextBox ID="TxtTotaleRic" Enabled="false" onchange="CheckNumeroValido('TxtTotaleRic','Totale')"  runat="server" CssClass="textbox" ></asp:TextBox>
        </div>
        <div class="collable" style="width:10%">
        <asp:Label ID="lblRiduzioneEnte" AssociatedControlID="TxtRiduzioneEnte" Text="Riduzione Ente" CssClass="label" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:10%">
        <asp:TextBox ID="TxtRiduzioneEnte"  onchange="CheckNumeroValido('TxtRiduzioneEnte','Riduzione Ente')" runat="server" CssClass="textbox" ></asp:TextBox>
        </div>
    </div>
    </div>
   </fieldset>
<% End If %>
    <fieldset>
    <legend>Orario Servizio</legend>
     <div class="wrapper" style="width:100%;border:0px">
        <div class="row">
            <div class="collable" style="width:15%">
                <asp:Label ID="Label3" runat="server" AssociatedControlID="CheckMonteOreTipo" Text="Modalità"></asp:Label>
            </div>
            <div class="collable" style="width:30%">
                <asp:DropDownList runat="server" ID="CheckMonteOreTipo" AutoPostBack="true" >
                    <asp:ListItem runat="server" ID="selezionareMonteOreTipo" Value="selezionare" Selected="True"  Text="Selezionare"></asp:ListItem>
                    <asp:ListItem ID="chkOreSett"  Value="0" Text="Settimanali" Runat="server"  ></asp:ListItem>
                    <asp:ListItem ID="chkMonteOre" Value="1"  Text="Monte Ore Annuo" Runat="server"  ></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="collable" style="width:15%">
                <asp:Label ID="lblOreServizioSettimanali" AssociatedControlID="txtOreServizioSettimanali" Text="N&#186; Ore Di Servizio Settimanale" runat="server" CssClass="label"></asp:Label>
            </div>
            <div class="colOggetti" style="width:15%">
                <asp:textbox id="txtOreServizioSettimanali" MaxLength="5" CssClass="textbox" Width="50%" onchange="CheckNumeroValido('txtOreServizioSettimanali', 'N&#186; Ore Di Servizio Settimanale')" runat="server"/>
            </div>
            <div class="collable" style="width:15%">
                <asp:Label ID="lblMonteOre" AssociatedControlID="txtMonteOreAnnuo" Text="N&#186; Ore Annuo" runat="server" CssClass="label"></asp:Label>
            </div>
            <div class="colOggetti" style="width:15%">
                <asp:textbox id="txtMonteOreAnnuo" MaxLength="5" Enabled="false" CssClass="textbox" Width="50%" onchange="CheckNumeroValido('txtMonteOreAnnuo','N&#186; Ore Annuo')" runat="server"/>
            </div>
            <div class="collable" style="width:15%">
                <asp:Label ID="lblNumGiorniServizio" AssociatedControlID="txtNumGiorniServizio" Text="N&#186; Giorni di Servizio Settimanali" runat="server" CssClass="label"></asp:Label>
            </div>
            <div class="colOggetti" style="width:15%">
                <asp:textbox id="txtNumGiorniServizio"  MaxLength="5" CssClass="textbox" Width="50%" onchange="CheckNumeroValido('txtNumGiorniServizio','N&#186; Giorni di Servizio Settimanali')" runat="server"/>
            </div>
        </div>
    </div>
    </fieldset>
    <div id="DivEsteroTutoraggio" runat="server" visible="true">
    <fieldset>
    <legend>Estero (UE)/Tutoraggio</legend>
         <div class="wrapper" style="width:100%;border:0px">
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:RadioButton id="optEstero" runat="server" AutoPostBack="true" 	GroupName="OptEsteroTutoraggio" Checked="False" Text="Estero (UE)" />
                </div>
                <div class="collable" style="width:15%">
                    <asp:RadioButton id="optTutoraggio"  runat="server" AutoPostBack="true" GroupName="OptEsteroTutoraggio" Checked="False" Text="Tutoraggio" />
                </div>
                <div id="divMesiUE" visible="false" runat="server">
                    <div class="collable" style="width:20%">
                        <asp:Label ID="LblMesiPrevistiUE" AssociatedControlID="txtMesiPrevistiUE" Text="Mesi Previsti Estero (UE)" runat="server" CssClass="label"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:15%">
                        <asp:textbox id="txtMesiPrevistiUE"  MaxLength="5" CssClass="textbox" Width="50%" onchange="CheckNumeroValido('txtMesiPrevistiUE','Mesi Previsti Estero (UE)')" runat="server"/>
                    </div>
                </div>
                <div id="divMesiTutoraggio" visible="false" runat="server">                
                     <div class="collable" style="width:20%">
                        <asp:Label ID="LblMesiPrevistiTutoraggio" AssociatedControlID="txtMesiPrevistiTutoraggio" Text="Mesi Previsti Tutoraggio" runat="server" CssClass="label"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:15%">
                        <asp:textbox id="txtMesiPrevistiTutoraggio"  MaxLength="5" CssClass="textbox" Width="50%" onchange="CheckNumeroValido('txtMesiPrevistiTutoraggio','Mesi Previsti Tutoraggio')" runat="server"/>
                    </div>
                </div>
                <div class="colHyperLink" style="width:25%">
                    <asp:LinkButton ID="lkbClearOptEUE_T" runat="server" style="cursor:pointer" Text="Pulisci"></asp:LinkButton>
                </div>
            </div>
     
        </div>
    </fieldset>
    </div>
     <div class="row">
        <div class="collable" style="width:15%">
            <asp:Label ID="lblTipoFinanziamento" Text="Tipo Finanziamento" runat="server" CssClass="label"></asp:Label>
        </div>
        <div class="collable" style="width:10%">
            <asp:RadioButton id="OptFinStatale" runat="server" 	GroupName="OptFinanziamento" Checked="True" Text="Statale" />
        </div>
        <div class="collable" style="width:10%">
            <asp:RadioButton id="OptFinRegionale"  runat="server" 	GroupName="OptFinanziamento" Checked="False" Text="Regionale" />
        </div>
        <div class="collable" style="width:10%">
            <asp:RadioButton id="OptFinPrivato"  runat="server" 	GroupName="OptFinanziamento" Checked="False" Text="Privato" />
        </div>
    </div>
 </div>
 </fieldset>
  
  <fieldset id="flsCaratteristicheOrganizzative" class="ContornoPagina"  >
 <legend id="CaratteristicheOrganizzative"><img  src="images/plus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor:pointer;" />&nbsp;&nbsp;<img id="TornaSuCaratteristicheOrganizzative"  style="cursor:pointer;" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Caratteristiche Organizzative</legend>
 <div class="wrapper" id="CaratteristicheOrganizzativeContent" style="width:100%;border:0px" >
  <div class="row">
    <div class="collable" style="width:50%">
    <asp:CheckBox ID="chkSistemaSelezione"  Text="Sistema di selezione accreditato" runat="server"/>
    </div>
    <div class="collable" style="width:50%">
    <asp:CheckBox ID="chkSistemaMonitoraggioAccreditato"  Text="Sistema di monitoraggio accreditato" runat="server"/>
    </div>
  </div>
   <div class="row">
    <div class="collable" style="width:15%">
    <asp:Label ID="lblEventualiCriteri" AssociatedControlID="ddlEventualiCriteri" Text="Criteri di selezione" CssClass="label" runat="server"></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
    <asp:DropDownList ID="ddlEventualiCriteri" runat="server" CssClass="ddlClass" AutoPostBack="true" >
        <asp:ListItem Value="0" Selected="True" Text="Selezionare"></asp:ListItem>
		<asp:ListItem Value="4" Text="Criteri autonomi di selezione proposti nel progetto"></asp:ListItem>
		<asp:ListItem Value="1" Text="Criteri UNSC"></asp:ListItem>
		<asp:ListItem Value="2" Text="Criteri autonomi di selezione verificati nell'accreditamento"></asp:ListItem>
		<asp:ListItem Value="3" Text="Criteri autonomi di selezione verificati nell'accreditamento da enti di 1&#170; Classe"></asp:ListItem>
    </asp:DropDownList>
    </div>
    <div class="collable" style="width:15%">
    <asp:Label ID="lblCodEnteEventuali" AssociatedControlID="txtCodEnteEventuali" Text="Codice Ente" CssClass="label" runat="server"></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
        <asp:TextBox ID="txtCodEnteEventuali" CssClass="textbox" runat="server" MaxLength="10" Enabled="false" AutoPostBack="true"  ></asp:TextBox>
    </div>
    </div>
<div class="row">
    <div class="collable" style="width:15%">
    <asp:Label ID="Label2" AssociatedControlID="ddlPianoMonitoraggio" Text="Piano di monitoraggio" CssClass="label" runat="server"></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
    <asp:DropDownList ID="ddlPianoMonitoraggio" runat="server" CssClass="ddlClass"  AutoPostBack="true"  >
        <asp:ListItem Enabled="true"  Value="0" Selected="True" Text="Selezionare"></asp:ListItem>
		<asp:ListItem Enabled="true" Value="1" Text="Piano di Monitoraggio Autonomo"></asp:ListItem>
		<asp:ListItem Enabled="true" Value="2" Text="Piano di Monitoraggio Acquisito da Ente di 1&#170; Classe"></asp:ListItem>
    </asp:DropDownList>
    </div>
    <div class="collable" style="width:15%">
    <asp:Label ID="lblPianoMonitoraggio" AssociatedControlID="txtPianoMonitoraggio" Text="Codice Ente" CssClass="label" runat="server"></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
        <asp:TextBox ID="txtPianoMonitoraggio" CssClass="textbox" runat="server" MaxLength="10" Enabled="false" AutoPostBack="true"  ></asp:TextBox>
    </div>
    </div>
<div class="row">
       <div class="collable" style="width:15%">
    <asp:Label ID="lblOrePromozioneSensibilizzazione" AssociatedControlID="txtOrePromozioneSensibilizzazione" Text="Ore promozione e sensibilizzazione" CssClass="label" runat="server"></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
        <asp:TextBox ID="txtOrePromozioneSensibilizzazione" onchange="CheckNumeroValido('txtOrePromozioneSensibilizzazione','Ore promozione e sensibilizzazione')"  CssClass="textbox" Width="50%" runat="server"  ></asp:TextBox>
    </div>
    </div>
    </div>
 </fieldset>
 
  <fieldset id="flsConoscenzeAcquisite" class="ContornoPagina" >
 <legend id="ConoscenzeAcquisite"><img  src="images/plus_xsmall.png" alt="Espandi/Riduci" style="cursor:pointer;"/>&nbsp;&nbsp;<img style="cursor:pointer;" id="TornaSuConoscenzeAcquisite" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Caratteristiche Conoscenze Acquisite</legend>
 <div class="wrapper" id="ConoscenzeAcquisiteContent" style="width:100%;border:0px">
 <div class="row">
       <div class="collable" style="width:15%">
    <asp:Label ID="lblCreditiFormativiRiconosciuti" AssociatedControlID="ddlCreditiFormativiRiconosciuti" Text="Crediti Formativi Riconosciuti" CssClass="label" runat="server"></asp:Label>
    </div>
     <div class="colOggetti" style="width:15%">
        <asp:DropDownList ID="ddlCreditiFormativiRiconosciuti" runat="server" CssClass="ddlClass">
        		<asp:ListItem Value="10" Selected="True" Text="Selezionare"></asp:ListItem>
				<asp:ListItem Value="0" Text="Assenti"></asp:ListItem>
				<asp:ListItem Value="1" Text="Presenti"></asp:ListItem>
        </asp:DropDownList>
    </div>
        <div class="collable" style="width:15%">
 <asp:Label ID="lblEventualiTirociniRiconosciuti" AssociatedControlID="ddlEventualiTirociniRiconosciuti" Text="Eventuali Tirocini Riconosciuti" CssClass="label" runat="server"></asp:Label>
    </div>
     <div class="colOggetti" style="width:15%">
        <asp:DropDownList ID="ddlEventualiTirociniRiconosciuti" runat="server" CssClass="ddlClass">
        		<asp:ListItem Value="10" Selected="True" Text="Selezionare"></asp:ListItem>
				<asp:ListItem Value="0" Text="Assenti"></asp:ListItem>
				<asp:ListItem Value="1" Text="Presenti"></asp:ListItem>
        </asp:DropDownList>
    </div>
           <div class="collable" style="width:15%">
 <asp:Label ID="lblCompetenzeAcquisibili" AssociatedControlID="ddlCompetenzeAcquisibili" Text="Competenze Acquisibili" CssClass="label" runat="server"></asp:Label>
    </div>
     <div class="colOggetti" style="width:15%">
        <asp:DropDownList ID="ddlCompetenzeAcquisibili" runat="server" CssClass="ddlClass">
        		<asp:ListItem Value="10" Selected="True" Text="Selezionare"></asp:ListItem>
				<asp:ListItem Value="0" Text="Assenti"></asp:ListItem>
				<asp:ListItem Value="1" Text="Presenti"></asp:ListItem>
        </asp:DropDownList>
    </div>
 </div>
 </div>
 </fieldset>

  <fieldset id="flsFormazioneGenerale" class="ContornoPagina" >
 <legend id="FormazioneGenerale"><img style="cursor:pointer;" src="images/plus_xsmall.png" alt="Espandi/Riduci" />&nbsp;&nbsp;<img style="cursor:pointer;" id="TornaSuFormazioneGenerale" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Formazione Generale</legend>
 <div class="wrapper" id="FormazioneGeneraleContent" style="width:100%;border:0px">
 <div class="row">
       <div class="collable" style="width:15%">
    <asp:Label ID="lblDurata" AssociatedControlID="txtDurata" Text="Durata (Ore)" CssClass="label" runat="server"></asp:Label>
    </div>
     <div class="colOggetti" style="width:35%">
     <asp:TextBox ID="txtDurata" CssClass="textbox" AutoPostBack="true" onchange="CheckNumeroValido('txtDurata','Durata (Ore)')" Width="50%" runat="server" MaxLength="5"></asp:TextBox>
     </div>
       <div class="collable" style="width:15%">
    <asp:Label ID="lblModalitaAttuazione" AssociatedControlID="ddlModalitaAttuazione" Text="Modalit&#224; di Attuazione" CssClass="label" runat="server"></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
    <asp:DropDownList ID="ddlModalitaAttuazione" CssClass="ddlClass" runat="server" AutoPostBack="true" >
		<asp:ListItem Value="0" Selected="True" Text="Selezionare"></asp:ListItem>
		<asp:ListItem Value="1" Text="In Proprio"></asp:ListItem>
		<asp:ListItem Value="2" Text="Servizio Acquisito da Ente di 1&#170; Classe"></asp:ListItem>
		<asp:ListItem Value="3" Text="Servizio Acquisito da Regione o Provincia Autonoma"></asp:ListItem>
    </asp:DropDownList>
    </div>
</div>
 <div class="row">
       <div class="collable" style="width:15%">
    <asp:Label ID="labelModalitaAttuazione" AssociatedControlID="txtModalitaAttuazione" Text="Codice Ente" CssClass="label" runat="server"></asp:Label>
    </div>
     <div class="colOggetti" style="width:35%">
     <asp:TextBox ID="txtModalitaAttuazione" CssClass="textbox" runat="server" Enabled="false"  MaxLength="10"></asp:TextBox>
     </div>
</div>
 <div class="row">
       <div class="collable" style="width:15%">
    <asp:Label ID="labelModErogazione"  AssociatedControlID="ddlFormazioneGeneraleErogazione" Text="Modalit&#224; di erogazione" CssClass="label" runat="server"></asp:Label>
    </div>
     <div class="colOggetti" style="width:35%">
        <asp:DropDownList runat="server" ID="ddlFormazioneGeneraleErogazione" AutoPostBack="true" >
             <asp:ListItem ID="selezionareFormazioneGenerale"  Value="selezionare" Text="Selezionare" Runat="server"  Selected="True" ></asp:ListItem>
             <asp:ListItem ID="chkUnica"  Value="unicaTranche" Text="Unica Tranche" Runat="server"  ></asp:ListItem>
             <asp:ListItem ID="chkPercentuale" Value="percentuale"  Text="80% - 20%" Runat="server"  ></asp:ListItem>
     </asp:DropDownList>
  
     </div>
</div>
 <div class="row">
 <div class="collable" style="width:15%">
 <asp:Label id="lbl180" AssociatedControlID="txt180" CssClass ="label" runat="server" Text="Entro il 180&#186; giorno"></asp:Label>
 </div> 
 <div class="colOggetti" style="width:35%">
 	<asp:Label id="txt180" runat="server"  CssClass ="labelDati" Text="0" ></asp:Label>
</div>
<div class="collable" style="width:15%">
	<asp:Label id="lbl270" AssociatedControlID="txt270"  CssClass ="label" Text="Dal 210&#186; al 270&#186; giorno" runat="server"></asp:Label>
</div>
 <div class="colOggetti" style="width:35%">
	<asp:Label id="txt270" runat="server"   CssClass="labelDati" Text="0" ></asp:Label>
</div>
 </div>
</div>
 </fieldset>
  <fieldset id="flsFormazioneSpecifica" class="ContornoPagina"  >
 <legend id="FormazioneSpecifica"><img style="cursor:pointer;" src="images/plus_xsmall.png" alt="Espandi/Riduci" />&nbsp;&nbsp;<img style="cursor:pointer;" id="TornaSuFormazioneSpecifica" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Formazione Specifica</legend>
 <div class="wrapper" id="FormazioneSpecificaContent" style="width:100%;border:0px">
 <div class="row">
       <div class="collable" style="width:15%">
    <asp:Label ID="lblDurataSpec" AssociatedControlID="txtDurataSpec" Text="Durata (Ore)" CssClass="label" runat="server"></asp:Label>
    </div>
     <div class="colOggetti" style="width:35%">
     <asp:TextBox ID="txtDurataSpec" onchange="CheckNumeroValido('txtDurataSpec','Durata (Ore)')" CssClass="textbox" Width="50%"  AutoPostBack="true" runat="server" MaxLength="5"></asp:TextBox>
     </div>
       <div class="collable" style="width:15%">
    <asp:Label ID="lblModAttuazione" AssociatedControlID="ddlmodattuazione" Text="Modalit&#224; di Attuazione" CssClass="label" runat="server"></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
    <asp:DropDownList ID="ddlmodattuazione" CssClass="ddlClass" runat="server" AutoPostBack="true" >
		<asp:ListItem Value="0" Selected="True" Text="Selezionare"></asp:ListItem>
		<asp:ListItem Value="1" Text="In Proprio"></asp:ListItem>
		<asp:ListItem Value="2" Text="Affidata ad enti di 1&#170; classe"></asp:ListItem>
		<asp:ListItem Value="3" Text="Affidata a Terzi"></asp:ListItem>
    </asp:DropDownList>
    </div>
</div>
 <div class="row">
       <div class="collable" style="width:15%">
    <asp:Label ID="labelmodattuazione" AssociatedControlID="txtmodattuazione" Text="Codice Ente" CssClass="label" runat="server"></asp:Label>
    </div>
     <div class="colOggetti" style="width:35%">
     <asp:TextBox ID="txtmodattuazione" CssClass="textbox" runat="server" Enabled="false" MaxLength="10"></asp:TextBox>
     </div>
</div>
 <div class="row">
       <div class="collable" style="width:15%">
    <asp:Label ID="lblErogazione"  Text="Modalit&#224; di erogazione" AssociatedControlID="ddlFormazioneSpecificaErogazione" CssClass="label" runat="server"></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">

          <asp:DropDownList runat="server" ID="ddlFormazioneSpecificaErogazione" AutoPostBack="true" >
             <asp:ListItem ID="selezionareFormazioneSpecifica"  Value="selezionare" Text="Selezionare" Runat="server"  Selected="True" ></asp:ListItem>
             <asp:ListItem ID="ChkUnicaS"  Value="unicaTranche" Text="Unica Tranche" Runat="server"  ></asp:ListItem>
             <asp:ListItem ID="ChkPercentualeS" Value="percentuale"  Text="70% - 30%" Runat="server"  ></asp:ListItem>
     </asp:DropDownList>
     </div>
</div>
 <div class="row">
 <div class="collable" style="width:15%">
 <asp:Label id="lbl90S" AssociatedControlID="txt90S" CssClass ="label" runat="server" Text="Entro il 90&#186; giorno"></asp:Label>
 </div> 
 <div class="colOggetti" style="width:35%">
 	<asp:Label id="txt90S" runat="server" Text="0" CssClass ="labelDati" ></asp:Label>
</div>
<div class="collable" style="width:15%">
	<asp:Label id="lbl270S" AssociatedControlID="txt270S"  CssClass ="label" Text="Fino al 270&#186; giorno" runat="server"></asp:Label>
</div>
 <div class="colOggetti" style="width:35%">
	<asp:Label id="txt270S" runat="server" Text="0" CssClass="labelDati" ></asp:Label>
</div>
 </div>
</div>
 </fieldset>
  <fieldset id="flsAltriElementi" class="ContornoPagina"  >
 <legend id="AltriElementi"><img  src="images/plus_xsmall.png" alt="Espandi/Riduci" style="cursor:pointer;" />&nbsp;&nbsp;<img style="cursor:pointer;" id="TornaSuAltriElementi" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Altri Elementi Formazione</legend>
 <div class="wrapper" id="AltriElementiContent" style="width:100%;border:0px">

 <div class="row">
   <div class="collable" style="width:100%">
 <asp:Label ID="lblSezioneNonPrevista"  Text="Sezione ancora non prevista per l'inserimento dei Progetti."  CssClass="msgInfo" runat="server" ></asp:Label>
 </div>
 </div>
 </div>
 </fieldset>

  <fieldset id="flsEstero" class="ContornoPagina" >
    <legend id="Estero"><img id="ExpandEstero" src="images/plus_xsmall.png" style="cursor:pointer;" alt="Espandi/Riduci" />&nbsp;&nbsp; <img id="TornaSuEstero" onclick="TornaAdInizioPagina()" style="cursor:pointer;" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Estero</legend> 
<div class="wrapper"  id="EsteroContent" style="width:100%;border:0px">

<div class="row" style="height:auto">
<div class="collable" style="width:100%">
<asp:Label id="lblSezioneEsteroNonPresente" CssClass="msgInfo" runat="server" Visible="false" Text="Sezione non presente per i progetti in Italia."></asp:Label>
</div>
</div>
<div class="rowGroup" id="DivSezioneEstero" runat="server">
 <div class="row">
    <div class="collable" style="width:15%">
        <asp:Label ID="lblCondizioniRischio" AssociatedControlID="ddlCondizioniRischio" Text="Particolari Condizioni di Rischio" CssClass="label" runat="server"></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
        <asp:DropDownList ID="ddlCondizioniRischio" runat="server" CssClass="ddlClass">
        		<asp:ListItem Value="10" Selected="True" Text="Selezionare"></asp:ListItem>
				<asp:ListItem Value="0" Text="Esistenti"></asp:ListItem>
				<asp:ListItem Value="1" Text="Non Esistenti"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="collable" style="width:15%">
        <asp:Label ID="lblLivelliSicurezza" AssociatedControlID="ddlLivelliSicurezza" Text="Livelli Misure di Sicurezza" CssClass="label" runat="server"></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
        <asp:DropDownList ID="ddlLivelliSicurezza" runat="server" CssClass="ddlClass">
        		<asp:ListItem Value="10" Selected="True" Text="Selezionare"></asp:ListItem>
				<asp:ListItem Value="0" Text="Adottate"></asp:ListItem>
				<asp:ListItem Value="1" Text="Non Adottate"></asp:ListItem>
        </asp:DropDownList>
    </div>
  
 </div>
 <div class="row">
    <div class="collable" style="width:15%">
        <asp:Label ID="lblCondizioniDisagio" AssociatedControlID="ddlCondizioniDisagio" Text="Particolari Condizioni di Disagio" CssClass="label" runat="server"></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
        <asp:DropDownList ID="ddlCondizioniDisagio" runat="server" CssClass="ddlClass">
        		<asp:ListItem Value="10" Selected="True" Text="Selezionare"></asp:ListItem>
				<asp:ListItem Value="0" Text="Esistenti"></asp:ListItem>
				<asp:ListItem Value="1" Text="Non Esistenti"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="collable" style="width:15%">
        <asp:Label ID="lblEventualeAssicurazione" AssociatedControlID="ddlEventualeAssicurazione" Text="Eventuale Assicurazione Integrativa" CssClass="label" runat="server"></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
        <asp:DropDownList ID="ddlEventualeAssicurazione" runat="server" CssClass="ddlClass">
        		<asp:ListItem Value="10" Selected="True" Text="Selezionare"></asp:ListItem>
				<asp:ListItem Value="0" Text="Si"></asp:ListItem>
				<asp:ListItem Value="1" Text="No"></asp:ListItem>
        </asp:DropDownList>
    </div>
  
 </div>
 </div>
 
 </div>
 </fieldset>

 <div class="rowGroup" id="divCoProgettazione" runat="server" visible="true">
   <div class="row">
       <div class="collable" style="width:20%" >
        <asp:CheckBox id="chkCoProgettato" Runat="server" Enabled="False" Text="Progetto in coprogettazione"></asp:CheckBox>
       </div>
       <div class="colOggetti" >
         <asp:ImageButton id="imgCoProgettazione" ToolTip="Gestione Enti Coprogettanti" AlternateText="Gestione Enti Coprogettanti" ImageUrl="Images/Icona_Progetto_small.png"    Runat="server"></asp:ImageButton>
       </div>
   </div>

  
  
</div>

<fieldset class="ContornoPagina" id="DivStatoValutazione" runat="server" visible="false" style="height:auto" >
<legend>Stato Valutazione</legend>
 <div class="wrapper" style="width:100%;border:0px;height:auto">

<div class="row">
      <div class="collable" style="width:15%" >
      <asp:Label ID="LblStatoValutazione" AssociatedControlID="ddlStatoValutazione" Text="Stato Valutazione"  CssClass="label" runat="server"></asp:Label>
      </div>
        <div class="colOggetti" style="width:35%" >
        	<asp:dropdownlist id="ddlStatoValutazione" Enabled="False" runat="server" >
			<asp:ListItem Value="0" Selected="True" Text="Non Definito"></asp:ListItem>
			<asp:ListItem Value="1" Text="Ammissibile"></asp:ListItem>
			<asp:ListItem Value="2" Text="Non Ammissibile"></asp:ListItem>
            <asp:ListItem Value="3" Text="Escluso"></asp:ListItem>
            <asp:ListItem Value="4" Text="Non Valutabile"></asp:ListItem>
		</asp:dropdownlist>
        </div>
</div>
<div class="RigaPulsanti">
    <asp:Button id="ImgApplicaStatoValutazione" Text="Applica" CssClass="Pulsante" Runat="server" visible="false" ></asp:Button>
   </div>
   </div>

</fieldset>


<div class="RigaVuota">&nbsp;</div>
<div class="RigaPulsanti" > 
<asp:Button id="imgRevisioneProgetto" CssClass="Pulsante" runat="server" Text="Revisione Progetto" Visible="false"> </asp:Button>
	<asp:Button id="imgRicorso" CssClass="Pulsante" runat="server" Text="Ricorso Progetto" Visible="false" ></asp:Button>
	<asp:Button id="ImgDuplica" CssClass="Pulsante" runat="server" Text="Duplica Progetto" Visible="false" ></asp:Button>
    <asp:Button id="imgCollegaIstanza" CssClass="Pulsante" runat="server" Text="Collega progetto ad istanza" Visible="false"></asp:Button>
</div>
<div class="RigaVuota">&nbsp;</div>
<div class="RigaVuota">&nbsp;</div>
<div class="RigaPulsanti" > 
	<asp:Button id="cmdSalva" CssClass="Pulsante" runat="server" OnClientClick="return Salva();" Text="Salva"></asp:Button>
    <asp:Button id="imgCancella" CssClass="Pulsante" runat="server" Visible="False" Text="Ritira"></asp:Button>
	<asp:Button id="imgRipristina" CssClass="Pulsante" runat="server" Visible="False" Text="Ripristina" BorderWidth="1px"></asp:Button>
    <asp:Button id="imgChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" ></asp:Button>
</div>
</div>
<asp:HiddenField id="hddRicordaFascicolo" runat="server" />
<asp:HiddenField id="TxtIdFascicolo" runat="server" />
<asp:HiddenField runat="server" id="AltroVociSpesa" />
<asp:HiddenField runat="server" id="AltroImporto" />
<asp:HiddenField runat="server" id="TotAltroVociSpesa" /> 
<asp:HiddenField runat="server" id="TotVociSpesa" />
<asp:HiddenField runat="server" id="VociSpesa" /> 
<asp:HiddenField runat="server" id="Importo" /> 
<asp:HiddenField runat="server" id="txtIdAree" />
<asp:HiddenField runat="server" id="txtIdPartner" /> 
<asp:HiddenField runat="server" id="txtIdTipoPubblicita" />
<asp:HiddenField runat="server" id="txtIdFormatori" /> 
<asp:HiddenField runat="server" id="txtIdAreaPrincipale" />
<asp:HiddenField runat="server" id="txtIdArea" /> 
<asp:HiddenField runat="server" id="txtIdAltro" /> 
<asp:HiddenField runat="server" id="txtIdAttOrganizzativa" />
<asp:HiddenField runat="server" id="txtIdFormSpecifica" /> 
<asp:HiddenField runat="server" id="txtCodAtt" />
<asp:HiddenField runat="server" id="txtIdMacroAm" /> 
<asp:HiddenField runat="server" id="txtIdAreaPrincipaleSelezionata" />
<asp:HiddenField id="txtIdAmbitoAttivita" runat="server" />
<asp:HiddenField id="strIdFormatoriModifica" runat="server" />
<asp:HiddenField id="strIdPartnerModifica" runat="server" />
<asp:HiddenField id="strIdAmbitiModifica" runat="server" />
<asp:HiddenField id="txtIdAttivitaCaratteristicaOrganizzativa" runat="server" />
<asp:HiddenField id="txtIdAttivitaFormazioneSpecifica" runat="server" />
<asp:HiddenField id="txtIdAttivitaAltroFormazione" runat="server" />
<asp:HiddenField id="txtCodAttivita" runat="server" />
<asp:HiddenField id="strIdMacroAmbito" runat="server" />
<asp:HiddenField id="chkVisualizzazione" runat="server" />
<asp:HiddenField runat="server" ID="hdnIsPostbackFG" />
<asp:HiddenField runat="server" ID="hdnIsPostbackFS" />
<asp:HiddenField runat="server" ID="hdnIsPostbackProg" />
<asp:HiddenField runat="server" ID="hdnIsPostbackCO" />
<asp:HiddenField runat="server" ID="hdnIsPostbackCCA" />
<asp:HiddenField runat="server" ID="hdnIsPostbackEstero" />
</fieldset>
 
      </div>
        </div>
 
        </div>
 
</asp:Content>

