<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmVisualizzaMeseVol.aspx.vb" Inherits="Futuro.WfrmVisualizzaMeseVol" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head>
    <title>Visualizzazione Assenze Mensili Volontari</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmAssenzeMensiliVolontari" method="post" runat="server">
        <div class="fieldsetrasparente">
            <fieldset class="ContornoPagina">
                <legend>Visualizzazione Assenze Mensili Volontari</legend>
                <p>
                    <asp:label id="Lblmessaggio" runat="server" CssClass="msgErrore" Text="Non ci sono assenze per il mese selezionato"></asp:label>
                </p>
                <div class="wrapper" style="width:100%">
                    <div class="headers" >
                        <h2><asp:Label ID="lblTitolo" runat="server"  Text="Visualizzazione Assenze Mensili Volontari"></asp:Label></h2>
                    </div>
                    <div class="RigaVuota">&nbsp;</div>
                    <div class="row">
                        <div class="collable" style="width:15%">
                            <asp:Label ID="lblEnte" CssClass="label" AssociatedControlID="lblValEnte" runat="server" Text="Ente"></asp:Label>   
                        </div> 
                        <div class="colOggetti" style="width:15%">       
                            <asp:label id="lblValEnte" CssClass="bold" runat="server"></asp:label>         
                        </div>
                        <div class="collable" style="width:15%">
                            <asp:Label ID="lblProg" CssClass="label" AssociatedControlID="lblValProg" runat="server" Text="Denominazione"></asp:Label>   
                        </div> 
                        <div class="colOggetti" style="width:55%">        
                            <asp:label id="lblValProg" CssClass="bold" runat="server"></asp:label>      
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width:15%">
                            <asp:Label ID="lblMese" CssClass="label" AssociatedControlID="lblValMese" runat="server" Text="Mese"></asp:Label>   
                        </div> 
                        <div class="colOggetti" style="width:15%">       
                            <asp:label id="lblValMese" CssClass="bold" runat="server"></asp:label>         
                        </div>
                        <div class="collable" style="width:15%">
                            <asp:Label ID="lblAnno" CssClass="label" AssociatedControlID="lblValAnno" runat="server" Text="Anno"></asp:Label>   
                        </div> 
                        <div class="colOggetti" style="width:55%">        
                            <asp:label id="lblValAnno" CssClass="bold" runat="server"></asp:label>      
                        </div>
                    </div>
                </div>
                <br />
                <asp:datagrid id="DtgDettaglioMensileVolontari" runat="server" CssClass="table" Width="100%" ToolTip="Elenco Assenze"  CellPadding="2" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
					<SelectedItemStyle BackColor="White"></SelectedItemStyle>
		            <EditItemStyle></EditItemStyle>
		            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		            <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		            <HeaderStyle></HeaderStyle>
					<Columns>
						<asp:BoundColumn DataField="Cod" HeaderText="Cod Volontario">
		                </asp:BoundColumn>
						<asp:BoundColumn DataField="nom" HeaderText="Nome">	
						</asp:BoundColumn>
						<asp:BoundColumn DataField="cogn" HeaderText="Cognome">
						</asp:BoundColumn>
						<asp:BoundColumn DataField="desci" HeaderText="Causale">
						</asp:BoundColumn>
						<asp:BoundColumn DataField="gio" HeaderText="N. Giorni">
						</asp:BoundColumn>
						<asp:BoundColumn DataField="codprogetto" HeaderText="Cod Progetto">
						</asp:BoundColumn>
						<asp:BoundColumn Visible="False" DataField="tit" HeaderText="Progetto">
						</asp:BoundColumn>
					</Columns>
				</asp:datagrid>
                <div class="wrapper" style="width:100%; border-style:none">
                    <div class="RigaVuota">&nbsp;</div>
                    <div class="row">
                        <div class="collable" style="width:65%">&nbsp;</div> 
                        <div class="collable" style="width:10%; text-align:right">
                            <asp:Label ID="lblTotVol" CssClass="label" AssociatedControlID="lblTotValVol" runat="server" Text="Tot. Volontari"></asp:Label>   
                        </div> 
                        <div class="colOggetti" style="width:4%; text-align:right">       
                            <asp:label id="lblTotValVol" CssClass="bold" runat="server"></asp:label>         
                        </div>
                        <div class="collable" style="width:16%; text-align:right">
                            <asp:Label ID="lblTotGiorni" CssClass="label" AssociatedControlID="lblValTotGiorni" runat="server" Text="Tot. Giorni"></asp:Label>   
                        </div> 
                        <div class="colOggetti" style="width:4%; text-align:right">        
                            <asp:label id="lblValTotGiorni" CssClass="bold" runat="server"></asp:label>      
                        </div>
                    </div>
                    <div class="RigaPulsanti">
                        <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
                        <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
		                <br />
	                    <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
                    </div>
                </div>
            </fieldset>
        </div>
    </form>
</body>
</html>
