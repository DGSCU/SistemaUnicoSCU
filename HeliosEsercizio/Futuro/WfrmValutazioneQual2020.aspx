<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmValutazioneQual2020.aspx.vb" Inherits="Futuro.WfrmValutazioneQual2020" %>
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
            <script type="text/javascript">
                function SetRadio(radioId) {
                    var idRadioButton = '#' + CostruisciId(radioId)
                    if ($("" + idRadioButton + "").prop("checked") == true) {
                        $("" + idRadioButton + "").prop("checked", false);
                    } else {
                        $("" + idRadioButton + "").prop("checked", true);
                    }
                };
        </script>
    <script type="text/javascript">
        function AlertSalvataggioDati() {
            var pulsanteSalvaVisibile = '<%= cmdSalva.Visible  %>';
            if (pulsanteSalvaVisibile == 'True') {

                return confirm('Prima di essere reindirizzati alla pagina richiesta tutti i dati verranno salvati. CONTINUARE?');
            }
            return true;
        };
    </script>
         <script type="text/javascript" >
           /* <![CDATA[ */
            $(function () {
             var pulsanteAbilitato = '<%= txtDataSeduta.Enabled  %>';
               if (pulsanteAbilitato == 'True') {
                var clientId = "#" + CostruisciId('txtDataSeduta');
                    $("" + clientId + "").datepicker();
                }
            });

                 /* ]]> */
                 </script>
    <script  type="text/javascript">
      /* <![CDATA[ */
        var pathPlus = "images/plus_xsmall.png";
        var pathMinus = "images/minus_xsmall.png";
        var titlePathMinus = "Riduci";
        var titlePathPlus = "Espandi";
        var inserimento = '<%=Request.Params("idprogetto")%>';
        //var VengoDa = '<%=Request.Params("VengoDa")%>'; //Request.QueryString("") = "AssociaVolontari"
        var isPostbackEnte  = '<%= hdnIsPostbackCP.value %>' ;
        var isPostbackCaratteristicheProgetto = '<%= hdnIsPostbackCO.value %>' ;
        var isPostbackCaratteristicheCompetenze = '<%= hdnIsPostbackCCA.value %>' ;
        var isPostbackFormazioneSpecifica = '<%= hdnIsPostbackCG.value %>' ;
        var isPostbackNoteRiservate = '<%= hdnIsPostbackNOTE.value %>' ;
        var isPostbackUlterioriMisure = '<%= hdnIsPostbackUlterioriMisure.value %>' ;
        var isPostbackCoerenza = '<%=  hdnIsPostbackCoerenza.value %>' ;
        var isPostbackTutoraggio = '<%=  hdnIsPostbackTutoraggio.value %>' ;
        function CheckNumeroValido(valore, descrizione) {
            var clientIdErrore = CostruisciId("msgErrore");
            if(document.getElementById(CostruisciId(valore)).value == ""){
            document.getElementById(CostruisciId(valore)).value = 0;
            };
            var numeroValido = ValidaNumero(CostruisciId(valore), descrizione, clientIdErrore);
            if (numeroValido == false) {
                TornaAdInizioPagina();
            }
        };
        /* ]]> */  
    </script>
