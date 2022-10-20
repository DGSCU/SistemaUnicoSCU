<%@ Page Title="Ricerca Sedi Attuazione" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmElencoSediAttuazione.aspx.vb" Inherits="Futuro.WfrmElencoSediAttuazione" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script  type="text/javascript">
     /* <![CDATA[ */
        var formId;
        var prefissoIdClient = "MainContent_";
        var width = screen.width/1.5;
        var height = screen.height/1.5;
        var x = screen.width/2 - width;
        var y = screen.height/2 - height;
        var winOption = "dependent=no,scrollbars=yes,status=no,resizable=yes,width="+width+",height="+height+",top="+y+",left="+x

	    
	    function ImpostaFormId() {
	        formId = document.forms[0].id;
	        return formId;
	  }
        function CostruisciId(IdServer) {
           var IdClient = prefissoIdClient + IdServer
           return IdClient
        }
            /* ]]> */
    </script>
    <script type="text/javascript">
        function CancellaAssociazione() {
            var messaggio_conferma = "True";
            msg = window.confirm("Con questa operazione verranno cancellati definitivamente tutti i volontari delle graduatoria!Procedere ugualmente?");
            if (msg == false) {
                messaggio_conferma = "False";
            }
            document.getElementById('<%= confermaOperazione.ClientID %>').value = messaggio_conferma;
        };


        function ProrogaProgetto() {
            var messaggio_conferma = "True";
            msg = window.confirm("Si sta' abilitando il Progetto per il caricamento delle graduatorie volontari oltre i termini previsti per 7 giorni da oggi! Si desidera continuare?");
            if (msg == false) {
                messaggio_conferma = "False";
            }
            document.getElementById('<%= confermaOperazione.ClientID %>').value = messaggio_conferma;
        };

        function ProrogaEnte() {
            var messaggio_conferma = "True";
            msg = window.confirm("Si sta' abilitando l'Ente per il caricamento delle graduatorie volontari oltre i termini previsti per 7 giorni da oggi! Si desidera continuare?");
            if (msg == false) {
                messaggio_conferma = "False";
            }
            document.getElementById('<%= confermaOperazione.ClientID %>').value = messaggio_conferma;
        };

        function SalvaProtocollo() {
            var messaggio_conferma = "True";
            var numeroProtocollo = document.getElementById(CostruisciId("txtNumProt"));
            if (numeroProtocollo.value == '') {
                alert("E' necessario indicare il protocollo.")
                CostruisciId("txtNumProt").focus();
                messaggio_conferma = "False";
            }
            document.getElementById('<%= confermaOperazione.ClientID %>').value = messaggio_conferma;
        }

        function CancellaProtocolliAssociati(){
        	    var messaggio_conferma = "True";
            var codiceFascicolo = document.getElementById(CostruisciId("TxtCodiceFasc")).value;
			if (codiceFascicolo!='') {
				msg=confirm("Con questa operazione verranno cancellati definitivamente tutti protocolli associati! Procedere ugualmente?")
				if(msg==false){
				    messaggio_conferma = "False";
				}
			}
        document.getElementById('<%= confermaOperazione.ClientID %>').value = messaggio_conferma;
        };

    </script>
	<script  type="text/javascript">
    /* <![CDATA[ */
	    function SelProtocollo(NumProt, DataProt) {
	        formId = ImpostaFormId();
	        var IdNumProt = CostruisciId(NumProt);
	        var IdDataProt = CostruisciId(DataProt);
	        var codiceFascicolo = document.getElementById(CostruisciId("TxtCodiceFasc")).value;
	        if (document.getElementById(CostruisciId(NumProt)).style.backgroundColor != 'lightgrey')
				{
				    if (codiceFascicolo == '') {
		            alert("Specificare il numero fascicolo!");
		        } else {
		            myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?objForm=" + formId + "&TxtProt=" + IdNumProt + "&TxtData=" + IdDataProt + "&NumeroFascicolo=" + codiceFascicolo, +"Visualizza", winOption);
		        }	
            }

		}

	    function VisualizzaSanzione(IDEnteSedeAttuazione, IdEnte) {
	        window.open("WfrmSedeSanzionata.aspx?IDEnteSedeAttuazione=" + IDEnteSedeAttuazione + "&IdEnte=" + IdEnte, "SedeSanzionata", winOption);
	    };

        function ElencoFascicoli(TxtCodiceFasc,TxtCodiceFascicolo,txtDescFasc) {
        var messaggio_conferma = true;
        var IdTxtCodiceFasc = CostruisciId(TxtCodiceFasc);
        var IdTxtCodiceFascicolo = CostruisciId(TxtCodiceFascicolo);
        var IdDescrizioneFascicolo = CostruisciId(txtDescFasc);
        var codiceFascicolo = document.getElementById(IdTxtCodiceFasc).value;
        formId = ImpostaFormId();
		if(codiceFascicolo!=''){
			msg=confirm("Con questa operazione verranno cancellati definitivamente tutti protocolli associati! Procedere con l'operazione")
			if(msg==false){
				messaggio_conferma = false;
			}else{
				myWin = window.open("WfrmSIGEDElencoFascicoli.aspx?Processo=VOLONTARI&objForm=" + formId + "&objNumero="+IdTxtCodiceFascicolo+"&objCodice="+IdTxtCodiceFasc+"&objDescFasc="+IdDescrizioneFascicolo+"&Varsalva=1&VArUpdate=2&IdFascicolo=" + codiceFascicolo + "&IdAttivita=<%=Request.QueryString("IdAttivita")%>", "Visualizza", winOption);		
			}
		}else{
			myWin = window.open("WfrmSIGEDElencoFascicoli.aspx?Processo=VOLONTARI&objForm=" + formId + "&objNumero="+IdTxtCodiceFascicolo+"&objCodice="+IdTxtCodiceFasc+"&objDescFasc="+IdDescrizioneFascicolo+"&Varsalva=1&VArUpdate=2&IdFascicolo=" + codiceFascicolo + "&IdAttivita=<%=Request.QueryString("IdAttivita")%>", "Visualizza",winOption );		
		}	
        return messaggio_conferma;
        }
	    function Associa(IdEnteSede,IDAttivitasedeAssegnazione,idAttivita,presenta){
             var codiceFascicolo = document.getElementById(CostruisciId("TxtCodiceFasc")).value;
			if ('<%=Session("TipoUtente")%>'=='U'){
                var varTxtCodiceFascicolo = document.getElementById(CostruisciId("TxtCodiceFascicolo")).value;
                var descrizioneFascicolo = document.getElementById(CostruisciId("txtDescFasc")).value;
               document.location.href="WfrmAssociaVolontari.aspx?CheckIndietro=<%=Request.QueryString("CheckIndietro")%>&Ente=<%=Request.QueryString("Ente")%>&CodEnte=<%=Request.QueryString("CodEnte")%>&Progetto=<%=Request.QueryString("Progetto")%>&Bando=<%=Request.QueryString("Bando")%>&Settore=<%=Request.QueryString("Settore")%>&Area=<%=Request.QueryString("Area")%>&InAttesa=<%=Request.QueryString("InAttesa")%>&CodiceProgetto=<%=Request.QueryString("CodiceProgetto")%>&PaginaGrid=<%=Request.QueryString("PaginaGrid")%>&IdEnteSede=" + IdEnteSede + "&IdAttivitasedeAssegnazione=" + IDAttivitasedeAssegnazione + "&presenta=" + presenta + "&IdAttivita=" + idAttivita + "&CodiceFascicolo=" + varTxtCodiceFascicolo + "&DescFascicolo=" + descrizioneFascicolo + "&NumeroFascicolo=" + codiceFascicolo
			}
			else {
                document.location.href="WfrmAssociaVolontari.aspx?CheckIndietro=<%=Request.QueryString("CheckIndietro")%>&Ente=<%=Request.QueryString("Ente")%>&CodEnte=<%=Request.QueryString("CodEnte")%>&Progetto=<%=Request.QueryString("Progetto")%>&Bando=<%=Request.QueryString("Bando")%>&Settore=<%=Request.QueryString("Settore")%>&Area=<%=Request.QueryString("Area")%>&InAttesa=<%=Request.QueryString("InAttesa")%>&CodiceProgetto=<%=Request.QueryString("CodiceProgetto")%>&PaginaGrid=<%=Request.QueryString("PaginaGrid")%>&IdEnteSede=" + IdEnteSede + "&IdAttivitasedeAssegnazione=" + IDAttivitasedeAssegnazione + "&presenta=" + presenta + "&IdAttivita=" + idAttivita
			}
		
	    };
	function Presenta(IdEnteSede,IDAttivitasedeAssegnazione,idAttivita,presenta){
             var codiceFascicolo = document.getElementById(CostruisciId("TxtCodiceFasc")).value;
			if ('<%=Session("TipoUtente")%>'=='U'){
             var varTxtCodiceFascicolo = document.getElementById(CostruisciId("TxtCodiceFascicolo")).value;
             var descrizioneFascicolo = document.getElementById(CostruisciId("txtDescFasc")).value;
				document.location.href="WfrmAssociaVolontari.aspx?CheckIndietro=<%=Request.QueryString("CheckIndietro")%>&Ente=<%=Request.QueryString("Ente")%>&CodEnte=<%=Request.QueryString("CodEnte")%>&Progetto=<%=Request.QueryString("Progetto")%>&Bando=<%=Request.QueryString("Bando")%>&Settore=<%=Request.QueryString("Settore")%>&Area=<%=Request.QueryString("Area")%>&InAttesa=<%=Request.QueryString("InAttesa")%>&CodiceProgetto=<%=Request.QueryString("CodiceProgetto")%>&PaginaGrid=<%=Request.QueryString("PaginaGrid")%>&IdEnteSede=" + IdEnteSede + "&IdAttivitasedeAssegnazione=" + IDAttivitasedeAssegnazione + "&presenta=" + presenta + "&IdAttivita=" + idAttivita + "&CodiceFascicolo=" + varTxtCodiceFascicolo + "&DescFascicolo=" + descrizioneFascicolo + "&NumeroFascicolo=" + codiceFascicolo
			}
			else {
				document.location.href="WfrmAssociaVolontari.aspx?CheckIndietro=<%=Request.QueryString("CheckIndietro")%>&Ente=<%=Request.QueryString("Ente")%>&CodEnte=<%=Request.QueryString("CodEnte")%>&Progetto=<%=Request.QueryString("Progetto")%>&Bando=<%=Request.QueryString("Bando")%>&Settore=<%=Request.QueryString("Settore")%>&Area=<%=Request.QueryString("Area")%>&InAttesa=<%=Request.QueryString("InAttesa")%>&CodiceProgetto=<%=Request.QueryString("CodiceProgetto")%>&PaginaGrid=<%=Request.QueryString("PaginaGrid")%>&IdEnteSede=" + IdEnteSede + "&IdAttivitasedeAssegnazione=" + IDAttivitasedeAssegnazione + "&presenta=" + presenta + "&IdAttivita=" + idAttivita
			}
	};
		
	function SoloNumeri(){
		var evtobj=window.event;
		var keyascii=evtobj.keyCode;
		if (keyascii>57 || keyascii<48){
			window.event.keyCode=0;
		}else{
			window.event.keyCode=keyascii;
		}
	};
	    function ElencoDocumenti0(TxtCodiceFasc) {
        var varTxtCodiceFascicolo = CostruisciId("TxtCodiceFascicolo");
        var codiceFascicolo =  document.getElementById(CostruisciId(TxtCodiceFasc)).value; 
        if (codiceFascicolo ==''){
				varTxtCodiceFascicolo.focus();
			}else{
				myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?NumeroFascicolo=" + codiceFascicolo, "Visualizza", winOption);			
			}
			return false;
        };
 

   

    /* ]]> */
	</script>
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <fieldset class="ContornoPagina">
<legend >Ricerca sedi graduatoria</legend>
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:Label ID="Label5" runat="server"  Text="Ricerca Sedi Attuazione: "></asp:Label>
      <asp:label  id="lblProgetto" style="vertical-align:middle" Text="&nbsp;" runat="server"></asp:label>
      </h2>
    </div>
  
    <div class="rowGroup" style="height:auto">
        <div class="RigaVuota">
      &nbsp;</div>
         <asp:label id="LblErroreSediGraduatoria"  runat="server" CssClass="msgErrore" ></asp:label>
        <asp:label id="LblInfoSediGraduatoria"  runat="server" CssClass="msgConferma" ></asp:label>
        <asp:HiddenField id="confermaOperazione" Value="False" runat="server"  />
        <div class="RigaVuota">
      &nbsp;</div>
    </div>

    <div class="row" >
    
        <div class="colOggetti" style="width:15%">
            <asp:Label ID="IdLblComune" CssClass="label" AssociatedControlID="TxTComune"  runat="server" Text="Comune"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%" >
            <asp:TextBox ID="TxTComune" CssClass="textbox" autofocus="true" runat="server"></asp:TextBox>
        </div>
 
         <div class="collable" style="width:15%">
           <asp:Label ID="IdLblIndirizzo" CssClass="label" AssociatedControlID="TxTIndirizzo" runat="server" Text="Indirizzo"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">

           <asp:TextBox  ID="TxTIndirizzo" CssClass="textbox"  runat="server"></asp:TextBox>
        
       </div>
    </div>

    <div class="row" >
    
       <div class="collable" style="width:15%">
           <asp:Label ID="IdLblSedeAttuazione" CssClass="label"  AssociatedControlID="TxTCodSedAtt" onkeypress="javascript: SoloNumeri();"  runat="server" Text="Cod. Sede Attuazione"></asp:Label>
          
       </div>
       <div class="colOggetti" style="width:35%">           
           <asp:TextBox ID="TxTCodSedAtt" CssClass="textbox" runat="server"></asp:TextBox>
       </div>
      <div class="collable" style="width:15%" >
            <asp:Label ID="IdlblSanzione" CssClass="label" AssociatedControlID="ddlSegnalazioneSanzione" Visible="false" runat="server" Text="Presenza Sanzione" ></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:DropDownList ID="ddlSegnalazioneSanzione" CssClass="ddlClass" Visible="false" Width="55px" runat="server"  AutoPostBack="true">
                <asp:ListItem Selected="True">Tutti</asp:ListItem>
				<asp:ListItem Value="0">No</asp:ListItem>
				<asp:ListItem Value="1">Si</asp:ListItem>
              </asp:DropDownList>
       </div>
    </div>

    <div class="RigaPulsanti">
      <asp:Button ID="ProrogaEnte" OnClientClick="ProrogaEnte()" CssClass="Pulsante" runat="server" 
                Text="Proroga Ente" ToolTip="Proroga Ente" Visible="False" />
      <asp:Button ID="ProrogaProgetto" OnClientClick="ProrogaProgetto()" CssClass="Pulsante" runat="server"  AutoPostBack="True"
                Text="Proroga Progetto" ToolTip="Proroga Progetto" Visible="False" />
      <asp:Button ID="imgCancellaGrad" CssClass="Pulsante"  OnClientClick="return CancellaAssociazione();" 
              runat="server" Text="Elimina Graduatorie" ToolTip="Elimina graduatoria" Visible="True" 
              />
  </div>
  <div class="RigaVuota">&nbsp;
  </div>
    <div class="RigaPulsanti">
        <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />
        <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server"  Text="Chiudi"  />

   </div>
    <div class="RigaVuota">
        <input id="TxtCodiceFasc" type="hidden"  runat="server"  />
    </div>
   
