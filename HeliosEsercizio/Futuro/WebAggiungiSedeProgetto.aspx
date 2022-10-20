<%@ Page Title="Aggiungi Sede Progetto" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WebAggiungiSedeProgetto.aspx.vb" Inherits="Futuro.WebAggiungiSedeProgetto" %>
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
        <script type="text/javascript">
            $(function ()
            {
                var IdData = CostruisciId('txtDataNascita');
                var sharpIdData = "#" + IdData
                $("" + sharpIdData + "").datepicker();
            });
        </script>
    <script type="text/javascript">
       
        function CheckNumeroInteroValido(valore, descrizione) {
            var clientIdErrore = CostruisciId("lblMessaggi");
            var numeroValido = ValidaNumeroIntero(CostruisciId(valore), descrizione, clientIdErrore);
        };

        function AggiornaTotaleVolontariDaImpiegare() {
            var vittoAlloggio = parseInt(document.getElementById(CostruisciId("txtVittoAlloggio")).value);
            var noVittoAlloggio = parseInt(document.getElementById(CostruisciId("txtNoVittoNoAlloggio")).value);
            var soloVitto = parseInt(document.getElementById(CostruisciId("txtvitto")).value);

            if (isNaN(vittoAlloggio) == true) {
                vittoAlloggio = 0;
            }

            if (isNaN(noVittoAlloggio) == true) {
                noVittoAlloggio = 0;
            }

            if (isNaN(soloVitto) == true) {
                soloVitto = 0;
            }

            document.getElementById(CostruisciId("txtvolontari")).value = vittoAlloggio + noVittoAlloggio + soloVitto;
        };

        
        
		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <fieldset class="ContornoPagina">
