<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmProgrammi.aspx.vb" Inherits="Futuro.WfrmProgrammi" MaintainScrollPositionOnPostBack="true" %>
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
       /* <![CDATA[ */
        var pathPlus = "images/plus_xsmall.png";
        var pathMinus = "images/minus_xsmall.png";
        var titlePathMinus = "Riduci";
        var titlePathPlus = "Espandi";
        var inserimento = '<%=Request.Params("IdAttivita")%>';
        var isPostbackEnte = '<%= hdnIsPostbackEnte.value %>' ;
        var isPostbackProgetto = '<%= hdnIsPostbackProgetto.value %>' ;
        var isPostbackFormazioneSpecifica  = '<%= hdnIsPostbackFS.value %>' ;
        var isPostbackFormazioneGenerale = '<%= hdnIsPostbackFG.value %>' ;
        var isPostbackFormazioneProgetto = '<%= hdnIsPostbackProg.value %>' ;
        var isPostbackConoscenzeAcquisite = '<%= hdnIsPostbackCCA.value %>' ;
        var isPostbackCaratteristicheOrganizzative = '<%= hdnIsPostbackCO.value %>' ;
        var isPostbackEstero = '<%= hdnIsPostbackEstero.value %>' ;
  
        function CheckNumeroValido(valore, descrizione) {
            var clientIdErrore = CostruisciId("lblerrore")
            var numeroValido = ValidaNumero(CostruisciId(valore), descrizione, clientIdErrore);
            if (numeroValido == false) {
                TornaAdInizioPagina();
            }
        };

      
           /* ]]> */
    </script>
 
   
    <script type="text/javascript">
        /* <![CDATA[ */
          $(function () {
            
            var tornaSu = 0;
           if ('<%=request.querystring("idProgramma")%>' == '') {
               
                $('#CaratteristicheEnteContent').css('display', 'none');
                $('#CaratteristicheEnte').find("img").first().attr("src", pathPlus);
                $('#CaratteristicheEnte').find("img").first().attr("title", titlePathPlus);
            }

             $('#TornaSuEnti').click(function () 
             {
                tornaSu = 1;
                
             });
            $('#CaratteristicheEnte').click(function () 
            {
             
                if (tornaSu == 0) 
                {
                    var isVisible = $('#CaratteristicheEnteContent').is(':visible');
                    $('#CaratteristicheEnteContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    if (!isVisible) 
                    {
                        $(this).find("img").first().attr("src", pathMinus);
                        $(this).find("img").first().attr("title", titlePathMinus);
                        title = "Espandi"
                    }
                    else 
                    {
                        $(this).find("img").first().attr("src", pathPlus);
                        $(this).find("img").first().attr("title", titlePathPlus);
                    }
                }
                tornaSu = 0;
                return false;
            });
        });
        $(function () 
        {
           
         
            var tornaSu = 0;
          
                
                $('#ProgettoContent').css('display', 'yes');
                $('#Progetto').find("img").first().attr("src", pathMinus);
                $('#Progetto').find("img").first().attr("title", titlePathMinus);

          
            $('#TornaSuProgetto').click(function () 
            {
                tornaSu = 1;
            });
            $('#Progetto').click(function () 
            {
              
                if (tornaSu == 0) 
                {
              
                    var isVisible = $('#ProgettoContent').is(':visible');
                    $('#ProgettoContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    if (!isVisible) 
                    {
                      
                        $(this).find("img").first().attr("src", pathMinus);
                        $(this).find("img").first().attr("title", titlePathMinus);
                        title = "Espandi"
                    }
                    else 
                    {
                     
                        $(this).find("img").first().attr("src", pathPlus);
                        $(this).find("img").first().attr("title", titlePathPlus);
                       
                    }
                }
                tornaSu = 0;
                return false;
            });
        });
        $(function () 
        {
        
            var tornaSu = 0;
           
                $('#CaratteristicheOrganizzativeContent').css('display', 'yes');
                $('#CaratteristicheOrganizzative').find("img").first().attr("src", pathMinus);
                $('#CaratteristicheOrganizzative').find("img").first().attr("title", titlePathMinus);
                
            
            $('#TornaSuCaratteristicheOrganizzative').click(function () {
                tornaSu = 1;
            });
            $('#CaratteristicheOrganizzative').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#CaratteristicheOrganizzativeContent').is(':visible');
                    $('#CaratteristicheOrganizzativeContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    if (!isVisible) {
                        $(this).find("img").first().attr("src", pathMinus);
                        $(this).find("img").first().attr("title", titlePathMinus);
                        title = "Espandi"
                    }
                    else {
                        $(this).find("img").first().attr("src", pathPlus);
                        $(this).find("img").first().attr("title", titlePathPlus);
                    }
                }
                tornaSu = 0;
                return false;
            });
        });
       $(function () {
       
            var tornaSu = 0;
           
         
                $('#FormazioneGeneraleContent').css('display', 'yes');
                $('#FormazioneGenerale').find("img").first().attr("src", pathMinus);
                $('#FormazioneGenerale').find("img").first().attr("title", titlePathMinus);
      
            $('#TornaSuFormazioneGenerale').click(function () {
                tornaSu = 1;
            });
            $('#FormazioneGenerale').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#FormazioneGeneraleContent').is(':visible');
                    $('#FormazioneGeneraleContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    if (!isVisible) {
                        $(this).find("img").first().attr("src", pathMinus);
                        $(this).find("img").first().attr("title", titlePathMinus);
                          title = "Espandi"
                    }
                    else {
                        $(this).find("img").first().attr("src", pathPlus);
                        $(this).find("img").first().attr("title", titlePathPlus);
                    }
                }
                tornaSu = 0;
                return false;
            });
        });
           $(function () {
       
            var tornaSu = 0;
             
                $('#FormazioneSpecificaContent').css('display', 'yes');
                $('#FormazioneSpecifica').find("img").first().attr("src",pathMinus );
                $('#FormazioneSpecifica').find("img").first().attr("title",titlePathMinus );
      
            $('#TornaSuFormazioneSpecifica').click(function () {
                tornaSu = 1;
            });
            $('#FormazioneSpecifica').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#FormazioneSpecificaContent').is(':visible');
                    $('#FormazioneSpecificaContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    if (!isVisible) {
                        $(this).find("img").first().attr("src", pathMinus);
                        $(this).find("img").first().attr("title", titlePathMinus);
                        title = "Espandi"
                    }
                    else {
                        $(this).find("img").first().attr("src", pathPlus);
                        $(this).find("img").first().attr("title", titlePathPlus);
                    }
                }
                tornaSu = 0;
                return false;
            });
        });

          $(function () {
          
             var tornaSu = 0;
            
           
                $('#EsteroContent').css('display', 'yes');
                $('#Estero').find("img").first().attr("src", pathMinus);
                $('#Estero').find("img").first().attr("title", titlePathMinus);
           
             $('#TornaSuEstero').click(function () {
                 tornaSu = 1;
             });
             $('#Estero').click(function () {
                 if (tornaSu == 0) {
                     var isVisible = $('#EsteroContent').is(':visible');
                     $('#EsteroContent').slideToggle('fast');
                     $(this).toggleClass('slideSign');
                     if (!isVisible) {
                         $(this).find("img").first().attr("src", pathMinus);
                         $(this).find("img").first().attr("title", titlePathMinus);
                         title = "Espandi"
                     }
                     else {
                         $(this).find("img").first().attr("src", pathPlus);
                         $(this).find("img").first().attr("title", titlePathPlus);
                     }
                 }
                 tornaSu = 0;
                 return false;
             });

         });


          $(function () {
          
             var tornaSu = 0;
            
           
                $('#ContatoriContent').css('display', 'none');
                $('#Contatori').find("img").first().attr("src", pathPlus);
                $('#Contatori').find("img").first().attr("title", titlePathPlus);
           
             $('#TornaSuContatori').click(function () {
                 tornaSu = 1;
             });
             $('#Contatori').click(function () {
                 if (tornaSu == 0) {
                     var isVisible = $('#ContatoriContent').is(':visible');
                     $('#ContatoriContent').slideToggle('fast');
                     $(this).toggleClass('slideSign');
                     if (!isVisible) {
                         $(this).find("img").first().attr("src", pathMinus);
                         $(this).find("img").first().attr("title", titlePathMinus);
                         title = "Espandi"
                     }
                     else {
                         $(this).find("img").first().attr("src", pathPlus);
                         $(this).find("img").first().attr("title", titlePathPlus);
                     }
                 }
                 tornaSu = 0;
                 return false;
             });

         });


          
                /* ]]> */
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset id="GestioneProgetti"  class="ContornoPagina" runat="server" >
<legend >Programma</legend>
<div class="wrapper"  style="width:100%;border:0px">
  <div class="headers" >
    <h2>
      <asp:label  id="lblTitoloProgramma" style="vertical-align:middle" runat="server"></asp:label>
    </h2>
   </div>
