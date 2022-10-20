<%@ Page Title="Ricerca Progetti" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaOreFormazioneVolontari.aspx.vb" Inherits="Futuro.WfrmRicercaOreFormazioneVolontari" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        function VisualizzaVol(IdProg) {
            window.open('WfrmElencoVolontari.aspx?IdAttivita=' + IdProg, 'Volontari', 'width=800, height=650, status=no, toolbar=no, location=no, resizable=yes, menubar=no, scrollbars=yes')
        }

        function VisualizzaCrono(IdProg) {
            window.open('WfrmElencoCronoStampe.aspx?IdAttivita=' + IdProg, 'CronoStampe', 'width=800, height=550, status=no, toolbar=no, location=no, resizable=yes, menubar=no, scrollbars=yes')
        }

        function VisualizzaSanzioneProg(IDAttività, IdEntePresentante) {
            window.open("WfrmSedeSanzionata.aspx?IDAttivita=" + IDAttività + "&IdEntePresentante=" + IdEntePresentante, "ProgettoSanzionato", "width=950, height=600, toolbar=no, resizable=yes, location=no, menubar=no, scrollbars=yes");
        }

        function VisualizzaVerifiche(codiceente) {
            window.open("WfrmElencoVerificheProgetto.aspx?codiceente=" + codiceente, "ProgettoSottopostaVerifica", "width=955, height=500, toolbar=no, location=no, resizable=yes, menubar=no, scrollbars=yes");
        }

        function ConfermaApprova() {
            sMsg = confirm("Procedere con l'operazione di 'APPROVAZIONE'?")
            if (sMsg == false) {
                return false
            }
        }

        function ConfermaPagamento() {
            sMsg = confirm("Procedere con l'operazione di 'PAGAMENTO'?")
            if (sMsg == false) {
                return false
            }
        }

        function ConfermaRespingi() {
            sMsg = confirm("Procedere con l'operazione di 'ANNULLAMENTO'?")
            if (sMsg == false) {
                return false
            }
        }

        function ConfermaProroga() {
            sMsg = confirm("Procedere con l'operazione di 'PROROGA'?")
            if (sMsg == false) {
                return false
            }
        }

        function ConfermaFuoriTermine() {
            sMsg = confirm("Procedere con l'operazione di 'FUORI TERMINE'?")
            if (sMsg == false) {
                return false
            }
        }

        function ConfermaInoltra() {
            var sEsito;
            sEsito = '<%=sEsitoRicerca%>';

//            if (sEsito == 'True') {
//                sMsg = confirm("Attenzione: esistono progetti sui quali è possibile ancora effettuare subentri. Si desidera continuare?")
//                if (sMsg == false) {
//                    return false
//                }
//            } else {
//                sMsg = confirm("Procedere con l'operazione di 'Conferma'?")
//                if (sMsg == false) {
//                    return false
//                }
//            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="firefox" >
        <fieldset class="ContornoPaginaScroll">
        
            <legend id="lgContornoPagina" runat="server"></legend>
            <asp:label id="lblmessaggio"  runat="server" CssClass="msgErrore"></asp:label>
            <br />
            <br />
            <div class="wrapper" style="width:100%">
                <div class="headers" >
                    <h2><asp:Label ID="lblTitolo" runat="server"></asp:Label></h2>
                </div>
                <div class="RigaVuota" >
                &nbsp;
                
                </div>
                <div class="row" >
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblTitoloProgetto" CssClass="label" AssociatedControlID="txtTitoloProgetto" runat="server" Text="Titolo Progetto"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtTitoloProgetto" CssClass="textbox" runat="server"></asp:TextBox>         
                    </div>
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblTipoProgetto" CssClass="label" AssociatedControlID="DdlTipiProgetto" runat="server" Text="Tipo Progetto"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:DropDownList ID="DdlTipiProgetto" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                    </div>
                </div>
                <div class="row" >
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblCodiceProgetto" CssClass="label" AssociatedControlID="txtCodProg" runat="server" Text="Codice Progetto"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtCodProg" CssClass="textbox" runat="server"></asp:TextBox>         
                    </div>
                </div>
                <div class="row" >
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblCircolare" CssClass="label" AssociatedControlID="DdlBando" runat="server" Text="Circolare"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:DropDownList ID="DdlBando" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                    </div>
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblStatiProgetto" CssClass="label" AssociatedControlID="ddlStatoAttivita" runat="server" Text="Stati Progetto"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:DropDownList ID="ddlStatoAttivita" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                    </div>  
                </div>
                <div class="row" >
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblSettore" CssClass="label" AssociatedControlID="ddlMaccCodAmAtt" runat="server" Text="Settore"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:DropDownList ID="ddlMaccCodAmAtt" runat="server" CssClass="ddlClass" AutoPostBack="true"></asp:DropDownList>         
                    </div>
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblAreaIntervento" CssClass="label" AssociatedControlID="ddlCodAmAtt" runat="server" Text="Area Intervento"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:DropDownList ID="ddlCodAmAtt" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                    </div>  
                </div>
                <div class="row" id="divCodiceEnte" runat="server">
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblCodReg" CssClass="label" AssociatedControlID="txtCodReg" runat="server" Text="Cod Ente"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtCodReg" CssClass="textbox" runat="server"></asp:TextBox>         
                    </div>
                     <div class="collable" style="width:15%">      
                        <asp:Label ID="lblDenEnte" CssClass="label" AssociatedControlID="txtDenominazioneEnte" runat="server" Text="Ente Presentante"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtDenominazioneEnte" CssClass="textbox" runat="server"></asp:TextBox>         
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblEnteSecondario" CssClass="label" AssociatedControlID="txtDenominazioneEnteSecondario" runat="server" Text="Ente Secondario"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtDenominazioneEnteSecondario" CssClass="textbox" runat="server"></asp:TextBox>         
                    </div>
                     <div class="collable" style="width:15%">      
                        <asp:Label ID="lblRegione" CssClass="label" AssociatedControlID="txtRegione" runat="server" Text="Regione"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtRegione" CssClass="textbox" runat="server"></asp:TextBox>         
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblProvincia" CssClass="label" AssociatedControlID="txtProvincia" runat="server" Text="Provincia"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtProvincia" CssClass="textbox" runat="server"></asp:TextBox>         
                    </div>
                     <div class="collable" style="width:15%">      
                        <asp:Label ID="lblComune" CssClass="label" AssociatedControlID="txtcomune" runat="server" Text="Comune"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtcomune" CssClass="textbox" runat="server"></asp:TextBox>         
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width:15%">
                        <asp:Label ID="lblOreFormazione" CssClass="label" runat="server" Text="Ore Formazione"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:RadioButton ID="optOreTutte" Text="Tutto" GroupName="gOre" runat="server" Checked="True"/>&nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="optOreSi" Text="Si" GroupName="gOre" runat="server"/>&nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="optOreNo" Text="No" GroupName="gOre" runat="server"/>
                    </div>
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="LblTipoFormGenerale" CssClass="label" AssociatedControlID="ddlTipoFormazioneGenerale" runat="server" Text="Tipo Formazione Generale"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:DropDownList ID="ddlTipoFormazioneGenerale" runat="server" CssClass="ddlClass">
                            <asp:ListItem Value=""></asp:ListItem>
						    <asp:ListItem Value="1">Unica Tranche</asp:ListItem>
						    <asp:ListItem Value="2">Tranche 80% - 20%</asp:ListItem>
                        </asp:DropDownList>         
                    </div> 
                </div>
                <div class="row" id="divStato" runat="server">
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblStato" CssClass="label" AssociatedControlID="ddlStato" runat="server" Text="Stato"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:DropDownList ID="ddlStato" runat="server" CssClass="ddlClass" Visible="false">
                            <asp:ListItem Value="0">Tutti</asp:ListItem>
						    <asp:ListItem Value="1">Confermato</asp:ListItem>
						    <asp:ListItem Value="2">Approvato</asp:ListItem>
						    <asp:ListItem Value="3">In Pagamento</asp:ListItem>
						    <asp:ListItem Value="4">Fuori Termine</asp:ListItem>
                        </asp:DropDownList>         
                    </div>
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblRif" CssClass="label" AssociatedControlID="txtRif" runat="server" Text="Riferimento"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtRif" CssClass="txtRif" runat="server"></asp:TextBox>         
                    </div>
                </div>
                <div class="row" id="divRimborso" runat="server">
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="LblRimborso" CssClass="label" AssociatedControlID="ddlRimborso" runat="server" Text="Rimborso"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:DropDownList ID="ddlRimborso" runat="server" CssClass="ddlClass" Visible="false">
                            <asp:ListItem Value="2">Tutti</asp:ListItem>
						    <asp:ListItem Value="1">Chiede Rimborso</asp:ListItem>
						    <asp:ListItem Value="0">Non chiede Rimborso</asp:ListItem>
                        </asp:DropDownList>         
                    </div>
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblSanzione" CssClass="label" AssociatedControlID="ddlSegnalazioneSanzione" runat="server" Text="Presenza Sanzione" Visible="false"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:DropDownList ID="ddlSegnalazioneSanzione" runat="server" CssClass="ddlClass" Visible="false">
                            <asp:ListItem Value="Tutti" Selected="True">Tutti</asp:ListItem>
					        <asp:ListItem Value="0">No</asp:ListItem>
					        <asp:ListItem Value="1">Si</asp:ListItem>
                        </asp:DropDownList>         
                    </div>
                </div>
                <div class="RigaPulsanti">
                    <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                    <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
                    <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
		            <br />
	                <asp:HyperLink ID="hlVolontari" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
                </div>
                <div class="row" id="divConfermato" runat="server">
                    <div class="colOggetti" style="width:100%;">      
                        <asp:Image ImageUrl="images/DataDaVerificare.jpg" id="imgConfermato" runat="server" AlternateText="Confermato" Width="20"/>
                        <asp:Label ID="lblConfermato" CssClass="label" runat="server" Text="Confermato" AssociatedControlID="imgConfermato"></asp:Label>&nbsp;&nbsp;&nbsp;
                        <asp:Image ImageUrl="images/Lotus.jpg" id="imgApprovato" runat="server" AlternateText="Approvato"  Width="20"/>
                        <asp:Label ID="lblApprovato" CssClass="label" runat="server" Text="Approvato" AssociatedControlID="imgApprovato"></asp:Label>&nbsp;&nbsp;&nbsp;
                        <asp:Image ImageUrl="images/servizio_effettuato.jpg" id="imgInPagamento" runat="server" AlternateText="In Pagamento"  Width="20"/>
                        <asp:Label ID="lblInPagamento" CssClass="label" runat="server" Text="In Pagamento" AssociatedControlID="imgInPagamento"></asp:Label>&nbsp;&nbsp;&nbsp;
                        <asp:Image ImageUrl="images/DoppiaDomanda.jpg" id="imgFuoriTermine" runat="server" AlternateText="Fuori Termine"  Width="20"/>
                        <asp:Label ID="lblFuoriTermine" CssClass="label" runat="server" Text="Fuori Termine" AssociatedControlID="imgFuoriTermine"></asp:Label>&nbsp;&nbsp;&nbsp;
                    </div>
                </div>
                <div class="row" id="divOreFormazioneCaricate" runat="server">
                    <div class="colOggetti" style="width:100%;">
                        <asp:Image ImageUrl="images/DataDaVerificare.jpg" id="imgOreFormazioneCaricate" runat="server" AlternateText="Ore Formazione Caricate" Width="20"/>
                        <asp:Label ID="lblOreFormazioneCaricate" CssClass="label" runat="server" Text="Ore Formazione Caricate" AssociatedControlID="imgOreFormazioneCaricate"></asp:Label>&nbsp;&nbsp;&nbsp;
                        <asp:Image ImageUrl="images/Lotus.jpg" id="imgOreFormazioneConfermate" runat="server" AlternateText="Ore Formazione Confermate" Width="20"/>
                        <asp:Label ID="lblOreFormazioneConfermate" CssClass="label" runat="server" Text="Ore Formazione Confermate" AssociatedControlID="imgOreFormazioneConfermate"></asp:Label>&nbsp;&nbsp;&nbsp;   
                    </div>             
                </div>
                <div class="RigaPulsanti">
                    <asp:Button ID="imgApprova" runat="server" CssClass="Pulsante" Text="Approva" Visible="False" ToolTip="Approva  progetti" OnClientClick="return ConfermaApprova();"/>&nbsp;
                    <asp:Button ID="imgPagamento" runat="server" CssClass="Pulsante" Text="In Pagamento" Visible="False" ToolTip="In pagamento" OnClientClick="return ConfermaPagamento();"/>&nbsp;
                    <asp:Button ID="imgRespingi" runat="server" CssClass="Pulsante" Text="Respingi" Visible="False" ToolTip="Respingi  progetti" OnClientClick="return ConfermaRespingi();"/>&nbsp;
                    <asp:Button ID="imgProroga" runat="server" CssClass="Pulsante" Text="Proroga" Visible="False" ToolTip="Proroga progetti" OnClientClick="return ConfermaProroga();"/>&nbsp;
                    <asp:Button ID="ImaFuoriTermine" runat="server" CssClass="Pulsante" Text="Fuori Termine" Visible="False" ToolTip="Fuori Termine" OnClientClick="return ConfermaFuoriTermine();"/>&nbsp;
                    <asp:Button ID="imgStampa" runat="server" CssClass="Pulsante" Text="Stampa Modulo" Visible="False" ToolTip="Stampa Modulo" />&nbsp;
                    <asp:Button ID="imgInoltra" runat="server" CssClass="Pulsante" Text="Conferma" Visible="False" ToolTip="Inoltra  progetti al UNSC" OnClientClick="return ConfermaInoltra();"/>&nbsp;
                </div>
                <div class="RigaVuota" >
                &nbsp;
                
                </div>
                <div class="row">
                    <div class="colOggetti">
                        <asp:checkbox id="chkSelDesel" runat="server" Text="Seleziona tutto" Visible="False" AutoPostBack="true"></asp:checkbox>
                    </div>
                </div>


               


            </div>


            <fieldset class="ContornoPaginaScroll">
                    <div class="firefox">
                <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco Progetti" CssClass="table" CellPadding="3"  AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
				    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		            <EditItemStyle></EditItemStyle>
		            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		            <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		            <HeaderStyle></HeaderStyle>
				    <Columns>
					    <asp:TemplateColumn HeaderText="Sel.">
						    <HeaderStyle Width="1%"></HeaderStyle>
						    <ItemTemplate>
							    <asp:CheckBox id="chkSelProg" toolTip="Seleziona" AutoPostBack="False" runat="server" Text="&nbsp;"></asp:CheckBox>
						    </ItemTemplate>
					    </asp:TemplateColumn>
					    <asp:BoundColumn DataField="denominazione" HeaderText="Denominazione">
						    <HeaderStyle Width="12%"></HeaderStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn DataField="titolo" HeaderText="Titolo">
						    <HeaderStyle Width="6%"></HeaderStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn DataField="bando" HeaderText="Bando">
						    <HeaderStyle Width="12%"></HeaderStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn DataField="DataApprovazione" HeaderText="Data Approvazione" 
                            Visible="False">
						    <HeaderStyle HorizontalAlign="Center" Width="10%" VerticalAlign="Middle"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn Visible="False" DataField="selezionato" HeaderText="selezionato">
						    <HeaderStyle Width="1%"></HeaderStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn DataField="statoattivit&#224;" HeaderText="Stato Progetto">
						    <HeaderStyle HorizontalAlign="Center" Width="4%" VerticalAlign="Middle"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn Visible="False" DataField="VolRic" HeaderText="N&#176; Vol. Con.">
						    <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn DataField="Volontari" HeaderText="N&#176; Vol.">
						    <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn Visible="False" DataField="IDAttivit&#224;" HeaderText="IDAttivit&#224;">
						    <HeaderStyle Width="2%"></HeaderStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn DataField="VolRimborso" HeaderText="N&#176; Vol. Rimb.">
						    <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn Visible="False" HeaderText="Quest.">
						    <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn Visible="False" DataField="VolQuest" HeaderText="N&#176; Vol. Quest.">
						    <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn Visible="False" DataField="OrePrev" HeaderText="Ore previste">
						    <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn DataField="LinkVol" HeaderText="Vol.">
						    <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn Visible="False" DataField="CronoStampe" HeaderText="Stampe">
						    <HeaderStyle Width="6%"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn Visible="False" DataField="DataProroga" HeaderText="Data proroga">
						    <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn Visible="False" DataField="ChiedeRimborso" HeaderText="Chiede Rimborso">
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn Visible="False" DataField="nazionebase" HeaderText="nazionebase"></asp:BoundColumn>
					    <asp:BoundColumn Visible="False" DataField="idbando"></asp:BoundColumn>
					    <asp:BoundColumn Visible="False" DataField="RimborsoFormazioneItalia"></asp:BoundColumn>
					    <asp:BoundColumn Visible="False" DataField="RimborsoFormazioneEstero"></asp:BoundColumn>
					    <asp:BoundColumn Visible="False" DataField="DatiPianificazione" HeaderText="Dati Pianificazione(Si/No/Non Necessario)">
						    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn DataField="presenzasanzione" HeaderText="Presenza Sanzione">
						    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
					    </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Annulla Stato" Visible="False"> 
                            <ItemTemplate>
					            <asp:ImageButton ID="imgAnnullaStato" CommandName="Ripristina" ToolTip="Annulla Stato" AlternateText="Annulla Stato" runat="server" ImageURL="images/ripristina_small.png" CausesValidation="false"></asp:ImageButton>                         
                            </ItemTemplate>
                        </asp:TemplateColumn>
				    </Columns>
			    </asp:datagrid>
                
                    </div>
                </fieldset>
         
        </fieldset>
     </div> 
      <asp:HiddenField id="txtTitoloProgetto1" runat="server" />
            <asp:HiddenField id="txtDenominazioneEnte1" runat="server" />
            <asp:HiddenField id="ddlMaccCodAmAtt1" runat="server" />
            <asp:HiddenField id="ddlCodAmAtt1" runat="server" />
            <asp:HiddenField id="ddlStatoAttivita1" runat="server" />

</asp:Content>
