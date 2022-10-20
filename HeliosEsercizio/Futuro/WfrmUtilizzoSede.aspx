<%@ Page Title="Utilizzo Sede" Language="vb" AutoEventWireup="false" CodeBehind="WfrmUtilizzoSede.aspx.vb" Inherits="Futuro.WfrmUtilizzoSede" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head id="Head1" runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>Utilizzo Sede</title>
</head>
<body>
    <form id="form1" runat="server">
      <fieldset class="fieldsetrasparente">
     <div class="wrapper" style="width:100%;border:0px">

        <asp:datagrid id="dtgUtilizzoSede" runat="server" Width="100%"  ToolTip="Utilizzo Sede su Progetti Attivi" 
				Caption="Utilizzo Sede su Progetti Attivi" AllowPaging="true" CellPadding="2" Font-Size="Small"  CssClass="table"  
                AllowSorting="True" AutoGenerateColumns="false" UseAccessibleHeader="True" Visible="true">
			<FooterStyle></FooterStyle>
			<SelectedItemStyle Font-Bold="true" BackColor="White"></SelectedItemStyle>
			<EditItemStyle></EditItemStyle>
			<AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
			<ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
			<HeaderStyle></HeaderStyle>
            <Columns>
				<asp:BoundColumn DataField="BANDO" HeaderText="Bando">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="CODICE PROGETTO" HeaderText="Codice Progetto">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="TITOLO PROGETTO" HeaderText="Titolo Progetto">
				</asp:BoundColumn>
                <asp:BoundColumn DataField="STATO PROGETTO" HeaderText="Stato Progetto">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="POSTI CONCESSI" HeaderText="Posti Concessi">
				</asp:BoundColumn>
                    <asp:BoundColumn DataField="VOLONTARI IN SERVIZIO" HeaderText="Volontari in Servizio">
				</asp:BoundColumn>
            </Columns>
	         <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>                   
		</asp:datagrid>
           <div class="RigaVuota" >&nbsp;
        </div>
       <div class="RigaPulsanti" > 
        <asp:Button id="cmdChiudi" CssClass="Pulsante" OnClientClick="javascript:window.close();" runat="server" Text="Chiudi" ></asp:Button>&nbsp;
        <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="false"/>&nbsp;
                <br />
         <asp:HyperLink ID="ApriCSV1"   AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" 
        Text="DOWNLOAD CSV" runat="server" 
        ForeColor="#003399" Visible="False"></asp:HyperLink> 
           </div>
        </div>
    </fieldset>
    </form>
</body>
</html>
