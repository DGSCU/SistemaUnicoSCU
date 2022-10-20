<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="dettaglirisorsa.aspx.vb" Inherits="Futuro.dettaglirisorsa" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head runat="server">
    <title>Dettagli Risorsa</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmDettagliRisorsa" runat="server" method="post">
        <div class="fieldsetrasparente">
            <fieldset class="ContornoPagina">
                <legend>Dettagli Risorsa</legend>
                <asp:label id="lblErr"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
                <br />
                <div class="wrapper" style="width:100%">
                    <div class="RigaVuota" >&nbsp;</div>
                    <div class="row">
                        <div class="collable" style="width:15%">
                            <asp:Label ID="lblNominativo" CssClass="label" AssociatedControlID="txtNominativo" runat="server" Text="Nominativo"></asp:Label>   
                        </div> 
                        <div class="colOggetti" style="width:84%">       
                            <asp:label id="txtNominativo" CssClass="bold" runat="server"></asp:label>         
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width:15%">
                            <asp:Label ID="lblRuolo" CssClass="label" AssociatedControlID="txtRuolo" runat="server" Text="Ruolo"></asp:Label>   
                        </div> 
                        <div class="colOggetti" style="width:84%">       
                            <asp:label id="txtRuolo" CssClass="bold" runat="server"></asp:label>         
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width:15%">
                            <asp:Label ID="lblDataAccreditamento" CssClass="label" AssociatedControlID="txtDataAccreditamento" runat="server" Text="Data iscrizione"></asp:Label>   
                        </div> 
                        <div class="colOggetti" style="width:84%">       
                            <asp:label id="txtDataAccreditamento" CssClass="bold" runat="server"></asp:label>         
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width:15%">
                            <asp:Label ID="lblDataNascita" CssClass="label" AssociatedControlID="txtDataNascita" runat="server" Text="Data nascita"></asp:Label>   
                        </div> 
                        <div class="colOggetti" style="width:84%">       
                            <asp:label id="txtDataNascita" CssClass="bold" runat="server"></asp:label>         
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width:15%">
                            <asp:Label ID="lblComuneNascita" CssClass="label" AssociatedControlID="txtComuneNascita" runat="server" Text="Comune nascita"></asp:Label>   
                        </div> 
                        <div class="colOggetti" style="width:84%">       
                            <asp:label id="txtComuneNascita" CssClass="bold" runat="server"></asp:label>         
                        </div>   
                    </div> 
                </div>          
                <asp:datagrid id="dtgRisultatoRicerca" runat="server" CssClass="table" Width="100%" ToolTip="Dettagli Risorsa"  CellPadding="2" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" UseAccessibleHeader="True" PageSize="5">
					<SelectedItemStyle BackColor="White"></SelectedItemStyle>
		            <EditItemStyle></EditItemStyle>
		            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		            <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		            <HeaderStyle></HeaderStyle>
					<Columns>
						<asp:BoundColumn DataField="CodiceEnte" HeaderText="Codice Progetto">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="descrizione" HeaderText="Progetto">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="DataInizioAttivit&#224;" HeaderText="Data Inizio Progetto">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="DataFineAttivit&#224;" HeaderText="Data Fine Progetto">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="StatoAttivit&#224;" HeaderText="Stato Progetto">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="identesedeattuazione" HeaderText="Codice Sede">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="nomesede" HeaderText="Sede">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn Visible="False" DataField="nomesedeattuazione" HeaderText="Sede Attuazione">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="comunesede" HeaderText="Comune Sede">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn Visible="False" DataField="DataAssegnazione" HeaderText="Data Inizio Assegnazione">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn Visible="False" DataField="DataFineAssegnazione" HeaderText="Data Fine Assegnazione">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Settore" HeaderText="Settore">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
					</Columns>
					<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
				</asp:datagrid>
                <div class="wrapper" style="width:100%; border-style:none">
                    <div class="RigaVuota">&nbsp;</div>
                    <div class="RigaPulsanti">
                        <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
                    </div>
                </div>
            </fieldset>
        </div>
    </form>
</body>
</html>
