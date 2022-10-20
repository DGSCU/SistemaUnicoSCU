<%@ Page Title="Gestione Sostituzione Volontari" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmGestioneSostituisciVolontari.aspx.vb" Inherits="Futuro.WfrmGestioneSostituisciVolontari" %>
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
               var DataAvvio = CostruisciId('txtdataAvvio');
               var readonly = '<%=txtdataAvvio.ReadOnly%>';
               if (readonly == "False") {
                   var sharpDataAvvio = "#" + DataAvvio
                   $("" + sharpDataAvvio + "").datepicker();
               }
           });


	</script>
	
		
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <fieldset class="ContornoPagina">
    <legend >Gestione Sostituzione Volonario</legend>
<div class="wrapper" style="width:100%">
    <div class="rowGroup" style="height:auto">
       <div class="row" style="height:auto">
     <asp:label id="LblInfo"  runat="server" CssClass="msgInfo" ></asp:label>
     </div>
     <div class="row" style="height:auto">
     <asp:label id="lblMessaggi"  runat="server" CssClass="msgErrore" ></asp:label>
     </div>
     <div class="row" style="height:auto">
     <asp:label id="LblConfermaOperazione"  runat="server" CssClass="msgConferma" ></asp:label>
     </div>
  
     </div>
      <div class="RigaVuota">&nbsp;
    </div>
    <div class="row" style="height:auto">
    <div class="colHyperLink" style="width:100%;"  >
        <asp:LinkButton  ID="ImgDocumentazione"   style="cursor:pointer" Text="Elenco documentazione sostituzione volontario" ToolTip="Documentazione"  runat="server" />
        </div>

    <div class="colHyperLink" style="width:100%">
        <asp:LinkButton ID="imgVolontari" style="cursor:pointer" Text="Modifica dati chiusura volontario" ToolTip="Visualizza Volontario"  runat="server" />
    </div>
    <div class="colHyperLink" style="width:100%">
        <asp:LinkButton ID="CmdModChiVol"  style="cursor:pointer;" Text="Modifica tipologia chiusura volontario" ToolTip="Modifica Chiusura"  runat="server" />
    </div>
