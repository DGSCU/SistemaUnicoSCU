<%@ Page Title="Aggiornamento Corso Formatori" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmAggiornamentoFormatori.aspx.vb" Inherits="Futuro.WfrmAggiornamentoFormatori" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
		
	<%--	<script language="javascript">
		    function TogliCheck() {
		        document.all.chkSelDesel.checked = false;
		    }		
		</script>
		<script for="cmdAssegna" event="onclick">
			if (document.Form1.ddlAggiornaAnnoCorsoFor.value=="Selezionare") 
			{
				alert("E' necessario scegliere l'anno di aggiornamento corso.")
				document.Form1.ddlAggiornaAnnoCorsoFor.focus();
				return false;
			}
		</script>
		<script for="imgAssegnaCorso" event="onclick">
			if (document.Form1.ddlAggiornaCorsoFor.value=="0") 
			{
				alert("E' necessario scegliere il valore da assegnare.")
				document.Form1.ddlAggiornaCorsoFor.focus();
				return false;
			}
		</script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


 <fieldset class="ContornoPagina">
        <legend>Corso Aggiornamento Formatori</legend>


     

							<div id="Table2" class="wrapper" style="width:100%">
								<div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Formatori"></asp:Label></h2>
            </div>
                                <div class="RigaVuota" >
                                &nbsp;
                                </div>

								<div class="row"> 
									
									<div class="collable" style="width:15%">
                                    <asp:Label ID="Label3" CssClass="bold" AssociatedControlID="txtdenominazioneEnte" runat="server" Text="Ente"></asp:Label>
                                    </div>
									<div class="colOggetti" style="width:35%">
										<asp:textbox id="txtdenominazioneEnte" runat="server" MaxLength="200" ></asp:textbox>
									</div>
									<div class="collable" style="width:15%"> <asp:Label ID="Label4" CssClass="bold" AssociatedControlID="txtCodiceEnte" runat="server" Text="Codice Ente"></asp:Label></div>
									<div class="colOggetti" style="width:35%"><asp:textbox id="txtCodiceEnte" tabIndex="6" runat="server" MaxLength="10" ></asp:textbox></div>
									
								</div>
								<div class="row"> 
									<div class="collable" style="width:15%"><asp:Label ID="Label5" CssClass="bold" AssociatedControlID="txtCognome" runat="server" Text="Cognome"></asp:Label></div>
									<div class="colOggetti" style="width:35%"><asp:textbox id="txtCognome" runat="server" MaxLength="100" ></asp:textbox>
                                    </div>
									<div class="collable" style="width:15%"><asp:Label ID="Label6" CssClass="bold" AssociatedControlID="txtNome" runat="server" Text="Nome"></asp:Label></div>
									<div class="colOggetti" style="width:35%"><asp:textbox id="txtNome" runat="server" MaxLength="100" ></asp:textbox>
                                    </div>
								</div>
								<div class="row"> 
									<div class="collable" style="width:15%"><asp:Label ID="Label7" CssClass="bold" AssociatedControlID="txtCodiceFisclae" runat="server" Text="Codice Fiscale"></asp:Label></div>
									<div class="colOggetti" style="width:35%"><asp:textbox id="txtCodiceFisclae" runat="server" MaxLength="50" ></asp:textbox>
                                    </div>
									<div class="collable" style="width:15%"><asp:Label ID="Label8" CssClass="bold" runat="server" AssociatedControlID="ddlCorsoFormazione" Text="Corso Formazione"></asp:Label></div>
									<div class="colOggetti" style="width:35%">
										<asp:dropdownlist id="ddlCorsoFormazione" runat="server">
											<asp:ListItem Value="0">Selezionare</asp:ListItem>
											<asp:ListItem Value="1">Fatto</asp:ListItem>
											<asp:ListItem Value="2">Da Fare</asp:ListItem>
											<asp:ListItem Value="3">Non Necessario</asp:ListItem>
										</asp:dropdownlist>
                                     </div>
								</div>
								<div class="row"> 
									<div class="collable" style="width:30%">
                                    <asp:Label ID="Label9" CssClass="bold" runat="server" AssociatedControlID="ddlAnnoCorsoFormazione" Text="Anno Aggiornamento Formazione"></asp:Label>
                                    </div>
									<div class="colOggetti" style="width:70%">
                                    <asp:dropdownlist id="ddlAnnoCorsoFormazione" runat="server" ></asp:dropdownlist>
                                    </div>
								</div>
								
						
                             </div>
                             <br />

                            <div class="RigaPulsanti" style="width:100%;text-align:right">
											
                                                <asp:button  id="cmdRicerca" tabIndex="9" runat="server" CssClass="Pulsante" Text="Ricerca"  ToolTip="Ricerca"></asp:button>
                                                <asp:button id="imgStampa" text="Esporta CSV" Visible="False" CssClass="Pulsante" ToolTip="Esporta csv" Runat="server"></asp:button>
                                                <asp:button  id="cmdChiudi" tabIndex="10" runat="server"  CssClass="Pulsante" Text="Chiudi" ToolTip="Chiudi"></asp:button> 
                                                
							
                            
                             <asp:HyperLink ID="ApriCSV1"   AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" 
        Text="DOWNLOAD CSV" runat="server" 
        ForeColor="#003399" Visible="False"></asp:HyperLink>
                            </div>

  <p>                          
