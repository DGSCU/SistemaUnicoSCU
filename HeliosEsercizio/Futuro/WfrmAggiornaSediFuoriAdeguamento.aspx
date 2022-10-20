<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmAggiornaSediFuoriAdeguamento.aspx.vb" Inherits="Futuro.WfrmAggiornaSediFuoriAdeguamento" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
        <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css"/>
        <script src="Scripts/JHelper.js" type="text/javascript"></script>
        <script src="https://polyfill.io/v3/polyfill.min.js?features=default" type="text/javascript"></script>
        <style type="text/css">
              /* Always set the map height explicitly to define the size of the div
               * element that contains the map. */
              #map {
                height: 300px;
		        width: 550px;
              }

              /* Optional: Makes the sample page fill the window. */
              html,
              body {
                height: 100%;
                margin: 0;
                padding: 0;
              }
        </style>
<script type="text/javascript">


    /* <![CDATA[ */
    function SetContextKey(tipologiaIndirizzo) {

        $find('<%=AutoCompleteExtender1.ClientID%>').set_contextKey($get("<%=ddlComune.ClientID %>").value);

    };
    function OpenWindowUtilizzoSede() {
        var idSede = CostruisciId("txtidsede");
        mywin = window.open("WfrmUtilizzoSede.aspx?Id=" + document.getElementById(idSede).value, "", "height=400,width=730,dependent=no,scrollbars=no,status=no");

    };

    function OpenWindowCronologiaSede() {
        var idSede = CostruisciId("txtidsede");
        mywin = window.open("WfrmCronologiaDettaglioSede.aspx?Id=" + document.getElementById(idSede).value, "", "height=400,width=730,dependent=no,scrollbars=no,status=no");

    };

    function CheckNumeroIntero(valore, descrizione) {
        ValidaNumeroIntero(CostruisciId(valore), descrizione, CostruisciId("msgErrore"));
    };

    function CheckNumeroValido(valore, descrizione) {
        ValidaNumero(CostruisciId(valore), descrizione, CostruisciId("msgErrore"));
    };

    function CheckEmail(txtEmail, descrizione) {
        ValidaEmail(CostruisciId(txtEmail), descrizione, CostruisciId("msgErrore"));
    };

    function ShowPopUp() {
        $find('popUpControl').show();
        return false;
    }


//    function CheckCancella() {
//        if ('<%=Session("attivaDel")%>' == 'TRUE') {
//            var msg = confirm('Attenzione! La sede è utilizzata su progetti ancora non valutati, si desidera continuare?');
//            return msg;

//        }
//        if ('<%=Session("attivaDel")%>' == 'TRUEann') {
//            var msg = confirm('Attenzione! La sede è utilizzata su progetti ancora non valutati, si desidera continuare?');
//            return msg;
//        }
//        if ('<%=Session("attivaDel")%>' == 'TRUEinc') {
//            var msg = confirm('Attenzione! La sede è utilizzata su progetti ancora non valutati, si desidera continuare?');
//            return msg;
//        }
//        if ('<%=Session("attivaDel")%>' == '') {
//            var msg = confirm("Attenzione! Si sta richiedendo la cancellazione della sede, si desidera continuare?");
//            return msg;
//        }
//    }
	 /* ]]> */
     function pageLoad() 
     {
           
       var ShowPopUPControllo='<%=ShowPopUPControllo%>';
         if (ShowPopUPControllo == '1') {
            $find('popUpControl2').show();
         }
         if (ShowPopUPControllo == '2') {
            $find('popUpControl3').show();
         }
         if (ShowPopUPControllo == '3') {
            $find('popUpControl4').show();
         }
         if (ShowPopUPControllo == '4') {
             $find('popUpControl5').show();
         }

     
  }
      // This example requires the Places library. Include the libraries=places
      // parameter when you first load the API. For example:
      // <script src="https://maps.googleapis.com/maps/api/js?key=YOUR_API_KEY&libraries=places">
      let map;
      let service;
      let infowindow;

    function initMap() {
        const sydney = new google.maps.LatLng(41.8835171, 12.5023751);
        infowindow = new google.maps.InfoWindow();
        map = new google.maps.Map(document.getElementById("map"), {
            center: sydney,
            zoom: 15,
        });
        const request = {
            query: "<%=IndirizzoRicerca%>",
            fields: ["name", "geometry"]
        };
        service = new google.maps.places.PlacesService(map);
        service.findPlaceFromQuery(request, function (results, status) {
            if (status === google.maps.places.PlacesServiceStatus.OK) {
                for (var i = 0; i < results.length; i++) {
                    createMarker(results[i]);
                }
                map.setCenter(results[0].geometry.location);
            }
        });

    }

    function createMarker(place) {
        if (!place.geometry || !place.geometry.location) return;
        var marker = new google.maps.Marker({
            map: map,
            position: place.geometry.location
        });
        google.maps.event.addListener(marker, 'click', function () {
            infowindow.setContent(contentString);
            infowindow.open(map, marker);
        });
    }  

</script>

          <style type="text/css">
              .style1
              {
                  height: 34px;
              }
          </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
