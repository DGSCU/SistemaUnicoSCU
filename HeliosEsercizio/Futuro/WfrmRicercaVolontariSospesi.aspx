<%@ Page Title="Ricerca Volontari Sospesi" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaVolontariSospesi.aspx.vb" Inherits="Futuro.WfrmRicercaVolontariSospesi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="firefox">

<fieldset class="ContornoPaginaScroll">
<legend>Ricerca Volontari</legend>
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Volontari"></asp:Label>
      </h2>
      </div>
    <div class="RigaVuota" >&nbsp;
    </div>
    <div class="row" >
    
       <div class="collable"  style="width:15%">
           <asp:Label ID="lblEnte" AssociatedControlID="txtEnte" Visible="false" runat="server" Text="Ente"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:TextBox ID="txtEnte" CssClass="textbox" autofocus="true" Visible="false"  runat="server"></asp:TextBox>
       </div>
       <div class="collable" style="width:15%">
           <asp:Label ID="lblCodEnte" AssociatedControlID="txtCodEnte" Visible="false"  runat="server" Text="Cod. Ente"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:TextBox ID="txtCodEnte" CssClass="textbox" Visible="false"  runat="server"></asp:TextBox>
       </div>
    
    </div>
    <div class="row" >
       <div class="collable" style="width:15%">      
           <asp:Label ID="IdLblCognome" AssociatedControlID="txtCognome" runat="server" Text="Cognome"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">       
           <asp:TextBox ID="txtCognome" CssClass="textbox" runat="server"></asp:TextBox>         
       </div>
       <div class="collable" style="width:15%">
            <asp:Label ID="IdLblNome" AssociatedControlID="txtNome" runat="server" Text="Nome"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:TextBox ID="txtNome" CssClass="textbox" runat="server"></asp:TextBox>  
       </div>
    </div>
    <div class="row" >
        <div class="collable" style="width:15%">
            <asp:Label ID="IdLblProgetto" AssociatedControlID="txtProgetto" runat="server" Text="Progetto"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
             <asp:TextBox ID="txtProgetto" CssClass="textbox" runat="server"></asp:TextBox>  
       </div>
           <div class="collable" style="width:15%">
            <asp:Label ID="IdLblCodProgetto" AssociatedControlID="txtCodProgetto" runat="server" Text="Cod. Progetto"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">
            <asp:TextBox ID="txtCodProgetto" CssClass="textbox" runat="server"></asp:TextBox>
       </div>
     
    </div>
    <div class="row" >
        <div class="collable" style="width:15%">
            <asp:Label ID="lblFiltro" CssClass="label" AssociatedControlID="ddlFiltro" runat="server" Text="Filtro"></asp:Label> 
        </div>
        <div class="colOggetti" style="width:35%">         
            <asp:DropDownList  ID="ddlFiltro" CssClass="ddlClass"  Width="120px" runat="server">
            	<asp:ListItem Value="0" Selected="True">Tutti</asp:ListItem>
				<asp:ListItem Value="1">Da sostituire</asp:ListItem>
				<asp:ListItem Value="2">Sostituiti</asp:ListItem>
            </asp:DropDownList> 
        </div>
         <div class="collable" style="width:15%">
            <asp:Label ID="lblCodVolontario" CssClass="label" AssociatedControlID="TxtCodVolontario" runat="server" Text="Cod. Volontario"></asp:Label> 
        </div>
           <div class="colOggetti" style="width:35%">
            <asp:TextBox ID="TxtCodVolontario" CssClass="textbox" runat="server"></asp:TextBox>
       </div>
    </div>


    <div class="RigaPulsanti">
        <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
        <asp:Button ID="cmdChiudi"  runat="server" CssClass="Pulsante" Text="Chiudi"  />&nbsp;
        <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="false"/>
             <br />
         <asp:HyperLink ID="ApriCSV1"   AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" 
        Text="DOWNLOAD CSV" runat="server" 
        ForeColor="#003399" Visible="False"></asp:HyperLink>
      &nbsp;
    </div> 
   
</div>

<h3>
    <asp:label id="lblmessaggioRicerca"  runat="server"  AssociatedControlID="dgVolontari" ></asp:label>
  </h3>
  <div class="RigaVuota">&nbsp;</div>
<asp:datagrid id="dgVolontari" runat="server" Width="100%" ToolTip="Elenco Volontari" 
								 AllowPaging="True" CellPadding="2"  CssClass="table" PageSize="10"	AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
								<FooterStyle></FooterStyle>
								<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
								<EditItemStyle></EditItemStyle>
								<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
								<ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
								<HeaderStyle></HeaderStyle>
								<Columns>
									<asp:BoundColumn Visible="False" DataField="IdEntit&#224;" HeaderText="IdEntit&#224;"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="IdAttivit&#224;" HeaderText="IdAttivit&#224;"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="IdEnte" HeaderText="IdEnte"></asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Selez."> 
                                        <ItemTemplate>
											<asp:ImageButton ID="IdImgSelVolontari" style="cursor:pointer;" CommandName="Select" alt="Seleziona Volontario" ToolTip='Seleziona Volontario' runat="server" ImageURL="images/Icona_Volontario_small.png" CausesValidation="false"></asp:ImageButton>      
                                            </ItemTemplate>
                                            </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="codicevolontario" HeaderText="Codice Volontario"></asp:BoundColumn>
									<asp:BoundColumn DataField="nominativo" HeaderText="Nominativo">
										<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CodiceFiscale" HeaderText="Cod. Fiscale"></asp:BoundColumn>
									<asp:BoundColumn DataField="datanascita" HeaderText="Data Nascita" DataFormatString="{0:d}">
										<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
										<ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ComuneNascita" HeaderText="Comune Nascita">
										<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Progetto" HeaderText="Progetto"></asp:BoundColumn>
									<asp:BoundColumn DataField="SedeAttuazione" HeaderText="Sede Attuazione"></asp:BoundColumn>
									<asp:BoundColumn DataField="EntePresentante" HeaderText="Ente"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Sostituito" HeaderText="Sostituito"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="IdAttivit&#224;Entit&#224;"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="identePresentante" HeaderText="identePresentate"></asp:BoundColumn>
								</Columns>
								<PagerStyle NextPageText="&gt;&gt;"   
									PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
							</asp:datagrid>

</fieldset>

</div>
</asp:Content>
