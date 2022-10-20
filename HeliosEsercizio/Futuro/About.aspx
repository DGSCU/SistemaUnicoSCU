<%@ Page Title="Dettagli sull'organizzazione" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false"
    CodeBehind="About.aspx.vb" Inherits="Futuro.About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <fieldset class="ContornoPagina">
<legend >XXXXXXXX</legend>
<div class="wrapper" style="width:700px">
    <div class="headers" >
       <h2>
      <asp:Label ID="Label5" runat="server"  Text="RICERCA ENTE"></asp:Label>
      </h2>
 </div>
    <div class="RigaVuota">
      
      &nbsp;</div>
       <div class="row" >
     
        <p style="margin-left:0px">
        
            <asp:Label ID="lblDenominazione" runat="server" Width="150px" AssociatedControlID="txtDenominazione" Text="DENOMINAZIONE"></asp:Label><asp:TextBox ID="txtDenominazione"
              Width="500px"  runat="server"></asp:TextBox>
        </p>
      
    </div>
    <div class="row" >
    
        <div class="collable">
          
        &nbsp;
        
        </div>
        <div class="colOggetti" >
         
       
     &nbsp;
       
        </div>
        <div class="collable" >
         
      &nbsp;
       
        </div>
        <div class="colOggetti" >
         
           &nbsp;
        </div>
  
    </div>
    <div class="row" >
    
       <div class="collable" >
         &nbsp;
         
       </div>
       <div class="colOggetti" >
          
     &nbsp;
        
       </div>
       <div class="collable" >
           
           &nbsp;
         
       </div>
       <div class="colOggetti" >
           
              &nbsp;
       </div>
    
    </div>
    <div class="row" >
    
       <div class="collable" >
           
           &nbsp;
          
       </div>
       <div class="colOggetti" >
          
          &nbsp;
       
           
       </div>
       <div class="collable" >
          
            &nbsp;
         
       </div>
       <div class="colOggetti" >
           
              &nbsp;
         
       </div>
      
    </div>
    <div class="row" >
   
        <div class="collable" >
           
           &nbsp;
         
       </div>
        <div class="colOggetti" >
          
     &nbsp;
         
       </div>
        <div class="collable" >
          
           &nbsp;
          
       </div>
        <div class="colOggetti" >
          
             &nbsp;
       </div>
     
    </div>
  
    <div class="RigaPulsanti">
        <asp:Button
          ID="CmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;<asp:Button 
            ID="CmdChiudi" CssClass="Pulsante" runat="server"
              Text="Chiudi"  />

   </div>
    
   
</div>
</fieldset>


</asp:Content>
