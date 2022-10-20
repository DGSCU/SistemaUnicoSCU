<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmCOMPModificaConsulta.aspx.vb" Inherits="Futuro.WfrmCOMPModificaConsulta" %>
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
                var IdData = CostruisciId('TxtDataValuta');
                var sharpIdData = "#" + IdData
                $("" + sharpIdData + "").datepicker();
            });
           </script>
           <script type="text/javascript">
      /* <![CDATA[ */
        var pathPlus = "images/plus_xsmall.png";
        var pathMinus = "images/minus_xsmall.png";
        var titlePathMinus = "Riduci";
        var titlePathPlus = "Espandi";
        var inserimento = '1';
        var inserimento1 = '';

      
        /* ]]> */  
       </script>
        <script type="text/javascript">
       /* <![CDATA[ */
           $(function () 
         {
       
            var tornaSu = 0;
            if (inserimento1 != "") 
            {
                $('#RicercaVolontariContent').css('display', 'none');
                $('#RicercaVolontari').find("img").first().attr("src", pathPlus);
                $('#RicercaVolontari').find("img").first().attr("title", titlePathPlus);
            }
            else 
            {
                $('#RicercaVolontariContent').css('display', 'yes');
                $('#RicercaVolontari').find("img").first().attr("src", pathMinus);
                $('#RicercaVolontari').find("img").first().attr("title", titlePathMinus);
            }

            $('#TornaSuCaratteristicheEnti').click(function () {
                tornaSu = 1;
            });

            $('#RicercaVolontari').click(function () 
            {
                if (tornaSu == 0) 
                {
                    var isVisible = $('#RicercaVolontariContent').is(':visible');
                    $('#RicercaVolontariContent').slideToggle('fast');
                    $(this).toggleClass('slideSign');
                    if (!isVisible) 
                    {
                        $(this).find("img").first().attr("src", pathMinus);
                        $(this).find("img").first().attr("title", titlePathMinus);
                        title = "Espandi"
                    }
                    else 
                    {
                        $(this).find("img").first().attr("src", pathPlus);
                        $(this).find("img").first().attr("title", titlePathPlus);
                    }
                }
                tornaSu = 0;
                return false;
            });
          });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent"   runat="server">
    <fieldset class="ContornoPagina" >
        <legend>Gestione Elaborazione &nbsp; &nbsp;<asp:Label ID="Label1" runat="server" Text="">
            <asp:HiddenField ID="hdStatoElaborazione" runat="server" />
        </asp:Label></legend>

        <asp:label id="lblmess"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
     
        <br />
        <div class="wrapper" style="width:100%;border:0px"">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Dettaglio Elaborazione"></asp:Label>&nbsp;<asp:Label
                       ID="lblElaborazione" runat="server" Text=""></asp:Label></h2>
            </div>
          </div>
        <div class="wrapper" style="width:100%">
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblTipo" CssClass="label" AssociatedControlID="cboTipo" runat="server" Text="Tipo"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="cboTipo" runat="server">
                    </asp:DropDownList>
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lbldescrizione" CssClass="label" AssociatedControlID="TxtDescrizione" runat="server" Text="Descrizione"></asp:Label>
                </div> 
                <div class="colOggetti" style="width:35%">       
                          <asp:TextBox ID="TxtDescrizione" MaxLength="255" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row">

              <div class="collable" style="width:15%">

                    <asp:Label ID="lblDataValuta" CssClass="label" AssociatedControlID="TxtDataValuta" runat="server" Text="Data Valuta"></asp:Label>   

              </div>

               <div class="colOggetti" style="width:35%"> 
                    <asp:TextBox ID="TxtDataValuta" runat="server"></asp:TextBox>  
               </div>

                <div class="collable" style="width:15%">

                </div>
              <div class="colOggetti" style="width:35%">     
                         
              </div>
          
        </div>
      </div>
  <div class="wrapper" style="width:100%">
            <div class="RigaVuota" >&nbsp;</div>
        <div class="row">

              <div class="collable" style="width:15%">
                <asp:Label ID="lblImportoTotale" runat="server" Text="Importo Totale"></asp:Label>
                </div>
              <div class="colOggetti" style="width:35%">     
                  <asp:Label ID="lblTotale" runat="server" CssClass="bold" Text=""></asp:Label>
              </div>            
        </div>

     <div class="row">
            <div class="collable" style="width:15%">
                <asp:Label ID="lblNumeroPaghe" runat="server" Text="Numero Paghe"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">     
                <asp:Label ID="lblpaghe" CssClass="bold" runat="server" Text=""></asp:Label>
            </div>

            <div class="collable" style="width:15%">
                <asp:Label ID="lblNumeroPagheEffettive" runat="server" Text="Numero Paghe Effettive"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">     
                <asp:Label ID="lblpagheeffettive" CssClass="bold" runat="server" Text=""></asp:Label>
            </div>            
    </div>

         <div class="row">
         <div class="collable" style="width:15%">
             <asp:Label ID="lblNumeroVolontari" runat="server" Text="Numero Volontari"></asp:Label>
                </div>
              <div class="colOggetti" style="width:35%">     
                  <asp:Label ID="lblvolontari" CssClass="bold" runat="server" Text=""></asp:Label>
              </div>
              <div class="collable" style="width:15%">
                <asp:Label ID="lblNumeroVolontariEffettivi" runat="server" Text="Numero Volontari Effettivi"></asp:Label>
                </div>
              <div class="colOggetti" style="width:35%">     
                  <asp:Label ID="lblvolontarieffettivi" runat="server" CssClass="bold" Text=""></asp:Label>
              </div>            
        </div>

     <div class="row">
         <div class="collable" style="width:15%">
             <asp:Label ID="lblPagheRiproposte" runat="server" Text="Numero Paghe Riproposte"></asp:Label>
                </div>
              <div class="colOggetti" style="width:35%">     
                  <asp:Label ID="lblNumeroPagheRiproposte" CssClass="bold" runat="server" Text=""></asp:Label>
              </div>
              <div class="collable" style="width:15%">
                <asp:Label ID="lblImportiRiproposti" runat="server" Text="Importo Paghe Riproposte"></asp:Label>
                </div>
              <div class="colOggetti" style="width:35%">     
                  <asp:Label ID="lblTotaleImportiRiproposti" CssClass="bold" runat="server" Text=""></asp:Label>
              </div>
        </div>
