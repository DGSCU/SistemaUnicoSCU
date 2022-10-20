<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmAssicurazioneVolontarioCronoGG.aspx.vb" Inherits="Futuro.WfrmAssicurazioneVolontarioCronoGG" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
<legend >Cronologia Documenti Volontari Assicurati</legend>
<asp:datagrid  id="dtgRisultatoRicerca" runat="server" Width="100%" CssClass="table"
								AutoGenerateColumns="False" 
								 AllowPaging="True">
								<FooterStyle ></FooterStyle>
								<SelectedItemStyle BackColor="White"></SelectedItemStyle>
								<EditItemStyle ></EditItemStyle>
								<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
								<ItemStyle CssClass="tr"></ItemStyle>
								<HeaderStyle ></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="DataCreazione" HeaderText="Data Creazione Documento">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
										<FooterStyle Wrap="False"></FooterStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="DataAvvio" HeaderText="Data Avvio Volontari">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
										<FooterStyle Wrap="False"></FooterStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="NomeUtente" HeaderText="Utente">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
										<FooterStyle Wrap="False"></FooterStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="TipoDocumento" HeaderText="Tipologia Documento">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
										<FooterStyle Wrap="False"></FooterStyle>
									</asp:BoundColumn>
									<asp:ButtonColumn Text="...." HeaderText="Visualizza" CommandName="Documento">
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:ButtonColumn>
									<asp:BoundColumn DataField="IdDoc" Visible="False">
										<FooterStyle Wrap="False"></FooterStyle>
									</asp:BoundColumn>
								</Columns>
								<PagerStyle NextPageText="Successiva"  ForeColor="White"
									PrevPageText="Precedente" HorizontalAlign="Center" 
									Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
                             <div class="RigaPulsanti" style="float:right">
                                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
                            </div>
                            </fieldset>
</asp:Content>
