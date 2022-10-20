<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmVerGestioneBandiRequisiti.aspx.vb" Inherits="Futuro.WfrmVerGestioneBandiRequisiti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <fieldset id="Associa" class="ContornoPagina" runat="server">
        <legend>Associa Bando/Versione</legend>
       
        <asp:label id="lblmessaggio" CssClass="msgErrore"  runat="server"></asp:label>
        <div class="wrapper" style="width: 100%">
            <div class="headers">
                <h2>
                    <asp:Label ID="lblTitoloPagina"  runat="server" Text="Associa Bando/Versione"></asp:Label>
                </h2>
            </div>
            <div class="RigaVuota">
                &nbsp;
            </div>
            <div class="row" >
             <div class="colOggetti" style="width:10%">
								        <asp:label id="lblVersione" AssociatedControlID="txtDescrizione" runat="server">Versione
                                        </asp:label>
            
                </div>
                <div class="colOggetti" style="width:85%" >
                    <asp:textbox id="txtDescrizione" Runat="server" Width="100%" MaxLength="50"></asp:textbox>
                </div>
            </div>
             <div class="RigaVuota">
                &nbsp;
            </div>
              <div class="row">
              <fieldset>
                  <div class="colOggetti" style="width:50%;">
                  <div class="headers">
                        <h2>
                           <asp:Label ID="Label1" Width="100%"   runat="server" Text="Elenco bandi disponibili"></asp:Label>
                        </h2>
                        <asp:datagrid id="dtgElencoBandiDisponibili" runat="server"
															 ToolTip="Elenco bando associati"  AllowSorting="True" CssClass="table" AutoGenerateColumns="False"
															 PageSize="20" UseAccessibleHeader="true">
															<FooterStyle></FooterStyle>
															<SelectedItemStyle></SelectedItemStyle>
															<EditItemStyle></EditItemStyle>
															<AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
															<ItemStyle  CssClass="tr" HorizontalAlign="Center"></ItemStyle>
															<HeaderStyle></HeaderStyle>
															<Columns>
																<asp:TemplateColumn HeaderText="Seleziona">
																	<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
																	<ItemStyle HorizontalAlign="Center"></ItemStyle>
																	<ItemTemplate >
																		<asp:CheckBox id="chkbando" toolTip="Seleziona bando" AutoPostBack="false" Font-Bold="true" Text=" Sel " runat="server"></asp:CheckBox>
																	</ItemTemplate>
																</asp:TemplateColumn>
																<asp:BoundColumn DataField="bandobreve" HeaderText="Bando"></asp:BoundColumn>
																<asp:BoundColumn Visible="false" DataField="gruppo" HeaderText="gruppo"></asp:BoundColumn>
																<asp:BoundColumn Visible="False" DataField="idbando" HeaderText="idbando"></asp:BoundColumn>
															</Columns>
															<PagerStyle NextPageText="&gt;&gt;" 
																PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
														</asp:datagrid>

                                                       
                        <div class="RigaPulsanti">
                                                            <asp:Button ID="cmdAssocia" CssClass="Pulsante" runat="server" Text="Associa bando" />
                                                        </div>
                     </div>
                  </div>
                  <div class="colOggetti" style="width:50%;">
                  <div class="headers">
                    <h2>
                        <asp:Label ID="Label2" Width="100%" runat="server"   Text="Elenco bandi associati"></asp:Label>
                    </h2>
                    <asp:datagrid  id="dtgElencoBandiAssociati" runat="server" 
															  ToolTip="Elenco bando associati" AllowSorting="True"
															AutoGenerateColumns="False" CssClass="table" UseAccessibleHeader="True"  PageSize="30">
															<FooterStyle ></FooterStyle>
															<SelectedItemStyle></SelectedItemStyle>
															<EditItemStyle></EditItemStyle>
															<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
															<ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
															<HeaderStyle></HeaderStyle>
															<Columns>
																<%--<asp:ButtonColumn  Text="&lt;img src=images/cartella_small.png Width=20 Height=20 title='Seleziona Requisito' border=0&gt;"
																	CommandName="Select">
																	<HeaderStyle Width="1%"></HeaderStyle>
																</asp:ButtonColumn>--%>
																<asp:BoundColumn DataField="bandobreve" HeaderText="Bando"></asp:BoundColumn>
																<asp:BoundColumn Visible="false" DataField="GRUPPO" HeaderText="GRUPPO"></asp:BoundColumn>
																<asp:BoundColumn Visible="False" DataField="idbando" HeaderText="idbando"></asp:BoundColumn>
															</Columns>
															<PagerStyle NextPageText="&gt;&gt;" 
																PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
														</asp:datagrid>

                  </div>
                  
                  </div>

                  </fieldset>
              </div>


                <div class="RigaPulsanti"> 
             <asp:Button ID="cmdSalva" CssClass="Pulsante" Text="Salva" runat="server" />
             <asp:Button ID="cmdChiudi" CssClass="Pulsante" Text="Chiudi" runat="server"  />
        </div>
            </div>
      </fieldset>


							
									
									
									
									
													
</asp:Content>
