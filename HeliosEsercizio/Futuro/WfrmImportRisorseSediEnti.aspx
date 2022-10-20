<%@ Page Title="Importazione File" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmImportRisorseSediEnti.aspx.vb" Inherits="Futuro.WfrmImportRisorseSediEnti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">
    //<![CDATA[


        function Nascondi() {
            document.all.MainContent_lblMessaggioErrore.style.visibility = 'visible';
            document.all.MainContent_lblMessaggioErrore.style.fontSize = '1.6em'
            document.all.MainContent_lblMessaggioErrore.style.fontWeight = 'bold'
            document.all.MainContent_lblMessaggioErrore.style.color = '#3a4f63'
            document.all.MainContent_lblMessaggioErrore.innerText = 'ATTENDERE........';
            document.all.MainContent_CmdElabora.style.visibility = 'hidden';
        }

        function toggleSettoriAree() {
            var x = document.getElementById("divSettoriAree");
            var y = document.getElementById("btnSettoriAree");
            if (x.style.display === "none") {
                x.style.display = "block";
                y.innerHTML = "Nascondi elenco Settori/Aree"
            } else {
                x.style.display = "none";
                y.innerHTML = "Visualizza elenco Settori/Aree"
            }
        }

    //]]>
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
<fieldset class="ContornoPagina">
        <legend><asp:Label ID="lblTitoloLegend" runat="server"  Text=""/></legend>
        <div class="wrapper" style="width:100%">
            <div class="headers" >
                <h2><asp:Label ID="lblTitolo" runat="server"  Text=""></asp:Label></h2>
            </div>
            <div class="RigaVuota">&nbsp;<asp:Label ID="lblMessaggioErrore" CssClass="msgErroreBig" runat="server" Text=""></asp:Label>
            </div>
            
            <p> <asp:Label ID="lblTipoImport" runat="server"  Text=""></asp:Label>&nbsp;&nbsp;&nbsp;
                <asp:HyperLink ID="hplFileRicerca" NavigateUrl="" runat="server" Text="Ricerca"></asp:HyperLink> <br />
                <asp:Label ID="LblUp" runat="server"  Text="Eseguire l'Upload del file completo"></asp:Label>
                <asp:FileUpload ID="txtSelFile" runat="server" ToolTip="File di Upload" style="width:40%" />&nbsp;
                <asp:Button ID="CmdElabora"  OnClientClick="Nascondi()" CssClass="Pulsante" runat="server" Text="Elabora" />
            </p>
            <br />
        <fieldset> 
            <strong>Note:</strong>
            <h3 class="text-danger">Attenzione! Non inserire il carattere <strong>;</strong> in nessun campo</h3><br />
            <div id="DivAmbiti" class="rowGroup" runat="server">
                 <h3>I seguenti campi devono contenere il valore SI/NO</h3> 
                <div class="row">              
                   <% Call CaricaAmbiti()%> 
                </div>
            </div>
            <div id="DivAttivita" class="rowGroup" runat="server">
                 <h3>I seguenti campi devono contenere il valore SI</h3> 
                <div class="row">
                   <fieldset>
                    <ul>
                        <li><strong>Attivita negli ultimi tre anni</strong></li>
                        <li><strong>Attivita per Fini Istituzionali</strong></li>
                        <li><strong>Attivita senza scopo di Lucro</strong> (* Solo Enti Privati)</li>                    
                    </ul>
                   </fieldset>
                </div>
            </div>
            <div id="DivTipologia" class="rowGroup" runat="server">
                <h3>Il campo Tipologia deve essere valorizzato utilizzando la descrizione abbreviata di seguito indicata. </h3> 
                <div class="row">
                  <% Call CaricaTipologiaEnte()%> 
                </div>
               
            </div>
            <div id="DivTipoRelazione" class="rowGroup" runat="server">
                <h3>Il campo Tipo Relazione deve essere valorizzato utilizzando la descrizione abbreviata di seguito indicata.</h3> 
                <div class="row">
                     <% call CaricaTipoRelazione%> 
                </div>
            </div>
            <div id="DivSettori" class="rowGroup" runat="server">
                <h3>Per ogni settore il cui campo è stato valorizzato con <strong>SI</strong>, i campi successivi devono essere valorizzati come segue.</h3> 
                <div class="row">
                   <fieldset>
                    <ul>
                        <li>Il campo <strong>Ultimo Anno <i>settore</i></strong> va valorizzato con <%=DateTime.Now.Year - 1%>.</li>
                        <li>Il campo <strong>Penultimo Anno <i>settore</i></strong> va valorizzato con <%=DateTime.Now.Year - 2%>.</li>
                        <li>Il campo <strong>Terzultimo Anno <i>settore</i></strong> va valorizzato con <%=DateTime.Now.Year - 3%>.</li>
                        <li>I campi <strong>Esperienza (Ultimo, Penultimo,Terzultimo) Anno <i>settore</i></strong> devono avere una lunghezza compresa tra 500 e 1500 caratteri.</li>   
                        <li>Il campo <strong>Aree d'Intervento <i>settore</i></strong> va valorizzato con l'elenco separato da virgole dei codici delle Aree d'Intervento interessate (es. A01,A02,A03).</li>                 
                    </ul>
                    <button type="button" id="btnSettoriAree" class="collapsible" onclick="toggleSettoriAree()">Visualizza elenco Settori/Aree</button>
                    <div id="divSettoriAree" style="display:none">
                        <% Call CaricaSettoriAree() %>
                    </div>
                   </fieldset>
                </div>
            </div>
            <div id="DivRuolo" class="rowGroup" runat="server">
                <h3>Il campo Ruolo deve essere 
					valorizzato utilizzando la descrizione abbreviata di seguito indicata.</h3> 
                <div class="row">              
                   <% call CaricaRuolo%>
                </div>
            </div>
             <% If Session("TipoImport") = "riso" Then%>
            <div id="DivEsp" class="rowGroup" runat="server">
                <h3>Il campo Esperienza Servizio Civile deve essere 
					valorizzato ESCLUSIVAMENTE per i Ruoli di Responsabile della Formazione, Valorizzazione delle Competenze e Formatore utilizzando la numerazione di seguito indicata.</h3> 
                <div class="row">

                   <fieldset>
                <ul>
                <li><strong>1</strong> per <strong>Si</strong></li>
                <li><strong>2</strong> per <strong>No</strong></li>
                </ul>
                </fieldset>
                </div>
            </div>
             <div id="DivCorso" class="rowGroup" runat="server">
                <h3>Il campo Corso di Formazione deve essere 
					valorizzato ESCLUSIVAMENTE per i Ruoli di Responsabile della Formazione, Valorizzazione delle Competenze e Formatore utilizzando la numerazione di seguito indicata.</h3> 
                <div class="row">
                  <fieldset>
                <ul>
                <li><strong>1</strong> per <strong>Fatto </strong></li>
                <li><strong>2</strong> per <strong>Da Fare</strong></li>
                <li><strong>3</strong> per <strong>Non Necessario</strong></li>
                </ul>
                </fieldset>
                </div>
            </div>   
            <% End If%>                 
            <div id="DivTitoloGiuridico" class="rowGroup" runat="server">
                <h3>Il campo Titolo di disponibilit&agrave; deve contenere:</h3> 
                <div class="row">
                   <% Call CaricaTitoliGiuridici()%> 
                </div>
            </div>   
            
             <% If Session("TipoImport") = "sedi" Then%>
             <div id="DivVolontari" class="rowGroup" runat="server">
             <h3>Le colonne relative ai volontari devono contenere i seguenti valori:</h3>
             <ul>
             <li><b>Volontari allocabili</b>: inserire il numero di volontari</li>
             <li><b>Volontari maggiore di 20</b>: dichiarare esplicitamente <b>SI</b> se il numero &egrave; maggiore di 20, oppure <b>NO</b> se il numero &egrave; minore o uguale a 20</li>
             </ul>
             </div>

             <div id="DivNormativa81" class="rowGroup" runat="server">
               
                 <h3>Per le sedi nazionali il campo Normativa 81 è obbligatorio e deve essere valorizzato secondo la seguente codifica: </h3>  
                 <br />
                 <div class="row">

                   <fieldset>
                    <table>
                        <tr>
                            <td style="width:5%">
                             <strong>SI</strong>
                            </td>
                            <td style="width:10%">
                                si dichiara che:
                            </td>
                            <td style="width:80%; text-align:justify">
                                <strong>la sede è nella disponibilità dell’ente secondo il titolo giuridico indicato, è adeguata al numero di volontari indicati per spazi, servizi, strumenti e attrezzature ed è conforme alle disposizioni del decreto legislativo 9 aprile 2008, n. 81 e s.m.i, in materia di tutela della salute e della sicurezza nei luoghi di lavoro.</strong>
                            </td>
                        </tr>
                     </table>
                </fieldset>
                </div>

             </div> 
             
            <div id="DivConformita" class="rowGroup" runat="server">
                <h3>Il campo Conformit&agrave; Titolo giuridico deve essere valorizzato per le sedi estere con titolo di disponibilit&agrave; 
					diverso da LETTERA DI ACCORDO (LDA) secondo la seguente codifica </h3> 
                <div class="row">

                   <fieldset>
                    <table>
                        <tr>
                            <td style="width:5%">
                             <strong>CON</strong>
                            </td>
                            <td style="width:10%">
                                si dichiara che:
                            </td>
                            <td style="width:80%; text-align:justify">
                                <strong>la sede &egrave; nella disponibilit&agrave; dell&rsquo;ente secondo il titolo giuridico indicato, &egrave; adeguata al numero di volontari indicati per spazi, servizi, strumenti e attrezzature ed &egrave; conforme alle disposizioni dettate in materia di tutela della salute e della sicurezza nei luoghi di lavoro vigenti nel Paese ove &egrave; ubicata.</strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:5%">
                             &nbsp;
                            </td>
                            <td style="width:10%">
                                &nbsp;
                            </td>
                            <td style="width:80%">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width:5%">
                             <strong>ATT</strong>
                            </td>
                            <td style="width:10%">
                                si dichiara che:
                            </td>
                            <td style="width:80%; text-align:justify">
                                <strong>la sede &egrave; nella disponibilit&agrave; dell&rsquo;ente secondo il titolo giuridico indicato, &egrave; adeguata al numero di volontari indicati per spazi, servizi, strumenti e attrezzature e, considerata l&rsquo;assenza di norme in materia di tutela della salute e della sicurezza nei luoghi di lavoro nel Paese ove &egrave; ubicata la sede, lo svolgimento del servizio avverr&agrave; secondo le condizioni di sicurezza previste per i luoghi di lavoro su territorio italiano.</strong>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                </div>
            </div>
        
            <fieldset>
            <div id="DivSedeSoggettoEstero" class="rowGroup" runat="server">
                <h3>Il campo Soggetto Estero deve essere valorizzato per le sedi estere con titolo di disponibilit&agrave; 
					uguale LETTERA DI ACCORDO (LDA) con la denominazione del soggetto estero cui è in capo la Sede </h3>  
                    <br />
            </div>   
             </fieldset>
            <div id="DivSoggettoEstero" class="rowGroup" runat="server">
                <h3>Il campo Conformit&agrave; Lettera di accordo deve essere valorizzato per le sedi estere con titolo di disponibilit&agrave; 
					uguale LETTERA DI ACCORDO (LDA) secondo la seguente codifica </h3>  
                <div class="row">
                  <fieldset>
                    <table>
                        <tr>
                            <td style="width:5%">
                             <strong>CON</strong>
                            </td>
                            <td style="width:10%">
                                si dichiara che:
                            </td>
                            <td style="width:80%; text-align:justify">
                                <strong>la sede indicata &egrave; nella disponibilit&agrave; dell&rsquo;ente in virt&ugrave; della lettera di accordo allegata, &egrave; adeguata al numero di volontari indicati per spazi, servizi, strumenti e attrezzature ed &egrave; conforme alle disposizioni dettate in materia di tutela della salute e della sicurezza nei luoghi di lavoro vigenti nel Paese ove &egrave; ubicata.</strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:5%">
                             &nbsp;
                            </td>
                            <td style="width:10%">
                                &nbsp;
                            </td>
                            <td style="width:80%">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width:5%">
                             <strong>ATT</strong>
                            </td>
                            <td style="width:10%">
                                si dichiara che:
                            </td>
                            <td style="width:80%; text-align:justify">
                                <strong>la sede indicata &egrave; nella disponibilit&agrave; dell&rsquo;ente in virt&ugrave; della lettera di accordo allegata, &egrave; adeguata al numero di volontari indicati per spazi, servizi, strumenti e attrezzature e, considerata l&rsquo;assenza di norme in materia di tutela della salute e della sicurezza nei luoghi di lavoro nel Paese ove &egrave; ubicata la sede, lo svolgimento del servizio avverr&agrave; secondo le condizioni di sicurezza previste per i luoghi di lavoro su territorio italiano.</strong>
                            </td>
                        </tr>
                    </table>
                 </fieldset>
                </div>
            </div>   
            <% End If%>                 

        </fieldset>
        <% If Session("TipoImport") = "riso" Then%>
        <div id="DivImportCV" runat="server">
            <fieldset>
                <h3>Importazione Curricula Vitae</h3>
                <p> 
                    <asp:FileUpload ID="FileCV" runat="server" ToolTip="File zip dei Curricula" style="width:40%" />&nbsp;
                    <asp:Button ID="btnCaricaCV"  CssClass="Pulsante" runat="server" Text="Elabora" />
                </p>
                <asp:Label runat="server" ID="txtErroreCV" CssClass="msgErrore"></asp:Label>
                <fieldset>
				    <strong>Note:</strong>
				    <h3>Caricare un file in formato Zip contenente file con le seguenti caratteristiche:</h3>
				    <ul>
                        <li>I files devono essere di tipo PDF o Pdf.P7m</li>
                        <li>Il nome del file deve essere del tipo CV_CodiceFiscale.pdf</li>
                        <li>I codici fiscali associati ai files devono essere presenti nelle risorse</li>
                        <li>I files devono essere tutti distinti</li>
				    </ul>
                </fieldset>
            </fieldset>
                <%--POPUP Messaggi--%>
            <asp:ModalPopupExtender runat="server" ID="modalCVResult" TargetControlID="hfCVResult" 
                PopupControlID="pnlCVResult" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
                BehaviorID="pnlCVResult" Enabled="True" CancelControlID="btnCloseCVResult"> 
            </asp:ModalPopupExtender>
	        <asp:Panel ID="pnlCVResult" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
                <asp:HiddenField runat="server" ID="hfCVResult" />
                <p class="msgInfo">Elaborazione File Importazione Curricula Vitae</p>
                <asp:Label runat="server" ID="lblElaborazioneCV"></asp:Label>
                <asp:Literal runat="server" ID="lstErroriCV"></asp:Literal>
                <div style="margin-top: 1em;"></div>
                <asp:Button ID="btnCloseCVResult" runat="server" CssClass="Pulsante" Text="Chiudi"/>
	        </asp:Panel>
        </div>
        <% End If%>

        <% If Session("TipoImport") = "enti" Then%>
        <div id="DivImportDocumentiEnte" runat="server">
            <fieldset>
                <h3>Importazione Massiva Documenti</h3>
                <div class="wrapper" style="border:none">
				    <div class="row">
					    <div class="collable" style="width:40%">      
						    <asp:Label ID="Label1" AssociatedControlID="ddlDocumentiEnte" runat="server" Text="Tipo Documento"></asp:Label>
					    </div>
					    <div class="colOggetti" style="width:60%">       
						    <asp:DropDownList ID="ddlDocumentiEnte" runat="server" CssClass="ddlClass" AutoPostBack="true">
							    <asp:ListItem Value="0" Selected="True">Selezionare</asp:ListItem>
							    <asp:ListItem Value="1">Atto Costitutivo</asp:ListItem>
							    <asp:ListItem Value="2">Statuto</asp:ListItem>
							    <asp:ListItem Value="3">Delibera</asp:ListItem>
							    <asp:ListItem Value="4">Carta di Impegno Etico</asp:ListItem>
						    </asp:DropDownList>         
					    </div>				
				    </div>
                </div>
                <p> 
				    <asp:FileUpload ID="fileDocumenti" runat="server" ToolTip="File zip delle Carte di Impegno Etico" style="width:40%" />&nbsp;
				    <asp:Button ID="btnElaboraDocumenti"  CssClass="Pulsante" runat="server" Text="Elabora" />
                </p>
                <asp:Label runat="server" ID="txtErroreImportDocumenti" CssClass="msgErrore"></asp:Label>
                <fieldset>
				    <strong>Note:</strong>
				    <h3>Caricare un file in formato Zip contenente file con le seguenti caratteristiche:</h3>
                    <div id="divNessunaSelezione" runat="server" visible="true">
						<ul>
							<li>Selezionare un Tipo Documento per visualizzare le Note.</li>
						</ul>
					</div>
					<div id="divCarteImpegnoEtico" runat="server" visible="false">
						<ul>
							<li>I files devono essere di tipo PDF o Pdf.P7m</li>
							<li>Il nome del file deve essere del tipo CARTAIMPEGNOETICO_CodiceFiscaleEnte.pdf</li>
							<li>I codici fiscali associati ai files devono essere degli enti di accoglienza</li>
							<li>I files devono essere tutti distinti</li>
						</ul>
					</div>
					<div id="divDelibera" runat="server" visible="false">
						<ul>
							<li>I files devono essere di tipo PDF o Pdf.P7m</li>
							<li>Il nome del file deve essere del tipo DELIBERA_CodiceFiscaleEnte.pdf</li>
							<li>I codici fiscali associati ai files devono essere degli enti di accoglienza</li>
							<li>I files devono essere tutti distinti</li>
						</ul>
					</div>
					<div id="divAttoCostitutivo" runat="server" visible="false">
						<ul>
							<li>I files devono essere di tipo PDF o Pdf.P7m</li>
							<li>Il nome del file deve essere del tipo ATTOCOSTITUTIVO_CodiceFiscaleEnte.pdf</li>
							<li>I codici fiscali associati ai files devono essere degli enti di accoglienza</li>
							<li>I files devono essere tutti distinti</li>
						</ul>
					</div>
					<div id="divStatuto" runat="server" visible="false">
						<ul>
							<li>I files devono essere di tipo PDF o Pdf.P7m</li>
							<li>Il nome del file deve essere del tipo STATUTO_CodiceFiscaleEnte.pdf</li>
							<li>I codici fiscali associati ai files devono essere degli enti di accoglienza</li>
							<li>I files devono essere tutti distinti</li>
						</ul>
					</div>			                        					
                </fieldset>
            </fieldset>
                <%--POPUP Messaggi--%>
            <asp:ModalPopupExtender runat="server" ID="modalImportDocumentiEnteResult" TargetControlID="hfDocumentiEnteResult" 
                PopupControlID="pnlDocumentiEnteResult" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
                BehaviorID="pnlDocumentiEnteResult" Enabled="True" CancelControlID="btnCloseDocumentiEnteResult"> 
            </asp:ModalPopupExtender>
	        <asp:Panel ID="pnlDocumentiEnteResult" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
                <asp:HiddenField runat="server" ID="hfDocumentiEnteResult" />
                <p class="msgInfo" id="pTipoFile" runat="server">Elaborazione File Importazione Carte di Impegno Etico</p>
                <asp:Label runat="server" ID="lblElaborazioneDocumenti"></asp:Label>
                <asp:Literal runat="server" ID="lstErroriImportazioneDocumentiEnte"></asp:Literal>
                <div style="margin-top: 1em;"></div>
                <asp:Button ID="btnCloseDocumentiEnteResult" runat="server" CssClass="Pulsante" Text="Chiudi"/>
	        </asp:Panel>
        </div>
        <% End If%>

        <% If Session("TipoImport") = "sedi" Then%>
        <div id="DivImportLDA" runat="server">
            <fieldset>
                <h3>Importazione Lettere di Accordo</h3>
                <p> 
                    <asp:FileUpload ID="FileLDA" runat="server" ToolTip="File zip dei Curricula" style="width:40%" />&nbsp;
                    <asp:Button ID="btnCaricaLDA"  CssClass="Pulsante" runat="server" Text="Elabora" />
                </p>
                <asp:Label runat="server" ID="txtErroreLDA" CssClass="msgErrore"></asp:Label>
                <fieldset>
				    <strong>Note:</strong>
				    <h3>Caricare un file in formato Zip contenente file con le seguenti caratteristiche:</h3>
				    <ul>
                        <li>I files devono essere di tipo PDF o Pdf.P7m</li>
                        <li>Il nome del file deve essere del tipo LDA_NomeSede.pdf</li>
                        <li>I Nomi delle sedi associati ai files devono essere presenti nelle Sedi</li>
                        <li>I files devono essere tutti distinti</li>
				    </ul>
                </fieldset>
            </fieldset>
                <%--POPUP Messaggi--%>
            <asp:ModalPopupExtender runat="server" ID="modalLDAResult" TargetControlID="hfLDAResult" 
                PopupControlID="pnlLDAResult" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
                BehaviorID="pnlLDAResult" Enabled="True" CancelControlID="btnCloseLDAResult"> 
            </asp:ModalPopupExtender>
	        <asp:Panel ID="pnlLDAResult" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
                <asp:HiddenField runat="server" ID="hfLDAResult" />
                <p class="msgInfo">Elaborazione File Importazione Curricula Vitae</p>
                <asp:Label runat="server" ID="lblElaborazioneLDA"></asp:Label>
                <asp:Literal runat="server" ID="lstErroriLDA"></asp:Literal>
                <div style="margin-top: 1em;"></div>
                <asp:Button ID="btnCloseLDAResult" runat="server" CssClass="Pulsante" Text="Chiudi"/>
	        </asp:Panel>
        </div>
        <% End If%>

        <div class="RigaPulsanti">
        <asp:Button 
            ID="CmdChiudi" CssClass="Pulsante" runat="server"
              Text="Chiudi"  />&nbsp;

     </div>
        <p>&nbsp;</p>
     </div>
    </fieldset>
</asp:Content>