<asp:GridView id="DtgDettagio" runat="server" Width="100%"  Caption="Dettaglio"  AllowPaging="False" ShowHeaderWhenEmpty="true"
                    CellPadding="2" Font-Size="Small"  CssClass="table" PageSize="99" 
                    AllowSorting="false" AutoGenerateColumns="true" UseAccessibleHeader="True">
		<FooterStyle></FooterStyle>
		<SelectedRowStyle Font-Bold="true" BackColor="White"></SelectedRowStyle>
		<EditRowStyle></EditRowStyle>
		<AlternatingRowStyle  CssClass="tr"></AlternatingRowStyle>
		<RowStyle CssClass="tr"   HorizontalAlign="Center"  ></RowStyle>
		<HeaderStyle></HeaderStyle>
        <PagerStyle CssClass="linkPageGrid"  />
      <PagerSettings Mode="NumericFirstLast" Visible="true" pagebuttoncount="10" position="Bottom"></PagerSettings >
	</asp:GridView>
    <br />
  </div>
        <div class="RigaPulsanti" style="text-align:right">
              
              
              
              <asp:Button ID="CmdRicercaVol0" runat="server" CssClass="Pulsante" Text="RICERCA" />
              <asp:Button ID="CmdConferma" runat="server" CssClass="Pulsante" Visible="false" Text="Conferma" />&nbsp;
              <asp:Button ID="CmdInserisci" runat="server" CssClass="Pulsante" Visible="false" Text="Inserisci" />
              <asp:Button ID="CmdModifica" runat="server" CssClass="Pulsante" Visible="false" Text="Modifica" />&nbsp;
              <asp:Button ID="CmdElimina" runat="server" CssClass="Pulsante" Visible="false" Text="Elimina" />&nbsp;
              <asp:Button ID="Chiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
        
        </div>
        </fieldset>
        
        <div style="text-align:right">
         <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="true"/>&nbsp;
        </div>
      <div>
       <asp:HyperLink ID="ApriCSV1"   AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" 
        Text="DOWNLOAD CSV" runat="server" 
        ForeColor="#003399" Visible="False"></asp:HyperLink>
      </div>
     
         <fieldset class="ContornoPagina">
        <legend> Elenco Paghe</legend>

          <fieldset class="ContornoPagina" runat="server" id="filtriVolontari" visible="true">
        <legend id="RicercaVolontari">
            <img alt="Espandi/Riduci" src="images/plus_xsmall.png" style="cursor: pointer;" title="Espandi" /> Filtri per Ricerca Volontari</legend>
         <div class="wrapper1" id="RicercaVolontariContent" style="width:100%">
            <div class="RigaVuota" >&nbsp;</div>
             <div class="row">
                   <div class="collable" style="width:15%">
                    <asp:Label ID="lblCodiceVolontario" CssClass="label" AssociatedControlID="txtVolontario" runat="server" Text="Volontario"></asp:Label>   
                </div> 
                   <div class="colOggetti" style="width:35%">       
                         
                       <asp:TextBox ID="txtVolontario" runat="server"></asp:TextBox>
                         
                </div>
                   <div class="collable" style="width:15%">
                    <asp:Label ID="lblEnte" CssClass="label" AssociatedControlID="txtEnte" runat="server" Text="Ente"></asp:Label>   
                </div> 
                   <div class="colOggetti" style="width:35%">       
                        
                       <asp:TextBox ID="txtEnte" runat="server"></asp:TextBox>
                </div>
            </div>
             <div class="row">
                   <div class="collable" style="width:15%">
                    <asp:Label ID="lblProgetto" CssClass="label" AssociatedControlID="txtCodiceProgetto" runat="server" Text="Progetto"></asp:Label>   
                </div> 
                   <div class="colOggetti" style="width:35%">       
                         
                       <asp:TextBox ID="txtCodiceProgetto" runat="server"></asp:TextBox>
                         
                </div>
                   <div class="collable" style="width:15%">
                </div> 
                   <div class="colOggetti" style="width:35%">       
                        
                </div>
            </div>
             <div class="RigaPulsanti">
              
              <asp:Button ID="CmdRicercaVol" runat="server" CssClass="Pulsante" Text="RICERCA" />
              </div>

            </div>
            
           
         </fieldset>   

     <asp:datagrid id="dgInfoPaghe" runat="server" CssClass="table"  Width="100%" 
                 ToolTip="Elenco Paghe"  CellPadding="2"  AllowSorting="True" 
                 AutoGenerateColumns="False" AllowPaging="True" PageSize="20" 
                 UseAccessibleHeader="True">
        <SelectedItemStyle BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
		<Columns>
          <asp:BoundColumn Visible="False" DataField="IdElaborazione" HeaderText="IdElaborazione"
				DataFormatString="{0:d}">
				<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="IdPaga" HeaderText="IdPaga"
				DataFormatString="{0:d}">
				<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="IdEntità" HeaderText="IdEntità"
				DataFormatString="{0:d}">
				<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
            <asp:BoundColumn Visible="False" DataField="IdAttività" HeaderText="IdAttività"
				DataFormatString="{0:d}">
				<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
            <asp:BoundColumn DataField="Volontario" HeaderText="Volontario" ></asp:BoundColumn>
            <%--<asp:BoundColumn DataField="Nome" HeaderText="Nome" ></asp:BoundColumn>
            <asp:BoundColumn DataField="Cognome" HeaderText="Cognome" ></asp:BoundColumn>--%>
            <asp:BoundColumn DataField="Ente" HeaderText="Ente" ></asp:BoundColumn>
            <%--<asp:BoundColumn DataField="CodiceEnte" HeaderText="Codice Progetto" ></asp:BoundColumn>--%>
            <asp:BoundColumn DataField="Progetto" HeaderText="Progetto" ></asp:BoundColumn>
            <asp:BoundColumn DataField="StatoPaga" HeaderText="Stato Paga" ></asp:BoundColumn>
            <asp:BoundColumn DataField="Importo" HeaderText="Importo" ></asp:BoundColumn>
			
            <%--<asp:BoundColumn DataField="DataValuta" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data Valuta">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
             <asp:BoundColumn Visible="False" DataField="StatoElaborazione" HeaderText="StatoElaborazione" ></asp:BoundColumn>
             <asp:BoundColumn Visible="False" DataField="Modificabile" HeaderText="Si/No" ></asp:BoundColumn> --%>
            <asp:TemplateColumn> 
                <ItemTemplate>
					<asp:ImageButton ID="CmdDettaglio" CommandName="Dettaglio" ToolTip="Dettaglio paga"  AlternateText="Dettaglio Paga" runat="server" ImageURL="images/lenteIngrandimento_small.png" CausesValidation="false"></asp:ImageButton>                         
                </ItemTemplate>
            </asp:TemplateColumn>
			
		</Columns>
		<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
	</asp:datagrid>

        </fieldset>
          <div class="RigaPulsanti" style="text-align:right">
              &nbsp;&nbsp;&nbsp;&nbsp;
        </div>
</asp:Content>