<div class="RigaVuota">&nbsp;</div>
  <div class="row" style="height:auto">
<asp:Label ID="lblMessaggioConferma"  CssClass="msgConferma" runat="server"></asp:Label>
<asp:Label ID="lblerroreSettore" CssClass="msgErrore" runat="server"></asp:Label>
<asp:Label ID="lblareaerrore" CssClass="msgErrore" runat="server"></asp:Label>
<asp:Label ID="lblerrore" CssClass="msgErrore" runat="server"></asp:Label>


</div>
  <div class="RigaVuota">&nbsp;</div>
  <div class="rowGroup" id="idlinkalti" runat="server" style="height:auto"> 
    <div class="row" style="height:auto">  
          <div class="colHyperLink" style="width:50%">
                <asp:LinkButton ID="imgProgettiProgramma" runat="server" style="cursor:pointer" Text="Gestione Progetti Programma"></asp:LinkButton>
            </div>
            <div class="colHyperLink" style="width:50%">
                <asp:LinkButton ID="imgElencoDocumentiProg"  Text="Documenti Programma" style="cursor:pointer" runat="server"></asp:LinkButton>
            </div>    
    </div>
   
</div>
 
  <fieldset id="flsEnte" runat="server" class="ContornoPagina" >
 <legend id="CaratteristicheEnte"><img  src="images/minus_xsmall.png"  alt="Espandi/Riduci" style="cursor:pointer;" />&nbsp;&nbsp;<img id="TornaSuEnti"  style="cursor:pointer;" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />ENTE</legend>
  <div class="wrapper" id="CaratteristicheEnteContent2" style="width:100%;border:0px" >
    <div class="row">
                    <div class="collable" style="width:10%">
                        <asp:Label ID="lblcodiceente" AssociatedControlID="lblcodiceentePropo" Text="Codice Ente" runat="server" CssClass="label"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:40%">
                        <asp:Label id="lblcodiceentePropo"  CssClass="label"  runat="server"/>
                    </div>

                    <div class="collable" style="width:10%">
                        <asp:Label ID="lbldenominazione" AssociatedControlID="lblDenominazione" Text="Denominazione" runat="server" CssClass="label"></asp:Label>
                    </div>

                    <div class="colOggetti" style="width:40%">
                        <asp:Label id="lblDenominazioneEnte" CssClass="label"  runat="server"/>
                    </div>

                </div>
  </div>
 <div class="wrapper" id="CaratteristicheEnteContent" style="width:100%;border:0px" >
     <fieldset id="coop">
     <legend >Enti Coprogrammanti</legend>
     <div class="rowGroup" id="divCoProgrammazione" runat="server" visible="true">
       <div class="row">
       <div class="collable" style="width:45%">
        <asp:CheckBox id="chkCoProgrammato" Runat="server" Enabled="false" Text="Programma in coprogrammazione"></asp:CheckBox>
       </div>
       <div class="colOggetti" style="width:15%">
         <asp:ImageButton id="imgCoProgrammazione" ToolTip="Gestione Enti Coprogrammanti" AlternateText="Gestione Enti Coprogrammanti" ImageUrl="Images/Icona_Progetto_small.png" Runat="server"></asp:ImageButton>
       </div>       
   </div>
     </div>
     <div  style="width:100%;">
       <h3>
            <asp:label id="lblmessaggi2" runat="server" AssociatedControlID="dgRicercaEnteCoprogrammante"></asp:label>
        </h3>
        <br />
        <asp:datagrid id="dgRicercaEnteCoprogrammante" runat="server" Width="100%" Font-Size="Small" ToolTip="Elenco Enti Coprogrammanti" CssClass="table" CellPadding="2"   AllowSorting="True" AutoGenerateColumns="False"  Visible="False" UseAccessibleHeader="True" >	
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
           
				<asp:BoundColumn Visible="False" DataField="idente" HeaderText="idente"></asp:BoundColumn>
				<asp:BoundColumn DataField="CODICE_ENTE" HeaderText="Cod.Ente">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="ENTE" HeaderText="Denominazione"></asp:BoundColumn>
			</Columns>
			
		</asp:datagrid>
   </div>
  
     </fieldset>
  </div>
 </fieldset>
  <fieldset id="flsProgetto" class="ContornoPagina" >
  <legend id="Progetto"><img style="cursor:pointer;" src="images/minus_xsmall.png" alt="Espandi/Riduci" />&nbsp;&nbsp;<img style="cursor:pointer;" id="TornaSuProgetto" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Caratteristiche 
      Programma</legend> 
 <div class="wrapper" id="ProgettoContent" style="width:100%;border:0px" >
 
   <div  class="row">
       <div class="collable" style="width:15%">
    <asp:Label Id="Label2" Text="Titolo Programma" AssociatedControlID="txtTitoloProgramma"  runat="server"  CssClass="label"></asp:Label>
   
       </div>
       <div class="colOggetti" style="width:80%">
    <asp:TextBox ID="txtTitoloProgramma" runat="server"   CssClass="textbox" ></asp:TextBox>

       </div>
      
   </div>
   <div  class="row" id="DivTipoProgramma" runat="server">
        <div class="collable" style="width:15%">
            <asp:Label ID="lblTipoPRogramma"  Text="Tipo Programma" CssClass="label" AssociatedControlID="ddlTipoProgramma" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%">
            <asp:DropDownList ID="ddlTipoProgramma" runat="server" CssClass="ddlClass" AutoPostBack="True"></asp:DropDownList>
        </div>
   </div>
   <div  class="row" id="DivProgrammiGG" runat="server">
        <div class="collable" style="width:15%">
            <asp:Label ID="lblRegioneGG"  Text="Regione" CssClass="label" AssociatedControlID="ddlRegioneGG" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%">
            <asp:DropDownList ID="ddlRegioneGG" runat="server" CssClass="ddlClass" AutoPostBack="True"></asp:DropDownList>
        </div>
        <div class="collable" style="width:15%;text-align:center">
            <asp:Label ID="lblMisuraGG"  Text="Misura/Destinatari" CssClass="label" AssociatedControlID="ddlMisuraGG" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%">
            <asp:DropDownList ID="ddlMisuraGG" runat="server" CssClass="ddlClass"></asp:DropDownList>
        </div>
   </div>
