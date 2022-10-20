<%@ Page Title="Gestione Vincoli Sezione Ente" Language="vb" AutoEventWireup="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Site.Master" CodeBehind="WfrmAlbero.aspx.vb" Inherits="Futuro.WfrmAlbero" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <style type="text/css">
strong { 
    font-weight: bold;
}
</style>
<style type="text/css">
em 
{
    font-style:italic;
}
</style>
<script type="text/javascript">
var stampaReportConDocumenti = '<%=hf_StampaCoperinaConDocumenti.Value%>';
var stampaReportSenzaDocumenti = '<%=hf_StampaCoperinaSenzaDocumenti.Value%>';
if(stampaReportConDocumenti == 1){
ApriPopUpCopertinaConDocumenti();
};
if(stampaReportSenzaDocumenti == 1){
ApriPopUpCopertinaSenzaDocumenti();
};
function ApriPopUpCopertinaConDocumenti() {
    var idEnteFase = '<%=hf_IdEnteFase.Value%>';
		     window.open("WfrmReportistica.aspx?sTipoStampa=39&IDEnteFase=<%=hf_IdEnteFase.Value%>","Reportistica","width=800,height=800,toolbar=no,location=no,menubar=no,scrollbars=yes,resizable=yes");
          
		};
     function ApriPopUpCopertinaSenzaDocumenti() {
		     window.open("WfrmReportistica.aspx?sTipoStampa=40&IDEnteFase=<%=hf_IdEnteFase.Value%>","Reportistica","width=800,height=800,toolbar=no,location=no,menubar=no,scrollbars=yes,resizable=yes");
          
		};
</script>
<script type="text/javascript">
           //<![CDATA[
            function ApriPopUpControlliAggiuntiviPresentazione() {
		     window.open("WfrmVerificaCompletamentoAccreditamento.aspx?Id=<%=Session("IdEnte")%>","Accreditamento","width=650,height=400,toolbar=no,location=no,menubar=no,scrollbars=yes");

		};
    
      
        function ApriPopUpAmbitiTerritoriali() {
		     window.open("WFrmAssociaEntiComuni.aspx?Id=<%=Session("IdEnte")%>&tipoAmbito=<%=hddAmbito.Value%>&tipoSettore=<%=hddAmbito.Value%>","Comuni","width=700,height=450,toolbar=no,location=no,menubar=no,scrollbars=yes,resizable=yes");
          
		};
        
       function ApriPopUpSediSovradimensionate() {
		     window.open("WFrmSediSovradimensionate.aspx?Id=<%=Session("IdEnte")%>","SediSovradimensionate","width=700,height=400,toolbar=no,location=no,menubar=no,scrollbars=yes,resizable=yes");
          
		};
