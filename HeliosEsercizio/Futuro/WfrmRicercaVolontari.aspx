<%@ Page Title="Ricerca Volontari" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaVolontari.aspx.vb" Inherits="Futuro.WfrmRicercaVolontari" %>
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
               var IdData= CostruisciId('txtdatanascita');
               var sharpIdData = "#" + IdData
               $("" + sharpIdData + "").datepicker();
           });

           $(function () {
               var IdData = CostruisciId('txtDataInizServ');
               var sharpIdData = "#" + IdData
               $("" + sharpIdData + "").datepicker();
           });

           $(function () {
               var IdData = CostruisciId('txtalladata');
               var sharpIdData = "#" + IdData
               $("" + sharpIdData + "").datepicker();
           });



	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="firefox">
<fieldset class="ContornoPaginaScroll" >
<legend >Ricerca Volontari</legend>
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Volontari"></asp:Label>
      </h2>
      </div>
      <div class="rowGroup" style="height:auto">
        <div class="RigaVuota">
      &nbsp;</div>
         <asp:label id="LblErrore"  runat="server" CssClass="msgErrore" ></asp:label>
         <asp:HiddenField  ID="WindowSize" runat="server"    Value=""/>
        <asp:HiddenField  ID="IdVolontario" runat="server"    Value=""/>
        <div class="RigaVuota">
      &nbsp;</div>
    </div>
    <fieldset class="ContornoPagina" runat="server" id="filtripaghe">
            <legend>Filtri Paghe</legend>
            <div class="wrapper" style="width:100%;border:0px">
               
            <div class="row">       
                <div class="collable"  style="width:14%">
                    <asp:Label ID="Label1" AssociatedControlID="CboPagamentiHelios" CssClass="label"  runat="server" Text="Pagamenti Helios"></asp:Label>
                </div>
                <div class="colOggetti" style="width:34%">
                    <asp:DropDownList ID="CboPagamentiHelios" runat="server">
                     <asp:ListItem Value="0">TUTTI</asp:ListItem>
								    <asp:ListItem Value="1">SI</asp:ListItem>
								    <asp:ListItem Value="2">NO</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="collable"  style="width:14%">
                    <asp:Label ID="Label2" AssociatedControlID="CboDebitori" CssClass="label"  runat="server" Text="Debitore"></asp:Label>
                </div>
                <div class="colOggetti" style="width:34%">
                    <asp:DropDownList ID="CboDebitori" runat="server">
                     <asp:ListItem Value="0">TUTTI</asp:ListItem>
								    <asp:ListItem Value="1">SI</asp:ListItem>
								    <asp:ListItem Value="2">NO</asp:ListItem>
                    </asp:DropDownList>
                </div>
                      
            </div>

            <div class="row">
                <div class="collable"  style="width:14%">
                    <asp:Label ID="lblVolSospeso" AssociatedControlID="CboVolSospeso" CssClass="label"  runat="server" Text="Volontario Sospeso"></asp:Label>
                </div>
                <div class="colOggetti" style="width:34%">
                    <asp:DropDownList ID="CboVolSospeso" runat="server">
                     <asp:ListItem Value="0">TUTTI</asp:ListItem>
			    <asp:ListItem Value="1">SI</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="collable"  style="width:14%">
                    <asp:Label ID="lblPrgSospeso" AssociatedControlID="CboPrgSospeso" CssClass="label"  runat="server" Text="Progetto Sospeso"></asp:Label>
                </div>
                <div class="colOggetti" style="width:34%">
                    <asp:DropDownList ID="CboPrgSospeso" runat="server">
                     <asp:ListItem Value="0">TUTTI</asp:ListItem>
			    <asp:ListItem Value="1">SI</asp:ListItem>
                    </asp:DropDownList>
                </div>                      
             </div>
             </div>
           
            </fieldset>
    <div class="rowGroup" id="DivCampiRicercaEnte" runat="server" visible="false">
        <div class="row" >
            <div class="collable"  style="width:15%">
                <asp:Label ID="lblDescEnte" AssociatedControlID="txtDescEnte" CssClass="label"  runat="server" Text="Denominazione Ente"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtDescEnte" CssClass="textbox" autofocus="true" runat="server"></asp:TextBox>
            </div>
            <div class="collable"  style="width:15%">
                <asp:Label ID="lblBando" AssociatedControlID="txtBando" CssClass="label"  runat="server" Text="Circolare"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtBando" CssClass="textbox" autofocus="true" runat="server"></asp:TextBox>
            </div>
        </div>
        
               
       <div class="row" >     
           <div class="collable" style="width:15%">
               <asp:Label ID="lblCodEnte" AssociatedControlID="txtCodEnte"   runat="server" Text="Cod. Ente"></asp:Label>
           </div>
           <div class="colOggetti" style="width:35%">
                  <asp:TextBox ID="txtCodEnte" CssClass="textbox"  runat="server"></asp:TextBox>
           </div>
              <div class="collable" style="width:15%">
               <asp:Label ID="LblCodVolontario" AssociatedControlID="txtCodVolontario"  runat="server" Text="Cod. Volontario"></asp:Label>
           </div>
           <div class="colOggetti" style="width:35%">
                  <asp:TextBox ID="txtCodVolontario" CssClass="textbox"   runat="server"></asp:TextBox>
           </div>
        </div>
    </div>

    <div class="row" >
       <div class="collable" style="width:15%">      
           <asp:Label ID="IdLblCognome" AssociatedControlID="txtCognome" runat="server" Text="Cognome"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">       
           <asp:TextBox ID="txtCognome" CssClass="textbox" runat="server"></asp:TextBox>         
       </div>
       <div class="collable" style="width:15%">
            <asp:Label ID="IdLblNome" AssociatedControlID="txtNome" runat="server" Text="Nome"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:TextBox ID="txtNome" CssClass="textbox" runat="server"></asp:TextBox>  
       </div>
    </div>

    <div class="row">
    <div class="collable" style="width:15%">
            <asp:Label ID="LblCodFiscale" AssociatedControlID="txtCodFiscale" runat="server" Text="Cod. Fiscale"></asp:Label>
       </div>
            <div class="colOggetti" style="width:35%"> 
             <asp:TextBox ID="txtCodFiscale" CssClass="textbox" runat="server"></asp:TextBox>  
       </div>
       <div class="collable" style="width:15%">
            <asp:Label ID="LblStatoVerificaCF" AssociatedControlID="cboStatoVerificaCF" runat="server" Text="Verifica Cod. Fiscale"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
            <asp:DropDownList ID="cboStatoVerificaCF"  CssClass="ddlClass" runat="server"></asp:DropDownList>
       </div>         
    </div>
   
     <div class="row" >
        <div class="collable" style="width:15%">
            <asp:Label ID="LblDataNascita" AssociatedControlID="txtdatanascita"  runat="server" Text="Data di Nascita (gg/mm/aaaa)"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
             <asp:TextBox ID="txtdatanascita" CssClass="textboxData" MaxLength="10" runat="server"></asp:TextBox>  
       </div>
       <div class="collable" style="width:15%">
            <asp:Label ID="LblSesso" AssociatedControlID="cboSesso" runat="server" Text="Sesso"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">
            <asp:DropDownList ID="cboSesso"  CssClass="ddlClass" runat="server"></asp:DropDownList>
       </div>
    
     </div>

    <div class="row" >
        <div class="collable" style="width:15%">
            <asp:Label ID="LblStato" AssociatedControlID="cboStato" runat="server" Text="Stato"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
              <asp:DropDownList ID="cboStato" CssClass="ddlClass" runat="server"></asp:DropDownList> 
       </div>
           <div class="collable" style="width:15%">
            <asp:Label ID="LblModPagamento" AssociatedControlID="CboLibPost" runat="server" Text="Modalit&#224; di pagamento"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">
            <asp:DropDownList ID="CboLibPost" CssClass="ddlClass" runat="server">
            <asp:ListItem Value="0">TUTTI</asp:ListItem>
								<asp:ListItem Value="1">Con Libretto</asp:ListItem>
								<asp:ListItem Value="2">Senza Libretto</asp:ListItem>
								<asp:ListItem Value="3">Con Iban</asp:ListItem>
								<asp:ListItem Value="4">Senza Iban</asp:ListItem>
            </asp:DropDownList>
       </div>
     
    </div>

    <div class="row" >
        <div class="collable" style="width:15%">
            <asp:Label ID="LblStatoAss" AssociatedControlID="CboStatoAss" runat="server" Text="Stato Assicurazione"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
              <asp:DropDownList ID="CboStatoAss" CssClass="ddlClass" runat="server"></asp:DropDownList> 
       </div>
           <div class="collable" style="width:15%">
            <asp:Label ID="LblCategoria" AssociatedControlID="CboCategoria" runat="server" Text="Categoria"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">
            <asp:DropDownList ID="CboCategoria" CssClass="ddlClass" runat="server">
            </asp:DropDownList>
       </div>
     
    </div>

    <div class="row" >
        <div class="collable" style="width:15%">
            <asp:Label ID="LblDataInizServ" AssociatedControlID="txtDataInizServ"  runat="server" Text="Data di Inizio Servizio (gg/mm/aaaa)"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
             <asp:TextBox ID="txtDataInizServ" CssClass="textboxData" MaxLength="10" runat="server"></asp:TextBox>  
       </div>
        <div class="collable" style="width:15%">
            <asp:Label ID="LblAlladata" AssociatedControlID="txtalladata"  runat="server" Text="In servizio alla data del (gg/mm/aaaa)"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
             <asp:TextBox ID="txtalladata" CssClass="textboxData" MaxLength="10" runat="server"></asp:TextBox>  
       </div>
    
     </div>
      <div class="row" >
       <div class="collable" style="width:15%">
            
            <asp:Label ID="lblgmo" AssociatedControlID="Cbogmo" ToolTip="Giovani con minori opportunità" runat="server" 
               Text="GMO"></asp:Label>
            
       </div>
        <div class="colOggetti" style="width:35%">    
               <asp:DropDownList ID="Cbogmo"  CssClass="ddlClass" runat="server">
            <asp:ListItem Value=""></asp:ListItem>
								<asp:ListItem Value="1">Si</asp:ListItem>
								<asp:ListItem Value="2">No</asp:ListItem>
								
            </asp:DropDownList>
       </div>
       <div class="collable" style="width:15%">
             <asp:Label ID="lblfami" AssociatedControlID="Cbofami" ToolTip="Fami" runat="server" 
               Text="FAMI"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
              <asp:DropDownList ID="Cbofami"  CssClass="ddlClass" runat="server">
            <asp:ListItem Value=""></asp:ListItem>
								<asp:ListItem Value="1">Si</asp:ListItem>
								<asp:ListItem Value="2">No</asp:ListItem>
								
            </asp:DropDownList>
       </div>
       </div>
      <div class="row" >
        <div class="collable" style="width:15%">
            <asp:Label ID="IdLblProgetto" AssociatedControlID="txtProgetto" runat="server" Text="Progetto"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
             <asp:TextBox ID="txtProgetto" CssClass="textbox" runat="server"></asp:TextBox>  
       </div>
           <div class="collable" style="width:15%">
            <asp:Label ID="IdLblCodProgetto" AssociatedControlID="txtCodProgetto" runat="server" Text="Cod. Progetto"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">
            <asp:TextBox ID="txtCodProgetto" CssClass="textbox" runat="server"></asp:TextBox>
       </div>
    </div>

    <div class="row" >
        <div class="collable" style="width:15%">
            <asp:Label ID="LblRegione" AssociatedControlID="txtRegione" runat="server" Text="Regione"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
             <asp:TextBox ID="txtRegione" CssClass="textbox" runat="server"></asp:TextBox>  
       </div>
           <div class="collable" style="width:15%">
            <asp:Label ID="LblProvincia" AssociatedControlID="txtProvincia" runat="server" Text="Provincia"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">
            <asp:TextBox ID="txtProvincia" CssClass="textbox" runat="server"></asp:TextBox>
       </div>
    </div>

    <div class="row" >
        <div class="collable" style="width:15%">
            <asp:Label ID="LblComune" AssociatedControlID="txtComune" runat="server" Text="Comune"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
             <asp:TextBox ID="txtComune" CssClass="textbox" runat="server"></asp:TextBox>  
      </div>
       <div class="collable" style="width:15%">
             <asp:Label ID="lblDurataProg" AssociatedControlID="CboDurataProg" ToolTip="Durata Progetto" runat="server" 
               Text="Durata Progetto Mesi"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
              <asp:DropDownList ID="CboDurataProg"  CssClass="ddlClass" runat="server">
            <asp:ListItem Value=""></asp:ListItem>
								<asp:ListItem Value="8">8</asp:ListItem>
								<asp:ListItem Value="9">9</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
								
            </asp:DropDownList>
       </div>
    </div>
     

       <div class="row">
       <div class="collable" style="width:15%">
            <asp:Label ID="lblEsteroUe" AssociatedControlID="CboEsteroUe" ToolTip="Estero UE" runat="server" 
               Text="Estero UE"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
              <asp:DropDownList ID="CboEsteroUe"  CssClass="ddlClass" runat="server">
            <asp:ListItem Value=""></asp:ListItem>
								<asp:ListItem Value="1">Si</asp:ListItem>
								<asp:ListItem Value="2">No</asp:ListItem>
								
            </asp:DropDownList>
       </div>
       <div class="collable" style="width:15%">
            <asp:Label ID="lblTutoraggio" AssociatedControlID="CboTutoraggio" ToolTip="Tutoraggio" runat="server" 
               Text="Tutoraggio"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
              <asp:DropDownList ID="CboTutoraggio"  CssClass="ddlClass" runat="server">
            <asp:ListItem Value=""></asp:ListItem>
								<asp:ListItem Value="1">Si</asp:ListItem>
								<asp:ListItem Value="2">No</asp:ListItem>
								
            </asp:DropDownList>
       </div>
       </div>


      


    <div class="RigaPulsanti">
        <asp:Button
          ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;<asp:Button 
            ID="cmdChiudi" CssClass="Pulsante" runat="server"
              Text="Chiudi"  />
              <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="false"/>&nbsp;
                <br />
         <asp:HyperLink ID="ApriCSV1"   AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" 
        Text="DOWNLOAD CSV" runat="server" 
        ForeColor="#003399" Visible="False"></asp:HyperLink>
   </div>
    
   
