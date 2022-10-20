<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="TabProgetti2020.aspx.vb" Inherits="Futuro.TabProgetti2020" MaintainScrollPositionOnPostBack="true" %>
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
      
    <script  type="text/javascript">
       /* <![CDATA[ */
        var pathPlus = "images/plus_xsmall.png";
        var pathMinus = "images/minus_xsmall.png";
        var titlePathMinus = "Riduci";
        var titlePathPlus = "Espandi";
        var inserimento = '<%=Request.Params("IdAttivita")%>';
        var isPostbackEnte = '<%= hdnIsPostbackEnte.value %>' ;
        var isPostbackProgetto = '<%= hdnIsPostbackProgetto.value %>' ;
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

        function Totale() {
            var txtorecollettive = parseInt(document.getElementById(CostruisciId("txtorecollettive")).value);
            var txtoreindividuali = parseInt(document.getElementById(CostruisciId("txtoreindividuali")).value);
            //var txttotaleore = parseInt(document.getElementById(CostruisciId("txttotaleore")).value);

            if (isNaN(txtorecollettive) == true) {
                vittoAlloggio = 0;
            }
            if (isNaN(txtoreindividuali) == true) {
                noVittoAlloggio = 0;
            }
           
//            if (isNaN(txttotaleore) == true) {
//                noVittoAlloggio = 0;
//            }

            document.getElementById(CostruisciId("txttotaleore")).value = txtorecollettive + txtoreindividuali ;
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

            if ('<%=request.querystring("Nazionale")%>' != '12') {
                    
                document.getElementById(CostruisciId("GroupEstero")).visible == false                
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
                if ('<%=request.querystring("IdAttivita")%>' == '') {
                        $('#CaratteristicheEnteContent').css('display', 'none');
                        $('#CaratteristicheEnte').find("img").first().attr("src",pathPlus );
                        $('#CaratteristicheEnte').find("img").first().attr("title", titlePathPlus);
                }

             $('#TornaSuEnti').click(function () 
             {
                tornaSu = 1;
                
             });
            $('#CaratteristicheEnte').click(function () 
            {
             
                if (tornaSu == 0) 
                {
                    var isVisible = $('#CaratteristicheEnteContent').is(':visible');
                    $('#CaratteristicheEnteContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    if (!isVisible) 
                    {
                        $(this).find("img").first().attr("src", pathMinus);
                        $(this).find("img").first().attr("title", titlePathMinus);
                        title = "Espandi"
                    }
                    else 
                    {
                        $(this).find("img").first().attr("src", pathPlus);
                        $(this).find("img").first().attr("title", titlePathPlus);
                    }
                }
                tornaSu = 0;
                return false;
            });
        });
        $(function () 
        {
           
         
            var tornaSu = 0;
          
                
                $('#ProgettoContent').css('display', 'yes');
                $('#Progetto').find("img").first().attr("src", pathMinus);
                $('#Progetto').find("img").first().attr("title", titlePathMinus);

          
            $('#TornaSuProgetto').click(function () 
            {
              
                tornaSu = 1;
            });
            $('#Progetto').click(function () 
            {
              
                if (tornaSu == 0) 
                {
              
                    var isVisible = $('#ProgettoContent').is(':visible');
                    $('#ProgettoContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    if (!isVisible) 
                    {
                      
                        $(this).find("img").first().attr("src", pathMinus);
                        $(this).find("img").first().attr("title", titlePathMinus);
                        
                    }
                    else 
                    {
                     
                        $(this).find("img").first().attr("src", pathPlus);
                        $(this).find("img").first().attr("title", titlePathPlus);
                       
                    }
                }
                tornaSu = 0;
                return false;
            });
        });
        $(function () 
        {
        
            var tornaSu = 0;
           
                $('#CaratteristicheOrganizzativeContent').css('display', 'yes');
                $('#CaratteristicheOrganizzative').find("img").first().attr("src", pathMinus);
                $('#CaratteristicheOrganizzative').find("img").first().attr("title", titlePathMinus);
                
            
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
           
         
                $('#FormazioneGeneraleContent').css('display', 'yes');
                $('#FormazioneGenerale').find("img").first().attr("src", pathMinus);
                $('#FormazioneGenerale').find("img").first().attr("title", titlePathMinus);
               
               
              
           
           
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
             
                $('#FormazioneSpecificaContent').css('display', 'yes');
                $('#FormazioneSpecifica').find("img").first().attr("src",pathMinus );
                $('#FormazioneSpecifica').find("img").first().attr("title",titlePathMinus );
      
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
            
           
                $('#EsteroContent').css('display', 'yes');
                $('#Estero').find("img").first().attr("src", pathMinus);
                $('#Estero').find("img").first().attr("title", titlePathMinus);
           
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
             var Mesi = 0;
             var Giorni = 0;
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
            if (document.getElementById(CostruisciId("ddlRequisitiVolontari")).value == -1) {
                 messaggioDiErrore = "Sezione Progetto - Selezionare gli ulteriori requisiti richiesti ai candidati.";
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


                         if (parseInt(document.getElementById(CostruisciId("txtOreServizioSettimanali")).value) < 20) {
                             messaggioDiErrore = "Sezione Progetto - Inserire almeno 20 ore di Servizio Settimanale.";
                             ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                             validazioneOk = false;
                         }
                         else {
                             if (parseInt(document.getElementById(CostruisciId("txtOreServizioSettimanali")).value) > 36) {
                                 messaggioDiErrore = "Sezione Progetto - Il valore inserito  delle ore settimanali e' superiore al numero massimo consentito di 36.";
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
        
            <% If Request.QueryString("Nazionale") = 11  Then %>
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

                if (document.getElementById(CostruisciId("optEstero")).checked == true) {         
                
                    if (document.getElementById(CostruisciId("txtMesiPrevistiUE")).value == '' && document.getElementById(CostruisciId("txtGiorniPrevistiUE")).value == '') {
                        messaggioDiErrore = "Sezione Progetto - Inserire il numero di mesi o giorni del periodo all'Estero.";
                        ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                        validazioneOk = false;
                    }

                    if (parseInt(document.getElementById(CostruisciId("txtMesiPrevistiUE")).value) == 0 && parseInt(document.getElementById(CostruisciId("txtGiorniPrevistiUE")).value) == 0) {
                        messaggioDiErrore = "Sezione Progetto - Inserire il numero di mesi o giorni del periodo all'Estero.";
                        ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                        validazioneOk = false;
                    }

                    if (document.getElementById(CostruisciId("txtMesiPrevistiUE")).value != '') {
                        Mesi = parseInt(document.getElementById(CostruisciId("txtMesiPrevistiUE")).value)
                    }

                    if (document.getElementById(CostruisciId("txtGiorniPrevistiUE")).value != '') {
                        Giorni = parseInt(document.getElementById(CostruisciId("txtGiorniPrevistiUE")).value)
                    }

   //             alert("Mesi = " + Mesi)
   //             alert ("Giorni = " + Giorni)                
                
                    if (document.getElementById(CostruisciId("txtMesiPrevistiUE")).value != '' && Mesi > 3) { 
                       messaggioDiErrore = "Sezione Progetto - Il valore inserito dei mesi previsti per Estero (UE) è superiore al numero massimo consentito.";
                       ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                       validazioneOk = false;
                    }

                    if (document.getElementById(CostruisciId("txtMesiPrevistiUE")).value != ''  && Mesi == 0) { 
                       messaggioDiErrore = "Sezione Progetto - Il valore inserito dei mesi previsti per Estero (UE) è inferiore al numero minimo consentito.";
                       ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                       validazioneOk = false;
                    }

                    if (document.getElementById(CostruisciId("txtGiorniPrevistiUE")).value != ''  && Giorni > 90) { 
                       messaggioDiErrore = "Sezione Progetto - Il valore inserito dei giorni previsti per Estero (UE) è superiore al numero massimo consentito.";
                       ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                       validazioneOk = false;
                    }
                    
                    if (document.getElementById(CostruisciId("txtGiorniPrevistiUE")).value != ''  && Giorni < 30) { 
                       messaggioDiErrore = "Sezione Progetto - Il valore inserito dei giorni previsti per Estero (UE) è inferiore al numero minimo consentito.";
                       ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                       validazioneOk = false;
                    }


                    if (document.getElementById(CostruisciId("ddlTransfrontaliero")).value < 0) {
                        messaggioDiErrore = "Sezione Progetto - Selezionare il valore Transfrontaliero.";
                        ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                        validazioneOk = false;
                    }
                    if (document.getElementById(CostruisciId("ddlTransfrontaliero")).value == 1) {
                        if (document.getElementById(CostruisciId("ddlModalità")).value < 0) {
                            messaggioDiErrore = "Sezione Progetto - Selezionare la Modalità.";
                            ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                            validazioneOk = false;
                        }
                    }
                 }

                if (document.getElementById(CostruisciId("optTutoraggio")).checked == true) {                     
                    if (parseInt(document.getElementById(CostruisciId("txtMesiPrevistiTutoraggio")).value) == 0 && parseInt(document.getElementById(CostruisciId("txtGiorniPrevistiTutoraggio")).value) == 0) {
                        messaggioDiErrore = "Sezione Progetto - Inserire il numero di mesi o giorni del periodo di tutoraggio.";
                        ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                        validazioneOk = false;
                    }

                       if (parseInt(document.getElementById(CostruisciId("txtMesiPrevistiTutoraggio")).value) != ''  && parseInt(document.getElementById(CostruisciId("txtMesiPrevistiTutoraggio")).value) > 3){ 

                       messaggioDiErrore = "Sezione Progetto - Il valore inserito dei mesi previsti di Tutoraggio è superiore al numero massimo consentito.";
                        ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                        validazioneOk = false;

                      }
                     if (parseInt(document.getElementById(CostruisciId("txtMesiPrevistiTutoraggio")).value) != ''  && parseInt(document.getElementById(CostruisciId("txtMesiPrevistiTutoraggio")).value) == 0){ 

                       messaggioDiErrore = "Sezione Progetto - Il valore inserito dei mesi previsti di Tutoraggio è inferiore al numero minimo consentito.";
                        ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                        validazioneOk = false;

                      }



                      if (parseInt(document.getElementById(CostruisciId("txtGiorniPrevistiTutoraggio")).value) != ''  && parseInt(document.getElementById(CostruisciId("txtGiorniPrevistiTutoraggio")).value) == 0){ 

                       messaggioDiErrore = "Sezione Progetto - Il valore inserito dei giorni previsti di Tutoraggio è inferiore al numero minimo consentito.";
                        ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                        validazioneOk = false;

                      }


                     if (parseInt(document.getElementById(CostruisciId("txtGiorniPrevistiTutoraggio")).value) != ''  && parseInt(document.getElementById(CostruisciId("txtGiorniPrevistiTutoraggio")).value) > 90){ 

                       messaggioDiErrore = "Sezione Progetto - Il valore inserito dei giorni previsti di Tutoraggio è superiore al numero massimo consentito.";
                        ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                        validazioneOk = false;

                      }
                     if (parseInt(document.getElementById(CostruisciId("txtGiorniPrevistiTutoraggio")).value) != ''  && parseInt(document.getElementById(CostruisciId("txtGiorniPrevistiTutoraggio")).value) < 30){ 

                       messaggioDiErrore = "Sezione Progetto - Il valore inserito dei giorni previsti di Tutoraggio è inferiore al numero minimo consentito.";
                        ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                        validazioneOk = false;

                      }

                }
            <% End If %>

            <% If Request.QueryString("Nazionale") = 12 Then %>
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
            <% End If %>
                  
             //Criteri di Selezione
             
              if (document.getElementById(CostruisciId("ddlPartner")).value < 0) {
                 messaggioDiErrore = "Sezione Caratteristiche Organizzative - Selezionare Presenza Partner Si/No.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
                 }
             //Criteri di Selezione
           
            
             //Caratteristiche Conoscenze Acquisite
           
            
//             if (document.getElementById(CostruisciId("ddlCompetenzeAcquisibili")).value == 10) {
//                 messaggioDiErrore = "Sezione Caratteristiche Conoscenze Acquisite - Selezionare le Competenze Acquisibili.";
//                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
//                 validazioneOk = false;
//             }
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
//             if (document.getElementById(CostruisciId("ddlModalitaAttuazione")).value == 0) {
//                 messaggioDiErrore = "Sezione Formazione Generale - Selezionare la Modalit&#224; di Attuazione della Formazione Generale.";
//                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
//                 validazioneOk = false;
//             }
//             if ((document.getElementById(CostruisciId("ddlModalitaAttuazione")).value == 2) && (document.getElementById(CostruisciId("txtModalitaAttuazione")).value == '')) {
//                 messaggioDiErrore = "Sezione Formazione Generale - Inserire il codice ente per la Modalit&#224; di Attuazione.";
//                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
//                 validazioneOk = false;
//             }

//              if (document.getElementById(CostruisciId("TipoEnte")).value =="SI")
//              {
////                if (document.getElementById(CostruisciId("ddlModalitaAttuazione")).value != 1) 
////                {
////                messaggioDiErrore = "Sezione Formazione Generale - Selezionare la Modalit&#224; di Attuazione della Formazione Generale in 'In Proprio'";
////                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
////                 validazioneOk = false;
////                }
//              }
             if (document.getElementById(CostruisciId("ddlFormazioneGeneraleErogazione")).value == "selezionare")  {
                 messaggioDiErrore = "Sezione Formazione Generale - Selezionare una Modalit&#224; di Erogazione della Formazione Generale.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }

//             if (document.getElementById(CostruisciId("ddlFormazioneGeneraleErogazione")).value ==  "percentuale" && document.getElementById(CostruisciId("ddlDurata")).value != 12)  {
//                 messaggioDiErrore = "Sezione Formazione Generale - Per i progetti di durata inferiore a 12 mesi, l'erogazione della formazione generale deve essere ad Unica Tranche. ";
//                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
//                 validazioneOk = false;
//             }
//             //Formazione Specifica

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

//             if (document.getElementById(CostruisciId("ddlmodattuazione")).value == 0) {
//                 messaggioDiErrore = "Sezione Formazione Specifica - Selezionare la Modalit&#224; di Attuazione della Formazione Specifica.";
//                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
//                 validazioneOk = false;
//             }
//             if ((document.getElementById(CostruisciId("ddlmodattuazione")).value == 2) && (document.getElementById(CostruisciId("txtmodattuazione")).value == '')) {
//                 messaggioDiErrore = "Sezione Formazione Specifica - Inserire il codice ente per la Modalit&#224; di Attuazione della Formazione Specifica.";
//                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
//                 validazioneOk = false;
//             }

             if (document.getElementById(CostruisciId("ddlFormazioneSpecificaErogazione")).value == "selezionare")  {
                 messaggioDiErrore = "Sezione Formazione Specifica - Selezionare una Modalit&#224; di Erogazione della Formazione Specifica.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }

//            if (document.getElementById(CostruisciId("ddlFormazioneSpecificaErogazione")).value ==  "percentuale" && document.getElementById(CostruisciId("ddlDurata")).value != 12)  {
//                 messaggioDiErrore = "Sezione Formazione Specifica - Per i progetti di durata inferiore a 12 mesi, l'erogazione della formazione specifica deve essere ad Unica Tranche. ";
//                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
//                 validazioneOk = false;
//             }

             //SEZIONE ESTERO
             if ('<%=request.querystring("Nazionale")%>' == '12') { //Estero
                 //Particolari Condizioni di Rischio
                
                 //Livelli Misure di Sicurezza
               
                 //Particolari Condizioni di Disagio
               
                 //Eventuale Assicurazione Integrativa
                 

                   var numeromesi =  parseInt(document.getElementById(CostruisciId("txtNumeroMesiEstero")).value);
                   var durata = parseInt(document.getElementById(CostruisciId("ddlDurata")).value);

                   

                 if (numeromesi == ''){
                
                 messaggioDiErrore = "Sezione Estero - Specificare il numero dei mesi da un minimo di 5 ad un massimo di 12 asseconda del tipo di progetto scelto.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
                 }

                   if (durata == 8  ||  durata == 9) 
                   {
                 
                    if (numeromesi < 5 || numeromesi > durata)
                    {
                     messaggioDiErrore = "Sezione Estero -  il numero dei mesi all'estero deve essere da un minimo di 5 ad un massimo della durata del tipo di progetto scelto.";
                     ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                     validazioneOk = false;
                    }

                   }
                 
                    if (durata == 10) 
                   {
                  
                    if (numeromesi < 6 || numeromesi > durata)
                    {
                  

                     messaggioDiErrore = "Sezione Estero -  il numero dei mesi all'estero deve essere da un minimo di 6 ad un massimo della durata del tipo di progetto scelto.";
                     ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                     validazioneOk = false;
                    }

                   }


                    if (durata == 11  ||  durata== 12) 
                   {
                 
                    if (numeromesi < 7 || numeromesi > durata)
                    {
                     messaggioDiErrore = "Sezione Estero -  il numero dei mesi all'estero deve essere da un minimo di 7 ad un massimo della durata del tipo di progetto scelto.";
                     ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                     validazioneOk = false;
                    }

                   }

             }
             // validazioneOk = true
             if (validazioneOk == false) {
           
                 TornaAdInizioPagina();
             }
           return validazioneOk;
//             return true;//test
         };
              /* ]]> */

     </script>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server" >
    <fieldset id="GestioneProgetti"  class="ContornoPagina" runat="server" >
<legend >Progetto <asp:Label ID="tipoprog2020" runat="server" Text="Label"></asp:Label></legend>
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
  <div class="rowGroup" id="idlinkalti" runat="server" style="height:auto"> 
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
 
  <fieldset id="flsEnte" runat="server" class="ContornoPagina" >
 <legend id="CaratteristicheEnte"><img  src="images/minus_xsmall.png"  alt="Espandi/Riduci" style="cursor:pointer;" />&nbsp;&nbsp;<img id="TornaSuEnti"  style="cursor:pointer;" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />ENTE</legend>
  <div class="wrapper" id="CaratteristicheEnteContent2" style="width:100%;border:0px" >
    
    <div class="row">
                    <div class="collable" style="width:10%">
                        <asp:Label ID="lblcodiceente" AssociatedControlID="lblcodiceentePropo" Text="Codice Ente" runat="server" CssClass="label"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:40%">
                        <asp:Label id="lblcodiceentePropo"  CssClass="label"  runat="server"/>
                    </div>

                    <div class="collable" style="width:10%">
                        <asp:Label ID="lbldenominazione" AssociatedControlID="lblDenominazione" Text="Denominazione" runat="server" CssClass="label"></asp:Label>
                    </div>

                    <div class="colOggetti" style="width:40%">
                        <asp:Label id="lblDenominazioneEnte" CssClass="label"  runat="server"/>
                    </div>

                </div>
  </div>
 <div class="wrapper" id="CaratteristicheEnteContent" style="width:100%;border:0px" >
             
     <fieldset id="acco" >
     <legend >Enti di Accoglienza sul progetto</legend>
     <div style="width:100%;">
        <h3>
            <asp:label id="lblMessaggi1" runat="server" AssociatedControlID="dgRicercaEnteAccoglienza"></asp:label>
        </h3>
      
        <asp:datagrid id="dgRicercaEnteAccoglienza" runat="server" Width="100%"  
             ToolTip="Enti di Accoglienza su progetto" CellPadding="2" Font-Size="Small"  
             CssClass="table" PageSize="5" 
            AllowSorting="True" AutoGenerateColumns="False" Visible="false" UseAccessibleHeader="True">	
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
                <asp:BoundColumn Visible="False" DataField="idente" HeaderText="idente"></asp:BoundColumn>
				<asp:BoundColumn DataField="CODICE_ENTE" HeaderText="Cod.Ente">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="ENTE" HeaderText="Denominazione"></asp:BoundColumn>
                <asp:BoundColumn DataField="NSedi" HeaderText="n° Sedi"></asp:BoundColumn>
                <asp:BoundColumn DataField="NVolontari" HeaderText="n° Volontari"></asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
    </div>
     </fieldset>
     <br />
     <fieldset id="coop">
     <legend >Enti Coprogettanti</legend>
     <div class="rowGroup" id="divCoProgettazione" runat="server" visible="true">
       <div class="row">
       <div class="collable" style="width:45%">
        <asp:CheckBox id="chkCoProgettato" Runat="server" Enabled="false" Text="Progetto in coprogettazione"></asp:CheckBox>
       </div>
       <div class="colOggetti" style="width:15%">
         <asp:ImageButton id="imgCoProgettazione" ToolTip="Gestione Enti Coprogettanti" AlternateText="Gestione Enti Coprogettanti" ImageUrl="Images/Icona_Progetto_small.png" Runat="server"></asp:ImageButton>
       </div>       
   </div>
     </div>
     <div  style="width:100%;">
       <h3>
            <asp:label id="lblmessaggi2" runat="server" AssociatedControlID="dgRicercaEnteCooproggettante"></asp:label>
        </h3>
        <br />
        <asp:datagrid id="dgRicercaEnteCooproggettante" runat="server" Width="100%" Font-Size="Small" ToolTip="Elenco Enti Coprogettanti" CssClass="table" CellPadding="2"  AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"  Visible="False" UseAccessibleHeader="True" PageSize="5">	
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
           
				<asp:BoundColumn Visible="False" DataField="idente" HeaderText="idente"></asp:BoundColumn>
				<asp:BoundColumn DataField="CODICE_ENTE" HeaderText="Cod.Ente">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="ENTE" HeaderText="Denominazione"></asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
   </div>
     </fieldset>
     </div>
 </fieldset>
  <fieldset id="flsProgetto" class="ContornoPagina" >
  <legend id="Progetto"><img style="cursor:pointer;" src="images/minus_xsmall.png" alt="Espandi/Riduci" />&nbsp;&nbsp;<img style="cursor:pointer;" id="TornaSuProgetto" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Caratteristiche Progetto</legend> 
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
    <asp:Label Id="Label2" Text="Titolo Programma" AssociatedControlID="txtTitolo"  runat="server"  CssClass="label"></asp:Label></div>
    <div class="colOggetti" style="width:85%">
    <asp:TextBox ID="txtTitoloProgramma" runat="server" Text="non associato" Enabled="false" CssClass="textbox" ></asp:TextBox>
    </div>
   </div>
  <div class="row">
     <div class="collable" style="width:15%">
    <asp:Label Id="lblCodProgramma" Text="Codice Programma" AssociatedControlID="txtCodiceDelProgramma"  runat="server"  CssClass="label"></asp:Label></div>
    <div class="colOggetti" style="width:50%">
     <asp:TextBox ID="txtCodiceDelProgramma" runat="server" Text="" Enabled="false" CssClass="textbox" ></asp:TextBox>
    </div>
    <div  class="collable" style="width:30%">
   &nbsp;
    </div>
    <div class="colOggetti" style="width:5%">
     <asp:ImageButton ID="btnSeleziona" AlternateText="Selezione Programma" Visible="false"  runat="server" 
                                ImageUrl="~/Images/Icona_Progetto_small.png"  ToolTip="Selezione Programma" />
    </div>
    
    </div>
    
 </div>
    
    <div />
   <fieldset>
   <legend>Ente Proponente Programma</legend>
   <div class="wrapper" style="width:100%;border:0px">
    <div class="row" >
     <div class="colOggetti" style="width:30%">
     <asp:RadioButton id="chkEnteProponente"  runat="server" AutoPostBack="true" GroupName="enteprogramma"  Checked="False" Text="Ente Proponente Progetto" />
     </div>
     <div class="colOggetti" style="width:30%">
        &nbsp;
     </div>
     <div class="colOggetti" style="width:30%;text-align:right">
      <asp:RadioButton id="chkAltroEnte"  runat="server" AutoPostBack="true" GroupName="enteprogramma"  Checked="False" Text="Altro Ente (codice iscrizione)" />
      </div>
      <div class="colOggetti" style="width:10%">
       <asp:TextBox ID="txtcodNZ"  Visible="false"  runat="server" CssClass="textbox"  ></asp:TextBox>
      </div>
    </div>
    </div>
   </fieldset>

   <div class="row">
    <div class="collable" style="width:15%">
    <asp:Label Id="LblTitolo" Text="Titolo Progetto" AssociatedControlID="txtTitolo" runat="server"  CssClass="label"></asp:Label></div>
    <div class="colOggetti" style="width:85%">
    <asp:TextBox ID="txtTitolo" runat="server" CssClass="textbox" ></asp:TextBox>
    </div>
   </div>
   <div class="row">
      <div class="collable" style="width:15%">
        <asp:Label ID="lblddlSettore" AssociatedControlID="ddlSettore" Text="Settore" runat="server" CssClass="label" />
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
   
   <div class="row">
      <div class="collable" style="width:15%">
        
          &nbsp;</div>
   <div class="colOggetti" style="width:35%">
       
       &nbsp;</div>
        <div class="collable" style="width:15%">
        <asp:Label ID="lblAreaSecondaria" AssociatedControlID="ddlAreaSecondaria" Text="Area Secondaria" runat="server"   CssClass="label"/>
   </div>
    <div class="colOggetti" style="width:30%">
        <asp:dropdownlist id="ddlAreaSecondaria" runat="server" CssClass="ddlClass" Enabled="true"  AutoPostBack="True" ></asp:dropdownlist>
    </div>
    
   </div>  
   <div class="RigaVuota"  > &nbsp;
   </div>
  
    <div class="rowGroup" id="DivSelezionaAltriAmbiti"  visible="false" style="height:auto;" runat="server" >
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
   <div class="RigaVuota"  > &nbsp;
   </div>
    <div class="row" id="DivDurata" visible="true" style="height:auto" runat="server">
        <div class="collable" style="width:15%">
            <asp:Label Id="LblDurataProgetto" Text="Durata Progetto" AssociatedControlID="ddlDurata"  runat="server"  CssClass="label"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%">
            <asp:dropdownlist id="ddlDurata" runat="server" CssClass="ddlClass" AutoPostBack="True"></asp:dropdownlist>
        </div>
        <div class="collable" style="width:15%">
            <asp:Label Id="lblRequisitiVolonari" Text="Ulteriori requisiti richiesti ai candidati" AssociatedControlID="ddlRequisitiVolontari"  runat="server"  CssClass="label"></asp:Label>
        </div>
        <div class="colOggetti" style="width:30%">
            <asp:dropdownlist id="ddlRequisitiVolontari" runat="server" CssClass="ddlClass">
                <asp:ListItem Value="-1" Selected="True" Text="Selezionare"></asp:ListItem>
		        <asp:ListItem Value="0" Text="No"></asp:ListItem>
		        <asp:ListItem Value="1" Text="Si"></asp:ListItem>            
            </asp:dropdownlist>
        </div>
   </div>
   <br />
   <div class="wrapper" id="CaratteristicheOrganizzativeContent" style="width:100%;border:0px" >
 <div class="row">
    <div class="collable" style="width:15%">
    <asp:Label ID="lblPartner"  Text="Eventuali partner a sostegno del progetto" CssClass="label" runat="server"></asp:Label>
    </div>
    <div class="collable" style="width:35%">
     <asp:DropDownList ID="ddlPartner" runat="server" CssClass="ddlClass">
        <asp:ListItem Value="-1" Selected="True" Text="Selezionare"></asp:ListItem>
		<asp:ListItem Value="0" Text="No"></asp:ListItem>
		<asp:ListItem Value="1" Text="Si"></asp:ListItem>
    </asp:DropDownList>
    </div>
     <div class="collable" style="width:15%">
     </div>
      <div class="collable" style="width:35%">
      </div>
  </div>
  <%--<div class="row">
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
    </div>--%>
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
            </div>
            <div class="colOggetti" style="width:10%">
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
   <div id="DivMisureAggiuntive" runat="server" visible="true">
       <fieldset>
           <legend>Ulteriori Misure Eventualmente Adottate</legend>
           <div class="wrapper" style="width:100%;border:0px">
            <div class="row">
                <div class="collable" style="width:25%">
                    <asp:CheckBox id="chkGiovaniMinoriOp" Runat="server" Text="Giovani Minori Opportunità" AutoPostBack="true" ></asp:CheckBox>   
                </div>
                <div class="colOggetti" style="width:20%">
                    N. <asp:TextBox ID="txtNumeroGiovaniMinoriOpportunita" Enabled="false" Width="50%" runat="server" CssClass="textbox"  ></asp:TextBox>
                </div>
                <div class="collable" style="width:5%">
                    <asp:Label Id="lblGMOPerc" Text="%GMO" runat="server" Visible="true" AssociatedControlID="txtGMOPerc" CssClass="label"></asp:Label>
                </div>
                <div class="colOggetti" style="width:10%">
                   <asp:textbox id="txtGMOPerc" runat="server" CssClass="textbox" Enabled="false"    MaxLength="200" ></asp:textbox>
                </div>
            </div>
             <div class="wrapper" style="width:100%;border:0px">
            <div class="row">
                <div class="collable" style="width:25%">
                 <asp:Label Id="lblTipo" Text="Categoria di Minore Opportunità" AssociatedControlID="ddlCategoriaMO"  runat="server"  CssClass="label"></asp:Label>
                </div>
                 <div class="colOggetti" style="width:20%">
                  <asp:dropdownlist id="ddlCategoriaMO" runat="server" CssClass="ddlClass" AutoPostBack="True" Enabled ="false"></asp:dropdownlist>
                 </div>
                  <div class="collable" style="width:10%">
                   <asp:Label Id="lbltipodisabilità" Text="Tipo Disabilità" runat="server" AssociatedControlID="txttipodis" Visible="false"  CssClass="label"></asp:Label>
                </div>
                 <div class="colOggetti" style="width:40%">
                  <asp:textbox id="txttipodis" runat="server" CssClass="textbox"  Visible="false"  MaxLength="200" ></asp:textbox>
                 </div>
            </div>
            </div>
            <div class="row" id="divEsteroTutoraggio" visible = "false" runat="server">
                <div class="collable" style="width:15%">
                    <asp:RadioButton id="optEstero" runat="server" AutoPostBack="true" 	GroupName="OptEsteroTutoraggio" Checked="False" Text="Estero (UE)" />
                </div>
                <div class="collable" style="width:15%">
                    <asp:RadioButton id="optTutoraggio"  runat="server" AutoPostBack="true" GroupName="OptEsteroTutoraggio" Checked="False" Text="Tutoraggio" />
                </div>
                <div class="colHyperLink" style="width:25%">
                    <asp:LinkButton ID="lkbClearOptEUE_T" runat="server" style="cursor:pointer" Text="Pulisci"></asp:LinkButton>
                </div>            
            </div>
            <div id="DivEstero" visible="false" runat="server" >   
                <div class="row">
                    <div class="collable" style="width:25%">
                        <asp:Label ID="LblMesiPrevistiUE" AssociatedControlID="txtMesiPrevistiUE" Text="Mesi Previsti Estero (UE)" runat="server" CssClass="label"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:20%">
                        <asp:textbox id="txtMesiPrevistiUE"  MaxLength="5" CssClass="textbox" Width="50%" onchange="CheckNumeroValido('txtMesiPrevistiUE','Mesi Previsti Estero (UE)')" runat="server"/>
                    </div>

                    <div class="collable" style="width:25%">
                        <asp:Label ID="LblGiorniPrevistiUE" AssociatedControlID="txtGiorniPrevistiUE" Text="Giorni Previsti Estero (UE)" runat="server" CssClass="label"></asp:Label>
                    </div>

                    <div class="colOggetti" style="width:30%">
                        <asp:textbox id="txtGiorniPrevistiUE"  MaxLength="5" CssClass="textbox" Width="50%" onchange="CheckNumeroValido('txtGiorniPrevistiUE','Giorni Previsti Estero (UE)')" runat="server"/>
                    </div>

                </div>
                
                <div class="row">
                    <div class="collable" style="width:25%">
                        <asp:Label ID="lblTransfrontaliero" AssociatedControlID="ddlTransfrontaliero" Text="Transfrontaliero" runat="server" CssClass="label"></asp:Label>
                    </div>

                    <div class="colOggetti" style="width:20%">
                        <asp:dropdownlist id="ddlTransfrontaliero" width="80%" runat="server" CssClass="ddlClass" AutoPostBack="True">
                            <asp:ListItem Value="-1" Selected="True" Text="Selezionare"></asp:ListItem>
		                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
		                    <asp:ListItem Value="1" Text="Sì"></asp:ListItem>
                        </asp:dropdownlist>
                    </div>

                    <div class="collable" style="width:25%">
                        <asp:Label ID="lblModalità" AssociatedControlID="ddlModalità" Text="Modalità" runat="server" CssClass="label"></asp:Label>
                    </div>

                    <div class="colOggetti" style="width:30%">
                        <asp:dropdownlist id="ddlModalità" runat="server"  width="95%" Enabled="false" CssClass="ddlClass" AutoPostBack="True" >
                            <asp:ListItem Value="-1" Selected="True" Text="Selezionare"></asp:ListItem>
		                    <asp:ListItem Value="0" Text="Non Continuativo"></asp:ListItem>
		                    <asp:ListItem Value="1" Text="Continuativo"></asp:ListItem>
                        </asp:dropdownlist>
                    </div>
                </div>
            </div>
            <div class="row" id="DivTutoraggio" visible="false" runat="server">
            <div class="row" id="Div1"  runat="server">
                <div class="collable" style="width:30%">
                    <asp:Label ID="LblMesiPrevistiTutoraggio" AssociatedControlID="txtMesiPrevistiTutoraggio" Text="Mesi Previsti Tutoraggio" runat="server" CssClass="label"></asp:Label>
                </div>
                <div class="colOggetti" style="width:20%">
                    <asp:textbox id="txtMesiPrevistiTutoraggio"  MaxLength="5" CssClass="textbox" Width="50%" onchange="CheckNumeroValido('txtMesiPrevistiTutoraggio','Mesi Previsti Tutoraggio')" runat="server"/>
                </div>

                <div class="collable" style="width:30%">
                    <asp:Label ID="lblGiorniPrevistiTutoraggio" AssociatedControlID="txtGiorniPrevistiTutoraggio" Text="Giorni Previsti Tutoraggio" runat="server" CssClass="label"></asp:Label>
                </div>
                <div class="colOggetti" style="width:20%">
                    <asp:textbox id="txtGiorniPrevistiTutoraggio"  MaxLength="5" CssClass="textbox" Width="50%" onchange="CheckNumeroValido('txtGiorniPrevistiTutoraggio','Mesi Previsti Tutoraggio')" runat="server"/>
                </div>

            </div>
             <div class="row" id="Div2"  runat="server">
               <div class="collable" style="width:18%">
                   <asp:Label ID="lblorecollettive" runat="server" AssociatedControlID="txtorecollettive" Text="Numero ore collettive"></asp:Label>
               </div>
               <div class="colOggetti" style="width:15%">
                   <asp:TextBox ID="txtorecollettive" runat="server" onchange="CheckNumeroValido('txtorecollettive','Numero Ore Collettive')" onblur="Totale()" ></asp:TextBox>
               </div>
               <div class="collable" style="width:18%">
                   <asp:Label ID="lbloreindividuali" runat="server" AssociatedControlID="txtoreindividuali"  Text="Numero ore individuali" ></asp:Label>
               </div>
               <div class="colOggetti" style="width:15%">
                   <asp:TextBox ID="txtoreindividuali" runat="server"  onchange="CheckNumeroValido('txtoreindividuali','Numero Ore Collettive')" onblur="Totale()" ></asp:TextBox>
               </div>
               <div class="collable" style="width:18%">
                   <asp:Label ID="lbltotore" runat="server" AssociatedControlID="txttotaleore"  Text="Totale ore"></asp:Label>
               </div>
               <div class="colOggetti" style="width:15%">
                   <asp:TextBox ID="txttotaleore" runat="server" Enabled="false"></asp:TextBox>
               </div>

             </div>
            </div>
        </div>
       </fieldset>
   </div>
   <fieldset id="flsfinanziamento" runat="server">
   <legend>Tipo Finanziamento</legend>
   <div class="wrapper" style="width:100%;border:0px">
    
   <div class="row">
       <div class="collable" style="width:15%">
           <asp:Label ID="lblTipoFinanziamento" Text="Finanziamento" runat="server" CssClass="label"></asp:Label>
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
    </div>
 </fieldset>
 <%-- <fieldset id="flsCaratteristicheOrganizzative" class="ContornoPagina"  >
 <legend id="CaratteristicheOrganizzative"><img  src="images/minus_xsmall.png"  alt="Espandi/Riduci" style="cursor:pointer;" />&nbsp;&nbsp;<img id="TornaSuCaratteristicheOrganizzative"  style="cursor:pointer;" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Caratteristiche Organizzative</legend>
 
 </fieldset>--%>
  <fieldset id="flsFormazioneGenerale" class="ContornoPagina" >
 <legend id="FormazioneGenerale"><img style="cursor:pointer;" src="images/minus_xsmall.png" alt="Espandi/Riduci" />&nbsp;&nbsp;<img style="cursor:pointer;" id="TornaSuFormazioneGenerale" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Formazione Generale</legend>
 <div class="wrapper" id="FormazioneGeneraleContent" style="width:100%;border:0px">
 <div class="row">
       <div class="collable" style="width:15%">
    <asp:Label ID="lblDurata" AssociatedControlID="txtDurata" Text="Durata (Ore)" CssClass="label" runat="server"></asp:Label>
    </div>
     <div class="colOggetti" style="width:35%">
     <asp:TextBox ID="txtDurata" CssClass="textbox" AutoPostBack="true" onchange="CheckNumeroValido('txtDurata','Durata (Ore)')" Width="50%" runat="server" MaxLength="5"></asp:TextBox>
     </div>
       <div class="collable" style="width:15%">
    <asp:Label ID="labelModErogazione"  AssociatedControlID="ddlFormazioneGeneraleErogazione" Text="Modalit&#224; di erogazione" CssClass="label" runat="server"></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
        <asp:DropDownList runat="server" ID="ddlFormazioneGeneraleErogazione" AutoPostBack="True" >
             <asp:ListItem ID="selezionareFormazioneGenerale"  Value="selezionare" Text="Selezionare" Runat="server"  Selected="True" ></asp:ListItem>
             <asp:ListItem ID="chkUnica"  Value="unicaTranche" Text="Unica Tranche" Runat="server"  ></asp:ListItem>
             <asp:ListItem ID="chkPercentuale" Value="percentuale"  Text="80% - 20%" Runat="server"  ></asp:ListItem>
     </asp:DropDownList>
  
    </div>
</div>
 <%--<div class="row">
       <div class="collable" style="width:15%">
    <asp:Label ID="labelModalitaAttuazione" AssociatedControlID="txtModalitaAttuazione" Text="Codice Ente" CssClass="label" runat="server"></asp:Label>
    </div>
     <div class="colOggetti" style="width:35%">
     <asp:TextBox ID="txtModalitaAttuazione" CssClass="textbox" runat="server" Enabled="false"  MaxLength="10"></asp:TextBox>
     </div>
</div>
 --%>
 <div class="row">
 <div class="collable" style="width:15%">
 <asp:Label id="lbl180" AssociatedControlID="txt180" CssClass ="label" runat="server" Text="Entro la metà del progetto"></asp:Label>
 </div> 
 <div class="colOggetti" style="width:35%">
 	<asp:Label id="txt180" runat="server"  CssClass ="labelDati" Text="0" ></asp:Label>
</div>
<div class="collable" style="width:15%">
	<asp:Label id="lbl270" AssociatedControlID="txt270"  CssClass ="label" Text="Entro il penultimo mese" runat="server"></asp:Label>
</div>
 <div class="colOggetti" style="width:35%">
	<asp:Label id="txt270" runat="server"   CssClass="labelDati" Text="0" ></asp:Label>
</div>
 </div>
</div>
 </fieldset>
  <fieldset id="flsFormazioneSpecifica" class="ContornoPagina"  >
 <legend id="FormazioneSpecifica"><img style="cursor:pointer;" src="images/minus_xsmall.png" alt="Espandi/Riduci" />&nbsp;&nbsp;<img style="cursor:pointer;" id="TornaSuFormazioneSpecifica" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Formazione Specifica</legend>
 <div class="wrapper" id="FormazioneSpecificaContent" style="width:100%;border:0px">
 <div class="row">
       <div class="collable" style="width:15%">
    <asp:Label ID="lblDurataSpec" AssociatedControlID="txtDurataSpec" Text="Durata (Ore)" CssClass="label" runat="server"></asp:Label>
    </div>
     <div class="colOggetti" style="width:35%">
     <asp:TextBox ID="txtDurataSpec" onchange="CheckNumeroValido('txtDurataSpec','Durata (Ore)')" CssClass="textbox" Width="50%"  AutoPostBack="true" runat="server" MaxLength="5"></asp:TextBox>
     </div>
       <div class="collable" style="width:15%">
    <asp:Label ID="lblErogazione"  Text="Modalit&#224; di erogazione" AssociatedControlID="ddlFormazioneSpecificaErogazione" CssClass="label" runat="server"></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">

          <asp:DropDownList runat="server" ID="ddlFormazioneSpecificaErogazione" AutoPostBack="True" >
             <asp:ListItem ID="selezionareFormazioneSpecifica"  Value="selezionare" Text="Selezionare" Runat="server"  Selected="True" ></asp:ListItem>
             <asp:ListItem ID="ChkUnicaS"  Value="unicaTranche" Text="Unica Tranche" Runat="server"  ></asp:ListItem>
             <asp:ListItem ID="ChkPercentualeS" Value="percentuale"  Text="70% - 30%" Runat="server"  ></asp:ListItem>
     </asp:DropDownList>
    </div>
</div>
 <%--<div class="row">
       <div class="collable" style="width:15%">
    <asp:Label ID="labelmodattuazione" AssociatedControlID="txtmodattuazione" Text="Codice Ente" CssClass="label" runat="server"></asp:Label>
    </div>
     <div class="colOggetti" style="width:35%">
     <asp:TextBox ID="txtmodattuazione" CssClass="textbox" runat="server" Enabled="false" MaxLength="10"></asp:TextBox>
     </div>
</div>--%>
 
 <div class="row">
 <div class="collable" style="width:15%">
 <asp:Label id="lbl90S" AssociatedControlID="txt90S" CssClass ="label" runat="server" Text="Entro il 90&#186; giorno"></asp:Label>
 </div> 
 <div class="colOggetti" style="width:35%">
 	<asp:Label id="txt90S" runat="server" Text="0" CssClass ="labelDati" ></asp:Label>
</div>
<div class="collable" style="width:15%">
	<asp:Label id="lbl270S" AssociatedControlID="txt270S"  CssClass ="label" Text="Entro il terz'ultimo mese" runat="server"></asp:Label>
</div>
 <div class="colOggetti" style="width:35%">
	<asp:Label id="txt270S" runat="server" Text="0" CssClass="labelDati" ></asp:Label>
</div>
 </div>
</div>
 </fieldset>
  <fieldset id="flsEstero" class="ContornoPagina" >
    <legend id="Estero"><img id="ExpandEstero" src="images/minus_xsmall.png" style="cursor:pointer;" alt="Espandi/Riduci" />&nbsp;&nbsp; <img id="TornaSuEstero" onclick="TornaAdInizioPagina()" style="cursor:pointer;" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Estero</legend> 
<div class="wrapper"  id="EsteroContent" style="width:100%;border:0px">

<div class="row" style="height:auto">
<div class="collable" style="width:100%">
<asp:Label id="lblSezioneEsteroNonPresente" CssClass="msgInfo" runat="server" Visible="false" Text="Sezione non presente per i progetti in Italia."></asp:Label>
</div>
</div>
<div class="rowGroup" id="DivSezioneEstero" runat="server">
 
 <div class="row">
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
  <div class="collable" style="width:15%">
  <asp:Label ID="lblnumeromesiallestero" AssociatedControlID="txtNumeroMesiEstero" Text="Numero di mesi di permanenza all'estero" CssClass="label" runat="server"></asp:Label>
  </div>
    <div class="colOggetti" style="width:35%">
    <asp:TextBox ID="txtNumeroMesiEstero" CssClass="textbox" runat="server"  MaxLength="2"></asp:TextBox>
    </div>
  
 </div>
 </div>
 
 </div>
 </fieldset>
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
    <asp:Button id="ImgApplicaStatoValutazione" Text="Applica" CssClass="Pulsante" Runat="server" visible="false"></asp:Button>
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
<asp:HiddenField runat="server" id="TipoEnte" />
<asp:HiddenField runat="server" ID="hdnIsPostbackEnte" />
<asp:HiddenField runat="server" ID="hdnIsPostbackProgetto" />
<asp:HiddenField runat="server" id="txtIdAreaSecondaria" />
<asp:HiddenField id="strIdAmbitiSecondariModifica" runat="server" />
<asp:HiddenField id="txtCodAttivitaSec" runat="server" /> 
<asp:HiddenField id="idprogramma" runat="server" /> 
<asp:HiddenField runat="server" id="txtCodAttSec" />
</fieldset>
           </div>
      </div>
        </div>
</asp:Content>

