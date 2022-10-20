<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmResetPasswordUtenti.aspx.vb" Inherits="Futuro.WfrmResetPasswordUtenti" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head runat="server">
    <title>Conferma Ripristino Password</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmRipristinoPasswrd" runat="server" method="post">
        <div class="fieldsetrasparente">
            <fieldset class="ContornoPagina">
                <legend>Conferma Ripristino Password</legend>
                <div class="wrapper" style="width:100%">
                    <div class="headers" >
                        <h2><asp:Label ID="lblTitoloPagina" runat="server"  Text="Conferma Ripristino Password"></asp:Label></h2>
                    </div>
                    <div class="RigaVuota">&nbsp;</div>
                    <div class="row">
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblRipristino" CssClass="label" AssociatedControlID="LblUtente" runat="server" Text="Ripristino per"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:85%">
                            <asp:label id="LblUtente" CssClass="bold" runat="server"></asp:label>        
                        </div>
                    </div>
                    <div class="RigaVuota">&nbsp;</div>
                    <div class="row">
                        <div class="colOggetti" style="width:100%">
                            <asp:checkbox id="ChkCredenzialiEmail" runat="server"  Text="Invio Credenziali per EMail"></asp:checkbox>
                        </div>
                    </div>
                    <div class="RigaVuota">&nbsp;</div>
                    <div class="row">
                        <div class="colOggetti" style="width:100%">
                            <asp:checkbox id="ChkStampaCredenziali" runat="server"  Text="Stampa Credenziali"></asp:checkbox>
                        </div>
                    </div>
                    <div class="RigaVuota">&nbsp;</div>
                    <div class="RigaPulsanti">
                        <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" Text="Salva"  />
                        <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
                    </div>
                </div>
            </fieldset>
        </div>
    </form>
</body>
</html>
