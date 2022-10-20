<%@ Page Title="Lista Sedi Fuori Adeguamento" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmListaSediFuoriAdeguamento.aspx.vb" Inherits="Futuro.WfrmListaSediFuoriAdeguamento" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
        <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css"/>
        <script src="Scripts/JHelper.js" type="text/javascript"></script>
        <script src="https://polyfill.io/v3/polyfill.min.js?features=default" type="text/javascript"></script>


     <script  type="text/javascript">

         var prefissoIdClient = "MainContent_";

         function CostruisciId(IdServer) {
             var IdClient = prefissoIdClient + IdServer;
             return IdClient;
         };


         function TogliCheck() {
             var idchkSelDesel = CostruisciId("chkSelDesel");
             var chkSelDesel = document.getElementById(idchkSelDesel);
             var idHDText = CostruisciId("hdPerSelezione");
             var myHdText = document.getElementById(idHDText);
             chkSelDesel.checked = false;
             myHdText.value="blocca"
             __doPostBack("hdPerSelezione", ""); 
            
             
             

         }

         function pageLoad() {

             var ShowPopUPControllo = '<%=ShowPopUPControllo%>';
                 if (ShowPopUPControllo == '1') {
                     $find('popUpControl1').show();
                 }
                 if (ShowPopUPControllo == '2') {
                     $find('popUpControl2').show();
                 }


         }

     </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
<div class="firefox">

<fieldset class="ContornoPaginaScroll" style="width:auto;overflow:auto;">

        <legend>Modifiche fuori adeguamento sedi</legend>
      
        <div class="wrapper" style="width:100%">
        
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Modifiche fuori adeguamento Sedi"></asp:Label></h2>
            </div>
                 <div class="rowGroup" style="height:auto">
        <div class="RigaVuota">

      &nbsp;
      </div>
         <asp:label id="lblErrore"  runat="server" CssClass="msgErrore" ></asp:label>
        <div class="RigaVuota">
      &nbsp;</div>
    </div>
            <div class="RigaVuota" >
            &nbsp;
            </div>
            <div class="row" id="divSede" runat="server">
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDenominazioneEnte" CssClass="label" AssociatedControlID="txtDenominazioneEnte" runat="server" Text="Den. Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtDenominazioneEnte"  CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodiceEnte" CssClass="label" AssociatedControlID="TxtCodiceRegione" runat="server" Text="Cod. Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="TxtCodiceRegione" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
               <div class="row"  runat="server">
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDenominazione" CssClass="label" AssociatedControlID="txtdenominazione" runat="server" Text="Denominazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtdenominazione"  CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
<!-- 
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblStato" CssClass="label" AssociatedControlID="ddlstato" runat="server" Text="Stato"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList id="ddlstato"  runat="server" CssClass="ddlClass"></asp:DropDownList>        
                </div>
    -->
            </div>
            <div class="row"  runat="server">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblRegione" CssClass="label" AssociatedControlID="txtregione" runat="server" Text="Regione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtregione" CssClass="textbox" runat="server"></asp:TextBox>         
                </div> 
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblStatoUtenza" CssClass="label" AssociatedControlID="ddlTipologia" runat="server" Text="Tipologia"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlTipologia" runat="server">
                    </asp:DropDownList>
                </div> 
            </div>
            <div class="row"  runat="server">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblComune" CssClass="label" AssociatedControlID="txtComune" runat="server" Text="Comune"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtComune" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblProvincia" CssClass="label" AssociatedControlID="txtProvincia" runat="server" Text="Provincia"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtProvincia" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            
            <div  class="row"  >
                    <div class="collable" style="width:15%">      
                    <asp:Label ID="lblIndirizzo" CssClass="label" AssociatedControlID="txtIndirizzo" runat="server" Text="Indirizzo"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtIndirizzo" CssClass="textbox" runat="server"></asp:TextBox>      
                 </div>    
<!--
                <div id="divCertificazione" runat="server" visible="false" >
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lbCertificazione" CssClass="label" AssociatedControlID="ddlCertificazione" runat="server" Text="Certificazione"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:DropDownList ID="ddlCertificazione" runat="server" CssClass="ddlClass">
                            <asp:ListItem Selected="True">Tutti</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>
                            <asp:ListItem Value="1">Si</asp:ListItem>
                               <asp:ListItem Value="2">Da Valutare</asp:ListItem>
                        </asp:DropDownList>
                    </div> 
                 </div>
