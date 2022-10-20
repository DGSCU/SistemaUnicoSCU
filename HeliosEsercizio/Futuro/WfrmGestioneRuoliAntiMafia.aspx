<%@ Page Title="Gestione Ruoli Antimafia" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="WfrmGestioneRuoliAntiMafia.aspx.vb" Inherits="Futuro.WfrmGestioneRuoliAntiMafia" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <fieldset class="ContornoPagina">
        <legend id="lgContornoPagina" runat="server">Gestione Ruoli Antimafia</legend>
        <asp:Label ID="lblMessaggio" runat="server" CssClass="msgErrore"></asp:Label>
        <br />
        <div runat="server" id="divPrincipale">
            <div>
                <% Call CaricaBoxInfoAdeguamentoAntimafia()%>
            </div>
            <div class="wrapper" style="width: 100%">
                <div class="headers">
                    <h2>
                        <asp:Label ID="lblTitolo" runat="server" Text="Ruoli Antimafia"></asp:Label></h2>
                </div>
                <div class="RigaVuota">
                    &nbsp;
                </div>
            <div class="RigaPulsanti" style="text-align:right;">
                <asp:Button ID="btnAvviaAdeguamentoAntimafia" CssClass="Pulsante" runat="server" ToolTip="Avvia Aggiornamento Antimafia" Text="Avvia Aggiornamento Antimafia"  />
                <asp:Button ID="btnAnnullaAdeguamentoAntimafia" CssClass="Pulsante" runat="server" ToolTip="Annulla Aggiornamento Antimafia" Text="Annulla Aggiornamento Antimafia"/>
                <asp:Button ID="btnTerminaAdeguamentoAntimafia" CssClass="Pulsante" runat="server" ToolTip="Termina Aggiornamento Antimafia" Text="Termina Aggiornamento Antimafia" OnClick="btnTerminaAdeguamentoAntimafia_Click"/>
            </div>
                <div class="row">
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblEnte" CssClass="label" AssociatedControlID="ddlEnti" runat="server" Text="Ente"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:dropdownlist id="ddlEnti" CssClass="ddlClass" runat="server" AutoPostBack="true"></asp:dropdownlist>         
                    </div>    
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblCodiceEnte" CssClass="label" AssociatedControlID="txtCodiceEnte" runat="server" Text="Cod. Ente"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtCodiceEnte" CssClass="textbox" runat="server" MaxLength="10"></asp:TextBox>         
                    </div>
 
                </div>
                <div class="row">
                    <div class="collable" style="width:15%">          
                        <asp:Label ID="lblcodicefiscale" CssClass="label"  runat="server" Text="Codice Fiscale" AssociatedControlID="txtcodicefiscale" />
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:TextBox ID="txtcodicefiscale" CssClass="textbox" runat="server" Width="160px" MaxLength="16"></asp:TextBox>
                    </div>
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblRuoliAntimafia" CssClass="label" AssociatedControlID="ddlRuoliAntiMafia" runat="server" Text="Ruolo"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:dropdownlist id="ddlRuoliAntiMafia" CssClass="ddlClass" runat="server"></asp:dropdownlist>         
                    </div>    
                </div>
                <div class="row">
                    <div class="collable" style="width:15%">          
                        <asp:Label ID="lblcognome" CssClass="label"  runat="server" Text="Cognome" AssociatedControlID="txtcognome" />
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:TextBox ID="txtcognome" CssClass="textbox"  runat="server" MaxLength="100"></asp:TextBox>
                    </div>
                    <div class="collable" style="width:15%">          
                        <asp:Label ID="lblnome" CssClass="label"  runat="server" Text="Nome" AssociatedControlID="txtnome" />
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:TextBox ID="txtnome" CssClass="textbox"  runat="server" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
            <div class="RigaPulsanti" style="text-align:right;">
                <asp:Button ID="btnPulisci" CssClass="Pulsante" runat="server" ToolTip="Pulisci campi" Text="Pulisci campi"  />
                <asp:Button ID="btnRicerca" CssClass="Pulsante" runat="server" ToolTip="Ricerca" Text="Ricerca"  />
                <asp:Button ID="btnAutocertificazioni" CssClass="Pulsante" runat="server" ToolTip="Inserisci Autocertificazioni" Text="Inserisci Autocertificazioni" OnClick="btnAutocertificazioni_Click"  />
                <asp:Button ID="cmdInserisci" CssClass="Pulsante" runat="server" ToolTip="Inserisci Ruolo Antimafia" Text="Inserisci Ruolo Antimafia" visible="false"/>
            </div>
           </div>
           <br />
            <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" Caption="Risultato Ricerca Ruoli Antimafia"
                    ToolTip="Elenco Ruoli Antimafia" CssClass="table"  AllowPaging="True" 
                    AllowSorting="True" AutoGenerateColumns="False" 
                    UseAccessibleHeader="True">
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		        <HeaderStyle></HeaderStyle>
		        <Columns>
                    <asp:TemplateColumn HeaderText="Selez."> 
                        <ItemTemplate>
	                        <asp:ImageButton ID="IdImgSelRuolo" style="cursor:pointer;" CommandName="Select" alt="Seleziona Ruolo" ToolTip='Seleziona Ruolo' runat="server" ImageURL="images/lenteIngrandimento_small.png" CausesValidation="false"></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateColumn>
				    <asp:BoundColumn DataField="Denominazione" HeaderText="Ente"></asp:BoundColumn>
				    <asp:BoundColumn DataField="Cognome" HeaderText="Cognome"></asp:BoundColumn>
				    <asp:BoundColumn DataField="Nome" HeaderText="Nome"></asp:BoundColumn>
				    <asp:BoundColumn DataField="CodiceFiscale" HeaderText="Codice Fiscale"></asp:BoundColumn>
				    <asp:BoundColumn DataField="RuoloAntiMafia" HeaderText="Ruolo"></asp:BoundColumn>
				    <asp:BoundColumn Visible="False" DataField="idRuoloAntiMafia" HeaderText="idSede"></asp:BoundColumn>
				    <asp:BoundColumn Visible="False" DataField="idEnte" HeaderText="idEnte"></asp:BoundColumn>
			    </Columns>
			    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
		    </asp:datagrid>
        </div>
       <asp:HiddenField ID="hdsElencoRuoliAntiMafia" runat="server" />
       <asp:HiddenField ID="hIdEnteFaseAntimafia" runat="server" />
    </fieldset>
    <%--POPUP Apri Fase Antimafia--%>
    <asp:ModalPopupExtender runat="server" ID="popUpApriFaseAntimafia" TargetControlID="btnAvviaAdeguamentoAntimafia" 
        PopupControlID="pnlApriFaseAntimafia" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
        BehaviorID="popUpApriFaseAntimafia" Enabled="True" CancelControlID="btnCloseApriFaseAntimafia"> 
    </asp:ModalPopupExtender>
	<asp:Panel ID="pnlApriFaseAntimafia" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
        <asp:label id="lblErroreApriFaseAntimafia"  runat="server" CssClass="msgErrore" Visible="False" AssociatedControlID="cmdAllega"></asp:label>
        <p class="msgInfo">Avvia Aggiornamento Antimafia</p>
        <fieldset>
			<p>Confermare l'apertura di un nuovo aggiornamento antimafia?</p>
            <div style="margin-top: 1em;"></div>
            <asp:Button ID="btnApriFaseAntimafia" runat="server" CssClass="Pulsante" Text="Avvia Aggiornamento Antimafia"/>
            <asp:Button ID="btnCloseApriFaseAntimafia" runat="server" CssClass="Pulsante" Text="Chiudi"/>
        </fieldset>
	</asp:Panel>

    <%--POPUP Chiudi Fase Antimafia--%>
    <asp:ModalPopupExtender runat="server" ID="popUpChiudiFaseAntimafia" TargetControlID="hfChiudiFaseAntimafia" 
        PopupControlID="pnlChiudiFaseAntimafia" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
        BehaviorID="popUpChiudiFaseAntimafia" Enabled="True" CancelControlID="btnCloseChiudiFaseAntimafia"> 
    </asp:ModalPopupExtender>
    <asp:HiddenField runat="server" ID="hfChiudiFaseAntimafia" />
	<asp:Panel ID="pnlChiudiFaseAntimafia" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
        <p class="msgInfo">Chiusura Aggiornamento Antimafia</p>
        <asp:label id="lblErroreChiusuraAntimafia"  runat="server" CssClass="msgErrore" AssociatedControlID="btnConfermaChiusuraAntimafia"></asp:label>
        <fieldset>
			<p>È necessario scaricare la comunicazione antimafia, apporre la firma digitale e ricaricare il documento firmato</p>
            			<br />
				<asp:label runat="server"  CssClass="ddlClass" AutoPostBack="true" ID="lblTipoRappresentante" Text="Firmato da: "/>
				<asp:DropDownList ID="ddlTipoRappresentante" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoRappresentante_SelectedIndexChanged">
					<asp:ListItem Text="Rappresentante Legale" Value="Rappresentante Legale"></asp:ListItem>
					<asp:ListItem Text="Coordinatore Responsabile del Servizio Civile Universale" Value="Coordinatore Responsabile del Servizio Civile Universale"></asp:ListItem>
				</asp:DropDownList>
			<br />
			<br />
            <asp:Button ID="btnScaricaComunicazioneAntimafia" CssClass="Pulsante" runat="server" ToolTip="Scarica Comunicazione Antimafia" Text="Scarica Comunicazione Antimafia" OnClick="btnScaricaComunicazioneAntimafia_Click" />
            <div style="margin-top: 1em;"></div>

			<input id="fileAntimafia" type="file" runat="server" />
            <div style="margin-top: 1em;"></div>
            <hr />
            <asp:Button ID="btnConfermaChiusuraAntimafia" runat="server" CssClass="Pulsante" Text="Termina Aggiornamento Antimafia" OnClick="btnConfermaChiusuraAntimafia_Click"/>
            <asp:Button ID="btnCloseChiudiFaseAntimafia" runat="server" CssClass="Pulsante" Text="Chiudi"/>
        </fieldset>
	</asp:Panel>

    <%--POPUP Annulla Fase Antimafia--%>
    <asp:ModalPopupExtender runat="server" ID="popUpAnnullaFaseAntimafia" TargetControlID="btnAnnullaAdeguamentoAntimafia" 
        PopupControlID="pnlAnnullaFaseAntimafia" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
        BehaviorID="popUpAnnullaFaseAntimafia" Enabled="True" CancelControlID="btnCloseAnnullaFaseAntimafia"> 
    </asp:ModalPopupExtender>
	<asp:Panel ID="pnlAnnullaFaseAntimafia" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
        <p class="msgInfo">Chiusura Aggiornamento Antimafia</p>
        <asp:label id="Label1"  runat="server" CssClass="msgErrore" AssociatedControlID="btnConfermaChiusuraAntimafia"></asp:label>
        <fieldset>
			<p>Confermare l'annullamento dell'aggiornamento antimafia?</p>

            <div style="margin-top: 1em;"></div>

            <hr />
            <asp:Button ID="btnAnnullaFaseAntimafia" runat="server" CssClass="Pulsante" Text="Annulla Aggiornamento Antimafia" OnClick="btnAnnullaFaseAntimafia_Click"/>
            <asp:Button ID="btnCloseAnnullaFaseAntimafia" runat="server" CssClass="Pulsante" Text="Chiudi"/>
        </fieldset>
	</asp:Panel>



</asp:Content>
