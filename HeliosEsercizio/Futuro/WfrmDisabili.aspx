<%@ Page Title="Inserimento soggetti che usufruiscono dell'accompagnamento" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmDisabili.aspx.vb" MaintainScrollPositionOnPostBack="true" Inherits="Futuro.WfrmDisabili" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
            <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css"/>
        <script src="Scripts/JHelper.js" type="text/javascript"></script>
    <style type="text/css">
                .ui-datepicker {
                    font-size: 11px;
                }
            </style>
        <script  type="text/javascript">
            function SetContextKey(tipologiaIndirizzo) {
                $find('<%=AutoCompleteExtender1.ClientID%>').set_contextKey($get("<%=ddlComuneResidenza.ClientID %>").value);
            };

            </script>

        <script  type="text/javascript">
       /* <![CDATA[ */
        var pathPlus = "images/plus_xsmall.png";
        var pathMinus = "images/minus_xsmall.png";
        var titlePathMinus = "Riduci";
        var titlePathPlus = "Espandi";
        var inserimento = '<%=Request.Params("IdAttivita")%>';
        var isPostbackDisabili = '<%= hdnIsPostbackDisabili.value %>' ;
        var ChiusoAperto = '<%= ChiusoAperto.value %>' ;
            /* ]]> */
        </script>

        <script type="text/javascript">
        /* <![CDATA[ */

         $(function () 
        {
       // alert("pippo")
        
//            var tornaSu = 0;
          // var ChiusoAperto = document.getElementById("ChiusoAperto").value  

           var ChiusoAperto=(document.getElementById(CostruisciId("ChiusoAperto")).value);
          // alert(ChiusoAperto);


            if (ChiusoAperto=='0') {
                $('#CaratteristicheDisabileContent').css('display', 'none');
                $('#CaratteristicheDisabile').find("img").first().attr("src", pathPlus);
                $('#CaratteristicheDisabile').find("img").first().attr("title", titlePathPlus);
                 //title = "Riduci"
                 // alert("piu");
                }
                 else{
                  $('#CaratteristicheDisabileContent').css('display', 'yes');
                   $('#CaratteristicheDisabile').find("img").first().attr("src", pathMinus);
                $('#CaratteristicheDisabile').find("img").first().attr("title", titlePathMinus);
                   //$(this).find("img").first().attr("src", pathMinus);
                   //$(this).find("img").first().attr("title", titlePathMinus);
                   //title = "Espandi"
                  // alert("meno");
               }
            

            $('#CaratteristicheDisabile').click(function () {
             if (ChiusoAperto=='0') {
             ChiusoAperto='1';
             document.getElementById(CostruisciId("ChiusoAperto")).value='1';
               }
              else{
                  ChiusoAperto='0';
                   document.getElementById(CostruisciId("ChiusoAperto")).value='0';
              }
               $('#CaratteristicheDisabileContent').slideToggle('fast');
               $(this).toggleClass('slideSign');
              if (ChiusoAperto == 0) {
             // alert("apro");
              $(this).find("img").first().attr("src", pathPlus);
              $(this).find("img").first().attr("title", titlePathPlus);
              title = "Riduci"
               }
               else {
              // alert("chiudo")
               $(this).find("img").first().attr("src", pathMinus);
               $(this).find("img").first().attr("title", titlePathMinus);
               title = "Espandi"

                }

                return false;
            });
        });


          /* ]]> */
    </script>
    <script  type="text/javascript">

        $(function () {
            var IdData = "txtDataNascita";
            var sharpIdData = "#" + IdData
            $("" + sharpIdData + "").datepicker();
        });
				
    </script>

    <script  type="text/javascript">
        function CheckNumeroIntero(valore, descrizione) {
            ValidaNumeroIntero(valore, descrizione, "lblErrore");
        };

        function ValidaCampi() {
            AggiornaErrore("", "lblErrore");
            VerificaCampoObbligatorio("txtCognome", "Cognome", "lblErrore");
            VerificaCampoObbligatorio("txtNome", "Nome", "lblErrore");

            VerificaCampoObbligatorio("txtCodiceFiscale", "Codice Fiscale", "lblErrore");
            VerificaCampoObbligatorio("txtDataNascita", "Data di Nascita", "lblErrore");
            VerificaDataValida(document.getElementById("txtDataNascita").value, "Data di Nascita", "lblErrore");
            if (document.getElementById("ddlProvinciaNascita").value == 0) {
                ConcatenaErrore("Il campo 'Provincia/Nazione di Nascita' è obbligatorio ", "lblErrore");
            }
            if (document.getElementById("ddlComuneNascita").value == 'Selezionare Provincia/Nazione di Nascita') {
                ConcatenaErrore("Il campo 'Comune di nascita' è obbligatorio ", "lblErrore");
            }
            if (document.getElementById("ddlProvinciaResidenza").value == 0) {
                ConcatenaErrore("Il campo 'Provincia/Nazione di Residenza' è obbligatorio ", "lblErrore");
            }
            if (document.getElementById("ddlComuneResidenza").value == 'Selezionare Provincia/Nazione di Residenza') {
                ConcatenaErrore("Il campo 'Comune di Residenza' è obbligatorio ", "lblErrore");
            }
            VerificaCampoObbligatorio("TxtCapp", "C.A.P.", "lblErrore");
            VerificaCampoObbligatorio("txtIndirizzo", "Indirizzo", "lblErrore");
            VerificaCampoObbligatorio("txtCivico", "N&#176; Civico", "lblErrore");
            var risultato = $("#lblErrore").html()
            if (risultato == '' || risultato == null) {
                return true;
            } else {
                document.getElementById("lblErrore").scrollIntoView();
                return false;
            }
        };
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
    <fieldset class="fieldsetrasparente">
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:Label ID="lblTitolo" runat="server"  Text="Inserimento Soggetti che usufruiscono dell'accompagnamento"></asp:Label>
      </h2>
    </div>
    <div class="rowGroup" style="height:auto">
    <div class="RigaVuota">&nbsp;</div>
       <div class="row" >
       <asp:Label ID="lblErrore"  runat="server" CssClass="msgErrore" ></asp:Label>
       <asp:Label ID="lblConferma"  runat="server" CssClass="msgConferma" ></asp:Label>
       <asp:HiddenField ID="TxtMod" runat="server" />
       <asp:HiddenField ID="TxtAcco" runat="server" />
        <asp:HiddenField ID="TxtIdComNas" runat="server" />
         <asp:HiddenField ID="TxtIdComRes" runat="server" />
       </div>