<fieldset class="ContornoPagina">
        <legend>Anagrafica Sede</legend>
        
        <div class="wrapper" style="width:100%">

            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Anagrafica Sedi Ente - "></asp:Label>
               <asp:Label ID="lblEnte" runat="server"  ></asp:Label>
               </h2>
            </div>
            <div class="row" style="height:auto">
                <asp:Label ID="msgConferma" CssClass="msgConferma" runat="server"></asp:Label>
            </div>
            <div class="row" style="height:auto">
                <asp:Label ID="msgErrore" CssClass="msgErrore" runat="server" ></asp:Label>                
            </div>
            <div class="row" style="height:auto">
                <asp:Label ID="msgInfo" CssClass="msgInfo" runat="server" ></asp:Label>
            </div>
            <br />
           <div id="divSospendiSede" runat="server" visible="false">
             <fieldset class="ContornoPagina">

                <legend>Stato Sede</legend>
               <h3><asp:Label runat="server" ID="lblStatoSede" CssClass="labelDati"></asp:Label></h3>
                <div  style="text-align:right">
                 
                  <asp:Button ID="CmdPortaInSospeso" CssClass="Pulsante" runat="server" ToolTip="Cambio Stato Sede da Chiusa in Sospesa" Text="Applica Sospensione" Visible="false" />
                  </div>
              
              </fieldset>
            </div>
            <div id="divChiusuraSede" class="rowGroup" style="height:auto" runat="server" visible="false">
                <fieldset class="ContornoPagina">
                <legend>Chiudi Sede</legend>
                    <div class="wrapper" style="width:100%;border:0px">
                        <div class="row" style="height:auto">
                            <h3><asp:Label runat="server" ID="lblChiudiSede" CssClass="labelDati" style="background-color:"></asp:Label></h3>
                        </div>
                        <br />
                        <div class="RigaPulsanti" style="height:auto;text-align:center">
                            <asp:Button ID="cmdConfermaChiusura" CssClass="Pulsante" runat="server" ToolTip="Conferma di chiusura Ente" Text="Conferma" />
                            <asp:Button ID="cmdAnnullaChiusura" CssClass="Pulsante" runat="server" ToolTip="Annulla richiesta chiusura" Text="Annulla" />
                        </div>
                    </div>
                </fieldset>
            </div>
        <fieldset class="ContornoPagina" id="fsOperativita" runat="server">
            <div class="wrapper" style="width:100%;border:0px">
                <div class="rowGroup" style="height:auto"> 
                    <div class="row" style="height:auto">                  
                        <div class="colHyperLink" style="width:50%">
                            <asp:LinkButton ID="lblInfoProgetti" runat="server" style="cursor:pointer" Text="Info Progetti Attivi" Visible="true" OnClientClick="javascript:OpenWindowUtilizzoSede()"></asp:LinkButton> <br />
                            <asp:LinkButton ID="lblInfoCronologiaSedi" runat="server" 
                                style="cursor:pointer" Text="Cronologia Sedi" Visible="False" 
                                OnClientClick="javascript:OpenWindowCronologiaSede()"></asp:LinkButton>
                        </div>
                
                        <div class="colOggetti" style="width:50%" align="right">        

                         <asp:Button ID="imgRichiestaModifica" CssClass="Pulsante" runat="server" Visible="false"  Text="Richiesta Modifica Sede"  />   
                        <asp:Button ID="imgAnnullaRichiestaModifica" CssClass="Pulsante" runat="server" Visible="false"  Text="Annulla Richiesta Modifica Sede"  />   
                        </div>
                    </div>
             
                </div>
            </div>
        </fieldset>
        <div class="row" style="height:auto">
             <div class="collable" style="width:15%">
               <asp:label id="labelStato" AssociatedControlID="lblsedlblStatoeInclusa" CssClass="label" runat="server" Text="Stato"  Visible="false"/>
            </div>   
             <div class="collable" style="width:20%">
               <asp:label id="lblStato" AssociatedControlID="lblsedeInclusa" CssClass="labelDati" runat="server"   Visible="false"/>
            </div>    
            <div class="colOggetti" style="width:15%">
               <asp:label id="lblsedeInclusa" CssClass="labelDati" runat="server" Visible="false" />
            </div>
              <div class="collable" style="width:15%">
               <asp:label id="lblda" AssociatedControlID="lbldaEntePadre" CssClass="label" runat="server" Text="da" Visible="false" />
            </div>    
            <div class="colOggetti" style="width:35%">
               <asp:label id="lbldaEntePadre" CssClass="labelDati" runat="server" Visible="false" />
            </div>
            </div>
        <div class="row">
            <div class="collable" style="width:15%">          
                <asp:Label ID="lblEntefiglio" CssClass="label" runat="server" Text="Ente Proprietario" AssociatedControlID="ddlEntiFigli" />
  
            </div>
            <div class="colOggetti" style="width:80%">
                <asp:DropDownList ID="ddlEntiFigli" runat="server" CssClass="ddlClass" AutoPostBack="true" ></asp:DropDownList>
           </div>
            <div class="colOggetti" style="width:5%">
                <asp:ImageButton ID="imgOpenFigli" runat="server" ImageUrl="Images/lenteIngrandimento_small.png" AlternateText="Visualizza Enti Figli" ToolTip="Visualizza Enti Figli" />
           </div>  
        </div>
        <div class="row">
            <div class="collable" style="width:15%">          
                <asp:Label ID="lblSede" CssClass="label"  runat="server" Text="<strong>(*)</strong> Denominazione Sede" AssociatedControlID="txtdenominazione" />
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtdenominazione" CssClass="textbox"  runat="server"></asp:TextBox>
           </div>
                <div class="collable" style="width:15%">          
                <asp:Label ID="lblNumeroVol" Text="<strong>(*)</strong> N&#186; Vol." AssociatedControlID="TxtNumMaxVol" CssClass="label" runat="server" />
            </div>
                <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="TxtNumMaxVol" CssClass="textbox" runat="server" onchange="javascript: CheckNumeroIntero('TxtNumMaxVol','N&#186; Vol.');"></asp:TextBox>
           </div>
        </div>
         <div class="row">
            <div class="collable" style="width:15%">
            <asp:Label ID="lblProvincia" CssClass="label"  runat="server" Text="<strong>(*)</strong>Provincia/ Nazione" AssociatedControlID="ddlProvincia"></asp:Label>
        </div>
        <div class="colOggetti">
            <asp:dropdownlist id="ddlProvincia"  AutoPostBack="true" 
                runat="server" CssClass="ddlClass"  >
                </asp:dropdownlist>
        </div>
            <div class="collable" style="width:10%">
            <asp:CheckBox ID="ChkEstero"  AutoPostBack="true" Text="Estero" runat="server" />
            </div>
        <div class="collable" style="width:15%">
            <asp:Label ID="lblComune" CssClass="label" runat="server" Text="<strong>(*)</strong>Comune" AssociatedControlID="ddlComune"></asp:Label>
        </div>

        <div class="colOggetti" style="width:35%">
            <asp:dropdownlist id="ddlComune" CssClass="ddlClass" runat="server" Enabled="false">
            <asp:ListItem Text="Selezionare Comune/Nazione"></asp:ListItem>
            </asp:dropdownlist>
          <asp:TextBox id ="txtLocalita"  CssClass="textbox" runat="server" visible ="False"></asp:TextBox>
        </div>
    </div>
   
    <div class="row" id="city" runat="server" visible="false">
     <div class="collable" style="width:15%">
                <asp:Label ID="lblCity" CssClass="label" runat="server" Text="<strong>(*)			</strong>Città Estera" AssociatedControlID="txtCity"></asp:Label>
     </div>
     <div class="colOggetti" style="width:35%">
              <asp:TextBox ID="txtCity" CssClass="textbox" runat="server"  ></asp:TextBox>
     </div>

 </div>
        <div class="row">
            <div class="collable" style="width:15%">
                <asp:Label ID="lblIndirizzo" CssClass="label" runat="server" Text="<strong>(*)</strong>Indirizzo" AssociatedControlID="txtIndirizzo"></asp:Label>
            </div>
             <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtIndirizzo" CssClass="textbox"   runat="server"  onkeyup="javascript: SetContextKey();" Width="250px"></asp:TextBox>
                               <asp:AutoCompleteExtender 
                    ID="AutoCompleteExtender1" 
                    TargetControlID="txtIndirizzo" 
                    ContextKey ="ddlComune"
                    CompletionListCssClass="ddl_Autocomplete"
                    UseContextKey="true"
                    CompletionInterval="100" EnableCaching="false" 
                    runat="server" MinimumPrefixLength="5"    ServiceMethod="GetCompletionList" >
                </asp:AutoCompleteExtender>
            </div>
            <div class="collable" style="width:15%">
                <asp:Label ID="lblCivico" CssClass="label" runat="server" Text="<strong>(*)</strong>N&#176;" ToolTip="Numero Civico Domicilio" AssociatedControlID="txtCivico"></asp:Label>
            </div>
            <div class="colOggetti" style="width:8%">
                <asp:TextBox ID="txtCivico" CssClass="textbox" runat="server"  ></asp:TextBox>
            </div>
            <div class="collable" style="width:4%">
                    <asp:ImageButton Enabled="false"  ID="infocivico" runat="server" AlternateText="Informazioni civico domicilio"  ToolTip="NEL CAMPO CIVICO E' POSSIBILE INSERIRE SOLO I SEGUENTI FORMATI:
                            - 21
                            - 21/A 
                            - 21/A5 
                            - 21 BIS 
                            - KM 21,500 
                            OPPURE IL VALORE SNC"
                    ImageUrl="Images/info_xsmall.png" style="cursor:help"/>
            </div>
            <div class="collable" style="width:8%">
                <asp:Label ID="lblCap" CssClass="label"  runat="server" Text="<strong>(*)</strong>C.A.P." AssociatedControlID="txtCap"  ></asp:Label>
            </div>
            <div class="colOggetti" style="width:10%">
                <asp:TextBox ID="txtCap" CssClass="textbox" runat="server" onchange="javascript: CheckNumeroIntero('txtCap','C.A.P.');" ></asp:TextBox>
            </div>
            <div class="collable" style="width:5%">
                <asp:ImageButton ID="imgCap" runat="server" AlternateText="C.A.P." ToolTip="Seleziona il C.A.P." 
                    ImageUrl="Images/valida_small.png"  style="width:20px;height:20px"/>
            </div>
        </div>
 
       <div id="DivGeolocal" class="row" visible="False" runat="server">
            <div class="collable" style="width:15%;padding-top:5px;">
                <asp:Label ID="dummy" CssClass="label" runat="server" Text="Geolocalizzazione"></asp:Label>
            </div>
            <div class="colOggetti" style="width:85%">
                       <asp:ImageButton Enabled="true"  ID="imgGoogleMaps" runat="server" AlternateText="Geolocalizzazione su Google Maps"
                    ToolTip="Geolocalizzazione su Google Maps" 
                        ImageUrl="Images/google-maps32.png" Height="26px"/>
            </div>
 
        </div>
        <div id="DivItalia" class="rowGroup" style="height:auto" runat="server" visible="True">

            <div class="row">
                    <div class="collable" style="width:15%; height: 16px;">  
                        &nbsp;
                    </div>
                    <div class="colOggetti" style="width:5%">  
                        <asp:CheckBox ID="ChkTutela"  AutoPostBack="true" Text=" " runat="server" />
                    </div>
                    <div class="collable" style="width:75%">  
                    <asp:Label ID="lblTutela" CssClass="label"  runat="server" Text="<strong>(*)</strong>la sede è nella disponibilità dell’ente secondo il titolo giuridico indicato, è adeguata al numero di volontari indicati per spazi, servizi, strumenti e attrezzature ed è conforme alle disposizioni del decreto legislativo 9 aprile 2008, n. 81 e s.m.i, in materia di tutela della salute e della sicurezza nei luoghi di lavoro" AssociatedControlID="ChkTutela"/>
                    </div>
             </div>

         </div>          
        <div class="row">
            <div class="collable" style="width:15%">          
                <asp:Label ID="lblDettaglioRecapito" Text="Dettaglio Recapito" AssociatedControlID="TxtDettaglioRecapito" CssClass="label" runat="server"  />
            </div>
            <div class="colOggetti" style="width:80%">
                <asp:TextBox ID="TxtDettaglioRecapito" CssClass="textbox" MaxLength="30" Width="98%"  runat="server"></asp:TextBox>
           </div>
             <div class="colOggetti" style="width:5%">
                <asp:ImageButton ID="imgInfoDettaglio" Enabled="false" runat="server" style="cursor:help" ImageUrl="Images/info_xsmall.png" AlternateText="Info Dettaglio Recapito"  ToolTip="Nel campo Dettaglio Recapito devono essere indicate ESCLUSIVAMENTE informazioni AGGIUNTIVE, se presenti, all' indirizzo per consentire una maggiore accuratezza nelle spedizioni postali. NON VA PERTANTO RIPETUTO L'INDIRIZZO (per il quale già esiste il campo specifico) e NON VANNO INSERITI RECAPITI TELEFONICI o altre informazioni non attinenti. Si fa presente che questo campo NON E' OBBLIGATORIO e quindi VA LASCIATO VUOTO se non è necessario indicare informazioni aggiuntive al recapito relativo."  />
           </div>
        </div>
  
        <div class="row">
            <div class="collable" style="width:15%">          
                <asp:Label ID="lblPalazzina" Text="<strong>(*)</strong>Palazzina" AssociatedControlID="TxtPalazzina" CssClass="label" runat="server"/>
            </div>
            <div class="colOggetti" style="width:10%">
                <asp:TextBox ID="TxtPalazzina" MaxLength="5" CssClass="textbox" runat="server" Width="100px"></asp:TextBox>
           </div>
           <div class="collable" style="width:5%">
                <asp:ImageButton Enabled="false"  ID="imgInfoPalazzina" runat="server" AlternateText="Informazioni palazzina"
                    ToolTip="INDICARE ND NEL CASO NON SIA PREVISTO" 
                        ImageUrl="Images/info_xsmall.png" style="cursor:help" Width="20px"/>
            </div>
            <div class="collable" style="width:8%">          
                <asp:Label ID="lblScala" Text="<strong>(*)</strong>Scala"  AssociatedControlID="TxtScala" CssClass="label" runat="server"  visible="False"  />
            </div>
            <div class="colOggetti" style="width:10%">
                <asp:TextBox ID="TxtScala" MaxLength="5" CssClass="textbox" runat="server" Width="100px" visible="False" Text="ND"></asp:TextBox>
           </div>
           <div class="collable" style="width:5%">
                <asp:ImageButton Enabled="false"  ID="imgInfoScala" runat="server" AlternateText="Informazioni scala"
                    ToolTip="INDICARE ND NEL CASO NON SIA PREVISTO" 
                        ImageUrl="Images/info_xsmall.png" style="cursor:help" visible="False"/>
            </div>
            <div class="collable" style="width:8%">          
                <asp:Label ID="lblPiano" Text="<strong>(*)</strong>Piano" AssociatedControlID="TxtPiano" CssClass="label" runat="server" visible="False"/>
            </div>
            <div class="colOggetti" style="width:10%">
                <asp:TextBox ID="TxtPiano" CssClass="textbox" MaxLength="5" runat="server" Width="100px" visible="False" Text="0"></asp:TextBox>
           </div>&nbsp;
           <div class="collable" style="width:5%">
                <asp:ImageButton Enabled="false"  ID="imgInfoPiano" runat="server" AlternateText="Informazioni piano"
                    ToolTip="INDICARE 0 NEL CASO NON SIA PREVISTO" 
                        ImageUrl="Images/info_xsmall.png" style="cursor:help" visible="False"/>
            </div>
             <div class="collable" style="width:8%">          
                <asp:Label ID="lblInterno" Text="<strong>(*)</strong>Interno" AssociatedControlID="TxtInterno" CssClass="label" runat="server" visible="False"/>
            </div>
            <div class="colOggetti" style="width:10%; height: 26px;">
                <asp:TextBox ID="TxtInterno" CssClass="textbox" MaxLength="5" runat="server" Width="100px" visible="False" Text="ND"></asp:TextBox>
           </div>&nbsp;
                <div class="collable" style="width:5%">
                <asp:ImageButton Enabled="false"  ID="imgInfoInterno" runat="server" AlternateText="Informazioni interno"
                    ToolTip="INDICARE ND NEL CASO NON SIA PREVISTO" 
                        ImageUrl="Images/info_xsmall.png" style="cursor:help" visible="False"/>
            </div>
        </div>
        <div class="row">
            <div class="collable" style="width:15%">          
                <asp:Label ID="lbPrelTelefono" Text="<strong>(*)</strong>Prefisso Telefono" AssociatedControlID="txtprefisso" CssClass="label" runat="server" />
            </div>
            <div class="colOggetti" style="width:10%">
                <asp:TextBox ID="txtprefisso" CssClass="textbox" MaxLength="4" runat="server" Width="100px"></asp:TextBox>
           </div>
           <div class="collable" style="width:5%">
            &nbsp;
           </div> 
           <div class="collable" style="width:8%">          
                <asp:Label ID="lblTelefono" Text="<strong>(*)</strong>Telefono" AssociatedControlID="txtTelefono" CssClass="label" runat="server" />
            </div>

            <div class="colOggetti" style="width:15%">
                <asp:TextBox ID="txtTelefono" CssClass="textbox" MaxLength="9" runat="server" Width="100px" ></asp:TextBox>
           </div>
                <div class="collable" style="width:8%">          
                <asp:Label ID="lblPreFax" Text="Prefisso Fax" AssociatedControlID="txtPrefFax" CssClass="label" runat="server" visible="False"/>
            </div>
            <div class="colOggetti" style="width:10%">
                <asp:TextBox ID="txtPrefFax" CssClass="textbox" MaxLength="4" runat="server" Width="100px" visible="False" Text="0"></asp:TextBox>
           </div>
           <div class="collable" style="width:5%">
            &nbsp;
           </div> 
            <div class="collable" style="width:8%">          
                <asp:Label ID="lblFax" Text="Fax" AssociatedControlID="txtfax" CssClass="label" runat="server" visible="False"/>
            </div>
            <div class="colOggetti" style="width:15%">
                <asp:TextBox ID="txtfax" CssClass="textbox" MaxLength="9" runat="server" Width="100px" visible="False" Text="0"></asp:TextBox>
           </div>
   
        </div>

        <div class="row">
            <div class="collable" style="width:15%">          
                <asp:Label ID="Label1" Text="<strong>(*)</strong>Titolo di disponibilità" AssociatedControlID="ddlTitoloGiuridico" CssClass="label" runat="server" />
            </div>
            <div class="colOggetti" style="width:40%">
                <asp:dropdownlist id="ddlTitoloGiuridico"  AutoPostBack="true" 
                runat="server" CssClass="ddlClass"></asp:dropdownlist>
            </div>
        </div>
        <div class="row">
            <div class="collable" style="width:15%">          
                    <asp:Label ID="lblAltroTitoloGiuridico" Text="Specificare Altro" AssociatedControlID="txtAltroTitoloGiuridicio" CssClass="label" runat="server" />
            </div>
            <div class="colOggetti" style="width:35%">
                  <asp:TextBox ID="txtAltroTitoloGiuridicio" CssClass="textbox" runat="server" 
                      MaxLength="50"></asp:TextBox>
            </div>
            <div class="collable" style="width:15%">          
                <asp:Label ID="lblTipologia" Text="Tipologia" AssociatedControlID="ddlTipologia" CssClass="label" runat="server" />
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:dropdownlist id="ddlTipologia"  AutoPostBack="true"  runat="server" CssClass="ddlClass"  Enabled="false"></asp:dropdownlist>
            </div>
        </div>
       <div id="DivEstero2" class="rowGroup" runat="server" visible="true">
       
              <div id="DivSoggettoEstero" class="row"  runat="server" visible="true" style="width:100%">
                        <div class="row" runat="server" id="rowNoLSE">
                        <div class="collable" style="width:15%">          
                            <asp:Label ID="lblLettera" CssClass="label"  runat="server" Text="<strong>(*)</strong>Lettera di accordo" AssociatedControlID="cmdCaricaFile"/>
                        </div>
                        <div class="colOggetti" style="width:20%">
                            <asp:Button ID="cmdCaricaFile" runat="server" OnClientClick="return ShowPopUp()" CssClass="Pulsante" Text="Carica File"/> 
                        </div>

                </div> 

	        	<div class="row" runat="server" id="rowLSE">
                    <div class="collable" style="width:15%">          
                                &nbsp;
                     </div>			
                     <div class="collable" style="width:85%">                          
                        <table class="table" cellspacing="0" cellpadding="2" rules="all" border="1" id="MainContent_dtgConsultaDocumenti" style="font-size: Small; width: 100%; border-collapse: collapse;">
                            <caption>
						        Lettera di accordo
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
							            <td class="style1">
								            <asp:ImageButton runat="server" ID="btnDownloadLSE" ImageUrl="images/giu_small.png" AlternateText="Scarica Lettera di accordo" ToolTip="Scarica Lettera di accordo"/>
							            </td>
							            <td class="style1">
								            <asp:Literal runat="server" ID="txtLSEFilename"></asp:Literal>
							            </td>
							            <td class="style1">
								            <asp:Literal runat="server" ID="txtLSEData"></asp:Literal></td>
                                        <td class="style1">
								            <asp:Literal runat="server" ID="txtLSEHash"></asp:Literal></td>
							            <td class="style1">
								            <asp:ImageButton runat="server" ID="btnModificaLSE" OnClientClick="return ShowPopUp()" ImageUrl="images/ripristina_small.png" AlternateText="Modifica Lettera di accordo" ToolTip="Modifica Lettera di accordo" />
								            <asp:ImageButton runat="server" ID="btnEliminaLSE" ImageUrl="images/canc_small.png" AlternateText="Elimina Lettera di accordo" ToolTip="Elimina Lettera di accordo" />
							            </td>
						        </tr>
					        </tbody>
				          </table>
                            <br />
                            <br />
                 </div>	
  			    </div>              
                <div class="row">
                        <div class="collable" style="width:15%">          
                            <asp:Label ID="Label3" Text="<strong>(*)</strong>Soggetto estero cui è in capo la Sede" AssociatedControlID="TxtSoggettoCapoSede" CssClass="label" runat="server" />
                        </div>
                        <div class="colOggetti" style="width:80%">
                            <asp:TextBox ID="TxtSoggettoCapoSede" CssClass="textbox" runat="server" 
                             MaxLength="50" Width="200px"></asp:TextBox>
                        </div>

                    </div>

                <div class="row">
                        <div class="collable" style="width:15%">          
                            &nbsp;
                        </div>
                        <div class="collable" style="width:5%">          
                            <asp:RadioButton ID="rbSE1" Text="" GroupName="SiSe"   runat="server" />
                        </div>
                        <div class="collable" style="width:77%">          
                            <asp:Label ID="LblSe1" Text="la sede indicata è nella disponibilità dell’ente in virtù della lettera di accordo allegata, è adeguata al numero di volontari indicati per spazi, servizi, strumenti e attrezzature ed è conforme alle disposizioni dettate in materia di tutela della salute e della sicurezza nei luoghi di lavoro vigenti nel Paese ove è ubicata." AssociatedControlID="rbSE1" CssClass="label" runat="server" />
                        </div>
                     </div>
                <div class="row">
                        <div class="collable" style="width:15%">          
                            &nbsp;
                        </div>
                        <div class="collable" style="width:10%">          
                         oppure
                        </div>
                        <div class="collable" style="width:77%">
                            &nbsp;
                        </div>
                    </div>
                <div class="row">
                        <div class="collable" style="width:15%">          
                            &nbsp;
                        </div>
                        <div class="collable" style="width:5%">          
                            <asp:RadioButton ID="rbSE2" Text="" GroupName="SiSe"   runat="server" />
                        </div>
                        <div class="collable" style="width:77%">          
                            <asp:Label ID="lblSE2" Text="la sede indicata è nella disponibilità dell’ente in virtù della lettera di accordo allegata, è adeguata al numero di volontari indicati per spazi, servizi, strumenti e attrezzature e, considerata l’assenza di norme in materia di tutela della salute e della sicurezza nei luoghi di lavoro nel Paese ove è ubicata la sede, lo svolgimento del servizio avverrà secondo le condizioni di sicurezza previste per i luoghi di lavoro su territorio italiano." AssociatedControlID="rbSE2" CssClass="label" runat="server" />
                        </div>
                    </div>
               <div class="RigaVuota">&nbsp;</div>
              </div>
              <div id="DivNoSoggettoEstero" class="rowGroup" runat="server" visible="true">
                     <div class="row">
                        <div class="collable" style="width:15%">          
                            &nbsp;
                        </div>
                        <div class="collable" style="width:5%">          
                            <asp:RadioButton ID="rbNoSE1" Text="" GroupName="NoSe"   runat="server" />
                        </div>
                        <div class="collable" style="width:77%">          
                            <asp:Label ID="lblCheckNoSE1" Text="la sede è nella disponibilità dell’ente secondo il titolo giuridico indicato, è adeguata al numero di volontari indicati per spazi, servizi, strumenti e attrezzature ed è conforme alle disposizioni dettate in materia di tutela della salute e della sicurezza nei luoghi di lavoro vigenti nel Paese ove è ubicata." AssociatedControlID="rbNoSE1" CssClass="label" runat="server" />
                        </div>
                     </div>
                     <div class="row">
                        <div class="collable" style="width:15%">          
                            &nbsp;
                        </div>
                        <div class="collable" style="width:10%">          
                         oppure
                        </div>
                        <div class="collable" style="width:77%">
                            &nbsp;
                        </div>
                    </div>
                     <div class="row">
                        <div class="collable" style="width:15%">          
                            &nbsp;
                        </div>
                        <div class="collable" style="width:5%">          
                            <asp:RadioButton ID="rbNoSE2" Text="" GroupName="NoSe"   runat="server" />
                        </div>
                        <div class="collable" style="width:77%">          
                            <asp:Label ID="lblCheckNoSE2" Text="la sede è nella disponibilità dell’ente secondo il titolo giuridico indicato, è adeguata al numero di volontari indicati per spazi, servizi, strumenti e attrezzature e, considerata l’assenza di norme in materia di tutela della salute e della sicurezza nei luoghi di lavoro nel Paese ove è ubicata la sede, lo svolgimento del servizio avverrà secondo le condizioni di sicurezza previste per i luoghi di lavoro su territorio italiano" AssociatedControlID="rbNoSE2" CssClass="label" runat="server" />
                        </div>
                    </div>
                    <div class="RigaVuota">&nbsp;</div>
                </div>

        </div>

   <!--Inserire qui sopra il fine wrapper-->
 
         <div class="row">
            <div class="collable" style="width:15%">          
                <asp:Label ID="lblhttp" Text="http://" AssociatedControlID="txthttp" CssClass="label" runat="server" visible="False"/>
            </div>
            <div class="colOggetti" style="width:50%">
                <asp:TextBox ID="txthttp" CssClass="textbox" MaxLength="100" runat="server" ToolTip="Indirizzo Sito" AutoPostBack="true" visible="False"></asp:TextBox>
            </div>
             <div class="colOggetti" style="width:15%">
                <asp:HyperLink ID="cmdHttp" runat="server"  Text="Test Indirizzo web Sito" Target="_blank"></asp:HyperLink>
            </div>
            <div id="checkHttp" runat="server" >
              <div class="collable" style="width:10%">          
                <asp:RadioButton ID="chkVerohttp" Text="Vero" GroupName="checkHttp"   runat="server" />
            </div>
              <div class="collable" style="width:10%">          
                <asp:RadioButton ID="chkFalsohttp" Text="Falso" GroupName="checkHttp"   runat="server" />
            </div>
        </div>
        </div>
        <div class="row">
            <div class="collable" style="width:15%">          
                <asp:Label ID="lblEmail" Text="e-mail" AssociatedControlID="txtemail" CssClass="label" runat="server" visible="False"/>
            </div>
            <div class="colOggetti" style="width:45%">
                <asp:TextBox ID="txtemail" CssClass="textbox" MaxLength="75" runat="server" ToolTip="Indirizzo di Posta Elettronica" onchange="javascript: CheckEmail('txtemail', 'e-mail')" visible="False"></asp:TextBox>
            </div>
            <div class="colOggetti" style="width:5%">
             <asp:ImageButton id="cmdEmail" runat="server" ImageUrl="Images/valida_small.png" AlternateText="Verifica EMail Valida" ToolTip="Verifica EMail Valida" ></asp:ImageButton>
            </div>
            <div id="checkEmail" runat="server" >
              <div class="collable" style="width:10%">          
                <asp:RadioButton ID="chkVeroEmail" Text="Vero" GroupName="checkEmail"   runat="server" />
            </div>
              <div class="collable" style="width:10%">          
                <asp:RadioButton ID="chkFalsoEmail" Text="Falso" GroupName="checkEmail"   runat="server" />
            </div>
            </div>
        </div>
      </div> 

      <div id="GestioneCertificazioneNote" style="border:0px;display:table" runat="server" class="row" >
            <div class="row">
                <div class="collable" style="width:15%">
                <asp:Label ID="Label2" CssClass="label"  runat="server" Text="Presenza Iscrizione" AssociatedControlID="ddlCertificazione"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:dropdownlist id="ddlCertificazione"  AutoPostBack="true" 
                    runat="server" CssClass="ddlClass"  >
                    <asp:ListItem Value="0">No</asp:ListItem>
				    <asp:ListItem Value="1">Si</asp:ListItem>
				    <asp:ListItem Value="2">Da Valutare</asp:ListItem>
                    </asp:dropdownlist>
            </div>
            <div class="collable" style="width:15%">
                <asp:Label ID="lblUser" CssClass="label" runat="server" Text="User Iscrizione" AssociatedControlID="TxtUserCert"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">           
               <asp:TextBox ID="TxtUserCert" CssClass="textbox" runat="server" Enabled ="false" ></asp:TextBox>
            </div>
                    </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblData" CssClass="label" runat="server" Text="Data Iscrizione" AssociatedControlID="TxtDataCert"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%"> 
                  <asp:TextBox ID="TxtDataCert" CssClass="textbox" runat="server" Enabled ="false" ></asp:TextBox>
                </div>
                
            </div>
        <div class="row">
            <div class="collable" style="width:15%">          
                <asp:Label ID="lblNote" Text="Note" AssociatedControlID="txtNote" CssClass="label" runat="server" />
            </div>
            <div class="colOggetti" style="width:85%">
                    <asp:TextBox ID="txtNote" CssClass="textbox" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
            </div>
        </div>
     </div>
      <div class="row">
      <asp:CheckBox ID="chkRiferimentoRimborsi" runat="server" Text="Sede di riferimento per rimborsi" AutoPostBack="true" Visible="false"/>
      </div>
       <div class="wrapper"  id="divSedeSanzionata" runat="server" visible="false" style="width:100%;border:0px;height:auto">
       <div class="row">
       <div class="collable" style="width:15%">          
                <asp:Label ID="lblSanzione" Visible="false" CssClass="labelDati" runat="server" />
            </div>
        </div>
         <div  class="RigaPulsanti" >
            <asp:Button ID="imgSanzione" CssClass="Pulsante" runat="server" ToolTip="Sanziona" Text="Sanziona"  />
             <asp:Button ID="imgRipristinaSanzione" CssClass="Pulsante" runat="server" ToolTip="Ripristina Sanzione" Text="Ripristina Sanzione"  />
         </div>
        </div>


    <br />
      <div  class="RigaPulsanti" align="right">

      <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" ToolTip="Salva" Text="Salva"  />
      <asp:Button ID="cmdCancella" CssClass="Pulsante" runat="server"  ToolTip="Cancella Sede" Text="Cancella" />
      <asp:Button ID="cmdAnnullaModifica" CssClass="Pulsante" runat="server" Text="Annulla Modifica"  />
      <asp:Button ID="cmdAnnullaCancellazione" CssClass="Pulsante" runat="server"  Text="Annulla Cancellazione"  />
      <asp:Button ID="cmdRipristina" CssClass="Pulsante" runat="server"  Text="Aggiungi Sede Attuazione"  />     
      <asp:Button ID="cmdAccredita" CssClass="Pulsante" runat="server" Text="Accredita"  ToolTip="Accredita Sede" Visible="False" />
      <asp:Button ID="cmdIncludi" CssClass="Pulsante" runat="server" Text="Includi"  ToolTip="Includi Sede"  />
      <asp:Button ID="cmdAnnulla" CssClass="Pulsante" runat="server" Text="Annulla Inclusione"  ToolTip="Annulla Inclusione"  />
      <asp:Button ID="cmdVisualizzaDatiAccreditati" CssClass="Pulsante" runat="server"  ToolTip="Visualizza Dati Accreditati"  Text="Visualizza Dati Accreditati"  />
      <asp:Button ID="cmdAnnullaAccred" CssClass="Pulsante" runat="server" ToolTip="Annulla Iscrizione Sede" Text="Annulla Accreditamento" Visible="false" />
      <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server"  Text="Chiudi"  />

      </div>
      
   
            <br />
            <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" Caption="Elenco Sedi"
                    ToolTip="Elenco Sedi" CssClass="table"  AllowPaging="True" 
                    AllowSorting="True" AutoGenerateColumns="False" 
                    UseAccessibleHeader="True">
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		        <HeaderStyle></HeaderStyle>
		        <Columns>
                    <asp:TemplateColumn HeaderText="Selez." Visible="false"> 
                        <ItemTemplate>
	                        <asp:ImageButton ID="IdImgSelSedi" style="cursor:pointer;" CommandName="Select" alt="Seleziona Sede" ToolTip='Seleziona Sede' runat="server" ImageURL="images/sedi_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="identesedeattuazione" HeaderText="Cod. Sede"></asp:BoundColumn>
					<asp:BoundColumn DataField="Denominazione" HeaderText="Sede"></asp:BoundColumn>
					<asp:BoundColumn DataField="StatoEnteSede" HeaderText="Stato"></asp:BoundColumn>
					<asp:BoundColumn DataField="Note" HeaderText="Note" Visible="false"></asp:BoundColumn>
                    <asp:TemplateColumn Visible="false">
						<HeaderTemplate>
							<strong>Inclusa</strong>
						</HeaderTemplate>
						<ItemTemplate>
							&nbsp;&nbsp;
							<asp:CheckBox id="check1" runat="server" AutoPostBack="True" ToolTip="Sede Inclusa"></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn Visible="false" DataField="identeRelazione" HeaderText="idrel">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
                    <asp:BoundColumn Visible="False" DataField="acquisita" HeaderText="idass"></asp:BoundColumn>
                    <asp:BoundColumn Visible="False" DataField="idEntePadre" HeaderText="idEntePadre"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="idEnteFiglio" HeaderText="idEnteFiglio"></asp:BoundColumn>
					<asp:BoundColumn DataField="Certificazione" HeaderText="Presenza Iscrizione">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
	                <asp:TemplateColumn HeaderText="Anomalie" > 
                         <ItemStyle width="5%"></ItemStyle>
                        <ItemTemplate>
	                     <asp:ImageButton ID="IdImgAlert" runat="server" style="cursor:pointer;" alt="Il nome o indirizzo della sede presentano anomalie" ToolTip="Il nome o indirizzo della sede presentano anomalie" ImageURL="images/alert_small.png" visible = "false" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn Visible="False" DataField="AnomaliaNome" HeaderText="AnomaliaNome"></asp:BoundColumn>
                    <asp:BoundColumn Visible="False" DataField="AnomaliaIndirizzo" HeaderText="AnomaliaIndirizzo"></asp:BoundColumn>
                    <asp:BoundColumn Visible="False" DataField="AnomaliaIndirizzoGoogle" HeaderText="AnomaliaIndirizzoGoogle"></asp:BoundColumn>
				</Columns>
		    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
	    </asp:datagrid>

                <asp:HiddenField ID="txtRipristinoSanzione" runat="server" Value="NO" />
                <asp:HiddenField ID="txtCancellaSede" runat="server"  />
                 <asp:HiddenField ID="lblCodiceSede" runat="server"  />
                 <asp:HiddenField ID="txtCodice" runat="server"  />
                 <asp:HiddenField ID="lblidEnte" runat="server"  />
                 <asp:HiddenField ID="lblTipoUtente" runat="server"  />
                 <asp:HiddenField ID="idEnteFiglio" runat="server"  />
                 <asp:HiddenField ID="IDEnteProprietario" runat="server" />
                 <asp:HiddenField ID="lblPersonalizza" runat="server"  />
                 <asp:HiddenField ID="txtdataserver" runat="server"  />
                 <asp:HiddenField ID="lblsql" runat="server"  />
                  <asp:HiddenField ID="lblpage" runat="server"  />
                 <asp:HiddenField ID="txtidsede" runat="server"  />
                 <asp:HiddenField ID="lblacquisita" runat="server"  />
                 <asp:HiddenField ID="txtTipologia" runat="server"  />
                <asp:HiddenField ID="txtCopiaSEDE" runat="server"  />
                <asp:HiddenField ID="txtCopiaIndirizzo" runat="server"  />
                <asp:HiddenField ID="lblDataControlloHttp" runat="server"  />
                <asp:HiddenField ID="lbldataControlloEmail" runat="server"  />
                 <asp:HiddenField ID="chkStatoEnte" runat="server"  />
  
            </fieldset>
            <%--POPUP File--%>
            <asp:ModalPopupExtender runat="server" ID="popUpload" TargetControlID="hfPopup" 
                PopupControlID="pnlUpload" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
                PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpControl" DynamicServicePath="" Enabled="True" CancelControlID="btnCloseUpload"> 
            </asp:ModalPopupExtender>
	        <asp:Panel ID="pnlUpload" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
                <asp:HiddenField runat="server" ID="hfPopup" />
                <asp:label id="lblErroreUpload"  runat="server" CssClass="msgErrore" Visible="False" AssociatedControlID="cmdAllega"></asp:label>
                <p class="msgInfo">Caricamento Lettera di accordo</p>
                <fieldset>
			        <input id="fileLSE" type="file" runat="server" />
                    <div style="margin-top: 1em;"></div>
                    <asp:Button ID="cmdAllega" runat="server" CssClass="Pulsante" Text="Carica File"/>
                    <asp:Button ID="btnCloseUpload" runat="server" CssClass="Pulsante" Text="Chiudi"/>
                </fieldset>
	        </asp:Panel>
            <%--POPUP Messaggio di controllo sede--%>
            <asp:ModalPopupExtender runat="server" ID="PopUpControlloSede" TargetControlID="hfPopup2" 
                PopupControlID="pnlConrolloSede" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
                PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpControl2" DynamicServicePath="" Enabled="True" CancelControlID="btnCloseControllo"> 
            </asp:ModalPopupExtender>
	        <asp:Panel ID="pnlConrolloSede" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
                <asp:HiddenField runat="server" ID="hfPopup2" />
                <p class="msgInfo">Errori nel Controllo formale di Nome e Indirizzo Sede</p>
                <asp:label id="lblErroreControlloSede"  runat="server" CssClass="msgErrore" AssociatedControlID="cmdProcedi"></asp:label>
                <br/>
                <asp:HiddenField runat="server" ID="HiddenField1" />
                <br/>
                <div id="divSpiegazioni" runat="server" visible="false">
                      <p class="msgInfo" style="text-align:left; font-size:smaller">Il nome sede non dovrebbe:</p>
                <ul> 
                    <li>contenere 3 o più lettere consecutive uguali (ad esempio aaaa, bbbb, ecc.)
                    <li>contenere coppie ripetute di lettere diverse per 3 o più volte (ad esempio dfdfdfdf) 
                    <li>coincidere con altri nomi di sede tolti eventuali caratteri numerici (esempio “Sede 1”, “Sede 2”, ecc.)
                 </ul> 
                 </div>
                <fieldset>
			        <asp:Button ID="cmdProcedi"  runat="server" CssClass="Pulsante" Text="Procedi con il salva"/>&nbsp;&nbsp;
                    <asp:Button ID="btnCloseControllo" runat="server" CssClass="Pulsante" Text="Annulla"/>
                </fieldset>
	        </asp:Panel>
             <%--POPUP Messaggio di controllo geolocalizzazione--%>
            <asp:ModalPopupExtender runat="server" ID="PopUpGeolocalizzazione" TargetControlID="hfPopup3" 
                PopupControlID="pnlGeolocalizzazione" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
                PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpControl3" DynamicServicePath="" Enabled="True" CancelControlID="btnCloseGeolocalizzazione"> 
            </asp:ModalPopupExtender>
	        <asp:Panel ID="pnlGeolocalizzazione" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
                <asp:HiddenField runat="server" ID="hfPopup3" />
                <p class="msgInfo">Controllo Esistenza Indirizzo</p>
                <asp:label id="lblErrGeolocalizzazione"  runat="server" CssClass="msgErrore" Visible="False" style="text-align:center" AssociatedControlID="cmdProcediGeolocalizzazione"></asp:label>
                <asp:label id="lblGeolocalizzazione"  runat="server" CssClass="msgConferma" Visible="False"></asp:label>
                 <fieldset>
                 <asp:Button ID="cmdProcediGeolocalizzazione" runat="server" CssClass="Pulsante" Text="Procedi con il salva"/>&nbsp;&nbsp;
			       <asp:Button ID="btnCloseGeolocalizzazione" runat="server" CssClass="Pulsante" Text="Annulla"/>
                </fieldset>
	        </asp:Panel>
              <%--POPUP GoogleMaps--%>
            <asp:ModalPopupExtender runat="server" ID="PopUpMaps" TargetControlID="hfPopup4" 
                PopupControlID="PnlMaps" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
                PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpControl4" DynamicServicePath="" Enabled="True" CancelControlID="btnCloseMaps"> 
            </asp:ModalPopupExtender>
	        <asp:Panel ID="PnlMaps" runat="server" Style="background-color: White; padding-left: 20px; padding-right:0px; padding-top:10px; padding-bottom:10px; width: 100%; overflow-y: auto; max-height: 90vh">
                <asp:HiddenField runat="server" ID="hfPopup4" />
                <p class="msgInfo">Google Maps</p>
                  <div id="map" align="center"></div>
              <br />
                    <div class="collable" style="width:96%" align="center"> 
                     <asp:Button ID="btnCloseMaps" runat="server" CssClass="Pulsante" Text="Chiudi"/>
                   </div>
                            
	        </asp:Panel>


                 <%--POPUP Messaggio Conferma Salvataggio--%>
            <asp:ModalPopupExtender runat="server" ID="PopupSalvataggio" TargetControlID="hfPopup5" 
                PopupControlID="pnlSalvataggio" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
                PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpControl5" DynamicServicePath="" Enabled="True" CancelControlID="btnAnnullaSalvataggio"> 
            </asp:ModalPopupExtender>
	        <asp:Panel ID="pnlSalvataggio" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
                <asp:HiddenField runat="server" ID="hfPopup5" />
                <p class="msgInfo">Conferma Salvataggio Dati</p>
                <asp:label id="lblAvvisoSalvataggio"  runat="server" CssClass="msgInfo" Visible="True" style="text-align:center" AssociatedControlID="cmdProcediSalvataggio" Text="Si confermano le modifiche ai dati</br>da inviare al Dipartimento?"></asp:label>
                <fieldset>
                <asp:Button ID="cmdProcediSalvataggio" runat="server" CssClass="Pulsante" Text="Procedi con il salva"/>&nbsp;&nbsp;
			       <asp:Button ID="btnAnnullaSalvataggio" runat="server" CssClass="Pulsante" Text="Annulla"/>
                </fieldset>
	        </asp:Panel>
        
</asp:Content>