</div>
    
    <div class="RigaVuota">
    &nbsp;
    </div>
    </fieldset>
  <fieldset id="flsCaratteristicheOrganizzative" class="ContornoPagina"  >
 <legend id="CaratteristicheOrganizzative"><img  src="images/minus_xsmall.png"  alt="Espandi/Riduci" style="cursor:pointer;" />&nbsp;&nbsp;<img id="TornaSuCaratteristicheOrganizzative"  style="cursor:pointer;" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Elenco 
     Progetti</legend>
   <div class="wrapper" id="CaratteristicheOrganizzativeContent" style="width:100%;border:0px" >
      <div class="row" style="height:auto">
        <div class="collable" style="width:100%">
        <asp:Label id="lblprogetti" CssClass="msgInfo" runat="server" Visible="false" Text="Per selezionare i progetti è necessario effettuare prima l'inserimento del programma."></asp:Label>
        </div>
       </div>
      <div  style="width:100%;">
       <h3>
            <asp:label id="lblMesselencoProgetti" runat="server" AssociatedControlID="dgElencoProgettiProgramma"></asp:label>
        </h3>
        <br />
        <asp:datagrid id="dgElencoProgettiProgramma" runat="server" Width="100%" Font-Size="Small" ToolTip="Elenco Progetti Programma" CssClass="table" CellPadding="2"  AllowPaging="false" AllowSorting="True" AutoGenerateColumns="False"  Visible="False" UseAccessibleHeader="True" PageSize="5">	
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
           
				<asp:BoundColumn Visible="False" DataField="idAttività" HeaderText="idattività"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="IdProgramma" HeaderText="IdProgramma"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="IdEnteReferenteProgramma" HeaderText="IdEnteReferenteProgramma"></asp:BoundColumn>
				<asp:BoundColumn Visible="true" DataField="Titolo" HeaderText="Titolo"></asp:BoundColumn>
                <asp:BoundColumn DataField="NPostiVol" HeaderText="N° Posti Volontario">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
                <asp:BoundColumn Visible="false" DataField="MesiDurata" HeaderText="MesiDurata"></asp:BoundColumn>
				<asp:TemplateColumn HeaderText="Selez.">
		    <ItemTemplate>           
                <asp:ImageButton ID="btnSeleziona" AlternateText="Selezione Progetto"   runat="server" CommandName="Select"
                                ImageUrl="~/Images/Icona_Progetto_small.png"  ToolTip="Selezione Progetto" />
            </ItemTemplate>         
            </asp:TemplateColumn>
            <asp:BoundColumn Visible="False" DataField="IdTipoProgetto" HeaderText="IdTipoProgetto"></asp:BoundColumn>
            </Columns>

			
		</asp:datagrid>
   </div>

   <fieldset id="flsContatori">
   <legend id="Contatori"><img  src="images/plus_xsmall.png"  alt="Espandi/Riduci" style="cursor:pointer;" />&nbsp;&nbsp;<img id="TornaSuContatori"  style="cursor:pointer;" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" /> 
       Riepiloghi</legend>
   <div id="ContatoriContent" style="width:100%;border:0px">
  
   <fieldset>
    <legend>Riepilogo Dati Programma</legend>
     <div class="RigaVuota" style="width:100%;border:0px">&nbsp;</div>
     <div class="wrapper" id="Div1" style="width:100%;border:0px" >
     <div  class="row" id="NvolEDurata" runat="server" style="width:100%;border:0px">
        <div class="collable" style="width:33%;text-align:center;border-style:groove;border-width:thin">
       <div >
        <asp:Label Id="lbltotprogetti" Text="Numero Tot Progetti" AssociatedControlID="txttotprogetti"  runat="server"  CssClass="label"></asp:Label>
       </div>
       <div>
            <asp:Label ID="txttotprogetti" runat="server" Font-Bold="true" Enabled="false" AssociatedControlID="lbltotprogetti" CssClass="label" ></asp:Label>
       </div>
       </div>
       <div class="collable" style="width:33%;text-align:center;border-style:groove;border-width:thin">
       <div >
        <asp:Label Id="lbltotvol" Text="Numero Tot Volontari" AssociatedControlID="txttotvol1"  runat="server"  CssClass="label"></asp:Label>
       </div>
       <div>
            <asp:Label ID="txttotvol1" runat="server" Font-Bold="true" Enabled="false" AssociatedControlID="lbltotvol" CssClass="label" ></asp:Label>
       </div>
       </div>
       <div class="collable" style="width:33%;text-align:center;border-style:groove;border-width:thin">
       <div >
        <asp:Label Id="lbldurata" Text="Durata Programma (Mesi)" AssociatedControlID="txtdurataProgramma"  runat="server"  CssClass="label"></asp:Label>
       </div>
       <div>
        <asp:Label ID="txtdurataProgramma" runat="server" Font-Bold="true" AssociatedControlID="lbldurata" Enabled="false"  CssClass="label" ></asp:Label>
       </div>
       </div>
    </div>
    </div>
   </fieldset>
  
   <fieldset>
   <legend>Conteggio Progetti con Ulteriori Misure Eventualmente Adottate</legend>
     <div class="RigaVuota">&nbsp;</div>
     <div class="wrapper" id="Div2" style="width:100%;border:0px" >
     <div  class="row" id="ContatoriGMOUETUTORAGGIO1" runat="server" style="width:100%;border:0px">
       <div class="collable" style="width:19%;border-style:groove;border-width:thin">
       <div style="height:30px;text-align:center">
        <asp:Label Id="lblgmo" Text="Progetti con GMO" AssociatedControlID="txtPogetGMO1"  runat="server"  CssClass="label"></asp:Label>
        </div>
        <div style="text-align:center">
       <asp:Label ID="txtPogetGMO1" Width="5%" runat="server" Font-Bold="true" AssociatedControlID="lblgmo"  Enabled="false"  CssClass="label" ></asp:Label>
       </div>
     </div>

       <div class="collable" style="width:20%;border-style:groove;border-width:thin">
       <div style="height:30px;text-align:center">
        <asp:Label Id="lblPogetUE" Text="Progetti con max 3 mesi UE" AssociatedControlID="txtPogetUE1"  runat="server"  CssClass="label"></asp:Label>
       </div>
      <div style="text-align:center">
      <asp:Label ID="txtPogetUE1" Width="5%" runat="server" Font-Bold="true"  Enabled="false" AssociatedControlID="lblPogetUE" CssClass="label" ></asp:Label>
      </div>
      </div>

       <div class="collable" style="width:20%;border-style:groove;border-width:thin">
       <div style="height:30px;text-align:center">
        <asp:Label Id="lblProgetTUTO" Text="Progetti con max 3 mesi tutoraggio" AssociatedControlID="txtProgetTUTO1"  runat="server"  CssClass="label"></asp:Label>
       </div>
       <div style="text-align:center">
       <asp:Label ID="txtProgetTUTO1" Width="5%" runat="server" Font-Bold="true"  Enabled="false" AssociatedControlID="lblProgetTUTO" CssClass="label" ></asp:Label>
       </div>
       </div>

       <div class="collable" style="width:20%;border-style:groove;border-width:thin">
        <div style="height:30px;text-align:center">
        <asp:Label Id="lblProgetGMOUE" Text="Progetti con GMO e max 3 mesi UE" AssociatedControlID="txtProgetGMOUE1"  runat="server"  CssClass="label"></asp:Label>
       </div>
       <div style="text-align:center">
        <asp:Label ID="txtProgetGMOUE1" Width="5%" runat="server" Font-Bold="true"  Enabled="false" AssociatedControlID="lblProgetGMOUE"  CssClass="label" ></asp:Label>
       </div>
       </div>

       <div class="collable" style="width:19%;border-style:groove;border-width:thin" >
       <div style="height:30px;text-align:center">
        <asp:Label Id="lblProgetGMOTUTO" Text="Progetti con GMO e max 3 mesi tutoraggio" AssociatedControlID="txtProgetGMOTUTO1"  runat="server"  CssClass="label"></asp:Label>
       </div>
       <div style="text-align:center">
        <asp:Label ID="txtProgetGMOTUTO1" Width="5%" runat="server" Font-Bold="true"  Enabled="false" AssociatedControlID="lblProgetGMOTUTO"  CssClass="label" ></asp:Label>
       </div>
       </div>

   </div>
    </div>
   </fieldset>


   </div>


   </fieldset>

  </div>
  </fieldset>
  
  
  
  
  <fieldset id="flsEstero" class="ContornoPagina" >
    <legend id="Estero"><img id="ExpandEstero" src="images/minus_xsmall.png" style="cursor:pointer;" alt="Espandi/Riduci" />&nbsp;&nbsp; <img id="TornaSuEstero" onclick="TornaAdInizioPagina()" style="cursor:pointer;" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Settori</legend> 
