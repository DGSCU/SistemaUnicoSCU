<%@ Page Title="Ricerca Risorse Totali" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="RicercaRisorseTot.aspx.vb" Inherits="Futuro.RicercaRisorseTot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

  <fieldset id="RicercaRisorseTotali"  class="ContornoPagina" runat="server">
<legend >Ricerca Risorse Totali</legend>

	                    <div class="wrapper" id="tblRic" style="width:100%">
						<div class="headers" >
                                <h2>
							Ricerca Risorse Totali

                            </h2>
						</div>
                        <br />
						<div class="row">
							<div class="collable" style="width:25%">
                                <asp:Label ID="Label1" runat="server" AssociatedControlID="TxtCodEnte" Text="Cod Ente"></asp:Label></div>
							<div class="colOggetti" style="width:25%"><asp:textbox id="TxtCodEnte" runat="server"  MaxLength="255"></asp:textbox></div>
							<div class="collable" style="width:25%"><asp:Label ID="Label2" AssociatedControlID="TxtDenominazione" runat="server" Text="Denominazione"></asp:Label></div>
							<div class="colOggetti" style="width:25%"><asp:textbox id="TxtDenominazione" runat="server"  MaxLength="255"></asp:textbox></div>
						</div>
						<div class="row">
							<div class="collable" style="width:25%"><asp:Label ID="Label3" AssociatedControlID="txtCognome" runat="server" Text="Cognome"></asp:Label></div>
							<div class="colOggetti" style="width:25%"><asp:textbox id="txtCognome" runat="server"  MaxLength="255"></asp:textbox></div>
							<div class="collable" style="width:25%"><asp:Label ID="Label4" runat="server" AssociatedControlID="txtNome" Text="Nome"></asp:Label></div>
							<div class="colOggetti" style="width:25%"><asp:textbox id="txtNome" runat="server"  MaxLength="255"></asp:textbox></div>
						</div>
						<div class="row">
							<div class="collable" style="width:25%"><asp:Label ID="Label5" AssociatedControlID="ddlRuolo" runat="server" Text="Ruolo"></asp:Label></div>
							<div class="colOggetti" style="width:25%"><asp:dropdownlist id="ddlRuolo" runat="server"></asp:dropdownlist></div>
							<div class="collable" style="width:25%"><asp:Label ID="Label6" AssociatedControlID="txtPosizione" runat="server" Text="Posizione"></asp:Label></div>
							<div class="colOggetti" style="width:25%"><asp:textbox id="txtPosizione" runat="server"  MaxLength="100"></asp:textbox></div>
						</div>
						<div class="row">
							<div class="collable" style="width:25%"><asp:Label ID="Label7" runat="server" AssociatedControlID="txtTitolo" Text="Titolo"></asp:Label></div>
							<div class="colOggetti" style="width:25%"><asp:textbox id="txtTitolo" runat="server"  MaxLength="100"></asp:textbox></div>
							<div class="collable" style="width:25%"><asp:Label ID="Label8" AssociatedControlID="ddlStato" runat="server" Text="Stato"></asp:Label></div>
							<div class="colOggetti" style="width:25%">
                            <asp:dropdownlist id="ddlStato" runat="server">
									<asp:ListItem Value="0" Selected="True">Selezionare</asp:ListItem>
									<asp:ListItem Value="1">Attiva</asp:ListItem>
									<asp:ListItem Value="2">Cancellata</asp:ListItem>
								</asp:dropdownlist>
                            </div>
						</div>
						<div class="row">
							<div class="collable" style="width:25%"><asp:Label ID="Label9" AssociatedControlID="DDLesperienza" runat="server" Text="Esperienza SC"></asp:Label></div>
							<div class="colOggetti" style="width:25%">
								<asp:DropDownList id="DDLesperienza" runat="server">
									<asp:ListItem Value="0">Selezionare</asp:ListItem>
									<asp:ListItem Value="1">Con Esperienza</asp:ListItem>
									<asp:ListItem Value="2">Senza Esperienza</asp:ListItem>
								</asp:DropDownList>
                                </div>
							<div class="collable" style="width:25%"><asp:Label ID="Label10" AssociatedControlID="DDLcorso" runat="server" Text="Corso di Formazione"></asp:Label></div>
							<div class="colOggetti" style="width:25%">
								<asp:DropDownList id="DDLcorso" runat="server">
									<asp:ListItem Value="0">Selezionare</asp:ListItem>
									<asp:ListItem Value="1">Fatto</asp:ListItem>
									<asp:ListItem Value="2">Da Fare</asp:ListItem>
									<asp:ListItem Value="3">Non Necessario</asp:ListItem>
								</asp:DropDownList></div>
						</div>
						<div class="row">
							<div class="collable" style="width:25%"><asp:Label ID="Label11" AssociatedControlID="ddlStatoAccr" runat="server" Text="Stato Iscr."></asp:Label></div>
							<div class="colOggetti" style="width:25%"><asp:dropdownlist id="ddlStatoAccr" runat="server">
									<asp:ListItem Value="10" Selected="True">Selezionare</asp:ListItem>
									<asp:ListItem Value="-1">Chiuso</asp:ListItem>
									<asp:ListItem Value="0">Da Valutare</asp:ListItem>
									<asp:ListItem Value="1">Iscritto</asp:ListItem>
								</asp:dropdownlist>
                                </div>
							<div class="collable" style="width:25%"><asp:Label ID="Label12" AssociatedControlID="CboCompetenza" runat="server" Text="Competenza"></asp:Label></div>
						
							<div class="colOggetti" style="width:25%">
								<asp:dropdownlist id="CboCompetenza" runat="server"></asp:dropdownlist></div>
						</div>
						<div class="row">
							<div class="collable" style="width:25%"><asp:Label ID="Label13" AssociatedControlID="TxtCodiceFiscale" runat="server" Text="CodiceFiscale"></asp:Label></div>
							
							<div class="colOggetti" style="width:25%">
								<asp:textbox id="TxtCodiceFiscale" runat="server" MaxLength="255"></asp:textbox>
                                </div>

                            <div class="collable" style="width:25%">      
                                <asp:Label ID="lblFiltroFase" CssClass="label" AssociatedControlID="txtFiltroFase" runat="server" Text="Rif. Fase"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width:25%">       
                                <asp:TextBox ID="txtFiltroFase" CssClass="textbox" runat="server"></asp:TextBox>
                                <asp:Label ID="lblErroreFiltroFase" CssClass="msgErrore" Text="Inserire un valore numerico." Visible="false" runat="server" />
                            </div>	
						</div>
	
                        </div>
                        <br />
						<div class="RigaPulsanti" style="text-align:right">
                        <asp:Button runat="server" Text="Ricerca" CssClass="Pulsante" id="cmdRicerca" ToolTip="Ricerca"></asp:Button>
									 
								 <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" 
            Text="Esporta CSV" Visible="False" 
              ToolTip="Esporta elenco risultato ricerca" />
              <asp:Button id="cmdChiudi" CssClass="Pulsante" Text="Chiudi" runat="server"  ToolTip="Chiudi"></asp:Button>
								
            <br />
            <asp:HyperLink ID="ApriCSV"   AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" 
                Text="DOWNLOAD CSV" runat="server" 
                ForeColor="#003399" Visible="False"></asp:HyperLink>
            &nbsp;
								
							
							</div>
						<br />
						<div class="RigaPulsanti">
										<div align="right">
									
                                       
                                    </div>
						 
				                    </div>
				 <asp:label id="lblmessaggiosopra" runat="server" ></asp:label>
				        <div class="headers" >
                                <h2>
							Risultato Ricerca Risorse Totali

                            </h2>
			     </div>	
				
				        <div  id="Table1" >
				
					<asp:datagrid id="dtgRisultatoRicerca" Width="100%" CssClass="table" runat="server" UseAccessibleHeader="True" AutoGenerateColumns="False" tooltip="Elenco Risorse Ente" 
								AllowSorting="True" GridLines="None"  AllowPaging="True">
								<FooterStyle ></FooterStyle>
								<SelectedItemStyle></SelectedItemStyle>
								<EditItemStyle></EditItemStyle>
								<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
								<ItemStyle CssClass="tr"></ItemStyle>
								<HeaderStyle ></HeaderStyle>
								<Columns>
                                	    
									<asp:ButtonColumn HeaderText="Sel" Text="&lt;img src=images/Icona_Volontario_small.png title='Seleziona Nominativo' alt='Risorsa' border=0&gt;"
										CommandName="Select"></asp:ButtonColumn>
                                     
									<asp:BoundColumn Visible="False" DataField="IDEntePersonale" HeaderText="IDPersonale"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="IDEnte" HeaderText="IDEnte"></asp:BoundColumn>
									<asp:BoundColumn DataField="Nominativo" HeaderText="Nominativo"></asp:BoundColumn>
									<asp:BoundColumn DataField="Ruolo" HeaderText="Ruolo">
										<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="Posizione" HeaderText="Posizione">
										<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Accreditato" HeaderText="Stato Iscr.">
										<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="Tipologia" HeaderText="Tipologia">
										<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="Approvato" HeaderText="Approvato">
										<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="IDPersonaleAcquisito" HeaderText="Acquisito"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="Denominazione" HeaderText="Denominazione"></asp:BoundColumn>
									<asp:BoundColumn DataField="Stato" HeaderText="Stato">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="UsernameInseritore" HeaderText="UsernameInseritore"></asp:BoundColumn>
									<asp:BoundColumn DataField="CodReg" HeaderText="Codice Ente">
										<FooterStyle Font-Bold="True"></FooterStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="EsperienzaBIS" HeaderText="Esperienza SC">
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CorsoBIS" HeaderText="Corso Formazione">
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Competenza" HeaderText="Competenza"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="UtenteCompetenza" HeaderText="UtenteCompetenza"></asp:BoundColumn>
									<asp:BoundColumn DataField="DataInseritore" HeaderText="Data Inserimento" DataFormatString="{0:d}"></asp:BoundColumn>
								</Columns>
								<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
                          </div>
      <br />
      SERVIZI ACQUISITI<br />
