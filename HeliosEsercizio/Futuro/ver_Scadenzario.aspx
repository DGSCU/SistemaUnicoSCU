<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="ver_Scadenzario.aspx.vb" Inherits="Futuro.ver_Scadenzario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
    <script type="text/javascript">
    /* <![CDATA[ */
        var prefissoIdClient = "MainContent_";

        function CostruisciId(IdServer) {
            var IdClient = prefissoIdClient + IdServer;
            return IdClient;
        };

   
          /* ]]> */

    </script>
    <script type="text/javascript">
        function printDiv() {
            //Get the HTML of div
            var divElements = document.getElementById("Stampa").innerHTML;
            //Get the HTML of whole page
            var oldPage = divElements; //document.<span id="IL_AD6" class="IL_AD">body</span>.innerHTML;

            //Reset the page's HTML with div's HTML only
            document.body.innerHTML =
               "<title></title>" +
               divElements + "";

            //Print Page
            window.print();

            //Restore orignal HTML
            document.body.innerHTML = oldPage;
        }
    </script>

       <script  type="text/javascript">
      /* <![CDATA[ */
        var pathPlus = "images/plus_xsmall.png";
        var pathMinus = "images/minus_xsmall.png";
        var titlePathMinus = "Riduci";
        var titlePathPlus = "Espandi";
        var inserimento = '1';


      
        /* ]]> */  
    </script>
    
    
       <script type="text/javascript">
       /* <![CDATA[ */
        $(function () {
            var tornaSu = 0;
            if (inserimento != "") {
                $('#ScadenzaVerificheContent').css('display', 'none');
                $('#ScadenzaVerifiche').find("img").first().attr("src", pathPlus);
                $('#ScadenzaVerifiche').find("img").first().attr("title", titlePathPlus);
            }
            else {
                $('#ScadenzaVerificheContent').css('display', 'yes');
                $('#ScadenzaVerifiche').find("img").first().attr("src", pathMinus);
                $('#ScadenzaVerifiche').find("img").first().attr("title", titlePathMinus);
            }

            $('#TornaSuScadenzaVerifiche').click(function () {
                tornaSu = 1;
            });
            $('#ScadenzaVerifiche').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#ScadenzaVerificheContent').is(':visible');
                    $('#ScadenzaVerificheContent').slideToggle('fast');
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
            if (inserimento != "") {
                $('#ScadenzaRelazioniContent').css('display', 'none');
                $('#ScadenzaRelazioni').find("img").first().attr("src", pathPlus);
                $('#ScadenzaRelazioni').find("img").first().attr("title", titlePathPlus);
            }
            else {
                $('#ScadenzaRelazioniContent').css('display', 'yes');
                $('#ScadenzaRelazioni').find("img").first().attr("src", pathMinus);
                $('#ScadenzaRelazioni').find("img").first().attr("title", titlePathMinus);
            }

            $('#TornaSuScadenzaRelazioni').click(function () {
                tornaSu = 1;
            });
            $('#ScadenzaRelazioni').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#ScadenzaRelazioniContent').is(':visible');
                    $('#ScadenzaRelazioniContent').slideToggle('fast');
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
            if (inserimento != "") {
                $('#ScadenzaChiusuraVerificheContent').css('display', 'none');
                $('#ScadenzaChiusuraVerifiche').find("img").first().attr("src", pathPlus);
                $('#ScadenzaChiusuraVerifiche').find("img").first().attr("title", titlePathPlus);
            }
            else {
                $('#ScadenzaChiusuraVerificheContent').css('display', 'yes');
                $('#ScadenzaChiusuraVerifiche').find("img").first().attr("src", pathMinus);
                $('#ScadenzaChiusuraVerifiche').find("img").first().attr("title", titlePathMinus);
            }

            $('#TornaSuScadenzaChiusuraVerifiche').click(function () {
                tornaSu = 1;
            });
            $('#ScadenzaChiusuraVerifiche').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#ScadenzaChiusuraVerificheContent').is(':visible');
                    $('#ScadenzaChiusuraVerificheContent').slideToggle('fast');
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
            if (inserimento != "") {
                $('#ScadenzaContestazioniContent').css('display', 'none');
                $('#ScadenzaContestazioni').find("img").first().attr("src", pathPlus);
                $('#ScadenzaContestazioni').find("img").first().attr("title", titlePathPlus);
            }
            else {
                $('#ScadenzaContestazioniContent').css('display', 'yes');
                $('#ScadenzaContestazioni').find("img").first().attr("src", pathMinus);
                $('#ScadenzaContestazioni').find("img").first().attr("title", titlePathMinus);
            }

            $('#TornaSuScadenzaContestazioni').click(function () {
                tornaSu = 1;
            });
            $('#ScadenzaContestazioni').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#ScadenzaContestazioniContent').is(':visible');
                    $('#ScadenzaContestazioniContent').slideToggle('fast');
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
            if (inserimento != "") {
                $('#ScadenzaApplicazioneSanzioneContent').css('display', 'none');
                $('#ScadenzaApplicazioneSanzione').find("img").first().attr("src", pathPlus);
                $('#ScadenzaApplicazioneSanzione').find("img").first().attr("title", titlePathPlus);
            }
            else {
                $('#ScadenzaApplicazioneSanzioneContent').css('display', 'yes');
                $('#ScadenzaApplicazioneSanzione').find("img").first().attr("src", pathMinus);
                $('#ScadenzaApplicazioneSanzione').find("img").first().attr("title", titlePathMinus);
            }

            $('#TornaSuScadenzaApplicazioneSanzione').click(function () {
                tornaSu = 1;
            });
            $('#ScadenzaApplicazioneSanzione').click(function () {
                if (tornaSu == 0) {
                    var isVisible = $('#ScadenzaApplicazioneSanzioneContent').is(':visible');
                    $('#ScadenzaApplicazioneSanzioneContent').slideToggle('fast');
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
    <div class="firefox" id="Stampa" >
        <fieldset class="ContornoPaginaScroll">
            <legend>Elenco Scadenzari Verifiche</legend>
            <div class="RigaVuota">                &nbsp;</div>
             <div class="wrapper" style="width:100%;border:0px"> 

                <div class="headers">
                    <h2>
                        <asp:Label ID="lblTitolo" runat="server" Text="Scadenzari Verifiche"></asp:Label>
                    </h2>
                </div>
                <br />
         
                     <div class="RigaPulsanti">
                            <asp:Button ID="imgStampa" CssClass="Pulsante" runat="server" Text="Esporta CSV" >
                            </asp:Button>
                            <asp:Button ID="imgChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"></asp:Button>
                     </div>

                    <div class="row">
                        <asp:HyperLink ID="ApriCSVScadenzaVerifiche" runat="server" AccessKey="S" 
                           ForeColor="#003399" Text="DOWNLOAD CSV Scadenza Verifiche" ToolTip="Link per la stampa del risultato della ricerca"
                            Visible="False"></asp:HyperLink> <br />
                        <asp:HyperLink ID="ApriCSVScadenzaRelazioni" runat="server" AccessKey="S"  
                            ForeColor="#003399" Text="DOWNLOAD CSV Scadenza Relazioni" ToolTip="Link per la stampa del risultato della ricerca"
                            Visible="False"></asp:HyperLink> <br />
                        <asp:HyperLink ID="ApriCSVScadenzaChiusuraVerifiche" runat="server" AccessKey="S" 
                           ForeColor="#003399" Text="DOWNLOAD CSV Scadenza Chiusura Verifiche"
                            ToolTip="Link per la stampa del risultato della ricerca" Visible="False"></asp:HyperLink> <br />
                        <asp:HyperLink ID="ApriCSVScadenzaContestazioni" runat="server" AccessKey="S" 
                            ForeColor="#003399" Text="DOWNLOAD CSV Scadenza Contestazioni" ToolTip="Link per la stampa del risultato della ricerca"
                            Visible="False"></asp:HyperLink> <br />
                        <asp:HyperLink ID="ApriCSVScadenzaApplicazioneSanzione" runat="server" AccessKey="S"
                             ForeColor="#003399" Text="DOWNLOAD CSV Scadenza Applicazione Sanzione"
                            ToolTip="Link per la stampa del risultato della ricerca" Visible="False"></asp:HyperLink>
                    </div> <!--FILE -->
                
                 </div>
                <div class="row" >
                    <fieldset id="flsScadenzaVerifiche" class="ContornoPagina">
                        <legend id="ScadenzaVerifiche" >
                           <img src="images/plus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor: pointer;" />&nbsp;&nbsp;
                           <img id="TornaSuScadenzaVerifiche" style="cursor: pointer;" onclick="TornaAdInizioPagina()"
                            src="images/tornasu_xsmall.png" title="Torna ad inizio pagina" alt="Torna ad inizio pagina" />
                            <asp:Label ID="lblScadenzaVerifiche" AssociatedControlID="DG1" runat="server" Text="Scadenza Verifiche"></asp:Label>&nbsp;&nbsp;</legend>
                        <div class="wrapper" id="ScadenzaVerificheContent" style="width: 100%; border: 0px">
                            <asp:DataGrid ID="DG1" runat="server" Width="100%" AllowPaging="false" CssClass="table"
                                AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True" Visible="true">
                                <FooterStyle></FooterStyle>
                                <SelectedItemStyle BackColor="White" Font-Bold="true"></SelectedItemStyle>
                                <EditItemStyle></EditItemStyle>
                                <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                                <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                                <HeaderStyle></HeaderStyle>
                                <Columns>
                                    <asp:ButtonColumn Text="&lt;img src=images/statoente.jpg Width=20 Height=20 title='Seleziona Progetto' border=0&gt;"
                                        CommandName="seleziona" Visible="False"></asp:ButtonColumn>
                                    <asp:BoundColumn Visible="False" DataField="Idverifica" HeaderText="Idverifica">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="Idsegnalazione" HeaderText="Idsegnalazione">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Codicefascicolo" HeaderText="Numero Fascicolo"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="statoverifiche" HeaderText="Stato Verifica"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Programmazione" HeaderText="Programmazione"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="DataPrevistaVerifica" HeaderText="Data Inizio Prevista Verifica"
                                        DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="DataFinePrevistaVerifica" HeaderText="Data Fine Prevista Verifica"
                                        DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="DataFineVerifica" Visible="False" HeaderText="Data Chiusura Verifica"
                                        DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="nominativo" HeaderText="Verificatore"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="DenominazioneEnteProponente" HeaderText="Ente Proponente">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Titolo" HeaderText="Progetto">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Competenza" HeaderText="Competenza"></asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </div>
                    </fieldset>
                    <fieldset id="flsScadenzaRelazioni" class="ContornoPagina">
                        <legend id="ScadenzaRelazioni">
                         <img src="images/plus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor: pointer;" />&nbsp;&nbsp;<img
                            id="TornaSuScadenzaRelazioni" style="cursor: pointer;" onclick="TornaAdInizioPagina()"
                            src="images/tornasu_xsmall.png" title="Torna ad inizio pagina" alt="Torna ad inizio pagina" />
                            <asp:Label ID="LblScadenzaRelazioni" AssociatedControlID="DG2" runat="server" Text="Scadenza Relazioni"></asp:Label>&nbsp;&nbsp;</legend>
                        
                        <div class="wrapper" id="ScadenzaRelazioniContent" style="width: 100%; border: 0px">
                            <asp:DataGrid ID="DG2" runat="server" Width="100%" AllowPaging="false" CssClass="table"
                                AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True" Visible="true">
                                <FooterStyle></FooterStyle>
                                <SelectedItemStyle BackColor="White" Font-Bold="true"></SelectedItemStyle>
                                <EditItemStyle></EditItemStyle>
                                <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                                <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                                <HeaderStyle></HeaderStyle>
                                <Columns>
                                    <asp:ButtonColumn Text="&lt;img src=images/statoente.jpg Width=20 Height=20 title='Seleziona Progetto' border=0&gt;"
                                        CommandName="seleziona" Visible="False"></asp:ButtonColumn>
                                    <asp:BoundColumn Visible="False" DataField="Idverifica" HeaderText="Idverifica">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="Idsegnalazione" HeaderText="Idsegnalazione">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Codicefascicolo" HeaderText="Numero Fascicolo"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="statoverifiche" HeaderText="Stato Verifica"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Programmazione" HeaderText="Programmazione"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="DataPrevistaVerifica" HeaderText="Data Inizio Prevista Verifica"
                                        DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="DataFinePrevistaVerifica" HeaderText="Data Fine Prevista Verifica"
                                        DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="DataFineVerifica" HeaderText="Data Chiusura Verifica"
                                        DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="dataprotincarico" HeaderText="Data Protocollo Incarico"
                                        DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="nominativo" HeaderText="Verificatore"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="DenominazioneEnteProponente" HeaderText="Ente Proponente">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Titolo" HeaderText="Progetto">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="differenza" ItemStyle-HorizontalAlign="Center" HeaderText="Scadenza in giorni">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Competenza" HeaderText="Competenza"></asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </div>
                    </fieldset>
                    <fieldset id="flsScadenzaChiusuraVerifiche" class="ContornoPagina">
                        <legend id="ScadenzaChiusuraVerifiche">
                          <img src="images/plus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor: pointer;" />&nbsp;&nbsp;<img
                            id="TornaSuScadenzaChiusuraVerifiche" style="cursor: pointer;" onclick="TornaAdInizioPagina()"
                            src="images/tornasu_xsmall.png" title="Torna ad inizio pagina" alt="Torna ad inizio pagina" />
                            <asp:Label ID="LblScadenzaChiusuraVerifiche" AssociatedControlID="DG3" runat="server" Text="Scadenza Chiusura Verifiche"></asp:Label>&nbsp;&nbsp;</legend>
                        
                        <div class="wrapper" id="ScadenzaChiusuraVerificheContent" style="width: 100%; border: 0px">
                            <asp:DataGrid ID="DG3" runat="server" Width="100%" AllowPaging="false" CssClass="table"
                                AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True" Visible="true">
                                <FooterStyle></FooterStyle>
                                <SelectedItemStyle BackColor="White" Font-Bold="true"></SelectedItemStyle>
                                <EditItemStyle></EditItemStyle>
                                <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                                <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                                <HeaderStyle></HeaderStyle>
                                <Columns>
                                    <asp:ButtonColumn Text="&lt;img src=images/statoente.jpg Width=20 Height=20 title='Seleziona Progetto' border=0&gt;"
                                        CommandName="seleziona" Visible="False"></asp:ButtonColumn>
                                    <asp:BoundColumn Visible="False" DataField="Idverifica" HeaderText="Idverifica">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="Idsegnalazione" HeaderText="Idsegnalazione">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Codicefascicolo" HeaderText="Numero Fascicolo"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="statoverifiche" HeaderText="Stato Verifica"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Programmazione" HeaderText="Programmazione"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="DataPrevistaVerifica" HeaderText="Data Inizio Prevista Verifica"
                                        DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="DataFinePrevistaVerifica" HeaderText="Data Fine Prevista Verifica"
                                        DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="DataFineVerifica" HeaderText="Data Chiusura Verifica"
                                        DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="dataProtRelazione" HeaderText="Data Protocollo Relazione"
                                        DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="nominativo" HeaderText="Verificatore"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="DenominazioneEnteProponente" HeaderText="Ente Proponente">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Titolo" HeaderText="Progetto">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="differenza" ItemStyle-HorizontalAlign="Center" HeaderText="Scadenza in giorni">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Competenza" HeaderText="Competenza"></asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </div>
                    </fieldset>
                    <fieldset id="flsScadenzaContestazioni" class="ContornoPagina">
                        <legend id="ScadenzaContestazioni">
                           <img src="images/plus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor: pointer;" />&nbsp;&nbsp;<img
                            id="TornaSuScadenzaContestazioni" style="cursor: pointer;" onclick="TornaAdInizioPagina()"
                            src="images/tornasu_xsmall.png" title="Torna ad inizio pagina" alt="Torna ad inizio pagina" />
                            <asp:Label ID="LblScadenzaContestazioni" AssociatedControlID="DG4" runat="server" Text="Scadenza Contestazioni"></asp:Label>&nbsp;&nbsp;</legend>
                        
                        
                       
                        <div class="wrapper" id="ScadenzaContestazioniContent" style="width: 100%; border: 0px">
                            <asp:DataGrid ID="DG4" runat="server" Width="100%" AllowPaging="false" CssClass="table"
                                AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True" Visible="true">
                                <FooterStyle></FooterStyle>
                                <SelectedItemStyle BackColor="White" Font-Bold="true"></SelectedItemStyle>
                                <EditItemStyle></EditItemStyle>
                                <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                                <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                                <HeaderStyle></HeaderStyle>
                                <Columns>
                                    <asp:ButtonColumn Text="&lt;img src=images/statoente.jpg Width=20 Height=20 title='Seleziona Progetto' border=0&gt;"
                                        CommandName="seleziona" Visible="False"></asp:ButtonColumn>
                                    <asp:BoundColumn Visible="False" DataField="Idverifica" HeaderText="Idverifica">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="Idsegnalazione" HeaderText="Idsegnalazione">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Codicefascicolo" HeaderText="Numero Fascicolo"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="statoverifiche" HeaderText="Stato Verifica"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Programmazione" HeaderText="Programmazione"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="DataPrevistaVerifica" HeaderText="Data Inizio Prevista Verifica"
                                        DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="DataFinePrevistaVerifica" HeaderText="Data Fine Prevista Verifica"
                                        DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="DataFineVerifica" HeaderText="Data Chiusura Verifica"
                                        DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="DATAPROTINVIOLETTERACONTESTAZIONE" HeaderText="Data Protocollo Invio Lettera Contestazione"
                                        DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="nominativo" HeaderText="Verificatore"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="DenominazioneEnteProponente" HeaderText="Ente Proponente">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Titolo" HeaderText="Progetto">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="differenza" ItemStyle-HorizontalAlign="Center" HeaderText="Scadenza in giorni">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Competenza" HeaderText="Competenza"></asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </div>
                    </fieldset>
                    <fieldset id="flsScadenzaApplicazioneSanzione" class="ContornoPagina">
                        <legend id="ScadenzaApplicazioneSanzione">
                          <img src="images/plus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor: pointer;" />&nbsp;&nbsp;<img
                            id="TornaSuScadenzaApplicazioneSanzione" style="cursor: pointer;" onclick="TornaAdInizioPagina()"
                            src="images/tornasu_xsmall.png" title="Torna ad inizio pagina" alt="Torna ad inizio pagina" />
                            <asp:Label ID="lblScadenzaApplicazioneSanzione" AssociatedControlID="DG5" runat="server" Text="Scadenza Applicazione Sanzione"></asp:Label>&nbsp;&nbsp;</legend>
                        
                        
                        
                        <div class="wrapper" id="ScadenzaApplicazioneSanzioneContent" style="width: 100%;
                            border: 0px">
                            <asp:DataGrid ID="DG5" runat="server" Width="100%" AllowPaging="false" CssClass="table"
                                AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True" Visible="true">
                                <FooterStyle></FooterStyle>
                                <SelectedItemStyle BackColor="White" Font-Bold="true"></SelectedItemStyle>
                                <EditItemStyle></EditItemStyle>
                                <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                                <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                                <HeaderStyle></HeaderStyle>
                                <Columns>
                                    <asp:ButtonColumn Text="&lt;img src=images/statoente.jpg Width=20 Height=20 title='Seleziona Progetto' border=0&gt;"
                                        CommandName="seleziona" Visible="False"></asp:ButtonColumn>
                                    <asp:BoundColumn Visible="False" DataField="Idverifica" HeaderText="Idverifica">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="Idsegnalazione" HeaderText="Idsegnalazione">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Codicefascicolo" HeaderText="Numero Fascicolo"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="statoverifiche" HeaderText="Stato Verifica"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Programmazione" HeaderText="Programmazione"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="DataPrevistaVerifica" HeaderText="Data Inizio Prevista Verifica"
                                        DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="DataFinePrevistaVerifica" HeaderText="Data Fine Prevista Verifica"
                                        DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="DataFineVerifica" HeaderText="Data Chiusura Verifica"
                                        DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="DataProtTrasmissioneSanzione" HeaderText="Data Protocollo Trasmissione Sanzione"
                                        DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="nominativo" HeaderText="Verificatore"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="DenominazioneEnteProponente" HeaderText="Ente Proponente">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Titolo" HeaderText="Progetto">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="RegioneOrUffico" HeaderText="Regione/Servizio Esec. Sanzione">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="differenza" ItemStyle-HorizontalAlign="Center" HeaderText="Scadenza in giorni">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Competenza" HeaderText="Competenza"></asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </div>
                    </fieldset>
               </div>
            
        </fieldset>
    </div>
</asp:Content>
