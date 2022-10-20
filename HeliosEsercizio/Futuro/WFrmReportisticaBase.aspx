<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WFrmReportisticaBase.aspx.vb" Inherits="Futuro.WFrmReportisticaBase" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">

<legend >Reportistica Base</legend>
<div class="wrapper" style="width:100%">

							 
							 <div class="row" >
                             <div class="msgErrore">
							<asp:label id="lblErr" runat="server" ></asp:label>
                            </div>
							</div>

							 <div class="row" style="width:100%">

                             <div class="collable" style="width:40%">
								<asp:radiobutton id="OptEntiClassiRiep"  runat="server"  
										Text="Enti Accreditati - Riepilogo per Classe (> 01 min.)" GroupName="REPORT" Checked="True"></asp:radiobutton>
                            </div>

                             <div class="collable" style="width:40%">
                            
                            
                                &nbsp;</div>

							 <div class="colOggetti"  style="width:10%;margin-right:5px" >
								<asp:hyperlink id="hlEntiClassiRiep"    runat="server" ImageUrl="images\giu_small.png" Visible="false"
										Target="_blank"></asp:hyperlink>
							</div>

							</div>

							 <div class="row" style="width:100%">

                             <div class="collable" style="width:40%">
								<asp:radiobutton id="OptEntiClassi"  runat="server" 
										Text="Enti Accreditati - Lista per Classe" GroupName="REPORT"></asp:radiobutton>
								</div>

                             <div class="collable" style="width:40%">
								<asp:dropdownlist id="CboClasse"  Width="50%" Runat="server">
										<asp:ListItem Value="1">classe 1 (< 01 min.)</asp:ListItem>
										<asp:ListItem Value="2">classe 2 (< 01 min.)</asp:ListItem>
										<asp:ListItem Value="3">classe 3 (> 01 min.)</asp:ListItem>
										<asp:ListItem Value="4">classe 4 (> 01 min.)</asp:ListItem>
										<asp:ListItem Value="0">TUTTE    (> 02 min.)</asp:ListItem>
									</asp:dropdownlist>&nbsp;
								</div>

                             <div class="colOggetti"  style="width:10%;margin-right:5px" >
								<asp:hyperlink id="hlEntiClassi" runat="server" ImageUrl="images\giu_small.png" Visible="false" Target="_blank"></asp:hyperlink>
                                
                                </div>

							</div>

							 <div class="row" style="width:100%">
								<div class="collable" style="width:40%">
								<asp:radiobutton id="OptEntiRegioniRiep" runat="server"
										Text="Enti Accreditati - Riepilogo per Regione (> 02 min.)" GroupName="REPORT"></asp:radiobutton>

                                </div>
                                 <div class="collable" style="width:40%">
                            
                            
                                &nbsp;</div>

                                <div class="colOggetti"  style="width:10%;margin-right:5px" >
								<asp:hyperlink id="hlEntiRegioniRiep"  runat="server" ImageUrl="images\giu_small.png" Visible="false"
										Target="_blank"></asp:hyperlink>
								</div>
							</div>

							 <div class="row" style="width:100%">

								<div class="collable" style="width:40%">
								<asp:radiobutton id="OptEntiRegioni" runat="server" 
										Text="Enti Accreditati - Lista per Regione (> 02 min.)" GroupName="REPORT"></asp:radiobutton>
								</div>

                                <div class="collable" style="width:40%">
								<asp:dropdownlist id="CboRegione" Width="50%" Runat="server">
										<asp:ListItem Value="Abruzzo">Abruzzo</asp:ListItem>
										<asp:ListItem Value="Basilicata">Basilicata</asp:ListItem>
										<asp:ListItem Value="Calabria">Calabria</asp:ListItem>
										<asp:ListItem Value="Campania">Campania</asp:ListItem>
										<asp:ListItem Value="Emilia Romagna">Emilia Romagna</asp:ListItem>
										<asp:ListItem Value="Friuli Venezia Giulia">Friuli Venezia Giulia</asp:ListItem>
										<asp:ListItem Value="Lazio">Lazio</asp:ListItem>
										<asp:ListItem Value="Liguria">Liguria</asp:ListItem>
										<asp:ListItem Value="Lombardia">Lombardia</asp:ListItem>
										<asp:ListItem Value="Marche">Marche</asp:ListItem>
										<asp:ListItem Value="Molise">Molise</asp:ListItem>
										<asp:ListItem Value="Piemonte">Piemonte</asp:ListItem>
										<asp:ListItem Value="Puglia">Puglia</asp:ListItem>
										<asp:ListItem Value="Sardegna">Sardegna</asp:ListItem>
										<asp:ListItem Value="Sicilia">Sicilia</asp:ListItem>
										<asp:ListItem Value="Toscana">Toscana</asp:ListItem>
										<asp:ListItem Value="Bolzano - Bozen">Bolzano - Bozen</asp:ListItem>
										<asp:ListItem Value="Trento">Trento</asp:ListItem>
										<asp:ListItem Value="Umbria">Umbria</asp:ListItem>
										<asp:ListItem Value="Valle d'Aosta">Valle d'Aosta</asp:ListItem>
										<asp:ListItem Value="Veneto">Veneto</asp:ListItem>
									</asp:dropdownlist>&nbsp;
								</div> 

                                <div class="colOggetti" style="width:5%">
								<asp:hyperlink id="hlEntiRegioni" runat="server" Width="5%" ImageUrl="images\giu_small.png" Visible="false" Target="_blank"></asp:hyperlink>
                                </div>

							</div>

							 <div class="row" style="width:100%">

                             <div class="collable" style="width:40%">
								<asp:radiobutton id="OptEntiProgetti" runat="server" 
										Text="Progetti per Ente - Riepilogo per Bando (> 02 min.)" GroupName="REPORT"></asp:radiobutton>
                              </div>

                             <div class="collable" style="width:40%">
                                        <asp:dropdownlist id="CboBando" Width="50%"
                                         Runat="server"></asp:dropdownlist>&nbsp;
                             </div>

                             <div class="colOggetti" style="width:5%">
								<asp:hyperlink id="hlEntiProgetti" runat="server" Width="5%" ImageUrl="images\giu_small.png" Visible="false"
										Target="_blank"></asp:hyperlink>&nbsp;
								
                              </div>
							</div>

							 <div class="row" style="width:100%">

                             <div class="collable" style="width:40%">
								<asp:radiobutton id="OptProgettiBando" runat="server"
										Text="Progetti per Ente - Lista per Bando (> 02 min.)" GroupName="REPORT"></asp:radiobutton>

                                </div>

                             <div class="collable" style="width:40%">
								<asp:dropdownlist id="CboBando1" Width="50%"  Runat="server"></asp:dropdownlist>&nbsp;
								</div>

                             <div class="colOggetti" style="width:5%">
								<asp:hyperlink id="hlProgettiBando" runat="server" Width="5%" ImageUrl="images\giu_small.png" Visible="false"
										Target="_blank"></asp:hyperlink>
							</div>	
							</div>


							 <div class="row" style="width:100%">
                             <div class="collable" style="width:40%">
								<asp:radiobutton id="OptProgettiRegioneRiep"  runat="server"
										 Text="Posti per Regione - Riepilogo per Bando (> 02 min.)" GroupName="REPORT"></asp:radiobutton>
                             </div>

                             <div class="collable" style="width:40%">
								<asp:dropdownlist id="CboBando2" Width="50%" Runat="server"></asp:dropdownlist>&nbsp;
								</div>

                             <div class="colOggetti" style="width:5%">
								<asp:hyperlink id="hlProgettiRegioneRiep" runat="server" Width="5%" ImageUrl="images\giu_small.png" Visible="false"
										Target="_blank"></asp:hyperlink>
							
                                </div>
							</div>

							 <div class="row" style="width:100%">
								    <div class="collable" style="width:40%">
								<asp:radiobutton id="OptProgettiRegione" runat="server" 
										Text="Progetti per Regione - Lista per Bando (> 02 min.)" GroupName="REPORT"></asp:radiobutton>

                                        </div>

                                    <div class="collable" style="width:20%">
                                        <asp:dropdownlist id="CboRegione1"  Runat="server">
										<asp:ListItem Value="Abruzzo">Abruzzo</asp:ListItem>
										<asp:ListItem Value="Basilicata">Basilicata</asp:ListItem>
										<asp:ListItem Value="Calabria">Calabria</asp:ListItem>
										<asp:ListItem Value="Campania">Campania</asp:ListItem>
										<asp:ListItem Value="Emilia Romagna">Emilia Romagna</asp:ListItem>
										<asp:ListItem Value="Friuli Venezia Giulia">Friuli Venezia Giulia</asp:ListItem>
										<asp:ListItem Value="Lazio">Lazio</asp:ListItem>
										<asp:ListItem Value="Liguria">Liguria</asp:ListItem>
										<asp:ListItem Value="Lombardia">Lombardia</asp:ListItem>
										<asp:ListItem Value="Marche">Marche</asp:ListItem>
										<asp:ListItem Value="Molise">Molise</asp:ListItem>
										<asp:ListItem Value="Piemonte">Piemonte</asp:ListItem>
										<asp:ListItem Value="Puglia">Puglia</asp:ListItem>
										<asp:ListItem Value="Sardegna">Sardegna</asp:ListItem>
										<asp:ListItem Value="Sicilia">Sicilia</asp:ListItem>
										<asp:ListItem Value="Toscana">Toscana</asp:ListItem>
										<asp:ListItem Value="Bolzano - Bozen">Bolzano - Bozen</asp:ListItem>
										<asp:ListItem Value="Trento">Trento</asp:ListItem>
										<asp:ListItem Value="Umbria">Umbria</asp:ListItem>
										<asp:ListItem Value="Valle d'Aosta">Valle d'Aosta</asp:ListItem>
										<asp:ListItem Value="Veneto">Veneto</asp:ListItem>
									</asp:dropdownlist>&nbsp;
                                   </div>

                                    <div class="collable" style="width:20%">
                                    <asp:dropdownlist id="CboBando3"   Runat="server"></asp:dropdownlist>&nbsp;
                                    </div>

                                    <div class="colOggetti" style="width:5%">
								<asp:hyperlink id="hlProgettiRegione" Width="5%" runat="server" ImageUrl="images\giu_small.png" Visible="false"
										Target="_blank"></asp:hyperlink>&nbsp;
								
                                </div>

							</div>
							 <div class="row" style="width:100%">

                            <div class="collable" style="width:40%">								
								<asp:radiobutton id="OptVolontariRiep" runat="server"
										Text="Volontari per Bando - Riepilogo per Regione (> 03 min.)" GroupName="REPORT"></asp:radiobutton>
                            </div>

                            <div class="collable" style="width:40%">	
								<asp:dropdownlist id="CboBando5" Width="50%"  Runat="server"></asp:dropdownlist>&nbsp;
								</div>

                            <div class="colOggetti" style="width:5%">
								<asp:hyperlink id="hlVolontariRiep"  runat="server" ImageUrl="images\giu_small.png" Visible="false"
										Target="_blank"></asp:hyperlink>
								
                                </div>

							</div>
							 <div class="row" style="width:100%">
								<div class="collable" style="width:40%">
                                <asp:radiobutton id="OptVolontari"  runat="server"
										Text="Volontari per Bando - Lista per Status (> 04 min.)" GroupName="REPORT"></asp:radiobutton>
                                </div>

                                <div class="collable" style="width:20%">
								<asp:dropdownlist id="CboStato"  Runat="server">
										<asp:ListItem Value="3">In Servizio</asp:ListItem>
										<asp:ListItem Value="4">Rinunciatari</asp:ListItem>
										<asp:ListItem Value="5">Chiusi in Servizio</asp:ListItem>
									</asp:dropdownlist>
                                  </div>

                                <div class="collable" style="width:20%">
                                    <asp:dropdownlist id="CboBando4"  Runat="server"></asp:dropdownlist>&nbsp;
								</div>

                                <div class="colOggetti" style="width:5%">
								<asp:hyperlink id="hlVolontari" Width="5%" runat="server" ImageUrl="images\giu_small.png" Visible="false" Target="_blank"></asp:hyperlink>
								</div>

							</div>

						
								<div class="RigaPulsanti"  style="width:100%">

                                
                                    &nbsp;
									<asp:button id="cmdEsporta"  runat="server" Text="Esporta" CssClass="Pulsante" ToolTip="Esegui Esportazione"/>
                                    
                                    &nbsp;
                                    <asp:button id="CmdChiudi"  runat="server" Text="Chiudi" CssClass="Pulsante" ToolTip="Chiudi"/>
                                    </div>
							
							 
			</div>
                    </fieldset>
</asp:Content>
