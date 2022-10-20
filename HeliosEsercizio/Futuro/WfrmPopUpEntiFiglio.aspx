<%@ Page  Title="Fututo - Enti Figlio" Language="vb" AutoEventWireup="false" CodeBehind="WfrmPopUpEntiFiglio.aspx.vb" Inherits="Futuro.WfrmPopUpEntiFiglio" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head id="Head1" runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>Enti Figlio</title>
</head>
<body>
    <form id="form1" runat="server">
      <fieldset class="ContornoPagina">
     <div class="wrapper" style="width:100%;border:0px">
               <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Ente"></asp:Label>
               </h2>
            </div>
                    <div class="row" style="height:auto">
                <asp:Label ID="msgErrore" CssClass="msgErrore" runat="server"></asp:Label>
                <asp:Label ID="msgInfo" CssClass="msgInfo" runat="server"></asp:Label>
                <asp:Label ID="msgConferma" CssClass="msgConferma" runat="server"></asp:Label>
        </div>
        <br />
           <div class="row">
            <div class="collable" style="width:15%">          
                <asp:Label ID="lblDenominazione" Text="Denominazione" AssociatedControlID="txtdenominazione" CssClass="label" runat="server" />
            </div>
            <div class="colOggetti" style="width:85%">
                <asp:TextBox ID="txtdenominazione" CssClass="textbox" runat="server"></asp:TextBox>
           </div>
     
        </div>
        <div class="row">
            <div class="collable" style="width:15%">          
                <asp:Label ID="lblEnteFiglio" CssClass="label" runat="server" Text="Codice Ente Figlio" AssociatedControlID="txtEnteFiglio" />
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtEnteFiglio" CssClass="textbox" runat="server"></asp:TextBox>
           </div>
              <div class="collable" style="width:15%">          
                <asp:Label ID="lblCodFis" Text="Codice Fiscale" AssociatedControlID="txtCodFis" CssClass="label" runat="server" />
            </div>
                <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtCodFis" CssClass="textbox" runat="server" ></asp:TextBox>
           </div>
        </div>
               <div class="RigaPulsanti" > 
                <asp:Button id="cmdRicerca" CssClass="Pulsante" runat="server" Text="Ricerca" ></asp:Button>&nbsp;
        <asp:Button id="cmdChiudi" CssClass="Pulsante" OnClientClick="javascript:window.close();" runat="server" Text="Chiudi" ></asp:Button>&nbsp;
       
        </div>
    </div>
    <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%"  ToolTip="Utilizzo Sede su Progetti Attivi" 
				Caption="Risultato Ricerca Ente" AllowPaging="true"  PageSize="5"  CellPadding="2" Font-Size="Small"  CssClass="table"  
                AllowSorting="True" AutoGenerateColumns="false" UseAccessibleHeader="True" Visible="true">
			<FooterStyle></FooterStyle>
			<SelectedItemStyle Font-Bold="true" BackColor="White"></SelectedItemStyle>
			<EditItemStyle></EditItemStyle>
			<AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
			<ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
			<HeaderStyle></HeaderStyle>
            <Columns>
                <asp:TemplateColumn HeaderText="Selez."> 
                        <ItemTemplate>
	                        <asp:ImageButton ID="IdImgSelSedi" style="cursor:pointer;" CommandName="Select" alt="Seleziona Ente" ToolTip='Seleziona Sede' runat="server" ImageURL="images/ente.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                   </asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="IdEnteFiglio" HeaderText="IdFigliodiEnte"></asp:BoundColumn>
				<asp:BoundColumn DataField="Denominazione" HeaderText="Denominazione">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="CodiceRegione" HeaderText="Cod.EnteFiglio">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="codicefiscale" HeaderText="Cod.Fiscale">
				</asp:BoundColumn>
			</Columns>
	         <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>                   
		</asp:datagrid>
    </fieldset>
    </form>
</body>
</html>
