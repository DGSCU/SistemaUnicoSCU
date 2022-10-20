<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="ver_visibilita.aspx.vb" Inherits="Futuro.ver_visibilita" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
    <script type="text/javascript">
  /* <![CDATA[ */
  
       function SoloNumeri() 
       {
     
           var evtobj = window.event;
           var keyascii = evtobj.keyCode;
           if (keyascii > 57 || keyascii <= 47) 
           {
               window.event.keyCode = 0;
            return false;
           }
           else 
           {
               window.event.keyCode = keyascii;
                return true;
           }
       }
   
	/* ]]> */
    </script>
    <script type="text/javascript">
        function onlyNumbers(evt) {
            var theEvent = evt || window.event;
            var key = theEvent.keyCode || theEvent.which;
            key = String.fromCharCode(key);
            var regex = /[0-9]/;
            if (!regex.test(key)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault) theEvent.preventDefault();
            }
        }  
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset id="ContornoPagina" class="ContornoPagina" runat="server">
        <legend>Gestione Visibilità</legend>
        <div class="wrapper" style="width: 100%; border: 0px">
            <div class="headers">
                <h2>
                    <asp:Label ID="Label3" runat="server" Text="Gestione Visibilità"></asp:Label>
                </h2>
            </div>
            <div class="rowGroup" style="height: auto">
                <asp:Label ID="lblErrore" runat="server" CssClass="msgErrore"></asp:Label>
                <div class="RigaVuota">
                    &nbsp;</div>
            </div>
            <div class="RigaVuota">
                &nbsp;
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label2" CssClass="label" runat="server" Text="Periodo Visibilità"
                        AssociatedControlID="TxtPeriodo"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 15%">
                    <asp:TextBox ID="TxtPeriodo" CssClass="textbox" runat="server" onkeypress="onlyNumbers(event)" />
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="Label1" CssClass="label" runat="server" Text="Note" AssociatedControlID="TxtNote"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 85%">
                    <asp:TextBox ID="TxtNote" CssClass="textbox" runat="server" TextMode="MultiLine"
                        Style="width: 85%" />
                </div>
            </div>
            <div class="RigaVuota">
                &nbsp;
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="imgConferma" CssClass="Pulsante" runat="server" Text="Conferma">
                </asp:Button>
                <asp:Button ID="imgChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"></asp:Button>
            </div>
        </div>
    </fieldset>
</asp:Content>
