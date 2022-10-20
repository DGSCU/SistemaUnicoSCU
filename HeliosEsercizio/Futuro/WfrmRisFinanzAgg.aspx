<%@ Page Title="Calcolo Risorse Finanziarie Aggiuntive" Language="vb" AutoEventWireup="false" CodeBehind="WfrmRisFinanzAgg.aspx.vb" Inherits="Futuro.WfrmRisFinanzAgg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">

<head id="Head1" runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>Calcolo Risorse Finanziarie Aggiuntive</title>
           <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css"/>
        <script src="Scripts/JHelper.js" type="text/javascript"></script>
        <script type="text/javascript">
               function CheckNumeroValido(idCampo, descrizione) {
                   var clientIdErrore = "msgErrore";
                   var numeroValido = ValidaNumero(idCampo, descrizione, clientIdErrore);
            if (numeroValido == false) {
                document.getElementById(idCampo).value = 0;
                TornaAdInizioPagina();
            }
        };
        </script>

</head>
<body>
    <form id="form1" runat="server">
    <fieldset class="fieldsetrasparente">
    <div class="wrapper" style="width:100%">
      <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Calcolo Risorse Finanziarie Aggiuntive"></asp:Label></h2>
            </div>
         <div class="rowGroup" style="height:auto">
            <div class="RigaVuota"> &nbsp;</div>
		    <asp:label id="msgErrore"  runat="server" CssClass="msgErrore"></asp:label>
             <div class="RigaVuota">  &nbsp; </div>
		</div>
          <div class="row" >
    
       <div class="collable" style="width:15%" >
           <asp:Label ID="lblNVol" AssociatedControlID="txtnumvol"  runat="server" Text="N° Volontari"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:TextBox ID="txtnumvol" CssClass="labelDati" Text="0"  runat="server" onchange="CheckNumeroValido('txtnumvol','N° Volontari')"></asp:TextBox>
       </div>
</div>
       <div class="row" >
        <div class="collable" style="width:15%" >
           <asp:Label ID="lblImporto" AssociatedControlID="txtimporto"  runat="server" Text="Importo Aggiuntivo"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:TextBox ID="txtimporto" CssClass="labelDati" text="0" runat="server" onchange="CheckNumeroValido('txtimporto','Importo Aggiuntivo')"></asp:TextBox>
       </div>

       </div>
           <div class="row" >
        <div class="collable" style="width:15%" >
           <asp:Label ID="lblperc" AssociatedControlID="txtperc"  runat="server" Text="Percentuale"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="txtperc" CssClass="labelDati" Text="0"  runat="server"></asp:Label>
           <asp:Label ID="perc" CssClass="labelDati" text="%" runat="server" ></asp:Label>
       </div>

       </div>
       <div class="RigaPulsanti">
       <asp:Button ID="cmdCalcolaPerc" runat="server" ToolTip="Calcolo Percentuale" CssClass="Pulsante" 
                Text="Calcola Percentuale" />
            &nbsp;
            <asp:Button ID="imgchiudi" runat="server" CssClass="Pulsante" Text="Chiudi" OnClientClick="javascript:window.close();" />
            <br />
        </div>
    </div>
    </fieldset>
    </form>
</body>
</html>
