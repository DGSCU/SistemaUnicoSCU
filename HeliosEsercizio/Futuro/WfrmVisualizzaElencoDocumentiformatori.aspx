<%@ Page Title="Gestione Documenti Presenze Formatori" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmVisualizzaElencoDocumentiformatori.aspx.vb" Inherits="Futuro.WfrmVisualizzaElencoDocumentiformatori" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
 <script type="text/javascript">
     function infoPrefissi() {

         window.open("wfrmInfoPrefissiDocumentiFormatore.aspx?VengoDA='GestioneDocumenti'", "Elenco", "height=450,width=600,dependent=no,scrollbars=yes,status=no");
     }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <fieldset class="ContornoPagina">
        <legend>Gestione Documenti Presenze Formatori</legend>
        <div class="wrapper" style="width:100%">
            <div class="headers">
                <h2><asp:Label ID="lblTitoloPagina" runat="server"  Text="Gestione Documenti Presenze Formatori"></asp:Label></h2>
            </div>
               <div class="rowGroup" style="height:auto">
            <div class="row" style="height:auto">
            <asp:Label ID="lblConferma"  runat="server" CssClass="msgConferma"></asp:Label>
             <asp:Label ID="lblErrore"  runat="server" CssClass="msgErrore"></asp:Label>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            </div>
            <fieldset class="ContornoPagina">
                <legend>Informazioni Progetto</legend>
                <div class="wrapper" style="width:100%;border:0px">
                    <div id="DivDatiVol" class="row" runat="server">
                        <div class="collable" style="width:15%">
                            <asp:label id="LabelCodiceProg" CssClass="label" AssociatedControlID="lblCodiceProg" runat="server" Text="Codice Progetto" />
                        </div>
                        <div class="colOggetti" style="width:35%">
                            <asp:label id="lblCodiceProg" CssClass="labelDati" runat="server" />
                        </div>
                        <div class="collable" style="width:15%">
                            <asp:label id="LabelTitolo" CssClass="label" AssociatedControlID="lblTitolo" runat="server" Text="Titolo" />
                        </div>
                        <div class="colOggetti" style="width:35%">
                            <asp:label id="lblTitolo" CssClass="labelDati" runat="server" />
                        </div>
                    </div>
                </div>
            </fieldset>
            <fieldset class="ContornoPagina">
                <legend>Carica Documenti Presenze Formatori</legend>
                <div class="wrapper" style="width:99%;border:0px">

                    <div style="vertical-align:middle;">
                        <p>
                            <asp:ImageButton  id="imgPrefissiDocumenti" runat="server" style="cursor:hand" ImageUrl="images/info_small.png" OnClientClick="javascript:infoPrefissi()"  ToolTip="Prefissi Documenti" AlternateText="Prefissi Documenti"/>
                            <asp:label ID="LblSel" CssClass="label" AssociatedControlID="txtSelFile" runat="server" Text="Selezionare il file" ></asp:label>
                             <input ID="txtSelFile" type="file" style="width:70%"  runat="server" />
                            <asp:Button  id="cmdUpload" runat="server" CssClass="Pulsante"  Text="Upload File" />&nbsp;
                        </p>
                    </div>
                   <br />
                    <h3>
                        <asp:label id="LblMsgFile" runat="server" ></asp:label>
                    </h3>
                          
                </div>
            </fieldset>
            <fieldset class="ContornoPagina">
                <legend>Elenco Documenti Presenze Formatori</legend>
               <p style="width:100%">
                            <asp:label id="LblTipoDocumento" CssClass="label" AssociatedControlID="ddlTipoDocumenti" runat="server" Text="Tipo Documento" />
                            
                            <asp:DropDownList ID="ddlTipoDocumenti" Width="72%"  runat="server"/>
                       
                        <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />
                       
                   
           <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
                       
                   
                   </p>
                
                
            <h3>
                <asp:label id="lblmessaggio" runat="server"></asp:label>
            </h3>
                  
            <asp:datagrid id="dgElencoDocumenti" runat="server" Width="100%" 
                    ToolTip="Elenco Documenti Presenze Formatori" CssClass="table" 
                    AllowSorting="True" AutoGenerateColumns="False" 
                    UseAccessibleHeader="True"
                     >
	                 <FooterStyle></FooterStyle>
		        <SelectedItemStyle ></SelectedItemStyle>
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		        <HeaderStyle ></HeaderStyle>
	            <Columns>
                    
                    <asp:BoundColumn DataField="IdAttivitàDocumentoFormazione" HeaderText="IdAttivitàDocumentoFormazione" 
                        Visible="False"></asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="Download"> 
                        <ItemTemplate>
	                        <asp:ImageButton ID="imgScaricaDoc" style="cursor:pointer;" CommandName="Download" alt="Scarica Documento" ToolTip='Scarica Documento' runat="server" ImageURL="images/giu_small.png" CausesValidation="false"></asp:ImageButton>  
                         </ItemTemplate>
                    </asp:TemplateColumn>
          
                    <asp:BoundColumn DataField="Nomefile" HeaderText="Nome File">
                        <HeaderStyle Width="40%" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="DataInserimento" HeaderText="Data Inserimento">
                        <HeaderStyle Width="10%" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="HashValue" HeaderText="Hash Value">
                        <HeaderStyle Width="20%" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Tipologia" HeaderText="Tipologia Documento">
                        <HeaderStyle Width="10%" />
                    </asp:BoundColumn>
                      <asp:BoundColumn DataField="StatoDocumentoTesto" HeaderText="Stato Documento">
                        <HeaderStyle />
                    </asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="Rimuovi"> 
                        <ItemTemplate>
	                        <asp:ImageButton ID="imgCancDoc"  style="cursor:pointer;" CommandName="Elimina" alt="Rimuovi Documento" ToolTip='Rimuovi Documento' runat="server" ImageURL="images/vistacattiva_small.png" CausesValidation="false"  Visible='<%# IIF(Eval("PulsanteCancellaDocumento").ToString().Equals("NO"), False, True) %>' ></asp:ImageButton>  
                         </ItemTemplate>
                    </asp:TemplateColumn>
                        <asp:TemplateColumn  HeaderText="Valido"> 
                        <ItemTemplate>
				            <asp:ImageButton ID="imgValida"  CommandName="Valida" AlternateText="Documento Valido" 
                                ToolTip="Valida Documento" runat="server" ImageURL="Images/selezionato_small.png" ></asp:ImageButton>  
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn  HeaderText="Non Valido"> 
                        <ItemTemplate>
			    	        <asp:ImageButton ID="imgNonValida"  CommandName="Invalida" AlternateText="Documento non Validato" 
                                ToolTip="Documento Non Valido" runat="server" ImageURL="Images/deselezionato_small.png" ></asp:ImageButton>  
                        </ItemTemplate>
                    </asp:TemplateColumn>
                     <asp:BoundColumn DataField="StatoDocumento" HeaderText="Stato Documento" Visible="false">
                        <HeaderStyle Width="0%" />
                    </asp:BoundColumn>
                    
                </Columns>
	            <HeaderStyle></HeaderStyle>
                <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
            </asp:datagrid>
            <p>
            <asp:HyperLink ID="hlDownload"  runat="server"  Visible="false"></asp:HyperLink>
           </p>
            </fieldset>
           
        </div>
</fieldset>
</asp:Content>
