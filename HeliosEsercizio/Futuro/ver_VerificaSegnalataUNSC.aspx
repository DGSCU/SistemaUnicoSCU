<%@ Page Title="Verifica su Segnalazione DGSCN" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"    CodeBehind="ver_VerificaSegnalataUNSC.aspx.vb" Inherits="Futuro.ver_VerificaSegnalataUNSC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
   <script type="text/javascript" language="javascript">
        /* <![CDATA[ */
        var prefissoIdClient = "MainContent_";

        function CostruisciId(IdServer) {
            var IdClient = prefissoIdClient + IdServer;
            return IdClient;
        };
  
        /* ]]> */
    </script>


    <script type="text/javascript" language="JavaScript">
      /* <![CDATA[ */
           function fncmdStampa()
                {
	                myWin = window.open ('WfrmReportistica.aspx?sTipoStampa=30&IdVerifica=' + <%=Request.QueryString("IdVerifica")%>,'Report','height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes')
	                return false;
                }
                   /* ]]> */
    </script>
    <script type="text/javascript" language="JavaScript" >
      
       /* <![CDATA[ */
        function fnimgRicercaEnte() {
 

            var IdtxtIdEnteSedeAttuazione = CostruisciId("txtIdEnteSedeAttuazione");
            var IdtxtSede = CostruisciId("txtSede");
            var IdTxtEnteFiglio = CostruisciId("TxtEnteFiglio");

            var IdtxtIdAttivita = CostruisciId("txtIdAttivita");

            var IdtxtProgetto = CostruisciId("txtProgetto");

            var IdtxtIdEnte = CostruisciId("txtIdEnte");
            var IDtxtEnte = CostruisciId("txtEnte");

              formId = document.forms[0];
              var IdImgRicercaProgetto = CostruisciId("ImgRicercaProgetto");

              var IdimgRicercaSede = CostruisciId("imgRicercaSede");
 


              myWin = window.open("ver_RicercaEnteSegnalataUNSC.aspx?objForm=" + formId + "&IdImgRicercaProgetto=" + IdImgRicercaProgetto + "&IdimgRicercaSede=" + IdimgRicercaSede + "&objIdEnteSedeAttuazione=" + IdtxtIdEnteSedeAttuazione + "&objSede=" + IdtxtSede + "&objEnteF=" + IdTxtEnteFiglio + "&objIdAttivita=" + IdtxtIdAttivita + "&objProgetto=" + IdtxtProgetto + "&objIdEnte=" + IdtxtIdEnte + "&objDescrEnte=" + IDtxtEnte, "Ricerca", "width=800,height=600,dependent=no,scrollbars=yes,status=no,resizable=yes");
            return false;
 

        }




        function fnImgRicercaProgetto() {

            var IdtxtIdEnteSedeAttuazione = CostruisciId("txtIdEnteSedeAttuazione");
            var IdtxtSede = CostruisciId("txtSede");
            var IdTxtEnteFiglio = CostruisciId("TxtEnteFiglio");

            var IdtxtIdAttivita = CostruisciId("txtIdAttivita");

            var IdtxtProgetto = CostruisciId("txtProgetto");

            var IdtxtIdEnte = CostruisciId("txtIdEnte");
            var IDtxtEnte = CostruisciId("txtEnte");

            formId = document.forms[0];
            var IdImgRicercaProgetto = CostruisciId("ImgRicercaProgetto");

            var IdimgRicercaSede = CostruisciId("imgRicercaSede");
 
            myWin = window.open("ver_RicercaProgettoSegnalataUNSC.aspx?objForm=" + formId + "&objIdEnteSedeAttuazione=" + IdtxtIdEnteSedeAttuazione + "&objSede=" + IdtxtSede + "&objEnteF=" + IdTxtEnteFiglio + "&objIdAttivita=" + IdtxtIdAttivita + "&objProgetto=" + IdtxtProgetto + "&IDEnte=" + document.getElementById(CostruisciId("txtIdEnte")).value, "Ricerca", "width=800,height=600,dependent=no,scrollbars=yes,status=no,resizable=yes");
       
            return false;
        }
           /* ]]> */

    </script>
 
   
  
    <script type="text/javascript" language="JavaScript" >
     /* <![CDATA[ */
        function fnimgRicercaSede() {
            if (document.getElementById(CostruisciId("txtProgetto")).value   == '') {
                alert("E' necessario indicare il progetto prima di ricercare la sede.");
                return false;
            }
            else {
                var IdtxtIdEnteSedeAttuazione = CostruisciId("txtIdEnteSedeAttuazione");
                var IdtxtSede = CostruisciId("txtSede");
                var IdTxtEnteFiglio = CostruisciId("TxtEnteFiglio");

                var IdtxtIdAttivita = CostruisciId("txtIdAttivita");

                var IdtxtProgetto = CostruisciId("txtProgetto");

                var IdtxtIdEnte = CostruisciId("txtIdEnte");
                var IDtxtEnte = CostruisciId("txtEnte");

                formId = document.forms[0];
                var IdImgRicercaProgetto = CostruisciId("ImgRicercaProgetto");

                var IdimgRicercaSede = CostruisciId("imgRicercaSede");

                myWin = window.open("ver_RicercaSedeSegnalataUNSC.aspx?objForm=" + formId + "&objIdEnteSedeAttuazione=" + IdtxtIdEnteSedeAttuazione + "&objSede=" + IdtxtSede + "&objEnteF=" + IdTxtEnteFiglio + "&IdAttivita=" + document.getElementById(CostruisciId("txtIdAttivita")).value + "&IDEnte=" + document.getElementById(CostruisciId("txtIdEnte")).value, "Ricerca", "width=800,height=600,dependent=no,scrollbars=yes,status=no");
                return false;
            }
        }
        /* ]]> */
    </script>


    <script type="text/javascript" language="JavaScript" >
 
			 /* <![CDATA[ */

		function ApriAllegati(NumProt,DataProt)
			{

            	 
                   var IdFascicolo = document.getElementById(CostruisciId('TxtCodiceFasc')).value               
                   var idTxtCodiceFascicolo =document.getElementById(CostruisciId('TxtCodiceFascicolo')).value   
                
					if (  idTxtCodiceFascicolo  == ''){
							alert ("Specificare il numero fascicolo!");						 
					}else{	              					
                    		myWin = window.open("WfrmSIGEDElencoAllegati.aspx?NumeroProtocollo=" + document.getElementById(CostruisciId(NumProt)).value + "&DataProtocollo=" + document.getElementById(CostruisciId(DataProt)).value + "&CodiceFascicolo=" +IdFascicolo,"Visualizza","width=800,height=350,dependent=no,scrollbars=yes,status=no");		
						
					}
			}
			    

         function SelProtocollo(NumProt,DataProt)
			{
			 
             
            var IdTxtCodiceFascicolo = CostruisciId("TxtCodiceFascicolo");
            var codFasc =  $('#<%= TxtCodiceFasc.ClientID %>').val();  
            //var codFasc = IdFascicolo;
            var codiceFascicolo = document.getElementById(IdTxtCodiceFascicolo).value;
            var NumFascicolo = '<%= TxtCodiceFascicolo.Text %>';

            // alert (IdFascicolo);
            //   alert (codFasc);
					if (codFasc  =='')
					{
						alert ("Specificare il numero fascicolo!");				
					}
					
					else
					{
						var Val;
						var ChiudiContestata;
						Val=document.getElementById(CostruisciId('cmdChiusaContestata') ) ;
						if (Val==null)
						{
							ChiudiContestata=0;
						}
						else
						{
							ChiudiContestata=1;
						}
                         //myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?Classificazione=" + Classificazione + "&ChiudiContestata=" + ChiudiContestata + "&objForm=Form1&TxtProt=" + CostruisciId(NumProt) + "&TxtData=" + CostruisciId(DataProt) + "&NumeroFascicolo=" + codFasc, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");
                        myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?ChiudiContestata=" + ChiudiContestata + "&objForm=Form1+&TxtProt="+CostruisciId(NumProt)+"&TxtData="+CostruisciId(DataProt)+"&NumeroFascicolo=" + codFasc, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");			
						//myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?ChiudiContestata=" + ChiudiContestata + "&objForm=Form1&TxtProt="+CostruisciId(NumProt)+"&TxtData="+CostruisciId(DataProt)+"&NumeroFascicolo=" + codFasc, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");			
					}
                      return false;

			}

		function Servizi()
			{ 


				var IdVerifica;
                IdModello=94;
				IdVerifica='<%=request.querystring("IdVerifica")%>';
				window.open("WfrmSanzioneServizi.aspx?SanzioneServizi=SanzioneServizi&IdVerifica=" + IdVerifica + "&IdModello=" + IdModello, "Visualizza", "width=600,height=550,dependent=no,scrollbars=yes,status=no");
                  return false;
			}
               /* ]]> */
    </script>
    <script type="text/javascript" language="JavaScript" >
           /* <![CDATA[ */
             $(function () {
                var clientId = "#" + CostruisciId('txtDataSegnalazione');
                    $("" + clientId + "").datepicker();
             });
            $(function () {
                var clientId = "#" + CostruisciId('txtDataProtSegnalazione');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('txtDataProtRicSegnalazione');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('TxtDataProtApprovazione');
                    $("" + clientId + "").datepicker();
            });


 

            $(function () {
                var clientId = "#" + CostruisciId('txtDataProtLetteraContestazioneDG');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('txtDataProtocolloInvioLetteraContestazione');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('txtDataRicezioneLetteraContestazione');
                    $("" + clientId + "").datepicker();
            });
            $(function () {
                var clientId = "#" + CostruisciId('txtDataRispostaLetteraContestazione');
                    $("" + clientId + "").datepicker();
            });
            $(function () {
                var clientId = "#" + CostruisciId('txtDataProtocolloRispostaContestazione');
                    $("" + clientId + "").datepicker();
            });
           $(function () {
                var clientId = "#" + CostruisciId('TxtDataProtChiusuraContestazione');
                    $("" + clientId + "").datepicker();
            });


           $(function () {
                var clientId = "#" + CostruisciId('txtDataProtocolloTrasmissioneSanzione');
                    $("" + clientId + "").datepicker();
            });

               $(function () {
                var clientId = "#" + CostruisciId('txtDataProtocolloEsecuzioneSanzione');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('txtDataProtocolloTrasmServizi');
                    $("" + clientId + "").datepicker();
            });
           $(function () {
                var clientId = "#" + CostruisciId('txtDataEsecuzioneSanzione');
                    $("" + clientId + "").datepicker();
            });
           
       
                 /* ]]> */
    </script>
    <script language="JavaScript" type="text/JavaScript">


         /* <![CDATA[ */



        function ElencoFascicoli() {
            var winOption = PopUpOption();
            var IdTxtCodiceFasc = CostruisciId("TxtCodiceFasc");
            var IdTxtCodiceFascicolo = CostruisciId("TxtCodiceFascicolo");
            var IdDescrizioneFascicolo = CostruisciId("txtDescFasc");
            var codiceFascicolo = document.getElementById(IdTxtCodiceFascicolo).value;
            
            var NumFascicolo = document.getElementById(CostruisciId('TxtCodiceFascicolo')).value

            formId = document.forms[0];

            var CodiceFasc = document.getElementById(CostruisciId('TxtCodiceFascicolo')).value
            
        
            myWin = window.open("WfrmSIGEDElencoFascicoli.aspx?Processo=MONITORAGGIO&objForm=" + formId + "&objNumero=" + IdTxtCodiceFascicolo + "&objCodice=" + IdTxtCodiceFasc + "&objDescFasc=" + IdDescrizioneFascicolo + "&IdFascicolo=" + codiceFascicolo, "Visualizza", "width=800,height=350,dependent=no,scrollbars=yes,status=no");
            return false;
        }
         /* ]]> */
    </script>
    <script language="JavaScript" type="text/JavaScript">
     /* <![CDATA[ */
        function SelProtocollo0() {



            var codFasc = document.getElementById(CostruisciId("TxtCodiceFasc"));

            if (codFasc == '') {
                alert("Specificare il numero fascicolo!");

            }
            else {
              

                myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?NumeroFascicolo=" + codFasc, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");
            }
            return false;

        }
         /* ]]> */
    </script>
    <script language="JavaScript" type="text/JavaScript">
     /* <![CDATA[ */
        function SelDocumentoInterno(IDDoc, NumDoc, NomeFile, Classificazione) {

           

            var codFasc = document.getElementById(CostruisciId("TxtCodiceFasc"));
       


            if (document.getElementById(CostruisciId(NumDoc)).style.backgroundColor != 'lightgrey') {

                var idTxtCodiceFascicolo = document.getElementById(CostruisciId("TxtCodiceFascicolo"));
 
                if (idTxtCodiceFascicolo == '') {

                    alert("Specificare il numero fascicolo!");

                } else {


                    if (document.getElementById(CostruisciId("txtDataProtSegnalazione")).value != '' || document.getElementById(CostruisciId("txtDataProtRicSegnalazione")).value != '' || document.getElementById(CostruisciId("TxtDataProtApprovazione")).value != '') {
                        alert("Non è possibile associare il documento interno per la presenza di protocolli indicati alla segnalazione.");
                    }
                    else {
                        var Val;
                        var ChiudiContestata;
                        Val = document.getElementById(CostruisciId('cmdChiusaContestata'));
                        if (Val == null) {
                            ChiudiContestata = 0;
                        }
                        else {
                            ChiudiContestata = 1;
                        }
                    
                        

                        var winOption = PopUpOption();
                        formId = document.forms[0];



                        myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?Classificazione=" + Classificazione + "&ChiudiContestata=" + ChiudiContestata + "&objForm=" + formId + "&txtIDDocInterno=" + IDDoc + "&txtNomeFile=" + NomeFile + "&TxtNumDoc=" + NumDoc + "&NumeroFascicolo=" + codFasc, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=si");
                    }
                }
            }
        }



        function SelProtocolloProtSeg(NumProt, DataProt, Classificazione) {
        
            var IdTxtCodiceFascicolo = CostruisciId("TxtCodiceFascicolo");
            var codFasc = $('#<%= TxtCodiceFasc.ClientID %>').val();
            //var codFasc = IdFascicolo;
            var codiceFascicolo = document.getElementById(IdTxtCodiceFascicolo).value;
            var NumFascicolo = '<%= TxtCodiceFascicolo.Text %>';
            var numeroDoc = CostruisciId("txtNumDocInterno");
         //   if (document.getElementById(CostruisciId(NumProt)).style.backgroundColor != 'lightgrey') {
                if (document.getElementById(CostruisciId('TxtCodiceFascicolo')).value == '') {
                    alert("Specificare il numero fascicolo!");
                }
                else {
                    //if (document.getElementById(CostruisciId('txtNumDocInterno')).value != '') {
                    if (document.getElementById(numeroDoc).value != '') {
                        alert("Non e' possibile associare il protocollo per la presenza di un documento interno.");
                    }
                    else {
                        var Val;
                        var ChiudiContestata;
                        Val = document.getElementById('cmdChiusaContestata')
                        if (Val == null) {
                            ChiudiContestata = 0;
                        }
                        else {
                            ChiudiContestata = 1;
                        }
                    //= Replace(document.getElementById(CostruisciId('TxtCodiceFasc')).value);
                        //myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?Classificazione=" + Classificazione + "&ChiudiContestata=" + ChiudiContestata + "&objForm=Form1&txtNomeFile=" + DataProt + "&TxtProt=" + NumProt + "&TxtData=" + DataProt + "&NumeroFascicolo=" + codFasc, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");
                        myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?Classificazione=" + Classificazione + "&ChiudiContestata=" + ChiudiContestata + "&objForm=Form1&TxtProt=" + CostruisciId(NumProt) + "&TxtData=" + CostruisciId(DataProt) + "&NumeroFascicolo=" + codFasc, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");
                    }
                }
            //}
        }
          /* ]]> */
    </script>

      <script  type="text/javascript" language="JavaScript" >
      /* <![CDATA[ */
        var pathPlus = "images/plus_xsmall.png";
        var pathMinus = "images/minus_xsmall.png";
        var titlePathMinus = "Riduci";
        var titlePathPlus = "Espandi";
        var inserimento = '<%=Request.Params("IdAttivita")%>';
        var DataSegnalazione = '<%=txtDataSegnalazione.Text%>';
        var DocInterno = '<%=txtNumDocInterno.Text%>';
        var DataProtSeganalazione = '<%=txtDataProtSegnalazione%>';
        var DataProtRispControdeduzioni='<%=txtDataRicezioneLetteraContestazione.Text%>';
        var inserimento1 = '<%=lblStatoVerifica.Text%>';
      
        /* ]]> */  
    </script>
      <script type="text/javascript" language="JavaScript" >
       /* <![CDATA[ */
        $(function () {
            var tornaSu = 0;
           // alert(inserimento1);
            if (inserimento != "") {
                $('#InformazioniGeneraliContent').css('display', 'none');
                $('#InformazioniGenerali').find("img").first().attr("src", pathPlus);
                $('#InformazioniGenerali').find("img").first().attr("title", titlePathPlus);
            }
            else {
                $('#InformazioniGeneraliContent').css('display', 'yes');
                $('#InformazioniGenerali').find("img").first().attr("src", pathMinus);
                $('#InformazioniGenerali').find("img").first().attr("title", titlePathMinus);
            }
            $('#flsInformazioniGeneraliToggler').click(function () {
                var isVisible = $('#InformazioniGeneraliContent').is(':visible');
                if (!isVisible) {
                    $('#InformazioniGeneraliContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    $('#InformazioniGenerali').find("img").first().attr("src", pathMinus);
                    $('#InformazioniGenerali').find("img").first().attr("title", titlePathMinus);
                    $('#flsInformazioniGenerali').focus();
                    setTimeout(function () { $('#InformazioniGenerali').focus() }, 0);  
                }
                else {
                    $('#flsInformazioniGenerali').focus();
                    setTimeout(function () { $('#InformazioniGenerali').focus() }, 0); 
                }
                return false;
            });
            $('#TornaSuInformazioniGenerali').click(function () {
                tornaSu = 1;
            });
            $('#InformazioniGenerali').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#InformazioniGeneraliContent').is(':visible');
                    $('#InformazioniGeneraliContent').slideToggle('fast');
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
     //  alert(DataSegnalazione);
            var str = 0;
            if ( DataSegnalazione =='' && DocInterno  =='' && DataProtSeganalazione =='')
             {  str==1;
                alert('ok');
             }
            var tornaSu = 0;
            if (inserimento1 != "Contestata") {
           // alert(str);
                if ( str= 0) {
                        $('#DatiContestazioneContent').css('display', 'none');
                        $('#DatiContestazione').find("img").first().attr("src", pathPlus);
                        $('#DatiContestazione').find("img").first().attr("title", titlePathPlus);
                    }
                 else
                    {
                        $('#DatiContestazioneContent').css('display', 'yes');
                        $('#DatiContestazione').find("img").first().attr("src", pathMinus);
                        $('#DatiContestazione').find("img").first().attr("title", titlePathMinus);
                    }

                }
            else {
                $('#DatiContestazioneContent').css('display', 'yes');
                $('#DatiContestazione').find("img").first().attr("src", pathMinus);
                $('#DatiContestazione').find("img").first().attr("title", titlePathMinus);
            }
            $('#flsDatiContestazioneToggler').click(function () {
                var isVisible = $('#DatiContestazioneContent').is(':visible');
                if (!isVisible) {
                    $('#DatiContestazioneContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    $('#DatiContestazione').find("img").first().attr("src", pathMinus);
                    $('#DatiContestazione').find("img").first().attr("title", titlePathMinus);
                    $('#flsDatiContestazione').focus();
                    setTimeout(function () { $('#DatiContestazione').focus() }, 0);  
                }
                else {
                    $('#flsDatiContestazione').focus();
                    setTimeout(function () { $('#DatiContestazione').focus() }, 0); 
                }
                return false;
            });
            $('#TornaSuDatiContestazione').click(function () {
                tornaSu = 1;
            });
            $('#DatiContestazione').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#DatiContestazioneContent').is(':visible');
                    $('#DatiContestazioneContent').slideToggle('fast');
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
            if (inserimento != "Sanzionata")
             {
                if (DataProtRispControdeduzioni =='') {
                    $('#DatiSanzioneContent').css('display', 'none');
                    $('#DatiSanzione').find("img").first().attr("src", pathPlus);
                    $('#DatiSanzione').find("img").first().attr("title", titlePathPlus);
                    }
                else
                    {  
                    $('#DatiSanzioneContent').css('display', 'yes');
                    $('#DatiSanzione').find("img").first().attr("src", pathMinus);
                    $('#DatiSanzione').find("img").first().attr("title", titlePathMinus);
                    }
                }
            else {
                $('#DatiSanzioneContent').css('display', 'yes');
                $('#DatiSanzione').find("img").first().attr("src", pathMinus);
                $('#DatiSanzione').find("img").first().attr("title", titlePathMinus);
            }

            $('#TornaSuDatiSanzione').click(function () {
                tornaSu = 1;
            });
            $('#DatiSanzione').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#DatiSanzioneContent').is(':visible');
                    $('#DatiSanzioneContent').slideToggle('fast');
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
            <legend>Verifica su Segnalazione DGSCN</legend>
            <div class="RigaVuota">
                <asp:HiddenField ID="txtIDDocInterno" runat="server" />
                <asp:HiddenField ID="txtIdEnte" runat="server" />
                <asp:HiddenField ID="txtIdAttivita" runat="server" />
                <asp:HiddenField ID="txtIdEnteSedeAttuazione" runat="server" />
                &nbsp;</div>
            <div class="wrapper"  style="width:100%;border:0px">
                <div class="headers">
                    <h2>
                        <asp:Label ID="lblTitolo" runat="server" Text="Verifica su Segnalazione DGSCN"></asp:Label>
                    </h2>
                </div>
                 <div class="RigaVuota">&nbsp;</div>
                <div class="rowGroup" style="height: auto">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label3" CssClass="label" AssociatedControlID="lblStatoVerifica" runat="server"
                            Text="Stato Verifica:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 15%">
                        <asp:Label ID="lblStatoVerifica" CssClass="labelDati" runat="server" />
                    </div>
                </div>
                <div class="RigaVuota">
                    &nbsp;</div>
                <div class="rowGroup" style="height: auto">
                    <asp:Label ID="lblErrore" runat="server" CssClass="msgErrore"></asp:Label>
                    <div class="RigaVuota">
                        &nbsp;</div>
                </div>
                <asp:Label ID="lblmessaggio" runat="server" CssClass="msgConferma"></asp:Label>
                <fieldset class="ContornoPagina">
                    <legend>Sezioni</legend>
                    <div class="row" style="height: auto">
                        <div id="flsInformazioniGeneraliToggler" class="colHyperLink" style="width: 100%">
                            <asp:LinkButton ID="link6" runat="server" Style="cursor: pointer" Text="Informazioni Generali"></asp:LinkButton>
                        </div>
                        <div id="flsDatiContestazioneToggler" class="colHyperLink" style="width: 100%">
                            <asp:LinkButton ID="link1" runat="server" Style="cursor: pointer" Text="Dati Contestazione"></asp:LinkButton>
                        </div>
                        <div id="flsDatiSanzioneToggler" class="colHyperLink" style="width: 100%">
                            <asp:LinkButton ID="LinkButton1" runat="server" Style="cursor: pointer" Text="Dati Sanzione"></asp:LinkButton>
                        </div>
                    </div>
                </fieldset>
                <asp:HiddenField ID="TxtCodiceFasc" runat="server" />
                <fieldset id="flsInformazioniGenerali" class="ContornoPagina">
                    <legend id="InformazioniGenerali">
                        <img src="images/plus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor: pointer;" />&nbsp;&nbsp;<img
                            id="TornaSuInformazioniGenerali" style="cursor: pointer;" onclick="TornaAdInizioPagina()"
                            src="images/tornasu_xsmall.png" title="Torna ad inizio pagina" alt="Torna ad inizio pagina" />&nbsp;&nbsp;Informazioni Generali</legend>
                    <div class="wrapper" id="InformazioniGeneraliContent" style="width: 100%; border: 0px">
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="lblCodiceFascicolo" CssClass="label" runat="server" Text="Numero Fascicolo"
                                    AssociatedControlID="TxtCodiceFascicolo"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 65%">
                                <asp:TextBox ID="TxtCodiceFascicolo" CssClass="textbox" runat="server"  />
                            </div>
                            <div class="collable" style="width: 20%">
                                <asp:ImageButton ID="cmdSelFascicolo" runat="server" Visible="true" OnClientClick="javascript: return ElencoFascicoli();"
                                    ToolTip="Seleziona Codice Fascicolo" AlternateText="Seleziona Codice Fascicolo"
                                    ImageUrl="Images/cartella_small.png" Width="30px"></asp:ImageButton>
                                <%--   --%>
                                <asp:ImageButton ID="cmdSelProtocollo0" runat="server" Visible="true" OnClientClick="javascript: return SelProtocollo0();"
                                    ImageUrl="Images/busta_small.png" ToolTip="Elenco Documenti" 
                                    AlternateText="Elenco Documenti" Width="30px">
                                </asp:ImageButton>
                                <asp:ImageButton ID="cmdFascCanc" runat="server" Visible="true" ImageUrl="Images/canc_small.png"
                                    ToolTip="Rimuovi Fascicolo/Protocolli" 
                                    AlternateText="Rimuovi Fascicolo/Protocolli" Width="30px">
                                </asp:ImageButton>
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblDescrFascicolo" CssClass="label" runat="server" Text="Descr. Fascicolo"
                                    AssociatedControlID="txtDescFasc" />
                            </div>
                            <div class="colOggetti" style="width: 65%">
                                <asp:TextBox ID="txtDescFasc" runat="server" CssClass="textbox" TextMode="MultiLine"
                                    Width="90%"  />
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblServizi" CssClass="label" runat="server" Text="Servizi" AssociatedControlID="ddlServizi" />
                            </div>
                            <div class="colOggetti" style="width: 65%">
                                <asp:DropDownList ID="ddlServizi" runat="server" CssClass="ddlClass" Width="90%" />
                                <!--<asp:TextBox ID="txtServizi" runat="server" CssClass="textbox"   Width="90%" Visible="False"></asp:TextBox>-->
                            </div>
                        </div>
                        
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="lblDaatSegnalazione" CssClass="label" runat="server" Text="Data Segnalazione" AssociatedControlID="txtDataSegnalazione" />
                            </div>
                            <div class="colOggetti">
                                  <asp:TextBox ID="txtDataSegnalazione" CssClass="textboxData" runat="server" />
                            </div>
                        </div>

                        <fieldset class="ContornoPagina">
                            <legend>Documento Interno</legend>
                           
                                <div class="row" >
                                    <div class="collable" style="width: 20%">
                                        <asp:Label ID="lblNumDoc" CssClass="label" runat="server" Text="N°. Doc. Interno"
                                            AssociatedControlID="txtNumDocInterno" />
                                    </div>
                                    <div class="colOggetti" style="width: 15%">
                                        <asp:TextBox ID="txtNumDocInterno" CssClass="textbox" runat="server"  />
                                    </div>
                                    <div class="collable" style="width: 20%">
                                        <asp:Label ID="lblNomeFile" CssClass="label" runat="server" Text="Nome File" AssociatedControlID="txtNomeFile" />
                                    </div>
                                    <div class="colOggetti" style="width: 15%">
                                        <asp:TextBox ID="txtNomeFile" CssClass="textbox" runat="server"  />
                                    </div>
                                    <div class="colOggetti">
                                        <asp:ImageButton ID="imgSelDoc" runat="server" ImageUrl="images/documento_small.png" AlternateText="Seleziona Documento" ToolTip="Seleziona Documento"
                                            OnClientClick="javascript: return SelDocumentoInterno('txtIDDocInterno','txtNumDocInterno','txtNomeFile','Documenti')"  Width="30px"></asp:ImageButton>
                                        <asp:ImageButton ID="imgDownolad" runat="server" ImageUrl="images/giu_small.png"
                                            ToolTip="Download File" AlternateText="Download File"  BorderStyle="none" 
                                            ></asp:ImageButton>
                                        <asp:HyperLink ID="hlScarica" runat="server" Visible="False" ImageUrl="Images/salva_small.png"
                                           BorderStyle="none" Target="_blank"></asp:HyperLink>
                                        <asp:ImageButton ID="imgCancDoc" runat="server" ImageUrl="images/canc_small.png"
                                            ToolTip="Rimuovi Documento Interno" AlternateText="Rimuovi Documento Interno" ></asp:ImageButton>
                                    </div>
                                </div>
                      
                        </fieldset>
                        <fieldset class="ContornoPagina">
                            <legend>Protocolli Segnalazione</legend>
                                <div class="row">
                                    <div class="collable" style="width: 25%">
                                        <asp:Label ID="LblDataProtSegnalazione" CssClass="label" runat="server" Text="Data Prot. Segnalazione"
                                            AssociatedControlID="txtDataProtSegnalazione" />
                                    </div>
                                    <div class="colOggetti" style="width: 15%">
                                        <asp:TextBox ID="txtDataProtSegnalazione" runat="server" CssClass="textboxData" >
                                        </asp:TextBox>
                                    </div>
                                    <div class="collable" style="width: 25%">
                                        <asp:Label ID="lblNumProtSegnalazione" CssClass="label" runat="server" Text="N° Prot. Segnalazione"
                                            AssociatedControlID="txtNumProtSegnalazione" />
                                    </div>
                                    <div class="colOggetti" style="width: 15%">
                                        <asp:TextBox ID="txtNumProtSegnalazione" runat="server" CssClass="textbox">  </asp:TextBox>
                                     </div>
                                     <div class="colOggetti" style="width: 15%">

                                     
                                     <asp:ImageButton ID="ImgProtolloRichiesta" runat="server"  AlternateText="Seleziona Protocollo"
                                            ImageUrl="Images/busta_small.png" ToolTip="Seleziona Protocollo"
                                            OnClientClick="javascript: return SelProtocolloProtSeg('txtNumProtSegnalazione','txtDataProtSegnalazione')"
                                            Width="30px"></asp:ImageButton>
                                         <asp:ImageButton  ID="ImgApriAllegatiRichiesta" runat="server" OnClientClick="javascript: return ApriAllegati('txtNumProtSegnalazione','txtDataProtSegnalazione')"  ImageUrl="images/documento_small.png" AlternateText="Visualizza Allegati" ToolTip="Visualizza Allegati"   />
                                        <asp:ImageButton ID="imcCancProtSeg" runat="server" ImageUrl="images/canc_small.png" AlternateText="Rimuovi Protocollo Segnalazione" 
                                            ToolTip="Rimuovi Protocollo Segnalazione" Width="30px"></asp:ImageButton>
                                   </div>
                                </div>
                                <div class="row">
                                    <div class="collable" style="width: 25%">
                                        <asp:Label ID="lblDataProtRicSegnalazione" CssClass="label" runat="server" Text="Data Prot. Ricezione Segnalazione"
                                            AssociatedControlID="txtDataProtRicSegnalazione" />
                                    </div>
                                    <div class="colOggetti" style="width: 15%">
                                        <asp:TextBox ID="txtDataProtRicSegnalazione" runat="server" CssClass="textboxData"></asp:TextBox>
                                    </div>
                                    <div class="collable" style="width: 25%">
                                        <asp:Label ID="lblNumProtRicSegnalazione" CssClass="label" runat="server" Text="N° Prot. Ricezione Segnalazione"
                                            AssociatedControlID="TxtNumProtRicSegnalazione" />
                                    </div>
                                    <div class="colOggetti" style="width: 15%">
                                        <asp:TextBox ID="TxtNumProtRicSegnalazione" runat="server" CssClass="textbox">   </asp:TextBox> 
                                    </div>
                                    <div class="colOggetti" style="width: 15%">
                                       <asp:ImageButton ID="ImgProtolloAcquisizione" runat="server" ImageUrl="Images/busta_small.png" AlternateText="Seleziona Protocollo" 
                                            ToolTip="Seleziona Protocollo" OnClientClick="javascript: return SelProtocolloProtSeg('TxtNumProtRicSegnalazione','txtDataProtRicSegnalazione')"
                                            Width="30px"></asp:ImageButton>
                                        <asp:ImageButton ID="ImgApriAllegatiAcquisizione" runat="server" ImageUrl="images/documento_small.png" AlternateText="Visualizza Allegati"
                                            ToolTip="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('TxtNumProtRicSegnalazione','txtDataProtRicSegnalazione')"/>
                                      
                                        <asp:ImageButton ID="imgCancProtRicSeg" runat="server" ImageUrl="images/canc_small.png" AlternateText="Rimuovi Protocollo Richiesta Segnalazione"
                                            ToolTip="Rimuovi Protocollo Richiesta Segnalazione" Width="30px"></asp:ImageButton>
                                    
                                </div>
                                </div>
                                <div class="row">
                                    <div class="collable" style="width: 25%">
                                        <asp:Label ID="lblDataProtApprovazione" CssClass="label" runat="server" Text="Data Prot. Approvazione Segnalazione"
                                            AssociatedControlID="TxtDataProtApprovazione" />
                                    </div>
                                    <div class="colOggetti" style="width: 15%">
                                        <asp:TextBox ID="TxtDataProtApprovazione" runat="server" CssClass="textboxData"></asp:TextBox>
                                    </div>
                                    <div class="collable" style="width: 25%">
                                        <asp:Label ID="lblNumProtApprovazione" CssClass="label" runat="server" Text="N° Prot. Approvazione Segnalazione"
                                            AssociatedControlID="TxtNumProtApprovazione" />
                                    </div>
                                    <div class="colOggetti" style="width: 15%">
                                        <asp:TextBox ID="TxtNumProtApprovazione" runat="server" CssClass="textbox"> </asp:TextBox> 
                                    </div>
                                     <div class="colOggetti" style="width: 15%">
                                        <asp:ImageButton ID="ImgProtolloApprovazione" runat="server" 
                                            ImageUrl="Images/busta_small.png" AlternateText="Seleziona Protocollo"
                                            ToolTip="Seleziona Protocollo" OnClientClick="javascript: return SelProtocolloProtSeg('TxtNumProtApprovazione','TxtDataProtApprovazione')"
                                            Width="30px"></asp:ImageButton>
                                        <asp:ImageButton ID="ImgApriAllegatiApprovazione" runat="server" ImageUrl="images/documento_small.png"  AlternateText="Visualizza Allegati" 
                                            ToolTip="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('TxtNumProtApprovazione','TxtDataProtApprovazione')"/>
                                     
                                        <asp:ImageButton ID="imgCancProtAppSeg" runat="server" ImageUrl="images/canc_small.png" AlternateText="Rimuovi Protocollo Approvazione Segnalazione" 
                                            ToolTip="Rimuovi Protocollo Approvazione Segnalazione" Width="30px"></asp:ImageButton>
                                    </div>
                                </div>
                       
                        </fieldset>
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblEnte" CssClass="label" runat="server" Text="Ente" AssociatedControlID="txtEnte"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 85%">
                                <asp:TextBox ID="txtEnte" CssClass="textbox" runat="server"  TextMode="MultiLine"
                                    Style="width: 80%" />
                                <asp:ImageButton ID="imgRicercaEnte"  runat="server" ImageUrl="images/ente_small.png"
                                    OnClientClick="javascript: return fnimgRicercaEnte()" Width="30px" ToolTip="Ricerca Ente" AlternateText="Ricerca Ente" 
                                    BorderStyle="Outset" BorderWidth="0px" ></asp:ImageButton>
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblProgetto" CssClass="label" runat="server" Text="Progetto" AssociatedControlID="txtProgetto"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 85%">
                                <asp:TextBox ID="txtProgetto" CssClass="textbox" runat="server"  TextMode="MultiLine"
                                    Style="width: 80%" />
                                <asp:ImageButton ID="ImgRicercaProgetto"  runat="server" ImageUrl="Images/Icona_Progetto_small.png"
                                    OnClientClick=" return fnImgRicercaProgetto();" Width="30px" AlternateText="Ricerca Progetto"
                                    ToolTip="Ricerca Progetto" BorderStyle="Outset" BorderWidth="0px">
                                </asp:ImageButton>
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblSede" CssClass="label" runat="server" Text="Sede di Progetto" AssociatedControlID="txtSede"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 85%">
                                <asp:TextBox ID="txtSede" CssClass="textbox" runat="server"  TextMode="MultiLine"
                                    Style="width: 80%" />
                                <asp:ImageButton ID="imgRicercaSede"  runat="server" ImageUrl="images/sedi_small.png" AlternateText="Ricerca Sede di Progetto"
                                    OnClientClick="javascript: return fnimgRicercaSede()" Width="30px" ToolTip="Ricerca Sede di Progetto"
                                    BorderStyle="Outset" BorderWidth="0px" Enabled="true"></asp:ImageButton>
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblEnteFiglio" CssClass="label" runat="server" Text="Ente Partner"
                                    AssociatedControlID="TxtEnteFiglio"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 85%">
                                <asp:TextBox ID="TxtEnteFiglio" CssClass="textbox" runat="server" 
                                    TextMode="MultiLine" Style="width: 80%" />
                            </div>
                        </div>
                    </div>
                </fieldset>
                <fieldset id="flsDatiContestazione" class="ContornoPagina">
                    <legend id="DatiContestazione"><img  src="images/plus_xsmall.png" alt="Espandi/Riduci" style="cursor:pointer;"/>&nbsp;&nbsp;
                        <img id="TornaSuDatiContestazione" style="cursor: pointer;" onclick="TornaAdInizioPagina()"
                            src="images/tornasu_xsmall.png" title="Torna ad inizio pagina" alt="Torna ad inizio pagina" />&nbsp;&nbsp;Dati
                        Contestazione</legend>
                    <div class="wrapper" id="DatiContestazioneContent" style="width: 100%; border: 0px">
                        <div class="row" id="dvDGContestazione" runat="server">
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblDataProtLetteraContestazioneDG" CssClass="label" runat="server"
                                    Text="Data Prot. Trasmissiome Contest. al D.G.:" AssociatedControlID="txtDataProtLetteraContestazioneDG"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="txtDataProtLetteraContestazioneDG" runat="server" CssClass="textboxData"
                                    ></asp:TextBox>
                            </div>
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblNProtLetteraContestazioneDG" CssClass="label" runat="server" Text="N° Prot. Trasmissiome Contest. al D.G.:"
                                    AssociatedControlID="txtNProtLetteraContestazioneDG"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 20%">
                                <asp:TextBox ID="txtNProtLetteraContestazioneDG" runat="server" CssClass="textbox"
                                     />
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:ImageButton ID="ImgProtocolloTrasContestataDG" runat="server" ImageUrl="Images/busta_small.png"
                                    ToolTip="Seleziona Protocollo"  AlternateText="Seleziona Protocollo"   OnClientClick="javascript: return SelProtocollo('txtNProtLetteraContestazioneDG','txtDataProtLetteraContestazioneDG')"
                                   Width="30px"></asp:ImageButton>
                                <asp:ImageButton ID="ImgApriAllegatiTrasContestataDG" runat="server" ImageUrl="images/documento_small.png"
                                    ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati"  OnClientClick="javascript: return ApriAllegati('txtNProtLetteraContestazioneDG','txtDataProtLetteraContestazioneDG')"
                                   Width="30px"></asp:ImageButton>
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblDataProtocolloInvioLetteraContestazione" CssClass="label" runat="server"
                                    Text="Data Prot. Invio Contestazione:" AssociatedControlID="txtDataProtocolloInvioLetteraContestazione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="txtDataProtocolloInvioLetteraContestazione" runat="server" CssClass="textboxData"
                                    ></asp:TextBox>
                            </div>
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblNumeroProtocolloInvioLettContestazione" CssClass="label" runat="server"
                                    Text="N° Prot. Invio Lettera Contestazione:" AssociatedControlID="TxtNumeroProtocolloInvioLettContestazione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 20%">
                                <asp:TextBox ID="TxtNumeroProtocolloInvioLettContestazione" runat="server" CssClass="textbox"
                                     />
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:ImageButton ID="ImgProtocolloInvioLettContestazione" runat="server" ImageUrl="Images/busta_small.png"
                                    ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo"  OnClientClick="javascript: return SelProtocollo('TxtNumeroProtocolloInvioLettContestazione','txtDataProtocolloInvioLetteraContestazione')"
                                     Width="30px"></asp:ImageButton>
                                <asp:ImageButton ID="ImgApriAllegatiInvioLettContestazione" runat="server" ImageUrl="images/documento_small.png"
                                    ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('TxtNumeroProtocolloInvioLettContestazione','txtDataProtocolloInvioLetteraContestazione')"
                                     Width="30px"></asp:ImageButton>
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblDataRicezioneLetteraContestazione" CssClass="label" runat="server"
                                    Text="Data Ricezione Lettera Contestazione:" AssociatedControlID="txtDataRicezioneLetteraContestazione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="txtDataRicezioneLetteraContestazione" runat="server" CssClass="textboxData"> </asp:TextBox>
                            </div>
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblDataRispostaLetteraContestazione" CssClass="label" runat="server" Visible="false"
                                    Text="Data Lettera Controdeduzioni:" AssociatedControlID="txtDataRispostaLetteraContestazione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 20%">
                                <asp:TextBox ID="txtDataRispostaLetteraContestazione" runat="server" CssClass="textbox" Visible="false"> </asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblDataProtocolloRispostaContestazione" CssClass="label" runat="server"
                                    Text="Data Prot. Lettera Controdeduzioni:" AssociatedControlID="txtDataProtocolloRispostaContestazione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="txtDataProtocolloRispostaContestazione" runat="server" CssClass="textboxData"
                                    ></asp:TextBox>
                            </div>
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblNumProtocolloRispostaContestazione" CssClass="label" runat="server"
                                    Text="N° Prot. Lettera Controdeduzioni:" AssociatedControlID="TxtNumProtocolloRispostaContestazione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 20%">
                                <asp:TextBox ID="TxtNumProtocolloRispostaContestazione" runat="server" CssClass="textbox"></asp:TextBox>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:ImageButton ID="ImgProtocolloRispContestazione" runat="server" ImageUrl="Images/busta_small.png"
                                    ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('TxtNumProtocolloRispostaContestazione','txtDataProtocolloRispostaContestazione')"
                                     Width="30px"></asp:ImageButton>
                                <asp:ImageButton ID="ImgApriAllegatiRispContestazione" runat="server" ImageUrl="images/documento_small.png"
                                    ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('TxtNumProtocolloRispostaContestazione','txtDataProtocolloRispostaContestazione')"
                                    ></asp:ImageButton>
                      
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblDataProtChiusuraContestazione" CssClass="label" runat="server"
                                    Text="Data Prot. Chiusura Contestazione:" AssociatedControlID="TxtDataProtChiusuraContestazione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="TxtDataProtChiusuraContestazione" runat="server" CssClass="textboxData"
                                    ></asp:TextBox>
                            </div>
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblNumProtChiusuraContestazione" CssClass="label" runat="server" Text="N° Prot. Chiusura Contestazione:"
                                    AssociatedControlID="TxtNumProtChiusuraContestazione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 20%">
                                <asp:TextBox ID="TxtNumProtChiusuraContestazione" runat="server" CssClass="textbox"
                                     />
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:ImageButton ID="ImgProtocolloProtChiusuraContestazione" runat="server" ImageUrl="Images/busta_small.png"
                                    ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('TxtNumProtChiusuraContestazione','TxtDataProtChiusuraContestazione')"
                                    Width="30px"></asp:ImageButton>
                                <asp:ImageButton ID="ImgApriAllegatiProtChiusuraContestazione" runat="server" ImageUrl="images/documento_small.png"
                                    ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati"  OnClientClick="javascript: return ApriAllegati('TxtNumProtChiusuraContestazione','TxtDataProtChiusuraContestazione')"
                                    ></asp:ImageButton>
                    
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 20%">
                                <asp:Label ID="Label22" CssClass="label" runat="server" Text="Lettera Contestaz. Add"
                                    AssociatedControlID="cmdContestazione" />
                            </div>
                            <div class="colOggetti" style="width: 10%">
                                <asp:ImageButton ID="cmdContestazione" runat="server" ToolTip="Lettera Contestazione Addebiti" AlternateText="Lettera Contestazione Addebiti"
                                    ImageUrl="images/stampa_small.png"></asp:ImageButton>
                            </div>
                            <div class="collable" style="width: 20%">
                                <asp:Label ID="Label24" CssClass="label" runat="server" Text="Lettera Trasm D G e Contes." 
                                    AssociatedControlID="CmdLetteraTrasm" />
                            </div>
                            <div class="colOggetti" style="width: 10%">
                                <asp:ImageButton ID="CmdLetteraTrasm" runat="server" ToolTip="Lettera Trasmissione DG Contestazione" AlternateText="Lettera Trasmissione DG Contestazione"
                                    ImageUrl="images/stampa_small.png"></asp:ImageButton>
                            </div>
                            <div class="collable" style="width: 20%">
                                <asp:Label ID="Label25" CssClass="label" runat="server" Text="Lettera Chiusura Contestazione"
                                    AssociatedControlID="CmdLetteraTrasm" />
                            </div>
                            <div class="colOggetti" style="width: 10%">
                                <asp:ImageButton ID="CmdLetteraChiusContestazione" runat="server" ToolTip="Lettera Chiusura Contestazione" AlternateText="Lettera Chiusura Contestazione"
                                    ImageUrl="images/stampa_small.png"></asp:ImageButton>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <fieldset id="flsDatiSanzione" class="ContornoPagina">
                    <legend id="DatiSanzione">
                       <img src="images/plus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor: pointer;" />&nbsp;&nbsp; <img
                            id="TornaSuDatiSanzione" style="cursor: pointer;" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"
                            title="Torna ad inizio pagina" alt="Torna ad inizio pagina" />&nbsp;&nbsp;Dati
                        Sanzione</legend>
                    <div class="wrapper" id="DatiSanzioneContent" style="width: 100%; border: 0px">
                        <div class="row" id="dvDGSanzione" runat="server">
                            <div class="collable" style="width: 25%" >
                                <asp:Label ID="LblDataProtocolloTrasmissioneSanzione" CssClass="label" runat="server"
                                    Text="Data Protocollo Trasmissione Sanzione al D.G.:" AssociatedControlID="txtDataProtocolloTrasmissioneSanzione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="txtDataProtocolloTrasmissioneSanzione" runat="server" CssClass="textboxData"
                                    ></asp:TextBox>
                            </div>
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblNumeroProtocolloTrasmissioneSanzione" CssClass="label" runat="server"
                                    Text="N° Protocollo Trasmissione Sanzione al D.G.:" AssociatedControlID="TxtNumeroProtocolloTrasmissioneSanzione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 20%">
                                <asp:TextBox ID="TxtNumeroProtocolloTrasmissioneSanzione" runat="server" CssClass="textbox"
                                     />
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:ImageButton ID="ImgProtocolloTrasmissioneSanzione" runat="server" ImageUrl="Images/busta_small.png"
                                    ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('TxtNumeroProtocolloTrasmissioneSanzione','txtDataProtocolloTrasmissioneSanzione')"
                                    ></asp:ImageButton>
                                <asp:ImageButton ID="ImgApriAllegatiTrasmissioneSanzione" runat="server" ImageUrl="images/documento_small.png"
                                    ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('TxtNumeroProtocolloTrasmissioneSanzione','txtDataProtocolloTrasmissioneSanzione')"
                                     Width="30px"></asp:ImageButton>
                          
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblDataProtocolloEsecuzioneSanzione" CssClass="label" runat="server"
                                    Text="Data Protocollo Invio Sanzione:" AssociatedControlID="txtDataProtocolloEsecuzioneSanzione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="txtDataProtocolloEsecuzioneSanzione" runat="server" CssClass="textboxData"></asp:TextBox>
                            </div>
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblNumeroProtocolloEsecuzioneSanzione" CssClass="label" runat="server"
                                    Text="N° Prot. Invio Sanzione:" AssociatedControlID="TxtNumeroProtocolloEsecuzioneSanzione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 20%">
                                <asp:TextBox ID="TxtNumeroProtocolloEsecuzioneSanzione" runat="server" CssClass="textbox"
                                     />
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:ImageButton ID="ImgProtocolloEsecuzioneSanzione" runat="server" ImageUrl="Images/busta_small.png"
                                    ToolTip="Seleziona Protocollo"  AlternateText="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('TxtNumeroProtocolloEsecuzioneSanzione','txtDataProtocolloEsecuzioneSanzione')"
                                    ></asp:ImageButton>
                                <asp:ImageButton ID="ImgApriAllegatiEsecuzioneSanzione" runat="server" ImageUrl="images/documento_small.png"
                                    ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('TxtNumeroProtocolloEsecuzioneSanzione','txtDataProtocolloEsecuzioneSanzione')"
                                     Width="30px"></asp:ImageButton>
                        
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblDataProtocolloTrasmServizi" CssClass="label" runat="server" Text="Data Protocollo Trasmissione Servizi:"
                                    AssociatedControlID="txtDataProtocolloTrasmServizi"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="txtDataProtocolloTrasmServizi" runat="server" CssClass="textboxData"
                                    ></asp:TextBox>
                            </div>
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblNProtocolloTrasmServizi" CssClass="label" runat="server" Text="N° Prot. Trasmissione Servizi:"
                                    AssociatedControlID="txtNProtocolloTrasmServizi"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 20%">
                                <asp:TextBox ID="txtNProtocolloTrasmServizi" runat="server" CssClass="textbox"  />
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:ImageButton ID="ImgProtocolloTrasmServizi" runat="server" ImageUrl="Images/busta_small.png"
                                    ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('txtNProtocolloTrasmServizi','txtDataProtocolloTrasmServizi')"
                                    ></asp:ImageButton>
                                <asp:ImageButton ID="ImgApriAllegatiTrasmServizi" runat="server" ImageUrl="images/documento_small.png"
                                    ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('txtNProtocolloTrasmServizi','txtDataProtocolloTrasmServizi')"
                                     Width="30px"></asp:ImageButton>
                 
                                 <asp:ImageButton ID="imgSanzioneServizi" Visible="True" ImageUrl="images/Icona_Volontario_small.png"
                                    runat="server" Width="30px" ToolTip="Sanzione Servizi" AlternateText="Sanzione Servizi"  OnClientClick="javascript: return Servizi()"
                                    ></asp:ImageButton>
                                 
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblDataEsecuzioneSanzione" CssClass="label" runat="server" Text="Data Esecuzione Sanzione:"
                                    AssociatedControlID="txtDataEsecuzioneSanzione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="txtDataEsecuzioneSanzione" runat="server" CssClass="textboxData"> </asp:TextBox>
                            </div>
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblUfficioUnsc" CssClass="label" runat="server" Text="Ufficio UNSC:"
                                    AssociatedControlID="ddlUfficio"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 30%">
                                <asp:DropDownList ID="ddlUfficio" runat="server" CssClass="ddlClass">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblCompetenza" CssClass="label" runat="server" Text="Competenza:"
                                    AssociatedControlID="ddlCompetenze"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:DropDownList ID="ddlCompetenze" runat="server" CssClass="ddlClass">
                                </asp:DropDownList>
                            </div>
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblSanzione" CssClass="label" runat="server" Text="Sanzioni:" AssociatedControlID="lkbSanzione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 30%">
                                <!-- <asp:ImageButton ID="cmdSanzioneq" Style="cursor: hand" runat="server" ToolTip="Applica Sanzione"
                                    BorderWidth="1px" BorderStyle="Outset" BorderColor="#6699FF" ImageUrl="images/accorpa_progetto.jpg"
                                    Enabled="False"></asp:ImageButton>-->
                                 <asp:LinkButton ID="lkbSanzione" Text="Applica Sanzione" style="cursor:pointer" runat="server"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 20%">
                                <asp:Label ID="Label26" CssClass="label" runat="server" Text="Trasm DG Sanzione"
                                    AssociatedControlID="CmdArchiviazione" />
                            </div>
                            <div class="colOggetti" style="width: 5%">
                                <asp:ImageButton ID="CmdArchiviazione" runat="server" ToolTip="Trasmissione D.G. Sanzione" AlternateText="Trasmissione D.G. Sanzione"
                                    ImageUrl="images/stampa_small.png"></asp:ImageButton>
                            </div>
                            <div class="collable" style="width: 20%">
                                <asp:Label ID="Label27" CssClass="label" runat="server" Text="Trasm Provv Sanz."
                                    AssociatedControlID="CmdLetteraTrasmissioneProvv" />
                            </div>
                            <div class="colOggetti" style="width: 5%">
                                <asp:ImageButton ID="CmdLetteraTrasmissioneProvv" runat="server" ToolTip="Lettera Trasmissione Provvedimento Sanzionatorio" AlternateText="Lettera Trasmissione Provvedimento Sanzionatorio"
                                    ImageUrl="images/stampa_small.png"></asp:ImageButton>
                            </div>
                            <div class="collable" style="width: 20%">
                                <asp:Label ID="Label28" CssClass="label" runat="server" Text="Trasm. Provv. Serv."
                                    AssociatedControlID="CmdLETTERATRASMISPROVAISERVIZI" />
                            </div>
                            <div class="colOggetti" style="width: 5%">
                                <asp:ImageButton ID="CmdLETTERATRASMISPROVAISERVIZI" runat="server" ToolTip="Lettera Trasmissione ai Servizi" AlternateText="Lettera Trasmissione ai Servizi"
                                    ImageUrl="images/stampa_small.png"></asp:ImageButton>
                            </div>
                            <div class="collable" style="width: 20%">
                                <asp:Label ID="Label29" CssClass="label" runat="server" Text="Trasm. Provv. Reg."
                                    AssociatedControlID="CmdTRASMISSIONEPROVVEDIMENTOREGIONE" />
                            </div>
                            <div class="colOggetti" style="width: 5%">
                                <asp:ImageButton ID="CmdTRASMISSIONEPROVVEDIMENTOREGIONE" runat="server" ToolTip="Lettera Trasmissione Provvedimento Regione" AlternateText="Lettera Trasmissione Provvedimento Regione"
                                    ImageUrl="images/stampa_small.png"></asp:ImageButton>
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 20%">
                                <asp:Label ID="Label30" CssClass="label" runat="server" Text="Diffida" AssociatedControlID="cmdDiffida" />
                            </div>
                            <div class="colOggetti" style="width: 5%">
                                <asp:ImageButton ID="cmdDiffida" runat="server"  AlternateText="Diffida" ToolTip="Diffida" ImageUrl="images/stampa_small.png">
                                </asp:ImageButton>
                            </div>
                            <div class="collable" style="width: 20%">
                                <asp:Label ID="Label31" CssClass="label" runat="server"  Text="Revoca" AssociatedControlID="CmdRevoca" />
                            </div>
                            <div class="colOggetti" style="width: 5%">
                                <asp:ImageButton ID="CmdRevoca" runat="server"  AlternateText= "Revoca" ToolTip="REVOCA" ImageUrl="images/stampa_small.png">
                                </asp:ImageButton>
                            </div>
                            <div class="collable" style="width: 20%">
                                <asp:Label ID="Label32" CssClass="label" runat="server" Text="Interdizione" AssociatedControlID="CmdInterdizione" />
                            </div>
                            <div class="colOggetti" style="width: 5%">
                                <asp:ImageButton ID="CmdInterdizione" runat="server" ToolTip="Interdizione per un anno a presentare Progetti" AlternateText="Interdizione per un anno a presentare Progetti"
                                    ImageUrl="images/stampa_small.png"></asp:ImageButton>
                            </div>
                            <div class="collable" style="width: 20%">
                                <asp:Label ID="Label33" CssClass="label" runat="server" Text="Cancellazione" AssociatedControlID="CmdCancellazione" />
                            </div>
                            <div class="colOggetti" style="width: 5%">
                                <asp:ImageButton ID="CmdCancellazione" runat="server" ToolTip="Cancellazione" AlternateText="Cancellazione" ImageUrl="images/stampa_small.png">
                                </asp:ImageButton>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <div class="row" id="div11" runat="server">
                    <div class="collable" style="width: 20%">
                        <asp:Label ID="Label1" CssClass="label" runat="server" Text="NOTE:" AssociatedControlID="TxtNote" />
                    </div>
                </div>
                <div class="row" id="div12" runat="server">
                    <div class="collable" style="width: 100%">
                        <asp:TextBox ID="TxtNote" CssClass="textbox" TextMode="MultiLine" runat="server"
                            Width="98%" />
                    </div>
                </div>
                <div class="row" id="div13" runat="server">
                    <br />
                </div>
               
                    <div class="RigaPulsanti">
                        <asp:Button ID="cmdStampa" CssClass="Pulsante" runat="server" Text="Stampa Progetto"
                            OnClientClick="return fncmdStampa();"></asp:Button>
                        <asp:Button ID="cmdChiusaContestata" CssClass="Pulsante" runat="server" Text="Chiusa Contestata">
                        </asp:Button>
                        <asp:Button ID="cmdAnnullata" CssClass="Pulsante" runat="server" 
                        Text="Annulla Verifica" Visible="False"></asp:Button>
                        <asp:Button ID="cmdSospendi" CssClass="Pulsante" runat="server" 
                            Text="Sospendi Verifica">
                        </asp:Button>
                        <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" Text="Salva" />
                        <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"></asp:Button>
                    </div>
          
            </div>
        </fieldset>
    </div>
    <asp:TextBox ID="txtIdVerifica" runat="server" Width="80px" Visible="False"></asp:TextBox>
   
</asp:Content>
