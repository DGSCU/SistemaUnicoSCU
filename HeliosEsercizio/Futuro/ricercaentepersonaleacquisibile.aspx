<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ricercaentepersonaleacquisibile.aspx.vb" Inherits="Futuro.ricercaentepersonaleacquisibile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head runat="server">
    <title>Personale acquisibile</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmPersonaleAcquisibile" runat="server"  method="post">
        <div class="fieldsetrasparente">
            <fieldset class="ContornoPagina">
                <legend>Personale Acquisibile</legend>
                <asp:datagrid id="dtgPersAcquisibile" runat="server" CssClass="table" Width="100%" ToolTip="Elenco Ricerca Sedi"  CellPadding="2" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" UseAccessibleHeader="True" PageSize="10">
					<SelectedItemStyle BackColor="White"></SelectedItemStyle>
		            <EditItemStyle></EditItemStyle>
		            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		            <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		            <HeaderStyle></HeaderStyle>
					<Columns>
						<asp:BoundColumn DataField="Nominativo" HeaderText="Nominativo"></asp:BoundColumn>
						<asp:BoundColumn DataField="Ruolo" HeaderText="Ruolo">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Posizione" HeaderText="Posizione">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Accreditato" HeaderText="Stato Accr.">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Stato" HeaderText="Stato">
							<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
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
