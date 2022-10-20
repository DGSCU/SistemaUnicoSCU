<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" MaintainScrollPositionOnPostBack = True CodeBehind="WfrmProgrammiIstanza.aspx.vb" Inherits="Futuro.WfrmProgrammiIstanza" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server" >
    <script  type="text/javascript">

    function ShowPopUp(control) {
        $find(control).show();
        return false;
    }

    function Nascondi()
    {
        document.all.MainContent_msgPresentaIstanza.style.visibility = 'visible';
        document.all.MainContent_msgPresentaIstanza.style.fontSize = '1.4em'
        document.all.MainContent_msgPresentaIstanza.style.fontWeight = 'bold'
        document.all.MainContent_msgPresentaIstanza.style.color = '#3a4f63'
        document.all.MainContent_msgPresentaIstanza.innerText = 'Verifica in corso: ATTENDERE........';
        document.all.MainContent_cmdPresentaIstanza.style.visibility = 'hidden';
        document.all.MainContent_cmdmodifica.style.visibility = 'hidden';
        document.all.MainContent_cmdannulla.style.visibility = 'hidden';
        document.all.MainContent_ImgAnteprimaStampa.style.visibility = 'hidden';
        
    }
 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server" >
         <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <fieldset class="ContornoPagina">
        
        <legend>ISTANZA DI PRESENTAZIONE</legend>
            <asp:label id="lblErrore" runat="server" CssClass="msgErrore"></asp:label><br />
            <asp:label id="lblConferma" runat="server" CssClass="msgInfo"></asp:label>
            <p>
            <asp:LinkButton ID="PDFistanzaPre" Visible="false" CssClass="linkStampa" runat="server">Download PDF Istanza Presentata</asp:LinkButton>        
            </p>
            
        <div runat="server" id="divStoricoPresentazione" width="100%" visible="false">
             <fieldset class="ContornoPagina">
                    <legend>STORICO PRESENTAZIONI</legend>
                    <asp:datagrid id="dtgStoricoPresentazione" CssClass="table" runat="server"  Caption="PRESENTAZIONI" Width="100%"  ToolTip="Storico presentazioni"  AutoGenerateColumns="False" UseAccessibleHeader="True"  AllowSorting="True"  CellPadding="3" AllowPaging="True" PageSize="5">
	                    <FooterStyle ></FooterStyle>
	                    <SelectedItemStyle ></SelectedItemStyle>
	                    <EditItemStyle ></EditItemStyle>
	                    <AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
	                    <ItemStyle CssClass="tr"></ItemStyle>
	                    <HeaderStyle ></HeaderStyle>
	                    <Columns>
					        <asp:BoundColumn DataField="DataPresentazioneSistema" HeaderText="Data Presentazione"><ItemStyle HorizontalAlign="Center"></ItemStyle></asp:BoundColumn>
					        <asp:BoundColumn DataField="DataProtocollazione" HeaderText="Data Protocollazione presentazione"><ItemStyle HorizontalAlign="Center"></ItemStyle></asp:BoundColumn>
					        <asp:BoundColumn DataField="NumeroProtocollo" HeaderText="Numero protocollo presentazione"><ItemStyle HorizontalAlign="Center"></ItemStyle></asp:BoundColumn>
					        <asp:BoundColumn DataField="DataAnnullamento" HeaderText="Data Annullamento"><ItemStyle HorizontalAlign="Center"></ItemStyle></asp:BoundColumn>
					        <asp:BoundColumn DataField="DataProtocollazioneAnnullamento" HeaderText="Data Protocollazione annullamento"><ItemStyle HorizontalAlign="Center"></ItemStyle></asp:BoundColumn>
					        <asp:BoundColumn DataField="NumeroProtocolloAnnullamento" HeaderText="Numero protocollo annullamento"><ItemStyle HorizontalAlign="Center"></ItemStyle></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText=""><ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="PDFistanzaSto" Visible="true" CssClass="linkStampa" runat="server" CommandName="Scarica">Download PDF Istanza</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
					        <asp:BoundColumn DataField="Id" Visible="false"></asp:BoundColumn>
	                    </Columns>
		                <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
                    </asp:datagrid>
            </fieldset>
        </div>

 <p style="text-align:right">
                          <asp:LinkButton ID="imgCheckOLP" CssClass="bold"  CommandName="Olp" Visible="False" runat="server">CONTROLLO OLP</asp:LinkButton>
                                                           
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                           
                                                            <asp:LinkButton id="ImgControllaProvincie" CssClass="bold" CommandName="Provincia" runat="server" Visible="False" 
															ToolTip="Visualizza Risorse per Provincia" >PROVINCE</asp:LinkButton>
                                                </p>

 <fieldset class="ContornoPagina">
        <legend>DATI ISTANZA 
          
        </legend>
			   <div class="wrapper" style="width:100%">
                         <div class="headers" >
                    <h2>
                        <asp:Label ID="lblTitolo" runat="server"  Text="Istanza di Presentazione"></asp:Label>
                    </h2>
              </div>
              <p> </p>
					                                     <div class="row" >
															<div class="collable" style="width:15%">
                                                            Stato Istanza:
                                                            </div>
                                                            <div class="colOggetti" style="width:35%">
                                                            <asp:label id="lblstato" runat="server" CssClass="bold" ></asp:label>
															</div>
															<div class="collable" style="width:15%">
                                                            <asp:label id="lblVOCEStatoEnte" runat="server">Stato Ente: </asp:label>
                                                            </div>
                                                            <div class="colOggetti" style="width:35%">
                                                                    <asp:label id="lblStatoEnte" runat="server" CssClass="bold" >&nbsp;</asp:label>
                                                             </div>
														</div>

					                                     <div class="row" >
															<div class="collable" style="width:15%">
                                                            <asp:label id="lblTotProgrammi" runat="server" AssociatedControlID="TotProgrammi">Totale Programmi</asp:label>
                                                            </div>
															<div class="colOggetti" style="width:35%">
                                                             <asp:label id="TotProgrammi" AssociatedControlID="lblTotProgrammi" CssClass="bold" runat="server"></asp:label>
                                                            </div>
                                                            <div class="collable" style="width:15%">
                                                             <asp:label id="lblTotProgetti" AssociatedControlID="TotProgetti" runat="server">Totale Progetti</asp:label>
                                                            </div>
                                                            <div class="colOggetti" style="width:35%">
                                                             <asp:label id="TotProgetti" AssociatedControlID="lblTotProgetti" CssClass="bold" runat="server"></asp:label>
                                                            </div>
														</div>

                                                         <div class="row" >
                                                         <div class="collable" style="width:15%">
                                                          <asp:label id="lblTotVolontari" AssociatedControlID="TotVolontari" runat="server">Totale Volontari</asp:label>
                                                            </div>
                                                            <div class="colOggetti" style="width:35%">
                                                             <asp:label id="TotVolontari" AssociatedControlID="lblTotVolontari" 
                                                                    CssClass="bold" runat="server"></asp:label>
                                                            </div>
                                                         <div class="collable" style="width:15%">
                                                            </div>
                                                            <div class="colOggetti" style="width:35%">
                                                         
                                                               
                                                            </div>
                                                         </div>
			 </div>
			   <div class="RigaPulsanti" style="text-align:right" id="Table3" >
                                                          <asp:Label ID="msgPresentaIstanza" Visible="true" runat="server" Text=""></asp:Label>
												<asp:Button  id="cmdRipristina" runat="server" Visible="False" CssClass="Pulsante" Text="Ripristina Istanza" ToolTip="Ripristina Istanza" />
                                                <asp:Button id="cmdAnnullaPresentazione" runat="server" OnClientClick="return ShowPopUp('popUpControlAnnullaPresentazione')" Visible="False" ToolTip="Annulla Presentazione"  CssClass="Pulsante" Text="Annulla Presentazione" />
												<asp:Button  id="cmddissaccredita" runat="server" Visible="False" CssClass="Pulsante" Text="Respingi Istanza" OnClientClick="return MessaggioConferma();"  ToolTip="Respingi Istanza" />
												<asp:Button  id="cmdaccredita" runat="server" Visible="False" CssClass="Pulsante" Text="Accetta Istanza" ToolTip="Accetta Istanza"  />
												<asp:Button id="cmdPresentaIstanza" CssClass="Pulsante" OnClientClick="Nascondi()" Visible="False" runat="server" text="Presenta Istanza" ToolTip="Presenta Istanza"   />
												<asp:Button  id="cmdannulla" runat="server" Visible="False" CssClass="Pulsante" ToolTip="Annulla Istanza" Text="Annulla Istanza" />
												<asp:Button  id="cmdmodifica" runat="server" Visible="False" CssClass="Pulsante" ToolTip="Modifica" Text="Modifica" />	
												<asp:Button ID="cmdInserisci" CssClass="Pulsante" Visible="False" ToolTip="Inserisci" runat="server" Text="Inserisci" />
											    
										</div>	
                                        <br />
                                        <div  class="RigaPulsanti" style="text-align:right">
										
												<asp:button  id="ImgAnteprimaStampa" runat="server" Visible="False" text="Anteprima di Stampa" CssClass="Pulsante" ToolTip="Stampa" />
                                                <asp:button  id="imgStampaAll" runat="server" Visible="False" Text="Stampa" CssClass="Pulsante" ToolTip="Stampa"  />
											   <asp:Button ID="cmdChiudi" CssClass="Pulsante" ToolTip="Chiudi" runat="server" Text="Chiudi" />
												
											
										         </div>
              
