<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmAnomalieVolontari.aspx.vb" Inherits="Futuro.WfrmAnomalieVolontari" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" lang="it" xml:lang="it">

<head runat="server">
<link href="Styles/Site.css" rel="stylesheet" type="text/css" />
 <script language="javascript" type="text/javascript">
     function Chiudi() {
         window.close()
     }
    
    </script>
    <title>Visualizzazione Dettagli Volontario</title>
</head>
<body>
    <form id="form1" runat="server">
    <fieldset class="ContornoPagina">
<legend >Dettagli Volontario</legend>
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:Label ID="Label5" runat="server"  Text="Visualizzazione Dettagli 
													Volontario"></asp:Label>
      </h2>
 </div>
    <div class="RigaVuota">
      
      &nbsp;</div>
       <div class="row" >
     
       <div class="collable" >
        
            <asp:Label ID="lblVolontario" runat="server" Width="150px" AssociatedControlID="lblNominativo" Text="Volontario:"></asp:Label>
            
     
       </div>
         <div class="colOggetti" >
         &nbsp;
           <asp:Label ID="lblNominativo" CssClass="bold" runat="server" ></asp:Label>
     
       </div>
        <div class="collable" >
        
           
     
            <asp:Label ID="Label7" runat="server" Text="Verifica Lotus"></asp:Label>
       
     
       </div>
        <div class="colOggetti" >
         &nbsp;
           
     
            <asp:Label ID="lblAnomaliaLotus" CssClass="bold" runat="server" ></asp:Label>
        
     
       </div>
      </div>
     <div class="row" style="border-bottom-width:2px;border-bottom-color:#3a4f63;border-bottom-style:solid">
    
        <div class="collable">
          
   
        
            <asp:Label ID="lbldatanascita" runat="server" Width="150px" 
                AssociatedControlID="lbldatanascitaVal" Text="Data Nascita:"></asp:Label>
            
     
            </div>
        <div class="colOggetti" >
         
       
     &nbsp;
       
            <asp:Label ID="lbldatanascitaVal" CssClass="bold" runat="server" Text=""></asp:Label>
       
            </div>
        <div class="collable" >
         

       
            <asp:Label ID="lblDataPresentazione" runat="server" Width="150px" 
                AssociatedControlID="lblDataPresentazioneVal" Text="Data Presentazione Domanda:"></asp:Label>
            
     
            </div>
        <div class="colOggetti" >
         
           &nbsp;
                   
                   
                   
            <asp:Label ID="lblDataPresentazioneVal" CssClass="bold" runat="server" Text=""></asp:Label>
       
            </div>
  
    </div>
     <div class="row" style="border-bottom-width:2px;border-bottom-color:#3a4f63;border-bottom-style:solid">
    
        <div class="collable">
          

        
            <asp:Label ID="lblDataInizio" runat="server" Width="150px" 
                AssociatedControlID="lblDataInizioVal" Text="Data Inizio Domande Volontari:"></asp:Label>
            
     
            </div>
        <div class="colOggetti" >
         
       
     &nbsp;
       
            <asp:Label ID="lblDataInizioVal" CssClass="bold" runat="server" Text=""  ></asp:Label>
       
            </div>
        <div class="collable"  >
         
      
       
            <asp:Label ID="lbldatafine" runat="server" Width="150px" 
                AssociatedControlID="lbldatafineVal" Text="Data Fine Domande Volontari:" ></asp:Label>
            
     
            </div>
        <div class="colOggetti" >
         
       
                   
            <asp:Label ID="lbldatafineVal" CssClass="bold" runat="server" Text=""  ></asp:Label>
       
            </div>
  
    </div>
    <% If Session("Sistema") = "Futuro" Then%>
     <div class="row"  style="border-bottom-width:2px;border-bottom-color:#3a4f63;border-bottom-style:solid;visibility:hidden">
    
        <div class="collable" style="visibility:hidden">
          

        
            <asp:Label ID="lblDataRegistrazione" runat="server" Width="150px" 
                AssociatedControlID="lblDataRegistrazioneVal" Text="Data Registrazione Garanzia Giovani:"  Visible="false"></asp:Label>
            
     
            </div>
        <div class="colOggetti" style="visibility:hidden">
         
       
     &nbsp;
       
            <asp:Label ID="lblDataRegistrazioneVal" CssClass="bold" runat="server" Text=""  Visible="false"></asp:Label>
       
            </div>
        <div class="collable" style="visibility:hidden">
         
    
            <asp:Label ID="lblDataPresainCarico" runat="server" Width="150px" 
                AssociatedControlID="lblDataPresainCaricoVal" Text="Data Registrazione Portale ClicLavoro:"  Visible="false"></asp:Label>
            
     
            </div>
        <div class="colOggetti" style="visibility:hidden" >
         
           &nbsp;
                   
            <asp:Label ID="lblDataPresainCaricoVal" CssClass="bold" runat="server" Text=""  Visible="false"></asp:Label>
       
            </div>
  
    </div>
     <div class="row" style="border-bottom-width:2px;border-bottom-color:#3a4f63;border-bottom-style:solid;visibility:hidden">
      <div class="collable"  style="visibility:hidden">
         
    
            <asp:Label ID="Label1" runat="server" Width="150px" 
                AssociatedControlID="lblrequisitiVol" Text="Requisiti:" Visible="false"></asp:Label>
            
     
            </div>
        <div class="colOggetti" style="visibility:hidden">
         
           &nbsp;
                   
            <asp:Label ID="lblrequisitiVol" CssClass="bold" runat="server" Text="" Visible="false"></asp:Label>
       
            </div>
  
    </div>
  
    <%End If%>
      <% If Session("Sistema") = "Futuro" Then%>
      <asp:GridView id="dtgControlloAnomaliaVolontari" runat="server" Width="100%"  Caption="Controlli Garanzia Giovani"   ShowHeaderWhenEmpty="true" 
                        CellPadding="2" Font-Size="Small"  CssClass="table"
                        AllowSorting="false" AutoGenerateColumns="true" UseAccessibleHeader="True">
                <FooterStyle></FooterStyle>
                    <SelectedRowStyle Font-Bold="true" BackColor="White"></SelectedRowStyle>
                    <EditRowStyle></EditRowStyle>
                    <AlternatingRowStyle  CssClass="tr"></AlternatingRowStyle>
                    <RowStyle CssClass="tr"   HorizontalAlign="Center"  ></RowStyle>
                    <HeaderStyle></HeaderStyle>
                    <PagerStyle CssClass="linkPageGrid"  />
                <PagerSettings  Mode="NumericFirstLast" Visible="true" pagebuttoncount="100" position="Bottom"></PagerSettings >
            </asp:GridView>

   </div>
   
   <br />
       <asp:LinkButton ID="IdVedi" runat="server">Vedi Tutto</asp:LinkButton>
        <asp:LinkButton ID="IdVediUltimo" runat="server" Visible="false">Vedi Ultimo</asp:LinkButton>
    <br />
    <%End If%>
    <br />
    <div >
      <asp:datagrid id="dgVolGrad" runat="server" Width="100%" Height="16px"  Caption="Storico Domande"
            CssClass="table" AutoGenerateColumns="False"
											AllowSorting="True"  CellPadding="3" 
            ToolTip="Elenco Progetti" BorderWidth="1px" BorderStyle="Outset" 
            AllowPaging="True">
											<SelectedItemStyle BackColor="White"></SelectedItemStyle>
											<EditItemStyle ></EditItemStyle>
											<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
											<ItemStyle CssClass="tr"></ItemStyle>
											<HeaderStyle ></HeaderStyle>
											<FooterStyle></FooterStyle>
											<Columns>
												<asp:BoundColumn DataField="CodEnte" HeaderText="Cod. Ente "></asp:BoundColumn>
												<asp:BoundColumn DataField="denominazione" HeaderText="Ente"></asp:BoundColumn>
												<asp:BoundColumn DataField="titolo" HeaderText="Progetto"></asp:BoundColumn>
												<asp:BoundColumn DataField="codiceprogetto" HeaderText="Codice Progetto"></asp:BoundColumn>
												<asp:BoundColumn DataField="bando" HeaderText="Bando"></asp:BoundColumn>
												<asp:BoundColumn DataField="Datainziserv" HeaderText="Data Inizio Servizio" DataFormatString="{0:d}">
													<ItemStyle HorizontalAlign="Center"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="Statovol" HeaderText="Stato">
													<ItemStyle HorizontalAlign="Center"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="Idoneo" HeaderText="Idoneo">
													<ItemStyle HorizontalAlign="Center"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="Selez" HeaderText="Selezionato">
													<ItemStyle HorizontalAlign="Center"></ItemStyle>
												</asp:BoundColumn>
											</Columns>
											<PagerStyle NextPageText="Successiva" Font-Bold="True"
												PrevPageText="Precedente" HorizontalAlign="Center" 
												Mode="NumericPages"></PagerStyle>
										</asp:datagrid>


    </div>
   
    <br />
  
    <div class="RigaPulsanti">
        &nbsp;<asp:Button 
            ID="CmdChiudi" CssClass="Pulsante" OnClientClick="Chiudi()" runat="server"
              Text="Chiudi"  />

   </div>
    
   
</div>
</fieldset>
    </form>
</body>
</html>
