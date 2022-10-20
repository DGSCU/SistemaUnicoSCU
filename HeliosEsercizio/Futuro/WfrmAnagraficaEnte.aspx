<%@ Page Title="Anagrafica Enti" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="WfrmAnagraficaEnte.aspx.vb" Inherits="Futuro.WfrmAnagraficaEnte"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
    <style type="text/css">
        .ui-datepicker
        {
            font-size: 11px;
        }
    </style>
    <script type="text/javascript">
		var prefissoIdClient = "MainContent_";

		function SetContextKey() {
			$find('<%=AutoCompleteExtenderIndirizzo.ClientID%>').set_contextKey($get("<%=ddlComune.ClientID %>").value);
		}
	</script>
    <script type="text/javascript">



		$(function () {
			var DataRicezioneCartacea = CostruisciId("txtDataRicezioneCartacea");
			var readonly = '<%=txtDataRicezioneCartacea.ReadOnly%>';
			if (readonly == "False") {
				var sharpDataRicezioneCartacea = "#" + DataRicezioneCartacea
				$("" + sharpDataRicezioneCartacea + "").datepicker();
			}
		});

		$(function () {
			var DataCostituzione = CostruisciId("txtDataCostituzione");
			var readonly = '<%=txtDataCostituzione.ReadOnly%>';
			if (readonly == "False") {
				var sharpDataCostituzione = "#" + DataCostituzione
				$("" + sharpDataCostituzione + "").datepicker();
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
        }

        function ControlloCheckedVero() {
            var idEmail = CostruisciId("txtemail");
            var txtEmail = document.getElementById(idEmail);

            var idVeroEmail = CostruisciId("chkVeroEmail");
            var chkVeroEmail = document.getElementById(idVeroEmail);

            var idFalsoEmail = CostruisciId("chkFalsoEmail");
            var chkFalsoEmail = document.getElementById(idFalsoEmail);

            var idDataControlloEmail = CostruisciId("txtdataControlloEmail");
            var txtdataControlloEmail = document.getElementById(idDataControlloEmail);

            if (txtEmail.value != '') {
                if (chkVeroEmail.checked == true && chkFalsoEmail.checked == true) {
                    chkFalsoEmail.checked = false;
                    return;
                }
                if (chkVeroEmail.checked == true || chkFalsoEmail.checked == true) {
                    var dataServer = '<%=Session("dataserver")%>';
                    txtdataControlloEmail.innerText = dataServer.substring(0, 10);
                }
            }
            else {
                alert("Inserire l'indirizzo di posta prima di effettuare la validazione!");
                chkFalsoEmail.checked = false;
                chkVeroEmail.checked = false;
                return;
            }
        }

        function ControlloCheckedFalso() {
            var idEmail = CostruisciId("txtemail");
            var txtEmail = document.getElementById(idEmail);

            var idVeroEmail = CostruisciId("chkVeroEmail");
            var chkVeroEmail = document.getElementById(idVeroEmail);

            var idFalsoEmail = CostruisciId("chkFalsoEmail");
            var chkFalsoEmail = document.getElementById(idFalsoEmail);

            var idDataControlloEmail = CostruisciId("txtdataControlloEmail");
            var txtdataControlloEmail = document.getElementById(idDataControlloEmail);

            if (txtEmail.value != '') {
                if (chkVeroEmail.checked == true && chkFalsoEmail.checked == true) {
                    chkVeroEmail.checked = false;
                    return;
                }
                if (chkVeroEmail.checked == true || chkFalsoEmail.checked == true) {
                    var dataServer = '<%=Session("dataserver")%>';
                    txtdataControlloEmail.innerText = dataServer.substring(0, 10);
                }
            }
            else {
                alert("Inserire l'indirizzo di posta prima di effettuare la validazione!");
                chkFalsoEmail.checked = false;
                chkVeroEmail.checked = false;
                return;
            }
        }

        function ControlloEmailOrdinaria() {
            var idEmail = CostruisciId("txtemail");
            var txtEmail = document.getElementById(idEmail);

            if (txtEmail.value == '') {
                alert("Inserire indirizzo di posta Elettronica!");
            }
            else {

                var regEmail = new RegExp("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$");
                if (!regEmail.test(txtEmail.value)) {
                    alert("Il formato Email non è valido.");
                }
            }
        }

        function ControlloCheckedVeroHttp() {
            var idHttp = CostruisciId("txthttp");
            var txtHttp = document.getElementById(idHttp);

            var idVeroHttp = CostruisciId("chkVerohttp");
            var chkVeroHttp = document.getElementById(idVeroHttp);

            var idFalsoHttp = CostruisciId("chkFalsohttp");
            var chkFalsoHttp = document.getElementById(idFalsoHttp);

            var idDataControlloHttp = CostruisciId("txtdataControllohttp");
            var txtdataControlloHttp = document.getElementById(idDataControlloHttp);

            if (txtHttp.value != '') {
                if (chkVeroHttp.checked == true && chkFalsoHttp.checked == true) {
                    chkFalsoHttp.checked = false;
                    return;
                }
                if (chkVeroHttp.checked == true || chkFalsoHttp.checked == true) {
                    var dataServer = '<%=Session("dataserver")%>';
                    txtdataControlloHttp.innerText = dataServer.substring(0, 10);
                }
            }
            else {
                alert("Inserire il Sito prima di effettuare la validazione!");
                chkFalsoHttp.checked = false;
                chkVeroHttp.checked = false;
                return;
            }
        }

        function ControlloCheckedFalsoHttp() {
            var idHttp = CostruisciId("txthttp");
            var txtHttp = document.getElementById(idHttp);

            var idVeroHttp = CostruisciId("chkVerohttp");
            var chkVeroHttp = document.getElementById(idVeroHttp);

            var idFalsoHttp = CostruisciId("chkFalsohttp");
            var chkFalsoHttp = document.getElementById(idFalsoHttp);

            var idDataControlloHttp = CostruisciId("txtdataControllohttp");
            var txtdataControlloHttp = document.getElementById(idDataControlloHttp);

            if (txtHttp.value != '') {
                if (chkVeroHttp.checked == true && chkFalsoHttp.checked == true) {
                    chkVeroHttp.checked = false;
                    return;
                }
                if (chkVeroHttp.checked == true || chkFalsoHttp.checked == true) {
                    var dataServer = '<%=Session("dataserver")%>';
					txtdataControlloHttp.innerText = dataServer.substring(0, 10);
				}
			}
			else {
				alert("Inserire il Sito prima di effettuare la validazione!");
				chkFalsoHttp.checked = false;
				chkVeroHttp.checked = false;
				return;
			}
		}

		function Openhttp() {

			var idHttp = CostruisciId("txthttp");
			var txtHttp = document.getElementById(idHttp);
			var NomeSito = txtHttp.value;

			if (txtHttp.value == '') {
				alert("Inserire Indirizzo del Sito.")
			} else {

				var i = new RegExp("http:\/\/");
				if (!i.test(NomeSito)) {
					mywin = window.open("http://" + NomeSito);
				} else { mywin = window.open(NomeSito); }
			}
		}

		function OnChangeHttp() {
			var idHttp = CostruisciId("txthttp");
			var txtHttp = document.getElementById(idHttp);

			if (txtHttp.value == '') {

				var idDataControllohttp = CostruisciId("txtdataControllohttp");
				var txtDataControllohttp = document.getElementById(idDataControllohttp);

				var idFalsohttp = CostruisciId("chkFalsohttp");
				var chkFalsohttp = document.getElementById(idFalsohttp);

				var idVerohttp = CostruisciId("chkVerohttp");
				var chkVerohttp = document.getElementById(idVerohttp);

				txtDataControllohttp.innerText = '';
				chkFalsohttp.checked = false;
				chkVerohttp.checked = false;
			}

		}

		function OnChangeEmail() {
			var idEmail = CostruisciId("txtemail");
			var txtEmail = document.getElementById(idEmail);

			if (txtEmail.value == '') {

				var idDataControlloEmail = CostruisciId("txtdataControlloEmail");
				var txtDataControlloEmail = document.getElementById(idDataControlloEmail);

				var idFalsoEmail = CostruisciId("chkFalsoEmail");
				var chkFalsoEmail = document.getElementById(idFalsoEmail);

				var idVeroEmail = CostruisciId("chkVeroEmail");
				var chkVeroEmail = document.getElementById(idVeroEmail);

				txtDataControlloEmail.innerText = '';
				chkFalsoEmail.checked = false;
				chkVeroEmail.checked = false;
			}

		}

		function ShowPopUpAttoNomina() {
			$find('popUpAttoNomina').show();
			return false;
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
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <fieldset class="ContornoPagina">
        <legend>Anagrafica Enti</legend>
        <div class="row" style="text-align: right">
            <asp:Label ID="lblaccordo" runat="server" AssociatedControlID="imgAccordo" Text="Visualizza Accordo"
                Visible="false"></asp:Label>
            <asp:ImageButton ID="imgAccordo" runat="server" ImageUrl="Images/Icona_volontario_small.png"
                ToolTip="Visualizza Accordo" AlternateText="Visualizza Accordo" ImageAlign="Middle"
                Visible="false" />&nbsp;
            <asp:Label ID="lblterritorio" runat="server" AssociatedControlID="imbComuni" Text="Ambito Territoriale"></asp:Label>
            <asp:ImageButton ID="imbComuni" runat="server" ImageUrl="images/sedi_small.png" ToolTip="Comuni"
                AlternateText="Comuni" ImageAlign="Middle" />
            <asp:ImageButton ID="imgCronologiaDocumenti" runat="server" ImageUrl="images/cronologia_small.png"
                ToolTip="Mostra Cronologia Documentazione Ente" AlternateText="Mostra Cronologia Documentazione Ente"
                ImageAlign="Middle" />
            <asp:ImageButton ID="ImgCronologiaProgetti" runat="server" ImageUrl="images/cronologia_small.png"
                ToolTip="Mostra Cronologia Documentazione Progetti Ente" AlternateText="Mostra Cronologia Documentazione Progetti Ente"
                ImageAlign="Middle" />
            <asp:ImageButton ID="ImgEmail_Pec" runat="server" ImageUrl="images/cronologia_small.png"
                ToolTip="Mostra Cronologia Email/PEC" AlternateText="Mostra Cronologia Email/PEC"
                ImageAlign="Middle" />
            <asp:ImageButton ID="CmdInforScu1" runat="server" ImageUrl="Images/vincoli_small.png"
                ToolTip="Analisi Codice Fiscale" AlternateText="Analisi Codice Fiscale" ImageAlign="Middle" />
        </div>
        <asp:Label ID="lblMessaggio" runat="server" CssClass="msgErrore"></asp:Label>
        <br />
        <asp:Label ID="lblMessaggioArt2Art10" runat="server" style='color:red' Visible="false"></asp:Label>
        <br />
         <asp:Button ID="cmdModNoAdeguamento" runat="server" CssClass="Pulsante" Visible="false" Text="Modifica dei dati che non richiedono adeguamento" /> 
        <br />
        <br />
        <div class="wrapper" style="width: 100%">
            <div class="headers">
                <h2>
                    <asp:Label ID="lblTitolo" runat="server" Text="Anagrafica Enti"></asp:Label></h2>
            </div>
            <div class="RigaVuota">
                &nbsp;</div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblCodiceEnte" CssClass="label" AssociatedControlID="lblCodRegione"
                        runat="server" Text="Cod. Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:Label ID="lblCodRegione" CssClass="bold" runat="server"></asp:Label>
                </div>
            </div>
        </div>
        <div class="wrapper" style="width: 100%">
            <div class="RigaVuota">
                &nbsp;</div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblStatoLabel" CssClass="label" AssociatedControlID="lblStato" runat="server"
                        Text="Stato"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 20%">
                    <asp:Label ID="lblStato" CssClass="bold" runat="server"></asp:Label>
                </div>
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblClasseRichiesta" CssClass="label" AssociatedControlID="lblClasse"
                        runat="server" Text="Sezione Richiesta"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 20%">
                    <asp:Label ID="lblClasse" CssClass="bold" runat="server"></asp:Label>
                </div>
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblDataRichiestaAccredito" CssClass="label" AssociatedControlID="lblDataClasse"
                        runat="server" Text="Data Iscrizione"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 15%">
                    <asp:Label ID="lblDataClasse" CssClass="bold" runat="server"></asp:Label>
                </div>
            </div>
        </div>
        <div class="wrapper" style="width: 100%">
            <div class="RigaVuota">
                &nbsp;</div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblEnte" CssClass="label" AssociatedControlID="txtdenominazione" runat="server"
                        Text="(*)Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 60%">
                    <asp:TextBox ID="txtdenominazione" CssClass="textbox" runat="server"></asp:TextBox>
                </div>
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblCodEnte" CssClass="label" AssociatedControlID="txtDataRicezioneCartacea"
                        runat="server" Text="Data Richiesta Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 10%">
                    <asp:TextBox ID="txtDataRicezioneCartacea" CssClass="textbox" runat="server" MaxLength="10"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblCodiceFiscale" CssClass="label" AssociatedControlID="txtCodFis"
                        runat="server" Text="(*)Cod. Fis."></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:TextBox ID="txtCodFis" CssClass="textbox" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label2" CssClass="label" AssociatedControlID="ddlTipologia" runat="server"
                        Text="(*)Tipo"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 25%">
                    <asp:DropDownList ID="ddlTipologia" runat="server" CssClass="ddlClass" Width="100px"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </div>
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label6" CssClass="label" AssociatedControlID="ddlGiuridica" runat="server"
                        Text="Denominazione tipologia"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 45%">
                    <asp:DropDownList ID="ddlGiuridica" runat="server" CssClass="ddlClass" Width="60%"
                        AutoPostBack="true">
                    </asp:DropDownList>
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
            <br />
        </div>
        <div class="RigaVuota">
                &nbsp;</div>
        <div class="wrapper" style="width: 100%">
            <asp:HiddenField ID="HFRP" runat="server" />
            <p style="text-align: center">
                <asp:Label ID="Label3" runat="server" CssClass="bold" Text="(*)Dati Rappresentante Legale"></asp:Label>
            </p>
            <div class="RigaVuota">
                &nbsp;</div>
            <div class="row" runat="server" id="DivDataNomina">
                <div class="colOggetti" style="width: 35%">
                    <asp:Label runat="server" ID="lblDataNominaRL" Text="(*)Data di Nomina del Rappresentante Legale"
                        AssociatedControlID="txtDataNominaRL" />
                </div>
                <div class="colOggetti" style="width: 10%">
                    <asp:TextBox runat="server" ID="txtDataNominaRL" Text="" />
                </div>
            </div>
            <div class="row" runat="server" id="rowNoAN">
                <div class="collable" style="width: 35%">
                    <asp:Label ID="lblCV" CssClass="label" AssociatedControlID="fileAN" runat="server"
                        Text="(*)Atto Nomina/Altro del Rappresentante Legale"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 10%">
                    <asp:Button ID="CmdAttoNomina" runat="server"
                        CssClass="Pulsante" Text="Carica Atto Nomina/Altro" />
                </div>
            </div>
            <div class="row" runat="server" id="rowAN" visible="false">
                <table class="table" cellspacing="0" cellpadding="2" rules="all" border="1" id="MainContent_dtgConsultaDocumenti"
                    style="font-size: Small; width: 100%; border-collapse: collapse;">
                    <caption>
                        Atto di nomina/Altro
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
                                <asp:ImageButton runat="server" ID="btnDownloadAN" ImageUrl="images/giu_small.png"
                                    AlternateText="Scarica Atto Nomina/Altro" ToolTip="Scarica Atto di nomina/Altro" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtANFilename"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtANData"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtANHash"></asp:Literal>
                            </td>
                            <td>
                                <asp:ImageButton runat="server" ID="btnModificaAN"
                                    ImageUrl="images/ripristina_small.png" AlternateText="Modifica Atto di nomina/Altro"
                                    ToolTip="Modifica Atto Nomina/Altro" />
                                <asp:ImageButton runat="server" ID="btnEliminaAN" ImageUrl="images/canc_small.png"
                                    AlternateText="Elimina Atto Nomina/Altro" ToolTip="Elimina Atto di nomina/Altro" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="RigaVuota">
                &nbsp;</div>
        </div>
        <div class="RigaVuota">
                &nbsp;</div>
        <div class="wrapper" style="width: 100%">
             <asp:HiddenField ID="HFAttoCostitutivo" runat="server" />
             <asp:HiddenField ID="HFDeliberaEnte" runat="server" />
             <asp:HiddenField ID="HFStatuto" runat="server" />
             <p style="text-align: center">
                <asp:Label ID="Label1" runat="server" CssClass="bold" Text="(*)Documenti costitutivi ente"></asp:Label>
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
      <%--  <div class="wrapper" style="width: 100%">
           <asp:HiddenField ID="HFRTDP" runat="server" />
           <p style="text-align: center">
                <asp:Label ID="Label7" runat="server" CssClass="bold" Text="(*)Atto di designazione del Responsabile del trattamento dei dati personali"></asp:Label>
           </p>
           <div class="row" id="rowNoRTDP" runat="server">
                <div class="collable" style="width: 35%">
                    <asp:Label ID="Label8" CssClass="label" AssociatedControlID="CmdAttoDesignazione" runat="server"
                        Text="(*)Atto di Designazione del Responsabile"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 10%">
                    <asp:Button ID="CmdAttoDesignazione" runat="server"
                        CssClass="Pulsante" Text="Carica Atto di Designazione" />
                </div>
            </div>
            <div class="row" runat="server" id="rowRTDP" visible="false">
                <table class="table" cellspacing="0" cellpadding="2" rules="all" border="1" id="Table2"
                    style="font-size: Small; width: 100%; border-collapse: collapse;">
                    <caption>
                       Atto di Designazione del Responsabile del trattamento dei dati personali
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
                                <asp:ImageButton runat="server" ID="btnDownloadRTDP" ImageUrl="images/giu_small.png"
                                    AlternateText="Scarica Atto di Designazione" ToolTip="Scarica Atto di Designazione del Responsabile del trattamento dei dati personali" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtRTDPFilename"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtRTDPData"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtRTDPHash"></asp:Literal>
                            </td>
                            <td>
                                <asp:ImageButton runat="server" ID="btnModificaRTDP"
                                    ImageUrl="images/ripristina_small.png" AlternateText="Modifica Atto di Designazione Responsabile"
                                    ToolTip="Modifica Atto di Designazione del Responsabile del trattamento dei dati personali" />
                                <asp:ImageButton runat="server" ID="btnEliminaRTDP" ImageUrl="images/canc_small.png"
                                    AlternateText="Elimina Atto di Designazione" ToolTip="Elimina Atto di Designazione del Responsabile del trattamento dei dati personali" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>--%>
        <br />
        <br />
        <div class="wrapper" style="width: 100%">
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
                                                Rows="3" Style="resize: none" Enabled="false"></asp:TextBox>
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

            <div class="boxPrivacy">
				<p>Il Rapporto annuale deve essere redatto annualmente dall’ente, entro il 31 Marzo di ogni anno di attività. Il rapporto deve porre in evidenza le attività connesse ai Programmi di intervento di servizio civile universale conclusi nell’anno considerato, precedente a quello di redazione del rapporto. </p>
				<p>In particolare, il rapporto è tenuto a porre in evidenza:</p>
				<ul style="list-style-type: decimal;">
					<li>la struttura organizzativa e gestionale dell’ente;</li> 
					<li>il contesto in cui opera l’ente;</li>
					<li>le attività dei programmi di servizio civile universale con particolare riferimento all’impiego degli operatori volontari;</li>
					<li>l’autovalutazione, da parte dell’ente, circa il raggiungimento degli obiettivi e il livello di soddisfazione degli operatori volontari.</li>
				</ul>
				<asp:CheckBox runat="server" ID="chkRapportoAnnuale" />
				<strong>Accetto</strong>
			</div>

            <asp:HiddenField ID="HFDelibera" runat="server" />
            <div class="row" style="display:none">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblRichiedenteAccount" CssClass="label" AssociatedControlID="txtRichiedente"
                        runat="server" Text="(*)Richiedente Account"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 20%">
                    <asp:TextBox ID="txtRichiedente" CssClass="textbox" runat="server"></asp:TextBox>
                </div>
                <div class="collable" style="width: 10%">
                    <asp:Label ID="lblUtenza" CssClass="label" AssociatedControlID="txtUtenza" runat="server"
                        Text="Utenza"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 25%">
                    <asp:TextBox ID="txtUtenza" CssClass="textbox" runat="server"></asp:TextBox>
                </div>

            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblDataCostituzione" CssClass="label" AssociatedControlID="txtDataCostituzione"
                        runat="server" Text="Data Costituzione dell'ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 20%">
                    <asp:TextBox ID="txtDataCostituzione" CssClass="textbox" runat="server" MaxLength="10"
                        Width="100px"></asp:TextBox>
                </div>
                <div class="collable" style="width: 10%">
                    <asp:Label ID="lblTelefono" CssClass="label" AssociatedControlID="txtTelefono" runat="server"
                        Text="(*)Telefono"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 20%">
                    <asp:TextBox ID="txtprefisso" runat="server" CssClass="textbox" Width="50px"></asp:TextBox>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="textbox" Width="120px"></asp:TextBox>
                </div>
                <div class="collable" style="width: 10%">
                    <asp:Label ID="lblFax" CssClass="label" AssociatedControlID="txtFax" runat="server"
                        Text="Fax"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 25%">
                    <asp:TextBox ID="txtprefissofax" runat="server" CssClass="textbox" Width="50px"></asp:TextBox>
                    <asp:TextBox ID="txtFax" runat="server" CssClass="textbox" Width="120px"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width: 35%">
                    <asp:Label ID="lblEstremiDSG" CssClass="label" AssociatedControlID="txtEstremiDSG"
                        runat="server" Text="Estremi Delibera Struttura Gestione"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 20%">
                    <asp:TextBox ID="txtEstremiDSG" CssClass="textbox" runat="server" TextMode="MultiLine"
                        Rows="6"></asp:TextBox>
                </div>
            </div>
            <br />
            <br />
            <div class="row" runat="server" ID="rowNoDelibera">
                <div class="colOggetti" style="width: 45%">
                    <asp:Label runat="server" ID="Label5" Text="(*)Nomina della struttura di gestione"/>
                </div>
                <div class="colOggetti" style="width: 15%">
                    <asp:Button ID="cmdCaricaDelibera" CssClass="Pulsante" runat="server" Text="Carica Nomina della struttura di gestione" />
                </div>
            </div>

            <div class="row" runat="server" id="rowDelibera" visible="false">
                <table class="table" cellspacing="0" cellpadding="2" rules="all" border="1" id="Table1"
                    style="font-size: Small; width: 100%; border-collapse: collapse;">
                    <caption>
                        Nomina della struttura di gestione
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
                                <asp:ImageButton runat="server" ID="btnDownloadDelibera" ImageUrl="images/giu_small.png"
                                    AlternateText="Scarica Nomina della struttura di gestione" ToolTip="Scarica Nomina della struttura di gestione" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtDeliberaFilename"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtDeliberaData"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="txtDeliberaHash"></asp:Literal>
                            </td>
                            <td>
                                <asp:ImageButton runat="server" ID="btnModificaDelibera"
                                    ImageUrl="images/ripristina_small.png" AlternateText="Modifica Nomina della struttura di gestione"
                                    ToolTip="Modifica Nomina della struttura di gestione" />
                                <asp:ImageButton runat="server" ID="btnEliminaDelibera" ImageUrl="images/canc_small.png"
                                    AlternateText="Elimina Nomina della struttura di gestione" ToolTip="Elimina Nomina della struttura di gestione" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div class="RigaVuota">
                &nbsp;</div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblClasseRichiestaLabel" CssClass="label" AssociatedControlID="ddlClassi"
                        runat="server" Text="Sezione Richiesta"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <div class="wrapper" style="width: 100%">
                        <div class="row" style="height: 40px">
                            <div class="colOggetti" style="width: 100%">
                                <asp:DropDownList ID="ddlClassi" runat="server" CssClass="ddlClass" AutoPostBack="true"
                                    Width="92%">
                                </asp:DropDownList>
                                <asp:ImageButton ID="infoClassi" runat="server" AlternateText="Informazioni Classi"
                                    ImageUrl="Images/info_small.png" Style="width: 20px; height: 20px; vertical-align: middle"
                                    Visible="False" />
                            </div>
                        </div>
                        <div class="row" style="height: 20px">
                            <div class="collable" style="width: 50%">
                                &nbsp;</div>
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="lblMin" CssClass="bold" runat="server" Text="Min"></asp:Label>
                            </div>
                            <div class="collable" style="width: 25%">
                                <asp:Label ID="lblMax" CssClass="bold" runat="server" Text="Max"></asp:Label>
                            </div>
                        </div>
                        <div class="row" style="height: 20px">
                            <div class="collable" style="width: 50%">
                                <asp:Label ID="lblNumeroSedi" CssClass="bold" runat="server" Text="Numero Sedi"></asp:Label>
                            </div>
                            <div class="collable" style="width: 5%; text-align: right">
                                <asp:Label ID="lblminSedi" CssClass="bold" runat="server"></asp:Label>
                            </div>
                            <div class="collable" style="width: 26%; text-align: right">
                                <asp:Label ID="lblmaxsedi" CssClass="bold" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row" style="height: 20px">
                            <div class="collable" style="width: 50%">
                                <asp:Label ID="lblNumeroVolontari" CssClass="bold" runat="server" Text="Numero Volontari"></asp:Label>
                            </div>
                            <div class="collable" style="width: 5%; text-align: right">
                                <asp:Label ID="lblminVol" CssClass="bold" runat="server"></asp:Label>
                            </div>
                            <div class="collable" style="width: 26%; text-align: right">
                                <asp:Label ID="lblmaxVol" CssClass="bold" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="RigaVuota">
                &nbsp;</div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblHttp" CssClass="label" AssociatedControlID="txthttp" runat="server"
                        Text="(*)Http"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:TextBox ID="txthttp" CssClass="textbox" runat="server" Width="70%" onchange="OnChangeHttp()"></asp:TextBox>
                    <asp:Button ID="cmdHttp" CssClass="Pulsante" runat="server" Text="test" ToolTip="Verifica http Valido"
                        Height="50%" />
                </div>
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblDelHttp" CssClass="label" AssociatedControlID="txtdataControllohttp"
                        runat="server" Text="del"></asp:Label>
                    <asp:Label ID="txtdataControllohttp" CssClass="bold" runat="server"></asp:Label>
                </div>
                <div class="collable" style="width: 10%">
                    <asp:CheckBox ID="chkVerohttp" Text="Vero" runat="server" />
                </div>
                <div class="collable" style="width: 10%">
                    <asp:CheckBox ID="chkFalsohttp" Text="Falso" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblEmailOrdinaria" CssClass="label" AssociatedControlID="txtemail"
                        runat="server" Text="(*)E-mail Ordinaria"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:TextBox ID="txtemail" CssClass="textbox" runat="server" Width="70%" onchange="OnChangeEmail()"></asp:TextBox>
                    <asp:Button ID="cmdEmail" CssClass="Pulsante" runat="server" Text="test" ToolTip="Verifica EMail Valida"
                        Height="50%" />
                </div>
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblDelEmail" CssClass="label" AssociatedControlID="txtdataControlloEmail"
                        runat="server" Text="del"></asp:Label>
                    <asp:Label ID="txtdataControlloEmail" CssClass="bold" runat="server"></asp:Label>
                </div>
                <div class="collable" style="width: 10%">
                    <asp:CheckBox ID="chkVeroEmail" Text="Vero" runat="server" />
                </div>
                <div class="collable" style="width: 10%">
                    <asp:CheckBox ID="chkFalsoEmail" Text="Falso" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblPEC" CssClass="label" AssociatedControlID="txtEmailpec" runat="server"
                        Text="PEC"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:TextBox ID="txtEmailpec" CssClass="textbox" runat="server" Width="70%"></asp:TextBox>
                    <asp:ImageButton ID="imgStatoPec" runat="server" ImageUrl="Images/valida_small.png"
                        Style="width: 20px; height: 20px; vertical-align: middle" Enabled="false" visible="false"/>
                </div>
                <div class="collable" style="width: 15%">
                    &nbsp;</div>
                <div class="collable" style="width: 25%">
                    <asp:CheckBox ID="ChkFirma" Text="Firma Elettronica" runat="server" Enabled="False" />
                </div>
            </div>
            <br />
            <p style="text-align: center">
                <asp:Label ID="lblSedeLegale" runat="server" CssClass="bold" Text="(*)Sede Legale Ente"></asp:Label>
            </p>
            <div class="wrapper" style="width: 100%">
                <div class="RigaVuota">
                    &nbsp;</div>
                <div class="row">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblProvincia" CssClass="label" AssociatedControlID="ddlProvincia"
                            runat="server" Text="(*)Provincia/Nazione"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 25%">
                        <asp:DropDownList ID="ddlProvincia" CssClass="ddlClass" AutoPostBack="true" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="colOggetti" style="width: 10%">
                        <asp:CheckBox ID="ChkEstero" AutoPostBack="true" Text="Estero" ToolTip="Flag Estero"
                            runat="server" />
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblComune" CssClass="label" AssociatedControlID="ddlComune" runat="server"
                            Text="(*)Comune"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 35%">
                        <asp:DropDownList ID="ddlComune" CssClass="ddlClass" runat="server">
                            <asp:ListItem Value="0" Text=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblIndirizzo" CssClass="label" runat="server" Text="(*)Indirizzo" AssociatedControlID="txtIndirizzo"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 35%">
                        <asp:TextBox ID="txtIndirizzo" runat="server" CssClass="textbox" onkeyup="javascript: SetContextKey();"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtenderIndirizzo" TargetControlID="txtIndirizzo"
                            ContextKey="ddlComune" CompletionListCssClass="ddl_Autocomplete" UseContextKey="true"
                            CompletionInterval="100" EnableCaching="false" runat="server" MinimumPrefixLength="5"
                            ServiceMethod="GetCompletionList">
                        </asp:AutoCompleteExtender>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblNumeroCivico" runat="server" Text="(*)Numero civico" AssociatedControlID="txtCivico"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 10%">
                        <asp:TextBox ID="txtCivico" runat="server" CssClass="textbox"></asp:TextBox>
                    </div>
                    <div class="collable" style="width: 5%">
                        <asp:ImageButton ID="ImageButton1" runat="server" AlternateText="Informazioni Numero Civico"
                            ImageUrl="Images/info_small.png" Style="width: 20px; height: 20px" ToolTip="NEL CAMPO CIVICO E' POSSIBILE INSERIRE SOLO I SEGUENTI FORMATI:
                                - 21
                                - 21/A 
                                - 21/A5 
                                - 21 BIS 
                                - KM 21,500 
                                OPPURE IL VALORE SNC" disabled="disabled" />
                    </div>
                    <div class="collable" style="width: 10%">
                        <asp:Label ID="lblCAP" CssClass="label" runat="server" Text="(*)C.A.P." AssociatedControlID="txtCAP"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 5%">
                        <asp:TextBox ID="txtCAP" CssClass="textbox" runat="server"></asp:TextBox>
                    </div>
                    <div class="collable" style="width: 5%">
                        <asp:ImageButton ID="imgCap" runat="server" title="Seleziona il Cap" ImageUrl="Images/valida_small.png"
                            AlternateText="Seleziona il Cap" Style="width: 20px; height: 20px" />
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblDettaglioRecapito" CssClass="label" runat="server" Text="Dettaglio recapito"
                            AssociatedControlID="TxtDettaglioRecapito"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 80%">
                        <asp:TextBox ID="TxtDettaglioRecapito" CssClass="textbox" runat="server" ToolTip="Dettagli aggiuntivi del recapito"
                            TextMode="MultiLine" Rows="2" Width="99%"></asp:TextBox>
                    </div>
                    <div class="collable" style="width: 5%">
                        <asp:ImageButton ID="imgRecapito" runat="server" AlternateText="Informazioni Recapito"
                            ImageUrl="Images/info_small.png" Style="width: 20px; height: 20px" ToolTip="ATTENZIONE!!! Nel campo Dettaglio Recapito  devono essere indicate ESCLUSIVAMENTE informazioni AGGIUNTIVE, se presenti, all' indirizzo per consentire una maggiore accuratezza nelle spedizioni postali. NON VA PERTANTO RIPETUTO L'INDIRIZZO (per il quale già esiste il campo specifico) e NON VANNO INSERITI RECAPITI TELEFONICI o altre informazioni non attinenti. Si fa presente che questo campo NON E' OBBLIGATORIO e quindi VA LASCIATO VUOTO se non è necessario indicare informazioni aggiuntive al recapito relativo."
                            disabled="disabled" />
                    </div>
                </div>
            </div>
            <div class="wrapper" style="width: 100%; border-style: none">
                <div class="RigaVuota">
                    &nbsp;</div>
                <div class="RigaPulsanti">
                    <asp:Button ID="CmdRescissione" CssClass="Pulsante" runat="server" Text="Rescissione"
                        Visible="False" />
                    <asp:Button ID="cmdAnnullaModifica" runat="server" CssClass="Pulsante" Text="Annulla Modifica" />
                    <asp:Button ID="cmdVisualizzaDatiAccreditati" runat="server" CssClass="Pulsante"
                        Text="Visualizza Dati Accreditati" />
                    <asp:Button ID="cmdPdfSettori" runat="server" CssClass="Pulsante" Text="Pdf Settori" Visible="false"/>
                    <asp:Button ID="CmdSalva" runat="server" CssClass="Pulsante" Text="Salva" />
                    <asp:Button ID="CmdSalvaDoc" runat="server" Visible="false" CssClass="Pulsante" 
                        Text="Salva Modifiche Art.2/Art.10"/>
                    <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
                </div>
            </div>
            <asp:HiddenField ID="lblTipoUtente" runat="server" />
            <asp:HiddenField ID="lblInPartenariato" runat="server" />
            <asp:HiddenField ID="lblIdClasse" runat="server" />
            <asp:HiddenField ID="lblIdEnte" runat="server" />
            <asp:HiddenField ID="txtBlocco" runat="server" />
            <asp:HiddenField ID="txtCodFisArchiviato" runat="server" />
            <asp:HiddenField ID="txtpianolocale" runat="server" />
            <asp:HiddenField ID="ChkPec" runat="server" />
            <asp:HiddenField ID="ChkFirmaPrima" runat="server" />
            <asp:HiddenField ID="txtidClasseRichiesta" runat="server" />
            <asp:HiddenField ID="txtIDComunes" runat="server" />
            <asp:HiddenField ID="txtIDComune" runat="server" />
            <asp:HiddenField ID="txtIdSede" runat="server" />
        </div>

        

        <!-- INIZIO POPUP Maschera -->
        <asp:ModalPopupExtender runat="server" ID="mpe_EsperienzaAree" TargetControlID="hfRigaSelezionata"
            PopupControlID="pnlEsperienzaArea" BackgroundCssClass="modalBackgroundPopUp"
            DropShadow="True" PopupDragHandleControlID="pnlEsperienzaArea" DynamicServicePath=""
            Enabled="True">
        </asp:ModalPopupExtender>
        <asp:Panel ID="pnlEsperienzaArea" runat="server" Style="background-color: White;
            padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
            <div class="wrapper" style="width: 100%">
                <div class="headers">
                    <h2>
                        <asp:Label ID="lblTitoloEsperienzaArea" runat="server" Text="Dettaglio Settore"></asp:Label></h2>
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
        <!--POPUP File--Atto Nomina Inizio-->
        <asp:ModalPopupExtender runat="server" ID="mpeAttoNomina" TargetControlID="HFRP" 
            PopupControlID="pnlUploadAttoNomina" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
            PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpAttoNomina" DynamicServicePath=""
            Enabled="True" CancelControlID="btnCloseAttoNominaUpload"> 
        </asp:ModalPopupExtender>
        <asp:Panel ID="pnlUploadAttoNomina" runat="server" Style="background-color: White;
            padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
            <p class="msgInfo">
                Caricamento Atto Nomina/Altro del Rappresentante Legale</p>
            <asp:Label ID="lblErroreAttoNomina" runat="server" CssClass="msgErrore" AssociatedControlID="cmdAllegaAttoNomina"></asp:Label>
            <fieldset>
                <input id="fileAN" type="file" style="width: 95%" runat="server" onclick="return txtSelFile_onclick()" />
                <div style="margin-top: 1em;">
                </div>
                <asp:Button ID="cmdAllegaAttoNomina" runat="server" CssClass="Pulsante" Text="Carica Atto Nomina/Altro"/>
                <asp:Button ID="btnCloseAttoNominaUpload" runat="server" CssClass="Pulsante" Text="Chiudi"/>
            </fieldset>
        </asp:Panel>
        <!--POPUP File--Atto Nomina Fine-->

        <!--POPUP File--Delibera organi decisionali Inizio-->
        <asp:ModalPopupExtender runat="server" ID="mpeDelibera" TargetControlID="HFDelibera" 
            PopupControlID="pnlUploadOrganiDecisionali" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
            PopupDragHandleControlID="pnlEsperienzaArea" DynamicServicePath="" Enabled="True"
            CancelControlID="btnCloseDeliberaUpload"> 
        </asp:ModalPopupExtender>
        <asp:Panel ID="pnlUploadOrganiDecisionali" runat="server" Style="background-color: White;
            padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
            <p class="msgInfo">
                Nomina della struttura di gestione</p>
            <asp:Label ID="lblErroreUploadDelibera" runat="server" CssClass="msgErrore" AssociatedControlID="cmdAllegaDelibera"></asp:Label>
            <fieldset>
                <input id="fileDelibera" type="file" style="width: 95%" runat="server" onclick="return txtSelFile_onclick()" />
                <div style="margin-top: 1em;">
                </div>
                <asp:Button ID="cmdAllegaDelibera" runat="server" CssClass="Pulsante" Text="Carica File" />
                <asp:Button ID="btnCloseDeliberaUpload" runat="server" CssClass="Pulsante" Text="Chiudi"/>
            </fieldset>
        </asp:Panel>
        <!--POPUP File--Delibera organi decisionali Fine-->
        <%--asp:ModalPopupExtender runat="server" ID="mpeRTDP" TargetControlID="HFRTDP" 
            PopupControlID="pnlUploadRTDP" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
            PopupDragHandleControlID="pnlEsperienzaArea" DynamicServicePath="" Enabled="True"
            CancelControlID="btnCloseRTDP"> 
        </asp:ModalPopupExtender>
        <asp:Panel ID="pnlUploadRTDP" runat="server" Style="background-color: White;
            padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
            <asp:Label ID="Label9" runat="server" CssClass="msgErrore" Visible="False" AssociatedControlID="cmdAllegaRTDP"></asp:Label>
            <p class="msgInfo">Atto di designazione del Responsabile del trattamento dei dati personali</p>
            <fieldset>
                <input id="fileRTDP" type="file" style="width: 95%" runat="server" onclick="return txtSelFile_onclick()" />
                <div style="margin-top: 1em;">
                </div>
                <asp:Button ID="cmdAllegaRTDP" runat="server" CssClass="Pulsante" Text="Carica File" />
                <asp:Button ID="btnCloseRTDP" runat="server" CssClass="Pulsante" Text="Chiudi"/>
            </fieldset>
        </asp:Panel>--%>
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
                <input id="fileAttoCostitutivo" type="file" style="width: 95%" runat="server" onclick="return txtSelFile_onclick()" />
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
                <input id="fileStatuto" type="file" style="width: 95%" runat="server" onclick="return txtSelFile_onclick()" />
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
                <input id="fileDeliberaAdesione" type="file" style="width: 95%" runat="server" onclick="return txtSelFile_onclick()" />
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
                <input id="fileImpegnoEtico" type="file" style="width: 95%" runat="server" onclick="return txtSelFile_onclick()" />
                <div style="margin-top: 1em;">
                </div>
                <asp:Button ID="cmdAllegaIE" runat="server" CssClass="Pulsante" Text="Carica File" />
                <asp:Button ID="btnCloseImpegnoEtico" runat="server" CssClass="Pulsante" Text="Chiudi"/>
            </fieldset>
        </asp:Panel>
        <!--POPUP File--Carta impegno etico Fine-->
    </fieldset>
</asp:Content>
