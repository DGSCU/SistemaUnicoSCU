<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmConsultaDomandeOnline.aspx.vb" Inherits="Futuro.ConsultaDomandeOnline" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
        <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css"/>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <fieldset class="ContornoPagina">
 
<legend >Ricerca Domande Online</legend>
<div class="wrapper" style="width:100%">
    <div class="headers">
       <h2>
      <asp:Label ID="Label5" runat="server"  Text="RICERCA DOMANDE ONLINE"></asp:Label>
      </h2>
 </div>
    <div class="RigaVuota">
       <asp:label id="LblErrore"  runat="server" CssClass="msgErrore" ></asp:label>
      &nbsp;</div>
       <div class="row" >
         <div class="collable" style="width:15%">
         <asp:Label ID="lblBando" CssClass="label" AssociatedControlID="ddlBandoOnLine"   runat="server"
            Text="Bando:"></asp:Label>
        </div>
         <div class="colOggetti" style="width:35%">
          <asp:DropDownList ID="ddlBandoOnLine"  CssClass="ddlClass" runat="server"></asp:DropDownList>
         </div>
       </div>

      <div class="row" id="divente" runat="server">
        <div class="collable" style="width:15%">
          
        <asp:Label ID="lblCodiceEnte" CssClass="label" AssociatedControlID="txtcodiceente"   runat="server"
            Text="Codice Ente:"></asp:Label>
        
        </div>
        <div class="colOggetti" style="width:35%">
         
       
      <asp:TextBox ID="txtcodiceente" autofocus="true" CssClass="textbox" 
           runat="server" ></asp:TextBox>
       
        </div>
        <div class="collable" style="width:15%">
          
        <asp:Label ID="lblDenominazioneEnte" CssClass="label" AssociatedControlID="txtDenominazioneEnte"   runat="server"
            Text="Denominazione Ente:"></asp:Label>
        
        </div>
        <div class="colOggetti" style="width:35%">
         
       
      <asp:TextBox ID="txtDenominazioneEnte" autofocus="true" CssClass="textbox" 
           runat="server" ></asp:TextBox>
       
        </div>
      </div>
    <div class="row" >
        <div class="collable" style="width:15%">
         
        <asp:Label ID="lblProgetto" CssClass="label" AssociatedControlID="txtCodiceProgetto" runat="server"
            Text="Codice Progetto:"></asp:Label>
       
        </div>
        <div class="colOggetti" style="width:35%">
         
             <asp:TextBox ID="txtCodiceProgetto" autofocus="true" CssClass="textbox" 
           runat="server" ></asp:TextBox>
      
        </div>
        <div class="collable" style="width:15%">
         
        <asp:Label ID="lblTitoloProgetto" CssClass="label" AssociatedControlID="txtTitoloProgetto" runat="server"
            Text="Titolo Progetto:"></asp:Label>
       
        </div>
        <div class="colOggetti" style="width:35%">
         
             <asp:TextBox ID="txtTitoloProgetto" autofocus="true" CssClass="textbox" 
           runat="server" ></asp:TextBox>
      
        </div>
    </div>
       <div class="row" >
        <div class="collable" style="width:15%">
          
        <asp:Label ID="lblCodiceSede" CssClass="label" AssociatedControlID="txtcodicesede"   runat="server"
            Text="Codice Sede:"></asp:Label>
        
        </div>
        <div class="colOggetti" style="width:35%">
         
       
      <asp:TextBox ID="txtcodicesede" onkeypress="javascript: SoloNumeri();" autofocus="true" CssClass="textbox" 
           runat="server" ></asp:TextBox>
       
        </div>
        <div class="collable" style="width:15%">
          
        <asp:Label ID="lblDenominazioneSede" CssClass="label" AssociatedControlID="txtDenominazioneSede"   runat="server"
            Text="Denominazione Sede:"></asp:Label>
        
        </div>
        <div class="colOggetti" style="width:35%">
         
       
      <asp:TextBox ID="txtDenominazioneSede" autofocus="true" CssClass="textbox" 
           runat="server" ></asp:TextBox>
       
        </div>
    </div>
     <div class="row" >
        <div class="collable" style="width:15%">
          
        <asp:Label ID="lblCognomeVolontario" CssClass="label" AssociatedControlID="txtCognomeVolontario" runat="server"
            Text="Cognome Volontario:"></asp:Label>
        
        
        </div>
        <div class="colOggetti" style="width:35%">
         
        <asp:TextBox ID="txtCognomeVolontario" autofocus="true" CssClass="textbox" 
           runat="server" ></asp:TextBox>
      
       
        </div>
        <div class="collable" style="width:15%">
         
        <asp:Label ID="lblNomeVolontario" CssClass="label" AssociatedControlID="txtNomeVolontario"   runat="server"
            Text="Nome Volontario:"></asp:Label>
       
        </div>
        <div class="colOggetti" style="width:35%">
        
        <asp:TextBox ID="txtNomeVolontario" autofocus="true" CssClass="textbox" 
           runat="server" ></asp:TextBox> 
        
      
        </div>
    </div>

    <div class="row">
     <div class="collable" style="width:15%">
         
        <asp:Label ID="lblCodicefiscaleVolontario" CssClass="label" AssociatedControlID="txtCodicefiscaleVolontario" runat="server"
            Text="Codice Fiscale Volontario:"></asp:Label>
       
        </div>
     <div class="colOggetti" style="width:35%">
         
             <asp:TextBox ID="txtCodiceFiscaleVolontario" autofocus="true" CssClass="textbox" 
           runat="server" ></asp:TextBox>
      
        </div>
     <div class="collable" style="width:15%">
         
        <asp:Label ID="lblCodiceVolontario" CssClass="label" AssociatedControlID="txtCodiceVolontario" runat="server"
            Text="Codice Volontario:"></asp:Label>
       
        </div>
     <div class="colOggetti" style="width:35%">
         
             <asp:TextBox ID="txtCodiceVolontario" autofocus="true" CssClass="textbox" 
           runat="server" ></asp:TextBox>
      
        </div>
    </div>

     <div class="row">
     <div class="collable" style="width:15%">
      <asp:Label ID="lblincorso" CssClass="label" AssociatedControlID="ddlIncorso" runat="server"
            Text="In Corso"></asp:Label>
     </div>
     <div class="colOggetti" style="width:35%">
      <asp:DropDownList ID="ddlIncorso" runat="server">
								    <asp:ListItem Value="0">SI</asp:ListItem>
								    <asp:ListItem Value="1">NO</asp:ListItem>
                    </asp:DropDownList>
     </div>
     </div>
    <p>
    &nbsp;</p>

