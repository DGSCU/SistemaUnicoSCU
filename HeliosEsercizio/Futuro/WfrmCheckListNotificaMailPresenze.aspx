<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmCheckListNotificaMailPresenze.aspx.vb" Inherits="Futuro.WfrmCheckListNotificaMailPresenze" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
   <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>INVIO NOTIFICA EMAIL</title>
</head>
<body>

    
 <form id="form1" runat="server">
 <div style="width:95%">
 <br />
     Destinatario: <strong><asp:Label ID="lblTo" runat="server" ></asp:Label></strong>
     <br />
     <br />
      Oggetto: <strong><asp:Label ID="lblOggetto" runat="server" ></asp:Label></strong>
     <br />
     <br />
     Corpo del Messaggio: <strong></strong>
     <br />
    </div>
    <div style="width:95%">
    <asp:TextBox ID="txtNote" autofocus="true" runat="server" Height="400px"  TextMode="MultiLine" 
        Width="100%"></asp:TextBox>
       </div>
       <div style="width:95%;text-align:right;"> 
        <asp:Button CssClass="Pulsante" ID="CmdInvioEmail"
        runat="server" Text="Invia" />
        <asp:Button CssClass="Pulsante" ID="CmdChiudi"
        runat="server" Text="Chiudi" />
       
</div>


    </form>
</body>
</html>
