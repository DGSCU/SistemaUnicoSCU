<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="risultatoricercaIbanVolontari.aspx.vb" Inherits="Futuro.risultatoricercaIbanVolontari" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
	<head>
		<title>Elenco volontari con stesso codice IBAN</title>
		 <link href="Styles/Site.css" rel="stylesheet"  type="text/css" media="screen" />
        <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
	    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
	    <meta name="vs_defaultClientScript" content="JavaScript"/>
	    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
	</head>
	<body>
	<form id="Form1" method="post" runat="server">
    <fieldset class="ContornoPagina">
    <legend>Elenco Volontari</legend>
    <div class="wrapper" style="width:100%;border:0px">

  
  <asp:datagrid id="dgtElencoVolontari" runat="server"  CssClass="table" CellPadding="2" PageSize = "10"  AllowPaging="true" 	
        AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True" ShowFooter="false" Width="100%">
		<FooterStyle></FooterStyle>
		<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
								<Columns>
                                   <asp:TemplateColumn > 
                                        <ItemTemplate  >
											<asp:ImageButton  Enabled="false" AlternateText="Volontario" ToolTip="Volontario" runat="server" ImageURL="images/Icona_Volontario_small.png"></asp:ImageButton>      
                                            </ItemTemplate>
                                            </asp:TemplateColumn>
									<asp:BoundColumn Visible="False" DataField="identit&#224;"></asp:BoundColumn>
									<asp:BoundColumn DataField="CodiceVolontario" HeaderText="Codice Volontario"></asp:BoundColumn>
									<asp:BoundColumn DataField="Nominativo" HeaderText="Volontario"></asp:BoundColumn>
									<asp:BoundColumn DataField="IBAN" HeaderText="IBAN"></asp:BoundColumn>
									<asp:BoundColumn DataField="BIC_SWIFT" HeaderText="BIC/SWIFT"></asp:BoundColumn>
								</Columns>
	    
        <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
    </asp:datagrid>
                <div class="RigaPulsanti">
                 <asp:Button ID="cmdChiudi" CssClass="Pulsante" OnClientClick="javascript: window.close()"  Visible="true" runat="server"  Text="Chiudi"  />&nbsp;
              </div>
    </div>
    
    </fieldset>
		</form>
	</body>
</html>