<%@ Page Title="Elenco Documentazione Sostituzione Volontario" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmElencoDocumentazioneSostituzioneVolontario.aspx.vb" Inherits="Futuro.WfrmElencoDocumentazioneSostituzioneVolontario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
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
	</script>
    <script  type="text/JavaScript">
	    function SelProtocollo(NumProt,DataProt,TipoDocumento,Sezione){
            var IdFascicolo =  '<%= TxtIdFascP.Text %>';
			if (document.getElementById(CostruisciId("TxtCodiceFascicoloP")).value ==''){
				alert ("Nessun Fascicolo Selezionato!");
			}else{
				myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?objForm=Form1&TxtProt="+CostruisciId(NumProt)+"&IdAttivita=<%=Request.QueryString("IdAttivita")%>&IdEnte=<%=Request.QueryString("IdEnte")%>&VarVolontario=<%=Request.QueryString("identita")%>&TxtData="+CostruisciId(DataProt)+"&VArUpdate=3&TipoDocumento="+TipoDocumento+"&NumeroFascicolo="+IdFascicolo, "Visualizza", winOption);
			}
		}
		function SelProtocolloVSOS(NumProt,DataProt,TipoDocumento,TxtIdFasc){
            var IdFascicolo =  '<%= TxtIdFascVSOS.Text %>';
            var CodiceFascicoloVSos =  '<%= TxtCodiceFascicoloVSos.Text %>';
			if (CodiceFascicoloVSos ==''){
			    alert ("Nessun Fascicolo Selezionato!");
			}else{
			    myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?objForm=Form1&TxtProt="+CostruisciId(NumProt)+"&VarVolontario=<%=Request.QueryString("IdVolVecchio")%>&TxtData="+CostruisciId(DataProt)+"&VArUpdate=1&TipoDocumento="+CostruisciId(TipoDocumento)+"&NumeroFascicolo="+IdFascicolo, "Visualizza", winOption);		
			}
		}
        function SelProtocolloVSUB(NumProt,DataProt,TipoDocumento,TipoStampa){
            var IdFascicolo =  '<%= TxtIdFascVSUB.Text %>';
            var CodiceFascicoloVSub =  '<%= TxtCodiceFascicoloVSub.Text %>';
			if (CodiceFascicoloVSub ==''){
			    alert ("Nessun Fascicolo Selezionato!");
			}else{
			    myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?objForm=Form1&TxtProt="+CostruisciId(NumProt)+"&VarVolontario=<%=Request.QueryString("IdEntitaSubentrante")%>&TxtData="+DataProt+"&VArUpdate=1&TipoDocumento="+CostruisciId(TipoDocumento)+"&NumeroFascicolo="+IdFascicolo+"&TipoStampa="+TipoStampa, "Visualizza", winOption);						
			}
		}
			
		function ApriAllegati(NumProt,DataProt)	{
            var IdFascicolo =  '<%= TxtIdFascP.Text %>';
			if (document.all.TxtCodiceFascicoloP.value =='')
			{
				alert ("Nessun Fascicolo Selezionato!");
			}else{	
				myWin = window.open("WfrmSIGEDElencoAllegati.aspx?NumeroProtocollo=" + document.getElementById(NumProt).value  + "&DataProtocollo=" + document.getElementById(DataProt).value + "&CodiceFascicolo="+IdFascicolo,"Visualizza","width=800,height=350,dependent=no,scrollbars=yes,status=si");		
			}
		}

			function Protocollazione(NumProt,DataProt,TipoDocumento)
			{
                var IdFascicolo =  '<%= TxtIdFascP.Text %>';
				if (document.getElementById(CostruisciId(NumProt)).value=='' || document.getElementById(CostruisciId(DataProt)).value=='')
					{
						myWin = window.open("WfrmSIGEDProtocollazione.aspx?CorrCodUnivoco=<%= Session("txtCodEnte")%>&Processo=VOLONTARI&VengoDa=SostVolontario&objForm=Form1&TxtFasc=TxtIdFascP&TxtCodFasc=TxtCodiceFascicoloP&TxtDescFascicolo=txtDescFascP&TxtProt="+NumProt+"&Fascicolo=" + Form1.TxtCodiceFascicoloP.value+"&IdAttivita=<%=Request.QueryString("IdAttivita")%>&VarVolontario=<%=Request.QueryString("identita")%>&TxtData="+DataProt+"&VArUpdate=3&TipoDocumento="+TipoDocumento+"&NumeroFascicolo="+IdFascicolo+"&DescFascicolo="+document.all.txtDescFascP.value+"&CodEnte=<%= Session("txtCodEnte")%>", "Visualizza",winOption);
					}
					else
					{
						var msg;
						msg=confirm("Si vuole inserire il documento Principale?");
						if (msg==false)
							{
								myWin = window.open("WfrmSIGEDProtocollazione.aspx?CorrCodUnivoco=<%= Session("txtCodEnte")%>&Processo=VOLONTARI&VengoDa=SostVolontario&objForm=Form1&TxtFasc=MainContent_TxtIdFascP&TxtCodFasc=MainContent_TxtCodiceFascicoloP&TxtDescFascicolo=MainContent_txtDescFascP&TxtProt="+CostruisciId(NumProt)+"&Fascicolo=" + document.getElementById(CostruisciId(TxtCodiceFascicoloP)).value+"&IdAttivita=<%=Request.QueryString("IdAttivita")%>&VarVolontario=<%=Request.QueryString("identita")%>&TxtData="+DataProt+"&VArUpdate=3&TipoDocumento="+TipoDocumento+"&NumeroFascicolo="+IdFascicolo+"&DescFascicolo="+document.getElementById(CostruisciId(txtDescFascP)).value+"&CodEnte=<%= Session("txtCodEnte")%>", "Visualizza",winOption);
								return false;
							}else
							{	
								myWin = window.open("WfrmSIGEDDocumentoPrincipale.aspx?NumProt=" + document.getElementById(CostruisciId(NumProt)).value + "&DataProt=" + document.getElementById(CostruisciId(DataProt)).value, "Visualizza", "width=400,height=200,dependent=no,scrollbars=yes,status=no");			
							}
					}	
			}				

			
			function ApriAllegatiVSOS(NumProt,DataProt)
			{
            var IdFascicolo =  '<%= TxtIdFascVSOS.Text %>';
            var CodiceFascicoloVSos =  '<%= TxtCodiceFascicoloVSos.Text %>';
					if (CodiceFascicoloVSos =='')
					{
						alert ("Nessun Fascicolo Selezionato!");
					}else{	
					
						myWin = window.open("WfrmSIGEDElencoAllegati.aspx?NumeroProtocollo=" + document.getElementById(CostruisciId(NumProt)).value + "&DataProtocollo=" + document.getElementById(CostruisciId(DataProt)).value + "&CodiceFascicolo="+IdFascicolo,"Visualizza","width=800,height=350,dependent=no,scrollbars=yes,status=si");
						
					}
			}

			function ProtocollazioneVSOS(NumProt,DataProt,TipoDocumento){
                var FascicoloVsos = '<%= TxtCodiceFascicoloVSos.Text %>'; 
                var DescFascVSos = '<%= txtDescFascVSos.Text %>';
                var IdFascicolo = '<%= TxtIdFascVSOS.Text %>';
                var VolSostituito =  '<%= hddCodVolSostituito.Value %>';
                var NumeroProtocollo = document.getElementById(CostruisciId(NumProt)).value;
                var DataProtocollo = document.getElementById(CostruisciId(DataProt)).value

				if (DataProtocollo=='' || NumeroProtocollo=='')	{
					myWin = window.open("WfrmSIGEDProtocollazione.aspx?CorrCodUnivoco=" +VolSostituito+"&Processo=VOLONTARI&VengoDa=SostVolontario&objForm=Form1&TxtFasc=MainContent_TxtIdFascVSos&TxtCodFasc=MainContent_TxtCodiceFascicoloVSos&MainContent_TxtDescFascicolo=txtDescFascVSos&TxtProt="+CostruisciId(NumProt)+"&Fascicolo=" + FascicoloVsos +"&VarVolontario=<%=Request.QueryString("identita")%>&TxtData="+CostruisciId(DataProt)+"&VArUpdate=1&TipoDocumento="+TipoDocumento+"&NumeroFascicolo="+IdFascicolo+"&DescFascicolo="+ DescFascVSos+"&CodEnte=<%= Session("pCodEnte")%>", "Visualizza",winOption);			
				}
				else
				{
					var msg;
					msg=confirm("Si vuole inserire il documento Principale?");
					if (msg==false)
						{
							myWin = window.open("WfrmSIGEDProtocollazione.aspx?CorrCodUnivoco=" + VolSostituito+"&Processo=VOLONTARI&VengoDa=SostVolontario&objForm=Form1&TxtFasc=MainContent_TxtIdFascVSos&TxtCodFasc=MainContent_TxtCodiceFascicoloVSos&TxtDescFascicolo=MainContent_txtDescFascVSos&TxtProt="+NumProt+"&Fascicolo=" + FascicoloVsos+"&VarVolontario=<%=Request.QueryString("identita")%>&TxtData="+DataProt+"&VArUpdate=1&TipoDocumento="+TipoDocumento+"&NumeroFascicolo="+IdFascicolo+"&DescFascicolo="+DescFascVSos+"&CodEnte=<%= Session("pCodEnte")%>", "Visualizza",winOption);			
							return false;
						}else
						{	
							myWin = window.open("WfrmSIGEDDocumentoPrincipale.aspx?NumProt=" + NumeroProtocollo + "&DataProt=" + DataProtocollo, "Visualizza", "width=400,height=200,dependent=no,scrollbars=yes,status=no");			
						}
				}	
			}		
			
	
			function ApriAllegatiVSUB(NumProt,DataProt,TipoStampa){
                var NumeroProtocollo = document.getElementById(CostruisciId(NumProt)).value;
                var DataProtocollo = document.getElementById(CostruisciId(DataProt)).value
                var IdFascicolo = '<%= TxtIdFascVSUB.Text %>';
                var CodiceFascicoloVSub =  '<%= TxtCodiceFascicoloVSub.Text %>';
				if (CodiceFascicoloVSub ==''){
						alert ("Nessun Fascicolo Selezionato!");
				}else{	
						myWin = window.open("WfrmSIGEDElencoAllegati.aspx?NumeroProtocollo=" + NumeroProtocollo + "&DataProtocollo=" + DataProtocollo + "&CodiceFascicolo="+IdFascicolo+"&TipoStampa="+TipoStampa,"Visualizza","width=800,height=350,dependent=no,scrollbars=yes,status=si");						
				}
			}

			function ProtocollazioneVSUB(NumProt,DataProt,TipoDocumento,TipoStampa)
			{
             var NumeroProtocollo = document.getElementById(CostruisciId(NumProt)).value;
            var DataProtocollo = document.getElementById(CostruisciId(DataProt)).value
            var IdFascicolo =  '<%= TxtIdFascVSub.Text %>';
            var TxtCodiceFascicoloVSub =  '<%= TxtCodiceFascicoloVSub.Text %>';
            var VolSubentro = document.getElementById(CostruisciId(hddCodVolSubentro)).value;
             var DescFascVSub = document.getElementById(CostruisciId(txtDescFascVSub)).value;
				if (NumeroProtocollo=='' || DataProtocollo=='')

					{
						myWin = window.open("WfrmSIGEDProtocollazione.aspx?CorrCodUnivoco=" + VolSubentro+"&Processo=VOLONTARI&VengoDa=SostVolontario&objForm=Form1&TxtFasc=MainContent_TxtIdFascVSub&TxtCodFasc=MainContent_TxtCodiceFascicoloVSub&TxtDescFascicolo=MainContent_txtDescFascVSub&TxtProt="+CostruisciId(NumProt)+"&Fascicolo=" + TxtCodiceFascicoloVSub+"&VarVolontario=<%=Request.QueryString("IdEntitaSubentrante")%>&TxtData="+CostruisciId(DataProt)+"&VArUpdate=1&TipoDocumento="+TipoDocumento+"&NumeroFascicolo="+IdFascicolo+"&DescFascicolo="+DescFascVSub+"&CodEnte=<%= Session("txtCodEnte")%>&TipoStampa="+TipoStampa, "Visualizza",winOption);			
					}
					else
					{
						var msg;
						msg=confirm("Si vuole inserire il documento Principale?");
						if (msg==false)
							{
								myWin = window.open("WfrmSIGEDProtocollazione.aspx?CorrCodUnivoco=" + VolSubentro+"&&Processo=VOLONTARI&VengoDa=SostVolontario&objForm=Form1&TxtFasc=MainContent_TxtIdFascVSub&TxtCodFasc=MainContent_TxtCodiceFascicoloVSub&TxtDescFascicolo=MainContent_txtDescFascVSub&TxtProt="+CostruisciId(NumProt)+"&Fascicolo=" + TxtCodiceFascicoloVSub+"&VarVolontario=<%=Request.QueryString("IdEntitaSubentrante")%>&TxtData="+CostruisciId(DataProt)+"&VArUpdate=1&TipoDocumento="+TipoDocumento+"&NumeroFascicolo="+IdFascicolo+"&DescFascicolo="+DescFascVSub+"&CodEnte=<%= Session("txtCodEnte")%>&TipoStampa="+TipoStampa, "Visualizza",winOption);
								return false;
							}else
							{	
								myWin = window.open("WfrmSIGEDDocumentoPrincipale.aspx?NumProt=" + NumeroProtocollo + "&DataProt=" + DataProtocollo, "Visualizza", "width=400,height=200,dependent=no,scrollbars=yes,status=no");			
							}
					}	
			}
			</script>
        <script type="text/javascript">	

        function ElencoDocumenti(TxtCodiceFascicolo){
            var IdFascicolo =  '<%= TxtIdFascP.Text %>';
            var IdTxtCodiceFascicolo = CostruisciId(TxtCodiceFascicolo);
            var IdTxtIdFasc = CostruisciId("TxtIdFascP");
            var codiceFascicolo = document.getElementById(IdTxtCodiceFascicolo).value;

            if (codiceFascicolo ==''){
				alert ("Specificare il numero fascicolo!");
			}else{
				myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?NumeroFascicolo=" + IdFascicolo, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");			
			}
			return false;
            }

        function ElencoFascicoli(TxtCodiceFascicolo,txtDescFasc) {
            var IdTxtCodiceFascicolo = CostruisciId(TxtCodiceFascicolo);
            var IdDescrizioneFascicolo = CostruisciId(txtDescFasc);
            var IdFascicolo =  '<%= TxtIdFascP.Text %>';
            var IdTxtIdFasc = CostruisciId("TxtIdFascP");
            var codiceFascicolo = document.getElementById(IdTxtCodiceFascicolo).value;
		    if(codiceFascicolo!=''){
                myWin = window.open("WfrmSIGEDElencoFascicoli.aspx?Processo=VOLONTARI&VArUpdate=1&IdAttivita=<%=Request.QueryString("IdAttivita")%>&VarVolontario=<%=Request.QueryString("IdVolVecchio")%>&VengoDa=SostVolontario&objForm=Form1&objNumero="+IdTxtCodiceFascicolo+"&objCodice="+IdTxtIdFasc+"&objDescFasc="+IdDescrizioneFascicolo+"&IdFascicolo=" + IdFascicolo, "Visualizza", "width=800,height=350,dependent=no,scrollbars=yes,status=no");
		    }else{
				alert ("Cancellare il Fascicolo prima di associarne uno nuovo!");
			}			
        }
 
        function ElencoDocumentiVSos(TxtCodiceFascicolo){
            var IdTxtIdFasc = CostruisciId("TxtIdFascVSos");
            var IdFascicolo =  '<%= TxtIdFascVSos.Text %>';
            var IdTxtCodiceFascicolo = CostruisciId(TxtCodiceFascicolo);
            var codiceFascicolo = document.getElementById(IdTxtCodiceFascicolo).value;

            if (codiceFascicolo ==''){
				alert ("Specificare il numero fascicolo!");
			}else{
				myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?NumeroFascicolo=" + IdFascicolo, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");			
			}
			return false;
            }

        function ElencoFascicoliVSos(TxtCodiceFascicolo,txtDescFasc) {
            var IdTxtCodiceFascicolo = CostruisciId(TxtCodiceFascicolo);
            var IdDescrizioneFascicolo = CostruisciId(txtDescFasc);
            var IdFascicolo =  '<%= TxtIdFascVSos.Text %>';
            var IdTxtIdFasc = CostruisciId("TxtIdFascVSos");
            var codiceFascicolo = document.getElementById(IdTxtCodiceFascicolo).value;
		    if(codiceFascicolo!=''){
                myWin = window.open("WfrmSIGEDElencoFascicoli.aspx?Processo=VOLONTARI&VArUpdate=1&IdAttivita=<%=Request.QueryString("IdAttivita")%>&VarVolontario=<%=Request.QueryString("IdVolVecchio")%>&VengoDa=SostVolontario&objForm=Form1&objNumero="+IdTxtCodiceFascicolo+"&objCodice="+IdTxtIdFasc+"&objDescFasc="+IdDescrizioneFascicolo+"&IdFascicolo=" + IdFascicolo, "Visualizza", "width=800,height=350,dependent=no,scrollbars=yes,status=no");
		    }else{
				alert ("Cancellare il Fascicolo prima di associarne uno nuovo!");
			}			
        }
        function ElencoDocumentiVSub(TxtCodiceFascicolo){
            var IdFascicolo =  '<%= TxtIdFascVSub.Text %>';
            var IdTxtIdFasc = CostruisciId("TxtIdFascVSub");
            var IdTxtCodiceFascicolo = CostruisciId(TxtCodiceFascicolo);
            var codiceFascicolo = document.getElementById(IdTxtCodiceFascicolo).value;

            if (codiceFascicolo ==''){
				alert ("Specificare il numero fascicolo!");
			}else{
				myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?NumeroFascicolo=" + IdFascicolo, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");			
			}
			return false;
            }

        function ElencoFascicoliVSub(TxtCodiceFascicolo,txtDescFasc) {
            var IdTxtCodiceFascicolo = CostruisciId(TxtCodiceFascicolo);
            var IdDescrizioneFascicolo = CostruisciId(txtDescFasc);
            var IdFascicolo =  '<%= TxtIdFascVSub.Text %>';
            var IdTxtIdFasc = CostruisciId("TxtIdFascVSub");
            var codiceFascicolo = document.getElementById(IdTxtCodiceFascicolo).value;
		    if(codiceFascicolo!=''){
                myWin = window.open("WfrmSIGEDElencoFascicoli.aspx?Processo=VOLONTARI&VArUpdate=1&IdAttivita=<%=Request.QueryString("IdAttivita")%>&VarVolontario=<%=Request.QueryString("IdVolVecchio")%>&VengoDa=SostVolontario&objForm=Form1&objNumero="+IdTxtCodiceFascicolo+"&objCodice="+IdTxtIdFasc+"&objDescFasc="+IdDescrizioneFascicolo+"&IdFascicolo=" + IdFascicolo, "Visualizza", "width=800,height=350,dependent=no,scrollbars=yes,status=no");
		    }else{
				alert ("Cancellare il Fascicolo prima di associarne uno nuovo!");
			}			
        }

		</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina" >
