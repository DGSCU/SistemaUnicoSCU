<%@ Page Title="Sanzione Servizi" Language="vb" AutoEventWireup="false" CodeBehind="WfrmSanzioneServizi.aspx.vb" Inherits="Futuro.WfrmSanzioneServizi" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head runat="server">
     <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>Elenco Servizi</title>
</head>
<body>
    <form id="form1" runat="server">
        <fieldset class="ContornoPagina">
            <div class="wrapper" style="width:100%;border:0px">
                <div class="headers" >
                    <h2><asp:Label ID="lblTitolo" runat="server"  Text="Elenco Servizi"></asp:Label></h2>
                </div>
                <br />
               <asp:label id="LblInfo" Runat="server" CssClass="msgConferma" Visible="False"></asp:label>
               <div class="row">
                  <asp:datagrid id="dgElencoServizi" runat="server" Width="100%"
                        PageSize="5" CssClass="table" AllowSorting="True" AutoGenerateColumns="False"
                        UseAccessibleHeader="True">
                        <FooterStyle></FooterStyle>
                        <SelectedItemStyle BackColor="White" Font-Bold="true"></SelectedItemStyle>
                        <EditItemStyle></EditItemStyle>
                        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                        <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle></HeaderStyle>
		                <Columns>
                            <asp:TemplateColumn > 
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgProg" CommandName="Select" ToolTip="Servizi" AlternateText="Servizi" runat="server" ImageURL="images/Icona_Volontario_small.png" CausesValidation="false"></asp:ImageButton>                         
                                </ItemTemplate>
                            </asp:TemplateColumn>
						    <asp:BoundColumn DataField="Descrizione" HeaderText="Servizio">
							     
						    </asp:BoundColumn>
						    <asp:BoundColumn Visible="False" DataField="idservizio"></asp:BoundColumn>


						    <asp:TemplateColumn>
																
								<ItemTemplate>
                                    <asp:CheckBox id="ChkServizi" toolTip="Servizi"  runat="server"  Text="&nbsp;"></asp:CheckBox>    
								</ItemTemplate>
							</asp:TemplateColumn>

							    
		                </Columns>
		                <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center"
                            CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
	                </asp:datagrid>
               </div>
               <br />
                <div class="RigaPulsanti" > 
                    <asp:Button ID="btSalva" CssClass="Pulsante" runat="server" Text="Salva" />
                    <asp:Button id="cmdEsci" CssClass="Pulsante" OnClientClick="javascript:window.close();" runat="server" Text="Chiudi" ></asp:Button>&nbsp;
                </div>
            </div>
       
        </fieldset>
    </form>
</body>
</html>
