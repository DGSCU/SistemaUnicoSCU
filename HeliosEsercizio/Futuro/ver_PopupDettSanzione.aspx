<%@ Page Title="Dettaglio Sanzione" Language="vb" AutoEventWireup="false" CodeBehind="ver_PopupDettSanzione.aspx.vb"
    Inherits="Futuro.ver_PopupDettSanzione" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head>
    <title>Sanzione</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" media="screen" />
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrapper" style="width: 100%">
        <div class="headers">
            <h2>
                <asp:Label ID="lbl1" runat="server" Text="Info Verifiche"></asp:Label>
            </h2>
        </div>
            <div class= "row" >
	    <asp:label id="LblInfo" runat="server" CssClass="msgErrore" Visible="false" ></asp:label>
   </div>
            <asp:DataGrid ID="dtgElenco" runat="server" 
            
         CssClass="table" CellPadding="2" PageSize = "10"  AllowPaging="true" 	
        AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True" ShowFooter="True" Width="100%">
		<FooterStyle></FooterStyle>
		<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
                <Columns>
                    <asp:BoundColumn DataField="Sede" HeaderText="Sede"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Indirizzo" HeaderText="Indirizzo"></asp:BoundColumn>
                    <asp:BoundColumn DataField="SedeAttuazione" HeaderText="Sede Attuazione"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Titolo" HeaderText="Progetto"></asp:BoundColumn>
                    <asp:BoundColumn DataField="NumeroVolontari" HeaderText="N&#176;Vol."></asp:BoundColumn>
                </Columns>
             <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
            </asp:DataGrid>
        
         <div class="RigaVuota">
                        &nbsp;</div>

          <div class="RigaPulsanti">
                 
                    <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Esci"></asp:Button>
                       
                </div>

    </div>
    </form>
</body>
</html>