<div class="wrapper"  id="EsteroContent" style="width:100%;border:0px">
<div class="row" style="height:auto">
<div class="collable" style="width:100%">
<asp:Label id="lblSettori" CssClass="msgInfo" runat="server" Visible="false" Text="L'elenco dei settori viene aggiornato automaticamente in funzione dei progetti associati."></asp:Label>
</div>
</div>
<div  style="width:100%;">
       <h3>
            <asp:label id="lblMacroAmbito" runat="server" AssociatedControlID="dtgMacroAmbito"></asp:label>
        </h3>
        <br />
        <asp:datagrid id="dtgMacroAmbito" runat="server" Width="100%" Font-Size="Small" ToolTip="Elenco Settori" CssClass="table" CellPadding="2"  AllowPaging="false" AllowSorting="True" AutoGenerateColumns="False"  Visible="False" UseAccessibleHeader="True" >	
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
                <asp:BoundColumn Visible="False" DataField="IdProgramma" HeaderText="IdProgramma"></asp:BoundColumn>
				<asp:BoundColumn Visible="true" DataField="Codifica" HeaderText="Codifica"></asp:BoundColumn>
				<asp:BoundColumn DataField="MacroAmbitoAttività" HeaderText="Settore">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				
			</Columns>
			
		</asp:datagrid>
   </div>

 </div>

