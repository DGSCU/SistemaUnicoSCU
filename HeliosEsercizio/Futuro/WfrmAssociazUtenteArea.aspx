<%@ Page Title="Associazione Utenza ad Area di Riferimento Gestione Modelli" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmAssociazUtenteArea.aspx.vb" Inherits="Futuro.WfrmAssociazUtenteArea" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Associazione Utenza ad Area di Riferimento Gestione Modelli</legend>
        <asp:label id="LblSalvataggio"  runat="server" CssClass="msgErrore" Text="Salvataggio Avvenuto Con Successo."></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitoloPagina" runat="server"  Text="Associazione Utenza ad Area di Riferimento Gestione Modelli"></asp:Label></h2>
            </div>
            <div class="RigaVuota">&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblUtenteLabel" CssClass="label" AssociatedControlID="LblUtente" runat="server" Text="Utente"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="LblUtente" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblProfiloLabel" CssClass="label" AssociatedControlID="LblProfilo" runat="server" Text="Profilo"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="LblProfilo" CssClass="bold" runat="server"></asp:label>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblCognomeLabel" CssClass="label" AssociatedControlID="LblCognome" runat="server" Text="Cognome"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="LblCognome" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblNomeLabel" CssClass="label" AssociatedControlID="LblNome" runat="server" Text="Nome"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="LblNome" CssClass="bold" runat="server"></asp:label>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblCompetenzaLabel" CssClass="label" AssociatedControlID="LblCompetenza" runat="server" Text="Competenza"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="LblCompetenza" CssClass="bold" runat="server"></asp:label>         
                </div>
            </div>
        </div>
        <div class="wrapper" style="width:100%">
            <div class="RigaVuota">&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblAccreditamento" CssClass="label" AssociatedControlID="ChkAccredit" runat="server" Text="Accreditamento"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:CheckBox ID="ChkAccredit" runat="server" />       
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblVolontari" CssClass="label" AssociatedControlID="ChKVolontari" runat="server" Text="Volontari"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:CheckBox ID="ChKVolontari" runat="server" />         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblProgetti" CssClass="label" AssociatedControlID="ChKProgetti" runat="server" Text="Progetti"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:CheckBox ID="ChKProgetti" runat="server" />       
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblMonitoraggio" CssClass="label" AssociatedControlID="ChKMonitoraggio" runat="server" Text="Monitoraggio"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:CheckBox ID="ChKMonitoraggio" runat="server" />         
                </div>
            </div>
        </div>
        <div class="wrapper" style="width:100%; border-style:none">
            <div class="RigaVuota">&nbsp;</div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" Text="Salva" />
                <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
            </div>
        </div>
    </fieldset>
</asp:Content>