<legend>Elenco Documentazione Sostituzione Volontario</legend>
<div class="wrapper"  style="width:100%;border:0px">
    <div class= "row" style="height:auto" >
        <asp:label id="txtMSG" runat="server"  CssClass="msgInfo" ></asp:label>
        <asp:label id="lblmessaggiosopra"  runat="server" CssClass="msgErrore" ></asp:label>
        <input type="hidden" id="txtIdAttivita" runat="server" />
        <input type="hidden" id="hddCodVolSostituito"  runat="server"/> 
        <input type="hidden" id="hddCodVolSubentro" runat="server"/>
        <asp:TextBox  id="TxtIdFascP" visible="false" runat="server"/>
        <asp:TextBox  id="TxtIdFascVSos" visible="false" runat="server"/>
        <asp:TextBox  id="TxtIdFascVSub" visible="false" runat="server"/>
    </div>
<fieldset class="ContornoPagina"  >
<legend >Progetto</legend>
<div class="wrapper"  style="width:100%;border:0px">
    <div class= "row"  >
        <div class="collable" style="width:10%" >
            <asp:Label ID="LblProgetto" CssClass="label" AssociatedControlID="LblDatiProgetto" runat="server" Text="Progetto" ></asp:Label>
        </div>
        <div class="colOggetti" style="width:90%" >
            <asp:Label ID="LblDatiProgetto" CssClass="labelDati" runat="server" ></asp:Label>
        </div>
    </div>

    <div id="Progetto_RigheNascoste" class="rowGroup"  runat="server" visible="false" >
    <div class= "row" style="width:100%" >
        <div class="collable" style="width:25%" >
            <asp:Label ID="lblCodiceFascicoloP" CssClass="label" AssociatedControlID="TxtCodiceFascicoloP" runat="server" Text="Numero Fascicolo" Visible="False"></asp:Label>
        </div>
        <div class="colOggetti" style="width:60%" >
            <asp:TextBox ID="TxtCodiceFascicoloP" CssClass="textbox" ReadOnly="true" runat="server" Visible="False"></asp:TextBox>
        </div>
        <div class="collable" style="width:15%" >
            <asp:ImageButton style="CURSOR: hand;" id="cmdSelFascicoloP" 
                OnClientClick="ElencoFascicoli('TxtCodiceFascicoloP','txtDescFascP');" 
                runat="server" ImageUrl="images/cartella_small.png"
											AlternateText="Seleziona Codice Fascicolo"  
                ToolTip="Seleziona Codice Fascicolo" Visible="false"></asp:ImageButton>

            <asp:ImageButton style="CURSOR: hand;" id="cmdSelProtocollo0P" OnClientClick="ElencoDocumenti('TxtCodiceFascicoloP');" runat="server" ImageUrl="images/busta_small.png"
											ToolTip="Elenco Documenti" AlternateText="Elenco Documenti" Visible="false"></asp:ImageButton>
                <asp:ImageButton style="CURSOR: hand;" id="cmdFascCancP"  runat="server" ImageUrl="images/canc_small.png"
											ToolTip="Rimuovi Fascicolo/Protocolli" AlternateText="Rimuovi Fascicolo/Protocolli" Visible="false"></asp:ImageButton>


    </div>
    </div>
    <div class= "row" style="width:100%"  >
    <div class="collable" style="width:25%" >
        <asp:Label id="LblDescFascP" CssClass="label" AssociatedControlID="txtDescFascP"  Visible="false"   runat="server" Text="Descrizione"></asp:Label>
    </div>
   
    <div class="colOggetti" style="width:75%" >
            <asp:TextBox ID="txtDescFascP" Visible="false" CssClass="textbox"  ReadOnly="true" runat="server"
            TextMode="MultiLine"></asp:TextBox>

    </div>
    </div>
    </div>
     <div class="row">
        <div class="collable" style="width:100%">
            <asp:checkbox id="chkElencoVolontariAmmessi" runat="server" Text="Elenco Volontari Ammessi"></asp:checkbox>
        </div>
     </div>
   <div id="DivElencoVolontariAmmessi" class="rowGroup" runat="server" visible="false" >
     <div class="row" >
     <div class="colHyperLink" style="width:20%">
            <asp:hyperlink id="hplElencoVolontariAmmessi" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File"></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtEVA" AssociatedControlID="txtNumProtEVA" CssClass="label"  Text="N° Protocollo"  runat="server" Visible="false" ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="txtNumProtEVA" CssClass="textbox" runat="server"  Visible="false" ></asp:textbox>
        </div>
         <div class="colOggetti" style="width:15%" >
                <asp:imageButton id="ImgProtolloEVA" style="CURSOR: hand" OnClientClick="javascript:SelProtocollo('txtNumProtEVA','txtDataProtEVA','elencovolontariammessi')"
										runat="server" Visible="False" ImageUrl="images/busta_small.png" 
                    ToolTip="Seleziona Protocollo"  AlternateText="Seleziona Protocollo" 
                    Height="19px"></asp:imageButton>
                 <asp:imageButton id="ImgApriAllegatiEVA" style="CURSOR: hand" OnClientClick="javascript:ApriAllegati('txtNumProtEVA','txtDataProtEVA','elencovolontariammessi')"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="ImgProtocollazioneEVA" style="CURSOR: hand" OnClientClick="javascript:Protocollazione('txtNumProtEVA','txtDataProtEVA','elencovolontariammessi')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione" Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtEVA" CssClass="label"  AssociatedControlID="txtDataProtEVA"  runat="server" Visible="false" Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="txtDataProtEVA" CssClass="textbox" runat="server"  MaxLength="10"  Visible="false" ></asp:textbox>
	    </div>
     </div>
     </div>
     <div class="row">
        <div class="collable" style="width:100%">
            <asp:checkbox id="chkRinunciaserviziovolontarioMultipla" runat="server" Text="Rinuncia Servizio Volontario Multipla"></asp:checkbox>
        </div>
     </div>
   <div id="DivRinunciaserviziovolontarioMultipla" class="rowGroup" runat="server" visible="false" >
     <div class="row" >
     <div class="colHyperLink" style="width:20%">
            <asp:hyperlink id="hplRinunciaserviziovolontarioMultipla" runat="server" Visible="False" CssClass="colHyperLink" ToolTip="Download File" Text="Download File" ></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtRSVM" AssociatedControlID="txtNumProtRSVM" CssClass="label"  Text="N° Protocollo"  runat="server" Visible="false" ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="txtNumProtRSVM" CssClass="textbox" runat="server"  Visible="false" ></asp:textbox>
        </div>
         <div class="colOggetti" style="width:15%" >
                <asp:imageButton id="ImgProtolloRSVM" style="CURSOR: hand" OnClientClick="javascript:SelProtocollo('txtNumProtRSVM','txtDataProtRSVM','rinunciaserviziovolontariomultipla')"
										runat="server" Visible="False" ImageUrl="images/busta_small.png" ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo"></asp:imageButton>
                 <asp:imageButton id="ImgApriAllegatiRSVM" style="CURSOR: hand" OnClientClick="javascript:ApriAllegati('txtNumProtRSVM','txtDataProtRSVM','rinunciaserviziovolontariomultipla')"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="ImgProtocollazioneRSVM" style="CURSOR: hand" OnClientClick="javascript:Protocollazione('txtNumProtRSVM','txtDataProtRSVM','rinunciaserviziovolontariomultipla')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtRSVM" CssClass="label"  AssociatedControlID="txtDataProtRSVM"  runat="server" Visible="false" Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="txtDataProtRSVM" CssClass="textbox"  MaxLength="10" runat="server"    Visible="false" ></asp:textbox>
	    </div>

        </div>

        <div class="row" style="visibility:hidden;height:auto">
     <div class="colHyperLink"  style="width:20%">
            <asp:hyperlink id="hplRinunciaserviziovolontarioMultiplaCopiaReg" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File" ></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtRSVMCopiaReg" AssociatedControlID="txtNumProtRSVMCopiaReg" CssClass="label"  Text="N° Protocollo"  runat="server" Visible="false" ></asp:label>
        </div>
        <div class="colOggetti" style="width:25%" >
                <asp:textbox id="txtNumProtRSVMCopiaReg" CssClass="textbox" runat="server"  Visible="false" ReadOnly="true"></asp:textbox>
        </div>
         <div class="colOggetti" style="width:15%" >
                <asp:ImageButton id="ImgProtolloRSVMCopiaReg" style="CURSOR: hand" OnClientClick="javascript:SelProtocollo('txtNumProtRSVMCopiaReg','txtDataProtRSVMCopiaReg','rinunciaserviziovolontariomultiplaCopiaReg')"
										runat="server" Visible="False" ImageUrl="images/busta_small.png" ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo"></asp:ImageButton>
                 <asp:ImageButton id="ImgApriAllegatiRSVMCopiaReg" style="CURSOR: hand" OnClientClick="javascript:ApriAllegati('txtNumProtRSVMCopiaReg','txtDataProtRSVMCopiaReg','rinunciaserviziovolontariomultiplaCopiaReg')"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:ImageButton>
                <asp:ImageButton id="ImgProtocollazioneRSVMCopiaReg" style="CURSOR: hand" OnClientClick="javascript:Protocollazione('txtNumProtRSVMCopiaReg','txtDataProtRSVMCopiaReg','rinunciaserviziovolontariomultiplaCopiaReg')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione" Runat="server"></asp:ImageButton>
            </div>
        <div class="collable" style="width:10%" >
        <asp:label id="LblDataProtRSVMCopiaReg" CssClass="label"  AssociatedControlID="txtDataProtRSVMCopiaReg"  runat="server" Visible="false" Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="txtDataProtRSVMCopiaReg" CssClass="textbox" runat="server"  ReadOnly="true"  Visible="false" ></asp:textbox>
	    </div>

        </div>
        </div>
    </div>
     </fieldset>

