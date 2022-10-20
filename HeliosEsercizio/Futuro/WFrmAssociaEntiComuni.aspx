<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WFrmAssociaEntiComuni.aspx.vb" Inherits="Futuro.WFrmAssociaEntiComuni" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head runat="server">
    <title>Seleziona Comuni</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmSelezionaComuni" runat="server" method="post">
        <div class="fieldsetrasparente">
            <fieldset class="ContornoPagina">
                <legend>Seleziona Comuni</legend>
                <asp:label id="lblmessaggiosopra"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
                <br />
                <div class="wrapper" style="width:100%">
                    <div class="RigaVuota" >&nbsp;</div>
                    <div class="row" >
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblProvincia" CssClass="label" AssociatedControlID="ddlProvincia" runat="server" Text="Provincia/Nazione"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:25%">       
                            <asp:dropdownlist id="ddlProvincia" CssClass="ddlClass" AutoPostBack="true" runat="server"  ></asp:dropdownlist>           
                        </div>
                        <div class="colOggetti" style="width:20%">
                            <asp:CheckBox ID="ChkEstero" AutoPostBack="true" Text="Estero" ToolTip="Flag Estero" runat="server" />
                        </div>   
                        <div class="collable" style="width:10%">      
                            <asp:Label ID="lblComune" CssClass="label" AssociatedControlID="ddlComune" runat="server" Text="Comune"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:29%">       
                            <asp:dropdownlist id="ddlComune" CssClass="ddlClass" runat="server">
                                <asp:ListItem Value="0" Text=""></asp:ListItem>
                            </asp:dropdownlist>         
                        </div>    
                    </div>
                    <div class="RigaPulsanti">
                        <asp:Button ID="CmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />
                        <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
                        <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
		                <br />
	                    <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>&nbsp;
                    </div>
                </div>
                <br />
                <h3>
                <asp:label id="lblmessaggio" runat="server" Text="" AssociatedControlID="dtgComuniSelezionati" Visible ="false"></asp:label>
                </h3>

                <br />
                <asp:datagrid id="dtgComuniSelezionati" runat="server" Width="100%" ToolTip="Attività Ente" CssClass="table" CellPadding="2"  AllowPaging="True" PageSize="5" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
					<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		            <EditItemStyle></EditItemStyle>
		            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		            <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		            <HeaderStyle></HeaderStyle>
					<Columns>
						<asp:ButtonColumn Visible="False" Text="&lt;img src=images/ixs1.gif title='Cancella Ambiti' border=0&gt;"
							CommandName="Select"></asp:ButtonColumn>
						<asp:BoundColumn DataField="Comune" HeaderText="Comune">
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Provincia" HeaderText="Provincia">
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Regione" HeaderText="Regione">
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="NumeroSedi" HeaderText="Numero Sedi">
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
					</Columns>
					<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
				</asp:datagrid>
            </fieldset>
        </div>
    </form>
</body>
</html>
