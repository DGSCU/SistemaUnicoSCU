<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmCheckListDettaglio.aspx.vb" Inherits="Futuro.WfrmCheckListDettaglio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<%--<script  type="text/javascript">

    function ApriMail() {
        window.open("WfrmCheckListNotificaMailPresenze.aspx", "Elenco", "height=450,width=600,dependent=no,scrollbars=yes,status=no");
    }
		
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="firefox">
    <fieldset class="ContornoPaginaScroll">
        <legend>CHECK LIST PRESENZE</legend>

 <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="CHECK LIST PRESENZE PER"></asp:Label>&nbsp;<asp:Label
                       ID="LblLista" runat="server" Text=""></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
             <div class="collable" style="width:15%"> 
             <asp:Label ID="lblEnte" runat="server" Text="Ente"></asp:Label>
             </div>  
             <div class="colOggetti"  style="width:35%">
             <asp:Label ID="LblEnteDato" Font-Bold="true" runat="server" Text=""></asp:Label>
             </div>
              <div class="collable" style="width:15%"> 
              <asp:Label ID="lblTitoloProg" runat="server" Text="Titolo Progetto"></asp:Label>
             </div>  
             <div class="colOggetti"  style="width:35%">
                 <asp:Label ID="LblTitoloDato" runat="server" Font-Bold="true" Text=""></asp:Label>
             </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodVol" CssClass="label" AssociatedControlID="txtCodVol" runat="server" Text="Codice Volontario"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodVol" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblEsito" CssClass="label" AssociatedControlID="CboEsito" runat="server" Text="Esito"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="CboEsito" runat="server">
                    <asp:ListItem Value=""></asp:ListItem>
                    <asp:ListItem Value="Positivo">Positivo</asp:ListItem>
				    <asp:ListItem Value="Negativo">Negativo</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCognome" CssClass="label" AssociatedControlID="txtCognome" runat="server" Text="Cognome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCognome" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblNome" CssClass="label" AssociatedControlID="txtNome" runat="server" Text="Nome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtNome" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
            <div class="collable" style="width:15%">      
                    <asp:Label ID="lblIncluso" CssClass="label" AssociatedControlID="CboIncluso" runat="server" Text="Inclusione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    
                    <asp:DropDownList ID="CboIncluso" runat="server">
                    <asp:ListItem Value=""></asp:ListItem>
                    <asp:ListItem Value="Incluso">Incluso</asp:ListItem>
				    <asp:ListItem Value="Escluso">Escluso</asp:ListItem>
                    </asp:DropDownList>
                     
                </div>
               
            </div>
            
            <div class="RigaPulsanti">
             
                <asp:Button ID="cmdInizializza" runat="server" CssClass="Pulsante" 
                    Text="Inizializza" />&nbsp;<asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Visible="false" Text="Esporta CSV" />&nbsp;
                <asp:Button ID="CmdStampa" runat="server" CssClass="Pulsante" Text="Stampa" />&nbsp;
                <asp:Button ID="CmdConferma" runat="server" CssClass="Pulsante" Text="Conferma" />&nbsp;
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;

                <br />
	            <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
              &nbsp;
            </div>
         </div>
          <br />
        <p style="text-align:right;margin-right:10px">
         <strong><asp:LinkButton ID="imgStoricoNotifiche" runat="server" style="cursor:pointer;font-size:small;font-weight:bold" Text="Storico Notifiche" Visible="true"></asp:LinkButton></strong>
        </p>
    <h3>
        <asp:label id="lblmessaggio" runat="server" AssociatedControlID="dgRisultatoRicerca"></asp:label>
    </h3>
   
    <fieldset class="ContornoPaginaScroll">
    <legend>CODICE CHECKLIST: <asp:Label ID="lblCodCheckList" runat="server" ></asp:Label></legend>
     <div class="firefox">
