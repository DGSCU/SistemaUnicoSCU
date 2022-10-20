<%@ Page Title="Questionario Progetto" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmQuestionarioProgetto.aspx.vb" Inherits="Futuro.WfrmQuestionarioProgetto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
        <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css"/>
        <script src="Scripts/JHelper.js" type="text/javascript"></script>

    <script  type="text/javascript">
     /* <![CDATA[ */
        function CheckNumeroValido(valore, descrizione) {
            var clientIdErrore = CostruisciId("lblerrore")
            var numeroValido = ValidaNumero(CostruisciId(valore), descrizione, clientIdErrore);
            if (numeroValido == false) {
                TornaAdInizioPagina();
            }
        };
        /* ]]> */
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset id="GestioneProgetti"  class="ContornoPagina" runat="server">
<legend >Caratteristiche Progetto</legend>
<div class="wrapper"  style="width:100%;">
  <div class="headers" >
    <h2>
    <asp:Label ID="Label5" runat="server"  Text="Gestione Questionario Progetto"></asp:Label>
    </h2>
</div>
<div class="RigaVuota">&nbsp;</div>
<div class="rowGroup" style="height:auto">

<asp:Label ID="lblmessaggio"  CssClass="msgConferma" runat="server"></asp:Label>
<asp:Label ID="lblerrore" CssClass="msgErrore" runat="server"></asp:Label>
<div class="RigaVuota">&nbsp;</div>
</div>
 <div id="divBando" class="rowGroup" style="height:auto"  runat="server" visible="false"> 

    <div class="row" >  
          <div class="collable" style="width:15%">
                <asp:Label ID="lblTitolo" class="labelDati" AssociatedControlID="lblProgetto" visible="false" runat="server" Text="Bando"></asp:Label>
            </div>
            <div class="colOggetti" style="width:85%">
                  <asp:Label ID="lblProgetto" class="label" visible="false" runat="server" ></asp:Label>
            </div>    
    </div>
 </div>
   <div class="row" >  
            <div class="collable" style="width:15%">
            <asp:Label ID="lblTitolareProgetto" class="labelDati" AssociatedControlID="lblEnte"  runat="server" Text="Ente"></asp:Label>
        </div>
        <div class="colOggetti" style="width:85%">
                <asp:Label ID="lblEnte" class="label"  runat="server" ></asp:Label>
        </div>    
    </div>  
   
    <div class="row">
        <div class="collable" style="width:70%" >
            <asp:Label ID="lblFormatori" AssociatedControlID="txtNumFormatori" Text="Quanti formatori sono stati impiegati nella formazione generale dei volontari?" CssClass="labelDati" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:10%" >
         <asp:TextBox ID="txtNumFormatori" runat="server" CssClass="textbox"  MaxLength="3" Text="0"></asp:TextBox>
         </div>
        </div>
    <div class="row">
        <div class="collable" style="width:70%" >
            <asp:Label ID="lblNumVolontari"  AssociatedControlID="txtNumVolontari" Text="Quanti volontari hanno partecipato al/ai corso/i di formazione generale? Tot. N&#176;" CssClass="labelDati" runat="server"></asp:Label>
        </div>
        <div class="colOggetti" style="width:10%" >
            <asp:TextBox ID="txtNumVolontari" runat="server" CssClass="textbox"  MaxLength="4" Text="0"></asp:TextBox>
        </div>
   </div>

  
        <asp:Panel ID="panelCaratteristicheProgetto" runat="server"></asp:Panel>
             <div class="row">
        <div class="collable" style="width:100%" >
            <asp:Label ID="lblNote" AssociatedControlID="txtnote" Text="Se ha risposto Sì, specifichi le proposte (max 1000 caratteri)" 
                Font-Bold="true" CssClass="labelDati" runat="server"></asp:Label>
        </div>
          <asp:Panel ID="panelCaratteristicheOrganizzative" runat="server"></asp:Panel>
           <asp:Panel ID="panelConoscenzeAcquisite" runat="server"></asp:Panel>
    </div>
    <div class="row">
         <div class="colOggetti" style="width:100%;border:0px;" >    
            <asp:TextBox ID="txtnote" runat="server" CssClass="textbox" TextMode="MultiLine" MaxLength="1000" Rows="4" Width="98%"  ></asp:TextBox>
         </div>
    </div>
    <div class="RigaVuota">&nbsp;</div>
   

 <div class="rowGroup" > 
<div class="RigaPulsanti" > 
	<asp:Button id="cmdConferma" CssClass="Pulsante" runat="server" Text="Salva"></asp:Button>
    <asp:Button id="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" ></asp:Button>
    <asp:Button id="cmdStampaVQ" CssClass="Pulsante" runat="server" Text="Stampa" ></asp:Button>
</div>
</div>
</div>
 <asp:HiddenField id="txtx" Value="0" runat="server"/>
<asp:HiddenField id="txtnome" runat="server"/>
<asp:HiddenField id="txtBloccaMaschera" runat="server"/>
</fieldset>

</asp:Content>