-->
            </div>
            <div  class="row"  >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodSedeAtt" CssClass="label" AssociatedControlID="txtCodSedeAtt" runat="server" Text="Cod. Sede Attuazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodSedeAtt" CssClass="textbox" runat="server"></asp:TextBox>         
                 </div>
<!--
                <div class="collable" style="width:15%" visible="false">      
                    <asp:Label ID="lblDuplicati" CssClass="label" AssociatedControlID="ddlDuplicati" runat="server" Text="Indirizzi Duplicati"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%" visible="false">       
                    <asp:DropDownList ID="ddlDuplicati" runat="server" CssClass="ddlClass">
                   <asp:ListItem Selected="True">Tutti</asp:ListItem>
				    <asp:ListItem Value="0">Non Duplicati</asp:ListItem>
					<asp:ListItem Value="1">Solo Duplicati</asp:ListItem>
                    </asp:DropDownList>
                </div> 
-->
            </div>
              <div  class="row"  >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblLocalizzazione" CssClass="label" AssociatedControlID="ddlLocalizzazione" runat="server" Text="Localizzazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlLocalizzazione" runat="server" CssClass="ddlClass">
                        <asp:ListItem Selected="True">Tutti</asp:ListItem>
						<asp:ListItem Value="1">Italiana</asp:ListItem>
						<asp:ListItem Value="0">Estera</asp:ListItem>
                    </asp:DropDownList>
                </div> 
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblVisto" CssClass="label" AssociatedControlID="ddlVisto" runat="server" Text="Visto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlVisto" runat="server" CssClass="ddlClass">
                        <asp:ListItem Selected="True">TUTTI</asp:ListItem>
				        <asp:ListItem Value="0">Da Vistare</asp:ListItem>
					    <asp:ListItem Value="1">Vistato</asp:ListItem>
                    </asp:DropDownList>
                </div>    
<!--
                <div id="divForzaSanzione" runat="server" visible="false">
                   <div class="collable" style="width:15%">      
                        <asp:Label ID="lblSanzione" CssClass="label" AssociatedControlID="ddlSegnalazioneSanzione" runat="server" Text="Presenza Sanzione"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlSegnalazioneSanzione" runat="server" CssClass="ddlClass">
                       <asp:ListItem Selected="True">Tutti</asp:ListItem>
								<asp:ListItem Value="0">No</asp:ListItem>
								<asp:ListItem Value="1">Si</asp:ListItem>
                    </asp:DropDownList>
                    </div> 
                </div>
-->
              </div>
              <div  class="row"  >
<!--
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblRichiestaVariazione" CssClass="label" AssociatedControlID="ddlRichistaVariazione" runat="server" Text="Richiesta Variazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%" visible="false">       
                    <asp:DropDownList ID="ddlRichistaVariazione" runat="server" CssClass="ddlClass">
                        <asp:ListItem Text="Tutti" Value=""></asp:ListItem>
						<asp:ListItem Text="Variazione richieste" Value="1"></asp:ListItem>
						<asp:ListItem Text="Senza variazione" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
