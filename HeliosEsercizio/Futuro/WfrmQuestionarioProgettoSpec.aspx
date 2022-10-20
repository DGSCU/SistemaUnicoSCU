<%@ Page Title="Questionario Progetto" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmQuestionarioProgettoSpec.aspx.vb" Inherits="Futuro.WfrmQuestionarioProgettoSpec" %>
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
                }
                else {
                    $('#flsAnagrafica').focus();
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
                }
                else {
                    $('#flsFormatori').focus();
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
                }
                else {
                    $('#flsLineaGuida').focus();
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
<asp:Label ID="lblmessaggio"  CssClass="msgConferma" runat="server"></asp:Label>
<asp:Label ID="lblerrore" CssClass="msgErrore" runat="server"></asp:Label>
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
<div class="row" style="height:auto">  
    <div id="AnagraficaToggler" class="colHyperLink" style="width:100%">
        <asp:LinkButton ID="link6" runat="server" style="cursor:pointer"  Text="Area Anagrafica" ></asp:LinkButton>
    </div>
     <div id="FormatoriToggler" class="colHyperLink" style="width:100%">
                <asp:LinkButton ID="link1" runat="server" style="cursor:pointer"   Text="Area Formatori" ></asp:LinkButton>
                </div>
     <div id="FormazioneGeneraleToggler" class="colHyperLink" style="width:100%">
                <asp:LinkButton ID="LinkButton2" runat="server" style="cursor:pointer" Text="Area Formazione Generale"></asp:LinkButton>
                </div>
    <div id="LineaGuidaToggler" class="colHyperLink" style="width:100%">
            <asp:LinkButton ID="LinkButton3" runat="server" style="cursor:pointer" Text="Area Linea Guida"></asp:LinkButton>
    </div>
         
</div>   
</fieldset>
  
  <fieldset id="flsAnagrafica" class="ContornoPagina">
 <legend id="Anagrafica"><img  src="images/plus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor:pointer;" />&nbsp;&nbsp;<img id="TornaSuAnagrafica"  style="cursor:pointer;" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Area Anagrafica</legend>
 <div class="wrapper" id="AnagraficaContent" style="width:100%;border:0px">
        <asp:Panel ID="panelAnagrafica" runat="server"></asp:Panel>
        <div class="row">
        <div class="collable" style="width:100%" >
            <asp:Label ID="lblDenominazioneEnte" AssociatedControlID="txtDenominazioneEnte" Text="3.4.1 Denominazione Ente" CssClass="labelDati" runat="server"></asp:Label>
        </div>
        </div>
             <div class="row">
         <div class="colOggetti" style="width:100%" >
         <asp:TextBox ID="txtDenominazioneEnte" runat="server" CssClass="textbox" Width="100%" ></asp:TextBox>
         </div>
        </div>
              <div class="row">
        <div class="collable" style="width:100%" >
            <asp:Label ID="lblMotivazioniCambiamentoEnte" AssociatedControlID="txtMotivazioniCambiamentoEnte" Text="4. Nel caso in cui, nell'ultimo biennio, sia stato cambiato l'ente che eroga la formazione indicare le motivazioni (max 1000 caratteri)" 
                            Font-Bold="true" CssClass="labelDati" runat="server"></asp:Label>
        </div>
         </div>
             <div class="row">
         <div class="colOggetti" style="width:100%" >    
         <asp:TextBox ID="txtMotivazioniCambiamentoEnte" runat="server" CssClass="textbox" Width="100%" TextMode="MultiLine" MaxLength="1000" Rows="4" ></asp:TextBox>
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
         <asp:TextBox ID="txtNumFormatori" runat="server" CssClass="textbox"  MaxLength="3"  Text="0"></asp:TextBox>
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
                <asp:TextBox ID="txtNumeroFormatoriM" runat="server" CssClass="textbox" MaxLength="3"  Text="0"></asp:TextBox>
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
        <div class="collable" style="width:30%" >
            <asp:Label ID="lblFormatoriVolontari"  Text="2.4 N&#176; Formatori Volontari" CssClass="labelDati" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:15%" >
            <asp:Label ID="lblNumeroFormatoriVolontariM" AssociatedControlID="txtNumeroFormatoriVolontariM" Text="Uomini" CssClass="label" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:10%" >
            <asp:TextBox ID="txtNumeroFormatoriVolontariM" runat="server" CssClass="textbox"  MaxLength="3" Text="0"></asp:TextBox>
        </div>
        <div class="colOggetti" style="width:15%" >
            <asp:Label ID="lblNumeroFormatoriVolontariF" AssociatedControlID="txtNumeroFormatoriVolontariF" Text="Donne" CssClass="label" runat="server"></asp:Label>
        </div>
           <div class="colOggetti" style="width:10%" >
         <asp:TextBox ID="txtNumeroFormatoriVolontariF" runat="server" CssClass="textbox"  MaxLength="3" Text="0"></asp:TextBox>
         </div>
        </div>
    <div class="row">
        <div class="collable" style="width:30%" >
            <asp:Label ID="lblNumeroFormatoriAContratto"  Text="2.5 N&#176; Formatori a Contratto" CssClass="labelDati" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:15%" >
            <asp:Label ID="lblNumeroFormatoriAContrattoM" AssociatedControlID="txtNumeroFormatoriAContrattoM" Text="Uomini" CssClass="label" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:10%" >
            <asp:TextBox ID="txtNumeroFormatoriAContrattoM" runat="server" CssClass="textbox"  MaxLength="3" Text="0"></asp:TextBox>
        </div>
        <div class="colOggetti" style="width:15%" >
            <asp:Label ID="lblNumeroFormatoriAContrattoF" AssociatedControlID="txtNumeroFormatoriAContrattoF" Text="Donne" CssClass="label" runat="server"></asp:Label>
        </div>
           <div class="colOggetti" style="width:10%" >
         <asp:TextBox ID="txtNumeroFormatoriAContrattoF" runat="server" CssClass="textbox"  MaxLength="3" Text="0"></asp:TextBox>
         </div>
        </div>
    <div class="row">
        <div class="collable" style="width:30%" >
            <asp:Label ID="lblNumeroFormatoriDipendenti"  Text="2.6 N&#176; Formatori dipendenti dell'ente" CssClass="labelDati" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:15%" >
            <asp:Label ID="lblNumeroFormatoriDipendentiM" AssociatedControlID="txtNumeroFormatoriDipendentiM" Text="Uomini" CssClass="label" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:10%" >
            <asp:TextBox ID="txtNumeroFormatoriDipendentiM" runat="server" CssClass="textbox"  MaxLength="3" Text="0"></asp:TextBox>
        </div>
        <div class="colOggetti" style="width:15%" >
            <asp:Label ID="lblNumeroFormatoriDipendentiF" AssociatedControlID="txtNumeroFormatoriDipendentiF" Text="Donne" CssClass="label" runat="server"></asp:Label>
        </div>
           <div class="colOggetti" style="width:10%" >
         <asp:TextBox ID="txtNumeroFormatoriDipendentiF" runat="server" CssClass="textbox"  MaxLength="3" Text="0"></asp:TextBox>
         </div>
        </div>
    <div class="row">
        <div class="collable" style="width:70%" >
            <asp:Label ID="lblNumeroFormatoriCheHannoFrequentato"  AssociatedControlID="txtNumeroFormatoriCheHannoFrequentato" Text="2.7 N&#176; Formatori che hanno frequentato il corso formatori organizzato da UNSC o Regione" CssClass="labelDati" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:10%" >
            <asp:TextBox ID="txtNumeroFormatoriCheHannoFrequentato" runat="server" CssClass="textbox"  MaxLength="3" Text="0"></asp:TextBox>
        </div>

        </div>
    <div class="row">
        <div class="collable" style="width:100%" >
            <asp:Label ID="Label4" AssociatedControlID="txtPuntiDiForza" Text="2.8 Quali sono i punti di riscontrati dai formatori nelle linee guida della formazione generale? (max 1000 caratteri)" 
                Font-Bold="true" CssClass="labelDati" runat="server"></asp:Label>
        </div>
    </div>
    <div class="row">
         <div class="colOggetti" style="width:100%" >    
            <asp:TextBox ID="txtPuntiDiForza" runat="server" CssClass="textbox" TextMode="MultiLine" MaxLength="1000" Rows="4" Width="100%"  ></asp:TextBox>
         </div>
    </div>
    <div class="RigaVuota">&nbsp;</div>
    <div class="row">
        <div class="collable" style="width:100%" >
            <asp:Label ID="lblCriticita" AssociatedControlID="txtCriticita" Text="2.9 Quali sono le criticita' riscontrate dai formatori nelle linee guida della formazione generale? (max 1000 caratteri)" 
                Font-Bold="true" CssClass="labelDati" runat="server"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="colOggetti" style="width:100%" >    
            <asp:TextBox ID="txtCriticita" runat="server" CssClass="textbox" TextMode="MultiLine" MaxLength="1000" Rows="4" Width="100%"  ></asp:TextBox>
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
            <asp:Label ID="lblNumVolontari"  AssociatedControlID="txtNumVolontari" Text="3.1 Quanti volontari hanno partecipato al/ai corso/i di formazione generale? Tot. N&#176;" CssClass="labelDati" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:10%" >
            <asp:TextBox ID="txtNumVolontari" runat="server" CssClass="textbox"  MaxLength="3" Text="0"></asp:TextBox>
        </div>
   </div>
       <div class="row">
        <div class="collable" style="width:70%" >
            <asp:Label ID="lblNumeroVolontariDelVostroEnte"  AssociatedControlID="txtNumeroVolontariDelVostroEnte" Text="3.1.1 Volontari del vostro ente N&#176;" CssClass="labelDati" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:10%" >
            <asp:TextBox ID="txtNumeroVolontariDelVostroEnte" runat="server" CssClass="textbox"  MaxLength="3" Text="0"></asp:TextBox>
        </div>

   </div>
      <div class="row">
        <div class="collable" style="width:70%" >
            <asp:Label ID="lblNumeroVolontariAltriEnti"  AssociatedControlID="txtNumeroVolontariAltriEnti" Text="3.1.1 Volontari di altri enti N&#176;" CssClass="labelDati" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:10%" >
            <asp:TextBox ID="txtNumeroVolontariAltriEnti" runat="server" CssClass="textbox"  MaxLength="3" Text="0"></asp:TextBox>
        </div>

   </div>
   
   <div class="row">
        <div class="collable" style="width:100%" >
            <asp:Label ID="lblMetodologiaFormazioneGenerale"  Text="3.2 Nel/nei corso/i di formazione generale, quale metodologia didattica &#232; stata utilizzata? (Indicare la percentuale di utilizzo. Se la metodologia non &#232; stata utilizzata indicare 0)" CssClass="labelDati" runat="server"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="collable" style="width:20%">
            <asp:Label ID="lblLezioneFrontale" AssociatedControlID="txtLezioneFrontale" Text="Lezione frontale" runat="server" CssClass="label"></asp:Label>
        </div>
        <div class="collable" style="width:5%">
        <asp:TextBox ID="txtLezioneFrontale" CssClass="textbox" runat="server"  Text="0"></asp:TextBox>
        </div>
        <div class="collable" style="width:3%">
         <asp:Label id="lblPerc" runat="server" Text="%" CssClass="labelDati"></asp:Label>
        </div>
         </div>
         <div class="row">
               <div class="collable" style="width:20%">
            <asp:Label ID="lblDinamicheNonFormali" AssociatedControlID="txtDinamicheNonFormali" runat="server" CssClass="label" Text="Dinamiche non formali"></asp:Label>
        </div>
        <div class="collable" style="width:5%">
        <asp:TextBox ID="txtDinamicheNonFormali" CssClass="textbox" runat="server"  Text="0"></asp:TextBox>
            </div>
        <div class="collable" style="width:3%">
         <asp:Label ID="lblPerc1"  runat="server" Text="%" CssClass="labelDati"></asp:Label>
        </div>
         </div>
         <div class="row">
        <div class="collable" style="width:20%">
            <asp:Label ID="lbltxtFormazioneADistanza" AssociatedControlID="txtFormazioneADistanza" runat="server" CssClass="label" Text="Formazione a distanza"></asp:Label>
        </div>
        <div class="collable" style="width:5%">
        <asp:TextBox ID="txtFormazioneADistanza" CssClass="textbox" runat="server"  Text="0" ></asp:TextBox>
           </div>
        <div class="collable" style="width:3%">
         <asp:Label  ID="lblPerc2" runat="server" Text="%" CssClass="labelDati"></asp:Label>
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
            <asp:Label ID="lblSeSiQuali" AssociatedControlID="txtSeSiQuali" Text="3.16 Se si, quali? (max 1000 caratteri)" 
                Font-Bold="true" CssClass="labelDati" runat="server"></asp:Label>
        </div>
    </div>
    <div class="row">
         <div class="colOggetti" style="width:100%" >    
            <asp:TextBox ID="txtSeSiQuali" runat="server" CssClass="textbox" TextMode="MultiLine" MaxLength="1000" Rows="4" Width="100%"  ></asp:TextBox>
         </div>
    </div>
    <div class="RigaVuota">&nbsp;</div>
     <asp:Panel ID="panelFormazioneGenerale" runat="server"></asp:Panel>
</div>
 </fieldset>

  <fieldset id="flsLineaGuida" class="ContornoPagina" >
    <legend id="LineaGuida"><img id="ExpandLineaGuida" src="images/plus_xsmall.png" style="cursor:pointer;" alt="Espandi/Riduci" />&nbsp;&nbsp; <img id="TornaSuLineaGuida" onclick="TornaAdInizioPagina()" style="cursor:pointer;" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Area Linea Guida</legend> 
    <div class="wrapper"  id="LineaGuidaContent" style="width:100%;border:0px">
        <asp:Panel ID="panelLineaGuida" runat="server"></asp:Panel>
        
        <div class="row">
            <div class="collable" style="width:100%" >
                <asp:Label ID="lblDifficoltaAdeguamento" AssociatedControlID="txtDifficoltaAdeguamento" Text="4.5 Perch&#233; &#232; stato difficile per il suo ente, adeguarsi all'area selezionata nella domanda 4.2? (max 1000 caratteri)" 
                Font-Bold="true" CssClass="labelDati" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="colOggetti" style="width:100%" >    
                <asp:TextBox ID="txtDifficoltaAdeguamento" runat="server" CssClass="textbox" TextMode="MultiLine" MaxLength="1000" Rows="4" Width="100%"  ></asp:TextBox>
            </div>
        </div>
                 <div class="RigaVuota">&nbsp;</div>
        <div class="row">
            <div class="collable" style="width:100%" >
                <asp:Label ID="lblMotiviModificaAree" AssociatedControlID="txtMotiviModificaAree" Text="4.6 Perch&#233; dovrebbero essere modificate le aree selezionate nella domanda 4.3? (max 1000 caratteri)" 
                Font-Bold="true" CssClass="labelDati" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="colOggetti" style="width:100%" >    
                <asp:TextBox ID="txtMotiviModificaAree" runat="server" CssClass="textbox" TextMode="MultiLine" MaxLength="1000" Rows="4" Width="100%"  ></asp:TextBox>
            </div>
        </div>
                 <div class="RigaVuota">&nbsp;</div>
        <div class="row">
            <div class="collable" style="width:100%" >
                <asp:Label ID="lblMotiviRafforzamentoAree" AssociatedControlID="txtMotiviRafforzamentoAree" Text="4.7 Perch&#233; dovrebbero essere rafforzate le aree selezionate nella domanda 4.4? (max 1000 caratteri)" 
                Font-Bold="true" CssClass="labelDati" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="colOggetti" style="width:100%" >    
                <asp:TextBox ID="txtMotiviRafforzamentoAree" runat="server" CssClass="textbox" TextMode="MultiLine" MaxLength="1000" Rows="4" Width="100%" ></asp:TextBox>
            </div>
        </div>
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

