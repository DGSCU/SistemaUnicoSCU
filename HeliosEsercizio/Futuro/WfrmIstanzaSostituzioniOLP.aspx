<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmIstanzaSostituzioniOLP.aspx.vb" Inherits="Futuro.WfrmIstanzaSostituzioniOLP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script  type="text/javascript">
        function ShowPopUp(popupDaAprire) {
            $find(popupDaAprire).show();
            return false;
        }

        function hidepopUploadISTANZA() { var modalPopup = $find('popUploadISTANZA'); modalPopup.hide(); }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <fieldset class="ContornoPagina">
        <legend id="lgContornoPagina" runat="server">Istanza sostituzioni OLP</legend>
        <asp:Label ID="lblMessaggio" runat="server" CssClass="msgErrore"></asp:Label>
        <br />
        <div class="wrapper" style="width: 100%">
            <div class="headers">
                <h2><asp:Label ID="lblTitolo" runat="server" Text="Istanza sostituzioni OLP"></asp:Label></h2>
            </div>
            <div class="RigaVuota">&nbsp;</div>
			<div class="row" >
				<div class="collable" style="width:15%">
				&nbsp;<asp:Label ID="lblIdIstanza" AssociatedControlID="txtIdIstanza" runat="server" Text="CODICE ISTANZA"></asp:Label>
				</div>
				<div class="colOggetti" style="width:35%" >
					<asp:TextBox ID="txtIdIstanza" runat="server" enabled="false"></asp:TextBox>
				</div>
				<div class="collable" style="width:15%">
					<asp:Label ID="lblStatoIstanza"  AssociatedControlID="txtStatoIstanza" runat="server" Text="STATO ISTANZA"></asp:Label>
				</div>
				<div class="colOggetti" style="width:35%">
					<asp:TextBox ID="txtStatoIstanza" runat="server" enabled="false"></asp:TextBox>
				</div>
			</div>
            <asp:datagrid id="dgElencoSostituzioniOLP" Caption="Elenco sostituzioni OLP associate" runat="server" Width="100%" ToolTip="Elenco sostituzioni OLP associate" 
                            AllowPaging="true" PageSize="5"  CssClass="table" AllowSorting="false"  AutoGenerateColumns="False" UseAccessibleHeader="True" 
                            Visible="true">
	            <FooterStyle></FooterStyle>
	            <SelectedItemStyle  BackColor="White" ></SelectedItemStyle>
	            <EditItemStyle></EditItemStyle>
	            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	            <ItemStyle CssClass="tr" HorizontalAlign ="Center"></ItemStyle>
	            <HeaderStyle></HeaderStyle>
                <Columns>
				    <asp:TemplateColumn HeaderText="Selez."> 
				        <ItemTemplate>
					        <asp:ImageButton ID="IdImgSelSostituzione" style="cursor:pointer;" CommandName="Select" alt="Seleziona Sostituzione" ToolTip='Seleziona Sostituzione' runat="server" ImageURL="images/lenteIngrandimento_small.png" CausesValidation="false"></asp:ImageButton>      
					    </ItemTemplate>
				    </asp:TemplateColumn>
					<asp:BoundColumn DataField="Titolo" HeaderText="Titolo Progetto"></asp:BoundColumn>
					<asp:BoundColumn DataField="Codice" HeaderText="Codice Progetto"></asp:BoundColumn>
					<asp:BoundColumn DataField="IdEnteSedeAttuazione" HeaderText="Codice Sede"></asp:BoundColumn>
					<asp:BoundColumn DataField="CognomeSost" HeaderText="Cognome sostituito"></asp:BoundColumn>
					<asp:BoundColumn DataField="NomeSost" HeaderText="Nome sostituito"></asp:BoundColumn>
					<asp:BoundColumn DataField="Motivazione" HeaderText="Motivo sostituzione"></asp:BoundColumn>
					<asp:BoundColumn DataField="CognomeSub" HeaderText="Cognome subentrante"></asp:BoundColumn>
					<asp:BoundColumn DataField="NomeSub" HeaderText="Nome subentrante"></asp:BoundColumn>
					<asp:BoundColumn DataField="Stato" HeaderText="Stato" Visible="False"></asp:BoundColumn>
			        <asp:BoundColumn Visible="False" DataField="IdSostituzioneOLP" HeaderText="IdSostituzioneOLP"></asp:BoundColumn>
			        <asp:BoundColumn Visible="False" DataField="IdAttivita" HeaderText="IdAttivita"></asp:BoundColumn>
			        <asp:BoundColumn Visible="False" DataField="IdAttivitaSedeAttuazione" HeaderText="IdAttivitaSedeAttuazione"></asp:BoundColumn>
			        <asp:BoundColumn Visible="False" DataField="IdEntePersonaleRuolo" HeaderText="IdEntePersonaleRuolo"></asp:BoundColumn>
			        <asp:BoundColumn Visible="False" DataField="IdEnteSedeAttuazione" HeaderText="IdEnteSedeAttuazione"></asp:BoundColumn>
                    <asp:TemplateColumn> 
                        <ItemTemplate >
				            <asp:ImageButton ID="eliminaSostituzione" CommandName="EliminaSostituzione" style="cursor:pointer" AlternateText="Elimina Associazione" ToolTip="Elimina Associazione" runat="server" ImageUrl="images/canc_small.png" />
                        </ItemTemplate>
                    </asp:TemplateColumn>
				</Columns>
		        <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
            </asp:datagrid>
            <div class="RigaPulsanti" style="text-align:right;">
                <asp:Button ID="cmdInserisci" CssClass="Pulsante" runat="server" ToolTip="Inserisci associazione" Text="Inserisci associazione"  OnClientClick="return ShowPopUp('popUpControlSelezioneSostituzioneOlp')"/>
            </div>
        </div>
        </fieldset>
        <fieldset class="ContornoPagina">
        <legend id="Legend1" runat="server">Presenta Istanza sostituzioni OLP</legend>
            <div id="divNoPresentata" runat="server">
			    <p><b>È necessario scaricare il file PDF per la richiesta di Istanza, apporre la firma digitale e ricaricare il documento firmato.</b></p>
				&nbsp;<b><asp:label runat="server"  CssClass="ddlClass" AutoPostBack="true" ID="lblTipoRappresentante" Text="Firmato da: "/>
				<asp:DropDownList ID="ddlTipoRappresentante" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoRappresentante_SelectedIndexChanged" Width="375px">
					<asp:ListItem Text="Rappresentante Legale" Value="Rappresentante Legale"></asp:ListItem>
					<asp:ListItem Text="Coordinatore Responsabile del Servizio Civile Universale" Value="Coordinatore Responsabile del Servizio Civile Universale"></asp:ListItem>
				</asp:DropDownList></b>
			    <br />
			    <br />
			    <p>Si può anche salvare l'Istanza e presentarla in un secondo momento.</p>
                <asp:Button ID="btnScaricaIstanza" CssClass="Pulsante" runat="server" ToolTip="Scarica Istanza" Text="Scarica Istanza"/>
                <div class="RigaVuota">&nbsp;</div>
            </div>
            <asp:Label ID="lblMessSalva" runat="server" CssClass="msgErrore" Visible="false"></asp:Label>
            <div class="RigaVuota">&nbsp;</div>
            <div class="wrapper" style="width: 100%">
				<div class="row" runat="server" id="rowNoISTANZA">
					<div class="colOggetti" style="width: 35%">
					&nbsp;<asp:Button ID="cmdCaricaFileISTANZA" runat="server" OnClientClick="return ShowPopUp('popUpControlISTANZA')" CssClass="Pulsante" Text="Carica Istanza firmata"/> 
					</div>
				</div>
				<div class="row" runat="server" id="rowISTANZA">
					<table class="table" cellspacing="0" cellpadding="2" rules="all" border="1" id="MainContent_dtgConsultaDocumenti" style="font-size: Small; width: 100%; border-collapse: collapse;">
						<caption>
							Istanza firmata
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
									<asp:ImageButton runat="server" ID="btnDownloadISTANZA" ImageUrl="images/giu_small.png" AlternateText="Scarica Istanza firmata" ToolTip="Scarica Istanza firmata" />
								</td>
								<td>
									<asp:Literal runat="server" ID="txtISTANZAFilename"></asp:Literal>
								</td>
								<td>
									<asp:Literal runat="server" ID="txtISTANZAData"></asp:Literal></td>
								<td>
									<asp:Literal runat="server" ID="txtISTANZAHash"></asp:Literal></td>
								<td>
									<asp:ImageButton runat="server" ID="btnModificaISTANZA" OnClientClick="return ShowPopUp('popUpControlISTANZA')" ImageUrl="images/ripristina_small.png" AlternateText="Modifica Istanza firmata" ToolTip="Modifica Istanza firmata" />
									<asp:ImageButton runat="server" ID="btnEliminaISTANZA" ImageUrl="images/canc_small.png" AlternateText="Elimina ISTANZA firmata" ToolTip="Elimina Istanza firmata" />
								</td>
							</tr>
						</tbody>
					</table>
				</div>
                <div class="RigaVuota">&nbsp;</div>
                <div class="RigaPulsanti" style="text-align:right;">
                    <asp:Button ID="btnGeneraRisposta" CssClass="Pulsante" runat="server" ToolTip="Genera lettera risposta" Text="Genera lettera risposta" Visible="false"/>
                    <asp:Button ID="btnElimina" CssClass="Pulsante" runat="server" ToolTip="Elimina Istanza" Text="Elimina Istanza" Visible="false"/>
                    <asp:Button ID="btnSalva" CssClass="Pulsante" runat="server" ToolTip="Salva Istanza" Text="Salva Istanza"/>
                    <asp:Button ID="btnPresenta" CssClass="Pulsante" runat="server" ToolTip="Presenta Istanza" Text="Presenta Istanza"/>
                    <asp:Button ID="btnChiudi" CssClass="Pulsante" runat="server" ToolTip="Chiudi" Text="Chiudi"/>
                </div>
                 <asp:HyperLink ID="hplDownload" ToolTip="Scarica Documento" CssClass="bold"  runat="server" 
                 Visible="False" Text="DOWNLOAD" ForeColor="#003399"></asp:HyperLink>
            </div>
        </fieldset>
        <!--POPUP selezione sostitutione OLP - Inizio-->
        <asp:ModalPopupExtender runat="server" ID="popSelezioneSostituzioneOlp" TargetControlID="hfPopupSelezioneSostituzioneOlp" 
            PopupControlID="pnlSelezioneSostituzioneOlp" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
            PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpControlSelezioneSostituzioneOlp" DynamicServicePath="" Enabled="True" CancelControlID="btnCloseSelezioneSostituzioneOlp"> 
        </asp:ModalPopupExtender>
	    <asp:Panel ID="pnlSelezioneSostituzioneOlp" runat="server" Style="background-color: White; padding: 20px; width: 120vh; overflow-y: auto; max-height: 90vh">
            <asp:HiddenField runat="server" ID="hfPopupSelezioneSostituzioneOlp" />
            <asp:label id="lblErroreSelezioneSostituzioneOlp"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
            <p class="msgInfo">Selezione sostituzione OLP</p>
            <fieldset>
            <legend>Ricerca sostituzione OLP</legend>
                <div class="wrapper" style="width:100%">
                    <div class="RigaVuota" >&nbsp;</div>
					<div class="row" >
						<div class="collable" style="width:15%">
						&nbsp;<asp:Label ID="lblTitoloProgetto" AssociatedControlID="txtTitoloProgetto" runat="server" Text="Titolo Progetto"></asp:Label>
						</div>
						<div class="colOggetti" style="width:35%" >
							<asp:TextBox ID="txtTitoloProgetto" runat="server"></asp:TextBox>
						</div>
						<div class="collable" style="width:15%">
							<asp:Label ID="lblCodiceProgetto"  AssociatedControlID="TxtCodProg" runat="server" Text="Codice Progetto"></asp:Label>
						</div>
						<div class="colOggetti" style="width:35%">
							<asp:TextBox ID="TxtCodProg" runat="server" ></asp:TextBox>
						</div>
					</div>					
                    <div class="row">
                        <div class="collable" style="width:15%">
                        &nbsp;<asp:Label ID="lblNomeSostRicerca" CssClass="label" AssociatedControlID="txtNomeSostRicerca" runat="server" Text="Nome sostituito"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtNomeSostRicerca" CssClass="textbox" runat="server"></asp:TextBox>         
                        </div> 
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblCognomeSostRicerca" CssClass="label" AssociatedControlID="txtCognomeSostRicerca" runat="server" Text="Cognome sostituito"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtCognomeSostRicerca" CssClass="textbox" runat="server"></asp:TextBox>         
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width:15%">
                        &nbsp;<asp:Label ID="lblNomeSubRicerca" CssClass="label" AssociatedControlID="txtNomeSubRicerca" runat="server" Text="Nome subentrante"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtNomeSubRicerca" CssClass="textbox" runat="server"></asp:TextBox>         
                        </div> 
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblCognomeSubRicerca" CssClass="label" AssociatedControlID="txtCognomeSubRicerca" runat="server" Text="Cognome subentrante"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtCognomeSubRicerca" CssClass="textbox" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="RigaPulsanti">
                        <asp:Button ID="btnRicercaSostituzioniOLP" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                    </div>
                </div>
                <asp:datagrid id="dgRisultatoRicerca" Caption="Risultato Ricerca sostituzioni OLP" runat="server" Width="100%" ToolTip="Sostituzioni OLP" 
                              AllowPaging="true" PageSize="5"  CssClass="table" AllowSorting="false"  AutoGenerateColumns="False" UseAccessibleHeader="True" 
                              Visible="true">
	                <FooterStyle></FooterStyle>
	                <SelectedItemStyle  BackColor="White" ></SelectedItemStyle>
	                <EditItemStyle></EditItemStyle>
	                <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	                <ItemStyle CssClass="tr" HorizontalAlign ="Center"></ItemStyle>
	                <HeaderStyle></HeaderStyle>
                    <Columns>
						<asp:BoundColumn DataField="Titolo" HeaderText="Titolo Progetto"></asp:BoundColumn>
						<asp:BoundColumn DataField="Codice" HeaderText="Codice Progetto"></asp:BoundColumn>
					    <asp:BoundColumn DataField="IdEnteSedeAttuazione" HeaderText="Codice Sede"></asp:BoundColumn>
						<asp:BoundColumn DataField="CognomeSost" HeaderText="Cognome sostituito"></asp:BoundColumn>
						<asp:BoundColumn DataField="NomeSost" HeaderText="Nome sostituito"></asp:BoundColumn>
						<asp:BoundColumn DataField="Motivazione" HeaderText="Motivo sostituzione"></asp:BoundColumn>
						<asp:BoundColumn DataField="CognomeSub" HeaderText="Cognome subentrante"></asp:BoundColumn>
						<asp:BoundColumn DataField="NomeSub" HeaderText="Nome subentrante"></asp:BoundColumn>
					    <asp:BoundColumn DataField="Stato" HeaderText="Stato" Visible="False"></asp:BoundColumn>
			            <asp:BoundColumn Visible="False" DataField="IdSostituzioneOLP" HeaderText="IdSostituzioneOLP"></asp:BoundColumn>
			            <asp:BoundColumn Visible="False" DataField="IdAttivita" HeaderText="IdAttivita"></asp:BoundColumn>
			            <asp:BoundColumn Visible="False" DataField="IdAttivitaSedeAttuazione" HeaderText="IdAttivitaSedeAttuazione"></asp:BoundColumn>
			            <asp:BoundColumn Visible="False" DataField="IdEntePersonaleRuolo" HeaderText="IdEntePersonaleRuolo"></asp:BoundColumn>
			            <asp:BoundColumn Visible="False" DataField="IdEnteSedeAttuazione" HeaderText="IdEnteSedeAttuazione"></asp:BoundColumn>
                            <asp:TemplateColumn  > 
                                <ItemTemplate >
				                    <asp:ImageButton ID="selezionaOLP" CommandName="SelezionaOLP" style="cursor:pointer" AlternateText="Seleziona OLP" ToolTip="Seleziona OLP" runat="server" ImageUrl="images/valida_small.png" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
					</Columns>
		            <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
                </asp:datagrid>
                <div style="margin-top: 1em;"></div>
                <asp:Button ID="btnCloseSelezioneSostituzioneOlp" runat="server" CssClass="Pulsante" Text="Chiudi"/>
            </fieldset>
	    </asp:Panel>
        <!--POPUP selezione sostitutione OLP - Fine-->

        <!--POPUP File--ISTANZA Inizio-->
        <asp:ModalPopupExtender runat="server" ID="popUploadISTANZA" TargetControlID="hfPopupISTANZA" 
            PopupControlID="pnlUploadISTANZA" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
            PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpControlISTANZA" DynamicServicePath="" Enabled="True"> 
        </asp:ModalPopupExtender>
	    <asp:Panel ID="pnlUploadISTANZA" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
            <asp:HiddenField runat="server" ID="hfPopupISTANZA" />
            <asp:label id="lblErroreUploadISTANZA"  runat="server" CssClass="msgErrore" Visible="False" AssociatedControlID="cmdAllegaISTANZA"></asp:label>
            <p class="msgInfo">Caricamento ISTANZA</p>
            <fieldset>
			    <input id="fileISTANZA" type="file" runat="server" />
                <div style="margin-top: 1em;"></div>
                <asp:Button ID="cmdAllegaISTANZA" runat="server" CssClass="Pulsante" Text="Carica File"/>
                <asp:Button ID="btnCloseUploadISTANZA" runat="server" CssClass="Pulsante" Text="Chiudi" OnClientClick="hidepopUploadISTANZA()"/>
            </fieldset>
	    </asp:Panel>
        <!--POPUP File--ISTANZA Fine-->		
</asp:Content>
