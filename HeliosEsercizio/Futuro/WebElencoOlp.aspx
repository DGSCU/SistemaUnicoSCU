<%@ Page Title="Aggiungi/Modifica OLP" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WebElencoOlp.aspx.vb" Inherits="Futuro.WebElencoOlp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    	<script type="text/javascript">
    	    function check() {
    	        var conferma = document.getElementById("<%:txtconferma.ClientId%>");
    	        conferma.value = 'Conferma';
    	    }
		</script>
        <script type="text/javascript">
            function checkCV() {
                var conferma = document.getElementById("<%:txtconferma.ClientId%>");
                conferma.value = 'Conferma';
            }

            function showPopupAssegnaRisorsa() { $find('popUpControlAssegnaRisorsa').show(); return false; }

            function hidePopupAssegnaRisorsa() { $find('popUpControlAssegnaRisorsa').hide(); }

            function showPopUpCV() { $find('popUpControlCV').show(); return false; }

		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
<fieldset class="ContornoPagina">
<legend>Aggiungi/Modifica Risorse</legend>
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:Label ID="lblIntProgetto" runat="server"  Text="Aggiungi/Modifica: "></asp:Label>
      <asp:label  id="lblRuolo" style="vertical-align:middle"  Text="&nbsp;" runat="server"></asp:label>
      <asp:label  id="LabelAP" style="vertical-align:middle"  Text="assegnati - Progetto: '" runat="server"></asp:label>
      <asp:label  id="lblProgetto" style="vertical-align:middle"  Text="&nbsp;" runat="server"></asp:label>
      <asp:label  id="lblApiceFineProgetto" style="vertical-align:middle" Text="'" runat="server"></asp:label>
       <asp:label  id="apice" style="vertical-align:middle"  Text=" .Sede" runat="server"></asp:label>
       <asp:label  id="lblSede" style="vertical-align:middle"  Text="&nbsp;" runat="server"></asp:label>
      </h2>
    </div>
  
    <div class="rowGroup" style="height:auto">
        <div class="RigaVuota">
      &nbsp;</div>
         <asp:label id="lblErrore"  runat="server" CssClass="msgErrore" ></asp:label>
          <asp:label id="lblMessaggioConferma"  runat="server" CssClass="msgConferma" ></asp:label>
         <asp:label id="lblMessaggioInfo"  runat="server" CssClass="msgInfo" ></asp:label>
         <asp:HiddenField ID="lblidsedeattuazione" runat="server" />
         <asp:HiddenField ID="lblidSede" runat="server" />
         <asp:HiddenField ID="lblidattEs" runat="server" />
         <asp:HiddenField ID="lblidAttivita" runat="server" />
         <asp:HiddenField ID="lblModifica" runat="server" />
          <asp:HiddenField ID="txtconferma" runat="server" />
          <asp:HiddenField ID="txtpage" runat="server" />
          
        <div class="RigaVuota">
      &nbsp;</div>
    </div>
    
    <div class="row" >
    
        <div class="colOggetti" style="width:15%">
            <asp:Label ID="lblCognome" CssClass="label" AssociatedControlID="txtCognome"  runat="server" Text="Cognome"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%" >
            <asp:TextBox ID="txtCognome"  CssClass="textbox" runat="server"></asp:TextBox>
        </div>
          <div class="collable" style="width:15%">
           <asp:Label ID="lblNome" CssClass="label" AssociatedControlID="txtNome" runat="server" Text="Nome"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:TextBox  ID="txtNome" CssClass="textbox"   runat="server"></asp:TextBox>       
       </div>
    </div>

    <div class="row" >  
        <div class="colOggetti" style="width:15%">
            <asp:Label ID="lblComuneNascita" CssClass="label" AssociatedControlID="txtcomuneNascita"  runat="server" Text="Comune di Nascita"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%" >
            <asp:TextBox ID="txtcomuneNascita" CssClass="textbox" runat="server"></asp:TextBox>
        </div>
        <div class="colOggetti" style="width:50%">
            <asp:CheckBox ID="chkVisualizzaOlp"  runat="server" Text="Visualizza solo Risorse assegnate" Checked="True"></asp:CheckBox>
        </div>
    </div>

    <div class="RigaPulsanti"> 
     <asp:Button id="cmdSalva" CssClass="Pulsante"  runat="server" Text="Ricerca" ></asp:Button>
        <asp:Button id="imgNuovo" CssClass="Pulsante"  runat="server" Text="Nuova Risorsa" ></asp:Button>&nbsp;

    </div>
    <div class="RigaVuota">&nbsp;</div>
    

