<%@ Page Title="Elenco Documentazione Volontario" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmElencoDocumentazioneVolontario.aspx.vb" Inherits="Futuro.WfrmElencoDocumentazioneVolontario" %>
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
<%--<script type="text/javascript">
			function SelProtocollo(NumProt,DataProt,TipoDocumento)
			{
            var NumFascicolo =  '<%= TxtNumFascicolo.value %>';
				if (NumFascicolo =='')
				{
					alert ("Nessun Fascicolo Selezionato!");
				}else{
					myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?objForm=Form1&TxtProt="+CostruisciId(NumProt)+"&VarVolontario=<%=Request.QueryString("IdVol")%>&TxtData="+CostruisciId(DataProt)+"&VArUpdate=1&TipoDocumento="+TipoDocumento+"&NumeroFascicolo=<%=Request.QueryString("NumeroFascicolo")%>", "Visualizza", winOption);				
					
				}
			}
			
			function ApriAllegati(NumProt,DataProt)
			{
            var NumFascicolo =  '<%= TxtNumFascicolo.value %>';
				if (NumFascicolo =='')
				{
					alert ("Nessun Fascicolo Selezionato!");
				}else{	
					myWin = window.open("WfrmSIGEDElencoAllegati.aspx?NumeroProtocollo=" + CostruisciId(NumProt) + "&DataProtocollo=" + CostruisciId(DataProt)+ "&CodiceFascicolo=" + NumFascicolo,"Visualizza",winOption);		
				}
			}

			function Protocollazione(NumProt,DataProt,TipoDocumento)
			{
             var NumeroFascicoloinVisione =  '<%= TxtNumeroFascicoloinVisione.Text %>';
				if (document.getElementById(NumProt).value=='' || document.getElementById(CostruisciId(DataProt)).value=='')
				{							  
					myWin = window.open("WfrmSIGEDProtocollazione.aspx?CorrCodUnivoco=<%=Request.QueryString("CorrCodUnivoco")%>&Processo=VOLONTARI&objForm=Form1&txtnumerofascicoloinvisione=MainContent_txtNumeroFascicoloinVisione&txtcodicefascicolonascosto=MainContent_TxtCodFascicolo&txtdescrizionefascicolo=MainContent_txtdescrizionefascicolo&TxtFasc=TxtNumFascicolo&TxtCodFasc=MainContent_TxtCodFascicolo&TxtDescFascicolo=MainContent_TxtDescFAsc&TxtProt="+CostruisciId(NumProt)+"&Fascicolo=" + NumeroFascicoloinVisione+"&VarVolontario=<%=Request.QueryString("IdVol")%>&TxtData="+ CostruisciId(DataProt) +"&VArUpdate=1&TipoDocumento="+TipoDocumento+"&Fascicolo=<%=Request.QueryString("Fascicolo")%>&NumeroFascicolo=<%=Request.QueryString("NumeroFascicolo")%>&DescFascicolo=<%=Request.QueryString("DescFascicolo")%>&CodEnte=<%= Session("txtCodEnte")%>", "Visualizza",winOption);
				}
				else
				{
					var msg;
					msg=confirm("Si vuole inserire il documento Principale?");
					if (msg==false)
					{
						myWin = window.open("WfrmSIGEDProtocollazione.aspx?CorrCodUnivoco=<%=Request.QueryString("CorrCodUnivoco")%>&Processo=VOLONTARI&objForm=Form1&txtnumerofascicoloinvisione=MainContent_txtNumeroFascicoloinVisione&txtcodicefascicolonascosto=MainContent_TxtCodFascicolo&txtdescrizionefascicolo=MainContent_txtdescrizionefascicolo&TxtFasc=MainContent_TxtNumFascicolo&TxtCodFasc=MainContent_TxtCodFascicolo&TxtDescFascicolo=MainContent_TxtDescFAsc&TxtProt="+CostruisciId(NumProt)+"&Fascicolo=" + NumeroFascicoloinVisione+"&VarVolontario=<%=Request.QueryString("IdVol")%>&TxtData="+ CostruisciId(DataProt) +"&VArUpdate=1&TipoDocumento="+TipoDocumento+"&Fascicolo=<%=Request.QueryString("Fascicolo")%>&NumeroFascicolo=<%=Request.QueryString("NumeroFascicolo")%>&DescFascicolo=<%=Request.QueryString("DescFascicolo")%>&CodEnte=<%= Session("txtCodEnte")%>", "Visualizza",winOption);			
						return false;
					}else
					{	
						myWin = window.open("WfrmSIGEDDocumentoPrincipale.aspx?NumProt=" + document.getElementById(CostruisciId(NumProt)).value + "&DataProt=" + document.getElementById(CostruisciId(DataProt)).value, "Visualizza", winOption);			
					}
				}	
			}			
			function SelProtocolloAssVol(NumProt,DataProt,TipoDocumento,TipoStampa)
			{
				var NumFascicolo =  '<%= TxtNumFascicolo.value %>';
				if (NumFascicolo =='')
				{
					alert ("Nessun Fascicolo Selezionato!");
				}else
				{
					myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?objForm=Form1&TxtProt="+CostruisciId(NumProt)+"&VarVolontario=<%=Request.QueryString("IdVol")%>&TxtData="+CostruisciId(DataProt)+"&VArUpdate=1&TipoDocumento="+TipoDocumento+"&NumeroFascicolo=<%=Request.QueryString("NumeroFascicolo")%>&TipoStampa="+TipoStampa, "Visualizza", winOption);				
				}
			}
			
			function ApriAllegatiAssVol(NumProt,DataProt,TipoStampa)
			{
				var NumFascicolo =  '<%= TxtNumFascicolo.value %>';
				if (NumFascicolo =='')
				{
					alert ("Nessun Fascicolo Selezionato!");
				}else{	
					myWin = window.open("WfrmSIGEDElencoAllegati.aspx?NumeroProtocollo=" + CostruisciId(NumProt) + "&DataProtocollo=" + CostruisciId(DataProt)+ "&CodiceFascicolo=" + NumFascicolo + "&TipoStampa="+TipoStampa,"Visualizza",winOption);		
				}
		}

			function ProtocollazioneAssVol(NumProt,DataProt,TipoDocumento,TipoStampa)
			{	
            var NumeroFascicoloinVisione =  '<%= TxtNumeroFascicoloinVisione.Text %>';
				if (document.getElementById(CostruisciId(NumProt)).value=='' || document.getElementById(CostruisciId(DataProt)).value=='')
				{
					myWin = window.open("WfrmSIGEDProtocollazione.aspx?CorrCodUnivoco=<%=Request.QueryString("CorrCodUnivoco")%>&Processo=VOLONTARI&objForm=Form1&txtnumerofascicoloinvisione=MainContent_txtNumeroFascicoloinVisione&txtcodicefascicolonascosto=MainContent_TxtCodFascicolo&txtdescrizionefascicolo=MainContent_txtdescrizionefascicolo&TxtFasc=MainContent_TxtNumFascicolo&TxtCodFasc=MainContent_TxtCodFascicolo&TxtDescFascicolo=MainContent_TxtDescFAsc&TxtProt="+CostruisciId(NumProt)+"&VarVolontario=<%=Request.QueryString("IdVol")%>&TxtData="+CostruisciId(DataProt)+"&VArUpdate=1&TipoDocumento="+TipoDocumento+"&Fascicolo=<%=Request.QueryString("Fascicolo")%>&NumeroFascicolo=<%=Request.QueryString("NumeroFascicolo")%>&DescFascicolo=<%=Request.QueryString("DescFascicolo")%>&CodEnte=<%= Session("txtCodEnte")%>&TipoStampa="+TipoStampa, "Visualizza",winOption);
				}
				else
				{
					var msg;
					msg=confirm("Si vuole inserire il documento Principale?");
					if (msg==false)
					{
						myWin = window.open("WfrmSIGEDProtocollazione.aspx?CorrCodUnivoco=<%=Request.QueryString("CorrCodUnivoco")%>&Processo=VOLONTARI&objForm=Form1&txtnumerofascicoloinvisione=MainContent_txtNumeroFascicoloinVisione&txtcodicefascicolonascosto=MainContent_TxtCodFascicolo&txtdescrizionefascicolo=MainContent_txtdescrizionefascicolo&TxtFasc=MainContent_TxtNumFascicolo&TxtCodFasc=MainContent_TxtCodFascicolo&TxtDescFascicolo=MainContent_TxtDescFAsc&TxtProt="+ CostruisciId(NumProt) +"&VarVolontario=<%=Request.QueryString("IdVol")%>&TxtData="+CostruisciId(DataProt)+"&VArUpdate=1&TipoDocumento="+TipoDocumento+"&Fascicolo=<%=Request.QueryString("Fascicolo")%>&NumeroFascicolo=<%=Request.QueryString("NumeroFascicolo")%>&DescFascicolo=<%=Request.QueryString("DescFascicolo")%>&CodEnte=<%= Session("txtCodEnte")%>TipoStampa="+TipoStampa, "Visualizza",winOption);
						return false;
					}else
					{	
						myWin = window.open("WfrmSIGEDDocumentoPrincipale.aspx?NumProt=" + document.getElementById(CostruisciId(NumProt)).value + "&DataProt=" + document.getElementById(DataProt).value, "Visualizza", winOption);			
					}
				}	
			}	
		</script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina" >
