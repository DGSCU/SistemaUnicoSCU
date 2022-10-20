<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmSostituzioneOlp.aspx.vb" Inherits="Futuro.WfrmSostituzioneOlp" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <script src="Scripts/JHelper.js" type="text/javascript"></script>

    <style type="text/css"> .ui-datepicker {font-size: 11px;}</style>

    <script  type="text/javascript">

        var prefissoIdClient = "MainContent_";

        function CostruisciId(IdServer) {
            var IdClient = prefissoIdClient + IdServer;
            return IdClient;
        };

        $(function () {
            var DataNascitaIns = CostruisciId("txtDataNascitaIns");
            var readonlyDataNascitaIns = '<%=txtDataNascitaIns.ReadOnly%>';
            if (readonlyDataNascitaIns == "False") {
                var sharpDataNascitaIns = "#" + DataNascitaIns
                $("" + sharpDataNascitaIns + "").datepicker();
            }
        });

    </script>

    <script  type="text/javascript">
        function ShowPopUp(popupDaAprire) {
            $find(popupDaAprire).show();
            return false;
        }

        function PopupInsOlp() {
            //        usato all'interno del popup Selezione Olp Subentrante per switchare a quello di inserimento
            $find('popUpControlSelezioneOlpSubentrante').hide();
            $find('popUpControlInserimentoOlp').show();
            return false;                       
        }

        function OpenWindowInfoProgettiAttivi() {
            var idEntePersonale = CostruisciId("hfIdEntePersonaleSubentrante");
            mywin = window.open("dettaglirisorsa.aspx?idruolo=1&idrisorsa=" + document.getElementById(idEntePersonale).value, "", "height=500,toolbar=no,location=no,menubar=no,scrollbars=yes,resizable=yes");

        };

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:HiddenField runat="server" ID="hfVengoDa" />
    <fieldset class="ContornoPagina">
        <legend>Sostituzione OLP</legend>
        <div class="wrapper" style="width:100%">
            <div class="headers" >
                <h2><asp:Label ID="lblTitolo" runat="server"  Text="Sostituzione OLP"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <asp:label id="lblmess"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
            <div class="RigaVuota" >&nbsp;</div>
            <div id="rowStato" class="row" runat="server">
                <div class="collable" style="width:15%">      
                &nbsp;<asp:Label ID="lblStato" CssClass="label" AssociatedControlID="txtStato" runat="server" Text="Stato"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtStato" CssClass="textbox" runat="server" Enabled="false"></asp:TextBox>         
                </div> 
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblMotivoStato" CssClass="label" AssociatedControlID="txtMotivoStato" runat="server" Text="Motivazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtMotivoStato" CssClass="textbox" runat="server" TextMode="MultiLine" Rows="3" Enabled="false" style="width:91%;resize:none"></asp:TextBox>         
                </div>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">      
                &nbsp;<asp:Label ID="lblProgetto" CssClass="label" AssociatedControlID="txtProgetto" runat="server" Text="Progetto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtProgetto" CssClass="textbox" runat="server" Enabled="false"></asp:TextBox>         
                </div> 
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblSede" CssClass="label" AssociatedControlID="txtSede" runat="server" Text="Sede"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtSede" CssClass="textbox" runat="server" Enabled="false"></asp:TextBox>         
                </div>
            </div>
            <fieldset class="ContornoPagina">
            <legend>OLP da sostituire</legend>
                <div class="wrapper" style="width:100%">
                    <div class="RigaVuota" >&nbsp;</div>
                    <div class="row">
                        <div class="collable" style="width:15%">
                        &nbsp;<asp:Label ID="lblNome" CssClass="label" AssociatedControlID="txtNome" runat="server" Text="Nome"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtNome" CssClass="textbox" runat="server" Enabled="false"></asp:TextBox>         
                        </div> 
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblCognome" CssClass="label" AssociatedControlID="txtCognome" runat="server" Text="Cognome"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtCognome" CssClass="textbox" runat="server" Enabled="false"></asp:TextBox>         
                        </div>
                        <asp:HiddenField runat="server" ID="hfIdEntePersonaleRuoloSostituito" />
                        <asp:HiddenField runat="server" ID="hfCodiceFiscaleSostituito" />
                        <asp:HiddenField runat="server" ID="hfIdSostituzioneOLP" />
                    </div>
                    <div class="row">
                        <div class="collable" style="width:15%">      
                        &nbsp;<asp:Label ID="lblDataNascita" CssClass="label" AssociatedControlID="txtDataNascita" runat="server" Text="Data di nascita"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtDataNascita" CssClass="textbox" runat="server" Width="75px" Enabled="false"></asp:TextBox>         
                        </div> 
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblComune" CssClass="label" AssociatedControlID="txtComune" runat="server" Text="Comune di nascita"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtComune" CssClass="textbox" runat="server" Enabled="false"></asp:TextBox>         
                        </div>
                    </div>
                </div>
            </fieldset>
            <fieldset class="ContornoPagina">
                <legend>Motivo della sostituzione</legend>
                <asp:Label ID="lblMessaggioSost" runat="server" CssClass="msgErrore"></asp:Label>
                <div class="wrapper" style="width:100%">
                    <div class="RigaVuota" >&nbsp;</div>
                    <div class="row">
                        <div class="collable" style="width:15%">      
                       &nbsp;<asp:Label ID="lblMotivazione" CssClass="label" AssociatedControlID="ddlMotivazione" runat="server" Text="Tipo"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:DropDownList ID="ddlMotivazione" runat="server" CssClass="ddlClass" AutoPostBack="true">
                                <asp:ListItem Value="0">Rinuncia</asp:ListItem>
				                <asp:ListItem Value="1">Altra motivazione</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="RigaVuota" >&nbsp;</div>
				    <div class="row" runat="server" id="rowNoRINUNCIA">
					    <div class="collable" style="width: 15%">
						&nbsp;<asp:Label ID="lblRINUNCIA" CssClass="label" AssociatedControlID="fileRINUNCIA" runat="server" Text="(*)PDF Rinuncia"></asp:Label>
					    </div>
					    <div class="colOggetti" style="width: 35%">
						    <asp:Button ID="cmdCaricaFileRINUNCIA" runat="server" OnClientClick="return ShowPopUp('popUpControlRINUNCIA')" CssClass="Pulsante" Text="Carica File"/> 
					    </div>
				    </div>
				    <div class="row" runat="server" id="rowRINUNCIA">
					    <table class="table" cellspacing="0" cellpadding="2" rules="all" border="1" id="MainContent_dtgConsultaDocumenti" style="font-size: Small; width: 100%; border-collapse: collapse;">
						    <caption>
							    PDF RINUNCIA
						    </caption>
						    <tbody>
							    <tr>
								    <th scope="col">Download</th>
								    <th scope="col">Nome File</th>
								    <th scope="col">Data Inserimento</th>
								    <th scope="col">Hash Value</th>
								    <th scope="col"></th>
							    </tr>
							    <tr class="tr" align="center" runat="server" id="rowAllegato">
								    <td>
									    <asp:ImageButton runat="server" ID="btnDownloadRINUNCIA" ImageUrl="images/giu_small.png" AlternateText="Scarica PDF Rinuncia" ToolTip="Scarica Rinuncia" />
								    </td>
								    <td>
									    <asp:Literal runat="server" ID="txtRINUNCIAFilename"></asp:Literal>
								    </td>
								    <td>
									    <asp:Literal runat="server" ID="txtRINUNCIAData"></asp:Literal></td>
								    <td>
									    <asp:Literal runat="server" ID="txtRINUNCIAHash"></asp:Literal></td>
								    <td>
									    <asp:ImageButton runat="server" ID="btnModificaRINUNCIA" OnClientClick="return ShowPopUp('popUpControlRINUNCIA')" ImageUrl="images/ripristina_small.png" AlternateText="Modifica Rinuncia" ToolTip="Modifica Rinuncia" />
									    <asp:ImageButton runat="server" ID="btnEliminaRINUNCIA" ImageUrl="images/canc_small.png" AlternateText="Elimina Rinuncia" ToolTip="Elimina Rinuncia" />
								    </td>
							    </tr>
						    </tbody>
					    </table>
				    </div>
                    <div class="row" runat="server" id="rowAltraMotivazione">
                        <div class="collable" style="width:15%">      
                        &nbsp;<asp:Label ID="Label1" CssClass="label" AssociatedControlID="txtAltraMotivazione" runat="server" Text="Motivazione"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:85%">       
                            <asp:TextBox ID="txtAltraMotivazione" CssClass="textbox" runat="server"></asp:TextBox>         
                        </div> 
                    </div>
                </div>
            </fieldset>
            <fieldset class="ContornoPagina">
                <legend>OLP Subentrante</legend>
                <div class="wrapper" style="width:100%">
                    <div class="RigaVuota" >&nbsp;</div>
                    <div class="row">
                        <div class="collable" style="width:15%">
                        &nbsp;<asp:Label ID="lblNomeNew" CssClass="label" AssociatedControlID="txtNomeNew" runat="server" Text="Nome"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtNomeNew" CssClass="textbox" runat="server" Enabled="false"></asp:TextBox>         
                        </div> 
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblCognomeNew" CssClass="label" AssociatedControlID="txtCognomeNew" runat="server" Text="Cognome"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtCognomeNew" CssClass="textbox" runat="server" Enabled="false"></asp:TextBox>         
                        </div>
                        <asp:HiddenField runat="server" ID="hfIdEntePersonaleRuoloSubentrante" />
                        <asp:HiddenField runat="server" ID="hfIdEntePersonaleSubentrante" />
                        <asp:HiddenField runat="server" ID="hfCodiceFiscaleSubentrante" />
                        <asp:HiddenField runat="server" ID="hfCvObbligatorio" />
                    </div>
                    <div class="row">
                        <div class="collable" style="width:15%">      
                        &nbsp;<asp:Label ID="lblDataNascitaNew" CssClass="label" AssociatedControlID="txtDataNascitaNew" runat="server" Text="Data di nascita"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtDataNascitaNew" CssClass="textbox" runat="server" Width="75px" Enabled="false"></asp:TextBox>         
                        </div> 
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblComuneNew" CssClass="label" AssociatedControlID="txtComuneNew" runat="server" Text="Comune di nascita"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtComuneNew" CssClass="textbox" runat="server" Enabled="false"></asp:TextBox>         
                        </div>
                    </div>
                    <div class="row" id="divInfoProgetti" runat="server" visible="false">
                        <div class="colHyperLink" style="width:50%">
                            <asp:LinkButton ID="lblInfoProgetti" runat="server" style="cursor:pointer" Text="Info Progetti" Visible="true" OnClientClick="javascript:OpenWindowInfoProgettiAttivi()"></asp:LinkButton> <br />
                        </div>
                    </div>
                    <div class="RigaPulsanti">
                        <asp:Button ID="btnSelezionaOLP" runat="server" CssClass="Pulsante" Text="Seleziona OLP" OnClientClick="return ShowPopUp('popUpControlSelezioneOlpSubentrante')" />&nbsp;
                    </div>
                    <div class="RigaVuota" >&nbsp;</div>
                    <asp:Label ID="lblMessaggioCV" runat="server" CssClass="msgErrore"></asp:Label>
                    <div class="row" runat="server" id="rowNoCV">
                    <p>Scarica <a href="https://www.serviziocivile.gov.it/modulistica-unsc/modulistica-enti/moduli-programmi-di-intervento-e-progetti-di-servizio-civile-universale/allegato-5a-5b-e-5c-fac-simile-curricula-risorse-umane-figure-professionali.aspx" target="_blank" >qui</a> il Fac–Simile curriculum.<br />Il curriculum deve comprendere una copia di documento di riconoscimento in corso di validità.</p>
					    <div class="collable" style="width: 15%">
						&nbsp;<asp:Label ID="lblCV" CssClass="label" AssociatedControlID="fileCV" runat="server" Text="(*)CV OLP Subentrante"></asp:Label>
					    </div>
					    <div class="colOggetti" style="width: 35%">
						    <asp:Button ID="cmdCaricaFileCV" runat="server" OnClientClick="return ShowPopUp('popUpControlCV')" CssClass="Pulsante" Text="Carica File"/> 
					    </div>
				    </div>
				    <div class="row" runat="server" id="rowCV">
                    <p>Scarica <a href="https://www.politichegiovanili.gov.it/modulistica/modulistica-enti/moduli-programmi-di-intervento-e-progetti-di-servizio-civile-universale/" target="_blank" >qui</a> il Fac–Simile curriculum.<br />Il curriculum deve comprendere una copia di documento di riconoscimento in corso di validità.</p>
					    <table class="table" cellspacing="0" cellpadding="2" rules="all" border="1" id="Table1" style="font-size: Small; width: 100%; border-collapse: collapse;">
						    <caption>
							    CV OLP Subentrante
						    </caption>
						    <tbody>
							    <tr>
								    <th scope="col">Download</th>
								    <th scope="col">Nome File</th>
								    <th scope="col">Data Inserimento</th>
								    <th scope="col">Hash Value</th>
								    <th scope="col"></th>
							    </tr>
							    <tr class="tr" align="center" runat="server" id="Tr1">
								    <td>
									    <asp:ImageButton runat="server" ID="btnDownloadCV" ImageUrl="images/giu_small.png" AlternateText="Scarica CV" ToolTip="Scarica CV" />
								    </td>
								    <td>
									    <asp:Literal runat="server" ID="txtCVFilename"></asp:Literal>
								    </td>
								    <td>
									    <asp:Literal runat="server" ID="txtCVData"></asp:Literal></td>
								    <td>
									    <asp:Literal runat="server" ID="txtCVHash"></asp:Literal></td>
								    <td>
									    <asp:ImageButton runat="server" ID="btnModificaCV" OnClientClick="return ShowPopUp('popUpControlCV')" ImageUrl="images/ripristina_small.png" AlternateText="Modifica CV" ToolTip="Modifica CV" />
									    <asp:ImageButton runat="server" ID="btnEliminaCV" ImageUrl="images/canc_small.png" AlternateText="Elimina CV" ToolTip="Elimina CV" />
								    </td>
							    </tr>
						    </tbody>
					    </table>
				    </div>

                </div>
            </fieldset>
            <div class="RigaVuota" >&nbsp;</div>
            <asp:label id="lblSalva"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdValidaPositivamente" runat="server" CssClass="Pulsante" Text="Valida positivamente" visible="false"/>&nbsp;
                <asp:Button ID="cmdValidaNegativamente" runat="server" CssClass="Pulsante" Text="Valida negativamente" visible="false"/>&nbsp;
                <asp:Button ID="cmdElimina" runat="server" CssClass="Pulsante" Text="Elimina" />&nbsp;
                <asp:Button ID="cmdConferma" runat="server" CssClass="Pulsante" Text="Salva" />&nbsp;
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
            </div>
        </div>

        <!--POPUP File--RINUNCIA Inizio-->
        <asp:ModalPopupExtender runat="server" ID="popUploadRINUNCIA" TargetControlID="hfPopupRINUNCIA" 
            PopupControlID="pnlUploadRINUNCIA" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
            PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpControlRINUNCIA" DynamicServicePath="" Enabled="True" CancelControlID="btnCloseUploadRINUNCIA"> 
        </asp:ModalPopupExtender>
	    <asp:Panel ID="pnlUploadRINUNCIA" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
            <asp:HiddenField runat="server" ID="hfPopupRINUNCIA" />
            <asp:label id="lblErroreUploadRINUNCIA"  runat="server" CssClass="msgErrore" Visible="False" AssociatedControlID="cmdAllegaRINUNCIA"></asp:label>
            <p class="msgInfo">Caricamento Rinuncia</p>
            <fieldset>
			    <input id="fileRINUNCIA" type="file" runat="server" />
                <div style="margin-top: 1em;"></div>
                <asp:Button ID="cmdAllegaRINUNCIA" runat="server" CssClass="Pulsante" Text="Carica File"/>
                <asp:Button ID="btnCloseUploadRINUNCIA" runat="server" CssClass="Pulsante" Text="Chiudi"/>
            </fieldset>
	    </asp:Panel>
        <!--POPUP File--RINUNCIA Fine-->

        <!--POPUP selezione OLP subentrante - Inizio-->
        <asp:ModalPopupExtender runat="server" ID="popSelezioneOlpSubentrante" TargetControlID="hfPopupSelezioneOlpSubentrante" 
            PopupControlID="pnlSelezioneOlpSubentrante" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
            PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpControlSelezioneOlpSubentrante" DynamicServicePath="" Enabled="True" CancelControlID="btnCloseSelezioneOlpSubentrante"> 
        </asp:ModalPopupExtender>
	    <asp:Panel ID="pnlSelezioneOlpSubentrante" runat="server" Style="background-color: White; padding: 20px; width: 120vh; overflow-y: auto; max-height: 90vh">
            <asp:HiddenField runat="server" ID="hfPopupSelezioneOlpSubentrante" />
            <asp:label id="lblErroreSelezioneOlpSubentrante"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
            <p class="msgInfo">Selezione OLP subentrante</p>
            <fieldset>
                <legend>Ricerca OLP</legend>
                <div class="wrapper" style="width:100%">
                    <asp:label id="lblMessaggioInfo"  runat="server" CssClass="msgInfo" >Premere il pulsante RICERCA per ottenere la lista degli OLP disponibili.</asp:label>
                    <div class="RigaVuota" >&nbsp;</div>
                    <div class="row">
                        <div class="collable" style="width:15%">
                        &nbsp;<asp:Label ID="lblNomeRicerca" CssClass="label" AssociatedControlID="txtNomeRicerca" runat="server" Text="Nome"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtNomeRicerca" CssClass="textbox" runat="server"></asp:TextBox>         
                        </div> 
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblCognomeRicerca" CssClass="label" AssociatedControlID="txtCognomeRicerca" runat="server" Text="Cognome"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtCognomeRicerca" CssClass="textbox" runat="server"></asp:TextBox>         
                        </div>
                        <asp:HiddenField runat="server" ID="hfCodiceFiscaleRicerca" />
                    </div>
                    <div class="row">
                        <div class="collable" style="width:15%">      
                        &nbsp;<asp:Label ID="lblComuneRicerca" CssClass="label" AssociatedControlID="txtComuneRicerca" runat="server" Text="Comune di nascita"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtComuneRicerca" CssClass="textbox" runat="server"></asp:TextBox>         
                        </div>
                    </div>
                    <div class="RigaPulsanti">
                        <asp:Button ID="btnInsOlp" runat="server" CssClass="Pulsante" Text="Inserisci OLP" OnClientClick="return PopupInsOlp()"/>
                        <asp:Button ID="btnRicercaOLP" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                    </div>
                </div>
                <asp:datagrid id="dgRisultatoRicerca" Caption="Risultato Ricerca Risorse Progetto"   runat="server" Width="100%" ToolTip="Risorse Progetto" 
                                AllowPaging="true" PageSize="5"  CssClass="table" AllowSorting="false"  AutoGenerateColumns="False" UseAccessibleHeader="True" 
                                Visible="true">
	                <FooterStyle></FooterStyle>
	                <SelectedItemStyle  BackColor="White" ></SelectedItemStyle>
	                <EditItemStyle></EditItemStyle>
	                <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	                <ItemStyle CssClass="tr" HorizontalAlign ="Center"></ItemStyle>
	                <HeaderStyle></HeaderStyle>
                    <Columns>
			            <asp:BoundColumn DataField="nominativo" HeaderText="Nominativo">
			            </asp:BoundColumn>
			            <asp:BoundColumn DataField="datanascita" HeaderText="Data Nascita" DataFormatString="{0:d}">
			            </asp:BoundColumn>
			            <asp:BoundColumn DataField="denominazione" HeaderText="Comune Nascita">
			            </asp:BoundColumn>
			            <asp:BoundColumn DataField="telefono" HeaderText="Telefono">
			            </asp:BoundColumn>
			            <asp:BoundColumn DataField="email" HeaderText="Email">
			            </asp:BoundColumn>
			            <asp:BoundColumn Visible="False" DataField="idass" HeaderText="idAss">
				            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			            </asp:BoundColumn>
			            <asp:BoundColumn Visible="False" DataField="accreditato" HeaderText="Accreditato"></asp:BoundColumn>
			            <asp:BoundColumn Visible="False" DataField="identePersonaleRuolo" HeaderText="identePersonaleRuolo"></asp:BoundColumn>
			            <asp:BoundColumn Visible="False" DataField="identesedeattuazione" HeaderText="idSedeAttuazione"></asp:BoundColumn>
			            <asp:BoundColumn Visible="False" DataField="trovaChec" HeaderText="trovaChec"></asp:BoundColumn>
			            <asp:TemplateColumn HeaderText="Corso OLP da frequentare">
				            <ItemTemplate>
					            SI<asp:CheckBox id="chkCorsoOlp" toolTip="Corso OLP SI" Text="&nbsp;" runat="server"></asp:CheckBox>
                                NO<asp:CheckBox id="chkCorsoOlpNo" toolTip="Corso OLP NO" Text="&nbsp;" runat="server"></asp:CheckBox>
				            </ItemTemplate>
			            </asp:TemplateColumn>
			            <asp:BoundColumn Visible="False" DataField="CorsoOLP" HeaderText="CorsoOLP"></asp:BoundColumn>
			            <asp:BoundColumn Visible="False" DataField="DataFine" HeaderText="entepersonaleruoli.datafinevalidit&#224;"></asp:BoundColumn>
			            <asp:BoundColumn Visible="False" DataField="UserNameInseritore" HeaderText="UserNameInseritore"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Caricamento CV (*)">
				            <ItemTemplate>
					            <asp:CheckBox id="ChkCaricamentoCV" Text="&nbsp;" Checked="true" toolTip="(*)CaricamentoCV" runat="server"></asp:CheckBox>
				            </ItemTemplate>
			            </asp:TemplateColumn>
			            <asp:BoundColumn Visible="False" DataField="DataInseritore" HeaderText="Data Inserimento" DataFormatString="{0:d}"></asp:BoundColumn>
			            <asp:BoundColumn Visible="False" DataField="ControlloUsabilitaOLP" HeaderText="ControlloUsabilitaOLP"></asp:BoundColumn>
                        <asp:TemplateColumn  > 
                            <ItemTemplate >
				                <asp:ImageButton ID="InfoRisorsa" CommandName="InfoOLP" style="cursor:pointer" AlternateText="Verifica OLP" ToolTip="Verifica OLP" runat="server" ImageUrl="images/info_small.png" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn  > 
                            <ItemTemplate >
				                <asp:ImageButton ID="selezionaOLP" CommandName="SelezionaOLP" style="cursor:pointer" AlternateText="Seleziona OLP" ToolTip="Seleziona OLP" runat="server" ImageUrl="images/valida_small.png" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
			            <asp:BoundColumn Visible="False" DataField="codicefiscale" HeaderText="codicefiscale"></asp:BoundColumn>
			            <asp:BoundColumn Visible="False" DataField="Cognome" HeaderText="Cognome"></asp:BoundColumn>
			            <asp:BoundColumn Visible="False" DataField="Nome" HeaderText="Nome"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="denominazione" HeaderText="Comune"></asp:BoundColumn>
			            <asp:BoundColumn Visible="False" DataField="identePersonale" HeaderText="identePersonale"></asp:BoundColumn>
		            </Columns>
		            <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  
                                Mode="NumericPages"></PagerStyle>
                </asp:datagrid>
                <p>
                    <asp:Label ID="lblNota" CssClass="label" runat="server" 
                        Text="(*)Nota: Si fa presente che per il campo 'Caricamento CV' deselezionando la spunta si dichiara che l’OLP ha già operato su progetti del medesimo settore e non si intende caricare sul sistema il suo CV in quanto già acquisito in passato. " 
                        Visible="False"></asp:Label>
                </p>
                <div style="margin-top: 1em;"></div>
                <asp:Button ID="btnCloseSelezioneOlpSubentrante" runat="server" CssClass="Pulsante" Text="Chiudi"/>
            </fieldset>
	    </asp:Panel>
        <!--POPUP selezione OLP subentrante - Fine-->

        <!--POPUP File--CV Inizio-->
        <asp:ModalPopupExtender runat="server" ID="popUploadCV" TargetControlID="hfPopupCV" 
            PopupControlID="pnlUploadCV" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
            PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpControlCV" DynamicServicePath="" Enabled="True" CancelControlID="btnCloseUploadCV"> 
        </asp:ModalPopupExtender>
	    <asp:Panel ID="pnlUploadCV" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
            <asp:HiddenField runat="server" ID="hfPopupCV" />
            <asp:label id="lblErroreUploadCV"  runat="server" CssClass="msgErrore" Visible="False" AssociatedControlID="cmdAllegaCV"></asp:label>
            <p class="msgInfo">Caricamento CV</p>
            <fieldset>
			    <input id="fileCV" type="file" runat="server" />
                <div style="margin-top: 1em;"></div>
                <asp:Button ID="cmdAllegaCV" runat="server" CssClass="Pulsante" Text="Carica File"/>
                <asp:Button ID="btnCloseUploadCV" runat="server" CssClass="Pulsante" Text="Chiudi"/>
            </fieldset>
	    </asp:Panel>
        <!--POPUP File--CV Fine-->

        <!--POPUP inserimento OLP - Inizio-->
        <asp:ModalPopupExtender runat="server" ID="popInserimentoOlp" TargetControlID="hfPopupInserimentoOlp" 
            PopupControlID="pnlInserimentoOlp" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
            PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpControlInserimentoOlp" DynamicServicePath="" Enabled="True"> 
        </asp:ModalPopupExtender>
	    <asp:Panel ID="pnlInserimentoOlp" runat="server" Style="background-color: White; padding: 20px; width: 120vh; overflow-y: auto; max-height: 90vh">
            <asp:HiddenField runat="server" ID="hfPopupInserimentoOlp" />
            <asp:label id="lblErroreInserimentoOlp"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
			<p class="msgInfo">Inserimento OLP</p>
			<fieldset>
				<legend>Inserimento OLP</legend>
				<div class="wrapper" style="width:100%">
					<div class="RigaVuota" >&nbsp;</div>
					 <div class="row">
						<div class="collable" style="width:15%">
							<asp:Label ID="LblCognomeIns" CssClass="label" runat="server" AssociatedControlID="txtCognomeIns" Text="Cognome"></asp:Label>
						</div>
						<div class="colOggetti" style="width:35%">
							<asp:TextBox ID="txtCognomeIns" CssClass="textbox" autofocus="true"  runat="server" MaxLength="100" ></asp:TextBox>
						</div>           
						<div class="collable" style="width:15%">
							<asp:Label ID="LblNomeIns"  runat="server" CssClass="label" Text="Nome" AssociatedControlID="txtNomeIns"></asp:Label>
						</div>
						<div class="colOggetti" style="width:35%">
							<asp:TextBox ID="txtNomeIns" CssClass="textbox" runat="server" MaxLength="100"></asp:TextBox>
						</div>
					</div>
					<div class="row">
					   <div class="collable" style="width:15%">
							<asp:Label ID="LblCodFiscIns" CssClass="label" runat="server" Text="Codice Fiscale" AssociatedControlID="txtCodiceFiscaleIns"></asp:Label>
						</div>
						<div class="colOggetti" style="width:35%">
							<asp:TextBox ID="txtCodiceFiscaleIns" CssClass="textbox" runat="server" Style="text-transform: uppercase" MaxLength="16"  ></asp:TextBox>
						</div>
							  <div class="collable" style="width:15%">
							<asp:Label ID="LblDataNascIns" CssClass="label" runat="server" Text="Data di Nascita" AssociatedControlID="txtDataNascitaIns"></asp:Label>
						</div>
						<div class="colOggetti" style="width:35%">
							<asp:TextBox ID="txtDataNascitaIns" CssClass="textboxData" runat="server" Width="75px" MaxLength="10"></asp:TextBox>
						</div>
					</div>
					<div class="row">
						<div class="collable" style="width:15%">
							<asp:Label ID="LblProvinciaNascitaIns" CssClass="label" runat="server" Text="Provincia/Nazione di Nascita" AssociatedControlID="ddlProvinciaNascitaIns"></asp:Label>
						</div>
						  <div class="colOggetti" style="width:35%">
							<asp:dropdownlist id="ddlProvinciaNascitaIns" CssClass="ddlClass" AutoPostBack="true" runat="server"></asp:dropdownlist>
						</div>
						<div class="collable" style="width:50%">
							 <asp:CheckBox ID="chkEsteroNascitaIns" AutoPostBack="true" Text="Estero" ToolTip="Flag Estero Nazione di Nascita" runat="server" /> 
						</div>
					</div>
					<div class="row">
						<div class="collable" style="width:15%">
							<asp:Label ID="LblComuneNascitaIns" CssClass="label" runat="server" Text="Comune di Nascita" AssociatedControlID="ddlComuneNascitaIns"></asp:Label>
						</div>
						<div class="colOggetti" style="width:35%">
							<asp:dropdownlist id="ddlComuneNascitaIns" CssClass="ddlClass" runat="server" Enabled="false" >
								<asp:ListItem Text="Selezionare Provincia/Nazione di Nascita"></asp:ListItem>
							</asp:dropdownlist>
						</div>
					</div>
					<div class="RigaPulsanti">
						<asp:Button ID="btnInserisciOlp" runat="server" CssClass="Pulsante" Text="Inserisci" />&nbsp;
					</div>
				</div>
				<div style="margin-top: 1em;"></div>
				<asp:Button ID="btnCloseInserimentoOlp" runat="server" CssClass="Pulsante" Text="Chiudi"/>
			</fieldset>
	    </asp:Panel>
        <!--POPUP inserimento OLP - Fine-->
        
        <!--POPUP Motivo rifiuto Inizio-->
        <asp:ModalPopupExtender runat="server" ID="popupMotivoRifiuto" TargetControlID="hfPopupMotivoRifiuto" 
            PopupControlID="pnlMotivoRifiuto" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
            PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpControlRifiuto" DynamicServicePath="" Enabled="True" CancelControlID="btnCloseMotivoRifiuto"> 
        </asp:ModalPopupExtender>
	    <asp:Panel ID="pnlMotivoRifiuto" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh;width: 80vh">
            <asp:HiddenField runat="server" ID="hfPopupMotivoRifiuto" />
            <asp:label id="lblErroreMotivoRifiuto"  runat="server" CssClass="msgErrore" Visible="False" AssociatedControlID="cmdRifiuta"></asp:label>
            <p class="msgInfo">Sostituzione OLP: esito NEGATIVO</p>
<%--            <fieldset>--%>
				<div class="wrapper" style="width:100%">
					 <div class="RigaVuota" >&nbsp;</div>
					 <div class="row">
						<div class="collable" style="width:15%">
						&nbsp;<asp:Label ID="Label2" CssClass="label" runat="server" AssociatedControlID="txtMotivoRifiuto" Text="Motivazione"></asp:Label>
						</div>
						<div class="colOggetti" style="width:85%">
							<asp:TextBox ID="txtMotivoRifiuto" CssClass="textbox" autofocus="true" 
                                runat="server" TextMode="MultiLine" Rows="3" MaxLength="1000" style = "resize:none;width: 64vh"></asp:TextBox>
						</div>           
					</div>
					<div class="RigaVuota" >&nbsp;</div>
                </div>
                <asp:Button ID="cmdRifiuta" runat="server" CssClass="Pulsante" 
                Text="Valida negativamente"/>
                <asp:Button ID="btnCloseMotivoRifiuto" runat="server" CssClass="Pulsante" Text="Chiudi"/>
<%--            </fieldset>--%>
	    </asp:Panel>
        <!--POPUP Motivo rifiuto Fine-->
                	
    </fieldset>
</asp:Content>
