<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="wfrmExportRiattivazione.aspx.vb" Inherits="Futuro.wfrmExportRiattivazione" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
        <legend>Esportazione File Riattivazione Progetti</legend>
        <div id="TabellaImportAssenzeVolontari" class="wrapper" style="width: 100%">
            <div class="headers">
                <h2>
                    <asp:Label ID="Label5" runat="server" Text="Export File Riattivazione Progetti"></asp:Label>
                </h2>
            </div>
            <div class="RigaVuota">
                &nbsp;</div>
                <div>
            <asp:Label ID="lblMessaggioErrore" CssClass="msgErrore"
                            runat="server" Text=""></asp:Label>
            </div>
            <br />
                <div class="row">
           <div class="collable" style="width:25%">
                <asp:Label ID="lblsino" runat="server" CssClass="label" Text="Tipo Esportazione"></asp:Label>
              </div>
             <div class="colOggetti" style="width:35%">
                <asp:DropDownList ID="ddlSiNo" CssClass="ddlClass"  Runat="server">
                <asp:ListItem Value="SI">Tutte le sedi</asp:ListItem>
				<asp:ListItem Value="NO">Solo sedi con dati inseriti</asp:ListItem>
                </asp:DropDownList>
            </div>
         </div>
            <div class="row">
            
                

                <asp:Label CssClass="label" ID="Label2" runat="server" Width="330px" Text="Esportare il file Riattivazione Progetti   .csv"></asp:Label>
                <!--<asp:HyperLink ID="HplFileAssenze" NavigateUrl="~/download/Master/assenzevolontari.csv"
                    runat="server">File assenzevolontari.csv</asp:HyperLink>-->
            &nbsp;&nbsp;
             <asp:Button ID="imgEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV"
            Visible="true" ToolTip="Esporta nel formato CSV" />
            <asp:hyperlink id="hlVolontari" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" 
        Text="DOWNLOAD CSV" runat="server" 
        ForeColor="#003399" Visible="False"></asp:hyperlink>

            </div>
            <br />
            <br />
            </div>
             <div class="RigaPulsanti" style="text-align:right">
                <asp:Button ID="CmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />&nbsp;
            </div>
        
            </fieldset>
</asp:Content>
