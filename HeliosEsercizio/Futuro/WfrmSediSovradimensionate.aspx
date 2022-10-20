<%@ Page Title ="Sedi Sovradimensionate" Language="vb" AutoEventWireup="false" CodeBehind="WfrmSediSovradimensionate.aspx.vb" Inherits="Futuro.WfrmSediSovradimensionate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" lang="it-it" xml:lang="it-it">
<head id="Head1" runat="server">
<link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>Sedi Sovradimensionate</title>
</head>
<body>
    <form id="frmCronologiaDocumenti" runat="server" method="post">
        <div class="fieldsetrasparente">
            <fieldset class="ContornoPagina">
                <legend>Segnalazione Sedi Sovradimensionate</legend>
                <h2><asp:Label runat="server" ID="LabelNoSedi" Text="Nessuna Sede Sovradimensionata" Visible="false"></asp:Label></h2>
                <asp:datagrid id="dtgSediSovradimensionate" runat="server" CssClass="table" Width="100%" Caption="Sedi Sovradimensionate"  
                CellPadding="5"  AllowSorting="false" AllowPaging="false" AutoGenerateColumns="true" UseAccessibleHeader="True" 
                >
					<SelectedItemStyle BackColor="White"></SelectedItemStyle>
		            <EditItemStyle></EditItemStyle>
		            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		            <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		            <HeaderStyle></HeaderStyle>
					<Columns ></Columns>
					<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;"  HorizontalAlign="Right" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
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

