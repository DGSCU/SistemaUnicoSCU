<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicIstanzadiPresentazione.aspx.vb" Inherits="Futuro.WfrmRicIstanzadiPresentazione" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Ricerca Istanza Presenzatione</legend>
         <div class="wrapper" style="width:100%">
            <div class="headers">
                <h2>
                <asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Istanza di Presentazione"></asp:Label>
                </h2>
            </div>      
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblCircolare" AssociatedControlID="DdlBando" runat="server" Text="Circolare"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:textbox id="TxtdenominazioneSede1" runat="server" MaxLength="200" Width="0px" visible="False"></asp:textbox>
                    <asp:dropdownlist id="DdlBando" runat="server"  AutoPostBack="False"></asp:dropdownlist>
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="LblAnno" AssociatedControlID="ddlanno" runat="server" Text="Anno"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:dropdownlist id="ddlanno" runat="server"></asp:dropdownlist>
                    <asp:textbox id="Txtstato1" runat="server" MaxLength="20" Width="0px" Visible="False"></asp:textbox>
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="LblCompetenza" AssociatedControlID="ddlanno" runat="server" 
                        Text="Competenza"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:dropdownlist id="CboCompetenza" runat="server" ></asp:dropdownlist>
                </div>
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdSalva" runat="server" CssClass="Pulsante" ToolTip="Ricerca" Text="Ricerca" />
                <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" ToolTip="Chiudi" Text="Chiudi" />
                <asp:button id="imgStampa" Text="Esporta CSV" ToolTip="Esporta elenco risultato ricerca" CssClass="Pulsante" Runat="server"  Visible="False"></asp:button>
                <br />				
                <asp:HyperLink ID="ApriCSV1"   AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
            </div>               
         </div>      
         <div class="RigaVuota"> &nbsp; </div>
         <h3>				      
            <asp:label id="lblmessaggio" runat="server" CssClass="msgErrore"></asp:label>
        </h3> 
 
        <asp:datagrid id="dgRisultatoRicerca" Caption="Risultato Ricerca" runat="server" CssClass="table" ToolTip="Elenco Progetti" 
        AutoGenerateColumns="False" AllowSorting="True" HorizontalAlign="Center" CellPadding="3" PageSize="100" Width="100%">
        <FooterStyle ></FooterStyle>
        <SelectedItemStyle ></SelectedItemStyle>
        <EditItemStyle ></EditItemStyle>
        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
        <ItemStyle CssClass="tr"></ItemStyle>
        <HeaderStyle ></HeaderStyle>
        <Columns>
            <asp:TemplateColumn HeaderText="Selez."> 
                <ItemTemplate>
                <asp:ImageButton ID="IdImgSel" style="cursor:pointer;" CommandName="Select" alt="Seleziona Circolare" ToolTip='Seleziona Circolare' runat="server" ImageURL="images/documento_small.png" CausesValidation="false"></asp:ImageButton>      
                </ItemTemplate>
            </asp:TemplateColumn>
	        <asp:BoundColumn Visible="False" DataField="idbando" HeaderText="idbando"></asp:BoundColumn>
	        <asp:BoundColumn DataField="Bando" HeaderText="Circolare"></asp:BoundColumn>
	        <asp:BoundColumn DataField="datainizio" HeaderText="Data Inizio Circolare"></asp:BoundColumn>
	        <asp:BoundColumn DataField="datafine" HeaderText="Data Fine Circolare"></asp:BoundColumn>
	        <asp:BoundColumn DataField="Stato" HeaderText="Stato"></asp:BoundColumn>
	        <asp:BoundColumn DataField="progetti" HeaderText="N. Progetti">
		        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
		        <ItemStyle HorizontalAlign="Center"></ItemStyle>
	        </asp:BoundColumn>
	        <asp:BoundColumn DataField="descrizione" HeaderText="Competenza"></asp:BoundColumn>
	        <asp:ButtonColumn Text="..." HeaderText="Dettaglio Istanza" CommandName="Ricerca">
		        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
		        <ItemStyle HorizontalAlign="Center"></ItemStyle>
	        </asp:ButtonColumn>
	        <asp:BoundColumn Visible="False" DataField="idbandoattivit&#224;" HeaderText="idbandoattivit&#224;"></asp:BoundColumn>
        </Columns>
        <PagerStyle NextPageText="Successiva" Font-Size="XX-Small" PrevPageText="Precedente" HorizontalAlign="Center" ></PagerStyle>
    </asp:datagrid>	
        <asp:textbox id="txtModifica" runat="server"  Visible="False"  ></asp:textbox>	    
       
    </fieldset> 
</asp:Content>
