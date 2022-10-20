<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrmAnomaliaDati.aspx.vb" Inherits="Futuro.wfrmAnomaliaDati" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
<link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>ANOMALIA DATI</title>
</head>
<body>
    <form id="form1" runat="server">
                <div style="width:100%; height: 126px" class="wrapper" >
					
						<div style="width:100%;">
                        <asp:image id="Image1"  runat="server" ImageUrl="~/Images/UNICO_BANNER.jpg"></asp:image>
                        </div>
				
				
				
					
						<div style="text-align:center;BORDER-LEFT: #3a4f63 thin ridge; BORDER-TOP: #3a4f63 thin ridge; BORDER-RIGHT: #3a4f63 thin ridge">
                           
                            
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <asp:image id="Image2" runat="server" ImageUrl="images/alert3.gif"></asp:image>
                            <br />
                            <br />
                            <br />
							<br />
							<br />
                         
							<font size="2" face="Verdana,arial">Utenza non autorizzata ai dati richiesti</font>
						</div>
				
			 <div class="RigaPulsanti" style="BORDER-LEFT: #3a4f63 thin ridge; BORDER-RIGHT: #3a4f63 thin ridge; BORDER-BOTTOM: #3a4f63 thin ridge">
                 Vai al Log On<asp:imagebutton id="imgChiudi" Width="45px" Height="45px"  runat="server" ImageUrl="~/Images/vistacattiva_big.png" ToolTip="Chiudi" ></asp:imagebutton>
                             
             </div>
			
                
</div>
    </form>
</body>
</html>