</fieldset>
        <%if Session("TipoUtente") = "U" and TxtCodiceFascicolo.text<>"" then%>
 <fieldset class="ContornoPagina">
        <legend>DATI FASCICOLO</legend>
				
					<div class="wrapper1" id="Table2" style="width:100%" runat="server">
                                

								
                                <div class= "row"  >
               <div class="collable" style="width:30%" >
                  <asp:textbox id="TxtIdFascicolo" Visible="false" runat="server" Width="0px" ReadOnly="True"></asp:textbox>
<asp:label id="LblNumFascicolo" runat="server" >Numero Fascicolo :</asp:label>
               </div>
               <div class="colOggetti" style="width:55%" >
                   <asp:textbox id="TxtCodiceFascicolo" Width="40%" runat="server" ReadOnly="True"></asp:textbox>
               </div>
                <div class="collable" style="width:15%" >
                   <asp:image style="cursor: pointer" id="cmdSelFascicolo" runat="server" Visible="False"  ImageUrl="images/cartella_small.png" ToolTip="Seleziona Codice Fascicolo"></asp:image>

                     <asp:imagebutton  id="cmdSelProtocollo" runat="server" Visible="true" ImageUrl="images/busta_small.png" ToolTip="Elenco Documenti" />

                   <asp:imagebutton style="cursor: pointer" id="cmdFascCanc" runat="server" Visible="False" ImageUrl="images/canc_small.png" ToolTip="Rimuovi Fascicolo/Protocolli" />

            </div>
        </div>
                                <div class= "row"  >
            <div class="collable" style="width:30%" >
                <asp:label id="LblDescrFascicolo" runat="server" Visible="True">Descrizione :</asp:label>
            </div>
   
           <div class="colOggetti" style="width:55%" >
                   <asp:textbox id="txtDescFasc" runat="server" Rows="4"  ReadOnly="True" 
                       TextMode="MultiLine" Width="100%"
                                            ></asp:textbox>

           </div>
        </div>
 <div class="RigaPulsanti" >
 <input style="WIDTH: 32px; HEIGHT: 22px" id="hddRicordaFascicolo" size="1" type="hidden" runat="server"/>
