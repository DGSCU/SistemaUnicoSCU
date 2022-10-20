<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LogOn.aspx.vb" Inherits="Futuro.LogOn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="it" xml:lang="it">

<head runat="server">
    <title title="Accesso al sistema Accessibile Login" >ACCESSO AL SISTEMA</title>
    <link href="Styles/Site.css" rel="stylesheet"  type="text/css" media="screen" />
    <%--<link rel="icon" href="Images/futuro2.ico" />--%>
</head>
<body>
<form id="form1" runat="server" defaultbutton="btnAccedi">
    <div class="page">      
        <div class="header">
            <div class="title">
                <div class="logo">
                    <asp:Image ID="Logo" ToolTip="Logo Sistema Unico" ImageUrl="~/Images/UNICO_BANNER.jpg" AlternateText="Logo del sistema Futuro"  runat="server"   />
                </div>   
           </div>  
        </div>    
        <div class="clear hideSkiplink">&nbsp;
        </div> 
      <ul style="border-color:Red;border-style:ridge;margin:1% 20% 1% 20%;text-align:left;" runat="server" id="avviso">
        <dt style="margin-left:5px;font-size:1.6em;font-weight:bold;">Avviso:</dt>
        <dt style="margin-left:5px;font-size:1.2em;" runat="server" id="testoavviso"></dt>
       <%-- <li style="margin-left:5px;font-size:1.4em;font-weight:bold;">Il bando dei volontari funzionerà normalmente. Il concorso “Realizzazione di uno spot televisivo e di un cortometraggio di promozione del Servizio Civile Nazionale” funzionerà regolarmente.</li>--%>

    </ul>
        <fieldset class="FadcLogIn" title="Accedi"  runat="server" id="FORMLOGIN" >
            <legend>ACCESSO AI SISTEMI Helios-Futuro</legend>
            <asp:Button ID="btnAccediSpid" CssClass="PulsanteGrande" runat="server" Text="Accedi con SPID" />

           <p style="width:70%">
                <asp:Label ID="lblError" CssClass="msgErrore" runat="server" Text="" ></asp:Label>
            </p>
            <p class="PLogInADC" style="width:70%">
                Immettere nome utente e password.
            </p>
            <div align="right" style="width:70%">
                <asp:Label ID="lblUtente" AssociatedControlID="txtNomeUtente"  runat="server" Text="Nome Utente:"/>
                <asp:TextBox ID="txtNomeUtente" runat="server" ValidationGroup="login" Width="180px"/>
            </div>
            <br />
            <div  align="right" style="width:70%">
                <asp:Label ID="lblPassword" AssociatedControlID="txtPassword"  runat="server" Text="Password:"/>
                <asp:TextBox ID="txtPassword" CssClass="stilePass" TextMode="Password" runat="server" ValidationGroup="login" Width="180px"/>
            </div>
            <br />
            <div align="right" style="width:70%" >
                <asp:Button ID="btnAccedi" CssClass="Pulsante" runat="server" Text="Accedi" />
            </div> 
            <br />
            <br />
            <p class="PLogInADC" style="width:100%">
                NOTA AGLI ENTI
            </p>
            <p style="width:100%">
            Per entrare nella parte relativa all'Albo e in quella di gestione e presentazione di programmi e progetti, &egrave; necessario utilizzare SPID.
            Per le altre funzionalit&agrave; (operatori volontari, formazione ...) restano valide le credenziali  già inserite
            </p>
        </fieldset>
        <div class="crediti">
            <p>
               <%--<img alt="Logo Unione Europea" src="Images/Logo-ue_blu_medium.png" />
               <img alt="Logo Garanzia Giovani" src="Images/logoGaranziaGiovani_medium.png" />--%>
               <img alt="Logo Dipartimento per le politiche giovanili e il Servizio civile universale" src="Images/logo-presidenza_medium.png" />
            </p>
            <p class="h7">
                Realizzato da <asp:HyperLink ID="HplDGSCN"  NavigateUrl="http://www.serviziocivile.gov.it" Target="_blank" runat="server">DGSCU</asp:HyperLink>&nbsp;in 
                collaborazione con Logica Informatica S.r.l.
            </p>
            <p>
                <a href="https://jigsaw.w3.org/css-validator/check/referer">
                    <img style="border:0;width:88px;height:31px" src="https://jigsaw.w3.org/css-validator/images/vcss-blue" alt="CSS Valido!" />
                </a>
                <a href="https://validator.w3.org/check?uri=referer">
                    <img src="https://www.w3.org/Icons/valid-xhtml10" alt="Valid XHTML 1.0 Transitional" height="31" width="88" />
                </a>
            </p>
          </div>
        </div>
    </form>
</body>
</html>

