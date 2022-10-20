<%@ Page Title="Requisiti Verifiche" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"   CodeBehind="dettaglioverificarequisiti.aspx.vb" Inherits="Futuro.dettaglioverificarequisiti" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
    <script type="text/javascript">
      /* <![CDATA[ */
        var pathPlus = "images/plus_xsmall.png";
        var pathMinus = "images/minus_xsmall.png";
        var titlePathMinus = "Riduci";
        var titlePathPlus = "Espandi";
       
       
        var isPBAttuazionedelProgetto  = '<%= hdnIsPBAttuazionedelProgetto.value %>' ;
        var isPBFormazionedeiVolontari = '<%= hdnIsPBFormazionedeiVolontari.value %>' ;
        var isPBOperativitadeiVolontari = '<%= hdnIsPBOperativitadeiVolontari.value %>' ;
        var isPBRisorseFinanziarieErogate = '<%= hdnIsPBRisorseFinanziarieErogate.value %>' ;

        /* ]]> */  
    </script>



<script type="text/javascript">
    $(function () {     
        var tornaSu = 0;
        if (isPBAttuazionedelProgetto != "") {
            $('#AttuazionedelProgettoContent').css('display', 'none');
            $('#lgAttuazionedelProgetto').find("img").first().attr("src", pathPlus);
            $('#lgAttuazionedelProgetto').find("img").first().attr("title", titlePathPlus);
        }
        $('#AttuazionedelProgettoToggler').click(function () {
            var isVisible = $('#AttuazionedelProgettoContent').is(':visible');
            if (!isVisible) {
                $('#AttuazionedelProgettoContent').slideToggle('fast');
                $(this).toggleClass('slideSign');
                $('#lgAttuazionedelProgetto').find("img").first().attr("src", pathMinus);
                $('#lgAttuazionedelProgetto').find("img").first().attr("title", titlePathMinus);
                $('#flsAttuazionedelProgetto').focus();
                setTimeout(function () { $('#lgAttuazionedelProgetto').focus() }, 0);
            }
            else {
                $('#flsAttuazionedelProgetto').focus();
                setTimeout(function () { $('#lgAttuazionedelProgetto').focus() }, 0);
            }
            return false;
        });
        $('#TornaSuAttuazionedelProgetto').click(function () {
            tornaSu = 1;
        });
        $('#lgAttuazionedelProgetto').click(function () {
            if (tornaSu == 0) {
                var isVisible = $('#AttuazionedelProgettoContent').is(':visible');
                $('#AttuazionedelProgettoContent').slideToggle('fast');
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
  

    $(function () {
        var tornaSu = 0;

            if( isPBFormazionedeiVolontari != "")
            {  
            $('#FormazionedeiVolontariContent').css('display', 'none');
            $('#lgFormazionedeiVolontari').find("img").first().attr("src", pathPlus);
            $('#lgFormazionedeiVolontari').find("img").first().attr("title", titlePathPlus);
            }
          
            $('#FormazionedeiVolontariToggler').click(function () {
       
                var isVisible = $('#FormazionedeiVolontariContent').is(':visible');
                if (!isVisible) {
                    $('#FormazionedeiVolontariContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    $('#lgFormazionedeiVolontari').find("img").first().attr("src", pathMinus);
                    $('#lgFormazionedeiVolontari').find("img").first().attr("title", titlePathMinus);
                    $('#flsFormazionedeiVolontari').focus();
                    setTimeout(function () { $('#lgFormazionedeiVolontari').focus() }, 0);  
                    
                     
                }
                else {
                    $('#flsFormazionedeiVolontari').focus();
                    setTimeout(function () { $('#lgFormazionedeiVolontari').focus() }, 0); 
                    
                }
                return false;
            });
            $('#TornaSuFormazionedeiVolontari').click(function () {
                tornaSu = 1;
            });
            $('#lgFormazionedeiVolontari').click(function () {
 
                if (tornaSu == 0) {
                  
                    var isVisible = $('#FormazionedeiVolontariContent').is(':visible');
                    $('#FormazionedeiVolontariContent').slideToggle('fast');
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
          
        if( isPBOperativitadeiVolontari != "")
        { 
            $('#OperativitadeiVolontariContent').css('display', 'none');
            $('#lgOperativitadeiVolontari').find("img").first().attr("src", pathPlus);
            $('#lgOperativitadeiVolontari').find("img").first().attr("title", titlePathPlus); 
        }

        $('#OperativitadeiVolontariToggler').click(function () {
            var isVisible = $('#OperativitadeiVolontariContent').is(':visible');
       
            if (!isVisible) {
         
                $('#OperativitadeiVolontariContent').slideToggle('fast');
                $(this).toggleClass('slideSign');
                $('#flsOperativitadeiVolontari').focus();
                $('#lgOperativitadeiVolontari').find("img").first().attr("src", pathMinus);
                $('#lgOperativitadeiVolontari').find("img").first().attr("title", titlePathMinus);
                $('#flsOperativitadeiVolontari').focus();
                setTimeout(function () { $('#lgOperativitadeiVolontari').focus() }, 0); 
            }
            else {
         
                $('#flsOperativitadeiVolontari').focus();
                setTimeout(function () { $('#lgOperativitadeiVolontari').focus() }, 0); 
            }
               
            return false;
        });
        $('#TornaSuOperativitadeiVolontari').click(function () {
            tornaSu = 1;
        });
        $('#lgOperativitadeiVolontari').click(function () {
          
            if (tornaSu == 0) {
                var isVisible = $('#OperativitadeiVolontariContent').is(':visible');
                $('#OperativitadeiVolontariContent').slideToggle('fast');
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
  
    $(function () {
        var tornaSu = 0;

        if( isPBRisorseFinanziarieErogate != "")
        { 
            $('#RisorseFinanziarieErogateContent').css('display', 'none');
            $('#lgRisorseFinanziarieErogate').find("img").first().attr("src", pathPlus);
            $('#lgRisorseFinanziarieErogate').find("img").first().attr("title", titlePathPlus); 
        }

        $('#RisorseFinanziarieErogateToggler').click(function () {
            var isVisible = $('#RisorseFinanziarieErogateContent').is(':visible');
            if (!isVisible) {
                $('#RisorseFinanziarieErogateContent').slideToggle('fast');
                $(this).toggleClass('slideSign');
                $('#lgRisorseFinanziarieErogate').find("img").first().attr("src", pathMinus);
                $('#lgRisorseFinanziarieErogate').find("img").first().attr("title", titlePathMinus);
                $('#flsRisorseFinanziarieErogate').focus();
                setTimeout(function () { $('#lgRisorseFinanziarieErogate').focus() }, 0); 
            }
            else {
                $('#flsRisorseFinanziarieErogate').focus();
                setTimeout(function () { $('#lgRisorseFinanziarieErogate').focus() }, 0); 
            }
            return false;
        });
        $('#TornaSuRisorseFinanziarieErogate').click(function () {
            tornaSu = 1;
        });
        $('#lgRisorseFinanziarieErogate').click(function () {
            if (tornaSu == 0) {
                var isVisible = $('#RisorseFinanziarieErogateContent').is(':visible');
                $('#RisorseFinanziarieErogateContent').slideToggle('fast');
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
        function Checkall(tabnm, chknm) {
            var strname = tabnm;
            var strchknm = chknm;
            // Go through all items of a check list control
            var table = document.getElementById(CostruisciId(strname));
           var cells = table.getElementsByTagName("span");
            var ctlr;
            var x;
          
            x = 0;



            for (var i = 0; i < cells.length; i++) {
           
                ctlr  = cells[i].firstChild;
             
                if (ctlr.type == 'checkbox') {
                    if (document.getElementById(ctlr.id).checked == true) {
                        x = x + 1
                      
                        if (isTipo(ctlr.id) == true) {
                            if (x > 1) {
                                ctlr.checked = false;
                                
                                alert(unescape("Non %E8 possibile effettuare una scelta multipla"));
                            }
                           
                        }
                       
                    }
                    
                }
            }
        }
        function isTipo(what) {
            var i = new RegExp("[s]");
            //if(!i.test(what)) { alert(" Il formato EMAIL non è valido."); 
            if (i.test(what)) {
                //alert(i.test(what))
                return true;
            } else {//alert(i.test(what)) 
                return false;
            }
        }
        function isTipoGruppo(what) {
            var i = new RegExp("[g]");
            //if(!i.test(what)) { alert(" Il formato EMAIL non è valido."); 
            if (i.test(what)) {
                //alert(i.test(what))
                return true;
            } else {//alert(i.test(what)) 
                return false;
            }

        }
    </script>
 <%--   <script event="onclick" for="TabStrip1">		
			if (TabStrip1.selectedIndex==0){
					document.Form1.txtFocus2.focus();
						}
			if (TabStrip1.selectedIndex==1){
					document.Form1.txtFocus3.focus();
						}
			if (TabStrip1.selectedIndex==2){
					document.Form1.txtFocus4.focus();
						}
			if (TabStrip1.selectedIndex==3){
					document.Form1.txtFocus5.focus();
						}
    </script>--%>
 <%--   <script language="javascript" event="onclick" for="cmdConferma">
		
		
		var strname="tblAttuazioneProgetto";
		var table = document.getElementById (strname);
        var cells = table.getElementsByTagName("span");
        var ctlr;
        var x ;
        var blnRispostaMultipla;
        blnRispostaMultipla=false;
        x=0
        
        for (var i = 0; i < cells.length; i++)
        {
			ctrl = cells[i].firstChild;
			//alert(ctrl.id)
			if (ctrl.type == 'checkbox')
			{
				//si tratta di una domanda a risposta multipla
				//alert(isTipo(ctrl.id))
				if (isTipo(ctrl.id)==false) {
					//alert(x)
					//devo controllare se ho già
					//se è selezionato il check incremento il contatore
					if(document.getElementById(ctrl.id).checked==true)
					{
						//devo controllare se ho già checkato una risposta ad una domanda multipla
						if (blnRispostaMultipla==false){
							x=x + 1
							blnRispostaMultipla=true;
						}
					}
				}
				//domanda a risposta singola
				else{
					//ripristino a false la variabile che controlla le risposte multiple
					blnRispostaMultipla=false;
					//se è selezionato il check incremento il contatore
					if(document.getElementById(ctrl.id).checked==true)
					{
						x=x + 1	
					}
				}
			}
		}
		
		//alert(x)
		
		blnRispostaMultipla=false;
		
		var strname2="tblFormazioneVolontari";
		var table2 = document.getElementById (strname2);
        var cells2 = table2.getElementsByTagName("span");
		 for (var i = 0; i < cells2.length; i++)
        {
			ctrl = cells2[i].firstChild;
			//alert(ctrl.id)
			if (ctrl.type == 'checkbox')
			{
				//si tratta di una domanda a risposta multipla
				if (isTipo(ctrl.id)==false) {
					//devo controllare se ho già
					//se è selezionato il check incremento il contatore
					if(document.getElementById(ctrl.id).checked==true)
					{
						//devo controllare se ho già checkato una risposta ad una domanda multipla
						if (blnRispostaMultipla==false){
							x=x + 1
							blnRispostaMultipla=true;
						}
					}
				}
				//domanda a risposta singola
				else{
					//ripristino a false la variabile che controlla le risposte multiple
					blnRispostaMultipla=false;
					//se è selezionato il check incremento il contatore
					if(document.getElementById(ctrl.id).checked==true)
					{
						x=x + 1	
					}
				}
			}
		}
		blnRispostaMultipla=false;
		
		var strname3="tblOperativitaVolontari";
		var table3 = document.getElementById (strname3);
        var cells3 = table3.getElementsByTagName("span");
		 for (var i = 0; i < cells3.length; i++)
        {
			ctrl = cells3[i].firstChild;
			//alert(ctrl.id)
			if (ctrl.type == 'checkbox')
			{
				//si tratta di una domanda a risposta multipla
				if (isTipo(ctrl.id)==false) {
					//devo controllare se ho già
					//se è selezionato il check incremento il contatore
					if(document.getElementById(ctrl.id).checked==true)
					{
						//devo controllare se ho già checkato una risposta ad una domanda multipla
						if (blnRispostaMultipla==false){
							x=x + 1
							blnRispostaMultipla=true;
						}
					}
				}
				//domanda a risposta singola
				else{
					//ripristino a false la variabile che controlla le risposte multiple
					blnRispostaMultipla=false;
					//se è selezionato il check incremento il contatore
					if(document.getElementById(ctrl.id).checked==true)
					{
						x=x + 1	
					}
				}
			}
			
		}
		blnRispostaMultipla=false;
		
		var strname4="tblRisorseFinanziarieErogate";
		var table4 = document.getElementById (strname4);
        var cells4 = table4.getElementsByTagName("span");
		 for (var i = 0; i < cells4.length; i++)
        {
			ctrl = cells4[i].firstChild;
			//alert(ctrl.id)
			if (ctrl.type == 'checkbox')
			{
				//si tratta di una domanda a risposta multipla
				if (isTipo(ctrl.id)==false) {
					//devo controllare se ho già
					//se è selezionato il check incremento il contatore
					if(document.getElementById(ctrl.id).checked==true)
					{
						//devo controllare se ho già checkato una risposta ad una domanda multipla
						if (blnRispostaMultipla==false){
							x=x + 1
							blnRispostaMultipla=true;
						}
					}
				}
				//domanda a risposta singola
				else{
					//ripristino a false la variabile che controlla le risposte multiple
					blnRispostaMultipla=false;
					//se è selezionato il check incremento il contatore
					if(document.getElementById(ctrl.id).checked==true)
					{
						x=x + 1	
					}
				}
			}
		}		
		//alert(x)
		//alert (x -  document.Form1.txtx.value)
		
		if (x < document.Form1.txtx.value)
		{ 
			alert("Attenzione! E' necessario effettuare la valutazione di tutti requisiti per procedere alla Conferma.") 
			//alert(document.Form1.txtx.value)
			//alert (blnRispostaMultipla)
			return false;
			}
			
    </script>--%>
  <%--  <script event="onclick" for="cmdRipristina">--%>
     <script type="text/javascript">

         function ConfermaRipristina() {
             var msg;
             msg = confirm("Attenzione. L'operazione riporta lo stato dei requisiti a non confermato. Continuare?");
             if (msg == false) {
                 return false;
             }
         }
    </script>

      <script type="text/javascript">

          function ConfermaApplica() {
              //    <script event="onclick" for="cmdApplica">
              var msg;
              msg = confirm("Attenzione. L'operazione applica i requisiti impostati a tutte le sedi della verifica non confermate. Continuare?");
              if (msg == false) {
                  return false;
              }
          }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="txtx" runat="server" />
    <asp:HiddenField ID="txtnome" runat="server" />
    <div class="firefox">
        <fieldset class="ContornoPaginaScroll">
            <legend>Requisiti</legend>
            <div class="wrapper" style="width: 100%">
                <div class="headers">
                    <h2>
                        <asp:Label ID="lblTitolo" runat="server" Text="Requisiti"></asp:Label></h2>
                </div>
                <div class="rowGroup" style="height: auto">
                    <asp:Label ID="LblErrore" runat="server" CssClass="msgErroreBig" ></asp:Label>
                    <asp:Label ID="lblmessaggio" runat="server" CssClass="msgConferma"></asp:Label>
                    <div class="RigaVuota">
                        &nbsp;</div>
                </div>
                <div class="row" id="div3" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="LblVerificatore" CssClass="label" AssociatedControlID="lblEnte" runat="server"
                            Text="Ente Proponente:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 85%">
                        <asp:TextBox ID="lblEnte" runat="server" CssClass="textbox" ReadOnly="True" Width="90%"></asp:TextBox>
                    </div>
                </div>
                <div class="row" id="div1" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label1" CssClass="label" AssociatedControlID="lblsede" runat="server"
                            Text="Sede Attuazione:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 85%">
                        <asp:TextBox ID="lblsede" runat="server" CssClass="textbox" ReadOnly="True" Width="90%"></asp:TextBox>
                    </div>
                </div>
                <div class="row" id="div2" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label2" CssClass="label" AssociatedControlID="LblIndirizzoSede" runat="server"
                            Text="Indirizzo:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 85%">
                        <asp:TextBox ID="LblIndirizzoSede" runat="server" CssClass="textbox" ReadOnly="True"
                            Width="90%"></asp:TextBox>
                    </div>
                </div>
                <div class="row" id="div4" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label3" CssClass="label" AssociatedControlID="LblComuneSede" runat="server"
                            Text="Comune:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 35%">
                        <asp:TextBox ID="LblComuneSede" runat="server" CssClass="textbox" ReadOnly="True"
                            Width="90%"></asp:TextBox>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label4" CssClass="label" AssociatedControlID="LblRegioneSede" runat="server"
                            Text="Regione:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 35%">
                        <asp:TextBox ID="LblRegioneSede" runat="server" CssClass="textbox" ReadOnly="True"
                            Width="90%"></asp:TextBox>
                    </div>
                </div>
                <div class="row" id="div5" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label5" CssClass="label" AssociatedControlID="LblTelefonoSede" runat="server"
                            Text="Telefono:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 35%">
                        <asp:TextBox ID="LblTelefonoSede" runat="server" CssClass="textbox" ReadOnly="True"
                            Width="90%"></asp:TextBox>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="Label6" CssClass="label" AssociatedControlID="LblFaxSede" runat="server"
                            Text="Fax:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 35%">
                        <asp:TextBox ID="LblFaxSede" runat="server" CssClass="textbox" ReadOnly="True" Width="90%"></asp:TextBox>
                    </div>
                </div>
            </div>
            <fieldset class="ContornoPagina" id="FidsetSezioni" runat="server">
                <legend>Sezioni</legend>
                <div class="row" style="height: auto">
                    <div id="AttuazionedelProgettoToggler" class="colHyperLink" style="width: 100%">
                        <asp:LinkButton ID="lnkAttuazionedelProgettoToggler" runat="server" Style="cursor: pointer" Text="Sezione Attuazione del Progetto"></asp:LinkButton>
                    </div>
                    <div id="FormazionedeiVolontariToggler" class="colHyperLink" style="width: 100%">
                        <asp:LinkButton ID="lnkFormazionedeiVolontariToggler" runat="server" Style="cursor: pointer" Text="Sezione Formazione dei Volontari"></asp:LinkButton>
                    </div>
                    <div id="OperativitadeiVolontariToggler" class="colHyperLink" style="width: 100%">
                        <asp:LinkButton ID="lnkOperativitadeiVolontariToggler" runat="server" Style="cursor: pointer" Text="Sezione Operativita' dei Volontari"></asp:LinkButton>
                    </div>
                    <div id="RisorseFinanziarieErogateToggler" class="colHyperLink" style="width: 100%">
                        <asp:LinkButton ID="lnkRisorseFinanziarieErogateToggler" runat="server" Style="cursor: pointer" Text="Sezione Risorse Finanziarie Erogate"></asp:LinkButton>
                    </div>
                </div>
            </fieldset>
            <fieldset id="flsAttuazionedelProgetto" class="ContornoPagina">
                <legend id="lgAttuazionedelProgetto">
                    <img src="images/minus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor: pointer;" />&nbsp;&nbsp;<img
                        id="TornaSuAttuazionedelProgetto" style="cursor: pointer;" onclick="TornaAdInizioPagina()"
                        src="images/tornasu_xsmall.png" title="Torna ad inizio pagina" alt="Torna ad inizio pagina" />&nbsp;&nbsp;Attuazione
                    del Progetto</legend>
                <div class="wrapper" id="AttuazionedelProgettoContent" style="width: 100%; border: 0px">
                    <asp:Panel ID="panelAttuazionedelProgetto" runat="server">
                    </asp:Panel>
                </div>
            </fieldset>
            <fieldset id="flsFormazionedeiVolontari" class="ContornoPagina">
                <legend id="lgFormazionedeiVolontari">
                    <img src="images/minus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor: pointer;" />&nbsp;&nbsp;<img
                        id="TornaSuFormazionedeiVolontari" style="cursor: pointer;" onclick="TornaAdInizioPagina()"
                        src="images/tornasu_xsmall.png" title="Torna ad inizio pagina" alt="Torna ad inizio pagina" />&nbsp;&nbsp;Formazione
                    dei Volontari</legend>
                <div class="wrapper" id="FormazionedeiVolontariContent" style="width: 100%; border: 0px">
                    <asp:Panel ID="panelFormazionedeiVolontari" runat="server">
                    </asp:Panel>
                </div>
            </fieldset>
            <fieldset id="flsOperativitadeiVolontari" class="ContornoPagina">
                <legend id="lgOperativitadeiVolontari">
                    <img src="images/minus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor: pointer;" />&nbsp;&nbsp;<img
                        id="TornaSuOperativitadeiVolontari" style="cursor: pointer;" onclick="TornaAdInizioPagina()"
                        src="images/tornasu_xsmall.png" title="Torna ad inizio pagina" alt="Torna ad inizio pagina" />&nbsp;&nbsp;Operativita'
                    dei Volontari</legend>
                <div class="wrapper" id="OperativitadeiVolontariContent" style="width: 100%; border: 0px">
                    <asp:Panel ID="panelOperativitadeiVolontari" runat="server">
                    </asp:Panel>
                </div>
            </fieldset>
            <fieldset id="flsRisorseFinanziarieErogate" class="ContornoPagina">
                <legend id="lgRisorseFinanziarieErogate">
                    <img src="images/minus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor: pointer;" />&nbsp;&nbsp;<img
                        id="TornaSuRisorseFinanziarieErogate" style="cursor: pointer;" onclick="TornaAdInizioPagina()"
                        src="images/tornasu_xsmall.png" title="Torna ad inizio pagina" alt="Torna ad inizio pagina" />&nbsp;&nbsp;Risorse
                    Finanziarie Erogate</legend>
                <div class="wrapper" id="RisorseFinanziarieErogateContent" style="width: 100%; border: 0px">
                    <asp:Panel ID="panelRisorseFinanziarieErogate" runat="server">
                    </asp:Panel>
                </div>
            </fieldset>
            <div class="wrapper" style="width:100%;border:0px">
            <div class="RigaPulsanti">
                <asp:Button ID="cmdApplica" CssClass="Pulsante" runat="server" Text="Applica a tutti" OnClientClick="return ConfermaApplica();" >
                </asp:Button>
                <asp:Button ID="cmdRipristina" CssClass="Pulsante" runat="server" Text="Ripristina" OnClientClick="return ConfermaRipristina();">
                </asp:Button>
                <asp:Button ID="cmdConferma" runat="server" ToolTip="Esporta" CssClass="Pulsante" 
                    Text="Conferma"></asp:Button>
                <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" Text="Salva"></asp:Button>
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"></asp:Button>
            </div>
            </div> 
        </fieldset>


<asp:HiddenField runat="server" ID="hdnIsPBAttuazionedelProgetto" />
<asp:HiddenField runat="server" ID="hdnIsPBFormazionedeiVolontari" />
<asp:HiddenField runat="server" ID="hdnIsPBOperativitadeiVolontari" />
<asp:HiddenField runat="server" ID="hdnIsPBRisorseFinanziarieErogate" />



    </div>

</asp:Content>
