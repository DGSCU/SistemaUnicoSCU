<%@ Page Title="Documentazione Progetti" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="elencodocumentazioneProgetti.aspx.vb" Inherits="Futuro.elencodocumentazioneProgetti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    .ui-datepicker {
        font-size: 11px;
    }
</style>
    <script  type="text/javascript">

        var formId;
        var prefissoIdClient = "MainContent_";
        var width = screen.width / 1.5;
        var height = screen.height / 1.5;
        var x = screen.width / 2 - width;
        var y = screen.height / 2 - height;
        var winOption = "dependent=no,scrollbars=yes,status=no,resizable=yes,width=" + width + ",height=" + height + ",top=" + y + ",left=" + x

        function ImpostaFormId() {
            formId = document.forms[0].id;
            return formId;
        }
        function CostruisciId(IdServer) {
            var IdClient = prefissoIdClient + IdServer
            return IdClient
        }
        $(function () {
            var IdData = CostruisciId('txtdatadal');
            var sharpIdData = "#" + IdData
            $("" + sharpIdData + "").datepicker();
        });

	</script>
    <script  type="text/JavaScript">
	    function SelProtocollo(NumProt,DataProt,TipoDocumento,Sezione){
            var IdFascicolo =  '<%= TxtCodiceFasc.Value %>';
			if (document.getElementById(CostruisciId("TxtNumeroFascicolo")).value ==''){
				alert ("Nessun Fascicolo Selezionato!");
			}else{
                var parametri="VengoDa=Stampa&objHddModifica=hddModificaProtocollo&objForm=Form1&TxtProt="+CostruisciId(NumProt)+"&TxtData="+CostruisciId(DataProt)+"&NumeroFascicolo="+IdFascicolo
				myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?"+parametri, "", winOption);
			}
		}

		function ApriAllegati(NumProt,DataProt)	{
            var IdFascicolo =  '<%= TxtCodiceFasc.Value %>';
			if (document.all.TxtCodiceFascicolo.value =='')
			{
				alert ("Nessun Fascicolo Selezionato!");
			}else{	
				myWin = window.open("WfrmSIGEDElencoAllegati.aspx?NumeroProtocollo=" + document.getElementById(NumProt).value  + "&DataProtocollo=" + document.getElementById(DataProt).value + "&CodiceFascicolo="+IdFascicolo,"Visualizza",winOption);		
			}
		}

			function Protocollazione(NumProt,DataProt)
			{
                var IdFascicolo =  '<%= TxtCodiceFasc.Value %>';
                var txtDescFasc = document.getElementById(CostruisciId("txtDescFasc")).value;
                var TxtNumeroFascicolo =  document.getElementById(CostruisciId("TxtNumeroFascicolo")).value;
				if (document.getElementById(CostruisciId(NumProt)).value=='' || document.getElementById(CostruisciId(DataProt)).value=='')
					{
						myWin = window.open("WfrmSIGEDProtocollazione.aspx?CorrCodUnivoco=<%= Session("txtCodEnte")%>&Processo=VOLONTARI&objForm=Form1&TxtFasc=MainContent_TxtCodiceFasc&TxtCodFasc=MainContent_TxtCodiceFascicolo&TxtDescFascicolo=MainContent_txtDescFasc&TxtProt="+NumProt+"&TxtData="+DataProt+"&Fascicolo="+TxtNumeroFascicolo+"&NumeroFascicolo="+IdFascicolo+"&DescFascicolo="+txtDescFasc, "Visualizza",winOption);
					}
					else
					{
						var msg;
						msg=confirm("Si vuole inserire il documento Principale?");
						if (msg==false)
							{
								myWin = window.open("WfrmSIGEDProtocollazione.aspx?CorrCodUnivoco=<%= Session("txtCodEnte")%>&Processo=VOLONTARI&objForm=Form1&TxtFasc=MainContent_TxtCodiceFasc&TxtCodFasc=MainContent_TxtNumeroFascicolo&TxtDescFascicolo=MainContent_txtDescFasc&TxtProt="+CostruisciId(NumProt)+"&TxtData="+CostruisciI(DataProt)+"&Fascicolo=" + TxtNumeroFascicolo+"&NumeroFascicolo="+IdFascicolo+"&DescFascicolo="+document.getElementById(CostruisciId(txtDescFasc)).value, "Visualizza",winOption);
                                return false;
							}else
							{	
								myWin = window.open("WfrmSIGEDDocumentoPrincipale.aspx?NumProt=" + document.getElementById(CostruisciId(NumProt)).value + "&DataProt=" + document.getElementById(CostruisciId(DataProt)).value, "Visualizza", winOption);			
							}
					}	
			}				



        function ElencoDocumenti(TxtCodiceFascicolo){
            var IdFascicolo =  '<%= TxtCodiceFasc.Value %>';
            var codiceFascicolo = document.getElementById(CostruisciId(TxtCodiceFascicolo)).value;

            if (codiceFascicolo ==''){
				alert ("Specificare il numero fascicolo!");
			}else{
				myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?NumeroFascicolo=" + IdFascicolo, "Visualizza", winOption);			
			}
			return false;
            }

        function ElencoFascicoli(TxtCodiceFascicolo,txtDescFasc) {
            var IdTxtCodiceFascicolo = CostruisciId(TxtCodiceFascicolo);
            var IdDescrizioneFascicolo = CostruisciId(txtDescFasc);
            var IdFascicolo =  '<%= TxtCodiceFasc.Value %>';
            var IdTxtIdFasc = CostruisciId("TxtCodiceFasc");
            var codiceFascicolo = document.getElementById(IdTxtCodiceFascicolo).value;
		    if(codiceFascicolo!=''){
                myWin = window.open("WfrmSIGEDElencoFascicoli.aspx?Processo=VOLONTARI&objForm=Form1&objNumero="+IdTxtCodiceFascicolo+"&objCodice="+IdTxtIdFasc+"&objDescFasc="+IdDescrizioneFascicolo+"&VArUpdate=4", "Visualizza", winOption);		
            }else{
            
			alert ("Cancellare il Fascicolo prima di associarne uno nuovo!");
			}			
        }


		</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina" >