<fieldset class="ContornoPagina">
<legend>Volontario Sostituito</legend>
<div class ="wrapper" style="width:100%;border:0px">
    <div class= "row">
         <div class="collable" style="width:10%">
            <asp:Label ID="LblNominativoSostituito" CssClass="label" 
                    AssociatedControlID="LblDatiNominativoSostituito" Text="Nominativo" 
                    runat="server" />
        </div>
        <div class="colOggetti"  style="width:90%">
            <asp:Label ID="LblDatiNominativoSostituito" CssClass="labelDati" runat="server" ></asp:Label>
        </div>
    </div>

    <div id="Volontario_RigheNascoste" class="rowGroup"  runat="server" visible="false" >
            <div class= "row"  >
                <div class="collable" style="width:25%" >
                    <asp:Label ID="lblCodiceFascicoloVSos" CssClass="label" AssociatedControlID="TxtCodiceFascicoloVSos" runat="server" Text="Numero Fascicolo" Visible="False"></asp:Label>
                </div>
                <div class="colOggetti" style="width:60%" >
                    <asp:TextBox ID="TxtCodiceFascicoloVSos" CssClass="textbox" ReadOnly="true" runat="server" Visible="False"></asp:TextBox>
                </div>
                <div class="collable" style="width:15%" >
                    <asp:ImageButton style="CURSOR: hand;" id="cmdSelFascicoloVSos" OnClientClick="ElencoFascicoliVSos('TxtCodiceFascicoloVSos','txtDescFascVSos');" runat="server" ImageUrl="images/cartella_small.png"
											        ToolTip="Seleziona Codice Fascicolo" AlternateText="Seleziona Codice Fascicolo" Visible="false"></asp:ImageButton>

                    <asp:ImageButton style="CURSOR: hand;" id="cmdSelProtocollo0VSos" OnClientClick="ElencoDocumentiVSos('TxtCodiceFascicoloVSos');" runat="server" ImageUrl="images/busta_small.png"
											        ToolTip="Elenco Documenti" AlternateText="Elenco Documenti" Visible="false"></asp:ImageButton>
                            <asp:ImageButton style="CURSOR: hand;" id="cmdFascCancVSos"  runat="server" ImageUrl="images/canc_small.png"
											        ToolTip="Rimuovi Fascicolo/Protocolli" AlternateText="Rimuovi Fascicolo/Protocolli" Visible="false"></asp:ImageButton>

            </div>
        </div>
        <div class= "row"  >
            <div class="collable" style="width:25%" >
                <asp:Label id="LblDescFascVSos" CssClass="label" AssociatedControlID="txtDescFascVSos"  Visible="false"   runat="server" Text="Descrizione"></asp:Label>
            </div>
   
            <div class="colOggetti" style="width:75%" >
                    <asp:TextBox ID="txtDescFascVSos" Visible="false" CssClass="textbox"  ReadOnly="true" runat="server"
                    TextMode="MultiLine"></asp:TextBox>

            </div>
        </div>
        
