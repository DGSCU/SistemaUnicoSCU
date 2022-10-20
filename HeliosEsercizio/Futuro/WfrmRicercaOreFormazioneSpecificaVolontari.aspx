<%@ Page Title="Ricerca Ore Formazione Specifica Volontari" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaOreFormazioneSpecificaVolontari.aspx.vb" Inherits="Futuro.WfrmRicercaOreFormazioneSpecificaVolontari" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript">
    function test() {
        return true;
    };
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="firefox" >
        <fieldset class="ContornoPagina">
            <legend id="lgContornoPagina" runat="server">Ricerca Ore Formazione Specifica Volontari</legend>
            <asp:label id="lblmessaggio"  runat="server" CssClass="msgInfo"></asp:label>
            <asp:label id="lblErrore"  runat="server" CssClass="msgErrore"></asp:label>
            <asp:label id="lblConferma"  runat="server" CssClass="msgConferma"></asp:label>
            <br />
            <br />
            <div class="wrapper" style="width:100%">
                <div class="headers" >
                    <h2><asp:Label ID="lblTitolo" Text="Ricerca Progetti" runat="server"></asp:Label></h2>
                </div>
                <div class="RigaVuota" >&nbsp;</div>
                <div class="row" >
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblTitoloProgetto" CssClass="label" AssociatedControlID="txtTitoloProgetto" runat="server" Text="Titolo Progetto"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:TextBox ID="txtTitoloProgetto" CssClass="textbox" runat="server"></asp:TextBox>         
                    </div>
                    
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

                </div>
                <div class="RigaPulsanti">
                    <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                    <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
                    <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
		            <br />
	                <asp:HyperLink ID="hlVolontari" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
                </div>           
                <div class="RigaVuota" >&nbsp;</div>
                <div class="row">
                    <div class="colOggetti">
                        <asp:checkbox id="checkSelDesel" runat="server" Text="Seleziona tutto" Visible="False" AutoPostBack="true"></asp:checkbox>
                    </div>
                </div>
                <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" Caption="Risultato Ricerca Progetti" CssClass="table" CellPadding="3"  AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
				    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		            <EditItemStyle></EditItemStyle>
		            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		            <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		            <HeaderStyle ForeColor="White"></HeaderStyle>
				    <Columns>
					    <asp:TemplateColumn HeaderText = "Sel./Des."  > 
						    <ItemTemplate>
							    <asp:CheckBox  id="chkSelProg" ToolTip="Seleziona/Deseleziona"  AutoPostBack="False"  runat="server"  Text="Sel./Des."></asp:CheckBox>
						    </ItemTemplate>
                            <HeaderStyle Width="5%"></HeaderStyle>
					    </asp:TemplateColumn>
					    <asp:BoundColumn DataField="denominazione" HeaderText="Denominazione">
						    <HeaderStyle Width="20%"></HeaderStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn DataField="titolo" HeaderText="Titolo">
						    <HeaderStyle Width="20%"></HeaderStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn DataField="bando" HeaderText="Bando">
						    <HeaderStyle Width="10%"></HeaderStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn DataField="ambito" HeaderText="Settore / Area Intervento">
						    <HeaderStyle HorizontalAlign="Center" Width="15%" VerticalAlign="Middle"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn Visible="False" DataField="selezionato" HeaderText="selezionato">
						    <HeaderStyle Width="0%"></HeaderStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn DataField="statoattivit&#224;" HeaderText="Stato Progetto">
						    <HeaderStyle HorizontalAlign="Center" Width="10%" VerticalAlign="Middle"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn Visible="False" DataField="VolRic" HeaderText="N&#176; Vol. Con.">
						    <HeaderStyle HorizontalAlign="Center" Width="4%"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn DataField="Volontari" HeaderText="N&#176; Vol.">
						    <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn Visible="False" DataField="IdAttivita" HeaderText="IDAttivit&#224;">
                        <HeaderStyle HorizontalAlign="Center" Width="0%"></HeaderStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn Visible="False" HeaderText="Quest.">
						    <HeaderStyle HorizontalAlign="Center" Width="0%"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
					    </asp:BoundColumn>
					    <asp:BoundColumn Visible="False" DataField="VolQuest" HeaderText="N&#176; Vol. Quest.">
						    <HeaderStyle HorizontalAlign="Center" Width="0%"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
					    </asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="DataProroga" HeaderText="Data proroga">
												<HeaderStyle HorizontalAlign="Center" Width="0%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
											</asp:BoundColumn>
					    <asp:BoundColumn Visible="False" DataField="OrePrev" HeaderText="Ore previste">
						    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <HeaderStyle Width="5%"></HeaderStyle>
					    </asp:BoundColumn>
                        <asp:TemplateColumn  HeaderText="Vol." ItemStyle-Width="5%"> 
                        <ItemTemplate >
				        <asp:ImageButton  ID="linkVolontario" style="cursor:pointer;" CommandName="VisualizzaVolontari"  AlternateText="Elenco Volontari" ToolTip='Elenco Volontari' runat="server" ImageUrl="images/Icona_Volontario_small.png" ></asp:ImageButton>      
                        </ItemTemplate>
                        </asp:TemplateColumn>
					    <asp:BoundColumn Visible="False" DataField="nazionebase" HeaderText="nazionebase">
                        <HeaderStyle Width="0%"></HeaderStyle>
                        </asp:BoundColumn>
					    <asp:BoundColumn Visible="False" DataField="idbando">
                        <HeaderStyle Width="0%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="VisualizzaLinkVolontari">
                         <HeaderStyle Width="0%"></HeaderStyle>
                        </asp:BoundColumn>
				    </Columns>
			    </asp:datagrid>
            </div>
            <asp:HiddenField id="txtDenominazioneEnte1" runat="server" />
            <asp:HiddenField id="ddlMaccCodAmAtt1" runat="server" />
            <asp:HiddenField id="ddlCodAmAtt1" runat="server" />
            <asp:HiddenField id="ddlStatoAttivita1" runat="server" />
        </fieldset>
    </div>
</asp:Content>