<asp:label id="lblmessaggio" runat="server"  CssClass="msgErrore"></asp:label>
</p>


							<div class="wrapper" runat="server" visible="false" id="Table4" style="width:100%">
                            <p class="bold" style="text-align:center">
<asp:Label ID="Label2" runat="server" CssClass="bold" AssociatedControlID="dgRisultatoRicerca" Text="Risultato Ricerca"></asp:Label>
</p>
								<div class="row"> 
									<div class="colOggetti" style="width:25%">
                                    <asp:checkbox id="chkSelDesel" CssClass="bold" runat="server" Visible="False" 
											  TextAlign="left" Text="Seleziona tutto" AutoPostBack="True"></asp:checkbox>
                                      </div>
									<div class="collable" style="width:30%">
                                    <asp:label id="LblAnnoCorsoForm" CssClass="bold" AssociatedControlID="ddlAggiornaAnnoCorsoFor" runat="server" Visible="False">Anno Aggiornamento Formazione</asp:label>
                                    </div>
									<div class="colOggetti" style="width:20%">
										<asp:dropdownlist id="ddlAggiornaAnnoCorsoFor" runat="server"  Visible="False"></asp:dropdownlist></div>
									<div class="colOggetti" style="width:25%">
                                    <asp:button  id="cmdAssegna" CssClass="Pulsante" tabIndex="9" runat="server" 
											 ToolTip="Aggiorna Anno Corso Formazione" Text="Assegna Anno" Visible="False"></asp:button>

                                    </div>
								</div>


								<div class="row"> 
                                <div class="colOggetti" style="width:25%"> &nbsp;</div>
									<div class="collable" style="width:30%">
                                    <asp:label id="LblCorsoAggFormazione" runat="server" AssociatedControlID="ddlAggiornaCorsoFor" CssClass="bold" Visible="False">Corso Aggiornamento Formazione</asp:label>
                                    </div>
									<div class="colOggetti" style="width:20%">
										<asp:dropdownlist id="ddlAggiornaCorsoFor" runat="server"  Visible="False" Height="23px">
											<asp:ListItem Value="0">Selezionare</asp:ListItem>
											<asp:ListItem Value="1">Fatto</asp:ListItem>
											<asp:ListItem Value="2">Da Fare</asp:ListItem>
											<asp:ListItem Value="3">Non Necessario</asp:ListItem>
										</asp:dropdownlist>
									</div>
									<div class="colOggetti" style="width:25%">
                                    <asp:button  id="imgAssegnaCorso"  CssClass="Pulsante" runat="server" Text="Assegna Corso" ToolTip="Aggiorna Corso Formazione" Visible="False"></asp:button>
											 
                                      </div>
								</div>

								<div class="row"> 
									<div>
										<asp:datagrid CssClass="table" id="dgRisultatoRicerca" runat="server"  
											 ToolTip="Elenco Formatori" CellPadding="0" 
											AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" UseAccessibleHeader="True">
											<SelectedItemStyle BackColor="White"></SelectedItemStyle>
											<EditItemStyle></EditItemStyle>
											<AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
											<ItemStyle  CssClass="tr"></ItemStyle>
											<HeaderStyle></HeaderStyle>
											<FooterStyle></FooterStyle>
											<Columns>
												<asp:TemplateColumn HeaderText="Selez." > 
													<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
													<ItemStyle HorizontalAlign="Center"></ItemStyle>
													<ItemTemplate>
														<asp:CheckBox id="chkCorso" TextAlign="left" Text="Sel" toolTip="Sel"  AutoPostBack="False" runat="server"></asp:CheckBox>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:BoundColumn DataField="Ente" HeaderText="Ente">
													<HeaderStyle Width="40%"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="Competenza" HeaderText="Competenza">
													<HeaderStyle Width="2px"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="Formatore" HeaderText="Formatore">
													<HeaderStyle Width="25%"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="DataNascita" HeaderText="Data Nascita" DataFormatString="{0:d}">
													<HeaderStyle Width="2%"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="Comune" HeaderText="Comune ">
													<HeaderStyle Width="10%"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="CorsoFormazione" HeaderText="Corso Formazione">
													<HeaderStyle Width="1%"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="AnnoCorsoAggiornamento" HeaderText="Anno Aggiornamento Formazione">
													<HeaderStyle Width="1%"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="identepersonale" HeaderText="identepersonale">
													<HeaderStyle Width="1%"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="identepersonaleruolo" HeaderText="identepersonaleruolo"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="idente" HeaderText="idente"></asp:BoundColumn>
											</Columns>
											<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
										</asp:datagrid>
                                     </div>
								</div>
							</div>

                   


</fieldset>		
</asp:Content>
