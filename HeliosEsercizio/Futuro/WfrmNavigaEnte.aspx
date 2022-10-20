<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmNavigaEnte.aspx.vb" Inherits="Futuro.WfrmNavigaEnte" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="Styles/Site.css" rel="stylesheet" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
  <br />
<h3>
<asp:label id="lblmessaggio" runat="server" Text="" AssociatedControlID="dgRisultatoRicerca"></asp:label>
</h3>
<br />

<asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%"  ToolTip="Risultato Ricerca Enti"   
          AllowPaging="True" CellPadding="2"  CssClass="table" 
        AutoGenerateColumns="False" UseAccessibleHeader="True">
								<FooterStyle></FooterStyle>
								<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
								<EditItemStyle></EditItemStyle>
								<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
								<ItemStyle CssClass="tr"></ItemStyle>
								<HeaderStyle></HeaderStyle>
								<Columns>
                                <asp:BoundColumn Visible="False" DataField="idente" HeaderText="idEnte">
										<HeaderStyle ></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn  DataField="codiceregione" HeaderText="Cod.Ente" >
										<HeaderStyle ></HeaderStyle>
										<ItemStyle  ></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn  DataField="Denominazione" HeaderText="Denominazione">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle Width="200px"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Statoente" HeaderText="Stato"></asp:BoundColumn>
									<asp:BoundColumn DataField="tipologia" HeaderText="Tipologia">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle Width="100px"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ClasseRichiesta" HeaderText="Classe/Sezione Richiesta">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle Width="80px"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ClasseAttribuita" HeaderText="Classe/Sezione  Attribuita">
										<HeaderStyle  ></HeaderStyle>
										<ItemStyle Width="80px"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Numerototalesedi" Visible="false" HeaderText="Nr.Sedi Attuazione">
										<HeaderStyle  ></HeaderStyle>
										<ItemStyle Width="70px"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="http" HeaderText="http://">
										<HeaderStyle  ></HeaderStyle>
										<ItemStyle ></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CodiceFiscale" HeaderText="CodiceFiscale">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle ></ItemStyle>
									</asp:BoundColumn>
									
									<asp:BoundColumn Visible="False" DataField="datacontrollohttp" HeaderText="verHttp">
										<HeaderStyle ></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="datacontrolloemail" HeaderText="veremail">
										<HeaderStyle ></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Competenza" HeaderText="Competenza"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="EmailCertificata" HeaderText="PEC">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle ></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="idClasseAccreditamentoRichiesta" HeaderText="ClasseAccreditamentoRichiesta"></asp:BoundColumn>
								<asp:TemplateColumn  HeaderText="Numero Sedi Accreditate" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lblsediaccr"  CommandName="NumeroTotSedi" ToolTip="sedi" runat="server"><%# Eval("Numerototalesedi")%></asp:LinkButton>          
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Numero Enti Accoglienza" ItemStyle-HorizontalAlign="Center"> 
                                    <ItemTemplate >    
                                        <asp:LinkButton ID="lblEntiAccre" CommandName="NumeroEntiAccoglienza"  ToolTip="Enti Accoglienza" runat="server"><%# Eval("NumeroEntiAccoglienza")%></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Info" ItemStyle-HorizontalAlign="Center"> 
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lblInfo"  CommandName="Info" ToolTip="Info" runat="server">Info</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                
                                
                                </Columns>
								<PagerStyle NextPageText="&gt;&gt;"   
									PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
<p style="text-align:right">
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" OnClientClick="javascript: window.close();" runat="server" Text="Chiudi"  /></p>

    </div>
    </form>
</body>
</html>
