<%@ Page Title="Verifica controlli aggiuntivi OLP" Language="vb" AutoEventWireup="false" CodeBehind="WfrmVerificaUsabilitaOLP.aspx.vb" Inherits="Futuro.WfrmVerificaUsabilitaOLP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head runat="server">
<link href="Styles/Site.css" rel="stylesheet" type="text/css" />
<title>Verifica controlli aggiuntivi OLP</title>
</head>
<body>
    <form id="form1" runat="server">
     <fieldset class="ContornoPagina">
        <legend>Info anomalie</legend> 
        <div class="wrapper" style="width:100%;">
            <div class="headers" >
                <h2><asp:Label ID="lblIntProgetto" runat="server"  Text="Verifica "></asp:Label>
                   <asp:label  id="lblRuolo" style="vertical-align:middle"  Text="&nbsp;" runat="server"></asp:label>
                </h2>
            </div>
            <div class="RigaVuota">&nbsp;</div>
            <div class="row" >
                <asp:TextBox ID="TxtNote"  CssClass="textbox" runat="server" TextMode="MultiLine" Rows="6"  Width="90%" ></asp:TextBox>              
            </div>
            <div class="RigaVuota">&nbsp;</div>
            <div class="RigaPulsanti"> 
                <asp:Button id="cmdChiudi" CssClass="Pulsante" OnClientClick="javascript:window.close();" runat="server" Text="Chiudi" ></asp:Button>
            </div>
        </div>
    </fieldset>      
    </form>
</body>
</html>
