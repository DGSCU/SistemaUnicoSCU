<%@ Page Title="Ricerca Volontari Altro Progetto" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ricercavolontarialtroprogetto.aspx.vb" Inherits="Futuro.ricercavolontarialtroprogetto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <fieldset class="ContornoPagina">
   <legend >Ricerca Volontari Altro Progetto</legend>
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Volontari"></asp:Label>
      </h2>
      </div>
    <div class="RigaVuota" >&nbsp;
    </div>
    <div class="row" >
       <div class="collable" style="width:15%">      
           <asp:Label ID="IdLblCognome" AssociatedControlID="txtCognome" runat="server" Text="Cognome"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">       
           <asp:TextBox ID="txtCognome" autofocus="true" CssClass="textbox" runat="server"></asp:TextBox>         
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
        <div class="RigaPulsanti">
        <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
        <asp:Button ID="cmdChiudi"  runat="server" CssClass="Pulsante" Text="Chiudi"  />&nbsp;
    </div> 
</div>

<fieldset class="fieldsetDatagrid" >
<h3>
    <asp:label id="lblmessaggioRicerca"  runat="server"  AssociatedControlID="dgVolontari" ></asp:label>
  </h3>
  <div class="RigaVuota">&nbsp;</div>
<asp:datagrid id="dgVolontari" runat="server" Width="100%" ToolTip="Elenco Volontari"  AllowPaging="True" 
        CellPadding="2"  CssClass="table" PageSize="10"	AllowSorting="false" AutoGenerateColumns="False" UseAccessibleHeader="True">
								<FooterStyle></FooterStyle>
								<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
								<EditItemStyle></EditItemStyle>
								<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
								<ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
								<HeaderStyle></HeaderStyle>
								<Columns>
									<asp:BoundColumn Visible="False" DataField="IdEntit&#224;" HeaderText="IdEntit&#224;">
                                    	<HeaderStyle ></HeaderStyle>
										<ItemStyle Width="5%"></ItemStyle>
                                    </asp:BoundColumn>
                                      <asp:TemplateColumn > 
                                        <ItemTemplate >
				                            <asp:ImageButton ID="ImgSelezionaVolontario"  CommandName="Select" STYLE="cursor:hand" 
                                                AlternateText="Seleziona Volontario"  runat="server" title="Seleziona Volontario" 
                                                 ImageURL="Images/valida_small.png"  CausesValidation="false">
                                            </asp:ImageButton>                            
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
									<asp:BoundColumn DataField="nominativo" HeaderText="Nominativo">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle Width="15%"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CodiceFiscale" HeaderText="Cod. Fiscale">
                                    <HeaderStyle ></HeaderStyle>
										<ItemStyle Width="10%"></ItemStyle>
                                       </asp:BoundColumn>
									<asp:BoundColumn DataField="datanascita" HeaderText="Data Nascita" DataFormatString="{0:d}">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle Width="10%"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ComuneNascita" HeaderText="Comune Nascita">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle Width="10%"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Progetto" HeaderText="Progetto">
                                    	<HeaderStyle ></HeaderStyle>
										<ItemStyle Width="35%"></ItemStyle>
                                    </asp:BoundColumn>
									<asp:BoundColumn DataField="SedeAttuazione" HeaderText="Sede Attuazione">
                                    	<HeaderStyle ></HeaderStyle>
										<ItemStyle Width="15%"></ItemStyle>
                                    </asp:BoundColumn>
								</Columns>
								<PagerStyle NextPageText="&gt;&gt;"   
									PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
</fieldset>
</fieldset>

</asp:Content>