<asp:Button  id="cmdSalva" runat="server" Visible="False"   CssClass="Pulsante" text="Salva" ToolTip="Salva Fascicolo" />
	</div>
                                <div class="skip">
											<div class="tr" style="text-align:right">
												<div >
                                                <asp:datagrid id="dtgElencoProt" Caption="PROTOCOLLI ASSOCIATI"  runat="server" Visible="False" UseAccessibleHeader="True"  CssClass="table" ToolTip="Elenco esiti requisiti"
														BorderStyle="Ridge"   AutoGenerateColumns="False" AllowSorting="True"
														HorizontalAlign="Center" CellPadding="2" AllowPaging="True" PageSize="2">
														<FooterStyle ></FooterStyle>
														<SelectedItemStyle ></SelectedItemStyle>
														<EditItemStyle ></EditItemStyle>
														<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
														<ItemStyle CssClass="tr"></ItemStyle>
														<HeaderStyle></HeaderStyle>
														<Columns>
															<asp:BoundColumn Visible="False" DataField="IdBandoAttivit&#224;" HeaderText="IdBandoAttivit&#224;">
																<HeaderStyle Width="0%"></HeaderStyle>
															</asp:BoundColumn>
															<asp:BoundColumn DataField="NProt" HeaderText="Num. Protocollo">
																<HeaderStyle Width="1%"></HeaderStyle>
															</asp:BoundColumn>
															<asp:BoundColumn DataField="DataProt" HeaderText="Data Protocollo">
																<HeaderStyle Width="1%"></HeaderStyle>
															</asp:BoundColumn>
															<asp:ButtonColumn Text="&lt;img src=images/canc_small.png Width=20 Height=20 title='Seleziona Versione' border=0&gt;"
																CommandName="Cancella">
																<HeaderStyle Width="0%"></HeaderStyle>
																<ItemStyle HorizontalAlign="Center"></ItemStyle>
															</asp:ButtonColumn>
														</Columns>
														<PagerStyle NextPageText="&gt;&gt;" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
															PrevPageText="&lt;&lt;" HorizontalAlign="Center"   Mode="NumericPages"></PagerStyle>
													</asp:datagrid>
                                                </div>
                                               
                                                <div >
												<asp:label id="LblNumProt" runat="server" Visible="False">N° Protocollo</asp:label>
												<asp:textbox id="txtNumProt" runat="server" Visible="False" Width="104px" ReadOnly="True"></asp:textbox><asp:imagebutton  id="ImgSellProtollo" 
														runat="server" Visible="False" ImageUrl="images/busta_small.png" ToolTip="Seleziona Protocollo" />
											
												<asp:label id="LblDataProt" runat="server" Visible="False">Data Protocollo</asp:label>
												<asp:textbox id="txtDataProt" runat="server" Visible="False" Width="104px" ReadOnly="True"></asp:textbox>
												<asp:Button  id="imgSalvaProt" runat="server" Visible="False" 
														text="Salva" ToolTip="Salva Protocollo" CssClass="Pulsante" />
											  </div>
										    </div>
                                            </div>

							
 </div>
 </fieldset>
  	    <%end if%>   
 <fieldset class="ContornoPagina">
        <legend>ELENCO SEZIONI AVVISO</legend>    
                              
								 <div >
									<div >
                                    <asp:label  id="lblprogramma" runat="server" Visible="False" >Nessun Programma Disponibile</asp:label>
                                    <asp:datagrid id="DgdBandoProgramma" CssClass="table" Caption="SEZIONI AVVISO"  UseAccessibleHeader="True" AllowSorting="true" Width="100%" ToolTip="Elenco Sezioni Avviso"
											BorderStyle="None"  BorderWidth="1px"  AutoGenerateColumns="False"  HorizontalAlign="Center"
											CellPadding="3" runat="server">
											<FooterStyle ></FooterStyle>
											<SelectedItemStyle ></SelectedItemStyle>
											<EditItemStyle ></EditItemStyle>
											<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
											<ItemStyle CssClass="tr"></ItemStyle>
											<HeaderStyle></HeaderStyle>
											<Columns>
												<asp:ButtonColumn HeaderText="Seleziona" Text="&lt;img src=images/vincoli_small.png  title='Seleziona Avviso Presentazione Programmi' border=0&gt;"
													CommandName="Select"></asp:ButtonColumn>
												<asp:BoundColumn Visible="False" DataField="idbando" HeaderText="idbando"></asp:BoundColumn>
												<asp:BoundColumn DataField="Bando" HeaderText="Sezione Avviso Presentazione Programmi"></asp:BoundColumn>
											<asp:BoundColumn DataField="DataInizio" HeaderText="Data Inizio"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="DataFine" HeaderText="Data Fine"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="BandoAperto" HeaderText="StatoBando"></asp:BoundColumn>
                                            </Columns>
											<PagerStyle Visible="False" NextPageText="Successiva" Font-Size="XX-Small" Font-Names="Verdana"
												Font-Bold="True" PrevPageText="Precedente" HorizontalAlign="Center" ></PagerStyle>
										</asp:datagrid>
                                    <asp:label id="lblbando" runat="server" Visible="False">Nessun Avviso Presentazione Programmi Disponibile</asp:label>
                                    </div>
                                 </div>
                                
                         </fieldset>  
 <div  runat="server" id="divProgrammi" Width="100%"  >
            
     <fieldset class="ContornoPagina">
            <legend>ELENCO PROGRAMMI</legend>
        <div class="wrapper" id="EsportazioneFile" style="width:100%;border:0px" >
         
               <div class="row" id="export1" runat="server" visible="false">
                    <div class="collable" style="width:49%;border-style:double">
                             <asp:Label ID="lblesportazioneDoc" Width="92%" runat="server" Text="Esportazione elenco documenti" ></asp:Label>
						     <asp:imagebutton style="cursor: pointer" id="CmdEsportaDoc" runat="server"  Width="35px"
						    Height="35px" ImageUrl="images/cartella_small.png" ToolTip="Esportazione elenco documenti per Programma"></asp:imagebutton>
                    </div>
                    <div class="collable" style="width:49%;border-style:double">
                             <asp:Label ID="lblesportazioneDocRiepilogo" Width="92%"  runat="server" Text="Esportazione riepilogo documenti" ></asp:Label>
						     <asp:imagebutton style="cursor: pointer" id="CmdEsportaDocRiepilogo" runat="server"  Width="35px"
						    Height="35px" ImageUrl="images/cartella_small.png" ToolTip="Esportazione riepilogo documenti"></asp:imagebutton>
                    </div>
               </div>
               <div class="row" id="export3" runat="server" visible="false">
                    <div class="collable" style="width:49%"> 
                    &nbsp;
              <asp:hyperlink id="LinkDocProgramma" Text="Download CSV elenco documenti Programmi" CssClass="linkStampa" ToolTip="Esportazione elenco documenti Programmi" runat="server" Visible="false"  Target="_blank"></asp:hyperlink>
                 </div>
                    <div class="colOggetti" style="width:49%">&nbsp;
               <asp:hyperlink id="LinkRiepilogoProgrammi" runat="server" CssClass="linkStampa" 
                            ToolTip="Esportazione riepilogo documenti Programma" Text="Download CSV riepilogo documenti Programma" 
                            Visible="false"  Target="_blank"></asp:hyperlink>
              </div>
                </div>
               <div class="row" id="export2" runat="server" visible="false">
                    <div class="collable" style="width:49%">
                      &nbsp;
              <asp:hyperlink id="LinkDocProgetti" Text="Download CSV elenco documenti Progetti" CssClass="linkStampa" ToolTip="Esportazione elenco documenti Progetti" runat="server" Visible="false"  Target="_blank"></asp:hyperlink>
             </div>
                    <div class="colOggetti" style="width:49%"> &nbsp;
               <asp:hyperlink id="LinkRielilogoProgetti" runat="server" CssClass="linkStampa" ToolTip="Esportazione riepilogo documenti Progetti" Text="Download CSV riepilogo documenti Progetti" Visible="false"  Target="_blank"></asp:hyperlink>
              </div>

              </div>
          

         
	    <p>
          <asp:checkbox id="chkSelDesel2" runat="server" AutoPostBack="true"   CssClass="bold" Visible="false"
           Text="Seleziona Tutti i Programmi" TextAlign="Left"></asp:checkbox>
        </p>
        <asp:datagrid id="DgtProgrammi" CssClass="table" runat="server"  Caption="PROGRAMMI" Width="100%"  ToolTip="Elenco Programmi"
														      AutoGenerateColumns="False" UseAccessibleHeader="True"
														    AllowSorting="True"  CellPadding="3">
														    <FooterStyle ></FooterStyle>
														    <SelectedItemStyle ></SelectedItemStyle>
														    <EditItemStyle ></EditItemStyle>
														    <AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
														    <ItemStyle CssClass="tr"></ItemStyle>
														    <HeaderStyle ></HeaderStyle>
														    <Columns>
															    <asp:BoundColumn Visible="False" DataField="idProgramma" HeaderText="IdProgramma"></asp:BoundColumn>
															    <asp:TemplateColumn HeaderText="Sel. Program">
																    <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
																    <ItemStyle HorizontalAlign="Center"></ItemStyle>
																    <ItemTemplate>
																	    <asp:CheckBox Runat="server" AutoPostBack="false" ID="chk"></asp:CheckBox>
																    </ItemTemplate>
															    </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="titolo" HeaderText="Titolo Programma">
																    <HeaderStyle HorizontalAlign="Center" Width="70%"></HeaderStyle>
																    <ItemStyle HorizontalAlign="Center"></ItemStyle>
															    </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="numeroProgetti" HeaderText="N&#176; Proge.">
																    <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
																    <ItemStyle HorizontalAlign="Right"></ItemStyle>
															    </asp:BoundColumn>
															    <asp:BoundColumn DataField="numerovolontari" HeaderText="N&#176; Vol.">
																    <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
																    <ItemStyle HorizontalAlign="Right"></ItemStyle>
															    </asp:BoundColumn>
															    <asp:BoundColumn Visible="False" DataField="CONTROLLIPROGRAMMA" HeaderText="CONTROLLIPROGRAMMA"></asp:BoundColumn>
															    <asp:BoundColumn Visible="False" DataField="CONTROLLIPROGETTI" HeaderText="CONTROLLIPROGETTI"></asp:BoundColumn>
															    <asp:BoundColumn DataField="ndoc" HeaderText="N&#176; Doc.">
																    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																    <ItemStyle HorizontalAlign="Right"></ItemStyle>
															    </asp:BoundColumn>
															    <asp:ButtonColumn HeaderText="Doc" Text="&lt;img src=images/cartella_small.png Width=20 Height=20 title='Elenco Documenti Programma' border=0&gt;"
																    CommandName="Documenti"></asp:ButtonColumn>
															    <asp:ButtonColumn HeaderText="Info" Text="&lt;img src=images/info_small.png Width=20 Height=20 title='Informazione Programma' border=0&gt;"
																    CommandName="Select"></asp:ButtonColumn>
															    <asp:BoundColumn Visible="False" DataField="COPROGRAMMAZIONE" HeaderText="COPROGRAMMAZIONE"></asp:BoundColumn>
															    <asp:BoundColumn Visible="False" DataField="IDTERRITORIO" HeaderText="IDTERRITORIO"></asp:BoundColumn>
															    <asp:BoundColumn Visible="False" DataField="NREGIONI" HeaderText="NREGIONI"></asp:BoundColumn>
                                                                <asp:BoundColumn Visible="False" DataField="NSOVRACOMUNI" HeaderText="NSOVRACOMUNI"></asp:BoundColumn>
                                                                <asp:BoundColumn Visible="False" DataField="NPROVINCIE" HeaderText="NPROVINCIE"></asp:BoundColumn>
                                                                <asp:BoundColumn Visible="False" DataField="NCITTAMETROPOLITANE" HeaderText="NCITTAMETROPOLITANE"></asp:BoundColumn>
														        <asp:BoundColumn Visible="False" DataField="NCOMUNI" HeaderText="NCOMUNI"></asp:BoundColumn>
                                                                <asp:BoundColumn Visible="False" DataField="idBandoProgramma" HeaderText="idbandoprogramma"></asp:BoundColumn>
                                                            </Columns>
														    <PagerStyle Visible="False" NextPageText="Successiva" Font-Size="XX-Small" Font-Names="Verdana"
															    Font-Bold="True" PrevPageText="Precedente" HorizontalAlign="Center" ></PagerStyle>
													    </asp:datagrid>


	    <div >
												    <div ><asp:checkbox id="chkSelDesel" runat="server" Visible="false" AutoPostBack="true"  CssClass="bold"
														     Text="Seleziona Tutti i Programmi" TextAlign="Left"></asp:checkbox>
                                                            </div>
											    </div>
	    </div>									
	    </fieldset>
 </div>    
