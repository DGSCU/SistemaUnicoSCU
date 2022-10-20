<%@ Page Title="AnagraficaVolontario" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmVolontari.aspx.vb" Inherits="Futuro.WfrmVolontari" MaintainScrollPositionOnPostBack="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
            var prefissoIdClient = "MainContent_";

            function CostruisciId(IdServer) {
                var IdClient = prefissoIdClient + IdServer
                return IdClient
            }

            function SetContextKey(tipologiaIndirizzo) {
                if (tipologiaIndirizzo == 'RESIDENZA') {
                    $find('<%=AutoCompleteExtender1.ClientID%>').set_contextKey($get("<%=ddlComuneResidenza.ClientID %>").value);
                }
                else if (tipologiaIndirizzo == 'DOMICILIO') {
                    $find('<%=AutoCompleteExtender2.ClientID%>').set_contextKey($get("<%=ddlComuneDomicilio.ClientID %>").value);
                }
            };

                    </script>
    <script  type="text/javascript">          
           $(function () {
               var IdData = CostruisciId('txtDataInizioVolontarioSubentrato');
               var sharpIdData = "#" + IdData
               $("" + sharpIdData + "").datepicker();
           });
           $(function () {
               var IdData = CostruisciId('txtDataNascita');
               var sharpIdData = "#" + IdData
               $("" + sharpIdData + "").datepicker();
           });
           $(function () {
               var IdData = CostruisciId('TxtDataDomanda');
               var sharpIdData = "#" + IdData
               var IdVolontario ='<%= Request.Params("IdVol")%>';
               if( IdVolontario==""){
                $("" + sharpIdData + "").datepicker();
               }
           });

        </script>
    <script  type="text/javascript">

               function CheckNumeroIntero(valore, descrizione) {    
                   ValidaNumeroIntero(CostruisciId(valore), descrizione, CostruisciId("MsgErroreClient"));
               };

               function CheckNumeroValido(valore, descrizione) {
                   ValidaNumero(CostruisciId(valore), descrizione, CostruisciId("MsgErroreClient"));
               };

               function CheckEmail(txtEmail, descrizione) {
                   ValidaEmail(CostruisciId(txtEmail), descrizione, CostruisciId("MsgErroreClient"));
               };
          function VisualizzaAtt() {
            var IdVolontario = '<%= Request.Params("IdVol")%>';
            var winOption = PopUpOption();
            myWin = window.open("WfrmAttivitaVolontari.aspx?IdVolontario=" + IdVolontario, "dettagli", winOption);
          }; 

		function VisualizzaAss(){
            var IdVolontario ='<%= Request.Params("IdVol")%>';
            var winOption = PopUpOption();
			myWin = window.open ("riepilogoassenzevolontario.aspx?identita=" + IdVolontario,"dettagli", winOption)	
			return false;
        };
   
        
            function ValidaCampi() {
                var comuneDomicilio = "Selezionare Provincia/Nazione di Domicilio";
                var comuneResidenza= "Selezionare Provincia/Nazione di residenza";
                AggiornaErrore("", CostruisciId("MsgErroreClient"));
                AggiornaErrore("", CostruisciId("lblErr"));
                CongruenzaCodiceFiscale(document.getElementById(CostruisciId("txtCodiceFiscale")).value, document.getElementById(CostruisciId("txtCognome")).value, document.getElementById(CostruisciId("txtNome")).value, document.getElementById(CostruisciId("cboSesso")).value, document.getElementById(CostruisciId("txtDataNascita")).value, CostruisciId("MsgErroreClient"));
                VerificaCampoObbligatorio(CostruisciId("txtCodiceFiscale"), "Codice Fiscale", CostruisciId("MsgErroreClient"));
                VerificaCampoObbligatorio(CostruisciId("txtCognome"), "Cognome", CostruisciId("MsgErroreClient"));
                VerificaCampoObbligatorio(CostruisciId("txtNome"), "Nome", CostruisciId("MsgErroreClient"));
                VerificaCampoObbligatorio(CostruisciId("txtDataNascita"), "Data di Nascita", CostruisciId("MsgErroreClient"));
                VerificaCampoObbligatorio(CostruisciId("txtIndirizzo"), "Indirizzo di residenza", CostruisciId("MsgErroreClient"));
                VerificaCampoObbligatorio(CostruisciId("txtCivico"), "Civico di residenza", CostruisciId("MsgErroreClient"));
                VerificaCampoObbligatorio(CostruisciId("txtCAP"), "C.A.P. di residenza", CostruisciId("MsgErroreClient"));
                VerificaCampoObbligatorio(CostruisciId("txtPunteggio"), "Punteggio volontario", CostruisciId("MsgErroreClient"));
                VerificaDataValida(document.getElementById(CostruisciId("txtDataNascita")).value, "Data di Nascita", CostruisciId("MsgErroreClient"));
                 VerificaDataValida(document.getElementById(CostruisciId("TxtDataDomanda")).value, "Data Domanda", CostruisciId("MsgErroreClient"));

                if (document.getElementById(CostruisciId("ddlProvinciaResidenza")).value == 0) {
                    var errore = "Il campo 'Provincia di Residenza' è obbligatorio.";
                    ConcatenaErrore(errore, CostruisciId("MsgErroreClient"));
                }
                if (document.getElementById(CostruisciId("ddlComuneResidenza")).value == comuneResidenza || document.getElementById(CostruisciId("ddlComuneResidenza")).value == 0) {
                    var errore = "Il campo 'Comune di Residenza' è obbligatorio.";
                    ConcatenaErrore(errore, CostruisciId("MsgErroreClient"));
                }

                countCodLib = document.getElementById(CostruisciId("txtLibretto")).value
                if (countCodLib.length > 0 && countCodLib.length < 12) {
                    var errore = "La lunghezza del codice del libretto postale errata.";
                    ConcatenaErrore(errore, CostruisciId("MsgErroreClient"));
                }

                if (document.getElementById(CostruisciId("CboSedeAtt")).value == 'Selezionare') {
                    var errore = "Inserire il codice della Sede di Attuazione del progetto.";
                    ConcatenaErrore(errore, CostruisciId("MsgErroreClient"));
                }
                if (document.getElementById(CostruisciId("CboSedeAtt")).value == 0) {
                    var errore = "Inserire il codice della Sede di Attuazione del progetto.";
                    ConcatenaErrore(errore, CostruisciId("MsgErroreClient"));
                }

                if (document.getElementById(CostruisciId("txtIBAN")).value == '' && document.getElementById(CostruisciId("txtBicSwift")).value != '') {
                    var errore = "Inserire il Codice IBAN del volontario.";
                    ConcatenaErrore(errore, CostruisciId("MsgErroreClient"));
                }
                if (document.getElementById(CostruisciId("txtIBAN")).value != '' && document.getElementById(CostruisciId("txtLibretto")).value != '') {
                    var errore = "Non è possibile inserire Codice IBAN e Libretto Postale.";
                    ConcatenaErrore(errore, CostruisciId("MsgErroreClient"));
                }

                var countIban = '';
                var IbanIta = '';
                countIban = document.getElementById(CostruisciId("txtIBAN")).value;
                document.getElementById(CostruisciId("txtIBAN")).value = countIban.toUpperCase();
                IbanIta = countIban.slice(0, 2);
                var countBic = '';
                countBic = document.getElementById(CostruisciId("txtBicSwift")).value;
                document.getElementById(CostruisciId("txtBicSwift")).value = countBic.toUpperCase();

                // CONTROLLO L'IBAN SOLO SE è UN CONTO ITALIANO

                if (IbanIta == 'IT') {
                    if (countIban.length > 0 && countIban.length < 27) {
                        var errore = "La lunghezza del codice IBAN è errata.";
                        ConcatenaErrore(errore, CostruisciId("MsgErroreClient"));
                    }
                }

                //alert(IbanIta)
                if (IbanIta == 'IT') {
                    if (countBic.length != 0 && countBic.length != 8 && countBic.length != 11) {
                        var errore = "Lunghezza del BIC/SWIFT errata.";
                        ConcatenaErrore(errore, CostruisciId("MsgErroreClient"));
                    }
                }
                else {
                    if (countBic.length > 20) {
                        var errore = "Lunghezza del BIC/SWIFT errata..";
                        ConcatenaErrore(errore, CostruisciId("MsgErroreClient"));
                    }
                }
                var countCodLib = '';
                countCodLib = document.getElementById(CostruisciId("txtLibretto")).value;
                if (countCodLib.length > 0 && countCodLib.length < 12) {
                    var errore = "Lunghezza del codice del libretto postale errata.";
                    ConcatenaErrore(errore, CostruisciId("MsgErroreClient"));
                }
                var CampiValidi = 0;
                if (
                                 (document.getElementById(CostruisciId("ddlProvinciaDomicilio")).value == 0)
                                 && (document.getElementById(CostruisciId("ddlComuneDomicilio")).value == comuneDomicilio)
        	                    && (document.getElementById(CostruisciId("txtIndirizzoDomicilio")).value == '')
        	                    && (document.getElementById(CostruisciId("txtCivicoDomicilio")).value == '')
        	                    && (document.getElementById(CostruisciId("txtCapDomicilio")).value == '')
        	                    && (document.getElementById(CostruisciId("txtTelefonoDomicilio")).value == '')
        	                   ) {
                    CampiValidi = 1;
                }
                if (
                                 (document.getElementById(CostruisciId("ddlProvinciaDomicilio")).value != 0)
                                  && (document.getElementById(CostruisciId("ddlComuneDomicilio")).value != comuneDomicilio)
        	                    && (document.getElementById(CostruisciId("txtIndirizzoDomicilio")).value != '')
        	                    && (document.getElementById(CostruisciId("txtCivicoDomicilio")).value != '')
        	                    && (document.getElementById(CostruisciId("txtCapDomicilio")).value != '')
        	                   ) {
                    CampiValidi = 1;
                }

                if (CampiValidi == 0) {
                    var errore = "Completare le informazioni relative al domicilio fiscale o rimuoverle completatente.";
                    ConcatenaErrore(errore, CostruisciId("MsgErroreClient"));
                }

                var risultato = document.getElementById(CostruisciId("MsgErroreClient")).value;
                if (risultato == '' || risultato == null) {
                    return true;
                } else {
                    document.getElementById(CostruisciId("MsgErroreClient")).scrollIntoView();
                    return false;
                }

            };
  

            function VerificaPreesistenzaIBAN(){
                var iban = document.getElementById(CostruisciId("txtIBAN")).value;
                var cf = document.getElementById(CostruisciId("txtCodiceFiscale")).value;
                var winOption = PopUpOption();
                if (iban!=''){
				    window.open("risultatoricercaIbanVolontari.aspx?CodiceIBAN=" + iban + "&CodiceFiscale=" + cf,"",winOption);		
			    }
            };


      	    function SelProtocollo0() {
                var winOption = PopUpOption();
      	        var codiceFascicolo = CostruisciId("TxtCodiceFascicolo");
      	        var NumFascicolo = '<%= TxtIdFascicolo.value %>';

      	        if (codiceFascicolo == '') {
      	            alert("Specificare il numero fascicolo!");
      	        } else {
      	            myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?NumeroFascicolo=" + NumFascicolo, "Visualizza", winOption);
      	        }
      	        return false;
      	    };

      	    function ElencoFascicoli() {
                 var winOption = PopUpOption();
      	        var IdTxtCodiceFasc = CostruisciId("TxtIdFascicolo");
      	        var IdTxtCodiceFascicolo = CostruisciId("TxtCodiceFascicolo");
      	        var IdDescrizioneFascicolo = CostruisciId("txtDescFasc");
      	        var codiceFascicolo = document.getElementById(IdTxtCodiceFascicolo).value;
                var NumFascicolo = '<%= TxtCodiceFascicolo.Text %>';
                var CodiceVolontario = '<%= txtCodicevolontario.Text %>';
      	        formId = document.forms[0];

      	        if (codiceFascicolo != '') 
                    //msg = confirm("Con questa operazione verranno cancellati definitivamente tutti protocolli associati! Procedere con l'operazione")
                    //if (msg == false) {
                    //  return false;
                    //} else
                    //{  }
                {//&Maschera=Volontario&CodiceVolontario=" + frmMain.txtCodicevolontario.value + "
      	            myWin = window.open("WfrmSIGEDElencoFascicoli.aspx?Processo=VOLONTARI&Maschera=Volontario&CodiceVolontario=" + CodiceVolontario + "&objForm=" + formId + "&objNumero=" + IdTxtCodiceFascicolo + "&objCodice=" + IdTxtCodiceFasc + "&objDescFasc=" + IdDescrizioneFascicolo + "&IdFascicolo=" + codiceFascicolo, "Visualizza", winOption); 
      	        } 
                else 
                {
                //alert(NumFascicolo);objNumero
      	            myWin = window.open("WfrmSIGEDElencoFascicoli.aspx?Processo=VOLONTARI&Maschera=Volontario&CodiceVolontario=" + CodiceVolontario + "&objForm=" + formId + "&objNumero=" + IdTxtCodiceFascicolo + "&objCodice=" + IdTxtCodiceFasc + "&objDescFasc=" + IdDescrizioneFascicolo + "&IdFascicolo=" + codiceFascicolo, "Visualizza", winOption);
      	        }
      	        return false;
      	    };

            function Uploadfile(){
                var winOption = PopUpOption();
                var CodiceVolontario = '<%= txtCodicevolontario.Text %>';
                var IdTxtCodiceFascicolo = CostruisciId("TxtCodiceFascicolo");
                var codiceFascicolo = document.getElementById(IdTxtCodiceFascicolo).value;
                var IdDescrizioneFascicolo = CostruisciId("txtDescFasc");
                var DescrizioneFascicolo = document.getElementById(IdDescrizioneFascicolo).value;
                var NumFascicolo = '<%= TxtIdFascicolo.value %>';
			    window.open("WfrmSIGEDProtocollazione.aspx?CorrCodUnivoco=" + CodiceVolontario +"&Processo=VOLONTARI&VArUpdate=0&Fascicolo=" + codiceFascicolo+"&NumeroFascicolo=" + NumFascicolo+"&DescFascicolo=" + DescrizioneFascicolo + "&CodEnte=<%= Session("txtCodEnte")%>", "Visualizza", winOption);
            };

            function ApriAllegati(NumProt,DataProt)
			{
                 var winOption = PopUpOption();
                 var NumFascicolo = document.getElementById(CostruisciId("TxtCodiceFascicolo")).Value
				if (NumFascicolo =='')
				{
					alert ("Nessun Fascicolo Selezionato!");
				}else{	
					myWin = window.open("WfrmSIGEDElencoAllegati.aspx?NumeroProtocollo=" + CostruisciId(NumProt) + "&DataProtocollo=" + CostruisciId(DataProt)+ "&CodiceFascicolo=" + NumFascicolo,"Visualizza",winOption);		
				}
			};

            function SelProtocollo(NumProt,DataProt,TipoDocumento)
			{
             var winOption = PopUpOption();
             var NumFascicolo = document.getElementById(CostruisciId("TxtCodiceFascicolo")).Value
				if (NumFascicolo =='')
				{
					alert ("Nessun Fascicolo Selezionato!");
				}else{
					myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?objForm=Form1&TxtProt="+CostruisciId(NumProt)+"&VarVolontario=<%=Request.QueryString("IdVol")%>&TxtData="+CostruisciId(DataProt)+"&VArUpdate=1&TipoDocumento="+TipoDocumento+"&NumeroFascicolo=<%=Request.QueryString("NumeroFascicolo")%>", "Visualizza", winOption);				
					
				}
			};
            function ApriComunicazione() {
		        var winOption = PopUpOption();
		        window.open("WfrmComunicazione.aspx", "InformazioneDettaglioRecapito", winOption)
		    };

            function Protocollazione(NumProt,DataProt)
            {
                var winOption = PopUpOption();
                var IdTxtCodiceFascicolo = CostruisciId("TxtCodiceFascicolo");
                var codiceFascicolo = document.getElementById(IdTxtCodiceFasc).value;
                var IdDescrizioneFascicolo = CostruisciId("txtDescFasc");
                var DescrizioneFascicolo = document.getElementById(IdDescrizioneFascicolo).value;
                var NumFascicolo = '<%= TxtIdFascicolo.value %>';
            	if (document.getElementById(NumProt).value=='' || document.getElementById(CostruisciId(DataProt)).value=='')
				{							  
					myWin = window.open("WfrmSIGEDProtocollazione.aspx?objForm=frmMain&TxtFasc=MainContent_TxtIdFascicolo&TxtCodFasc=MainContent_TxtCodiceFascicolo&TxtDescFascicolo=MainContent_txtDescFasc&TxtProt="+CostruisciId(NumProt)+"&TxtData="+CostruisciId(DataProt)+"&Fascicolo=" + codiceFascicolo+"&NumeroFascicolo=" +NumFascicolo+"&DescFascicolo=" + DescrizioneFascicolo + "&CodEnte=<%= Session("pCodEnte")%>", "Visualizza",winOption);			
			    }
             };



  
		</script>
  
		
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
         <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
    <fieldset class="ContornoPagina">
      <legend>Scheda Volontario</legend>

        <div class="wrapper" style="width:100%;border:0px">
        <div class="headers" >
            <h2>
            <asp:Label ID="Label5" runat="server"  Text="ANAGRAFICA VOLONTARIO"></asp:Label>
            </h2>
        </div>
        <div class="RigaVuota" style="height:auto">
            <asp:label id="lblErr" runat="server" CssClass="msgErrore" ></asp:label>
            <asp:label  id="MsgErroreClient" runat="server" CssClass="msgErrore"></asp:label>
             <asp:label id="lblConferma" runat="server" CssClass="msgConferma"></asp:label>
            <asp:HiddenField id="TxtIdFascicolo" runat="server"/>
            <asp:HiddenField id="txtCodLibPost" runat="server"/>
            <asp:HiddenField id="HdControlloIdFascicolo"  runat="server" /> 
			<asp:HiddenField id="lblidVolontario" runat="server"  />
			<asp:HiddenField id="lblidattivitasedeassegnazione" runat="server"  />
			<asp:HiddenField id="lblidEntesede" runat="server"  />
			<asp:HiddenField id="lblpresenta" runat="server"  />
			<asp:HiddenField id="lblidAttivita" runat="server"  />
			<asp:HiddenField id="lblidComuneNascita" runat="server"  />
			<asp:HiddenField id="lblidComuneResidenza" runat="server"  />
			<asp:HiddenField id="lblidComuneDomicilio" runat="server"  />
			<asp:HiddenField id="lblIDAttivitaEntita" runat="server"/>
			<asp:HiddenField id="txtvisibile" Runat="server" />
			<asp:HiddenField id="TxtProvinciaDomN" Runat="server" />
			<asp:HiddenField id="txtIndirizzoDomN" Runat="server" />
			<asp:HiddenField id="txtCivicoDomN" Runat="server" />
			<asp:HiddenField id="txtCapDomN" Runat="server" />
			<asp:HiddenField id="txtTelefonoDomN" Runat="server" />
			<asp:HiddenField id="txtComuneDomN" Runat="server" />
            <asp:HiddenField id="IdComuneNascita" Runat="server" />
            <asp:HiddenField id="IdComuneResidenza" Runat="server" />
        </div>
        <div class="RigaVuota">
            &nbsp;
        </div>
        <fieldset class="ContornoPagina">
            <legend>Operatività Volontario</legend>
            <div class="wrapper" style="width:100%;border:0px">
                <div class="rowGroup" style="height:auto"> 
                    <div class="row" style="height:auto">                  
                        <div class="colHyperLink" style="width:50%">
                            <asp:LinkButton ID="hlDocumentazione" runat="server" style="cursor:pointer" Text="Elenco Documentazione" Visible="true"></asp:LinkButton>
                        </div>
                        <div class="colHyperLink" style="width:50%">
                            <asp:LinkButton ID="hlCrono" runat="server" style="cursor:pointer" Text="Visualizza Cronologia Volontario" OnClientClick="javascript: VisualizzaAtt()" Visible="true"></asp:LinkButton>
                        </div> 
                    </div>
                    <div class="row" style="height:auto">   
                        <div class="colHyperLink" style="width:50%">
                            <asp:LinkButton ID="ImgGrad"  runat="server" style="cursor:pointer" Text="Visualizza Graduatoria Volontario" Visible="false"></asp:LinkButton>
                        </div>
                        <div class="colHyperLink" style="width:50%;display:none">
                            <asp:LinkButton ID="hlAssenzeVolontario"  OnClientClick="javascript:VisualizzaAss()" runat="server" style="cursor:pointer" Text="Visualizza Assenze Volontario" Visible="false"></asp:LinkButton>
                        </div>
                    </div>
                    <div class="row" style="height:auto">    
                        <div class="colHyperLink" style="width:50%">
                            <asp:LinkButton ID="imgChiusuraIniziale"   runat="server" style="cursor:pointer" Text="Chiusura Iniziale" Visible="false"></asp:LinkButton>
                        </div>
                        <div class="colHyperLink" style="width:50%">
                            <asp:LinkButton ID="imgChiusuraServizio"   runat="server" style="cursor:pointer" Text="Chiusura in Servizio" Visible="false"></asp:LinkButton>
                        </div>
                    </div>
                    <div class="row" style="height:auto">  
                        <div class="colHyperLink" style="width:50%">
                            <asp:LinkButton ID="imgSostituisciVol"  runat="server" style="cursor:pointer" Text="Sostituisci Volontario" Visible="false"></asp:LinkButton>
                        </div>  
                        <div class="colHyperLink" style="width:50%">
                            <asp:LinkButton ID="ImgAssocia"  runat="server" style="cursor:pointer" Text="Assegna Volontario" Visible="false"></asp:LinkButton>
                        </div>
                    </div>
                    <div class="row" style="height:auto"> 
                        <div class="colHyperLink" style="display:none">
                            <asp:LinkButton ID="imgRiepilogoDocumentiVol" runat="server" style="cursor:pointer" Text="Riepilogo Documenti Volontario" Visible="false"></asp:LinkButton>
                        </div>
                        <div class="colHyperLink">
                            <asp:LinkButton id="HplScaricaContratto" runat="server" style="cursor:pointer"  Text="Download Contratto"  Visible="false" ></asp:LinkButton>
                        </div>
                    </div>
                    <div class="row" style="height:auto">                  
                        <div class="colHyperLink" style="width:50%">
                            <asp:LinkButton ID="hlVisualizzaDocVolontario" runat="server" 
                                style="cursor:pointer" Text="Gestione Documenti Volontario" Visible="false"></asp:LinkButton>
                        </div>
                    </div>
                     <div class="row" style="height:auto">                  
                        <div class="colHyperLink" style="width:50%">
                            <asp:LinkButton ID="hplInfoPaghe" runat="server" 
                                style="cursor:pointer" Text="Info Paghe" ></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
       </fieldset>


          
        
        
        <div class="row">
             <div class="collable" style="width:15%">
               <asp:label id="Label1" AssociatedControlID="lblStatoVol" CssClass="label" runat="server" Text=" Stato Volontario" />
            </div>    
            <div class="colOggetti" style="width:35%">
               <asp:label id="lblStatoVol" CssClass="labelDati" runat="server" />
            </div>
    
            </div>
           <div id ="IdDivDateServizio" class="rowGroup" runat="server" style="display:none">
            <div class="row">
               <div class="collable" style="width:15%">
               <asp:label id="LbltxtDataInizioVolontarioSubentrato" AssociatedControlID="txtDataInizioVolontarioSubentrato" CssClass="label" runat="server" Visible="False" />
               </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtDataInizioVolontarioSubentrato" CssClass="textboxData" MaxLength="10" Visible="false" runat="server"></asp:TextBox>
            </div>
        </div>
              <div class="RigaPulsanti" style="height:auto">
            <asp:Button ID="imgModificaData" Visible="false" CssClass="Pulsante" runat="server" ToolTip="Modifica Data" Text="Modifica Data"  />
       </div>
       </div>
        <div id="DivDateServizio" class="row" runat="server" visible="false"  >
             <div class="collable" style="width:15%">
                 <asp:label id="LabelInizioServizio" CssClass="label" AssociatedControlID="lblInizio" Visible="False" runat="server" Text="Data inizio Servizio" />
            </div>
            <div class="colOggetti" style="width:35%">
                 <asp:label id="lblInizio" CssClass="labelDati" runat="server" visible="False" />
            </div>
             <div class="collable" style="width:15%">
                <asp:label id="LabelDataFineServizio" AssociatedControlID="lblFine" runat="server" visible="False"  Text="Data Fine Servizio" /> 
             </div>
            <div class="colOggetti" style="width:35%">
		       <asp:label id="lblFine"  runat="server" visible="False" CssClass="labelDati" /> 
            </div>
            </div>
             <div class="row" >
            <div class="collable" style="width:15%">
		       <asp:label id="lblTitAss" CssClass="label" runat="server"  Visible="False"  Text="Stato Assicurazione" />
            </div>			
            <div class="colOggetti" style="width:35%">
                <asp:label id="lblAss" CssClass="labelDati" runat="server" Visible="False" />
            </div>
              <div class="collable" style="width:15%">
               <asp:Label ID="LblUtenzaWEB" AssociatedControlID="txtUtenzaWeb" CssClass="label" runat="server" Text="Utenza Web" ></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtUtenzaWeb" CssClass="textbox" runat="server" ></asp:TextBox>
            </div>
        </div>
         <div id="DivContratto" class="rowGroup" runat="server"> 
         <div  class="row">
                    <div class="collable" style="width:15%">
                <asp:Label ID="LabelContratto"  CssClass="label" runat="server"  Text="Contratto"></asp:Label>
            </div>
        <div class="collable" style="width:15%">
                <asp:Label ID="LblContratto" CssClass="labelDati" style="width:80%"  runat="server" ></asp:Label>

        </div>
      <div class="collable" style="width:15%">
            <asp:ImageButton style="CURSOR:hand;" ID="imgScaricaContratto" Visible="false" Runat="server"  ImageUrl="images/giu_small.png" AlternateText="Scarica Contratto Volontario"></asp:ImageButton>
            <asp:ImageButton style="CURSOR:hand;" ID="imgValutazioneContratto" Visible="false" Runat="server"   ImageUrl="Images/documento_small.png" AlternateText="Valutazione Contratto Volontario"></asp:ImageButton>
		              
       </div>
        </div>
        </div>
        <div Id="DivProrogaContratto" class="rowGroup" runat="server" visible="false" style="height:auto">
         <div class="RigaPulsanti" >
            <asp:Button ID="imgProroga" Visible="false" CssClass="Pulsante" runat="server" ToolTip="Proroga Contratto" Text="Proroga Contratto" />
       </div>
       <div class="RigaVuota">&nbsp;
       </div>
       </div>
         <div Id="DivContrattoProrogato" class="row" runat="server" visible="false" >
          <div class="collable" style="width:15%">
                <asp:label id="LabelProroga"  runat="server"  CssClass="label" AssociatedControlID="LblProroga" visible="false" Text="Caricamento Prorogato il"></asp:label>
			</div>
            <div class="colOggetti" style="width:35%">
                <asp:label id="LblProroga"  runat="server" CssClass="labelDati" visible="false"></asp:label>
			</div>
        </div>
    
        <div class="row">
            <div class="collable" style="width:15%">
                <asp:Label ID="lblCodiceFascicolo" CssClass="label" runat="server" Text="Numero Fascicolo" AssociatedControlID="TxtCodiceFascicolo"></asp:Label>   
            </div>
            <div class="colOggetti" style="width:65%">
                <asp:TextBox id="TxtCodiceFascicolo" CssClass="textbox" runat="server"  />	
	
            </div>
            <div class="collable" style="width:20%">
                <asp:ImageButton ID="cmdSelFascicolo" runat="server" Visible="true"  OnClientClick="javascript: return ElencoFascicoli();"  ToolTip="Seleziona Codice Fascicolo" AlternateText="Seleziona Codice Fascicolo" ImageUrl="Images/cartella_small.png" ></asp:ImageButton>
                <asp:ImageButton ID="cmdSelProtocollo0"  runat="server" Visible="true" OnClientClick="javascript: return SelProtocollo0();"  ImageUrl="Images/busta_small.png"   ToolTip="Elenco Documenti" AlternateText="Elenco Documenti" ></asp:ImageButton>
                <asp:ImageButton ID="cmdFascCanc" runat="server" Visible="true"  ImageUrl="Images/canc_small.png"  ToolTip="Rimuovi Fascicolo/Protocolli" AlternateText="Rimuovi Fascicolo/Protocolli" ></asp:ImageButton>
                <asp:ImageButton ID="cmdNuovoFasciocloDLA" runat="server" Visible="true"  onClientClick="javascript:Uploadfile()"    ToolTip="Allega File" AlternateText="Allega File" ImageUrl ="Images/documento_small.png" ></asp:ImageButton>	
            </div>      
        </div>
        <div class="row">
            <div class="collable" style="width:15%">          
                <asp:Label ID="LblDescrFascicolo" CssClass="label" runat="server" Text="Descr. Fascicolo" AssociatedControlID="txtDescFasc" />
            </div>
            <div class="colOggetti" style="width:85%">
                <asp:TextBox ID="txtDescFasc" runat="server" CssClass="textbox" TextMode="MultiLine"/>
           </div>    
        </div>
         <div class="RigaVuota">&nbsp;
         </div>
        <div class="row">
            <div class="collable" style="width:15%">
                <asp:Label ID="LblCognome" CssClass="label" runat="server" AssociatedControlID="txtCognome" Text="<strong>(*)</strong>Cognome"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtCognome" CssClass="textbox" runat="server" MaxLength="100" ></asp:TextBox>
            </div>           
             <div class="collable" style="width:15%">
                <asp:Label ID="LblNome"  runat="server" CssClass="label" Text="<strong>(*)</strong>Nome" AssociatedControlID="txtNome"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtNome" CssClass="textbox" runat="server" MaxLength="100"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="collable" style="width:15%">
                <asp:Label ID="LblCodvolontario" runat="server" Text="Codice Volontario" AssociatedControlID="txtCodicevolontario"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%" >
                <asp:TextBox ID="txtCodicevolontario" CssClass="textbox" runat="server"  ReadOnly="True"></asp:TextBox>
            </div>
            <div class="collable" style="width:15%">
                <asp:Label ID="LblDataDomanda"  runat="server" Text="Data Domanda" AssociatedControlID="TxtDataDomanda"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="TxtDataDomanda" runat="server" MaxLength="10" Enabled="false" CssClass="textboxData"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="collable" style="width:15%">
                <asp:Label ID="LblSesso" runat="server" Text="Sesso"  AssociatedControlID="cboSesso"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:dropdownlist id="cboSesso" CssClass="ddlClass" runat="server"  />
            </div>
            <div class="collable" style="width:15%">
                <asp:Label ID="LblStatoCivile" runat="server" Text="Stato Civile" AssociatedControlID="cboStatoCivile"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:dropdownlist id="cboStatoCivile" runat="server" CssClass="ddlClass" />
            </div>
        </div>
        <div class="row">
            <div class="collable" style="width:15%">
                <asp:Label ID="LblCodFisc" CssClass="label" runat="server" Text="<strong>(*)</strong>Codice Fiscale" AssociatedControlID="txtCodiceFiscale"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <div style="width:6%;float:left">               
                    <asp:Image ID="imgAnomaliaCF" runat="server" ImageUrl="images/alert_xsmall.png" ToolTip="Anomalia Codice Fiscale" Width="20px" Height="20px" AlternateText="Anomalia Codice Fiscale" />                
                </div> 
                <div style="width:94%;float:left">  
                    <asp:TextBox ID="txtCodiceFiscale" CssClass="textbox" runat="server" MaxLength="16"></asp:TextBox>
                </div> 
            </div>
            <div class="collable" style="width:15%">
                <asp:Label ID="LblStatoVerificaCF" AssociatedControlID="cboStatoVerificaCF" runat="server" Text="Verifica Cod. Fiscale"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:DropDownList ID="cboStatoVerificaCF"  CssClass="ddlClass" runat="server"></asp:DropDownList>
            </div>     
          </div>

        <div class="row"  id="rigaConiuge" >
            <div class="collable" style="width:15%">
                <asp:Label ID="lblCodFiscConiuge" CssClass="label" runat="server" Text="Codice Fiscale Coniuge" AssociatedControlID="txtCodiceFiscaleConiuge"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtCodiceFiscaleConiuge" CssClass="textbox" runat="server" MaxLength="16"></asp:TextBox>
            </div>

            <div class="collable" style="width:15%">
                <asp:Label ID="LblEmail"  CssClass="label" runat="server" Text="Email" AssociatedControlID="txtEmail"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtEmail" CssClass="textbox" runat="server" MaxLength="50" onchange="javascript: CheckEmail('txtEmail', 'Email')" ></asp:TextBox>
            </div>

          </div>

        <div class="row">
           <div class="collable" style="width:15%">
                <asp:Label ID="LblTelefono" CssClass="label" runat="server" Text="Telefono" AssociatedControlID="txtTelefono"></asp:Label>
            </div>
           <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtTelefono" runat="server" MaxLength="20" CssClass="textbox" onchange="javascript: CheckNumeroIntero('txtTelefono','Telefono');" ></asp:TextBox>
            </div>
            <div class="collable" style="width:15%">
                <asp:Label ID="LblFax" CssClass="label" runat="server" Text="Fax" AssociatedControlID="txtFax"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtFax" runat="server" MaxLength="20" CssClass="textbox" onchange="javascript: CheckNumeroIntero('txtFax','Fax');"></asp:TextBox>
            </div>
        </div>
        <div class="row">
           <div class="collable" style="width:15%">
                <asp:Label ID="LblCellulare" CssClass="label" runat="server" Text="Cellulare" AssociatedControlID="txtCellulare"></asp:Label>
            </div>
             <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtCellulare"  CssClass="textbox" runat="server" MaxLength="20" onchange="javascript: CheckNumeroIntero('txtCellulare','Cellulare');"></asp:TextBox>
            </div>
                   <div class="collable" style="width:15%">
                <asp:Label ID="LblNazionalita"  runat="server" Text="Nazionalità" AssociatedControlID="ddlNazionalita"></asp:Label>
            </div>
           <div class="colOggetti" style="width:35%">
                <asp:dropdownlist id="ddlNazionalita" CssClass="ddlClass" runat="server"  ></asp:dropdownlist>
            </div>   
        
        </div>
        <div class="row">
          <div class="collable" style="width:15%">
                <asp:Label ID="LblDataNasc" CssClass="label" runat="server" Text="<strong>(*)</strong>Data di Nascita" AssociatedControlID="txtDataNascita"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtDataNascita" CssClass="textbox" runat="server" MaxLength="10"></asp:TextBox>
            </div>
            <div class="collable" style="width:15%">
                <asp:Label ID="LblProvinciaNascita" CssClass="label" runat="server" Text="<strong>(*)</strong>Provincia/ Nazione di Nascita" AssociatedControlID="ddlProvinciaNascita"></asp:Label>
            </div>
              <div class="colOggetti" style="width:25%">
                <asp:dropdownlist id="ddlProvinciaNascita" CssClass="ddlClass" AutoPostBack="true" runat="server"  ></asp:dropdownlist>
            </div>
            <div class="collable" style="width:10%">
                 <asp:CheckBox ID="ChkEsteroNascita" AutoPostBack="true" Text="Estero" ToolTip="Flag Estero Nazione di Nascita" runat="server" />
                  
            </div>
  
            
        </div>
        <div class="row">
                  <div class="collable" style="width:15%">
                <asp:Label ID="LblComuneNascita" CssClass="label" runat="server" Text="<strong>(*)</strong>Comune di Nascita" AssociatedControlID="ddlComuneNascita"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:dropdownlist id="ddlComuneNascita" CssClass="ddlClass" AutoPostBack="true" runat="server" Enabled="false" >
                <asp:ListItem Text="Selezionare Provincia/Nazione di Nascita"></asp:ListItem>
                </asp:dropdownlist>
            </div>
                        <div class="collable" style="width:15%">
                <asp:Label ID="LblCategoria"  runat="server" Text="Categoria" AssociatedControlID="cboCategoria"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:dropdownlist id="cboCategoria" runat="server"></asp:dropdownlist>
            </div>
             
        </div>
        <div class="row">
            <div class="collable" style="width:15%">
                <asp:Label ID="LblProvResidenza"  CssClass="label" runat="server" Text="<strong>(*)</strong>Provincia/ Nazione di Residenza" AssociatedControlID="ddlProvinciaResidenza"></asp:Label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:dropdownlist id="ddlProvinciaResidenza" CssClass="ddlClass" AutoPostBack="true"  runat="server"></asp:dropdownlist>
                
            </div>
            <div class="collable" style="width:10%">
                <asp:CheckBox ID="ChkEsteroResidenza"  AutoPostBack="true" ToolTip="Flag Estero Nazione di Residenza" Text="Estero" runat="server" />
            </div>
           <div class="collable" style="width:15%">
                <asp:Label ID="LblComResidenza" CssClass="label" runat="server" Text="<strong>(*)</strong>Comune di Residenza" AssociatedControlID="ddlComuneResidenza"></asp:Label>
            </div>
           <div class="colOggetti" style="width:35%">
                <asp:dropdownlist id="ddlComuneResidenza" runat="server"  CssClass="ddlClass" Enabled="false">
                <asp:ListItem Text="Selezionare Provincia/Nazione di residenza">
                </asp:ListItem>
                </asp:dropdownlist>

            </div>
           
        </div>
        <div class="row">
              <div class="collable" style="width:15%">
                <asp:Label ID="Lblndirizzo" CssClass="label" runat="server" Text="<strong>(*)</strong>Indirizzo" AssociatedControlID="txtIndirizzo"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtIndirizzo" runat="server" CssClass="textbox" onkeyup="javascript: SetContextKey('RESIDENZA');"></asp:TextBox>
                        <asp:AutoCompleteExtender 
                    ID="AutoCompleteExtender1" 
                    TargetControlID="txtIndirizzo" 
                    ContextKey ="ddlComuneResidenza"
                    CompletionListCssClass="ddl_Autocomplete"
                    UseContextKey="true"
                    CompletionInterval="100" EnableCaching="false" 
                    runat="server" MinimumPrefixLength="5"    ServiceMethod="GetCompletionList" >
                </asp:AutoCompleteExtender>
            </div>
            <div class="collable" style="width:15%">
                <asp:Label ID="LblNumCiv"  runat="server" Text="<strong>(*)</strong>N&#176; Civico" ToolTip="Numero Civico Residenza" AssociatedControlID="txtCivico"></asp:Label>
            </div>
            <div class="colOggetti" style="width:10%">
                <asp:TextBox ID="txtCivico" runat="server" CssClass="textbox" ></asp:TextBox>

            </div>
            <div class="collable" style="width:5%">
                 <asp:ImageButton ID="infocivicoRec" runat="server" AlternateText="Informazioni Civico di Residenza" 
                    ImageUrl="Images/info_small.png" style="width:23px;height:23px"  
                    ToolTip="NEL CAMPO CIVICO E' POSSIBILE INSERIRE SOLO I SEGUENTI FORMATI:
                            - 21
                            - 21/A 
                            - 21/A5 
                            - 21 BIS 
                            - KM 21,500 
                            OPPURE IL VALORE SNC"/>
            </div>
            
             <div class="collable" style="width:10%">
                <asp:Label ID="LblCap" CssClass="label" runat="server" Text="<strong>(*)</strong>C.A.P." AssociatedControlID="txtCAP"  ></asp:Label>
            </div>
            <div class="colOggetti" style="width:5%">
                <asp:TextBox ID="txtCAP" CssClass="textbox" runat="server"  onchange="javascript: CheckNumeroIntero('txtCAP','C.A.P.');" ></asp:TextBox>

             </div>
            <div class="collable" style="width:5%">
            <asp:ImageButton ID="imgCap" runat="server" title="Seleziona il Cap di Residenza" 
                    ImageUrl="Images/valida_small.png" AlternateText="Selezione C.A.P. di Residenza"  />
            </div>
            </div>
        <div class="row">
            <div class="collable" style="width:15%">
                <asp:Label ID="LblDettRecResidenza" CssClass="label" runat="server" Text="Dettaglio Recapito Residenza" AssociatedControlID="txtDettaglioRecapitoResidenza" ></asp:Label>
            </div>
            <div class="colOggetti" style="width:30%">
                <asp:TextBox ID="txtDettaglioRecapitoResidenza" CssClass="textbox" runat="server" 
                    ToolTip="Dettagli aggiuntivi del recapito"></asp:TextBox>
             
            </div>
             <div class="collable" style="width:5%">
                 <asp:ImageButton ID="imgcomunicazione" runat="server"  AlternateText="Informazioni Recapito Residenza"
                 ToolTip="ATTENZIONE!!!Nel campo Dettaglio Recapito Residenza devono essere indicate ESCLUSIVAMENTE informazioni AGGIUNTIVE, se presenti, all' indirizzo per consentire una maggiore accuratezza nelle spedizioni postali. NON VA PERTANTO RIPETUTO L'INDIRIZZO (per il quale già esiste il campo specifico) e NON VANNO INSERITI RECAPITI TELEFONICI o altre informazioni non attinenti. Si fa presente che questo campo NON E' OBBLIGATORIO e quindi VA LASCIATO VUOTO se non è necessario indicare informazioni aggiuntive al recapito relativo."
                   ImageUrl="Images/info_small.png" style="width:23px;height:23px" />
             </div>
            <div class="collable" style="width:45%">
                <asp:CheckBox ID="chkIndirizzoErrato" runat="server" Text="Recapito postale da verificare" />
            </div>
        </div>
        <div class="row">
               <div class="collable" style="width:15%">
                <asp:Label ID="lblProvinciaDomicilio" CssClass="label"  runat="server" Text="Provincia/ Nazione di Domicilio" AssociatedControlID="ddlProvinciaDomicilio"></asp:Label>
            </div>
           <div class="colOggetti" style="width:25%">
                <asp:dropdownlist id="ddlProvinciaDomicilio"  AutoPostBack="true" 
                    runat="server" CssClass="ddlClass"  ></asp:dropdownlist>
            </div>
             <div class="collable" style="width:10%">
             <asp:CheckBox ID="ChkEsteroDomicilio"  AutoPostBack="true" Text="Estero" runat="server" />
             </div>
           <div class="collable" style="width:15%">
                <asp:Label ID="lblComuneDomicilio" CssClass="label" runat="server" Text="Comune di Domicilio" AssociatedControlID="ddlComuneDomicilio"></asp:Label>
            </div>

            <div class="colOggetti" style="width:35%">
                <asp:dropdownlist id="ddlComuneDomicilio" CssClass="ddlClass" runat="server" Enabled="false">
               <asp:ListItem Text="Selezionare Provincia/Nazione di Domicilio"></asp:ListItem>
                </asp:dropdownlist>
                
            </div>
     
        </div>
        <div class="row">
            <div class="collable" style="width:15%">
                <asp:Label ID="lblIndirizzoDomicilo" CssClass="label" runat="server" Text="Indirizzo Domicilio" AssociatedControlID="txtIndirizzoDomicilio"></asp:Label>
            </div>
             <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtIndirizzoDomicilio" CssClass="textbox" onkeyup="javascript:SetContextKey('DOMICILIO')" runat="server" ></asp:TextBox>
                        <asp:AutoCompleteExtender 
                    ID="AutoCompleteExtender2" 
                    TargetControlID="txtIndirizzoDomicilio" 
                    CompletionListCssClass="ddl_Autocomplete"
                    UseContextKey="true"
                    CompletionInterval="100" EnableCaching="false" 
                    runat="server" MinimumPrefixLength="5"    ServiceMethod="GetCompletionList" >
                </asp:AutoCompleteExtender>
            </div>
            <div class="collable" style="width:15%">
                <asp:Label ID="lblCivicoDomicilio" CssClass="label" runat="server" Text="N&#176; Civico Domicilio" ToolTip="Numero Civico Domicilio" AssociatedControlID="txtCivicoDomicilio"></asp:Label>
            </div>
            <div class="colOggetti" style="width:10%">
                <asp:TextBox ID="txtCivicoDomicilio" CssClass="textbox" runat="server"  ></asp:TextBox>
            </div>
            <div class="collable" style="width:5%">
                    <asp:ImageButton ID="infocivico" runat="server" AlternateText="Informazioni civico domicilio"  ToolTip="NEL CAMPO CIVICO E' POSSIBILE INSERIRE SOLO I SEGUENTI FORMATI:
                            - 21
                            - 21/A 
                            - 21/A5 
                            - 21 BIS 
                            - KM 21,500 
                            OPPURE IL VALORE SNC"
                    ImageUrl="Images/info_small.png" style="width:23px;height:23px"/>
            </div>
            <div class="collable" style="width:10%">
                <asp:Label ID="lblCapDom" CssClass="label"  runat="server" Text="C.A.P. Domicilio" AssociatedControlID="txtCapDomicilio"  ></asp:Label>
            </div>
            <div class="colOggetti" style="width:5%">
                <asp:TextBox ID="txtCapDomicilio" CssClass="textbox" runat="server" onchange="javascript: CheckNumeroIntero('txtCapDomicilio','C.A.P. Domicilio');" ></asp:TextBox>
            </div>
            <div class="collable" style="width:5%">
                                <asp:ImageButton ID="imgCapDom" runat="server" AlternateText="C.A.P. Domicilio" title="Seleziona il C.A.P. di Domicilio" 
                    ImageUrl="Images/valida_small.png" />
            </div>
        </div>
        <div class="row">
            <div class="collable" style="width:15%">
                <asp:Label ID="lblDettaglioRecapitoDomicilio" CssClass="label" runat="server" Text="Dettaglio Recapito Domicilio" AssociatedControlID="TxtDettaglioRecapitoDomicilio" ></asp:Label>
            </div>
            <div class="colOggetti" style="width:30%">
                <asp:TextBox ID="TxtDettaglioRecapitoDomicilio" runat="server" CssClass="textbox"
                    ToolTip="Dettagli aggiuntivi del recapito di domicilio" ></asp:TextBox>
            
            </div>
             <div class="collable" style="width:5%">
                 <asp:ImageButton ID="dettrecapitodom" runat="server" AlternateText="Informazioni Dettaglio Recapito Domicilio" ToolTip="ATTENZIONE!!!Nel campo Dettaglio Recapito Domicilio devono essere indicate ESCLUSIVAMENTE informazioni AGGIUNTIVE, se presenti, all' indirizzo per consentire una maggiore accuratezza nelle spedizioni postali. NON VA PERTANTO RIPETUTO L'INDIRIZZO (per il quale già esiste il campo specifico) e NON VANNO INSERITI RECAPITI TELEFONICI o altre informazioni non attinenti. Si fa presente che questo campo NON E' OBBLIGATORIO e quindi VA LASCIATO VUOTO se non è necessario indicare informazioni aggiuntive al recapito relativo."
                     ImageUrl="Images/info_small.png" style="width:23px;height:23px" />
             </div>
             <div class="collable" style="width:15%">
                <asp:Label ID="lblTelefonoDomicilio" CssClass="label" runat="server" Text="Telefono Domicilio" AssociatedControlID="txtTelefonoDomicilio" ></asp:Label>
               
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtTelefonoDomicilio" CssClass="textbox" onchange="javascript: CheckNumeroIntero('txtTelefonoDomicilio','Telefono Domicilio');"  runat="server" ></asp:TextBox>

            </div>
        </div>
        <div class="row">
            <div class="collable" style="width:15%">
                <asp:Label ID="LblCodLibretto" CssClass="label" runat="server" Text="Libretto" AssociatedControlID="txtLibretto"></asp:Label>
            </div>
             <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtLibretto" CssClass="textbox" runat="server" MaxLength="12" ></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="collable" style="width:15%">
                <asp:Label ID="LblIBAN"  runat="server" CssClass="label" Text="IBAN" AssociatedControlID="txtIBAN"></asp:Label>
            </div>
             <div class="colOggetti" style="width:35%">
               <asp:TextBox ID="txtIBAN" runat="server" MaxLength="50" CssClass="textbox" onchange="javascript:VerificaPreesistenzaIBAN()"></asp:TextBox>
            </div>
            <div class="collable" style="width:15%">
                <asp:Label ID="LblBicSwift" CssClass="label" runat="server" Text="BIC/SWIFT" AssociatedControlID="txtBicSwift"></asp:Label>
            </div>
           <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtBicSwift" runat="server" MaxLength="12" CssClass="textbox" ></asp:TextBox>
            </div>
        </div>
        <div class="row">
          <div class="collable" style="width:15%">
              <asp:Label ID="LblTitoloStudio" CssClass="label"  runat="server" Text="Titolo di Studio" AssociatedControlID="txtTitoloStudio"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
              <asp:TextBox ID="txtTitoloStudio" runat="server" CssClass="textbox"></asp:TextBox>
            </div>
           <div class="collable" style="width:15%">
                <asp:Label ID="LblConsTitoloStudio" CssClass="label" runat="server" Text="Conseguimento Titolo Studio" AssociatedControlID="CboConseguimentoTS"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:DropDownList ID="CboConseguimentoTS" runat="server" CssClass="ddlClass" />
             </div>
        </div>
            
        <div class="row">
            <div class="collable" style="width:15%">
              <asp:Label ID="LblAltreInfo"  runat="server" Text="Altre informazioni" AssociatedControlID="txtAltreInformazioni" />
            </div>
            <div class="colOggetti" style="width:50%">
                <asp:TextBox ID="txtAltreInformazioni" CssClass="textbox" runat="server"  Width="50%" TextMode="MultiLine" Enabled="False" />
            </div>
            <div class="colOggetti" style="width:15%">
               <div class="colHyperLink" style="width:100%">
                        <asp:LinkButton ID="cmdInsNote" runat="server" style="cursor: pointer" ToolTip="Inserimento note" Text="Inserisci Nota" ></asp:LinkButton>
                </div>
            </div>
            <div class="colOggetti" style="width:15%">
            <div class="colHyperLink" style="width:100%">
             <asp:LinkButton ID="cmdStoricoNote" Text="Visualizza Note" runat="server"></asp:LinkButton>
            </div>
            </div>
        </div>
        <br />
        <div id="GestioneNote_RigheNascoste" style="border:0px;display:table;width:100%;" runat="server" >
     
            
               <div class="row">
                    <div class="collable" style="width:15%">
                        <asp:Label ID="lblNuovaNota" CssClass="label"  runat="server" Text="Nota" AssociatedControlID="txtNuovaNota" />
                    </div>
                    <div class="colOggetti" style="width:28%">
                        <asp:textbox id="txtNuovaNota" CssClass="textbox" runat="server" Width="90%" TextMode="MultiLine"></asp:textbox>&nbsp;&nbsp;
                     </div>   
                    <div class="colOggetti" style="width:40%">
                        <asp:Button ID="cmdSalvaNota" CssClass="Pulsante" runat="server" ToolTip="Salva Nota" Text="Aggiungi Nota"  />
                    </div>
                </div>

            </div>


        <div  id="gridSoricoNote" runat="server">
                <fieldset class="ContornoPagina"  >
       <legend>Storico Note Volontario</legend>
                <asp:datagrid id="dtgStoricoNote" CssClass="table" runat="server" CellPadding="2" ToolTip="Elenco storico note"  PageSize = "2"   AllowPaging="true" 
                    AllowSorting="True" AutoGenerateColumns="False"  UseAccessibleHeader="True" Visible="true" ShowFooter="false"  Width="100%">
			    <FooterStyle></FooterStyle>
		        <SelectedItemStyle ></SelectedItemStyle>
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		        <HeaderStyle ></HeaderStyle>
                    <Columns>
					    <asp:BoundColumn Visible="False" DataField="idCronologiaNoteEntit&#224;" HeaderText="idCronologiaNoteEntit&#224;"></asp:BoundColumn>
						<asp:BoundColumn DataField="UserNameNota" HeaderText="UserName">
						</asp:BoundColumn>
						<asp:BoundColumn DataField="DataNota" HeaderText="Data Nota">
						</asp:BoundColumn>
						<asp:BoundColumn DataField="NoteEntit&#224;" HeaderText="Nota Entit&#224;"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Rimuovi" > 
                                 <ItemTemplate>
											<asp:ImageButton ID="ImageButton1"  Enabled="true"   CommandName="Rimuovi" AlternateText="Rimuovi Nota" ToolTip="Rimuovi Nota" runat="server" ImageURL="Images/canc_small.png"></asp:ImageButton>      
                                            </ItemTemplate>
                                            </asp:TemplateColumn>
				    </Columns>
                    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
			    </asp:datagrid>
             </fieldset>  
         </div> 
        <div class="row">
            <div class="collable" style="width:15%">
              <asp:Label ID="lblGMO" CssClass="label" runat="server" Text="GMO" AssociatedControlID="ddlGMO"/>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:DropDownList ID="ddlGMO" CssClass="ddlClass" runat="server" Enabled="false"></asp:DropDownList>
            </div>
            <div class="collable" style="width:15%">
              <asp:Label ID="lblFAMI" CssClass="label" runat="server" Text="FAMI" AssociatedControlID="ddlFAMI" />
            </div>
             <div class="colOggetti" style="width:35%">
                <asp:DropDownList ID="ddlFAMI" CssClass="ddlClass" runat="server" Enabled="false"></asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="collable" style="width:15%">
              <asp:Label ID="LblCodSedeAtt" CssClass="label" runat="server" Text="Codice Sede Attuazione" AssociatedControlID="CboSedeAtt" />
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:DropDownList ID="CboSedeAtt" CssClass="ddlClass" runat="server"></asp:DropDownList>
            </div>
            <div class="collable" style="width:15%">
              <asp:Label ID="LblPunteggio" CssClass="label" runat="server" Text="Punteggio" AssociatedControlID="txtPunteggio" />
            </div>
             <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtPunteggio" CssClass="textbox" onchange="javascript: CheckNumeroValido('txtPunteggio','Punteggio');" runat="server" />
            </div>
        </div>
        <div  class="row" id ="DivSedeSecondaria" >
            <div class="collable" style="width:15%">
              <asp:Label ID="LblCodSedeAttSecondaria" CssClass="label" runat="server" Text="Codice Sede Attuazione Secondaria" AssociatedControlID="CboSedeAttSecondaria" />
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:DropDownList ID="CboSedeAttSecondaria" CssClass="ddlClass" runat="server"></asp:DropDownList>
            </div>
        </div>


         </div>
       
  
   
        <p>
        <div class="row">
            <div class="collable" style="width:50%;text-align:left;">
                <asp:CheckBox ID="CheckboxIdoneo" runat="server" Text="Idoneo" />
                                <asp:Image id="Bloccato" runat="server" 
                    ImageUrl="Images/locked.png" style="width:35px;height:35px" 
                    AlternateText="Bloccato in quanto Selezionato" Visible="false" ToolTip="Bloccato in quanto Selezionato"  />
             </div>
        </div>
        <div class="row">
            <div class="collable" style="width:50%">
                <asp:CheckBox ID="chkDisp1" runat="server" Text="Disponibile a subentro su stesso progetto" />

            </div>
        </div>
        <div class="row">
            <div class="collable" style="width:50%">
                <asp:CheckBox ID="chkDisp2" runat="server" Text="Disponibile a subentro su altri progetti"/>
            </div>
        </div>
        </p>
     
        <div class="RigaVuota">&nbsp;
        </div>
         <div class="row">
         <div class="collable" style="width:100%">
             <asp:Label runat="server" ID="LblInfoCampiObbligatori" Font-Bold="true"  Font-Size="Small" Text="I campi contrassegnati con (*) sono obbligatori."></asp:Label>
            </div>
           
         </div>
         <div class="RigaPulsanti" style="text-align:right">
            <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" ToolTip="Salva" Text="Salva" OnClientClick ="JavaScript:return ValidaCampi();" />
            <asp:Button ID="cmdnuovo" CssClass="Pulsante" runat="server" Text="Nuovo" ToolTip="Nuovo" Visible="False" />
            <asp:Button ID="CmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
       </div>

        


 </fieldset>

</asp:Content>