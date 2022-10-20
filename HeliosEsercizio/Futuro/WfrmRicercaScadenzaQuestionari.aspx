<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaScadenzaQuestionari.aspx.vb" Inherits="Futuro.WfrmRicercaScadenzaQuestionari" %>
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
        <legend>Scadenzario Caricamento Questionario Formazione Generale</legend>

					<div class="wrapper1" style="width:100%">

									
								
									<div class="row">

										<div class="collable" style="width:15%"><asp:Label ID="lblDataDal" runat="server" AssociatedControlID="txtDataScadenzaDal" Text="Data Scadenza Dal"></asp:Label></div>
									    <div class="colOggetti" style="width:35%"><asp:textbox id="txtDataScadenzaDal"  Runat="server"></asp:textbox></div>
										<div class="collable" style="width:15%"><asp:Label ID="lblAl" AssociatedControlID="txtDataScadenzaAl" runat="server" Text="Data Scadenza Al"></asp:Label></div>
										<div class="colOggetti" style="width:35%"><asp:textbox id="txtDataScadenzaAl"  Runat="server"></asp:textbox></div>
		
									</div>
                                    
                                   
									<%If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then%>
									<div class="row">
										<div class="collable" style="width:15%"><asp:label id="lblCodReg" runat="server"  Text="Cod Ente" ></asp:label></div>
										<div class="colOggetti" style="width:35%"><asp:textbox id="txtCodReg" runat="server"  MaxLength="255"></asp:textbox></div>
                                        <div class="collable" style="width:15%"><asp:label id="lblDenEnte" runat="server" Text="Denominazione Ente"></asp:label></div>
									    <div class="colOggetti" style="width:35%"><asp:textbox id="txtDenEnte" runat="server"  MaxLength="255"></asp:textbox></div>
									</div>
									<% end if %>
                                    <div class="row">
										
										<div class="collable" style="width:15%"><asp:Label ID="lblCircolare" runat="server" AssociatedControlID="DdlBando" Text="Circolare"></asp:Label></div>
										<div class="colOggetti" style="width:35%"><asp:dropdownlist id="DdlBando" runat="server"  AutoPostBack="False"></asp:dropdownlist></div>
										 <div class="collable" style="width:15%"></div>
                                         <div  class="colOggetti" style="width:35%"><asp:checkbox id="chkEscludi" runat="server"  Checked="True" Text="Escludi scaduti"></asp:checkbox></div>
											
                                    </div>

									<div class="RigaPulsanti">
					
											<asp:button id="cmdSalva" text="Ricerca" CssClass="Pulsante" runat="server" 
												ToolTip="Esegui Ricerca" />
											<asp:button id="cmdChiudi" text="Chiudi" CssClass="Pulsante" runat="server" 
												ToolTip="Esci" />
									</div>
					
							</div>
							
                       
					<p>
                    <asp:label id="lblmessaggio" runat="server" Font-Names="Verdana" Font-Size="X-Small" Font-Bold="True"
								ForeColor="Red" Height="8px"></asp:label>
                                </p>
					

					<div class="wrapper1" style="width:100%">

								
								<div class="row">
									<asp:datagrid id="dgRisultatoRicerca" Width="100%" Caption="SCADENZARIO QUESTIONARIO" runat="server"  ToolTip="Elenco Progetti"
											 AllowPaging="True"  AutoGenerateColumns="False" AllowSorting="True" CssClass="table"
											CellPadding="3">
											<FooterStyle ></FooterStyle>
											<SelectedItemStyle ></SelectedItemStyle>
											<EditItemStyle ></EditItemStyle>
											<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
											<ItemStyle CssClass="tr"></ItemStyle>
											<HeaderStyle ></HeaderStyle>
											<Columns>
												<asp:BoundColumn DataField="CodiceRegione" HeaderText="Ente"></asp:BoundColumn>
												<asp:BoundColumn DataField="Bando" HeaderText="Bando"></asp:BoundColumn>
												<asp:BoundColumn DataField="TipoFormazioneGeneraleDescr" HeaderText="Tipo Formazione"></asp:BoundColumn>
												<asp:BoundColumn DataField="UltimaDataInizioProgetto" HeaderText="Data Inizio Ultimo Progetto">
													<HeaderStyle ></HeaderStyle>
													<ItemStyle ></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="GiorniRestanti" HeaderText="Giorni alla scadenza">
													<HeaderStyle ></HeaderStyle>
													<ItemStyle ></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="DataScadenza" HeaderText="Data Scadenza">
													<HeaderStyle ></HeaderStyle>
													<ItemStyle ></ItemStyle>
												</asp:BoundColumn>
												<asp:ButtonColumn Text="&lt;img src=images/proroga_small.png title='Proroga' border=0 ;"
													HeaderText="Proroga" CommandName="Proroga"></asp:ButtonColumn>
												<asp:BoundColumn Visible="False" DataField="IdBandoAttivit&#224;" HeaderText="IdBandoAttivit&#224;"></asp:BoundColumn>
											</Columns>
											<PagerStyle NextPageText="Successiva" CssClass="linkPageGridRidotto"
												PrevPageText="Precedente" HorizontalAlign="Center" 
												Mode="NumericPages"></PagerStyle>
										</asp:datagrid>
								</div>
								
                                 <div class="RigaPulsanti">
											<asp:button id="imgStampa" text="Esporta CSV" CssClass="Pulsante" Runat="server" ToolTip="Stampa modulo"
												Visible="False" />
                                                <br />
                                                <asp:HyperLink ID="ApriCSV1"   AccessKey="S" ToolTip="Link per la Stampa OLP su sedi diverse" CssClass="linkStampa" 
                Text="DOWNLOAD CSV" runat="server" 
                ForeColor="#003399" Visible="False"></asp:HyperLink>
                                   </div>
							</div>
						
                </fieldset>
       
</asp:Content>
       