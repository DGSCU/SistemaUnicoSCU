<%@ Page Title="Ricerca Progetti" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaAttivitaGraduatoria.aspx.vb" Inherits="Futuro.WfrmRicercaAttivitaGraduatoria" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">


</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="firefox">
    <fieldset class="ContornoPaginaScroll">
<legend >Ricerca Progetti</legend>
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Progetti"></asp:Label>
      </h2>
 </div>
   <div class="RigaVuota" >&nbsp;
   </div> 
     <div class="row"  style="height:auto">
   <asp:label id="lblmessaggio" CssClass="msgErrore"  runat="server" ></asp:label>
  </div>
      <div class="row" >
       <div class="collable"  style="width:15%">
           <asp:Label ID="lbldenominazione" CssClass="label" AssociatedControlID="txtdenominazione" Visible="false" runat="server" Text="Ente"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:TextBox ID="txtdenominazione" autofocus="true" CssClass="textbox" Visible="false" runat="server"></asp:TextBox>
       </div>
       <div class="collable" style="width:15%">
           <asp:Label ID="lblCodEnte" CssClass="label" AssociatedControlID="txtcodiceEnte" Visible="false" runat="server" Text="Cod. Ente"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:TextBox ID="txtcodiceEnte" Visible="false" CssClass="textbox" runat="server"></asp:TextBox>
       </div>
    
    </div>
    <div class="row" >
       <div class="collable" style="width:15%">      
           <asp:Label ID="IdLblProgetto" CssClass="label" AssociatedControlID="txtTitoloProgetto" runat="server" Text="Progetto"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">       
           <asp:TextBox ID="txtTitoloProgetto" CssClass="textbox" runat="server"></asp:TextBox>         
       </div>
       <div class="collable" style="width:15%">
            <asp:Label ID="IdLblBando" CssClass="label" AssociatedControlID="cbobando" runat="server" Text="Bando"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:DropDownList ID="cbobando" runat="server" CssClass="ddlClass"  AutoPostBack="True">
              </asp:DropDownList>
       </div>
    </div>
    <div class="row" >
        <div class="collable" style="width:15%">
            <asp:Label ID="IdLblSettore" CssClass="label" AssociatedControlID="cboSettore" runat="server" Text="Settore"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
            <asp:DropDownList ID="cboSettore" runat="server" CssClass="ddlClass" AutoPostBack="True">
            </asp:DropDownList>
       </div>
           <div class="collable" style="width:15%">
            <asp:Label ID="IdLblRegione" CssClass="label" AssociatedControlID="txtRegione" runat="server" Text="Regione"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">
            <asp:TextBox ID="txtRegione" CssClass="textbox" runat="server"></asp:TextBox>
       </div>
     
    </div>
  <div class="row" >
        <div class="collable" style="width:15%">
            <asp:Label ID="IdLblProvincia" CssClass="label" AssociatedControlID="txtProvincia" runat="server" Text="Provincia"></asp:Label>
          
       </div>
        <div class="colOggetti" style="width:35%">
             <asp:TextBox ID="txtProvincia"   CssClass="textbox" runat="server"></asp:TextBox>
       </div>
             <div class="collable" style="width:15%">
            <asp:Label ID="IdLblComune" CssClass="label" AssociatedControlID="txtComune"  runat="server" Text="Comune"></asp:Label>
         
       </div>
        <div class="colOggetti" style="width:35%">
            <asp:TextBox ID="txtComune" CssClass="textbox"  runat="server"></asp:TextBox>
       </div>
    </div>
    
    <div class="row" >
 
        <div class="collable" style="width:15%">
            <asp:Label ID="IdLblCodProgetto" CssClass="label" AssociatedControlID="txtCodProgetto" runat="server" Text="Cod. Progetto"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">
             <asp:TextBox ID="txtCodProgetto" CssClass="textbox" runat="server"></asp:TextBox>
       </div>

               <div class="collable" style="width:15%">
            <asp:Label ID="lblSanzione" CssClass="label" AssociatedControlID="ddlSegnalazioneSanzione" runat="server" Text="Presenza Sanzione"></asp:Label>
         
       </div>
        <div class="colOggetti" style="width:35%">         
            <asp:DropDownList  ID="ddlSegnalazioneSanzione" CssClass="ddlClass"  Width="55px" runat="server">
            	<asp:ListItem Selected="True">Tutti</asp:ListItem>
				<asp:ListItem Value="0">No</asp:ListItem>
				<asp:ListItem Value="1">Si</asp:ListItem>
            </asp:DropDownList>
         
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

       <div class="row" >
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
            <div class="collable" style="width:15%">
                <asp:Label ID="lblDataInizioServizio" AssociatedControlID="txtAnno" runat="server" 
                Text="Inizio Richiesta (mm/aaaa)"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">  
                <asp:TextBox ID="txtAnno" CssClass="textbox"  runat="server" MaxLength="7" ></asp:TextBox>
            </div>
    </div>

    <div class="row" >
       <div class="collable" style="width:15%">      
           <asp:Label ID="IdLblProgramma" CssClass="label" AssociatedControlID="txtTitoloProgramma" runat="server" Text="Programma"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">       
           <asp:TextBox ID="txtTitoloProgramma" CssClass="textbox" runat="server"></asp:TextBox>         
       </div>
        <div class="collable" style="width:15%">
            <asp:Label ID="IdLblCodProgramma" CssClass="label" AssociatedControlID="txtCodProgramma" runat="server" Text="Cod. Programma"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">
             <asp:TextBox ID="txtCodProgramma" CssClass="textbox" runat="server"></asp:TextBox>
       </div>
    </div>
  
    <div class="row" >
        <div class="collable"  style="width:50%">
            <asp:CheckBox ID="chkAttessaGraduatoria"   runat="server" Visible="false" 
                     Text="Progetti in Attesa di Graduatoria"  />
       </div>
    </div>
    <div class="RigaPulsanti">
        <asp:Button
          ID="cmdSalva" runat="server" CssClass="Pulsante" Text="Ricerca" /><asp:Button 
            ID="cmdChiudi" CssClass="Pulsante" runat="server"
              Text="Chiudi"  />

                <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" 
                    Text="Esporta CSV" Visible="False" />
            &nbsp;<br />
        <br />


   </div>
    
   
