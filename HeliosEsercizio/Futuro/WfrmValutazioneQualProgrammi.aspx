<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmValutazioneQualProgrammi.aspx.vb" Inherits="Futuro.WfrmValutazioneQualProgrammi" %>
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
        var inserimento = '<%=Request.Params("IdProgramma")%>';
        //var VengoDa = '<%=Request.Params("VengoDa")%>'; //Request.QueryString("") = "AssociaVolontari"
        var isPostbackCaratteristicheProgramma  = '<%= hdnIsPostbackCP.value %>' ;
        var isPostbackCaratteristicheOrganizzative = '<%= hdnIsPostbackCO.value %>' ;
        var isPostbackCaratteristicheConAcquisite = '<%= hdnIsPostbackCCA.value %>' ;
        var isPostbackCoerenzaGenerale = '<%= hdnIsPostbackCG.value %>' ;
        var isPostbackNoteRiservate = '<%= hdnIsPostbackNOTE.value %>' ;
        var isPostbackMinoriOpportunita = '<%= hdnIsPostbackMinoriOpportunita.value %>' ;
        var isPostbackPaeseUE = '<%=  hdnIsPostbackPaeseUE.value %>' ;
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

             if( isPostbackCaratteristicheProgramma != "")
             {  
                $('#CaratteristicheProgrammaContent').css('display', 'none');
                $('#CaratteristicheProgramma').find("img").first().attr("src", pathPlus);
                $('#CaratteristicheProgramma').find("img").first().attr("title", titlePathPlus);
              }
          
                $('#CaratteristicheProgrammaToggler').click(function () {
       
                    var isVisible = $('#CaratteristicheProgrammaContent').is(':visible');
                    if (!isVisible) {
                        $('#CaratteristicheProgrammaContent').slideToggle('fast');
                        $(this).toggleClass('slideSign');
                        $('#CaratteristicheProgramma').find("img").first().attr("src", pathMinus);
                        $('#CaratteristicheProgramma').find("img").first().attr("title", titlePathMinus);
                        $('#flsCaratteristicheProgramma').focus();
                        setTimeout(function () { $('#CaratteristicheProgramma').focus() }, 0);  
                    
                     
                    }
                    else {
                        $('#flsCaratteristicheProgramma').focus();
                        setTimeout(function () { $('#CaratteristicheProgramma').focus() }, 0); 
                    
                    }
                    return false;
                });
                $('#TornaSuCaratteristicheProgramma').click(function () {
                    tornaSu = 1;
                });
                $('#CaratteristicheProgramma').click(function () {
 
                    if (tornaSu == 0) {
                  
                        var isVisible = $('#CaratteristicheProgrammaContent').is(':visible');
                        $('#CaratteristicheProgrammaContent').slideToggle('fast');
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
        </script>

  
   
    <script type="text/javascript">
       /* <![CDATA[ */
        $(function () {
            var tornaSu = 0;
        
            if( isPostbackCoerenzaGenerale != "")
            { 
                    $('#CoerenzaGeneraleContent').css('display', 'none');
                    $('#CoerenzaGenerale').find("img").first().attr("src", pathPlus);
                    $('#CoerenzaGenerale').find("img").first().attr("title", titlePathPlus); 
            }

            $('#CoerenzaGeneraleToggler').click(function () {
                var isVisible = $('#CoerenzaGeneraleContent').is(':visible');
                if (!isVisible) {
                    $('#CoerenzaGeneraleContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    $('#CoerenzaGenerale').find("img").first().attr("src", pathMinus);
                    $('#CoerenzaGenerale').find("img").first().attr("title", titlePathMinus);
                    $('#flsCoerenzaGenerale').focus();
                    setTimeout(function () { $('#CoerenzaGenerale').focus() }, 0);
                }
                else {
                    $('#flsCoerenzaGenerale').focus();
                    setTimeout(function () { $('#CoerenzaGenerale').focus() }, 0);
                }
                return false;
            });
            $('#TornaSuCoerenzaGenerale').click(function () {
                tornaSu = 1;
            });
            $('#CoerenzaGenerale').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#CoerenzaGeneraleContent').is(':visible');
                    $('#CoerenzaGeneraleContent').slideToggle('fast');
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
    <fieldset id="GestioneProgrammi"  class="ContornoPagina" runat="server">
<legend >Valutazione Qualit&#224; Programmi</legend>
<div class="wrapper"  style="width:100%;border:0px">
  <div class="headers" >
    <h2>
    <asp:Label ID="lblTitoloPagina" runat="server"  Text="Valutazione Qualit&#224; Programmi"></asp:Label>
    </h2>
</div>  
    
<br />
  

<div class="wrapper" style="width:100%;border:0px">
    <div class="row" style="height:auto">                  
        <div class="colHyperLink" style="width:50%">
            <asp:LinkButton ID="Prog" runat="server" style="cursor:pointer" Text="Visualizza Programma" Visible="true"  OnClientClick="return AlertSalvataggioDati();"></asp:LinkButton>
        </div>
        <div class="colHyperLink" style="width:50%">
            <asp:LinkButton ID="imgElStorici" runat="server" style="cursor:pointer" Text="Cronologia" Visible="true" OnClientClick="return AlertSalvataggioDati();"></asp:LinkButton>
        </div>
        </div>
          
        <div class="row" style="height:auto">
                <div class="colHyperLink" style="width:50%">
            <asp:LinkButton ID="imgElencoDocumentiProg" runat="server" style="cursor:pointer" Text="Visualizza Documenti Programma" Visible="true" OnClientClick="return AlertSalvataggioDati();"></asp:LinkButton>
        </div>                 
            <div class="colHyperLink" style="width:50%">
            
        </div>
        </div>
    </div>
  
  <fieldset  class="ContornoPagina" >
  <legend>Info Programma</legend>
   <div class="wrapper"  style="width:100%;border:0px">
    <div class="row" >  
          <div class="collable" style="width:15%">
                <asp:Label ID="lblTitolo" class="label" AssociatedControlID="lblProgramma"  runat="server" Text="Titolo"></asp:Label>
            </div>
 
            <div class="colOggetti" style="width:35%">
                  <asp:Label ID="lblProgramma" class="labelDati"  runat="server" ></asp:Label>
            </div>      
            <div class="collable" style="width:15%">
            <asp:Label ID="labelEnte" class="label" AssociatedControlID="lblEnte"  runat="server" Text="Ente"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%">
                <asp:Label ID="lblEnte" class="labelDati"  runat="server" ></asp:Label>
        </div>    
    </div>
    <div class="rowGroup" runat="server" id="divTipoProgramma_0" visible="false">
    <div class="row" >  
            <div class="collable" style="width:15%">
            
        </div>
        <div class="colOggetti" style="width:35%">
            
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
<%--  <fieldset  class="ContornoPagina"  id="FidsetSezioni" runat="server" >
  <legend>Sezioni</legend>
<div class="row" style="height:auto" >  
  
    <div id="CaratteristicheProgrammaToggler" class="colHyperLink" style="width:100%">
        <asp:LinkButton ID="link6" runat="server" style="cursor:pointer"  Text="Sezione Caratteristiche Programma" ></asp:LinkButton>
    </div>
 
    <div id="CoerenzaGeneraleToggler" class="colHyperLink" style="width:100%" >
            <asp:LinkButton  ID="LinkButton3" runat="server" style="cursor:pointer" Text="Sezione Coerenza Generale" ></asp:LinkButton>
    </div>
     
</div>   
</fieldset>--%>
  <fieldset id="Fieldset1" class="ContornoPagina" style="text-align:right">
      <asp:LinkButton ID="LinkButtonSx" runat="server" Visible="false" ToolTip="ATTENZIONE. Per applicare la scelta sarete reindirizzati alla Main del Sistema.">Punteggio a Sinistra</asp:LinkButton>
      &nbsp;&nbsp;
      <asp:LinkButton ID="LinkButtonDx" runat="server" Visible="false" ToolTip="ATTENZIONE. Per applicare la scelta sarete reindirizzati alla Main del Sistema.">Punteggio a Destra</asp:LinkButton>
  </fieldset>
  <fieldset id="flsCaratteristicheProgramma" class="ContornoPagina">
 <legend id="CaratteristicheProgramma"><img  src="images/minus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor:pointer;" />&nbsp;&nbsp;<img id="TornaSuCaratteristicheProgramma"  style="cursor:pointer;" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Caratteristiche 
     Programma</legend>
 <div class="wrapper" id="CaratteristicheProgrammaContent" style="width:100%;border:0px">
        <asp:Panel ID="panelCaratteristicheProgramma" runat="server">
        
        </asp:Panel>
    
    </div>
      
     
 </fieldset>
 
  <%--<fieldset id="flsCaratteristicheOrganizzative" class="ContornoPagina" >
 <legend id="CaratteristicheOrganizzative"><img  src="images/minus_xsmall.png" alt="Espandi/Riduci" style="cursor:pointer;"/>&nbsp;&nbsp;<img style="cursor:pointer;" id="TornaSuCaratteristicheOrganizzative" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Caratteristiche Organizzative</legend>
 <div class="wrapper" id="CaratteristicheOrganizzativeContent" style="width:100%;border:0px">
      <asp:Panel ID="panelCaratteristicheOrganizzative" runat="server"></asp:Panel>
 </div>
 </fieldset>--%>

 <%-- <fieldset id="flsCaratteristicheConAcquisite" class="ContornoPagina" >
 <legend id="CaratteristicheConAcquisite"><img style="cursor:pointer;" src="images/minus_xsmall.png" alt="Espandi/Riduci" />&nbsp;&nbsp;<img style="cursor:pointer;" id="TornaSuCaratteristicheConAcquisite" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Caratteristiche delle Conoscenze Acquisite</legend>
 <div class="wrapper" id="CaratteristicheConAcquisiteContent" style="width:100%;border:0px">
     <asp:Panel ID="panelCaratteristicheConAcquisite"  runat="server">
     
     </asp:Panel>
</div>
 </fieldset>--%>
  <fieldset id="flsCoerenzaGenerale" class="ContornoPagina" >
    <legend id="CoerenzaGenerale"><img id="ExpandCoerenzaGenerale" src="images/minus_xsmall.png" style="cursor:pointer;" alt="Espandi/Riduci" />&nbsp;&nbsp; <img id="TornaSuCoerenzaGenerale" onclick="TornaAdInizioPagina()" style="cursor:pointer;" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Coerenza</legend> 
    <div class="wrapper"  id="CoerenzaGeneraleContent" style="width:100%;border:0px">
        <asp:Panel ID="panelCoerenzaGenerale" runat="server"></asp:Panel>    
 </div>
 </fieldset>

  <%--<fieldset id="flsMinoriOpportunita" class="ContornoPagina" >
    <legend id="MinoriOpportunita"><img id="ExpandMinoriOpportunita" src="images/minus_xsmall.png" style="cursor:pointer;" alt="Espandi/Riduci" />&nbsp;&nbsp; <img id="TornaSuMinoriOpportunita" onclick="TornaAdInizioPagina()" style="cursor:pointer;" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Minori Opportunita</legend> 
    <div class="wrapper"  id="MinoriOpportunitaContent" style="width:100%;border:0px">
        <asp:Panel ID="panelMinoriOpportunita" runat="server"></asp:Panel>    
 </div>
 </fieldset>
   <fieldset id="flsPaeseUE" class="ContornoPagina" >
    <legend id="PaeseUE"><img id="ExpandPaeseUe" src="images/minus_xsmall.png" style="cursor:pointer;" alt="Espandi/Riduci" />&nbsp;&nbsp; <img id="TornaSuPaeseUE" onclick="TornaAdInizioPagina()" style="cursor:pointer;" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Paese UE</legend> 
    <div class="wrapper"  id="PaeseUEContent" style="width:100%;border:0px">
        <asp:Panel ID="panelPaeseUE" runat="server"></asp:Panel>    
 </div>
 </fieldset>
    <fieldset id="flsTutoraggio" class="ContornoPagina" >
    <legend id="Tutoraggio"><img id="EspandTutoraggio" src="images/minus_xsmall.png" style="cursor:pointer;" alt="Espandi/Riduci" />&nbsp;&nbsp; <img id="TornaSuTutoraggio" onclick="TornaAdInizioPagina()" style="cursor:pointer;" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Tutoraggio</legend> 
    <div class="wrapper"  id="TutoraggioContent" style="width:100%;border:0px">
        <asp:Panel ID="panelTutoraggio" runat="server"></asp:Panel>    
 </div>
 </fieldset>--%>

   <%--<fieldset id="flsNoteRiservate" class="ContornoPagina" >
    <legend id="NoteRiservate"><img id="ExpandNoteRiservate"   src="images/minus_xsmall.png" style="cursor:pointer;" alt="Espandi/Riduci" />&nbsp;&nbsp; <img id="TornaSuNoteRiservate" onclick="TornaAdInizioPagina()" style="cursor:pointer;" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Note Riservate</legend> 
    <div class="wrapper"  id="NoteRiservateContent" style="width:100%;border:0px">
        <asp:Panel ID="panelNoteRiservate" runat="server">
        <div class="row">
        <div class="collable" style="width:50%">
            <asp:Label CssClass="labelDati" Visible="false" ID="lbl24EsteroAssenti" AssociatedControlID="Txt24EsteroAssenti" runat="server" Text="(24) Eventuale assicurazione integrativa a quella stipulata dall'UNSC (Punteggio 0)"></asp:Label>
        </div>
        <div class="colOggetti" style="width:50%">
            <asp:TextBox ID="Txt24EsteroAssenti"  runat="server" TextMode="MultiLine" Rows="4" Visible="false" CssClass="textbox"></asp:TextBox>
        </div>
        </div>
            <div class="row">
        <div class="collable" style="width:50%">
            <asp:Label ID="lbl24A" CssClass="labelDati" Visible="false" AssociatedControlID="Txt24Assenti"   runat="server" Text="(24) Eventuali risorse finanziarie aggiuntive destinate in modo specifico alla realizzazione del progetto (Punteggio 0)"></asp:Label>
        <asp:Label ID="lbl31A" CssClass="labelDati" Visible="false" AssociatedControlID="Txt24Assenti"   runat="server" Text="(31) Eventuali risorse finanziarie aggiuntive destinate in modo specifico alla realizzazione del progetto (Punteggio 0)"></asp:Label>
        </div>
        <div class="collable" style="width:50%">
             	<asp:TextBox id="Txt24Assenti" runat="server" Enabled="false" Rows="4" TextMode="MultiLine" CssClass="textbox" ></asp:TextBox>
        </div>
        </div>
        <div class="row">
        <div class="collable" style="width:50%">
            <asp:Label ID="lbl24P" CssClass="labelDati" Visible="false" AssociatedControlID="Txt24Presenti"   runat="server" Text="(24) Eventuali risorse finanziarie aggiuntive destinate in modo specifico alla realizzazione del progetto (Punteggio 1)"></asp:Label>
        <asp:Label ID="lbl31P" CssClass="labelDati" Visible="false" AssociatedControlID="Txt24Presenti"   runat="server" Text="(31) Eventuali risorse finanziarie aggiuntive destinate in modo specifico alla realizzazione del progetto (Punteggio 1)"></asp:Label>
        </div>
        <div class="collable" style="width:50%">
             	<asp:TextBox id="Txt24Presenti" runat="server" Enabled="false" Rows="4" CssClass="textbox" TextMode="MultiLine"></asp:TextBox>
        </div>
        </div>
        <div class="row">
        <div class="collable" style="width:50%">
            <asp:Label ID="lbl25" CssClass="labelDati" Visible="false" AssociatedControlID="Txt25Assenti"   runat="server" Text="(25) Eventuali copromotori e partners del progetto con la specifica del ruolo concreto rivestito dagli stessi all'interno del progetto (Punteggio 0)"></asp:Label>
        <asp:Label ID="lbl32" CssClass="labelDati" Visible="false" AssociatedControlID="Txt25Assenti"   runat="server" Text="(32)Eventuali copromotori e partners del progetto con la specifica del ruolo concreto rivestito dagli stessi all'interno del progetto (Punteggio 0)"></asp:Label>
        </div>
        <div class="collable" style="width:50%">
             	<asp:TextBox id="Txt25Assenti" runat="server" Enabled="false" Rows="4" CssClass="textbox" TextMode="MultiLine"></asp:TextBox>
        </div>
        </div>
        <div class="row">
        <div class="collable" style="width:50%">
            <asp:Label ID="lbl28" CssClass="labelDati" Visible="false" AssociatedControlID="Txt28Assenti"   runat="server" Text="(28) Eventuali tirocini riconosciuti (Punteggio 0)"></asp:Label>
        <asp:Label ID="lbl35" CssClass="labelDati" Visible="false" AssociatedControlID="Txt28Assenti"   runat="server" Text="(35) Eventuali tirocini riconosciuti (Punteggio 0)"></asp:Label>
        </div>
        <div class="collable" style="width:50%">
             	<asp:TextBox id="Txt28Assenti" runat="server" Enabled="false" Rows="4" CssClass="textbox" TextMode="MultiLine"></asp:TextBox>
        </div>
        </div>
        <div class="row">
        <div class="collable" style="width:50%">
            <asp:Label ID="lbl29" CssClass="labelDati" Visible="false" AssociatedControlID="Txt29Assenti"   runat="server" Text="(29) Competenze e professionalità acquisibili dai volontari durante l'espletamento del servizio certificabili e valide ai fini del CV (Punteggio 0)"></asp:Label>
        <asp:Label ID="lbl36" CssClass="labelDati" Visible="false" AssociatedControlID="Txt29Assenti"   runat="server" Text="(36) Competenze e professionalità acquisibili dai volontari durante l'espletamento del servizio certificabili e valide ai fini del CV (Punteggio 0)"></asp:Label>
        </div>
        <div class="collable" style="width:50%">
             	<asp:TextBox id="Txt29Assenti" runat="server" Enabled="false" Rows="4" CssClass="textbox" TextMode="MultiLine"></asp:TextBox>
        </div>
        </div>
        </asp:Panel>  
         <asp:Panel ID="panelNoteRiservateDisabilitate" runat="server" Visible="false">
        <div class="row">
        <div class="collable" style="width:100%">
            <asp:Label CssClass="labelDati" ID="LabelNoteRiservateDisabilitate" runat="server" Text="Note Riservate non presenti per questo progetto." ></asp:Label>

        </div>
        </div>    
        </asp:Panel>
 </div>
 </fieldset>--%>
    
   <fieldset  class="ContornoPagina" >
    <div class="wrapper" style="width:100%;border:0px">
        <div class="row">
         <div class="collable" style="width:10%">
                <asp:Label ID="labelPunteggioFinale" class="label" AssociatedControlID="LblPuntegioFinale"  runat="server" Text="Punteggio Programma"></asp:Label>
          </div>
          <div class="colOggetti" style="width:10%">
                  <asp:Label ID="LblPuntegioFinale" class="labelDati" Text="0" runat="server" ></asp:Label>
           </div>
           <div class="collable" style="width:10%">
                <asp:Label ID="labelPunteggioCP" class="label" AssociatedControlID="lblPunteggioCP" runat="server" Text="Punteggio CP" Visible = "false"></asp:Label>
          </div>
          <div class="colOggetti" style="width:10%">
                  <asp:Label ID="lblPunteggioCP" class="labelDati" Text="0" runat="server"  Visible = "false"></asp:Label>
           </div>
             <div class="collable" style="width:10%">
                <asp:Label ID="labelPunteggioCOE" class="label" AssociatedControlID="lblpunteggioCOE"  runat="server" Text="Punteggio COE" Visible = "false"></asp:Label>
          </div>
          <div class="colOggetti" style="width:10%">
                  <asp:Label ID="lblpunteggioCOE" class="labelDati" Text="0" runat="server" Visible = "false" ></asp:Label>
           </div>
            <div class="collable" style="width:10%">
                
                <asp:Label ID="labelPunteggioTotale" class="label" 
                    AssociatedControlID="LblPuntegioTotaleRis"  runat="server" Text="Punteggio Totale"></asp:Label>
                
              </div>
              <div class="colOggetti" style="width:10%">
                   
                  <asp:Label ID="LblPuntegioTotaleRis" class="labelDati" Text="0" runat="server" ></asp:Label>
                   
              </div>
        <div class="collable" style="width:10%">
             
          </div>
          <div class="colOggetti" style="width:10%">
                  
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
<%--          <div class="row">
          <div class="collable" style="width:15%">
                <asp:Label ID="labelNoteRiservate" class="label" AssociatedControlID="lblNoteRiservate" runat="server" Text="Note Riservate"></asp:Label>
          </div>
          <div class="colOggetti" style="width:85%">
                  <asp:Label ID="lblNoteRiservate" class="labelDati"  runat="server" ></asp:Label>
           </div>
        </div>--%>
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
<asp:HiddenField id="hf_StatoProgramma" runat="server"/>
<asp:HiddenField id="hf_Personalizza" runat="server"/>
<asp:HiddenField id="hf_Pagina" runat="server"/>
<asp:HiddenField id="hf_BloccaMaschera" runat="server"/>
<asp:HiddenField id="txtx" Value="0" runat="server"/>
<asp:HiddenField id="txtTOTALEPUNTEGGIODESCRIZIONE" runat="server"/>
<asp:HiddenField id="txtTOTALEPUNTEGGIOCONTESTO" runat="server"/>
<asp:HiddenField id="txtTOTALEPUNTEGGIOOBIETTIVI" runat="server"/>
<asp:HiddenField id="Hdd_ConfermaValutazione" runat="server"/>
<%--<asp:HiddenField id="txtNazionale" runat="server"/>--%>
<asp:HiddenField id="TxtTipoprog1" runat="server"/>
<asp:HiddenField id="hf_IdProgramma" runat="server"/>

<asp:HiddenField runat="server" ID="hdnIsPostbackCP" />
<asp:HiddenField runat="server" ID="hdnIsPostbackCO" />
<asp:HiddenField runat="server" ID="hdnIsPostbackCCA" />
<asp:HiddenField runat="server" ID="hdnIsPostbackCG" />
<asp:HiddenField runat="server" ID="hdnIsPostbackNOTE" />
<asp:HiddenField runat="server" ID="hdnIsPostbackMinoriOpportunita" />
<asp:HiddenField runat="server" ID="hdnIsPostbackPaeseUE" />
<asp:HiddenField runat="server" ID="hdnIsPostbackTutoraggio" />

 </fieldset>
</asp:Content>
