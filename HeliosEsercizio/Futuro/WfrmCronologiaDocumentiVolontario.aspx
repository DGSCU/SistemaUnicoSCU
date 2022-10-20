<%@ Page Title="Cronologia Documenti Volontario" Language="vb" AutoEventWireup="false" CodeBehind="WfrmCronologiaDocumentiVolontario.aspx.vb" Inherits="Futuro.WfrmCronologiaDocumentiVolontario" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head id="Head1" runat="server">
    <title>Cronologia Documenti Volontario</title>
    		 <link href="Styles/Site.css" rel="stylesheet"  type="text/css" media="screen" />
        <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
	    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
	    <meta name="vs_defaultClientScript" content="JavaScript"/>
	    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
</head>
<body>
	<form id="Form1" method="post" runat="server">
    <fieldset class="ContornoPagina">
    <legend>Cronologia Documenti</legend>
    <div class="wrapper" style="width:100%;border:0px">
     <div class="row" style="height:auto">
		    <asp:label id="lblConferma" runat="server" CssClass="msgConferma"></asp:label>
		</div>
    <div class="RigaVuota" style="height:auto">&nbsp;
    </div>
    <h3>
        <asp:label id="lblNominativo"  runat="server" AssociatedControlID="dtgRisultatoRicerca" CssClass="msgInfo"  Text="Cronologia Documenti: "></asp:label>
    </h3>
    <asp:datagrid id="dtgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco Volontari" 
		    AllowPaging="true" CellPadding="2"  CssClass="table" PageSize="20" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr" HorizontalAlign="Center" Font-Size="Small" ></ItemStyle>
	    <HeaderStyle></HeaderStyle>
		    <Columns>
			    <asp:BoundColumn Visible="False" DataField="IdCronologiaEntit&#224;Documento" HeaderText="IdCronologiaEntitaDocumento"></asp:BoundColumn>
			    <asp:BoundColumn DataField="DataDocumento" HeaderText="Data Creazione Documento">
			    </asp:BoundColumn>
			    <asp:BoundColumn DataField="Username" HeaderText="Utente">
			    </asp:BoundColumn>
			    <asp:BoundColumn DataField="Documento" HeaderText="Tipologia Documento">
			    </asp:BoundColumn>
                 <asp:TemplateColumn HeaderText="Rimuovi" > 
                    <ItemTemplate  >
						<asp:ImageButton ID="ImgCronologia"   style="cursor:pointer;"   CommandName="Rimuovi" AlternateText="Rimuovi Documento" ToolTip="Rimuovi Documento" runat="server" ImageURL="images/canc_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                </asp:TemplateColumn>
		    </Columns>
        
			<PagerStyle NextPageText="&gt;&gt;"	PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>					
    </asp:datagrid>
        <div class="RigaVuota">&nbsp;</div>
      <div class="RigaPulsanti">
        <asp:Button ID="imgChiudi"  runat="server" OnClientClick="javascript: window.close()" CssClass="Pulsante" Text="Chiudi"  />&nbsp;
    </div> 
        </div>
    </fieldset>
    </form>
</body>
</html>
