<%@ Page Title="Calcolo Punteggio Rilevanza e Coerenza" Language="vb" AutoEventWireup="false" CodeBehind="WfrmCalcoloRilevanzaCoerenza.aspx.vb" Inherits="Futuro.WfrmCalcoloRilevanzaCoerenza" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">

<head id="Head1" runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>Calcolo Punteggio Rilevanza e Coerenza</title>

</head>
<body>
    <form id="form1" runat="server">
    <fieldset class="ContornoPagina">
    <legend>Calcolo Rilevanza e Coerenza</legend>
    <div class="wrapper" style="width:100%;">
      <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Calcolo Punteggio Rilevanza e Coerenza"></asp:Label></h2>
            </div>
       <fieldset class="ContornoPagina">
    <legend>Valori Impostati</legend>
    <div class="wrapper" style="width:100%;border:0px">
        <div class="row" >
            <div class="collable" style="width:25%" >
                <asp:Label ID="labelCont" AssociatedControlID="lblContesto"  runat="server" Text="Contesto"></asp:Label>
            </div>
            <div class="colOggetti" style="width:75%">
                <asp:Label ID="lblContesto" CssClass="labelDati"   runat="server"></asp:Label>
            </div>
       </div>
        <div class="row" >
        <div class="collable" style="width:25%" >
            <asp:Label ID="labelObie" AssociatedControlID="lblObiettivi"  runat="server" Text="Obiettivi"></asp:Label>
        </div>
        <div class="colOggetti" style="width:75%">
            <asp:Label ID="lblObiettivi" CssClass="labelDati"   runat="server"></asp:Label>
        </div>
    </div>
            <div class="row" >
        <div class="collable" style="width:25%" >
            <asp:Label ID="labelDesc" AssociatedControlID="lblDescrizione"  runat="server" Text="Descrizione"></asp:Label>
        </div>
        <div class="colOggetti" style="width:75%">
            <asp:Label ID="lblDescrizione" CssClass="labelDati"   runat="server"></asp:Label>
        </div>
    </div>
    </div>
    </fieldset>
     <fieldset class="ContornoPagina">
    <legend>Valori Previsti</legend>
    <div class="wrapper" style="width:100%;border:0px">
      <div class="row" >
        <div class="collable" style="width:25%" >
            <asp:Label ID="labelRilev" AssociatedControlID="lblRilevanza"  runat="server" Text="Rilevanza"></asp:Label>
        </div>
        <div class="colOggetti" style="width:75%">
            <asp:Label ID="lblRilevanza" CssClass="labelDati"   runat="server"></asp:Label>
        </div>
    </div>
           <div class="row" >
        <div class="collable" style="width:25%" >
            <asp:Label ID="labelCoeren" AssociatedControlID="lblCoerenza"  runat="server" Text="Coerenza"></asp:Label>
        </div>
        <div class="colOggetti" style="width:75%">
            <asp:Label ID="lblCoerenza" CssClass="labelDati"   runat="server"></asp:Label>
        </div>
    </div>
    </div>
    </fieldset>

    <div class="RigaPulsanti">

            <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" OnClientClick="javascript:window.close();" />
            <br />
        </div>
    </div>
    </fieldset>
    </form>
</body>
</html>