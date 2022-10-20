<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmCOMPGestionePresenzeEstero.aspx.vb" Inherits="Futuro.WfrmCOMPGestionePresenzeEstero" %>
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
                var IdData = CostruisciId('TxtDataValuta');
                var sharpIdData = "#" + IdData
                $("" + sharpIdData + "").datepicker();
            });

//            function Modificato(Valore) {
////                alert(Valore)
//               document.all.MainContent_txtApplica1.value = Valore
//            }
           </script>
        <script type="text/javascript" >
  /* <![CDATA[ */
  
      
    function SoloNumeri(evt) {
        var iKeyCode = (evt.which) ? evt.which : evt.keyCode
        if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
            return false;

        return true;
    }    
 
	/* ]]> */
     </script>
        <script type="text/javascript">
           function Modificato(Valore,id) {
//               alert(Valore)
//                  alert(id)
//                  document.all.MainContent_txtApplica1.value = Valore
                 // document.all.MainContent_txtgiorniestero.value = Valore
                  id.value = Valore;
            }
          
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
                $('#FiltriEnteContent').css('display', 'none');
                $('#FiltriEnte').find("img").first().attr("src", pathPlus);
                $('#FiltriEnte').find("img").first().attr("title", titlePathPlus);
            }
            else 
            {
                $('#FiltriEnteContent').css('display', 'yes');
                $('#FiltriEnte').find("img").first().attr("src", pathMinus);
                $('#FiltriEnte').find("img").first().attr("title", titlePathMinus);
            }

            $('#TornaSuCaratteristicheEnti').click(function () {
                tornaSu = 1;
            });

            $('#FiltriEnte').click(function () 
            {
                if (tornaSu == 0) 
                {
                    var isVisible = $('#FiltriEnteContent').is(':visible');
                    $('#FiltriEnteContent').slideToggle('fast');
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>ANNO MESE &nbsp; &nbsp;</legend>
        <asp:label id="lblmess"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
     
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="SCELTA ANNO MESE COMPETENZA"></asp:Label>&nbsp;</h2>
            </div>
          </div>
        <div class="wrapper" style="width:100%">
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblAnnoMese" CssClass="label" AssociatedControlID="cboAnnoMeseEstero" runat="server" Text="MESE COMPETENZA"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                         
                    <asp:DropDownList ID="cboAnnoMeseEstero" runat="server">
                    </asp:DropDownList>
                         
                </div>
                <div class="collable" style="width:15%">

                   <asp:ImageButton ID="CmdDettaglio" CommandName="Dettaglio" ToolTip="Dettaglio paga"  AlternateText="Dettaglio Paga" runat="server" ImageURL="images/selezionato_small.png" CausesValidation="false"></asp:ImageButton>

                    <asp:Label ID="lbldescrizione" CssClass="label" AssociatedControlID="CmdDettaglio" 
                      runat="server" Text="SELEZIONA"></asp:Label>   

                </div> 
               
               
            </div>
            
           
 </div>
     
        </fieldset>

  <fieldset class="ContornoPagina">
        <legend>ELENCO ENTI &nbsp; &nbsp;</legend>
        
        <fieldset class="ContornoPagina" id="FlsFiltriEnte">
        <legend id="FiltriEnte">
        <img src="images/plus_xsmall.png" title="Espandi" alt="Espandi/Riduci" style="cursor: pointer;" /> Filtri per Ricerca Enti</legend>
         <div class="wrapper1" id="FiltriEnteContent" style="width:100%">
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                   <div class="collable" style="width:15%">
                    <asp:Label ID="lblConfermaAssenze" CssClass="label" AssociatedControlID="CboConfermaAssenze" runat="server" Text="Conferma Assenze"></asp:Label>   
                </div> 
                   <div class="colOggetti" style="width:35%">       
                         
                    <asp:DropDownList ID="CboConfermaAssenze" runat="server">
                    <asp:ListItem Value="Selezionare" Selected="True">Selezionare</asp:ListItem>
									<asp:ListItem Value="SI">SI</asp:ListItem>
									<asp:ListItem Value="NO">NO</asp:ListItem>
                    </asp:DropDownList>
                         
                </div>
                   <div class="collable" style="width:15%">
                    <asp:Label ID="lblCompletato" CssClass="label" AssociatedControlID="CboCompletato" runat="server" Text="Completato"></asp:Label>   
                </div> 
                   <div class="colOggetti" style="width:35%">       
                         
                    <asp:DropDownList ID="CboCompletato" runat="server">
                    <asp:ListItem Value="Selezionare" Selected="True">Selezionare</asp:ListItem>
									<asp:ListItem Value="SI">SI</asp:ListItem>
									<asp:ListItem Value="NO">NO</asp:ListItem>
                    </asp:DropDownList>
                         
                </div>
            </div>
            </div>
            
         <div class="wrapper1" style="width:100%">
            
            <div class="row">
             <div class="collable" style="width:15%">
                 &nbsp;</div>

             <div class="colOggetti" style="width:35%">
                 &nbsp;</div>

              <div class="collable" style="width:15%">
                  &nbsp;</div>

              <div class="colOggetti" style="width:35%;text-align:right; font-size:1.0em;height:35px;font-weight: bold;">
              
              <asp:Button ID="CmdRicerca" runat="server"  CssClass="Pulsante" Text="RICERCA" />
              </div>

            </div>
 
            </div>
         </fieldset>   
  <div class="RigaPulsanti" style="text-align:right" >
        <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" 
            Text="Esporta CSV" Visible="False" />
            <br />
         <asp:HyperLink ID="ApriCSV1"   AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" 
        Text="DOWNLOAD CSV" runat="server" 
        ForeColor="#003399" Visible="False"></asp:HyperLink>
      &nbsp;
    </div>
           
 <h3>
<asp:label id="lblMessaggio" runat="server" Text="Selezionare Mese Competenza per risultati." AssociatedControlID="dgElencoEnti"></asp:label>
</h3>

<br />
        <asp:datagrid id="dgElencoEnti" runat="server" CssClass="table"  
            Width="100%" ToolTip="Elenco Enti"  CellPadding="2"  
            AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" PageSize="5" 
            UseAccessibleHeader="True">
        <SelectedItemStyle BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
		<Columns>
            <asp:TemplateColumn> 
                <ItemTemplate>
					<asp:ImageButton ID="CmdSeleziona" CommandName="Seleziona" ToolTip="Seleziona" AlternateText="Seleziona" runat="server" ImageURL="images/valida_small.png" CausesValidation="false"></asp:ImageButton>                         
                </ItemTemplate>
            </asp:TemplateColumn>
			<asp:BoundColumn Visible="False" DataField="idEnte" HeaderText="idEnte"
				DataFormatString="{0:d}">
				<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
     
			<asp:BoundColumn DataField="CodiceEnte" HeaderText="COD.ENTE">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>

			<asp:BoundColumn DataField="Denominazione" HeaderText="DENOMINAZIONE">
				<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			
			<asp:BoundColumn DataField="TotaleVolontari" HeaderText="TOT.VOL">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>    
			
			<asp:BoundColumn DataField="TotaleVolontariLavorati" HeaderText="TOT.VOL LAVORATI">
				<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>

           <asp:BoundColumn DataField="ConfermaAssenze" HeaderText="CONFERMA ASSENZE SI/NO">
				<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>

			<asp:BoundColumn DataField="Completato" HeaderText="COMPLETATO SI/NO">
				<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
		</Columns>
		<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
    </fieldset>
     <fieldset class="ContornoPagina">
        <legend>ELENCO VOLONTARI ESTERO&nbsp; &nbsp;</legend>
         

          <fieldset class="ContornoPagina" runat="server" id="filtriVolontari" visible="false">
        <legend id="RicercaVolontari">
            <img alt="Espandi/Riduci" src="images/plus_xsmall.png" style="cursor: pointer;" 
                title="Espandi" /> Filtri per Ricerca Volontari</legend>
         <div class="wrapper1" id="RicercaVolontariContent" style="width:100%">
            <div class="RigaVuota" >&nbsp;</div>
             <div class="row">
                   <div class="collable" style="width:15%">
                    <asp:Label ID="lblCodiceVolontario" CssClass="label" AssociatedControlID="txtCodiceVolontario" runat="server" Text="Codice Volontario"></asp:Label>   
                </div> 
                   <div class="colOggetti" style="width:35%">       
                         
                       <asp:TextBox ID="txtCodiceVolontario" runat="server"></asp:TextBox>
                         
                </div>
                   <div class="collable" style="width:15%">
                    <asp:Label ID="lblNominativo" CssClass="label" AssociatedControlID="txtNominativo" runat="server" Text="Nominativo"></asp:Label>   
                </div> 
                   <div class="colOggetti" style="width:35%">       
                        
                       <asp:TextBox ID="txtNominativo" runat="server"></asp:TextBox>
                </div>
            </div>
             <div class="row">
                   <div class="collable" style="width:15%">
                    <asp:Label ID="lblProgetto" CssClass="label" AssociatedControlID="txtCodiceProgetto" runat="server" Text="Codice Progetto"></asp:Label>   
                </div> 
                   <div class="colOggetti" style="width:35%">       
                         
                       <asp:TextBox ID="txtCodiceProgetto" runat="server"></asp:TextBox>
                         
                </div>
                   <div class="collable" style="width:15%">
                    <asp:Label ID="lblFascia" CssClass="label" AssociatedControlID="txtFascia" runat="server" Text="Fascia"></asp:Label>   
                </div> 
                   <div class="colOggetti" style="width:35%">       
                        
                       <asp:TextBox ID="txtFascia" runat="server"></asp:TextBox>
                </div>
            </div>
             <div class="row">
              <div class="collable" style="width:15%">
                 <asp:Label ID="lblgiornivuoto" runat="server" AssociatedControlID="CboVuoto" Text="Giorni"></asp:Label>
               
               </div>

              <div class="colOggetti" style="width:35%">
          <asp:DropDownList ID="CboVuoto" runat="server">
                    <asp:ListItem Value="Selezionare" Selected="True">Selezionare</asp:ListItem>
									<asp:ListItem Value="SI">SI</asp:ListItem>
									<asp:ListItem Value="NO">NO</asp:ListItem>
                    </asp:DropDownList>
          </div>

              <div class="collable" style="width:15%">
                  &nbsp;
               </div>

              <div class="colOggetti" style="width:35%;text-align:right; font-size:1.0em;height:35px;font-weight: bold;">
              
              <asp:Button ID="CmdRicercaVol" runat="server" CssClass="Pulsante" Text="RICERCA" />
              </div>

            </div>
            
            </div>
         </fieldset>   

<h3>
<asp:label id="lblmessaggio1" runat="server" Text="Selezionare dalla lista dei risultati per Ente." AssociatedControlID="dgvolontariestero"></asp:label>
</h3>

    <div class="RigaPulsanti" style="text-align:right">
    
               <asp:Label ID="lblApplica" Visible="false" runat="server" Text="Applica lo stesso valore a tutti i campi GIORNI ESTERO non valorizzati "></asp:Label>
                 &nbsp; 
                 &nbsp;
        <asp:TextBox ID="txtApplica" Visible="false" runat="server" onkeypress="javascript:return SoloNumeri(event)" Width="40px"></asp:TextBox>
        <%--<asp:HiddenField ID="txtApplica1" runat="server" />--%>
                 &nbsp; 
                 &nbsp;
       <asp:Button ID="CmdApplica" runat="server" Visible="false" CssClass="Pulsante" 
                  Text="Applica" /> 
                 &nbsp; 
                 &nbsp;
              <asp:Button ID="CmdSalva" runat="server" Visible="false" CssClass="Pulsante" 
                  Text="SALVA" />
              &nbsp;
              &nbsp;
              <asp:Button ID="CmdCompleta" runat="server" Visible="false" CssClass="Pulsante" Text="COMPLETA" />
        
        </div>
        
    <br />
   
     <asp:datagrid id="dgvolontariestero" runat="server" CssClass="table"  Width="100%" 
                 ToolTip="Elenco volontari estero"  CellPadding="2"  AllowSorting="True" 
                 AutoGenerateColumns="False" AllowPaging="false" PageSize="100000" 
                 UseAccessibleHeader="True">
        <SelectedItemStyle BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
		<Columns>
          <asp:BoundColumn Visible="False" DataField="IDEntità" HeaderText="IdVolontario"
				DataFormatString="{0:d}">
				<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="IdEnte" HeaderText="IdEnte"
				DataFormatString="{0:d}">
				<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
            <asp:BoundColumn Visible="False" DataField="IdAttività" HeaderText="IdAttività"
				DataFormatString="{0:d}">
				<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
           
            <asp:BoundColumn DataField="CodiceEnte" HeaderText="COD.ENTE" ></asp:BoundColumn>
          
            <asp:BoundColumn DataField="CodiceVolontario" HeaderText="COD.VOL" ></asp:BoundColumn>
           
            <asp:BoundColumn DataField="Nominativo" HeaderText="NOMINATIVO" ></asp:BoundColumn>
            <asp:BoundColumn DataField="DIS" HeaderText="DIS" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
            <asp:BoundColumn DataField="DFS" HeaderText="DFS" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
             <asp:BoundColumn DataField="Progetto" HeaderText="COD.PROG" ></asp:BoundColumn>
              <asp:BoundColumn DataField="DIP" HeaderText="DIP" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
            <asp:BoundColumn DataField="DFP" HeaderText="DFP" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
            <asp:BoundColumn DataField="FASCIA" HeaderText="FASCIA(A-B-C)" ></asp:BoundColumn>
            <asp:TemplateColumn HeaderText="GIORNI ESTERO"> 
                    <ItemTemplate>
                        <asp:TextBox ID="txtgiorniestero" Width="40px" Text='<%# Eval("Giorni")%>' onkeypress="javascript:return SoloNumeri(event)" onblur="Modificato(this.value,this.id);"  runat="server"></asp:TextBox><asp:Label
                            ID="lblesito" runat="server" Text=""></asp:Label>         
                    </ItemTemplate>
                </asp:TemplateColumn>
			<%--<asp:TemplateColumn HeaderText="Applica a tutti"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgBtnApplica"  CommandName="Applica" Visible="true" ToolTip="Applica a tutti"   AlternateText="Applica a tutti" runat="server" ImageURL="images/selezionato_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>--%>
                 <asp:TemplateColumn HeaderText="Rimborsi Decurtazioni"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgBtnVai" CommandName="Vai" ToolTip="Indennità Rimborso" AlternateText="Indennità Rimborso" runat="server" ImageURL="images/vai_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
                 <asp:BoundColumn Visible="False" DataField="Mensilità" HeaderText="Mensilità"
				DataFormatString="{0:d}">
				<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
         

		</Columns>
		
	</asp:datagrid>

     <div class="RigaPulsanti" style="text-align:right">
              &nbsp;
              <asp:Button ID="CmdSalva1" runat="server" Visible="false" CssClass="Pulsante" 
                  Text="SALVA" />
              &nbsp;
              &nbsp;
              <asp:Button ID="CmdCompleta1" runat="server" Visible="false" CssClass="Pulsante" Text="COMPLETA" />
        
        </div>
       
     </fieldset>
     <div class="RigaPulsanti" style="text-align:right">
        <asp:Button ID="CmdChiudi" runat="server" Visible="true" CssClass="Pulsante" Text="CHIUDI" />
       </div>
</asp:Content>
