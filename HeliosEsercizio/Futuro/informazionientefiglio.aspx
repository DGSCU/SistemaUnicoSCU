<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="informazionientefiglio.aspx.vb" Inherits="Futuro.informazionientefiglio" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head runat="server">
    <title>Futuro - Riepilogo Informazioni Ente Figlio</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function Stampa() {
            document.all.StampaPagina.style.visibility = 'hidden';
            window.print()
            window.close()
        }
	</script>

</head>
<body>
    <form id="frmEnteFiglio" runat="server" method="post">
        <fieldset class="ContornoPagina">
            <legend> Riepilogo Informazioni Ente Figlio</legend>
            <div class="wrapper"  style="width:100%;border-width:0px">
                <div class="row" style="text-align:right">
                    <img style="CURSOR: pointer" id="StampaPagina" onclick="javascript: Stampa()" name="StampaPagina" alt ="Stampa" src="images/printHELIOS.jpg"  width="50" height="34"/>
                </div>
                <div class="row" >
                    <div class="collable" style="width:15%">
                        <asp:Label id="lblStatoEnteLabel" AssociatedControlID="lblStato" CssClass="label" runat="server" Text="Stato Ente" ></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:85%">
                        <asp:Label id="lblStato" runat="server" CssClass="labelDati"></asp:Label>
                    </div>
                </div>
                <div class="row" >
                    <div class="collable" style="width:15%">
                        <asp:Label id="lblEnteLabel" AssociatedControlID="lblEnte" CssClass="label" runat="server" Text="Ente"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:85%">
                        <asp:Label id="lblEnte" runat="server" CssClass="labelDati"></asp:Label>
                    </div>
                </div>
                <div class="row" >
                    <div class="collable" style="width:15%">
                        <asp:Label id="lblCodiceFiscaleLabel" AssociatedControlID="lblCodiceFiscale" CssClass="label" runat="server" Text="Codice Fiscale" ></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:Label id="lblCodiceFiscale" runat="server" CssClass="labelDati"></asp:Label>
                    </div>
                    <div class="collable" style="width:15%">
                        <asp:Label id="lblFaxLabel" AssociatedControlID="lblFax" runat="server" CssClass="label" Text="Fax"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:Label id="lblFax" runat="server" CssClass="labelDati"></asp:Label>
                    </div>
                </div>
                <div class="row" >
                    <div class="collable" style="width:15%">
                        <asp:Label id="lblTelefonoLabel" AssociatedControlID="lblTelefono" CssClass="label" runat="server" Text="Telefono" ></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:Label id="lblTelefono" runat="server" CssClass="labelDati"></asp:Label>
                    </div>
                    <div class="collable" style="width:15%">
                        <asp:Label id="lblEmailLabel" AssociatedControlID="lblEmail" runat="server" CssClass="label" Text="Email"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:Label id="lblEmail" runat="server" CssClass="labelDati"></asp:Label>
                    </div>
                </div>
                <div class="row" >
                    <div class="collable" style="width:15%">
                        <asp:Label id="lblHTTPLabel" AssociatedControlID="lblHTTP" CssClass="label" runat="server" Text="HTTP" ></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:Label id="lblHTTP" runat="server" CssClass="labelDati"></asp:Label>
                    </div>
                    <div class="collable" style="width:15%">
                        <asp:Label id="lblTipologiaLabel" AssociatedControlID="lblTipologia" runat="server" CssClass="label" Text="Tipo"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:Label id="lblTipologia" runat="server" CssClass="labelDati"></asp:Label>
                    </div>
                </div> 
                <div class="row" >
                    <div class="collable" style="width:15%">
                        <asp:Label id="LblTipoRelazioneLabel" AssociatedControlID="LblTipoRelazione" CssClass="label" runat="server" Text="Tipo Relazione" ></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:Label id="LblTipoRelazione" runat="server" CssClass="labelDati"></asp:Label>
                    </div>
                </div>
                <div class="row" >
                     <div class="collable" style="width:15%">
                        <asp:Label id="lblIndirizzoLabel" AssociatedControlID="lblIndirizzo" runat="server" CssClass="label" Text="Indirizzo"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:Label id="lblIndirizzo" runat="server" CssClass="labelDati"></asp:Label>
                    </div>
                    <div class="collable" style="width:15%">
                        <asp:Label id="lblNumeroLabel" AssociatedControlID="lblNumero" CssClass="label" runat="server" Text="N." ></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:Label id="lblNumero" runat="server" CssClass="labelDati"></asp:Label>
                    </div>
                </div>
                <div class="row" >
                    <div class="collable" style="width:15%">
                        <asp:Label id="LblDettaglioRecapitoLabel" AssociatedControlID="LblDettaglioRecapito" CssClass="label" runat="server" Text="Dettaglio Recapito" ></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:85%">
                        <asp:Label id="LblDettaglioRecapito" runat="server" CssClass="labelDati"></asp:Label>
                    </div>
                </div>
                <div class="row" >
                     <div class="collable" style="width:15%">
                        <asp:Label id="lblComuneLabel" AssociatedControlID="lblComune" runat="server" CssClass="label" Text="Comune"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:Label id="lblComune" runat="server" CssClass="labelDati"></asp:Label>
                    </div>
                    <div class="collable" style="width:15%">
                        <asp:Label id="lblCAPLabel" AssociatedControlID="lblCAP" CssClass="label" runat="server" Text="CAP" ></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:Label id="lblCAP" runat="server" CssClass="labelDati"></asp:Label>
                    </div>
                </div>
                <br />
                <p style="text-align:center">
                    <asp:label id="lblSettoriIntervento" runat="server" CssClass="bold" Text="Settori Intervento Accreditati"></asp:label>
                </p>
                <asp:datagrid id="dtgSettori"  runat="server" CssClass="table"  Width="100%" ToolTip="Elenco Settori" CellPadding="2"  AllowSorting="True" AutoGenerateColumns="False" PageSize="4" UseAccessibleHeader="True">
			        <SelectedItemStyle BackColor="White"></SelectedItemStyle>
		            <EditItemStyle></EditItemStyle>
		            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		            <ItemStyle CssClass="tr" HorizontalAlign="left"></ItemStyle>
		            <HeaderStyle></HeaderStyle>
					<Columns>
						<asp:ButtonColumn Text="" CommandName="Select" Visible="False"></asp:ButtonColumn>
						<asp:BoundColumn Visible="False" DataField="IDMacroAmbitoAttivit&#224;" HeaderText="Settori di Intervento"></asp:BoundColumn>
						<asp:BoundColumn DataField="MacroAmbitoAttivit&#224;" HeaderText="Settori di Intervento"></asp:BoundColumn>
					</Columns>
				</asp:datagrid>
                <div class="RigaVuota" >&nbsp;</div>  
                <div class="row" style="text-align:right">
                    <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" OnClientClick="javascript: window.close();" Text="Chiudi" />
                </div>
            </div>
        </fieldset>
    </form>
</body>
</html>
