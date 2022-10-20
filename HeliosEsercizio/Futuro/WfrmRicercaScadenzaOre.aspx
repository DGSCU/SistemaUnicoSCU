<%@ Page Title="Scadenzario Caricamento Ore Formazione Generale" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaScadenzaOre.aspx.vb" Inherits="Futuro.WfrmRicercaScadenzaOre" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<link href="Styles/Site.css" rel="stylesheet" type="text/css" />
        <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .ui-datepicker {
                font-size: 11px;
            }
        </style>
            <script  type="text/javascript" language="javascript">

                var formId;
                var prefissoIdClient = "MainContent_";

                function ImpostaFormId() {
                    formId = document.forms[0].id;
                    return formId;
                }
                function CostruisciId(IdServer) {
                    var IdClient = prefissoIdClient + IdServer
                    return IdClient
                }


                $(function () {
                    var IdData = CostruisciId('txtDataScadenzaDal');
                    var sharpIdData = "#" + IdData
                    $("" + sharpIdData + "").datepicker();
                });

                $(function () {
                    var IdData = CostruisciId('txtDataScadenzaAl');
                    var sharpIdData = "#" + IdData
                    $("" + sharpIdData + "").datepicker();
                });

 
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
        <legend>Scadenzario Caricamento Ore Formazione Generale</legend>
<div class="wrapper1" style="width:100%">
					<div class="row">
						
							<div class="wrapper1" style="width:100%">
							
									<div class="row">
										<div class="collable" style="width:15%">
                                            <asp:Label ID="lblTitolo" runat="server" AssociatedControlID="txtTitoloProgetto" Text="Titolo Progetto"></asp:Label></div>
											<div class="colOggetti" style="width:35%"><asp:textbox id="txtTitoloProgetto" runat="server"  MaxLength="255"></asp:textbox></div>
											<div class="collable" style="width:15%">
                                                <asp:Label ID="lblCodProg" runat="server" AssociatedControlID="txtCodProg" Text="Codice Progetto"></asp:Label></div>
											<div class="colOggetti" style="width:35%"><asp:textbox id="txtCodProg" runat="server"  MaxLength="255"></asp:textbox></div>		
					
									</div>
									<div class="row">
										
											<div class="collable" style="width:15%">
                                                <asp:Label ID="lblCircolare" runat="server" AssociatedControlID="DdlBando" Text="Circolare"></asp:Label></div>
										
										<div class="colOggetti" style="width:35%"><asp:dropdownlist id="DdlBando" runat="server" ></asp:dropdownlist></div>
									</div>
									<div class="row">
										
											<div class="collable" style="width:15%">
                                                <asp:Label ID="lblDataDal" runat="server" AssociatedControlID="txtDataScadenzaDal" Text="Data Scadenza Dal"></asp:Label></div>
										
										<div class="colOggetti" style="width:35%">
											<asp:textbox id="txtDataScadenzaDal"  Runat="server"></asp:textbox>&nbsp;</div>
											<div class="collable" style="width:15%">
                                                <asp:Label ID="lblAl" runat="server" AssociatedControlID="txtDataScadenzaAl" Text="Data Scadenza Al"></asp:Label></div>
										<div class="colOggetti" style="width:35%">
											<asp:textbox id="txtDataScadenzaAl"  Runat="server"></asp:textbox>
											</div>
										</div>
							
									<%If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then%>
									
                                    <div class="row">

										<div class="collable" style="width:15%">
											<asp:label id="lblCodReg" AssociatedControlID="txtCodReg" runat="server"
												  >Cod Ente</asp:label>
                                          </div>
										<div class="colOggetti" style="width:35%">
											
											<asp:textbox id="txtCodReg" runat="server"  MaxLength="255"></asp:textbox>
                                            </div>
											<div class="collable" style="width:15%">
                                            <asp:label id="lblDenEnte" AssociatedControlID="txtDenEnte" runat="server" 
												>Denominazione Ente</asp:label>
                                             </div>
                                                <div class="colOggetti" style="width:35%">
											<asp:textbox id="txtDenEnte" runat="server" MaxLength="255"></asp:textbox>
                                         </div>
									</div>

									<% end if %>

									<div class="row">
										<div class="collable" style="width:15%">
											<asp:label  id="lblSanzione" AssociatedControlID="ddlSegnalazioneSanzione"
												 Runat="server">Presenza Sanzione</asp:label>
                                        </div>
										<div class="colOggetti" style="width:35%">
											<asp:dropdownlist id="ddlSegnalazioneSanzione" runat="server">
												<asp:ListItem Selected="True">Tutti</asp:ListItem>
												<asp:ListItem Value="0">No</asp:ListItem>
												<asp:ListItem Value="1">Si</asp:ListItem>
											</asp:dropdownlist>
										</div>
                                        <div class="collable" style="width:15%"></div>