</div>
  
<div class="row">
    <div class="collable" style="width:100%">
        <asp:checkbox id="chkRinunciaserviziovolontario" runat="server" Text="Rinuncia Servizio Volontario"></asp:checkbox>
    </div>
</div>
    <div id="DivRinunciaserviziovolontario" class="rowGroup" runat="server" visible="false" >
    <div class="row" >
    <div class="colHyperLink"  style="width:20%">
        <asp:hyperlink id="hplRinunciaserviziovolontario" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File" ></asp:hyperlink>
    </div>
    <div class="collable" style="width:15%" >
        <asp:label id="LblNumProtRSV" AssociatedControlID="txtNumProtRSV" CssClass="label"  Text="N° Protocollo"  runat="server" Visible="false" ></asp:label>
    </div>
    <div class="colOggetti" style="width:15%" >
            <asp:textbox id="txtNumProtRSV" CssClass="textbox" runat="server"  Visible="false" ></asp:textbox>
    </div>
        <div class="colOggetti" style="width:15%" >
            <asp:imageButton id="ImgProtolloRSV" style="CURSOR: hand" OnClientClick="SelProtocolloVSOS('txtNumProtRSV','txtDataProtRSV','Rinuncia Servizio Volontario')"
									runat="server" Visible="False" ImageUrl="images/busta_small.png" ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo"></asp:imageButton>
                <asp:imageButton id="ImgApriAllegatiRSV" style="CURSOR: hand" OnClientClick="ApriAllegatiVSOS('txtNumProtRSV','txtDataProtRSV','Rinuncia Servizio Volontario')"
									runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
            <asp:imageButton id="ImgProtocollazioneRSV" style="CURSOR: hand" OnClientClick="ProtocollazioneVSOS('txtNumProtRSV','txtDataProtRSV','Rinuncia Servizio Volontario')"
									Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
        </div>
    <div class="collable" style="width:20%" >
    <asp:label id="LblDataProtRSV" CssClass="label"  AssociatedControlID="txtDataProtRSV"  runat="server" Visible="false" Text="Data Protocollo"></asp:label>
