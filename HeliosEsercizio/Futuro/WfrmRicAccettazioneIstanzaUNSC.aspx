<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicAccettazioneIstanzaUNSC.aspx.vb" Inherits="Futuro.WfrmRicAccettazioneIstanzaUNSC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <fieldset class="ContornoPagina">
<legend >Ricerca Istanze Progetti</legend>
<div class="wrapper" style="width:100%">
    <div class="headers">
       <h2>
      <asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Istanza di Presentazione Progetti"></asp:Label>
      </h2>
      </div>

    <div class="RigaVuota">
       
    &nbsp;

    </div>
   
							
								
								
								 <div class="row" >
									  <div class="collable" style="width:15%">
                                       <asp:Label ID="lblEnte" AssociatedControlID="txtDenominazioneEnte" runat="server" Text="Ente"></asp:Label>
                                      </div>
									 <div class="colOggetti" style="width:85%">    
										<asp:textbox id="txtDenominazioneEnte" runat="server" MaxLength="200"></asp:textbox>
                                        <asp:textbox id="txtDenominazioneEnte1" runat="server" Width="0px" MaxLength="200" visible="False"></asp:textbox>
									</div>
								</div>
								 <div class="row" >
									 <div class="collable" style="width:15%">
										<asp:Label ID="lblBando" AssociatedControlID="DdlBando" runat="server" Text="Bando"></asp:Label>
									</div>
									<div class="colOggetti" style="width:35%">
                                    <asp:textbox id="TxtdenominazioneSede1" runat="server" Width="0px" MaxLength="200" visible="False"></asp:textbox>
										<asp:dropdownlist id="DdlBando" runat="server"  AutoPostBack="False"></asp:dropdownlist>
									</div>
									<div class="collable" style="width:15%">
										<asp:Label ID="lblAnno" AssociatedControlID="ddlanno" runat="server" Text="Anno"></asp:Label>
									</div>
									<div class="colOggetti" style="width:35%">
										<asp:dropdownlist id="ddlanno" runat="server" ></asp:dropdownlist><asp:textbox id="Txtstato1" runat="server" Width="0px" MaxLength="20" Visible="False"></asp:textbox>
									</div>
								</div>
								 <div class="row" >
									<div class="collable" style="width:15%">
										<asp:Label ID="lblCodEnte" AssociatedControlID="txtCodEnte" runat="server" Text="Codice Ente"></asp:Label>
									</div>
									<div class="colOggetti" style="width:35%">
										<asp:textbox id="txtCodEnte" runat="server" MaxLength="100"></asp:textbox>
                                        <asp:textbox id="Textbox2" runat="server" Width="0px" MaxLength="200" visible="False"></asp:textbox>
									</div>
									<div class="collable" style="width:15%">
										<asp:Label ID="lblStatoIstanza" AssociatedControlID="ddlStatoAttivita" runat="server" Text="Stato Istanza"></asp:Label>
									</div>
									<div class="colOggetti" style="width:35%">
										<asp:dropdownlist id="ddlStatoAttivita"  Runat="server"></asp:dropdownlist>
									</div>
								</div>
								 <div class="row" >
									<div class="collable" style="width:15%">
										<asp:Label ID="lblCompetenza" AssociatedControlID="CboCompetenza" runat="server" Text="Competenza"></asp:Label>
									</div>
									<div class="colOggetti" style="width:35%">
                                    <asp:textbox id="Textbox3" runat="server" Width="0px" MaxLength="200" visible="False"></asp:textbox>
								    <asp:DropDownList id="CboCompetenza" runat="server"></asp:DropDownList>
									</div>
									
								</div>
								 <div class="row" >
									<div class="colOggetti" style="width:35%">
                                    <asp:textbox id="txtModifica" runat="server"  BorderStyle="None" 
											Visible="False"></asp:textbox>
                                            </div>
                                      </div>
							     <div class="RigaPulsanti">
										<asp:Button ID="cmdSalva" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                                       
                                         <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
                                          <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
		            <br />
	                <asp:HyperLink ID="hlVolontari" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
								</div>
								 
							
		 <div class="RigaVuota" >
         &nbsp;

         </div>					
					
						
	 <div class="row" >						
                               <h3>
<asp:label id="lblmessaggio"  runat="server" AssociatedControlID="dgRisultatoRicerca"></asp:label>
</h3> 
                                
                                <asp:datagrid id="dgRisultatoRicerca" Width="100%" runat="server" 
                                   AllowPaging="True" CellPadding="2"  CssClass="table"	AllowSorting="True" 
                                   AutoGenerateColumns="False" UseAccessibleHeader="True">
											<FooterStyle ></FooterStyle>
											<SelectedItemStyle ></SelectedItemStyle>
											<EditItemStyle ></EditItemStyle>
											<AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
											<ItemStyle  CssClass="tr"></ItemStyle>
											<HeaderStyle></HeaderStyle>
											<Columns>
												<asp:ButtonColumn Text="&lt;img src=images/documento_small.png  title='Seleziona Bando' border=0&gt;"
													CommandName="Select"></asp:ButtonColumn>
												<asp:BoundColumn Visible="False" DataField="idbando" HeaderText="idbando"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="idente" HeaderText="idente"></asp:BoundColumn>
												<asp:BoundColumn DataField="denominazione" HeaderText="Ente"></asp:BoundColumn>
												<asp:BoundColumn DataField="CodiceRegione" HeaderText="Cod. Ente"></asp:BoundColumn>
												<asp:BoundColumn DataField="Bando" HeaderText="Bando">
													<HeaderStyle Width="50px"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="datainizio" HeaderText="Data Inizio Bando"></asp:BoundColumn>
												<asp:BoundColumn DataField="datafine" HeaderText="Data Fine Bando"></asp:BoundColumn>
												<asp:BoundColumn DataField="progetti" HeaderText="N. Progetti">
													<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
													<ItemStyle HorizontalAlign="Center"></ItemStyle>
												</asp:BoundColumn>
												<asp:ButtonColumn Text="..." HeaderText="Dettaglio Istanza" CommandName="Dettagli">
													<HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
													<ItemStyle HorizontalAlign="Center"></ItemStyle>
												</asp:ButtonColumn>
												<asp:BoundColumn Visible="False" DataField="idbandoattivit&#224;" HeaderText="idbandoattivit&#224;"></asp:BoundColumn>
												<asp:BoundColumn DataField="Competenza" HeaderText="Competenza"></asp:BoundColumn>
											    <asp:BoundColumn DataField="stato" Visible="False"></asp:BoundColumn>
											</Columns>
											<PagerStyle NextPageText="Successiva"
												PrevPageText="Precedente" CssClass="linkPageGrid" HorizontalAlign="Center"
												Mode="NumericPages"></PagerStyle>
										</asp:datagrid>
								
					
			<!--hidden che serve per copntrollare se sto facendo una ricerca così setto il currentpageindex a 0-->
			<input type="hidden" name="checkpage"/>
      </div>
     </div>
</fieldset>
     

</asp:Content>
