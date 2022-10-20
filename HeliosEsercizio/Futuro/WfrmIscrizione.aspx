<%@ Page Title="Gestione Vincoli Classe Ente" Language="vb" AutoEventWireup="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Site.Master" CodeBehind="WfrmIscrizione.aspx.vb" Inherits="Futuro.WfrmIscrizione" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>

<script type="text/javascript">
	function ShowPopUp(control) {
		$find(control).show();
		return false;
	}
	$(document).ready(function (){
        <%  If hfLettoPrivacy.Value <> True Then%>
        $("#<%=chkPrivacy.ClientID%> ").prop("disabled", true);
		$("#boxPrivacy").scroll(function (e) {
				e.preventDefault();
				var elem = $(this);
				if (elem.scrollTop() > 0 &&
					(elem[0].scrollHeight - elem.scrollTop() <= elem.outerHeight() + 2)) {
					$("#<%=chkPrivacy.ClientID%> ").prop("disabled", false);
			}
        });
        <% End If%>
	<%  If hfLettoImpegno.Value <> True Then%>
		$("#<%=chkImpegno.ClientID%> ").prop("disabled", true);
		$("#boxImpegno").scroll(function (e) {
			e.preventDefault();
			var elem = $(this);
			if (elem.scrollTop() > 0 &&
				(elem[0].scrollHeight - elem.scrollTop() <= elem.outerHeight() + 2)) {
				$("#<%=chkImpegno.ClientID%> ").prop("disabled", false);
			}
		});
        <% End If%>
	});

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
	<asp:HiddenField runat="server" ID="txtCodiceRegione" />
    <fieldset class="ContornoPagina" id="FidsetPagina" runat="server">
        <legend>Presentazione domanda di Iscrizione</legend>
            <div class="wrapper" style="width:100%;border:0px">
            <asp:Label runat="server" ID="lblError" CssClass ="msgErrore"></asp:Label>
            <asp:HiddenField runat="server" ID="hfCodiceFiscaleEnte" />
			<h5>L'ente <strong><asp:Label runat="server" ID="txtEnte"></asp:Label></strong></h5>
			<br />
			<asp:CheckBox runat="server" ID="chkRichiestaIscrizione" />
			<strong>Chiede di essere iscritto alla Sezione <asp:Label runat="server" ID="txtSezione"></asp:Label> dell'Albo degli enti di servizio civile universale per n <asp:Literal runat="server" ID="txtNumeroSedi" ></asp:Literal> sedi di attuazione di progetto</strong>
            <hr />
			<div id="boxPrivacy" class="boxPrivacy">
                <asp:Literal runat="server" ID="txtPrivacy"></asp:Literal>
            </div>
            <asp:LinkButton runat="server" ID="lnkPrivacy" Text="Scarica l'informativa sulla Privacy" OnClick="lnkDownload_Click" ></asp:LinkButton>
            <asp:label runat="server" ID="hashPrivacy"/>
			<br />
            <asp:CheckBox runat="server" ID="chkPrivacy" OnCheckedChanged="chkPrivacy_CheckedChanged"/>
            <strong>Letta l'informativa sulla privacy</strong>
            <asp:HiddenField runat="server" ID="hfLettoPrivacy" Value="false"/>
			<br />

			<div id="boxImpegno" class="boxPrivacy">
                <asp:Literal runat="server" ID="txtImpegno"></asp:Literal>
            </div>
            <asp:LinkButton runat="server" ID="lnkImpegno" Text="Scarica la dichiarazione d'impegno" OnClick="lnkDownload_Click" ></asp:LinkButton>
            <asp:label runat="server" ID="hashImpegno"/>
            <br /><br />
            <asp:CheckBox runat="server" ID="chkImpegno" OnCheckedChanged="chkImpegno_CheckedChanged" />
			<strong>Accettata la dichiarazione d'impegno</strong>
            <asp:HiddenField runat="server" ID="hfLettoImpegno" Value="false"/>
