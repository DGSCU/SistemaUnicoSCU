<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmAnomalieCreazioneFascicoli.aspx.vb" Inherits="Futuro.WfrmAnomalieCreazioneFascicoli" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>ANOMALIE CREAZIONE FASCICOLO</title>
</head>
<body>
    <form id="form1" runat="server">

     <fieldset class="ContornoPagina">
<legend >ANOMALIE CREAZIONE FASCICOLO</legend>
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:Label ID="Label5" runat="server"  Text="ANOMALIE CREAZIONE FASCICOLO"></asp:Label>
      </h2>
 </div>
    <div class="RigaVuota">
      
      &nbsp;</div>
       <div class="row" >
     
        <p style="margin-left:0px">
        
             <asp:Label id="lblElenco"  runat="server" CssClass="bold"></asp:Label>
        </p>
      
    </div>
   
  
    <div class="RigaPulsanti">
        <asp:Button 
            ID="CmdChiudi" CssClass="Pulsante" runat="server"
              Text="Chiudi"  />

   </div>
    
   
</div>
</fieldset>


  
    </form>
</body>
</html>