<legend>Elenco Documentazione</legend>
<div class="wrapper"  id="TableIni" style="width:100%">
        <div class="rowGroup">
            <asp:label id="lblmessaggiosopra" runat="server" Visible="false"></asp:label>
            <input type="hidden" id="HdValoreSalva" runat="server" value="0"/>
            <input type="hidden" id="hddModificaProtocollo" runat="server" value="0" />
            <input type="hidden" id="TxtCodiceFasc" runat="server"/>
            <div class="skip">
                <asp:textbox id="TxtNumFascicoloControllo"  runat="server" Width="0px"></asp:textbox>                                 
            </div>
           
        </div>

  <div class= "rowGroup" style="height:auto" >

            <div class="row" >
                <div class="collable" style="width:20%">
                    <asp:Label ID="LblBando" AssociatedControlID="ddlBando" runat="server" Text="Circolare"></asp:Label>
                </div>
                <div class="colOggetti" style="width:80%">    
                    <asp:DropDownList ID="ddlBando" CssClass="ddlClass" runat="server" AutoPostBack="true"  ></asp:DropDownList> 
                </div>
            </div>
   
            <div id="DivFasc" runat="server" class= "rowGroup" >
                <h2> <asp:label  id="lblTitolo" Font-Bold="true"  runat="server" Text="Dati fascicolo" /></h2>
                <div class="RigaVuota">&nbsp;</div>
                <div class="row">
                    <div class="collable" style="width:20%" >
                        <asp:Label ID="lblCodiceFascicoloP" CssClass="label" AssociatedControlID="TxtNumeroFascicolo" runat="server" Text="Numero Fascicolo" ></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:60%" >
                        <asp:TextBox ID="TxtNumeroFascicolo" CssClass="textbox" ReadOnly="true" runat="server" ></asp:TextBox>
                    </div>
                    <div class="collable" style="width:15%" >
                        <asp:ImageButton style="CURSOR: hand;" id="cmdSelFascicolo" 
                            OnClientClick="ElencoFascicoli('TxtNumeroFascicolo','txtDescFasc');" 
                            runat="server" ImageUrl="images/cartella_small.png" 
                            AlternateText="Seleziona Codice Fascicolo" ToolTip="Seleziona Codice Fascicolo" 
                            Visible="False"/>
                        <asp:ImageButton style="CURSOR: hand;" id="cmdSelProtocollo" OnClientClick="ElencoDocumenti('TxtNumeroFascicolo');" runat="server" ImageUrl="images/busta_small.png" ToolTip="Elenco Documenti" AlternateText="Elenco Documenti"/>
                        <asp:ImageButton style="CURSOR: hand;" id="cmdFascCanc"  runat="server" ImageUrl="images/canc_small.png" ToolTip="Rimuovi Fascicolo/Protocolli" AlternateText="Rimuovi Fascicolo/Protocolli"/>
                    </div>
                </div>
                <div class= "row" >
                    <div class="collable" style="width:20%" >
                        <asp:Label id="LblDescFasc" CssClass="label" AssociatedControlID="txtDescFasc" runat="server" Text="Descrizione"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:80%" >
                        <asp:TextBox ID="txtDescFasc"  CssClass="textbox"  ReadOnly="true" runat="server" TextMode="MultiLine"/>
                    </div>
                </div>
            </div>
            <div class="RigaVuota">&nbsp;</div>

      <%--<div id="DivProgetti" runat="server" class= "rowGroup" >--%>
     <p runat="server" id="DivProgetti" class="rowGroup">
        <div class="row">
			<div class="colOggetti" style="width:70%">
                <asp:checkbox id="ChkLetteraPositivaNegativa" runat="server"></asp:checkbox><asp:label id="LblLetteraPositivaNegativa" runat="server" AssociatedControlID="ChkLetteraPositivaNegativa"  Text="Lettera di Trasmissione"></asp:label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:hyperlink id="hplPositivaNegativa" runat="server" Visible="False" CssClass="linkStampa" ToolTip="Scarica File" Target="_blank">Download File</asp:hyperlink>			
            </div>
		</div>

        <div class="row">
			<div class="colOggetti" style="width:70%">
                <asp:checkbox id="chkletteraNegativa" runat="server"></asp:checkbox><asp:label id="LblletteraNegativa" runat="server" AssociatedControlID="chkletteraNegativa"  Text="Comunicazione Positiva"></asp:label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:hyperlink id="hplletteraNegativa" runat="server" Visible="False" CssClass="linkStampa" ToolTip="Scarica File" Target="_blank">Download File</asp:hyperlink>			
            </div>
		</div>

        <div class="row">
			<div class="colOggetti" style="width:70%">
                <asp:checkbox id="chkdetNegSingola" runat="server"></asp:checkbox><asp:label id="Label2" runat="server" AssociatedControlID="chkdetNegSingola"  Text="Determina Limitata Plurima"></asp:label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:hyperlink id="hpldetNegSingola" runat="server" Visible="False" CssClass="linkStampa" ToolTip="Scarica File" Target="_blank">Download File</asp:hyperlink>			
            </div>
		</div>

        <div class="row">
			<div class="colOggetti" style="width:70%">
                <asp:checkbox id="chkLetteraPositiva" runat="server"></asp:checkbox><asp:label id="lblLetteraPositiva" runat="server" AssociatedControlID="chkLetteraPositiva"  Text="Determina Limitata Singola"></asp:label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:hyperlink id="hplLetteraPositiva" runat="server" Visible="False" CssClass="linkStampa" ToolTip="Scarica File" Target="_blank">Download File</asp:hyperlink>			
            </div>
		</div>

        <div class="row">
			<div class="colOggetti" style="width:70%">
                <asp:checkbox id="chkDetNegMultipla" runat="server"></asp:checkbox><asp:label id="lblDetNegMultipla" runat="server" AssociatedControlID="chkDetNegMultipla"  Text="Determina Negativa Plurima"></asp:label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:hyperlink id="hplDetNegMultipla" runat="server" Visible="False" CssClass="linkStampa" ToolTip="Scarica File" Target="_blank">Download File</asp:hyperlink>			
            </div>
		</div>

        <div class="row">
			<div class="colOggetti" style="width:70%">
                <asp:checkbox id="chkDetPosSingola" runat="server"></asp:checkbox><asp:label id="lblDetPosSingola" runat="server" AssociatedControlID="chkDetPosSingola"  Text="Determina Negativa Singola"></asp:label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:hyperlink id="hplDetPosSingola" runat="server" Visible="False" CssClass="linkStampa" ToolTip="Scarica File" Target="_blank">Download File</asp:hyperlink>			
            </div>
		</div>

        <div class="row">
			<div class="colOggetti" style="width:70%">
                <asp:checkbox id="chkElencoProgNeg" runat="server"></asp:checkbox><asp:label id="lblElencoProgNeg" runat="server" AssociatedControlID="chkElencoProgNeg"  Text="Allegato Elenco Progetti Negativi"></asp:label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:hyperlink id="hplElencoProgNeg" runat="server" Visible="False" CssClass="linkStampa" ToolTip="Scarica File" Target="_blank">Download File</asp:hyperlink>			
            </div>
		</div>


        <div class="row">
			<div class="colOggetti" style="width:70%">
                <asp:checkbox id="chkdetPosMultipla" runat="server"></asp:checkbox><asp:label id="lbldetPosMultipla" runat="server" AssociatedControlID="chkdetPosMultipla"  Text="Allegato Elenco Progetti Positivi"></asp:label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:hyperlink id="hpldetPosMultipla" runat="server" Visible="False" CssClass="linkStampa" ToolTip="Scarica File" Target="_blank">Download File</asp:hyperlink>			
            </div>
		</div>

        <div class="row">
			<div class="colOggetti" style="width:70%">
                <asp:checkbox id="chkElencoProgPos" runat="server"></asp:checkbox><asp:label id="lblElencoProgPos" runat="server" AssociatedControlID="chkElencoProgPos"  Text="Allegato Elenco Progetti Positivi Limitati"></asp:label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:hyperlink id="hplElencoProgPos" runat="server" Visible="False" CssClass="linkStampa" ToolTip="Scarica File" Target="_blank">Download File</asp:hyperlink>			
            </div>
		</div>
    </p>
  <%--  </div>--%>     
    <p runat="server" id="DivProgrammi" class="rowGroup">    
        <div class="row">
            <div class="colOggetti" style="width:70%">
                <asp:checkbox id="chkDecretoProgrammaEsclusoSingolo" runat="server"></asp:checkbox><asp:label id="lblDecretoProgrammaEsclusoSingolo" runat="server" AssociatedControlID="chkDecretoProgrammaEsclusoSingolo"  Text="Decreto Programma Escluso Singolo"></asp:label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:hyperlink id="hplDecretoProgrammaEsclusoSingolo" runat="server" Visible="False" CssClass="linkStampa" ToolTip="Scarica File" Target="_blank">Download File</asp:hyperlink>	
            </div>
        </div>

        <div class="row">
            <div class="colOggetti" style="width:70%">
                <asp:checkbox id="chkDecretoProgrammaInammissibileSingolo" runat="server"></asp:checkbox><asp:label id="lblDecretoProgrammaInammissibileSingolo" runat="server" AssociatedControlID="chkDecretoProgrammaInammissibileSingolo"  Text="Decreto Programma Inammissibile Singolo"></asp:label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:hyperlink id="hplDecretoProgrammaInammissibileSingolo" runat="server" Visible="False" CssClass="linkStampa" ToolTip="Scarica File" Target="_blank">Download File</asp:hyperlink>	
            </div>
        </div>

        <div class="row">
            <div class="colOggetti" style="width:70%">
                <asp:checkbox id="chkDecretoProgrammaPositivoconProgettiLimitatiSingolo" runat="server"></asp:checkbox><asp:label id="lblDecretoProgrammaPositivoconProgettiLimitatiSingolo" runat="server" AssociatedControlID="chkDecretoProgrammaPositivoconProgettiLimitatiSingolo"  Text="Decreto Programma Positivo con Progetti Limitati Singolo"></asp:label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:hyperlink id="hplDecretoProgrammaPositivoconProgettiLimitatiSingolo" runat="server" Visible="False" CssClass="linkStampa" ToolTip="Scarica File" Target="_blank">Download File</asp:hyperlink>	
            </div>
        </div>

        <div class="row">
            <div class="colOggetti" style="width:70%">
                <asp:checkbox id="chkDecretoProgrammaRidottoSingolo" runat="server"></asp:checkbox><asp:label id="lblDecretoProgrammaRidottoSingolo" runat="server" AssociatedControlID="chkDecretoProgrammaRidottoSingolo"  Text="Decreto Programma Ridotto Singolo"></asp:label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:hyperlink id="hplDecretoProgrammaRidottoSingolo" runat="server" Visible="False" CssClass="linkStampa" ToolTip="Scarica File" Target="_blank">Download File</asp:hyperlink>	
            </div>
        </div>

        <div class="row">
			<div class="colOggetti" style="width:70%">
                <asp:checkbox id="chkDecretoProgrammiEsclusiPlurimo" runat="server"></asp:checkbox><asp:label id="lblDecretoProgrammiEsclusiPlurimo" runat="server" AssociatedControlID="chkDecretoProgrammiEsclusiPlurimo"  Text="Decreto Programmi Esclusi Plurimo"></asp:label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:hyperlink id="hplDecretoProgrammiEsclusiPlurimo" runat="server" Visible="False" CssClass="linkStampa" ToolTip="Scarica File" Target="_blank">Download File</asp:hyperlink>			
            </div>
		</div>

        <div class="row">
			<div class="colOggetti" style="width:70%">
                <asp:checkbox id="chkDecretoProgrammiInammissibiliPlurimo" runat="server"></asp:checkbox><asp:label id="lblDecretoProgrammiInammissibiliPlurimo" runat="server" AssociatedControlID="chkDecretoProgrammiInammissibiliPlurimo"  Text="Decreto Programmi Inammissibili Plurimo"></asp:label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:hyperlink id="hplDecretoProgrammiInammissibiliPlurimo" runat="server" Visible="False" CssClass="linkStampa" ToolTip="Scarica File" Target="_blank">Download File</asp:hyperlink>			
            </div>
		</div>

        <div class="row">
			<div class="colOggetti" style="width:70%">
                <asp:checkbox id="chkDecretoProgrammiPositiviconProgettiLimitatiPlurimo" runat="server"></asp:checkbox><asp:label id="lblDecretoProgrammiPositiviconProgettiLimitatiPlurimo" runat="server" AssociatedControlID="chkDecretoProgrammiPositiviconProgettiLimitatiPlurimo"  Text="Decreto Programmi Positivi con Progetti Limitati Plurimo"></asp:label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:hyperlink id="hplDecretoProgrammiPositiviconProgettiLimitatiPlurimo" runat="server" Visible="False" CssClass="linkStampa" ToolTip="Scarica File" Target="_blank">Download File</asp:hyperlink>			
            </div>
		</div>

        <div class="row">
			<div class="colOggetti" style="width:70%">
                <asp:checkbox id="chkDecretoProgrammiRidottiPlurimo" runat="server"></asp:checkbox><asp:label id="lblDecretoProgrammiRidottiPlurimo" runat="server" AssociatedControlID="chkDecretoProgrammiRidottiPlurimo"  Text="Decreto Programmi Ridotti Plurimo"></asp:label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:hyperlink id="hplDecretoProgrammiRidottiPlurimo" runat="server" Visible="False" CssClass="linkStampa" ToolTip="Scarica File" Target="_blank">Download File</asp:hyperlink>			
            </div>
		</div>
        <div class="row">
			<div class="colOggetti" style="width:70%">
                <asp:checkbox id="chkLetteraTrasmissioneNegativi" runat="server"></asp:checkbox><asp:label id="lblLetteraTrasmissioneNegativi" runat="server" AssociatedControlID="chkLetteraTrasmissioneNegativi"  Text="Lettera Trasmissione Programmi Inammissibili/Esclusi"></asp:label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:hyperlink id="hplLetteraTrasmissioneNegativi" runat="server" Visible="False" CssClass="linkStampa" ToolTip="Scarica File" Target="_blank">Download File</asp:hyperlink>			
            </div>
		</div>
        <div class="row">
			<div class="colOggetti" style="width:70%">
                <asp:checkbox id="chkLetteraTrasmissionePositivi" runat="server"></asp:checkbox><asp:label id="lblLetteraTrasmissionePositivi" runat="server" AssociatedControlID="chkLetteraTrasmissionePositivi"  Text="Lettera Trasmissione Positivi/Positivi Ridotti"></asp:label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:hyperlink id="hplLetteraTrasmissionePositivi" runat="server" Visible="False" CssClass="linkStampa" ToolTip="Scarica File" Target="_blank">Download File</asp:hyperlink>			
            </div>
		</div>
    </p>
        <div class="RigaPulsanti"> 
            <asp:button id="imgGeneraFile" runat="server" Text="Genera" CssClass="Pulsante"></asp:button>
            <asp:button id="imgChiudi" text="Chiudi" CssClass="Pulsante" runat="server" ></asp:button>                   
        </div>

    </div>
 </div>
</fieldset>

</asp:Content>
