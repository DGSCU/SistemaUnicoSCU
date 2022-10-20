<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmCronologiaRiattivazione.aspx.vb" Inherits="Futuro.WfrmCronologiaRiattivazione" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cronologia Dettaglio Riattivazione</title>
    <link href="Styles/Site.css" rel="stylesheet"  type="text/css" media="screen" />
        <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
	    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
	    <meta name="vs_defaultClientScript" content="JavaScript"/>
	    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
</head>
<body>
    <form id="form1" method="post" runat="server">
   
    <fieldset class="ContornoPagina">
    <legend> Dettagli  Riattivazione</legend>
            <div class="wrapper" style="width:100%;border:0px">
             <h3>
                <asp:label id="lblsede"  runat="server" AssociatedControlID="dtgRisultatoRicerca" CssClass="msgInfo"  Text="Cronologia Riattivazione SEDE "></asp:label>
            </h3>
          <asp:datagrid id="dtgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco" 
		    AllowPaging="false" CellPadding="2"  CssClass="table" PageSize="50" 
            AllowSorting="false" AutoGenerateColumns="true" UseAccessibleHeader="True">
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr" HorizontalAlign="Center" Font-Size="Small" ></ItemStyle>
	    <HeaderStyle ForeColor="White"></HeaderStyle>
       
            
    </asp:datagrid>
    
            </div>
            <br />
            <div class="wrapper" style="width:100%;border:0px">
             <h3>
                <asp:label id="lblNominativo"  runat="server" AssociatedControlID="dtgRisultatoRicerca1" CssClass="msgInfo"  Text="Cronologia sospensione temporanea volontario: "></asp:label>
            </h3>
            <asp:datagrid id="dtgRisultatoRicerca1" runat="server" Width="100%" ToolTip="Elenco" 
		    AllowPaging="false" CellPadding="2"  CssClass="table" PageSize="50" 
            AllowSorting="false" AutoGenerateColumns="true" UseAccessibleHeader="True">
            <FooterStyle ForeColor="White"></FooterStyle>
	    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr" HorizontalAlign="Center" Font-Size="Small" ></ItemStyle>
	    <HeaderStyle ForeColor="White"></HeaderStyle>
       
              
            
            
            </asp:datagrid>
        </div>
    </fieldset>
     <div class="RigaVuota">&nbsp;</div>
      <div class="RigaPulsanti" style="text-align:right">
      <asp:Button ID="cmdIndietro"  runat="server"  CssClass="Pulsante" Text="Indietro" />&nbsp;
        <asp:Button ID="imgChiudi"  runat="server" OnClientClick="javascript: window.close()" CssClass="Pulsante" Text="Chiudi"  />&nbsp;
    </div> 
  
    </form>
</body>
</html>
