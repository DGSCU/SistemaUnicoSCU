<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LogOnRead.aspx.vb" Inherits="Futuro.LogOnRead" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title title="Accesso al sistema Accessibile LogOn Read" >ACCESSO AL SISTEMA</title>
    <link href="Styles/Site.css" rel="stylesheet"  type="text/css" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="page">
       
         <div class="header">
            <div class="title">
                <div class="logo">
                     <asp:Image ID="Logo" ToolTip="Logo Sistema Unico Consultazione" ImageUrl="~/Images/UNICO_BANNER_consultazione.jpg" AlternateText="Logo del sistema Futuro"  runat="server"   />
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

     
         <fieldset class="FadcLogIn"   title="Accedi" runat="server" id="FORMLOGIN">
     <legend>ACCESSO AL SISTEMA</legend>
    
    <p style="width:70%">
        <asp:Label ID="lblError" CssClass="msgErrore" runat="server" Text="" ></asp:Label>

    </p>
    <p class="PLogInADC" style="width:70%">
    Immettere nome utente e password.
    </p>
    <div  align="right" style="width:70%">
    <asp:Label ID="lblUtente" AssociatedControlID="txtNomeUtente"  runat="server" Text="Nome Utente:"   ></asp:Label>
    <asp:TextBox ID="txtNomeUtente" autofocus="true" runat="server" ValidationGroup="login" 
            Width="180px" ></asp:TextBox>
    </div>
    <br />
    <div  align="right" style="width:70%">
    <asp:Label ID="lblPassword" AssociatedControlID="txtPassword"  runat="server" Text="Password:" ></asp:Label>
    <asp:TextBox ID="txtPassword" CssClass="stilePass" TextMode="Password" 
            runat="server" ValidationGroup="login" Width="180px"></asp:TextBox>
     </div>
     <br />
     <div align="right" style="width:70%" >
    <asp:Button ID="btnAccedi" CssClass="Pulsante" runat="server" Text="Accedi" />
    
   </div>

  
  </fieldset>

  <div class="crediti">
       
           <p>
               <%--<img alt="Logo Unione Europea" src="Images/Logo-ue_blu_medium.png" />
               <img alt="Logo Garanzia Giovani" src="Images/logoGaranziaGiovani_medium.png" />--%>
               <img alt="Logo Dipartimento per le politiche giovanili e il Servizio civile universale" src="Images/logo-presidenza_medium.png" />
           </p>
          <p class="h7">
           Realizzato da <asp:HyperLink ID="HplDGSCN"  NavigateUrl="http://www.serviziocivile.gov.it" Target="_blank" runat="server">DGSCU</asp:HyperLink>&nbsp;in 
          collaborazione con Logica informatica S.r.l.
          </p>
           <p>
<a href="http://jigsaw.w3.org/css-validator/check/referer">
    <img style="border:0;width:88px;height:31px"
        src="https://jigsaw.w3.org/css-validator/images/vcss-blue"
        alt="CSS Valido!" />
</a>
<a href="http://validator.w3.org/check?uri=referer"><img
      src="https://www.w3.org/Icons/valid-xhtml10" alt="Valid XHTML 1.0 Transitional" height="31" width="88" /></a>
</p>
 

      </div>

    </div>

   
    </form>
    
</body>
</html>