</fieldset>
  <fieldset id="flsFormazioneGenerale" class="ContornoPagina" >
 <legend id="FormazioneGenerale"><img style="cursor:pointer;" src="images/minus_xsmall.png" alt="Espandi/Riduci" />&nbsp;&nbsp;<img style="cursor:pointer;" id="TornaSuFormazioneGenerale" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Obiettivi</legend>
 <div class="wrapper" id="FormazioneGeneraleContent" style="width:100%;border:0px">

   <div  style="width:100%;">
       <h3>
            <asp:label id="lblObiettivi" runat="server" AssociatedControlID="dtgObiettivi"></asp:label>
        </h3>
        <br />
        <asp:datagrid id="dtgObiettivi" runat="server" Width="100%" Font-Size="Small" ToolTip="Obiettivi" CssClass="table" CellPadding="2"  AllowPaging="false" AllowSorting="True" AutoGenerateColumns="False"   UseAccessibleHeader="True" >	
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
				<asp:BoundColumn Visible="False" DataField="idObiettivo" HeaderText="idObiettivo"></asp:BoundColumn>
                 <asp:TemplateColumn HeaderText="Sel.">
				        <HeaderStyle Width="5%" ></HeaderStyle>
				        <ItemTemplate>
					        <asp:CheckBox id="chkSeleziona" Text="Sel"  TextAlign="Left" AutoPostBack="true" toolTip="Seleziona Obiettivo" OnCheckedChanged="OnCheckChanged1" runat="server"></asp:CheckBox>
				        </ItemTemplate>
			        </asp:TemplateColumn>
                <asp:BoundColumn DataField="Codifica" HeaderText="Codifica">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Obiettivo" HeaderText="Obiettivo"></asp:BoundColumn>
                <asp:BoundColumn DataField="Descrizione" HeaderText="Descrizione"></asp:BoundColumn>
			</Columns>
			
		</asp:datagrid>
  