<legend>Aggiungi Sede Progetto</legend>
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:Label ID="lblTitolo" runat="server" CssClass="label"  Text="Aggiungi Sede Per Progetto ' "></asp:Label>
       <asp:Label ID="lblprogetto"  CssClass="label" runat="server"  Text=""></asp:Label>
       <asp:Label ID="Label1" runat="server" CssClass="label"  Text="'"></asp:Label>

      </h2>
      </div>
    <div class="row" style="height:auto" >
     <div class="RigaVuota" >&nbsp;
     </div>
        <asp:Label id="lblMessaggi" CssClass="msgErrore" runat="server" ></asp:Label>
         <asp:Label id="lblConferma" CssClass="msgConferma" runat="server" ></asp:Label>
        <asp:HiddenField id="txtvedi" runat="server"/>
        <asp:HiddenField id="lblidsedeattuazione" runat="server"/>
        <asp:HiddenField id="lblidSede" runat="server"/>
        <asp:HiddenField id="lblidAttivita" runat="server"/>
        <asp:HiddenField id="lblidattEs" runat="server"/>
        <asp:HiddenField id="isUpdate" Value="0" runat="server"/>
    </div>
    <div class="RigaVuota" >&nbsp;
    </div>

    <div class="row" >
       <div class="collable" style="width:15%">      
           <asp:Label ID="labelSedeFisica" AssociatedControlID="lblSedeFisica"  runat="server" Text="Nome Sede"></asp:Label>
       </div>
       <div class="colOggetti" style="width:85%">       
           <asp:Label ID="lblSedeFisica" CssClass="labelDati" runat="server"></asp:Label>         
       </div>
       </div>
       <div class="row" >
       <div class="collable" style="width:15%">
            <asp:Label ID="labelIndirizzo" AssociatedControlID="lblIndirizzo" runat="server" Text="Indirizzo"></asp:Label>
       </div>
       <div class="colOggetti" style="width:85%">
              <asp:Label ID="lblIndirizzo" CssClass="labelDati" runat="server"></asp:Label>  
       </div>
    </div>
    <div class="row" style="display: none" >
        <div class="collable" style="width:15%">
            <asp:Label ID="labelSedeAttuazione" AssociatedControlID="lblSedeAttuazione" runat="server" Text="Sede Attuazione"></asp:Label>
       </div>
        <div class="colOggetti" style="width:85%">    
            <asp:Label ID="lblSedeAttuazione" runat="server" CssClass="labelDati" ></asp:Label>
       </div>
       </div>

    <div class="row" >
        <div class="collable" style="width:15%">
            <asp:Label ID="labelTelefono" AssociatedControlID="lbltelefono" runat="server" Text="Telefono"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">
            <asp:Label ID="lbltelefono" runat="server" CssClass="labelDati" ></asp:Label>
       </div>

        <div class="collable" style="width:15%">
            <asp:Label ID="labelProgettiAttivi" AssociatedControlID="lblProgettiAttivi" runat="server" Text="N&#186; Progetti Attivi"></asp:Label>
       </div>
        <div class="colOggetti" style="width:5%">    
             <asp:Label ID="lblProgettiAttivi" runat="server" CssClass="labelDati" ></asp:Label> 
             
       </div>
        <div class="collable" style="width:30%">   
        <asp:ImageButton id="imgdettProg"  runat="server" ImageUrl="images/lenteIngrandimento_small.png" AlternateText="Visualizza Dettagli Progetti Attivi" ToolTip="Dettagli Progetti Attivi" style="cursor:pointer"/>
        </div>
    </div>


    <div id="divDatiPersonali" class="rowGroup" visible="false" runat="server">
    <div class="row" >
       <div class="collable" style="width:15%">      
           <asp:Label ID="LblCognomeRif" AssociatedControlID="txtCognomeRif" runat="server" Text="Cognome"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">       
           <asp:TextBox ID="txtCognomeRif" CssClass="textbox" autofocus="true" runat="server"  ></asp:TextBox>         
       </div>
       <div class="collable" style="width:15%">
            <asp:Label ID="LblNomeRif" AssociatedControlID="txtNomeRif" runat="server" Text="Nome"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:TextBox ID="txtNomeRif" CssClass="textbox" runat="server"></asp:TextBox>  
       </div>
    </div>
      <div class="row" >
        <div class="collable" style="width:15%">
           <asp:Label ID="lblCitta" AssociatedControlID="txtCitta" runat="server" Text="Citt&#224; estera Sede"></asp:Label>
      </div> 
       <div class="colOggetti" style="width:35%">
        
           <asp:TextBox ID="txtCitta" CssClass="textbox" runat="server"></asp:TextBox>         
        
       </div> 
      <div class="collable" style="width:15%">   
       <asp:Label ID="lblDataNascita" AssociatedControlID="txtDataNascita" Visible="false" runat="server" Text="Data di Nascita"></asp:Label>
      </div>
      <div class="colOggetti" style="width:35%">

                <asp:TextBox ID="txtDataNascita" CssClass="textbox" runat="server" Visible="false" MaxLength="10"></asp:TextBox>

       </div>
       
      </div>
      
      <div class="row" >
       <div class="collable" style="width:15%">      
            <asp:Label ID="lblNote" AssociatedControlID="txtNote" runat="server" 
                Text="Note" Visible="False"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">       
              <asp:TextBox ID="txtNote" CssClass="textbox" TextMode="MultiLine" Visible="False"   runat="server"></asp:TextBox>  
       </div>
       <div class="collable" style="width:15%">
       </div>
       <div class="colOggetti" style="width:35%">
       </div>
    </div>
    </div>

    <div id="divSedeSecondaria" class="rowGroup" visible="false" runat="server">
        <div class="row" >
           <div class="collable" style="width:15%">      
               <asp:Label ID="lblCodSedeSecondaria" AssociatedControlID="txtCodSedeSecondaria"  runat="server" Text="Codice Sede Secondaria"></asp:Label>
           </div>
           <div class="colOggetti" style="width:15%">       
                   <asp:TextBox ID="txtCodSedeSecondaria" CssClass="textbox" runat="server"></asp:TextBox>        
           </div>
           <div class="colOggetti" style="width:70%">
                  <asp:Label ID="lblInfoSedeSecondaria" CssClass="labelDati" runat="server"></asp:Label>  
           </div>
        </div>
    </div>
    <fieldset >
    <legend>Numero Posti</legend>
        <div class="row" >
            <div class="collable" style="width:10%">      
                <asp:Label ID="lblNoVittoVoAlloggio" AssociatedControlID="txtNoVittoNoAlloggio" runat="server" Text="Senza Vitto e Alloggio"></asp:Label>
            </div>
            <div class="colOggetti" style="width:5%">       
                <asp:TextBox ID="txtNoVittoNoAlloggio" CssClass="textbox" runat="server" onchange="CheckNumeroInteroValido('txtNoVittoNoAlloggio','Posti Senza Vitto e Alloggio')" onkeyup="AggiornaTotaleVolontariDaImpiegare()"></asp:TextBox>         
            </div>
            <div class="colOggetti" style="width:5%"> &nbsp;
            </div>
            <div class="collable" style="width:10%">      
                <asp:Label ID="lblVittoAlloggio" AssociatedControlID="txtVittoAlloggio" runat="server" Text="Con Vitto e Alloggio"></asp:Label>
            </div>
            <div class="colOggetti" style="width:5%">       
                <asp:TextBox ID="txtVittoAlloggio" CssClass="textbox" runat="server" onchange="CheckNumeroInteroValido('txtVittoAlloggio','Posti Vitto e Alloggio')" onkeyup="AggiornaTotaleVolontariDaImpiegare()"></asp:TextBox>         
            </div>
            <div class="colOggetti" style="width:5%"> &nbsp;
            </div>
            <div class="collable" style="width:10%">      
                <asp:Label ID="lblVitto" AssociatedControlID="txtvitto" runat="server" Text="Solo Vitto"></asp:Label>
            </div>
            <div class="colOggetti" style="width:5%">       
                <asp:TextBox ID="txtvitto" CssClass="textbox" runat="server"  onchange="CheckNumeroInteroValido('txtvitto','Posti Solo Vitto')" onkeyup="AggiornaTotaleVolontariDaImpiegare()"></asp:TextBox>         
            </div>
                <div class="colOggetti" style="width:10%"> &nbsp;
            </div>
            <div class="collable" style="width:10%">
                <asp:Label ID="lblTotaleVolontari" AssociatedControlID="txtvolontari" runat="server" Text="N&#186; Volontari"></asp:Label>
            </div>
            <div class="colOggetti" style="width:10%">    
                <asp:TextBox ID="txtvolontari" runat="server" CssClass="labelDati"  style="border:0px" ReadOnly="true" ></asp:TextBox> 
            </div>
        </div>
        <div class="row" id="divFAMI" runat="server" visible="false">
            <div class="collable" style="width:10%">      
                <asp:Label ID="lblPostiFami" AssociatedControlID="TxtNumeroPostiFami" runat="server" Text="di cui Fami"></asp:Label>
            </div>
            <div class="colOggetti" style="width:5%">       
                <asp:TextBox ID="TxtNumeroPostiFami" CssClass="textbox" runat="server" onchange="CheckNumeroInteroValido('TxtNumeroPostiFami','Posti FAMI')" onkeyup="AggiornaTotaleVolontariDaImpiegare()"></asp:TextBox>         
            </div>
        </div>
        <div class="row" id="divGMO" runat="server" visible="false">
            <div class="collable" style="width:10%">      
                <asp:Label ID="lblPostiGMO" AssociatedControlID="TxtNumeroPostiGMO" runat="server" Text="di cui GMO"></asp:Label>
            </div>
            <div class="colOggetti" style="width:5%">       
                <asp:TextBox ID="TxtNumeroPostiGMO" CssClass="textbox" runat="server" onchange="CheckNumeroInteroValido('TxtNumeroPostiGMO','Posti GMO')" onkeyup="AggiornaTotaleVolontariDaImpiegare()"></asp:TextBox>         
            </div>
        </div>
    </fieldset>
    <div class="RigaPulsanti">
        <asp:Button ID="cmdConferma" runat="server" CssClass="Pulsante" Text="Salva" />&nbsp;
          <asp:Button  ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"   />&nbsp;     
   </div>
    </div>
    </fieldset>
</asp:Content>