<%--			<hr />
			<asp:CheckBox runat="server" ID="chkFiniIstituzionali" />
			<strong>Dichiaro che vi è corrispondenza tra i propri fini istituzionali e le finalità di cui all’art. 1 della legge n. 64/2001 e all’art. 2 del d.lgs. n.40 del 2017</strong>
			<br /><br />
			<div runat="server" id="divStatuto">
				<asp:CheckBox runat="server" ID="chkStatuto" />
				<strong>Dichiaro di essere da Statuto senza scopo di lucro</strong>
				<br /><br />
			</div>
			<asp:CheckBox runat="server" ID="chkAttivita" />
			<strong>Dichiaro di svolgere attività nel/i settore/i richiesto da almeno 3 anni</strong>--%>
			<br /><br />

			<hr />
			<h4>Domanda di Iscrizione</h4>
			<p>È necessario scaricare la domanda di iscrizione, apporre la firma digitale e ricaricare il documento firmato</p>
            <asp:Button runat="server" Text="Scarica domanda di iscrizione" Id="Riepilogo" CssClass="Pulsante"/>
			<br /><br />
			<div class="row" runat="server" id="NorowRiepilogo">
				<div class="collable" style="width: 25%">
					<asp:Label ID="lblRiepilogo" CssClass="label" AssociatedControlID="cmdCaricaFileRiepilogo" runat="server" Text="Domanda di iscrizione Firmata"></asp:Label>
				</div>
                <div class="colOggetti" style="width: 35%">
                    <asp:Button ID="cmdCaricaFileRiepilogo" runat="server" OnClientClick="return ShowPopUp('popUpControlRiepilogo')" CssClass="Pulsante" Text="Carica File"/> 
				</div>
			</div>
			<div class="row" runat="server" id="rowRiepilogo">
				<table class="table" cellspacing="0" cellpadding="2" rules="all" border="1" id="MainContent_dtgConsultaDocumenti" style="font-size: Small; width: 100%; border-collapse: collapse;">
					<caption>
						Domanda di iscrizione
					</caption>
					<tbody>
						<tr>
							<th scope="col">Download</th>
							<th scope="col">Nome File</th>
							<th scope="col">Data Inserimento</th>
							<th scope="col">Hash Value</th>
							<th scope="col"></th>
						</tr>
						<tr class="tr" align="center" runat="server" id="rowAllegato">
							<td>
								<asp:ImageButton runat="server" ID="btnDownloadCV" ImageUrl="images/giu_small.png" AlternateText="Scarica Domanda di Iscrizione" ToolTip="Scarica Domanda di Iscrizion" OnClick="btnDownloadRiepilogo_Click"/>
							</td>
							<td>
								<asp:Literal runat="server" ID="txtCVFilename"></asp:Literal>
							</td>
							<td>
								<asp:Literal runat="server" ID="txtCVData"></asp:Literal></td>
							<td>
								<asp:Literal runat="server" ID="txtCVHash"></asp:Literal></td>
							<td>
								<asp:ImageButton runat="server" ID="btnModificaRiepilogo" OnClientClick="return ShowPopUp('popUpControlRiepilogo')" ImageUrl="images/ripristina_small.png" AlternateText="Modifica Curriculum Vitae" ToolTip="Modifica Curriculum Vitae" />
								<asp:ImageButton runat="server" ID="btnEliminaRiepilogo" ImageUrl="images/canc_small.png" AlternateText="Elimina Domanda" ToolTip="Elimina Domanda" OnClick="btnDeleteRiepilogo_Click" />
							</td>
						</tr>
					</tbody>
				</table>
			</div>
			<hr />

			<div class="RigaPulsanti">
				<asp:Button Text="Presenta" runat="server" Id="btnPresenta" CssClass="Pulsante" OnClick="btnPresenta_Click"/>
			</div>
		</div>
		</fieldset>


    <%--POPUP File--%>
    <asp:ModalPopupExtender runat="server" ID="popUploadRiepilogo" TargetControlID="hfPopupRiepilogo" 
        PopupControlID="pnlUploadRiepilogo" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
        BehaviorID="popUpControlRiepilogo" Enabled="True" CancelControlID="btnCloseUploadRiepilogo"> 
    </asp:ModalPopupExtender>
	<asp:Panel ID="pnlUploadRiepilogo" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
        <asp:HiddenField runat="server" ID="hfPopupRiepilogo" />
        <asp:label id="lblErroreRiepilogo"  runat="server" CssClass="msgErrore" AssociatedControlID="btnUploadRiepilogo"></asp:label>
        <p class="msgInfo">Caricamento Domanda di Iscrizione</p>
        <fieldset>
			<input id="fileRiepilogo" type="file" runat="server" />
            <div style="margin-top: 1em;"></div>
            <asp:Button ID="btnUploadRiepilogo" runat="server" CssClass="Pulsante" Text="Carica File" OnClick="btnUploadRiepilogo_Click"/>
            <asp:Button ID="btnCloseUploadRiepilogo" runat="server" CssClass="Pulsante" Text="Chiudi"/>
        </fieldset>
	</asp:Panel>

</fieldset>
        
</asp:Content>
