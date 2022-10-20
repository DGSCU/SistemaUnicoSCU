<%@ Page Title="Riepilogo Informazioni Sede" Language="vb" AutoEventWireup="false" CodeBehind="informazionientesede.aspx.vb" Inherits="Futuro.informazionientesede" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head id="Head1" runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>Riepilogo Informazioni Sede</title>
    <script language="javascript" type="text/javascript">
        function Stampa() {
            document.all.StampaPagina.style.visibility = 'hidden';
            window.print()
            window.close()
        }
		</script>
</head>
<body>
    <form id="form1" runat="server">
    <fieldset class="ContornoPagina">
    <legend>Dettaglio Sede</legend>
    
    <div class="wrapper"  style="width:100%;border-width:0px">
          <div class="row" style="text-align:right">
           <img style="CURSOR: pointer" id="StampaPagina" onclick="javascript: Stampa()" name="StampaPagina"
							alt ="Stampa" src="images/printHELIOS.jpg" 
    width="50" height="34"/>
          
          </div>  
     <div class="row" >
     <div class="collable" style="width:15%">
        <asp:Label id="labelStato" AssociatedControlID="lblStato" CssClass="label" runat="server" Text="Stato Sede" ></asp:Label>
    </div>
    <div class="colOggetti" style="width:85%">
        <asp:Label id="lblStato" runat="server" CssClass="labelDati" Visible="true"></asp:Label>
     </div>
        </div>  
     <div class="row" >
     <div class="collable" style="width:15%">
        <asp:Label id="label1" AssociatedControlID="lblSedeEnte" CssClass="label" runat="server" Text="Sede"></asp:Label>
    </div>
     <div class="colOggetti" style="width:85%">
        <asp:Label id="lblSedeEnte" runat="server" CssClass="labelDati"  Visible="true"></asp:Label>
     </div>
            </div>
   
   <div class="row" >
     <div class="collable" style="width:15%">
        <asp:Label id="labelCodSedeAttuazione" AssociatedControlID="lblCodSedeAttuazione" CssClass="label" runat="server" Text="Cod. Sede" ></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
        <asp:Label id="lblCodSedeAttuazione" runat="server" CssClass="labelDati" Visible="true"></asp:Label>
     </div>
     <div class="collable" style="width:15%">
        <asp:Label id="tipologia" AssociatedControlID="lbltipologia" runat="server" CssClass="label" Text="Tipologia"></asp:Label>
    </div>
     <div class="colOggetti" style="width:35%">
        <asp:Label id="lbltipologia" runat="server" CssClass="labelDati" Visible="true"></asp:Label>
     </div>
   </div>
   <div class="row" >
     <div class="collable" style="width:15%">
        <asp:Label id="indirizzo" AssociatedControlID="lblIndirizzo" CssClass="label" runat="server" Text="Indirizzo" ></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
        <asp:Label id="lblIndirizzo" runat="server" CssClass="labelDati" Visible="true"></asp:Label>
     </div>
     <div class="collable" style="width:15%">
        <asp:Label id="numero" AssociatedControlID="lblNumero" runat="server" CssClass="label" Text="N&#186;"></asp:Label>
    </div>
     <div class="colOggetti" style="width:35%">
        <asp:Label id="lblNumero" runat="server" CssClass="labelDati" Visible="true"></asp:Label>
     </div>
   </div>
   <div class="row" >
     <div class="collable" style="width:15%">
        <asp:Label id="comunue" AssociatedControlID="lblComunue" CssClass="label" runat="server" Text="Comune" ></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
        <asp:Label id="lblComunue" runat="server" CssClass="labelDati" Visible="true"></asp:Label>
     </div>
     <div class="collable" style="width:15%">
        <asp:Label id="cap" AssociatedControlID="lblCAP" runat="server" CssClass="label" Text="CAP"></asp:Label>
    </div>
     <div class="colOggetti" style="width:35%">
        <asp:Label id="lblCAP" runat="server" CssClass="labelDati" Visible="true"></asp:Label>
     </div>
   </div>
   <div class="row" >
     <div class="collable" style="width:15%">
        <asp:Label id="numeroVolontari" AssociatedControlID="lblNumVo" runat="server" CssClass="label" Text="N&#186; Vol." ></asp:Label>
    </div>
     <div class="colOggetti" style="width:35%">
        <asp:Label id="lblNumVo" runat="server" CssClass="labelDati" Visible="true"></asp:Label>
     </div>
          <div class="collable" style="width:15%">
        <asp:Label id="dettRecapito" AssociatedControlID="lblDettRecapito" CssClass="label" runat="server" Text="Dettaglio Recapito" ></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
        <asp:Label id="lblDettRecapito" runat="server" CssClass="labelDati" Visible="true" Text="&nbsp;"></asp:Label>
     </div>
   </div>
   <div class="row" >
     <div class="collable" style="width:15%">
        <asp:Label id="palazzina" AssociatedControlID="lblPalazzina" CssClass="label" runat="server" Text="Palazzina" ></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
        <asp:Label id="lblPalazzina" runat="server" CssClass="labelDati" Visible="true" Text="&nbsp;"></asp:Label>
     </div>
     <div class="collable" style="width:15%">
        <asp:Label id="scala" AssociatedControlID="lblScala" runat="server" CssClass="label" Text="Scala" ></asp:Label>
    </div>
     <div class="colOggetti" style="width:35%">
        <asp:Label id="lblScala" runat="server" CssClass="labelDati" Visible="true" Text="&nbsp;"></asp:Label>
     </div>
   </div>
   <div class="row" >
     <div class="collable" style="width:15%">
        <asp:Label id="piano" AssociatedControlID="lblPiano" CssClass="label" runat="server" Text="Piano" ></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
        <asp:Label id="lblPiano" runat="server" CssClass="labelDati" Visible="true" Text="&nbsp;"></asp:Label>
     </div>
     <div class="collable" style="width:15%">
        <asp:Label id="interno" AssociatedControlID="lblInterno" runat="server" CssClass="label" Text="Interno" ></asp:Label>
    </div>
     <div class="colOggetti" style="width:35%">
        <asp:Label id="lblInterno" runat="server" CssClass="labelDati" Visible="true" Text="&nbsp;"></asp:Label>
     </div>
   </div>
   <div class="row" >
     <div class="collable" style="width:15%">
        <asp:Label id="telefono" AssociatedControlID="lblTelefono" CssClass="label" runat="server" Text="Telefono" ></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
        <asp:Label id="lblTelefono" runat="server" CssClass="labelDati" Visible="true" Text="&nbsp;"></asp:Label>
     </div>
     <div class="collable" style="width:15%">
        <asp:Label id="fax" AssociatedControlID="lblFax" runat="server" CssClass="label" Text="Fax" ></asp:Label>
    </div>
     <div class="colOggetti" style="width:35%">
        <asp:Label id="lblFax" runat="server" CssClass="labelDati" Visible="true" Text="&nbsp;"></asp:Label>
     </div>
   </div>
   <div class="row" >
     <div class="collable" style="width:15%">
        <asp:Label id="titoloGiuridico" AssociatedControlID="lblTitoloGiuridico" CssClass="label" runat="server" Text="Titolo Giuridico" ></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
        <asp:Label id="lblTitoloGiuridico" runat="server" CssClass="labelDati" Visible="true" Text="&nbsp;"></asp:Label>
     </div>
     <div class="collable" style="width:15%">
        <asp:Label id="specAltro" AssociatedControlID="lblSpecAltro" runat="server" CssClass="label" Text="Specificare Altro" ></asp:Label>
    </div>
     <div class="colOggetti" style="width:35%">
        <asp:Label id="lblSpecAltro" runat="server" CssClass="labelDati" Visible="true" Text="&nbsp;"></asp:Label>
     </div>
   </div>
   <div class="row" >
     <div class="collable" style="width:15%">
        <asp:Label id="indirizzoWeb" AssociatedControlID="lblHTTP" CssClass="label" runat="server" Text="http" ></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
        <asp:Label id="lblHTTP" runat="server" CssClass="labelDati" Visible="true" Text="&nbsp;"></asp:Label>
     </div>
     <div class="collable" style="width:15%">
        <asp:Label id="email" AssociatedControlID="lblEmail" runat="server" CssClass="label" Text="Email" ></asp:Label>
    </div>
     <div class="colOggetti" style="width:35%">
        <asp:Label id="lblEmail" runat="server" CssClass="labelDati" Visible="true"></asp:Label>
     </div>
   </div>

    <div class="RigaPulsanti">
        <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" OnClientClick="javascript: window.close()"  />
    </div>

    </div>
    </fieldset>			

    </form>
</body>
