<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmCronologiaDettaglioSede.aspx.vb" Inherits="Futuro.WfrmCronologiaDettaglioSede" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>Cronologia Sede</title>
 
</head>
<body>
    <form id="form1" runat="server">
      <fieldset class="fieldsetrasparente">
     <div class="wrapper" style="width:100%;border:0px">

            <asp:GridView id="dtgSede" runat="server" Width="100%"  ToolTip="Cronologia sedi" ShowHeaderWhenEmpty="true" 
				Caption="Cronologia sedi" AllowPaging="true" CellPadding="2" Font-Size="Small"  CssClass="table"  
                AllowSorting="false" AutoGenerateColumns="true" UseAccessibleHeader="True" Visible="true"  PageSize="10">
	                <FooterStyle></FooterStyle>
		            <SelectedRowStyle Font-Bold="true" BackColor="White"></SelectedRowStyle>
		            <EditRowStyle></EditRowStyle>
		            <AlternatingRowStyle  CssClass="tr"></AlternatingRowStyle>
		            <RowStyle CssClass="tr"   HorizontalAlign="Center"  ></RowStyle>
		            <HeaderStyle></HeaderStyle>
                    <PagerStyle CssClass="linkPageGrid"  />
                  <PagerSettings  Mode="NumericFirstLast" Visible="true" pagebuttoncount="10" position="Bottom"></PagerSettings >
	            </asp:GridView>
         </div> 
          <div class="RigaVuota" >&nbsp;
        
       <div class="RigaPulsanti" > 
        <asp:Button id="cmdChiudi" CssClass="Pulsante" OnClientClick="javascript:window.close();" runat="server" Text="Chiudi" ></asp:Button>&nbsp;
      
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