</div>
 

 
</div>
 </fieldset>
  <fieldset id="flsFormazioneSpecifica" class="ContornoPagina"  >
 <legend id="FormazioneSpecifica"><img style="cursor:pointer;" src="images/minus_xsmall.png" alt="Espandi/Riduci" />&nbsp;&nbsp;<img style="cursor:pointer;" id="TornaSuFormazioneSpecifica" onclick="TornaAdInizioPagina()" src="images/tornasu_xsmall.png"  title="Torna ad inizio pagina"  alt="Torna ad inizio pagina" />&nbsp;&nbsp;Ambito 
     di Azione</legend>
 <div class="wrapper" id="FormazioneSpecificaContent" style="width:100%;border:0px">
  <div  style="width:100%;">
       <h3>
            <asp:label id="lblambitodiAzione" runat="server" AssociatedControlID="dtgAmbitodiAzione"></asp:label>
        </h3>
        <br />
        <asp:datagrid id="dtgAmbitodiAzione" runat="server" Width="100%" Font-Size="Small" ToolTip="Ambito di Azione" CssClass="table" CellPadding="2"  AllowPaging="false" AllowSorting="True" AutoGenerateColumns="False"   UseAccessibleHeader="True" >	
			<SelectedItemStyle  BackColor="#6699FF" Font-Bold="True" ForeColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
                
				<asp:BoundColumn Visible="False" DataField="IdAmbitoAzione"  HeaderText="IdAmbitoAzione"></asp:BoundColumn>
				
                    <asp:TemplateColumn HeaderText="Sel." >
				        <HeaderStyle Width="5%" ></HeaderStyle>
				        <ItemTemplate>
					        <asp:RadioButton id="chkSeleziona" Text="Sel" TextAlign="Left" AutoPostBack="true" toolTip="Seleziona Obiettivo" ValidationGroup="adc" GroupName="adc" OnCheckedChanged="OnCheckChanged"  runat="server"></asp:RadioButton>
				        </ItemTemplate>
			        </asp:TemplateColumn>
                <asp:BoundColumn DataField="codifica" HeaderText="Codifica">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Descrizione" HeaderText="Descrizione"></asp:BoundColumn>
			</Columns>
			
		</asp:datagrid>
   </div>

 