</div>



         <div class="row">
		   
            <input type="hidden" id="txtCodiceVolontarioSubentrante" runat="server" />
            <input type="hidden" id="txtCodiceVolontarioVecchio" runat="server" />
            <input type="hidden" id="txtVolSubentarata" runat="server" />
            <input type="hidden" id="txtidattivitasedeassegnazione" runat="server" />
            <input type="hidden" id="txtIdentitaVecchia" runat="server" />
            <input type="hidden" id="txtidentitaSbentrante" runat="server" />
            <input type="hidden" id="txtidattivita" runat="server" />
            <input type="hidden" id="txtdatalimite" runat="server" />
            <input type="hidden" id="txtCodiceFiscale" runat="server" />
		</div>
        <div class="row">
           <div class="collable" style="width:10%">
                <asp:label id="lblMsgStato"  AssociatedControlID="lblStato" runat="server" CssClass="label" Text="Stato Volontario"></asp:label>
            </div>
             <div class="colOggetti" style="width:23%">
                 <asp:label id="lblStato"  runat="server" CssClass="labelDati"></asp:label>
            </div>
            <div class="collable" style="width:10%">
                <asp:label id="lblDataChiusuraEV"  AssociatedControlID="txtdatachiusuraEV" runat="server" CssClass="label"></asp:label>
            </div>
             <div class="colOggetti" style="width:23%">
                <asp:textbox id="txtdatachiusuraEV" runat="server" ReadOnly="True" Visible="false" BorderStyle="None" CssClass="textbox"></asp:textbox>
            </div>
                
   
		</div>

        <div class="row">
           <div class="collable" style="width:10%">
                <asp:label id="LabellblCognome"  AssociatedControlID="lblCognome" runat="server" CssClass="label" Text="Cognome"></asp:label>
            </div>
             <div class="colOggetti" style="width:23%">
                 <asp:label id="lblCognome"  runat="server" CssClass="labelDati"></asp:label>
            </div>
            <div class="collable" style="width:10%">
                <asp:label id="LabellblNome"  AssociatedControlID="lblNome" runat="server" CssClass="label" Text="Nome"></asp:label>
            </div>
             <div class="colOggetti" style="width:23%">
                  <asp:label id="lblNome"  runat="server" CssClass="labelDati"></asp:label>
            </div>
                 <div class="collable" style="width:10%">
                <asp:label id="LabellbldataNascita"  AssociatedControlID="lbldataNascita" runat="server" CssClass="label" Text="Data Nascita"></asp:label>
            </div>
             <div class="colOggetti" style="width:23%">
                  <asp:label id="lbldataNascita"  runat="server" CssClass="labelDati"></asp:label>
            </div>   
   
		</div>
       <div class="row">
           <div class="collable" style="width:10%">
                <asp:label id="LabellblComuneNascita"  AssociatedControlID="lblComuneNascita" Text="Comune Nascita" runat="server" CssClass="label" ></asp:label>
            </div>
             <div class="colOggetti" style="width:23%">
                 <asp:label id="lblComuneNascita"  runat="server" CssClass="labelDati"></asp:label>
            </div>
            <div class="collable" style="width:10%">
                <asp:label id="LabellblComuneResidenza"  AssociatedControlID="lblComuneResidenza" Text="Comune Residenza" runat="server" CssClass="label" ></asp:label>
            </div>
             <div class="colOggetti" style="width:23%">
                  <asp:label id="lblComuneResidenza"  runat="server" CssClass="labelDati"></asp:label>
            </div>
                 <div class="collable" style="width:10%">
                <asp:label id="LabellblCodFis"  AssociatedControlID="lblCodFis" Text="Cod. Fiscale" runat="server" CssClass="label" ></asp:label>
            </div>
             <div class="colOggetti" style="width:23%">
                  <asp:label id="lblCodFis"  runat="server" CssClass="labelDati"></asp:label>
            </div>   
   
		</div>
    <div class="row">
           <div class="collable" style="width:10%">
                <asp:label id="LabelLblDataInizioServizio"  AssociatedControlID="LblDataInizioServizio" Text="Data Inizio Servizio" runat="server" CssClass="label" ></asp:label>
            </div>
             <div class="colOggetti" style="width:23%">
                 <asp:label id="LblDataInizioServizio"  runat="server" CssClass="labelDati"></asp:label>
            </div>
            <div class="collable" style="width:10%">
                <asp:label id="LabelLblDataFineServizio"  AssociatedControlID="LblDataFineServizio" Text="Data Fine Servizio" runat="server" CssClass="label" ></asp:label>
            </div>
             <div class="colOggetti" style="width:23%">
                  <asp:label id="LblDataFineServizio"  runat="server" CssClass="labelDati"></asp:label>
            </div>

             <div class="colOggetti" style="width:23%">
                  <asp:label id="LblAmmessoRecupero" Visible="false"  runat="server" CssClass="labelDati"></asp:label>
            </div>   
   
		</div>
        <div class="row">
           <div class="collable" style="width:10%">
                <asp:label id="LabellblPreogetto"  AssociatedControlID="lblPreogetto" Text="Progetto" runat="server" CssClass="label" ></asp:label>
            </div>
             <div class="colOggetti" style="width:23%">
                 <asp:label id="lblPreogetto"  runat="server" CssClass="labelDati"></asp:label>
            </div>
            <div class="collable" style="width:10%">
                <asp:label id="LabellblDataInizio"  AssociatedControlID="lblDataInizio" Text="Data Inizio Progetto" runat="server" CssClass="label" ></asp:label>
            </div>
             <div class="colOggetti" style="width:23%">
                  <asp:label id="lblDataInizio"  runat="server" CssClass="labelDati"></asp:label>
            </div>
                 <div class="collable" style="width:10%">
                <asp:label id="LabellbldataFine"  AssociatedControlID="lbldataFine" Text="Data Fine Progetto" runat="server" CssClass="label" ></asp:label>
            </div>
             <div class="colOggetti" style="width:23%">
                  <asp:label id="lbldataFine"  runat="server" CssClass="labelDati"></asp:label>
            </div>   
   
		</div>
</div>
 <div class="RigaVuota">&nbsp;
 </div>
<div class="wrapper" style="width:100%;border:none">
 <h4>
<asp:label id="lblRisultato" CssClass="msgInfo" runat="server" AssociatedControlID="dgRisultatoRicercaSedi" Text="Sedi Progetto"></asp:label>
</h4>
<div class="RigaVuota">&nbsp;
 </div>