<div id="Table2">
                            <asp:datagrid id="dgServizi" runat="server" CssClass="table" Width="100%" UseAccessibleHeader="true"
								 AutoGenerateColumns="False" tooltip="Elenco Risorse Ente" AllowSorting="True" GridLines="None" >
								<SelectedItemStyle ></SelectedItemStyle>
                                <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
								<ItemStyle CssClass="tr"></ItemStyle>
								<HeaderStyle ></HeaderStyle>
								<Columns>
									<asp:ButtonColumn Visible="False" HeaderText="Sel" Text="&lt;img src=images/selez_comune.jpg title='Seleziona Nominativo' border=0&gt;"
										CommandName="Select">
										<HeaderStyle Width="6%" ></HeaderStyle>
									</asp:ButtonColumn>
									<asp:BoundColumn Visible="False" DataField="IDsistema" HeaderText="IDSistema">
										<HeaderStyle Width="6%" ></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Sistema" HeaderText="Servizi Acquisiti">
										
										<ItemStyle ></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Denominazione" HeaderText="Ente"></asp:BoundColumn>
									<asp:BoundColumn DataField="Codiceregione" HeaderText="Codice Ente" FooterStyle-Font-Bold="True"></asp:BoundColumn>
								</Columns>
								<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
							</asp:datagrid>

					

					 </div>
		
      
        </fieldset>
				<input type="hidden" name="checkpage"/>
</asp:Content>
