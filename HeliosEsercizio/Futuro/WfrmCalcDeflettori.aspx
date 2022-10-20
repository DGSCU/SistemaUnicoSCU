<%@ Page Title="Calcolo Deflettori" Language="vb" AutoEventWireup="false" CodeBehind="WfrmCalcDeflettori.aspx.vb" Inherits="Futuro.WfrmCalcDeflettori" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">

<head id="Head1" runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>Calcolo Deflettori</title>

</head>
<body>
    <form id="form1" runat="server">
    <fieldset class="ContornoPagina">
    <legend>Calcolo Deflettori</legend>
    <div class="wrapper" style="width:100%;border:0px">
      <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Calcolo Deflettori"></asp:Label></h2>
            </div>
       <fieldset class="ContornoPagina">
    <legend>Sanzioni Lievi</legend>
    <div class="wrapper" style="width:100%;border:0px">
        <div class="row" >
            <div class="collable" style="width:25%" >
                <asp:Label ID="labelIspAnno" AssociatedControlID="lblIspAnno" CssClass="label" runat="server" Text="Verifiche anno"></asp:Label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:Label ID="lblIspAnno" CssClass="labelDati"   runat="server"></asp:Label>
            </div>
            <div class="collable" style="width:25%" >
                <asp:Label ID="labelTotIspAnnoLIEVE" AssociatedControlID="txtTotIspAnnoLIEVE" CssClass="label"  runat="server" Text="Totale"></asp:Label>
            </div>
            <div class="colOggetti" style="width:20%">
                <asp:TextBox ID="txtTotIspAnnoLIEVE" CssClass="textbox"   runat="server"></asp:TextBox>
            </div>
            <div class="colOggetti" style="width:5%">
             <asp:ImageButton ID="lTotIspAnno" AlternateText="Dettaglio Totale" ToolTip="Dettaglio" runat="server" ImageUrl="Images/lenteIngrandimento_small.png" />
            </div>
       </div>
        <div class="row" >
            <div class="collable" style="width:25%" >
                <asp:Label ID="labelDiffLieve2" AssociatedControlID="lblDiffLieve" CssClass="label"  runat="server" Text="Sanzioni lievi anno"></asp:Label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:Label ID="lblDiffLieve" CssClass="labelDati"   runat="server"></asp:Label>
            </div>
            <div class="collable" style="width:25%" >
                <asp:Label ID="labelTotDiffLieve" AssociatedControlID="txtTotDiffLieve"  runat="server" Text="Totale"></asp:Label>
            </div>
            <div class="colOggetti" style="width:20%">
                <asp:TextBox ID="txtTotDiffLieve" CssClass="textbox"   runat="server"></asp:TextBox>
            </div>
            <div class="colOggetti" style="width:5%">
             <asp:ImageButton ID="lTotDiffLieve" AlternateText="Dettaglio Totale Sanzioni" ToolTip="Dettaglio Sanzioni" runat="server" CausesValidation="false"   ImageUrl="Images/lenteIngrandimento_small.png" />
            </div>
       </div>
            <div class="row" >
              <div class="collable" style="width:25%" >
                <asp:Label ID="labelValorelieveperc" AssociatedControlID="lblValorelieveperc"  runat="server" Text="Valore % Deflettore"></asp:Label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:TextBox ID="lblValorelieveperc" CssClass="textbox"   runat="server"></asp:TextBox>
            </div>
            <div class="collable" style="width:25%" >
                <asp:Label ID="labelValoreLieveAnno" AssociatedControlID="lblValoreLieveAnno"  runat="server" Text="Valore Deflettore"></asp:Label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:TextBox ID="lblValoreLieveAnno" CssClass="textbox"   runat="server"></asp:TextBox>
            </div>

       </div>
    </div>
    </fieldset>
     <fieldset class="ContornoPagina">
    <legend>Sanzioni Gravi</legend>
    <div class="wrapper" style="width:100%;border:0px">
        <div class="row" >
            <div class="collable" style="width:25%" >
                <asp:Label ID="labelIspAnnoGrave" AssociatedControlID="lblIspAnnoGrave" CssClass="label" runat="server" Text="Verifiche anno"></asp:Label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:Label ID="lblIspAnnoGrave" CssClass="labelDati"   runat="server"></asp:Label>
            </div>
            <div class="collable" style="width:25%" >
                <asp:Label ID="labeltxtTotIspAnnoGrave" AssociatedControlID="txtTotIspAnnoGrave" CssClass="label"  runat="server" Text="Totale"></asp:Label>
            </div>
            <div class="colOggetti" style="width:20%">
                <asp:TextBox ID="txtTotIspAnnoGrave" CssClass="textbox"   runat="server"></asp:TextBox>
            </div>
            <div class="colOggetti" style="width:5%">
             <asp:ImageButton ID="lTotIspAnnoGrave" AlternateText="Dettaglio Totale" ToolTip="Dettaglio" runat="server" ImageUrl="Images/lenteIngrandimento_small.png" style="width: 30px" />
            </div>
       </div>
        <div class="row" >
            <div class="collable" style="width:25%" >
                <asp:Label ID="labelSazioniAnno" AssociatedControlID="lblSazioniAnno" CssClass="label"  runat="server" Text="Sanzioni gravi anno"></asp:Label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:Label ID="lblSazioniAnno" CssClass="labelDati"   runat="server"></asp:Label>
            </div>
            <div class="collable" style="width:25%" >
                <asp:Label ID="labeltxtTotSanzioneAnnoGRAVE" AssociatedControlID="txtTotSanzioneAnnoGRAVE"  runat="server" Text="Totale"></asp:Label>
            </div>
            <div class="colOggetti" style="width:20%">
                <asp:TextBox ID="txtTotSanzioneAnnoGRAVE" CssClass="textbox"   runat="server"></asp:TextBox>
            </div>
            <div class="colOggetti" style="width:5%">
             <asp:ImageButton ID="lTotSanzioneAnnoGR" AlternateText="Dettaglio Totale Sanzioni" ToolTip="Dettaglio Sanzioni" runat="server" ImageUrl="Images/lenteIngrandimento_small.png" />
            </div>
       </div>
            <div class="row" >
              <div class="collable" style="width:25%" >
                <asp:Label ID="labelValoreGravePerc" AssociatedControlID="lblValoreGravePerc"  runat="server" Text="Valore % Deflettore"></asp:Label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:TextBox ID="lblValoreGravePerc" CssClass="textbox"   runat="server"></asp:TextBox>
            </div>
            <div class="collable" style="width:25%" >
                <asp:Label ID="labelValoreGraveAnno" AssociatedControlID="lblValoreGraveAnno"  runat="server" Text="Valore Deflettore"></asp:Label>
            </div>
            <div class="colOggetti" style="width:20%">
                <asp:TextBox ID="lblValoreGraveAnno" CssClass="textbox"   runat="server"></asp:TextBox>
            </div>

       </div>
    </div>
    </fieldset>
    <div class="row" >
            <div class="collable" style="width:50%" >&nbsp;
            </div>

            <div class="collable" style="width:25%" >
                <asp:Label ID="labeltxtTotDeffSanzioni" AssociatedControlID="txtTotDeffSanzioni"  runat="server" Text="Totale Deflettore Sanzioni"></asp:Label>
            </div>
            <div class="colOggetti" style="width:20%">
                <asp:TextBox ID="txtTotDeffSanzioni" CssClass="textbox"   runat="server"></asp:TextBox>
            </div>
       </div>
    <div class="RigaPulsanti">

            <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" OnClientClick="javascript:window.close();" />
            <br />
        </div>
    </div>
    </fieldset>
    </form>
</body>
</html>
