<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmPaginaDiStampaPassword.aspx.vb" Inherits="Futuro.WfrmPaginaDiStampaPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head runat="server">
    <title>Stampa Credenziali</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmStampaPassword" runat="server" method="post">
        <div class="fieldsetrasparente">
            <fieldset class="ContornoPagina">
                <legend>Stampa Credenziali</legend>
                <div class="wrapper" style="width:100%; text-align:center">
                    <div class="RigaVuota" >&nbsp;</div>
                    <h2 style="font-weight:bold">L'Ufficio Nazionale per il Servizio Civile e' lieto di comunicarLe che da oggi potra' accedere al nuovo Sistema 
                        Unico.</h2>
                    <div class="RigaVuota" >&nbsp;</div>
                    <div class="RigaVuota" >&nbsp;</div>
                    <div class="RigaVuota" >&nbsp;</div>
                    <h2 style="font-weight:bold">L'indirizzo internet per l'accesso al sistema e': <a href="https://futuro.serviziocivile.it"> https://SISTEMAUNICOSCN.serviziocivile.it </a></h2>
                    <div class="RigaVuota" >&nbsp;</div>
                    <div class="RigaVuota" >&nbsp;</div>
                    <div class="RigaVuota" >&nbsp;</div>
                    <h3 style="font-weight:bold">I Suoi parametri di ingresso sono i seguenti:</h3>
                    <div class="RigaVuota" >&nbsp;</div>
                    <div class="RigaVuota" >&nbsp;</div>
                    <div class="RigaVuota" >&nbsp;</div>
                    <div class="row">
                        <div class="collable" style="width:45%;text-align:right">
                            <h3><asp:Label id="Label1" runat="server" Font-Bold="True" CssClass="bold" Text="USER NAME"></asp:Label></h3>
                        </div>
                        <div class="collable" style="width:10%">&nbsp;</div>
                        <div class="collable" style="width:45%;text-align:left">
                             <h3><asp:Label id="LblUser" runat="server" Font-Bold="True" CssClass="bold"></asp:Label></h3>
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width:45%;text-align:right">
                            <h3><asp:Label id="Label3" runat="server" Font-Bold="True" CssClass="bold" Text="PASSWORD"></asp:Label></h3>
                        </div>
                        <div class="collable" style="width:10%">&nbsp;</div>
                        <div class="collable" style="width:45%;text-align:left">
                             <h3><asp:Label id="LblPassword" runat="server" Font-Bold="True" CssClass="bold"></asp:Label></h3>
                        </div>
                    </div>
                    <div class="RigaVuota">&nbsp;</div>
                    <div class="RigaPulsanti">
                        <asp:Button ID="imgStampa" CssClass="Pulsante" runat="server" Text="Stampa"  />
                    </div>
                </div>
            </fieldset>
        </div>
    </form>
</body>
</html>
