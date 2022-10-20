<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmProgrammiVerificaIstanzaAnomaliaProgramma.aspx.vb" Inherits="Futuro.WfrmProgrammiVerificaIstanzaAnomaliaProgramma" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Verifica anomalie programma</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
     <fieldset class="ContornoPagina">
        <legend>Verifica Anomalie Programma</legend>    
                              
            <div class="wrapper">
				
				<div class="tr">
					<div ><asp:textbox id="TxtNote" runat="server" Width="99%"
							TextMode="MultiLine" Height="400px"></asp:textbox></div>
				</div>
				<div class="tr">
					<div class="collable" style="width:100%;height:30px;">
						NOTA BENE:Le eventuali anomalie qui riscontrate sono riferite al singolo Programma.	
					</div>
					
				</div>
               
                <div class="RigaPulsanti">
                        <asp:Button id="imgChiudi" runat="server" Text="Chiudi" CssClass="Pulsante" />
                       
			  </div>
			</div>
            
            </fieldset>
    </form>
</body>
</html>