<asp:datagrid id="dgRisultatoRicercaSedi" runat="server" Width="100%" ToolTip="Elenco Sedi" 
		    AllowPaging="false" CellPadding="2"  CssClass="table" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle  BackColor="White" Font-Bold="true" ></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
	    <HeaderStyle></HeaderStyle>
		    <Columns>
                   <asp:TemplateColumn Visible="false"> 
              <ItemTemplate >
				<asp:ImageButton ID="ImgSelezionaEnte"   STYLE="cursor:hand;width:20px" 
                    alt="Ente"  runat="server"  title="Ente" ToolTip="Ente" ImageURL="Images/ente.png"  CausesValidation="false">
                  </asp:ImageButton>                            
                </ItemTemplate>
                </asp:TemplateColumn>
			    <asp:BoundColumn DataField="sedefisica" HeaderText="Sede Fisica">
				      <HeaderStyle ></HeaderStyle>
				    <ItemStyle ></ItemStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn DataField="sedeAttuazione" HeaderText="Sede Att.">
				     <HeaderStyle ></HeaderStyle>
				    <ItemStyle ></ItemStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn DataField="indirizzo" HeaderText="Indirizzo">
				     <HeaderStyle ></HeaderStyle>
				    <ItemStyle ></ItemStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn DataField="comune" HeaderText="Comune ">
				      <HeaderStyle ></HeaderStyle>
				    <ItemStyle ></ItemStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn Visible="False" HeaderText="N&#176; Vol. Richiesti">
				    <HeaderStyle ></HeaderStyle>
				    <ItemStyle ></ItemStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn Visible="False" HeaderText="N&#176; Vol. Assegnati">
				     <HeaderStyle ></HeaderStyle>
				    <ItemStyle ></ItemStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn Visible="False" DataField="identit&#224;" HeaderText="identit&#224;"></asp:BoundColumn>
			    <asp:BoundColumn Visible="False" DataField="idattivit&#224;entesedeattuazione" HeaderText="idattivit&#224;entesedeattuazione"></asp:BoundColumn>
			    <asp:BoundColumn Visible="False" DataField="datafineattivit&#224;entit&#224;" HeaderText="datafineattivit&#224;entit&#224;"
				    DataFormatString="{0:d}"></asp:BoundColumn>
			    <asp:BoundColumn Visible="False" DataField="statodefault" HeaderText="statodefault"></asp:BoundColumn>
			    <asp:BoundColumn Visible="False" DataField="note" HeaderText="note"></asp:BoundColumn>
			    <asp:BoundColumn Visible="False" DataField="percentualeutilizzo" HeaderText="percentualeutilizzo"></asp:BoundColumn>
			    <asp:BoundColumn Visible="False" DataField="idtipologiaposto" HeaderText="idtipologiaposto"></asp:BoundColumn>
			    <asp:BoundColumn Visible="False" DataField="identeSede" HeaderText="identeSede"></asp:BoundColumn>
		    </Columns>
</asp:datagrid>
 <h4>
<asp:label id="Label1" CssClass="msgInfo" runat="server" AssociatedControlID="dgRisultatoRicercaVolontari" Text="Elenco Volontari"></asp:label>
</h4>
<div class="RigaVuota">&nbsp;
 </div>
 <div class="row" style="height:auto">
 <div class="collable" style="width:100%">
 <asp:checkbox id="chkStessaSede" runat="server" Visible="False" Font-Names="verdana" Font-Bold="True"
												ForeColor="Navy" Font-Size="Small" Text="Visualizza solo Volontari stessa Sede" AutoPostBack="True">
                                                </asp:checkbox>
</div>
</div>
 <div class="RigaVuota">&nbsp;
 </div>
<asp:datagrid id="dgRisultatoRicercaVolontari" runat="server" style="width:100%" ToolTip="Elenco Volontari" 
		    AllowPaging="true" PageSize="5" CellPadding="2"  CssClass="table" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle  BackColor="White" Font-Bold="true" ></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
	    <HeaderStyle></HeaderStyle>
		    <Columns>
                <asp:TemplateColumn > 
              <ItemTemplate >
				<asp:ImageButton ID="ImgSelezionaVolontario"  CommandName="Select" STYLE="cursor:hand" 
                    AlternateText="Seleziona Volontario"  runat="server" title="Seleziona Volontario"  ImageURL="Images/valida_small.png"  CausesValidation="false">
                  </asp:ImageButton>                            
                </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn DataField="nominativo" HeaderText="Nominativo">
					 <HeaderStyle ></HeaderStyle>
				    <ItemStyle ></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Codicefiscale" HeaderText="Cod.Fis.">
					 <HeaderStyle ></HeaderStyle>
				    <ItemStyle ></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="datanascita" HeaderText="data di Nascita" DataFormatString="{0:d}">
					 <HeaderStyle ></HeaderStyle>
				    <ItemStyle ></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="comunenascita" HeaderText="Comune Nascita">
					 <HeaderStyle ></HeaderStyle>
				    <ItemStyle ></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" HeaderText="Comune Residenza">
					 <HeaderStyle ></HeaderStyle>
				    <ItemStyle ></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="punteggio" HeaderText="Punteggio">
					 <HeaderStyle ></HeaderStyle>
				    <ItemStyle ></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="sedeassegnazione" HeaderText="Sede Ass.">
					 <HeaderStyle ></HeaderStyle>
				    <ItemStyle ></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="identit&#224;" HeaderText="identit&#224;"></asp:BoundColumn>
                <asp:BoundColumn HeaderText="Info Anomalie" >
				    <HeaderStyle ></HeaderStyle>
					<ItemStyle Width="5%"></ItemStyle>
				</asp:BoundColumn>
			</Columns>
	    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center"
             CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>