</div>



<h3>
<asp:label id="lblmessaggioRicerca"  runat="server" AssociatedControlID="dtgRisultatoRicerca"></asp:label>
</h3>
 <div class="RigaVuota">
&nbsp;

</div>

<asp:datagrid id="dtgRisultatoRicerca" runat="server" Width="100%"  ToolTip="Elenco Volontari" 
								 AllowPaging="True" CellPadding="2" Font-Size="Small"  CssClass="table" 
        AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
								<FooterStyle></FooterStyle>
								<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
								<EditItemStyle></EditItemStyle>
								<AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
								<ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
								<HeaderStyle></HeaderStyle>
								<Columns>
									<asp:TemplateColumn HeaderText="Selez."> 
                                        <ItemTemplate >
											<asp:ImageButton ID="IdImgSelVolontari"  style="cursor:pointer;" CommandName="seleziona" AlternateText="Seleziona Volontario" ToolTip='Seleziona Volontario' runat="server" ImageURL="images/Icona_Volontario_small.png" CausesValidation="false"></asp:ImageButton>      
                                            </ItemTemplate>
                                            </asp:TemplateColumn>
									<asp:BoundColumn DataField="codicevolontario"  HeaderText="Codice Volontario">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle ></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="IdAttivit&#224;" HeaderText="IdAttivit&#224;">
                                    <ItemStyle ></ItemStyle>
                                    </asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="IDEntit&#224;"  HeaderText="IDEntit&#224;">
                                    <ItemStyle></ItemStyle>
                                    </asp:BoundColumn>
									<asp:BoundColumn DataField="Nominativo" HeaderText="Nominativo">
                                    <ItemStyle></ItemStyle>
                                    </asp:BoundColumn>
									<asp:BoundColumn DataField="Ente" HeaderText="Ente">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle ></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="DataInizio" HeaderText="Data Inizio Servizio">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle ></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="DataFine" HeaderText="Data Fine Servizio">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ComProv" HeaderText="Comune Progetto">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="AltreInfo" HeaderText="Altre Informazioni">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Stato" HeaderText="Stato">
                                    <ItemStyle></ItemStyle>
                                    </asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="IdEnte" HeaderText="IdEnte">
                                    <ItemStyle ></ItemStyle>
                                    </asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="Ente1" HeaderText="Ente">
                                    <ItemStyle ></ItemStyle>
                                    </asp:BoundColumn>
									<asp:BoundColumn DataField="CLP" Visible="false" HeaderText="Libretto Postale">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle ></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="cf" HeaderText="cf">
                                    <ItemStyle ></ItemStyle>
                                    </asp:BoundColumn>
									<asp:BoundColumn DataField="iban" HeaderText="Codice Iban">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle></ItemStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="Info Anomalie" >
										<HeaderStyle ></HeaderStyle>
										<ItemStyle></ItemStyle>
									</asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Crono."> 
                                        <ItemTemplate  >
											<asp:ImageButton ID="ImgCronologia"   style="cursor:pointer;"   CommandName="selezionaCrono" AlternateText="Seleziona Cronologia" ToolTip="Seleziona Cronologia" runat="server" ImageURL="images/cronologia_small.png" CausesValidation="false"></asp:ImageButton>      
                                            </ItemTemplate>
                                            </asp:TemplateColumn>
                                    
									<asp:BoundColumn Visible="False" DataField="CodiceRegione" HeaderText="Codice Ente">
                                    <ItemStyle ></ItemStyle>
                                    </asp:BoundColumn>
                               
								</Columns>
								<PagerStyle NextPageText="&gt;&gt;"   
									PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
							</asp:datagrid>

<div class="RigaVuota">
&nbsp;

</div>
<div id="rigaLegenda" runat="server" class="rowGroup" style="width:100%" visible="false" >

<p>
    <asp:Label ID="lnlLbgenda" Text="Legenda:" runat="server" CssClass="labelDati"></asp:Label>
</p>

  <p>      
       <img  alt="Da Verificare" src="images/DataDaVerificare.jpg"  style="border:0px;width:30px;height:30px" /><asp:Label 
                ID="Label11" runat="server" Text="&nbsp;Da verificare" CssClass="label" style="vertical-align:super"></asp:Label>
        
       
       &nbsp;
        
       
       <img  src="images/DoppiaDomanda.jpg" alt="Anomalia" style="border:0px;width:30px;height:30px"/><asp:Label ID="Label12" 
                runat="server" Text="&nbsp;Anomalia" CssClass="label" style="vertical-align:super"></asp:Label>
       
  
</p> 
</div>

</fieldset>
</div>
</asp:Content>