</div>

        <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
        <br />

<asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%"  ToolTip="Risultato Ricerca Enti"   
         AllowPaging="True" CellPadding="2"  CssClass="table" PageSize="10"	AllowSorting="True" 
         AutoGenerateColumns="False" UseAccessibleHeader="True" CaptionAlign="Top" Caption="">
								<FooterStyle></FooterStyle>
								<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
								<EditItemStyle></EditItemStyle>
								<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
								<ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
								<HeaderStyle></HeaderStyle>
							    <Columns>
										<asp:TemplateColumn HeaderText="Selez."> 
                                        <ItemTemplate>
											<asp:ImageButton ID="ImageButton1" CommandName="Select" AlternateText="Seleziona Progetto" ToolTip="Seleziona Progetto" runat="server" ImageURL="images/Icona_Progetto_small.png" CausesValidation="false"></asp:ImageButton>
                                                    
                                            </ItemTemplate>
                                            </asp:TemplateColumn>
										<asp:BoundColumn Visible="False" DataField="CodiceRegione" HeaderText="Cod.Ente">
											<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn Visible="False" DataField="Ente" HeaderText="Ente">
											<HeaderStyle ></HeaderStyle>
										    <ItemStyle  ></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn Visible="False" DataField="Idente" HeaderText="idEnte">
											<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="titolo" HeaderText="Titolo">
												<HeaderStyle ></HeaderStyle>
										        <ItemStyle  ></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="bando" HeaderText="Bando">
												<HeaderStyle ></HeaderStyle>
										        <ItemStyle  ></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="ambito" HeaderText="Settore / Area Intervento">
													<HeaderStyle ></HeaderStyle>
										        <ItemStyle ></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="NumSediAtt" HeaderText="N&#176; Sedi Att.">
											<HeaderStyle ></HeaderStyle>
										    <ItemStyle  ></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="NsedidaConfrmare" HeaderText="Graduatorie da Confermare">
												<HeaderStyle ></HeaderStyle>
										        <ItemStyle  ></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="NsedidaPresentare" HeaderText="Graduatorie da Presentare">
												<HeaderStyle ></HeaderStyle>
										        <ItemStyle  ></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="NumVolRic" HeaderText="N&#176; Vol Ric">
												<HeaderStyle ></HeaderStyle>
										        <ItemStyle  ></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="NumVolIns" HeaderText="N&#176; Vol Ins">
												<HeaderStyle ></HeaderStyle>
										        <ItemStyle  ></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn Visible="False" DataField="idattivit&#224;" HeaderText="idattivit&#224;">
												<HeaderStyle ></HeaderStyle>
										        <ItemStyle  ></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="NumVolAmm" HeaderText="N&#176; Vol Sel">
												<HeaderStyle ></HeaderStyle>
										        <ItemStyle  ></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="NvolPart" HeaderText="N&#176; Vol Part"></asp:BoundColumn>
										<asp:BoundColumn DataField="NumVolAss" HeaderText="N&#176; Vol Ass">
												<HeaderStyle ></HeaderStyle>
										        <ItemStyle  ></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn Visible="False" DataField="datainvio" HeaderText="Data Invio Graduatoria" DataFormatString="{0:d}"></asp:BoundColumn>
										<asp:BoundColumn Visible="False" DataField="datainizioprevista" HeaderText="Data Inizio Richiesta"
											DataFormatString="{0:d}"></asp:BoundColumn>
										<asp:BoundColumn DataField="datainizioattivit&#224;" HeaderText="Data Inizio Progetto" DataFormatString="{0:d}"></asp:BoundColumn>
										<asp:BoundColumn Visible="False" DataField="SegnalaGraduatorieVolontari" HeaderText="SegnalaGraduatorieVolontari"></asp:BoundColumn>
										<asp:BoundColumn DataField="SegnalazioneSanzione" HeaderText="Presenza Sanzione">
												<HeaderStyle ></HeaderStyle>
										        <ItemStyle  ></ItemStyle>
										</asp:BoundColumn>
									</Columns>
								<PagerStyle NextPageText="&gt;&gt;"   
									PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
    <p>&nbsp;</p>
</fieldset>
</div>
                   
</asp:Content>

