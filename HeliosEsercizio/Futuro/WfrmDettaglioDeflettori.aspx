<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmDettaglioDeflettori.aspx.vb" Inherits="Futuro.WfrmDettaglioDeflettori" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>Elenco eccezioni nell'assegnazione degli importi</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrapper" style="width:100%">
    <div class="headers">
       <h2>
      <asp:Label ID="lblTitolo" runat="server"  Text="Elenco eccezioni nell'assegnazione degli importi"></asp:Label>
      </h2>
      </div>

    <div class="RigaVuota">
       
    &nbsp;

    </div>
   
				<div class="row" >
                <div class="collable" style="width:15%">
                                       <asp:Label ID="lbltesto" AssociatedControlID="lblTitle" runat="server" Text="Dettaglio"></asp:Label>
                                      </div>
					<div class="colOggetti" style="width:85%">
						<asp:Label id="lblTitle" runat="server"></asp:Label></div>
				</div>
				<div class="row" >
                <div class="colOggetti" style="width:100%">
					<asp:label  id="lblmessaggiosopra" runat="server" CssClass="msgErrore" Visible="false"
							>Attenzione. Deflettori non presenti</asp:label>
                   </div>      
				</div>
				<div class="row" >
					<div class="colOggetti" style="width:100%">
						<asp:datagrid id="dtDeflettori" CssClass="table" Width="100%"  runat="server" 
							CellPadding="0" AllowSorting="True"  ToolTip="Elenco Deflettori"
							ShowFooter="True" UseAccessibleHeader="True">
								<FooterStyle ></FooterStyle>
											<SelectedItemStyle ></SelectedItemStyle>
											<EditItemStyle ></EditItemStyle>
											<AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
											<ItemStyle  CssClass="tr"></ItemStyle>
											<HeaderStyle ForeColor="White"></HeaderStyle>
						</asp:datagrid>
					</div>
				</div>

				<div class="row" >
					<div class="RigaPulsanti">
						 <asp:Button ID="cmdChiudi" CssClass="Pulsante" OnClientClick="javascript:window.close();" runat="server" Text="Chiudi"  /></div>
				  </div>
			
    </div>
    </form>
</body>
</html>