<script type="text/javascript">
    $(function () {
        // alert("NoteRiservate");

        var tornaSu = 0;


        if (isPostbackNoteRiservate != "") {
            $('#NoteRiservateContent').css('display', 'none');
            $('#NoteRiservate').find("img").first().attr("src", pathPlus);
            $('#NoteRiservate').find("img").first().attr("title", titlePathPlus);
        }



        $('#NoteRiservateToggler').click(function () {
            var isVisible = $('#NoteRiservateContent').is(':visible');
            if (!isVisible) {
                $('#NoteRiservateContent').slideToggle('fast');
                $(this).toggleClass('slideSign');
                $('#NoteRiservate').find("img").first().attr("src", pathMinus);
                $('#NoteRiservate').find("img").first().attr("title", titlePathMinus);
                $('#flsNoteRiservate').focus();
                setTimeout(function () { $('#NoteRiservate').focus() }, 0);
            }
            else {
                $('#flsNoteRiservate').focus();
                setTimeout(function () { $('#NoteRiservate').focus() }, 0);
            }
            return false;
        });
        $('#TornaSuNoteRiservate').click(function () {
            tornaSu = 1;
        });
        $('#NoteRiservate').click(function () {
            if (tornaSu == 0) {
                var isVisible = $('#NoteRiservateContent').is(':visible');
                $('#NoteRiservateContent').slideToggle('fast');
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

         </script>
    <script type="text/javascript">
  
       /* <![CDATA[ */
        $(function () {
            var tornaSu = 0;

             if( isPostbackEnte != "")
             {  
                $('#EnteContent').css('display', 'none');
                $('#Ente').find("img").first().attr("src", pathPlus);
                $('#Ente').find("img").first().attr("title", titlePathPlus);
              }
          
                $('#EnteToggler').click(function () {
       
                    var isVisible = $('#EnteContent').is(':visible');
                    if (!isVisible) {
                        $('#EnteContent').slideToggle('fast');
                        $(this).toggleClass('slideSign');
                        $('#Ente').find("img").first().attr("src", pathMinus);
                        $('#Ente').find("img").first().attr("title", titlePathMinus);
                        $('#flsEnte').focus();
                        setTimeout(function () { $('#Ente').focus() }, 0);  
                    
                     
                    }
                    else {
                        $('#flsEnte').focus();
                        setTimeout(function () { $('#Ente').focus() }, 0); 
                    
                    }
                    return false;
                });
                $('#TornaSuEnte').click(function () {
                    tornaSu = 1;
                });
                $('#Ente').click(function () {
 
                    if (tornaSu == 0) {
                  
                        var isVisible = $('#EnteContent').is(':visible');
                        $('#EnteContent').slideToggle('fast');
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
          
            if( isPostbackCaratteristicheProgetto != "")
            { 
                $('#CaratteristicheProgettoContent').css('display', 'none');
                $('#CaratteristicheProgetto').find("img").first().attr("src", pathPlus);
                $('#CaratteristicheProgetto').find("img").first().attr("title", titlePathPlus); 
            }

            $('#CaratteristicheProgettoToggler').click(function () {
                var isVisible = $('#CaratteristicheProgettoContent').is(':visible');
                //alert(isVisible)
                if (!isVisible) {
                //alert("NelIF")
                    $('#CaratteristicheProgettoContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    $('#flsCaratteristicheProgetto').focus();
                    $('#CaratteristicheProgetto').find("img").first().attr("src", pathMinus);
                    $('#CaratteristicheProgetto').find("img").first().attr("title", titlePathMinus);
                    $('#flsCaratteristicheProgetto').focus();
                    setTimeout(function () { $('#CaratteristicheProgetto').focus() }, 0); 
                }
                else {
                //alert("NelIF")
                    $('#flsCaratteristicheProgetto').focus();
                    setTimeout(function () { $('#CaratteristicheProgetto').focus() }, 0); 
                }
                 //alert("Booooooo")
                return false;
            });
            $('#TornaSuCaratteristicheProgetto').click(function () {
                tornaSu = 1;
            });
            $('#CaratteristicheProgetto').click(function () {
             //alert("A")
                if (tornaSu == 0) {
                    var isVisible = $('#CaratteristicheProgettoContent').is(':visible');
                    $('#CaratteristicheProgettoContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    if (!isVisible) {
                     //alert("B")
                        $(this).find("img").first().attr("src", pathMinus);
                        $(this).find("img").first().attr("title", titlePathMinus);
                    }
                    else {
                    //alert("C")
                        $(this).find("img").first().attr("src", pathPlus);
                        $(this).find("img").first().attr("title", titlePathPlus);
                    }
                }
                tornaSu = 0;
                //alert("exit 1")
                return false;
            });
             //alert("exit 2")
        });
         </script>
    <script type="text/javascript">
       /* <![CDATA[ */
        $(function () {
            var tornaSu = 0;

            if( isPostbackCaratteristicheCompetenze != "")
            { 
                $('#CaratteristicheCompetenzeContent').css('display', 'none');
                $('#CaratteristicheCompetenze').find("img").first().attr("src", pathPlus);
                $('#CaratteristicheCompetenze').find("img").first().attr("title", titlePathPlus); 
           }

            $('#CaratteristicheCompetenzeToggler').click(function () {
                var isVisible = $('#CaratteristicheCompetenzeContent').is(':visible');
                if (!isVisible) {
                    $('#CaratteristicheCompetenzeContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    $('#CaratteristicheCompetenze').find("img").first().attr("src", pathMinus);
                    $('#CaratteristicheCompetenze').find("img").first().attr("title", titlePathMinus);
                    $('#flsCaratteristicheCompetenze').focus();
                    setTimeout(function () { $('#CaratteristicheCompetenze').focus() }, 0); 
                }
                else {
                    $('#flsCaratteristicheCompetenze').focus();
                    setTimeout(function () { $('#CaratteristicheCompetenze').focus() }, 0); 
                }
                return false;
            });
            $('#TornaSuCaratteristicheCompetenze').click(function () {
                tornaSu = 1;
            });
            $('#CaratteristicheCompetenze').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#CaratteristicheCompetenzeContent').is(':visible');
                    $('#CaratteristicheCompetenzeContent').slideToggle('fast');
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
         </script>
    <script type="text/javascript">
       /* <![CDATA[ */
        $(function () {
            var tornaSu = 0;
        
            if( isPostbackFormazioneSpecifica != "")
            { 
                    $('#FormazioneSpecificaContent').css('display', 'none');
                    $('#FormazioneSpecifica').find("img").first().attr("src", pathPlus);
                    $('#FormazioneSpecifica').find("img").first().attr("title", titlePathPlus); 
            }

            $('#FormazioneSpecificaToggler').click(function () {
                var isVisible = $('#FormazioneSpecificaContent').is(':visible');
                if (!isVisible) {
                    $('#FormazioneSpecificaContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    $('#FormazioneSpecifica').find("img").first().attr("src", pathMinus);
                    $('#FormazioneSpecifica').find("img").first().attr("title", titlePathMinus);
                    $('#flsFormazioneSpecifica').focus();
                    setTimeout(function () { $('#FormazioneSpecifica').focus() }, 0);
                }
                else {
                    $('#flsFormazioneSpecifica').focus();
                    setTimeout(function () { $('#FormazioneSpecifica').focus() }, 0);
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
        /* ]]> */
    </script>
    <script type="text/javascript">
       /* <![CDATA[ */
        $(function () {
            var tornaSu = 0;
        
            if( isPostbackUlterioriMisure != "")
            { 
                    $('#UlterioriMisureContent').css('display', 'none');
                    $('#UlterioriMisure').find("img").first().attr("src", pathPlus);
                    $('#UlterioriMisure').find("img").first().attr("title", titlePathPlus); 
            }

            $('#UlterioriMisureToggler').click(function () {
                var isVisible = $('#UlterioriMisureContent').is(':visible');
                if (!isVisible) {
                    $('#UlterioriMisureContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    $('#UlterioriMisure').find("img").first().attr("src", pathMinus);
                    $('#UlterioriMisure').find("img").first().attr("title", titlePathMinus);
                    $('#flsUlterioriMisure').focus();
                    setTimeout(function () { $('#UlterioriMisure').focus() }, 0);
                }
                else {
                    $('#flsUlterioriMisure').focus();
                    setTimeout(function () { $('#UlterioriMisure').focus() }, 0);
                }
                return false;
            });
            $('#TornaSuUlterioriMisure').click(function () {
                tornaSu = 1;
            });
            $('#UlterioriMisure').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#UlterioriMisureContent').is(':visible');
                    $('#UlterioriMisureContent').slideToggle('fast');
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
        
            if( isPostbackCoerenza != "")
            { 
                    $('#CoerenzaContent').css('display', 'none');
                    $('#Coerenza').find("img").first().attr("src", pathPlus);
                    $('#Coerenza').find("img").first().attr("title", titlePathPlus); 
            }

            $('#CoerenzaToggler').click(function () {
                var isVisible = $('#CoerenzaContent').is(':visible');
                if (!isVisible) {
                    $('#CoerenzaContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    $('#Coerenza').find("img").first().attr("src", pathMinus);
                    $('#Coerenza').find("img").first().attr("title", titlePathMinus);
                    $('#flsCoerenza').focus();
                    setTimeout(function () { $('#Coerenza').focus() }, 0);
                }
                else {
                    $('#flsCoerenza').focus();
                    setTimeout(function () { $('#Coerenza').focus() }, 0);
                }
                return false;
            });
            $('#TornaSuCoerenza').click(function () {
                tornaSu = 1;
            });
            $('#Coerenza').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#CoerenzaContent').is(':visible');
                    $('#CoerenzaContent').slideToggle('fast');
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
        
            if( isPostbackTutoraggio != "")
            { 
                    $('#TutoraggioContent').css('display', 'none');
                    $('#Tutoraggio').find("img").first().attr("src", pathPlus);
                    $('#Tutoraggio').find("img").first().attr("title", titlePathPlus); 
            }

            $('#TutoraggioToggler').click(function () {
                var isVisible = $('#TutoraggioContent').is(':visible');
                if (!isVisible) {
                    $('#TutoraggioContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    $('#Tutoraggio').find("img").first().attr("src", pathMinus);
                    $('#Tutoraggio').find("img").first().attr("title", titlePathMinus);
                    $('#flsTutoraggio').focus();
                    setTimeout(function () { $('#Tutoraggio').focus() }, 0);
                }
                else {
                    $('#flsTutoraggio').focus();
                    setTimeout(function () { $('#Tutoraggio').focus() }, 0);
                }
                return false;
            });
            $('#TornaSuTutoraggio').click(function () {
                tornaSu = 1;
            });
            $('#Tutoraggio').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#TutoraggioContent').is(':visible');
                    $('#TutoraggioContent').slideToggle('fast');
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server" >
    <fieldset id="GestioneProgetti"  class="ContornoPagina" runat="server">
<legend >Valutazione Qualit&#224;</legend>
<div class="wrapper"  style="width:100%;border:0px">
  <div class="headers" >
    <h2>
    <asp:Label ID="lblTitoloPagina" runat="server"  Text="Valutazione Qualit&#224;"></asp:Label>
    </h2>
</div>  
    
<br />
  

<div class="wrapper" style="width:100%;border:0px">
    <div class="row" style="height:auto">                  
        <div class="colHyperLink" style="width:50%">
            <asp:LinkButton ID="Prog" runat="server" style="cursor:pointer" Text="Visualizza Progetto" Visible="true"  OnClientClick="return AlertSalvataggioDati();"></asp:LinkButton>
        </div>
        <div class="colHyperLink" style="width:50%">
            <asp:LinkButton ID="imgElStorici" runat="server" style="cursor:pointer" Text="Cronologia" Visible="true" OnClientClick="return AlertSalvataggioDati();"></asp:LinkButton>
        </div>
        </div>
           
        <div class="row" style="height:auto">
                <div class="colHyperLink" style="width:50%">
            <asp:LinkButton ID="imgElencoDocumentiProg" runat="server" style="cursor:pointer" Text="Visualizza Documenti Progetto" Visible="true" OnClientClick="return AlertSalvataggioDati();"></asp:LinkButton>
        </div>                 
            <div class="colHyperLink" style="width:50%">
 
        </div>
        </div>

    </div>
  
  <fieldset  class="ContornoPagina" >
  <legend>Info Progetto</legend>
   <div class="wrapper"  style="width:100%;border:0px">
    <div class="row" >  
          <div class="collable" style="width:15%">
                <asp:Label ID="lblTitolo" class="label" AssociatedControlID="lblProgetto"  runat="server" Text="Titolo"></asp:Label>
            </div>
 
            <div class="colOggetti" style="width:35%">
                  <asp:Label ID="lblProgetto" class="labelDati"  runat="server" ></asp:Label>
            </div>      
            <div class="collable" style="width:15%">
            <asp:Label ID="labelEnte" class="label" AssociatedControlID="lblEnte"  runat="server" Text="Ente"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%">
                <asp:Label ID="lblEnte" class="labelDati"  runat="server" ></asp:Label>
        </div>    
    </div>
    <div class="rowGroup" runat="server" id="divTipoProgetto_0" visible="false">
    <div class="row" >  
            <div class="collable" style="width:15%">
            <asp:Label ID="labelNazione" class="label" AssociatedControlID="lblNazione"  runat="server" Text="Nazionalit&#224;"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%">
            <asp:Label ID="lblNazione" class="labelDati"  runat="server" ></asp:Label>
        </div>    
        <div class="collable" style="width:15%">
            <asp:Label ID="lblDataSeduta" class="label" AssociatedControlID="txtDataSeduta"  runat="server" Text="Data Seduta"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%">
            <asp:TextBox ID="txtDataSeduta" class="textbox" runat="server" ></asp:TextBox>
        </div>    
    </div>
     
     
    
    </div>
       
    </div>
    </fieldset>
  <fieldset  class="ContornoPagina"  id="FidsetSezioni" runat="server" visible="false">
  <legend>Sezioni</legend>
<div class="row" style="height:auto" >  
  
    <div id="EnteToggler" class="colHyperLink" style="width:100%">
        <asp:LinkButton ID="link6" runat="server" style="cursor:pointer"  Text="Sezione Caratteristiche Progetto" ></asp:LinkButton>
    </div>
     <div id="CaratteristicheProgettoToggler" class="colHyperLink" style="width:100%">
                <asp:LinkButton ID="link1" runat="server" style="cursor:pointer"   Text="Sezione Caratteristiche Organizzative" ></asp:LinkButton>
                </div>
     <div id="CaratteristicheCompetenzeToggler" class="colHyperLink" style="width:100%">
                <asp:LinkButton ID="LinkButton2" runat="server" style="cursor:pointer" Text="Sezione Caratteristiche delle Conoscenze Acquisite"></asp:LinkButton>
                </div>
    <div id="FormazioneSpecificaToggler" class="colHyperLink" style="width:100%" >
            <asp:LinkButton  ID="LinkButton3" runat="server" style="cursor:pointer" Text="Sezione Coerenza Generale" ></asp:LinkButton>
    </div>
     <div id="UlterioriMisureToggler" class="colHyperLink" style="width:100%" >
            <asp:LinkButton  ID="LinkButton4" runat="server" style="cursor:pointer" Text="Sezione Minori Opportunita" ></asp:LinkButton>
    </div>
     <div id="CoerenzaToggler" class="colHyperLink" style="width:100%" >
            <asp:LinkButton  ID="LinkButton5" runat="server" style="cursor:pointer" Text="Sezione Paese UE"  ></asp:LinkButton>
    </div>
         <div id="TutoraggioToggler" class="colHyperLink" style="width:100%"  >
            <asp:LinkButton  ID="LinkButton6" runat="server" style="cursor:pointer" Text="Sezione Tutoraggio" ></asp:LinkButton>
    </div>
             <div id="NoteRiservateToggler" class="colHyperLink" style="width:100%" >
            <asp:LinkButton  ID="LinkButton1" runat="server" style="cursor:pointer" Text="Sezione Note Riservate" ></asp:LinkButton>
    </div>     
</div>   
</fieldset>
  <fieldset id="Fieldset1" class="ContornoPagina" style="text-align:right">
      <asp:LinkButton ID="LinkButtonSx" runat="server" Visible="false" ToolTip="ATTENZIONE. Per applicare la scelta sarete reindirizzati alla Main del Sistema.">Punteggio a Sinistra</asp:LinkButton>
      &nbsp;&nbsp;
      <asp:LinkButton ID="LinkButtonDx" runat="server" Visible="false" ToolTip="ATTENZIONE. Per applicare la scelta sarete reindirizzati alla Main del Sistema.">Punteggio a Destra</asp:LinkButton>
  </fieldset>
  <fieldset id="flsEnte" class="ContornoPagina">
 <legend id="Ente"><img  src="images/minus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor:pointer;" />&nbsp;&nbsp;<img id="TornaSuEnte"  style="cursor:pointer;" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Ente</legend>
 <div class="wrapper" id="EnteContent" style="width:100%;border:0px">
        <asp:Panel ID="panelEnte" runat="server">
        
        </asp:Panel>
    
    </div>
      
     
 </fieldset>
 
  <fieldset id="flsCaratteristicheProgetto" class="ContornoPagina" >
 <legend id="CaratteristicheProgetto"><img  src="images/minus_xsmall.png" alt="Espandi/Riduci" style="cursor:pointer;"/>&nbsp;&nbsp;<img style="cursor:pointer;" id="TornaSuCaratteristicheProgetto" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Caratteristiche Progetto</legend>
 <div class="wrapper" id="CaratteristicheProgettoContent" style="width:100%;border:0px">
      <asp:Panel ID="panelCaratteristicheProgetto" runat="server"></asp:Panel>
 </div>
 </fieldset>

  <fieldset id="flsCaratteristicheCompetenze" class="ContornoPagina" >
 <legend id="CaratteristicheCompetenze"><img style="cursor:pointer;" src="images/minus_xsmall.png" alt="Espandi/Riduci" />&nbsp;&nbsp;<img style="cursor:pointer;" id="TornaSuCaratteristicheCompetenze" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Caratteristiche delle Competenze Acquisibili</legend>
 <div class="wrapper" id="CaratteristicheCompetenzeContent" style="width:100%;border:0px">
     <asp:Panel ID="panelCaratteristicheCompetenze"  runat="server">
     
     </asp:Panel>
</div>
 </fieldset>
  <fieldset id="flsFormazioneSpecifica" class="ContornoPagina" >
    <legend id="FormazioneSpecifica"><img id="ExpandFormazioneSpecifica" src="images/minus_xsmall.png" style="cursor:pointer;" alt="Espandi/Riduci" />&nbsp;&nbsp; <img id="TornaSuFormazioneSpecifica" onclick="TornaAdInizioPagina()" style="cursor:pointer;" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Formazione Specifica Degli Operatori Volontari</legend> 
    <div class="wrapper"  id="FormazioneSpecificaContent" style="width:100%;border:0px">
        <asp:Panel ID="panelFormazioneSpecifica" runat="server"></asp:Panel>    
 </div>
 </fieldset>

  <fieldset id="flsUlterioriMisure" class="ContornoPagina" >
    <legend id="UlterioriMisure"><img id="ExpandUlterioriMisure" src="images/minus_xsmall.png" style="cursor:pointer;" alt="Espandi/Riduci" />&nbsp;&nbsp; <img id="TornaSuUlterioriMisure" onclick="TornaAdInizioPagina()" style="cursor:pointer;" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Ulteriori Eventuali Misure a Favore Dei Giovani</legend> 
    <div class="wrapper"  id="UlterioriMisureContent" style="width:100%;border:0px">
        <asp:Panel ID="panelUlterioriMisure" runat="server"></asp:Panel>    
 </div>
 </fieldset>
   <fieldset id="flsCoerenza" class="ContornoPagina" >
    <legend id="Coerenza"><img id="ExpandCoerenza" src="images/minus_xsmall.png" style="cursor:pointer;" alt="Espandi/Riduci" />&nbsp;&nbsp; <img id="TornaSuCoerenza" onclick="TornaAdInizioPagina()" style="cursor:pointer;" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Coerenza</legend> 
    <div class="wrapper"  id="CoerenzaContent" style="width:100%;border:0px">
        <asp:Panel ID="panelCoerenza" runat="server"></asp:Panel>    
 </div>
 </fieldset>
<%--    <fieldset id="flsTutoraggio" class="ContornoPagina" >
    <legend id="Tutoraggio"><img id="EspandTutoraggio" src="images/minus_xsmall.png" style="cursor:pointer;" alt="Espandi/Riduci" />&nbsp;&nbsp; <img id="TornaSuTutoraggio" onclick="TornaAdInizioPagina()" style="cursor:pointer;" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Tutoraggio</legend> 
    <div class="wrapper"  id="TutoraggioContent" style="width:100%;border:0px">
        <asp:Panel ID="panelTutoraggio" runat="server"></asp:Panel>    
 </div>
 </fieldset>--%>

  
   <fieldset  class="ContornoPagina" >
    <div class="wrapper" style="width:100%;border:0px">
        <div class="row">
         <div class="collable" style="width:10%">
                <asp:Label ID="labelPunteggioFinale" class="label" AssociatedControlID="LblPuntegioFinale"  runat="server" Text="Punteggio Finale"></asp:Label>
          </div>
          <div class="colOggetti" style="width:10%">
                  <asp:Label ID="LblPuntegioFinale" class="labelDati" Text="0" runat="server" ></asp:Label>
           </div>
           <div class="collable" style="width:10%">
                <asp:Label ID="labelPunteggioCP" class="label" AssociatedControlID="lblPunteggioCP" runat="server" Text="Punteggio CP" Visible = "false"></asp:Label>
          </div>
          <div class="colOggetti" style="width:10%">
                  <asp:Label ID="lblPunteggioCP" class="labelDati" Text="0" runat="server" Visible = "false" ></asp:Label>
           </div>
             <div class="collable" style="width:10%">
                <asp:Label ID="labelpunteggioCO" class="label" AssociatedControlID="lblpunteggioCO"  runat="server" Text="Punteggio CO" Visible = "false"></asp:Label>
          </div>
          <div class="colOggetti" style="width:10%">
                  <asp:Label ID="lblpunteggioCO" class="labelDati" Text="0" runat="server" Visible = "false" ></asp:Label>
           </div>
            <div class="collable" style="width:10%">
                <asp:Label ID="labelpunteggioCKH" class="label" AssociatedControlID="lblPunteggioCKH"  runat="server" Text="Punteggio CHK"  Visible = "false" ></asp:Label>       
              </div>
              <div class="colOggetti" style="width:10%">
                   <asp:Label ID="lblPunteggioCKH" class="labelDati" Text="0" runat="server"  Visible = "false"></asp:Label>
              </div>
        <div class="collable" style="width:10%">
             <asp:Label ID="labelPunteggioCOE" class="label" AssociatedControlID="lblpunteggioCOE"  runat="server" Text="Punteggio COE" Visible = "false"></asp:Label>
          </div>
          <div class="colOggetti" style="width:10%">
                  <asp:Label ID="lblpunteggioCOE" class="labelDati" Text="0" runat="server"  Visible = "false"></asp:Label>
           </div>      
       </div>
        <div class="row">
          <div class="collable" style="width:15%">
                <asp:Label ID="labelDataUltima" class="label" AssociatedControlID="lbldataUltima" runat="server" Text="Valutazione del"></asp:Label>
          </div>
          <div class="colOggetti" style="width:35%">
                  <asp:Label ID="lbldataUltima" class="labelDati"  runat="server" ></asp:Label>
           </div>
        </div>
           <div class="row">
          <div class="collable" style="width:15%">
                <asp:Label ID="labelNoteRiservate" class="label" AssociatedControlID="lblNoteRiservate" runat="server" Text="Note Riservate"></asp:Label>
          </div>
          <div class="colOggetti" style="width:85%">
                  <asp:Label ID="lblNoteRiservate" class="labelDati"  runat="server" ></asp:Label>
           </div>
        </div>
             <div class="row">
          <div class="collable" style="width:15%">
                <asp:Label ID="labelNote" class="label" AssociatedControlID="txtnote" runat="server" Text="Note"></asp:Label>
          </div>
          <div class="colOggetti" style="width:85%">
                  <asp:TextBox ID="txtnote" class="textbox"  TextMode="MultiLine" Rows="4" runat="server" MaxLength="7000" ></asp:TextBox>
           </div>
        </div>
         </div>
        </fieldset>
       





       <div class="rowGroup" style="height:auto">
    <div class="row" style="height:auto">
                <asp:Label ID="msgErrore" CssClass="msgErrore" runat="server"></asp:Label>
                <asp:Label ID="msgInfo" CssClass="msgInfo" runat="server"></asp:Label>
               
    <asp:Label ID="msgConferma" runat="server" CssClass="msgConferma"></asp:Label>
               
                </div>
            <div class="RigaVuota" style="height:auto">&nbsp;</div>
   </div>
  












<div class="RigaPulsanti" > 
	<asp:Button id="cmdConferma" CssClass="Pulsante" runat="server" Text="Conferma"></asp:Button>
    <asp:Button id="cmdSalva" CssClass="Pulsante" runat="server" Text="Salva"></asp:Button>
    <asp:Button id="cmdStampaVQ" CssClass="Pulsante" runat="server" Text="Stampa" 
        Visible="False" ></asp:Button>
    <asp:Button id="imgStampaTot" CssClass="Pulsante" runat="server" 
        Text="Stampa Valutazione Totale" Visible="False" ></asp:Button>
    <asp:Button id="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" ></asp:Button>
</div>

</div>

<asp:HiddenField id="txtnome" runat="server"/>
<asp:HiddenField id="hf_IdStorico" runat="server" />
<asp:HiddenField id="hf_StatoProgetto" runat="server"/>
<asp:HiddenField id="hf_Personalizza" runat="server"/>
<asp:HiddenField id="hf_Pagina" runat="server"/>
<asp:HiddenField id="hf_BloccaMaschera" runat="server"/>
<asp:HiddenField id="txtx" Value="0" runat="server"/>
<asp:HiddenField id="txtTOTALEPUNTEGGIODESCRIZIONE" runat="server"/>
<asp:HiddenField id="txtTOTALEPUNTEGGIOCONTESTO" runat="server"/>
<asp:HiddenField id="txtTOTALEPUNTEGGIOOBIETTIVI" runat="server"/>
<asp:HiddenField id="Hdd_ConfermaValutazione" runat="server"/>
<asp:HiddenField id="txtNazionale" runat="server"/>
<asp:HiddenField id="TxtTipoprog1" runat="server"/>
<asp:HiddenField id="hf_IdProgetto" runat="server"/>

<asp:HiddenField runat="server" ID="hdnIsPostbackCP" />
<asp:HiddenField runat="server" ID="hdnIsPostbackCO" />
<asp:HiddenField runat="server" ID="hdnIsPostbackCCA" />
<asp:HiddenField runat="server" ID="hdnIsPostbackCG" />
<asp:HiddenField runat="server" ID="hdnIsPostbackNOTE" />
<asp:HiddenField runat="server" ID="hdnIsPostbackUlterioriMisure" />
<asp:HiddenField runat="server" ID="hdnIsPostbackCoerenza" />
<asp:HiddenField runat="server" ID="hdnIsPostbackTutoraggio" />

 </fieldset>
</asp:Content>