<asp:datagrid id="dgRisultatoRicerca" Caption="Risultato Ricerca Risorse Progetto"   runat="server" Width="100%" ToolTip="Risorse Progetto" 
              AllowPaging="true" PageSize="5"  CssClass="table" AllowSorting="false"  AutoGenerateColumns="False" UseAccessibleHeader="True" 
              Visible="true">
	<FooterStyle></FooterStyle>
	<SelectedItemStyle  BackColor="White" ></SelectedItemStyle>
	<EditItemStyle></EditItemStyle>
	<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	<ItemStyle CssClass="tr" HorizontalAlign ="Center"></ItemStyle>
	<HeaderStyle></HeaderStyle>
    <Columns>
			<asp:BoundColumn DataField="nominativo" HeaderText="Nominativo">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="datanascita" HeaderText="Data Nascita" DataFormatString="{0:d}">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="denominazione" HeaderText="Comune Nascita">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="telefono" HeaderText="Telefono">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="email" HeaderText="Email">
			</asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Risorsa assegnata">
				<ItemTemplate>
					<asp:CheckBox id="check1" onclick="javascript:check()" Text="&nbsp;"  toolTip="OLP" AutoPostBack="False"
						runat="server" Enabled="false"></asp:CheckBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn Visible="False" DataField="idass" HeaderText="idAss">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="accreditato" HeaderText="Accreditato"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="identePersonaleRuolo" HeaderText="identePersonaleRuolo"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="identesedeattuazione" HeaderText="idSedeAttuazione"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="trovaChec" HeaderText="trovaChec"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Corso OLP da frequentare">
				<ItemTemplate>
					SI<asp:CheckBox id="chkCorsoOlp" toolTip="Corso OLP SI" Text="&nbsp;"  AutoPostBack="true" runat="server" Enabled="false"></asp:CheckBox>
                    NO<asp:CheckBox id="chkCorsoOlpNo" toolTip="Corso OLP NO" Text="&nbsp;"  AutoPostBack="true" runat="server" Enabled="false"></asp:CheckBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn Visible="False" DataField="CorsoOLP" HeaderText="CorsoOLP"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="DataFine" HeaderText="entepersonaleruoli.datafinevalidit&#224;"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="UserNameInseritore" HeaderText="UserNameInseritore"></asp:BoundColumn>
            <asp:TemplateColumn HeaderText="Caricamento CV (*)" Visible="false">
				<ItemTemplate>
					<asp:CheckBox id="ChkCaricamentoCV" onclick="javascript:checkCV()" Text="&nbsp;" Checked="true"  toolTip="(*)CaricamentoCV" AutoPostBack="False"
						runat="server"></asp:CheckBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="DataInseritore" HeaderText="Data Inserimento" DataFormatString="{0:d}"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="ControlloUsabilitaOLP" HeaderText="ControlloUsabilitaOLP"></asp:BoundColumn>
             <asp:TemplateColumn  > 
            <ItemTemplate >
				<asp:ImageButton ID="InfoRisorsa" CommandName="Select" style="cursor:pointer" AlternateText="Verifica OLP" ToolTip="Verifica OLP" runat="server" ImageUrl="images/info_small.png" />
                </ItemTemplate>
        </asp:TemplateColumn>
			<asp:BoundColumn Visible="False" DataField="codicefiscale" HeaderText="codicefiscale"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="ControlloUsabilitaRLEA" HeaderText="ControlloUsabilitaRLEA"></asp:BoundColumn>
            <asp:TemplateColumn  > 
				<ItemTemplate >
					<asp:ImageButton ID="SostOLP" CommandName="SostOLP" style="cursor:pointer" AlternateText="Sostituisci OLP" ToolTip="Sostituisci OLP" runat="server" ImageUrl="images/ripristina_small.png" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn Visible="False" DataField="inCorso" HeaderText="inCorso"></asp:BoundColumn>
            <asp:TemplateColumn  > 
				<ItemTemplate >
					<asp:ImageButton ID="AssOLP" CommandName="AssOLP" style="cursor:pointer" AlternateText="Assegna Risorsa" ToolTip="Assegna Risorsa" runat="server" ImageUrl="images/selezionato_small.png" />
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn  > 
				<ItemTemplate >
					<asp:ImageButton ID="CancOLP" CommandName="CancOLP" style="cursor:pointer" AlternateText="Elimina Risorsa" ToolTip="Elimina Risorsa" runat="server" ImageUrl="images/canc_small.png" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn Visible="False" DataField="ControlloUsabilitaCV" HeaderText="ControlloUsabilitaCV"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="EsistenzaCV" HeaderText="EsistenzaCV"></asp:BoundColumn>
        </Columns>
		<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  
                    Mode="NumericPages"></PagerStyle>
