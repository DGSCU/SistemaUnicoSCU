<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AccreditamentiRisorseNegativi.aspx.vb" Inherits="Futuro.AccreditamentiRisorseNegativi" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>Accreditamenti Negativi</title>
    <script language="javascript" type="text/javascript">
        </script>
</head>
<body>
    <form id="form1" runat="server">
    <fieldset class="ContornoPagina">
    <legend>Accreditamenti Negativi</legend>
    
    <div class="wrapper"  style="width:100%;border-width:0px">
        <div class="row" >
            <div class="collable" style="width:15%">      
                <asp:Label ID="lblCognome" CssClass="label" AssociatedControlID="txtCognome" runat="server" Text="Cognome"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">       
                <asp:TextBox ID="txtCognome" CssClass="textbox" runat="server" ReadOnly="True"></asp:TextBox>         
            </div>
            <div class="collable" style="width:15%">      
                <asp:Label ID="lblNome" CssClass="label" AssociatedControlID="txtNome" runat="server" Text="Nome"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">       
                <asp:TextBox ID="txtNome" CssClass="textbox" runat="server" ReadOnly="True"></asp:TextBox>         
            </div>
        </div>

        <div class="row" >
            <div class="collable" style="width:15%">      
                <asp:Label ID="lblCodiceFiscale" CssClass="label" AssociatedControlID="txtCodiceFiscale" runat="server" Text="Codice Fiscale"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">       
                <asp:TextBox ID="txtCodiceFiscale" CssClass="textbox" runat="server" ReadOnly="True"></asp:TextBox>         
            </div>
        </div>        
        
        <asp:datagrid id="dtgRicerca" runat="server" Width="100%" 
            ToolTip="Elenco Accreditamenti" CssClass="table" CellPadding="2" PageSize="8" 
            AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
				<asp:BoundColumn Visible="false" DataField="IDRuolo"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="IDEnte" ></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="IDEntePersonaleRuolo"></asp:BoundColumn>
				<asp:BoundColumn DataField="DataAccreditamento" HeaderText="Data Presentazione" 
                    Visible="False"></asp:BoundColumn>
				<asp:BoundColumn DataField="CodiceRegione" HeaderText="Codice Ente"></asp:BoundColumn>
				<asp:BoundColumn DataField="Competenza" HeaderText="Competenza">
                </asp:BoundColumn>
				<asp:BoundColumn DataField="Denominazione" HeaderText="Ente"></asp:BoundColumn>
				<asp:BoundColumn DataField="Ruolo" HeaderText="Ruolo"></asp:BoundColumn>
				<asp:BoundColumn DataField="DataValutazione" HeaderText="Data Valutazione Negativa">
                </asp:BoundColumn>
			    <asp:BoundColumn DataField="UsernameAccreditatore" HeaderText="Valutatore">
                </asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>

        <div class="row" style="text-align:right">
            <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" OnClientClick="javascript: window.close();" Text="Chiudi" />           
        </div>
    
    </div>
    </fieldset>			

    </form>
</body>
</html>
