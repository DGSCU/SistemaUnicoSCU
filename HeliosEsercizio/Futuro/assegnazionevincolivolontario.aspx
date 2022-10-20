<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="assegnazionevincolivolontario.aspx.vb" Inherits="Futuro.assegnazionevincolivolontario" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>ASSEGNAZIONE VINCOLI VOLONTARIO</title>

    <script language="javascript" type="text/javascript">
        function Chiudi() 
        {
        window.close()
        }
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <fieldset class="ContornoPagina">
<legend >Assegnazione Vincoli Volontario</legend>
<asp:label id="lblmessaggiosopra" runat="server" Visible="false" 
					 Font-Bold="True"></asp:label>
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:Label ID="Label5" runat="server"  Text="Gestione Vincoli Volontario"></asp:Label>
      </h2>
 </div>
    <div class="RigaVuota">
      
      &nbsp;</div>
       
    <div class="row" >
    
        <div class="collable" style="width:15%">
          
        &nbsp;
        
            <asp:Label ID="Label6"  runat="server" Text="Nominativo"></asp:Label>
        
        </div>
        <div class="colOggetti" style="width:35%">
         
       
     &nbsp;
       
        
            <asp:Label ID="lblNominativo" CssClass="bold" Runat="server"></asp:Label>
          
       
        </div>
        <div class="collable" style="width:15%">
         
      &nbsp;
       
            <asp:Label ID="Label7" runat="server" Text="Codice Fiscale"></asp:Label>
       
        </div>
        <div class="colOggetti" style="width:35%">
         
           &nbsp;
        
            <asp:Label ID="lblCodFis" CssClass="bold" Runat="server"></asp:Label>
            
        </div>
  
    </div>
    <div class="row" style="border-bottom-width:2px;border-bottom-color:#3a4f63;border-bottom-style:solid">
    
       <div class="collable" style="width:15%">
         &nbsp;
         
           <asp:Label ID="Label8" runat="server" Text="Data di Nascita"></asp:Label>
         
       </div>
       <div class="colOggetti" style="width:35%">
          
     &nbsp;
        
       
           <asp:Label ID="lblDataNascita" CssClass="bold" Runat="server"></asp:Label>
          
       </div>
       <div class="collable" style="width:15%">
           
           &nbsp;
         
           <asp:Label ID="Label9" runat="server" Text="Comune di Nascita"></asp:Label>
         
       </div>
       <div class="colOggetti" style="width:35%">
           
              &nbsp;
       
              <asp:Label ID="lblComuneNascita" CssClass="bold" Runat="server"></asp:Label>
              
       </div>
    
    </div>
    <br />
    <div >
    <asp:datagrid id="dtgVincoliVolontari" runat="server" Width="100%" 
            AutoGenerateColumns="False" CssClass="table"
								AllowSorting="True"  CellPadding="3"  
            BorderStyle="Solid" BorderWidth="1px" AllowPaging="True">
								<FooterStyle ></FooterStyle>
								<SelectedItemStyle BackColor="White"></SelectedItemStyle>
								<EditItemStyle ></EditItemStyle>
								<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
								<ItemStyle CssClass="tr"></ItemStyle>
								<HeaderStyle></HeaderStyle>
								<Columns>
									<asp:BoundColumn Visible="False" DataField="IdVincolo" HeaderText="IdVincolo"></asp:BoundColumn>
									<asp:BoundColumn DataField="Vincolo" HeaderText="Vincolo">
										<ItemStyle Wrap="False"></ItemStyle>
										<FooterStyle Wrap="False"></FooterStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn>
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle Wrap="False" Width="20%" ></ItemStyle>
										<HeaderTemplate >
											Esito
										</HeaderTemplate>
										<ItemTemplate>
                                            <asp:Label ID="lblEsito" AssociatedControlID="ddlEsito" runat="server" Text="Esito"></asp:Label>
											<asp:DropDownList ID="ddlEsito" Width="65%"  Runat="server">
												<asp:ListItem Value="1">Positivo</asp:ListItem>
												<asp:ListItem Value="0">Negativo</asp:ListItem>
											</asp:DropDownList>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn Visible="False" DataField="Valore" HeaderText="Valore"></asp:BoundColumn>

									<asp:TemplateColumn>
                                    <HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    <HeaderTemplate>
											Note
										</HeaderTemplate>
										<ItemTemplate>
                                            <asp:Label ID="lblNote" runat="server" AssociatedControlID="txtNote" Text="Nota"></asp:Label>
											<asp:TextBox id="txtNote" runat="server" Rows="2" TextMode="MultiLine" MaxLength="100"></asp:TextBox>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn Visible="False" DataField="ValoreNote" HeaderText="ValoreNote"></asp:BoundColumn>
								</Columns>
								<PagerStyle NextPageText="Successiva" Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True"
									PrevPageText="Precedente" HorizontalAlign="Center" ></PagerStyle>
							</asp:datagrid>
                          <br />

      
      
    </div>
    
  
    <div class="RigaPulsanti">
        <asp:Button
          ID="CmdSalva" runat="server" CssClass="Pulsante" Text="Salva" />&nbsp;
           <asp:Button ID="CmdChiudi" CssClass="Pulsante" OnClientClick="Chiudi()" runat="server"
              Text="Chiudi"  />

   </div>
    
   
</div>
</fieldset>
    </form>
</body>
</html>
