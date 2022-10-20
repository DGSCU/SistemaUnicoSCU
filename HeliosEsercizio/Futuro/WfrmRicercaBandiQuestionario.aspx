<%@ Page Title="Ricerca Bandi" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaBandiQuestionario.aspx.vb" Inherits="Futuro.WfrmRicercaBandiQuestionario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
<legend >Ricerca Bandi</legend>
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:label  id="lblTitolo" style="vertical-align:middle"  Text="&nbsp;" runat="server"></asp:label>
      </h2>
    </div>
  
    <div class="rowGroup" style="height:auto">
        <div class="RigaVuota">
      &nbsp;</div>
         <asp:label id="lblmessaggio"  runat="server" CssClass="msgErrore" ></asp:label>
        <div class="RigaVuota">
      &nbsp;</div>
    </div>
    

    <div class="row" > 
        <div class="colOggetti" style="width:15%">
            <asp:Label ID="lblBando" CssClass="label" AssociatedControlID="ddlBando"  runat="server" Text="Bando"></asp:Label>
        </div>
        <div class="colOggetti" style="width:85%" >
           <asp:dropdownlist id="ddlBando" runat="server" CssClass="ddlClass"  >
			</asp:dropdownlist>
        </div>

    </div>
        <div class="row" > 
        <div class="colOggetti" style="width:15%">
            <asp:Label ID="LblTipoFormazioneGenerale" CssClass="label" AssociatedControlID="ddlTipoFormazioneGenerale"  runat="server" Text="Tipo Formazione Generale"></asp:Label>
        </div>
        <div class="colOggetti" style="width:85%" >
           <asp:dropdownlist id="ddlTipoFormazioneGenerale" runat="server" CssClass="ddlClass" >
				<asp:ListItem Value=""></asp:ListItem>
				<asp:ListItem Value="1">Unica Tranche</asp:ListItem>
				<asp:ListItem Value="2">Tranche 80% - 20%</asp:ListItem>
			</asp:dropdownlist>
        </div>

    </div>
    <div class="RigaPulsanti" > 
       <asp:Button id="cmdRicerca" CssClass="Pulsante"  runat="server" Text="Ricerca"  ></asp:Button>
    <asp:Button id="cmdChiudi" CssClass="Pulsante"  runat="server"  Text="Chiudi" ></asp:Button>&nbsp;
</div>
</div>
<div class="RigaVuota">&nbsp;</div>
  <asp:datagrid id="dgRisultatoRicerca" Caption="Risultato Ricerca Bandi"   runat="server" Width="100%"
                   AllowPaging="true" PageSize="10"  CssClass="table" AllowSorting="True" 
                   AutoGenerateColumns="False" UseAccessibleHeader="True" Visible="true">
	<FooterStyle></FooterStyle>
	<SelectedItemStyle  BackColor="White" Font-Bold="true"></SelectedItemStyle>
	<EditItemStyle></EditItemStyle>
	<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	<ItemStyle CssClass="tr" HorizontalAlign ="Center"></ItemStyle>
	<HeaderStyle></HeaderStyle>
    <Columns>
        <asp:TemplateColumn HeaderText="Selez." > 
            <ItemTemplate >
				<asp:ImageButton ID="linkQuestionario" style="cursor:pointer;" AlternateText="Questionario" CommandName="Seleziona" ToolTip=" Seleziona Questionario" CausesValidation="false" runat="server" ImageUrl="images/vincoli_small.png" />  
                </ItemTemplate>
        </asp:TemplateColumn>
		<asp:BoundColumn DataField="bando" HeaderText="Bando">
		</asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="StatoBando" HeaderText="Stato">
		</asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="DataInizio" HeaderText="Data Inizio">
		</asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="DataFine" HeaderText="Data Fine">
		</asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="idbando" HeaderText="idbando"></asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="color" HeaderText="Color"></asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="DataInizioVolontari" HeaderText="Data Inizio Vol."></asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="DataFineVolontari" HeaderText="Data Fine Vol."></asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="importoStanziato" HeaderText="Importo Stanziato" DataFormatString="{0:N2}">
		</asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="AssociazioneAutomatica" HeaderText="Ass. Automatica">
		</asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="RevisioneFormazione" HeaderText="RevisioneFormazione"></asp:BoundColumn>
	</Columns>
		<PagerStyle NextPageText="&gt;&gt;"   
PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
</asp:datagrid>
</fieldset>
</asp:Content>
