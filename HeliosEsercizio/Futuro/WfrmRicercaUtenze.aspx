<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmRicercaUtenze.aspx.vb" Inherits="Futuro.WfrmRicercaUtenze" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head runat="server">
    <title>Ricerca Profilo Utenze</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        var prefissoIdClient = "MainContent_";

        function CostruisciId(IdServer) {
            var IdClient = prefissoIdClient + IdServer
            return IdClient
        }

        function Chiudi() {
            var txtUtenza = document.getElementById('txtUtenza');
            var idUtenzaSim = CostruisciId('txtUtenzaSim');
            var txtUtenzaSim = window.opener.document.getElementById(idUtenzaSim); 
            txtUtenzaSim.value = txtUtenza.value;
            window.close();
        }

    </script>
</head>
<body>
    <form id="frmRicercaProfiloUtenze"  runat="server" method="post">
        <div class="fieldsetrasparente">
            <fieldset class="ContornoPagina">
                <legend>Ricerca Profilo Utenze</legend>
                <div class="wrapper" style="width:100%">
                    <div class="headers" >
                       <h2><asp:Label ID="lblTitoloPagina" runat="server"  Text="Ricerca Profilo Utenze"></asp:Label></h2>
                    </div>
                    <div class="RigaVuota">&nbsp;</div>
                    <div class="row">
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblUtenza" CssClass="label" AssociatedControlID="txtUtenza" runat="server" Text="Utenza"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:85%">
                            <asp:textbox id="txtTipUtenza" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="Small" Width="5%" ReadOnly="True" ToolTip="Tipo Utenza"></asp:textbox>       
                            <asp:TextBox ID="txtUtenza" CssClass="textbox" runat="server" Width="60%"></asp:TextBox>         
                        </div>
                    </div>
                    <div class="RigaVuota">&nbsp;</div>
                    <div class="row" style="text-align:center">
                        <asp:Label ID="lblMessage" CssClass="bold" runat="server" Text="ID - Profilo - Read Only"></asp:Label>   
                    </div>
                    <div class="RigaVuota">&nbsp;</div>
                    <div class="RigaPulsanti">
                        <asp:Button ID="cmdRicerca" CssClass="Pulsante" runat="server" Text="Ricerca"  />
                        <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  OnClientClick="Chiudi()"/>
                    </div>
                </div>
            </fieldset>
        </div>
    </form>
</body>
</html>
