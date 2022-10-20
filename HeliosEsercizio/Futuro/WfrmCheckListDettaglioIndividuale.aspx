<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmCheckListDettaglioIndividuale.aspx.vb" Inherits="Futuro.WfrmCheckListDettaglioIndividuale" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div class="firefox">
    <fieldset class="ContornoPaginaScroll">
        <legend>CHECK LIST PRESENZE INDIVIDUALE</legend>
        <div class="wrapper" style="width:100%">
        <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="CHECK LIST PRESENZE INDIVIDUALE PER"></asp:Label>&nbsp;<asp:Label
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
            
            <div class="RigaPulsanti" style="text-align:right">
            
                <asp:Button ID="cmdInizializza" runat="server" CssClass="Pulsante" 
                    Text="Inizializza" />&nbsp;<asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Visible="false" Text="Esporta CSV" />&nbsp;
                <asp:Button ID="CmdStampa" runat="server" CssClass="Pulsante" Text="Stampa" />&nbsp;
                <asp:Button ID="CmdConferma" runat="server" CssClass="Pulsante" Text="Conferma" />&nbsp;
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
<asp:datagrid id="dgRisultatoRicerca" Caption="Dettaglio Individuale Presenza" runat="server"  CssClass="table" CellPadding="2"  PageSize = "10"   	
        AllowSorting="True"  AutoGenerateColumns="False" UseAccessibleHeader="True"  Width="100%">
		<FooterStyle></FooterStyle>
		<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
	    <Columns>
        <asp:BoundColumn Visible="False" DataField="idEntità" HeaderText="idVol"></asp:BoundColumn>   
         <asp:BoundColumn DataField="CodiceVolontario"     HeaderText="Cod Vol"></asp:BoundColumn>
        <asp:BoundColumn DataField="Nominativo"    HeaderText="Nominativo"></asp:BoundColumn>
		<asp:BoundColumn DataField="CodiceFiscale" Visible="false"   HeaderText="C.F."></asp:BoundColumn>
        <asp:BoundColumn DataField="InizioServizio"    HeaderText="In Serv"></asp:BoundColumn>
        <asp:BoundColumn DataField="Contratto"   HeaderText="Contr"></asp:BoundColumn>
        <asp:BoundColumn DataField="IBAN"     HeaderText="Iban"></asp:BoundColumn>
        <asp:BoundColumn DataField="FoglioPresenze"    HeaderText="P Ins"></asp:BoundColumn>
        <asp:BoundColumn DataField="NP"     HeaderText="N°P"></asp:BoundColumn>
        <asp:BoundColumn DataField="NMAL"    HeaderText="N° Mal"></asp:BoundColumn>
        <asp:BoundColumn DataField="NPR"    HeaderText="N° Pr"></asp:BoundColumn>
        <asp:BoundColumn DataField="AssCons"     HeaderText="Ass Cons"></asp:BoundColumn>
        <asp:BoundColumn DataField="NMALTot"    HeaderText="N° Mal Tot"></asp:BoundColumn>
        <asp:BoundColumn DataField="NPRTot"    HeaderText="N° Pr Tot"></asp:BoundColumn>
        <asp:BoundColumn DataField="AssDecur"    HeaderText="Ass Dec"></asp:BoundColumn>
        <asp:BoundColumn DataField="NDecur"    HeaderText="N° Dec"></asp:BoundColumn>
        <asp:BoundColumn DataField="ConsDoc"    HeaderText="Doc"></asp:BoundColumn>
                <asp:BoundColumn DataField="MesiServizio"  HeaderText="Mesi 3"></asp:BoundColumn>
        <asp:BoundColumn DataField="SenzaSanzioni"  HeaderText="No Sanz"></asp:BoundColumn>
        <asp:TemplateColumn HeaderText="Mail">
             
             <ItemTemplate>
                       <asp:LinkButton ID="lbNotifica"  CommandName="Notifica" ToolTip="Inoltro Email" runat="server">NM</asp:LinkButton>          
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
