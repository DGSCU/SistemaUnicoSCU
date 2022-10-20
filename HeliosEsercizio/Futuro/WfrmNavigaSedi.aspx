<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmNavigaSedi.aspx.vb" Inherits="Futuro.WfrmNavigaSedi" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="Styles/Site.css" rel="stylesheet" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
</head>
 
<body>
    <form id="form1" runat="server">
    <div>
    <div class="wrapper" style="width:100%">
        
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Sede"></asp:Label></h2>
            </div>
                 <div class="rowGroup" style="height:auto">
        <div class="RigaVuota">

      &nbsp;
      </div>
         <asp:label id="lblErrore"  runat="server" CssClass="msgErrore" ></asp:label>
        <div class="RigaVuota">
      &nbsp;</div>
    </div>
            <div class="RigaVuota" >
            &nbsp;
                </div>
            
               <div id="Div1" class="row"  runat="server">
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDenominazione" CssClass="label" AssociatedControlID="txtdenominazione" runat="server" Text="Denominazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtdenominazione"  CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblStato" CssClass="label" AssociatedControlID="ddlstato" runat="server" Text="Stato"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList id="ddlstato"  runat="server" CssClass="ddlClass"></asp:DropDownList>        
                </div>
            </div>
            <div id="Div2" class="row"  runat="server">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblRegione" CssClass="label" AssociatedControlID="txtregione" runat="server" Text="Regione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtregione" CssClass="textbox" runat="server"></asp:TextBox>         
                </div> 
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblStatoUtenza" CssClass="label" Visible="false" AssociatedControlID="ddlTipologia" runat="server" Text="Tipologia"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlTipologia" Visible="false" runat="server">
                    </asp:DropDownList>
                </div> 
            </div>
            <div id="Div3" class="row"  runat="server">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblComune" CssClass="label" AssociatedControlID="txtComune" runat="server" Text="Comune"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtComune" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblProvincia" CssClass="label" AssociatedControlID="txtProvincia" runat="server" Text="Provincia"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtProvincia" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            
            <div  class="row"  >
                    <div class="collable" style="width:15%">      
                    <asp:Label ID="lblIndirizzo" CssClass="label" AssociatedControlID="txtIndirizzo" runat="server" Text="Indirizzo"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtIndirizzo" CssClass="textbox" runat="server"></asp:TextBox>      
                 </div>    
                <div id="divCertificazione" runat="server"  >
                    <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodSedeAtt" CssClass="label" AssociatedControlID="txtCodSedeAtt" runat="server" Text="Cod. Sede Attuazione"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodSedeAtt" CssClass="textbox" runat="server"></asp:TextBox>         
                    </div> 
                 </div>
            </div>
         
              
              
              

            <div class="RigaPulsanti">
            
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" 
                    Text="Indietro"  />&nbsp;<asp:Button ID="cmdChiudiPagina" 
                    CssClass="Pulsante" runat="server" Text="Chiudi" OnClientClick="javascript: window.close();" />&nbsp;<asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
		        <br />
	            <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
              &nbsp;
            </div>
    </div>
    <br />
    <br />
    <br />
    <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" Caption="Risultato Ricerca Sedi Ente"
                    ToolTip="Elenco Sedi" CssClass="table"  AllowPaging="True" 
                    AllowSorting="True" AutoGenerateColumns="False" 
                    UseAccessibleHeader="True">
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		        <HeaderStyle></HeaderStyle>
		        <Columns>
                    <asp:TemplateColumn HeaderText="Selez." Visible="false"> 
                        <ItemTemplate>
	                        <asp:ImageButton ID="IdImgSelSedi"  style="cursor:pointer;" CommandName="Select" alt="Seleziona Sede" ToolTip='Seleziona Sede' runat="server" ImageURL="images/sedi_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                    </asp:TemplateColumn>
					<asp:BoundColumn DataField="Stato" HeaderText="Stato"></asp:BoundColumn>
					<asp:BoundColumn DataField="Sede" HeaderText="Ente Sede"></asp:BoundColumn>
					<asp:BoundColumn DataField="Ente" HeaderText="Ente"></asp:BoundColumn>
					<asp:BoundColumn DataField="Tiposede" Visible="false" HeaderText="Tipologia"></asp:BoundColumn>
					<asp:BoundColumn DataField="NSedi" HeaderText="Cod.Sede Attuaz.">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="Codicesede" HeaderText="Cod"></asp:BoundColumn>
					<asp:BoundColumn DataField="Indirizzo" HeaderText="Indirizzo"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="Cap" HeaderText="CAP"></asp:BoundColumn>
					<asp:BoundColumn DataField="Comune" HeaderText="Comune"></asp:BoundColumn>
					<asp:BoundColumn DataField="Telefono" HeaderText="Telefono"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="Email" HeaderText="Email"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="idEnteSede" HeaderText="idSede"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="idente" HeaderText="idEnte"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="acquisita" HeaderText="idass"></asp:BoundColumn>
					<asp:BoundColumn DataField="DataCreazioneRecord" HeaderText="Data Inserimento" DataFormatString="{0:d}">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Certificazione" Visible="false" HeaderText="Presenza Iscrizione">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Segnalazione" Visible="false" HeaderText="Presenza Sanzione">
						<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="verifica" Visible="false" HeaderText="Presenza Verifica">
						<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundColumn>
                    <asp:BoundColumn DataField="Palazzina" HeaderText="Palazzina"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Scala" HeaderText="Scala"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Piano" HeaderText="Piano"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Interno" HeaderText="Interno"></asp:BoundColumn>
				</Columns>
		    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
	    </asp:datagrid>
    </div>
    </form>
</body>
</html>
