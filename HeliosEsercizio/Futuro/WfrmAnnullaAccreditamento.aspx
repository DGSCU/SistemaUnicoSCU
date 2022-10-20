<%@ Page Title="Annulla Iscrizione" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmAnnullaAccreditamento.aspx.vb" Inherits="Futuro.WfrmAnnullaAccreditamento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
    <legend>Sede</legend>
    
    <div class="wrapper"  style="width:100%;border-width:0px">
                <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Annulla Iscrizione "></asp:Label>
               <asp:Label ID="Label1" runat="server"  ></asp:Label>
               </h2>
            </div>
              <div class="RigaVuota" >&nbsp;
              </div>
     <div class="row" >
     <div class="collable" style="width:15%">
        <asp:Label id="ente" AssociatedControlID="lblEnte" CssClass="label" runat="server" Text="Ente" ></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
        <asp:Label id="lblEnte" runat="server" CssClass="labelDati" Visible="true"></asp:Label>
     </div>

     <div class="collable" style="width:15%">
        <asp:Label id="sede" AssociatedControlID="lblsede" CssClass="label" runat="server" Text="Sede"></asp:Label>
    </div>
     <div class="colOggetti" style="width:35%">
        <asp:Label id="lblsede" runat="server" CssClass="labelDati"  Visible="true"></asp:Label>
     </div>
            </div>
   
   <div class="row" >
     <div class="collable" style="width:15%">
        <asp:Label id="lblsedeAtt" AssociatedControlID="lblSedeAttuazione" CssClass="label" runat="server" Text="Sede Attuazione" ></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%">
        <asp:Label id="lblSedeAttuazione" runat="server" CssClass="labelDati" Visible="true"></asp:Label>
     </div>
  
   </div>
   <div class="row" >
     <div class="collable" style="width:15%">
        <asp:Label id="indirizzo" AssociatedControlID="ddlStato" CssClass="label" runat="server" Text="Stato da associare alla Sede" ></asp:Label>
    </div>
    <div class="colOggetti" style="width:85%">
        <asp:dropdownlist id="ddlStato" runat="server" CssClass="ddlClass" Visible="true"></asp:dropdownlist>
     </div>

   </div>
 
    <div class="RigaPulsanti">
    <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" Text="Salva"   />
        <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"   />
    </div>

    </div>
    <asp:HiddenField ID="lblAcquisita" runat="server"/>
    <asp:HiddenField ID="lblTipoAzione" runat="server"/>
    <asp:HiddenField ID="lblIdSede" runat="server"/>
    <asp:HiddenField ID="lblidEnte" runat="server"/>
    <asp:HiddenField ID="lblTipologia" runat="server"/>
     <asp:HiddenField ID="lblstatoSedeFisica" runat="server"/>
      <asp:HiddenField ID="lblidSedeAtt" runat="server"/>
    </fieldset>



</asp:Content>
