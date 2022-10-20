<%@ Page Title="Elenco Documentazione" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="elencodocumentazionegrad.aspx.vb" Inherits="Futuro.elencodocumentazionegrad" %>
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

            var IdData1 = CostruisciId('txtdatadal1');
            var sharpIdData1 = "#" + IdData1
            $("" + sharpIdData1 + "").datepicker();
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
<legend>Elenco Documentazione Volontario</legend>

    <div class= "rowGroup" style="height:auto" >
        <div class="row" style="height:auto">
        <asp:label id="txtMSG" runat="server"  CssClass="msgInfo" ></asp:label>
        <asp:label id="lblmessaggiosopra"  runat="server" CssClass="msgErrore" ></asp:label>
        <input type="hidden" id="hddModificaProtocollo" runat="server" />
        <input type="hidden" id="HdValoreSalva"  runat="server"/> 
        <input type="hidden" id="hddCodVolSubentro" runat="server"/>
        <input type="hidden"  id="TxtCodiceFasc" runat="server"/>
        <input type="hidden"  id="TxtNumFascicoloControllo" runat="server"/>
        </div>
         <div class="RigaVuota" >&nbsp;
         </div>
    </div>

    <fieldset class="ContornoPagina" >
        <legend>Per Programma </legend>
         <div class="wrapper"  style="width:100%;border:0px">
        <div class= "rowGroup" style="height:auto" >

     <div class="row" >
        <div class="collable" style="width:20%">
            <asp:Label ID="Label1" AssociatedControlID="txtProgramma" runat="server" Text="Codice Programma"></asp:Label>
       </div>
        <div class="colOggetti" style="width:80%">    
             <asp:TextBox ID="txtProgramma"   runat="server"></asp:TextBox>  
       </div>
    </div>
     <div class="row" >
        <div class="collable" style="width:20%">
            <asp:Label ID="Label2" AssociatedControlID="txtdatadal1"  runat="server" Text="Data (gg/mm/aaaa)"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
             <asp:TextBox ID="txtdatadal1" CssClass="textboxData" MaxLength="10" runat="server"></asp:TextBox>  
       </div>
    </div>


    </div>
    <div class="row">
        <div class="collable" style="width:100%">
            <asp:checkbox id="chkLetteraApprovazioneGraduatoriaProgramma" runat="server" Text="Lettera Approvazione Graduatoria da Programma"></asp:checkbox>
        </div>
     </div>
     <div class="row" >
     <div class="colHyperLink" style="width:20%">
            <asp:hyperlink id="hplLetteraApprovazioneGraduatoriaProgramma" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File"></asp:hyperlink>
        </div>
        </div>
    <div class="RigaPulsanti">
  <asp:Button
    ID="imgGenerafileProgamma" runat="server" CssClass="Pulsante" Text="Genera" />