</div>
          <div class="row">          
               <div class="collable" style="width:15%">
                <asp:Label ID="LblNome"  runat="server" CssClass="label" Text="Nome" AssociatedControlID="txtNome"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtNome" CssClass="textbox" autofocus="true" runat="server" MaxLength="100"></asp:TextBox>
            </div>
              <div class="collable" style="width:15%">
                <asp:Label ID="LblCognome" CssClass="label" runat="server" AssociatedControlID="txtCognome" Text="Cognome"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtCognome" CssClass="textbox" runat="server" MaxLength="100" ></asp:TextBox>
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
                <asp:TextBox ID="txtDataNascita" CssClass="textboxData" Width="50%" runat="server" MaxLength="10"></asp:TextBox>
            </div>
        </div>
         <div class="row">
            <div class="collable" style="width:15%">
                <asp:Label ID="lblddlAmbiti" CssClass="label" runat="server" Text="Causali" AssociatedControlID="ddlAmbiti"></asp:Label>
            </div>
              <div class="colOggetti" style="width:35%">
                <asp:dropdownlist id="ddlAmbiti" CssClass="ddlClass" AutoPostBack="true" runat="server"  ></asp:dropdownlist>
            </div>

                      <div class="collable" style="width:15%">
                <asp:Label ID="LblProvinciaNascita" CssClass="label" runat="server" Text="Provincia / Nazione di nascita" AssociatedControlID="ddlProvinciaNascita"></asp:Label>
            </div>
              <div class="colOggetti" style="width:30%">
                <asp:dropdownlist id="ddlProvinciaNascita" CssClass="ddlClass" AutoPostBack="true" runat="server"  ></asp:dropdownlist>
            </div>
            <div class="collable" style="width:5%">
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
        </div>
        <div class="row">
            <div class="collable" style="width:15%">
                <asp:Label ID="LblProvResidenza"  CssClass="label" runat="server" Text="Provincia/ Nazione di Residenza" AssociatedControlID="ddlProvinciaResidenza"></asp:Label>
            </div>
            <div class="colOggetti" style="width:25%">
                <asp:dropdownlist id="ddlProvinciaResidenza" CssClass="ddlClass" AutoPostBack="true"  runat="server"></asp:dropdownlist>
                
            </div>
            <div class="collable" style="width:10%">
                <asp:CheckBox ID="ChkEsteroResidenza"  AutoPostBack="true" ToolTip="Flag Estero Nazione di Residenza" Text="Estero" runat="server" />
            </div>
           <div class="collable" style="width:15%">
                <asp:Label ID="LblComResidenza" CssClass="label" runat="server" Text="Comune di Residenza" AssociatedControlID="ddlComuneResidenza"></asp:Label>
            </div>
           <div class="colOggetti" style="width:35%">
                <asp:dropdownlist id="ddlComuneResidenza" runat="server"  CssClass="ddlClass" Enabled="false">
                <asp:ListItem Text="Selezionare Provincia/Nazione di residenza">
                </asp:ListItem>
                </asp:dropdownlist>

            </div>
           
        </div>
        <div class="row">
              <div class="collable" style="width:15%">
                <asp:Label ID="Lblndirizzo" CssClass="label" runat="server" Text="Indirizzo" AssociatedControlID="txtIndirizzo"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtIndirizzo" runat="server" CssClass="textbox" onkeyup="javascript: SetContextKey('RESIDENZA');"></asp:TextBox>
                        <asp:AutoCompleteExtender 
                    ID="AutoCompleteExtender1" 
                    TargetControlID="txtIndirizzo" 
                    ContextKey ="ddlComuneResidenza"
                    CompletionListCssClass="ddl_Autocomplete"
                    UseContextKey="true"
                    CompletionInterval="100" EnableCaching="false" 
                    runat="server" MinimumPrefixLength="5"    ServiceMethod="GetCompletionList" >
                </asp:AutoCompleteExtender>
            </div>
            <div class="collable" style="width:15%">
                <asp:Label ID="LblNumCiv"  runat="server" Text="N&#176; Civico" ToolTip="Numero Civico Residenza" AssociatedControlID="txtCivico"></asp:Label>
            </div>
            <div class="colOggetti" style="width:10%">
                <asp:TextBox ID="txtCivico" runat="server" CssClass="textbox" ></asp:TextBox>

            </div>
            <div class="collable" style="width:5%">
                 <asp:Image ID="infocivicoRec" runat="server" AlternateText="Informazioni Civico di Residenza" 
                    ImageUrl="Images/info_small.png"
                    ToolTip="NEL CAMPO CIVICO E' POSSIBILE INSERIRE SOLO I SEGUENTI FORMATI:
                        - 21
                        - 21/A 
                        - 21/A5 
                        - 21 BIS 
                        - KM 21,500 
                        OPPURE IL VALORE SNC" />
            </div>
 
             <div class="collable" style="width:5%">
                <asp:Label ID="LblCap" CssClass="label" runat="server" Text="C.A.P." AssociatedControlID="TxtCapp"  ></asp:Label>
            </div>
            <div class="colOggetti" style="width:10%">
                <asp:TextBox ID="TxtCapp" CssClass="textbox" runat="server"  onchange="javascript: CheckNumeroIntero('TxtCapp','C.A.P.');" ></asp:TextBox>

             </div>
            <div class="collable" style="width:5%">
            <asp:ImageButton ID="imgCap" runat="server" title="Seleziona il Cap di Residenza" 
                    ImageUrl="Images/valida_small.png" AlternateText="Selezione C.A.P. di Residenza"  />
            </div>
            </div>
       <div class="RigaPulsanti">
            <asp:Button ID="cmdConferma" CssClass="Pulsante" runat="server" ToolTip="Conferma" Text="Conferma" OnClientClick ="javascript:return ValidaCampi();" />
            <asp:Button ID="imgChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
                  <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" />&nbsp;
                <br />
         <asp:HyperLink ID="ApriCSV1"   AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" 
        Text="DOWNLOAD CSV" runat="server" 
        ForeColor="#003399" Visible="False"></asp:HyperLink>
       </div>
 <fieldset id="flsDisabile" runat="server" class="ContornoPagina" >
 <legend id="CaratteristicheDisabile"><img  src="images/plus_xsmall.png"  alt="Espandi/Riduci" style="cursor:pointer;" />&nbsp;&nbsp;
     Importazione soggetti che usufruiscono dell'accompagnamento</legend>
  <div class="wrapper" id="CaratteristicheDisabileContent" style="width:100%;border:0px" >
