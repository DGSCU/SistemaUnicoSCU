<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmVisualizzaStoricoNotifiche.aspx.vb" Inherits="Futuro.WfrmVisualizzaStoricoNotifiche" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
<link href="Styles/Site.css" rel="stylesheet" type="text/css" />
   <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .ui-datepicker {
                font-size: 11px;
            }
        </style>
          
       <script  type="text/javascript">

           var formId;
           var prefissoIdClient = "";

           function ImpostaFormId() {
               formId = document.forms[0].id;
               return formId;
           }
           function CostruisciId(IdServer) {
               var IdClient = prefissoIdClient + IdServer
               return IdClient
           }


           $(function () {
               var IdData = CostruisciId('TxtDataDal');
               var sharpIdData = "#" + IdData
               $("" + sharpIdData + "").datepicker();
           });


           $(function () {
               var IdData = CostruisciId('txtDataAl');
               var sharpIdData = "#" + IdData
               $("" + sharpIdData + "").datepicker();
           });
      	</script>

    <title></title>
</head>
<body>
    <form id="form1" runat="server">
     <!--<fieldset class="ContornoPagina">
        <legend>VISUALIZZAZIONE STORICO NOTIFICHE</legend>-->
        <asp:label id="lblmess"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
        <br />
       
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="RICERCA STORICO NOTIFICHE"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
                <div class="collable" style="width:15%">
                    
                <asp:label id="LblCodiceEnte" CssClass="label" AssociatedControlID="lblCodEnte" runat="server" Text="Codice Ente" />
                    
                </div>
                <div class="colOggetti" style="width:35%">
                    
                <asp:label id="lblCodEnte"  CssClass="labelDati"
                        AssociatedControlID="LblCodiceEnte" runat="server" Text="" />
                    
                </div>
                <div class="collable" style="width:15%">
                    
                <asp:label id="LblDenominazione" CssClass="label" 
                        AssociatedControlID="LblDenomin" runat="server" Text="Denominazione" />
                    
                </div>
                <div class="colOggetti" style="width:35%">
                    
                <asp:label id="LblDenomin" CssClass="labelDati"
                        AssociatedControlID="LblDenominazione" runat="server" Text="" />
                    
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">
                    
               
                    
                <asp:label id="LblTipoNotifica" CssClass="label" AssociatedControlID="ddlTipoNotifica" 
                        runat="server" Text="Tipo Notifica" />
                    
               
                    
                </div>
                <div class="colOggetti" style="width:35%">
                    
                    <asp:DropDownList ID="ddlTipoNotifica" runat="server">

                    <asp:ListItem Value="0">Selezionare</asp:ListItem>
					<asp:ListItem Value="1">Documenti</asp:ListItem>
                    <asp:ListItem Value="2">Presenze</asp:ListItem>
                    <asp:ListItem Value="3">CheckList Collettive</asp:ListItem>
                    <asp:ListItem Value="4">CheckList Individuali</asp:ListItem>
                     <asp:ListItem Value="5">CheckList Formazione Generale</asp:ListItem>
                    </asp:DropDownList>
                    
                </div>
                <div class="collable" style="width:15%">
                    
                <asp:label id="LblTipoDocumento2" CssClass="label" 
                        AssociatedControlID="ddlUserNameInvio" runat="server" Text="User Name Invio" />
                    
                </div>
                <div class="colOggetti" style="width:35%">
                    
                    <asp:DropDownList ID="ddlUserNameInvio" runat="server">
                    </asp:DropDownList>
                    
                </div>
            </div>
             <div class="row" >
                <div class="collable" style="width:15%">
                    
                <asp:label id="LblCodiceVolontario" CssClass="label" 
                        AssociatedControlID="txtCodVol" runat="server" Text="Codice Volontario" />
                    
                </div>
                <div class="colOggetti" style="width:35%">
                    
                    <asp:TextBox ID="txtCodVol" runat="server"></asp:TextBox>
                    
                </div>
                <div class="collable" style="width:15%">
                    
                <asp:label id="LblcodicecheekList" CssClass="label" 
                        AssociatedControlID="TxtCodCheeKList" runat="server" Text="Codice Check List" />
                    
                </div>
                <div class="colOggetti" style="width:35%">
                    
                    <asp:TextBox ID="TxtCodCheeKList" runat="server"></asp:TextBox>
                    
                </div>
            </div>
             <div class="row" >
                <div class="collable" style="width:15%">
                    
                <asp:label id="lblOggettoMail" CssClass="label" 
                        AssociatedControlID="txtOggetto" runat="server" Text="Oggetto" />
                    
                </div>
                <div class="colOggetti" style="width:35%">
                    
                    <asp:TextBox ID="txtOggetto" runat="server"></asp:TextBox>
                    
                </div>
                <div class="collable" style="width:15%">
                    
                <asp:label id="lblTesto" CssClass="label" 
                        AssociatedControlID="txtTesto" runat="server" Text="Testo" />
                    
                </div>
                <div class="colOggetti" style="width:35%">
                    
                    <asp:TextBox ID="txtTesto" runat="server"></asp:TextBox>
                    
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">
                    
                <asp:label id="lblDataDal" CssClass="label" 
                        AssociatedControlID="TxtDataDal" runat="server" Text="Data Dal" />
                    
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:TextBox ID="TxtDataDal" CssClass="textbox" Text="01/01/2015" runat="server"></asp:TextBox>
                </div>
                <div class="collable" style="width:15%">
                    
                <asp:label id="LbldataAl" CssClass="label" 
                        AssociatedControlID="txtDataAl" runat="server" Text="Data Al" />
                    
                </div>
                <div class="colOggetti" style="width:35%">
                                        <asp:TextBox ID="txtDataAl" CssClass="textbox" runat="server"></asp:TextBox>         
                
                </div>
            </div>
            <asp:HiddenField ID="HiddenFieldIdEnte" runat="server" />
            <div class="RigaPulsanti" >
                 <div style="margin-right:5px">
                 <asp:Button ID="cmdRicerca" runat="server"  CssClass="Pulsante" Text="Ricerca" />
                </div>
            </div>
        </div>
        <br />
        <asp:datagrid id="dtgNotificheEffettuate" runat="server" CssClass="table"  Width="100%" CellPadding="2"  AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" PageSize="5" UseAccessibleHeader="True">
			<SelectedItemStyle BackColor="White" Font-Bold="true"></SelectedItemStyle>
            <EditItemStyle></EditItemStyle>
            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
            <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle></HeaderStyle>
			<Columns>
                <asp:TemplateColumn HeaderText="Sel"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImageButton1" CommandName="Select" ToolTip="Seleziona Dettaglio" AlternateText="Seleziona Dettaglio" runat="server" ImageURL="images/busta_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="IdNotifica" HeaderText=""></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="IdTipoNotifica" HeaderText=""></asp:BoundColumn>
		        <asp:BoundColumn DataField="TipoNotifica" HeaderText="Tipo Notifica"></asp:BoundColumn>
		        <asp:BoundColumn DataField="DataInvio" HeaderText="Data Invio"></asp:BoundColumn>
		        <asp:BoundColumn DataField="SintesiOggetto" HeaderText="Sintesi Oggetto"></asp:BoundColumn>
		        <asp:BoundColumn DataField="UsernameInviatore" HeaderText="User Name"></asp:BoundColumn>
		        <asp:BoundColumn DataField="Volontario" HeaderText="Volontario"></asp:BoundColumn>
                <asp:BoundColumn DataField="CheckList" HeaderText="Check List"></asp:BoundColumn>
			</Columns>
            <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
        
         <div style="width:100%" id="Testobody" visible="false" runat="server">
        
          <fieldset class="ContornoPagina">
         <legend>DETTAGLIO EMAIL</legend>
     
      <strong>Oggetto:</strong> <asp:Label ID="lblOggetto"  runat="server" ></asp:Label>
     <br />
    <br />
     
    <strong> Corpo del Messaggio: </strong>
     <br />
     <br />
    <asp:Label ID="lblTestoMail" runat="server" Text=""></asp:Label>

    </fieldset>
    </div>
   
      
        <div class="RigaPulsanti" style="text-align:right">
            <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
        </div>
   <!-- </fieldset> -->
    </form>
</body>
</html>
