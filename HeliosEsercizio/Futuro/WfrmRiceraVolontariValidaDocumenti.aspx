<%@ Page Title="ConvalidaDocumenti" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRiceraVolontariValidaDocumenti.aspx.vb" Inherits="Futuro.WfrmRiceraVolontariConvalidaDocumenti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
           var prefissoIdClient = "MainContent_";

           function ImpostaFormId() {
               formId = document.forms[0].id;
               return formId;
           }
           function CostruisciId(IdServer) {
               var IdClient = prefissoIdClient + IdServer
               return IdClient
           }


           $(function () {
               var IdData = CostruisciId('TxtDataInizioServizio');
               var sharpIdData = "#" + IdData
               $("" + sharpIdData + "").datepicker();
           });
      	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="firefox">

    <fieldset  class="ContornoPaginaScroll">

        <legend>Ricerca Documenti Volontari per Validazione</legend>
      <h3>
        <asp:label id="Lblmsg" runat="server"></asp:label>
      </h3>
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Documenti Volontari per Validazione"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" id="divTipoUtente" runat="server">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblEnte" CssClass="label" AssociatedControlID="txtEnte" runat="server" Text="Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtEnte" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodEnte" CssClass="label" AssociatedControlID="txtCodEnte" runat="server" Text="Cod. Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodEnte" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCognome" CssClass="label" AssociatedControlID="txtCognome" runat="server" Text="Cognome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCognome" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblNome" CssClass="label" AssociatedControlID="txtNome" runat="server" Text="Nome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtNome" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
            <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodVolontario" CssClass="label" AssociatedControlID="TxtCodVolontario" runat="server" Text="Cod. Volontario"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="TxtCodVolontario" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
       

                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodProgetto" CssClass="label" AssociatedControlID="txtCodProgetto" runat="server" Text="Cod. Progetto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodProgetto" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblProgetto" CssClass="label" AssociatedControlID="txtProgetto" runat="server" Text="Progetto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtProgetto" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                   <asp:Label ID="LblCodiceSedeAttuazione" CssClass="label" AssociatedControlID="TxtCodiceSedeAttuazione" runat="server" Text="Cod. Sede"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="TxtCodiceSedeAttuazione" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
             </div>
             <div  class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblDoc" CssClass="label" 
                        AssociatedControlID="ddlPrefissiDocumenti" runat="server" Text="Tipo Documento"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlPrefissiDocumenti" runat="server">
                    </asp:DropDownList>
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblStatoDocumenti" CssClass="label" 
                        AssociatedControlID="ddlStatoDocumenti" runat="server" Text="Stato Documento"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlStatoDocumenti" runat="server">
                        <asp:ListItem Value="0" Text="Da Convalidare">Da Validare</asp:ListItem>
                        <asp:ListItem Value="1" Text="Valido">Validato</asp:ListItem>
                        <asp:ListItem Value="2" Text="Non Valido">Non Valido</asp:ListItem>
                    </asp:DropDownList>
                </div>
             </div>
             <div  class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LdlDataInizioServizio" CssClass="label" AssociatedControlID="TxtDataInizioServizio" runat="server" Text="Data Inizio Servizio (gg/mm/aaaa)"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="TxtDataInizioServizio" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="LblRegione" CssClass="label" 
                        AssociatedControlID="ddlRegione" runat="server" Text="Regione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlRegione" runat="server">                      
                    </asp:DropDownList>
                </div>
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
                <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
		        <br />
	            <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
              &nbsp;
            </div>
         </div>
        <br />
        <h3>
        <asp:label id="lblmessaggio" runat="server" AssociatedControlID="dgVolontari"></asp:label>
    </h3>
        <br/>
    <fieldset class="ContornoPaginaScrollT">

    <div class="firefox">

    <asp:datagrid id="dgVolontari" runat="server" 
            ToolTip="Elenco Volontari" CssClass="table"  AllowPaging="True" 
            AllowSorting="True" AutoGenerateColumns="False"  Visible="False" 
            UseAccessibleHeader="True">
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		<HeaderStyle></HeaderStyle>
		<Columns>
			<asp:BoundColumn Visible="False" DataField="IdEntit&#224;" HeaderText="IdEntit&#224;"></asp:BoundColumn>
		    <asp:BoundColumn DataField="idente" HeaderText="idente" Visible="False"></asp:BoundColumn>
            <asp:BoundColumn DataField="identitàdocumento" HeaderText="identitàdocumento" Visible="False"></asp:BoundColumn>
		    <asp:BoundColumn DataField="Ente" HeaderText="Ente"></asp:BoundColumn>
             <asp:TemplateColumn HeaderText="Codice Volontario">
             
             <ItemTemplate>
                       <asp:LinkButton ID="lblCodiceVolontario"  CommandName="Volontario" runat="server"><%# Eval("codicevolontario")%></asp:LinkButton>          
             </ItemTemplate>

            </asp:TemplateColumn>
			<asp:BoundColumn DataField="nominativo" HeaderText="Nominativo">
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="CodiceFiscale" HeaderText="Cod. Fiscale"></asp:BoundColumn>
			<asp:BoundColumn DataField="Documento" HeaderText="Documento"></asp:BoundColumn>
            <asp:BoundColumn DataField="NomeFile" HeaderText="FileName" Visible="False"></asp:BoundColumn>
			<asp:BoundColumn DataField="RiferimentoTemporale" HeaderText="Riferimento Temporale"></asp:BoundColumn>
			<asp:BoundColumn DataField="StatoDocumento" HeaderText="Stato Documento"></asp:BoundColumn>
            <asp:BoundColumn DataField="idattività" HeaderText="idattività" Visible="False">
            </asp:BoundColumn>
            <asp:TemplateColumn HeaderText="Download"> 
                <ItemTemplate>
	                <asp:ImageButton ID="imgScaricaDoc" style="cursor:pointer;" CommandName="Download" alt="Scarica Documento" ToolTip='Scarica Documento' runat="server" ImageURL="images/giu_small.png" CausesValidation="false"></asp:ImageButton>  
                </ItemTemplate>
            </asp:TemplateColumn>                                   
            <asp:TemplateColumn HeaderText="Valido"> 
                <ItemTemplate>
				    <asp:ImageButton ID="imgValida"  CommandName="Valida" AlternateText="Documento Valido" 
                        ToolTip="Valida Documento" runat="server" ImageURL="Images/selezionato_small.png" Visible='<%# IIF(Eval("PulsanteConfermaValida").ToString().Equals("NO"), False, True) %>'></asp:ImageButton>  
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Non Valido"> 
                <ItemTemplate>
			    	<asp:ImageButton ID="imgNonValida"  CommandName="NonValida" AlternateText="Documento non Validato" 
                        ToolTip="Documento Non Valido" runat="server" ImageURL="Images/deselezionato_small.png" Visible='<%# IIF(Eval("PulsanteConfermaNONValida").ToString().Equals("NO"), False, True) %>'></asp:ImageButton>  
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Presenze/ Rimborso/ Ore Formazione"> 
                <ItemTemplate >
			    	<asp:ImageButton  ID="imgDett"  CommandName="Dettaglio" AlternateText="Entra nel Dettaglio" 
                        ToolTip="Dettaglio Presenze/Rimborso/Ore Formazione" runat="server" ImageURL="Images/ZoomIn_small.png" Visible='<%# IIF(Eval("PulsantePresenzeViaggio").ToString().Equals("NO"), False, True) %>'></asp:ImageButton>
                </ItemTemplate>
            </asp:TemplateColumn>
          <asp:BoundColumn  DataField="OreFormazioneSpecifica"  HeaderText="Ore Formazione Specifica" Visible="false" runat="server" > </asp:BoundColumn>
   		    <asp:BoundColumn DataField="iban" HeaderText="iban" Visible="False">
            </asp:BoundColumn>
            <asp:BoundColumn DataField="tipodocumento" HeaderText="tipodocumento" 
                Visible="False"></asp:BoundColumn>
                <asp:BoundColumn  DataField="OreFormazione"  HeaderText="Ore Formazione" Visible="false" runat="server" > </asp:BoundColumn>
   		    <asp:BoundColumn DataField="dapagare" HeaderText="Da Pagare"></asp:BoundColumn>
   		</Columns>
		<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
     </div>
    </fieldset>
    
    <p></p>
    </fieldset>
</div>
</asp:Content>

