<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmCheckListElencoFormazione.aspx.vb" Inherits="Futuro.WfrmCheckListElencoFormazione" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
        <legend>CHECK LIST ELENCO FORMAZIONE</legend>
    <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="CHECK LIST FORMAZIONE"></asp:Label>&nbsp;</h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
             <div class="collable" style="width:15%">  
                 <asp:Label ID="lblCodiceEnte" runat="server" AssociatedControlID="txtCodiceEnte" Text="Codice Ente"></asp:Label>
             </div>
             <div class="colOggetti" style="width:35%">
              <asp:TextBox ID="txtCodiceEnte" runat="server">
                    
                    </asp:TextBox>
             </div>
             <div class="collable" style="width:15%">  
                 <asp:Label ID="lblNomeEnte" runat="server" AssociatedControlID="txtNomeEnte" Text="Nome Ente"></asp:Label>
             </div>
             <div class="colOggetti" style="width:35%">
              <asp:TextBox ID="txtNomeEnte" runat="server">
                    
                    </asp:TextBox>
             </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodProgRend" CssClass="label" AssociatedControlID="txtCodProgRendi" runat="server" Text="Codice Progetto Rendicontazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    
                    <asp:TextBox ID="txtCodProgRendi" runat="server">
                    
                    </asp:TextBox>
                     
                </div>
                <div class="collable" style="width:15%"> 
                <asp:Label ID="lblTitoloProg" AssociatedControlID="txtTitoloProg" runat="server" Text="Titolo Progetto"></asp:Label>     
                    
                </div>
                <div class="colOggetti" style="width:35%">
                    
                   <asp:TextBox ID="txtTitoloProg" runat="server"></asp:TextBox>
                     
                </div>
            </div>
          

             <div class="row" >

              <div class="collable" style="width:15%">     

                    <asp:Label ID="lblStatoChecklist" CssClass="label" AssociatedControlID="CboStatoChecklist" runat="server" Text="Stato Checklist"></asp:Label>

              </div>
              <div class="colOggetti" style="width:35%">

                    <asp:DropDownList ID="CboStatoChecklist" runat="server">
                    
                    </asp:DropDownList>
                     
              </div>
               <div class="collable" style="width:15%">
                 <asp:Label ID="lblcodicechecklist" CssClass="label" AssociatedControlID="txtcodicechecklist" runat="server" Text="Codice Check List"></asp:Label>
                </div>
                 <div class="colOggetti" style="width:35%">
                 <asp:TextBox ID="txtcodicechecklist" CssClass="textbox" runat="server"></asp:TextBox>  
                
                 </div>
             </div>
              <div class="row" >

              <div class="collable" style="width:15%">     
                    <asp:Label ID="LblCausaleArchiviazione" CssClass="label" AssociatedControlID="cboCausaleArchiviazione" runat="server" Text="Causale Archiviazione"></asp:Label>
              </div>
                  <div class="colOggetti" style="width:35%">

                        <asp:DropDownList ID="cboCausaleArchiviazione" runat="server">
                    
                        </asp:DropDownList>
                     
                  </div>
              </div>
            
            <div class="RigaPulsanti">
                <asp:Button ID="CmdEsporta" runat="server" Visible="false" CssClass="Pulsante" Text="Esporta CSV" />&nbsp;
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
                 <br />
                <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
                <br />
               
	           
            </div>
         </div>
         <p></p>
         <asp:datagrid id="dgRisultatoRicerca" runat="server" Caption="Risultato Ricerca Elenco Formazione"  CssClass="table" CellPadding="2" PageSize = "10"  AllowPaging="true" 	
        AllowSorting="True"  AutoGenerateColumns="False" UseAccessibleHeader="True"  Width="100%">
		<FooterStyle></FooterStyle>
		<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
	    <Columns>
        <asp:BoundColumn Visible="False" DataField="IdCheckList" HeaderText="IdCheckList"></asp:BoundColumn>
        <asp:BoundColumn Visible="False" DataField="IdAttività" HeaderText="IdAttivita"></asp:BoundColumn>    
         <asp:TemplateColumn HeaderText="Lista"> 
             <ItemTemplate >
                 <asp:ImageButton ID="CmdImgLista" AlternateText="Seleziona" CommandName="Selezionato" ImageUrl="~/Images/vincoli_small.png" runat="server" />
             </ItemTemplate>
        </asp:TemplateColumn>
         <asp:BoundColumn DataField="codicechecklist"   HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Codice Checklist"></asp:BoundColumn>
        <asp:BoundColumn DataField="CodiceProgettoRendicontazione"   HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Codice Progetto Rendicontazione"></asp:BoundColumn>
        <asp:BoundColumn DataField="Titolo" HeaderStyle-CssClass="linkPageGridRidotto"  HeaderText="Titolo"></asp:BoundColumn>
        <asp:BoundColumn DataField="Ente" HeaderStyle-CssClass="linkPageGridRidotto"  HeaderText="Ente"></asp:BoundColumn>
        <asp:BoundColumn DataField="Importo" HeaderStyle-CssClass="linkPageGridRidotto"  HeaderText="Importo Euro"></asp:BoundColumn>
        <asp:BoundColumn DataField="StatoFormazione" HeaderStyle-CssClass="linkPageGridRidotto"  HeaderText="Stato Formazione"></asp:BoundColumn>
        <asp:BoundColumn DataField="StatoCheckList"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Stato CheckList"></asp:BoundColumn>
        
    </Columns>
	<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGridRidotto"  Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
    </fieldset>
</asp:Content>