//          function ApriPopUpSediCondivise() {
//		     window.open("WFrmSediCondivise.aspx?Id=<%=Session("IdEnte")%>","SediSovradimensionate","width=850,height=400,toolbar=no,location=no,menubar=no,scrollbars=yes,resizable=yes");
//          
//		};
            //]]>
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

    <fieldset class="ContornoPagina" id="FidsetPagina" runat="server">
        <legend><asp:Label ID="lblNomeFrameset" runat="server" AssociatedControlID="FidsetPagina" Text=""></asp:Label></legend>
        <div class="wrapper" style="width:100%;border:0px">
            <div id="codiciAssegnati" class="rowGroup" style="height:auto" runat="server" visible="false">
                <fieldset class="ContornoPagina">
                <legend>Codici Assegnati</legend>
                    <div class="wrapper" style="width:100%;border:0px">
                        <div class="row" style="height:auto">
                            <h3><asp:Label runat="server" ID="labelCodiceEnte" CssClass="labelDati" Text="Gentile Ente, La informiamo che il codice a Lei assegnato e':"></asp:Label></h3>
                        </div>
                        <div class="row" style="height:auto;text-align:center">
                            <h1><asp:Label runat="server" ID="LblCodice" CssClass="msgConferma" ></asp:Label></h1>
                        </div>
                        <div class="row" style="height:auto">
                            <h3><asp:Label runat="server" ID="label1" CssClass="labelDati" Text="Tutta la documentazione prevista deve essere inviata via PEC all'ufficio competente:"></asp:Label></h3>
                        </div>
                        <div class="row" style="height:auto;text-align:center">
                            <h1><asp:Label runat="server" ID="LblRegione" CssClass="msgConferma"></asp:Label></h1>
                        </div>
                    </div>
                </fieldset>
            </div>
            <div id="Div1" class="rowGroup" style="height:auto" runat="server" visible="false">
                <fieldset class="ContornoPagina">
                <legend>Fase Art.2</legend>
                    <div class="wrapper" style="width:100%;border:0px">
                        <div class="row" style="height:auto">
                            <h3><asp:Label runat="server" ID="label2" CssClass="labelDati" Text="Gentile Ente, la Fase Art.2 è stata presentata"></asp:Label></h3>
                        </div>
                    </div>
                </fieldset>
            </div>
            <div id="divChiusuraEnte" class="rowGroup" style="height:auto" runat="server" visible="false">
                <fieldset class="ContornoPagina">
                <legend>Chiudi Ente</legend>
                    <div class="wrapper" style="width:100%;border:0px">
                        <div class="row" style="height:auto">
                            <h3><asp:Label runat="server" ID="lblChiudiEnte" CssClass="labelDati"></asp:Label></h3>
                        </div>
                        <br />
                        <div class="RigaPulsanti" style="height:auto;text-align:center">
                            <asp:Button ID="cmdConfermaChiusura" CssClass="Pulsante" runat="server" ToolTip="Conferma di chiusura Ente" Text="Conferma" />
                            <asp:Button ID="cmdAnnulla" CssClass="Pulsante" runat="server" ToolTip="Annulla richiesta chiusura" Text="Annulla" />
                        </div>
                    </div>
                </fieldset>
            </div>
           <p style="text-align:right">
           <asp:ImageButton id="CmdInforScu2" runat="server" ImageUrl="Images/vincoli_small.png" ToolTip="Analisi Codice Fiscale" AlternateText="Analisi Codice Fiscale" ImageAlign="Middle"/>
           </p>
    <div class="row" style="height:auto">
        <asp:Label ID="LblStato" CssClass="msgInfo" runat="server"></asp:Label><br />
        <asp:Label ID="lblmess" CssClass="msgInfo" runat="server"></asp:Label><br />
        <asp:Label ID="msgErrore" CssClass="msgErrore" runat="server"></asp:Label>
        <asp:Label ID="msgInfo" CssClass="msgInfo" runat="server"></asp:Label>
        <asp:Label ID="msgConferma" CssClass="msgConferma" runat="server"></asp:Label>
    </div>

    <div class="row" style="height:auto">
    <div  style="width:100%;display:table-column;float:left;vertical-align:middle;">
    <div style="width:50%;display:table-column;float:left;vertical-align:middle;">
         <fieldset class="ContornoPagina" id="fsOperativita" runat="server">
            <legend>Gestione Documenti</legend>
            <div class="wrapper" style="width:100%;border:0px">
                <div class="rowGroup" style="height:auto"> 
                    <div class="row" style="height:auto">                  
                        <div class="colHyperLink" style="width:50%">
                            <asp:LinkButton ID="ImgVariazioneEnte" runat="server" style="cursor:pointer" Text="Variazioni Ente" ></asp:LinkButton>
                        </div>
                     <%--   <div class="colHyperLink" style="width:50%">
                            <asp:LinkButton ID="imgsediduplicate" runat="server" style="cursor:pointer" Text="Sedi Condivise Con Altri Enti" Visible="False" OnClientClick="javascript:ApriPopUpSediCondivise();" ></asp:LinkButton>
                        </div>--%>
                      </div>
                <div class="row" style="height:auto">                  
                <div class="colHyperLink" style="width:50%">
                    <asp:LinkButton ID="imgConsultaDoc" runat="server" style="cursor:pointer" Text="Consulta Documenti Ente" ></asp:LinkButton>
                    
                </div>
                <div class="colHyperLink" style="width:50%">
                    <asp:LinkButton ID="imgSediSovradimensionate" runat="server" style="cursor:pointer" Text="Sedi Sovradimensionate" Visible="False" OnClientClick="javascript:ApriPopUpSediSovradimensionate()"></asp:LinkButton>
                </div>
                </div>
                <div class="row" style="height:auto">                  
                <div class="colHyperLink" style="width:50%">
                    <asp:LinkButton ID="imgElencoDocumentiEnti" runat="server" style="cursor:pointer" Text="Inserisci Documenti Ente" Visible="False" ></asp:LinkButton>
                </div>
                <div class="colHyperLink" style="width:50%">
                    <asp:LinkButton ID="imgsettori" runat="server" style="cursor:pointer" Text="Gestione Settori Ente" Visible="False" ></asp:LinkButton>
                </div>
                </div>
                <div class="row" style="height:auto">                  
                <div class="colHyperLink" style="width:50%">
                    <asp:LinkButton ID="imgVisErrori" runat="server" style="cursor:pointer" Text="Controlli Aggiuntivi Sulla Presentazione" Visible="False" OnClientClick="javascript:ApriPopUpControlliAggiuntiviPresentazione()"></asp:LinkButton>
                </div>
                <div class="colHyperLink" style="width:50%">
                    <asp:LinkButton ID="imbComuni" runat="server" style="cursor:pointer" Text="Ambito Territoriale" Visible="False" OnClientClick="javascript:ApriPopUpAmbitiTerritoriali()" ></asp:LinkButton>
                </div>
                </div>
                <div class="row" style="height:auto">                  
                    <div class="colHyperLink" style="width:50%">
                        <asp:LinkButton ID="imgStampaCopertina" runat="server" style="cursor:pointer" Text="Copertina Ultima Fase Chiusa" Visible="False" ></asp:LinkButton>
                    </div>
                    <div class="colHyperLink" style="width:50%">
                    <asp:LinkButton ID="imgAnagraficaSistemi" runat="server" style="cursor:pointer" 
                            Text="Gestione Sistemi" Visible="true" ></asp:LinkButton>
                </div>
                
                
                </div>
                 <div class="row" style="height:auto">                  
                    <div class="colHyperLink" style="width:50%">
                        <asp:LinkButton ID="LinkAggiornaSediArt2" runat="server" style="cursor:pointer" Text="Aggiorna Sedi Art2" Visible="False" ></asp:LinkButton>
                    </div>
                    <div class="colHyperLink" style="width:50%">
                    
                </div>
                </div>
               </div>


             </div>
        </fieldset>
    </div>
    <div style="width:50%;display:table-column;float:left;vertical-align:middle;">
         <fieldset class="ContornoPagina" id="fsFasi" runat="server">
            <legend>Gestione Fasi</legend>
            <div class="wrapper" style="width:100%;border:0px">
 
              <div class="RigaPulsanti" >    

                <asp:Button ID="ImgPresentazioneDomanda" CssClass="Pulsante" runat="server" Visible="false"  Text="Presenta"  />
                <asp:Button ID="ImgPresentaDocArt2" CssClass="Pulsante" runat="server" Visible="false"  Text="Presenta Doc Art.2"  />
                <asp:Button ID="ImgPresentaDocArt10" CssClass="Pulsante" runat="server" Visible="false"  Text="Presenta Doc Art.10"  />
                <asp:Button ID="ImgAbilitaAdeguamento" CssClass="Pulsante" runat="server" Visible="false"  Text="Inizio Adeguamento"  />
                 <asp:Button ID="ImgAnnullaInizioAdeguamento" CssClass="Pulsante" runat="server" Visible="false"  Text="Annulla Inizio Adeguamento"  />   
                 
              </div>
                    <div class="RigaPulsanti" >    
                         <asp:Button ID="ImgCompleta" CssClass="Pulsante" runat="server" Visible="false"  Text="Valutazione Completa"  />              
                         <asp:Button ID="Imgaccredita" CssClass="Pulsante" runat="server" Visible="false"  Text="Iscrivi"  />
                         <asp:Button ID="cmdChiudiEnte" CssClass="Pulsante" runat="server" Visible="false" ToolTip="Annulla Iscrizione"  Text="Chiudi Ente"  />
                             <asp:Button ID="ImgInizioArt2" CssClass="Pulsante" runat="server" Visible="false"  Text="Inizio Art.2"  />
                         <asp:Button ID="ImgInizioArt10" CssClass="Pulsante" runat="server" Visible="false"  Text="Inizio Art.10"  />                
                         <asp:Button ID="imgApplicaRiserva" CssClass="Pulsante" runat="server" Visible="false"  Text="Applica Riserva"  />   
                         <asp:Button ID="imgTogliRiserva" CssClass="Pulsante" runat="server" Visible="false"  Text="Togli Riserva"  />   
                
                <asp:Button ID="btnRiserva" CssClass="Pulsante" runat="server" Visible="false"  Text="Riserva"  />
                
               
                
                </div>
 
       <div class="RigaPulsanti" >    
         <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server"  Text="Chiudi"  />
       </div>

        <div id="DivAccreditaRiserva" class="row"  runat="server" visible="false" >
                <fieldset class="ContornoPagina" id="RiservaSiNo" runat="server">
                <legend>Seleziona/Deseleziona Causali Riserva</legend>
                
                
                <asp:datagrid id="dtgCausaliRiserva" runat="server" CssClass="table"  Width="100%" ToolTip="Elenco Causali Riserva"  CellPadding="2"  AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
		        <SelectedItemStyle BackColor="White"></SelectedItemStyle>
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		        <HeaderStyle></HeaderStyle>
		        <Columns>
                    
			        <asp:BoundColumn Visible="False" DataField="IDCausale" HeaderText=""></asp:BoundColumn>
                    <asp:BoundColumn Visible="False" DataField="IdEnteCausaleRiserva" HeaderText=""></asp:BoundColumn>
			        <asp:BoundColumn  DataField="Descrizione" HeaderText=" Causali Riserva">
                        <ItemStyle HorizontalAlign="Left"/>
                    </asp:BoundColumn>
			        <asp:TemplateColumn HeaderText="Sel.">
				        <HeaderStyle Width="20%" ></HeaderStyle>
				        <ItemTemplate>
					        <asp:CheckBox id="chkSeleziona" Text="Sel" TextAlign="Left" toolTip="Seleziona  Causali Riserva" runat="server"></asp:CheckBox>
				        </ItemTemplate>
			        </asp:TemplateColumn>
		        </Columns>
	        </asp:datagrid>
                
                <div style="float:left;width:40%" ></div> <div style="text-align:right"><asp:Button ID="BtnAssegna"  CssClass="Pulsante" runat="server" Visible="false"  Text="Applica"  /><asp:Button ID="CmdAnnullaRiservaSiNo"  CssClass="Pulsante" runat="server"  Text="Chiudi"  /></div>

             </fieldset>
             </div>
            </div> 
        </fieldset>
    </div>
    </div>
   
   <%-- <div  style="width:0%;display:table-column;float:left;vertical-align:middle;">&nbsp;
    </div>--%>
    <div  style="width:100%;display:table-column;float:left;vertical-align:middle;">

         <fieldset  class="ContornoPagina" >
            <legend>Sintesi Requisiti</legend>
            <div class="row " >  
            <div class="collable" style="width:100%">
            <asp:Label runat="server" ID="lblMessaggio" CssClass="msgInfo"></asp:Label>
            </div>
            </div>
            <div class="wrapper" style="width:100%;border:0px">
            <div class="collable" style="width:100%;"   >
	        <asp:TreeView id="TrwVincoli" CssClass="Treeview"  Target="_parent" runat="server" NodeWrap="true"  >
               <LevelStyles >
          <asp:TreeNodeStyle 
            Font-Bold="true" 
            Font-Size="11pt" 
            />
          <asp:TreeNodeStyle 
            Font-Bold="true" 
            Font-Size="9pt"/>
          <asp:TreeNodeStyle 
            Font-Size="9pt"/>
          <asp:TreeNodeStyle 
            Font-Size="9pt"/>
        </LevelStyles>
            </asp:TreeView>
        </div>
            </div>

            </fieldset>
    </div>

    </div>
     </div>
