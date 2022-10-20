<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="verificarequisiti.aspx.vb" Inherits="Futuro.verificarequisiti" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>

<%--<style>
    
 input:disabled 
{
    -webkit-filter: opacity(30%);  
    filter: opacity(30%);
}


input[type="image"]:disabled {
   -webkit-filter: opacity(30%); /* Chrome, Safari, Opera */
    filter: opacity(30%);
}      
           
  </style>         --%>

  <script type="text/javascript">
    /* <![CDATA[ */
        var prefissoIdClient = "MainContent_";

        function CostruisciId(IdServer) {
            var IdClient = prefissoIdClient + IdServer;
            return IdClient;
        };
  
          /* ]]> */

    </script>
    <script type="text/javascript">
//           function fncmdStampa()
//                {
//	                myWin = window.open ('WfrmReportistica.aspx?sTipoStampa=30&IdVerifica=' + <%=Request.QueryString("IdVerifica")%>,'Report','height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes')
//	                return false;
//                }
			function Verificatore()
				{
				var IdVerifica;
				var IdVerificatore;
				IdVerifica='<%=request.querystring("IdVerifica")%>';			
				IdVerificatore='<%=txtIdVerificatore.Value%>';

				mywin=window.open("Ver_AssegnaVerificatoriSupp.aspx?IdVerificatore=" + IdVerificatore + "&IdVerifica=" + IdVerifica,"","height=800,width=800,dependent=no,scrollbars=yes,status=no,resizable=yes");	
                  return false;
				}
 
          
    </script>
    <script type="text/javascript">
        function Replace(cod) {
            var str = cod; //"2011#FASC#1018/2011"; 
            var appo;
            //var appo2 ;
            for (var i = 0; i < str.length; i++) {
                appo = str.substring(i, 1); //<BR> mid(i,str[i];
                if (appo = !'#')

                { }
                else {
                    str = str.replace('#', '%23')
                }
            }
            return str //str.replace("#", "%23"); 
        }
				
    </script>
    <script language="JavaScript" type="text/JavaScript">
        function SelProtocollo0() {

            if ('<%= hfTxtCodiceFasc.value %>' == '') {
                alert("Specificare il numero fascicolo!");

            }
            else {
                var codFasc;
                codFasc = Replace('<%= hfTxtCodiceFasc.value %>');

                myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?NumeroFascicolo=" + codFasc, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");
            }
            return false;

        }
    </script>
    <script language="JavaScript" type="text/JavaScript">

        function ElencoFascicoli() {

            var winOption = PopUpOption();
            var IdTxtCodiceFascicolo = CostruisciId("TxtCodiceFascicolo");
            var IdTxtCodiceFasc = CostruisciId("hfTxtCodiceFasc");
            var IdtxtDescFasc = CostruisciId("txtDescFasc");
            var codiceFascicolo = document.getElementById(IdTxtCodiceFascicolo).value;
            var NumFascicolo = '<%= TxtCodiceFascicolo.Text %>';
            //var codiceFascicolo =        
            
            formId = document.forms[0];

            myWin = window.open("WfrmSIGEDElencoFascicoli.aspx?Processo=MONITORAGGIO&objForm=" + formId + "&objNumero=" + IdTxtCodiceFascicolo + "&objCodice=" + IdTxtCodiceFasc + "&objDescFasc=" + IdtxtDescFasc + "&IdFascicolo=" + codiceFascicolo, "Visualizza", winOption);

            //"width=800,height=350,dependent=no,scrollbars=yes,status=no");
            return false;

        }
    </script>
    
    <script language="JavaScript" type="text/JavaScript">
			function ApriAllegati(NumProt,DataProt)
			{
                   var IdFascicolo =  $('#<%= hfTxtCodiceFasc.ClientID %>').val();              
                   var idTxtCodiceFascicolo = $('#<%= TxtCodiceFascicolo.ClientID %>').val();   
                
					if (  DataProt  == ''){
							alert ("Specificare il Protocollo!");						 
					}else{	              					
							myWin = window.open("WfrmSIGEDElencoAllegati.aspx?NumeroProtocollo=" + document.getElementById(CostruisciId(NumProt)).value   + "&DataProtocollo=" + document.getElementById(CostruisciId(DataProt)).value  + "&CodiceFascicolo=" + IdFascicolo,"Visualizza","width=800,height=350,dependent=no,scrollbars=yes,status=no");							
					}
			}
			
			function SelProtocollo(NumProt,DataProt)
			{
			 
             
            var IdTxtCodiceFascicolo = CostruisciId("TxtCodiceFascicolo");
            var codFasc =  $('#<%= hfTxtCodiceFasc.ClientID %>').val();  
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
 
						myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?ChiudiContestata=" + ChiudiContestata + "&objForm=Form1&TxtProt="+CostruisciId(NumProt)+"&TxtData="+CostruisciId(DataProt)+"&NumeroFascicolo=" + codFasc, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");			
					}
                      return false;

			}

			function Protocollazione(NumProt,DataProt,IdIGF,DR,SanzServ)
			{
				var IdVerifica;
				var codFasc;
				IdVerifica='<%=request.querystring("IdVerifica")%>';				
				codFasc =  Replace('<%= hfTxtCodiceFasc.value %>');
			
				if (document.getElementById(CostruisciId(NumProt)).style.backgroundColor!='lightgrey')
					{
					if (document.getElementById(CostruisciId(NumProt)).value=='' || document.getElementById(CostruisciId(DataProt)).value=='')
					{
						myWin = window.open("WfrmSIGEDProtocollazione.aspx?CorrCodUnivoco=<%= Session("pCodEnte")%>&Processo=MONITORAGGIO&IdVerifica=" + IdVerifica + "&SanzServ="+parseInt(SanzServ)+"&DR="+parseInt(DR)+"&IdIGF="+parseInt(IdIGF)+"&objForm=Form1&TxtFasc=TxtCodiceFasc&TxtCodFasc=TxtCodiceFascicolo&TxtDescFascicolo=txtDescFasc&TxtProt="+NumProt+"&TxtData="+DataProt+"&Fascicolo=" + Form1.TxtCodiceFascicolo.value+"&NumeroFascicolo=" + codFasc+"&DescFascicolo=" + Form1.txtDescFasc.value + "&CodEnte=<%= Session("pCodEnte")%>", "Visualizza","width=800,height=400,dependent=no,scrollbars=yes,status=no");			
						
					}
					else
					{
						var msg;
						msg=confirm("Si vuole inserire il documento Principale?");
						if (msg==false)
						{
							
							myWin = window.open("WfrmSIGEDProtocollazione.aspx?CorrCodUnivoco=<%= Session("pCodEnte")%>&Processo=MONITORAGGIO&IdVerifica=" + IdVerifica + "&SanzServ="+parseInt(SanzServ)+"&DR="+parseInt(DR)+"&IdIGF="+parseInt(IdIGF)+"&objForm=Form1&TxtFasc=TxtCodiceFasc&TxtCodFasc=TxtCodiceFascicolo&TxtDescFascicolo=txtDescFasc&TxtProt="+NumProt+"&TxtData="+DataProt+"&Fascicolo=" + Form1.TxtCodiceFascicolo.value+"&NumeroFascicolo=" + codFasc+"&DescFascicolo=" + Form1.txtDescFasc.value + "&CodEnte=<%= Session("pCodEnte")%>", "Visualizza","width=800,height=400,dependent=no,scrollbars=yes,status=no");			
							return false;
						}else
						{	
							myWin = window.open("WfrmSIGEDDocumentoPrincipale.aspx?NumProt=" + document.getElementById(CostruisciId(NumProt)).value + "&DataProt=" + document.getElementById(CostruisciId(DataProt)).value, "Visualizza", "width=400,height=200,dependent=no,scrollbars=yes,status=no");			
						}
					}							
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

    </script>
    <script language="JavaScript" type="text/JavaScript">
           
    </script>
    <script type="text/javascript">
       // if ('<%=Session("TipoUtente")="R"%>')
        //{ 
           /* <![CDATA[ */
            $(function () {
                var clientId = "#" + CostruisciId('txtDataProtCredenziali');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('txtDataInizioVerifica');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('txtDataFineVerifica');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('txtDataProtocolloIncarico');
                    $("" + clientId + "").datepicker();
            });

               $(function () {
                var clientId = "#" + CostruisciId('txtDataProtocolloRelazione');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('TxtDataProtTrasDG');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('txtDataProtocolloInvioChiusura');
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
               $(function () {
                var clientId = "#" + CostruisciId('txtDataProtCredenziali');
                    $("" + clientId + "").datepicker();
            });
                
          /* ]]> */
           // }
    </script>

  <script type="text/javascript">
      /* <![CDATA[ */
        var pathPlus = "images/plus_xsmall.png";
        var pathMinus = "images/minus_xsmall.png";
        var titlePathMinus = "Riduci";
        var titlePathPlus = "Espandi";
        var inserimento1 = '';
        var inserimento = '<%=lblStatoVerifica.Text%>';
        var DataRelazione = '<%=txtDataProtocolloRelazione.Text%>';
        var DataProtRispControdeduzioni='<%=txtDataProtocolloRispostaContestazione.Text%>';
      
        /* ]]> */  
    </script>
    <script type="text/javascript">
       /* <![CDATA[ */
        $(function () {
            var tornaSu = 0;
            if (inserimento1  != "") {
                $('#InformazioniGeneraliContent').css('display', 'none');
                $('#InformazioniGenerali').find("img").first().attr("src", pathPlus);
                $('#InformazioniGenerali').find("img").first().attr("title", titlePathPlus);
            }
            else {
                $('#InformazioniGeneraliContent').css('display', 'yes');
                $('#InformazioniGenerali').find("img").first().attr("src", pathMinus);
                $('#InformazioniGenerali').find("img").first().attr("title", titlePathMinus);
            }

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
     
            var tornaSu = 0;

            if (inserimento != "Contestata") {
                if (DataRelazione =='') {
                    //alert('RELAZIONE VUOTA');
                    $('#DatiContestazioneContent').css('display', 'none');
                    $('#DatiContestazione').find("img").first().attr("src", pathPlus);
                    $('#DatiContestazione').find("img").first().attr("title", titlePathPlus);
                    }
                else
                
                    {//alert('RELAZIONE PIENA');
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
            if (inserimento != "Sanzionata") {
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
            <legend>Verifica Requisiti</legend>
            <div class="RigaVuota">
                &nbsp;</div>
            <div class="wrapper" style="width: 100%">

             <div class="headers">
                    <h2>
                        <asp:Label ID="lblTitolo" runat="server" Text="Verifica Requisiti"></asp:Label>
                        </h2>
                </div>
                  <div class="RigaVuota">
                &nbsp;</div>
                <div class="row" id="div1" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label1" CssClass="label" AssociatedControlID="lblProgrammazione" runat="server"
                            Text="Programmazione:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 25%">
                        <asp:Label ID="lblProgrammazione" CssClass="labelDati" runat="server" />
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label23" CssClass="label" AssociatedControlID="lblIspettore" runat="server"
                            Text="Verificatore:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 15%">
                        <asp:Label ID="lblIspettore" CssClass="labelDati" runat="server" />
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label3" CssClass="label" AssociatedControlID="lblStatoVerifica" runat="server"
                            Text="Stato Verifica:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 15%">
                        <asp:Label ID="lblStatoVerifica" CssClass="labelDati" runat="server" />
                    </div>
                </div>
                <div class="row" id="div2" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label4" CssClass="label" AssociatedControlID="lblEnte" runat="server"
                            Text="Ente Proponente:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 85%">
                        <asp:Label ID="lblEnte" CssClass="labelDati" runat="server" />
                    </div>
                </div>
                <div class="row" id="div3" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label5" CssClass="label" AssociatedControlID="LblDatiEnte" runat="server"
                            Text="Dati Ente:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 85%">
                        <asp:Label ID="LblDatiEnte" CssClass="labelDati" runat="server" />
                    </div>
                </div>
                <div class="row" id="div4" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label6" CssClass="label" AssociatedControlID="lblProgetto" runat="server"
                            Text="Progetto:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 85%">
                        <asp:Label ID="lblProgetto" CssClass="labelDati" runat="server" />
                    </div>
                </div>
                <div class="row" id="div5" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label2" CssClass="label" AssociatedControlID="lblDataInizioProgetto"
                            runat="server" Text="Data Inizio Progetto:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 25%">
                        <asp:Label ID="lblDataInizioProgetto" CssClass="labelDati" runat="server" />
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label8" CssClass="label" AssociatedControlID="lblDataFineProgetto"
                            runat="server" Text="Data Fine Progetto:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 15%">
                        <asp:Label ID="lblDataFineProgetto" CssClass="labelDati" runat="server" />
                    </div>
                </div>
                <div class="row" id="div6" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label7" CssClass="label" AssociatedControlID="lblDataAssegnazione"
                            runat="server" Text="Data Assegnazione:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 25%">
                        <asp:Label ID="lblDataAssegnazione" CssClass="labelDati" runat="server" />
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label10" CssClass="label" AssociatedControlID="lblDataApprovazione"
                            runat="server" Text="Data Approvazione:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 15%">
                        <asp:Label ID="lblDataApprovazione" CssClass="labelDati" runat="server" />
                    </div>
                </div>
                <div class="row" id="div7" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label9" CssClass="label" AssociatedControlID="lblDataPrevistaVerifica"
                            runat="server" Text="Data Inizio Prevista Verifica:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 25%">
                        <asp:Label ID="lblDataPrevistaVerifica" CssClass="labelDati" runat="server" />
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label12" CssClass="label" AssociatedControlID="lblDataFinePrevistaVerifica"
                            runat="server" Text="Data Fine Prevista Verifica:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 15%">
                        <asp:Label ID="lblDataFinePrevistaVerifica" CssClass="labelDati" runat="server" />
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label11" CssClass="label" AssociatedControlID="lblTipologiaVerifica"
                            runat="server" Text="Tipologia Verifica:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 15%">
                        <asp:Label ID="lblTipologiaVerifica" CssClass="labelDati" runat="server" />
                    </div>
                </div>
                <div class="row" id="div8" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label13" CssClass="label" AssociatedControlID="LblCompetenzaProg"
                            runat="server" Text="Competenza:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 85%">
                        <asp:Label ID="LblCompetenzaProg" CssClass="labelDati" runat="server" />
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width: 15%">    
                        <asp:Label ID="LblDocProgetto" CssClass="label" AssociatedControlID="LblCompetenzaProg" runat="server" Text="Documenti Progetto:"></asp:Label>
                    </div>
                     <div class="colHyperLink" style="width:85%">
                <asp:LinkButton ID="imgElencoDocumentiProg" Text="Elenco Documenti Progetto" style="cursor:pointer" runat="server"></asp:LinkButton>
            </div> 
                </div>
                <div class="row" id="div9" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label14" CssClass="label" runat="server" Text="Elenco Sedi Attuazione:"></asp:Label>
                    </div>
                </div>
                <asp:DataGrid ID="dgRisultatoRicerca" runat="server" Width="100%" AllowPaging="true"
                    PageSize="1" CssClass="table" AllowSorting="True" AutoGenerateColumns="False"
                    UseAccessibleHeader="True" Visible="true">
                    <FooterStyle></FooterStyle>
                    <SelectedItemStyle BackColor="White" Font-Bold="true"></SelectedItemStyle>
                    <EditItemStyle></EditItemStyle>
                    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                    <HeaderStyle></HeaderStyle>
                    <Columns>
                        <asp:ButtonColumn Text="&lt;img src=images/vincoli_small.png Width=25 Height=25 title='Seleziona Sede' border=0&gt;"
                            HeaderText="Req." CommandName="Requisiti">
                            <HeaderStyle Width="1%"></HeaderStyle>
                        </asp:ButtonColumn>
                        <asp:BoundColumn Visible="False" DataField="IDEnteSedeAttuazione" HeaderText="IDEnteSedeAttuazione">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="SedeAttuazione" HeaderText="Sede Attuazione">
                            <HeaderStyle Width="8%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Indirizzo" HeaderText="Indirizzo">
                            <HeaderStyle Width="8%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="comune" HeaderText="Comune">
                            <HeaderStyle Width="3%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Regione" HeaderText="Regione">
                            <HeaderStyle Width="3%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Telefono" HeaderText="Telefono">
                            <HeaderStyle Width="1%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fax" HeaderText="Fax">
                            <HeaderStyle Width="1%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ConfermaRequisiti" HeaderText="Conferma Requisiti">
                            <HeaderStyle Width="1%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="IDVerifica"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="IDVerificheAssociate"></asp:BoundColumn>
                        <asp:ButtonColumn Text="&lt;img src=images/sedi_small.png Width=25 Height=25 title='Requisiti' border=0&gt;"
                            HeaderText="Sost. Sede" CommandName="Sede">
                            <HeaderStyle Width="1%"></HeaderStyle>
                        </asp:ButtonColumn>
                        <asp:ButtonColumn Text="&lt;img src=images/stampa_small.png Width=25 Height=25 title='Stampa Requisiti' border=0&gt;"
                            HeaderText="Stampa Relazione" CommandName="Relazione">
                            <HeaderStyle Width="1%"></HeaderStyle>
                        </asp:ButtonColumn>
                        <asp:BoundColumn Visible="False" DataField="IDAttivit&#224;EnteSedeAttuazione"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center"
                        CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </div>
            <div class="row" id="div10" runat="server">
            </div>


       

            <div class="rowGroup" style="height: auto">
                <asp:Label ID="lblErrore" runat="server" CssClass="msgErrore"></asp:Label>
                <div class="RigaVuota">
                    &nbsp;</div>
            </div>
            <asp:Label ID="lblmessaggio" runat="server" CssClass="msgConferma"></asp:Label>
            <fieldset id="flsInformazioniGenerali" class="ContornoPagina">
                <legend id="InformazioniGenerali">
                    <img src="images/plus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor: pointer;" />&nbsp;&nbsp;<img
                        id="TornaSuInformazioniGenerali" style="cursor: pointer;" onclick="TornaAdInizioPagina()"
                        src="images/tornasu_xsmall.png" title="Torna ad inizio pagina" alt="Torna ad inizio pagina" />&nbsp;&nbsp;Informazioni
                    Generali</legend>
                <div class="wrapper" id="InformazioniGeneraliContent" style="width: 100%; border: 0px">


                    <div class="row">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="lblCodiceFascicolo" CssClass="label" runat="server" Text="Numero Fascicolo"
                                AssociatedControlID="TxtCodiceFascicolo"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 65%">
                             <asp:TextBox id="TxtCodiceFascicolo" CssClass="textbox" runat="server"  />	
                        </div>
                        <div class="collable" style="width: 20%">
                        
                            <asp:ImageButton ID="cmdSelFascicolo" runat="server" Visible="true"  OnClientClick="javascript: return ElencoFascicoli();"  ToolTip="Seleziona Codice Fascicolo" AlternateText="Seleziona Codice Fascicolo" ImageUrl="Images/cartella_small.png" ></asp:ImageButton>
                            <asp:ImageButton ID="cmdSelProtocollo0"  runat="server" Visible="true" OnClientClick="javascript: return SelProtocollo0();"  ImageUrl="Images/busta_small.png"   ToolTip="Elenco Documenti" AlternateText="Elenco Documenti" ></asp:ImageButton>
                            <asp:ImageButton ID="cmdFascCanc" runat="server" Visible="true"  ImageUrl="Images/canc_small.png"  ToolTip="Rimuovi Fascicolo/Protocolli" AlternateText="Rimuovi Fascicolo/Protocolli" ></asp:ImageButton>                               

              
                        </div>
                        <div class="colOggetti" style="width: 1%">
                            <asp:HiddenField ID="hfTxtCodiceFasc" runat="server" />
                            <%--<asp:TextBox ID="" CssClass="textbox" runat="server" Width="0px" />--%>
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="LblDescrFascicolo" CssClass="label" runat="server" Text="Descr. Fascicolo" AssociatedControlID="txtDescFasc" />
                        </div>
                        <div class="colOggetti" style="width: 65%">
                             <asp:TextBox ID="txtDescFasc" runat="server" CssClass="textbox" TextMode="MultiLine" Width="90%" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label16" CssClass="label" runat="server" Text="Data Prot.Credenziali:"
                                AssociatedControlID="txtDataProtCredenziali" />
                        </div>
                        <div class="colOggetti" style="width: 15%">
                             <asp:TextBox ID="txtDataProtCredenziali" runat="server" CssClass="textboxData"/>
                        </div>
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="Label17" CssClass="label" runat="server" Text="N° Prot. Credenziali:"
                                AssociatedControlID="txtNumProtCredenziali" />
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="txtNumProtCredenziali" CssClass="textbox" runat="server"/>
                        </div>
                        <div class="colOggetti" style="width: 20%">
                            <asp:ImageButton ID="ImgProtolloCredenziali" runat="server" ImageUrl="Images/busta_small.png"
                                ToolTip="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('txtNumProtCredenziali','txtDataProtCredenziali')"></asp:ImageButton>
                            <asp:ImageButton ID="ImgApriAllegatiCredenziali" runat="server" ImageUrl="images/documento_small.png"
                                ToolTip="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('txtNumProtCredenziali' ,'txtDataProtCredenziali')" Width="30px"></asp:ImageButton>
                             
                        &nbsp;</div>
                        <div class="collable" style="width: 10%">
                            <asp:Label ID="LblStampaCredenzialiIncarico" CssClass="label" runat="server" Text="Credenziali" />
                        </div>
                        <div class="colOggetti" style="width: 10%">
                            <asp:ImageButton ID="CmdStampaCredenzialiIncarico" runat="server" ToolTip="CREDENZIALI INTERNO"
                                ImageUrl="images/stampa_small.png"></asp:ImageButton>
                            <asp:ImageButton ID="CmdStampaCredenzialiIncaricoIGF" runat="server" ToolTip="CREDENZIALI IGF"
                                ImageUrl="images/stampa_small.png"></asp:ImageButton>
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="lblDataInizioVerifica" CssClass="label" runat="server" Text="Data Inizio Verifica:"
                                AssociatedControlID="txtDataInizioVerifica" />
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="txtDataInizioVerifica" runat="server" CssClass="textboxData"> </asp:TextBox>
                        </div>
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="lblDataFineVerifica" CssClass="label" runat="server" Text="Data Fine Verifica:"
                                AssociatedControlID="txtDataFineVerifica" />
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="txtDataFineVerifica" runat="server" CssClass="textboxData"> </asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="lblDataProtIncarico" CssClass="label" runat="server" Text="Data Prot. Incarico:"
                                AssociatedControlID="txtDataProtocolloIncarico" />
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="txtDataProtocolloIncarico" runat="server" CssClass="textboxData"/>
                        </div>
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="lblNumProtIncarico" CssClass="label" runat="server" Text="N° Prot. Incarico:"
                                AssociatedControlID="TxtNumProtIncarico" />
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="TxtNumProtIncarico" CssClass="textbox" runat="server" />
                        </div>
                        <div class="collable" style="width: 20%">
                            <asp:ImageButton ID="cmdSc1SelProtocollo1" runat="server" ImageUrl="Images/busta_small.png"
                                ToolTip="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('TxtNumProtIncarico','txtDataProtocolloIncarico')"
                                Style="cursor: hand"></asp:ImageButton>
                            <asp:ImageButton ID="cmdSc1Allegati1" runat="server" 
                                ImageUrl="images/documento_small.png" ToolTip="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('TxtNumProtIncarico','txtDataProtocolloIncarico')"
                                Style="cursor: hand" Width="30px"></asp:ImageButton>
                            
                        &nbsp;</div>
                        <div class="collable" style="width: 10%">
                            <asp:Label ID="LblStampaLettereIncarico" CssClass="label" runat="server" Text="Incarico" />
                        </div>
                        <div class="collable" style="width: 10%">
                            <asp:ImageButton ID="CmdStampaLettereIncarico" runat="server" ToolTip="Lettere incarico INTERNO"
                                ImageUrl="images/stampa_small.png"></asp:ImageButton>
                            <asp:ImageButton ID="CmdStampaLettereIncaricoIGF" runat="server" ToolTip="Lettere incarico IGF"
                                ImageUrl="images/stampa_small.png"></asp:ImageButton>
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width: 30%">
                            <asp:CheckBox ID="chkRelazione" runat="server" Text="Relazione" TextAlign="Left">
                            </asp:CheckBox>
                        </div>
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="LblDataProtocolloRelazione" CssClass="label" runat="server" Text="Data Relazione:"
                                AssociatedControlID="txtDataProtocolloRelazione" />
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="txtDataProtocolloRelazione" runat="server" CssClass="textboxData"> </asp:TextBox>
                        </div>
                        <div class="collable" style="width: 15%">
                            <asp:CheckBox ID="chkNonEffettuata" runat="server" AutoPostBack="True" Text="Chiusa"
                                TextAlign="left"></asp:CheckBox>
                            <%--   <asp:ImageButton ID="cmdRelazioneStand" runat="server" ToolTip="relazionestand" ImageUrl="images/download.jpg">
                            </asp:ImageButton>--%>
                        </div>
                    </div>
                    <div class="row" id="dvDGPositiva" runat="server">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="LblDataProtTrasDG" CssClass="label" runat="server" Text="Data Prot. Trasmissione al D.G.:"
                                AssociatedControlID="TxtDataProtTrasDG" />
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="TxtDataProtTrasDG" runat="server" CssClass="textboxData"></asp:TextBox>
                        </div>
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="LblNumProtTrasDG" CssClass="label" runat="server" Text="N° Prot. Trasmissione al D.G.:"
                                AssociatedControlID="TxtNumProtTrasDG" />
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="TxtNumProtTrasDG" runat="server" CssClass="textbox" />
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:ImageButton ID="ImgProtocolloTrasDG" runat="server" ImageUrl="Images/busta_small.png"
                                ToolTip="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('TxtNumProtTrasDG','TxtDataProtTrasDG')"
                                Style="cursor: hand"></asp:ImageButton>
                            <asp:ImageButton ID="ImgApriAllegatiTrasDG" runat="server" ImageUrl="images/documento_small.png"
                                ToolTip="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('TxtNumProtTrasDG','TxtDataProtTrasDG')"
                                Style="cursor: hand" Width="30px"></asp:ImageButton>
                            <%--<asp:ImageButton ID="ImgProtocollazioneTrasDG" Visible="True" ImageUrl="images/GraduatoriaVol.gif"
                                runat="server" Height="20px" ToolTip="Richiesta Protocollazione" OnClientClick="javascript: return Protocollazione('TxtNumProtTrasDG','TxtDataProtTrasDG','0','1','0')"
                                Style="cursor: hand"></asp:ImageButton>--%>
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="LblDataProtocolloInvioChiusura" CssClass="label" runat="server" Text="Data Prot. Invio Lettera di Chiusura:"
                                AssociatedControlID="txtDataProtocolloInvioChiusura" />
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="txtDataProtocolloInvioChiusura" runat="server" CssClass="textboxData"></asp:TextBox>
                        </div>
                        <div class="collable" style="width: 15%">
                            <asp:Label ID="LblNumProtocolloInvioLetteraChiusura" CssClass="label" runat="server"
                                Text="N° Prot. Invio Lettera Chiusura:" AssociatedControlID="TxtNumProtocolloInvioLetteraChiusura" />
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="TxtNumProtocolloInvioLetteraChiusura" runat="server" CssClass="textbox"/>
                        </div>
         
                        <div class="colOggetti" style="width: 20%">
                            <asp:ImageButton ID="ImgProtocolloLettChiusura" runat="server" ImageUrl="Images/busta_small.png"
                                ToolTip="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('TxtNumProtocolloInvioLetteraChiusura','txtDataProtocolloInvioChiusura')"
                                Style="cursor: hand"></asp:ImageButton>
                            <asp:ImageButton ID="ImgApriAllegatiLettChiusura" runat="server" ImageUrl="images/documento_small.png"
                                ToolTip="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('TxtNumProtocolloInvioLetteraChiusura','txtDataProtocolloInvioChiusura')"
                                Style="cursor: hand" Width="30px"></asp:ImageButton>

                        &nbsp;</div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:CheckBox ID="chkRichiamo" runat="server"  Text="Richiamo"
                                TextAlign="left"></asp:CheckBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width: 20%">
                            <asp:Label ID="Label18" CssClass="label" runat="server" Text="Conclusione Verifica senza Irregolarita'"
                                AssociatedControlID="CmdConclusione" />
                        </div>
                        <div class="colOggetti" style="width: 10%">
                            <asp:ImageButton ID="CmdConclusione" runat="server" ToolTip="LetteraConclusioneVerificaPositiva"
                                ImageUrl="images/stampa_small.png"></asp:ImageButton>
                        </div>
                        <div class="collable" style="width: 20%">
                            <asp:Label ID="Label19" CssClass="label" runat="server" Text="Verifica con Richiamo"
                                AssociatedControlID="CmdIrregolarita" />
                        </div>
                        <div class="colOggetti" style="width: 10%">
                            <%--        <asp:ImageButton ID="CmdIrregolarita" runat="server" ToolTip="VerificasenzaIRREGOLARITAconrichiamo"
                                ImageUrl="images/stampa_small.png" Width="26px"></asp:ImageButton>--%>
                            <asp:ImageButton ID="CmdIrregolarita" runat="server" ToolTip="VerificasenzaIRREGOLARITAconrichiamo"
                                ImageUrl="images/stampa_small.png" />
                        </div>
                    </div>
                    <div class="row" id="dvDGPositiveStampe" runat="server">
                        <div class="collable" style="width: 20%">
                            <asp:Label ID="Label20" CssClass="label" runat="server" Text="Trasmissione Relazione Al D.G"
                                AssociatedControlID="cmdTrasmissione"  />
                        </div>
                        <div class="colOggetti" style="width: 10%">
                            <asp:ImageButton ID="cmdTrasmissione" runat="server" ToolTip="TrasmissionerelazionealDG"
                                ImageUrl="images/stampa_small.png"></asp:ImageButton>
                        </div>
                        <div class="collable" style="width: 20%">
                            <asp:Label ID="Label21" CssClass="label" runat="server" Text="Trasm. Ver. al D.G con richiamo"
                                AssociatedControlID="CmdTrasmRich" />
                        </div>
                        <div class="colOggetti" style="width: 10%">
                            <asp:ImageButton ID="CmdTrasmRich" runat="server" ToolTip="Trasmissione_relazione_al_D.G_con_richiamo"
                                ImageUrl="images/stampa_small.png"></asp:ImageButton>
                        </div>
                    </div>
                </div>
            </fieldset>
            
            <fieldset id="flsDatiContestazione" class="ContornoPagina">
                <legend id="DatiContestazione">
                    <img src="images/plus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor: pointer;" />&nbsp;&nbsp;<img
                        id="TornaSuDatiContestazione" style="cursor: pointer;" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"
                        title="Torna ad inizio pagina" alt="Torna ad inizio pagina" />&nbsp;&nbsp;Dati Contestazione</legend>
                <div class="wrapper" id="DatiContestazioneContent" style="width: 100%; border: 0px">				
                    <div class="row" id ="dvDGContestata" runat="server">
                        <div class="collable" style="width: 25%">
                            <asp:Label ID="LblDataProtLetteraContestazioneDG" CssClass="label" runat="server"
                                Text="Data Prot. Trasmissiome Contest. al D.G.:" AssociatedControlID="txtDataProtLetteraContestazioneDG"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="txtDataProtLetteraContestazioneDG" runat="server" CssClass="textboxData"></asp:TextBox>
                        </div>
                        <div class="collable" style="width: 25%">
                            <asp:Label ID="LblNProtLetteraContestazioneDG" CssClass="label" runat="server" Text="N° Prot. Trasmissiome Contest. al D.G.:"
                                AssociatedControlID="txtNProtLetteraContestazioneDG"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 20%">
                            <asp:TextBox ID="txtNProtLetteraContestazioneDG" runat="server" CssClass="textbox" />
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:ImageButton ID="ImgProtocolloTrasContestataDG" runat="server" ImageUrl="Images/busta_small.png"
                                ToolTip="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('txtNProtLetteraContestazioneDG','txtDataProtLetteraContestazioneDG')"
                                Style="cursor: hand"></asp:ImageButton>
                            <asp:ImageButton ID="ImgApriAllegatiTrasContestataDG" runat="server" ImageUrl="images/documento_small.png"
                                ToolTip="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('txtNProtLetteraContestazioneDG','txtDataProtLetteraContestazioneDG')"
                                Style="cursor: hand" Width="30px"></asp:ImageButton>
                            <%--<asp:ImageButton ID="ImgProtocollazioneTrasContestataDG" Visible="True" ImageUrl="images/GraduatoriaVol.gif"
                                runat="server" Height="20px" ToolTip="Richiesta Protocollazione" OnClientClick="javascript: return Protocollazione('txtNProtLetteraContestazioneDG','txtDataProtLetteraContestazioneDG','0','1','0')"
                                Style="cursor: hand"></asp:ImageButton>--%>
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width: 25%">
                            <asp:Label ID="LblDataProtocolloInvioLetteraContestazione" CssClass="label" runat="server"
                                Text="Data Prot. Invio Contestazione:" AssociatedControlID="txtDataProtocolloInvioLetteraContestazione"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="txtDataProtocolloInvioLetteraContestazione" runat="server" CssClass="textboxData"></asp:TextBox>
                        </div>
                        <div class="collable" style="width: 25%">
                            <asp:Label ID="LblNumeroProtocolloInvioLettContestazione" CssClass="label" runat="server"
                                Text="N° Prot. Invio Lettera Contestazione:" AssociatedControlID="TxtNumeroProtocolloInvioLettContestazione"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 20%">
                            <asp:TextBox ID="TxtNumeroProtocolloInvioLettContestazione" runat="server" CssClass="textbox" />
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:ImageButton ID="ImgProtocolloInvioLettContestazione" runat="server" ImageUrl="Images/busta_small.png"
                                ToolTip="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('TxtNumeroProtocolloInvioLettContestazione','txtDataProtocolloInvioLetteraContestazione')"
                                Style="cursor: hand"></asp:ImageButton>
                            <asp:ImageButton ID="ImgApriAllegatiInvioLettContestazione" runat="server" ImageUrl="images/documento_small.png"
                                ToolTip="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('TxtNumeroProtocolloInvioLettContestazione','txtDataProtocolloInvioLetteraContestazione')"
                                Style="cursor: hand" Width="30px"></asp:ImageButton>
                            <%--<asp:ImageButton ID="ImgProtocollazioneInvioLettContestazione" Visible="True" ImageUrl="images/GraduatoriaVol.gif"
                                runat="server" Height="20px" ToolTip="Richiesta Protocollazione" OnClientClick="javascript: return Protocollazione('TxtNumeroProtocolloInvioLettContestazione','txtDataProtocolloInvioLetteraContestazione','0','0','0')"
                                Style="cursor: hand" Width="20px"></asp:ImageButton>--%>
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
                            <asp:Label ID="LblDataRispostaLetteraContestazione" CssClass="label" runat="server" Visible="False"
                                Text="Data Lettera Controdeduzioni:" AssociatedControlID="txtDataRispostaLetteraContestazione"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="txtDataRispostaLetteraContestazione" runat="server" CssClass="textboxData" Visible="False"> </asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width: 25%">
                            <asp:Label ID="LblDataProtocolloRispostaContestazione" CssClass="label" runat="server"
                                Text="Data Prot. Lettera Controdeduzioni:" AssociatedControlID="txtDataProtocolloRispostaContestazione"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="txtDataProtocolloRispostaContestazione" runat="server" CssClass="textboxData"></asp:TextBox>
                        </div>
                        <div class="collable" style="width: 25%">
                            <asp:Label ID="LblNumProtocolloRispostaContestazione" CssClass="label" runat="server"
                                Text="N° Prot. Lettera Controdeduzioni:" AssociatedControlID="TxtNumProtocolloRispostaContestazione"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 20%">
                            <asp:TextBox ID="TxtNumProtocolloRispostaContestazione" runat="server" CssClass="textbox"/>
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:ImageButton ID="ImgProtocolloRispContestazione" runat="server" ImageUrl="Images/busta_small.png"
                                ToolTip="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('TxtNumProtocolloRispostaContestazione','txtDataProtocolloRispostaContestazione')"
                                Style="cursor: hand"></asp:ImageButton>
                            <asp:ImageButton ID="ImgApriAllegatiRispContestazione" runat="server" ImageUrl="images/documento_small.png"
                                ToolTip="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('TxtNumProtocolloRispostaContestazione','txtDataProtocolloRispostaContestazione')"
                                Style="cursor: hand" Width="30px"></asp:ImageButton>
                            <%--<asp:ImageButton ID="ImgProtocollazioneRispContestazione" Visible="True" ImageUrl="images/GraduatoriaVol.gif"
                                runat="server" Height="20px" ToolTip="Richiesta Protocollazione" OnClientClick="javascript: return Protocollazione('TxtNumProtocolloRispostaContestazione','txtDataProtocolloRispostaContestazione','0','0','0')"
                                Style="cursor: hand"></asp:ImageButton>--%>
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width: 25%">
                            <asp:Label ID="LblDataProtChiusuraContestazione" CssClass="label" runat="server"
                                Text="Data Prot. Chiusura Contestazione:" AssociatedControlID="TxtDataProtChiusuraContestazione"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="TxtDataProtChiusuraContestazione" runat="server" CssClass="textboxData"></asp:TextBox>
                        </div>
                        <div class="collable" style="width: 25%">
                            <asp:Label ID="LblNumProtChiusuraContestazione" CssClass="label" runat="server" Text="N° Prot. Chiusura Contestazione:"
                                AssociatedControlID="TxtNumProtChiusuraContestazione"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 20%">
                            <asp:TextBox ID="TxtNumProtChiusuraContestazione" runat="server" CssClass="textbox"/>
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:ImageButton ID="ImgProtocolloProtChiusuraContestazione" runat="server" ImageUrl="Images/busta_small.png"
                                ToolTip="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('TxtNumProtChiusuraContestazione','TxtDataProtChiusuraContestazione')"
                                Style="cursor: hand"></asp:ImageButton>
                            <asp:ImageButton ID="ImgApriAllegatiProtChiusuraContestazione" runat="server" ImageUrl="images/documento_small.png"
                                ToolTip="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('TxtNumProtChiusuraContestazione','TxtDataProtChiusuraContestazione')"
                                Style="cursor: hand" Width="30px"></asp:ImageButton>
                            <%--<asp:ImageButton ID="ImgProtocollazioneProtChiusuraContestazione" Visible="True" ImageUrl="images/GraduatoriaVol.gif"
                                runat="server" Height="20px" ToolTip="Richiesta Protocollazione" OnClientClick="javascript: return Protocollazione('TxtNumProtChiusuraContestazione','TxtDataProtChiusuraContestazione','0','0','0')"
                                Style="cursor: hand"></asp:ImageButton>--%>
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width: 20%">
                            <asp:Label ID="Label22" CssClass="label" runat="server" Text="Lettera Contestaz. Add"
                                AssociatedControlID="cmdContestazione" />
                        </div>
                        <div class="colOggetti" style="width: 10%">
                            <asp:ImageButton ID="cmdContestazione" runat="server" ToolTip="Lettera Contestazione Addebiti"
                                ImageUrl="images/stampa_small.png"></asp:ImageButton>
                        </div>
                        <div class="collable" style="width: 20%">
                            <asp:Label ID="Label24" CssClass="label" runat="server" Text="Lettera Trasm D G e Contes."
                                AssociatedControlID="CmdLetteraTrasm" />
                        </div>
                        <div class="colOggetti" style="width: 10%">
                            <asp:ImageButton ID="CmdLetteraTrasm" runat="server" ToolTip="LetteratrasmDGecontestazione"
                                ImageUrl="images/stampa_small.png"></asp:ImageButton>
                        </div>
                        <div class="collable" style="width: 20%">
                            <asp:Label ID="Label25" CssClass="label" runat="server" Text="Lettera Chiusura Contestazione"
                                AssociatedControlID="CmdLetteraTrasm" />
                        </div>
                        <div class="colOggetti" style="width: 10%">
                            <asp:ImageButton ID="CmdLetteraChiusContestazione" runat="server" ToolTip="Lettera Chiusura Contestazione"
                                ImageUrl="images/stampa_small.png"></asp:ImageButton>
                        </div>
                    </div>
                </div>
            </fieldset>
         <fieldset id="flsDatiSanzione" class="ContornoPagina">
                <legend id="DatiSanzione">
                    <img src="images/plus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor: pointer;" />&nbsp;&nbsp;<img
                        id="TornaSuDatiSanzione" style="cursor: pointer;" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"
                        title="Torna ad inizio pagina" alt="Torna ad inizio pagina" />&nbsp;&nbsp;Dati Sanzione</legend>
                <div class="wrapper" id="DatiSanzioneContent" style="width: 100%; border: 0px">				
                    <div class="row" id="dvDGSanzione" runat="server">
                        <div class="collable" style="width: 25%" >
                            <asp:Label ID="LblDataProtocolloTrasmissioneSanzione" CssClass="label" runat="server"
                                Text="Data Protocollo Trasmissione Sanzione al D.G.:" AssociatedControlID="txtDataProtocolloTrasmissioneSanzione"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="txtDataProtocolloTrasmissioneSanzione" runat="server" CssClass="textboxData"></asp:TextBox>
                        </div>
                        <div class="collable" style="width: 25%">
                            <asp:Label ID="LblNumeroProtocolloTrasmissioneSanzione" CssClass="label" runat="server"
                                Text="N° Protocollo Trasmissione Sanzione al D.G.:" AssociatedControlID="TxtNumeroProtocolloTrasmissioneSanzione"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 20%">
                            <asp:TextBox ID="TxtNumeroProtocolloTrasmissioneSanzione" runat="server" CssClass="textbox"/>
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:ImageButton ID="ImgProtocolloTrasmissioneSanzione" runat="server" ImageUrl="Images/busta_small.png"
                                ToolTip="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('TxtNumeroProtocolloTrasmissioneSanzione','txtDataProtocolloTrasmissioneSanzione')"
                                Style="cursor: hand"></asp:ImageButton>
                            <asp:ImageButton ID="ImgApriAllegatiTrasmissioneSanzione" runat="server" ImageUrl="images/documento_small.png"
                                ToolTip="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('TxtNumeroProtocolloTrasmissioneSanzione','txtDataProtocolloTrasmissioneSanzione')"
                                Style="cursor: hand" Width="30px"></asp:ImageButton>
                            
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
                            <asp:TextBox ID="TxtNumeroProtocolloEsecuzioneSanzione" runat="server" CssClass="textbox"/>
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:ImageButton ID="ImgProtocolloEsecuzioneSanzione" runat="server" ImageUrl="Images/busta_small.png"
                                ToolTip="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('TxtNumeroProtocolloEsecuzioneSanzione','txtDataProtocolloEsecuzioneSanzione')"
                                Style="cursor: hand"></asp:ImageButton>
                            <asp:ImageButton ID="ImgApriAllegatiEsecuzioneSanzione" runat="server" ImageUrl="images/documento_small.png"
                                ToolTip="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('TxtNumeroProtocolloEsecuzioneSanzione','txtDataProtocolloEsecuzioneSanzione')"
                                Style="cursor: hand" Width="30px"></asp:ImageButton>
                            <%--<asp:ImageButton ID="ImgProtocollazioneEsecuzioneSanzione" Visible="True" ImageUrl="images/GraduatoriaVol.gif"
                                runat="server" Height="20px" ToolTip="Richiesta Protocollazione" OnClientClick="javascript: return Protocollazione('TxtNumeroProtocolloEsecuzioneSanzione','txtDataProtocolloEsecuzioneSanzione','0','0','0')"
                                Style="cursor: hand" Width="16px"></asp:ImageButton>--%>
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width: 25%">
                            <asp:Label ID="LblDataProtocolloTrasmServizi" CssClass="label" runat="server" Text="Data Protocollo Trasmissione Servizi:"
                                AssociatedControlID="txtDataProtocolloTrasmServizi"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:TextBox ID="txtDataProtocolloTrasmServizi" runat="server" CssClass="textboxData"></asp:TextBox>
                        </div>
                        <div class="collable" style="width: 25%">
                            <asp:Label ID="LblNProtocolloTrasmServizi" CssClass="label" runat="server" Text="N° Prot. Trasmissione Servizi:"
                                AssociatedControlID="txtNProtocolloTrasmServizi"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 20%">
                            <asp:TextBox ID="txtNProtocolloTrasmServizi" runat="server" CssClass="textbox"/>
                        </div>
                        <div class="colOggetti" style="width: 15%">
                            <asp:ImageButton ID="ImgProtocolloTrasmServizi" runat="server" ImageUrl="Images/busta_small.png"
                                ToolTip="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('txtNProtocolloTrasmServizi','txtDataProtocolloTrasmServizi')"
                                Style="cursor: hand"></asp:ImageButton>
                            <asp:ImageButton ID="ImgApriAllegatiTrasmServizi" runat="server" ImageUrl="images/documento_small.png"
                                ToolTip="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('txtNProtocolloTrasmServizi','txtDataProtocolloTrasmServizi')"
                                Style="cursor: hand" Width="30px"></asp:ImageButton>
                            <%--<asp:ImageButton ID="ImgProtocollazioneTrasmServizi" Visible="True" ImageUrl="images/GraduatoriaVol.gif"
                                runat="server" Height="20px" ToolTip="Richiesta Protocollazione" OnClientClick="javascript: return Protocollazione('txtNProtocolloTrasmServizi','txtDataProtocolloTrasmServizi','0','0','1')"
                                Style="cursor: hand"></asp:ImageButton>--%>
                            <asp:ImageButton ID="imgSanzioneServizi" Visible="True" ImageUrl="images/Icona_Volontario_small.png"
                                runat="server" ToolTip="Servizi" OnClientClick="javascript: return Servizi()"
                                Style="cursor: hand" Width="30px"></asp:ImageButton>
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
                            <asp:Label ID="LblSanzione" CssClass="label" runat="server" Text="Sanzioni:" AssociatedControlID="cmdSanzione"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width: 30%">
                           <!-- <asp:ImageButton ID="cmdSanzione" Style="cursor: hand" runat="server" ToolTip="Applica Sanzione"
                                BorderWidth="1px" BorderStyle="Outset" BorderColor="#6699FF" ImageUrl="images/accorpa_progetto.jpg"
                                Enabled="False"></asp:ImageButton> -->
                                <asp:LinkButton ID="lkbSanzione" Text="Applica Sanzione" style="cursor:pointer" runat="server"></asp:LinkButton>
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width: 20%">
                            <asp:Label ID="Label26" CssClass="label" runat="server" Text="Trasm DG Sanzione"
                                AssociatedControlID="CmdArchiviazione" />
                        </div>
                        <div class="colOggetti" style="width: 5%">
                            <asp:ImageButton ID="CmdArchiviazione" runat="server" ToolTip="Trasmissione D.G. Sanzione"
                                ImageUrl="images/stampa_small.png"></asp:ImageButton>
                        </div>
                        <div class="collable" style="width: 20%">
                            <asp:Label ID="Label27" CssClass="label" runat="server" Text="Trasm Provv Sanz."
                                AssociatedControlID="CmdLetteraTrasmissioneProvv" />
                        </div>
                        <div class="colOggetti" style="width: 5%">
                            <asp:ImageButton ID="CmdLetteraTrasmissioneProvv" runat="server" ToolTip="Letteratrasmissioneprovvedimentosanzionatorio"
                                ImageUrl="images/stampa_small.png"></asp:ImageButton>
                        </div>
                        <div class="collable" style="width: 20%">
                            <asp:Label ID="Label28" CssClass="label" runat="server" Text="Trasm. Provv. Serv."
                                AssociatedControlID="CmdLETTERATRASMISPROVAISERVIZI" />
                        </div>
                        <div class="colOggetti" style="width: 5%">
                            <asp:ImageButton ID="CmdLETTERATRASMISPROVAISERVIZI" runat="server" ToolTip="LETTERATRASMISPROVAISERVIZI"
                                ImageUrl="images/stampa_small.png"></asp:ImageButton>
                        </div>
                        <div class="collable" style="width: 20%">
                            <asp:Label ID="Label29" CssClass="label" runat="server" Text="Trasm. Provv. Reg."
                                AssociatedControlID="CmdTRASMISSIONEPROVVEDIMENTOREGIONE" />
                        </div>
                        <div class="colOggetti" style="width: 5%">
                            <asp:ImageButton ID="CmdTRASMISSIONEPROVVEDIMENTOREGIONE" runat="server" ToolTip="TRASMISSIONEPROVVEDIMENTOREGIONE"
                                ImageUrl="images/stampa_small.png"></asp:ImageButton>
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width: 20%">
                            <asp:Label ID="Label30" CssClass="label" runat="server" Text="Diffida" AssociatedControlID="cmdDiffida" />
                        </div>
                        <div class="colOggetti" style="width: 5%">
                            <asp:ImageButton ID="cmdDiffida" runat="server" ToolTip="Diffida" ImageUrl="images/stampa_small.png">
                            </asp:ImageButton>
                        </div>
                        <div class="collable" style="width: 20%">
                            <asp:Label ID="Label31" CssClass="label" runat="server" Text="Revoca" AssociatedControlID="CmdRevoca" />
                        </div>
                        <div class="colOggetti" style="width: 5%">
                            <asp:ImageButton ID="CmdRevoca" runat="server" ToolTip="REVOCA" ImageUrl="images/stampa_small.png">
                            </asp:ImageButton>
                        </div>
                        <div class="collable" style="width: 20%">
                            <asp:Label ID="Label32" CssClass="label" runat="server" Text="Interdizione" AssociatedControlID="CmdInterdizione" />
                        </div>
                        <div class="colOggetti" style="width: 5%">
                            <asp:ImageButton ID="CmdInterdizione" runat="server" ToolTip="INTERDIZIONE PER UN ANNO A PRESENTARE PROGETTI"
                                ImageUrl="images/stampa_small.png"></asp:ImageButton>
                        </div>
                        <div class="collable" style="width: 20%">
                            <asp:Label ID="Label33" CssClass="label" runat="server" Text="Cancellazione" AssociatedControlID="CmdCancellazione" />
                        </div>
                        <div class="colOggetti" style="width: 5%">
                            <asp:ImageButton ID="CmdCancellazione" runat="server" ToolTip="CANCELLAZIONE" ImageUrl="images/stampa_small.png">
                            </asp:ImageButton>
                        </div>
                    </div>
                </div>
            </fieldset>



            <div class="row" id="div11" runat="server">
                <div class="collable" style="width: 20%">
                    <asp:Label ID="LblNote" CssClass="label" runat="server" Text="NOTE:" AssociatedControlID="TxtNote" />
                </div>
            </div>
            <div class="row" id="div12" runat="server">
                <div class="collable">
                    <asp:TextBox ID="TxtNote" CssClass="textbox" TextMode="MultiLine" runat="server"
                        Width="100%" />
                </div>
            </div>
            <div class="row" id="div13" runat="server">
            </div>

            <div class="wrapper" style="width: 100%">
            <div class="RigaPulsanti">
                <%------function cmdStampa()----%>
                <asp:Button ID="cmdStampa" CssClass="Pulsante" runat="server" Text="Stampa Progetto"></asp:Button>
                <asp:Button ID="cmdStampaVQ" CssClass="Pulsante" runat="server" Text="Stampa Valutazione">
                </asp:Button>
                <asp:Button ID="cmdChiusaContestata" CssClass="Pulsante" runat="server" Text="Chiusa Contestata">
                </asp:Button>
                <%-----------%>
                <asp:Button ID="imgAssegnaVer" CssClass="Pulsante" runat="server" OnClientClick="Verificatore();"
                    Text="Assegna Verificatore"></asp:Button>
                <asp:Button ID="cmdAnnullata" CssClass="Pulsante" runat="server" 
                    Text="Annulla Verifica" Visible="False"></asp:Button>
                <asp:Button ID="cmdIncludi" CssClass="Pulsante" runat="server" Text="Includi"></asp:Button>
                <asp:Button ID="cmdSospendi" CssClass="Pulsante" runat="server" Text="Sospendi Verifica">
                </asp:Button>
                <asp:Button ID="cmdRipristina" CssClass="Pulsante" runat="server" 
                    Text="Ripristina Verifica">
                </asp:Button>
                <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" Text="Salva" />
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"></asp:Button>
            </div>
            <input id="txtIdVerificatore" type="hidden" name="txtIdVerificatore" runat="server" />
            </div> 
        </fieldset>



        
            <asp:HiddenField ID="hftxtCodProgetto" runat="server" />
            <asp:HiddenField ID="hftxtidente" runat="server" />
            <asp:HiddenField ID="hftxtidProgrammazione" runat="server" />
            <asp:HiddenField ID="hftxtIdVerifica" runat="server" />
            <asp:HiddenField ID="hftxtIdEnteSedeAtt" runat="server" />
       
    </div>
</asp:Content>