<h3>
<asp:label id="lblmessaggioRicerca"  runat="server" AssociatedControlID="dtgRisultatoRicerca"></asp:label>
</h3>
<div class="RigaVuota">
    
&nbsp;

</div>

 <div class="RigaPulsanti" style="text-align:right">
        <asp:Button
          ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;<asp:Button 
            ID="cmdChiudi" CssClass="Pulsante" runat="server"
              Text="Chiudi"  />
  </div>
 <br/>
 <div class="RigaVuotaPrint" >      
         <asp:Button ID="CmdEsportaCSV" runat="server" CssClass="Pulsante" 
            Text="Esporta CSV" Visible="false" 
              ToolTip="Esporta elenco risultato ricerca" />
            <br />
            <asp:HyperLink ID="ApriCSV1"   AccessKey="S" 
             ToolTip="Link per l'esportazione delle domande online in corso" CssClass="linkStampa" 
                Text="CSV Domande On Line" runat="server" 
                ForeColor="#003399" Visible="False"></asp:HyperLink>
                <p></p>
                <asp:HyperLink ID="ApriCSV3"   AccessKey="S" 
             ToolTip="Link per l'esportazione delle domande online terminate" CssClass="linkStampa" 
                Text=" CSV Domande On Line" runat="server" 
                ForeColor="#003399" Visible="False"></asp:HyperLink>
                 <p></p>
                <asp:HyperLink ID="ApriCSV2"   AccessKey="S" ToolTip="Link per l'esportazione del file per Graduatorie da caricare " CssClass="linkStampa" 
                Text=" CSV per importazione Graduatorie" runat="server" 
                ForeColor="#003399" Visible="False"></asp:HyperLink>
               
                
         </div>
        