<legend>Elenco Documentazione Volontario</legend>
<div class="wrapper"  style="width:100%;border:0px">
    <div class= "row" style="height:auto" >
        <asp:label id="lblmessaggiosopra"  runat="server" CssClass="msgErrore" ></asp:label>
        <input type="hidden" id="TxtCodFascicolo" runat="server"/>
        <input type="hidden" id="TxtNumFascicolo" runat="server"/>
        <input type="hidden" id="TxtDescFAsc" runat="server"/>
    </div>

    <div class= "row" >
        <div class="collable" style="width:25%" >
            <asp:Label ID="lblCodiceFascicoloP" CssClass="label" AssociatedControlID="txtNumeroFascicoloinVisione" runat="server" Text="Numero Fascicolo" ></asp:Label>
        </div>
                <div class="colOggetti" style="width:75%" >
            <asp:TextBox ID="txtNumeroFascicoloinVisione" CssClass="textbox" ReadOnly="true" runat="server"  ></asp:TextBox>
        </div>
        
    </div>
    <div class= "row" >
    <div class="collable" style="width:25%" >
        <asp:Label id="LblDescFascP" CssClass="label" AssociatedControlID="txtdescrizionefascicolo"    runat="server" Text="Descrizione"></asp:Label>
    </div>
   
    <div class="colOggetti" style="width:75%" >
            <asp:TextBox ID="txtdescrizionefascicolo"  CssClass="textbox"  ReadOnly="true" runat="server"
            TextMode="MultiLine"></asp:TextBox>

    </div>
    </div>
    <%--inizio chkLetteraAssegnazioneVolontario--%>
    <div class="row" >
       <div class="collable" style="width:100%">
            <asp:checkbox id="chkLetteraAssegnazioneVolontario" runat="server" Text="Lettera Assegnazione Volontario"></asp:checkbox>
        </div>
    </div>
        <div class="row" style="height:auto">
     <div class="colHyperLink" style="width:100%">
            <asp:hyperlink id="hplLetteraAssegnazioneVolontario" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File"></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtocollo" AssociatedControlID="TxtNumProtocollo" Visible="false" CssClass="label"  Text="N° Protocollo"  runat="server"  ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="TxtNumProtocollo" Visible="false" CssClass="textbox" runat="server"   ></asp:textbox>
        </div>
         <div class="colOggetti" style="width:15%" >
                <asp:imageButton id="cmdScSelProtocolloLV" style="CURSOR: hand" OnClientClick="javascript:SelProtocollo('TxtNumProtocollo','TxtDataProtocollo','Lettera Assegnazione Volontario - Nazionale','AssVol')"
										runat="server" Visible="False" ImageUrl="images/busta_small.png" 
                    ToolTip="Seleziona Protocollo"  AlternateText="Seleziona Protocollo" 
                    Height="19px"></asp:imageButton>
                 <asp:imageButton id="cmdScAllegatiLV" style="CURSOR: hand" OnClientClick="javascript:ApriAllegatiAssVol(TxtNumProtocollo.value,TxtDataProtocollo.value,'AssVol')"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="cmdNuovoFasciocloLV" style="CURSOR: hand" OnClientClick="javascript:ProtocollazioneAssVol('TxtNumProtocollo','TxtDataProtocollo','Lettera Assegnazione Volontario - Nazionale','AssVol')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtocollo" Visible="false" CssClass="label"  AssociatedControlID="TxtDataProtocollo"  runat="server"  Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="TxtDataProtocollo" CssClass="textbox"  runat="server"  ReadOnly="true"  Visible="false" ></asp:textbox>
	    </div>
     </div>
     <div class="row" style="height:auto">
     <div class="colHyperLink" style="width:100%">
            <asp:hyperlink id="hplLetteraAssegnazioneVolontarioB" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File"></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtocolloB" Visible="false" AssociatedControlID="TxtNumProtocolloB" CssClass="label"  Text="N° Protocollo"  runat="server"  ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="TxtNumProtocolloB" Visible="false" CssClass="textbox" runat="server"   ></asp:textbox>
        </div>
         <div class="colOggetti" style="width:15%" >
                <asp:imageButton id="cmdScSelProtocolloLVB" Visible="false" style="CURSOR: hand" OnClientClick="javascript:SelProtocolloAssVol('TxtNumProtocolloB','TxtDataProtocolloB','Lettera Assegnazione Volontario - Estero','AssVolB')"
										runat="server"  ImageUrl="images/busta_small.png" 
                    ToolTip="Seleziona Protocollo"  AlternateText="Seleziona Protocollo" 
                    Height="19px"></asp:imageButton>
                 <asp:imageButton id="cmdScAllegatiLVB" style="CURSOR: hand" OnClientClick="javascript:ApriAllegatiAssVol(TxtNumProtocolloB.value,TxtDataProtocolloB.value,'AssVolB')"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="cmdNuovoFasciocloLVB" style="CURSOR: hand" OnClientClick="javascript:ProtocollazioneAssVol('TxtNumProtocolloB','TxtDataProtocolloB','Lettera Assegnazione Volontario - Estero','AssVolB')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtocolloB" CssClass="label"  Visible="false" AssociatedControlID="TxtDataProtocolloB"  runat="server"  Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="TxtDataProtocolloB"  CssClass="textbox" runat="server"  ReadOnly="true"  Visible="false" ></asp:textbox>
	    </div>
     </div>
 <%-- fine chkLetteraAssegnazioneVolontario--%>
   
    <div class="row" >
    <div class="collable" style="width:100%">
        <asp:checkbox id="chkLetteraEsclusione" runat="server" Text="Lettera Esclusione Doppia Domanda"></asp:checkbox>
    </div>
