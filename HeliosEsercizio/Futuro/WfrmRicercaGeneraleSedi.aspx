<%@ Page Title="Ricerca Generale Sedi" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaGeneraleSedi.aspx.vb" Inherits="Futuro.WfrmRicercaGeneraleSedi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="firefox">
    <fieldset  class="ContornoPaginaScroll">
        <legend>Ricerca Generale Sedi</legend>
         <div class="wrapper" style="width:100%">   
            <div class="headers">
                <h2><asp:Label ID="Label5" runat="server"  Text="RICERCA GENERALE SEDI"></asp:Label> </h2>
            </div>
            <div class="rowGroup" style="height:auto">
        <div class="RigaVuota">
        
           &nbsp;
      </div>
         <asp:label id="lblmessaggio"  runat="server" CssClass="msgErrore" ></asp:label>
        <div class="RigaVuota">
      &nbsp;</div>
    </div>
            <div class="RigaVuota" >
            &nbsp;
            </div>
        
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDenEnte" CssClass="label" AssociatedControlID="txtDenominazioneEnte" runat="server" Text="Denominazione Ente"/>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:TextBox ID="txtDenominazioneEnte" runat="server" CssClass="textbox" />
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="LblCodRegione" CssClass="label" AssociatedControlID="txtCodRegione" runat="server" Text="Cod. Ente"/>
                </div>
                <div class="colOggetti" style="width:35%">
                     <asp:TextBox ID="txtCodRegione" runat="server" CssClass="textbox" />
                </div> 
            </div>
            <div  class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDenSede" CssClass="label" AssociatedControlID="txtDenominazione" runat="server" Text="Denominazione Sede"/>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:TextBox ID="txtDenominazione" runat="server" CssClass="textbox" />
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="LblCodSede" CssClass="label" AssociatedControlID="txtCodSedeAtt" runat="server" Text="Cod. Sede Attuazione"/>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:TextBox ID="txtCodSedeAtt" runat="server" CssClass="textbox" /><asp:textbox id="txtCodice" runat="server" Width="0px" ReadOnly="True"  Visible="false"/>
                </div> 
            </div> 
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblRegione" CssClass="label" AssociatedControlID="txtregione" runat="server" Text="Regione"/>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:TextBox ID="txtregione" runat="server" CssClass="textbox" />
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblProvincia" CssClass="label" AssociatedControlID="txtProvincia" runat="server" Text="Provincia"/>
                </div>
                <div class="colOggetti" style="width:35%"> 
                    <asp:TextBox ID="txtProvincia" runat="server" CssClass="textbox" />
                </div>
            </div>
            <div class="row">  
                <div class="collable" style="width:15%">
                    <asp:Label ID="LblComune" CssClass="label" AssociatedControlID="txtComune" runat="server" Text="Comune"/>
                </div>
                <div class="colOggetti" style="width:35%"> 
                    <asp:TextBox ID="txtComune" runat="server" CssClass="textbox" />
                </div> 
                <div class="collable" style="width:15%">
                    <asp:Label ID="LblCompetenza" CssClass="label" AssociatedControlID="CboCompetenza" runat="server" Text="Competenza"/>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="CboCompetenza" CssClass="ddlClass"   runat="server"/>
                </div>  
            </div>
            <div  class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="LblTipologia" CssClass="label" AssociatedControlID="ddlTipologia" runat="server" Text="Tipologia"/>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="ddlTipologia" CssClass="ddlClass" runat="server" Enabled="False"/>
                </div> 
                <div class="collable" style="width:15%">
                    <asp:Label ID="LblStato" CssClass="label" AssociatedControlID="ddlstato" runat="server" Text="Stato"/>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="ddlstato" CssClass="ddlClass" runat="server"/>
                </div> 
            </div>

            <div  class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="LblPresCertificazione" CssClass="label" AssociatedControlID="ddlCertificazione" runat="server" Text="Presenza Iscrizione"/>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="ddlCertificazione" CssClass="ddlClass" runat="server">
                        <asp:ListItem Selected="True">Tutti</asp:ListItem>
                        <asp:ListItem Value="0">No</asp:ListItem>
                        <asp:ListItem Value="1">Si</asp:ListItem>
                        <asp:ListItem Value="2">Da Valutare</asp:ListItem>
                    </asp:DropDownList>
                </div> 
                <div class="collable" style="width:15%">
                    <asp:Label ID="LblPresSanzione" CssClass="label" AssociatedControlID="ddlSegnalazioneSanzione" runat="server" Text="Presenza Sanzione"/>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="ddlSegnalazioneSanzione" CssClass="ddlClass" runat="server">
                        <asp:ListItem Selected="True">Tutti</asp:ListItem>
					    <asp:ListItem Value="0">No</asp:ListItem>
						<asp:ListItem Value="1">Si</asp:ListItem>
                    </asp:DropDownList>
                </div> 
            </div> 
            <div  class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="LblIndirizziDuplicati" CssClass="label" AssociatedControlID="ddlDuplicati" runat="server" Text="Indirizzi Duplicati"/>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="ddlDuplicati" CssClass="ddlClass" runat="server">
                        <asp:ListItem Selected="True">Tutti</asp:ListItem>
				        <asp:ListItem Value="0">Non Duplicati</asp:ListItem>
					    <asp:ListItem Value="1">Solo Duplicati</asp:ListItem>
                    </asp:DropDownList>
                </div> 
                <div class="collable" style="width:15%">
                    <asp:Label ID="LblLocalizzazione" CssClass="label" AssociatedControlID="ddlLocalizzazione" runat="server" Text="Localizzazione"/>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="ddlLocalizzazione" CssClass="ddlClass" runat="server">
                        <asp:ListItem Selected="True">Tutti</asp:ListItem>
			            <asp:ListItem Value="1">Italiana</asp:ListItem>
			            <asp:ListItem Value="0">Estera</asp:ListItem>
                    </asp:DropDownList>
                </div> 
            </div>
          <div  class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="Label1" CssClass="label" AssociatedControlID="ddlAnomalie" runat="server" Text="Anomalie"/>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="ddlAnomalie" CssClass="ddlClass" runat="server">
                        <asp:ListItem Selected="True">Tutti</asp:ListItem>
				        <asp:ListItem Value="0">Con anomalie</asp:ListItem>
					    <asp:ListItem Value="1">Senza anomalie</asp:ListItem>
					    <asp:ListItem Value="2">Anomalia GoogleMaps</asp:ListItem>
					    <asp:ListItem Value="3">Anomalia nome sede</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblFiltroFase" CssClass="label" AssociatedControlID="txtFiltroFase" runat="server" Text="Rif. Fase"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtFiltroFase" CssClass="textbox" runat="server"></asp:TextBox>
                    <asp:Label ID="lblErroreFiltroFase" CssClass="msgErrore" Text="Inserire un valore numerico." Visible="false" runat="server" />
                </div>                     
            </div>
 
            <div class="RigaPulsanti">
                <asp:Button ID="CmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                <asp:Button ID="CmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
                <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
                <br/>
                <asp:HyperLink ID="ApriCSV1"   AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"/>&nbsp;
            </div>           
        </div>
       
         
        <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" Caption="Risultato Ricerca Sedi" ToolTip="Risultato Ricerca Sedi" AllowPaging="True" CellPadding="2"  CssClass="table" AutoGenerateColumns="False" UseAccessibleHeader="True">
                    <FooterStyle></FooterStyle>
                    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
                    <EditItemStyle></EditItemStyle>
                    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                    <ItemStyle CssClass="tr"></ItemStyle>
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
	                    <asp:BoundColumn Visible="False" DataField="Tiposede" HeaderText="Tipologia"></asp:BoundColumn>
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
	                    <asp:TemplateColumn Visible="False">
		                    <HeaderTemplate>
			                    <b>Inclusa</b>
		                    </HeaderTemplate>
		                    <ItemTemplate>
			                    &nbsp;&nbsp;
			                    <asp:CheckBox id="check1" runat="server" AutoPostBack="True" ToolTip="Sede Inclusa"></asp:CheckBox>
		                    </ItemTemplate>
	                    </asp:TemplateColumn>
	                    <asp:BoundColumn DataField="DataCreazioneRecord" HeaderText="Data Inserimento" DataFormatString="{0:d}">
		                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
	                    </asp:BoundColumn>
	                    <asp:BoundColumn DataField="codiceregione" HeaderText="Codice Nazionale"></asp:BoundColumn>
	                    <asp:BoundColumn DataField="Competenza" HeaderText="Competenza"></asp:BoundColumn>
	                    <asp:BoundColumn DataField="UtenteCompetenza" HeaderText="CompetenzaUtente" Visible="False"></asp:BoundColumn>
	                    <asp:BoundColumn DataField="Certificazione" HeaderText="Presenza Iscrizione">
		                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
		                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
	                    </asp:BoundColumn>
	                    <asp:BoundColumn DataField="Segnalazione" HeaderText="Presenza Sanzione" Visible="False">
		                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
	                    </asp:BoundColumn>
	                    <asp:BoundColumn DataField="verifica" HeaderText="Presenza Verifica">
		                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
	                    </asp:BoundColumn>
	                    <asp:BoundColumn DataField="IdClasseAccreditamentoRichiesta" HeaderText="ClasseAccreditamentoRichiesta" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="NMaxVolontari" HeaderText="N° Volontari" Visible="True">
                           <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                        </asp:BoundColumn>
                      	<asp:TemplateColumn HeaderText="Anomalie" > 
                         <ItemStyle width="5%" HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                        <ItemTemplate>
	                     <asp:ImageButton ID="IdImgAlert" runat="server" style="cursor:pointer;align-self:center" alt="Il nome o indirizzo della sede presentano anomalie" ToolTip="Il nome o indirizzo della sede presentano anomalie" ImageURL="images/alert_small.png" visible = "false" CausesValidation="false"/>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn Visible="False" DataField="AnomaliaNome" HeaderText="AnomaliaNome"></asp:BoundColumn>
                    <asp:BoundColumn Visible="False" DataField="AnomaliaIndirizzo" HeaderText="AnomaliaIndirizzo"></asp:BoundColumn>
                    <asp:BoundColumn Visible="False" DataField="AnomaliaIndirizzoGoogle" HeaderText="AnomaliaIndirizzoGoogle"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
                    </asp:datagrid>
                
      
        <asp:textbox id="txtRicerca" runat="server"  Width="0px" ReadOnly="True"  Visible="false"  /> 
    </fieldset>
    </div>
</asp:Content>
