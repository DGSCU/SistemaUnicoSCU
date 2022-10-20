<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="page_error.aspx.vb" Inherits="Futuro.page_error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" lang="it" xml:lang="it">
<head id="Head1" runat="server">
    <title title="Errore di Sistema" >Errore di Sistema</title>
    <link href="Styles/Site.css" rel="stylesheet"  type="text/css" media="screen" />
</head>
<body>
    <form  id="form1" runat="server" >
    <div class="page">
       
         <div class="header">
            <div class="title">
                <div class="logo">
                    <asp:Image ID="Logo" ToolTip="Logo Sistema Futuro" ImageUrl="~/Images/UNICO_BANNER.jpg" AlternateText="Logo del sistema Futuro"  runat="server"   />
                </div>
               
           </div>  
         </div>    
         <div class="clear hideSkiplink">&nbsp; 
               
         </div>
        
         <fieldset class="FadcLogIn"   title="Errore Imprevisto">
     <legend>Errore imprevisto</legend>
    <p  class="msgErrore">
      
        Attenzione. Si e&#39; verificato un 
        errore non gestito.</p>
    <p>
    
        Se il problema persiste</p>
   <p>
   
   
       siete pregati di contattare l&#39;assistenza tecnica HELIOS/FUTURO</p>
     <p>
   
   
         attraverso questo indirizzo email: <a href="mailto:heliosweb@serviziocivile.it">heliosweb@serviziocivile.it</a></p>
    <p>
   
   
        oppure tramite questo numero di telefono: 06-67796175</p>
        
        <p>
        Clicca <asp:HyperLink ID="Hyllogon" NavigateUrl="~/LogOn.aspx" runat="server">quì</asp:HyperLink>
        
        &nbsp;per riconnetterti al sistema</p>
       
        
     </fieldset>        
  <div class="crediti">
        Realizzato da <asp:HyperLink ID="HplDGSCN"  NavigateUrl="http://www.serviziocivile.gov.it" Target="_blank" runat="server">DGSCU</asp:HyperLink>&nbsp;in 
          collaborazione con Logica Informatica S.r.l.
           <p>
               <img alt="Logo Dipartimento per le politiche giovanili e il Servizio civile universale" src="Images/logo-presidenza_medium.png" />
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