<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmVisualizzaCorsiOLP.aspx.vb" Inherits="Futuro.WfrmVisualizzaCorsiOLP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
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

            //DataDalValutazione
            var DataDal = CostruisciId('txtdatadal');
            var readonly = '<%=txtdatadal.ReadOnly%>';
            if (readonly == "False") 
          {
                var sharpDataAvvio = "#" + DataDal
                $("" + sharpDataAvvio + "").datepicker();
            }

            //DataDalRichiesta
            var DataDal1 = CostruisciId('txtdatadal1');
            var readonly = '<%=txtdatadal1.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataAvvio1 = "#" + DataDal1
                $("" + sharpDataAvvio1 + "").datepicker();
            }

            //DataAlValutazione
            var DataAl = CostruisciId('txtdataal');
            var readonly = '<%=txtdataal.ReadOnly%>';
            if (readonly == "False") 
            {
                var sharpDataAvvio = "#" + DataAl
                $("" + sharpDataAvvio + "").datepicker();
            }


            //DataAlRichiesta
            var DataAl1 = CostruisciId('txtdataal1');
            var readonly = '<%=txtdataal1.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataAvvio1 = "#" + DataAl1
                $("" + sharpDataAvvio1 + "").datepicker();
            }

        });

	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
        <legend>RICERCA CORSI OLP</legend>
<div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="CORSI OLP"></asp:Label>&nbsp;</h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" runat="server" id="riganonente" visible="false">
             <div class="collable" style="width:15%">  
                 <asp:Label ID="lblCodiceEnte" runat="server"  AssociatedControlID="txtCodiceEnte" Text="Codice Ente"></asp:Label>
             </div>
             <div class="colOggetti" style="width:35%">
              <asp:TextBox ID="txtCodiceEnte" runat="server"> </asp:TextBox>
             </div>
             <div class="collable" style="width:15%">  
                 <asp:Label ID="lblNomeEnte" runat="server" AssociatedControlID="txtNomeEnte" Text="Nome Ente"></asp:Label>
             </div>
             <div class="colOggetti" style="width:35%">
              <asp:TextBox ID="txtNomeEnte" runat="server"> </asp:TextBox>
             </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodProgRend" CssClass="label" AssociatedControlID="txtCodCORSO" runat="server" Text="Codice Riferimento"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    
                    <asp:TextBox ID="txtCodCORSO" runat="server"> </asp:TextBox>
                     
                </div>
                <div class="collable" style="width:15%"> 
                <asp:Label ID="lblTitoloProg" AssociatedControlID="ddlStatoRichiesta" runat="server" Text="Stato Richiesta"></asp:Label>     
                    
                </div>
                <div class="colOggetti" style="width:35%">
                    
                    <asp:DropDownList ID="ddlStatoRichiesta" runat="server">
                    <asp:ListItem Value="0" text=""></asp:ListItem>
                    <asp:ListItem  Value="1">Registrata</asp:ListItem>
                    <asp:ListItem  Value="2">Approvata</asp:ListItem>
                    <asp:ListItem  Value="3">Respinta</asp:ListItem>
                    </asp:DropDownList>
                    
                </div>
            </div>
          

           <div class="row" id="divDataRichiesta" runat="server">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lbldal1" CssClass="label" AssociatedControlID="txtdatadal1" runat="server" Text="Data Richiesta Dal"></asp:Label>
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtdatadal1" Width="75px"  CssClass="textbox" MaxLength="10" runat="server"></asp:TextBox>         
                </div> 
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblAl1" CssClass="label" AssociatedControlID="txtdataal1" runat="server" Text="Data Richiesta Al"></asp:Label>
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtdataal1" Width="75px"  CssClass="textbox" MaxLength="10" runat="server"></asp:TextBox>         
                </div>    
            </div>
             <div class="row" id="divDataValutazione" runat="server">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lbldal" CssClass="label" AssociatedControlID="txtdatadal" runat="server" Text="Data Valutazione Dal"></asp:Label>
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtdatadal" Width="75px"  CssClass="textbox" MaxLength="10" runat="server"></asp:TextBox>         
                </div> 
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblAl" CssClass="label" AssociatedControlID="txtdataal" runat="server" Text="Data Valutazione Al"></asp:Label>
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtdataal" Width="75px"  CssClass="textbox" MaxLength="10" runat="server"></asp:TextBox>         
                </div>    
            </div>
             

            
            <div class="RigaPulsanti">
                
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
                 <br />
               
               
               
            </div>
        
         <p></p>
         </div>
        
         <asp:datagrid id="dgRisultatoRicerca" runat="server" Caption="Risultato Ricerca Corsi OLP"  CssClass="table" CellPadding="2" PageSize = "10"  AllowPaging="true" 	
        AllowSorting="True"  AutoGenerateColumns="False" UseAccessibleHeader="True"  Width="100%">
		<FooterStyle></FooterStyle>
		<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
	    <Columns>
        <asp:BoundColumn Visible="False" DataField="IdCorsoFormazioneOLP" HeaderText="IdCorsoFormazioneOLP"></asp:BoundColumn>
        <asp:BoundColumn Visible="False" DataField="IdEnte" HeaderText="IdEnte"></asp:BoundColumn>    
         <asp:TemplateColumn HeaderText="Dettaglio"> 
             <ItemTemplate >
                 <asp:ImageButton ID="CmdImgCorsoOLP" AlternateText="Seleziona" CommandName="Selezionato" ImageUrl="~/Images/ZoomIn_small.png" runat="server" />
             </ItemTemplate>
        </asp:TemplateColumn>
         <asp:BoundColumn DataField="IdCorsoFormazioneOLP"   HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Cod. Rif."></asp:BoundColumn>
         <asp:BoundColumn DataField="Ente"   HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Denominazione Ente"></asp:BoundColumn>
         <asp:BoundColumn DataField="StatoRichiesta"    HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Stato Ric."></asp:BoundColumn>
        <asp:BoundColumn DataField="UsernameRichiesta"   HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="User Name Ric."></asp:BoundColumn>
        <asp:BoundColumn DataField="DataRichiesta" HeaderStyle-CssClass="linkPageGridRidotto"  HeaderText="Data Ric."></asp:BoundColumn>
        <asp:BoundColumn DataField="UsernameValutazione" HeaderStyle-CssClass="linkPageGridRidotto"  HeaderText="User Name Val."></asp:BoundColumn>
        <asp:BoundColumn DataField="DataValutazione" HeaderStyle-CssClass="linkPageGridRidotto"  HeaderText="Data Val."></asp:BoundColumn>
        
        <asp:TemplateColumn HeaderText="Pdf"> 
             <ItemTemplate >
                 <asp:ImageButton ID="CmdPdf" AlternateText="Genera PDF" CommandName="SelezionatoPdf" ImageUrl="~/Images/pdf.png" Width="30px" Height="30px" Visible='<%# IIF(Eval("StatoRichiesta").ToString().Equals("Approvata"), True, False) %>' runat="server" />
             </ItemTemplate>

        </asp:TemplateColumn>
        <asp:BoundColumn DataField="Denominazione" Visible="false"   HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Denominazione"></asp:BoundColumn>
        <asp:BoundColumn DataField="Competenza" Visible="false"   HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Competenza"></asp:BoundColumn>
        <asp:BoundColumn DataField="CodiceRegione" Visible="false"   HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="CodiceRegione"></asp:BoundColumn>
    </Columns>
	<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGridRidotto"  Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
    </fieldset>
</asp:Content>
