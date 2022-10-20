<%@ Page Title="Questionario" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmQuestionarioProgettoRev2.aspx.vb" Inherits="Futuro.WfrmQuestionarioProgettoRev2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
        <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css"/>
        <script src="Scripts/JHelper.js" type="text/javascript"></script>

    <script  type="text/javascript">
      /* <![CDATA[ */
        var pathPlus = "images/plus_xsmall.png";
        var pathMinus = "images/minus_xsmall.png";
        var titlePathMinus = "Riduci";
        var titlePathPlus = "Espandi";
        var inserimento = '<%=Request.Params("IdAttivita")%>';


        function CheckNumeroValido(valore, descrizione) {
            var clientIdErrore = CostruisciId("lblerrore")
            var numeroValido = ValidaNumero(CostruisciId(valore), descrizione, clientIdErrore);
            if (numeroValido == false) {
                TornaAdInizioPagina();
            }
        };
        /* ]]> */  
    </script>

    <script type="text/javascript">
       /* <![CDATA[ */
        $(function () {
            var tornaSu = 0;
            if (inserimento != "") {
                $('#AnagraficaContent').css('display', 'none');
                $('#Anagrafica').find("img").first().attr("src", pathPlus);
                $('#Anagrafica').find("img").first().attr("title", titlePathPlus);
            }
            else {
                $('#AnagraficaContent').css('display', 'yes');
                $('#Anagrafica').find("img").first().attr("src", pathMinus);
                $('#Anagrafica').find("img").first().attr("title", titlePathMinus);
            }
            $('#AnagraficaToggler').click(function () {
                var isVisible = $('#AnagraficaContent').is(':visible');
                if (!isVisible) {
                    $('#AnagraficaContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    $('#Anagrafica').find("img").first().attr("src", pathMinus);
                    $('#Anagrafica').find("img").first().attr("title", titlePathMinus);
                    $('#flsAnagrafica').focus();
                    setTimeout(function () { $('#Anagrafica').focus() }, 0);  
                }
                else {
                    $('#flsAnagrafica').focus();
                    setTimeout(function () { $('#Anagrafica').focus() }, 0); 
                }
                return false;
            });
            $('#TornaSuAnagrafica').click(function () {
                tornaSu = 1;
            });
            $('#Anagrafica').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#AnagraficaContent').is(':visible');
                    $('#AnagraficaContent').slideToggle('fast');
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
                $('#FormatoriContent').css('display', 'none');
                $('#Formatori').find("img").first().attr("src", pathPlus);
                $('#Formatori').find("img").first().attr("title", titlePathPlus);
            }
            else {
                $('#FormatoriContent').css('display', 'yes');
                $('#Formatori').find("img").first().attr("src", pathMinus);
                $('#Formatori').find("img").first().attr("title", titlePathMinus);
            }

            $('#FormatoriToggler').click(function () {
                var isVisible = $('#FormatoriContent').is(':visible');
                if (!isVisible) {
                    $('#FormatoriContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    $('#flsFormatori').focus();
                    $('#Formatori').find("img").first().attr("src", pathMinus);
                    $('#Formatori').find("img").first().attr("title", titlePathMinus);
                    $('#flsFormatori').focus();
                    setTimeout(function () { $('#Formatori').focus() }, 0); 
                }
                else {
                    $('#flsFormatori').focus();
                    setTimeout(function () { $('#Formatori').focus() }, 0); 
                }
                return false;
            });
            $('#TornaSuFormatori').click(function () {
                tornaSu = 1;
            });
            $('#Formatori').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#FormatoriContent').is(':visible');
                    $('#FormatoriContent').slideToggle('fast');
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
                $('#FormazioneGeneraleContent').css('display', 'none');
                $('#FormazioneGenerale').find("img").first().attr("src", pathPlus);
                $('#FormazioneGenerale').find("img").first().attr("title", titlePathPlus);
            }
            else {
                $('#FormazioneGeneraleContent').css('display', 'yes');
                $('#FormazioneGenerale').find("img").first().attr("src", pathMinus);
                $('#FormazioneGenerale').find("img").first().attr("title", titlePathMinus);
            }

            $('#FormazioneGeneraleToggler').click(function () {
                var isVisible = $('#FormazioneGeneraleContent').is(':visible');
                if (!isVisible) {
                    $('#FormazioneGeneraleContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    $('#FormazioneGenerale').find("img").first().attr("src", pathMinus);
                    $('#FormazioneGenerale').find("img").first().attr("title", titlePathMinus);
                    $('#flsFormazioneGenerale').focus();
                    setTimeout(function () { $('#FormazioneGenerale').focus() }, 0); 
                }
                else {
                    $('#flsFormazioneGenerale').focus();
                    setTimeout(function () { $('#FormazioneGenerale').focus() }, 0); 
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
            if (inserimento != "") {
                $('#LineaGuidaContent').css('display', 'none');
                $('#LineaGuida').find("img").first().attr("src", pathPlus);
                $('#LineaGuida').find("img").first().attr("title", titlePathPlus);
            }
            else {
                $('#LineaGuidaContent').css('display', 'yes');
                $('#LineaGuida').find("img").first().attr("src", pathMinus);
                $('#LineaGuida').find("img").first().attr("title", titlePathMinus);
            }

            $('#LineaGuidaToggler').click(function () {
                var isVisible = $('#LineaGuidaContent').is(':visible');
                if (!isVisible) {
                    $('#LineaGuidaContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    $('#LineaGuida').find("img").first().attr("src", pathMinus);
                    $('#LineaGuida').find("img").first().attr("title", titlePathMinus);
                    $('#flsLineaGuida').focus();
                    setTimeout(function () { $('#LineaGuida').focus() }, 0); 
                }
                else {
                    $('#flsLineaGuida').focus();
                    setTimeout(function () { $('#LineaGuida').focus() }, 0); 
                }
                return false;
            });
            $('#TornaSuLineaGuida').click(function () {
                tornaSu = 1;
            });
            $('#LineaGuida').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#LineaGuidaContent').is(':visible');
                    $('#LineaGuidaContent').slideToggle('fast');
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
             var validazioneOk = false;
             var IdMsgErrore = CostruisciId("lblerrore");
             var txtx = '<%= txtx.value %>';
             AggiornaErrore("", IdMsgErrore);
             if (document.getElementById(CostruisciId("txtNumFormatori")).value == '') {
                 messaggioDiErrore = "Area Formatori - Inserire il numero dei formatori.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
             if (document.getElementById(CostruisciId("txtNumVolontari")).value == '') {
                 messaggioDiErrore = "Area Formatori - Inserire il numero dei volontari.";
                 ConcatenaErrore(messaggioDiErrore, IdMsgErrore);
                 validazioneOk = false;
             }
             //             var strname = "Table5";
             //             var table = document.getElementById(strname);
             var cells = table.getElementsByTagName("span");
             var ctlr;
             var x;
             var blnRispostaMultipla;
             blnRispostaMultipla = false;
             x = 0

             for (var i = 0; i < cells.length; i++) {
                 ctrl = cells[i].firstChild;
                 //alert(ctrl.id)
                 if (ctrl.type == 'checkbox') {
                     //si tratta di una domanda a risposta multipla
                     //alert(isTipo(ctrl.id))
                     if (isTipo(ctrl.id) == false) {
                         //alert(x)
                         //devo controllare se ho già
                         //se è selezionato il check incremento il contatore
                         if (document.getElementById(CostruisciId(ctrl.id)).checked == true) {
                             //devo controllare se ho già checkato una risposta ad una domanda multipla
                             if (blnRispostaMultipla == false) {
                                 x = x + 1
                                 blnRispostaMultipla = true;
                             }
                         }
                     }
                     //domanda a risposta singola
                     else {
                         //ripristino a false la variabile che controlla le risposte multiple
                         blnRispostaMultipla = false;
                         //se è selezionato il check incremento il contatore
                         if (document.getElementById(CostruisciId(ctrl.id)).checked == true) {
                             x = x + 1
                         }
                     }
                 }
             }

             //alert(x)

             blnRispostaMultipla = false;

             //             var strname2 = "Table6";
             //             var table2 = document.getElementById(strname2);
             var cells2 = table2.getElementsByTagName("span");
             for (var i = 0; i < cells2.length; i++) {
                 ctrl = cells2[i].firstChild;
                 //alert(ctrl.id)
                 if (ctrl.type == 'checkbox') {
                     //si tratta di una domanda a risposta multipla
                     if (isTipo(CostruisciId(ctrl.id)) == false) {
                         //devo controllare se ho già
                         //se è selezionato il check incremento il contatore
                         if (document.getElementById(CostruisciId(ctrl.id)).checked == true) {
                             //devo controllare se ho già checkato una risposta ad una domanda multipla
                             if (blnRispostaMultipla == false) {
                                 x = x + 1
                                 blnRispostaMultipla = true;
                             }
                         }
                     }
                     //domanda a risposta singola
                     else {
                         //ripristino a false la variabile che controlla le risposte multiple
                         blnRispostaMultipla = false;
                         //se è selezionato il check incremento il contatore
                         if (document.getElementById(CostruisciId(ctrl.id)).checked == true) {
                             x = x + 1
                         }
                     }
                 }
             }
             blnRispostaMultipla = false;

             //             var strname3 = "Table7";
             //             var table3 = document.getElementById(strname3);
             var cells3 = table3.getElementsByTagName("span");
             for (var i = 0; i < cells3.length; i++) {
                 ctrl = cells3[i].firstChild;
                 //alert(ctrl.id)
                 if (ctrl.type == 'checkbox') {
                     //si tratta di una domanda a risposta multipla
                     if (isTipo(ctrl.id) == false) {
                         //devo controllare se ho già
                         //se è selezionato il check incremento il contatore
                         if (document.getElementById(CostruisciId(ctrl.id)).checked == true) {
                             //devo controllare se ho già checkato una risposta ad una domanda multipla
                             if (blnRispostaMultipla == false) {
                                 x = x + 1
                                 blnRispostaMultipla = true;
                             }
                         }
                     }
                     //domanda a risposta singola
                     else {
                         //ripristino a false la variabile che controlla le risposte multiple
                         blnRispostaMultipla = false;
                         //se è selezionato il check incremento il contatore
                         if (document.getElementById(CostruisciId(ctrl.id)).checked == true) {
                             x = x + 1
                         }
                     }
                 }
             }
             //alert(x)
             //alert(document.Form1.txtx.value)

             if (x < txtx) {
                 alert("E' necessario effettuare la valutazione di tutte le domande per effettuare la Conferma.")
                 return false;
             }

         };
            /* ]]> */
</script>
  <script type="text/javascript" >
  /* <![CDATA[ */
  
       function SoloNumeri() 
       {
           var evtobj = window.event;
           var keyascii = evtobj.keyCode;
           if (keyascii > 57 || keyascii <= 47) 
           {
               window.event.keyCode = 0;
           }
           else 
           {
               window.event.keyCode = keyascii;
           }
       }
   
	/* ]]> */
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server" >
    <fieldset id="GestioneProgetti"  class="ContornoPagina" runat="server">
<legend >Questionario Progetto</legend>
<div class="wrapper"  style="width:100%;border:0px">
  <div class="headers" >
    <h2>
    <asp:Label ID="Label5" runat="server"  Text="Gestione Questionario Progetto"></asp:Label>
    </h2>
</div>
<div class="RigaVuota">&nbsp;</div>
<div class="row" style="height:auto">
<asp:Label ID="lblMessaggio"  CssClass="msgConferma" runat="server"></asp:Label>
<asp:Label ID="lblErrore" CssClass="msgErrore" runat="server"></asp:Label>
<div class="RigaVuota">&nbsp;</div>
</div>

     <div id="divBando" class="rowGroup" style="height:auto"  runat="server" visible="false"> 
    <div class="row" >  
          <div class="collable" style="width:50%">
                <asp:Label ID="lblTitolo" class="labelDati" AssociatedControlID="lblProgetto" visible="false" runat="server" Text="Bando"></asp:Label>
            </div>
            <div class="colOggetti" style="width:50%">
                  <asp:Label ID="lblProgetto" class="label" visible="false" runat="server" ></asp:Label>
            </div>    
    </div>
 </div>
    <div class="row" >  
            <div class="collable" style="width:50%">
            <asp:Label ID="lblTitolareProgetto" class="labelDati" AssociatedControlID="lblEnte"  runat="server" Text="1.1 Denominazione ente titolare del progetto"></asp:Label>
        </div>
        <div class="colOggetti" style="width:50%">
                <asp:Label ID="lblEnte" class="label"  runat="server" ></asp:Label>
        </div>    
    </div>

    <div class="row" >  
        <div class="collable" style="width:50%">
            <asp:Label ID="labelCodice" class="labelDati" AssociatedControlID="lblCodice"  runat="server" Text="1.2 Codice Ente"></asp:Label>
        </div>
        <div class="colOggetti" style="width:50%">
            <asp:Label ID="lblCodice" class="label"  runat="server" ></asp:Label>
        </div>    
    </div>
    <div class="row" >  
        <div class="collable" style="width:50%">
            <asp:Label ID="labelCompetenza" class="labelDati" AssociatedControlID="lblEnteNazionale"  runat="server" Text="1.3 - 1.4 Competenza Ente"></asp:Label>
        </div>
        <div class="colOggetti" style="width:50%">
            <asp:Label ID="lblEnteNazionale" class="label"  runat="server" ></asp:Label>
        </div>    
    </div>
     <div class="row" >  
        <div class="collable" style="width:50%">
            <asp:Label ID="labelClasse" class="labelDati" AssociatedControlID="lblClasseEnte"  runat="server" Text="1.5 Classe/Sezione Ente"></asp:Label>
        </div>
        <div class="colOggetti" style="width:50%">
            <asp:Label ID="lblClasseEnte" class="label"  runat="server" ></asp:Label>
        </div>    
    </div>  


  <fieldset  class="ContornoPagina" >
  <legend>Sezioni</legend>
<div class="row" style="height:auto" >  
    <div id="AnagraficaToggler" class="colHyperLink" style="width:100%">
        <asp:LinkButton ID="link6" runat="server" style="cursor:pointer"  Text="Area Anagrafica" ></asp:LinkButton>
    </div>
     <div id="FormatoriToggler" class="colHyperLink" style="width:100%">
                <asp:LinkButton ID="link1" runat="server" style="cursor:pointer"   Text="Area Formatori" ></asp:LinkButton>
                </div>
     <div id="FormazioneGeneraleToggler" class="colHyperLink" style="width:100%">
                <asp:LinkButton ID="LinkButton2" runat="server" style="cursor:pointer" Text="Area Formazione Generale"></asp:LinkButton>
                </div>
    <div id="LineaGuidaToggler" class="colHyperLink" style="width:100%" >
            <asp:LinkButton  ID="LinkButton3" runat="server" style="cursor:pointer" Text="Area Linea Guida" ></asp:LinkButton>
    </div>
         
</div>   
</fieldset>
  
  <fieldset id="flsAnagrafica" class="ContornoPagina">
 <legend id="Anagrafica"><img  src="images/plus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor:pointer;" />&nbsp;&nbsp;<img id="TornaSuAnagrafica"  style="cursor:pointer;" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Area Anagrafica</legend>
 <div class="wrapper" id="AnagraficaContent" style="width:100%;border:0px">
        <asp:Panel ID="panelAnagrafica" runat="server"></asp:Panel>
        <div class="row">
        <div class="collable" style="width:100%" >
            <asp:Label ID="lblDenominazioneEnte" AssociatedControlID="txtDenominazioneEnte" Text="3.3 Denominazione e codice Ente (Da compilare solo se è stata selezionata la casella 3.2)" CssClass="labelDati" runat="server"></asp:Label>
        </div>
        </div>
             <div class="row">
         <div class="colOggetti" style="width:70%" >
         <asp:TextBox ID="txtDenominazioneEnte" runat="server" CssClass="textbox" Width="100%" ></asp:TextBox>
         </div>
        </div>
    </div>
 </fieldset>
 
  <fieldset id="flsFormatori" class="ContornoPagina" >
 <legend id="Formatori"><img  src="images/plus_xsmall.png" alt="Espandi/Riduci" style="cursor:pointer;"/>&nbsp;&nbsp;<img style="cursor:pointer;" id="TornaSuFormatori" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Area Formatori</legend>
 <div class="wrapper" id="FormatoriContent" style="width:100%;border:0px">
              <div class="row">
        <div class="collable" style="width:70%" >
            <asp:Label ID="lblNumFormatori" AssociatedControlID="txtNumFormatori" Text="2.1 Quanti formatori sono stati impiegati nella formazione generale dei volontari?" CssClass="labelDati" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:10%" >
         <asp:TextBox ID="txtNumFormatori" runat="server" CssClass="textbox"  
                MaxLength="4"  Text="0"></asp:TextBox>
         </div>
        </div>
        <div class="row">
            <div class="collable" style="width:30%" >
                <asp:Label ID="lblNumeroFormatori" Text="2.2 Numero Formatori" CssClass="labelDati" runat="server"></asp:Label>
            </div>
            <div class="colOggetti" style="width:15%" >
                <asp:Label ID="lblNumeroFormatoriM" AssociatedControlID="txtNumeroFormatoriM" Text="Uomini" CssClass="label" runat="server"></asp:Label>
            </div>
            <div class="colOggetti" style="width:10%" >
                <asp:TextBox ID="txtNumeroFormatoriM" runat="server" CssClass="textbox"  MaxLength="3"  Text="0"></asp:TextBox>
            </div>
             <div class="colOggetti" style="width:15%" >
                <asp:Label ID="lblNumeroFormatoriF" AssociatedControlID="txtNumeroFormatoriF" Text="Donne" CssClass="label" runat="server"></asp:Label>
            </div>
            <div class="colOggetti" style="width:10%" >
                <asp:TextBox ID="txtNumeroFormatoriF" runat="server" CssClass="textbox"  MaxLength="3"  Text="0"></asp:TextBox>
            </div>
        </div>
    <div class="row">
        <div class="collable" style="width:30%" >
            <asp:Label ID="lblTitoloDiStudio"  Text="2.3 Titolo Studio Formatori" CssClass="labelDati" runat="server"></asp:Label>
        </div>

           <div class="colOggetti" style="width:15%" >
            <asp:Label ID="Label3" AssociatedControlID="txtNumeroFormatoriDiplomati" Text="N&#176; Diplomati" CssClass="label" runat="server"  ></asp:Label>
             </div>
              <div class="colOggetti" style="width:10%" >
                <asp:TextBox ID="txtNumeroFormatoriDiplomati" runat="server" CssClass="textbox"  MaxLength="3" Text="0"></asp:TextBox>
                </div>
                <div class="colOggetti" style="width:15%" >
                    <asp:Label ID="Label2" AssociatedControlID="txtNumeroFormatoriLaureati" Text="N&#176; Laureati" CssClass="label" runat="server"></asp:Label>
                   </div>
           <div class="colOggetti" style="width:10%" >
         <asp:TextBox ID="txtNumeroFormatoriLaureati" runat="server" CssClass="textbox"  MaxLength="3" Text="0"></asp:TextBox>
         </div>
        </div>
    <div class="row">
        <div class="collable" style="width:70%" >
            <asp:Label ID="lblNumeroFormatoriCheHannoFrequentato"  AssociatedControlID="txtNumeroFormatoriCheHannoFrequentato" Text="2.4 N&#176; Formatori che hanno frequentato il corso formatori organizzato da UNSC o RPA" CssClass="labelDati" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:10%" >
            <asp:TextBox ID="txtNumeroFormatoriCheHannoFrequentato" runat="server" CssClass="textbox"  MaxLength="3" Text="0"></asp:TextBox>
        </div>

        </div>


      <asp:Panel ID="panelFormatori" runat="server"></asp:Panel>
 
 </div>
 </fieldset>

  <fieldset id="flsFormazioneGenerale" class="ContornoPagina" >
 <legend id="FormazioneGenerale"><img style="cursor:pointer;" src="images/plus_xsmall.png" alt="Espandi/Riduci" />&nbsp;&nbsp;<img style="cursor:pointer;" id="TornaSuFormazioneGenerale" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Area Formazione Generale</legend>
 <div class="wrapper" id="FormazioneGeneraleContent" style="width:100%;border:0px">
    
       <div class="row">
        <div class="collable" style="width:70%" >
            <asp:Label ID="lblNumeroVolontariDelVostroEnte"  AssociatedControlID="txtNumeroVolontariDelVostroEnte" Text="3.1 Quanti volontari hanno partecipato al/ai corso/i di formazione generale? (indicare solo i volontari del proprio Ente) ;" CssClass="labelDati" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:10%" >
            <asp:TextBox ID="txtNumeroVolontariDelVostroEnte" runat="server" CssClass="textbox"  MaxLength="4" Text="0"></asp:TextBox>
        </div>

   </div>
      <div class="row">
        <div class="collable" style="width:70%" >
            <asp:Label ID="lblNumeroVolontariAltriEnti"  AssociatedControlID="txtNumeroVolontariAltriEnti" Text="3.1.1 Se l’Ente ha erogato la formazione per altri Enti, indicare quanti volontari di altri Enti ha formato (Da compilare solo se l’Ente è di Prima classe)" CssClass="labelDati" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:10%" >
            <asp:TextBox ID="txtNumeroVolontariAltriEnti" runat="server" CssClass="textbox"  MaxLength="3" Text="0"></asp:TextBox>
        </div>

   </div>
   
      <div class="row">
        <div class="collable" style="width:100%" >
            <asp:Label ID="lblTempoPerFormazione"  Text="3.2 In merito ai tempi di erogazione della formazione generale indicare l’opzione scelta per i  singoli progetti:" CssClass="labelDati" runat="server"></asp:Label>
        </div>

   </div>
       <div class="row">
        <div class="collable" style="width:70%" >
            <asp:Label ID="lblFormazioneEntro180"  AssociatedControlID="txtFormazioneConclusaEntro180GG" Text="3.2.1 Formazione conclusa entro 180 giorni dall’avvio del progetto. Totale progetti" CssClass="labelDati" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:10%" >
            <asp:TextBox ID="txtFormazioneConclusaEntro180GG" runat="server" CssClass="textbox"   MaxLength="3" Text="0"></asp:TextBox>
        </div> 

   </div>
      <div class="row">
        <div class="collable" style="width:70%" >
            <asp:Label ID="lblFormazioneEntro270"  AssociatedControlID="txtFormazioneConclusaEntro270GG" Text="3.2.2 Formazione conclusa entro 270 giorni dall’avvio del progetto. Totale progetti" CssClass="labelDati" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:10%" >
            <asp:TextBox ID="txtFormazioneConclusaEntro270GG" onchange="CheckNumeroValido('txtFormazioneConclusaEntro270GG','Formazione conclusa entro 270 giorni dall’avvio del progetto')" runat="server" CssClass="textbox"  MaxLength="3" Text="0"></asp:TextBox>
        </div>

   </div>

   <div class="row">
        <div class="collable" style="width:100%" >
            <asp:Label ID="lblMetodologiaFormazioneGenerale"  Text="3.3 Nel/nei corso/i di formazione generale, quale metodologia didattica &#232; stata utilizzata? (Indicare la percentuale di utilizzo. Se la metodologia non &#232; stata utilizzata indicare 0)" CssClass="labelDati" runat="server"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="collable" style="width:20%">
            <asp:Label ID="lblLezioneFrontale" AssociatedControlID="txtLezioneFrontale" Text="Lezione frontale" runat="server" CssClass="label"></asp:Label>
        </div>
        <div class="collable" style="width:5%">
        <asp:TextBox ID="txtLezioneFrontale" CssClass="textbox" onkeypress="javascript: SoloNumeri();" MaxLength="3"  runat="server"  Text="0"></asp:TextBox>
        </div>
        <div class="collable" style="width:3%">
         <asp:Label ID="lblPerc"  runat="server" Text="%" CssClass="labelDati"></asp:Label>
        </div>
         </div>
         <div class="row">
               <div class="collable" style="width:20%">
            <asp:Label ID="lblDinamicheNonFormali" AssociatedControlID="txtDinamicheNonFormali" runat="server" CssClass="label" Text="Dinamiche non formali"></asp:Label>
        </div>
        <div class="collable" style="width:5%">
        <asp:TextBox ID="txtDinamicheNonFormali" CssClass="textbox" MaxLength="3"  onkeypress="javascript: SoloNumeri();" runat="server"  Text="0"></asp:TextBox>
            </div>
        <div class="collable" style="width:3%">
         <asp:Label ID="lblPerc2"  runat="server" Text="%" CssClass="labelDati"></asp:Label>
        </div>
         </div>
         <div class="row">
        <div class="collable" style="width:20%">
            <asp:Label ID="lbltxtFormazioneADistanza" AssociatedControlID="txtFormazioneADistanza" runat="server" CssClass="label" Text="Formazione a distanza"></asp:Label>
        </div>
        <div class="collable" style="width:5%">
        <asp:TextBox ID="txtFormazioneADistanza" CssClass="textbox" MaxLength="3"  onkeypress="javascript: SoloNumeri();" runat="server"  Text="0" ></asp:TextBox>
           </div>
        <div class="collable" style="width:3%">
         <asp:Label ID="lblPerc3"  runat="server" Text="%" CssClass="labelDati"></asp:Label>
        </div>
         </div>
         <div class="row">
           <div class="collable" style="width:20%">
            <asp:Label ID="labelTotale" AssociatedControlID="lblTotale"  runat="server" CssClass="labelDati" Text="Totale"></asp:Label>
        </div>
        <div class="collable" style="width:5%">
         <asp:Label ID="lblTotale" CssClass="labelDati" runat="server"  Text="100%"></asp:Label>
        </div>
    </div>
           <div class="row">
           <div class="collable" style="width:100%" >
            <asp:Label ID="lblEspertiFormADistanza"  Text="3.7 Durante il/i corso/i di formazione generale  sono stati utilizzati esperti (Se sono stati utilizzati indicare il numero)?" CssClass="labelDati" runat="server"></asp:Label>
        </div>
    </div>
        <div class="row">
        <div class="collable" style="width:70%">
            <asp:Label ID="lblPattoFormativo" AssociatedControlID="NumeroEspertiPattoFormativo" runat="server" CssClass="label" Text="<strong>a.</strong> l’identità del gruppo in formazione e patto formativo. Numero esperti"></asp:Label>
        </div>
        <div class="collable" style="width:10%">
        <asp:TextBox ID="NumeroEspertiPattoFormativo" CssClass="textbox" runat="server"  Text="0" ></asp:TextBox>
           </div>
      
         </div>
        <div class="row">
     <div class="collable" style="width:70%">
            <asp:Label ID="lblObiezioneCoscienza" AssociatedControlID="NumeroEspertiObiezioneCoscienza" runat="server" CssClass="label" Text="<strong>b.</strong> dall’obiezione di coscienza al servizio civile nazionale. Numero esperti"></asp:Label>
        </div>
        <div class="collable" style="width:10%">
        <asp:TextBox ID="NumeroEspertiObiezioneCoscienza" CssClass="textbox" runat="server"  Text="0" ></asp:TextBox>
           </div>
         </div>
         <div class="row">
     <div class="collable" style="width:70%">
            <asp:Label ID="Label1" AssociatedControlID="NumeroEspertiDifesaPatria" runat="server" CssClass="label" Text="<strong>c.</strong> il dovere di difesa della Patria - difesa civile non armata e nonviolenta. Numero esperti"></asp:Label>
        </div>
        <div class="collable" style="width:10%">
        <asp:TextBox ID="NumeroEspertiDifesaPatria" CssClass="textbox" runat="server"  Text="0" ></asp:TextBox>
           </div>
         </div>
         <div class="row">
        <div class="collable" style="width:70%">
            <asp:Label ID="lblEspertiCartaImpegnoEtico" AssociatedControlID="NumeroEspertiCartaImpegnoEtico" runat="server" CssClass="label" Text="<strong>d.</strong> la normativa vigente e la Carta di impegno etico. Numero esperti"></asp:Label>
        </div>
        <div class="collable" style="width:10%">
        <asp:TextBox ID="NumeroEspertiCartaImpegnoEtico" CssClass="textbox" runat="server"  Text="0" ></asp:TextBox>
           </div>
         </div>
         <div class="row">
        <div class="collable" style="width:70%">
            <asp:Label ID="lblEspertiFormazioneCivica" AssociatedControlID="NumeroEspertiFormazioneCivica" runat="server" CssClass="label" Text="<strong>e.</strong> la formazione civica. Numero esperti"></asp:Label>
        </div>
        <div class="collable" style="width:10%">
        <asp:TextBox ID="NumeroEspertiFormazioneCivica" CssClass="textbox" runat="server"  Text="0" ></asp:TextBox>
           </div>
         </div>
         <div class="row">
        <div class="collable" style="width:70%">
            <asp:Label ID="lblEspertiFormeCittadinanza" AssociatedControlID="NumeroEspertiFormeCittadinanza" runat="server" CssClass="label" Text="<strong>f.</strong> le forme di cittadinanza. Numero esperti"></asp:Label>
        </div>
        <div class="collable" style="width:10%">
        <asp:TextBox ID="NumeroEspertiFormeCittadinanza" CssClass="textbox" runat="server"  Text="0" ></asp:TextBox>
           </div>
         </div>
         <div class="row">
        <div class="collable" style="width:70%">
            <asp:Label ID="lblEspertiProtezioneCivile" AssociatedControlID="NumeroEspertiProtezioneCivile" runat="server" CssClass="label" Text="<strong>g.</strong> la protezione civile. Numero esperti"></asp:Label>
        </div>
        <div class="collable" style="width:10%">
        <asp:TextBox ID="NumeroEspertiProtezioneCivile" CssClass="textbox" runat="server"  Text="0" ></asp:TextBox>
           </div>
         </div>
         <div class="row">
        <div class="collable" style="width:70%">
            <asp:Label ID="lblEspertiVolNelServCiv" AssociatedControlID="NumeroEspertiVolNelServCiv" runat="server" CssClass="label" Text="<strong>h.</strong> la rappresentanza dei volontari nel servizio civile. Numero esperti"></asp:Label>
        </div>
        <div class="collable" style="width:10%">
        <asp:TextBox ID="NumeroEspertiVolNelServCiv" CssClass="textbox" runat="server"  Text="0" ></asp:TextBox>
           </div>
         </div>
         <div class="row">
        <div class="collable" style="width:70%">
            <asp:Label ID="lblEspertiPresEnte" AssociatedControlID="NumeroEspertiPresentazioneEnte" runat="server" CssClass="label" Text="<strong>i.</strong> presentazione dell’Ente. Numero esperti"></asp:Label>
        </div>
        <div class="collable" style="width:10%">
        <asp:TextBox ID="NumeroEspertiPresentazioneEnte" CssClass="textbox" runat="server"  Text="0" ></asp:TextBox>
           </div>
         </div>
         <div class="row">
        <div class="collable" style="width:70%">
            <asp:Label ID="lblEspertiLavoroPerProg" AssociatedControlID="NumeroEspertiLavoroPerProgetti" runat="server" CssClass="label" Text="<strong>j.</strong> il lavoro per progetti. Numero esperti"></asp:Label>
        </div>
        <div class="collable" style="width:10%">
        <asp:TextBox ID="NumeroEspertiLavoroPerProgetti" CssClass="textbox" runat="server"  Text="0" ></asp:TextBox>
           </div>
         </div>
         <div class="row">
        <div class="collable" style="width:70%">
            <asp:Label ID="lblEspertiOrgSCNEFigure12" AssociatedControlID="NumeroEspertiOrganizzazioneSCN" runat="server" CssClass="label" Text="<strong>k.</strong> l’organizzazione del servizio civile e le sue figure. Numero esperti"></asp:Label>
        </div>
        <div class="collable" style="width:10%">
        <asp:TextBox ID="NumeroEspertiOrganizzazioneSCN" CssClass="textbox" runat="server"  Text="0" ></asp:TextBox>
           </div>
         </div>
         <div class="row">
        <div class="collable" style="width:70%">
            <asp:Label ID="lblEspertiDiscEntiEVolontari" AssociatedControlID="NumeroEspertiRapportiEntiEVolontari" runat="server" CssClass="label" Text="<strong>l.</strong> disciplina dei rapporti tra enti e volontari del servizio civile nazionale. Numero esperti"></asp:Label>
        </div>
        <div class="collable" style="width:10%">
        <asp:TextBox ID="NumeroEspertiRapportiEntiEVolontari" CssClass="textbox" runat="server"  Text="0" ></asp:TextBox>
           </div>
         </div>
         <div class="row">
        <div class="collable" style="width:70%">
            <asp:Label ID="lblEspertiComunicazioneInterpersonale" AssociatedControlID="NumeroEspertiComunicazioneInterpersonale" runat="server" CssClass="label" Text="<strong>m.</strong> comunicazione interpersonale e gestione dei conflitti. Numero esperti"></asp:Label>
        </div>
        <div class="collable" style="width:10%">
        <asp:TextBox ID="NumeroEspertiComunicazioneInterpersonale" CssClass="textbox" runat="server"  Text="0" ></asp:TextBox>
           </div>
         </div>

    <div class="RigaVuota">&nbsp;</div>
     <asp:Panel ID="panelFormazioneGenerale"  runat="server"></asp:Panel>
  
</div>
 </fieldset>

  <fieldset id="flsLineaGuida" class="ContornoPagina" >
    <legend id="LineaGuida"><img id="ExpandLineaGuida" src="images/plus_xsmall.png" style="cursor:pointer;" alt="Espandi/Riduci" />&nbsp;&nbsp; <img id="TornaSuLineaGuida" onclick="TornaAdInizioPagina()" style="cursor:pointer;" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Area Linea Guida</legend> 
    <div class="wrapper"  id="LineaGuidaContent" style="width:100%;border:0px">
        <asp:Panel ID="panelLineaGuida" runat="server"></asp:Panel>
        
 </div>
 </fieldset>


 <div class="rowGroup" > 
<div class="RigaPulsanti" > 
	<asp:Button id="cmdConferma" CssClass="Pulsante" runat="server" Text="Salva"></asp:Button>
    <asp:Button id="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" ></asp:Button>
    <asp:Button id="cmdStampaVQ" CssClass="Pulsante" runat="server" Text="Stampa" ></asp:Button>
</div>
</div>
</div>
<asp:HiddenField id="txtnome" runat="server"/>
<asp:HiddenField id="txtidStorico" runat="server" />
<asp:HiddenField id="txtStatoProgetto" runat="server"/>
<asp:HiddenField id="txtPrsonalizza" runat="server"/>
<asp:HiddenField id="txtpagina" runat="server"/>
<asp:HiddenField id="txtBloccaMaschera" runat="server"/>
<asp:HiddenField id="txtx" Value="0" runat="server"/>
</fieldset>
 
</asp:Content>

