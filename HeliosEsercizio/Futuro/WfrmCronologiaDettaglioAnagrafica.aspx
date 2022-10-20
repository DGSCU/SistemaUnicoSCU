<%@ Page Title="Cronologia Dettaglio Anagrafica" Language="vb" AutoEventWireup="false" CodeBehind="WfrmCronologiaDettaglioAnagrafica.aspx.vb" Inherits="Futuro.WfrmCronologiaDettaglioAnagrafica" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head id="Head1" runat="server">
    <title>Cronologia Dettaglio Anagrafica</title>
    		 <link href="Styles/Site.css" rel="stylesheet"  type="text/css" media="screen" />
        <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
	    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
	    <meta name="vs_defaultClientScript" content="JavaScript"/>
	    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
</head>
<body>
	<form id="Form1" method="post" runat="server">
    <fieldset class="ContornoPagina">
    <legend> Dettagli volontario</legend>
    <div class="wrapper" style="width:100%;border:0px">
    <h3>
        <asp:label id="lblNominativo"  runat="server" AssociatedControlID="dtgRisultatoRicerca" CssClass="msgInfo"  Text="Cronologia Dettaglio Anagrafica: "></asp:label>
    </h3>
    <asp:datagrid id="dtgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco Volontari" 
		    AllowPaging="True" CellPadding="2"  CssClass="table" PageSize="20" 
            AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr" HorizontalAlign="Center" Font-Size="Small" ></ItemStyle>
	    <HeaderStyle></HeaderStyle>
        		<Columns>
						<asp:BoundColumn Visible="False" DataField="Nome" HeaderText="Nome">
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="Cognome" HeaderText="Cognome"></asp:BoundColumn>
				<asp:BoundColumn DataField="indi" HeaderText="Indirizzo">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Cap" HeaderText="Cap">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Nciv" HeaderText="Numero Civico">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Iban" HeaderText="Iban"></asp:BoundColumn>
				<asp:BoundColumn DataField="BIC_SWIFT" HeaderText="Bic/Swift"></asp:BoundColumn>
				<asp:BoundColumn DataField="Libretto" HeaderText="Libretto Postale"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="dataval" HeaderText="Data Inizio"></asp:BoundColumn>
				<asp:BoundColumn DataField="DataFine" HeaderText="Data Modifica"></asp:BoundColumn>
				<asp:BoundColumn DataField="Usermod" HeaderText="User Modificatore"></asp:BoundColumn>
				<asp:BoundColumn DataField="ComuneResidenza" HeaderText="Comune Residenza"></asp:BoundColumn>
			            <asp:BoundColumn DataField="gmo" HeaderText="GMO"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FAMI" HeaderText="FAMI"></asp:BoundColumn>
                        <asp:BoundColumn DataField="sedesecondaria" HeaderText="Sede Secondaria">
                        </asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;"	PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>					
    </asp:datagrid>
        <div class="RigaVuota">&nbsp;</div>
      <div class="RigaPulsanti">
        <asp:Button ID="imgChiudi"  runat="server" OnClientClick="javascript: window.close()" CssClass="Pulsante" Text="Chiudi"  />&nbsp;
    </div> 
        </div>
    </fieldset>
    </form>
</body>
</html>