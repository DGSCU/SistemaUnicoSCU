<%@ Page Title="Inserimento Nuova Figura di Progetto" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="inserimentorisorseprogetti.aspx.vb" Inherits="Futuro.inserimentorisorseprogetti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css"/>
        <script src="Scripts/JHelper.js" type="text/javascript"></script>
    <title>Inserimento Nuova Figura di Progetto</title>

      <style type="text/css">
            .ui-datepicker {
                font-size: 11px;
            }
        </style>

            <script  type="text/javascript">
                $(function () {
                    var IdData = CostruisciId("txtDataNascita");
                    var sharpIdData = "#" + IdData
                    $("" + sharpIdData + "").datepicker();
                });
				
           </script>

            <script  type="text/javascript">
                function ValidaCampi() {
                var idErrore = CostruisciId("lblErrore");
                    AggiornaErrore("", idErrore);
                    VerificaCampoObbligatorio(CostruisciId("txtNome"), "Nome", idErrore);
                    VerificaCampoObbligatorio(CostruisciId("txtCognome"), "Cognome", idErrore);
                    VerificaCampoObbligatorio(CostruisciId("txtCodiceFiscale"), "Codice Fiscale", idErrore);
                    VerificaCampoObbligatorio(CostruisciId("txtDataNascita"), "Data di Nascita", idErrore);
                    VerificaDataValida(document.getElementById(CostruisciId("txtDataNascita")).value, "Data di Nascita", idErrore);
                    if (document.getElementById(CostruisciId("ddlProvinciaNascita")).value == 0) {
                        ConcatenaErrore("Il campo 'Provincia/Nazione di Nascita' è obbligatorio ", idErrore);
                    }
                    if (document.getElementById(CostruisciId("ddlComuneNascita")).value == 'Selezionare Provincia/Nazione di Nascita') {
                        ConcatenaErrore("Il campo 'Comune di nascita' è obbligatorio ", idErrore);
                    }
                    var risultato = $("#"+idErrore).html()
                    if (risultato == '' || risultato == null) {
                        return true;
                    } else {
                        document.getElementById(idErrore).scrollIntoView();
                        return false;
                    }
                };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <fieldset class="fieldsetrasparente">
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:Label ID="lblTitolo" runat="server"  Text="Inserimento Risorsa"></asp:Label>
      </h2>
    </div>
    <div class="rowGroup" style="height:auto">
    <div class="RigaVuota">&nbsp;</div>
       <div class="row" >
       <asp:Label ID="lblErrore"  runat="server" CssClass="msgErrore" ></asp:Label>
       <asp:HiddenField ID="AppoID" runat="server" />
       <asp:HiddenField ID="txtIDComuneNascita" runat="server" />
        <asp:HiddenField ID="chkKeyLenComune" runat="server" />
         <asp:HiddenField ID="chkKeyPressNascita" runat="server" />
         <asp:HiddenField ID="txtCAP" runat="server" />
       </div>
        <div class="RigaVuota">&nbsp;</div>
       </div>
          <div class="row">
            <div class="collable" style="width:15%">
                <asp:Label ID="LblCognome" CssClass="label" runat="server" AssociatedControlID="txtCognome" Text="Cognome"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtCognome" CssClass="textbox" autofocus="true"  runat="server" MaxLength="100" ></asp:TextBox>
            </div>           
                       <div class="collable" style="width:15%">
                <asp:Label ID="LblNome"  runat="server" CssClass="label" Text="Nome" AssociatedControlID="txtNome"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtNome" CssClass="textbox" runat="server" MaxLength="100"></asp:TextBox>
            </div>
        </div>
        <div class="row">
           <div class="collable" style="width:15%">

                <asp:Label ID="LblCodFisc" CssClass="label" runat="server" Text="Codice Fiscale" AssociatedControlID="txtCodiceFiscale"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtCodiceFiscale" CssClass="textbox" runat="server" Style="text-transform: uppercase" MaxLength="16"  ></asp:TextBox>
            </div>
                  <div class="collable" style="width:15%">
                <asp:Label ID="LblDataNasc" CssClass="label" runat="server" Text="Data di Nascita" AssociatedControlID="txtDataNascita"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtDataNascita" CssClass="textboxData" runat="server" MaxLength="10"></asp:TextBox>
            </div>
        </div>
               <div class="row">

            <div class="collable" style="width:15%">
                <asp:Label ID="LblProvinciaNascita" CssClass="label" runat="server" Text="Provincia / Nazione di Nascita" AssociatedControlID="ddlProvinciaNascita"></asp:Label>
            </div>
              <div class="colOggetti" style="width:35%">
                <asp:dropdownlist id="ddlProvinciaNascita" CssClass="ddlClass" AutoPostBack="true" runat="server"  ></asp:dropdownlist>
            </div>
            <div class="collable" style="width:50%">
                 <asp:CheckBox ID="chkEsteroNascita" AutoPostBack="true" Text="Estero" ToolTip="Flag Estero Nazione di Nascita" runat="server" />
                  
            </div>
  
            
        </div>
        <div class="row">
                  <div class="collable" style="width:15%">
                <asp:Label ID="LblComuneNascita" CssClass="label" runat="server" Text="Comune di Nascita" AssociatedControlID="ddlComuneNascita"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:dropdownlist id="ddlComuneNascita" CssClass="ddlClass" AutoPostBack="true" runat="server" Enabled="false" >
                <asp:ListItem Text="Selezionare Provincia/Nazione di Nascita"></asp:ListItem>
                </asp:dropdownlist>
            </div>
                 <div class="collable" style="width:50%">
                 <asp:CheckBox ID="chkCorsoOlp" AutoPostBack="true" Text="Corso OLP da frequentare s/n" visible="false" runat="server" />
                  
            </div>
        </div>
   <div class="RigaPulsanti">
            <asp:Button ID="cmdConferma" CssClass="Pulsante" runat="server" ToolTip="Conferma" Text="Conferma" OnClientClick ="javascript:return ValidaCampi();" />
            <asp:Button ID="imgChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
       </div>
       
    </div>
    </fieldset>
</asp:Content>
