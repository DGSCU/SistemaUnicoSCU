<%@ Page Title="Dettaglio CheckList Formazione"   Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmCheckListDettaglioFormazione.aspx.vb" Inherits="Futuro.WfrmCheckListDettaglioFormazione" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="firefox">
        <fieldset class="ContornoPaginaScroll">
        <legend>CHECK LIST DETTAGLIO FORMAZIONE</legend>
        <div class="wrapper" style="width:100%">
            <div class="headers" >
                <h2><asp:Label ID="lblTitolo" runat="server"  Text="CHECK LIST FORMAZIONE PER"></asp:Label>&nbsp;
                <asp:Label ID="LblLista" runat="server" Text=""></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row"><asp:Label ID="lblTitolo1" runat="server" Text="Titolo :" ></asp:Label>&nbsp;
                <asp:Label ID="lblTitoloDato" Font-Bold="true" runat="server" Text=""></asp:Label>
            </div>

            <div class="RigaPulsanti" style="text-align:right">
                <asp:Button ID="cmdArchiviata" runat="server" CssClass="Pulsante" 
                    Text="Archivia" Visible="false"/>
                <asp:Button ID="cmdInizializza" runat="server" CssClass="Pulsante" Text="Inizializza" />&nbsp;
                <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Visible="false" Text="Esporta CSV" />&nbsp;
                <asp:Button ID="CmdStampa" runat="server" CssClass="Pulsante" Text="Stampa" />&nbsp;
                <asp:Button ID="CmdConferma" runat="server" CssClass="Pulsante" Text="Conferma" />&nbsp;
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
                <br />
                <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
                &nbsp;
            </div>
           

            <div id="divArchiviazione" runat="server" visible="false" style="width:100%">
            
                <fieldset class="ContornoPagina">
                    <legend>Archiviazione</legend>
                    <div class="wrapper" style="width:100%;border:0px">
                        <asp:label id="lblerrore" runat="server" AssociatedControlID="CboCausaleArchiviazione" Visible="false" CssClass="msgErrore"></asp:label>
                        <div class="RigaVuota">&nbsp;</div>
                        <div class="rowGroup" style="height:auto">
                            <div class="row">
                        <div class="collable" style="width:20%">     
                            <asp:Label ID="lblCausaleArchiviazione" CssClass="label" AssociatedControlID="CboCausaleArchiviazione" runat="server" Text="Causale Archiviazione"/>
                        </div>
                        <div class="colOggetti" style="width:55%">
                            <asp:DropDownList ID="CboCausaleArchiviazione" runat="server"/>                                        
                        </div>
                    </div>
                            <div class="row">
                        <div class="collable" style="width:20%">     
                            <asp:Label ID="LblNoteArchiviazione" CssClass="label" AssociatedControlID="txtNoteArchiviazione" runat="server" Text="Note Archiviazione"/>
                        </div>
                        <div class="colOggetti" style="width:55%">
                            <asp:TextBox ID="txtNoteArchiviazione" runat="server" TextMode="MultiLine" Width="100%" />
                        </div>
                    </div>
                            <div class="RigaPulsanti" style="text-align:right">
                        <asp:Button ID="CmdConfermaArch" runat="server" CssClass="Pulsante" Text="Conferma" Visible="false"/>
                        <asp:Button ID="cmdRipristinata" runat="server" CssClass="Pulsante" Text="Ripristina" Visible="false"/>
                    </div>
                        </div>
                    </div>
                </fieldset>
            </div>

        </div>     
                 
        
       <h3>  <asp:label id="lblmessaggio" runat="server" AssociatedControlID="dgRisultatoRicerca"></asp:label> </h3>
    
    <div>
            <p style="text-align:right">
            
             
                <strong><asp:LinkButton ID="imgStoricoNotifiche" runat="server" style="cursor:pointer;font-size:small;font-weight:bold" Text="Storico Notifiche" Visible="true"></asp:LinkButton></strong>
            
            </p>
            
            </div>
    <fieldset class="ContornoPaginaScroll">
    <legend>CODICE CHECKLIST: <asp:Label ID="lblCodCheckList" runat="server" ></asp:Label></legend>
     <div class="firefox">
        <asp:datagrid id="dgRisultatoRicerca" runat="server" Caption="Dettaglio Formazione" 
                CssClass="table" CellPadding="2" PageSize = "10"  AllowPaging="true" 	
                 AllowSorting="True"  AutoGenerateColumns="False" UseAccessibleHeader="True"  Width="100%">
            <FooterStyle></FooterStyle>
            <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
            <EditItemStyle></EditItemStyle>
            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
            <ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle></HeaderStyle>
            <Columns>
                <asp:BoundColumn Visible="False" DataField="idEntità" HeaderText="idVol"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="idAttività" HeaderText="idAttivita"></asp:BoundColumn>   
                <asp:BoundColumn DataField="CodiceVolontario"     HeaderText="Cod Vol"></asp:BoundColumn>
                <asp:BoundColumn DataField="Nominativo"    HeaderText="Nominativo"></asp:BoundColumn>
                <asp:BoundColumn DataField="CodiceFiscale"    HeaderText="C.F."></asp:BoundColumn>
                <asp:BoundColumn DataField="ProgettoFinanziato"    HeaderText="Prog Fin"></asp:BoundColumn>
                <asp:BoundColumn DataField="Contratto"   HeaderText="Contr"></asp:BoundColumn>
                <asp:BoundColumn DataField="ModuloF"     HeaderText="Mod F"></asp:BoundColumn>
                <asp:BoundColumn DataField="CongruenzaPeriodo"    HeaderText="Period"></asp:BoundColumn>
                <asp:BoundColumn DataField="FirmaVolontari"     HeaderText="Firma Vol"></asp:BoundColumn>
                <asp:BoundColumn DataField="Docenti"    HeaderText="Docenti"></asp:BoundColumn>
                <asp:BoundColumn DataField="FirmaDocenti"    HeaderText="Firma Doce"></asp:BoundColumn>
                <asp:BoundColumn DataField="Esito"  HeaderText="Esito"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Notifica Mail">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbNotifica"  CommandName="Notifica" ToolTip="Inoltro Email" runat="server">NM</asp:LinkButton>          
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Ver. Doc" > 
                    <ItemTemplate>    
                        <asp:LinkButton ID="lbDocumenti" CommandName="Documenti" ToolTip="Verifica Documenti Volontario" runat="server">Doc</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="IdEnte" HeaderText="IdEnte" Visible="false"></asp:BoundColumn>
                <asp:BoundColumn DataField="Denominazione" HeaderText="Denominazione" Visible="false"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Ver. Doc Docent" > 
                    <ItemTemplate>
                        <asp:LinkButton ID="lblDocumenti" CommandName="DocumentiDocent" ToolTip="Verifica Documenti Docente" runat="server">Doc Docent</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
               
                <asp:TemplateColumn HeaderText="Inc"> 
                    <ItemTemplate>
			    	    <asp:ImageButton ID="cmdIncludiSi" ToolTip="Escludi" AlternateText="Escludi" CommandName="Escludi" ImageUrl="~/Images/selezionato_small.png" Visible='<%# IIF(Eval("Incluso").ToString().Equals("1"), True, False) %>' runat="server" />
                        <asp:ImageButton ID="cmdIncludiNo" ToolTip="Includi" AlternateText="Includi" CommandName="Includi" ImageUrl="~/Images/deselezionato_small.png" Visible='<%# IIF(Eval("Incluso").ToString().Equals("0"), True, False) %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateColumn>      
 
            </Columns>
            <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGridRidotto"  Mode="NumericPages"></PagerStyle>
        </asp:datagrid>
    </div>
                
    </fieldset>

        </fieldset>
        </div>
     
</asp:Content>