<div runat="server" id="divProgetti" Width="100%" >
     <fieldset class="ContornoPagina">
            <legend>ELENCO PROGETTI</legend>
            <asp:datagrid id="Dgtattivita" CssClass="table" runat="server"  Caption="PROGETTI" Width="100%"  ToolTip="Elenco progetti"
		            AutoGenerateColumns="False" UseAccessibleHeader="True"
	            AllowSorting="True"  CellPadding="3">
	            <FooterStyle ></FooterStyle>
	            <SelectedItemStyle ></SelectedItemStyle>
	            <EditItemStyle ></EditItemStyle>
	            <AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
	            <ItemStyle CssClass="tr"></ItemStyle>
	            <HeaderStyle ></HeaderStyle>
	            <Columns>
		            <asp:BoundColumn Visible="False" DataField="IdProgramma" 
                        HeaderText="IdProgramma">
		            </asp:BoundColumn>
		            <asp:BoundColumn DataField="TitoloProgramma" HeaderText="Titolo Programma">
			            <HeaderStyle HorizontalAlign="Center" Width="30%"></HeaderStyle>
			            <ItemStyle HorizontalAlign="Center"></ItemStyle>
		            </asp:BoundColumn>
		            <asp:BoundColumn Visible="False" DataField="idattivit&#224;" HeaderText="idattivit&#224;"></asp:BoundColumn>
		            <asp:BoundColumn DataField="TitoloProgetto" HeaderText="Titolo Progetto">
			            <HeaderStyle HorizontalAlign="Center" Width="50%"></HeaderStyle>
			            <ItemStyle HorizontalAlign="Center"></ItemStyle>
		            </asp:BoundColumn>
		            <asp:BoundColumn DataField="numerovolontari" HeaderText="N&#176; Vol.">
			            <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
			            <ItemStyle HorizontalAlign="Right"></ItemStyle>
		            </asp:BoundColumn>
		            <asp:TemplateColumn HeaderText="Sel. Progetto">
			            <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
			            <ItemStyle HorizontalAlign="Center"></ItemStyle>
			            <ItemTemplate>
				            <asp:CheckBox Runat="server" AutoPostBack="false" ID="chk" Enabled ="false" ></asp:CheckBox>
			            </ItemTemplate>
		            </asp:TemplateColumn>
		            <asp:BoundColumn Visible="False" DataField="idbandoattività" 
                        HeaderText="idbandoattività"></asp:BoundColumn>
		            <asp:BoundColumn DataField="NOlpRic" HeaderText="N&#176; Olp Rich.">
			            <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
			            <ItemStyle HorizontalAlign="Right"></ItemStyle>
		            </asp:BoundColumn>
		            <asp:BoundColumn DataField="NolpIns" HeaderText="N&#176; Olp Ins.">
			            <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
			            <ItemStyle HorizontalAlign="Right"></ItemStyle>
		            </asp:BoundColumn>
		            <asp:BoundColumn DataField="ndoc" HeaderText="N&#176; Doc.">
			            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
			            <ItemStyle HorizontalAlign="Right"></ItemStyle>
		            </asp:BoundColumn>
		            <asp:ButtonColumn HeaderText="Doc" Text="&lt;img src=images/cartella_small.png Width=20 Height=20 title='Elenco Documenti' border=0&gt;"
			            CommandName="Documenti"></asp:ButtonColumn>
		            <asp:ButtonColumn HeaderText="Apl" Text="&lt;img src=images/Icona_Progetto_small.png Width=20 Height=20 title='Applica Documenti' border=0&gt;"
			            CommandName="Applica"></asp:ButtonColumn>
		            <asp:ButtonColumn HeaderText="Info" Text="&lt;img src=images/info_small.png Width=20 Height=20 title='Informazione Progetto' border=0&gt;"
			            CommandName="Select"></asp:ButtonColumn>
		            <asp:BoundColumn Visible="False" DataField="ESITOFINALE" 
                        HeaderText="ESITOFINALE"></asp:BoundColumn>
	            </Columns>
	            <PagerStyle Visible="False" NextPageText="Successiva" Font-Size="XX-Small" Font-Names="Verdana"
		            Font-Bold="True" PrevPageText="Precedente" HorizontalAlign="Center" ></PagerStyle>
            </asp:datagrid>
    </fieldset>