</div>
    <div class="colOggetti" style="width:15%" >
		<asp:textbox id="txtDataProtRSV" CssClass="textbox" runat="server"    Visible="false" ></asp:textbox>
	</div>
    </div>
    <div class="row" >
    <div class="colHyperLink"  style="width:20%">
        <asp:hyperlink id="hplRinunciaserviziovolontarioCopiaReg" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File" ></asp:hyperlink>
    </div>
    <div class="collable" style="width:15%" >
        <asp:label id="LblNumProtRSVCopiaReg" AssociatedControlID="txtNumProtRSVCopiaReg" CssClass="label"  Text="N° Protocollo"  runat="server" Visible="false" ></asp:label>
    </div>
    <div class="colOggetti" style="width:15%" >
            <asp:textbox id="txtNumProtRSVCopiaReg" CssClass="textbox" runat="server"  Visible="false" ></asp:textbox>
    </div>
        <div class="colOggetti" style="width:15%" >
            <asp:imageButton id="ImgProtolloRSVCopiaReg" style="CURSOR: hand" OnClientClick="javascript:SelProtocolloVSOS('txtNumProtRSVCopiaReg','txtDataProtRSVCopiaReg','rinunciaserviziovolontarioCopiaReg')"
									runat="server" Visible="False" ImageUrl="images/busta_small.png" ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo"></asp:imageButton>
                <asp:imageButton id="ImgApriAllegatiRSVCopiaReg" style="CURSOR: hand" OnClientClick="javascript:ApriAllegatiVSOS('txtNumProtRSVCopiaReg','txtDataProtRSVCopiaReg','rinunciaserviziovolontarioCopiaReg')"
									runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
            <asp:imageButton id="ImgProtocollazioneRSVCopiaReg" style="CURSOR: hand" OnClientClick="javascript:ProtocollazioneVSOS('txtNumProtRSVCopiaReg','txtDataProtRSVCopiaReg','rinunciaserviziovolontarioCopiaReg')"
									Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
        </div>
    <div class="collable" style="width:20%" >
    <asp:label id="LblDataProtRSVCopiaReg" CssClass="label"  AssociatedControlID="txtDataProtRSVCopiaReg"  runat="server" Visible="false" Text="Data Protocollo"></asp:label>