</asp:datagrid>
    <p>
        <asp:Label ID="lblNota" CssClass="label" runat="server" 
            Text="(*)Nota: Si fa presente che per il campo 'Caricamento CV' deselezionando la spunta si dichiara che l’OLP ha già operato su progetti del medesimo settore e non si intende caricare sul sistema il suo CV in quanto già acquisito in passato. " 
            Visible="False"></asp:Label>
    </p>
            


       <div class="RigaVuota">&nbsp;</div>
       <div class="RigaPulsanti">
            <asp:Button id="cmdConferma" CssClass="Pulsante"  runat="server" Text="Conferma" ></asp:Button>
            <asp:Button id="cmdChiudi" CssClass="Pulsante"  runat="server" Text="Chiudi" ></asp:Button>
        </div>
    </div>
</fieldset>
    <!--POPUP Assegna Risorsa Inizio-->
    <asp:ModalPopupExtender runat="server" ID="popAssegnaRisorsa" TargetControlID="hfAssegnaRisorsa" 
        PopupControlID="pnlAssegnaRisorsa" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
        PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpControlAssegnaRisorsa" DynamicServicePath="" Enabled="True"> 
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlAssegnaRisorsa" runat="server" Style="background-color: White; padding: 20px; width: 120vh; overflow-y: auto; max-height: 90vh">
        <asp:HiddenField runat="server" ID="hfAssegnaRisorsa" />
        <asp:label id="lblErroreAssegnaRisorsa"  runat="server" CssClass="msgErrore" Visible="False" AssociatedControlID="cmdAssegnaRisorsa"></asp:label>
        <p class="msgInfo" id="pMesg" runat="server">Assegna Risorsa</p>
        <fieldset>
            <legend id="lgContornoPagina" runat="server">Assegna Risorsa</legend>
            <div class="wrapper" style="width:100%">
                <div class="RigaVuota" >&nbsp;</div>
                <div class="row">
                    <div class="collable" style="width:15%">      
                    &nbsp;<asp:Label ID="Label1" CssClass="label" AssociatedControlID="txtProgetto" runat="server" Text="Progetto"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtProgetto" CssClass="textbox" runat="server" Enabled="false"></asp:TextBox>         
                    </div> 
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="Label2" CssClass="label" AssociatedControlID="txtSede" runat="server" Text="Sede"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtSede" CssClass="textbox" runat="server" Enabled="false"></asp:TextBox>         
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width:15%">
                    &nbsp;<asp:Label ID="Label3" CssClass="label" AssociatedControlID="txtNominativo" runat="server" Text="Nominativo"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtNominativo" CssClass="textbox" runat="server" Enabled="false"></asp:TextBox>         
                    </div> 
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="Label4" CssClass="label" AssociatedControlID="txtCodiceFiscale" runat="server" Text="Codice fiscale"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtCodiceFiscale" CssClass="textbox" runat="server" Enabled="false"></asp:TextBox>         
                    </div>
                    <asp:HiddenField runat="server" ID="hfIdAssociaEntePersonaleRuoliAttivitàEntiSediAttuazione" />
                    <asp:HiddenField runat="server" ID="hfIdEntePersonaleRuolo" />
                </div>
                <div class="row">
                    <div class="collable" style="width:15%">      
                    &nbsp;<asp:Label ID="lblDataNascita" CssClass="label" AssociatedControlID="txtDataNascita" runat="server" Text="Data di nascita"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtDataNascita" CssClass="textbox" runat="server" Width="75px" Enabled="false"></asp:TextBox>         
                    </div> 
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblComune" CssClass="label" AssociatedControlID="txtComune" runat="server" Text="Comune di nascita"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtComune" CssClass="textbox" runat="server" Enabled="false"></asp:TextBox>         
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width:15%">
                    &nbsp;<asp:Label ID="lblTelefono" CssClass="label" AssociatedControlID="txtTelefono" runat="server" Text="Telefono"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtTelefono" CssClass="textbox" runat="server" Enabled="false"></asp:TextBox>         
                    </div> 
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="Label5" CssClass="label" AssociatedControlID="txtEmail" runat="server" Text="Email"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtEmail" CssClass="textbox" runat="server" Enabled="false"></asp:TextBox>         
                    </div>
                </div>
               <div class="row">
                    <div class="collable" style="width:25%">
                    &nbsp;<asp:Label ID="lblCorsoOlp" CssClass="label" AssociatedControlID="checkCorsoOlpSI" runat="server" Text="Corso OLP da frequentare"></asp:Label>
                    </div>
                    <div class="collable" style="width:15%"">
					    <asp:CheckBox ID="checkCorsoOlpSI" AutoPostBack="false" Text="Corso OLP SI" ToolTip="Corso OLP SI" runat="server" /> 
                    </div>
                    <div class="collable" style="width:15%"">
					    <asp:CheckBox ID="checkCorsoOlpNO" AutoPostBack="false" Text="Corso OLP NO" ToolTip="Corso OLP NO" runat="server" /> 
                    </div>
               </div>
               <div class="row">
                    <div class="collable" style="width:25%">
                    &nbsp;<asp:Label ID="lblObbligoCV" CssClass="label" AssociatedControlID="chkObbligoCV" runat="server" Text="Obbligo caricamento CV (*)"></asp:Label>
                    </div>
                    <div class="collable" style="width:15%"">
                        <asp:CheckBox ID="chkObbligoCV" AutoPostBack="false" Text="" ToolTip="" runat="server" Enabled="false"/> 
                    </div>
               </div>
                <p id="idPobbligatorioCV" runat="server">(*) Sul sistema non risulta che l’OLP è associato a progetti positivamente valutati dello stesso settore. Il caricamento del CV in questo caso è obbligatorio.</p>
                <p id="idPnOobbligatorioCV" runat="server">(*) Sul sistema risulta che l’OLP è associato a progetti positivamente valutati dello stesso settore. Il caricamento del CV in questo caso non è obbligatorio.</p>
                <div class="row" runat="server" id="rowNoCV">
	                <div class="colOggetti" style="width: 35%">
		            &nbsp;<asp:Button ID="cmdCaricaFileCV" runat="server" OnClientClick="return showPopUpCV()" CssClass="Pulsante" Text="Carica Curriculum"/> 
	                </div>
                </div>
                <div class="row" runat="server" id="rowCV">
	                <table class="table" cellspacing="0" cellpadding="2" rules="all" border="1" id="Table1" style="font-size: Small; width: 100%; border-collapse: collapse;">
		                <caption>
			                Curriculum
		                </caption>
		                <tbody>
			                <tr>
				                <th scope="col">Download</th>
				                <th scope="col">Nome File</th>
				                <th scope="col">Data Inserimento</th>
				                <th scope="col">Hash Value</th>
				                <th scope="col"></th>
			                </tr>
			                <tr class="tr" align="center" runat="server" id="Tr1">
				                <td>
					                <asp:ImageButton runat="server" ID="btnDownloadCV" ImageUrl="images/giu_small.png" AlternateText="Scarica CV" ToolTip="Scarica CV" />
				                </td>
				                <td>
					                <asp:Literal runat="server" ID="txtCVFilename"></asp:Literal>
				                </td>
				                <td>
					                <asp:Literal runat="server" ID="txtCVData"></asp:Literal></td>
				                <td>
					                <asp:Literal runat="server" ID="txtCVHash"></asp:Literal></td>
				                <td>
					                <asp:ImageButton runat="server" ID="btnModificaCV" OnClientClick="return showPopUpCV()" ImageUrl="images/ripristina_small.png" AlternateText="Modifica CV" ToolTip="Modifica CV" />
					                <asp:ImageButton runat="server" ID="btnEliminaCV" ImageUrl="images/canc_small.png" AlternateText="Elimina CV" ToolTip="Elimina CV" />
				                </td>
			                </tr>
		                </tbody>
	                </table>
                </div>
                <div class="RigaPulsanti">
                    <asp:Button ID="cmdAssegnaRisorsa" runat="server" CssClass="Pulsante" Text="Assegna Risorsa"/>
                    <asp:Button ID="btnCloseAssegnaRisorsa" runat="server" CssClass="Pulsante" Text="Chiudi" OnClientClick="hidePopupAssegnaRisorsa()"/>
                </div>
            </div>
        </fieldset>
    </asp:Panel>
    <!--POPUP Assegna Risorsa Fine-->

    <!--POPUP File--CV Inizio-->
    <asp:ModalPopupExtender runat="server" ID="popUploadCV" TargetControlID="hfPopupCV" 
        PopupControlID="pnlUploadCV" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
        PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpControlCV" DynamicServicePath="" Enabled="True" CancelControlID="btnCloseUploadCV"> 
    </asp:ModalPopupExtender>
	<asp:Panel ID="pnlUploadCV" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh;border-style: solid;border-width:1px">
        <asp:HiddenField runat="server" ID="hfPopupCV" />
        <asp:label id="lblErroreUploadCV"  runat="server" CssClass="msgErrore" Visible="False" AssociatedControlID="cmdAllegaCV"></asp:label>
        <p class="msgInfo">Caricamento CV</p>
        <fieldset>
			<input id="fileCV" type="file" runat="server" />
            <div style="margin-top: 1em;"></div>
            <asp:Button ID="cmdAllegaCV" runat="server" CssClass="Pulsante" Text="Carica File"/>
            <asp:Button ID="btnCloseUploadCV" runat="server" CssClass="Pulsante" Text="Chiudi"/>
        </fieldset>
	</asp:Panel>
    <!--POPUP File--CV Fine-->
</asp:Content>
