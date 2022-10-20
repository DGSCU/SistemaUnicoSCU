<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WebAggiungiSedeProgettoEU.aspx.vb" Inherits="Futuro.WebAggiungiSedeProgettoEU" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
          <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css"/>
        <script src="Scripts/JHelper.js" type="text/javascript"></script>
<script type="text/javascript">
   
 /* <![CDATA[ */
     
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
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
<fieldset class="ContornoPagina">
        <legend>Sede UE non iscritta</legend>
        
        <div class="wrapper" style="width:100%">

            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Sede UE non iscritta"></asp:Label>
               <asp:Label ID="lblEnte" runat="server"  ></asp:Label>
               </h2>
            </div>
            <div class="row" style="height:auto">
                <asp:Label ID="msgConferma" CssClass="msgConferma" runat="server"></asp:Label>
            </div>
            <div class="row" style="height:auto">
                <asp:Label ID="msgErrore" CssClass="msgErrore" runat="server"></asp:Label>                
            </div>
            <div class="row" style="height:auto">
                <asp:Label ID="msgInfo" CssClass="msgInfo" runat="server"></asp:Label>
            </div>
            <br />

        <div class="row">
            <div class="collable" style="width:15%">          
                <asp:Label ID="lblSede" CssClass="label"  runat="server" Text="<strong>(*)</strong>Denominazione Sede" AssociatedControlID="txtdenominazione" />
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtdenominazione" CssClass="textbox"  runat="server"></asp:TextBox>
           </div>
              <div class="collable" style="width:15%">          
            <asp:Label ID="lblProvincia" CssClass="label"  runat="server" Text="<strong>(*)</strong>Nazione" AssociatedControlID="ddlProvincia"></asp:Label>
            </div>
                <div class="colOggetti" style="width:35%">
            <asp:dropdownlist id="ddlProvincia"  AutoPostBack="true" 
                runat="server" CssClass="ddlClass"  >
                </asp:dropdownlist>
           </div>
        </div>
        <div class="row">
            <div class="collable" style="width:15%">
                <asp:Label ID="lblCity" CssClass="label" runat="server" Text="<strong>(*)</strong>Città Estera" AssociatedControlID="txtCity"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%">
              <asp:TextBox ID="txtCity" CssClass="textbox" runat="server"  ></asp:TextBox>
        </div>
            
        <div class="collable" style="width:15%">
                <asp:Label ID="lblIndirizzo" CssClass="label" runat="server" Text="<strong>(*)</strong>Indirizzo completo" AssociatedControlID="txtIndirizzo"></asp:Label>
        </div>

        <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtIndirizzo" CssClass="textbox"  runat="server"  onkeyup="javascript: SetContextKey();"></asp:TextBox>
                             
        </div>
    </div>
   
   
     <div class="collable" style="width:15%">
                <asp:Label ID="lblNote" Text="Note" AssociatedControlID="txtNote" CssClass="label" runat="server" />
     </div>
     <div class="colOggetti" style="width:35%">
                    <asp:TextBox ID="txtNote" CssClass="textbox" runat="server" 
             TextMode="MultiLine" Rows="3" Width="90%" ></asp:TextBox>
     </div>


      
    <br />
      <div  class="RigaPulsanti" >

      <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" ToolTip="Salva" Text="Salva"  />
      <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server"  Text="Chiudi"  />

      </div>
      
       
            <br />
            <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" Caption="Sede Inserita"
                    ToolTip="Sede Inserita" CssClass="table"  AllowPaging="True" 
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
					<asp:BoundColumn DataField="Denominazione" HeaderText="Nome Sede"></asp:BoundColumn>
					<asp:BoundColumn DataField="StatoEnteSede" HeaderText="Stato" Visible="false"></asp:BoundColumn>
					<asp:BoundColumn DataField="Note" HeaderText="Note" Visible="false"></asp:BoundColumn>
					<asp:BoundColumn DataField="Certificazione" HeaderText="Presenza Iscrizione" Visible="false">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
				</Columns>
		    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
	    </asp:datagrid>

            
               </div>  
            </fieldset>
        
        <asp:HiddenField id="lblidsedeattuazione" runat="server"/>
        <asp:HiddenField id="lblidentesede" runat="server"/>
</asp:Content>
