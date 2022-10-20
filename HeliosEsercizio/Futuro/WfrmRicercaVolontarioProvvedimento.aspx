<%@ Page Title="RicervaVolontariProvvedimentoDisciplinare" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaVolontarioProvvedimento.aspx.vb" Inherits="Futuro.WfrmRicercaVolontarioProvvedimento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="firefox">
        <fieldset class="ContornoPaginaScroll" >
        <legend>Ricerca Volontari</legend>
            <div class="wrapper" style="width:100%">
            
                <div class="headers">
                    <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Volontari"/></h2>
                </div>
                <asp:label id="LblErrore"  runat="server" CssClass="msgErrore"/>
                <div class="RigaVuota">&nbsp;</div>
                
                <div class="row">
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
                          <asp:TextBox ID="txtNome" CssClass="textbox" runat="server" TabIndex="1"></asp:TextBox>  
                   </div>
                </div>
                <div class="row" >
                    <div class="collable" style="width:15%">
                        <asp:Label ID="LblCodVolontario" AssociatedControlID="txtCodVolontario"  runat="server" Text="Cod. Volontario"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:TextBox ID="txtCodVolontario" CssClass="textbox"   runat="server" 
                            TabIndex="2"></asp:TextBox>
                    </div>
                    <div class="collable" style="width:15%">
                        <asp:Label ID="LblCodFiscale" AssociatedControlID="txtCodFiscale" runat="server" Text="Cod. Fiscale"></asp:Label>
                    </div>   
                    <div class="colOggetti" style="width:35%">    
                        <asp:TextBox ID="txtCodFiscale" CssClass="textbox" runat="server" TabIndex="3"></asp:TextBox>  
                    </div>
                </div>
                <div class="row" >  
                   <div class="collable" style="width:15%">
                        <asp:Label ID="LblStato" AssociatedControlID="cboStato" runat="server" Text="Stato"></asp:Label>
                   &nbsp;Volontario</div>
                    <div class="colOggetti" style="width:35%">    
                          <asp:DropDownList ID="cboStato" CssClass="ddlClass" runat="server" TabIndex="4"></asp:DropDownList> 
                   </div>
                    <div class="collable" style="width:15%">
                        <asp:Label ID="lblCodEnte" AssociatedControlID="txtCodEnte"   runat="server" Text="Cod. Ente"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:TextBox ID="txtCodEnte" CssClass="textbox"  runat="server" TabIndex="5"></asp:TextBox>
                    </div> 
                </div>
                <div class="row" >
                    <div class="collable"  style="width:15%">
                        <asp:Label ID="lblDescEnte" AssociatedControlID="txtDescEnte" CssClass="label"  runat="server" Text="Denominazione Ente"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:TextBox ID="txtDescEnte" CssClass="textbox" autofocus="true" 
                            runat="server" TabIndex="6"></asp:TextBox>
                    </div>   
             
                    <div class="collable" style="width:15%">
                        <asp:Label ID="IdLblProgetto" AssociatedControlID="txtProgetto" runat="server" Text="Progetto"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">    
                         <asp:TextBox ID="txtProgetto" CssClass="textbox" runat="server" TabIndex="7"></asp:TextBox>  
                   </div>
                </div>
                <div class="row" >
                    <div class="collable" style="width:15%">
                        <asp:Label ID="IdLblCodProgetto" AssociatedControlID="txtCodProgetto" runat="server" Text="Cod. Progetto"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:TextBox ID="txtCodProgetto" CssClass="textbox" runat="server" TabIndex="8"></asp:TextBox>
                    </div>
                </div>
               

                <div class="RigaPulsanti">
                    <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                    <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
                    <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="false"/>&nbsp;<br />
                    <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server"         ForeColor="#003399" Visible="False"/>
                </div>

                <h3><asp:label id="lblmessaggioRicerca"  runat="server" AssociatedControlID="dtgRisultatoRicerca"></asp:label></h3>
                <div class="RigaVuota">&nbsp;</div>
              
                    <fieldset class="ContornoPaginaScrollT">

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
                                    <asp:BoundColumn HeaderText="IDEntità" DataField="IDEntità" Visible="False" >
						                <HeaderStyle ></HeaderStyle>
						                <ItemStyle></ItemStyle>
					                </asp:BoundColumn>
                                    <asp:BoundColumn DataField="IDAttivitàEntità" HeaderText="IDAttivitàEntità" 
                                        Visible="False"></asp:BoundColumn>
					                <asp:BoundColumn Visible="False" DataField="IdEnte" HeaderText="IdEnte">
                                    <ItemStyle ></ItemStyle>
                                    </asp:BoundColumn>
					                <asp:BoundColumn DataField="codicevolontario"  HeaderText="Codice Volontario">
						                <HeaderStyle ></HeaderStyle>
						                <ItemStyle ></ItemStyle>
					                </asp:BoundColumn>
					                <asp:BoundColumn DataField="Nominativo" HeaderText="Nominativo">
                                    <ItemStyle></ItemStyle>
                                    </asp:BoundColumn>
					                <asp:BoundColumn DataField="DataInizio" HeaderText="Data Inizio Servizio">
						                <HeaderStyle ></HeaderStyle>
						                <ItemStyle ></ItemStyle>
					                </asp:BoundColumn>
					                <asp:BoundColumn DataField="DataFine" HeaderText="Data Fine Servizio">
						                <HeaderStyle ></HeaderStyle>
						                <ItemStyle></ItemStyle>
					                </asp:BoundColumn>
					                <asp:BoundColumn DataField="StatoEntità" HeaderText="Stato">
                                    <ItemStyle></ItemStyle>
                                    </asp:BoundColumn>
					                <asp:BoundColumn DataField="CodiceProgetto" HeaderText="Codice Progetto" 
                                        Visible="False">
                                    <ItemStyle ></ItemStyle>
                                    </asp:BoundColumn>
					                <asp:BoundColumn DataField="Progetto" HeaderText="Progetto">
						                <HeaderStyle ></HeaderStyle>
						                <ItemStyle ></ItemStyle>
					                </asp:BoundColumn>
					                <asp:BoundColumn DataField="Ente" HeaderText="Ente">
                                    <ItemStyle ></ItemStyle>
                                    </asp:BoundColumn>
					                <asp:BoundColumn DataField="CodiceSede" HeaderText="Codice Sede" 
                                        Visible="False">
                                    <ItemStyle ></ItemStyle>
                                    </asp:BoundColumn>
					                <asp:BoundColumn DataField="Sede" HeaderText="Sede">
						                <HeaderStyle ></HeaderStyle>
						                <ItemStyle></ItemStyle>
					                </asp:BoundColumn>
                                    <asp:BoundColumn DataField="comune" HeaderText="Comune Sede"></asp:BoundColumn>
                               
				                </Columns>
				                <PagerStyle NextPageText="&gt;&gt;"   
					                PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
			            </asp:datagrid>
                    </fieldset>
               
                <div class="RigaVuota">&nbsp;</div>

            </div>
           
        </fieldset>
    </div>
</asp:Content>
