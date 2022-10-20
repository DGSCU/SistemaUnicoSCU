<%@ Page Title="Elenco documenti" Language="vb" AutoEventWireup="false" CodeBehind="WfrmSIGEDElencoAllegati.aspx.vb" Inherits="Futuro.WfrmSIGEDElencoAllegati" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
	<head>
		<title>Elenco Allegati</title>
		 <link href="Styles/Site.css" rel="stylesheet"  type="text/css" media="screen" />
        <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
	    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
	    <meta name="vs_defaultClientScript" content="JavaScript"/>
	    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
	</head>
	<body>
    <form id="form1" method="post" runat="server">
     <div class="wrapper" style="width:100%">
       <div class="headers" >
        <h2>
            <asp:label id="lblAllegati" runat="server" Text="Elenco Allegati"></asp:label>
        </h2>
       </div>
        <div class= "row" >
	    <asp:label id="lblmessaggio" runat="server" CssClass="msgErrore" ></asp:label>
   </div>
    <asp:datagrid id="dgRisultatoRicerca" runat="server"  CssClass="table" CellPadding="2" PageSize = "10"  AllowPaging="true" 	
        AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True" ShowFooter="True" Width="100%">
		<FooterStyle></FooterStyle>
		<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
	    <Columns>   
            <asp:TemplateColumn> 
             <ItemTemplate>
			<asp:ImageButton ID="ImageButton1" Width="20px" CommandName="Select" STYLE="cursor:hand" runat="server" 
            title="Scarica Documento" AlternateText="Scarica Documento" ImageURL="Images/giu_small.png" CausesValidation="false">
                  </asp:ImageButton>                            
             </ItemTemplate>
                </asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="iddocumento" HeaderText="iddocumento"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="Codice Documento" HeaderText="Codice Documento"></asp:BoundColumn>
								<asp:BoundColumn DataField="Descrizione" HeaderText="Descrizione"></asp:BoundColumn>
								<asp:BoundColumn DataField="Nome File" HeaderText="Nome File"></asp:BoundColumn>
							</Columns>
							<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
						</asp:datagrid>
					
		</div>
    </form>
</body>
</html>