</div>
    <div class="row" style="height:auto" >
     <div class="colHyperLink" style="width:100%">
            <asp:hyperlink id="hplLetteraEsclusione" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File"></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtocolloLEDD" Visible="false" AssociatedControlID="TxtNumProtocolloLEDD" CssClass="label"  Text="N° Protocollo"  runat="server"  ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="TxtNumProtocolloLEDD" Visible="false" CssClass="textbox" runat="server"   ></asp:textbox>
        </div>
         <div class="colOggetti" style="width:15%" >
                <asp:imageButton id="cmdScSelProtocolloLEDD" style="CURSOR: hand" OnClientClick="javascript:SelProtocollo('TxtNumProtocolloLEDD','TxtDataProtocolloLEDD','Lettera Esclusione Volontario')"
										runat="server" Visible="False" ImageUrl="images/busta_small.png" 
                    ToolTip="Seleziona Protocollo"  AlternateText="Seleziona Protocollo" 
                    Height="19px"></asp:imageButton>
                 <asp:imageButton id="cmdScAllegatiLEDD" style="CURSOR: hand" OnClientClick="javascript:ApriAllegati(TxtNumProtocolloLEDD.value,TxtDataProtocolloLEDD.value)"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="cmdNuovoFasciocloLEDD" style="CURSOR: hand" OnClientClick="javascript:Protocollazione('TxtNumProtocolloLEDD','TxtDataProtocolloLEDD','Lettera Esclusione Volontario')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtocolloLEDD" CssClass="label" Visible="false"  AssociatedControlID="TxtDataProtocolloLEDD"  runat="server"  Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="TxtDataProtocolloLEDD" CssClass="textbox" runat="server"  ReadOnly="true"  Visible="false" ></asp:textbox>
	    </div>
     </div>

    <div class="row">
        <div class="collable" style="width:100%">
            <asp:checkbox id="chkLetteraChiusuraInServizio" runat="server" Text="Lettera Chiusura In Servizio"></asp:checkbox>
        </div>
    </div>
    <div class="row" style="height:auto">
     <div class="colHyperLink" style="width:100%">
            <asp:hyperlink id="hplLetteraChiusuraInServizio" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File"></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtocolloLCIS" AssociatedControlID="TxtNumProtocolloLCIS" Visible="False" CssClass="label"  Text="N° Protocollo"  runat="server"  ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="TxtNumProtocolloLCIS" Visible="False" CssClass="textbox" runat="server"   ></asp:textbox>
        </div>
         <div class="colOggetti" style="width:15%" >
                <asp:imageButton id="cmdScSelProtocolloLCIS" style="CURSOR: hand" OnClientClick="javascript:SelProtocollo('TxtNumProtocolloLCIS','TxtDataProtocolloLCIS','Chiusura In Servizio')"
										runat="server" Visible="False" ImageUrl="images/busta_small.png" 
                    ToolTip="Seleziona Protocollo"  AlternateText="Seleziona Protocollo" 
                    Height="19px"></asp:imageButton>
                 <asp:imageButton id="cmdScAllegatiLCIS" style="CURSOR: hand" OnClientClick="javascript:ApriAllegati(TxtNumProtocolloLCIS.value,TxtDataProtocolloLCIS.value)"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="cmdNuovoFasciocloLCIS" style="CURSOR: hand" OnClientClick="javascript:Protocollazione('TxtNumProtocolloLCIS','TxtDataProtocolloLCIS','Chiusura In Servizio')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtocolloLCIS" CssClass="label" Visible="False"  AssociatedControlID="TxtDataProtocolloLCIS"  runat="server"  Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="TxtDataProtocolloLCIS" CssClass="textbox" runat="server"  ReadOnly="true"  Visible="false" ></asp:textbox>
	    </div>
     </div>

    <div class="row" >
        <div class="collable" style="width:100%">
            <asp:checkbox id="chkLetteraEsclusionePerAssenzaIngiustificata" runat="server" Text="Lettera Esclusione Per Assenza Ingiustificata"></asp:checkbox>
        </div>
    </div>
    <div class="row" style="height:auto">
     <div class="colHyperLink" style="width:100%">
            <asp:hyperlink id="hplLetteraEsclusionePerAssenzaIngiustificata" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File"></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtocolloLEPAI" Visible="False" AssociatedControlID="TxtNumProtocolloLEPAI" CssClass="label"  Text="N° Protocollo"  runat="server"  ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="TxtNumProtocolloLEPAI" Visible="False" CssClass="textbox" runat="server"   ></asp:textbox>
        </div>
         <div class="colOggetti" style="width:15%" >
                <asp:imageButton id="cmdScSelProtocolloLEPAI" style="CURSOR: hand" OnClientClick="javascript:SelProtocollo('TxtNumProtocolloLEPAI','TxtDataProtocolloLEPAI','Lettera Esclusione Per Assenza Ingiustificata')"
										runat="server" Visible="False" ImageUrl="images/busta_small.png" 
                    ToolTip="Seleziona Protocollo"  AlternateText="Seleziona Protocollo" 
                    Height="19px"></asp:imageButton>
                 <asp:imageButton id="cmdScAllegatiLEPAI" style="CURSOR: hand" OnClientClick="javascript:ApriAllegati(TxtNumProtocolloLEPAI.value,TxtDataProtocolloLEPAI.value)"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="cmdNuovoFasciocloLEPAI" style="CURSOR: hand" OnClientClick="javascript:Protocollazione('TxtNumProtocolloLEPAI','TxtDataProtocolloLEPAI','Lettera Esclusione Per Assenza Ingiustificata')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtocolloLEPAI" CssClass="label" Visible="False"  AssociatedControlID="TxtDataProtocolloLEPAI"  runat="server"  Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="TxtDataProtocolloLEPAI" CssClass="textbox" runat="server"  ReadOnly="true"  Visible="false" ></asp:textbox>
	    </div>
     </div>

    <div class="row" >
        <div class="collable" style="width:100%">
            <asp:checkbox id="chkLetteraEsclusionePerGiorniPermesso" runat="server" Text="Lettera Esclusione Per Giorni Permesso"></asp:checkbox>
        </div>
    </div>
    <div class="row" style="height:auto">
     <div class="colHyperLink" style="width:100%">
            <asp:hyperlink id="hplLetteraEsclusionePerGiorniPermesso" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File"></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtocolloLEPGP" AssociatedControlID="TxtNumProtocolloLEPGP" Visible="False" CssClass="label"  Text="N° Protocollo"  runat="server"  ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="TxtNumProtocolloLEPGP" CssClass="textbox" Visible="False" runat="server"></asp:textbox>
        </div>
         <div class="colOggetti" style="width:15%" >
                <asp:imageButton id="cmdScSelProtocolloLEPGP" style="CURSOR: hand" OnClientClick="javascript:SelProtocollo('TxtNumProtocolloLEPGPA','TxtDataProtocolloLEPGP','Lettera Esclusione Per Giorni Permesso')"
										runat="server" Visible="False" ImageUrl="images/busta_small.png" 
                    ToolTip="Seleziona Protocollo"  AlternateText="Seleziona Protocollo" 
                    Height="19px"></asp:imageButton>
                 <asp:imageButton id="cmdScAllegatiLEPGP" style="CURSOR: hand" OnClientClick="javascript:ApriAllegati(TxtNumProtocolloLEPGP.value,TxtDataProtocolloLEPGP.value)"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="cmdNuovoFasciocloLEPGP" style="CURSOR: hand" OnClientClick="javascript:Protocollazione('TxtNumProtocolloLEPGP','TxtDataProtocolloLEPGP','Lettera Esclusione Per Giorni Permesso')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione" Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtocolloLEPGP" CssClass="label"  AssociatedControlID="TxtDataProtocolloLEPGP" Visible="False" runat="server"  Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="TxtDataProtocolloLEPGP" CssClass="textbox" runat="server"  ReadOnly="true"  Visible="false" ></asp:textbox>
	    </div>
     </div>

     <div class="row" >
        <div class="collable" style="width:100%">
            <asp:checkbox id="chkLetteraEsclusionePerSuperamentoMalattia" runat="server" Text="Lettera Esclusione Per Superamento Malattia"></asp:checkbox>
        </div>
    </div>
    <div class="row" style="height:auto">
     <div class="colHyperLink" style="width:100%">
            <asp:hyperlink id="hplLetteraEsclusionePerSuperamentoMalattia" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File"></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtocolloLEPSM" AssociatedControlID="TxtNumProtocolloLEPSM" Visible="False" CssClass="label"  Text="N° Protocollo"  runat="server"  ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="TxtNumProtocolloLEPSM" CssClass="textbox" Visible="False" runat="server"   ></asp:textbox>
        </div>
         <div class="colOggetti" style="width:15%" >
                <asp:imageButton id="cmdScSelProtocolloLEPSM" style="CURSOR: hand" OnClientClick="javascript:SelProtocollo('TxtNumProtocolloLEPSM','TxtDataProtocolloLEPSM','Lettera Esclusione Per Superamento Malattia')"
										runat="server" Visible="False" ImageUrl="images/busta_small.png" 
                    ToolTip="Seleziona Protocollo"  AlternateText="Seleziona Protocollo" 
                    Height="19px"></asp:imageButton>
                 <asp:imageButton id="cmdScAllegatiLEPSM" style="CURSOR: hand" OnClientClick="javascript:ApriAllegati(TxtNumProtocolloLEPSM.value,TxtDataProtocolloLEPSM.value)"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="cmdNuovoFasciocloLEPSM" style="CURSOR: hand" OnClientClick="javascript:Protocollazione('TxtNumProtocolloLEPSM','TxtDataProtocolloLEPSM','Lettera Esclusione Per Superamento Malattia')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione" Height="20px" Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtocolloLEPSM" CssClass="label"  Visible="False" AssociatedControlID="TxtDataProtocolloLEPSM"  runat="server"  Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="TxtDataProtocolloLEPSM" CssClass="textbox" runat="server"  ReadOnly="true"  Visible="false" ></asp:textbox>
	    </div>
     </div>

    <div class="row" style="height:auto">
       <div class="collable" style="width:100%">
            <asp:checkbox id="chkLetteraSubentro" runat="server" Text="Lettera Subentro"></asp:checkbox>
        </div>
    </div>
     <div class="row" >
     <div class="colHyperLink" style="width:100%">
            <asp:hyperlink id="hplLetteraSubentro" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File"></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtocolloLS" AssociatedControlID="TxtNumProtocolloLS" Visible="False" CssClass="label"  Text="N° Protocollo"  runat="server"  ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="TxtNumProtocolloLS" CssClass="textbox" Visible="False" runat="server"   ></asp:textbox>
        </div>
         <div class="colOggetti" style="width:15%" >
                <asp:imageButton id="cmdScSelProtocolloLS" style="CURSOR: hand" OnClientClick="javascript:SelProtocolloAssVol('TxtNumProtocolloLS','TxtDataProtocolloLS','Lettera Subentro','LettSub')"
										runat="server" Visible="False" ImageUrl="images/busta_small.png" 
                    ToolTip="Seleziona Protocollo"  AlternateText="Seleziona Protocollo" 
                    Height="19px"></asp:imageButton>
                 <asp:imageButton id="cmdScAllegatiLS" style="CURSOR: hand" OnClientClick="javascript:ApriAllegatiAssVol(TxtNumProtocolloLS.value,TxtDataProtocolloLS.value,'LettSub')"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="cmdNuovoFasciocloLS" style="CURSOR: hand" OnClientClick="javascript:ProtocollazioneAssVol('TxtNumProtocolloLS','TxtDataProtocolloLS','Lettera Subentro','LettSub')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione" Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtocolloLS" CssClass="label" Visible="False" AssociatedControlID="TxtDataProtocolloLS"  runat="server"  Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="TxtDataProtocolloLS" CssClass="textbox" runat="server"  ReadOnly="true"  Visible="false" ></asp:textbox>
	    </div>
     </div>


     <div class="row"   >
     <div class="colHyperLink" style="width:100%">
            <asp:hyperlink id="hplLetteraSubentroCopia" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File"></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtocolloLSB" AssociatedControlID="TxtNumProtocolloLSB" Visible="False" CssClass="label"  Text="N° Protocollo"  runat="server"  ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="TxtNumProtocolloLSB" CssClass="textbox" runat="server" Visible="False"></asp:textbox>
        </div>
         <div class="colOggetti" style="width:15%" >
                <asp:imageButton id="cmdScSelProtocolloLSB" style="CURSOR: hand" OnClientClick="javascript:SelProtocolloAssVol('TxtNumProtocolloLSB','TxtDataProtocolloLSB','Lettera Subentro','LettSubB')"
										runat="server" Visible="False"  ImageUrl="images/busta_small.png" 
                    ToolTip="Seleziona Protocollo"  AlternateText="Seleziona Protocollo" 
                    Height="19px"></asp:imageButton>
                 <asp:imageButton id="cmdScAllegatiLSB" style="CURSOR: hand" OnClientClick="javascript:ApriAllegatiAssVol(TxtNumProtocolloLSB.value,TxtDataProtocolloLSB.value,'LettSubB')"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="cmdNuovoFasciocloLSB" style="CURSOR: hand" OnClientClick="javascript:ProtocollazioneAssVol('TxtNumProtocolloLSB','TxtDataProtocolloLSB','Lettera Subentro','LettSubB')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtocolloLSB" CssClass="label" Visible="False" AssociatedControlID="TxtDataProtocolloLSB"  runat="server"  Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="TxtDataProtocolloLSB" CssClass="textbox" runat="server"  ReadOnly="true"  Visible="false" ></asp:textbox>
	    </div>
     </div>
    <div class="row" style="height:auto">
       <div class="collable" style="width:100%">
            <asp:checkbox id="chkAttoAggiuntivo" runat="server" Text="Atto Aggiuntivo"></asp:checkbox>
        </div>
    </div>
    <div class="row" >
     <div class="colHyperLink" style="width:100%">
            <asp:hyperlink id="hplAttoAggiuntivo" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File"></asp:hyperlink>
        </div>
    </div>

        <%--Certificato servizio svolto dal volontario (MEV 219)--%>
        <div runat="server" id="divCertificatoServizioSvolto" class="row" style="height: auto">
            <div class="collable" style="width: 100%">
                <asp:CheckBox runat="server" ID="chkCertificatoServizioSvolto" Text="Certificato servizio svolto" />
            </div>
        </div>
        <div class="row" >
         <div class="colHyperLink" style="width:100%">
                <asp:hyperlink id="hplCertificatoServizioSvolto" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File"></asp:hyperlink>
            </div>
        </div>
     <div class="rowGroup" id="DivElementiObsoleti" style="visibility:hidden">
         <div class="row" style="height:auto;visibility:hidden">
    <div class="collable" style="width:100%">
        <asp:checkbox id="chkLetteraAssegnazioneRitornoMittente" runat="server" Text="Lettera Assegnazione Ritorno al Mittente"></asp:checkbox>
    </div>
