<%@ Page Title="Elenco Prefissi Documenti" Language="vb" AutoEventWireup="false" CodeBehind="wfrmInfoPrefissiDocumenti.aspx.vb" Inherits="Futuro.wfrmInfoPrefissiDocumenti" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" lang="it-it" />
    <title>Elenco Prefissi Documenti</title>
</head>
<body>
    <form id="form1" runat="server">
      <fieldset class="fieldsetrasparente">
     <div class="wrapper" style="width:100%;border:0px">

        <asp:datagrid id="dgElencoPrefissi" runat="server" Width="100%"  ToolTip="Elenco Prefissi Documenti" 
				Caption="Elenco Prefissi Documenti" AllowPaging="false" CellPadding="2" Font-Size="Small"  CssClass="table"  
                AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
			<FooterStyle></FooterStyle>
			<SelectedItemStyle Font-Bold="true" BackColor="White"></SelectedItemStyle>
			<EditItemStyle></EditItemStyle>
			<AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
			<ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
			<HeaderStyle></HeaderStyle>
			<Columns>
				<asp:BoundColumn DataField="Prefisso" HeaderText="Prefisso">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="TipologiaDocumento" HeaderText="Tipologia Documento">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Modalit&#224;Invio" HeaderText="Modalit&#224; Invio">
				</asp:BoundColumn>
			</Columns>                              
		</asp:datagrid>
           <div class="RigaVuota" >&nbsp;
        </div>
       <div class="RigaPulsanti" > 
        <asp:Button id="cmdChiudi" CssClass="Pulsante" OnClientClick="javascript:window.close();" runat="server" Text="Chiudi" ></asp:Button>&nbsp;
            </div>
        </div>
    </fieldset>
    </form>
</body>
</html>
