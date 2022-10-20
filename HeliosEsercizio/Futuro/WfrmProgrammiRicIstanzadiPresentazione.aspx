<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmProgrammiRicIstanzadiPresentazione.aspx.vb" Inherits="Futuro.WfrmProgrammiRicIstanzadiPresentazione" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Ricerca Istanza Presentazione Programmi</legend>
         <div class="wrapper" style="width:100%">
            <div class="headers">
                <h2>
                <asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Istanza di Presentazione Programmi"></asp:Label>
                </h2>
            </div>      
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblCircolare" AssociatedControlID="DdlBando" runat="server" Text="Avvisi"></asp:Label>
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
                </div>
                <div class="colOggetti" style="width:35%">
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
 
        <asp:datagrid id="dgRisultatoRicerca" Caption="Risultato Ricerca" runat="server" CssClass="table" ToolTip="Elenco Istanze Programmi" 
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
                <asp:ImageButton ID="IdImgSel" style="cursor:pointer;" CommandName="Select" alt="Seleziona Avviso" ToolTip='Seleziona Avviso' runat="server" ImageURL="images/documento_small.png" CausesValidation="false"></asp:ImageButton>      
                </ItemTemplate>
            </asp:TemplateColumn>
	        <asp:BoundColumn Visible="False" DataField="idbando" HeaderText="idbando"></asp:BoundColumn>
	        <asp:BoundColumn DataField="Avviso" HeaderText="Avviso"></asp:BoundColumn>
	        <asp:BoundColumn DataField="DataInizioPresentazione" HeaderText="Data Inizio Avviso"></asp:BoundColumn>
	        <asp:BoundColumn DataField="DataFinePresentazione" HeaderText="Data Fine Avviso"></asp:BoundColumn>
	        <asp:BoundColumn DataField="Stato" HeaderText="Stato"></asp:BoundColumn>
	        <asp:BoundColumn DataField="NProgrammi" HeaderText="N. Programmi">
		        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
		        <ItemStyle HorizontalAlign="Center"></ItemStyle>
	        </asp:BoundColumn>
	        <asp:ButtonColumn Text="..." HeaderText="Dettaglio Istanza" CommandName="Ricerca">
		        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
		        <ItemStyle HorizontalAlign="Center"></ItemStyle>
	        </asp:ButtonColumn>
	        <asp:BoundColumn Visible="False" DataField="idBandoProgramma" HeaderText="idBandoProgramma"></asp:BoundColumn>
        </Columns>
        <PagerStyle NextPageText="Successiva" Font-Size="XX-Small" PrevPageText="Precedente" HorizontalAlign="Center" ></PagerStyle>
    </asp:datagrid>	
        <asp:textbox id="txtModifica" runat="server"  Visible="False"  ></asp:textbox>	    
       
    </fieldset> 
</asp:Content>
