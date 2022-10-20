<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmAbilitaEntiCorsoOLP.aspx.vb" Inherits="Futuro.WfrmAbilitaEntiCorsoOLP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
<legend >Abilitazione Enti</legend>
<br />
<h3>
<asp:label id="lblmessaggio" runat="server" Text="" AssociatedControlID="dgRisultatoRicerca"></asp:label>
</h3>
<br />
 <div class="RigaPulsanti" style="text-align:right">

                                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />

</div>
<asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%"  ToolTip="Risultato Ricerca Abilitazione Enti" Caption="Elenco di Abilitazione e Disabilitazione Enti per corsi OLP"   
          AllowPaging="True" CellPadding="2"  CssClass="table" 
        AutoGenerateColumns="False" UseAccessibleHeader="True" PageSize="1000">
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
										<ItemStyle ></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="tipologia" ItemStyle-HorizontalAlign="Center"  HeaderText="Tipologia">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle ></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Idclasseaccreditamento"  ItemStyle-HorizontalAlign="Center" HeaderText="Classe">
										<HeaderStyle  ></HeaderStyle>
										<ItemStyle ></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CodiceFiscale" HeaderText="CodiceFiscale">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle ></ItemStyle>
									</asp:BoundColumn>
									
									
							<asp:TemplateColumn HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderText="Abilita Disabilita"> 
                             <ItemTemplate >
                                <asp:ImageButton ID="CmdIncludiSi" ToolTip="Disabilita" AlternateText="Disabilita"  CommandName="Disabilita" ImageUrl="~/Images/selezionato_small.png"  Visible='<%# IIF(Eval("AbilitazioneCorsiOLP").ToString().Equals("True"), True, False) %>' runat="server" />
                                <asp:ImageButton ID="CmdEscludiNo" ToolTip="Abilita" AlternateText="Abilita" CommandName="Abilita" ImageUrl="~/Images/deselezionato_small.png" Visible='<%# IIF(Eval("AbilitazioneCorsiOLP").ToString().Equals("False"), True, False) %>' runat="server" />  
                            </ItemTemplate>
                            </asp:TemplateColumn>
                           
                                </Columns>
								<PagerStyle NextPageText="&gt;&gt;"   
									PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
<p>
    &nbsp;</p>

</fieldset>

</asp:Content>
