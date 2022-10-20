<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmCOMPInfoPaghe.aspx.vb" Inherits="Futuro.WfrmCOMPInfoPaghe" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
            $(function () {
                var IdData = CostruisciId('TxtDataRiferimento');
                var sharpIdData = "#" + IdData
                $("" + sharpIdData + "").datepicker();
            });

            function VisualizzaAtt() {
                var IdVolontario = '<%= Request.Params("IdVol")%>';
                var winOption = PopUpOption();
                myWin = window.open("WfrmAttivitaVolontari.aspx?IdVolontario=" + IdVolontario, "dettagli", winOption);
            }; 

           </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
            <legend>Paghe Volontario</legend>
            <div class="wrapper" style="width:100%;border:0px">
                <div class="rowGroup" style="height:auto"> 
                    <div class="row" style="height:auto">                  
                        <div class="colHyperLink" style="width:49%">
                            <asp:LinkButton ID="HplSommeResitituite" runat="server" style="cursor:pointer" Text="Somme Restituite" Visible="true"></asp:LinkButton>
                        </div>
                        <div class="colHyperLink" style="width:49%;float:right;text-align:right">
                            <asp:LinkButton ID="HplRimborsidecurtazioni" runat="server" style="cursor:pointer;" Text="Rimborsi/Decurtazioni" Visible="true"></asp:LinkButton>
                        </div>                      
                     </div>
                    <div class="row" style="height:auto">                  
                        <div class="colHyperLink" style="width:49%; height: 19px;">
                            <asp:LinkButton ID="HplPresenzeEstero" runat="server" style="cursor:pointer" Text="Presenze Estero" Visible="true"></asp:LinkButton>
                        </div> 
                        <div class="colHyperLink" style="width:49%;height: 19px;float:right;text-align:right">
                            <asp:LinkButton ID="HplGestioneAssenze" runat="server" style="cursor:pointer;" Text="Gestione Assenze" Visible="true"></asp:LinkButton>
                        </div>                      
                    </div>
                     <div class="row" style="height:auto">                  
                        <div class="colHyperLink" style="width:49%; height: 19px;">
                            <asp:LinkButton ID="hlCrono" runat="server" style="cursor:pointer" Text="Visualizza Cronologia Volontario" OnClientClick="javascript: VisualizzaAtt()" Visible="true"></asp:LinkButton>
                         </div>                     
                    </div>
                </div>
            </div>
            
            <fieldset class="ContornoPagina">
        <legend>Volontario</legend>
        <asp:label id="lblmess"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
      
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="dati volontario"></asp:Label></h2>
            </div>
          </div>
        <div class="wrapper" style="width:100%">
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblCodiceVolontario" CssClass="label" AssociatedControlID="lblCognome" runat="server" Text="Codice Volontario"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblCognome" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblNominativo" CssClass="label" AssociatedControlID="lblNome" runat="server" Text="Nominativo"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblNome" CssClass="bold" runat="server"></asp:label>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblSessoLabel" CssClass="label" AssociatedControlID="lblsesso" runat="server" Text="Sesso"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblsesso" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDataNascitaLabel" CssClass="label" AssociatedControlID="lbldataNascita" runat="server" Text="Data Nascita"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lbldataNascita" CssClass="bold" runat="server"></asp:label>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblCodiceFiscale" CssClass="label" AssociatedControlID="lblCodFis" runat="server" Text="Codice Fiscale"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblCodFis" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblIban" CssClass="label" AssociatedControlID="lblIbanVol" runat="server" Text="IBAN"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblIbanVol" CssClass="bold" runat="server"></asp:label>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDataInizioLabel" CssClass="label" AssociatedControlID="lblDataInizio" runat="server" Text="Data Inizio"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblDataInizio" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDataFineLabel" CssClass="label" AssociatedControlID="lbldataFine" runat="server" Text="Data Fine"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lbldataFine" CssClass="bold" runat="server"></asp:label>         
                </div>
            </div>



             <div class="row" runat="server" id="dateIFinterruzione" visible="false">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDataInizioInterruzioneLabel" CssClass="label" AssociatedControlID="lblDataInizioInterruzione" runat="server" Text="Data Inizio Interruzione"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lbldataInizioInterruzione" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDataFineInterruzioneLabel" CssClass="label" AssociatedControlID="lbldataFineInterruzione" runat="server" Text="Data Fine Interruzione"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lbldataFineInterruzione" CssClass="bold" runat="server"></asp:label>         
                </div>
            </div>





            <div class="row" runat="server" id="dateRipreseServizio" visible="false">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDataRipresaServiziolabel" CssClass="label" AssociatedControlID="lblDataRipresaServizio" runat="server" Text="Data Ripresa Servizio"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblDataRipresaServizio" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDataFineServizioOriginariaLabel" CssClass="label" AssociatedControlID="lblDataFineServizioOriginaria" runat="server" Text="Data Fine Servizio Originaria"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblDataFineServizioOriginaria" CssClass="bold" runat="server"></asp:label>         
                </div>
            </div>
        </div>


        <br />
        <div class="RigaPulsanti" style="width:100%">

        <div style="width:15%;float:left;">&nbsp;</div>
         <div style="width:30%;float:left;">   
         <asp:Button ID="cmdSospendiVol" CssClass="Pulsante" runat="server" Visible="false" Text="Sospendi Volontario" />
         <asp:Button ID="CmdRiattivaVol" CssClass="Pulsante" runat="server" Visible="false" Text="Ripristina Volontario" />
         </div>
          <div style="width:30%;float:left;text-align:right"> 
          <asp:Button ID="cmdSospendiProg" CssClass="Pulsante" Visible="false" runat="server" Text="Sospendi Progetto" />
          <asp:Button ID="CmdRiattivaProg" CssClass="Pulsante" Visible="false"  runat="server" Text="Ripristina Progetto" />
          </div>
         <div style="width:20%;float:right;text-align:right">&nbsp;</div>
        </div>
        </fieldset>
         <fieldset class="ContornoPagina">
        <legend>Elenco Paghe</legend>

       
            
     <asp:datagrid id="dgInfoPaghe" runat="server" CssClass="table"  Width="100%" 
                 ToolTip="Elenco Paghe"  CellPadding="2"  AllowSorting="True" 
                 AutoGenerateColumns="False" AllowPaging="True" PageSize="5" 
                 UseAccessibleHeader="True">
        <SelectedItemStyle BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
		<Columns>
          
			<asp:BoundColumn Visible="False" DataField="IdPaga" HeaderText="IdPaga"
				DataFormatString="{0:d}">
				<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="IdElaborazione" HeaderText="Cod. Elaborazione">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="DataValuta" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data Valuta">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Importo" HeaderText="Importo" ></asp:BoundColumn>
			<asp:BoundColumn DataField="StatoPaga" HeaderText="Stato" ></asp:BoundColumn> 
             <asp:BoundColumn Visible="False" DataField="StatoElaborazione" HeaderText="StatoElaborazione" ></asp:BoundColumn>
             <asp:BoundColumn Visible="False" DataField="Modificabile" HeaderText="Si/No" ></asp:BoundColumn> 
            <asp:TemplateColumn> 
                <ItemTemplate>
					<asp:ImageButton ID="CmdDettaglio" CommandName="Dettaglio" ToolTip="Dettaglio paga"  AlternateText="Dettaglio Paga" runat="server" ImageURL="images/lenteIngrandimento_small.png" CausesValidation="false"></asp:ImageButton>                         
                </ItemTemplate>
            </asp:TemplateColumn>
			
		</Columns>
		<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
	</asp:datagrid>

        </fieldset>

          <fieldset class="ContornoPagina">
        <legend>Dettaglio Paga Rif.<asp:Label ID="lblRiferimentoIdPaga" runat="server" Text=""></asp:Label></legend>

        <p style="text-align:center">
        <asp:label id="lbltesto" runat="server" CssClass="bold" Text="Dettaglio Paga"></asp:label>
    </p>
     <asp:datagrid id="dgDettaglioPaga" runat="server" CssClass="table"  Width="100%" 
                  ToolTip="Elenco Dettaglio Paghe"  CellPadding="2"  AllowSorting="True" 
                  AutoGenerateColumns="False"  
                  UseAccessibleHeader="True">
        <SelectedItemStyle BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
		<Columns>
          
			<asp:BoundColumn Visible="False" DataField="IdPaga" HeaderText="IdPaga"
				DataFormatString="{0:d}">
				<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Descrizione" HeaderText="Tipo">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<%--<asp:BoundColumn DataField="DataValuta" Visible="false" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data Valuta">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>--%>
            
            <asp:BoundColumn DataField="Descr" HeaderText="Descrizione" ></asp:BoundColumn>
			<asp:BoundColumn DataField="Segno" HeaderText="+/-" ></asp:BoundColumn> 
			<asp:BoundColumn DataField="Importo" HeaderText="Importo" ></asp:BoundColumn>
			<asp:BoundColumn DataField="StatoElemento" HeaderText="Stato" ></asp:BoundColumn> 
             <asp:BoundColumn DataField="IdElementoRetributivo" Visible="false" HeaderText="IdElementoRetributivo" ></asp:BoundColumn> 
           
			
		</Columns>
		
	</asp:datagrid>

        </fieldset>


         <div class="RigaPulsanti"style="text-align:right">
             <asp:Button ID="cmdRicalcola" CssClass="Pulsante" Visible="False" 
                 runat="server" Text="Ricalcola" />&nbsp;
              <asp:Button ID="CmdAnnulla" runat="server" CssClass="Pulsante" 
                 Text="Annulla Paga" Visible="False" />
        &nbsp;<asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
        </div>
        
        </fieldset>
        <br />
        <div style="text-align:right">
            &nbsp;&nbsp;&nbsp;&nbsp;
    </div>
</asp:Content>