</div>
 </fieldset>
  <fieldset class="ContornoPagina" id="DivStatoValutazione" runat="server"  style="height:auto" >
<legend>Territorio/Reti</legend>
 <div class="wrapper" style="width:100%;border:0px;height:auto">

<div class="row" >
     <div class="collable" style="width:10%">
    <asp:Label ID="lblterritorio"  Text="Territorio" CssClass="label" AssociatedControlID="ddlTerritorio" runat="server"></asp:Label>
     </div>
     <div class="colOggetti" style="width:55%">
     <asp:DropDownList ID="ddlTerritorio" runat="server" CssClass="ddlClass">
       
    </asp:DropDownList>
   </div>
   <div class="collable" style="width:15%;text-align:center">
   
    <asp:Label ID="lblReti"  Text="Reti" CssClass="label" AssociatedControlID="ddlReti" runat="server"></asp:Label>
   
   </div>
  <div class="colOggetti" style="width:10%">
  
     <asp:DropDownList ID="ddlReti" runat="server" CssClass="ddlClass">
        <asp:ListItem Value="-1" Selected="True" Text=""></asp:ListItem>
		<asp:ListItem Value="0" Text="No"></asp:ListItem>
		<asp:ListItem Value="1" Text="Si"></asp:ListItem>
    </asp:DropDownList>
  
  </div>
</div>
</div>


 </fieldset>
 
 
  
  <div class="RigaPulsanti" > 
	<asp:Button id="cmdSalva" CssClass="Pulsante" runat="server"  Text="Salva"></asp:Button>
    <asp:Button ID="cmdElimina" runat="server" CssClass="Pulsante" Text="Elimina" />
    <asp:Button id="imgChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" ></asp:Button>
</div>

</div>
<asp:HiddenField runat="server" ID="hdnIsPostbackEnte" />
<asp:HiddenField runat="server" ID="hdnIsPostbackFG" />
<asp:HiddenField runat="server" ID="hdnIsPostbackFS" />
<asp:HiddenField runat="server" ID="hdnIsPostbackProg" />
<asp:HiddenField runat="server" ID="hdnIsPostbackCO" />
<asp:HiddenField runat="server" ID="hdnIsPostbackCCA" />
<asp:HiddenField runat="server" ID="hdnIsPostbackEstero" />
<asp:HiddenField runat="server" ID="hdnIsPostbackProgetto" />
<asp:HiddenField runat="server" ID="IdAmbitoSelezionato" />
<asp:HiddenField runat="server" ID="IdProgramma" />




</fieldset>
</asp:Content>
