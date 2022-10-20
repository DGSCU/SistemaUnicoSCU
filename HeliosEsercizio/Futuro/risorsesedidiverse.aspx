<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="risorsesedidiverse.aspx.vb" Inherits="Futuro.risorsesedidiverse" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="Styles/Site.css" rel="stylesheet" type="text/css" />
<%--<script event="onclick" for="imgStampa">						
			StampaPagina()
			return false;			
		</script>
		<script event="onclick" for="imgStampaRuoli">						
			StampaPagina(0)
			return false;			
		</script>--%>
		
    <title>OLP</title>
</head>
<body>
    <form id="form1" runat="server">
    <p>
    <asp:label id="lblmessaggio" runat="server" Height="8px" Font-Names="Verdana" Font-Bold="True"
											Font-Size="XX-Small" ForeColor="Red"></asp:label></p>
    <div class="wrapper" style="width:100%">
   
								
							
								
								<div class="tr">
									<asp:datagrid id="dgRisultatoRicerca" CssClass="table" Width="100%" Caption="Elenco OLP su Sedi diverse" runat="server"  CellPadding="0" 
											AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="true"  PageSize="5" AllowPaging="True">
											<FooterStyle ></FooterStyle>
											<SelectedItemStyle ></SelectedItemStyle>
											<EditItemStyle ></EditItemStyle>
											<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
											<ItemStyle CssClass="tr"></ItemStyle>
											<HeaderStyle ></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="IDEntePersonaleRuolo">
													<HeaderStyle HorizontalAlign="Center"  VerticalAlign="Middle"></HeaderStyle>
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="cognome" HeaderText="Cognome">
													<HeaderStyle HorizontalAlign="Center"  VerticalAlign="Middle"></HeaderStyle>
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="nome" HeaderText="Nome">
													<HeaderStyle ></HeaderStyle>
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="codicefiscale" HeaderText="Codice Fiscale">
													<HeaderStyle HorizontalAlign="Center"  VerticalAlign="Middle"></HeaderStyle>
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="nsedi" HeaderText="N&#176; Sedi">
													<HeaderStyle HorizontalAlign="Center"  VerticalAlign="Middle"></HeaderStyle>
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
												</asp:BoundColumn>
												<asp:ButtonColumn HeaderText="Selez" Text="&lt;img src=images/Icona_Volontario_small.png border=0&gt;" CommandName="Select">
													<HeaderStyle ></HeaderStyle>
												</asp:ButtonColumn>
											</Columns>
											<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
										</asp:datagrid>
										
								</div>
                               
                      <div class="RigaVuotaPrint" >
       <br/>
       
         <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" 
            Text="Esporta CSV" Visible="false" 
              ToolTip="Esporta elenco risultato ricerca" />
            <br />
            <asp:HyperLink ID="ApriCSV1"   AccessKey="S" ToolTip="Link per la Stampa OLP su sedi diverse" CssClass="linkStampa" 
                Text="DOWNLOAD CSV" runat="server" 
                ForeColor="#003399" Visible="False"></asp:HyperLink>
         </div>
                                
                                
                            
								
								<div class="tr">
									<asp:datagrid CssClass="table" Caption="Elenco ruoli incompatibili" id="DtgIncompatibilitaRuoli" runat="server"  CellPadding="0" Width="100%"
											AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="true" PageSize="5" AllowPaging="True">
											<FooterStyle ></FooterStyle>
											<SelectedItemStyle   ></SelectedItemStyle>
											<EditItemStyle  ></EditItemStyle>
											<AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
											<ItemStyle CssClass="tr"></ItemStyle>
											<HeaderStyle></HeaderStyle>
											<Columns>
												<asp:BoundColumn DataField="cognome" HeaderText="Cognome">
													<HeaderStyle HorizontalAlign="Center"  VerticalAlign="Middle"></HeaderStyle>
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="nome" HeaderText="Nome">
													<HeaderStyle ></HeaderStyle>
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="codicefiscale" HeaderText="Codice Fiscale">
													<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="nruoli" HeaderText="N&#176; Ruoli">
													<HeaderStyle HorizontalAlign="Center"  VerticalAlign="Middle"></HeaderStyle>
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
												</asp:BoundColumn>
												<asp:ButtonColumn HeaderText="Selez" Text="&lt;img src=images/dettaglioolp.jpg border=0&gt;" CommandName="Select">
													<HeaderStyle ></HeaderStyle>
												</asp:ButtonColumn>
												<asp:BoundColumn Visible="False" DataField="identepersonale"></asp:BoundColumn>
											</Columns>
											<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
										</asp:datagrid>
								</div>

								
							
					<div class="RigaVuotaPrint" >
       <br/>
       
         <asp:Button ID="CmdEsportaCSV" runat="server" CssClass="Pulsante" 
            Text="Esporta CSV" Visible="false" 
              ToolTip="Esporta elenco risultato ricerca" /><asp:button id="cmdChiudi" text="Chiudi" CssClass="Pulsante" runat="server" 
											ToolTip="Chiudi" />
            <br />
            <asp:HyperLink ID="ApriCSV2"   AccessKey="S" ToolTip="Link per la Stampa OLP su sedi diverse" CssClass="linkStampa" 
                Text="DOWNLOAD CSV" runat="server" 
                ForeColor="#003399" Visible="False"></asp:HyperLink>
         </div>
			<div class="RigaPulsanti">
									
								</div>
    </div>
    </form>
</body>
</html>