<div id="TabellaImportDisabili" class="wrapper" style="width:100%">

     <div class="headers">
      <h2>
      <asp:Label ID="Label5" runat="server"  Text="Import soggetti che usufruiscono dell'accompagnamento"></asp:Label>
       </h2>
    </div>
   
     <div class="RigaVuota">
    
      &nbsp;</div>

    

    

     <div class="row" >
     
   
            <asp:Label ID="Label4" runat="server"  Text="1) Per il download del file csv da compilare"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:HyperLink ID="HplFileGraduatorie" NavigateUrl="~/download/Master/ElencoSoggetti.csv" runat="server">Clicca qui..</asp:HyperLink>
   
      
    </div>

     <div class="row" >
     
            <asp:Label ID="Label3" runat="server"  Text="2) Eseguire l'Upload del file completo"></asp:Label>
                
               
                <br />
                 
     </div> 
     

    <div class="row" style="text-align:center">
  <asp:Label ID="lblUpLoad" runat="server" AssociatedControlID="txtSelFile" Text="Seleziona File: "></asp:Label><asp:FileUpload ID="txtSelFile"  runat="server" /> &nbsp;<asp:Button ID="CmdElabora" OnClientClick="Nascondi()" AutoPostBack="true" CssClass="Pulsante" runat="server" Text="Elabora" /><asp:Label ID="lblMessaggioErrore" CssClass="msgErrore" runat="server" Text=""></asp:Label>
     <p>&nbsp;</p>
       </div> 
     <fieldset>
     <h3><strong>Note:</strong></h3>
  
     <br/>
    <h3>
		Il campo CAUSALE deve contenere:
    </h3>
    <% Call CaricaCausaliAccompagno()%>
    <br />
    <h3>
	    Il campo DATADINASCITA  deve essere nel formato gg/mm/aaaa:
    </h3>
    <br />
     <h3>
	    Il campo ISTAT COMUNE DI NASCITA o ISTAT COMUNE DI RESIDENZA se Estero va indicato il nome della Nazione es. SPAGNA.
    </h3>
    <%--<fieldset>
    <ul>
	    <li><strong>0</strong> per <strong>Italia</strong></li>
	    <li><strong>1</strong> per <strong>Estero</strong></li>
    </ul>
    
   
   </fieldset>--%>
    <%-- <h3>
	    Il campo StatoCivile deve contenere:
    </h3> 
     <%Call CaricaTipiStatoCivile()%>                           
  --%>
                           
      </fieldset>
     
