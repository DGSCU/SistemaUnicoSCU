<%@ Page Title="Elenco Progetti in Attesa di Graduatoria" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="elencoprogettiattesagraduatoria.aspx.vb" Inherits="Futuro.elencoprogettiattesagraduatoria" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
<legend>Variazioni Ente</legend>
     <div class="wrapper" style="width:100%">
      <div class="headers" >
            <h2>
            <asp:Label ID="lblTitolo" runat="server"  Text="Progetti in Attesa di Graduatoria"></asp:Label>
            </h2>
        </div>
        <br />
          <div class="row" >

        <asp:Label ID="lblInfo"  CssClass="msgInfo" runat="server" ></asp:Label> 
       </div>
    <div class="row" >
      <div class="collable" style="width:15%">
        <asp:Label ID="lblCompetenza"  AssociatedControlID="ddlRegioni"   runat="server" Text="Competenza"></asp:Label> 
       </div>
       <div class="colOggetti" style="width:85%">           
         <asp:DropDownList ID="ddlRegioni"  runat="server"  AutoPostBack="true">  </asp:DropDownList>
       </div>
    </div>

        <div class="RigaPulsanti" > 
    <asp:Button id="cmdChiudi" CssClass="Pulsante"  runat="server" Text="Chiudi" ></asp:Button>
          <asp:Button ID="cmdEsporta" runat="server" ToolTip="Esporta OLP su sedi diverse" CssClass="Pulsante" 
                Text="Esporta CSV" />
            &nbsp;
            <br />
            <asp:HyperLink ID="ApriCSV1" runat="server" AccessKey="S" CssClass="linkStampa" 
                ForeColor="#003399" Text="DOWNLOAD CSV" 
                ToolTip="Link per la stampa del risultato della ricerca" Visible="False"></asp:HyperLink>
</div>
    </div>
    </fieldset>
</asp:Content>