</div>
    </div>
        </fieldset>

    <fieldset class="ContornoPagina">
        <legend>Per Circolare/progetto </legend>
        <div class="wrapper"  style="width:100%;border:0px">
  <div class= "rowGroup" style="height:auto" >

     <div class="row" >
        <div class="collable" style="width:20%">
            <asp:Label ID="LblBando" AssociatedControlID="ddlBando" runat="server" Text="Circolare"></asp:Label>
       </div>
        <div class="colOggetti" style="width:80%">    
              <asp:DropDownList ID="ddlBando" CssClass="ddlClass" runat="server" AutoPostBack="true"  ></asp:DropDownList> 
       </div>
    </div>
     <div class="row" >
        <div class="collable" style="width:20%">
            <asp:Label ID="LblData" AssociatedControlID="txtdatadal"  runat="server" Text="Data (gg/mm/aaaa)"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
             <asp:TextBox ID="txtdatadal" CssClass="textboxData" MaxLength="10" runat="server"></asp:TextBox>  
       </div>
    </div>

     <div id="SezioneFascicolo" runat="server" visible="false">
      <h2>
     <asp:label  id="lblTitolo" Font-Bold="true"  runat="server" Text="Dati fascicolo" />
    </h2>
     <div class= "RigaVuota" >&nbsp;
     </div>
    <div class= "row" >
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
											AlternateText="Seleziona Codice Fascicolo"  
                ToolTip="Seleziona Codice Fascicolo"  ></asp:ImageButton>

            <asp:ImageButton style="CURSOR: hand;" id="cmdSelProtocollo" OnClientClick="ElencoDocumenti('TxtNumeroFascicolo');" runat="server" ImageUrl="images/busta_small.png"
											ToolTip="Elenco Documenti" AlternateText="Elenco Documenti" ></asp:ImageButton>
                <asp:ImageButton style="CURSOR: hand;" id="cmdFascCanc"  runat="server" ImageUrl="images/canc_small.png"
											ToolTip="Rimuovi Fascicolo/Protocolli" AlternateText="Rimuovi Fascicolo/Protocolli" ></asp:ImageButton>


    </div>
    </div>

    <div class= "row" >
    <div class="collable" style="width:20%" >
        <asp:Label id="LblDescFasc" CssClass="label" AssociatedControlID="txtDescFasc" runat="server" Text="Descrizione"></asp:Label>
    </div>
   
    <div class="colOggetti" style="width:80%" >
            <asp:TextBox ID="txtDescFasc"  CssClass="textbox"  ReadOnly="true" runat="server"
            TextMode="MultiLine"></asp:TextBox>

    </div>
        </div>

    </div>
      <div class="RigaVuota" >&nbsp;

      </div>
    <div class="RigaPulsanti" >
       <%--<asp:Button ID="Salva" CssClass="Pulsante"  runat="server" Text="Salva" ToolTip="Salva" />--%>
    </div>
    </div>
    

     <div class="row">
        <div class="collable" style="width:100%">
            <asp:checkbox id="chkLetteraApprovazioneGraduatoria" runat="server" Text="Lettera Approvazione Graduatoria Nazionale"></asp:checkbox>
        </div>
     </div>
   <div id="DivLetteraApprovazioneGraduatoria" class="rowGroup" runat="server" visible="false" >
     <div class="row" >
     <div class="colHyperLink" style="width:20%">
            <asp:hyperlink id="hplLetteraApprovazioneGraduatoria" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File"></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtocollo1" AssociatedControlID="TxtNumProtocollo1" CssClass="label"  Text="N° Protocollo"  runat="server" Visible="false" ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="TxtNumProtocollo1" CssClass="textbox" runat="server"  Visible="false" ></asp:textbox>
        </div>
         <div class="colOggetti" style="width:15%" >
                <asp:imageButton id="cmdSelProtocollo1" style="CURSOR: hand" OnClientClick="javascript:SelProtocollo('TxtNumProtocollo1','TxtDataProtocollo1')"
										runat="server" Visible="False" ImageUrl="images/busta_small.png" 
                    ToolTip="Seleziona Protocollo"  AlternateText="Seleziona Protocollo" 
                    Height="19px"></asp:imageButton>
                 <asp:imageButton id="cmdScAllegati1" style="CURSOR: hand" OnClientClick="javascript:ApriAllegati('TxtNumProtocollo1','TxtDataProtocollo1')"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="cmdNuovoFascicolo1" style="CURSOR: hand" OnClientClick="javascript:Protocollazione('TxtNumProtocollo1','TxtDataProtocollo1')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtocollo1" CssClass="label"  AssociatedControlID="TxtDataProtocollo1"  runat="server" Visible="false" Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="TxtDataProtocollo1" CssClass="textbox" runat="server"  MaxLength="10"  Visible="false" ></asp:textbox>
	    </div>
     </div>
     <div class="row" style="height:auto">
     <div class="colHyperLink" style="width:20%">
            <asp:hyperlink id="hplLetteraApprovazioneGraduatoriaCopiaReg" runat="server" Visible="False" CssClass="colHyperLink" Text="Download File"></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtocollo2" AssociatedControlID="TxtNumProtocollo2" CssClass="label"  Text="N° Protocollo"  runat="server" Visible="false" ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="TxtNumProtocollo2" CssClass="textbox" runat="server"  Visible="false" ></asp:textbox>
        </div>
         <div class="colOggetti" style="width:15%" >
                <asp:imageButton id="cmdSelProtocollo2" style="CURSOR: hand" OnClientClick="javascript:SelProtocollo('TxtNumProtocollo2','TxtDataProtocollo2')"
										runat="server" Visible="False" ImageUrl="images/busta_small.png" 
                    ToolTip="Seleziona Protocollo"  AlternateText="Seleziona Protocollo" 
                    Height="19px"></asp:imageButton>
                 <asp:imageButton id="cmdScAllegati2" style="CURSOR: hand" OnClientClick="javascript:ApriAllegati('TxtNumProtocollo2','TxtDataProtocollo2')"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="cmdNuovoFascicolo2" style="CURSOR: hand" OnClientClick="javascript:Protocollazione('TxtNumProtocollo2','TxtDataProtocollo2')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtocollo2" CssClass="label"  AssociatedControlID="TxtDataProtocollo2"  runat="server" Visible="false" Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="TxtDataProtocollo2" CssClass="textbox" runat="server"  MaxLength="10"  Visible="false" ></asp:textbox>
	    </div>
     </div>
     </div>
     <div class="row">
        <div class="collable" style="width:100%">
            <asp:checkbox id="chkLetteraApprovazioneGraduatoriaEstero" runat="server" Text="Lettera Approvazione Graduatoria Estero"></asp:checkbox>
        </div>
     </div>
   <div id="DivLetteraApprovazioneGraduatoriaEstero" class="rowGroup" runat="server" visible="false" >
     <div class="row" >
     <div class="colHyperLink" style="width:20%">
            <asp:hyperlink id="hplLetteraApprovazioneGraduatoriaEstero" runat="server" Visible="False" CssClass="colHyperLink" ToolTip="Download File" Text="Download File" ></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtocollo3" AssociatedControlID="TxtNumProtocollo3" CssClass="label"  Text="N° Protocollo"  runat="server" Visible="false" ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="TxtNumProtocollo3" CssClass="textbox" runat="server"  Visible="false" ></asp:textbox>
        </div>
         <div class="colOggetti" style="width:15%" >
                <asp:imageButton id="cmdSelProtocollo3" style="CURSOR: hand" OnClientClick="javascript:SelProtocollo('TxtNumProtocollo3','TxtDataProtocollo3')"
										runat="server" Visible="False" ImageUrl="images/busta_small.png" ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo"></asp:imageButton>
                 <asp:imageButton id="cmdScAllegati3" style="CURSOR: hand" OnClientClick="javascript:ApriAllegati('TxtNumProtocollo3','TxtDataProtocollo3')"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="cmdNuovoFascicolo3" style="CURSOR: hand" OnClientClick="javascript:Protocollazione('TxtNumProtocollo3','TxtDataProtocollo3')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtocollo3" CssClass="label"  AssociatedControlID="TxtDataProtocollo3"  runat="server" Visible="false" Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="TxtDataProtocollo3" CssClass="textbox"  MaxLength="10" runat="server"    Visible="false" ></asp:textbox>
	    </div>

        </div>
        </div>


         <div class="row">
        <div class="collable" style="width:100%">
            <asp:checkbox id="chkLetteraApprovazioneGraduatoriaEsteroNazionale" runat="server" Text="Lettera Approvazione Graduatoria Estero/Nazionale"></asp:checkbox>
        </div>
     </div>
    <div id="DivLetteraApprovazioneGraduatoriaEsteroNazionale" class="rowGroup" runat="server" visible="false" >
     <div class="row" >
     <div class="colHyperLink" style="width:20%">
            <asp:hyperlink id="hplLetteraApprovazioneGraduatoriaEsteroNazionale" runat="server" Visible="False" CssClass="colHyperLink" ToolTip="Download File" Text="Download File" ></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtocollo4" AssociatedControlID="TxtNumProtocollo4" CssClass="label"  Text="N° Protocollo"  runat="server" Visible="false" ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="TxtNumProtocollo4" CssClass="textbox" runat="server"  Visible="false" ></asp:textbox>
        </div>
         <div class="colOggetti" style="width:15%" >
                <asp:imageButton id="cmdSelProtocollo4" style="CURSOR: hand" OnClientClick="javascript:SelProtocollo('TxtNumProtocollo4','TxtDataProtocollo4')"
										runat="server" Visible="False" ImageUrl="images/busta_small.png" ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo"></asp:imageButton>
                 <asp:imageButton id="cmdScAllegati4" style="CURSOR: hand" OnClientClick="javascript:ApriAllegati('TxtNumProtocollo4','TxtDataProtocollo4')"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="cmdNuovoFascicolo4" style="CURSOR: hand" OnClientClick="javascript:Protocollazione('TxtNumProtocollo4','TxtDataProtocollo4')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtocollo4" CssClass="label"  AssociatedControlID="TxtDataProtocollo4"  runat="server" Visible="false" Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="TxtDataProtocollo4" CssClass="textbox"  MaxLength="10" runat="server"    Visible="false" ></asp:textbox>
	    </div>

        </div>
        </div>

    <div class="row" style="visibility:hidden;height:auto">
        <div class="collable" style="width:100%">
            <asp:checkbox id="chkletteraAssegnazioneS" runat="server" Text="Lettera singola di Assegnazione Volontario dopo Approvazione Graduatoria"></asp:checkbox>
        </div>
    </div>
   
    <div id="DivletteraAssegnazioneS" class="rowGroup" runat="server" visible="false" >
     <div class="row" >
     <div class="colHyperLink" style="width:20%">
            <asp:hyperlink id="hplletteraAssegnazioneS" runat="server" Visible="False" CssClass="colHyperLink" ToolTip="Download File" Text="Download File" ></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtocollo5" AssociatedControlID="TxtNumProtocollo5" CssClass="label"  Text="N° Protocollo"  runat="server" Visible="false" ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="TxtNumProtocollo5" CssClass="textbox" runat="server"  Visible="false" ></asp:textbox>
        </div>
         <div class="colOggetti" style="width:15%" >
                <asp:imageButton id="cmdSelProtocollo5" style="CURSOR: hand" OnClientClick="javascript:SelProtocollo('TxtNumProtocollo5','TxtDataProtocollo5')"
										runat="server" Visible="False" ImageUrl="images/busta_small.png" ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo"></asp:imageButton>
                 <asp:imageButton id="cmdScAllegati5" style="CURSOR: hand" OnClientClick="javascript:ApriAllegati('TxtNumProtocollo5','TxtDataProtocollo5')"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="cmdNuovoFascicolo5" style="CURSOR: hand" OnClientClick="javascript:Protocollazione('TxtNumProtocollo5','TxtDataProtocollo5')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione"  Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtocollo5" CssClass="label"  AssociatedControlID="TxtDataProtocollo5"  runat="server" Visible="false" Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="TxtDataProtocollo5" CssClass="textbox"  MaxLength="10" runat="server"    Visible="false" ></asp:textbox>
	    </div>

        </div>
        </div>

    <div class="row" style="visibility:hidden;height:auto">
        <div class="collable" style="width:100%">
            <asp:checkbox id="chkletteraAssegnazioneM" runat="server" Text="Lettera multipla di Assegnazione Volontario dopo Approvazione Graduatoria"></asp:checkbox>
        </div>
    </div>
    <div id="DivletteraAssegnazioneM" class="rowGroup" runat="server" visible="false" >
     <div class="row" >
     <div class="colHyperLink" style="width:20%">
            <asp:hyperlink id="hplletteraAssegnazioneM" runat="server" Visible="False" CssClass="colHyperLink" ToolTip="Download File" Text="Download File" ></asp:hyperlink>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProtocollo6" AssociatedControlID="TxtNumProtocollo6" CssClass="label"  Text="N° Protocollo"  runat="server" Visible="false" ></asp:label>
        </div>
        <div class="colOggetti" style="width:15%" >
                <asp:textbox id="TxtNumProtocollo6" CssClass="textbox" runat="server"  Visible="false" ></asp:textbox>
        </div>
         <div class="colOggetti" style="width:15%" >
                <asp:imageButton id="cmdSelProtocollo6" style="CURSOR: hand" OnClientClick="javascript:SelProtocollo('TxtNumProtocollo6','TxtDataProtocollo6')"
										runat="server" Visible="False" ImageUrl="images/busta_small.png" ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo"></asp:imageButton>
                 <asp:imageButton id="cmdScAllegati6" style="CURSOR: hand" OnClientClick="javascript:ApriAllegati('TxtNumProtocollo6','TxtDataProtocollo6')"
										runat="server" Visible="False" ImageUrl="images/giu_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" Height="20px"></asp:imageButton>
                <asp:imageButton id="cmdNuovoFascicolo6" style="CURSOR: hand" OnClientClick="javascript:Protocollazione('TxtNumProtocollo6','TxtDataProtocollo6')"
										Visible="False" ImageUrl="images/documento_small.png" ToolTip="Richiesta Protocollazione" AlternateText="Richiesta Protocollazione" Runat="server"></asp:imageButton>
            </div>
        <div class="collable" style="width:20%" >
        <asp:label id="LblDataProtocollo6" CssClass="label"  AssociatedControlID="TxtDataProtocollo6"  runat="server" Visible="false" Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:15%" >
		    <asp:textbox id="TxtDataProtocollo6" CssClass="textbox"  MaxLength="10" runat="server"    Visible="false" ></asp:textbox>
	    </div>

        </div>
        </div>

    <div class="RigaPulsanti">
  <asp:Button
    ID="imgGeneraFile" runat="server" CssClass="Pulsante" Text="Genera" />
</div>
       
        <br />
         </div>
        </fieldset>
 
    <div class="RigaPulsanti" style="text-align:right">
    <asp:Button 
    ID="imgChiudi" CssClass="Pulsante" runat="server"
        Text="Chiudi"  />

     </div>

</fieldset>

</asp:Content>