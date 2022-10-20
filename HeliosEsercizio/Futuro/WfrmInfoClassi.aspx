<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmInfoClassi.aspx.vb" Inherits="Futuro.WfrmInfoClassi" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head runat="server">
    <title>Futuro - Dettagli Classi Accreditamento</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmDettagliClassiAccreditamenti" runat="server" method="post">
        <div class="fieldsetrasparente">
            <fieldset class="ContornoPagina">
                <legend>Dettagli Classi Accreditamento</legend>
                <asp:datagrid id="dgRisultatoRicerca" runat="server" CssClass="table" Width="100%" ToolTip="Dettagli Classi Accreditamento"  CellPadding="2" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
				    <SelectedItemStyle BackColor="White"></SelectedItemStyle>
		            <EditItemStyle></EditItemStyle>
		            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		            <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		            <HeaderStyle></HeaderStyle>
				    <Columns>
					    <asp:BoundColumn DataField="ClasseAccreditamento" HeaderText="Classe/Sezione Accreditamento">
						    <HeaderStyle HorizontalAlign="Center" Width="300px"></HeaderStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn DataField="MinsediTesto" HeaderText="N&#176; Sedi Min">
						    <HeaderStyle Width="100px"></HeaderStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn DataField="MaxSediTesto" HeaderText="N&#176; Sedi Max" DataFormatString="{0:N0}">
						    <HeaderStyle Width="100px"></HeaderStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn DataField="MinEntit&#224;perannoTesto" HeaderText="N&#176; Volontari Min">
						    <HeaderStyle Width="100px"></HeaderStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn DataField="MaxEntit&#224;perannoTesto" HeaderText="N&#176; Volontari Max" DataFormatString="{0:N0}">
						    <HeaderStyle Width="100px"></HeaderStyle>
					    </asp:BoundColumn>
				    </Columns>
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
