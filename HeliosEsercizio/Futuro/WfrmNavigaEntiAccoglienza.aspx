<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmNavigaEntiAccoglienza.aspx.vb" Inherits="Futuro.WfrmNavigaEntiAccoglienza" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="Styles/Site.css" rel="stylesheet" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <div id="diverrore"  runat="server">
        <asp:label id="lblControlloClasse"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
       </div>
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Enti Disponibili"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
                <div class="collable" style="width:10%">      
                    <asp:Label ID="lblDenominazione" CssClass="label" AssociatedControlID="txtdenominazione" runat="server" Text="Denominazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:23%">       
                    <asp:TextBox ID="txtdenominazione" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:10%">      
                    <asp:Label ID="lblCodEnte" CssClass="label" AssociatedControlID="txtCodRegione" runat="server" Text="Cod. Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:23%">       
                    <asp:TextBox ID="txtCodRegione" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:10%">      
                    <asp:Label ID="Label1" CssClass="label" AssociatedControlID="TxtCodiceFiscale" runat="server" Text="Cod. Fis."></asp:Label>
                </div>
                <div class="colOggetti" style="width:23%">       
                    <asp:TextBox ID="TxtCodiceFiscale" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:10%">      
                    <asp:Label ID="lblTipoRelazione" CssClass="label" AssociatedControlID="ddlCAccreditamento" runat="server" Text="Tipo Relazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:23%">       
                     <asp:DropDownList ID="ddlCAccreditamento" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                </div>
                <div class="collable" style="width:10%">      
                    <asp:Label ID="lblStatoEnte" CssClass="label" AssociatedControlID="CboStatoEnte" runat="server" Text="Stato Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:23%">       
                     <asp:DropDownList ID="CboStatoEnte" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                </div>
                 <div class="collable" style="width:10%">      
                    <asp:Label ID="Label2" CssClass="label" AssociatedControlID="ddltipologia" runat="server" Text="Tipologia"></asp:Label>
                </div>
                <div class="colOggetti" style="width:23%">       
                     <asp:DropDownList ID="ddltipologia" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                </div> 
            </div>
            <div class="row">
                <div class="collable" style="width:10%">      
                </div>
                <div class="colOggetti" style="width:23%">       
                </div>
                <div class="collable" style="width:10%">      
                </div>
                <div class="colOggetti" style="width:23%">       
                </div>
                <div class="collable" style="width:10%">      
                </div>
                <div class="colOggetti" style="width:23%">       
                </div> 
                <asp:PlaceHolder id="phStatoAccreditamento" Visible="false" runat="server">
                    <div class="collable" style="width:10%">      
                        <asp:Label ID="Label4" CssClass="label" AssociatedControlID="ddlstato" runat="server" Text="Stato Acc."></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:23%">       
                         <asp:DropDownList ID="ddlstato" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                    </div>  
                </asp:PlaceHolder>  
            </div>
            
            <div class="RigaPulsanti">
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Indietro" />
                <asp:Button ID="cmdChiudiPagina" CssClass="Pulsante" OnClientClick="javascript: window.close();" runat="server" 
                    Text="Chiudi" />
                <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
		        <br />
	            <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>&nbsp;
            </div>
        </div>
        <h3>
            <asp:label id="lblMessaggi" runat="server" AssociatedControlID="dgRicercaEnte"></asp:label>
        </h3>
        <br />
        <asp:datagrid id="dgRicercaEnte" runat="server" Width="100%" ToolTip="Elenco Enti" CssClass="table" CellPadding="2"  AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"  Visible="False" UseAccessibleHeader="True">	
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
           
               <%-- <asp:TemplateColumn HeaderText="Sel.">
                    <ItemTemplate>
					    <asp:ImageButton ID="ImbBtnEnte" CommandName="Select" ToolTip="Seleziona Ente" AlternateText="Seleziona Ente" runat="server" ImageURL="images/ente-mini.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>--%>
				<asp:BoundColumn DataField="CodiceRegione" HeaderText="Cod.Ente">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Denominazione" HeaderText="Denominazione"></asp:BoundColumn>
				<asp:BoundColumn DataField="tiporelazione" HeaderText="Tipo di Relazione"></asp:BoundColumn>
				<asp:BoundColumn DataField="Tipologia" HeaderText="Tipologia"></asp:BoundColumn>
				<asp:BoundColumn DataField="ClasseAccreditamento" Visible="False" HeaderText="Classe"></asp:BoundColumn>
				<asp:BoundColumn DataField="Http" Visible="False" HeaderText="HTTP"></asp:BoundColumn>
				<asp:BoundColumn DataField="Email" Visible="False" HeaderText="E-mail"></asp:BoundColumn>
                <asp:BoundColumn DataField="CodiceFiscale" Visible="True" HeaderText="CodiceFiscale"></asp:BoundColumn>
                <asp:TemplateColumn  HeaderText="Numero Sedi Accreditate">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="CMDsediaccr" runat="server" ToolTip="sedi"  CommandName="NumeroTotSedi"  HeaderText="Totale Sedi"><%# Eval("Numerototalesedi2")%></asp:LinkButton>
                                        </ItemTemplate>
                                </asp:TemplateColumn>
				<asp:BoundColumn Visible="false" DataField="idente" HeaderText="idente"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="Stato" HeaderText="Stato Accordo">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
                  <asp:TemplateColumn HeaderText="Info" > 
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lblInfo" CommandName="Info" ToolTip="Info" runat="server">Info</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
				<asp:BoundColumn Visible="false" DataField="identerelazione" HeaderText="identeRelazione"></asp:BoundColumn>
				<asp:BoundColumn DataField="StatoEnte" HeaderText="Stato Ente">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="idstatoente" HeaderText="idstatoente"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="DataInizioValidita" HeaderText="Data Inserimento" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:d}"></asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
    </div>
    </form>
</body>
</html>
