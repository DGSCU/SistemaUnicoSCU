<%@ Page Title="Gestione Enti in Accordo" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmAnagraficaEnteAccordo.aspx.vb" Inherits="Futuro.WfrmAnagraficaEnteAccordo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <script src="Scripts/JHelper.js" type="text/javascript"></script>

    <style type="text/css"> .ui-datepicker {font-size: 11px;}</style>


    <script  type="text/javascript">

        var prefissoIdClient = "MainContent_";

        function SetContextKey() {
            $find('<%=AutoCompleteExtenderIndirizzo.ClientID%>').set_contextKey($get("<%=ddlComune.ClientID %>").value);
        }

        $(function () {
            var DataCostituzione = CostruisciId("txtDataCostituzione");
            var readonly = '<%=txtDataCostituzione.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataCostituzione = "#" + DataCostituzione
                $("" + sharpDataCostituzione + "").datepicker();
            }
        });

        $(function () {
            var DataStipula = CostruisciId("txtDataStipula");
            var readonly = '<%=txtDataStipula.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataStipula = "#" + DataStipula
                $("" + sharpDataStipula + "").datepicker();
            }
        });

        $(function () {
            var DataScadenza = CostruisciId("txtDataScadenza");
            var readonly = '<%=txtDataScadenza.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataScadenza = "#" + DataScadenza
                $("" + sharpDataScadenza + "").datepicker();
            }
        });

        $(function () {
            var txtDataNominaRL = CostruisciId("txtDataNominaRL");
            var readonly = '<%=txtDataNominaRL.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataAttoNomina = "#" + txtDataNominaRL
                $("" + sharpDataAttoNomina + "").datepicker();
            }
        });
       
	   

        function leggidata() {

            var dataOggi = document.getElementById(CostruisciId("txtdataoggi"));
            var dataStipula = document.getElementById(CostruisciId("txtDataStipula"));
            var dataScadenza = document.getElementById(CostruisciId("txtDataScadenza"));
            var dataCostituzione = document.getElementById(CostruisciId("txtDataCostituzione"));

            var arrDataOggi;
            var arrDataStipula;
            var arrDataScadenza;
            var arrDataCostituzione;

            var dtOggi;
            var dtStipula;
            var dtScadenza;
            var dtCostituzione;

            var timeOggi;
            var timeStilpula;
            var timeScadenza;
            var timeCostituzione;

            arrDataOggi = dataOggi.value.split("/");
            dtOggi = new Date(arrDataOggi[2].split(" ")[0], arrDataOggi[1] - 1, arrDataOggi[0]);
            timeOggi = dtOggi.getTime();

            if (dataStipula.value != '') {
                arrDataStipula = dataStipula.value.split("/");
                dtStipula = new Date(arrDataStipula[2], arrDataStipula[1] - 1, arrDataStipula[0]);
                timeStilpula = dtStipula.getTime();
            }

   
            if (dataScadenza.value != '') {
                arrDataScadenza = dataScadenza.value.split("/")
                dtScadenza = new Date(arrDataScadenza[2], arrDataScadenza[1] - 1, arrDataScadenza[0]);
                timeScadenza = dtScadenza.getTime();   
            }


            if (dataCostituzione.value != '') {
                arrDataCostituzione = dataCostituzione.value.split("/")
                dtCostituzione = new Date(arrDataCostituzione[2], arrDataCostituzione[1] - 1, arrDataCostituzione[0]);
                timeCostituzione = dtCostituzione.getTime();
            }

            if (dataCostituzione.value != '' && dataStipula.value != '') {
                if (timeStilpula < timeCostituzione) {
                    alert("La data di Stipula non puo' essere antecedente " & vbcrlf & " alla data di Costituzione.");
                    dataStipula.focus();
                    return false;
                }
            }

            if (dataStipula.value != '' && dataScadenza.value != '') {
                if (timeStilpula >= timeScadenza) {
                    alert("La data di Scadenza non puo' essere uguale o antecedente " & vbcrlf & " alla data di Stipula.");
                    dataScadenza.focus();
                    return false;
                }
            }

            if (dataStipula.value == '' && dataScadenza.value != '') {
                alert("E' necessario inserire la data di Stipula se si vuole inserire la data di Scadenza.");
                dataStipula.focus();
                return false;
            }

            if (dataScadenza.value != '' && timeScadenza < timeOggi) {
                alert("La data di Scadenza non puo' essere antecedente alla data odierna.");
                dataScadenza.focus()
                return false;
            }

            return true;
        }

        function CodiceFiscaleUpperCase() {
            var idCodiceFiscale = CostruisciId("txtCodFis");
            var txtCodiceFiscale = document.getElementById(idCodiceFiscale);
            txtCodiceFiscale.value = txtCodiceFiscale.value.toUpperCase();
        }

        function HttpLowerCase() {
            var idHttp = CostruisciId("txthttp");
            var txtHttp = document.getElementById(idHttp);
            txtHttp.value = txtHttp.value.toLowerCase();
        }

        function isCorrectFormatDate(dataRiferimento, nomeDataRiferimento) {
            var idDataRiferimento = CostruisciId(dataRiferimento);
            var txtDataRiferimento = document.getElementById(idDataRiferimento);

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

        function ControllaCkeck(chkBox) {
            if (!$(chkBox).find('input').prop('checked')) {

                if (!window.confirm("Attenzione! Verranno cancellate tutte le esperienze del settore scelto.\r\n Si vuole procedere?\r\n\ In caso di cancellazione seguita da un salvataggio del settore tutte le esperienze andranno reinserite")) {
                    $(chkBox).find('input').prop('checked', true);
                    return false;
                }

            }
            __doPostBack($(chkBox).find('input').prop('id'), 'OnCheckedChanged');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <fieldset class="ContornoPagina">
        <legend>Gestione Enti in Accordo</legend>
        <p style="text-align:right"><asp:ImageButton id="CmdInforScu1" runat="server" ImageUrl="Images/vincoli_small.png" ToolTip="Analisi Codice Fiscale" AlternateText="Analisi Codice Fiscale" ImageAlign="Middle"/></p>
        <asp:label id="msgConferma"  runat="server" CssClass="msgConferma"></asp:label><br />
        <asp:label id="msgErrore"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
        <br />
        <asp:Label ID="lblMessaggioArt2Art10" runat="server" style='color:red' Visible="false"></asp:Label>
        <br />
         <asp:Button ID="cmdModNoAdeguamento" runat="server" CssClass="Pulsante" Visible="false" Text="Modifica dei dati che non richiedono adeguamento" /> 
        <br />
        <br />
        <div id="divChiusuraEnte" class="rowGroup" style="height:auto" runat="server" visible="false">
                <fieldset class="ContornoPagina">
                <legend>Chiudi Ente</legend>
                    <div class="wrapper" style="width:100%;border:0px">
                        <div class="row" style="height:auto">
                            <h3><asp:Label runat="server" ID="lblChiudiEnte" CssClass="labelDati"></asp:Label></h3>
                        </div>
                        <br />
                        <div class="RigaPulsanti" style="height:auto;text-align:center">
                            <asp:Button ID="cmdConfermaChiusura" CssClass="Pulsante" runat="server" ToolTip="Conferma di chiusura Ente" Text="Conferma" />
                            <asp:Button ID="cmdAnnulla" CssClass="Pulsante" runat="server" ToolTip="Annulla richiesta chiusura" Text="Annulla" />
                        </div>
                    </div>
                </fieldset>
            </div>
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Gestione Enti in Accordo"></asp:Label></h2>
            </div>
            <asp:PlaceHolder ID="phStato" runat="server">
                <div class="RigaVuota" >&nbsp;</div>
                <div class="row">
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblaccordo" CssClass="label" AssociatedControlID="lblStatoAccordo" runat="server" Text="Stato Accordo"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:label id="lblStatoAccordo" CssClass="bold" runat="server"></asp:label>         
                    </div>
                     <div class="collable" style="width:15%">      
                        <asp:Label ID="Label1" CssClass="label" AssociatedControlID="lblStato" runat="server" Text="Stato Ente"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:label id="lblStato" CssClass="bold" runat="server"></asp:label>         
                    </div>
                </div>
               
                <div class="row"  id ="divRiserva" runat="server" visible="false">
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="LblRiserva" CssClass="label" AssociatedControlID="LblDataRiserva" runat="server" Text="Data Indicazione Riserva"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:label id="LblDataRiserva" CssClass="bold" runat="server" >&nbsp;</asp:label>         
                    </div>
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="LblNoRiserva" CssClass="label" AssociatedControlID="LblDataNORiserva" runat="server" Text="Data Rimozione Riserva"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:label id="LblDataNORiserva" CssClass="bold" runat="server">&nbsp;</asp:label>         
                    </div>
                </div>

                <div class="row" >
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblClasseAccreditamento" CssClass="label" AssociatedControlID="lblClasse" runat="server" Text="Classe/Sezione Accreditamento" Visible="false"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:label id="lblClasse" CssClass="bold" runat="server" Visible="false"></asp:label>         
                    </div>

                </div>
                <div class="row" id="DIVdataaccr" runat="server" visible="true">
                      <div class="collable" style="width:15%">
                      <asp:Label ID="lblTipoFase" CssClass="label" AssociatedControlID="lblFaseTesto" runat="server" Text="Tipo Fase Iscrizione"></asp:Label>   
                     </div>
                     <div class="colOggetti" style="width:18%">
                      <asp:Label ID="lblFaseTesto" CssClass="bold" AssociatedControlID="lblTipoFase" runat="server" Text=""></asp:Label> 
                     </div>
                    <div class="collable" style="width:18%">
                        <asp:Label ID="lblDataRichiestaAccredito" CssClass="label" AssociatedControlID="lblDataAccreditamento" runat="server" Text="Data Valutazione Ente"></asp:Label>   
                    </div> 
                    <div class="colOggetti" style="width:15%">       
                        <asp:label id="lblDataAccreditamento" CssClass="bold" AssociatedControlID="lblDataRichiestaAccredito" Text="" runat="server"></asp:label>         
                    </div>
                     
                     <div class="collable" style="width:15%">
                     <asp:Label ID="lblDataValFase" CssClass="label" AssociatedControlID="lblDataFase" runat="server" Text="Data Valutazione Fase"></asp:Label> 
                     </div>
                     <div class="colOggetti" style="width:18%"> 
                      <asp:Label ID="lblDataFase" CssClass="bold" AssociatedControlID="lblDataValFase" runat="server" Text=""></asp:Label>
                     </div>
                </div>

            </asp:PlaceHolder>
        </div>
        <div class="wrapper" style="width:100%">
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblEnte" CssClass="label" AssociatedControlID="txtdenominazione" runat="server" Text="(*)Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:textbox id="txtdenominazione" runat="server" CssClass="textbox"></asp:textbox>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:label id="lblCodiceEnte" CssClass="label" runat="server" AssociatedControlID="txtCodRegione" Text="Cod. Ente"></asp:label>
                </div>
                <div class="colOggetti" style="width:35%">
                   <asp:textbox id="txtCodRegione" runat="server" CssClass="textbox" Enabled="false"></asp:textbox>
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">
                    <asp:label id="lblCodiceFiscale" CssClass="label" runat="server" AssociatedControlID="txtCodFis" Text="(*)Cod. Fis."></asp:label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:textbox id="txtCodFis" runat="server" CssClass="textbox" Onchange="CodiceFiscaleUpperCase()"></asp:textbox>
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label2" CssClass="label" AssociatedControlID="ddlTipologia" runat="server"
                        Text="(*)Tipo"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:DropDownList ID="ddlTipologia" runat="server" CssClass="ddlClass" Width="100px" AutoPostBack="true"></asp:DropDownList>
                </div>
                <div class="collable" style="width: 15%">
                    <asp:Label  ID="lblDDlGiuridica" 
                                CssClass="label"
                                AssociatedControlID="ddlGiuridica" 
                                runat="server"
                                Text="(*)Denominazione tipologia"
                                Visible="false"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:DropDownList ID="ddlGiuridica" runat="server" CssClass="ddlClass" Width="60%" Visible="false" AutoPostBack="true"></asp:DropDownList>
                </div>
            </div>
            <div class="row" id="divAltraTipologia" runat="server" visible="false">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblAltroTipoEnte" CssClass="label" AssociatedControlID="txtAltraTipoEnte"
                        runat="server" Text="(*)Altra tipologia di ente" ></asp:Label>
                </div>
                <div class="colOggetti" style="width: 40%">
                    <asp:TextBox runat="server" ID="txtAltraTipoEnte" MaxLength ="50" Width="100%" />
                </div>
            </div>

            <div class="row" id="dvCodiceFiscaleArchiviato" runat="server" visible="false">
                <div class="collable" style="width:15%">
                    <asp:label id="lblCodiceFiscaleArchiviato" CssClass="label" runat="server" AssociatedControlID="txtCodFisArchivio" Text="Cod. Fis. Archiviato" ></asp:label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:textbox id="txtCodFisArchivio" runat="server" CssClass="textbox"></asp:textbox>
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">
                    <asp:label id="lblHttp" CssClass="label" runat="server" AssociatedControlID="txthttp" Text="Http"></asp:label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:textbox id="txthttp" runat="server" CssClass="textbox" Onchange="HttpLowerCase()"></asp:textbox>
                </div>
                <div class="collable" style="width:15%">
                    <asp:label id="lblDataCostituzione" CssClass="label" runat="server" AssociatedControlID="txtDataCostituzione" Text="Data Costituzione"></asp:label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtDataCostituzione" CssClass="textbox" runat="server" Width="75px" MaxLength="10"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">
                    <asp:label id="lblEmail" CssClass="label" runat="server" AssociatedControlID="txtEmail" Text="E-mail"></asp:label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:textbox id="txtEmail" runat="server" CssClass="textbox"></asp:textbox>
                </div>
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblPEC" CssClass="label" AssociatedControlID="txtEmailpec" runat="server"
                        Text="PEC"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:TextBox ID="txtEmailpec" CssClass="textbox" runat="server" Width="70%"/>
                    <asp:ImageButton ID="imgStatoPec" runat="server" ImageUrl="Images/valida_small.png"
                        Style="width: 20px; height: 20px; vertical-align: middle" Enabled="false" />
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:label id="lblTipoRelazione" CssClass="label" runat="server" AssociatedControlID="ddlrelazione" Text="(*)Tipo Relazione"></asp:label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:dropdownlist id="ddlrelazione" runat="server" CssClass="ddlClass" AutoPostBack="true"/>
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">
                <asp:label id="lblPrefisso" CssClass="label" runat="server" ToolTip="Prefisso" AssociatedControlID="txtprefisso" Text="(*)Pref."></asp:label>
                   <asp:textbox id="txtprefisso" runat="server" CssClass="textbox" Width="50%"></asp:textbox>
                </div>
                <div class="colOggetti" style="width:35%">
                     <asp:label id="lblTelefono" CssClass="label" runat="server" ToolTip="Telefono" AssociatedControlID="txtTelefono" Text="(*)Tel."></asp:label>
                    <asp:textbox id="txtTelefono" runat="server" CssClass="textbox" Width="70%"></asp:textbox>
                </div>
                <div class="collable" style="width:15%">
                   <asp:label id="lblPrefissoFax" CssClass="label" ToolTip="Prefisso Fax" runat="server" AssociatedControlID="txtPrefissoFax"  Text="Pref."></asp:label>
                   <asp:textbox id="txtPrefissoFax" runat="server" CssClass="textbox" Width="50%"></asp:textbox> 
                </div>
                <div class="colOggetti" style="width:35%">
                     <asp:label id="lblFax" CssClass="label" runat="server" AssociatedControlID="txtFax"  Text="Fax"></asp:label>   
                    <asp:textbox id="txtFax" runat="server" CssClass="textbox" Width="70%"></asp:textbox>         
               </div>
            </div>
             <div class="row" >
                <div class="collable" style="width:15%">
                    <asp:label id="lblDataStipula" CssClass="label" runat="server" AssociatedControlID="txtDataStipula" Text="Data Stipula"></asp:label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:textbox id="txtDataStipula" runat="server" CssClass="textbox" Width="75px" MaxLength="10"></asp:textbox>
                </div>
                <div class="collable" style="width:15%">
                    <asp:label id="lblDataScadenza" CssClass="label" runat="server" AssociatedControlID="txtDataScadenza" Text="Data Scadenza"></asp:label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:textbox id="txtDataScadenza" runat="server" CssClass="textbox" Width="75px" MaxLength="10"></asp:textbox>  
                </div>
            </div>
        </div>
        <br />
        <div class="wrapper" style="width:100%">
        <div class="RigaVuota" >&nbsp;</div>
        <p>
        <asp:label id="messaggioNotificaRL"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
        </p>
            <p style="text-align: center">
                <asp:Label ID="Label3" runat="server" CssClass="bold" Text="(*)Dati Rappresentante Legale"></asp:Label>
            </p>
            <div class="row" >
                <div class="collable" style="width:25%">
                    <asp:label id="lblCodFiscRL" CssClass="label" runat="server" AssociatedControlID="txtCodFiscRL" Text="(*)Codice Fiscale Rappresentante Legale"></asp:label>
                </div>
                <div class="colOggetti" style="width:25%">
                    <asp:textbox id="txtCodFiscRL" runat="server" CssClass="textbox" Onchange="CodiceFiscaleUpperCase()"></asp:textbox>
                </div>
            </div>
            <div class="row">
                <div class="colOggetti" style="width: 25%">
                    <asp:Label runat="server" ID="lblDataNominaRL" Text="(*)Data di Nomina del Rappresentante Legale"
                        AssociatedControlID="txtDataNominaRL" />
                </div>
                <div class="colOggetti" style="width: 25%">
                    <asp:TextBox runat="server" ID="txtDataNominaRL" Text="" Width="50%" />
                </div>
            </div>
        </div>
         <div class="RigaVuota">
                &nbsp;</div>
        <div class="wrapper" style="width: 100%">
             <asp:HiddenField ID="HFAttoCostitutivo" runat="server" />
             <asp:HiddenField ID="HFDeliberaEnte" runat="server" />
             <asp:HiddenField ID="HFStatuto" runat="server" />
             <asp:HiddenField ID="HFDelibera" runat="server" />

             <p style="text-align: center">
                <asp:Label ID="Label5" runat="server" CssClass="bold" Text="(*)Documenti costitutivi ente"></asp:Label>
            </p>
            <div class="row" runat="server" id="rowNoAttoCostitutivo">
                <div class="collable" style="width: 35%">
                    <asp:Label ID="lblAttoCostitutivoEnte" CssClass="label" AssociatedControlID="fileAttoCostitutivo" runat="server"
                        Text="Atto Costitutivo dell'ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 10%">
                    <asp:Button ID="cmdAllegaAttoCostitutivo" runat="server"
                        CssClass="Pulsante" Text="Carica Atto Costitutivo" />
                </div>
            </div>
            <div class="row" runat="server" id="rowAttoCostitutivo" visible="false">
                <table class="table" cellspacing="0" cellpadding="2" rules="all" border="1" id="Table2"
                    style="font-size: Small; width: 100%; border-collapse: collapse;">
                    <caption>
                        Atto Costitutivo dell'ente
                    </caption>
                    <tbody>
                        <tr>
                            <th scope="col">
                                Download
                            </th>
                            <th scope="col">
                                Nome File
                            </th>
                            <th scope="col">
                                Data Inserimento
                            </th>
                            <th scope="col">
                                Hash Value
                            </th>
                            <th scope="col">
                            </th>
                        </tr>
                        <tr class="tr" align="center">
                            <td>
                                <asp:ImageButton runat="server" ID="btnDownloadAttoCostitutivo" ImageUrl="images/giu_small.png"
                                    AlternateText="Scarica Atto Costitutivo dell'ente" ToolTip="Scarica Atto Costitutivo dell'ente" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtACFilename"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtACData"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtACHash"></asp:Literal>
                            </td>
                            <td>
                                <asp:ImageButton runat="server" ID="btnModificaAttoCostitutivo"
                                    ImageUrl="images/ripristina_small.png" AlternateText="Modifica Atto Costitutivo dell'ente"
                                    ToolTip="Modifica Atto Costitutivo dell'ente" />
                                <asp:ImageButton runat="server" ID="btnEliminaAttoCostitutitvo" ImageUrl="images/canc_small.png"
                                    AlternateText="Elimina Atto Costitutivo dell'ente" ToolTip="Elimina Atto Costitutivo dell'ente" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="RigaVuota">&nbsp;</div>
            <div class="row" id="rowNoStatuto" runat="server">
                  <div class="collable" style="width: 35%">
                    <asp:Label ID="lblStatutoEnte" CssClass="label" AssociatedControlID="fileStatuto" runat="server"
                        Text="Statuto dell'ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 10%">
                    <asp:Button ID="cmdAllegaStatuto" runat="server"
                        CssClass="Pulsante" Text="Carica Statuto" />
                </div>
            </div>
            <div class="row" runat="server" id="rowStatuto" visible="false">
                <table class="table" cellspacing="0" cellpadding="2" rules="all" border="1" id="Table3"
                    style="font-size: Small; width: 100%; border-collapse: collapse;">
                    <caption>
                        Statuto dell'ente
                    </caption>
                    <tbody>
                        <tr>
                            <th scope="col">
                                Download
                            </th>
                            <th scope="col">
                                Nome File
                            </th>
                            <th scope="col">
                                Data Inserimento
                            </th>
                            <th scope="col">
                                Hash Value
                            </th>
                            <th scope="col">
                            </th>
                        </tr>
                        <tr class="tr" align="center">
                            <td>
                                <asp:ImageButton runat="server" ID="btnDownloadStatuto" ImageUrl="images/giu_small.png"
                                    AlternateText="Scarica Statuto dell'ente" ToolTip="Scarica Statuto dell'ente" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtStatutoFilename"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtStatutoData"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtStatutoHash"></asp:Literal>
                            </td>
                            <td>
                                <asp:ImageButton runat="server" ID="btnModificaStatuto"
                                    ImageUrl="images/ripristina_small.png" AlternateText="Modifica Statuto dell'ente"
                                    ToolTip="Modifica Statuto dell'ente" />
                                <asp:ImageButton runat="server" ID="btnEliminaStatuto" ImageUrl="images/canc_small.png"
                                    AlternateText="Elimina Statuto dell'ente" ToolTip="Elimina Statuto dell'ente" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="RigaVuota">&nbsp;</div>
            <div class="row" id="rowNoDeliberaAdesione" runat="server">
                <div class="collable" style="width: 35%">
                <asp:Label ID="lblDeliberaAdesione" CssClass="label" AssociatedControlID="fileDeliberaAdesione" runat="server"
                    Text="Delibera"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 10%">
                    <asp:Button ID="cmdAllegaDeliberaAdesione" runat="server"
                        CssClass="Pulsante" Text="Carica Delibera" />
                </div>
            </div>
             <div class="row" runat="server" id="rowDeliberaAdesione" visible="false">
                <table class="table" cellspacing="0" cellpadding="2" rules="all" border="1" id="Table4"
                    style="font-size: Small; width: 100%; border-collapse: collapse;">
                    <caption>
                        Delibera
                    </caption>
                    <tbody>
                        <tr>
                            <th scope="col">
                                Download
                            </th>
                            <th scope="col">
                                Nome File
                            </th>
                            <th scope="col">
                                Data Inserimento
                            </th>
                            <th scope="col">
                                Hash Value
                            </th>
                            <th scope="col">
                            </th>
                        </tr>
                        <tr class="tr" align="center">
                            <td>
                                <asp:ImageButton runat="server" ID="btnDownloadDeliberaAdesione" ImageUrl="images/giu_small.png"
                                    AlternateText="Scarica Delibera" ToolTip="Scarica Delibera" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtDeliberaAdesioneFileName"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtDeliberaAdesioneData"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtDeliberaAdesioneHash"></asp:Literal>
                            </td>
                            <td>
                                <asp:ImageButton runat="server" ID="btnModificaDeliberaAdesione"
                                    ImageUrl="images/ripristina_small.png" AlternateText="Modifica Delibera "
                                    ToolTip="Modifica Delibera" />
                                <asp:ImageButton runat="server" ID="btnEliminaDeliberaAdesione" ImageUrl="images/canc_small.png"
                                    AlternateText="Elimina Delibera " ToolTip="Elimina Delibera " />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
           <div class="RigaVuota">
                &nbsp;</div>
        <div class="wrapper" style="width: 100%">
            <asp:HiddenField ID="HFImpegnoEtico" runat="server" />
            <p style="text-align: center">
                <asp:Label ID="Label7" runat="server" CssClass="bold" Text="(*)Carta Impegno Etico"></asp:Label>
            </p>
            <div class="row" runat="server" id="rowNoImpegnoEtico">
                <div class="collable" style="width: 35%">
                    <asp:Label ID="Label10" CssClass="label" AssociatedControlID="fileImpegnoEtico" runat="server"
                        Text="Carta Impegno Etico"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 10%">
                    <asp:Button ID="cmdAllegaImpegnoEtico" runat="server"
                        CssClass="Pulsante" Text="Carica Impegno Etico" />
                </div>
                <div class="collable" style="width: 100%">
                   <br />  
                   <p>Scarica <asp:HyperLink ID="hplScaricaCartaImpegnoEtico" 
                    NavigateUrl="https://www.serviziocivile.gov.it/menusx/servizio-civile-nazionale/carta-etica.aspx" 
                    Text="Qui" runat="server" Target="_blank" ToolTip="Carta di Impegno Etico">
                    </asp:HyperLink> il modello della Carta di Impegno Etico da firmare&nbsp;&nbsp;&nbsp;</p>
                </div>
            </div>
            <div class="row" runat="server" id="rowImpegnoEtico" visible="false">
                <table class="table" cellspacing="0" cellpadding="2" rules="all" border="1" id="Table5"
                    style="font-size: Small; width: 100%; border-collapse: collapse;">
                    <caption>
                        Carta d'Impegno Etico
                    </caption>
                    <tbody>
                        <tr>
                            <th scope="col">
                                Download
                            </th>
                            <th scope="col">
                                Nome File
                            </th>
                            <th scope="col">
                                Data Inserimento
                            </th>
                            <th scope="col">
                                Hash Value
                            </th>
                            <th scope="col">
                            </th>
                        </tr>
                        <tr class="tr" align="center">
                            <td>
                                <asp:ImageButton runat="server" ID="btnDownloadCIE" ImageUrl="images/giu_small.png"
                                    AlternateText="Scarica Carta Impegno Etico" ToolTip="Scarica Carta Impegno Etico" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtCIEFilename"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtCIEData"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtCIEHash"></asp:Literal>
                            </td>
                            <td>
                                <asp:ImageButton runat="server" ID="btnModificaCIE"
                                    ImageUrl="images/ripristina_small.png" AlternateText="Modifica Carta Impegno Etico"
                                    ToolTip="Modifica Carta Impegno Etico" />
                                <asp:ImageButton runat="server" ID="btnEliminaCIE" ImageUrl="images/canc_small.png"
                                    AlternateText="Modifica Carta Impegno Etico" ToolTip="Modifica Carta Impegno Etico" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <br />
           <div class="RigaVuota">
                &nbsp;</div>
        <div class="wrapper" style="width: 100%">
            <asp:HiddenField ID="HFImpegno" runat="server" />
            <p style="text-align: center">
                <asp:Label ID="Label6" runat="server" CssClass="bold" Text="Dichiarazione di impegno"></asp:Label>
            </p>
             <div class="row" runat="server" id="rowNoCartaImpegno">
                <div class="collable" style="width: 35%">
                    <asp:Label ID="Label8" CssClass="label" AssociatedControlID="fileImpegno" runat="server"
                        Text="Dichiarazione di Impegno"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 10%">
                    <asp:Button ID="cmdAllegaImpegno" runat="server"
                        CssClass="Pulsante" Text="Carica dichiarazione di Impegno" />
                </div>
                <div class="collable" style="width: 100%">
                   <br />  
                   <p>Scarica <asp:HyperLink ID="hplScaricaDichiarazioneImpegno" 
                    NavigateUrl="https://www.serviziocivile.gov.it/modulistica-unsc/modulistica-enti/moduli-iscrizione-albo-servizio-civile-universale/dichiarazione-di-impegno.aspx" 
                    Text="Qui" runat="server" Target="_blank" ToolTip="Dichiarazione di Impegno">
                    </asp:HyperLink> il modello della Dichiarazione di Impegno da firmare&nbsp;&nbsp;&nbsp;</p>
                </div>
            </div>
            <div class="row" runat="server" id="rowCartaImpegno" visible="false">
                <table class="table" cellspacing="0" cellpadding="2" rules="all" border="1" id="Table1"
                    style="font-size: Small; width: 100%; border-collapse: collapse;">
                    <caption>
                        Dichiarazione di Impegno 
                    </caption>
                    <tbody>
                        <tr>
                            <th scope="col">
                                Download
                            </th>
                            <th scope="col">
                                Nome File
                            </th>
                            <th scope="col">
                                Data Inserimento
                            </th>
                            <th scope="col">
                                Hash Value
                            </th>
                            <th scope="col">
                            </th>
                        </tr>
                        <tr class="tr" align="center">
                            <td>
                                <asp:ImageButton runat="server" ID="btnDownloadCI" ImageUrl="images/giu_small.png"
                                    AlternateText="Scarica Dichiarazione di Impegno " ToolTip="Scarica Dichiarazione di Impegno " />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtCIFilename"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtCIData"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtCIHash"></asp:Literal>
                            </td>
                            <td>
                                <asp:ImageButton runat="server" ID="btnModificaCI"
                                    ImageUrl="images/ripristina_small.png" AlternateText="Modifica Dichiarazione di Impegno "
                                    ToolTip="Modifica Dichiarazione di Impegno " />

                                <asp:ImageButton runat="server" ID="btnEliminaCI" ImageUrl="images/canc_small.png"
                                    AlternateText="Elimina Dichiarazione di Impegno" ToolTip="Elimina Dichiarazione di Impegno" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <asp:GridView ID="dtgSettori" runat="server" CssClass="table" Width="100%" ToolTip="Elenco Settori"
                CellPadding="2" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True"
                DataKeyNames="IDMacroAmbitoAttivit&#224;" OnRowCommand="dtgSettori_RowCommand">
                <SelectedRowStyle BackColor="White"></SelectedRowStyle>
                <EditRowStyle></EditRowStyle>
                <AlternatingRowStyle CssClass="tr"></AlternatingRowStyle>
                <RowStyle CssClass="tr" HorizontalAlign="Center"></RowStyle>
                <HeaderStyle></HeaderStyle>
                <Columns>
                    <asp:ButtonField Text="" Visible="false"></asp:ButtonField>
                    <%--<asp:BoundField  DataField="IDMacroAmbitoAttivit&#224;" HeaderText="Settori di Intervento"/>--%>
                    <asp:TemplateField >
                        <ItemTemplate>
                            <asp:HiddenField runat="server" ID="HFIdMacroAttivita" Value ='<%# Eval("IDMacroAmbitoAttivit&#224;") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Settori di Intervento" ItemStyle-HorizontalAlign="Left" >
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblSettoriIntervento" Text='<%# Eval("MacroAmbitoAttivit&#224;") %>' />
                            <asp:Button ID="btnAbArt2Art10" runat="server" Text="Abilita Mod. Art.2/Art.10" style="float: right;" visible="false" CommandName="AbModArt2Art10"/>
                            <asp:Button ID="btnDisabArt2Art10" runat="server" Text="Disabilita Mod. Art.2/Art.10" style="float: right;" visible="false" CommandName="DisabModArt2Art10"/>
                            <br />
                            <br />
                            <div runat="server" id="divAreaIntervento" style="display: none">
                                <table>
                                    <tr>
                                        <td style="">
                                            Anno Esperienza:
                                        </td>
                                        <td>
                                            <asp:Label runat="server" CssClass="label" ID="lblAnnoEsperienza3" Text="Non impostato" />
                                            <asp:Button ID="btnCron3" runat="server" Text="Mostra cronologia" style="float: right;" visible="false" CommandName="MostraCronologia3"/>
                                            <asp:Button ID="btnEsp3" runat="server" Text="Mostra variazioni" style="float: right;" visible="false" CommandName="MostraVariazioni3"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Descrizione Esperienza:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDesEsperienza3" TextMode="MultiLine" Columns="100"
                                                Rows="3" Style="resize: none" Enabled="false" ></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Anno Esperienza:
                                        </td>
                                        <td>
                                            <asp:Label runat="server" CssClass="label" ID="lblAnnoEsperienza2" Text="Non impostato" />
                                            <asp:Button ID="btnCron2" runat="server" Text="Mostra cronologia" style="float: right;" visible="false" CommandName="MostraCronologia2"/>
                                            <asp:Button ID="btnEsp2" runat="server" Text="Mostra variazioni" style="float: right;" visible="false" CommandName="MostraVariazioni2"/>
                                        </td>
                                    </tr>
                                    <tr class="row">
                                        <td>
                                            Descrizione Esperienza:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDesEsperienza2" TextMode="MultiLine" Columns="100"
                                                Rows="3" Style="resize: none" Enabled="false" ></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Anno Esperienza:
                                        </td>
                                        <td>
                                            <asp:Label runat="server" CssClass="label" ID="lblAnnoEsperienza1" Text="Non impostato" />
                                            <asp:Button ID="btnCron1" runat="server" Text="Mostra cronologia" style="float: right;" visible="false" CommandName="MostraCronologia1"/>
                                            <asp:Button ID="btnEsp1" runat="server" Text="Mostra variazioni" style="float: right;" visible="false" CommandName="MostraVariazioni1"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Descrizione Esperienza:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDesEsperienza1" TextMode="MultiLine" Columns="100"
                                                Rows="3" Style="resize: none" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                                <div runat="server" id="divArea" style="display: none"></div>
                                <br />
                                <table align="right">
                                    <tr>
                                        <td>
                                            <asp:Button ID="cmdModifica" runat="server" CssClass="Pulsante" Text="Inserisci" CommandName="ApriModifica" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sel.">
                        <HeaderStyle Width="20%"></HeaderStyle>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSeleziona" TextAlign="Left" ToolTip="Seleziona Settori" onchange="ControllaCkeck(this);return false;"
                                runat="server" OnCheckedChanged="ChkChange"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        <br />
		
         <div class="wrapper" style="width:100%" >
            <p style="text-align: center">
                <asp:Label ID="Label4" runat="server" CssClass="bold" Text="(*)Dichiarazioni"></asp:Label>
            </p>
            <div class="row">
			    <asp:CheckBox runat="server" ID="chkAttivita" Text="L’ente svolge attività nel/i settore/i richiesto da almeno 3 anni"/>
            </div>
            <div class="row">
			    <asp:CheckBox runat="server" ID="chkFiniIstituzionali" Text="Gli scopi istituzionali dell’ente sono conformi alle finalità di cui all’art. 1 della legge n. 64/2001 e all’art. 2 del d.lgs. n. 40 del 2017 "/>
            </div>
            <div class="row" runat="server" id="divSenzaScopoLucro">
			    <asp:CheckBox runat="server" ID="chkSenzaScopoLucro" Text="L’ente è senza scopo di lucro."/>
            </div>
        </div>
        <br />
        <p style="text-align:center">
            <asp:label id="lblSedeLegale" runat="server" CssClass="bold" Text="(*)Sede Legale Ente in Accordo"></asp:label>
        </p>
        <div class="wrapper" style="width:100%">
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblProvincia" CssClass="label" 
                        AssociatedControlID="ddlProvincia" runat="server" Text="(*)Provincia/Nazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:25%">       
                    <asp:dropdownlist id="ddlProvincia" CssClass="ddlClass" AutoPostBack="true" runat="server"  ></asp:dropdownlist>           
                </div>
                <div class="colOggetti" style="width:10%">
                    <asp:CheckBox ID="ChkEstero" AutoPostBack="true" Text="Estero" ToolTip="Flag Estero" runat="server" />
                </div>   
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblComune" CssClass="label" AssociatedControlID="ddlComune" runat="server" Text="Comune"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:dropdownlist id="ddlComune" CssClass="ddlClass" runat="server">
                        <asp:ListItem Value="0" Text=""></asp:ListItem>
                    </asp:dropdownlist>         
                </div>    
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblIndirizzo" CssClass="label" runat="server" Text="(*)Indirizzo" AssociatedControlID="txtIndirizzo"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:TextBox ID="txtIndirizzo" runat="server" CssClass="textbox" onkeyup="javascript: SetContextKey();"></asp:TextBox>
                    <asp:AutoCompleteExtender 
                        ID="AutoCompleteExtenderIndirizzo" 
                        TargetControlID="txtIndirizzo" 
                        ContextKey ="ddlComune"
                        CompletionListCssClass="ddl_Autocomplete"
                        UseContextKey="true"
                        CompletionInterval="100" EnableCaching="false" 
                        runat="server" MinimumPrefixLength="5" ServiceMethod="GetCompletionList" >
                    </asp:AutoCompleteExtender>
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblNumeroCivico"  runat="server" Text="(*)Numero civico" AssociatedControlID="txtCivico"></asp:Label>
                </div>
                <div class="colOggetti" style="width:10%">
                    <asp:TextBox ID="txtCivico" runat="server" CssClass="textbox" ></asp:TextBox>
                </div>
                <div class="collable" style="width:5%">
                        <asp:ImageButton ID="infocivico" runat="server" AlternateText="Informazioni Numero Civico" 
                        ImageUrl="Images/info_small.png" style="width:20px;height:20px"  
                        ToolTip="NEL CAMPO CIVICO E' POSSIBILE INSERIRE SOLO I SEGUENTI FORMATI:
                                - 21
                                - 21/A 
                                - 21/A5 
                                - 21 BIS 
                                - KM 21,500 
                                OPPURE IL VALORE SNC" disabled="disabled"/>
                </div>
                <div class="collable" style="width:10%">
                    <asp:Label ID="lblCAP" CssClass="label" runat="server" Text="(*)C.A.P." AssociatedControlID="txtCAP"  ></asp:Label>
                </div>
                <div class="colOggetti" style="width:5%">
                    <asp:TextBox ID="txtCAP" CssClass="textbox" runat="server"></asp:TextBox>

                </div>
                <div class="collable" style="width:5%">
                    <asp:ImageButton ID="imgCap" runat="server" title="Seleziona il Cap di Residenza" ImageUrl="Images/valida_small.png" AlternateText="Seleziona il Cap di Residenza"  style="width:20px;height:20px" />
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDettaglioRecapito" CssClass="label" runat="server" Text="Dettaglio recapito" AssociatedControlID="TxtDettaglioRecapito" ></asp:Label>
                </div>
                <div class="colOggetti" style="width:80%">
                    <asp:TextBox ID="TxtDettaglioRecapito" CssClass="textbox" runat="server" ToolTip="Dettagli aggiuntivi del recapito" TextMode="MultiLine" Rows="2" Width="99%"></asp:TextBox>
                </div>
                <div class="collable" style="width:5%">
                        <asp:ImageButton ID="imgRecapito" runat="server" AlternateText="Informazioni Recapito" 
                        ImageUrl="Images/info_small.png" style="width:20px;height:20px"  
                        ToolTip="ATTENZIONE!!! Nel campo Dettaglio Recapito  devono essere indicate ESCLUSIVAMENTE informazioni AGGIUNTIVE, se presenti, all' indirizzo per consentire una maggiore accuratezza nelle spedizioni postali. NON VA PERTANTO RIPETUTO L'INDIRIZZO (per il quale già esiste il campo specifico) e NON VANNO INSERITI RECAPITI TELEFONICI o altre informazioni non attinenti. Si fa presente che questo campo NON E' OBBLIGATORIO e quindi VA LASCIATO VUOTO se non è necessario indicare informazioni aggiuntive al recapito relativo." 
                        disabled="disabled"/>
                </div>
            </div>
        </div>
        <div class="wrapper" style="width:100%; border-style:none">
            <div class="RigaVuota" >&nbsp;</div>
            <div class="RigaPulsanti">
           <asp:Button ID="cmdPdfSettori" CssClass="Pulsante" runat="server"  Text="Pdf Settori" Visible="false" />

            <asp:Button ID="btnRiserva" CssClass="Pulsante" runat="server" Visible="false"  Text="Riserva"  />
          
                <asp:Button ID="cmdAnnullaModifica" CssClass="Pulsante" runat="server" Text="Annulla Modifica" />
                <asp:Button ID="cmdAnnullaCancellazione" runat="server" CssClass="Pulsante" Text="Annulla Cancellazione" />
                <asp:Button ID="cmdVisualizzaDatiAccreditati" runat="server" CssClass="Pulsante" Text="Visualizza Dati Accreditati" />
                <asp:Button ID="cmdElimina" runat="server" CssClass="Pulsante" Text="Elimina" />
                <asp:Button ID="CmdSalva" runat="server" CssClass="Pulsante" Text="Salva"/>
                <asp:Button ID="CmdSalvaDoc" runat="server" Visible="false" CssClass="Pulsante" Text="Salva Modifiche Art.2/Art.10"/>
                <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
            </div>
        </div>
        <br />
         <div id="DivAccreditaRiserva" class="row"  runat="server" visible="false" >
                <fieldset class="ContornoPagina" id="RiservaSiNo" runat="server">
                <legend>Seleziona/Deseleziona Causali Riserva</legend>
                
                
                <asp:datagrid id="dtgCausaliRiserva" runat="server" CssClass="table"  Width="100%" ToolTip="Elenco Causali Riserva"  CellPadding="2"  AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
		        <SelectedItemStyle BackColor="White"></SelectedItemStyle>
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		        <HeaderStyle></HeaderStyle>
		        <Columns>
                    
			        <asp:BoundColumn Visible="False" DataField="IDCausale" HeaderText=""></asp:BoundColumn>
                    <asp:BoundColumn Visible="False" DataField="IdEnteCausaleRiserva" HeaderText=""></asp:BoundColumn>
			        <asp:BoundColumn  DataField="Descrizione" HeaderText=" Causali Riserva">
                        <ItemStyle HorizontalAlign="Left"/>
                    </asp:BoundColumn>
			        <asp:TemplateColumn HeaderText="Sel.">
				        <HeaderStyle Width="20%" ></HeaderStyle>
				        <ItemTemplate>
					        <asp:CheckBox id="chkSeleziona" Text="Sel" TextAlign="Left" toolTip="Seleziona  Causali Riserva" runat="server"></asp:CheckBox>
				        </ItemTemplate>
			        </asp:TemplateColumn>
		        </Columns>
	        </asp:datagrid>
                
                <div style="float:left;width:40%" ></div> <div style="text-align:right"><asp:Button ID="BtnAssegna"  CssClass="Pulsante" runat="server" Visible="false"  Text="Applica"  /><asp:Button ID="CmdAnnullaRiservaSiNo"  CssClass="Pulsante" runat="server"  Text="Chiudi"  /></div>

             </fieldset>
        <p style="text-align:center">
            <asp:label id="lblTitotloProgettiEnte" runat="server" CssClass="bold" Text="Progetti Ente"></asp:label>
        </p>
        <asp:datagrid id="dtgAttivitaEnte" runat="server" CssClass="table"  Width="100%" ToolTip="Attività Ente"  CellPadding="2"  AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" PageSize="5" UseAccessibleHeader="True">
			<SelectedItemStyle BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
				<asp:BoundColumn DataField="CodiceEnte" HeaderText="Codice Progetto"></asp:BoundColumn>
				<asp:BoundColumn DataField="Titolo" HeaderText="Titolo"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="StatoAttivit&#224;" HeaderText="Stato"></asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
        <asp:HiddenField id="lblIdEnte" runat="server" />
        <asp:HiddenField id="txtmodificaqueristringdiINS" runat="server" />
        <asp:HiddenField id="txtBlocco" runat="server" />
        <asp:HiddenField id="txtUtenza" runat="server" />
        <asp:HiddenField id="txtPassword" runat="server" />
        <asp:HiddenField id="txtdataoggi" runat="server" />
        <asp:HiddenField id="lblTipoUtente" runat="server" />
        <asp:HiddenField id="txtRichiedente" runat="server" />
        <asp:HiddenField id="lblIdClasse" runat="server" />
        <asp:HiddenField id="lblInPartenariato" runat="server" />
        <asp:HiddenField id="txtIdSede" runat="server" />
        <asp:HiddenField id="txtIDComunes" runat="server" />
        <asp:HiddenField id="txtIDComune" runat="server" />
        <asp:HiddenField id="txtpianolocale" runat="server" />
        <asp:HiddenField ID="hdGiuridica" runat="server" />
        <asp:HiddenField ID="txtCDFRespLegal" runat="server" />
        <asp:HiddenField ID="hdDataNominaRL" runat="server" />
             </div>

        <!-- INIZIO POPUP Maschera Esperienze-->
        <asp:ModalPopupExtender runat="server" ID="mpe_EsperienzaAree" TargetControlID="hfRigaSelezionata"
            PopupControlID="pnlEsperienzaArea" BackgroundCssClass="modalBackgroundPopUp"
            DropShadow="True" PopupDragHandleControlID="pnlEsperienzaArea" DynamicServicePath=""
            Enabled="True">
        </asp:ModalPopupExtender>
        <asp:Panel ID="pnlEsperienzaArea" runat="server" Style="background-color: White;
            padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
            <div class="wrapper" style="width: 100%">
                <div class="headers">
                    <h2><asp:Label ID="lblTitoloEsperienzaArea" runat="server" Text="Dettaglio Settore"></asp:Label></h2>
                </div>
            </div>
            <br />
            <asp:Label ID="lblMsgAreeInervento" runat="server" CssClass="msgErrore"></asp:Label>
            <br />
            <br />
            <div class="wrapper" style="width: 100%">
                <div class="RigaVuota">
                    &nbsp;</div>
                <asp:HiddenField ID="hfRigaSelezionata" runat="server" />
                <asp:HiddenField ID="hfRigaAmbiti" runat="server" />
                <div class="row">
                    <div class="collable" style="width: 15%">
                        &nbsp;
                        <asp:Label ID="lblAnno3Esperienza" CssClass="label" runat="server" AssociatedControlID="txtAnno3"
                            Text="Anno esperienza"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 15%">
                        <asp:TextBox ID="txtAnno3" runat="server" CssClass="textbox" MaxLength="4" Style="resize: none;"></asp:TextBox>
                    </div>
                    <div class="collable" style="width: 10%">
                        <asp:Label ID="lblEsperienza3" CssClass="label" runat="server" AssociatedControlID="txtEsperienza3"
                            Text="Descrizione esperienza"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 60%">
                        <asp:TextBox ID="txtEsperienza3" TextMode="MultiLine" runat="server" CssClass="textbox"
                            Columns="80" Rows="3" MaxLength="1000" ></asp:TextBox>
                    </div>
                </div>
                <br />
                <br />
                <div class="row">
                    <div class="collable" style="width: 15%">
                        &nbsp;
                        <asp:Label ID="lblAnno2Esperienza" CssClass="label" runat="server" AssociatedControlID="txtAnno2"
                            Text="Anno esperienza"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 15%">
                        <asp:TextBox ID="txtAnno2" runat="server" CssClass="textbox" MaxLength="4" Style="resize: none;"></asp:TextBox>
                    </div>
                    <div class="collable" style="width: 10%">
                        <asp:Label ID="lblEsperienza2" CssClass="label" runat="server" AssociatedControlID="txtEsperienza2"
                            Text="Descrizione esperienza" ></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 60%">
                        <asp:TextBox ID="txtEsperienza2" TextMode="MultiLine" runat="server" CssClass="textbox"
                            Columns="80" Rows="3" MaxLength="1000" ></asp:TextBox>
                    </div>
                </div>
                <br />
                <br />
                <div class="row">
                    <div class="collable" style="width: 15%">
                        &nbsp;
                        <asp:Label ID="lblAnno1Esperienza" CssClass="label" runat="server" AssociatedControlID="txtAnno1"
                            Text="Anno esperienza"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 15%">
                        <asp:TextBox ID="txtAnno1" runat="server" CssClass="textbox" MaxLength="4" Style="resize: none;"></asp:TextBox>
                    </div>
                    <div class="collable" style="width: 10%">
                        <asp:Label ID="lblEsperienza1" CssClass="label" runat="server" AssociatedControlID="txtEsperienza1"
                            Text="Descrizione esperienza"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 60%">
                        <asp:TextBox ID="txtEsperienza1" TextMode="MultiLine" runat="server" CssClass="textbox"
                            Columns="80" Rows="3" MaxLength="1000" ></asp:TextBox>
                    </div>
                </div>
                <br />
                <br />
                <asp:GridView ID="grdAmbiti" Width="100%" runat="server" AutoGenerateColumns="false"
                    DataKeyNames="CODIFICA">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HiddenField ID="HFCodifica" runat="server" Value='<%# Eval("CODIFICA") %>'/>
                            </ItemTemplate> 
                        </asp:TemplateField>
                        <asp:BoundField DataField="AREAINTERVENTO" HeaderText="AREE D'INTERVENTO"
                            HeaderStyle-CssClass="table" />
                        <asp:TemplateField HeaderText="Sel." HeaderStyle-CssClass="table" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chkSeleziona" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HiddenField ID="HFIDSettore" runat="server" Value='<%# Eval("IDSETTORE") %>'/>
                            </ItemTemplate> 
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HiddenField ID="HFIDAREA" runat="server" Value='<%# Eval("IDAREA") %>'/>
                            </ItemTemplate> 
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="wrapper" style="width: 100%; border-style: none">
                <div class="RigaVuota">
                    &nbsp;</div>
                <div class="RigaPulsanti">
                    <asp:Button ID="CmdConferma" runat="server" CssClass="Pulsante" Text="Conferma" />
                    &nbsp;
                    <asp:Button ID="Button1" runat="server" CssClass="Pulsante" Text="Chiudi" />
                </div>
            </div>
        </asp:Panel>
        <!-- FINE POPUP Maschera -->
         <!--POPUP File--Atto Costitutivo Inizio-->
        <asp:ModalPopupExtender runat="server" ID="mpeAttoCostitutivo" TargetControlID="HFAttoCostitutivo" 
            PopupControlID="pnlUploadAttoCostitutivo" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
            PopupDragHandleControlID="pnlEsperienzaArea" DynamicServicePath="" Enabled="True"
            CancelControlID="btnCloseAC"> 
        </asp:ModalPopupExtender>
        <asp:Panel ID="pnlUploadAttoCostitutivo" runat="server" Style="background-color: White;
            padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
            <asp:Label ID="lblErroreAttoCostitutivo" runat="server" CssClass="msgErrore" AssociatedControlID="cmdAllegaAC"></asp:Label>
            <p class="msgInfo">Atto Costitutivo dell'Ente</p>
            <fieldset>
                <input id="fileAttoCostitutivo" type="file" style="width: 95%" runat="server"  />
                <div style="margin-top: 1em;">
                </div>
                <asp:Button ID="cmdAllegaAC" runat="server" CssClass="Pulsante" Text="Carica File" />
                <asp:Button ID="btnCloseAC" runat="server" CssClass="Pulsante" Text="Chiudi"/>
            </fieldset>
        </asp:Panel>
        <!--POPUP File--Atto Costitutivo Fine-->

        <!--POPUP File--Statuto Inizio-->
        <asp:ModalPopupExtender runat="server" ID="mpeStatuto" TargetControlID="HFStatuto" 
            PopupControlID="pnlUploadStatuto" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
            PopupDragHandleControlID="pnlEsperienzaArea" DynamicServicePath="" Enabled="True"
            CancelControlID="btnCloseStatuto"> 
        </asp:ModalPopupExtender>
        <asp:Panel ID="pnlUploadStatuto" runat="server" Style="background-color: White;
            padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
            <asp:Label ID="lblErroreStatuto" runat="server" CssClass="msgErrore" AssociatedControlID="cmdAllegaStatuto"></asp:Label>
            <p class="msgInfo">Statuto dell'Ente</p>
            <fieldset>
                <input id="fileStatuto" type="file" style="width: 95%" runat="server"  />
                <div style="margin-top: 1em;">
                </div>
                <asp:Button ID="cmdAllegaST" runat="server" CssClass="Pulsante" Text="Carica File" />
                <asp:Button ID="btnCloseStatuto" runat="server" CssClass="Pulsante" Text="Chiudi"/>
            </fieldset>
        </asp:Panel>
        <!--POPUP File--Statuto Fine-->
        <!--POPUP File--Delibera adesione Inizio-->
        <asp:ModalPopupExtender runat="server" ID="mpeDeliberaAdesione" TargetControlID="HFDelibera" 
            PopupControlID="pnlUploadDeliberaAdesione" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
            PopupDragHandleControlID="pnlEsperienzaArea" DynamicServicePath="" Enabled="True"
            CancelControlID="btnCloseDeliberaAdesione"> 
        </asp:ModalPopupExtender>
         <asp:Panel ID="pnlUploadDeliberaAdesione" runat="server" Style="background-color: White;
            padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
            <asp:Label ID="lblErroreDeliberaAdesione" runat="server" CssClass="msgErrore" AssociatedControlID="cmdAllegaDeliberaAdesione"></asp:Label>
            <p class="msgInfo">Delibera</p>
            <fieldset>
                <input id="fileDeliberaAdesione" type="file" style="width: 95%" runat="server"  />
                <div style="margin-top: 1em;">
                </div>
                <asp:Button ID="cmdAllegaDA" runat="server" CssClass="Pulsante" Text="Carica File" />
                <asp:Button ID="btnCloseDeliberaAdesione" runat="server" CssClass="Pulsante" Text="Chiudi"/>
            </fieldset>
        </asp:Panel>
         <!--POPUP File--Delibera adesione Fine-->

        <!--POPUP File--Carta impegno etico Inizio-->
        <asp:ModalPopupExtender runat="server" ID="mpeImpegnoEtico" TargetControlID="HFImpegnoEtico" 
            PopupControlID="pnlUploadImpegnoEtico" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
            PopupDragHandleControlID="pnlEsperienzaArea" DynamicServicePath="" Enabled="True"
            CancelControlID="btnCloseImpegnoEtico"> 
        </asp:ModalPopupExtender>
         <asp:Panel ID="pnlUploadImpegnoEtico" runat="server" Style="background-color: White;
            padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
            <asp:Label ID="lblErroreImpegnoEtico" runat="server" CssClass="msgErrore" AssociatedControlID="cmdAllegaImpegnoEtico"></asp:Label>
            <p class="msgInfo">Carta Impegno Etico</p>
            <fieldset>
                <input id="fileImpegnoEtico" type="file" style="width: 95%" runat="server"  />
                <div style="margin-top: 1em;">
                </div>
                <asp:Button ID="cmdAllegaIE" runat="server" CssClass="Pulsante" Text="Carica File" />
                <asp:Button ID="btnCloseImpegnoEtico" runat="server" CssClass="Pulsante" Text="Chiudi"/>
            </fieldset>
        </asp:Panel>
        <!--POPUP File--Carta impegno etico Fine-->

        <!--POPUP File--Carta di impegno  Inizio-->
         <asp:ModalPopupExtender runat="server" ID="mpeImpegno" TargetControlID="HFImpegno" 
            PopupControlID="pnlUploadImpegno" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
            PopupDragHandleControlID="pnlEsperienzaArea" DynamicServicePath="" Enabled="True"
            CancelControlID="btnCloseImpegno"> 
        </asp:ModalPopupExtender>
         <asp:Panel ID="pnlUploadImpegno" runat="server" Style="background-color: White;
            padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
            <asp:Label ID="lblErroreImpegno" runat="server" CssClass="msgErrore" AssociatedControlID="cmdAllegaImpegno"></asp:Label>
            <p class="msgInfo">Dichiarazione di Impegno </p>
            <fieldset>
                <input id="fileImpegno" type="file" style="width: 95%" runat="server"  />
                <div style="margin-top: 1em;">
                </div>
                <asp:Button ID="cmdAllegaCI" runat="server" CssClass="Pulsante" Text="Carica File" />
                <asp:Button ID="btnCloseImpegno" runat="server" CssClass="Pulsante" Text="Chiudi"/>
            </fieldset>
        </asp:Panel>
        <!--POPUP File--Carta di impegno  Fine-->

    </fieldset>  
</asp:Content>