</div>
    <div class="colOggetti" style="width:15%" >
		<asp:textbox id="txtDataProtRSVCopiaReg" CssClass="textbox" runat="server"    Visible="false" ></asp:textbox>
	</div>

    </div>
</div>

<div class="row" >
        <div class="collable" style="width:100%">
        <asp:checkbox id="chkLetteraChiusuraInServizio" runat="server" Text="Lettera Chiusura In Servizio"></asp:checkbox>
    </div>
    </div>
<div id="DivLetteraChiusuraInServizio" class="rowGroup" runat="server" visible="false" >
<div class="row" >
    <div class="colHyperLink"  style="width:20%">
        <asp:hyperlink id="hplLetteraChiusuraInServizio" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File" ></asp:hyperlink>
    </div>
    <div class="collable" style="width:15%" >
        <asp:label id="LblNumProtLCS" AssociatedControlID="txtNumProtLCS" CssClass="label"  Text="N° Protocollo"  runat="server" Visible="false" ></asp:label>
    </div>
    <div class="colOggetti" style="width:15%" >
            <asp:textbox id="txtNumProtLCS" CssClass="textbox" runat="server"  Visible="false" ReadOnly="true"></asp:textbox>
              
        </div>
        <div class="colOggetti" style="width:15%" >
            <asp:imageButton id="ImgProtolloLCS" style="CURSOR: hand" OnClientClick="SelProtocolloVSOS('txtNumProtLCS','txtDataProtLCS','Chiusura In Servizio')"
									runat="server" Visible="False" ImageUrl="images/busta_small.png" ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo"></asp:imageButton>
                <asp:imageButton id="ImgApriAllegatiLCS" style="CURSOR: hand" OnClientClick="ApriAllegatiVSOS('txtNumProtLCS','txtDataProtLCS','Chiusura In Servizio')"
									runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
            <asp:imageButton id="ImgProtocollazioneLCS" style="CURSOR: hand" OnClientClick="ProtocollazioneVSOS('txtNumProtLCS','txtDataProtLCS','Chiusura In Servizio')"
									Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
        </div>
    <div class="collable" style="width:20%" >
    <asp:label id="LblDataProtLCS" CssClass="label"  AssociatedControlID="txtDataProtLCS"  runat="server" Visible="false" Text="Data Protocollo"></asp:label>
</div>
    <div class="colOggetti" style="width:15%" >
		<asp:textbox id="txtDataProtLCS" CssClass="textbox" MaxLength="10" runat="server"  ReadOnly="true"  Visible="false" ></asp:textbox>
	</div>
</div>
</div>
<div class="row" >
    <div class="collable" style="width:100%">
        <asp:checkbox id="chkLetteraEsclusionePerAssenzaIngiustificata" runat="server" Text="Lettera Esclusione Per Assenza Ingiustificata"></asp:checkbox>
    </div>
</div>
<div id="DivLetteraEsclusionePerAssenzaIngiustificata" class="rowGroup" runat="server" visible="false" >
    <div class="row">
    <div class="colHyperLink"  style="width:20%">
        <asp:hyperlink id="hplLetteraEsclusionePerAssenzaIngiustificata" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File" ></asp:hyperlink>
    </div>
    <div class="collable" style="width:15%" >
        <asp:label id="LblNumProtLEAI" AssociatedControlID="txtNumProtLEAI" CssClass="label"  Text="N° Protocollo"  runat="server" Visible="false" ></asp:label>
    </div>
    <div class="colOggetti" style="width:15%" >
            <asp:textbox id="txtNumProtLEAI" CssClass="textbox" runat="server"  Visible="false" ReadOnly="true"></asp:textbox>

        </div>
    <div class="colOggetti" style="width:15%" >
                    <asp:imageButton id="ImgProtolloLEAI" style="CURSOR: hand" OnClientClick="SelProtocolloVSOS('txtNumProtLEAI','txtDataProtLEAI','Lettera Esclusione Per Assenza Ingiustificata')"
								runat="server" Visible="False" ImageUrl="images/busta_small.png" ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo"></asp:imageButton>
            <asp:imageButton id="ImgApriAllegatiLEAI" style="CURSOR: hand" OnClientClick="ApriAllegatiVSOS('txtNumProtLEAI','txtDataProtLEAI','Lettera Esclusione Per Assenza Ingiustificata')"
								runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
        <asp:imageButton id="ImgProtocollazioneLEAI" style="CURSOR: hand" OnClientClick="ProtocollazioneVSOS('txtNumProtLEAI','txtDataProtLEAI','Lettera Esclusione Per Assenza Ingiustificata')"
								Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
    </div>
    <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtLEAI" CssClass="label"  AssociatedControlID="txtDataProtLEAI"  runat="server" Visible="false" Text="Data Protocollo"></asp:label>
    </div>
    <div class="colOggetti" style="width:15%" >
		<asp:textbox id="txtDataProtLEAI" CssClass="textbox" MaxLength="10" runat="server"  ReadOnly="true"  Visible="false" ></asp:textbox>
	</div>

    </div>
</div>
<div class="row" >
    <div class="collable" style="width:100%">
    <asp:checkbox id="chkLetteraEsclusionePerGiorniPermesso" runat="server" Text="Lettera Esclusione Per Giorni Permesso"></asp:checkbox>
</div>
</div>
<div id="DivEsclusionePerGiorniPermesso" class="rowGroup" runat="server" visible="false" >
<div class="row" >
    <div class="colHyperLink"  style="width:20%">
        <asp:hyperlink id="hplLetteraEsclusionePerGiorniPermesso" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File" ></asp:hyperlink>
    </div>
    <div class="collable" style="width:15%" >
        <asp:label id="LblNumProtLEGP" AssociatedControlID="txtNumProtLEGP" CssClass="label"  Text="N° Protocollo"  runat="server" Visible="false" ></asp:label>
    </div>
    <div class="colOggetti" style="width:15%" >
            <asp:textbox id="txtNumProtLEGP" CssClass="textbox" runat="server"  Visible="false" ReadOnly="true"></asp:textbox>
    </div>
        <div class="colOggetti" style="width:15%" >
            <asp:imageButton id="ImgProtolloLEGP" style="CURSOR: hand" OnClientClick="SelProtocolloVSOS('txtNumProtLEGP','txtDataProtLEGP','Lettera Esclusione Per Giorni Permesso')"
									runat="server" Visible="False" ImageUrl="images/busta_small.png" ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo"></asp:imageButton>
                <asp:imageButton id="ImgApriAllegatiLEGP" style="CURSOR: hand" OnClientClick="ApriAllegatiVSOS('txtNumProtLEGP','txtDataProtLEGP','Lettera Esclusione Per Giorni Permesso')"
									runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
            <asp:imageButton id="ImgProtocollazioneLEGP" style="CURSOR: hand" OnClientClick="ProtocollazioneVSOS('txtNumProtLEGP','txtDataProtLEGP','Lettera Esclusione Per Giorni Permesso')"
									Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
        </div>
    <div class="collable" style="width:20%" >
    <asp:label id="LblDataProtLEGP" CssClass="label"  AssociatedControlID="txtDataProtLEGP"  runat="server" Visible="false" Text="Data Protocollo"></asp:label>
