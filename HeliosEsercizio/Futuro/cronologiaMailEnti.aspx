<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="cronologiaMailEnti.aspx.vb" Inherits="Futuro.cronologiaMailEnti" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head runat="server">
    <title>Cronologia Mail Ente</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmCronologiaMailEnte" runat="server" method="post">
        <div class="fieldsetrasparente">
            <fieldset class="ContornoPagina">
                <legend>Cronologia Mail Ente</legend>
                <asp:datagrid id="dtgCronologia" runat="server" CssClass="table" Width="100%" ToolTip="Cronologia mail Enti"  CellPadding="2" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
					<SelectedItemStyle BackColor="White"></SelectedItemStyle>
		            <EditItemStyle></EditItemStyle>
		            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		            <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		            <HeaderStyle></HeaderStyle>
					<Columns>
						<asp:BoundColumn DataField="VecchiaEmail" HeaderText="Vecchia Email"></asp:BoundColumn>
						<asp:BoundColumn DataField="NuovaEmail" HeaderText="Nuova Email"></asp:BoundColumn>
						<asp:BoundColumn DataField="VecchiaPEC" HeaderText="Vecchia PEC"></asp:BoundColumn>
						<asp:BoundColumn DataField="NuovaPEC" HeaderText="Nuova PEC"></asp:BoundColumn>
						<asp:BoundColumn DataField="Username" HeaderText="Username"></asp:BoundColumn>
						<asp:BoundColumn DataField="DataModifica" HeaderText="Data Modifica"></asp:BoundColumn>
					</Columns>
					<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
				</asp:datagrid> 
                <div class="wrapper" style="width:100%; border-style:none">
                    <div class="RigaVuota">&nbsp;</div>
                    <div class="RigaPulsanti">
                        <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
                    </div>
                </div>   
            </fieldset>   
        </div>
    </form>
</body>
</html>