</div>
    <div class="row" style="height:auto" >
     <div class="colHyperLink" style="width:100%">
            <asp:hyperlink id="hplLetteraAssegnazioneRitornoMittente" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File"></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtocolloLARM" AssociatedControlID="TxtNumProtocolloLARM" Visible="false"  CssClass="label"  Text="N° Protocollo"  runat="server"  ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="TxtNumProtocolloLARM" CssClass="textbox" runat="server"  Visible="false"  ></asp:textbox>
        </div>
         <div class="colOggetti" style="width:15%" >
                <asp:imageButton id="cmdScSelProtocolloLARM" style="CURSOR: hand" OnClientClick="javascript:SelProtocollo('TxtNumProtocolloLARM','TxtDataProtocolloLARM','Lettera Assegnazione Ritorno al mittente')"
										runat="server" Visible="False" ImageUrl="images/busta_small.png" 
                    ToolTip="Seleziona Protocollo"  AlternateText="Seleziona Protocollo" 
                    Height="19px"></asp:imageButton>
                 <asp:imageButton id="cmdScAllegatiLARM" style="CURSOR: hand" OnClientClick="javascript:ApriAllegati(TxtNumProtocolloLARM.value,TxtDataProtocolloLARM.value)"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="cmdNuovoFasciocloLARM" style="CURSOR: hand" OnClientClick="javascript:Protocollazione('TxtNumProtocolloLARM','TxtDataProtocolloLARM','Lettera Assegnazione Ritorno al mittente')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtocolloLARM" CssClass="label" Visible="false"  AssociatedControlID="TxtDataProtocolloLARM"  runat="server"  Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="TxtDataProtocolloLARM" CssClass="textbox" runat="server"  ReadOnly="true"  Visible="false" ></asp:textbox>
	    </div>
     </div>

      <div class="row" style="visibility:hidden;height:auto">
    <div class="collable" style="width:100%">
        <asp:checkbox id="chkDuplicatoletteraAssegnazioneEnte" runat="server" Text="Duplicato Lettera Assegnazione Ente"></asp:checkbox>
    </div>