<asp:HiddenField runat="server" ID="txtCodiceRegione"/>
<asp:HiddenField runat="server" ID="hf_idEntePadre"/>
<asp:HiddenField runat="server" ID="hddAmbito"/>
<asp:HiddenField runat="server" ID="hddTipologia"/>
<asp:HiddenField runat="server" ID="txtclasse"/>
<asp:HiddenField runat="server" ID="txtstato"/>
<asp:HiddenField runat="server" ID="hf_AdeguamentoOk"/>
<asp:HiddenField runat="server" ID="hf_ClasseAccreditamentoSubIudice"/>
<asp:HiddenField runat="server" ID="txtCodiceNazionale"/>
<asp:HiddenField runat="server" ID="hf_NodeData" />
<asp:HiddenField runat="server" ID="hf_IdEnteFase" />
<asp:HiddenField runat="server" ID="hf_StampaCoperinaConDocumenti" Value="0"  />
<asp:HiddenField runat="server" ID="hf_StampaCoperinaSenzaDocumenti" Value="0" />


    <%--POPUP Presentazione--%>
    <asp:ModalPopupExtender runat="server" ID="popPresenta" TargetControlID="hfpopPresenta" 
        PopupControlID="pnlPresenta" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
        PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpControl" DynamicServicePath="" Enabled="True" CancelControlID="btnClosePresenta"> 
    </asp:ModalPopupExtender>
	<asp:Panel ID="pnlPresenta" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
        <asp:HiddenField runat="server" ID="hfpopPresenta" />
        <asp:label id="lblErroreUpload"  runat="server" CssClass="msgErrore" Visible="False" AssociatedControlID="cmdAllega"></asp:label>
        <p class="msgInfo">Caricamento Curriculum Vitae</p>
        <fieldset>
			<input id="fileCV" type="file" runat="server" />
            <div style="margin-top: 1em;"></div>
            <asp:Button ID="cmdAllega" runat="server" CssClass="Pulsante" Text="Carica File"/>
            <asp:Button ID="btnPresentaDomanda" CssClass="Pulsante" runat="server"  Text="Presenta"  />
            <asp:Button ID="btnClosePresenta" runat="server" CssClass="Pulsante" Text="Chiudi"/>
        </fieldset>
	</asp:Panel>

</fieldset>
        
</asp:Content>
