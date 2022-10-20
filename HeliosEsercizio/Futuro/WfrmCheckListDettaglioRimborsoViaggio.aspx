<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmCheckListDettaglioRimborsoViaggio.aspx.vb" Inherits="Futuro.WfrmCheckListDettaglioRimborsoViaggio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div class="firefox">
    <fieldset class="ContornoPaginaScroll">
        <legend>CHECK LIST RIMBORSO VIAGGIO</legend>
        <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="CHECK LIST RIMBORSO VIAGGIO PER"></asp:Label>&nbsp;<asp:Label
                       ID="LblLista" runat="server" Text=""></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >


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
            <br />         
           
    <h3>
        <asp:label id="lblmessaggio" runat="server" Visible="false" AssociatedControlID="dgRisultatoRicerca"></asp:label>
    </h3>
    <br />
       
    <fieldset class="ContornoPaginaScroll">
    <legend>
    
    CODICE CHECKLIST: <asp:Label ID="lblCodCheckList" runat="server" ></asp:Label></legend>
     <div class="firefox">
<asp:datagrid id="dgRisultatoRicerca" runat="server"  Caption="Dettaglio Rimborso Viaggio" CssClass="table" CellPadding="2" PageSize = "10"  AllowPaging="true" 	
        AllowSorting="True"  AutoGenerateColumns="False" UseAccessibleHeader="True"  Width="100%">
	
		<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
	    <Columns>
        <asp:BoundColumn Visible="False" DataField="idEntità"  HeaderText="idVol"></asp:BoundColumn>
        <asp:BoundColumn Visible="False" DataField="idEntitàRimborso" HeaderText="idEntitaRimb"></asp:BoundColumn>   
         <asp:BoundColumn DataField="CodiceVolontario"     HeaderText="Cod Vol"></asp:BoundColumn>
        <asp:BoundColumn DataField="Nominativo"    HeaderText="Nominativo"></asp:BoundColumn>
		<asp:BoundColumn DataField="CodiceFiscale"    HeaderText="C.F."></asp:BoundColumn>
        <asp:BoundColumn DataField="InizioServizio"    HeaderText="In Serv"></asp:BoundColumn>
        <asp:BoundColumn DataField="Contratto"   HeaderText="Contr"></asp:BoundColumn>
        <asp:BoundColumn DataField="IBAN"     HeaderText="Iban"></asp:BoundColumn>
        <asp:BoundColumn DataField="ResidenzaDomicilio"    HeaderText="Res Dom"></asp:BoundColumn>
        <asp:BoundColumn DataField="VerificaTitolo"     HeaderText="Ver Tit"></asp:BoundColumn>
        <asp:BoundColumn DataField="VerificaCorrispondenzaTitolo"    HeaderText="Ver Corr Tit"></asp:BoundColumn>
        <asp:BoundColumn DataField="Esito"  HeaderText="Esito"></asp:BoundColumn>
        <asp:TemplateColumn HeaderText="Notifica Mail">
             
             <ItemTemplate>
                       <asp:LinkButton ID="lbNotifica" ToolTip="Inoltro Email"   CommandName="Notifica" runat="server">NM</asp:LinkButton>          
             </ItemTemplate>

            </asp:TemplateColumn>

								<asp:BoundColumn DataField="IdEnte" HeaderText="IdEnte" Visible="false"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Denominazione" HeaderText="Denominazione" Visible="false"></asp:BoundColumn>
                                 <asp:TemplateColumn HeaderText="Ver. Doc" > 
             <ItemTemplate>
                 
             <asp:LinkButton ID="lbDocumenti" CommandName="Documenti" ToolTip="Verifica Documenti Volontario" runat="server">Doc</asp:LinkButton>
             </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Ver. Doc Rimb" > 
             <ItemTemplate>
                 
             <asp:LinkButton ID="lblDocumenti" CommandName="DocumentiRimb" ToolTip="Verifica Documenti Rimborso" runat="server">Doc</asp:LinkButton>
             </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="IdAttività" HeaderText="IdAttivita" Visible="false"></asp:BoundColumn>      
							</Columns>
							<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGridRidotto"  Mode="NumericPages"></PagerStyle>

						</asp:datagrid>
               </div>
                
    </fieldset>

        </fieldset>
        </div>
</asp:Content>
