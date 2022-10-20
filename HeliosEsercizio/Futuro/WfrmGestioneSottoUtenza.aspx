<%@ Page Title="GestioneSottoUtenza" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmGestioneSottoUtenza.aspx.vb" Inherits="Futuro.WfrmGestioneSottoUtenza" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend id="myLegend" runat="server">Gestione Utenza Sede</legend>
        <div class="wrapper" style="width:100%">
            <div class="headers">
                <h2><asp:Label ID="lblTitolo" runat="server"  Text="Gestore degli operatori volontari per sede"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
                <fieldset class="ContornoPagina">
                    <legend>Informazioni Sede</legend>
                        <div class="wrapper" style="width:100%;border:0px">
                        <div id="DivDatiSede" class="row" runat="server">
                            <div class="collable" style="width:15%">
                                <asp:label id="LabelCodiceSede" CssClass="label" AssociatedControlID="lblCodiceSede" runat="server" Text="Codice Sede" />
                            </div>
                            <div class="colOggetti" style="width:35%">
                                <asp:label id="lblCodiceSede" CssClass="labelDati" runat="server" />
                            </div>
                            <div class="collable" style="width:15%">
                                <asp:label id="LabelSede" CssClass="label" AssociatedControlID="lblSede" runat="server" Text="Sede" />
                            </div>
                            <div class="colOggetti" style="width:35%">
                                <asp:label id="lblSede" CssClass="labelDati" runat="server" />
                            </div>
                        </div>  
                        <div id="Div3" class="row" runat="server">
                            <div class="collable" style="width:15%">
                                <asp:label id="LabelIndirizzo" CssClass="label" AssociatedControlID="LblIndirizzo" runat="server" Text="Indirizzo" />
                            </div>
                            <div class="colOggetti" style="width:35%">
                                <asp:label id="LblIndirizzo" CssClass="labelDati" runat="server" />
                            </div>
                            <div class="collable" style="width:15%">
                                <asp:label id="LabelComune" CssClass="label" AssociatedControlID="LblComune" runat="server" Text="Comune" />
                            </div>
                            <div class="colOggetti" style="width:35%">
                                <asp:label id="LblComune" CssClass="labelDati" runat="server" />
                            </div>
                        </div>  

                    </div>
                </fieldset>
                <div runat="server" id="divUtenzeSede">
                    <fieldset class="ContornoPagina">
                    <legend>Gestione Utenze Sede</legend>
                        <div class="wrapper" style="width:100%;border:0px">
                             <div id="Div6" class="row" runat="server">
                                <div class="collable" style="width:15%">
                                    <asp:label id="LblCodiceFiscale" CssClass="label" AssociatedControlID="TxtCodiceFiscale" runat="server" Text="CodiceFiscale" />
                                </div>
                                <div class="colOggetti" style="width:35%">
                                    <asp:TextBox ID="TxtCodiceFiscale" runat="server"></asp:TextBox>
                                </div>
                            </div>
                             <div id="Div7" class="row" runat="server">
                                <div class="collable" style="width:15%">
                                    <asp:label id="LblCognome" CssClass="label" AssociatedControlID="TxtCognome" runat="server" Text="Cognome" />
                                </div>
                                <div class="colOggetti" style="width:35%">
                                    <asp:TextBox ID="TxtCognome" runat="server"></asp:TextBox>
                                </div>
                            </div>
                             <div id="Div8" class="row" runat="server">
                                <div class="collable" style="width:15%">
                                    <asp:label id="LblNome" CssClass="label" AssociatedControlID="TxtNome" runat="server" Text="Nome" />
                                </div>
                                <div class="colOggetti" style="width:35%">
                                    <asp:TextBox ID="TxtNome" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <h3>
                                <asp:label id="lblMsgUtenzaSPID" runat="server"
                                    CssClass="msgErrore"></asp:label>
                            </h3>
                            <br />
                            <div id="Div9" class="row" runat="server">
                                <div class="wrapper" style="width:100%;border:0px">
                                    <div class="RigaPulsanti" style="margin-right:5px">
                                        <asp:Button ID="cmdInserisci" runat="server" CssClass="Pulsante" Text="Inserisci Utenza"  />&nbsp;
                                        <asp:Button ID="Button1" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
                                    </div>
                                </div>
                            </div>						
                        </div>
                        <div class="RigaVuota">&nbsp;</div>
                        <asp:datagrid id="dtgUtenzeSede" runat="server" Width="100%" 
                                    ToolTip="Elenco Sedi" CssClass="table"  AllowPaging="True" 
                                    AllowSorting="True" AutoGenerateColumns="False" 
                                    UseAccessibleHeader="True" >
	                            <FooterStyle></FooterStyle>
		                        <SelectedItemStyle ></SelectedItemStyle>
		                        <EditItemStyle></EditItemStyle>
		                        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		                        <ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		                        <HeaderStyle ></HeaderStyle>
	                            <Columns>
			                    <asp:BoundColumn Visible="False" DataField="IdSedePassword"></asp:BoundColumn>
			                    <asp:BoundColumn DataField="CodiceFiscale" HeaderText="Codice Fiscale"></asp:BoundColumn>
			                    <asp:BoundColumn DataField="Cognome" HeaderText="Cognome"></asp:BoundColumn>
			                    <asp:BoundColumn DataField="Nome" HeaderText="Nome"></asp:BoundColumn>
			                    <asp:BoundColumn DataField="Stato" HeaderText="Stato"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Attiva/Disattiva" Visible="true"> 
                                    <ItemTemplate  >
						                <asp:ImageButton ID="ImgCambiaStato" style="cursor:pointer;" CommandName="CambiaStato" AlternateText="Cambia Stato" ToolTip='Cambia Stato' 
                                             runat="server" ImageURL="images/ripristina_small.png" CausesValidation="false"></asp:ImageButton>      
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                </Columns>
	                            <HeaderStyle></HeaderStyle>
                                <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
                            </asp:datagrid>                    	
                    </fieldset>
                </div>
                <div runat="server" id="divUtenzaSede">
                    <fieldset class="ContornoPagina">
                    <legend>Gestione Utenza Sede</legend>
                        <div class="wrapper" style="width:100%;border:0px">
                             <div id="Div1" class="row" runat="server">
                                <div class="collable" style="width:15%">
                                    <asp:label id="LblEmail" CssClass="label" AssociatedControlID="TxtEmail" runat="server" Text="Email Sede" />
                                </div>
                                <div class="colOggetti" style="width:35%">
                                    <asp:TextBox ID="TxtEmail" runat="server"></asp:TextBox>
                                </div>
                                <div class="collable" style="width:15%">
                                    <asp:label id="LabelAbilitato" CssClass="label" 
                                        AssociatedControlID="LblAbilitato" runat="server" Text="Stato Utenza" />
                                </div>
                                <div class="colOggetti" style="width:35%">
                                    <asp:label id="LblAbilitato" CssClass="labelDati" runat="server" />
                                </div>
                            </div>
                            
                            <div id="Div4" class="row" runat="server">
                                <div class="collable" style="width:15%">
                                    <asp:label id="LabelUser" CssClass="label" AssociatedControlID="LblUserName" runat="server" Text="UserName" />
                                </div>
                                <div class="colOggetti" style="width:35%">
                                    <asp:label id="LblUserName" CssClass="labelDati" runat="server" />
                                </div>
                            </div>
                            <br />
                            <h3>
                                <asp:label id="LblMsgUtenza" runat="server"
                                    CssClass="msgErrore"></asp:label>
                            </h3>
                            <br />
                            <div id="Div5" class="row" runat="server">
                                <div class="wrapper" style="width:100%;border:0px">
                                    <div class="RigaPulsanti" style="margin-right:5px">
                                        <asp:Button ID="cmdAttiva" runat="server" CssClass="Pulsante" Text="Attiva Utenza"  />&nbsp;
                                        <asp:Button ID="cmdDisattiva" runat="server" CssClass="Pulsante" Text="Disattiva Utenza" />&nbsp;
                                        <asp:Button ID="cmdInoltroPW" runat="server" CssClass="Pulsante" Text="Inoltro Password" />&nbsp;
                                        <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </div>
                <fieldset class="ContornoPagina">
                <legend>Sedi Gestite</legend>
                    <div class="wrapper" style="width:100%;border:0px">
                        <div id="Div2" class="row" runat="server">
                            <div class="collable" style="width:15%">
                                <asp:label id="LblCodiceSedeAgg" CssClass="label" AssociatedControlID="TxtCodiceSede" runat="server" Text="Codice Sede" />
                            </div>
                            <div class="colOggetti" style="width:35%">
                                <asp:TextBox ID="TxtCodiceSede" runat="server" ></asp:TextBox>
                            </div>
                            <div class="collable" style="width:15%">
                                <asp:Button ID="cmdAggiungi" runat="server" CssClass="Pulsante" Text="Aggiungi Sede"/>
                            </div>
                        </div>
                        <br />
                        <h3>
                        <asp:label id="lblmessaggio" runat="server" CssClass="msgErrore"></asp:label>
                        </h3>
                        <br />
                        
                        <asp:datagrid id="dgSediAssegnate" runat="server" Width="100%" 
                                ToolTip="Elenco Sedi" CssClass="table"  AllowPaging="True" 
                                AllowSorting="True" AutoGenerateColumns="False" 
                                UseAccessibleHeader="True" >
	                        <FooterStyle></FooterStyle>
		                    <SelectedItemStyle ></SelectedItemStyle>
		                    <EditItemStyle></EditItemStyle>
		                    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		                    <ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		                    <HeaderStyle ></HeaderStyle>
	                        <Columns>
                                <asp:BoundColumn DataField="identesedeattuazione" HeaderText="Codice Sede"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Denominazionesede" HeaderText="Sede"></asp:BoundColumn>
                                <asp:BoundColumn DataField="indirizzo" HeaderText="Indirizzo"></asp:BoundColumn>
                                <asp:BoundColumn DataField="comune" HeaderText="Comune"></asp:BoundColumn>
                                
                                  <asp:TemplateColumn HeaderText="Cancella Sede" > 
                                 <ItemTemplate>
									<asp:ImageButton ID="ImageButton1"  CommandName="Cancella" AlternateText="Cancella Sede" ToolTip="Cancella Sede" runat="server" ImageURL="Images/canc_small.png"></asp:ImageButton>  
                                    </ItemTemplate>
                                 </asp:TemplateColumn>
                                <asp:BoundColumn DataField="idsedepassworddelega" Visible="False">
                                </asp:BoundColumn>
                            </Columns>
	                        <HeaderStyle></HeaderStyle>
                            <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
                        </asp:datagrid>
                        
                    </div>
                </fieldset>
        </div>
    </fieldset>       
</asp:Content>