</div>




 <div id="DivUtenteUNSC_RigheNascoste" class="rowGroup" runat="server"  >
<fieldset class="ContornoPagina" >
<legend >Collegamento Protocollo</legend>

<div class="wrapper" style="border:0px;width:100%;padding:0" >

    <div class= "row" style="height:auto" >
        <asp:label id="txtMSG" runat="server" Text="&nbsp;" CssClass="msgInfo" ></asp:label>
        <asp:label id="lblmessaggiosopra" Text="&nbsp;" runat="server" CssClass="msgErrore" ></asp:label>
        <asp:label id="lblmessaggio" Text="&nbsp;" runat="server" CssClass="msgConferma" ></asp:label>
        <input type="hidden" id="txtIdAttivita" runat="server" />
    </div>
    <div id="Fascicolo_RigheNascoste" class="rowGroup" runat="server" >
         <div class= "row"  >
               <div class="collable" style="width:30%" >
                   <asp:Label ID="LblNumFascicolo" CssClass="label" AssociatedControlID="TxtCodiceFascicolo" runat="server" Text="Numero Fascicolo" Visible="False"></asp:Label>
               </div>
               <div class="colOggetti" style="width:55%" >
                   <asp:TextBox ID="TxtCodiceFascicolo" CssClass="textbox"  runat="server" Visible="False"></asp:TextBox>
               </div>
                <div class="collable" style="width:15%" >
                    <asp:ImageButton style="CURSOR: hand;" id="cmdSelFascicolo" OnClientClick="return ElencoFascicoli('TxtCodiceFasc','TxtCodiceFascicolo','txtDescFasc');" runat="server" ImageUrl="images/cartella_small.png"
											        ToolTip="Seleziona Codice Fascicolo" AlternateText="Seleziona Codice Fascicolo" Visible="false"></asp:ImageButton>

                    <asp:ImageButton style="CURSOR: hand;" id="cmdSelProtocollo0" OnClientClick="ElencoDocumenti0('TxtCodiceFasc');" runat="server" ImageUrl="images/busta_small.png"
											        ToolTip="Elenco Documenti" AlternateText="Elenco Documenti" Visible="false"></asp:ImageButton>

                    <asp:ImageButton style="CURSOR: hand;" OnClientClick="CancellaProtocolliAssociati();" id="cmdFascCanc" runat="server" ImageUrl="images/canc_small.png"
											        ToolTip="Rimuovi Fascicolo/Protocolli" AlternateText="Rimuovi Fascicolo/Protocolli" Visible="false"></asp:ImageButton>

            </div>
        </div>
        <div class= "row"  >
            <div class="collable" style="width:30%" >
               <asp:Label id="LblDescrFascicolo" CssClass="label" AssociatedControlID="txtDescFasc"  Visible="false"   runat="server" Text="Descrizione"></asp:Label>
            </div>
   
           <div class="colOggetti" style="width:55%" >
                  <asp:TextBox ID="txtDescFasc" Visible="false" CssClass="textbox"  runat="server"
                  TextMode="MultiLine"></asp:TextBox>

           </div>
        </div>
        <div class="RigaPulsanti" >
             <asp:Button ID="ImgSalva" CssClass="Pulsante"  runat="server" Text="Salva Fascicolo" ToolTip="Salva Fascicolo" Visible="false" />
        </div>
     </div>
    <div id="ProtocolliAssociati_RigheNascoste" class="rowGroup" runat="server" >
    
  
        <div class="row">
            <div class="collable" style="width:50%">
              <fieldset class="fieldsetDatagrid">
              <h3>
                <asp:label id="lblTesto" runat="server"  AssociatedControlID="dtgElencoProt" Font-Bold="true" Visible="False"  Text="Protocolli associati"/>
                </h3>
            <asp:datagrid id="dtgElencoProt"   CssClass="table" runat="server" CellPadding="1"  Caption="Protocolli associati" CaptionAlign="Top"
                    ToolTip="Elenco esiti requisiti"  PageSize = "2"   AllowPaging="true"
		        AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True"
                        Visible="true" ShowFooter="false" Width="90%">
			        <FooterStyle></FooterStyle>
		            <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		            <EditItemStyle></EditItemStyle>
		            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		            <ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		            <HeaderStyle ></HeaderStyle>
                        <Columns>
					        <asp:BoundColumn Visible="False" DataField="idAttivit&#224;" HeaderText="idAttivit&#224;">
						        <HeaderStyle ></HeaderStyle>
						        <ItemStyle Width="0%"  ></ItemStyle>
					        </asp:BoundColumn>
					        <asp:BoundColumn DataField="NProt" HeaderText="Num. Protocollo">
						        <HeaderStyle Width="45%" ></HeaderStyle>
                                <ItemStyle  ></ItemStyle>
					        </asp:BoundColumn>
					        <asp:BoundColumn DataField="DataProt" HeaderText="Data Protocollo">
						        <HeaderStyle></HeaderStyle>
						        <ItemStyle Width="45%" ></ItemStyle>
					        </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Azione" > 
                                <ItemTemplate>
                                <asp:ImageButton ID="ImageButton1" CommandName="Cancella" ToolTip="Cancella Protocollo Associato" AlternateText="Cancella Protocollo Associato" runat="server" ImageURL="images/canc_small.png" CausesValidation="false">
                                </asp:ImageButton>
                                </ItemTemplate>
                                </asp:TemplateColumn>
				        </Columns>
						        <PagerStyle NextPageText="&gt;&gt;"   
		        PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
			        </asp:datagrid>
            </fieldset>
            </div>
        </div>

        <div class="collable" style="width:15%" >
            <asp:label id="LblNumProt" AssociatedControlID="txtNumProt" CssClass="label"  Text="N° Protocollo"  runat="server" Visible="false" ></asp:label>
        </div>
        <div class="colOggetti" style="width:35%" >
                <asp:textbox id="txtNumProt" CssClass="textbox" runat="server"  Visible="false" ></asp:textbox>
            </div>
        <div class="collable" style="width:15%" >
        <asp:label id="LblDataProt" CssClass="label"  AssociatedControlID="txtDataProt"  runat="server" Visible="false" Text="Data Protocollo"></asp:label>
    </div>
        <div class="colOggetti" style="width:30%" >
		    <asp:textbox id="txtDataProt" CssClass="textbox" runat="server"    Visible="false" ></asp:textbox>
	    </div>
        <div class="collable" style="width:5%"> 
            <asp:ImageButton style="CURSOR: hand" id= "ImgSellProtollo" runat="server" ImageUrl="images/busta_small.png" ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo" 
            Visible="false" OnClientClick="javascript: SelProtocollo('txtNumProt','txtDataProt');" >
            </asp:ImageButton>
            </div>
    
        <div class="RigaPulsanti">
                <asp:Button  id="imgSalvaProt"  OnClientClick="SalvaProtocollo();" CssClass="Pulsante" runat="server" Text="Salva Protocollo" ToolTip="Salva Protocollo"
										    Visible="False"></asp:Button>
        </div>
    </div>
