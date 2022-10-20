	<%@ Page Title="Riepilogo Dichiarazioni Antimafia  Ente" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRiepilogoAntimafia.aspx.vb" Inherits="Futuro.WfrmRiepilogoAntimafia" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
	<fieldset class="ContornoPagina">
		<legend>Comunicazioni Antimafia</legend>
		<div class="wrapper" style="width:100%;border:0px;">
			<div class="row" style="height:auto">
				<asp:Label ID="msgErrore" CssClass="msgErrore" runat="server"></asp:Label><br />
				<asp:Label ID="msgInfo" CssClass="msgInfo" runat="server"></asp:Label><br />
				<asp:Label ID="msgConferma" CssClass="msgConferma" runat="server"></asp:Label>
			</div>
		</div>
   
		<asp:datagrid id="dtgElencoDocumenti" runat="server" CssClass="table" 
			Width="100%" Caption="Comunicazioni"  CellPadding="2" 
			AllowPaging="True" AutoGenerateColumns="False" UseAccessibleHeader="True" 
			PageSize="15">
			<SelectedItemStyle BackColor="White"></SelectedItemStyle>
			<EditItemStyle></EditItemStyle>
			<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
			<ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
			<HeaderStyle></HeaderStyle>
			<Columns>
				<asp:BoundColumn DataField="IdEnteFaseAntimafia" HeaderText="Rif. Fase" Visible="false"></asp:BoundColumn>
				<asp:BoundColumn DataField="DataInizioFase" HeaderText="Data Apertura Fase"></asp:BoundColumn>
				<asp:BoundColumn DataField="DataChiusuraFase" HeaderText="Data Chiusura Fase"/>
				<asp:BoundColumn DataField="Stato" HeaderText="Stato"></asp:BoundColumn>
				<asp:BoundColumn DataField="IdAllegatoComunicazioneAntimafia" Visible="false"/>
				<asp:TemplateColumn HeaderText="Scarica Domanda"> 
					<ItemTemplate>
						<asp:ImageButton ID="ImgDownload" style="cursor:pointer;" CommandName="Download" AlternateText="Scarica Domanda" ToolTip='Scarica Domanda' runat="server" ImageURL="images/stampa_small.png" CausesValidation="false"></asp:ImageButton>	  
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn HeaderText="Ruoli Antimafia"> 
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
			<p class="msgInfo">Elenco Ruoli Antimafia</p>
			<fieldset>
				<asp:datagrid id="dtgDichiarazioni" runat="server" CssClass="table" 
					Width="700px" Caption="Ruoli"  CellPadding="2" 
					AllowPaging="True" AutoGenerateColumns="False" UseAccessibleHeader="True" 
					PageSize="15">
					<SelectedItemStyle BackColor="White"></SelectedItemStyle>
					<EditItemStyle></EditItemStyle>
					<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
					<ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
					<HeaderStyle></HeaderStyle>
					<Columns>
						<asp:BoundColumn DataField="Denominazione" HeaderText="Ente"></asp:BoundColumn>
						<asp:BoundColumn DataField="RuoloAntiMafia" HeaderText="Ruolo"></asp:BoundColumn>
						<asp:BoundColumn DataField="CodiceFiscale" HeaderText="CodiceFiscale"></asp:BoundColumn>
						<asp:BoundColumn DataField="Nome" HeaderText="Nome"></asp:BoundColumn>
					</Columns>
					<PagerStyle Visible="false"></PagerStyle>
				</asp:datagrid>
				<br />
				<asp:Button ID="btnCloseDichiarazioni" runat="server" CssClass="Pulsante" Text="Chiudi"/>
			</fieldset>
		</asp:Panel>
	</fieldset>
</asp:Content>