</asp:datagrid>
<div id="rigaLegenda" runat="server" class="rowGroup" visible="false" >

<div class="row">
<div class="collable">
<asp:Label ID="lnlLbgenda" Text="Legenda:" runat="server" CssClass="labelDati"></asp:Label>
</div>
</div>
<div class="row">
        <div class="collable" style="width:15%">         
       <img  alt="Da Verificare" src="images/DataDaVerificare.jpg"  style="border:0px;width:30px;height:30px" /><asp:Label 
                ID="Label11" runat="server" Text="&nbsp;Da verificare" CssClass="label" style="vertical-align:super"></asp:Label>
        </div>
        <div class="collable" style="width:15%">
       <img  src="images/DoppiaDomanda.jpg" alt="Anomalia" style="border:0px;width:30px;height:30px"/><asp:Label ID="Label12" 
                runat="server" Text="&nbsp;Anomalia" CssClass="label" style="vertical-align:super"></asp:Label>
        </div>
   </div>
    </div>

<asp:datagrid id="dtgElencoVolontariAltroProgetto" runat="server" Width="100%" ToolTip="Volontario altra sede" 
		    AllowPaging="false"  CellPadding="2"  CssClass="table" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle  BackColor="White" Font-Bold="true" ></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
	    <HeaderStyle></HeaderStyle>
		<Columns>
               <asp:TemplateColumn > 
                <ItemTemplate >
				    <asp:ImageButton ID="ImgSelezionaVolontario"  CommandName="Select" STYLE="cursor:hand" 
                        AlternateText="Seleziona Volontario"  runat="server" title="Seleziona Volontario" 
                        ImageURL="Images/valida_small.png"  CausesValidation="false">
                    </asp:ImageButton>                            
                </ItemTemplate>
            </asp:TemplateColumn>
			<asp:BoundColumn DataField="nominativo" HeaderText="Nominativo">
				<HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Codicefiscale" HeaderText="Cod.Fis.">
				<HeaderStyle HorizontalAlign="Center" Width="16%" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="datanascita" HeaderText="data di Nascita" DataFormatString="{0:d}">
				<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="comunenascita" HeaderText="Comune Nascita">
				<HeaderStyle HorizontalAlign="Center" Width="20%" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn Visible="False" HeaderText="Comune Residenza">
				<HeaderStyle HorizontalAlign="Center" Width="8%" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="punteggio" HeaderText="Punteggio">
				<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="sedeassegnazione" HeaderText="Sede Ass.">
				<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="identit&#224;" HeaderText="identit&#224;"></asp:BoundColumn>
		</Columns>
	    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center"
             CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>


</asp:datagrid>
 <div class="RigaVuota">&nbsp;
 </div>
        <div class="row">

            <div class="collable" style="width:17%">
                <asp:label id="lbltxtdataAvvio"  AssociatedControlID="txtdataAvvio" runat="server" CssClass="label" Text="Data di avvio al servizio (gg/mm/aaaa)"></asp:label>
            </div>
             <div class="collable" style="width:35%">
                <asp:textbox id="txtdataAvvio" runat="server" Width="75px"  CssClass="textbox"></asp:textbox>
            </div>
       </div>
        <div class="row">
             <div class="collable" style="width:50%">
                <asp:label id="lblTesto"  AssociatedControlID="ImageButton1" runat="server" CssClass="label" Text="Volontari di Altri Progetti disponibili al Subentro"></asp:label>
                <asp:imagebutton id="ImageButton1" AlternateText="Volontari di Altri Progetti" ToolTip="Volontari di Altri Progetti" style="CURSOR: hand;" runat="server" ImageUrl="images/Icona_Progetto_medium.png"></asp:imagebutton>
            </div>
		</div>

        <div class="RigaPulsanti">
            <asp:Button ID="cmdSalva" runat="server" CssClass="Pulsante" Text="Salva" />
            <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server"    Text="Chiudi"  />
            <asp:Button ID="ImgAnnullaChiusura" runat="server" CssClass="Pulsante"  Text="Annulla Chiusura"  /> &nbsp;
        </div>
   </div>
</fieldset>




</asp:Content>
