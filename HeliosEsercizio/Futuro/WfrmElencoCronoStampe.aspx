<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmElencoCronoStampe.aspx.vb" Inherits="Futuro.WfrmElencoCronoStampe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head runat="server">
    <title>Cronologia Stampa modulo "F" </title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmElencoCronoStampe" runat="server" method="post">
        <div class="fieldsetrasparente">
            <fieldset class="ContornoPagina">
                <legend> Cronologia Stampa modulo "F" </legend>
                <asp:datagrid id="dgRisultatoRicerca" runat="server" CssClass="table" Width="100%" ToolTip="Cronologia Stampa"  CellPadding="2" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" UseAccessibleHeader="True" PageSize="10">
					<SelectedItemStyle BackColor="White"></SelectedItemStyle>
		            <EditItemStyle></EditItemStyle>
		            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		            <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		            <HeaderStyle></HeaderStyle>
					<Columns>
						<asp:BoundColumn DataField="IdCronologiaStampa" HeaderText="Riferimento">
							<HeaderStyle Width="15%"></HeaderStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Titolo" HeaderText="Titolo Progetto">
							<HeaderStyle Width="55%"></HeaderStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="DataStampa" HeaderText="Data Stampa">
							<HeaderStyle Width="15%"></HeaderStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Username" HeaderText="Utente">
							<HeaderStyle Width="15%"></HeaderStyle>
						</asp:BoundColumn>
					</Columns>
					<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
				</asp:datagrid>
                <div class="wrapper" style="width:100%; border-style:none">
                    <div class="RigaVuota">&nbsp;</div>
                    <div class="RigaPulsanti">
                        <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
                    </div>
                </div>
            </fieldset>
        </div>
    </form>
</body>
</html>