</div>
    
  </div>
  </fieldset>


       
    </div>
    <asp:datagrid id="dgDisabili" runat="server" Width="100%"  Caption="Elenco soggetti che usufruiscono dell'accompagnamento" 
			AllowPaging="True" CellPadding="2" Font-Size="Small" ShowFooter="false"  CssClass="table" PageSize="10" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
		<FooterStyle></FooterStyle>
		<SelectedItemStyle  BackColor="White" Font-Bold="true"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
		<HeaderStyle></HeaderStyle>
        <Columns>
            	<asp:TemplateColumn > 
                    <ItemTemplate >
						<asp:ImageButton ID="ImageButton1"  style="cursor:pointer;" CommandName="Modifica" AlternateText="Nominativo da Modificare" ToolTip='Seleziona Nominativo da Modificare' runat="server" ImageURL="images/Icona_Volontario_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                 </asp:TemplateColumn>
			<asp:BoundColumn Visible="False" DataField="IdAcco" HeaderText="id">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="CodiceFiscale" HeaderText="Codice Fiscale">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Cognome" HeaderText="Cognome">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Nome" HeaderText="Nome">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="DataNascita" HeaderText="Data di Nascita" DataFormatString="{0:d}">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="ComNas" HeaderText="Comune di Nascita">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="ComRes" HeaderText="Comune di Residenza">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Indirizzo" HeaderText="Indirizzo">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Civico" HeaderText="Numero Civico">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="ProvRes" HeaderText="Provincia">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Causale" HeaderText="Causale"></asp:BoundColumn>
                <asp:TemplateColumn > 
                    <ItemTemplate >
						<asp:ImageButton ID="IdImgSelVolontari"  style="cursor:pointer;" CommandName="Rimuovi" AlternateText="Elimina Nominativo di Riferimento" ToolTip='Elimina Nominativo di Riferimento' runat="server" ImageURL="images/canc_small.png" CausesValidation="false"></asp:ImageButton>      
                    </ItemTemplate>
             </asp:TemplateColumn>
		</Columns>
		<PagerStyle NextPageText="&gt;&gt;"   
			PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
	</asp:datagrid>



    <asp:HiddenField runat="server" ID="hdnIsPostbackDisabili" />
    <asp:HiddenField runat="server" ID="ChiusoAperto" />
    </fieldset>
</asp:Content>
