<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaRequisitiVerifiche.aspx.vb" Inherits="Futuro.WfrmRicercaRequisitiVerifiche" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
    function Salva() {
			if (document.all.MainContent_txtDescrizione.value=="") 
			{
				alert("Inserire la descrizione della versione.")
				//document.Form1.txtDescrizione.focus();
				return false;
			}
            }
		</script>
		<script type="text/javascript" language="javascript">
		    function Chiudi() {
		        if (document.all.MainContent_txtDescrizione.value == "") {
		            alert("Inserire la descrizione della versione.")
		            //document.Form1.txtDescrizione.focus();
		            return false;
		        }
		    }
		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <fieldset id="requisito" class="ContornoPagina" runat="server">
        <legend>Dettaglio Versione Requisiti</legend>
        <asp:label id="lblmsgVersioneErrore" AssociatedControlID="requisito" Visible="false" CssClass="msgErrore" runat="server"></asp:label>
        <asp:label id="lblmsgVersione" AssociatedControlID="requisito" Visible="false" CssClass="msgConferma" runat="server"></asp:label>
        <div class="wrapper" style="width: 100%">
            <div class="headers">
                <h2>
                    <asp:Label ID="lblTitoloPagina" runat="server" Text="Dettaglio Versione Requisiti"></asp:Label>
                </h2>
            </div>
            <div class="RigaVuota">
                &nbsp;
            </div>
            <div class="row" >
                <div class="colOggetti" style="width:10%">
								        <asp:label id="LblDescrizione" AssociatedControlID="txtDescrizione" runat="server">Esito</asp:label>
            
                </div>
                <div class="colOggetti" style="width:85%" >
                    <asp:textbox id="txtDescrizione" Runat="server" Width="100%" MaxLength="50"></asp:textbox>
                </div>
            </div>
             <div class="row" >
                        <div class="colOggetti" style="width:10%">
                        <asp:label id="LblNote" AssociatedControlID="txtNote" runat="server">Note</asp:label>
                        </div>
                        <div class="colOggetti" style="width:85%">
                        <asp:textbox id="txtNote" Width="100%" Runat="server" MaxLength="500" TextMode="MultiLine"></asp:textbox>
                        </div>
              </div>
               <div class="RigaPulsanti"> 
             <asp:Button ID="cmdSalva" CssClass="Pulsante" Text="Salva" OnClientClick="return Salva()" runat="server"/>
        </div>
         </div>
   </fieldset>

   <fieldset class="ContornoPagina">
   <legend>Elenco requisti</legend>
    <asp:label id="lblmsgErrore" AssociatedControlID="requisito" Visible="false" CssClass="msgErrore" runat="server"/>
     <asp:label id="lblmsgRequisiti" AssociatedControlID="requisito" Visible="false" CssClass="msgConferma" runat="server"/>
       <div class="wrapper" style="width: 100%">
            <div class="headers">
                <h2>
                    <asp:Label ID="Label1" runat="server" Text="Elenco requisti"></asp:Label>
                </h2>
            </div>
            <div class="RigaVuota">
                &nbsp;
            </div>
            <div class="row" >
   <asp:datagrid id="dgElencoRequisiti" runat="server"  ToolTip="Elenco requisiti" 
											AutoGenerateColumns="False" CssClass="table" Width="100%"  HorizontalAlign="Center"  AllowPaging="True">
											<FooterStyle ></FooterStyle>
											<SelectedItemStyle  ></SelectedItemStyle>
											<EditItemStyle ></EditItemStyle>
											<AlternatingItemStyle CssClass="tr" ></AlternatingItemStyle>
											<ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
											<HeaderStyle  HorizontalAlign="Center"></HeaderStyle>
											<Columns>
                                              <asp:TemplateColumn  HeaderText="Selez.">
									            <ItemTemplate>
                                        
                                         <asp:ImageButton ID="btnSeleziona" CommandName="Seleziona" runat="server" AlternateText="Seleziona Requisito" 
                                                            ImageUrl="~/Images/ripristina_small.png"  
                                                            ToolTip="Seleziona Requisito" />
                                                </ItemTemplate>
                                       
                                            </asp:TemplateColumn>
												
												<asp:BoundColumn Visible="False" DataField="idrequisito"></asp:BoundColumn>
												<asp:BoundColumn DataField="requisito" HeaderText="Requisito"></asp:BoundColumn>
												<asp:BoundColumn DataField="TIPOREQUISITO" HeaderText="Tipo Requisito"></asp:BoundColumn>
									           <asp:TemplateColumn  HeaderText="Canc.">
                                                <ItemTemplate>
                                        
                                                        <asp:ImageButton ID="btnCancella" CommandName="Cancella" runat="server" AlternateText="Cancella Requisito" 
                                                            ImageUrl="~/Images/canc_small.png"  
                                                            ToolTip="Cancella Requisito" />
                                                </ItemTemplate>
                                                </asp:TemplateColumn>
                                                
                                              
											</Columns>
											<PagerStyle NextPageText="&gt;&gt;" 
												 PrevPageText="&lt;&lt;" CssClass="linkPageGrid" HorizontalAlign="Center" 
												 Mode="NumericPages"></PagerStyle>
										</asp:datagrid>

                                       

                                        </div>
                                          <div class="RigaPulsanti"> 
                                        <asp:Button ID="cmdNuovo" CssClass="Pulsante" Text="Nuovo" runat="server" />
                                        <asp:Button ID="cmdRevisione" CssClass="Pulsante" Text="Revisiona" runat="server" />
                                        <asp:Button ID="cmdChiudi" CssClass="Pulsante" Text="Chiudi" OnClientClick="return Chiudi()" runat="server"  />
                                        </div>

                                       

                      </div>
                      
   </fieldset>
   
</asp:Content>
