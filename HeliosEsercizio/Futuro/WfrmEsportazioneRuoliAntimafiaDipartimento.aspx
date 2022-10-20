<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmEsportazioneRuoliAntimafiaDipartimento.aspx.vb" Inherits="Futuro.WfrmEsportazioneRuoliAntimafiaDipartimento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
	<legend >Esportazione Ruoli Antimafia (Dipartimento)</legend>
    <div id="divForbidden" runat="server" visible="false">
        <asp:label id="lblForbidden"  runat="server" CssClass="msgErrore">Attenzione: l'utente collegato non ha accesso a questa funzionalità.</asp:label>
    </div>
    <div id="divEsporta" runat="server">
            <div class="row" >
                <div class="collable" >      
                    <asp:Label ID="LblEnteEsportazione" CssClass="msgInfo" runat="server" Text=""></asp:Label>
                </div>
            </div> 
             <div class="rigavuota">
                 &nbsp;
             </div>
            <div class="row" >
                <div class="collable" >      
                    <asp:Label ID="LblMsgEsportazione" CssClass="msgConferma" runat="server" Text=""></asp:Label>
                </div>
            </div> 
             <div class="rigavuota">
                 &nbsp;
             </div>

            <div class="Row" style="width:30%">
                <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
                <asp:HiddenField ID="hdIdEnteFase" runat="server" Value="" />
                <asp:HiddenField ID="HdEnte" runat="server" Value="" />
            </div>
            <div class="rigavuota">
                &nbsp;
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="btnTest" runat="server" CssClass="Pulsante" Text="Visualizzazione CSV" />
                <asp:Button ID="btnExport" runat="server" CssClass="Pulsante" Text="Esportazione per Invio a BDNA" />
                <asp:Button ID="btnChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
            </div>
    </div>
    <div id="divRicerca" runat="server">
        <asp:label id="lblmess"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
                <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Enti da Esportare"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDenominazione" CssClass="label" AssociatedControlID="txtdenominazione" runat="server" Text="Denominazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtdenominazione" CssClass="textbox" runat="server" MaxLength="200"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodiceEnte" CssClass="label" AssociatedControlID="txtCodiceEnte" runat="server" Text="Cod. Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodiceEnte" CssClass="textbox" runat="server" MaxLength="10"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblClasseAttribuita" CssClass="label" AssociatedControlID="ddlClasseAttribuita" runat="server" Text="Sezione Attribuita"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlClasseAttribuita" runat="server" CssClass="ddlClass"></asp:DropDownList>             
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblStatoAntimafia" CssClass="label" AssociatedControlID="ddlStatoAntimafia" runat="server" Text="Stato dichiarazione antimafia"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlStatoAntimafia" runat="server" CssClass="ddlClass">
                    <asp:ListItem Value="0">TUTTI</asp:ListItem>
				    <asp:ListItem Value="1">Dati antimafia non presenti</asp:ListItem>
				    <asp:ListItem Value="2">Dati antimafia in aggiornamento</asp:ListItem>
				    <asp:ListItem Value="3">Dati antimafia da esportare</asp:ListItem>
				    <asp:ListItem Value="4">Dati antimafia esportati da più di sei mesi</asp:ListItem>
				    <asp:ListItem Value="5">Dati antimafia esportati e non ancora scaduti</asp:ListItem>
                    </asp:DropDownList>

                </div>

            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblStatoEnte" CssClass="label" AssociatedControlID="ddlStatoEnte" runat="server" Text="Stato Ente" Visible="true"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlStatoEnte" runat="server" CssClass="ddlClass" Visible="true"></asp:DropDownList>             
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblClasseRichiesta" CssClass="label" AssociatedControlID="ddlClasseRichiesta" runat="server" Text="Sezione Richiesta" Visible="false"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlClasseRichiesta" runat="server" CssClass="ddlClass" Visible="false"></asp:DropDownList>             
                </div>
            </div>            
            <div class="RigaPulsanti">
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
                <asp:Button ID="cmdEsporta" CssClass="Pulsante" runat="server" 
                    Text="Esporta CSV" Visible="False" />
 		        <br />
	            <asp:HyperLink ID="ApriCSV2" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>&nbsp;
           </div>
        </div>
        <br />
        <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco Enti" CssClass="table" CellPadding="2"  AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
		    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
		    <Columns>
                <asp:TemplateColumn HeaderText="Sel"> 
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px"></HeaderStyle>
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgEnte" CommandName="Select" ToolTip="Seleziona Ente" AlternateText="Seleziona Ente" runat="server" ImageURL="images/ente-mini.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
			    <asp:BoundColumn DataField="Codiceregione" HeaderText="Cod.Ente">
				    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px"></HeaderStyle>
				    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn DataField="Denominazione" HeaderText="Denominazione">
				    <HeaderStyle HorizontalAlign="Center" Width="500px"></HeaderStyle>
				    <ItemStyle HorizontalAlign="Center"></ItemStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn DataField="Sezione" HeaderText="Sezione">
				    <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn DataField="StatoEnte" HeaderText="Stato Ente">
                </asp:BoundColumn>
			    <asp:BoundColumn DataField="stato" HeaderText="Stato Dichiarazione Antimafia">
				    <HeaderStyle HorizontalAlign="Center" Width="200px"></HeaderStyle>
			    </asp:BoundColumn>
                <asp:BoundColumn DataField="DataChiusuraFase" HeaderText="Chiusura aggiornamento">
				    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
			    </asp:BoundColumn>
               <asp:BoundColumn DataField="UltimaEsportazioneDati" HeaderText="Data Ultima Esportazione per BDNA">
				    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn Visible="False" DataField="idente" HeaderText="idEnte">
				    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn Visible="False" DataField="CodiceFiscale" HeaderText="CodiceFiscale">
				    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn Visible="False" DataField="Tipologia" HeaderText="Tipologia">
				    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn Visible="False" DataField="Provincia" HeaderText="Provincia">
				    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn Visible="False" DataField="Comune" HeaderText="Comune">
				    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
			    </asp:BoundColumn>

		    </Columns>
		    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
        </asp:datagrid>
       <asp:HiddenField ID="hdsElencoEntiPerAntiMafia" runat="server" />
    </div>
    </fieldset>
</asp:Content>
