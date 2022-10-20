<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmStrutturaOrganizzativaSistemi.aspx.vb" Inherits="Futuro.WfrmStrutturaOrganizzativaSistemi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script  type="text/javascript">
        function ShowPopUp(popupDaAprire) {
            $find(popupDaAprire).show();
            return false;
        }

        function MaxChar(ctrl, numCaratteri) {
            ctrl.value = ctrl.value.substring(0, numCaratteri)
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <fieldset class="ContornoPagina">
        <legend>Gestione Struttura Organizzativa/Sistemi Funzionali</legend>
        <asp:Label ID="lblMessaggio" runat="server" CssClass="msgErrore"></asp:Label>
        <br />
        <br />
        <div class="RigaVuota">
                &nbsp;</div>
        <div class="wrapper" style="width: 100%">
            <div class="headers">
                <h2>
                    <asp:Label ID="lblTitolo" runat="server" Text="Struttura Organizzativa/Sistemi Funzionali"></asp:Label>
                </h2>
            </div>
            <div class="RigaVuota">
                &nbsp;</div>
                <div class="row">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblSistemaDiComunicazione" CssClass="label" runat="server" Text="(*)Sistema di Comunicazione"
                            AssociatedControlID="TxtSistemaDiComunicazione"></asp:Label>
                        <br /><asp:Button ID="pdfSistemaComunicazione" CssClass="Pulsante" runat="server" ToolTip="esporta in pdf" Text="pdf" Visible="false"/>

                    </div>
                    <div class="colOggetti" style="width: 85%">
                        <asp:TextBox ID="TxtSistemaDiComunicazione" CssClass="textbox" runat="server" ToolTip="Sistema di Comunicazione"
                            TextMode="MultiLine" Rows="5" Width="99%"></asp:TextBox>
                     </div>
                </div>
            <div class="RigaVuota">
                &nbsp;</div>
                <div class="row">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblSistemaDiSelezione" CssClass="label" runat="server" Text="(*)Sistema di Selezione"
                            AssociatedControlID="TxtSistemaDiSelezione"></asp:Label>
                        <br /><asp:Button ID="pdfSistemaSelezione" CssClass="Pulsante" runat="server" ToolTip="esporta in pdf" Text="pdf" Visible="false"/>
                    </div>
                    <div class="colOggetti" style="width: 85%">
                        <asp:TextBox ID="TxtSistemaDiSelezione" CssClass="textbox" runat="server" ToolTip="Sistema di Selezione"
                            TextMode="MultiLine" Rows="5" Width="99%"></asp:TextBox>
                    </div>
                </div>
            <div class="RigaVuota">
                &nbsp;</div>
                <div class="row">
                    <div class="collable" style="width: 15%">
                        <div style="padding-right:5px">
                            <asp:Label ID="lblFormazioneGenerale" CssClass="label" runat="server" Text="(*)Sistema per la Formazione Generale degli Operatori Volontari e per la formazione delle figure dell’Ente"
                            AssociatedControlID="TxtFormazioneGenerale"></asp:Label>
                        <br /><asp:Button ID="pdfFormazioneGenerale" CssClass="Pulsante" runat="server" ToolTip="esporta in pdf" Text="pdf" Visible="false"/>
                        </div>
                    </div>
                    <div class="colOggetti" style="width: 85%">
                        <asp:TextBox ID="TxtFormazioneGenerale" CssClass="textbox" runat="server" ToolTip="Sistema per la Formazione Generale degli Operatori Volontari e per la formazione delle figure dell’Ente"
                            TextMode="MultiLine" Rows="5" Width="99%"></asp:TextBox>
                    </div>
                </div>
            <div class="RigaVuota">
                &nbsp;</div>
                <div class="row">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblMonitoraggio" CssClass="label" runat="server" Text="(*)Sistema di Monitoraggio"
                            AssociatedControlID="TxtMonitoraggio"></asp:Label>
                        <br /><asp:Button ID="pdfMonitoraggio" CssClass="Pulsante" runat="server" ToolTip="esporta in pdf" Text="pdf" Visible="false"/>
                    </div>
                    <div class="colOggetti" style="width: 85%">
                        <asp:TextBox ID="TxtMonitoraggio" CssClass="textbox" runat="server" ToolTip="Sistema di Monitoraggio"
                            TextMode="MultiLine" Rows="5" Width="99%"></asp:TextBox>
                    </div>
                </div>
            <div class="RigaVuota">
                &nbsp;</div>
			<div class="row" runat="server" id="rowNoSO">
				<div class="collable" style="width: 15%">
					<asp:Label ID="lblSO" CssClass="label" AssociatedControlID="fileSO" runat="server" Text="(*)PDF Struttura Organizzativa"></asp:Label>
				</div>
                <div class="colOggetti" style="width: 35%">
                    <br /><asp:Button ID="cmdCaricaFileSO" runat="server" OnClientClick="return ShowPopUp('popUpControlSO')" CssClass="Pulsante" Text="Carica File"/> 
				</div>
			</div>
			<div class="row" runat="server" id="rowSO">
				<table class="table" cellspacing="0" cellpadding="2" rules="all" border="1" id="MainContent_dtgConsultaDocumenti" style="font-size: Small; width: 100%; border-collapse: collapse;">
					<caption>
						PDF Struttura Organizzativa
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
								<asp:ImageButton runat="server" ID="btnDownloadSO" ImageUrl="images/giu_small.png" AlternateText="Scarica Struttura Organizzativa" ToolTip="Scarica Struttura Organizzativa" />
							</td>
							<td>
								<asp:Literal runat="server" ID="txtSOFilename"></asp:Literal>
							</td>
							<td>
								<asp:Literal runat="server" ID="txtSOData"></asp:Literal></td>
							<td>
								<asp:Literal runat="server" ID="txtSOHash"></asp:Literal></td>
							<td>
								<asp:ImageButton runat="server" ID="btnModificaSO" OnClientClick="return ShowPopUp('popUpControlSO')" ImageUrl="images/ripristina_small.png" AlternateText="Modifica Struttura Organizzativa" ToolTip="Modifica Struttura Organizzativa" />
								<asp:ImageButton runat="server" ID="btnEliminaSO" ImageUrl="images/canc_small.png" AlternateText="Elimina Struttura Organizzativa" ToolTip="Elimina Struttura Organizzativa" />
							</td>
						</tr>
					</tbody>
				</table>
            </div>
            <div class="RigaVuota">
                &nbsp;</div>
			<div class="row" runat="server" id="rowNoSdS">
				<div class="collable" style="width: 15%">
					<asp:Label ID="lblSdS" CssClass="label" AssociatedControlID="fileSdS" runat="server" Text="PDF Sistema di Selezione"></asp:Label>
				</div>
                <div class="colOggetti" style="width: 35%">
                    <asp:Button ID="cmdCaricaFileSdS" runat="server" OnClientClick="return ShowPopUp('popUpControlSdS')" CssClass="Pulsante" Text="Carica File"/> 
				</div>
			</div>
			<div class="row" runat="server" id="rowSdS">
				<table class="table" cellspacing="0" cellpadding="2" rules="all" border="1" id="Table1" style="font-size: Small; width: 100%; border-collapse: collapse;">
					<caption>
						PDF Sistema di Selezione
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
								<asp:ImageButton runat="server" ID="btnDownloadSdS" ImageUrl="images/giu_small.png" AlternateText="Scarica Sistema di Selezione" ToolTip="Scarica Sistema di Selezione" />
							</td>
							<td>
								<asp:Literal runat="server" ID="txtSdSFilename"></asp:Literal>
							</td>
							<td>
								<asp:Literal runat="server" ID="txtSdSData"></asp:Literal></td>
							<td>
								<asp:Literal runat="server" ID="txtSdSHash"></asp:Literal></td>
							<td>
								<asp:ImageButton runat="server" ID="btnModificaSdS" OnClientClick="return ShowPopUp('popUpControlSdS')" ImageUrl="images/ripristina_small.png" AlternateText="Modifica Sistema di Selezione" ToolTip="Modifica Sistema di Selezione" />
								<asp:ImageButton runat="server" ID="btnEliminaSdS" ImageUrl="images/canc_small.png" AlternateText="Elimina Sistema di Selezione" ToolTip="Elimina Sistema di Selezione" />
							</td>
						</tr>
					</tbody>
				</table>
            </div>
            <div class="RigaVuota">&nbsp;</div>
			<div class="row" runat="server" id="rowNoAtD">
				<div class="collable" style="width: 15%">
					<asp:Label ID="lblAtD" CssClass="label" AssociatedControlID="fileAtD" runat="server" Text="PDF Atto di designazione del Responsabile del trattamento dei dati personali"></asp:Label>
				</div>
                <div class="colOggetti" style="width: 35%">
                    <asp:Button ID="cmdCaricaFileAtD" runat="server" OnClientClick="return ShowPopUp('popUpControlAtD')" CssClass="Pulsante" Text="Carica File"/> 
				</div>
			</div>
			<div class="row" runat="server" id="rowAtD">
				<table class="table" cellspacing="0" cellpadding="2" rules="all" border="1" id="Table2" style="font-size: Small; width: 100%; border-collapse: collapse;">
					<caption>
						PDF Atto di designazione del Responsabile del trattamento dei dati personali
					</caption>
					<tbody>
						<tr>
							<th scope="col">Download</th>
							<th scope="col">Nome File</th>
							<th scope="col">Data Inserimento</th>
							<th scope="col">Hash Value</th>
							<th scope="col"></th>
						</tr>
						<tr class="tr" align="center" runat="server" id="Tr2">
							<td>
								<asp:ImageButton runat="server" ID="btnDownloadAtD" ImageUrl="images/giu_small.png" AlternateText="Scarica Atto di designazione" ToolTip="Scarica Atto di designazione" />
							</td>
							<td>
								<asp:Literal runat="server" ID="txtAtDFilename"></asp:Literal>
							</td>
							<td>
								<asp:Literal runat="server" ID="txtAtDData"></asp:Literal></td>
							<td>
								<asp:Literal runat="server" ID="txtAtDHash"></asp:Literal></td>
							<td>
								<asp:ImageButton runat="server" ID="btnModificaAtD" OnClientClick="return ShowPopUp('popUpControlAtD')" ImageUrl="images/ripristina_small.png" AlternateText="Modifica Atto di designazione" ToolTip="Modifica Atto di designazione" />
								<asp:ImageButton runat="server" ID="btnEliminaAtD" ImageUrl="images/canc_small.png" AlternateText="Elimina Atto di designazione" ToolTip="Elimina Atto di designazione" />
							</td>
						</tr>
					</tbody>
				</table>
            </div>
            <div class="RigaVuota">&nbsp;</div>

        <asp:datagrid id="dtgSistemi" runat="server" Width="100%"  
             ToolTip="Elenco Sistemi Ente" CellPadding="2" Font-Size="Small"  
             CssClass="table" PageSize="5" 
            AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True" Visible="false">
		    <FooterStyle></FooterStyle>
		    <SelectedItemStyle Font-Bold="true" BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
		    <Columns>
                <asp:TemplateColumn HeaderText="Assegna Sistema" Visible="false">
	                <ItemTemplate>
		                <asp:CheckBox id="chkAssegnaSistema" TextAlign="Left"  toolTip="Assegna Sistema" AlternateText="Assegna Sistema" runat="server"></asp:CheckBox>
	                </ItemTemplate>
                </asp:TemplateColumn>
			    <asp:BoundColumn Visible="False" DataField="IdSistema"></asp:BoundColumn>
			    <asp:BoundColumn DataField="Sistema" HeaderText="Sistema">
			    </asp:BoundColumn>
			    <asp:BoundColumn DataField="Stato" HeaderText="Stato">
			    </asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Visualizza Documenti" Visible="false"> 
                    <ItemTemplate  >
						<asp:ImageButton ID="ImgDocumenti" style="cursor:pointer;" CommandName="Documento" AlternateText="Dettaglio Documenti" ToolTip='Dettaglio Documenti' 
                             runat="server" ImageURL="images/documento_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                </asp:TemplateColumn>
                 <asp:TemplateColumn HeaderText="Valido"> 
                    <ItemTemplate>
				        <asp:ImageButton ID="imgValida"  CommandName="Valida" AlternateText="Documento Valido" 
                            ToolTip="Valida Documento" runat="server" ImageURL="Images/selezionato_small.png" Visible='<%# IIF(Eval("PulsanteConfermaValida").ToString().Equals("NO"), False, True) %>'></asp:ImageButton>  
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Non Valido"> 
                    <ItemTemplate>
			    	    <asp:ImageButton ID="imgNonValida"  CommandName="NonValida" AlternateText="Documento non Validato" 
                            ToolTip="Documento Non Valido" runat="server" ImageURL="Images/deselezionato_small.png" Visible='<%# IIF(Eval("PulsanteConfermaNONValida").ToString().Equals("NO"), False, True) %>'></asp:ImageButton>  
                    </ItemTemplate>
                </asp:TemplateColumn>
		    </Columns>
            <PagerStyle NextPageText="&gt;&gt;"   
		    PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
        </asp:datagrid>

            <div class="RigaVuota">&nbsp;</div>

        <div class="RigaPulsanti" > 
            <div class="RigaPulsanti" >
            <asp:Button id="cmdConferma" CssClass="Pulsante"  runat="server" Text="Conferma" ToolTip="Conferma" ></asp:Button>
                <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" ToolTip="Salva" Text="Salva" />
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" ToolTip="Chiudi" Text="Chiudi" />
            </div>
        </div>

        <!--POPUP File--Struttura Organizzativa Inizio-->
        <asp:ModalPopupExtender runat="server" ID="popUploadSO" TargetControlID="hfPopupSO" 
            PopupControlID="pnlUploadSO" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
            PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpControlSO" DynamicServicePath="" Enabled="True" CancelControlID="btnCloseUploadSO"> 
        </asp:ModalPopupExtender>
	    <asp:Panel ID="pnlUploadSO" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
            <asp:HiddenField runat="server" ID="hfPopupSO" />
            <asp:label id="lblErroreUploadSO"  runat="server" CssClass="msgErrore" Visible="False" AssociatedControlID="cmdAllegaSO"></asp:label>
            <p class="msgInfo">Caricamento Struttura Organizzativa</p>
            <fieldset>
			    <input id="fileSO" type="file" runat="server" />
                <div style="margin-top: 1em;"></div>
                <asp:Button ID="cmdAllegaSO" runat="server" CssClass="Pulsante" Text="Carica File"/>
                <asp:Button ID="btnCloseUploadSO" runat="server" CssClass="Pulsante" Text="Chiudi"/>
            </fieldset>
	    </asp:Panel>
        <!--POPUP File--Struttura Organizzativa Fine-->

        <!--POPUP File--Sistema di Selezione Inizio-->
        <asp:ModalPopupExtender runat="server" ID="popUploadSdS" TargetControlID="hfPopupSdS" 
            PopupControlID="pnlUploadSdS" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
            PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpControlSdS" DynamicServicePath="" Enabled="True" CancelControlID="btnCloseUploadSdS"> 
        </asp:ModalPopupExtender>
	    <asp:Panel ID="pnlUploadSdS" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
            <asp:HiddenField runat="server" ID="hfPopupSdS" />
            <asp:label id="lblErroreUploadSdS"  runat="server" CssClass="msgErrore" Visible="False" AssociatedControlID="cmdAllegaSdS"></asp:label>
            <p class="msgInfo">Caricamento Sistema di Selezione</p>
            <fieldset>
			    <input id="fileSdS" type="file" runat="server" />
                <div style="margin-top: 1em;"></div>
                <asp:Button ID="cmdAllegaSdS" runat="server" CssClass="Pulsante" Text="Carica File"/>
                <asp:Button ID="btnCloseUploadSdS" runat="server" CssClass="Pulsante" Text="Chiudi"/>
            </fieldset>
	    </asp:Panel>
        <!--POPUP File--Sistema di Selezione Fine-->

        <!--POPUP File--Atto di designazione Inizio-->
        <asp:ModalPopupExtender runat="server" ID="popUploadAtD" TargetControlID="hfPopupAtD" 
            PopupControlID="pnlUploadAtD" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
            PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpControlAtD" DynamicServicePath="" Enabled="True" CancelControlID="btnCloseUploadAtD"> 
        </asp:ModalPopupExtender>
	    <asp:Panel ID="pnlUploadAtD" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
            <asp:HiddenField runat="server" ID="hfPopupAtD" />
            <asp:label id="lblErroreUploadAtD"  runat="server" CssClass="msgErrore" Visible="False" AssociatedControlID="cmdAllegaAtD"></asp:label>
            <p class="msgInfo">Caricamento Atto di designazione</p>
            <fieldset>
			    <input id="fileAtD" type="file" runat="server" />
                <div style="margin-top: 1em;"></div>
                <asp:Button ID="cmdAllegaAtD" runat="server" CssClass="Pulsante" Text="Carica File"/>
                <asp:Button ID="btnCloseUploadAtD" runat="server" CssClass="Pulsante" Text="Chiudi"/>
            </fieldset>
	    </asp:Panel>
        <!--POPUP File--Atto di designazione Fine-->
    </fieldset>
</asp:Content>
