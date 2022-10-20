<%@ Page Title="Associa Volontari" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmAssociaVolontari.aspx.vb" Inherits="Futuro.WfrmAssociaVolontari"  MaintainScrollPositionOnPostBack="true"  %>
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

           <script type="text/javascript">
        var isPostbackProgetto = '<%= hdnIsPostbackProgetto1.value %>';   
        var pathPlus = "images/plus_xsmall.png";
        var pathMinus = "images/minus_xsmall.png";
        var titlePathMinus = "Riduci";
        var titlePathPlus = "Espandi";
        /* <![CDATA[ */
          $(function () {
                             if ('<%=request.querystring("IdAttivita")%>' == '') {

                                 document.getElementById('MainContent_flsProgramma').style.display = 'none'
                             }
                             else {


                                 document.getElementById('MainContent_flsProgramma').style.display = 'yes'
                             }

                             

//                             $('#VisualizzaProgrammaContent').css('display', 'none');
//                             $('#VisualizzaProgramma').find("img").first().attr("src", pathPlus);
//                             $('#VisualizzaProgramma').find("img").first().attr("title", titlePathPlus);

           


            $('#VisualizzaProgramma').click(function () 
            {
              
             
                            
                    var isVisible = $('#VisualizzaProgrammaContent').is(':visible');
                    $('#VisualizzaProgrammaContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                   
                   
                    if (!isVisible) 
                    {
                      
                    
                      $(this).find("img").first().attr("src", pathMinus);
                      $(this).find("img").first().attr("title", titlePathMinus);
                        
                    }
                    else 
                    {
                   

                      $(this).find("img").first().attr("src", pathPlus);
                      $(this).find("img").first().attr("title", titlePathPlus);
                       
                    }
              
            
                return false;
            });
        });
        </script>
       <script  type="text/javascript">

           var formId;
           var prefissoIdClient = "MainContent_";

           function ImpostaFormId() {
               formId = document.forms[0].id;
               return formId;
           }
           function CostruisciId(IdServer) {
               var IdClient = prefissoIdClient + IdServer
               return IdClient
           }

           $(function () {
               var IdtxtinizioEff = CostruisciId('txtinizioEff');
               var sharptxtinizioEff = "#" + IdtxtinizioEff
               
               var operazione = '<%=piker.value%>'

               if (operazione != 0) {
                   $("" + sharptxtinizioEff + "").datepicker();
               }
              
           });
           $(function () {
               var IdtxtInizioServizioSede = CostruisciId('txtInizioServizioSede');
               var sharptxtInizioServizioSede = "#" + IdtxtInizioServizioSede

               $("" + sharptxtInizioServizioSede + "").datepicker();
           });
	</script>
       <script type="text/javascript">/* non utilizzate */
      /* <![CDATA[ */
			function SelProtocollo(NumProt,DataProt)
			{
				if (document.all.MainContent_TxtNumFascicolo.value =='')
					{
						alert ("Nessun Fascicolo Selezionato!");
					}else{
						myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?objForm=Form1&TxtProt="+NumProt+"&VarVolontario=<%=Request.QueryString("IdVol")%>&TxtData="+DataProt+"&VArUpdate=2&NumeroFascicolo=<%=Request.QueryString("NumeroFascicolo")%>", "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=si");				
						
					}
			}

            function ApriAllegati(NumProt,DataProt)/* non utilizzate */
			{
					if (document.all.MainContent_TxtNumFascicolo.value =='')
					{
						alert ("Nessun Fascicolo Selezionato!");
					}else{	
						myWin = window.open("WfrmSIGEDElencoAllegati.aspx?NumeroProtocollo=" + NumProt + "&DataProtocollo=" + DataProt+ "&CodiceFascicolo=" + document.all.TxtNumFascicolo.value,"Visualizza","width=800,height=350,dependent=no,scrollbars=yes,status=si");		
					}
			}

			function Protocollazione(NumProt,DataProt)/* non utilizzate */
			
			{

				if (document.getElementById(NumProt).value=='' || document.getElementById(DataProt).value=='')

					{
						myWin = window.open("WfrmSIGEDProtocollazione.aspx?CorrCodUnivoco=<%= Session("txtCodEnte")%>&Processo=VOLONTARI&objForm=Form1&txtnumerofascicoloinvisione=txtNumeroFascicoloinVisione&txtdescrizionefascicolo=txtdescrizionefascicolo&TxtFasc=TxtNumFascicolo&TxtCodFasc=TxtCodFascicolo&TxtDescFascicolo=TxtDescFAsc&TxtProt="+NumProt+"&Fascicolo=" + Form1.txtNumeroFascicoloinVisione.value+"&IDAttivitasedeAssegnazione=<%=Request.QueryString("IDAttivitasedeAssegnazione")%>&IdAttivita=<%=Request.QueryString("IdAttivita")%>&VarVolontario=<%=Request.QueryString("IdVol")%>&TxtData="+DataProt+"&VArUpdate=2&NumeroFascicolo=<%=Request.QueryString("NumeroFascicolo")%>&DescFascicolo=<%=Request.QueryString("DescFascicolo")%>&CodEnte=<%= Session("pCodEnte")%>", "Visualizza","width=800,height=400,dependent=no,scrollbars=yes,status=si");
					}
					else
					{
						var msg;
						msg=confirm("Si vuole inserire il documento Principale?");
						if (msg==false)
							{
								myWin = window.open("WfrmSIGEDProtocollazione.aspx?CorrCodUnivoco=<%= Session("txtCodEnte")%>&Processo=VOLONTARI&objForm=Form1&txtnumerofascicoloinvisione=txtNumeroFascicoloinVisione&txtdescrizionefascicolo=txtdescrizionefascicolo&TxtFasc=TxtNumFascicolo&TxtCodFasc=TxtCodFascicolo&TxtDescFascicolo=TxtDescFAsc&TxtProt="+NumProt+"&Fascicolo=" + Form1.txtNumeroFascicoloinVisione.value+"&IDAttivitasedeAssegnazione=<%=Request.QueryString("IDAttivitasedeAssegnazione")%>&IdAttivita=<%=Request.QueryString("IdAttivita")%>&VarVolontario=<%=Request.QueryString("IdVol")%>&TxtData="+DataProt+"&VArUpdate=2&NumeroFascicolo=<%=Request.QueryString("NumeroFascicolo")%>&DescFascicolo=<%=Request.QueryString("DescFascicolo")%>&CodEnte=<%= Session("pCodEnte")%>", "Visualizza","width=800,height=400,dependent=no,scrollbars=yes,status=si");			
								return false;
							}else
							{	
								myWin = window.open("WfrmSIGEDDocumentoPrincipale.aspx?NumProt=" + document.getElementById(NumProt).value + "&DataProt=" + document.getElementById(DataProt).value, "Visualizza", "width=400,height=200,dependent=no,scrollbars=yes,status=no");			
							}
					}	
			}

/* ]]> */
  </script>
	   <script  type="text/javascript">		    /* da portare successivamente lato server*/
        /* <![CDATA[ */
		    function ConfermaCanc(IdVol, StatoVol) {
		        var msg;
		        msg = confirm("Procedere con l'operazione?");
		        if (msg == false) {
		            return false;
		        }
		        else {
		            document.all.txtIdVolCanc.value = IdVol;
		            document.all.MainContent_txtStatoVol.value = StatoVol;
		            ctl01.submit();
		        }
		    }
		    /* ]]> */
		</script>
	   <script   type="text/javascript">
        /* <![CDATA[ */
		    function onkeypressTxt() 
        {
			var evtobj=window.event;
			var keyascii=evtobj.keyCode;
			if (parseInt(keyascii)==13){
			}
			if (keyascii>59 || keyascii<45)
			{
				window.event.keyCode=0;
			}
			else
			{
				window.event.keyCode=keyascii;
			}
          }
          /* ]]> */
		</script>
  
		<script type="text/javascript">
        /* <![CDATA[ */

		    function Verificalotus(cf, codicefiscale, eta, identita) 
            {
            
		        window.open("WfrmAnomalieVolontari.aspx?cflotus=" + cf + "&codicefiscale=" + codicefiscale + "&eta=" + eta + "&identita=" + identita, "VisualizzaVolontari", "width=900,height=500,toolbar=no,location=no,menubar=no,scrollbars=yes");

		    }

		/* ]]> */
           </script>
	
		<script  type="text/javascript">
         /* <![CDATA[ */
		    function ModificaVolontario(IdEntita, IDAttivitasedeAssegnazione, IdEnteSede, presenta, IdAttivita) {
		        document.location.href = "WfrmVolontari.aspx?IdVol=" + IdEntita + "&IDAttivitasedeAssegnazione=" + IDAttivitasedeAssegnazione + "&IdEnteSede=" + IdEnteSede + "&presenta=" + presenta + "&IdAttivita=" + IdAttivita
		    }
		    function ApriVincoli(IdVolontario) {
           
		        myWin = window.open("assegnazionevincolivolontario.aspx?IdVolontario=" + IdVolontario, "vincoli", "width=700,height=700,toolbar=no,location=no,menubar=no,scrollbars=yes")
		    }
		    function Modificato() {
		        document.all.MainContent_txtmodificato.value = "Cambiato"
		    }

		    function ControlloAlfabetico(id, txt) {
		        //for=document.getElementById(id)
		        var evtobj = window.event;
		        var keyascii = evtobj.keyCode;
		        var txt = document.getElementById(txt);
		        var i = new RegExp("[,,]");
		        if (parseInt(keyascii) == 13) {

		        }
		        if (keyascii > 57 || keyascii < 48) {
		            if (keyascii == 44) {
		            }
		            else {
		                if (keyascii == 46) {
		                    keyascii = 44
		                    window.event.keyCode = keyascii;
		                } else
		                { window.event.keyCode = 0; }
		            }
		        }
		        else {
		            window.event.keyCode = keyascii;
		        }
		        if (i.test(txt.value) && (keyascii == 44)) {
		            window.event.keyCode = 0;
		        }
		    }
			/* ]]> */
		</script>
			<script  type="text/javascript">
             /* <![CDATA[ */
              function CancellaGraduatoria() 
              {

			     msg=confirm("Con questa operazione verranno cancellati definitivamente tutti i volontari della graduatoria selezionata!Procedere ugualmente?")
			        if(msg==false){
				    return false;
			        }else
                    {
				    document.all.MainContent_lblMessaggioAlert.innerText ="ELIMINAZIONE GRADUATORIA IN CORSO... ATTENDERE!";				
			        }
                }
            /* ]]> */
		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div class="firefox" >
 <fieldset class="ContornoPaginaScroll" style="width:96%">
    <legend >Graduatoria</legend>
       <asp:label id="alertgmo" runat="server" Visible="false" CssClass="msgErroreGrid"></asp:label><br />
														 
      <asp:image id="imgmess" runat="server" Visible="False" Height="17px" Width="24px" ImageUrl="images/conf1.jpg"></asp:image><asp:label id="lblMessaggi" runat="server" Visible="False"  CssClass="msgInfo"></asp:label>
     <fieldset runat="server" id="riattivazione" class="ContornoPaginaScroll" style="width:97%">
    <div style="text-align:right">
        <div class="colHyperLink" style="width:100%">
            <asp:LinkButton ID="imgCronoRiattivazione" 
                style="cursor:pointer; font-weight: 700;" Text="Cronologia Riattivazione Sede" 
                ToolTip="Cronologia Riattivazione Sede" Visible="false"  runat="server" />
        </div>
        <div class="colHyperLink" style="width:100%">
            <asp:LinkButton ID="imgCronoRimodulazione" 
                style="cursor:pointer; font-weight: 700;" Text="Cronologia Rimodulazione Sede" 
                ToolTip="Cronologia Rimodulazione Sede" Visible="false"  runat="server" />
        </div>
    </div>
    </fieldset>
     <div class="wrapper" style="width:99%">
            <div class="headers" >
                <h2>
                <asp:Label ID="lblTitolo" runat="server"  Text="Graduatoria Volontari"></asp:Label>
                </h2>
            </div>
            <div class="RigaVuota"></div>
            <div class="row" id="riga1" style="border-top-color:#3a4f63;border-top-style:inset;" runat="server">
            <div class="collable" style="width:140px">&nbsp;</div>
            <div class="colOggetti" style="width:140px">
                <asp:TextBox ID="txtdatainizioDB" CssClass="skip" ToolTip="nascosto" Text="" runat="server" Width="0px"></asp:TextBox>
                <asp:TextBox ID="txtDataOdierna" CssClass="skip" ToolTip="nascosto" Text="" runat="server" Width="0px"></asp:TextBox>
                <asp:TextBox ID="lblidattivita" CssClass="skip" ToolTip="nascosto" Text="" runat="server"  Width="0px"></asp:TextBox>
                <asp:TextBox ID="lblidSede" CssClass="skip" ToolTip="nascosto" Text="" runat="server" Width="0px"></asp:TextBox>
                <asp:TextBox ID="lblpresentata" CssClass="skip" ToolTip="nascosto" Text="" runat="server" Width="0px"></asp:TextBox>
                <asp:TextBox ID="txtidente" CssClass="skip" ToolTip="nascosto" Text="" runat="server" Width="0px"></asp:TextBox>
                <asp:TextBox ID="txtmodificato" CssClass="skip" ToolTip="nascosto" Text="" runat="server" Width="0px"></asp:TextBox>
            </div>
            <div class="collable" style="width:140px">&nbsp;</div>
            <div class="colOggetti" style="width:140px">    
                <asp:Label ID="lblGenera" AssociatedControlID="imgGeneraDoc" runat="server" CssClass="bold" Visible="False">Genera Documento</asp:Label>
            </div>
            <div class="collable" style="width:160px">&nbsp;
                <asp:ImageButton ID="imgGeneraDoc" AlternateText="Genera Documento"  ToolTip="Genera Documento" runat="server" ImageUrl="images/documento_small.png" 
                 Visible="False" />
                <asp:HyperLink ID="hplDownload" ToolTip="Scarica Documento" CssClass="bold"  runat="server" 
                 Visible="False" Text="DOWNLOAD" ForeColor="#003399"></asp:HyperLink>
            </div>
            <div class="colOggetti" style="width:140px" >               
                <input style="WIDTH: 0px; HEIGHT: 18px" id="TxtCodFascicolo" size="7" type="hidden" name="TxtCodFascicolo" runat="server"/>
                <input style="WIDTH: 0px; HEIGHT: 18px" id="TxtDescFAsc" size="7" type="hidden" name="TxtDescFAsc" runat="server"/>
                <input style="WIDTH: 0px; HEIGHT: 18px" id="TxtNumFascicolo" size="14" type="hidden" name="TxtNumFascicolo" runat="server"/>
            </div>
        </div>
            <div class="row" id="riga2" runat="server">
            <div class="collable" style="width:140px">&nbsp;
                <asp:label id="LblNumFascicolo" AssociatedControlID="txtNumeroFascicoloinVisione" runat="server" Visible="False" >Numero Fascicolo</asp:label>
            </div>
            <div class="colOggetti" style="width:190px">
                <asp:textbox id="txtNumeroFascicoloinVisione" runat="server" Visible="False" Width="180px"  ReadOnly="True" MaxLength="50"></asp:textbox>
            </div>
            <div class="collable" style="width:140px"> &nbsp;
                <asp:label id="LblDescFascicolo" AssociatedControlID="txtdescrizionefascicolo" runat="server" Visible="False" >Descrizione</asp:label>
            </div>
            <div class="colOggetti" style="width:350px">
                <asp:textbox id="txtdescrizionefascicolo" runat="server" Visible="False" Width="300px"  ReadOnly="True" MaxLength="50" TextMode="MultiLine"></asp:textbox>
            </div>    
            </div>
            <div class="row" id="rigaNascosta" runat="server" visible="false">
            <asp:Label ID="LblNumProtocollo" AssociatedControlID="TxtNumProtocollo" runat="server"  Visible="False">Num. Protocollo :</asp:Label>
            <asp:TextBox ID="TxtNumProtocollo" runat="server"  MaxLength="50" ReadOnly="True" Visible="False" ></asp:TextBox>
            <asp:Image ID="cmdScSelProtocolloLV" AlternateText="Seleziona Protocollo" runat="server" ImageUrl="images/busta_small.png" 
                 onclick="javascript:SelProtocollo('TxtNumProtocollo','TxtDataProtocollo')" 
                 style="CURSOR: hand" ToolTip="Seleziona Protocollo" Visible="False" Width="16px" />
            <asp:Image ID="cmdScAllegatiLV" AlternateText="Visualizza Allegati" runat="server" Height="20px" 
                 ImageUrl="images/giu_small.png" 
                 onclick="javascript:ApriAllegati(TxtNumProtocollo.value,TxtDataProtocollo.value)" 
                 style="CURSOR: hand" ToolTip="Visualizza Allegati" Visible="False" />
            <asp:Image ID="cmdNuovoFasciocloLV" AlternateText="Richiesta Protocollazione"  Runat="server" 
                 ImageUrl="images/documento_small.png" 
                 onclick="javascript:Protocollazione('TxtNumProtocollo','TxtDataProtocollo')" 
                 style="CURSOR: hand" ToolTip="Richiesta Protocollazione" Visible="False" />   
            <asp:Label ID="LblDataProtocollo" AssociatedControlID="TxtDataProtocollo" runat="server"   Visible="False">Data Protocollo :</asp:Label>
            <asp:TextBox ID="TxtDataProtocollo" runat="server"  MaxLength="10" ReadOnly="True" Visible="False"></asp:TextBox>
            <asp:Button ID="ImgSalvaProt" runat="server" Text="Salva"  CssClass="Pulsante" ToolTip="Salva" Visible="False" />
    </div>
    <p>
    &nbsp;
    </p>
    <p style="border-bottom-color:#3a4f63;border-bottom-style:inset;margin-left:0px;width:100%">
        <asp:Label ID="LblDescFascicolo0" CssClass="bold" Width="100px" runat="server" Visible="true">Progetto</asp:Label>&nbsp;&nbsp;<asp:Label ID="lblcodprogetto" CssClass="bold" runat="server" >lblcodprogetto</asp:Label>
            &nbsp;&nbsp;<asp:ImageButton ID="imgValutazioneProg" AlternateText="Informazione" runat="server" 
                ImageUrl="images/i5.gif" ToolTip="Valutazione Qualità Progetto" 
                Visible="False" />&nbsp;&nbsp; <asp:Label ID="lblProgetto" CssClass="bold" runat="server"></asp:Label>
        <asp:TextBox ID="lblidattivitasedeassegnazione" CssClass="skip" Text="" ToolTip="Assegnazione" runat="server" ReadOnly="True" ></asp:TextBox>
         <fieldset id="flsProgramma" runat="server" class="ContornoPagina" >
            <legend id="VisualizzaProgramma"><img  src="images/minus_xsmall.png"  alt="Espandi/Riduci" style="cursor:pointer;" />Dati Programma</legend>
                <div class="wrapper" id="VisualizzaProgrammaContent" style="width:100%;border:0px" >
                     <div class="row">
                  <div class="collable" style="width:19%" >
                      <asp:Label ID="Label9" runat="server" Text="Label"></asp:Label>
                  </div>
                    <div class="colOggetti"  style="width:30%">
                    <asp:Label ID="Label10" runat="server" Text="Label" Font-Bold="true"></asp:Label>
                    </div>
                    <div class="collable"  style="width:19%">
                 <asp:Label ID="Label13" runat="server" Text="Label" ></asp:Label>
                  </div>
                    <div class="colOggetti"  style="width:30%" >
                    <asp:Label ID="Label14" runat="server" Text="Label" Font-Bold="true"></asp:Label>
                    </div>
                 </div>
                     <div class="row">
                     <div class="collable"  style="width:19%">
                     <asp:Label ID="Label15" runat="server" Text="Label"></asp:Label>
                  </div>
                    <div class="colOggetti"  style="width:30%">
                    <asp:Label ID="Label16" runat="server" Text="Label"  Font-Bold="true"></asp:Label>
                    </div>
                     <div class="collable"  style="width:19%" >
                     <asp:Label ID="Label17" runat="server" Text="Label"></asp:Label>
                  </div>
                    <div class="colOggetti" style="width:30%" >
                    <asp:Label ID="Label18" runat="server" Text="Label" Font-Bold="true"></asp:Label>
                    </div>
                
                    </div>
                     <div class="row">
               





          

                    <asp:GridView id="dtgProgrammi1" runat="server" Width="100%"  ToolTip="Progetti nel Programma" ShowHeaderWhenEmpty="true" 
				Caption="Progetti nel Programma" AllowPaging="true" CellPadding="2" Font-Size="Small"  CssClass="table"  
                AllowSorting="false" AutoGenerateColumns="true" UseAccessibleHeader="True" Visible="true"  >
	                <FooterStyle></FooterStyle>
		            <SelectedRowStyle Font-Bold="true" BackColor="White"></SelectedRowStyle>
		            <EditRowStyle></EditRowStyle>
		            <AlternatingRowStyle  CssClass="tr"></AlternatingRowStyle>
		            <RowStyle CssClass="tr"   HorizontalAlign="Center"  ></RowStyle>
		            <HeaderStyle></HeaderStyle>
                    <%--<PagerStyle CssClass="linkPageGrid"  />   adc  asp:GridView se serve paginare qui funziona e mettere il pagesize sopra
                  <PagerSettings  Mode="NumericFirstLast" Visible="true"  pagebuttoncount="100" position="Bottom"></PagerSettings >--%>
	            </asp:GridView>

                </div>
               </div>
         </fieldset>
 
    </p>
    
    <div class="row" >
        <div class="collable" style="width:150px" >
            <asp:label id="lblA" runat="server" Visible="True" >Stato Graduatoria:</asp:label>
        </div>
        <div class="colOggetti"  style="width:150px">
            <asp:label id="lblStatoGraduatoria" CssClass="bold" runat="server" ></asp:label>
        </div>
        <div class="collable" style="width:150px">&nbsp;
          <asp:label id="lblVisConfermataDa" runat="server">Confermata da:</asp:label>
        </div>
        <div class="colOggetti" style="width:150px"> &nbsp;               
            <asp:label id="lblConfermataDa" CssClass="bold" runat="server"></asp:label>
        </div>
        <div class="collable" style="width:150px"> 
            <asp:label id="lblb" runat="server" Visible="true">Nr. Vol. Prog.:</asp:label>
            <asp:label  id="lblNVolRichiesti" CssClass="bold" runat="server" >0</asp:label>
        </div>
        <div class="colOggetti" style="width:140px">
            <asp:label id="lblc"  runat="server" Visible="true" >Nr. Vol. Sel.:</asp:label>
            <asp:label  id="lblVolSel" CssClass="bold" runat="server" >0</asp:label>
        </div>
    </div>
    <div class="row" >
        <div class="collable" style="width:150px">
            <asp:Label ID="Label2" runat="server" Text="Inizio Servizio Previsto"></asp:Label>
        </div>
        <div class="colOggetti" style="width:150px">
            <asp:Label ID="Label3" runat="server" Text="Fine Servizio Previsto"></asp:Label>
        </div>
        <div class="collable" style="width:150px">
            <asp:Label ID="Label1" AssociatedControlID="txtinizioEff" runat="server" Text="Inizio Servizio Effettivo"></asp:Label>         
        </div>
        <div class="colOggetti" style="width:150px">
            <asp:Label ID="Label4" runat="server" AssociatedControlID="txtFineEff"  Text="Fine Servizio Effettivo"></asp:Label>
        </div>
        <div class="collable" style="width:160px"> &nbsp;
            <asp:ImageButton ID="imgSegnalaOk"  AlternateText="Segnala Progetto" ToolTip="Segnala Progetto" runat="server" 
                 ImageUrl="images/vistabuona_small.png" />
           <asp:label id="lblSegnalazione" runat="server">Segnala Progetto</asp:label>
        </div>      
    </div>
     

    <div class="row" >
        <div class="collable" style="width:150px">&nbsp;
            <asp:label id="lblinPrev" CssClass="bold" runat="server" ></asp:label>
        </div>
        <div class="colOggetti" style="width:150px">&nbsp;
            <asp:label id="lblFinePre" CssClass="bold" runat="server" ></asp:label>
        </div>
        <div class="collable" style="width:150px">
            <asp:TextBox ID="txtinizioEff"  onkeypress="javascript: onkeypressTxt();"   ToolTip="Inizio Effettivo"  runat="server" MaxLength="10" Width="76px"></asp:TextBox>
        </div>
        <div class="colOggetti" style="width:150px">
            <asp:TextBox ID="txtFineEff" runat="server" MaxLength="10" onkeypress="javascript: onkeypressTxt();"  ToolTip="Fine Effettiva" ReadOnly="True" Width="76px"></asp:TextBox>
        </div>
        <div class="collable" style="width:160px">&nbsp;
         <asp:ImageButton ID="ImgPartenzaDiff" AlternateText="Partenza Differita" 
                ToolTip="Partenza Differita" runat="server" Visible="false" 
                 ImageUrl="images/proroga_small.png" style="height: 30px" />
                 <asp:label id="lblPartenzaDifferita" runat="server" Visible="false" >Partenza Differita</asp:label>
            <asp:ImageButton ID="imgRecuperoOk" AlternateText="Recupero Posti" ToolTip="Recupero Posti" runat="server" 
                 ImageUrl="images/vistabuona_small.png" />
            <asp:label id="lblRecupero" runat="server"  >Recupero Posti</asp:label>
        </div>
    </div>


    <div class="row" id="PartDiff" runat="server" Visible="false">
   <fieldset class="ContornoPagina">
   <legend>Partenza Differita </legend>
      <div class="collable" style="width:15%">
          <asp:Label ID="lblInizioServizioSede" runat="server" Text="Inizio Servizio Sede"></asp:Label>
      </div>
      <div class="colOggetti" style="width:15%">
         <asp:TextBox ID="txtInizioServizioSede" runat="server"></asp:TextBox>
     </div>
      <div class="collable" style="width:15%">
          <asp:Label ID="lblFineServizioSede" runat="server" Text="Fine Servizio Sede"></asp:Label>
      </div>
      <div class="colOggetti" style="width:15%">
         <asp:TextBox ID="txtFineServizioSede" runat="server" Enabled="false"></asp:TextBox>
     </div>
      <div class="colOggetti" style="width:15%">
     <asp:Button ID="cmdAnnullaDifferita" CssClass="Pulsante" text="Annulla" runat="server" BorderStyle="Outset" ToolTip="Annulla" />
     </div>
   </fieldset>
     </div>
  
     <div class="row" style="border-bottom-color:#3a4f63;border-bottom-style:inset;">
        <div class="collable" style="width:150px">&nbsp;</div>
        <div class="colOggetti" style="width:150px">&nbsp;</div>
        <div class="collable" style="width:150px">&nbsp;</div>
        <div class="colOggetti" style="width:150px">&nbsp;
            <asp:HiddenField ID="piker" runat="server" />
        </div>
        <div class="collable" style="width:160px">&nbsp;
           <asp:ImageButton ID="imgConfermaRecupero" AlternateText="Conferma Recupero" ToolTip="Conferma Recupero" runat="server" ImageUrl="images/save.gif" Visible="False" />
           <asp:textbox id="txtInizioRecupero" onkeypress="javascript: onkeypressTxt();"   runat="server" Width="76px" MaxLength="10" Visible="False"></asp:textbox>
        </div>
        <div class="colOggetti" style="width:140px">
            <asp:label id="lblConfermaRecupero" AssociatedControlID="txtInizioRecupero" runat="server"  Visible="False">Data Avvio Recupero</asp:label>
        </div>
    </div>
     <div  class="row">
        <div class="collable" style="width:140px">
            <asp:Label ID="Label5" runat="server" Text="Sede Graduatoria"></asp:Label>
        </div>
        <div id="divSede" class="colOggetti" style="width:420px; float: left;" runat="server" >    
                <asp:label id="lblSedeFisica" CssClass="bold" runat="server"></asp:label>                     
          </div>  
          <div  style=" float: left;">
           <asp:ImageButton ID="imgAvvisoSede"  
                AlternateText="Richiesta modifica sede" runat="server" 
                ToolTip="Richiesta modifica sede" ImageUrl="images/alert_xsmall.png" 
                Visible="false" />
         </div>
       
    </div>
     <div class="row" >
   
        <div class="collable" style="width:140px">
           
            <asp:Label ID="Label6" AssociatedControlID="lblIndirizzo" runat="server" Text="Indirizzo"></asp:Label>
         </div>
        <div class="colOggetti" style="width:420px">
          
           <asp:label id="lblIndirizzo" AssociatedControlID="Label6" CssClass="bold" runat="server" ></asp:label></div>
       
     
    </div>
     <div class="row" >
   
        <div class="collable" style="width:140px">
           
            <asp:Label ID="Label7" AssociatedControlID="lblComune" runat="server" Text="Comune"></asp:Label>
         </div>
        <div class="colOggetti" style="width:420px">
          
            <asp:label id="lblComune" AssociatedControlID="Label7" CssClass="bold" runat="server" ></asp:label></div>

    </div>
     <div class="row" >
   
        <div class="collable" style="width:140px">
           
            <asp:Label ID="Label8" AssociatedControlID="lbltelefono" runat="server" Text="Telefono"></asp:Label>
         </div>
        <div class="colOggetti" style="width:140px">
        
			<asp:label id="lbltelefono" AssociatedControlID="Label8" CssClass="bold" runat="server" ></asp:label></div>
       
       
    </div>
    
     
      <%-- <div class="row" id="RigaNote" runat="server" style="border-bottom-color:#3a4f63;width:98%;">
       <div class="collable" style="width:140px" ><asp:Label ID="" AssociatedControlID="txtNote" runat="server" Text="Note"></asp:Label> &nbsp;&nbsp;&nbsp;<asp:LinkButton ID=""  runat="server">Crea Nota</asp:LinkButton> </div>
        <div class="colOggetti" style="width:420px"><asp:textbox id="" runat="server" Width="420px" ReadOnly="True" MaxLength="200" TextMode="MultiLine"></asp:textbox></div> &nbsp;&nbsp;&nbsp;
        <div class="colOggetti" style="width:20%;text-align:center;"><asp:LinkButton ID="" Text="Vedi tutte le Note" runat="server"></asp:LinkButton></div>
       </div> --%> 
       <%--  <div class="row" id="RigaNoteIns"  visible="false"  runat="server">
      <asp:Label ID="lblinsNota" AssociatedControlID="txtNuovaNota"  runat="server" Text="Inserisci Nota"></asp:Label> 
       <div class="collable" style="width:400px" >
            <asp:textbox id=""  runat="server" Width="365px"  MaxLength="200" TextMode="MultiLine"></asp:textbox>
         </div>
        <div class="colOggetti" style="width:160px" >
          &nbsp;&nbsp;
        
         
       &nbsp;</div>--%>





        <div class="row">
            <div class="collable" style="width:15%">
              <asp:Label ID="lblNote"  runat="server" Text="Altre informazioni" AssociatedControlID="txtNote" />
            </div>
            <div class="colOggetti" style="width:50%">
                <asp:TextBox ID="txtNote" CssClass="textbox" runat="server"  Width="50%" TextMode="MultiLine" Enabled="False" />
             
            </div>
            <div class="colOggetti" style="width:15%">
               <div class="colHyperLink" style="width:100%">
                        <asp:LinkButton ID="cmdInsNote" runat="server" style="cursor: pointer" ToolTip="Inserimento note" Text="Inserisci Nota" ></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        
                    </div>
            </div>

            <div class="colOggetti" style="width:15%">
            <div class="colHyperLink" style="width:100%">
            <asp:LinkButton ID="cmdStoricoNote" Text="Visualizza Note" runat="server"></asp:LinkButton>
            </div>
            </div>

        </div>

        <div id="RigaNoteIns" style="border:0px;display:table;width:100%;" runat="server" visible="false">
               <div class="row">
                    <div class="collable" style="width:15%">
                        <asp:Label ID="lblNuovaNota" CssClass="label"  runat="server" Text="Nota" AssociatedControlID="txtNuovaNota" />
                    </div>
                    <div class="colOggetti" style="width:28%">
                        <asp:textbox id="txtNuovaNota" CssClass="textbox" runat="server" Width="90%" TextMode="MultiLine"></asp:textbox>&nbsp;&nbsp;
                     </div>   
                    <div class="colOggetti" style="width:40%">
                         <asp:button id="CmdSalvaNota" runat="server" CssClass="Pulsante" Text="Aggiungi Nota" ToolTip="Aggiungi Nota" ></asp:button>
                    </div>
                </div>
         </div>

        <div  id="RigaGrigliaNote" runat="server" visible="false">
                <fieldset class="ContornoPagina"  >
       <legend>Storico Note Volontario</legend>
                <asp:datagrid id="dtgStoricoNote" Visible="true" runat="server" CssClass="table" 
													ToolTip="Elenco Ricerca Sedi" Width="100%" AllowPaging="True" AutoGenerateColumns="false" AllowSorting="True"  CellPadding="0" 
													PageSize="3">
													<FooterStyle  ></FooterStyle>
													<SelectedItemStyle BackColor="White"></SelectedItemStyle>
													<EditItemStyle ></EditItemStyle>
													<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
													<ItemStyle CssClass="tr"></ItemStyle>
													<HeaderStyle
														Height="30px" 
														></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="IdCronologiaNoteGraduatoria" HeaderText="IdNotaGraduatoria"></asp:BoundColumn>
														<asp:BoundColumn DataField="UserNameNota" HeaderText="UserName">
															<HeaderStyle></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="DataNota" HeaderText="Data Nota">
															<HeaderStyle ></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="NoteGraduatoria" HeaderText="Nota Graduatoria">
                                                        <HeaderStyle ></HeaderStyle>
                                                        </asp:BoundColumn>
														<asp:ButtonColumn Text="&lt;img src='images/canc_small.png' title='Rimuovi Sede' border=0&gt;"
															HeaderText="Rimuovi" CommandName="Rimuovi">
                                                            <HeaderStyle Width="30px"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
														</asp:ButtonColumn>
													</Columns>
													 <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
												</asp:datagrid>
             </fieldset>  
         </div> 
      </div>
      


       
     


      <br/>
   
    
    <div class="RigaPulsanti" style="margin-right:5px">
         
            <asp:Button ID="cmdAggiungi" runat="server" text="Aggiungi Volontario" 
                CssClass="Pulsante" ToolTip="Aggiungi Volontario" Visible="False" />
         <asp:button id="CmdFCGG" Text="Forza controllo GG" CssClass="Pulsante" runat="server" Visible="False"></asp:button>
             <asp:button id="imgAnnullaConferma" Text="Annulla Conferma" CssClass="Pulsante" runat="server" Visible="False"></asp:button>
             <asp:Button ID="imgCancellaGrad" runat="server" text="Cancella" 
                CssClass="Pulsante" OnClientClick="return CancellaGraduatoria();"  
                ToolTip="Elimina graduatoria" Visible="False" />
             <asp:Button ID="cmdModificaUNSC" text="Modifica" CssClass="Pulsante" runat="server"  ToolTip="Modifica Graduatoria" Visible="False" />
             <asp:Button ID="cmdConferma" runat="server" text="Presenta" CssClass="Pulsante"  ToolTip="Presenta Graduatoria" Visible="False" />
             <asp:Button ID="cmdSalva" runat="server" text="Conferma"  CssClass="Pulsante" ToolTip="Salva" />
             <asp:Button ID="cmdRespingi" runat="server" text="Respingi" CssClass="Pulsante" ToolTip="Respingi Graduatoria" Visible="False" />
             <asp:Button ID="cmdChiudi" CssClass="Pulsante" text="Chiudi" runat="server" BorderStyle="Outset" ToolTip="Chiudi" />
       </div>
    
   

<h3>
Elenco Graduatoria Volontari
</h3>
<div >
													<asp:label id="lblMessaggioAlert" runat="server" Visible="true" CssClass="msgErroreGrid"  Width="463px"
														 ></asp:label>
                                                         <% if Session("TipoUtente") = "U" then %>
													<asp:label id="LblAssociaProtocollo"  runat="server" 
														>Associa Protocollo a Fascicoli</asp:label>
													<asp:imagebutton id="imgAssociaProtocollo" AlternateText="Associa Protocollo" runat="server" ImageUrl="images/File2.png" Width="30px"
														Height="30px" ToolTip="Associa Protocollo a Fascicoli"></asp:imagebutton>
                                                        <% end if %>
</div>
<div class="RigaVuota">
 <asp:HyperLink ID="hlDownload"  runat="server"  Visible="false"></asp:HyperLink>
&nbsp;</div>
<asp:datagrid id="dgRisultatoRicerca"  runat="server" Width="99%"   ToolTip="Associa Volontari"  CssClass="table" UseAccessibleHeader="True"
											  CellPadding="0" AllowSorting="True"  AutoGenerateColumns="False">
											<FooterStyle ></FooterStyle>
											<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
											<EditItemStyle ></EditItemStyle>
											<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
											<ItemStyle CssClass="tr"></ItemStyle>
											<HeaderStyle ></HeaderStyle>
											<Columns>
												<asp:BoundColumn DataField="img" HeaderText="Selez.">
													<ItemStyle CssClass="tr"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn  HeaderText="N&#176;">
													<HeaderStyle  Width="3%"></HeaderStyle>
													<ItemStyle ></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="ordine"  HeaderText="ordine"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="idgraduatoriaentit&#224;" HeaderText="idGraduatoria"></asp:BoundColumn>
												<asp:BoundColumn DataField="nominativo" HeaderText="Nominativo">
													<ItemStyle ></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="CodiceFiscale" HeaderText="Cod. Fiscale"></asp:BoundColumn>
												<asp:BoundColumn DataField="datanascita" HeaderText="Data Nascita" DataFormatString="{0:d}">
													<HeaderStyle ></HeaderStyle>
													<ItemStyle Wrap="False" ></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="Comune" HeaderText="Comune Nascita">
													<ItemStyle ></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="telefono" HeaderText="Telefono">
													<ItemStyle></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn Visible="False" HeaderImageUrl="email" HeaderText="Email">
													<ItemStyle ></ItemStyle>
												</asp:BoundColumn>
												<asp:TemplateColumn>
													<HeaderStyle ></HeaderStyle>
													<ItemStyle ></ItemStyle>
													<HeaderTemplate>
														Idoneo
													</HeaderTemplate>
													<ItemTemplate>
														<asp:CheckBox id="check1" Text="Idoneo"  onclick="Modificato();" runat="server" AutoPostBack="false" ToolTip="Volontario Idoneo"></asp:CheckBox>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn>
													<HeaderStyle ></HeaderStyle>
													<ItemStyle ></ItemStyle>
													<HeaderTemplate>
														Selezionato
													</HeaderTemplate>
													<ItemTemplate>
														<asp:CheckBox id="check2" onclick="Modificato();" Text="Selezionato"  runat="server" AutoPostBack="false" ToolTip="Volontario Selezionato"></asp:CheckBox>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn>
													<HeaderStyle ></HeaderStyle>
													<ItemStyle ></ItemStyle>
													<HeaderTemplate>
														Punti
													</HeaderTemplate>
													<ItemTemplate>
														<asp:TextBox id="Text1" runat="server" Width="45px"   MaxLength="5" ToolTip="Punteggio" text="" onblur="Modificato();"
															onkeypress="ControlloAlfabetico(id.keyascii,id);"></asp:TextBox>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn>
													<HeaderStyle ></HeaderStyle>
													<ItemStyle ></ItemStyle>
													<HeaderTemplate>
														Tipo Posto
													</HeaderTemplate>
													<ItemTemplate>
														<asp:DropDownList id="DDL1" ToolTip="Tipo Posto" runat="server" onclick="Modificato();" Width="100px"></asp:DropDownList>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:BoundColumn Visible="False" DataField="amm" HeaderText="amm"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="ido"  HeaderText="ido"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="Punteggio" HeaderText="valPunteggio"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="tipoposto" HeaderText="tipoPosto"></asp:BoundColumn>
												<asp:ButtonColumn Text="&lt;img src=images/canc_small.png style=cursor:pointer; title='Elimina Associazione Volontario' border=0&gt;"
													CommandName="Cancella"></asp:ButtonColumn>
												<asp:BoundColumn DataField="Valutazione" HeaderText="Valut.">
													<ItemStyle ></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="identit&#224;" HeaderText="identit&#224;"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="eta" HeaderText="eta"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="tmpidsedeattuazione" HeaderText="tmpidsedeattuazione"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="codfis"   HeaderText="Verifica">
													<HeaderStyle ></HeaderStyle>
													<ItemStyle></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="identit&#224;" HeaderText="identit&#224;"></asp:BoundColumn>
												<asp:BoundColumn DataField="codiceSede" ItemStyle-HorizontalAlign="Right" HeaderText="Cod. Sede">
													<ItemStyle ></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="Info" HeaderText="Info"></asp:BoundColumn>
												<asp:BoundColumn Visible="False"  DataField="CancellaV"  HeaderText="Canc">
                                                
                                                </asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="statoentit&#224;" HeaderText="statoentita"></asp:BoundColumn>
												<asp:ButtonColumn Text="&lt;img src='images/DocModelli.png' width='20px' height='20px' style='cursor: pointer;' alt='Associazione Documenti Volontario'  /&gt;"
													CommandName="Documenti"></asp:ButtonColumn>
												<asp:BoundColumn Visible="False" DataField="AmmessoRecupero" HeaderText="AmmessoRecupero"></asp:BoundColumn>
											    <asp:BoundColumn DataField="GMO" Visible="False"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="FAMI" Visible="False"></asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="Pdf Domanda Online" ItemStyle-HorizontalAlign="Center"> 
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgPdf"   style="cursor:pointer;"   CommandName="selezionaPdf" Height="30px" Width="30px" AlternateText="Scarica Pdf" ToolTip="Scarica PDF" runat="server" ImageURL="images/pdf.png" CausesValidation="false"></asp:ImageButton>      
                                                        <asp:HyperLink ID="hlDownload1"  runat="server"  Visible="false"></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                    
                                                <asp:BoundColumn Visible="False" DataField="Dol_Id" HeaderText="pdf">
                                                    <ItemStyle ></ItemStyle>
                                                </asp:BoundColumn>

											</Columns>
											<PagerStyle NextPageText="&gt;&gt;" Height="15px"
												 PrevPageText="&lt;&lt;"
												BackColor="#6699FF" Mode="NumericPages"></PagerStyle>
										</asp:datagrid>

                                        <input id="hdd_CountGriglia" size="1" type="hidden" name="hdd_CountGriglia" runat="server"/>
				                        <input id="txtIdVolCanc" type="hidden" name="txtIdVolCanc"/> <input id="txtStatoVol" size="1" type="hidden" name="txtStatoVol" runat="server"/>
    <br />
    LEGENDA:


      <div id="rigaLegenda"  runat="server" class="wrapper" 
        style="border-bottom-color:#3a4f63;border-bottom-style:inset;height:22px;">
        
   
        <div class="collable" style="width:140px">
           
         
       <img  alt="Da verificare" src="images/DataDaVerificare.jpg" width="20"/><asp:Label 
                ID="Label11" runat="server" Text="Da verificare"></asp:Label>
        </div>

        <div class="collable" style="width:140px">
          
           
          
       <img  src="images/DoppiaDomanda.jpg" alt="Anomalia" width="20"/><asp:Label ID="Label12" 
                runat="server" Text="Anomalia"></asp:Label>
        </div>

        
    </div>
  
  </fieldset>
  </div>
<input type="hidden" id="strAbilitaPulsante" name="strAbilitaPulsante" runat="server" />
<input type="hidden" id="strAbilitaDiv" name="strAbilitaDiv" runat="server" />
<asp:HiddenField ID="hdnIsPostbackProgetto1" runat="server" />
  
</asp:Content>
