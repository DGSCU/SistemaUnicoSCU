<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmSessioneMultiplaErrore.aspx.vb" Inherits="Futuro.WfrmSessioneMultiplaErrore" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
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
     <legend>Errore Sessione Multipla</legend>
    <p  class="msgErrore">
      Attenzione! E' stato rilevato un tentativo di accesso multiplo su piu' schede dello stesso browser.
      Pertanto e' necessario chiudere tutte le schede utilizzate per Helios/Futuro ed accedere nuovamente al sistema.
      <br />
      NOTA: Per il corretto funzionamento del sistema l'accesso va effettuato su di un'unica scheda del browser. 
      Nel caso sia fortemente necessario avere più pagine aperte contemporaneamente si puo' effettuare un ulteriore accesso solo utilizzando browser diversi (ad esempio Internet Explorer e Chrome/Firefox etc).</p>
    <p>
    
        Se il problema persiste</p>
   <p>
   
   
       siete pregati di contattare l&#39;assistenza tecnica HELIOS/FUTURO</p>
     <p>
   
   
         attraverso questo indirizzo email: <a href="mailto:heliosweb@serviziocivile.it">heliosweb@serviziocivile.it</a></p>
    <p>
   
   
        oppure tramite questo numero di telefono: 06-67796175</p>
        
        
       
        
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