</div>
    <div class="row" style="visibility:hidden;height:auto" >
     <div class="colHyperLink" style="width:100%">
            <asp:hyperlink id="hplDuplicatoLetteraAssegnazioneEnte" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File"></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtocolloDLA" AssociatedControlID="TxtNumProtocolloDLA" Visible="false" CssClass="label"  Text="N° Protocollo"  runat="server"  ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="TxtNumProtocolloDLA" Visible="false" CssClass="textbox" runat="server"   ></asp:textbox>
        </div>
         <div class="colOggetti" style="width:15%" >
                <asp:imageButton id="cmdScSelProtocolloDLA" style="CURSOR: hand" OnClientClick="javascript:SelProtocollo('TxtNumProtocolloDLA','TxtDataProtocolloDLA','Lettera Assegnazione Volontario Duplicato')"
										runat="server" Visible="False" ImageUrl="images/busta_small.png" 
                    ToolTip="Seleziona Protocollo"  AlternateText="Seleziona Protocollo" 
                    Height="19px"></asp:imageButton>
                 <asp:imageButton id="cmdScAllegatiDLA" style="CURSOR: hand" OnClientClick="javascript:ApriAllegati(TxtNumProtocolloDLA.value,TxtDataProtocolloDLA.value)"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="cmdNuovoFasciocloDLA" style="CURSOR: hand" OnClientClick="javascript:Protocollazione('TxtNumProtocolloDLA','TxtDataProtocolloDLA','Lettera Assegnazione Volontario Duplicato')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtocolloDLA" CssClass="label" Visible="false"  AssociatedControlID="TxtDataProtocollo"  runat="server"  Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="TxtDataProtocolloDLA" CssClass="textbox" runat="server"  ReadOnly="true"  Visible="false" ></asp:textbox>
	    </div>
     </div>

          <div class="row" style="visibility:hidden;height:auto">
    <div class="collable" style="width:100%">
        <asp:checkbox id="chkLetteraChiusura" runat="server" Text="Lettera Chiusura Iniziale"></asp:checkbox>
    </div>