<asp:datagrid id="dgRisultatoRicerca" runat="server" Caption="Dettaglio Presenze"  
             CssClass="table" CellPadding="2"  AllowPaging="True" 	
        AllowSorting="True"  AutoGenerateColumns="False" UseAccessibleHeader="True"  
             Width="100%">
		<FooterStyle></FooterStyle>
		<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
	    <Columns>
        <asp:BoundColumn Visible="False" DataField="idEntità" HeaderText="idVol"></asp:BoundColumn>   
         <asp:BoundColumn DataField="CodiceVolontario"   HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Cod Vol"></asp:BoundColumn>
        <asp:BoundColumn DataField="Nominativo"  HeaderStyle-CssClass="linkPageGridRidotto"  HeaderText="Nominativo"></asp:BoundColumn>
		<asp:BoundColumn DataField="CodiceFiscale"  Visible="false" HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="C.F."></asp:BoundColumn>
        <asp:BoundColumn DataField="InizioServizio"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="In Serv"></asp:BoundColumn>
        <asp:BoundColumn DataField="Contratto"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Contr"></asp:BoundColumn>
        <asp:BoundColumn DataField="IBAN"  HeaderStyle-CssClass="linkPageGridRidotto"  HeaderText="Iban"></asp:BoundColumn>
        <asp:BoundColumn DataField="FoglioPresenze"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="P Ins"></asp:BoundColumn>
        <asp:BoundColumn DataField="NP" HeaderStyle-CssClass="linkPageGridRidotto"  HeaderText="N°P"></asp:BoundColumn>
        <asp:BoundColumn DataField="NMAL"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="N° Mal"></asp:BoundColumn>
        <asp:BoundColumn DataField="NPR"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="N° Pr"></asp:BoundColumn>
        <asp:BoundColumn DataField="AssCons"  HeaderStyle-CssClass="linkPageGridRidotto"  HeaderText="Ass Cons"></asp:BoundColumn>
        <asp:BoundColumn DataField="NMALTot"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="N° Mal Tot"></asp:BoundColumn>
        <asp:BoundColumn DataField="NPRTot"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="N° Pr Tot"></asp:BoundColumn>
        <asp:BoundColumn DataField="AssDecur"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Ass Dec"></asp:BoundColumn>
        <asp:BoundColumn DataField="NDecur"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="N° Dec"></asp:BoundColumn>
        <asp:BoundColumn DataField="ConsDoc"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Doc"></asp:BoundColumn>
        <asp:BoundColumn DataField="MesiServizio"   HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Mesi 3"></asp:BoundColumn>
        <asp:BoundColumn DataField="SenzaSanzioni"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="No Sanz"></asp:BoundColumn>
        
        <asp:TemplateColumn HeaderText="Incl"> 
             <ItemTemplate >
                 <asp:ImageButton ID="CmdIncludiSi" ToolTip="Escludi" AlternateText="Escludi"  CommandName="Escludi" ImageUrl="~/Images/selezionato_small.png"  Visible='<%# IIF(Eval("Incluso").ToString().Equals("1"), True, False) %>' runat="server" />
                 <asp:ImageButton ID="CmdEscludiNo" ToolTip="Includi" AlternateText="Includi" CommandName="Includi" ImageUrl="~/Images/deselezionato_small.png" Visible='<%# IIF(Eval("Incluso").ToString().Equals("0"), True, False) %>' runat="server" />  
             </ItemTemplate>
        </asp:TemplateColumn>
            
        <asp:TemplateColumn HeaderText="Mail">
             
             <ItemTemplate>
                       <asp:LinkButton ID="lbNotifica" ToolTip="Inoltro Email"  CommandName="Notifica" runat="server">NM</asp:LinkButton>          
             </ItemTemplate>

            </asp:TemplateColumn>

        <asp:TemplateColumn HeaderText="Ver. Doc" > 
             <ItemTemplate>
                 
             <asp:LinkButton ID="lbDocumenti" CommandName="Documenti" ToolTip="Verifica Documenti Volontario" runat="server">Doc</asp:LinkButton>
             </ItemTemplate>
            </asp:TemplateColumn>

        <asp:TemplateColumn HeaderText="Ver. P Mese"> 
             <ItemTemplate>
                
                 <asp:LinkButton ID="lbPresenze" CommandName="Presenze" ToolTip="Verifica Presenze Volontario" runat="server">P</asp:LinkButton>
             
             </ItemTemplate>
            </asp:TemplateColumn>
								<asp:BoundColumn DataField="IdEnte" HeaderText="IdEnte" Visible="false"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Denominazione" HeaderText="Denominazione" Visible="false"></asp:BoundColumn>
							</Columns>
							<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGridRidotto"  Mode="NumericPages"></PagerStyle>

						</asp:datagrid>
               </div>         
                       </fieldset>
                        
      </fieldset>
     </div>
     
</asp:Content>
