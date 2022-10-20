<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmAssicurazioneVolontariGG.aspx.vb" Inherits="Futuro.WfrmAssicurazioneVolontariGG" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
<legend >Assicurazione</legend>
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:Label ID="Label5" runat="server"  Text="Assicurazione Volontari Garanzia Giovani"></asp:Label>
      </h2>
 </div>
    <div class="RigaVuota">
      
      &nbsp;</div>
    <div class="row" >
     
       
        
            
      
       <div class="collable" style="Width:33%">
          
        <asp:Label ID="lblDataavvioVol" runat="server"  AssociatedControlID="ddlDataAvvio" Text="Data Avvio Volontari:"></asp:Label>
        </div>
       <div class="colOggetti" style="Width:33%">
          
        <asp:dropdownlist id="ddlDataAvvio" runat="server" 
               AutoPostBack="True"></asp:dropdownlist>
        </div>
       <div class="collable" style="Width:33%">
          
       <asp:Button ID="imgGenera" CssClass="Pulsante" runat="server" Text="Genera" />
        </div>
    </div>
    <asp:datagrid id="GridViewGG" runat="server" Width="100%"    
           CellPadding="2"  CssClass="table" 
        AutoGenerateColumns="False" UseAccessibleHeader="True">
								<FooterStyle></FooterStyle>
								<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
								<EditItemStyle></EditItemStyle>
								<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
								<ItemStyle CssClass="tr"></ItemStyle>
								<HeaderStyle></HeaderStyle>
								<Columns>
                                 <asp:BoundColumn  DataField="regione" HeaderText="Regione" >
										<HeaderStyle ></HeaderStyle>
										<ItemStyle  ></ItemStyle>
									</asp:BoundColumn>
                                     <asp:BoundColumn  DataField="tipo" HeaderText="Tipo" >
										<HeaderStyle ></HeaderStyle>
										<ItemStyle  ></ItemStyle>
									</asp:BoundColumn>
                                    <asp:TemplateColumn  HeaderText="Download" ItemStyle-HorizontalAlign="Center">
                               
									<ItemTemplate >
                                        
                                         <asp:HyperLink ID="btnSeleziona"   runat="server" Target="_blank" Text="Scarica" NavigateUrl='<%# Eval("Download")%>'/>
                                        </ItemTemplate>
                                       
                                        </asp:TemplateColumn>


                         
								</Columns>
								
							</asp:datagrid>
   

   </div>
   <asp:Panel ID="PanelDocs" runat="server" Height="48px" HorizontalAlign="Left" 
            Visible="True" Width="100%">
            <asp:LinkButton ID="hlCrono" runat="server">
						<strong>Visualizza Storico...</strong></asp:LinkButton>
        </asp:Panel>
   </fieldset>
</asp:Content>