</div>
    <div class="colOggetti" style="width:15%" >
		<asp:textbox id="txtDataProtLEGP" CssClass="textbox" MaxLength="10" runat="server"  ReadOnly="true"  Visible="false" ></asp:textbox>
	</div>
    </div>
    </div>
    <div class="row" >
    <div class="collable" style="width:100%">
        <asp:checkbox id="chkLetteraEsclusionePerSuperamentoMalattia" runat="server" Text="Lettera Esclusione Per Superamento Malattia"></asp:checkbox>
    </div>
    </div>
    <div id="DivLetteraEsclusionePerSuperamentoMalattia" class="rowGroup" runat="server" visible="false" >
    <div class="row" >
        <div class="colHyperLink"  style="width:20%">
        <asp:hyperlink id="hplLetteraEsclusionePerSuperamentoMalattia" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File" ></asp:hyperlink>
    </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtLESM" AssociatedControlID="txtNumProtLESM" CssClass="label"  Text="N° Protocollo"  runat="server" Visible="false" ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="txtNumProtLESM" CssClass="textbox" runat="server"  Visible="false" ReadOnly="true"></asp:textbox>
        </div>
        <div class="colOggetti" style="width:15%" >
            <asp:ImageButton id="ImgProtolloLESM" style="CURSOR: hand" OnClientClick="SelProtocolloVSOS('txtNumProtLESM','txtDataProtLESM','Lettera Esclusione Per Superamento Malattia')"
									runat="server" Visible="False" ImageUrl="images/busta_small.png" ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo"></asp:ImageButton>
                <asp:ImageButton id="ImgApriAllegatiLESM" style="CURSOR: hand" OnClientClick="ApriAllegatiVSOS('txtNumProtLESM','txtDataProtLESM','Lettera Esclusione Per Superamento Malattia')"
									runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:ImageButton>
            <asp:ImageButton id="ImgProtocollazioneLESM" style="CURSOR: hand" OnClientClick="ProtocollazioneVSOS('txtNumProtLESM','txtDataProtLESM','Lettera Esclusione Per Superamento Malattia')"
									Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:ImageButton>
       </div>
        <div class="collable" style="width:20%" >
            <asp:label id="LblDataProtLESM" CssClass="label"  AssociatedControlID="txtDataProtLESM"  runat="server" Visible="false" Text="Data Protocollo"></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="txtDataProtLESM" CssClass="textbox" MaxLength="10" runat="server"  ReadOnly="true"  Visible="false" ></asp:textbox>
	    </div>
    </div>
    </div>
    <div class="row" >
        <div class="collable" style="width:100%">
        <asp:checkbox id="chkLetteraEsclusione" runat="server" Text="Lettera Esclusione Doppia Domanda"></asp:checkbox>
    </div>
    </div>
    <div id="DivLetteraEsclusioneDoppiaDomanda" class="rowGroup" runat="server" visible="false" >
    <div class="row">
    <div class="colHyperLink"  style="width:20%">
        <asp:hyperlink id="hplLetteraEsclusione" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File" ></asp:hyperlink>
    </div>
    <div class="collable" style="width:15%" >
        <asp:label id="LblNumProtLEDD" AssociatedControlID="txtNumProtLEDD" CssClass="label"  Text="N° Protocollo"  runat="server" Visible="false" ></asp:label>
    </div>
    <div class="colOggetti" style="width:15%" >
        <asp:textbox id="txtNumProtLEDD" CssClass="textbox" runat="server"  Visible="false" ReadOnly="true"></asp:textbox>
     </div>
    <div class="colOggetti" style="width:15%" >
        <asp:imageButton id="ImgProtolloLEDD" style="CURSOR: hand" OnClientClick="SelProtocolloVSOS('txtNumProtLEDD','txtDataProtLEDD','Lettera Esclusione Volontario')"
								runat="server" Visible="False" ImageUrl="images/busta_small.png" ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo"></asp:imageButton>
            <asp:imageButton id="ImgApriAllegatiLEDD" style="CURSOR: hand" OnClientClick="ApriAllegatiVSOS('txtNumProtLEDD','txtDataProtLEDD','Lettera Esclusione Volontario')"
								runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
        <asp:imageButton id="ImgProtocollazioneLEDD" style="CURSOR: hand" OnClientClick="ProtocollazioneVSOS('txtNumProtLEDD','txtDataProtLEDD','Lettera Esclusione Volontario')"
								Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
    </div>

    <div class="collable" style="width:20%" >
    <asp:label id="LblDataProtLEDD" CssClass="label"  AssociatedControlID="txtDataProtLEDD"  runat="server" Visible="false" Text="Data Protocollo"></asp:label>
</div>
    <div class="colOggetti" style="width:15%" >
		<asp:textbox id="txtDataProtLEDD" CssClass="textbox" MaxLength="10" runat="server"  ReadOnly="true"  Visible="false" ></asp:textbox>
	</div>

    </div>
    </div>

    <div class="rowGroup" id="DivElementiObsoleti" style="visibility:hidden">
    <div class="row" style="visibility:hidden;height:auto;">
    <div class="collable" style="width:100%">
        <asp:checkbox id="chkLetteraChiusura" runat="server" Text="Lettera Chiusura Iniziale"></asp:checkbox>
    </div>
    </div>
    <div id="DivLetteraChiusura" class="rowGroup" runat="server" visible="false" >
<div class="row" >
    <div class="colHyperLink"  style="width:20%">
        <asp:hyperlink id="hplLetteraChiusura" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File" ></asp:hyperlink>
    </div>
    <div class="collable" style="width:15%" >
        <asp:label id="LblNumProtLCI" AssociatedControlID="txtNumProtLCI" CssClass="label"  Text="N° Protocollo"  runat="server" Visible="false" ></asp:label>
    </div>
    <div class="colOggetti" style="width:15%" >
            <asp:textbox id="txtNumProtLCI" CssClass="textbox" runat="server"  Visible="false" ReadOnly="true"></asp:textbox>
    </div>
        <div class="colOggetti" style="width:15%" >
            <asp:imageButton id="ImgProtolloLCI" style="CURSOR: hand" OnClientClick="SelProtocolloVSOS('txtNumProtLCI','txtDataProtLCI','Chiusura Iniziale')"
									runat="server" Visible="False" ImageUrl="images/busta_small.png" ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo"></asp:imageButton>
                <asp:imageButton id="ImgApriAllegatiLCI" style="CURSOR: hand" OnClientClick="ApriAllegatiVSOS('txtNumProtLCI','txtDataProtLCI','Chiusura Iniziale')"
									runat="server" Visible="False" ImageUrl="images/giu_small.png" 
                ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px" 
                Width="20px"></asp:imageButton>
            <asp:imageButton id="ImgProtocollazioneLCI" style="CURSOR: hand" OnClientClick="ProtocollazioneVSOS('txtNumProtLCI','txtDataProtLCI','Chiusura Iniziale')"
									Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
        </div>
    <div class="collable" style="width:20%" >
    <asp:label id="LblDataProtLCI" CssClass="label"  AssociatedControlID="txtDataProtLCI"  runat="server" Visible="false" Text="Data Protocollo"></asp:label>
