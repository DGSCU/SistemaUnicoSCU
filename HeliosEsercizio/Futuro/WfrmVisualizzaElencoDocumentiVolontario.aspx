<%@ Page Title="Gestione Documenti Volontario" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmVisualizzaElencoDocumentiVolontario.aspx.vb" Inherits="Futuro.WfrmVisualizzaElencoDocumentiVolontario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <script type="text/javascript">
       function infoPrefissi() {
           
           window.open("wfrmInfoPrefissiDocumentiEntita.aspx?VengoDA='GestioneDocumenti'", "Elenco", "height=450,width=800,dependent=no,scrollbars=yes,status=no,resizable=yes");
            }
           
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="firefox">
    <fieldset  class="ContornoPaginaScroll" style="width:96%">
        <legend>Gestione Documenti Volontario</legend>
       <div class="wrapper" style="width:100%">
      
            <div class="headers" >
                <h2><asp:Label ID="lblTitolo" runat="server"  Text="Gestione Documenti Volontario"></asp:Label></h2>
            </div>
        </div>
            <fieldset class="ContornoPagina" style="width:96%">
                <legend>Informazione Volontario</legend>
                <div class="wrapper"  style="width:98%">
                    <div id="DivDatiVol" class="row" runat="server">
                        <div class="collable" style="width:15%">
                            <asp:label id="LabelCodiceVol" CssClass="label" AssociatedControlID="lblCodiceVol" runat="server" Text="Codice Volontario" />
                        </div>
                        <div class="colOggetti" style="width:35%">
                            <asp:label id="lblCodiceVol" CssClass="labelDati" runat="server" />
                        </div>
                        <div class="collable" style="width:15%">
                            <asp:label id="LabelVolontario" CssClass="label" AssociatedControlID="lblVolontario" runat="server" Text="Volontario" />
                        </div>
                        <div class="colOggetti" style="width:35%">
                            <asp:label id="lblVolontario" CssClass="labelDati" runat="server" />
                        </div>
                    </div>
                    <div class="row" runat="server">
                        <div class="collable" style="width:15%">
                            <asp:label id="LabelDIS" CssClass="label" AssociatedControlID="LblDataInizioServizio" runat="server" Text="Data Inizio Servizio" />
                        </div>
                        <div class="colOggetti" style="width:35%">
                            <asp:label id="LblDataInizioServizio" CssClass="labelDati" runat="server" />
                        </div>
                        <div class="collable" style="width:15%">
                            <asp:label id="LabelDFS" CssClass="label" AssociatedControlID="LblDataFineServizio" runat="server" Text="Data Fine Servizio" />
                        </div>
                        <div class="colOggetti" style="width:35%">
                            <asp:label id="LblDataFineServizio" CssClass="labelDati" runat="server" />
                        </div>
                    </div>
                    <div  class="row" runat="server">
                     <div class="collable" style="width:15%">
                            <asp:label id="LabelStato" CssClass="label" AssociatedControlID="LblStato" runat="server" Text="Stato Volontario" />
                        </div>
                        <div class="colOggetti" style="width:35%">
                            <asp:label id="LblStato" CssClass="labelDati" runat="server" />
                        </div>
                    </div>
                     <div id="Div1" class="row" runat="server">
                         <div class="collable" style="width:15%">
                            <asp:label id="LabelIban" CssClass="label" AssociatedControlID="LblIban" runat="server" Text="IBAN" />
                        </div>
                        <div class="colOggetti" style="width:35%">
                            <asp:label id="LblIban" CssClass="labelDati" runat="server" />
                        </div>
                        <div class="collable" style="width:15%">
                            <asp:label id="lblNotifica" CssClass="label" AssociatedControlID="CmdNotifica" runat="server" Text="Notifica Ente" />
                        </div>
                        <div class="colOggetti" style="width:35%">
                            <asp:Button ID="CmdNotifica" runat="server" CommandName="NotificaEnte" Visible="false" CssClass="Pulsante" Text="Notifica" />
                        <strong>&nbsp;&nbsp;&nbsp; <asp:LinkButton ID="imgStoricoNotifiche" runat="server" style="cursor:pointer;font-size:small;font-weight:bold" Text="Storico Notifiche" Visible="false"></asp:LinkButton></strong>
                        </div>
                     </div>
                </div>
            </fieldset>
            <fieldset class="ContornoPagina" style="width:96%">
                <legend>Carica Documenti Volontario</legend>
                <div class="wrapper" style="width:100%;border:0px">
                    <div  class="row">
                        <div class="collable" style="width:70%;text-align:left">
                            <asp:ImageButton  id="imgPrefissiDocumenti" runat="server" style="cursor:hand" ImageUrl="images/info_small.png" OnClientClick="javascript:infoPrefissi()"  ToolTip="Prefissi Documenti" AlternateText="Prefissi Documenti"/>
                             <asp:label ID="LblSel" CssClass="label" AssociatedControlID="txtSelFile" runat="server" Text="Selezionare il file" ></asp:label>
                             <input ID="txtSelFile" type="file" style="width:60%"  runat="server" />
                        </div>
                         <div class="colOggetti" style="width:30%;text-align:left">
                                 <asp:Button  id="cmdUpload" runat="server" CssClass="Pulsante"  Text="Upload File" />&nbsp;
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class ="collable"  style="width:100%;text-align:left">
                            <asp:label id="LblMsgFile"  ToolTip="Messaggio File" AssociatedControlID="cmdUpload"  Visible="false" runat="server" ></asp:label>
                        </div>
                    </div>
                </div>
                <br />    
            </fieldset>   
            <fieldset class="ContornoPaginaScroll" style="width:96%">
                <legend>Elenco Documenti Volontario</legend>
                <div class="firefox">
                  <div class="wrapper" style="width:100%;border:0px">
               <div class="collable" style="width:70%;text-align:left">
                <asp:label id="LblTipoDocumento" CssClass="label" AssociatedControlID="ddlTipoDocumenti" runat="server" Text="Tipo Documento" />
                <asp:DropDownList ID="ddlTipoDocumenti" style="width:50%" runat="server"/>       
               </div>
                 <div class="colOggetti" style="width:30%;text-align:left">
                            <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />                 
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
               </div>
                </div>
              <br />   
                
           
                <asp:label id="lblmessaggio" CssClass="msgInfo"   ToolTip="Messaggio" AssociatedControlID="dgElencoDocumenti" Visible="false" runat="server"></asp:label>
           
                  
            <asp:datagrid id="dgElencoDocumenti" runat="server" Width="100%" 
                    ToolTip="Elenco Documenti Volontario" CssClass="table" 
                    AllowSorting="True" AutoGenerateColumns="False" 
                    UseAccessibleHeader="True" >
	            <FooterStyle></FooterStyle>
		        <SelectedItemStyle ></SelectedItemStyle>
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		        <HeaderStyle ></HeaderStyle>
	            <Columns>
                    <asp:BoundColumn DataField="IdEntitàDocumento" HeaderText="IdEntitàDocumento" 
                        Visible="False"></asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="Download">
                    <HeaderStyle />
                        <ItemTemplate>
	                        <asp:ImageButton ID="imgScaricaDoc" style="cursor:pointer;" CommandName="Download" alt="Scarica Documento" ToolTip='Scarica Documento' runat="server" ImageURL="images/giu_small.png" CausesValidation="false"></asp:ImageButton>  
                         </ItemTemplate>
                    </asp:TemplateColumn>
          
                    <asp:BoundColumn DataField="Nomefile" HeaderText="Nome File">
                        <HeaderStyle Width="40%"/>
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="DataInserimento" HeaderText="Data Inserimento">
                        <HeaderStyle/>
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="HashValue" HeaderText="Hash Value">
                        <HeaderStyle />
                    </asp:BoundColumn>

                    <asp:BoundColumn DataField="RiferimentoTemporale" 
                        HeaderText="Riferimento Temporale/ Ore Formazione" >
                        <HeaderStyle />
                    </asp:BoundColumn>

                    <asp:BoundColumn DataField="Tipologia" HeaderText="Tipologia Documento">
                        <HeaderStyle />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="StatoDocumento" HeaderText="Stato Documento">
                        <HeaderStyle />
                    </asp:BoundColumn>
                     <asp:TemplateColumn HeaderText="Rimuovi"> 
                        <ItemTemplate>
	                        <asp:ImageButton ID="imgCancDoc"  style="cursor:pointer;" CommandName="Elimina" alt="Rimuovi Documento" ToolTip='Rimuovi Documento' runat="server" ImageURL="images/vistacattiva_small.png" CausesValidation="false"  Visible='<%# IIF(Eval("PulsanteCancellaDocumento").ToString().Equals("NO"), False, True) %>' ></asp:ImageButton>  
                         </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn Visible="False" HeaderText="Valido"> 
                        <ItemTemplate>
				            <asp:ImageButton ID="imgValida"  CommandName="Valida" AlternateText="Documento Valido" 
                                ToolTip="Valida Documento" runat="server" ImageURL="Images/selezionato_small.png" Visible='<%# IIF(Eval("PulsanteConfermaValida").ToString().Equals("NO"), False, True) %>'></asp:ImageButton>  
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn Visible="False" HeaderText="Non Valido"> 
                        <ItemTemplate>
			    	        <asp:ImageButton ID="imgNonValida"  CommandName="NonValida" AlternateText="Documento non Validato" 
                                ToolTip="Documento Non Valido" runat="server" ImageURL="Images/deselezionato_small.png" Visible='<%# IIF(Eval("PulsanteConfermaNONValida").ToString().Equals("NO"), False, True) %>'></asp:ImageButton>  
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn  Visible="False" HeaderText="Presenze/ Rimborso"> 
                        <ItemTemplate>
			    	        <asp:ImageButton ID="imgDett"  CommandName="Dettaglio" AlternateText="Entra nel Dettaglio" 
                                ToolTip="Dettaglio Presenze/Rimborso" runat="server" ImageURL="Images/ZoomIn_small.png" Visible='<%# IIF(Eval("PulsantePresenzeViaggio").ToString().Equals("NO"), False, True) %>'></asp:ImageButton>  
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn  DataField="OreFormazioneSpecifica"  HeaderText="Ore Formazione Specifica" Visible="false" > </asp:BoundColumn>
                     <asp:BoundColumn  DataField="OreFormazione"  HeaderText="Ore Formazione" Visible="false" > </asp:BoundColumn>
                </Columns>
	            <HeaderStyle></HeaderStyle>
                <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
            </asp:datagrid>

            <p>
            <asp:HyperLink ID="hlDownload"  runat="server"  Visible="false"></asp:HyperLink>
           </p>
           </div>
            </fieldset>
         
      
     </fieldset>
    </div>
</asp:Content>