<asp:CheckBox id="chkEscludi" runat="server" Text="Escludi scaduti" Checked="True"></asp:CheckBox>
                                        <div class="colOggetti" style="width:35%"></div>
									</div>
									
									<div class="RigaPulsanti">
											<asp:button id="cmdSalva"  runat="server" text="Ricerca" CssClass="Pulsante" ToolTip="Esegui Ricerca" />
											<asp:button id="cmdChiudi"  runat="server" CssClass="Pulsante" Text="Chiudi"  ToolTip="Esci"  BorderStyle="Outset" />
                                         </div>
							
							</div>
							
					</div>

                            <p>
                            <asp:label id="lblmessaggio" runat="server" CssClass="msgErrore"></asp:label>
                            </p>

					      
					<div class="row">
						
							
									<asp:datagrid id="dgRisultatoRicerca" Caption="ELENCO PROGETTI" runat="server" CssClass="table" ToolTip="Elenco Progetti"
											 BorderStyle="None"  AutoGenerateColumns="False" AllowSorting="True" 
											CellPadding="3" AllowPaging="True">
											<FooterStyle ></FooterStyle>
											<SelectedItemStyle ></SelectedItemStyle>
											<EditItemStyle></EditItemStyle>
											<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
											<ItemStyle CssClass="tr"></ItemStyle>
											<HeaderStyle></HeaderStyle>
											<Columns>
												<asp:BoundColumn DataField="CodiceRegione" HeaderText="Ente"></asp:BoundColumn>
												<asp:BoundColumn DataField="CodiceProgetto" HeaderText="Cod. Progetto"></asp:BoundColumn>
												<asp:BoundColumn DataField="Titolo" HeaderText="Titolo"></asp:BoundColumn>
												<asp:BoundColumn DataField="DataInizioAttivit&#224;" HeaderText="Data Inizio Progetto">
													<HeaderStyle ></HeaderStyle>
													<ItemStyle></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="GiorniRestanti" HeaderText="Giorni alla scadenza">
													<HeaderStyle ></HeaderStyle>
													<ItemStyle ></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="DataScadenza" HeaderText="Data Scadenza">
													<HeaderStyle ></HeaderStyle>
													<ItemStyle ></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="SegnalazioneSanzione" HeaderText="Presenza Sanzione">
													<HeaderStyle ></HeaderStyle>
													<ItemStyle ></ItemStyle>
												</asp:BoundColumn>
											</Columns>
											<PagerStyle NextPageText="Successiva" CssClass="linkPageGridRidotto"
												PrevPageText="Precedente" HorizontalAlign="Center" 
												Mode="NumericPages"></PagerStyle>
										</asp:datagrid>
							
					</div>
                    <div class="RigaPulsanti">
											<asp:button id="imgStampa1" text="Esporta CSV" CssClass="Pulsante" Runat="server" Visible="False" ToolTip="Stampa modulo"  />
                                         <br />
                                         <asp:HyperLink ID="ApriCSV1"   AccessKey="S" ToolTip="Link per la Stampa OLP su sedi diverse" CssClass="linkStampa" 
                Text="DOWNLOAD CSV" runat="server" 
                ForeColor="#003399" Visible="False"></asp:HyperLink>
                                         </div>
				</div>
                </fieldset>
</asp:Content>
