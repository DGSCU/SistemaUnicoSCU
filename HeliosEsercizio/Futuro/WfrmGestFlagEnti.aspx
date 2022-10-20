<%@ Page Title="Vincoli" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmGestFlagEnti.aspx.vb" Inherits="Futuro.WfrmGestFlagEnti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
 <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
        <style type="text/css">
            .ui-datepicker {
                font-size: 11px;
            }
        </style>
          <script  type="text/javascript">

              $(function () {
                  var idDataProt = CostruisciId('txtData');
                  var sharpDataProt = "#" + idDataProt
                      $("" + sharpDataProt + "").datepicker();
              });

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
<legend >Vincoli</legend>
<div class="wrapper" style="width:100%">
        <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Vincoli "></asp:Label>
               </h2>
            </div>
         <div class="rowGroup" style="height:auto">
		    <asp:label id="msgErrore"  runat="server" CssClass="msgErrore"></asp:label>
            <asp:label id="msgConferma"  runat="server" CssClass="msgConferma" ></asp:label>
		</div>
         <br />
        <div class="row" >
         <div class="collable" style="width:50%" >
           <asp:Label ID="lblvincolo" CssClass="labelDati"  runat="server" ></asp:Label>
       </div>
       <div class="colOggetti" style="width:10%">
           <asp:RadioButton ID="rdbsi" GroupName="IF" runat="server" Text="SI"/>
       </div>
       <div class="collable" style="width:10%">
            <asp:RadioButton ID="rdbno" GroupName="IF" runat="server" Text="NO"/>
       </div>
        </div>
        <div class="row" >
    
       <div class="collable" style="width:15%" >
           <asp:Label ID="lblTxtProt" CssClass="label" AssociatedControlID="TxtProt" runat="server" Text="Numero Protocollo"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:TextBox id="TxtProt" CssClass="ddlClass" runat="server"></asp:TextBox>
       </div>
       <div class="collable" style="width:15%">
           <asp:Label ID="lblData" CssClass="label" AssociatedControlID="txtData"  runat="server" Text="Data Protocollazione (gg/mm/aaaa)"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:TextBox ID="txtData" CssClass="textbox" Width="25%"  MaxLength="10" runat="server"></asp:TextBox>
       </div>
        </div>
    <div class="row" >
       <div class="collable" style="width:15%">      
           <asp:Label ID="IdLblNote" CssClass="label" AssociatedControlID="TxtNotaStorico" runat="server" Text="Note"></asp:Label>
       </div>
       <div class="colOggetti" style="width:85%">       
           <asp:TextBox ID="TxtNotaStorico" CssClass="textbox" runat="server" MaxLength="500" Rows="4" TextMode="MultiLine"></asp:TextBox>         
       </div>

    </div>
    <br />
    <div class="RigaPulsanti">
        <asp:Button ID="imgConferma" runat="server" CssClass="Pulsante" Text="Conferma"  />
          <asp:Button ID="imgCancella" runat="server" CssClass="Pulsante"   Text="Cancella"  />
          <asp:Button 
            ID="imgChiudi" CssClass="Pulsante" runat="server"
              Text="Chiudi"  />&nbsp;

   </div>
</div>
</fieldset>
</asp:Content>
