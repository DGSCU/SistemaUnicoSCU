<%@ Page Title="Assicurazione Volontari" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmAssicurazioneVolontari.aspx.vb" Inherits="Futuro.WfrmAssicurazioneVolontari" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
<legend >Assicurazione</legend>
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:Label ID="Label5" runat="server"  Text="Assicurazione Volontari"></asp:Label>
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
                                 <asp:BoundColumn  DataField="Gruppo" HeaderText="Regione" >
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
   
   <asp:panel id="Panel1" runat="server" Visible="false" Width="100%" >



                     <%--<fieldset class="ContornoPagina">
<legend >Volontari Assicurati</legend>
<div class="wrapper" style="width:100%" >
    <div class="headers" >
       <h2>
      <asp:Label ID="Label6" runat="server"  Text="RICERCA ENTE"></asp:Label>
      </h2>

 </div>
    <div class="RigaVuota">
      <asp:label id="lblErr" runat="server" ForeColor="#ff0066"></asp:label>
      &nbsp;</div>
      
    <div class="row" >
    
        <div class="collable" style="width:50%">
          
        &nbsp;
        
            <asp:Label ID="lblAss" runat="server" AssociatedControlID="Label1" Text="Assicurati Italia" ></asp:Label>
            <asp:Label ID="Label1" runat="server" ForeColor="#ff0066"></asp:Label>
        
        </div>
        <div class="colOggetti" style="width:50%">
         
       
     &nbsp;
       
            <asp:HyperLink ID="hlAssIt" runat="server" Text="Download Italia"  
                Target="_blank" Visible="false">Download Italia</asp:HyperLink>
       
        </div>
        
        
    </div>
    <div class="row" >
    
       <div class="collable" style="width:50%">
         &nbsp;<asp:Label ID="lblEst" AssociatedControlID="Label2" runat="server" Text="Assicurati Estero" 
               Width="120px"></asp:Label>
           <asp:Label ID="Label2" runat="server" ForeColor="#ff0066"></asp:Label>
         
       </div>
       <div class="colOggetti" style="width:50%">
          
     &nbsp;
        
           <asp:HyperLink ID="hlAssEst" runat="server" Text="Download Estero"  
               Target="_blank" Visible="false">Download Estero</asp:HyperLink>
        
       </div>
       
    
    </div>
    <div class="row" >
    
       <div class="collable" style="width:50%">
           
           &nbsp;<asp:Label ID="lblsost" AssociatedControlID="Label3" runat="server" Text="Sostituiti." Width="48px"></asp:Label>
&nbsp;<asp:Label ID="Label3" runat="server" ForeColor="#ff0066"></asp:Label>
          
       </div>
       <div class="colOggetti" style="width:50%">
          
          &nbsp;
       
           
           <asp:HyperLink ID="hlSost" runat="server" Text="Download Sostituiti" 
               Target="_blank" Visible="false">Download Sostituiti</asp:HyperLink>
       
           
       </div>
      
      
    </div>
    <div class="row" >
   
        <div class="collable" style="width:50%">
           
           &nbsp;<asp:Label ID="lblDep" AssociatedControlID="Label4" runat="server" Text="Download Depennati."></asp:Label>
&nbsp;<asp:Label ID="Label4" runat="server" ForeColor="#ff0066"></asp:Label>
         
       </div>
        <div class="colOggetti" style="width:50%">
          
     &nbsp;
         
            <asp:HyperLink ID="hldep" runat="server" Text="Dowenload Depennati"  
                Target="_blank" Visible="false">Download Depennati</asp:HyperLink>
         
       </div>
       
     
    </div>
  
    
    
   
</div>
</fieldset>--%>



				</asp:panel>
    </div>

        <asp:Panel ID="PanelDocs" runat="server" Height="48px" HorizontalAlign="Left" 
            Visible="True" Width="100%">
            <asp:LinkButton ID="hlCrono" runat="server">
						<strong>Visualizza Storico...</strong></asp:LinkButton>
        </asp:Panel>
					
    </fieldset>

</asp:Content>