-->
               
               </div>
                <div  class="row"  >
                <div class="colOggetti" style="width:50%">  
                <asp:CheckBox ID="chkRiferimentoRimborsi" Text="Sede di riferimento per rimborsi" Visible="False" runat="server"/>     
                </div> 
                   <div class="colOggetti" style="width:50%">  
                <asp:CheckBox ID="chkIncludi" Text="Includi tutte le sedi" Visible="False" runat="server"/>     
                </div> 
                </div>

 
            <div class="RigaPulsanti" >
            
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
                <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
		        <br />
	            <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
              &nbsp;
            </div>
    </div>
  
        <br />
           <div class="row">
                <div class="colOggetti" style="width:50%">
                    <asp:checkbox id="chkSelDesel" runat="server" Text="Seleziona tutto per la validazione" Visible="False" AutoPostBack="True" ></asp:checkbox>
                </div>
            </div>   
    
            <div class="RigaPulsanti" style="text-align:right">
                <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" Text="Conferma Validazione" Visible="false"/>
            </div>
            <div class="row">
            <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" Caption="Risultato Ricerca Sedi Ente"
                    ToolTip="Elenco Sedi" CssClass="table"  AllowPaging="True" 
                    AllowSorting="True" AutoGenerateColumns="False" 
                    UseAccessibleHeader="True">
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		        <HeaderStyle></HeaderStyle>
		        <Columns>
                    <asp:TemplateColumn HeaderText="Selez."> 
                        <ItemTemplate>
	                     <asp:ImageButton ID="IdImgSelSedi" style="cursor:pointer;" CommandName="Select" alt="Seleziona Sede" ToolTip='Seleziona Sede' runat="server" ImageURL="images/sedi_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                    </asp:TemplateColumn>
					<asp:BoundColumn DataField="DataVariazione" HeaderText="Data e Ora" DataFormatString="{0:dd'/'MM'/'yyyy HH':'mm}">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="CodiceEnte" HeaderText="Codice Ente"></asp:BoundColumn>
					<asp:BoundColumn DataField="Ente" HeaderText="Ente"></asp:BoundColumn>
					<asp:BoundColumn DataField="Sede" HeaderText="Sede"></asp:BoundColumn>
					<asp:BoundColumn DataField="Variazioni" HeaderText="Variazioni"></asp:BoundColumn>
                    <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Center" Width="0%"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<HeaderTemplate><strong>Visto</strong></HeaderTemplate>
					  <ItemTemplate>
					   <asp:CheckBox id="chkSedi" runat="server" checked='<%# DataBinder.Eval(Container, "DataItem.FlgValidazione") %>' Text="&nbsp;" ToolTip="Variazioni Validate" ></asp:CheckBox>
					  </ItemTemplate>
					</asp:TemplateColumn>	
                    <asp:BoundColumn Visible="False" DataField="FlgValidazione" HeaderText="FlgValidazione"></asp:BoundColumn>
                    <asp:BoundColumn Visible="False" DataField="idEnteSede" HeaderText="idSede"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="idente" HeaderText="idEnte"></asp:BoundColumn>
			    	<asp:BoundColumn Visible="False" DataField="IdVariazioneFuoriAdeguamento" HeaderText="IdVariazioneFuoriAdeguamento"></asp:BoundColumn>
    		        <asp:BoundColumn Visible="False" DataField="StatoEnteSede" HeaderText="StatoEnteSede"></asp:BoundColumn>
                </Columns>
		    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
	    </asp:datagrid>
        </div>
        <asp:HiddenField ID="hdd_Check" runat="server" />
       <asp:HiddenField ID="txtModifica" runat="server" />
       <asp:HiddenField ID="txtstrsql" runat="server"  />
       <asp:HiddenField ID="lblpage" runat="server"  />
       <asp:HiddenField ID="txtRicerca" runat="server"  />
       <asp:HiddenField ID="txtCodice" runat="server"  />
       <asp:HiddenField ID="hdPerSelezione" runat="server" />

    </fieldset>

</div>
            <asp:ModalPopupExtender runat="server" ID="popSeleziona" TargetControlID="hfPopup" 
                PopupControlID="pnlSeleziona" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
                PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpControl1" DynamicServicePath="" Enabled="True" CancelControlID="btnChiudiSeleziona"> 
            </asp:ModalPopupExtender>
	        <asp:Panel ID="pnlSeleziona" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
                <asp:HiddenField runat="server" ID="hfPopup" />
                <p class="msgInfo">Seleziona/Deseleziona tutte le variazioni</p>
                <asp:label id="lblMsgSeleziona"  runat="server" CssClass="msgInfo" Visible="True" AssociatedControlID="cmdSeleziona"></asp:label>
                
                <fieldset>
			        <asp:Button ID="cmdSeleziona" runat="server" CssClass="Pulsante" Text="Procedi"/>
                    <asp:Button ID="btnChiudiSeleziona" runat="server" CssClass="Pulsante" Text="Chiudi"/>
                </fieldset>
	        </asp:Panel>

    			<asp:ModalPopupExtender runat="server" ID="popSalva" TargetControlID="hfPopupSalva" 
                PopupControlID="pnlSalva" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
                PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpControl2" DynamicServicePath="" Enabled="True" CancelControlID="btnChiudiSalva"> 
            </asp:ModalPopupExtender>
	        <asp:Panel ID="pnlSalva" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
                <asp:HiddenField runat="server" ID="hfPopupSalva" />
                <p class="msgInfo">Stai per validare i dati selezionati <br/>Sicuro di voler proseguire con il salvataggio dei dati?<bbr/></p>
               <fieldset>
			        <asp:Button ID="cmdSalvaDati" runat="server" CssClass="Pulsante" Text="Procedi"/>
                    <asp:Button ID="btnChiudiSalva" runat="server" CssClass="Pulsante" Text="Chiudi"/>
                </fieldset>
	        </asp:Panel>
</asp:Content>
