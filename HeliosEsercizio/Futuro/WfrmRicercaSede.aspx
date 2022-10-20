<%@ Page Title="Ricerca Sede" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaSede.aspx.vb" Inherits="Futuro.WfrmRicercaSede" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="firefox">

<fieldset class="ContornoPaginaScroll" style="width:auto;overflow:auto;">

        <legend>Ricerca Sede</legend>
      
        <div class="wrapper" style="width:100%">
        
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Sede"></asp:Label></h2>
            </div>
                 <div class="rowGroup" style="height:auto">
        <div class="RigaVuota">

      &nbsp;
      </div>
         <asp:label id="lblErrore"  runat="server" CssClass="msgErrore" ></asp:label>
        <div class="RigaVuota">
      &nbsp;</div>
    </div>
            <div class="RigaVuota" >
            &nbsp;
            </div>
            <div class="row" id="divSede" runat="server">
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDenominazioneEnte" CssClass="label" AssociatedControlID="txtDenominazioneEnte" runat="server" Text="Den. Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtDenominazioneEnte"  CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodiceEnte" CssClass="label" AssociatedControlID="TxtCodiceRegione" runat="server" Text="Cod. Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="TxtCodiceRegione" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
               <div class="row"  runat="server">
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDenominazione" CssClass="label" AssociatedControlID="txtdenominazione" runat="server" Text="Denominazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtdenominazione"  CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblStato" CssClass="label" AssociatedControlID="ddlstato" runat="server" Text="Stato"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList id="ddlstato"  runat="server" CssClass="ddlClass"></asp:DropDownList>        
                </div>
            </div>
            <div class="row"  runat="server">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblRegione" CssClass="label" AssociatedControlID="txtregione" runat="server" Text="Regione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtregione" CssClass="textbox" runat="server"></asp:TextBox>         
                </div> 
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblStatoUtenza" CssClass="label" AssociatedControlID="ddlTipologia" runat="server" Text="Tipologia"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlTipologia" runat="server">
                    </asp:DropDownList>
                </div> 
            </div>
            <div class="row"  runat="server">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblComune" CssClass="label" AssociatedControlID="txtComune" runat="server" Text="Comune"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtComune" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblProvincia" CssClass="label" AssociatedControlID="txtProvincia" runat="server" Text="Provincia"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtProvincia" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            
            <div  class="row"  >
                    <div class="collable" style="width:15%">      
                    <asp:Label ID="lblIndirizzo" CssClass="label" AssociatedControlID="txtIndirizzo" runat="server" Text="Indirizzo"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtIndirizzo" CssClass="textbox" runat="server"></asp:TextBox>      
                 </div>    
                <div id="divCertificazione" runat="server"  >
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lbCertificazione" CssClass="label" AssociatedControlID="ddlCertificazione" runat="server" Text="Iscrizione"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:DropDownList ID="ddlCertificazione" runat="server" CssClass="ddlClass">
                            <asp:ListItem Selected="True">Tutti</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>
                            <asp:ListItem Value="1">Si</asp:ListItem>
                               <asp:ListItem Value="2">Da Valutare</asp:ListItem>
                        </asp:DropDownList>
                    </div> 
                 </div>
            </div>
            <div  class="row"  >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodSedeAtt" CssClass="label" AssociatedControlID="txtCodSedeAtt" runat="server" Text="Cod. Sede Attuazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodSedeAtt" CssClass="textbox" runat="server"></asp:TextBox>         
                 </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDuplicati" CssClass="label" AssociatedControlID="ddlDuplicati" runat="server" Text="Indirizzi Duplicati"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlDuplicati" runat="server" CssClass="ddlClass">
                   <asp:ListItem Selected="True">Tutti</asp:ListItem>
				    <asp:ListItem Value="0">Non Duplicati</asp:ListItem>
					<asp:ListItem Value="1">Solo Duplicati</asp:ListItem>
                    </asp:DropDownList>
                </div> 
            </div>
              <div  class="row"  >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblLocalizzazione" CssClass="label" AssociatedControlID="ddlLocalizzazione" runat="server" Text="Localizzazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlLocalizzazione" runat="server" CssClass="ddlClass">
                        <asp:ListItem Selected="True">Tutti</asp:ListItem>
						<asp:ListItem Value="1">Italiana</asp:ListItem>
						<asp:ListItem Value="0">Estera</asp:ListItem>
                    </asp:DropDownList>
                </div> 
                <div id="divForzaSanzione" runat="server">
                   <div class="collable" style="width:15%">      
                    <asp:Label ID="lblSanzione" CssClass="label" AssociatedControlID="ddlSegnalazioneSanzione" runat="server" Text="Presenza Sanzione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlSegnalazioneSanzione" runat="server" CssClass="ddlClass">
                       <asp:ListItem Selected="True">Tutti</asp:ListItem>
								<asp:ListItem Value="0">No</asp:ListItem>
								<asp:ListItem Value="1">Si</asp:ListItem>
                    </asp:DropDownList>
                </div> 
                </div>
              </div>
              <div  class="row"  >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblRichiestaVariazione" CssClass="label" AssociatedControlID="ddlRichistaVariazione" runat="server" Text="Richiesta Variazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlRichistaVariazione" runat="server" CssClass="ddlClass">
                        <asp:ListItem Text="Tutti" Value=""></asp:ListItem>
						<asp:ListItem Text="Variazione richieste" Value="1"></asp:ListItem>
						<asp:ListItem Text="Senza variazione" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblFiltroFase" CssClass="label" AssociatedControlID="txtFiltroFase" runat="server" Text="Rif. Fase" Visible="false"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtFiltroFase" CssClass="textbox" runat="server" Visible="false"></asp:TextBox>
                    <asp:Label ID="lblErroreFiltroFase" CssClass="msgErrore" Text="Inserire un valore numerico." Visible="false" runat="server" />
                </div>                 
               </div>
                <div  class="row"  >
                <div class="colOggetti" style="width:50%">  
                <asp:CheckBox ID="chkRiferimentoRimborsi" Text="Sede di riferimento per rimborsi" Visible="False" runat="server"/>     
                </div> 
                   <div class="colOggetti" style="width:50%">  
                <asp:CheckBox ID="chkIncludi" Text="Includi tutte le sedi" Visible="False" runat="server"/>     
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
        
            <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" Caption="Risultato Ricerca Sedi Ente"
                    ToolTip="Elenco Sedi" CssClass="table"  AllowPaging="True" 
                    AllowSorting="True" AutoGenerateColumns="False" 
                    UseAccessibleHeader="True">
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		        <HeaderStyle></HeaderStyle>
		        <Columns>
                    <asp:TemplateColumn HeaderText="Selez."> 
                        <ItemTemplate>
	                     <asp:ImageButton ID="IdImgSelSedi" style="cursor:pointer;" CommandName="Select" alt="Seleziona Sede" ToolTip='Seleziona Sede' runat="server" ImageURL="images/sedi_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                    </asp:TemplateColumn>
					<asp:BoundColumn DataField="Stato" HeaderText="Stato"></asp:BoundColumn>
					<asp:BoundColumn DataField="Sede" HeaderText="Ente Sede"></asp:BoundColumn>
					<asp:BoundColumn DataField="Ente" HeaderText="Ente"></asp:BoundColumn>
					<asp:BoundColumn DataField="Tiposede" HeaderText="Tipologia"></asp:BoundColumn>
					<asp:BoundColumn DataField="NSedi" HeaderText="Cod.Sede Attuaz.">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="Codicesede" HeaderText="Cod"></asp:BoundColumn>
					<asp:BoundColumn DataField="Indirizzo" HeaderText="Indirizzo"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="Cap" HeaderText="CAP"></asp:BoundColumn>
					<asp:BoundColumn DataField="Comune" HeaderText="Comune"></asp:BoundColumn>
					<asp:BoundColumn DataField="Telefono" HeaderText="Telefono"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="Email" HeaderText="Email"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="idEnteSede" HeaderText="idSede"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="idente" HeaderText="idEnte"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="acquisita" HeaderText="idass"></asp:BoundColumn>
					<asp:TemplateColumn>
						<HeaderTemplate>
							<strong>Inclusa</strong>
						</HeaderTemplate>
						<ItemTemplate>
							&nbsp;&nbsp;
							<asp:CheckBox id="check1" runat="server" AutoPostBack="True" ToolTip="Sede Inclusa"></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="DataCreazioneRecord" HeaderText="Data Inserimento" DataFormatString="{0:d}">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Certificazione" HeaderText="Presenza Iscrizione">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Segnalazione" HeaderText="Presenza Sanzione">
						<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="verifica" HeaderText="Presenza Verifica">
						<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundColumn>
  	                <asp:TemplateColumn HeaderText="Anomalie" > 
                         <ItemStyle width="5%"></ItemStyle>
                        <ItemTemplate>
	                     <asp:ImageButton ID="IdImgAlert" runat="server" style="cursor:pointer;" alt="Il nome o indirizzo della sede presentano anomalie" ToolTip="Il nome o indirizzo della sede presentano anomalie" ImageURL="images/alert_small.png" visible = "false" CausesValidation="false"/>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn Visible="False" DataField="AnomaliaNome" HeaderText="AnomaliaNome"></asp:BoundColumn>
                    <asp:BoundColumn Visible="False" DataField="AnomaliaIndirizzo" HeaderText="AnomaliaIndirizzo"></asp:BoundColumn>
                    <asp:BoundColumn Visible="False" DataField="AnomaliaIndirizzoGoogle" HeaderText="AnomaliaIndirizzoGoogle"></asp:BoundColumn>
				</Columns>
		    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
	    </asp:datagrid>
       <asp:HiddenField ID="txtModifica" runat="server" />
       <asp:HiddenField ID="txtstrsql" runat="server"  />
       <asp:HiddenField ID="lblpage" runat="server"  />
       <asp:HiddenField ID="txtRicerca" runat="server"  />
       <asp:HiddenField ID="txtCodice" runat="server"  />
     

    </fieldset>

</div>
</asp:Content>
