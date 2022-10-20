<%@ Page Title=">Associazione Multipla Protocollo per Volontario" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmAssociazioneMultiplaProtocolloVolontari.aspx.vb" Inherits="Futuro.WfrmAssociazioneMultiplaProtocolloVolontari" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
<legend >Associazione Protocollo a Volontari</legend>

<div class="wrapper" style="width:100%">

    <div>
   <asp:label id="lblmsg" runat="server" Height="8px" Width="594px" Font-Bold="True" Font-Names="Verdana"
							Font-Size="X-Small" ForeColor="Red"></asp:label>
    </div>
    <div class="headers" >
       <h2>
       
      <asp:Label ID="Label5" runat="server"  Text="Associazione Protocollo a Volontari"></asp:Label>
      </h2>
   </div>
       <div class="RigaVuota">
      
      &nbsp;</div>
       <div class="row" >
    
       <div class="collable" style="width:45%" >
         <asp:Label id="Label1" runat="server"  AssociatedControlID="lblCodProg">Codice Progetto:</asp:Label>&nbsp;<asp:Label id="lblCodProg" AssociatedControlID="Label1" CssClass="bold" runat="server" ></asp:Label>
																
       </div>
       
       <div class="collable" style="width:45%">
           
           <asp:Label id="Label2" runat="server" AssociatedControlID="lblTitoloprog" >Titolo Progetto:</asp:Label>&nbsp;<asp:Label id="lblTitoloprog" AssociatedControlID="Label2" CssClass="bold" runat="server" ></asp:Label>
         
       </div>
       
    
      </div>
       <div class="row" >
            <div class="colOggetti" style="width:140px">
         
           &nbsp;  <asp:Label ID="lbldomanda" runat="server" Width="150px" AssociatedControlID="ddlFiltro" Text="Quali 
															Volontari si vuole caricare?"></asp:Label>
            </div>
            <div class="colOggetti" style="width:140px">
         
           &nbsp;
        </div>
            <div class="colOggetti" style="width:140px">
         
           &nbsp;<asp:label id="LblNumProtocollo" AssociatedControlID="TxtNumProtocollo" runat="server">N° Protocollo</asp:label>
        </div>
            <div class="colOggetti" style="width:140px">
         
           &nbsp;<asp:textbox id="TxtNumProtocollo" runat="server" Width="80px" ReadOnly="False" Enabled="True"
																MaxLength="9"></asp:textbox>
         </div>
       </div>
     
       <div class="row" >
    
        <div class="collable" style="width:280px">
          
        &nbsp;<asp:dropdownlist id="ddlFiltro" runat="server" Width="256px" AutoPostBack="True">
														<asp:ListItem Value="1">Graduatoria</asp:ListItem>
														<asp:ListItem Value="2">Progetto</asp:ListItem>
													</asp:dropdownlist>
        
           </div>
        <div class="collable" style="width:140px">
         
      &nbsp;<asp:label id="LblAnno" runat="server" AssociatedControlID="TxtAnno">Anno</asp:label></div>
        <div class="colOggetti" style="width:140px">
         
           &nbsp;<asp:textbox id="TxtAnno" runat="server" Width="80px" ReadOnly="False" MaxLength="4"></asp:textbox></div>
        <div class="colOggetti" style="width:70px">

                   <asp:Button
          ID="cmdApplica" runat="server" CssClass="Pulsante" Text="Applica" />
           </div>
  
    </div>
    
       <div class="row" >
    
       <div class="collable" style="width:100%;margin-right:2px">
           
           &nbsp;<asp:checkbox id="chkSelDesel"  runat="server" Font-Bold="True" 
								  TextAlign="Left" Text="Seleziona tutto" AutoPostBack="True"></asp:checkbox>
          
       </div>
       
    </div>
   
   
       <asp:datagrid id="dtgVolontari" runat="server" Width="100%" BorderWidth="2px" BorderStyle="Ridge" CssClass="table"
								 ToolTip="Elenco volontari graduatoria"  CellPadding="2"
								AllowSorting="True" AutoGenerateColumns="False"  PageSize="5">
								<FooterStyle ></FooterStyle>
								<SelectedItemStyle BackColor="White"></SelectedItemStyle>
								<EditItemStyle ></EditItemStyle>
								<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
								<ItemStyle CssClass="tr"></ItemStyle>
								<HeaderStyle ></HeaderStyle>
								<Columns>
									<asp:ButtonColumn Visible="False" Text="&lt;img src=images/pers.jpg Width=20 Height=20 title='Volontario' border=0&gt;">
										<HeaderStyle Width="0%"></HeaderStyle>
									</asp:ButtonColumn>
									<asp:BoundColumn Visible="False" DataField="IdEntit&#224;" HeaderText="IdEntit&#224;">
										<HeaderStyle ></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Codicevolontario" HeaderText="Codice Volontario">
										<HeaderStyle ></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Cognome" HeaderText="Cognome">
										<HeaderStyle ></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Nome" HeaderText="Nome">
										<HeaderStyle ></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="StatoVolontario" HeaderText="Stato">
										<HeaderStyle ></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CodiceFascicolo" HeaderText="Fasciolo">
										<HeaderStyle ></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="idfascicolo">
										<HeaderStyle ></HeaderStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn>
										<HeaderStyle ></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
										<ItemTemplate>
											<asp:CheckBox id="chkSelVol" Text="Seleziona" runat="server"  AutoPostBack="false" ToolTip="Seleziona Tipo Documento"></asp:CheckBox>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
								<PagerStyle NextPageText="&gt;&gt;" Font-Size="XX-Small" 
									PrevPageText="&lt;&lt;" HorizontalAlign="Center" Mode="NumericPages"></PagerStyle>
							</asp:datagrid>

                 
  
    <div class="RigaPulsanti">
        &nbsp;<asp:Button 
            ID="CmdChiudi" CssClass="Pulsante" runat="server"
              Text="Chiudi"  />

   </div>
    
   
</div>
</fieldset>
</asp:Content>
