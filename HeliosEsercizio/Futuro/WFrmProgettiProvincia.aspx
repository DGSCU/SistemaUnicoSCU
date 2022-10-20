<%@ Page Title="Progetti per Provincia" Language="vb" AutoEventWireup="false" CodeBehind="WFrmProgettiProvincia.aspx.vb" Inherits="Futuro.WFrmProgettiProvincia" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
	<head>
		<title>Progetti per Provincia</title>
		 <link href="Styles/Site.css" rel="stylesheet"  type="text/css" media="screen" />
        <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
	    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
	    <meta name="vs_defaultClientScript" content="JavaScript"/>
	    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
	</head>
	<body>
    <form id="form1" method="post" runat="server">
     <div class="wrapper" style="width:100%">
     <div class="RigaVuota">&nbsp;</div>
        <div class= "row" style="height:auto">  
	    <asp:label id="lblmessaggio" runat="server" CssClass="msgErrore" ></asp:label>
   </div>
    <asp:datagrid id="dgRisultatoRicerca" runat="server" Caption="Elenco Progetti per Provincia"  CssClass="table" CellPadding="2"  AllowPaging="false" 	
        AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True" ShowFooter="True" Width="100%">
		<FooterStyle></FooterStyle>
		<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
	    <Columns>
			<asp:BoundColumn DataField="CodiceEnte" HeaderText="Codice Progetto">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Titolo" HeaderText="Titolo">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Stato" HeaderText="Stato">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Volontari" HeaderText="Volontari">
			</asp:BoundColumn>
		</Columns>
		<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
	    <div class="RigaPulsanti"> 
        <asp:Button id="cmdChiudi" CssClass="Pulsante"  runat="server" Text="Chiudi" OnClientClick="javascript:window.close();" ></asp:Button>
             <asp:Button ID="cmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" />&nbsp;
                <br />
         <asp:HyperLink ID="ApriCSV1"   AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" 
        Text="DOWNLOAD CSV" runat="server" 
        ForeColor="#003399" Visible="False"></asp:HyperLink>
    </div>				
		</div>
    </form>
</body>
</html>
