<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRisultatoImportProgrammazione.aspx.vb" Inherits="Futuro.WfrmRisultatoImportProgrammazione" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Risultato Importazione Programmazione</legend>
        <div id="TabellaImportProgrammazione" class="wrapper" style="width: 100%">
            <div class="headers">
                <h2>
                    <asp:Label ID="Label5" runat="server" Text="Risultato Importazione Programmazione"></asp:Label>
                </h2>
            </div>
            <div class="RigaVuota">&nbsp;</div>       
            <div class="row">
                <div class="colOggetti" style="width:15%">
                    <asp:Label ID="LblDescrizione" CssClass="label" AssociatedControlID="TxtDescrizione"  runat="server" Text="Programmazione"/>
                </div>
                 <div class="colOggetti" style="width:85%">
                    <asp:textbox id="TxtDescrizione" runat="server" CssClass="textbox" />
                 </div>
            </div>
            <div class="row">
                <div class="colOggetti" style="width:15%">
                    <asp:Label ID="LblDataInizio" CssClass="label" AssociatedControlID="TxtDataInizio"  runat="server" Text="Data Inizio"/>
                </div>
                <div class="colOggetti" style="width:35%" >
                    <asp:textbox id="TxtDataInizio" runat="server" CssClass="textbox" />
                </div>
                <div class="colOggetti" style="width:15%">
                    <asp:Label ID="LblDataFine" CssClass="label" AssociatedControlID="TxtDataFine"  runat="server" Text="Data Fine"/>
                </div>
                <div class="colOggetti" style="width:35%" >
                    <asp:textbox id="TxtDataFine" runat="server" CssClass="textbox" />
                </div>
            </div>
            <div class="row">
                <div class="colOggetti" style="width:15%">
                    <asp:Label ID="LblRegioneCompetenza" CssClass="label" AssociatedControlID="ddlCompetenza"  runat="server" Text="Competenza"/>
                </div>
                 <div class="colOggetti" style="width:85%">
                    <asp:dropdownlist id="ddlCompetenza" runat="server" CssClass="ddlClass" />
                 </div>
            </div>
            <div class="row">
                <div class="colOggetti" style="width:15%">
                    <asp:Label ID="lblCircolare" CssClass="label" AssociatedControlID="ddlBando"  runat="server" Text="Circolare"/>
                </div>
                 <div class="colOggetti" style="width:85%">
                    <asp:dropdownlist id="ddlBando" runat="server" CssClass="ddlClass" />
                 </div>
            </div>
            <div class="row">
                <div class="colOggetti" style="width:15%">
                    <asp:Label ID="LblNote" CssClass="label" AssociatedControlID="TxtNote"  runat="server" Text="Note"/>
                </div>
                 <div class="colOggetti" style="width:85%">  
                    <asp:textbox id="TxtNote" runat="server" CssClass="textbox"  
                         TextMode="MultiLine" Width="424px" />
                 </div>
            </div>
            <br />
            <asp:Label ID="LblErrore" AssociatedControlID="dtgCSV" runat="server" Visible="false" CssClass="msgErrore"></asp:Label>
            <asp:Label ID="lblEsito"  AssociatedControlID="dtgCSV" runat="server" Visible="false" CssClass="msgConferma"></asp:Label>
            <br />
            <div class="collable" style="width:100%">
                <asp:Label ID="lblTotali" CssClass="bold" AssociatedControlID="hlDownLoad" runat="server"></asp:Label>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
              <div id="AvvisoConferma" runat="server" visible="false">
     <ul style="border-color:Red;border-style:ridge;margin:1% 20% 1% 20%;text-align:left;" runat="server" id="avviso">
        <dt style="margin-left:5px;font-size:1.6em;font-weight:bold;color:Red">Attenzione!!!</dt>
        <dt style="margin-left:5px;font-size:1.2em;font-weight:bold" runat="server" id="testoavviso"></dt>
       <%-- <li style="margin-left:5px;font-size:1.4em;font-weight:bold;">Il bando dei volontari funzionerà normalmente. Il concorso “Realizzazione di uno spot televisivo e di un cortometraggio di promozione del Servizio Civile Nazionale” funzionerà regolarmente.</li>--%>

    </ul>
    </div>
           <p><asp:HyperLink ID="hlDownLoad" CssClass="linkStampa" runat="server">Scarica il file completo nel formato CSV</asp:HyperLink></p>
            <div class="RigaPulsanti">
                <asp:Button ID="CmdConferma" runat="server" CssClass="Pulsante" Text="Conferma" />&nbsp;
                <asp:Button ID="CmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
            </div>
            <br />
            <div class="wrapper" style="width:100%; border-style:none">
            <div class="RigaVuota" >&nbsp;</div>
            <asp:DataGrid ID="dtgCSV" runat="server" Width="100%" CssClass="table" CellPadding="3" PageSize="5" AutoGenerateColumns="True">
				<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr"></ItemStyle>
		        <HeaderStyle></HeaderStyle>
			</asp:DataGrid>
            <div class="RigaVuota" >&nbsp;</div>

        </div>
      </div>
    </fieldset>
      
        </div>
      
</asp:Content>
