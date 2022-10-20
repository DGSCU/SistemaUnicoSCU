<%@ Page Title="Personale Ente" Language="vb" AutoEventWireup="false" CodeBehind="ver_PersonaleEnte.aspx.vb" Inherits="Futuro.ver_PersonaleEnte" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">

<head runat="server">
     <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>Personale Ente</title>
</head>
<body>
    <form id="form1" runat="server">
    <fieldset class="ContornoPagina">
         <div class="wrapper" style="width:100%;border:0px">
            <div class="headers" >
            <h2><asp:Label ID="lblTitolo" runat="server"  Text="Dettaglio Ente"></asp:Label>
            </h2>
        </div>
            <br />
            <div class="row">
            <div class="collable" style="width:15%">          
                <asp:Label ID="LblEntePro" Text="Ente Proponente" AssociatedControlID="LblDescEntePro" CssClass="label" runat="server" />
            </div>
             <div class="colOggetti" style="width: 85%">
                <asp:Label ID="LblDescEntePro" CssClass="labelDati" runat="server" />
            </div>
        </div>
            <div class="row">
            <div class="collable" style="width:15%">          
                <asp:Label ID="LblProgetto" Text="Progetto" AssociatedControlID="LblDescProgetto" CssClass="label" runat="server" />
            </div>
             <div class="colOggetti" style="width: 85%">
                <asp:Label ID="LblDescProgetto" CssClass="labelDati" runat="server" />
            </div>
        </div>
            <!-- sede -->
            <div class="row">
            <div class="collable" style="width:15%">          
                <asp:Label ID="LblEnteFiglio" Text="Sede Attuazione" AssociatedControlID="LblDescEnteFiglio" CssClass="label" runat="server" />
            </div>
             <div class="colOggetti" style="width: 85%">
                <asp:Label ID="LblDescEnteFiglio" CssClass="labelDati" runat="server" />
            </div>
        </div>
            <!-- indirizzo -->
            <div class="row">
                <div class="collable" style="width:15%">          
                    <asp:Label ID="LblIndirizzo" Text="Indirizzo" AssociatedControlID="LblDescIndirizzo" CssClass="label" runat="server" />
                </div>
                 <div class="colOggetti" style="width: 35%">
                    <asp:Label ID="LblDescIndirizzo" CssClass="labelDati" runat="server" />
                </div>
         
                <div class="collable" style="width:15%">          
                    <asp:Label ID="LblCapE" Text="Cap" AssociatedControlID="LblDescCap" CssClass="label" runat="server" />
                </div>
                 <div class="colOggetti" style="width: 35%">
                    <asp:Label ID="LblDescCap" CssClass="labelDati" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">          
                    <asp:Label ID="LblComune" Text="Comune" AssociatedControlID="LblDescComune" CssClass="label" runat="server" />
                </div>
                 <div class="colOggetti" style="width: 35%">
                    <asp:Label ID="LblDescComune" CssClass="labelDati" runat="server" />
                </div>
         
                <div class="collable" style="width:15%">          
                    <asp:Label ID="LblRegione" Text="Regione" AssociatedControlID="LblDescRegione" CssClass="label" runat="server" />
                </div>
                 <div class="colOggetti" style="width: 35%">
                    <asp:Label ID="LblDescRegione" CssClass="labelDati" runat="server" />
                </div>
            </div>
            <div class="RigaPulsanti" > 
                <asp:Button id="cmdChiudi" CssClass="Pulsante" OnClientClick="javascript:window.close();" runat="server" Text="Chiudi" ></asp:Button>&nbsp;
            </div>
        </div>
        </fieldset>
        <fieldset class="ContornoPagina">
             <legend>Elenco Volontari</legend>
             <div class="wrapper" style="width:100%;border:0px">
                    <asp:datagrid id="dtgVolontariEnte" runat="server" Width="100%" AllowPaging="True"
                        PageSize="5" CssClass="table" AllowSorting="True" AutoGenerateColumns="False"
                        UseAccessibleHeader="True">
                        <FooterStyle></FooterStyle>
                        <SelectedItemStyle BackColor="White" Font-Bold="true"></SelectedItemStyle>
                        <EditItemStyle></EditItemStyle>
                        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                        <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle></HeaderStyle>
		                <Columns>
			                <asp:BoundColumn DataField="CodiceVolontario" ReadOnly="True" HeaderText="Cod. Vol.">
				                <HeaderStyle Wrap="False" HorizontalAlign="Center" Width="20%"></HeaderStyle>
				                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
			                </asp:BoundColumn>
			                <asp:BoundColumn DataField="Volontario" HeaderText="Volontario"></asp:BoundColumn>
			                <asp:BoundColumn DataField="CodiceFiscale" HeaderText="Codice Fiscale"></asp:BoundColumn>
			                <asp:BoundColumn DataField="DataInizioServizio" HeaderText="Data Inizio Servizio"></asp:BoundColumn>
			                <asp:BoundColumn DataField="DataFineServizio" HeaderText="Data Fine Servizio"></asp:BoundColumn>
			                <asp:BoundColumn DataField="OreFormazione" HeaderText="Ore Formazione"></asp:BoundColumn>
		                </Columns>
		                <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center"
                            CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
	                </asp:datagrid>
         
            </div>
        </fieldset>
        <fieldset class="ContornoPagina">
             <legend>Elenco Personale</legend>
                <div class="wrapper" style="width:100%;border:0px">
                    <asp:datagrid id="dtgPersonaleEnte" runat="server" Width="100%" AllowPaging="True"
                        PageSize="5" CssClass="table" AllowSorting="True" AutoGenerateColumns="False"
                        UseAccessibleHeader="True">
                        <FooterStyle></FooterStyle>
                        <SelectedItemStyle BackColor="White" Font-Bold="true"></SelectedItemStyle>
                        <EditItemStyle></EditItemStyle>
                        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                        <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle></HeaderStyle>
						<Columns>
							<asp:BoundColumn DataField="Nominativo" ReadOnly="True" HeaderText="Nominativo">
								<HeaderStyle Wrap="False" HorizontalAlign="Center" Width="70%"></HeaderStyle>
								<FooterStyle Wrap="False"></FooterStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="CodiceFiscale" ReadOnly="True" HeaderText="Codice Fiscale">
								<HeaderStyle Wrap="False" HorizontalAlign="Center" Width="15%"></HeaderStyle>
								<ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
								<FooterStyle Wrap="False"></FooterStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="DescrAbb" HeaderText="Ruolo"></asp:BoundColumn>
						</Columns>
						 <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center"
                            CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
					</asp:datagrid>
             </div>
        </fieldset>
    </form>
</body>
</html>