</div>
</fieldset>   
</div>
 <fieldset class="fieldsetDatagrid">

  <asp:datagrid id="dgRisultatoRicerca" Caption="Elenco Graduatorie" CaptionAlign="Top"  runat="server" Width="100%" ToolTip="Elenco Ricerca Sedi Progetto" 
                   AllowPaging="true" PageSize="5"  CssClass="table" AllowSorting="True" 
                   AutoGenerateColumns="False" UseAccessibleHeader="True" Visible="true">
	<FooterStyle></FooterStyle>
	<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	<EditItemStyle></EditItemStyle>
	<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	<ItemStyle CssClass="tr" HorizontalAlign ="Center"></ItemStyle>
	<HeaderStyle></HeaderStyle>
	<Columns>
        <asp:BoundColumn DataField="img" Visible="false">
							<HeaderStyle Width="0px"></HeaderStyle>
						</asp:BoundColumn>
		<asp:BoundColumn DataField="SedeAssegnazione" HeaderText="Graduatorie">
				<HeaderStyle Width="20%" ></HeaderStyle>
			<ItemStyle Width="20%"  ></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="Indirizzo" HeaderText="Indirizzo">
			<HeaderStyle Width="10%" ></HeaderStyle>
			<ItemStyle Width="10%"  ></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="Comune" HeaderText="Comune">
				<HeaderStyle Width="5%" ></HeaderStyle>
			<ItemStyle Width="5%"  ></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="StatoGraduatoria" HeaderText="Stato Graduatoria">
				<HeaderStyle Width="5%" ></HeaderStyle>
			<ItemStyle Width="5%"  ></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn Visible="False" HeaderText="Sede Attuazione">
			<HeaderStyle Width="5%" ></HeaderStyle>
			<ItemStyle Width="5%"  ></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="NumVolIns"  HeaderText="N&#176;. Vol. Inseriti">
				<HeaderStyle Width="5%" ></HeaderStyle>
			<ItemStyle Width="5%"  ></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="NumVolIdo" HeaderText="N&#176;. Vol. Assegnati">
			<HeaderStyle Width="5%" ></HeaderStyle>
			<ItemStyle Width="5%"  ></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn Visible="False" HeaderText="Riferimento"  >
				<HeaderStyle Width="5%" ></HeaderStyle>
			<ItemStyle Width="5%"  ></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="Associa" HeaderText="Associazione Volontari">
				<HeaderStyle Width="5%" ></HeaderStyle>
			<ItemStyle Width="5%"  ></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="idEntesede" HeaderText="idSede">
        <HeaderStyle Width="5%" ></HeaderStyle>
			<ItemStyle Width="5%"  ></ItemStyle>
        </asp:BoundColumn>
		<asp:BoundColumn Visible="False" HeaderText="idSedeAtt">
        <HeaderStyle Width="5%" ></HeaderStyle>
			<ItemStyle Width="5%"  ></ItemStyle>
        </asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="idAtti" HeaderText="idAtti">
        <HeaderStyle Width="5%" ></HeaderStyle>
			<ItemStyle Width="5%"  ></ItemStyle>
        </asp:BoundColumn>
		<asp:BoundColumn DataField="presenta" HeaderText="Presentazione Graduatoria">
				<HeaderStyle Width="5%" ></HeaderStyle>
			<ItemStyle Width="5%"  ></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="VOLPREVISTI" HeaderText="N&#176; Vol Previsti">
			<HeaderStyle Width="5%" ></HeaderStyle>
			<ItemStyle Width="5%"  ></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="Conferma" HeaderText="Conferma Graduatoria">
				<HeaderStyle Width="5%" ></HeaderStyle>
			<ItemStyle Width="5%"  ></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="Segnalazione" HeaderText="Presenza Sanzione">
				<HeaderStyle Width="5%" ></HeaderStyle>
			<ItemStyle Width="5%"  ></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="verifica" HeaderText="Presenza Verifica">
				<HeaderStyle Width="5%" ></HeaderStyle>
			<ItemStyle Width="5%"  ></ItemStyle>
		</asp:BoundColumn>
	</Columns>
		<PagerStyle NextPageText="&gt;&gt;"   
PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
</asp:datagrid>
</fieldset>
</fieldset>
</asp:Content>