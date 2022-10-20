<%@ Page Title="Ricerca Modelli" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmEditorElencoModelli.aspx.vb" Inherits="Futuro.WfrmEditorElencoModelli" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
<legend >Ricerca Modelli</legend>
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Modelli"></asp:Label>
      </h2>
 </div>
   <div class="RigaVuota" >&nbsp;
   </div> 
     <div class="row"  style="height:auto">
   <asp:label id="msgErrore" CssClass="msgErrore"  runat="server" ></asp:label>
  </div>
      <div class="row" >
       <div class="collable"  style="width:15%">
           <asp:Label ID="lblNomeLogico" CssClass="label" AssociatedControlID="TxtNomeLogico"  runat="server" Text="Nome Logico"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:TextBox ID="TxtNomeLogico"  CssClass="textbox" runat="server"></asp:TextBox>
       </div>
          <div class="collable" style="width:15%">      
           <asp:Label ID="lblDescrizione" CssClass="label" AssociatedControlID="txtDescrizione" runat="server" Text="Descrizione"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">       
           <asp:TextBox ID="txtDescrizione" CssClass="textbox" runat="server"></asp:TextBox>         
       </div>
    
    </div>
    <div class="row" >
    
       <div class="collable" style="width:15%">
            <asp:Label ID="lblAreaModello" CssClass="label" AssociatedControlID="ddlAreaModello" runat="server" Text="Area di Riferimento"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:DropDownList ID="ddlAreaModello" runat="server" CssClass="ddlClass"  AutoPostBack="True">
                <asp:ListItem Value="0">Selezionare</asp:ListItem>
              </asp:DropDownList>
       </div>
    </div>
    <div class="RigaPulsanti">
        <asp:Button
          ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;<asp:Button 
            ID="cmdChiudi" CssClass="Pulsante" runat="server"
              Text="Chiudi"  />

   </div>
    
   
</div>



<asp:datagrid id="dtgRisultatoRicerca" runat="server" Width="100%"  ToolTip="Risultato Ricerca Modelli"   
         AllowPaging="True" CellPadding="2"  CssClass="table" PageSize="10"	AllowSorting="True" 
         AutoGenerateColumns="False" UseAccessibleHeader="True" Caption="Risultato Ricerca Modelli">
				<FooterStyle></FooterStyle>
				<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
				<EditItemStyle></EditItemStyle>
				<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
				<ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
				<HeaderStyle></HeaderStyle>
				<Columns>
                    <asp:TemplateColumn HeaderText="Selez."> 
                        <ItemTemplate>
						    <asp:ImageButton ID="imgModello" CommandName="Select" AlternateText="Seleziona Modello" ToolTip="Seleziona Modello" runat="server" ImageURL="images/documento_small.png" CausesValidation="false"></asp:ImageButton>
                        </ItemTemplate>
                     </asp:TemplateColumn>
					<asp:BoundColumn DataField="NomeLogico" HeaderText="Nome Logico">
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Descrizione" HeaderText="Descrizione">
					</asp:BoundColumn>
					<asp:BoundColumn DataField="DataCreazione" HeaderText="Data Ultimo Aggiornamento">
					</asp:BoundColumn>
					<asp:BoundColumn DataField="UserNameProprietario" HeaderText="User Name Ultimo Aggiornamento">
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Area" HeaderText="Area di Riferimento">
					</asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="idArea" HeaderText="Id Area"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="IdModello" HeaderText="Id Modello"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="NomeFisico" HeaderText="FileFisico"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="Path" HeaderText="Path"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="idRegioneCompetenza" HeaderText="RegioneCompetenza"></asp:BoundColumn>
				</Columns>
			<PagerStyle NextPageText="&gt;&gt;"   
				PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
    <p>&nbsp;</p>
</fieldset>
</asp:Content>
