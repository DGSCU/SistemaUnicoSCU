<%@ Page Title="GestioneProvvedimentoDisciplinare" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmGestioneProvvedimentoDisciplinare.aspx.vb" Inherits="Futuro.WfrmGestioneProvvedimentoDisciplinare" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
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
     /* <![CDATA[ */
        function SelProtocollo0() {
            var codFasc = document.getElementById(CostruisciId("TxtCodiceFasc"));
            if (codFasc == '') 
            {
                alert("Specificare il numero fascicolo!");

            }
            else 
            {
                myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?NumeroFascicolo=" + codFasc, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");
            }
            return false;
        }
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
//						var Val;
//						var ChiudiContestata;
//						Val=document.getElementById(CostruisciId('cmdChiusaContestata') ) ;
//						if (Val==null)
//						{
//							ChiudiContestata=0;
//						}
//						else
//						{
//							ChiudiContestata=1;
//						}ChiudiContestata=" + ChiudiContestata + "&
                         //myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?Classificazione=" + Classificazione + "&ChiudiContestata=" + ChiudiContestata + "&objForm=Form1&TxtProt=" + CostruisciId(NumProt) + "&TxtData=" + CostruisciId(DataProt) + "&NumeroFascicolo=" + codFasc, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");
                        myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?objForm=Form1+&TxtProt="+CostruisciId(NumProt)+"&TxtData="+CostruisciId(DataProt)+"&NumeroFascicolo=" + codFasc, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");			
						//myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?ChiudiContestata=" + ChiudiContestata + "&objForm=Form1&TxtProt="+CostruisciId(NumProt)+"&TxtData="+CostruisciId(DataProt)+"&NumeroFascicolo=" + codFasc, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");			
					}
                      return false;

			}
         /* ]]> */
    </script>
    <script type="text/javascript" language="JavaScript" >
           /* <![CDATA[ */
             $(function () {
                var clientId = "#" + CostruisciId('TxtDataProtocolloComunicazione');
                    $("" + clientId + "").datepicker();
             });
            $(function () {
                var clientId = "#" + CostruisciId('txtDataProtocolloAvvioProcedimento');
                    $("" + clientId + "").datepicker();
            }); 
            $(function () {
                var clientId = "#" + CostruisciId('txtDataProtocolloControdeduzioni');
                    $("" + clientId + "").datepicker();
            });
            $(function () {
                var clientId = "#" + CostruisciId('txtDataProtocolloChiusuraProvvedimento');
                    $("" + clientId + "").datepicker();
            });
            $(function () {
                var clientId = "#" + CostruisciId('txtDataProtocolloSanzione');
                    $("" + clientId + "").datepicker();
            });
     /* ]]> */
    </script>
    <script  type="text/javascript" language="JavaScript" >
      /* <![CDATA[ */
        var pathPlus = "images/plus_xsmall.png";
        var pathMinus = "images/minus_xsmall.png";
        var titlePathMinus = "Riduci";
        var titlePathPlus = "Espandi";
        var inserimento = '<%=Request.Params("IdAttivitàEntità")%>';
        var stato = '<%=lblDatoStatoProvedimento.Text%>';
        var DataProtocolloComunicazione='<%=TxtDataProtocolloComunicazione.Text%>';
        var inserimento1 = '<%=lblDatoStatoProvedimento.Text%>';
      
        /* ]]> */  
    </script>
  
      <script type="text/javascript" language="JavaScript" >
       /* <![CDATA[ */
        $(function () {
            var tornaSu = 0;
            //alert(inserimento);
            if (inserimento == "") {
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
            var tornaSu = 0;
            if (inserimento != "REGISTRATO")
             {
                if (DataProtocolloComunicazione =='') {
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
        <legend>Gestione Provvedimento Disciplinare</legend>
            <asp:HiddenField ID="TxtCodiceFasc" runat="server" />
            <asp:HiddenField ID="txtIDProvvedimentoDisciplinare" runat="server" />
            <div class="wrapper"  style="width:100%;border:0px">
                <div class="headers">
                    <h2>
                        <asp:Label ID="lblTitolo" runat="server" Text="Gestione Provvedimento Disciplinare del Volontario"/>
                    </h2>
                </div>
                <div class="RigaVuota">&nbsp;</div>
                <div class="rowGroup" style="height: auto">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblStatoProvedimento" CssClass="label" AssociatedControlID="lblDatoStatoProvedimento" runat="server"
                            Text="Stato Provvedimento"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 15%">
                        <asp:Label ID="lblDatoStatoProvedimento" CssClass="labelDati" runat="server"/>
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
                    <div class="row" style="height: auto"> </div
                        <legend>Sezioni</legend>
                    <div class="row" style="height: auto">
                        <div id="flsInformazioniGeneraliToggler" class="colHyperLink" style="width: 100%">
                            <asp:LinkButton ID="link6" runat="server" Style="cursor: pointer" Text="Informazioni Generali"></asp:LinkButton>
                        </div>
                        <div id="flsDatiSanzioneToggler" class="colHyperLink" style="width: 100%">
                            <asp:LinkButton ID="LinkButton1" runat="server" Style="cursor: pointer" Text="Dati Sanzione"></asp:LinkButton>
                        </div>
                    </div>
                </fieldset>
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
                    </div><br/>
                    <fieldset class="ContornoPagina">
                        <legend>Procolli</legend>
                        <div class="row"> 
                            <div class="collable" style="width: 25%" >
                                <asp:Label ID="LblDataProtocolloComunicazione" CssClass="label" runat="server"
                                    Text="Data Protocollo Comunicazione Ente" AssociatedControlID="TxtDataProtocolloComunicazione"/>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="TxtDataProtocolloComunicazione" runat="server" CssClass="textboxData"/>
                            </div>
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblNumeroProtocolloComunicazione" CssClass="label" runat="server"
                                    Text="N° Protocollo Comunicazione Ente" AssociatedControlID="TxtNumeroProtocolloComunicazione"/>
                            </div>
                            <div class="colOggetti" style="width: 20%">
                                <asp:TextBox ID="TxtNumeroProtocolloComunicazione" runat="server" CssClass="textbox"/>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:ImageButton ID="imgSelezionaProtComunicazione" runat="server" ImageUrl="Images/busta_small.png" ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('TxtNumeroProtocolloComunicazione','TxtDataProtocolloComunicazione')"/>
                                <asp:ImageButton ID="imgAllegatoProtComunicazione" runat="server" ImageUrl="images/documento_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('TxtNumeroProtocolloComunicazione','TxtDataProtocolloComunicazione')" Width="30px"/>
                                 <asp:ImageButton ID="imgCancProtComunicazione" runat="server" ImageUrl="images/canc_small.png" ToolTip="Rimuovi Protocollo Comunicazione" AlternateText="Rimuovi Protocollo Comunicazione" />

                            </div>
                        </div>
                                               
                    </fieldset>
                    <fieldset class="ContornoPagina">
                        <legend>Volontario</legend>
                            <div class="row">
                                <div class="collable" style="width: 20%">
                                    <asp:Label ID="Label1" CssClass="label" runat="server" Text="Volontario"
                                        AssociatedControlID="lblDatoVolontario" />
                                </div>
                                <div class="collable" style="width: 80%">
                                    <asp:Label ID="lblDatoVolontario" CssClass="labelDati" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="collable" style="width: 20%">
                                    <asp:Label ID="LblStatoVolontario" CssClass="label" runat="server" Text="Stato Volontario"
                                        AssociatedControlID="LblDatoStatoVolontario" />
                                </div>
                                <div class="colOggetti" style="width: 80%">
                                    <asp:Label ID="LblDatoStatoVolontario" CssClass="labelDati" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="collable" style="width: 20%">
                                    <asp:Label ID="lblDataInzioServizio" CssClass="label" runat="server" Text="Data Inizio Servizio"
                                        AssociatedControlID="lblDatoDataInzioServizio" />
                                </div>
                                <div class="colOggetti" style="width: 30%">
                                    <asp:Label ID="lblDatoDataInzioServizio" CssClass="labelDati" runat="server" />
                                </div>
                                <div class="collable" style="width: 20%">
                                    <asp:Label ID="lblDataFineServizio" CssClass="label" runat="server" Text="Data Fine Servizio"
                                        AssociatedControlID="lblDatoDataFineServizio" />
                                </div>
                                <div class="colOggetti" style="width: 30%">
                                    <asp:Label ID="lblDatoDataFineServizio" CssClass="labelDati" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="collable" style="width: 20%">
                                    <asp:Label ID="lblSede" CssClass="label" runat="server" Text="Sede"
                                        AssociatedControlID="lblDatoSede" />
                                </div>
                                <div class="collable" style="width: 80%">
                                    <asp:Label ID="lblDatoSede" CssClass="labelDati" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="collable" style="width: 20%">
                                    <asp:Label ID="LblProgetto" CssClass="label" runat="server" Text="Progetto"
                                        AssociatedControlID="LblDatoProgetto" />
                                </div>
                                <div class="collable" style="width: 80%">
                                    <asp:Label ID="LblDatoProgetto" CssClass="labelDati" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="collable" style="width: 20%">
                                    <asp:Label ID="lblEnte" CssClass="label" runat="server" Text="Ente Riferimento"
                                        AssociatedControlID="lblDatoEnte" />
                                </div>
                                <div class="collable" style="width: 80%">
                                    <asp:Label ID="lblDatoEnte" CssClass="labelDati" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="collable" style="width: 20%">
                                    <asp:Label ID="lblEnteProg" CssClass="label" runat="server" Text="Ente Progetto"
                                        AssociatedControlID="lblDatoEnteProg" />
                                </div>
                                <div class="collable" style="width: 80%">
                                    <asp:Label ID="lblDatoEnteProg" CssClass="labelDati" runat="server" />
                                </div>
                            </div>
                        </fieldset> 
                    </div>
                </fieldset>
                <fieldset id="flsDatiSanzione" class="ContornoPagina">
                    <legend id="DatiSanzione">
                    <img src="images/plus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor: pointer;" />&nbsp;&nbsp; <img
                        id="TornaSuDatiSanzione" style="cursor: pointer;" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"
                        title="Torna ad inizio pagina" alt="Torna ad inizio pagina" />&nbsp;&nbsp;Dati Sanzione</legend>
                    <div class="wrapper" id="DatiSanzioneContent" style="width: 100%; border: 0px">
                        <div class="row" id="dvDGSanzione" runat="server">
                            <div class="collable" style="width: 25%" >
                                <asp:Label ID="LblDataProtocolloAvvioProvedimento" CssClass="label" runat="server"
                                    Text="Data Protocollo Avvio Provvedimento" 
                                    AssociatedControlID="txtDataProtocolloAvvioProcedimento"/>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="txtDataProtocolloAvvioProcedimento" runat="server" CssClass="textboxData"
                                    ></asp:TextBox>
                            </div>
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblNumeroProtocolloAvvioProcedimento" CssClass="label" runat="server"
                                    Text="N° Protocollo Avvio Provvedimento" 
                                    AssociatedControlID="TxtNumeroProtocolloAvvioProcedimento"/>
                            </div>
                            <div class="colOggetti" style="width: 20%">
                                <asp:TextBox ID="TxtNumeroProtocolloAvvioProcedimento" runat="server" CssClass="textbox"/>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:ImageButton ID="ImgProtocolloAvvioProcedimento" runat="server" ImageUrl="Images/busta_small.png" 
                                    ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('TxtNumeroProtocolloAvvioProcedimento','txtDataProtocolloAvvioProcedimento')" />
                                <asp:ImageButton ID="ImgApriAllegatiAvvioProcedimento" runat="server" ImageUrl="images/documento_small.png"
                                    ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('TxtNumeroProtocolloAvvioProcedimento','txtDataProtocolloAvvioProcedimento')" Width="30px"/>
                                <asp:ImageButton ID="imgCancAvvioProcedimento" runat="server" ImageUrl="images/canc_small.png" ToolTip="Rimuovi Protocollo Avvio Provvedimento" AlternateText="Rimuovi Protocollo Avvio Provvedimento" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblDataProtocolloControdeduzioni" CssClass="label" runat="server"
                                    Text="Data Protocollo Controdeduzioni" AssociatedControlID="txtDataProtocolloControdeduzioni"/>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="txtDataProtocolloControdeduzioni" runat="server" CssClass="textboxData"/>
                            </div>
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblNumeroProtocolloControdeduzioni" CssClass="label" runat="server"
                                    Text="N° Protocollo Controdeduzioni" 
                                    AssociatedControlID="TxtNumeroProtocolloControdeduzioni"/>
                            </div>
                            <div class="colOggetti" style="width: 20%">
                                <asp:TextBox ID="TxtNumeroProtocolloControdeduzioni" runat="server" CssClass="textbox"/>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:ImageButton ID="ImgProtocolloControdeduzioni" runat="server" ImageUrl="Images/busta_small.png"
                                    ToolTip="Seleziona Protocollo"  AlternateText="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('TxtNumeroProtocolloControdeduzioni','txtDataProtocolloControdeduzioni')"/>
                                <asp:ImageButton ID="ImgApriAllegatiControdeduzioni" runat="server" ImageUrl="images/documento_small.png"
                                    ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('TxtNumeroProtocolloControdeduzioni','txtDataProtocolloControdeduzioni')" Width="30px"/>
                                <asp:ImageButton ID="imgCancControdeduzioni" runat="server" ImageUrl="images/canc_small.png" ToolTip="Rimuovi Protocollo Controdeduzioni" AlternateText="Rimuovi Protocollo Controdeduzioni" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblDataProtocolloChiusuraProvvedimento" CssClass="label" runat="server" Text="Data Protocollo Chiusura Provvedimento"
                                    AssociatedControlID="txtDataProtocolloChiusuraProvvedimento"/>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="txtDataProtocolloChiusuraProvvedimento" runat="server" CssClass="textboxData"/>
                            </div>
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblNProtocolloChiusuraProvvedimento" CssClass="label" 
                                    runat="server" Text="N° Protocollo Chiusura Provvedimento"
                                    AssociatedControlID="txtNProtocolloChiusuraProvvedimento" />
                            </div>
                            <div class="colOggetti" style="width: 20%">
                                <asp:TextBox ID="txtNProtocolloChiusuraProvvedimento" runat="server" CssClass="textbox" />
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:ImageButton ID="ImgProtocolloChiusuraProvvedimento" runat="server" ImageUrl="Images/busta_small.png"
                                    ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('txtNProtocolloChiusuraProvvedimento','txtDataProtocolloChiusuraProvvedimento')"/>
                                <asp:ImageButton ID="ImgApriAllegatiChiusuraProvvedimento" runat="server" ImageUrl="images/documento_small.png"
                                    ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('txtNProtocolloChiusuraProvvedimento','txtDataProtocolloChiusuraProvvedimento')" Width="30px"/>
                                <asp:ImageButton ID="imgCancChiusuraProvvedimento" runat="server" ImageUrl="images/canc_small.png" ToolTip="Rimuovi Protocollo Chiusura Provvedimento" AlternateText="Rimuovi Protocollo Chiusura Provvedimento" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblDataProtocolloSanzione" CssClass="label" runat="server" Text="Data Protocollo Sanzione" AssociatedControlID="txtDataProtocolloSanzione"/>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="txtDataProtocolloSanzione" runat="server" CssClass="textboxData"/>
                            </div>
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="LblNumeroProtocolloSanzione" CssClass="label" runat="server" 
                                    Text="N° Protocollo Sanzione" AssociatedControlID="txtNProtocolloSanzione"/>
                            </div>
                            <div class="colOggetti" style="width: 20%">
                                <asp:TextBox ID="txtNProtocolloSanzione" runat="server" CssClass="textbox" />
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:ImageButton ID="ImgProtocolloSanzione" runat="server" ImageUrl="Images/busta_small.png"
                                    ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('txtNProtocolloSanzione','txtDataProtocolloSanzione')"/>
                                <asp:ImageButton ID="ImgApriAllegatiSanzione" runat="server" ImageUrl="images/documento_small.png"
                                    ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('txtNProtocolloSanzione','txtDataProtocolloSanzione')" Width="30px"/>
                                <asp:ImageButton ID="imgCancSanzione" runat="server" ImageUrl="images/canc_small.png" ToolTip="Rimuovi Protocollo Sanzione" AlternateText="Rimuovi Protocollo Sanzione" />
                            </div>
                        </div>
                         <br />
                         <br />
                            <div class="row">
                                <div class="collable" style="width: 20%;">
                                    
                                    <asp:Label ID="lblLetteraAvvioProvvDisciplinare" CssClass="label" runat="server" Text="Lettera Avvio Provvedimento" AssociatedControlID="cmdLetteraAvvio" />
                                </div>
                                <div class="colOggetti" style="width: 5%">
                                    <asp:ImageButton ID="cmdLetteraAvvio" runat="server"  AlternateText="LetteraAvvio" ToolTip="Lettara Avvio Provvedimento Disciplinare" ImageUrl="images/stampa_small.png" />
                                </div>
                                <div class="collable" style="width: 20%">
                                    
                                    <asp:Label ID="lblLetteraChiusuraProvvDisciplinare" CssClass="label" runat="server"  Text="Lettera Chiusura Provvedimento" AssociatedControlID="cmdLetteraChiusura" />
                                </div>
                                <div class="colOggetti" style="width: 5%">
                                    <asp:ImageButton ID="cmdLetteraChiusura" runat="server"  AlternateText= "LetteraChiusura" ToolTip="Lettara Chiusura Provvedimento Disciplinare" ImageUrl="images/stampa_small.png"/>
                                </div>
                                <div class="collable" style="width: 20%">
                                    
                                    <asp:Label ID="lblLetteraSanzione" CssClass="label" runat="server" Text="Lettera Sanzione Disciplinare" AssociatedControlID="cmdLetteraSanzione" />
                                </div>
                                <div class="colOggetti" style="width: 5%">
                                    <asp:ImageButton ID="cmdLetteraSanzione" runat="server" ToolTip="Lettera Sanzione Disciplinare" AlternateText="LetteraSanzione" ImageUrl="images/stampa_small.png"/>
                                </div>
                                <div class="collable" style="width:  5%">&nbsp;</div>
         
                                 <div class="colOggetti" style="width: 5%">&nbsp;</div>
                            </div>
                      
                    </div>
                </fieldset>
                <div class="row" id="div11" runat="server">
                    <div class="collable" style="width: 20%">
                        <asp:Label ID="Label2" CssClass="label" runat="server" Text="NOTE:" AssociatedControlID="TxtNote" />
                    </div>
                </div>
                <div class="row" id="div12" runat="server">
                    <div class="collable" style="width: 100%">
                        <asp:TextBox ID="TxtNote" CssClass="textbox" TextMode="MultiLine" runat="server" Width="98%" />
                    </div>
                </div>
                <div class="row" id="div13" runat="server">
                </div>

                <div class="RigaPulsanti">
                    <asp:Button ID="cmdAnnulla" CssClass="Pulsante" runat="server" Text="Annulla Provvedimento" />
                    <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" Text="Salva" />
                    <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"></asp:Button>
                </div>
            </div>
        </fieldset>
 </div>
</asp:Content>
