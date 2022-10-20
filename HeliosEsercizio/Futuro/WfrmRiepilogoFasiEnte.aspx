<%@ Page Title="Riepilogo Fasi Ente" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRiepilogoFasiEnte.aspx.vb" Inherits="Futuro.WfrmRiepilogoFasiEnte" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
            <fieldset class="ContornoPagina">
             <legend>Fasi Ente</legend>
                <div class="wrapper" style="width:100%;border:0px;">
                    <div class="row" style="height:auto">
                        <asp:Label ID="msgErrore" CssClass="msgErrore" runat="server"></asp:Label><br />
                        <asp:Label ID="msgInfo" CssClass="msgInfo" runat="server"></asp:Label><br />
                        <asp:Label ID="msgConferma" CssClass="msgConferma" runat="server"></asp:Label>
                 </div>
                </div>
   
                <asp:datagrid id="dtgElencoFasiEnte" runat="server" CssClass="table" 
                    Width="100%" Caption="Riepilogo Fasi Ente"  CellPadding="2" 
                    AllowPaging="True" AutoGenerateColumns="False" UseAccessibleHeader="True" 
                    PageSize="15">
					<SelectedItemStyle BackColor="White"></SelectedItemStyle>
		            <EditItemStyle></EditItemStyle>
		            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		            <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		            <HeaderStyle></HeaderStyle>
				<Columns>
                <asp:TemplateColumn HeaderText="Variazioni Ente"> 
                    <ItemTemplate  >
						<asp:ImageButton  style="cursor:pointer;" CommandName="Select" AlternateText="Riepilogo Variazioni" ToolTip='Storico Variazioni' runat="server" ImageURL="images/lenteIngrandimento_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn DataField="TipoFase" HeaderText="Tipo Fase"></asp:BoundColumn>
				<asp:BoundColumn DataField="IdEnteFase" HeaderText="Rif. Fase"></asp:BoundColumn>
				<asp:BoundColumn DataField="stato" HeaderText="Stato"></asp:BoundColumn>
				<asp:BoundColumn DataField="DataInizioFase" HeaderText="Data Inizio Fase"></asp:BoundColumn>
				<asp:BoundColumn DataField="DataFineFase" HeaderText="Data Fine Fase"/>
                    <asp:TemplateColumn HeaderText="Adeguamento"> 
                    <ItemTemplate  >
						<asp:ImageButton   style="cursor:pointer;" CommandName="Adeguamento" AlternateText="Adeguamento Ok" ToolTip='Adeguamento OK' runat="server" ImageURL="images/conferma_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Art.2"> 
                    <ItemTemplate  >
						<asp:ImageButton style="cursor:pointer;" CommandName="Articolo2" AlternateText="Inizio Articolo 2" ToolTip='Inizio Articolo 2' runat="server" ImageURL="images/InizioArt2_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Art.10"> 
                    <ItemTemplate  >
						<asp:ImageButton  style="cursor:pointer;" CommandName="Articolo10" AlternateText="Inizio Articolo 10" ToolTip='Inizio Articolo 10' runat="server" ImageURL="images/InizioArt10_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Inserimento Documenti"> 
                    <ItemTemplate  >
						<asp:ImageButton   style="cursor:pointer;" CommandName="DocumentiEnte" AlternateText="Inserisci Documenti" ToolTip='Inserisci Documenti' runat="server" ImageURL="images/documento_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                </asp:TemplateColumn>
                   <asp:TemplateColumn HeaderText="Elenco Documenti"> 
                          <ItemTemplate >
	                        <asp:ImageButton style="cursor:pointer;" CommandName="ConsultaDocumenti" AlternateText="Visualizza Documenti" ToolTip='Consultazione Documenti' runat="server" ImageURL="images/documento_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Presenta Art.2"> 
                    <ItemTemplate >
						<asp:ImageButton ID="ImageButton1" style="cursor:pointer;" CommandName="PresentaArt2" AlternateText="Presenta Art.2" runat="server"  ToolTip="Presenta Art.2" ImageURL="images/presenta_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Presenta Art.10"> 
                    <ItemTemplate  >
						<asp:ImageButton ID="ImageButton2" style="cursor:pointer;" CommandName="PresentaArt10" AlternateText="Presenta Art.10" ToolTip='Presenta Art.10' runat="server" ImageURL="images/presenta_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Stampa Documento Riassuntivo"> 
                    <ItemTemplate>
						<asp:ImageButton ID="ImageButton3" style="cursor:pointer;" CommandName="Stampa" AlternateText="Stampa Documento Riassuntivo" ToolTip='Stampa Documento Riassuntivo' runat="server" ImageURL="images/stampa_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                </asp:TemplateColumn>
  <asp:TemplateColumn HeaderText="Aggiorna Sedi Art2"> 
                    <ItemTemplate>
						<asp:ImageButton ID="CmdModIndirizzoArt2" style="cursor:pointer;" CommandName="AggiornaIndirizzoSediART2" AlternateText="Aggiorna Indirizzo Art2" ToolTip='Aggiorna Indirizzo Art2' runat="server" ImageURL="images/sedi_small.png" CausesValidation="false"></asp:ImageButton>      
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
            </fieldset>  
</asp:Content>
