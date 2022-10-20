<%@ Page Title="Elenco eccezioni nell'assegnazione degli importi" Language="vb" AutoEventWireup="false" CodeBehind="WfrmDettaglioAssegnazioneImportiVirtuali.aspx.vb" Inherits="Futuro.WfrmDettaglioAssegnazioneImportiVirtuali" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">

<head id="Head1" runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>Elenco eccezioni nell'assegnazione degli importi</title>
</head>
<body>
    <form id="form1" runat="server">
    <fieldset class="fieldsetrasparente">
    <div class="wrapper" style="width:100%">
      <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Elenco eccezioni nell'assegnazione degli importi"></asp:Label></h2>
            </div>
            <br />
      <div class="row">
		    <asp:label id="lblInfo"  runat="server" CssClass="msgInfo"></asp:label>
            <asp:label id="lblErrore"  runat="server" CssClass="msgErrore"></asp:label>
    </div>
    <br />
    <asp:datagrid id="dtgAssegna" runat="server" Width="100%" Caption="Elenco progetti con importi virtuali errati"
		    AllowPaging="true" CellPadding="2"  PageSize="10" CssClass="table" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
	    <HeaderStyle></HeaderStyle>
       <Columns>
			<asp:BoundColumn DataField="Titolo" HeaderText="Progetto"></asp:BoundColumn>
			<asp:BoundColumn DataField="Descrizione" HeaderText="Tipologia Progetto"></asp:BoundColumn>
			<asp:BoundColumn DataField="Volontariattivit&#224;" HeaderText="Volontari Attivit&#224;">
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="volontarisediatt" HeaderText="Volontari Sede Attuazione">
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="idbando"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" HeaderImageUrl="idattivit&#224;"></asp:BoundColumn>
		</Columns>
         <PagerStyle CssClass="linkPageGrid" HorizontalAlign="Center" 
                Mode="NumericPages" NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" />				
    </asp:datagrid>
      <div class="wrapper" style="width:100%;border:0px">
           <div class="RigaPulsanti">
            <asp:Button ID="imgchiudi" runat="server" CssClass="Pulsante" Text="Chiudi" OnClientClick="javascript:window.close();" />
   </div>
   </div>
    </fieldset>
    </form>
</body>
</html>