</div>             
                                                
	
 <p>
							<asp:label id="lblMessaggioPresenta" runat="server" Visible="False"
											Font-Bold="True" >Ogni ente potra' effettuare la presentazione dell’istanza UNA SOLA VOLTA per TUTTI i Programmi che vuole inserire. </asp:label>
                                            <asp:textbox id="txtidbandoAttivita" Visible="false" runat="server" Width="0px" ReadOnly="True"> </asp:textbox>
								
                            </p>
                            
		</fieldset>	
<asp:HiddenField runat="server" ID="IdBandoProgramma" />
<asp:HiddenField runat="server" ID="IdBando" />
<asp:HiddenField runat="server" ID="BandoAperto" />
<asp:HiddenField runat="server" ID="IdBandoAttivita" />	


<asp:HiddenField runat="server" ID="txtEnte" />
<asp:HiddenField runat="server" ID="cboBando" />
<asp:HiddenField runat="server" ID="cboAnno" />
<asp:HiddenField runat="server" ID="cboStatoAttivita" />	
<asp:HiddenField runat="server" ID="cboCompetenza" />


 <%--POPUP File--%>
    <asp:ModalPopupExtender runat="server" ID="popUpAnnullaPresentazione" TargetControlID="hfPopupAnnullaPresentazione" 
        PopupControlID="pnlAnnullaPresentazione" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
        BehaviorID="popUpControlAnnullaPresentazione" Enabled="True" CancelControlID="btnCloseUpAnnullaPresentazione"> 
    </asp:ModalPopupExtender>
	<asp:Panel ID="pnlAnnullaPresentazione" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
        <asp:HiddenField runat="server" ID="hfPopupAnnullaPresentazione" />
        <asp:label id="lblErroreAnnullaPresentazione"  runat="server" CssClass="msgErrore" AssociatedControlID="btnConfermaAnnullaPresentazione"></asp:label>
        <p class="msgInfo">Annulla Istanza di presentazione Programmi di intervento</p>
        <fieldset>
        <h3>IMPORTANTE</h3><b>Nel caso si intenda confermare l’annullamento della presentazione effettuata è fondamentale sapere che:</b><ul><li>L’istanza presentata sarà definitivamente annullata e non avrà nessun valore.<li>I dati dei programmi associati e dei relativi progetti torneranno ad essere modificabili.<li>Se l'istanza viene annullata per operare delle modifiche è assolutamente necessario effettuare nuovamente la procedura di presentazione dell’istanza a sistema.<li>L'eventuale nuova presentazione dovrà essere effettuata comunque entro i termini previsti per l'avviso.</ul>
	          <div  class="RigaPulsanti" style="text-align:center">
                <asp:Button ID="btnConfermaAnnullaPresentazione" runat="server" CssClass="Pulsante" Text="Annulla Presentazione" OnClick="btnConfermaAnnullaPresentazione_Click"/>
                <asp:Button ID="btnCloseUpAnnullaPresentazione" runat="server" CssClass="Pulsante" Text="Chiudi"/>
                 </div>
        </fieldset>
	</asp:Panel>


</asp:Content>

