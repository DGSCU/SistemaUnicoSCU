<%@ Page  Language="vb" AutoEventWireup="false"  CodeBehind="WfrmControlliGraduatoria.aspx.vb" Inherits="Futuro.WfrmControlliGraduatoria" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
 
<head id="Head1" runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>Controlli conferma Graduatoria</title>
</head>
 
<body>
    <form id="form1" runat="server">
         <fieldset class="ContornoPagina">
<legend>Controlli Graduatoria</legend>
     <div class="wrapper" style="width:100%">
      <div class="headers" >
            <h2>
            <asp:Label ID="Label5" runat="server"  Text="Elenco anomalie riscontrate"></asp:Label>
            </h2>
        </div>
    </div>
   
    <div class="RigaVuota" >&nbsp;</div>

    <div class="row" style="height:auto">
       	<asp:label id="lblMessaggioAlert" runat="server" Visible="true" CssClass="msgErroreGrid"  Width="463px">
        </asp:label>
      </div>

    <div class="RigaVuota" >&nbsp;</div>

     <div Id="DivControlloCapienzaSedi" class="row" runat="server" visible="false" >
    
            <asp:GridView id="dtgControlloCapienzaSedi" runat="server" Width="100%"  Caption="Controllo Capienza Sedi"   ShowHeaderWhenEmpty="true" 
                        CellPadding="2" Font-Size="Small"  CssClass="table"
                        AllowSorting="false" AutoGenerateColumns="true" UseAccessibleHeader="True">
                <FooterStyle></FooterStyle>
                    <SelectedRowStyle Font-Bold="true" BackColor="White"></SelectedRowStyle>
                    <EditRowStyle></EditRowStyle>
                    <AlternatingRowStyle  CssClass="tr"></AlternatingRowStyle>
                    <RowStyle CssClass="tr"   HorizontalAlign="Center"  ></RowStyle>
                    <HeaderStyle></HeaderStyle>
                    <PagerStyle CssClass="linkPageGrid"  />
                <PagerSettings  Mode="NumericFirstLast" Visible="true" pagebuttoncount="10" position="Bottom"></PagerSettings >
            </asp:GridView>
            <asp:Label ID="lblSedi" runat="server"  Text="NOTA: Elenco delle sedi che nel periodo indicato risultano sovrautilizzate."/>
            <br />
       </div>
        <div Id="DivControlloOLP" class="row" runat="server" visible="false" >
            <asp:GridView id="dtgControlloOLP" runat="server" Width="100%"  Caption="Controllo impegno OLP"   ShowHeaderWhenEmpty="true" 
                        CellPadding="2" Font-Size="Small"  CssClass="table" 
                        AllowSorting="false" AutoGenerateColumns="true" UseAccessibleHeader="True">
	            <FooterStyle></FooterStyle>
		            <SelectedRowStyle Font-Bold="true" BackColor="White"></SelectedRowStyle>
		            <EditRowStyle></EditRowStyle>
		            <AlternatingRowStyle  CssClass="tr"></AlternatingRowStyle>
		            <RowStyle CssClass="tr"   HorizontalAlign="Center"  ></RowStyle>
		            <HeaderStyle></HeaderStyle>
                    <PagerStyle CssClass="linkPageGrid"  />
                  <PagerSettings  Mode="NumericFirstLast" Visible="true" pagebuttoncount="10" position="Bottom"></PagerSettings >
	         </asp:GridView>
            <asp:Label ID="lblOlp" runat="server"  
                Text="NOTA: Elenco degli OLP che nel periodo indicato risultano sovrautilizzati sulle sedi visualizzate."/>
             <br /><br />
        </div>
        <div Id="DivControlloOLP1" class="row" runat="server" visible="false" >
            <asp:GridView id="dtgControlloOLP1" runat="server" Width="100%"  Caption="Controllo OLP come RLEA"   ShowHeaderWhenEmpty="true" 
                        CellPadding="2" Font-Size="Small"  CssClass="table" 
                        AllowSorting="false" AutoGenerateColumns="true" UseAccessibleHeader="True">
	            <FooterStyle></FooterStyle>
		            <SelectedRowStyle Font-Bold="true" BackColor="White"></SelectedRowStyle>
		            <EditRowStyle></EditRowStyle>
		            <AlternatingRowStyle  CssClass="tr"></AlternatingRowStyle>
		            <RowStyle CssClass="tr"   HorizontalAlign="Center"  ></RowStyle>
		            <HeaderStyle></HeaderStyle>
                    <PagerStyle CssClass="linkPageGrid"  />
                  <PagerSettings  Mode="NumericFirstLast" Visible="true" pagebuttoncount="10" position="Bottom"></PagerSettings >
	         </asp:GridView>
             <asp:Label ID="lblOlp1" runat="server"  
                Text="NOTA: Elenco degli OLP che nel periodo indicato risultano impegnati come RLEA sulle sedi visualizzate."/>
             <br /><br />
         </div>
         <div Id="DivControlloOLP2" class="row" runat="server" visible="false" >
            <asp:GridView id="dtgControlloOLP2" runat="server" Width="100%"  
                  Caption="Controllo OLP altra sede"   ShowHeaderWhenEmpty="True" 
                        CellPadding="2" Font-Size="Small"  CssClass="table">
	            <FooterStyle></FooterStyle>
		            <SelectedRowStyle Font-Bold="true" BackColor="White"></SelectedRowStyle>
		            <EditRowStyle></EditRowStyle>
		            <AlternatingRowStyle  CssClass="tr"></AlternatingRowStyle>
		            <RowStyle CssClass="tr"   HorizontalAlign="Center"  ></RowStyle>
		            <HeaderStyle></HeaderStyle>
                    <PagerStyle CssClass="linkPageGrid"  />
                  <PagerSettings  Mode="NumericFirstLast" Visible="true" pagebuttoncount="10" position="Bottom"></PagerSettings >
	         </asp:GridView>
             <asp:Label ID="lblOlp2" runat="server"  Text="NOTA: Elenco degli OLP che nel periodo indicato risultano impegnati su altre sedi."/>
             <br />
             <br />
         </div>
         <div Id="DivControlloRLEA" class="row" runat="server" visible="false" >
            <asp:GridView id="dtgControlloRLEA" runat="server" Width="100%"  Caption="Controllo RLEA come OLP"   ShowHeaderWhenEmpty="true"
                        CellPadding="2" Font-Size="Small"  CssClass="table"  
                        AllowSorting="false" AutoGenerateColumns="true" UseAccessibleHeader="True">
		        <FooterStyle></FooterStyle>
		            <SelectedRowStyle Font-Bold="true" BackColor="White"></SelectedRowStyle>
		            <EditRowStyle></EditRowStyle>
		            <AlternatingRowStyle  CssClass="tr"></AlternatingRowStyle>
		            <RowStyle CssClass="tr"   HorizontalAlign="Center"  ></RowStyle>
		            <HeaderStyle></HeaderStyle>
                <PagerStyle CssClass="linkPageGrid"  />
                <PagerSettings Mode="NumericFirstLast" Visible="true" pagebuttoncount="10" position="Bottom"></PagerSettings >
	        </asp:GridView>
            <asp:Label ID="lblRLEA" runat="server"  
                 Text="NOTA: Elenco degli RLEA che nel periodo indicato risultano impegnati come OLP sulle sedi visualizzate."/>
          <br /><br />
        </div>
            <div class="wrapper" style="width:100%;border:0px">
              <div class="RigaPulsanti" > 
                <asp:Button id="cmdSbloccaProgetto" CssClass="Pulsante" OnClientClick="" runat="server" Text="Sblocca Progetto" ></asp:Button>
                <asp:Button id="cmdChiudi" CssClass="Pulsante" OnClientClick="javascript:window.close();" runat="server" Text="Chiudi" ></asp:Button>
            </div>
        </div>
    </fieldset>
    </form>
</body>
</html>