</div>
    <div class="row" style="height:auto" >
     <div class="colHyperLink" style="width:100%">
            <asp:hyperlink id="hplLetteraChiusura" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File"></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtocolloLCI" AssociatedControlID="TxtNumProtocolloLCI"  Visible="false" CssClass="label"  Text="N° Protocollo"  runat="server"  ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="TxtNumProtocolloLCI" CssClass="textbox" Visible="false" runat="server"   ></asp:textbox>
        </div>
         <div class="colOggetti" style="width:15%" >
                <asp:imageButton id="cmdScSelProtocolloLCI" style="CURSOR: hand" OnClientClick="javascript:SelProtocollo('TxtNumProtocolloLCI','TxtDataProtocolloLCI','Chiusura Iniziale')"
										runat="server" Visible="False" ImageUrl="images/busta_small.png" 
                    ToolTip="Seleziona Protocollo"  AlternateText="Seleziona Protocollo" 
                    Height="19px"></asp:imageButton>
                 <asp:imageButton id="cmdScAllegatiLCI" style="CURSOR: hand" OnClientClick="javascript:ApriAllegati(TxtNumProtocolloLCI.value,TxtDataProtocolloLCI.value)"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="cmdNuovoFasciocloLCI" style="CURSOR: hand" OnClientClick="javascript:Protocollazione('TxtNumProtocolloLCI','TxtDataProtocolloLCI','Chiusura Iniziale')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtocolloLCI" CssClass="label" Visible="false"  AssociatedControlID="TxtDataProtocolloLCI"  runat="server"  Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="TxtDataProtocolloLCI" CssClass="textbox" runat="server"  ReadOnly="true"  Visible="false" ></asp:textbox>
	    </div>
     </div>

     </div>

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
