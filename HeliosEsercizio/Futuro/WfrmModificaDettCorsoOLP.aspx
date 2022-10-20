<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmModificaDettCorsoOLP.aspx.vb" Inherits="Futuro.WfrmModificaDettCorsoOLP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<%--<script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <style type="text/css"> .ui-datepicker {font-size: 11px;}</style>

    <script  type="text/javascript">

        var prefissoIdClient = "MainContent_";

        function CostruisciId(IdServer) 
        {
            var IdClient = prefissoIdClient + IdServer
            return IdClient
        }

        $(function () 
        {


            var DataDal = CostruisciId('TextDataSvolgimento');
            var readonly = '<%=TextDataSvolgimento.ReadOnly%>';
            if (readonly == "False") 
          {
                var sharpDataAvvio = "#" + DataDal
                $("" + sharpDataAvvio + "").datepicker();
            }
           
            

        });

	</script>--%>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
        <legend>DETTAGLIO OLP</legend>
          <div class="RigaVuota" >&nbsp;<asp:label id="lblmessaggio"  runat="server" CssClass="msgErrore" ></asp:label>
            </div>
        <div class="wrapper" style="width:100%">
            <div class="headers">
               <h2><asp:Label ID="lblTitolo" AssociatedControlID="lblmessaggio" runat="server"  Text="OLP"></asp:Label></h2>
            </div>
             <div class="row" >
             <div class="collable" style="width:15%"> 
             <asp:Label ID="lblCognome" AssociatedControlID="TextCognome" runat="server" Text="Cognome"></asp:Label>
             </div>  
             <div class="colOggetti"  style="width:35%">
            
                 <asp:TextBox ID="TextCognome" runat="server"></asp:TextBox>
            
             </div>
              <div class="collable" style="width:15%"> 
              <asp:Label ID="lblNome" AssociatedControlID="TextNome" runat="server" Text="Nome"></asp:Label>
             </div>  
             <div class="colOggetti"  style="width:35%">
                
                 <asp:TextBox ID="TextNome" runat="server"></asp:TextBox>
                
             </div>
             </div>
             <div class="row" >
             <div class="collable" style="width:15%"> 
             <asp:Label ID="lblEnteRiferimento" AssociatedControlID="TextEnteRiferimento" runat="server" Text="Ente Riferimento"></asp:Label>
             </div>  
             <div class="colOggetti"  style="width:35%">
                 <asp:TextBox ID="TextEnteRiferimento" runat="server"></asp:TextBox>
             </div>
              <div class="collable" style="width:15%"> 
              <asp:Label ID="lblLuogoSvol" AssociatedControlID="TextLuogoSvolgimento" runat="server" Text="Luogo Svolgimento"></asp:Label>
             </div>  
             <div class="colOggetti"  style="width:35%">
                 <asp:TextBox ID="TextLuogoSvolgimento" runat="server"></asp:TextBox>
             </div>
            </div>
             <div class="row" >
             <div class="collable" style="width:15%"> 
             <asp:Label ID="lblDataSvol" runat="server" AssociatedControlID="TextDataSvolgimento" Text="Data Svolgimento"></asp:Label>
             </div>  
             <div class="colOggetti"  style="width:35%">
            
                 <asp:TextBox ID="TextDataSvolgimento" runat="server"></asp:TextBox>
            
             </div>
              <div class="collable" style="width:15%"> 
              <asp:Label ID="Label2" AssociatedControlID="TextNumeroOre" runat="server" Text="Numero Ore"></asp:Label>
             </div>  
             <div class="colOggetti"  style="width:35%">
                 
                 <asp:TextBox ID="TextNumeroOre" runat="server"></asp:TextBox>
                 
             </div>
            </div>
            <div class="RigaPulsanti" style="text-align:right">
                <asp:Button ID="CmdSalva" runat="server" CssClass="Pulsante" Text="Salva" />&nbsp;
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
             </div>
             </div>
          </fieldset>   
</asp:Content>
