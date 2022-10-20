<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmImportProgrammazione.aspx.vb" Inherits="Futuro.WfrmImportProgrammazione" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">
//<![CDATA[


        function Nascondi() {
            document.all.MainContent_lblMessaggioErrore.style.visibility = 'visible';
            document.all.MainContent_lblMessaggioErrore.style.fontSize = '1.6em'
            document.all.MainContent_lblMessaggioErrore.style.fontWeight = 'bold'
            document.all.MainContent_lblMessaggioErrore.style.color = '#3a4f63'
            document.all.MainContent_lblMessaggioErrore.innerText = 'ATTENDERE........';
            document.all.MainContent_CmdElabora.style.visibility = 'hidden';
        }
 
//]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Importazione Programmazione</legend>
        <div id="TabellaImportProgrammazione" class="wrapper" style="width: 100%">
        <div class="headers">
            <h2>
                <asp:Label ID="Label5" runat="server" Text="Importazione Programmazione"></asp:Label>
            </h2>
        </div>
        <div class="RigaVuota">  &nbsp;</div>
           
            <div class="row">
                <div class="colOggetti" style="width:25%">
                    <asp:Label ID="LblEsporta" runat="server" Text="Scaricare elenco Verificari "/>
                </div>
                <div class="colOggetti" style="width:75%">
                    <asp:Button ID="cmdEsporta" runat="server" ToolTip="Esporta" CssClass="Pulsante" text="Esporta" />
                    &nbsp;<asp:HyperLink ID="hlVerificatore"  Target="_blank" Text="Scarica Elenco Veriifcatori"   runat="server" ForeColor="#003399" Visible="False"/>
                    <asp:Label ID="lblErr" CssClass="msgErrore" runat="server" Text=""></asp:Label><br />
                </div>
            </div>
            <div class="row">
                <div class="colOggetti" style="width:100%">
                    <asp:Label ID="Label3" runat="server" Width="330px" Text=" Scaricare il file programmazione.csv "></asp:Label>
                    <asp:HyperLink ID="HplFileProgrammazione" NavigateUrl="~/download/Master/programmazione.csv" runat="server">File programmazione.csv</asp:HyperLink>
                </div>
            </div>

            <div class="row">
                <div class="colOggetti" style="width:100%">          
                    <asp:Label ID="lblUpLoad" runat="server" AssociatedControlID="txtSelFile" Text="Seleziona File: "></asp:Label>
                    <asp:FileUpload ID="txtSelFile" runat="server" />&nbsp;
                    <asp:Button ID="CmdElabora" OnClientClick="Nascondi()" CssClass="Pulsante" runat="server" Text="Elabora" /> &nbsp;
                    <asp:Label ID="lblMessaggioErrore" CssClass="msgErrore" runat="server" Text="" />           
                </div>
            </div>
            <div class="row" id="divForzaCaricamentoVerifiche" runat="server" visible="false">
                <div class="colOggetti" style="width:100%">
                    <asp:checkbox id="ChkForzaCaricamento" runat="server" Font-Size="Small" AutoPostBack="True" Text="Carica Progetti Terminati"></asp:checkbox>
                </div>
            </div>
            <div class="RigaPulsanti" > 
                <asp:Button ID="CmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
            </div>
      </div>
    </fieldset>
</asp:Content>
