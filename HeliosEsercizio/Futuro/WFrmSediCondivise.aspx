<%@ Page Title="Anomalia Sedi Condivise" Language="vb" AutoEventWireup="false" CodeBehind="WFrmSediCondivise.aspx.vb" Inherits="Futuro.WFrmSediCondivise" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" lang="it-it" xml:lang="it-it">
<head runat="server">
<link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>Anomalia Sedi Duplicate</title>
</head>
<body>
    <form id="frmCronologiaDocumenti" runat="server" method="post">
        <div class="fieldsetrasparente">
            <fieldset class="ContornoPagina">
                <legend>Anomalia Sedi Duplicate</legend>
                <asp:datagrid id="dtgSediCondivise" runat="server" CssClass="table" Width="100%" Caption="Sedi Duplicate"  
                CellPadding="2" AllowSorting="false" AllowPaging="True" AutoGenerateColumns="true" UseAccessibleHeader="True"
                PageSize="5">
					<SelectedItemStyle BackColor="White"></SelectedItemStyle>
		            <EditItemStyle></EditItemStyle>
		            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		            <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		            <HeaderStyle></HeaderStyle>
					
					<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
				</asp:datagrid>
                <div class="wrapper" style="width:100%; border-style:none">
                    <div class="RigaVuota">&nbsp;</div>
                    <div class="RigaPulsanti">
                        <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" OnClientClick="javascript: window.close()" />
                         <asp:Button ID="cmdEsporta" runat="server" ToolTip="Esporta OLP su sedi diverse" CssClass="Pulsante" 
                Text="Esporta CSV" />
            &nbsp;
            <br />
            <asp:HyperLink ID="ApriCSV1" runat="server" AccessKey="S" CssClass="linkStampa" 
                ForeColor="#003399" Text="DOWNLOAD CSV" 
                ToolTip="Link per la stampa del risultato della ricerca" Visible="False"></asp:HyperLink>
                    </div>
                </div>
            </fieldset>  
        </div>
    </form>
</body>
</html>

