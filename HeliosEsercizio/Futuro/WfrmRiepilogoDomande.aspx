	<%@ Page Title="Riepilogo Domande Ente" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRiepilogoDomande.aspx.vb" Inherits="Futuro.WfrmRiepilogoDomande" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
	<fieldset class="ContornoPagina">
		<legend>Domande Ente</legend>
		<div class="wrapper" style="width:100%;border:0px;">
			<div class="row" style="height:auto">
				<asp:Label ID="msgErrore" CssClass="msgErrore" runat="server"></asp:Label><br />
				<asp:Label ID="msgInfo" CssClass="msgInfo" runat="server"></asp:Label><br />
				<asp:Label ID="msgConferma" CssClass="msgConferma" runat="server"></asp:Label>
			</div>
		</div>
		<asp:datagrid id="dtgElencoDocumenti" runat="server" CssClass="table" 
			Width="100%" Caption="Domande"  CellPadding="2" 
			AllowPaging="True" AutoGenerateColumns="False" UseAccessibleHeader="True" 
			PageSize="15">
			<SelectedItemStyle BackColor="White"></SelectedItemStyle>
			<EditItemStyle></EditItemStyle>
			<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
			<ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
			<HeaderStyle></HeaderStyle>
			<Columns>
				<asp:BoundColumn DataField="TipoFase" HeaderText="Tipo Domanda"></asp:BoundColumn>
				<asp:BoundColumn DataField="IdEnteFase" HeaderText="Rif. Fase"></asp:BoundColumn>
				<asp:BoundColumn DataField="stato" HeaderText="Stato"></asp:BoundColumn>
				<asp:BoundColumn DataField="DataInizioFase" HeaderText="Data Inizio Fase"></asp:BoundColumn>
				<asp:BoundColumn DataField="DataFineFase" HeaderText="Data Fine Fase"/>
				<asp:BoundColumn DataField="Privacy" HeaderText="Privacy"/>
				<asp:BoundColumn DataField="DichiarazioneImpegno" HeaderText="Dichiarazione Impegno"/>
				<asp:TemplateColumn HeaderText="Scarica Domanda"> 
					<ItemTemplate>
						<asp:ImageButton ID="ImgDownload" style="cursor:pointer;" CommandName="Domanda" AlternateText="Scarica Domanda" ToolTip='Scarica Domanda' runat="server" ImageURL="images/stampa_small.png" CausesValidation="false"></asp:ImageButton>	  
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Dichiarazioni di Impegno"> 
					<ItemTemplate>
						<asp:ImageButton ID="ImgDettaglio" style="cursor:pointer;" CommandName="Dichiarazioni" AlternateText="Dichiarazioni" ToolTip="Visualizza dichiarazioni di impegno Enti di Accoglienza" runat="server" ImageURL="images/lenteIngrandimento_small.png" CausesValidation="false"></asp:ImageButton>	  
					</ItemTemplate>
				</asp:TemplateColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
		<br />
		<div class="wrapper" style="width:100%;border:0px;">
			<div class="RigaPulsanti" > 
				<asp:Button id="cmdChiudi" CssClass="Pulsante"  runat="server" Text="Chiudi" ></asp:Button>
			</div>
		</div>
		<%--POPUP Dichiarazioni--%>
		<asp:ModalPopupExtender runat="server" ID="popUpDichiarazioni" TargetControlID="hfDichiarazioni" 
			PopupControlID="pnlDichiarazioni" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
			BehaviorID="popUpDichiarazioni" Enabled="True" CancelControlID="btnCloseDichiarazioni"> 
		</asp:ModalPopupExtender>
		<asp:HiddenField ID="hfDichiarazioni" runat="server" />
		<asp:Panel ID="pnlDichiarazioni" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
			<asp:label id="lblErroreApriFaseAntimafia"  runat="server" CssClass="msgErrore" Visible="False" AssociatedControlID="cmdAllega"></asp:label>
			<p class="msgInfo">Elenco Dichiarazioni d'Impegno</p>
			<fieldset>
				<asp:datagrid id="dtgDichiarazioni" runat="server" CssClass="table" 
					Width="700px" Caption="Dichiarazioni di Impegno"  CellPadding="2" 
					AllowPaging="True" AutoGenerateColumns="False" UseAccessibleHeader="True" 
					PageSize="15">
					<SelectedItemStyle BackColor="White"></SelectedItemStyle>
					<EditItemStyle></EditItemStyle>
					<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
					<ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
					<HeaderStyle></HeaderStyle>
					<Columns>
						<asp:BoundColumn Visible="false" DataField="IdEnteDocumento" HeaderText="Id"></asp:BoundColumn>
						<asp:BoundColumn DataField="Denominazione" HeaderText="Ente"></asp:BoundColumn>
						<asp:BoundColumn DataField="CodiceRegione" HeaderText="Rif. Ente"></asp:BoundColumn>
						<asp:TemplateColumn HeaderText="Scarica Dichiarazione di Impegno"> 
							<ItemTemplate>
								<asp:ImageButton ID="IdgDownload" style="cursor:pointer;" CommandName="Download" AlternateText="Scarica Dichiarazione di Impegno" ToolTip='Scarica Dichiarazione di Impegno' runat="server" ImageURL="images/stampa_small.png" CausesValidation="false"></asp:ImageButton>	  
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
					<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
				</asp:datagrid>
				<br />
				<asp:Button ID="btnCloseDichiarazioni" runat="server" CssClass="Pulsante" Text="Chiudi"/>
			</fieldset>
		</asp:Panel>
	</fieldset>
</asp:Content>
