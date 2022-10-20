<%@ Page Title="Documentazione Iscrizione" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="elencodocumentazioneaccr.aspx.vb" Inherits="Futuro.elencodocumentazioneaccr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
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
//        $(function () {
//            var IdData = CostruisciId('txtdatadal');
//            var sharpIdData = "#" + IdData
//            $("" + sharpIdData + "").datepicker();
//        });

	</script>
    <script  type="text/JavaScript">
	    function SelProtocollo(NumProt,DataProt,TipoDocumento,Sezione){
            var IdFascicolo =  '<%= TxtCodiceFasc.value %>';
			if (document.getElementById(CostruisciId("TxtNumeroFascicolo")).text ==''){
				alert ("Nessun Fascicolo Selezionato!");
			}else{
                var parametri="VengoDa=Stampa&objHddModifica=hddModificaProtocollo&objForm=Form1&TxtProt="+CostruisciId(NumProt)+"&TxtData="+CostruisciId(DataProt)+"&NumeroFascicolo="+IdFascicolo
				myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?"+parametri, "", winOption);
			}
		}

		function ApriAllegati(NumProt,DataProt)	{
            var IdFascicolo =  '<%= TxtCodiceFasc.Value %>';
			if (document.all.TxtCodiceFascicolo.Text =='')
			{
				alert ("Nessun Fascicolo Selezionato!");
			}else{	
				myWin = window.open("WfrmSIGEDElencoAllegati.aspx?NumeroProtocollo=" + document.getElementById(NumProt).value  + "&DataProtocollo=" + document.getElementById(DataProt).value + "&CodiceFascicolo="+IdFascicolo,"Visualizza",winOption);		
			}
		}

			function Protocollazione(NumProt,DataProt)
			{
                var IdFascicolo =  '<%= TxtCodiceFasc.value %>';
                var txtDescFasc = document.getElementById(CostruisciId("txtDescFasc")).text;
                var TxtNumeroFascicolo =  document.getElementById(CostruisciId("TxtNumeroFascicolo")).Text;
				if (document.getElementById(CostruisciId(NumProt)).value=='' || document.getElementById(CostruisciId(DataProt)).Text=='')
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
            var IdFascicolo =  '<%= TxtCodiceFasc.value %>';
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
            //alert("IdTxtCodiceFascicolo" + IdTxtCodiceFascicolo)
            var IdDescrizioneFascicolo = CostruisciId(txtDescFasc);
             //alert("IdDescrizioneFascicolo" + IdDescrizioneFascicolo)
            var IdFascicolo =  '<%= TxtCodiceFasc.value %>';
             //alert("IdFascicolo" + IdFascicolo)
            var IdTxtIdFasc = CostruisciId("TxtCodiceFasc");
              //alert("IdTxtIdFasc" + IdTxtIdFasc)
            var codiceFascicolo = document.getElementById(IdTxtCodiceFascicolo).value;
             //alert("codiceFascicolo" + codiceFascicolo)
		    if(codiceFascicolo!=''){
                myWin = window.open("WfrmSIGEDElencoFascicoli.aspx?Processo=VOLONTARI&objForm=Form1&objNumero="+IdTxtCodiceFascicolo+"&objCodice="+IdTxtIdFasc+"&objDescFasc="+IdDescrizioneFascicolo+"&VArUpdate=4", "Visualizza", winOption);		
            }else{
				//alert ("Cancellare il Fascicolo prima di associarne uno nuovo!");
                 myWin = window.open("WfrmSIGEDElencoFascicoli.aspx?Processo=VOLONTARI&objForm=Form1&objNumero="+IdTxtCodiceFascicolo+"&objCodice="+IdTxtIdFasc+"&objDescFasc="+IdDescrizioneFascicolo+"&VArUpdate=4", "Visualizza", winOption);		
			}			
        }
 

		</script>






		

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
&nbsp;<fieldset class="ContornoPagina" >
<legend>Elenco Documentazione Iscrizione</legend>
				<asp:label id="lblmessaggiosopra" runat="server" Visible="false"></asp:label>
	<div class="wrapper"  id="TableIni" style="width:100%">
					


                     <div class="headers" >
            <h2>
             <asp:Label ID="Label9" runat="server" Text="Fascicolo Ente"></asp:Label> 
            </h2>
        </div>

                     <br />
                   

					 <div class="row">
						<div class="collable" style="width:100%">
							<div class="wrapper" id="Table" style="width:99%;margin-left:3px;">
                          <br />
								<div class="row">
									<div class="collable" style="width:20%">
                                    <asp:label id="Label11" runat="server" AssociatedControlID="TxtNumeroFascicolo" Text="Numero Fascicolo :"></asp:label>
                                    </div>   
												
									<div class="colOggetti" style="width:60%">
                                    <asp:textbox id="TxtNumeroFascicolo" runat="server"  ReadOnly="True"></asp:textbox>
									</div>	

                                    <div class="colOggetti" style="width:19%;text-align:right">
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
								<div class="row">
									<div class="collable" style="width:20%"><asp:label id="Label31" runat="server" AssociatedControlID="txtDescFasc" Text="Descrizione :" Visible="True"></asp:label></div>   
											
									<div class="colOggetti" style="width:79%"><asp:textbox id="txtDescFasc" runat="server" Width="100%"  ReadOnly="True" TextMode="MultiLine"></asp:textbox></div>
								</div>
                                <div class="RigaPulcsanti" style="text-align:right">
                                   <%-- <asp:button style="CURSOR: hand" id="Salva" runat="server" Text="Salva"  CssClass="Pulsante"
											ToolTip="Salva" ></asp:button>--%>
								</div>
							</div>
						   </div>
					 </div>
              
                     <div class="RigaVuota">
        
        
                    &nbsp;</div>

                     <div class="headers" style="text-align:center">
                        <h2>
                            <asp:Label ID="Label3" runat="server" Text="Iscrizione"></asp:Label> 
                        </h2>
                      </div>

                     <div class="RigaVuota">
        
        
                    &nbsp;</div>



					<div class="row">
						<div class="colOggetti" style="width:70%">
                        <asp:checkbox id="chkComAvvioProd" runat="server"></asp:checkbox><asp:label id="Label4" runat="server" AssociatedControlID="chkComAvvioProd"  Text="Comunicazione avvio procedimento"></asp:label>
                        </div>
                        <div class="colOggetti" style="width:25%">
                        <asp:hyperlink id="hpComAvvioProd" runat="server" Visible="False" CssClass="linkStampa" ToolTip="Scarica File" Target="_blank">Download File</asp:hyperlink>			
                        </div>
					</div>

					<%--<div class="clear" style="visibility:hidden">
							<div class="wrapper"  id="Table1">
								<div class="row">
									<div><asp:hyperlink id="hpComAvvioProd" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo1" runat="server" Visible="False">Num. Protocollo</asp:label></div>
									<div><asp:textbox id="TxtNumProtocollo1" runat="server" Visible="False"  ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo1" onclick="javascript:SelProtocollo('TxtNumProtocollo1','TxtDataProtocollo1')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati1" onclick="javascript:ApriAllegati(TxtNumProtocollo1.value,TxtDataProtocollo1.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo1" onclick="javascript:Protocollazione('TxtNumProtocollo1','TxtDataProtocollo1')"
											Visible="False"  ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div><asp:label id="LblDataProtocollo1" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo1" runat="server" Visible="False"  ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
					</div>--%>

					<div class="row">
						<div class="colOggetti" style="width:70%">
                        <asp:checkbox id="chkLetteraCompleDocu" runat="server"></asp:checkbox><asp:label id="Label5" runat="server" AssociatedControlID="chkLetteraCompleDocu" Text="Lettera completamento documentazione"></asp:label>
                        </div>
                        <div class="colOggetti" style="width:25%">
                        <asp:hyperlink id="hpLetteraCompleDocu" runat="server" Visible="False" ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
											
                        </div>
					</div>

					<%--<div class="clear" style="visibility:hidden">
							<div class="wrapper">
								<div class="row">
									<div><asp:hyperlink id="hpLetteraCompleDocu" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo2" runat="server" Visible="False"
											  >Num. Protocollo</asp:label></div>
									<div><asp:textbox id="TxtNumProtocollo2" runat="server" Visible="False"  ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo2" onclick="javascript:SelProtocollo('TxtNumProtocollo2','TxtDataProtocollo2')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati2" onclick="javascript:ApriAllegati(TxtNumProtocollo2.value,TxtDataProtocollo2.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo2" onclick="javascript:Protocollazione('TxtNumProtocollo2','TxtDataProtocollo2')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div ><asp:label id="LblDataProtocollo2" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo2" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
					</div>--%>

					<div class="row">
					<div class="colOggetti" style="width:70%">
                    <asp:checkbox id="chkRicClassIIstep" runat="server"></asp:checkbox>
                    <asp:label id="Label1" AssociatedControlID="chkRicClassIIstep" runat="server" Text="False sedi"></asp:label>
                    </div>
					<div class="colOggetti" style="width:25%">
                    <asp:hyperlink id="hpRicClassIIstep" runat="server" Visible="False" ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
                     </div>
                    </div>

					<%--<div class="row" style="visibility:hidden">
						<div>
							<div class="wrapper" id="Table3">
								<div class="row">
									<div><asp:hyperlink id="hpRicClassIIstep" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo3" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo3" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo3" onclick="javascript:SelProtocollo('TxtNumProtocollo3','TxtDataProtocollo3')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati3" onclick="javascript:ApriAllegati(TxtNumProtocollo3.value,TxtDataProtocollo3.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo3" onclick="javascript:Protocollazione('TxtNumProtocollo3','TxtDataProtocollo3')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo3" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo3" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div>--%>

					<div class="row"> <!-- sono qui -->
						<div class="colOggetti" style="width:70%"><asp:checkbox id="ChkArt2IIstep" runat="server"></asp:checkbox><asp:label AssociatedControlID="ChkArt2IIstep" id="lblart2IIstep" runat="server" 
							Text="Articolo 2"></asp:label>
                            </div>
                                <div class="colOggetti" style="width:25%">
                        <asp:hyperlink id="HplArt2IIstep" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
                        </div>
					</div>

					<%--<div class="row" style="visibility:hidden">
						<div>
							<div class="wrapper"  id="Table4" >
								<div class="row">
									<div><asp:hyperlink id="HplArt2IIstep" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo4" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo4" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo4" onclick="javascript:SelProtocollo('TxtNumProtocollo4','TxtDataProtocollo4')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati4" onclick="javascript:ApriAllegati(TxtNumProtocollo4.value,TxtDataProtocollo4.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo4" onclick="javascript:Protocollazione('TxtNumProtocollo4','TxtDataProtocollo4')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo4" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo4" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div>--%>

					<div class="row">
						<div  class="colOggetti" style="width:70%"><asp:checkbox id="chkart10IIstep" runat="server"></asp:checkbox><asp:label id="lblart10IIstep" AssociatedControlID="chkart10IIstep" runat="server"   
							Text="Articolo 10"></asp:label></div>
                                <div class="colOggetti" style="width:25%">
                                <asp:hyperlink id="hplart10IIstep" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
                        </div>
					</div>

					<%--<div class="row" style="visibility:hidden">
						<div>
							<div class="wrapper"  id="Table5">
								<div class="row">
									<div><asp:hyperlink id="hplart10IIstep" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo5" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo5" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo5" onclick="javascript:SelProtocollo('TxtNumProtocollo5','TxtDataProtocollo5')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati5" onclick="javascript:ApriAllegati(TxtNumProtocollo5.value,TxtDataProtocollo5.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo5" onclick="javascript:Protocollazione('TxtNumProtocollo5','TxtDataProtocollo5')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo5" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo5" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div>--%>

					<div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="chkLetAccPos" runat="server"></asp:checkbox>
                        <asp:label id="lblLetAccPos" runat="server" AssociatedControlID="chkLetAccPos" Text="Lettera Iscrizione Positivo"></asp:label>
                        </div>
                                <div class="colOggetti" style="width:25%">
                                <asp:hyperlink id="hplLetAccPos" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
                        </div>
					</div>

					<%--<div class="row" style="visibility:hidden">
						<div>
							<div class="wrapper"  id="Table6">
								<div class="row">
									<div><asp:hyperlink id="hplLetAccPos" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo6" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo6" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo6" onclick="javascript:SelProtocollo('TxtNumProtocollo6','TxtDataProtocollo6')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati6" onclick="javascript:ApriAllegati(TxtNumProtocollo6.value,TxtDataProtocollo6.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo6" onclick="javascript:Protocollazione('TxtNumProtocollo6','TxtDataProtocollo6')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo6" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo6" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div>--%>

					<div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="chkLetAccNeg" runat="server"></asp:checkbox>
                        <asp:label id="lblLetAccNeg" runat="server" AssociatedControlID="chkLetAccNeg"  Text="Lettera Iscrizione Negativo" ></asp:label></div>
                                <div class="colOggetti" style="width:25%">
                                <asp:hyperlink id="hplLetAccNeg" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
                        </div>
					</div>

					<%--<div class="row" style="visibility:hidden">
						<div>
							<div class="wrapper" id="Table7">
								<div class="row">
									<div><asp:hyperlink id="hplLetAccNeg" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo7" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo7" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo7" onclick="javascript:SelProtocollo('TxtNumProtocollo7','TxtDataProtocollo7')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati7" onclick="javascript:ApriAllegati(TxtNumProtocollo7.value,TxtDataProtocollo7.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo7" onclick="javascript:Protocollazione('TxtNumProtocollo7','TxtDataProtocollo7')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo7" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo7" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div>--%>

					<div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="chkDetAccPos" runat="server" Enabled="False"></asp:checkbox>
                        <asp:label id="lblDetAccPos" runat="server" Text="Decreto Iscrizione Albo SCU Positivo" AssociatedControlID="chkDetAccPos"  Enabled="False"></asp:label></div>
                                 <div class="colOggetti" style="width:25%">
                                 <asp:hyperlink id="hplDetAccPos" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
                        </div>
					</div>

					<%--<div class="row" style="visibility:hidden">
						<div >
							<div class="wrapper"  id="Table8" >
								<div class="row">
									<div><asp:hyperlink id="hplDetAccPos" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo8" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo8" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo8" onclick="javascript:SelProtocollo('TxtNumProtocollo8','TxtDataProtocollo8')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati8" onclick="javascript:ApriAllegati(TxtNumProtocollo8.value,TxtDataProtocollo8.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo8" onclick="javascript:Protocollazione('TxtNumProtocollo8','TxtDataProtocollo8')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo8" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo8" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div>--%>

					<div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="chkDetAccrPosArt10" runat="server" Enabled="False"></asp:checkbox><asp:label id="lblDetAccrPosArt10" runat="server"   
								 Enabled="False" AssociatedControlID="chkDetAccrPosArt10" Text="Decreto Iscrizione Albo SCU Positivo Art. 10"></asp:label></div>
                                 <div class="colOggetti" style="width:25%">
                                <asp:hyperlink id="hplDetAccrPosArt10" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download file</asp:hyperlink>
                        </div>
					</div>

					<%--<div class="row" style="visibility:hidden">
						<div >
							<div class="wrapper"  id="Table9" >
								<div class="row">
									<div><asp:hyperlink id="hplDetAccrPosArt10" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo9" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo9" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo9" onclick="javascript:SelProtocollo('TxtNumProtocollo9','TxtDataProtocollo9')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati9" onclick="javascript:ApriAllegati(TxtNumProtocollo9.value,TxtDataProtocollo9.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo9" onclick="javascript:Protocollazione('TxtNumProtocollo9','TxtDataProtocollo9')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo9" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo9" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div>--%>

					<div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="chkdetAccPosLim" runat="server" Enabled="False"></asp:checkbox><asp:label id="lbldetAccPosLim" runat="server"   
								 Enabled="False" AssociatedControlID="chkdetAccPosLim" Text="Decreto Iscrizione Albo SCU Positivo con Limitazioni Sedi e Figure"></asp:label></div>
                                 <div class="colOggetti" style="width:25%">
                                 <asp:hyperlink id="hpldetAccPosLim" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
                        </div>
					</div>

                    <div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="chkdetAccPosLimSediFigure" runat="server" Enabled="False"></asp:checkbox><asp:label id="lbldetAccPosLimSediFigure" runat="server"   
								 Enabled="False" AssociatedControlID="chkdetAccPosLimSediFigure" Text="Decreto Iscrizione Albo SCU Positivo con Limitazioni Sedi o Figure"></asp:label></div>
                                 <div class="colOggetti" style="width:25%">
                                 <asp:hyperlink id="hpldetAccPosLimSediFigure" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
                        </div>
					</div>
					<%--<div class="row" style="visibility:hidden">
						<div >
							<div class="wrapper"  id="Table10">
								<div class="row">
									<div><asp:hyperlink id="hpldetAccPosLim" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo10" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo10" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo10" onclick="javascript:SelProtocollo('TxtNumProtocollo10','TxtDataProtocollo10')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati10" onclick="javascript:ApriAllegati(TxtNumProtocollo10.value,TxtDataProtocollo10.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo10" onclick="javascript:Protocollazione('TxtNumProtocollo10','TxtDataProtocollo10')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo10" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo10" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div>--%>

					<div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="chkDetAccNeg" runat="server" Enabled="False"></asp:checkbox><asp:label id="lblDetAccNeg" runat="server"   
								 Enabled="False" AssociatedControlID="chkDetAccNeg" Text="Decreto Iscrizione Albo SCU Negativo"></asp:label></div>
                                 <div class="colOggetti" style="width:25%">
                                 <asp:hyperlink id="hplDetAccNeg" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
                        </div>
					</div>

					<%--<div class="row" style="visibility:hidden">
						<div >
							<div class="wrapper"  id="Table11" 
								>
								<div class="row">
									<div><asp:hyperlink id="hplDetAccNeg" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo11" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo11" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo11" onclick="javascript:SelProtocollo('TxtNumProtocollo11','TxtDataProtocollo11')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati11" onclick="javascript:ApriAllegati(TxtNumProtocollo11.value,TxtDataProtocollo11.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo11" onclick="javascript:Protocollazione('TxtNumProtocollo11','TxtDataProtocollo11')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo11" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo11" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div>--%>

					<div class="row" >
						<div class="colOggetti" style="width:70%"><asp:checkbox id="chkAllegatoA1IIStep" runat="server"></asp:checkbox><asp:label id="lblAllegatoA1IIStep" AssociatedControlID="chkAllegatoA1IIStep" runat="server"   
								Text="Allegato A1"> </asp:label></div>
                                <div class="colOggetti" style="width:25%">
                                <asp:hyperlink id="hplAllegatoA1IIStep" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
                        </div>
					</div>

					<%--<div class="clear" style="visibility:hidden">
						
							<div class="wrapper"  id="Table12"> 
								
								<div class="row">
									<div><asp:hyperlink id="hplAllegatoA1IIStep" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">hplAllegatoA1IIStep</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo12" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo12" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo12" onclick="javascript:SelProtocollo('TxtNumProtocollo12','TxtDataProtocollo12')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati12" onclick="javascript:ApriAllegati(TxtNumProtocollo12.value,TxtDataProtocollo12.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo12" onclick="javascript:Protocollazione('TxtNumProtocollo12','TxtDataProtocollo12')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo12" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo12" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
					
					</div>--%>

					<div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="chkAllegatoA2IIStep" runat="server"></asp:checkbox><asp:label id="lblAllegatoA2IIStep" AssociatedControlID="chkAllegatoA2IIStep" runat="server"   
								Text="Allegato A2"></asp:label></div>
                                <div class="colOggetti" style="width:25%">
                                <asp:hyperlink id="hplAllegatoA2IIStep" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
                        </div>
					</div>

					<%--<div class="clear" style="visibility:hidden">
						<div >
							<div class="wrapper"  id="Table13" >
								<div class="row">
									<div><asp:hyperlink id="hplAllegatoA2IIStep" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">hplAllegatoA2IIStep</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo13" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo13" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo13" onclick="javascript:SelProtocollo('TxtNumProtocollo13','TxtDataProtocollo1')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati13" onclick="javascript:ApriAllegati(TxtNumProtocollo13.value,TxtDataProtocollo13.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo13" onclick="javascript:Protocollazione('TxtNumProtocollo13','TxtDataProtocollo13')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo13" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo13" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div>--%>

					<div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="chkAllegatoBIIStep" runat="server"></asp:checkbox><asp:label id="lblAllegatoBIIStep" AssociatedControlID="chkAllegatoBIIStep" runat="server"   
								Text="Allegato B"> </asp:label></div>
                                <div class="colOggetti" style="width:25%">
                                <asp:hyperlink id="hplAllegatoBIIStep" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
                        </div>
					</div>

					<%--<div class="clear" style="visibility:hidden">
						<div >
							<div class="wrapper"  id="Table14" >
								<div class="row">
									<div><asp:hyperlink id="hplAllegatoBIIStep" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo14" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo14" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo14" onclick="javascript:SelProtocollo('TxtNumProtocollo14','TxtDataProtocollo14')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati14" onclick="javascript:ApriAllegati(TxtNumProtocollo14.value,TxtDataProtocollo14.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo14" onclick="javascript:Protocollazione('TxtNumProtocollo14','TxtDataProtocollo14')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo14" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo14" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div> --%>
                    
                    <!-- Adeguamento -->

					<div class="headers" >
                        <h2>
             <asp:Label ID="Label8" runat="server" Text="Adeguamento"></asp:Label>
                        </h2>
                      </div>

                    <div class="RigaVuota">
        
        
                       &nbsp;</div>

					<div class="row">
						
							<div class="wrapper" >
								<div class="row">
									<div class="colOggetti" style="width:25%">
                                        <asp:Label ID="Label10" runat="server" AssociatedControlID="ddlFasi" Text="Fase"></asp:Label>
                                        </div>
									<div  class="colOggetti" style="width:70%"><asp:dropdownlist id="ddlFasi" runat="server" AutoPostBack="True"></asp:dropdownlist></div>
								</div>
							</div>
                       
				    </div>
         
					<div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="chkComAvvioProdAdeg" runat="server"></asp:checkbox><asp:label id="Label6" AssociatedControlID="chkComAvvioProdAdeg" runat="server"  Text="Comunicazione avvio procedimento"  ></asp:label></div>
					<div class="colOggetti" style="width:25%">

                    <asp:hyperlink id="hpComAvvioProdAdeg" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
                    </div>
                    </div>

					<%--<div style="visibility:hidden" class="row">
						
							<div class="wrapper"  id="Table15" >
								<div class="row">
									<div><asp:hyperlink id="hpComAvvioProdAdeg" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo15" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo15" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo15" onclick="javascript:SelProtocollo('TxtNumProtocollo15','TxtDataProtocollo15')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati15" onclick="javascript:ApriAllegati(TxtNumProtocollo15.value,TxtDataProtocollo15.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo15" onclick="javascript:Protocollazione('TxtNumProtocollo15','TxtDataProtocollo15')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo15" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo15" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						
					</div>--%>

					<div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="chkLetteraCompleDocuAdeg" runat="server"></asp:checkbox><asp:label id="Label7" runat="server" AssociatedControlID="chkLetteraCompleDocuAdeg"  Text="Lettera completamento documentazione"  ></asp:label></div>
					<div class="colOggetti" style="width:25%">
                    <asp:hyperlink id="hpLetteraCompleDocuAdeg" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>

                    </div>
                    </div>

					<%--<div class="row" style="visibility:hidden">
						<div >
							<div class="wrapper"  id="Table16" >
								<div class="row">
									<div><asp:hyperlink id="hpLetteraCompleDocuAdeg" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo16" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo16" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo16" onclick="javascript:SelProtocollo('TxtNumProtocollo16','TxtDataProtocollo16')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati16" onclick="javascript:ApriAllegati(TxtNumProtocollo16.value,TxtDataProtocollo16.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo16" onclick="javascript:Protocollazione('TxtNumProtocollo16','TxtDataProtocollo16')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo16" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo16" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div>--%>

					<div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="chkRicClassAdeg" runat="server"></asp:checkbox><asp:label id="lblRicClassAdeg" runat="server" AssociatedControlID="chkRicClassAdeg"  Text="False sedi"  ></asp:label></div>
					<div class="colOggetti" style="width:25%">
                    <asp:hyperlink id="hpRicClassAdeg" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>

                    </div>
                    </div>

                   <%-- <div class="row" style="visibility:hidden">
                            <div class="wrapper"  id="Table17" >
								<div class="row">
									<div><asp:hyperlink id="hpRicClassAdeg" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo17" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo17" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo17" onclick="javascript:SelProtocollo('TxtNumProtocollo17','TxtDataProtocollo17')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati17" onclick="javascript:ApriAllegati(TxtNumProtocollo17.value,TxtDataProtocollo17.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo17" onclick="javascript:Protocollazione('TxtNumProtocollo17','TxtDataProtocollo17')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo17" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo17" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
				    </div>--%>

					<div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="ChkArt2adeg" runat="server"></asp:checkbox><asp:label id="LblArt2adeg" AssociatedControlID="ChkArt2adeg" runat="server"   
								Text="Articolo 2"></asp:label></div>
                                <div class="colOggetti" style="width:25%">

                                <asp:hyperlink id="HplArt2adeg" runat="server" Visible="False"  ToolTip="Scarica File" CssClass="linkStampa"
											Target="_blank">Download File</asp:hyperlink>
                    </div>
					</div>

					<%--<div class="row" style="visibility:hidden">
						<div >
							<div class="wrapper"  id="Table18" >
								<div class="row">
									<div><asp:hyperlink id="HplArt2adeg" runat="server" Visible="False" ImageUrl="images/download.gif" ToolTip="Scarica File"
											Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo18" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo18" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo18" onclick="javascript:SelProtocollo('TxtNumProtocollo18','TxtDataProtocollo18')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati18" onclick="javascript:ApriAllegati(TxtNumProtocollo18.value,TxtDataProtocollo18.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo18" onclick="javascript:Protocollazione('TxtNumProtocollo18','TxtDataProtocollo18')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo18" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo18" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div>--%>

					<div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="chkArt10adeg" runat="server"></asp:checkbox><asp:label id="lblart10adeg" AssociatedControlID="chkArt10adeg" runat="server"   
								Text="Articolo 10"></asp:label></div>
                                <div class="colOggetti" style="width:25%">
                                <asp:hyperlink id="hplart10adeg" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>

                    </div>
					</div>

					<%--<div class="row" style="visibility:hidden">
						<div>
							<div class="wrapper"  id="Table19" >
								<div class="row">
									<div><asp:hyperlink id="hplart10adeg" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo19" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo19" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo19" onclick="javascript:SelProtocollo('TxtNumProtocollo19','TxtDataProtocollo19')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati19" onclick="javascript:ApriAllegati(TxtNumProtocollo19.value,TxtDataProtocollo19.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo19" onclick="javascript:Protocollazione('TxtNumProtocollo19','TxtDataProtocollo19')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo19" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo19" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div>--%>

					<div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="chkLettAdegPosNeg" runat="server"></asp:checkbox><asp:label id="lblLettAdegPosNeg" AssociatedControlID="chkLettAdegPosNeg" runat="server"   
								Text="Lettera Adeguamento Positivo e Negativo"></asp:label></div>
                                <div class="colOggetti" style="width:25%">

                                <asp:hyperlink id="hplLettAdegPosNeg" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
                    </div>
					</div>

					<%--<div class="row" style="visibility:hidden">
						<div >
							<div class="wrapper"  id="Table20" >
								<div class="row">
									<div><asp:hyperlink id="hplLettAdegPosNeg" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo20" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo20" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo20" onclick="javascript:SelProtocollo('TxtNumProtocollo20','TxtDataProtocollo20')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati20" onclick="javascript:ApriAllegati(TxtNumProtocollo20.value,TxtDataProtocollo20.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo20" onclick="javascript:Protocollazione('TxtNumProtocollo20','TxtDataProtocollo20')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo20" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo20" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div>--%>

					<div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="chkDetAdegPos" runat="server" Enabled="False"></asp:checkbox><asp:label id="lblDetAdegPos" AssociatedControlID="chkDetAdegPos"  runat="server"   
								 Enabled="False" Text="Determina Adeguamento Positivo"></asp:label></div>
                                 <div class="colOggetti" style="width:25%">
                                 <asp:hyperlink id="hplDetAdegPos" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>

                    </div>
					</div>

					<%--<div class="row" style="visibility:hidden">
						<div>
							<div class="wrapper"  id="Table21" >
								<div class="row">
									<div><asp:hyperlink id="hplDetAdegPos" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo21" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo21" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo21" onclick="javascript:SelProtocollo('TxtNumProtocollo21','TxtDataProtocollo21')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati21" onclick="javascript:ApriAllegati(TxtNumProtocollo21.value,TxtDataProtocollo21.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo21" onclick="javascript:Protocollazione('TxtNumProtocollo21','TxtDataProtocollo21')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo21" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo21" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div>--%>

					<div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="ChkDetAdegPosArt10" runat="server" Enabled="False"></asp:checkbox><asp:label id="lblDetAdegPosArt10" AssociatedControlID="ChkDetAdegPosArt10" runat="server"   
								 Enabled="False" Text="Determina Adeguamento Positivo Art. 10"></asp:label></div>
                                 <div class="colOggetti" style="width:25%">
                                 <asp:hyperlink id="HplDetAdegPosArt10" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>

                    </div>
					</div>

					<%--<div class="row" style="visibility:hidden">
						<div >
							<div class="wrapper"  id="Table22" >
								<div class="row">
									<div><asp:hyperlink id="HplDetAdegPosArt10" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank"></asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo22" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo22" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo22" onclick="javascript:SelProtocollo('TxtNumProtocollo22','TxtDataProtocollo22')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati22" onclick="javascript:ApriAllegati(TxtNumProtocollo22.value,TxtDataProtocollo22.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo22" onclick="javascript:Protocollazione('TxtNumProtocollo22','TxtDataProtocollo22')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo22" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo22" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div>--%>

					<div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="chkDetAdegPosLim" runat="server" Enabled="False"></asp:checkbox><asp:label id="lblDetAdegPosLim" runat="server"   
								 Enabled="False" AssociatedControlID="chkDetAdegPosLim" Text="Determina Adeguamento Positivo con Limitazioni"></asp:label></div>
                                 <div class="colOggetti" style="width:25%">
                                 <asp:hyperlink id="hplDetAdegPosLim" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>

                    </div>
					</div>

					<%--<div class="row" style="visibility:hidden">
						<div >
							<div class="wrapper"  id="Table23" >
								<div class="row">
									<div><asp:hyperlink id="hplDetAdegPosLim" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo23" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo23" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo23" onclick="javascript:SelProtocollo('TxtNumProtocollo23','TxtDataProtocollo23')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati23" onclick="javascript:ApriAllegati(TxtNumProtocollo23.value,TxtDataProtocollo23.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo23" onclick="javascript:Protocollazione('TxtNumProtocollo23','TxtDataProtocollo23')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo23" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo23" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div>--%>

					<div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="chkDetAdegNeg" runat="server"></asp:checkbox><asp:label id="lblDetAdegNeg" AssociatedControlID="chkDetAdegNeg" runat="server"   
								Text="Determina Adeguamento Negativo"></asp:label></div>
                                <div class="colOggetti" style="width:25%">
                                <asp:hyperlink id="hplDetAdegNeg" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>

                    </div>
					</div>

					<%--<div class="row" style="visibility:hidden">
						<div >
							<div class="wrapper"  id="Table24" >
								<div class="row">
									<div><asp:hyperlink id="hplDetAdegNeg" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo24" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo24" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo24" onclick="javascript:SelProtocollo('TxtNumProtocollo24','TxtDataProtocollo24')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati24" onclick="javascript:ApriAllegati(TxtNumProtocollo24.value,TxtDataProtocollo24.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo24" onclick="javascript:Protocollazione('TxtNumProtocollo24','TxtDataProtocollo24')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo24" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo24" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div>--%>

					<div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="chkAllegatoA1" runat="server"></asp:checkbox>
							<asp:label id="lblAllegatoA1" AssociatedControlID="chkAllegatoA1" runat="server"   
								Text="Allegato A1"></asp:label>
						</div>
                        <div class="colOggetti" style="width:25%">

                        <asp:hyperlink id="hplAllegatoA1" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
                    </div>
					</div>

					<%--<div class="row" style="visibility:hidden">
						<div >
							<div class="wrapper"  id="Table25" >
								<div class="row">
									<div><asp:hyperlink id="hplAllegatoA1" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo25" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo25" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo25" onclick="javascript:SelProtocollo('TxtNumProtocollo25','TxtDataProtocollo25')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati25" onclick="javascript:ApriAllegati(TxtNumProtocollo25.value,TxtDataProtocollo25.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo25" onclick="javascript:Protocollazione('TxtNumProtocollo25','TxtDataProtocollo25')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo25" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo25" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div>--%>

					<div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="chkAllegatoA2" runat="server"></asp:checkbox>
							<asp:label id="lblAllegatoA2" AssociatedControlID="chkAllegatoA2" runat="server"   
								Text="Allegato A2"></asp:label>
						</div>
                        <div class="colOggetti" style="width:25%">
                        <asp:hyperlink id="hplAllegatoA2" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>

                    </div>
					</div>

					<%--<div class="row" style="visibility:hidden">
						<div >
							<div class="wrapper"  id="Table26" >
								<div class="row">
									<div><asp:hyperlink id="hplAllegatoA2" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo26" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo26" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo26" onclick="javascript:SelProtocollo('TxtNumProtocollo26','TxtDataProtocollo26')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati26" onclick="javascript:ApriAllegati(TxtNumProtocollo26.value,TxtDataProtocollo26.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo26" onclick="javascript:Protocollazione('TxtNumProtocollo26','TxtDataProtocollo26')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo26" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo26" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div>--%>

					<div class="row">
						<div class="colOggetti" style="width:70%"><asp:checkbox id="chkAllegatoB" runat="server"></asp:checkbox>
							<asp:label id="lblAllegatoB" AssociatedControlID="chkAllegatoB" runat="server"   
								Text="Allegato B"></asp:label>
						</div>
                        <div class="colOggetti" style="width:25%">
                        <asp:hyperlink id="hplAllegatoB" runat="server" Visible="False" 
											ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>

                    </div>
					</div>

					<%--<div class="row" style="visibility:hidden">
						<div >
							<div class="wrapper"  id="Table27" >
								<div class="row">
									<div><asp:hyperlink id="hplAllegatoB" runat="server" Visible="False" ImageUrl="images/download.gif"
											ToolTip="Scarica File" Target="_blank">HyperLink</asp:hyperlink></div>
									<div><asp:label id="LblNumProtocollo27" runat="server" Visible="False" Width="56px" 
											  >Num. Protocollo</asp:label></div>
									<div style="WIDTH: 758px"><asp:textbox id="TxtNumProtocollo27" runat="server" Visible="False" Width="112px" ReadOnly="True"
											MaxLength="50"></asp:textbox><asp:image style="CURSOR: hand" id="cmdSelProtocollo27" onclick="javascript:SelProtocollo('TxtNumProtocollo27','TxtDataProtocollo27')"
											runat="server" Visible="False" ImageUrl="images/Busta.gif" ToolTip="Seleziona Protocollo"></asp:image><asp:image style="CURSOR: hand" id="cmdScAllegati27" onclick="javascript:ApriAllegati(TxtNumProtocollo27.value,TxtDataProtocollo27.value)"
											runat="server" Visible="False" Height="20px" ImageUrl="images/giu.jpg" ToolTip="Visualizza Allegati"></asp:image><asp:image style="CURSOR: hand" id="cmdNuovoFascicolo27" onclick="javascript:Protocollazione('TxtNumProtocollo27','TxtDataProtocollo27')"
											Visible="False" Height="20px" ImageUrl="images/GraduatoriaVol.gif" ToolTip="Richiesta Protocollazione" Runat="server"></asp:image></div>
									<div style="WIDTH: 120px"><asp:label id="LblDataProtocollo27" runat="server" Visible="False"  
											 >Data Protocollo</asp:label></div>
									<div><asp:textbox id="TxtDataProtocollo27" runat="server" Visible="False" Width="128px" ReadOnly="True"
											MaxLength="10"></asp:textbox></div>
								</div>
							</div>
						</div>
					</div>--%>
					<!-- Revisioni -->

                    <div class="row">
                        <div class="colOggetti" style="width:70%">
	                        <asp:checkbox id="chkDetAdegPosTot" runat="server" Enabled="False"></asp:checkbox>
	                        <asp:label id="lblDetAdegPosTot" AssociatedControlID="chkDetAdegPosTot"  runat="server"  Enabled="False" Text="Determina Adeguamento Positivo Totale"></asp:label></div>
                        <div class="colOggetti" style="width:25%">
                             <asp:hyperlink id="hplDetAdegPosTot" runat="server" Visible="False" ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
                        </div>
                        </div>
                        <div class="row">
                        <div class="colOggetti" style="width:70%">
	                        <asp:checkbox id="ChkDetAdegPosArt10Tot" runat="server" Enabled="False"></asp:checkbox>
	                        <asp:label id="lblDetAdegPosArt10Tot" AssociatedControlID="ChkDetAdegPosArt10Tot" runat="server"  Enabled="False" Text="Determina Adeguamento Positivo Art. 10 Totale"></asp:label>
                        </div>
                        <div class="colOggetti" style="width:25%">
	                        <asp:hyperlink id="HplDetAdegPosArt10Tot" runat="server" Visible="False" ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
                        </div>
                     </div>

                    <div class="row">
                        <div class="colOggetti" style="width:70%">
                            <asp:checkbox id="chkDetAdegPosLimTot" runat="server" Enabled="False"></asp:checkbox>
                            <asp:label id="lblDetAdegPosLimTot" runat="server" Enabled="False" AssociatedControlID="chkDetAdegPosLimTot" Text="Determina Adeguamento Positivo con Limitazioni Totale"></asp:label>
                        </div>
                        <div class="colOggetti" style="width:25%">
                            <asp:hyperlink id="hplDetAdegPosLimTot" runat="server" Visible="False"  ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
                        </div>
                    </div>
                    <div class="row">
                        <div class="colOggetti" style="width:70%">
                            <asp:checkbox id="chkDetAdegNegTot" runat="server" Enabled="False"></asp:checkbox>
                            <asp:label id="lblDetAdegNegTot" AssociatedControlID="chkDetAdegNegTot" runat="server"  Text="Determina Adeguamento Negativo Totale"></asp:label>
                        </div>
                        <div class="colOggetti" style="width:25%">
                            <asp:hyperlink id="hplDetAdegNegTot" runat="server" Visible="False" ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
                        </div>
                    </div>
                    <div class="row">
	                    <div class="colOggetti" style="width:70%" >
                            <asp:checkbox id="chkAllegatoA1Tot" runat="server" Enabled="False"></asp:checkbox>
		                    <asp:label id="lblAllegatoA1Tot" AssociatedControlID="chkAllegatoA1Tot" runat="server" Text="Allegato A1 Totale"></asp:label>
	                    </div>
                        <div class="colOggetti" style="width:25%">
                            <asp:hyperlink id="hplAllegatoA1Tot" runat="server" Visible="False" ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
	                    </div>
                    </div>
                    <div class="row">
	                    <div class="colOggetti" style="width:70%">
		                    <asp:checkbox id="chkAllegatoA2Tot" runat="server" Enabled="False"></asp:checkbox>
		                    <asp:label id="lblAllegatoA2Tot" AssociatedControlID="chkAllegatoA2Tot" runat="server"   Text="Allegato A2 Totale"></asp:label>
	                    </div>
                        <div class="colOggetti" style="width:25%">
		                    <asp:hyperlink id="hplAllegatoA2Tot" runat="server" Visible="False" ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
                        </div>
                    </div>
                    <div class="row">
	                    <div class="colOggetti" style="width:70%">
		                    <asp:checkbox id="chkAllegatoBTot" runat="server" Enabled="False"></asp:checkbox>
		                    <asp:label id="lblAllegatoBTot" AssociatedControlID="chkAllegatoBTot" runat="server" Text="Allegato B Totale"></asp:label>
	                    </div>
                        <div class="colOggetti" style="width:25%"> 
		                    <asp:hyperlink id="hplAllegatoBTot" runat="server" Visible="False" ToolTip="Scarica File" CssClass="linkStampa" Target="_blank">Download File</asp:hyperlink>
	                    </div>
                    </div>
                    
					<div class="RigaPulsanti">
					 
                        <asp:button id="imgGeneraFile" runat="server" Text="Genera" CssClass="Pulsante"></asp:button>

                        <asp:button id="imgChiudi" text="Chiudi" CssClass="Pulsante" runat="server" ></asp:button>
                     
					</div>




				</div>



   
				<input style="Z-INDEX: 101; POSITION: absolute; WIDTH: 17px; HEIGHT: 22px; TOP: 16px; LEFT: 32px"
					id="HdValoreSalva" value="0" size="1" type="hidden" name="HdValoreSalva" runat="server"/>
				<input style="Z-INDEX: 102; POSITION: absolute; WIDTH: 17px; HEIGHT: 22px; TOP: 16px; LEFT: 56px"
					id="hddModificaProtocollo" value="0" size="1" type="hidden" name="HdValoreSalva" runat="server"/>
                    <div class="skip">
                   <input type="hidden"  id="TxtCodiceFasc" runat="server"/>
                 
                    <%--<asp:textbox id="TxtCodiceFasc" runat="server" Width="100px" ReadOnly="True"></asp:textbox>--%>
                                            <asp:textbox id="TxtNumFascicoloControllo"  runat="server" Width="0px"></asp:textbox>
                                     
                    </div>
             </fieldset>
</asp:Content>
