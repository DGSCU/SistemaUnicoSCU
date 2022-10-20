<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" Async="True" CodeBehind="WfrmIstanzaPresentazione.aspx.vb" Inherits="Futuro.WfrmIstanzaPresentazione" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />

		<script language="javascript" type="text/javascript">
        function MessaggioConferma()
        {

        var msg;
						msg=confirm("Attenzione!!! Richiesta di Respingi Instanza, si desidera continuare?");
						if (msg==false)
							{
                                return false;
							}else
							{
							    return true;
							}
					}	
        
        </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>ISTANZA DI PRESENTAZIONE</legend>
                 <asp:label id="lblMessaggio" runat="server" CssClass="msgErrore"></asp:label>
                    
               

              
                
                  <p style="text-align:right">
                          <asp:LinkButton ID="imgCheckOLP" CssClass="bold"  CommandName="Olp" Visible="False" runat="server">CONTROLLO OLP</asp:LinkButton>
                                                           
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                           
                                                            <asp:LinkButton id="ImgControllaProvincie" CssClass="bold" CommandName="Provincia" runat="server" Visible="False" 
															ToolTip="Visualizza Risorse per Provincia" >PROVINCE</asp:LinkButton>
                                                </p>

<fieldset class="ContornoPagina">
        <legend>DATI ISTANZA </legend>
        
			   <div class="wrapper1" style="width:100%">
              
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
                                                            <asp:label id="lblVoceClasseAttribuita" runat="server">Classe/Sezione attribuita:</asp:label>
                                                            </div>
                                                            <div class="colOggetti" style="width:35%">
                                                            <asp:label id="lblClasseAttribuita" CssClass="bold" runat="server">&nbsp;</asp:label> 
																	 
                                                             </div>
															<div class="collable" style="width:15%">
                                                            <asp:label id="lblVoceTotaleVolontariRichiesti" runat="server">Totale Vol. Ric.:</asp:label>
                                                            </div>
                                                            <div class="colOggetti" style="width:35%">
                                                            <asp:label id="lblTotVolRic" runat="server" CssClass="bold"  ></asp:label>
                                                            </div>
														</div>

					                                     <div class="row" >
															<div class="collable" style="width:15%">
                                                            <asp:label id="lblVoceMassimoVolontariPrevisti" runat="server">Massimo Vol. Prev.: </asp:label>
                                                            </div>
                                                            <div class="colOggetti" style="width:35%">
                                                            <asp:label id="lblMassimoVolontari" CssClass="bold" runat="server"  ></asp:label>
                                                             </div>
                                                            <div class="collable" style="width:15%">
                                                            
                                                            </div >
															<div class="colOggetti" style="width:35%">
                                                            
                                                            </div >
														</div>

			 </div>
			   <div class="RigaPulsanti" style="text-align:right" id="Table3" >
											
												<img alt="attendere" style="visibility: hidden" id="imgAttesa" src="images/attendere.gif" width="250"
														height="40" />
												<asp:Button  id="cmdRipristina" runat="server" Visible="False" CssClass="Pulsante" Text="Ripristina Istanza" ToolTip="Ripristina Istanza" />
                                                <asp:Button id="cmdAnnullaPresentazione" runat="server" Visible="False" ToolTip="Annulla Presentazione"  CssClass="Pulsante" Text="Annulla Presentazione" />
												<asp:Button  id="cmddissaccredita" runat="server" Visible="False" CssClass="Pulsante" Text="Respingi Istanza" OnClientClick="return MessaggioConferma();"  ToolTip="Respingi Istanza" />
												<asp:Button  id="cmdaccredita" runat="server" Visible="False" CssClass="Pulsante" Text="Accetta Istanza" ToolTip="Accetta Istanza"  />
												<asp:Button id="cmdPresentaIstanza" CssClass="Pulsante" runat="server" text="Presenta Istanza" ToolTip="Presenta Istanza"   />
												<asp:Button  id="cmdannulla" runat="server"  CssClass="Pulsante" ToolTip="Annulla Istanza" Text="Annulla Istanza" />
												<asp:Button  id="cmdmodifica" runat="server" CssClass="Pulsante" ToolTip="Modifica" Text="Modifica" />	
												<asp:Button ID="cmdInserisci" CssClass="Pulsante" ToolTip="Inserisci" runat="server" Text="Inserisci" />
											    <asp:Button ID="cmdChiudi" CssClass="Pulsante" ToolTip="Chiudi" runat="server" Text="Chiudi" />
										</div>
                                        <br />
                                        <div  class="RigaPulsanti" style="text-align:right">
												<asp:button  id="ImgAnteprimaStampa" runat="server" Visible="False" text="Anteprima di Stampa" CssClass="Pulsante" ToolTip="Stampa" />
                                                <asp:button  id="imgStampaAll" runat="server" Visible="False" Text="Stampa" CssClass="Pulsante" ToolTip="Stampa"  />
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
        <legend>ELENCO CIRCOLARI</legend>    
                              

								 <div >
									<div >
                                    <asp:label  id="lblprogetto" runat="server" Visible="False" >Nessun progetto Disponibile</asp:label>
                                    <asp:datagrid id="DgdBando" CssClass="table" Caption="CIRCOLARI"  UseAccessibleHeader="True" AllowSorting="true" Width="100%" ToolTip="Elenco Circolari presentazione progetti"
											BorderStyle="None"  BorderWidth="1px"  AutoGenerateColumns="False"  HorizontalAlign="Center"
											CellPadding="3" runat="server">
											<FooterStyle ></FooterStyle>
											<SelectedItemStyle ></SelectedItemStyle>
											<EditItemStyle ></EditItemStyle>
											<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
											<ItemStyle CssClass="tr"></ItemStyle>
											<HeaderStyle></HeaderStyle>
											<Columns>
												<asp:ButtonColumn HeaderText="Seleziona" Text="&lt;img src=images/vincoli_small.png  title='Seleziona Circolare presentazione progetti' border=0&gt;"
													CommandName="Select"></asp:ButtonColumn>
												<asp:BoundColumn Visible="False" DataField="idbando" HeaderText="idbando"></asp:BoundColumn>
												<asp:BoundColumn DataField="Bando" HeaderText="Circolare presentazione progetti"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="idregionecompetenza"></asp:BoundColumn>
											</Columns>
											<PagerStyle Visible="False" NextPageText="Successiva" Font-Size="XX-Small" Font-Names="Verdana"
												Font-Bold="True" PrevPageText="Precedente" HorizontalAlign="Center" ></PagerStyle>
										</asp:datagrid>
                                    <asp:label id="lblbando" runat="server" Visible="False">Nessuna Circolare presentazione progetti Disponibile</asp:label>
                                    </div>
                                 </div>
                                
                         </fieldset>  
                         
                               <fieldset class="ContornoPagina">
        <legend>ELENCO PROGETTI</legend>
                              
                                       <div class="RigaPulsanti" style="text-align:right" >
                                           
                                            
                                                                    <asp:Label ID="lblperImgEsxport" Visible="false" runat="server" Text="Esportazione elenco documenti per progetto"></asp:Label>
															<asp:imagebutton style="cursor: pointer" id="imgEsporta" runat="server" Visible="False" Width="35px"
																	Height="35px" ImageUrl="images/cartella_small.png" ToolTip="Esportazione elenco documenti per progetto"></asp:imagebutton>
                                          
                                                                    <asp:hyperlink id="hlDw" runat="server" CssClass="linkStampa" ToolTip="Esportazione elenco documenti per progetto" Text="Download CSV" Visible="false"  Target="_blank"></asp:hyperlink>
															 <asp:Label ID="lblperImgEsxportRiepig" Visible="false" runat="server" Text="Esportazione riepilogo documenti caricati"></asp:Label>
                                                            <asp:imagebutton style="cursor: pointer" id="imgEsportaRiepilogo" runat="server" Visible="False" Width="35px"
																	Height="35px" ImageUrl="images/cartella_small.png" ToolTip="Esportazione riepilogo documenti per progetto e tipologia"></asp:imagebutton>
                                                                    
                                                                    <asp:hyperlink id="hlDwRip" Text="Download CSV" CssClass="linkStampa" ToolTip="Esportazione elenco documenti per progetto e riepilogo" runat="server" Visible="false"  Target="_blank"></asp:hyperlink>
                                            </div>

											<p>
                                            <asp:checkbox id="chkSelDesel2" runat="server" AutoPostBack="true"  CssClass="bold" Visible="false"
																	 Text="Seleziona Tutti i Progetti" TextAlign="Left"></asp:checkbox>
                                            </p>
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
															<asp:BoundColumn Visible="False" DataField="idattivit&#224;" HeaderText="idattivit&#224;"></asp:BoundColumn>
															<asp:BoundColumn DataField="titolo" HeaderText="Progetti">
																<HeaderStyle HorizontalAlign="Center" Width="70%"></HeaderStyle>
																<ItemStyle HorizontalAlign="Center"></ItemStyle>
															</asp:BoundColumn>
															<asp:BoundColumn DataField="numerovolontari" HeaderText="N&#176; Vol.">
																<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
															</asp:BoundColumn>
															<asp:TemplateColumn HeaderText="Seleziona Progetto">
																<HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
																<ItemStyle HorizontalAlign="Center"></ItemStyle>
																<ItemTemplate>
																	<asp:CheckBox Runat="server" AutoPostBack="false" ID="chk"></asp:CheckBox>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:BoundColumn Visible="False" DataField="bandiattivit&#224;" HeaderText="idbandiattivit&#224;"></asp:BoundColumn>
															<asp:BoundColumn Visible="False" DataField="idattsedeass" HeaderText="N&#176;Sedi Ass.">
																<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
															</asp:BoundColumn>
															<asp:BoundColumn DataField="idattsedeatt" HeaderText="N&#176;Sedi Att.">
																<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
															</asp:BoundColumn>
															<asp:BoundColumn DataField="NOlpRic" HeaderText="N&#176; Olp Rich.">
																<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
															</asp:BoundColumn>
															<asp:BoundColumn DataField="NolpIns" HeaderText="N&#176; Olp Ins.">
																<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
															</asp:BoundColumn>
															<asp:BoundColumn DataField="nRleaIns" HeaderText="RLEA Ins.">
																<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
															</asp:BoundColumn>
															<asp:BoundColumn DataField="nTutorIns" HeaderText="Tutor Ins.">
																<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
															</asp:BoundColumn>
															<asp:BoundColumn DataField="RegComp" HeaderText="Competenza"></asp:BoundColumn>
															<asp:BoundColumn Visible="False" DataField="CONTROLLOSETTORI" HeaderText="CONTROLLOSETTORI"></asp:BoundColumn>
															<asp:BoundColumn Visible="False" DataField="CONTROLLOMAXVOL" HeaderText="CONTROLLOMAXVOL"></asp:BoundColumn>
															<asp:BoundColumn Visible="False" DataField="CONTROLLOVOLPROGETTO" HeaderText="CONTROLLOVOLPROGETTO"></asp:BoundColumn>
															<asp:BoundColumn Visible="False" DataField="CONTROLLOVOLSEDI" HeaderText="CONTROLLOVOLSEDI"></asp:BoundColumn>
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
															<asp:BoundColumn Visible="False" DataField="CONTROLLOOLP" HeaderText="CONTROLLOOLP"></asp:BoundColumn>
															<asp:BoundColumn Visible="False" DataField="CONTROLLOSEDI" HeaderText="CONTROLLOSEDI"></asp:BoundColumn>
															<asp:BoundColumn Visible="False" DataField="CONTROLLODOCUMENTI" HeaderText="CONTROLLODOCUMENTI"></asp:BoundColumn>
                                                            <asp:BoundColumn Visible="False" DataField="CONTROLLOGEOGRAFICO" HeaderText="CONTROLLOGEOGRAFICO"></asp:BoundColumn>
                                                            <asp:BoundColumn Visible="False" DataField="CONTROLLOMINORIOPPORTUNITA" HeaderText="CONTROLLOMINORIOPPORTUNITA"></asp:BoundColumn>
                                                            <asp:BoundColumn Visible="False" DataField="CONTROLLOFAMI" HeaderText="CONTROLLOFAMI"></asp:BoundColumn>
														</Columns>
														<PagerStyle Visible="False" NextPageText="Successiva" Font-Size="XX-Small" Font-Names="Verdana"
															Font-Bold="True" PrevPageText="Precedente" HorizontalAlign="Center" ></PagerStyle>
													</asp:datagrid>


							           <div >
												<div ><asp:checkbox id="chkSelDesel" runat="server" Visible="false" AutoPostBack="true"  CssClass="bold"
														 Text="Seleziona Tutti i Progetti" TextAlign="Left"></asp:checkbox>
                                                        </div>
											</div>
										
								</fieldset>
						
								

								        

								
     
		
                                <p>
							<asp:label id="lblMessaggioPresenta" runat="server" Visible="False"
											Font-Bold="True" >Ogni ente potra' effettuare la presentazione dell’istanza UNA SOLA VOLTA per TUTTI i progetti che vuole inserire. </asp:label>
                                            <asp:textbox id="txtidbandoAttivita" Visible="false" runat="server" Width="0px" ReadOnly="True"> </asp:textbox><asp:textbox id="txtidbando" Visible="false" runat="server" Width="0px" ReadOnly="True"></asp:textbox>
								
                            </p>
                            
		</fieldset>		
</asp:Content>