</div>
    <div class="colOggetti" style="width:15%" >
		<asp:textbox id="txtDataProtLCI" CssClass="textbox" MaxLength="10" runat="server"  ReadOnly="true"  Visible="false" ></asp:textbox>
	</div>

    </div>
    </div>

    </div>
    </div>
</fieldset>

<fieldset class="ContornoPagina" style="width:100%">
<legend>Volontario Subentrante</legend>
<div class ="wrapper" style="width:100%;border:0px">
 <div class="rowGroup" >
    <div class="collable" style="width:10%" >
        <asp:Label ID="LblNominativoSubentrante"  CssClass="label" AssociatedControlID="LblDatiNominativoSubentrante" runat="server" Text="Nominativo" ></asp:Label>
    </div>
        <div class="colOggetti" style="width:90%" >
        <asp:Label ID="LblDatiNominativoSubentrante" CssClass="labelDati" runat="server" ></asp:Label>
    </div>
</div>
     <div id="Subentrante_RigheNascoste" class="rowGroup" style="width:100%"  runat="server" visible="false" >
             <div class= "row"   >
                   <div class="collable" style="width:25%" >
                       <asp:Label ID="lblCodiceFascicoloVSub" CssClass="label" AssociatedControlID="TxtCodiceFascicoloVSub" runat="server" Text="Numero Fascicolo" Visible="False"></asp:Label>
                   </div>
                   <div class="colOggetti" style="width:60%" >
                       <asp:TextBox ID="TxtCodiceFascicoloVSub" CssClass="textbox" ReadOnly="true" runat="server" Visible="False"></asp:TextBox>
                   </div>
                    <div class="collable" style="width:15%" >
                        <asp:ImageButton style="CURSOR: hand;" id="cmdSelFascicoloVSub" OnClientClick="ElencoFascicoliVSub('TxtCodiceFascicoloVSub','txtDescFascVSub');" runat="server" ImageUrl="images/cartella_small.png"
											            ToolTip="Seleziona Codice Fascicolo" AlternateText="Seleziona Codice Fascicolo" Visible="false"></asp:ImageButton>

                        <asp:ImageButton style="CURSOR: hand;" id="cmdSelProtocollo0VSub" OnClientClick="ElencoDocumentiVSub('TxtCodiceFascicoloVSub');" runat="server" ImageUrl="images/busta_small.png"
											            ToolTip="Elenco Documenti" AlternateText="Elenco Documenti" Visible="false"></asp:ImageButton>
                                  <asp:ImageButton style="CURSOR: hand;" id="cmdFascCancVSub"  runat="server" ImageUrl="images/canc_small.png"
											        ToolTip="Rimuovi Fascicolo/Protocolli" AlternateText="Rimuovi Fascicolo/Protocolli" Visible="false"></asp:ImageButton>

                </div>
            </div>
            <div class= "row"  >
                <div class="collable" style="width:25%" >
                   <asp:Label id="LblDescFascVSub" CssClass="label" AssociatedControlID="txtDescFascVSub"  Visible="false"   runat="server" Text="Descrizione"></asp:Label>
                </div>
   
               <div class="colOggetti" style="width:75%" >
                      <asp:TextBox ID="txtDescFascVSub" Visible="false" CssClass="textbox"  ReadOnly="true" runat="server"
                      TextMode="MultiLine"></asp:TextBox>

               </div>
            </div>

      </div>
     <div class="row" >
        <div class="collable"  style="width:100%">
            <asp:checkbox id="chkAssegnazioneVolontario" runat="server" Text="Assegnazione Volontario"></asp:checkbox>
        </div>
     </div>
     <div id="DivAssegnazioneVolontario" class="rowGroup" style="width:100%" runat="server" visible="false" >
     <div class="row" >
         <div class="colHyperLink"  style="width:20%">
            <asp:hyperlink id="hplAssegnazioneVolontario" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File" ></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtAV" AssociatedControlID="txtNumProtAV" CssClass="label"  Text="N° Protocollo"  runat="server" Visible="false" ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="txtNumProtAV" CssClass="textbox" runat="server"  Visible="false" ></asp:textbox>
        </div>
         <div class="collable" style="width:15%" >
                <asp:imageButton id="ImgProtolloAV" style="CURSOR: hand" OnClientClick="javascript:SelProtocolloVSUB('txtNumProtAV','txtDataProtAV','Assegnazione Volontario - Nazionale')"
										runat="server" Visible="False" ImageUrl="images/busta_small.png" ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo"></asp:imageButton>
                 <asp:imageButton id="ImgApriAllegatiAV" style="CURSOR: hand" OnClientClick="javascript:ApriAllegatiVSUB('txtNumProtAV','txtDataProtAV','Assegnazione Volontario - Nazionale')"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="ImgProtocollazioneAV" style="CURSOR: hand" OnClientClick="javascript:ProtocollazioneVSUB('txtNumProtAV','txtDataProtAV','Assegnazione Volontario - Nazionale')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtAV" CssClass="label"  AssociatedControlID="txtDataProtAV"  runat="server" Visible="false" Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="txtDataProtAV" CssClass="textbox" runat="server"    Visible="false" ></asp:textbox>
	    </div>
        </div>
       </div>
     <div class="row"  >
         <div class="colHyperLink"  style="width:20%">
                <asp:hyperlink id="hplAssegnazioneVolontarioB" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File" ></asp:hyperlink>
            </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtAVB" AssociatedControlID="txtNumProtAVB" CssClass="label"  Text="N° Protocollo"  runat="server" Visible="false" ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="txtNumProtAVB" CssClass="textbox" runat="server"  Visible="false" ></asp:textbox>
        </div>
         <div class="collable" style="width:15%" >
                <asp:imageButton id="ImgProtolloAVB" style="CURSOR: hand" OnClientClick="javascript:SelProtocolloVSUB('txtNumProtAVB','txtDataProtAVB','Assegnazione Volontario - Nazionale','AssVol')"
										runat="server" Visible="False" ImageUrl="images/busta_small.png" ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo"></asp:imageButton>
                 <asp:imageButton id="ImgApriAllegatiAVB" style="CURSOR: hand" OnClientClick="javascript:ApriAllegatiVSUB('txtNumProtAVB','txtDataProtAVB','Assegnazione Volontario - Nazionale','AssVol')"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="ImgProtocollazioneAVB" style="CURSOR: hand" OnClientClick="javascript:ProtocollazioneVSUB('txtNumProtAVB','txtDataProtAVB','Assegnazione Volontario - Nazionale','AssVol')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtAVB" CssClass="label"  AssociatedControlID="txtDataProtAVB"  runat="server" Visible="false" Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="txtDataProtAVB" CssClass="textbox" MaxLength="10" runat="server"    Visible="false" ></asp:textbox>
	    </div>
     </div>
</div>
</fieldset>
  
<div class="RigaPulsanti">
<asp:Button
    ID="imgGeneraFile" runat="server" CssClass="Pulsante" Text="Genera" />&nbsp;
    <asp:Button 
    ID="imgChiudi" CssClass="Pulsante" runat="server"
        Text="Chiudi"  />

</div>


</div>
</fieldset>
</asp:Content>