<div class="RigaPulsanti">
								</div>
<asp:datagrid id="dtgRisultatoRicerca" runat="server" Width="100%"  ToolTip="Elenco Volontari domande OnLine" 
								 AllowPaging="True" CellPadding="2" Font-Size="Small"  CssClass="table" 
        AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
								<FooterStyle></FooterStyle>
								<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
								<EditItemStyle></EditItemStyle>
								<AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
								<ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
								<HeaderStyle></HeaderStyle>
								<Columns>
									<asp:TemplateColumn Visible="false" HeaderText="Selez."> 
                                    <ItemTemplate >
											<asp:ImageButton ID="IdImgSelVolontari"   style="cursor:pointer;" CommandName="seleziona" AlternateText="Seleziona Volontario" ToolTip='Seleziona Volontario' runat="server" ImageURL="images/Icona_Volontario_small.png" CausesValidation="false"></asp:ImageButton>      
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

                                    <asp:BoundColumn Visible="True" DataField="codicefiscale" HeaderText="Codice Fiscale">
                                    <ItemStyle ></ItemStyle>
                                    </asp:BoundColumn>

                                    <asp:BoundColumn Visible="True" DataField="telefono" HeaderText="Telefono">
                                    <ItemStyle ></ItemStyle>
                                    </asp:BoundColumn>

                                    <asp:BoundColumn Visible="True" DataField="email" HeaderText="Email">
                                    <ItemStyle ></ItemStyle>
                                    </asp:BoundColumn>

                                    <asp:BoundColumn DataField="codiceente" Visible="false" HeaderText="Codice Ente">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle ></ItemStyle>
									</asp:BoundColumn>

									<asp:BoundColumn DataField="Ente"  Visible="false" HeaderText="Ente">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle ></ItemStyle>
									</asp:BoundColumn>
                                  
									<asp:BoundColumn DataField="Progetto" HeaderText="Progetto">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle ></ItemStyle>
									</asp:BoundColumn>

								

									<asp:BoundColumn Visible="True" DataField="sede" HeaderText="Sede">
                                    <ItemStyle ></ItemStyle>
                                    </asp:BoundColumn>

                                   

									<asp:BoundColumn Visible="False" DataField="IdEnte" HeaderText="IdEnte">
                                    <ItemStyle ></ItemStyle>
                                    </asp:BoundColumn>
									
                                    <asp:TemplateColumn HeaderText="Pdf Domanda Online"> 
                                        <ItemTemplate  >
											<asp:ImageButton ID="ImgPdf"   style="cursor:pointer;"   CommandName="selezionaPdf" Height="35px" Width="35px" AlternateText="Scarica Pdf" ToolTip="Scarica PDF" runat="server" ImageURL="images/pdf.png" CausesValidation="false"></asp:ImageButton>      
                                            <asp:HyperLink ID="hlDownload1"  runat="server"  Visible="false"></asp:HyperLink>
                                            </ItemTemplate>
                                            </asp:TemplateColumn>
                                    
									<asp:BoundColumn Visible="False" DataField="pdf" HeaderText="pdf">
                                    <ItemStyle ></ItemStyle>
                                    </asp:BoundColumn>
                               
								</Columns>
								<PagerStyle NextPageText="&gt;&gt;"   
									PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
							</asp:datagrid>

<div class="RigaVuota">
 <asp:HyperLink ID="hlDownload"  runat="server"  Visible="false"></asp:HyperLink>
&nbsp;</div>



</div>
</fieldset>
</asp:Content>
