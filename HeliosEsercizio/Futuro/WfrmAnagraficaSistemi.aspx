<%@ Page Title="AnagraficaSistemi" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmAnagraficaSistemi.aspx.vb" Inherits="Futuro.WfrmAnagraficaSistemi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<fieldset class="ContornoPagina">
<legend>Sistemi</legend>
     <div class="wrapper" style="width:100%">
        <div class="headers" >
            <h2>
            <asp:Label ID="Label5" runat="server"  Text="Elenco Sistemi Ente"></asp:Label>
            </h2>
        </div>
        <div class="row" style="height:auto">
             <div class="row" style="height:auto" >
                <asp:Label ID="msgErrore" CssClass="msgErrore" runat="server"></asp:Label>
                 <asp:Label ID="msgInfo" CssClass="msgInfo" runat="server"></asp:Label>
                <asp:Label ID="msgConferma" CssClass="msgConferma" runat="server"></asp:Label>

        </div>
    </div>
       <div class="RigaVuota" >&nbsp;</div>
         <h3>Si ricorda agli Enti che oltre ad indicare il possesso dei Sistemi e' necessario inserire la relativa modulistica dalla funzione "Inserisci Documenti Ente".</h3>
        <div class="RigaVuota" >&nbsp;</div>
        <asp:datagrid id="dtgSistemi" runat="server" Width="100%"  
             ToolTip="Elenco Sistemi Ente" CellPadding="2" Font-Size="Small"  
             CssClass="table" PageSize="5" 
            AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
		    <FooterStyle></FooterStyle>
		    <SelectedItemStyle Font-Bold="true" BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
		    <Columns>
                <asp:TemplateColumn HeaderText="Assegna Sistema">
	                <ItemTemplate>
		                <asp:CheckBox id="chkAssegnaSistema" TextAlign="Left"  toolTip="Assegna Sistema" AlternateText="Assegna Sistema" runat="server"></asp:CheckBox>
	                </ItemTemplate>
                </asp:TemplateColumn>
			    <asp:BoundColumn Visible="False" DataField="IdSistema"></asp:BoundColumn>
			    <asp:BoundColumn DataField="Sistema" HeaderText="Sistema">
			    </asp:BoundColumn>
			    <asp:BoundColumn DataField="Stato" HeaderText="Stato">
			    </asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Visualizza Documenti"> 
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
        <div class="RigaPulsanti" > 
            <asp:Button id="cmdConferma" CssClass="Pulsante"  runat="server" Text="Conferma" ToolTip="Conferma" ></asp:Button>
            <asp:Button id="cmdChiudi" CssClass="Pulsante"  runat="server" Text="Chiudi" ToolTip="Chiudi"  ></asp:Button>
        </div>
         <div class="RigaVuota" >&nbsp;</div>
        <div id="DivDettaglioDocumenti" class="row" style="height:auto" visible="false" runat="server">
        
        <asp:datagrid id="dtgElencoDocumenti" runat="server" Width="100%"  ToolTip="Elenco Documenti Sistemi" 
			AllowPaging="True" CellPadding="2" Font-Size="Small"  CssClass="table" PageSize="5" 
            AllowSorting="True" UseAccessibleHeader="True" AutoGenerateColumns="False">
		    <FooterStyle></FooterStyle>
		    <SelectedItemStyle Font-Bold="true" BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
            <Columns>
                <asp:BoundColumn DataField="FILENAME" HeaderText="Nome File"></asp:BoundColumn>
                <asp:BoundColumn DataField="StatoDocumento" HeaderText="Stato Documento">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="IdEnteFase" HeaderText="Fase Ente">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="DataInserimento" HeaderText="Data Inserimento">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="IdEnteDocumento" Visible="False"></asp:BoundColumn>
                <asp:BoundColumn DataField="IdSistema" Visible="False"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Download"> 
                    <ItemTemplate>
	                    <asp:ImageButton ID="imgScaricaDoc" style="cursor:pointer;" CommandName="Download" alt="Scarica Documento" ToolTip='Scarica Documento' runat="server" ImageURL="images/giu_small.png" CausesValidation="false"></asp:ImageButton>  
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
        </div>
    </div>
    <asp:HiddenField ID="hf_IdEnteFase" runat="server"/>
    </fieldset>
</asp:Content>

