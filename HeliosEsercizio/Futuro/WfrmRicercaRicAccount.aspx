<%@ Page Title="Ricerca Richieste Account" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaRicAccount.aspx.vb" Inherits="Futuro.WfrmRicercaRicAccount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <script src="Scripts/JHelper.js" type="text/javascript"></script>

    <style type="text/css"> .ui-datepicker {font-size: 11px;}</style>

    <script  type="text/javascript">
//<![CDATA[
        var prefissoIdClient = "MainContent_";

        function CostruisciId(IdServer) {
            var IdClient = prefissoIdClient + IdServer;
            return IdClient;
        };

        $(function () {
            var DataRichiestaDal = CostruisciId("txtDataDal");
            var readonly = '<%=txtDataDal.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataRichiestaDal = "#" + DataRichiestaDal
                $("" + sharpDataRichiestaDal + "").datepicker();
            }
        });

        $(function () {
            var DataRichiestaAl = CostruisciId("txtdataa");
            var readonly = '<%=txtdataa.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataRichiestaAl = "#" + DataRichiestaAl
                $("" + sharpDataRichiestaAl + "").datepicker();
            }
        });


        function ValidazioneClientRicerca() {

            var idRichiestaDal = CostruisciId("txtDataDal");
            var txtRichiestaDal = document.getElementById(idRichiestaDal);
            if (txtRichiestaDal.value != '') {
                if (isCorrectFormatDate(txtRichiestaDal, "Data Richiesta Dal") == false) {
                    return false;
                }
            }

            var idRichiestaAl = CostruisciId("txtdataa");
            var txtRichiestaAl = document.getElementById(idRichiestaAl);
            if (txtRichiestaAl.value != '') {
                if (isCorrectFormatDate(txtRichiestaAl, "Data Richiesta Al") == false) {
                    return false;
                }
            }
            return true;
        }


        function isCorrectFormatDate(txtDataRiferimento, nomeDataRiferimento) {

            //Checks for dd/mm/yyyy format.
            var currVal = txtDataRiferimento.value;
            var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
            var dtArray = currVal.match(rxDatePattern); // is format OK?
            if (dtArray == null) {
                alert("La '" + nomeDataRiferimento + "' non è valida. Inserire la data nel formato GG/MM/AAAA.")
                txtDataRiferimento.focus();
                return false;
            }

            dtDay = dtArray[1];
            dtMonth = dtArray[3];
            dtYear = dtArray[5];

            if (dtMonth < 1 || dtMonth > 12) {
                alert("La '" + nomeDataRiferimento + "' non è valida. Inserire la data nel formato GG/MM/AAAA.")
                txtDataRiferimento.focus();
                return false;
            }
            else if (dtDay < 1 || dtDay > 31) {
                alert("La '" + nomeDataRiferimento + "' non è valida. Inserire la data nel formato GG/MM/AAAA.")
                txtDataRiferimento.focus();
                return false;
            }
            else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31) {
                alert("La '" + nomeDataRiferimento + "' non è valida. Inserire la data nel formato GG/MM/AAAA.")
                txtDataRiferimento.focus();
                return false;
            }
            else if (dtMonth == 2) {
                var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
                if (dtDay > 29 || (dtDay == 29 && !isleap)) {
                    alert("La '" + nomeDataRiferimento + "' non è valida. Inserire la data nel formato GG/MM/AAAA.")
                    txtDataRiferimento.focus();
                    return false;
                }
            }

            return true;
        }
//]]>
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Ricerca Richieste Account</legend>
        <asp:label id="lblMessaggi"  runat="server" CssClass="msgErrore"></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Richieste Account"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDenominazione" CssClass="label" AssociatedControlID="txtdenominazione" runat="server" Text="Denominazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:85%">       
                    <asp:TextBox ID="txtdenominazione" CssClass="textbox" runat="server" Width="96%"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodiceEnte" CssClass="label" AssociatedControlID="txtCodiceEnte" runat="server" Text="Codice Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodiceEnte" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodFis" CssClass="label" AssociatedControlID="txtCodFis" runat="server" Text="Codice Fiscale"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodFis" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblRichiedente" CssClass="label" AssociatedControlID="txtRichiedente" runat="server" Text="Richiedente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtRichiedente" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblStato" CssClass="label" AssociatedControlID="ddlStato" runat="server" Text="Stato"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlStato" runat="server" CssClass="ddlClass"></asp:DropDownList>            
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDataRichiestaDal" CssClass="label" AssociatedControlID="txtDataDal" runat="server" Text="Data Richiesta Dal"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtDataDal" CssClass="textbox" runat="server"  Width="25%" MaxLength="10"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDataRichiestaAl" CssClass="label" AssociatedControlID="txtdataa" runat="server" Text="Data Richiesta Al"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtdataa" CssClass="textbox" runat="server"  Width="25%" MaxLength="10"></asp:TextBox>         
                </div>
            </div>
             <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCompetenza" CssClass="label" AssociatedControlID="ddlCompetenze" runat="server" Text="Competenza"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlCompetenze" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                </div>
                <div class="colOggetti" style="width:20%">
                    <asp:checkbox id="chkCodiceAttribuito" runat="server" Text="Codice Attribuito"></asp:checkbox>
                 </div>
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" Text="Ricerca" OnClientClick="return ValidazioneClientRicerca()" />
                <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
            </div>
        </div>
    </fieldset>
</asp:Content>
