<%@ Page Title="Inoltro Password" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WebInvioPassword.aspx.vb" Inherits="Futuro.WebInvioPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Inoltro Password</legend>
        <asp:label id="lblMessaggio"  runat="server" CssClass="msgErrore" Visible="false"></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:70%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Inoltro Password"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
                <div class="collable" style="width:20%">&nbsp;</div>
                <div class="colOggetti" style="width:80%">       
                    <asp:label id="lblEnte" CssClass="bold" runat="server"></asp:label>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:20%">      
                    <asp:Label ID="lblUtente" CssClass="label" AssociatedControlID="txtUser" runat="server" Text="(*)Utente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:30%">   
                    <asp:TextBox ID="txtUser" CssClass="textbox" runat="server" ReadOnly="true"></asp:TextBox>        
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblPassword" CssClass="label" AssociatedControlID="txtPassword" runat="server" Text="Password"></asp:Label>
                </div>
                <div class="colOggetti" style="width:30%">   
                    <asp:TextBox ID="txtPassword" CssClass="textbox" runat="server" ReadOnly="true" TextMode="Password"></asp:TextBox>        
                </div>       
            </div>
            <div class="row">
                <div class="collable" style="width:20%">      
                    <asp:Label ID="lblDa" CssClass="label" AssociatedControlID="txtDA" runat="server" Text="(*)Da"></asp:Label>
                </div>
                <div class="colOggetti" style="width:80%">   
                    <asp:TextBox ID="txtDA" CssClass="textbox" runat="server" ReadOnly="true"></asp:TextBox>        
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:20%">      
                    <asp:Label ID="lblA" CssClass="label" AssociatedControlID="txtA" runat="server" Text="(*)A"></asp:Label>
                </div>
                <div class="colOggetti" style="width:80%">   
                    <asp:TextBox ID="txtA" CssClass="textbox" runat="server" ReadOnly="true"></asp:TextBox>        
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:20%">      
                    <asp:Label ID="lblCC" CssClass="label" AssociatedControlID="txtCC" runat="server" Text="CC"></asp:Label>
                </div>
                <div class="colOggetti" style="width:80%">   
                    <asp:TextBox ID="txtCC" CssClass="textbox" runat="server" ReadOnly="true"></asp:TextBox>        
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:20%">      
                    <asp:Label ID="lblOggetto" CssClass="label" AssociatedControlID="txtOggetto" runat="server" Text="(*)Oggetto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:80%">   
                    <asp:TextBox ID="txtOggetto" CssClass="textbox" runat="server" ReadOnly="true"></asp:TextBox>        
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:20%">      
                    <asp:Label ID="lblMsg" CssClass="label" AssociatedControlID="txtmessaggio" runat="server" Text="(*)Messaggio"></asp:Label>
                </div>
                <div class="colOggetti" style="width:80%">   
                    <asp:TextBox ID="txtmessaggio" CssClass="textbox" runat="server" ReadOnly="true" TextMode="MultiLine" Rows="5"></asp:TextBox>        
                </div>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:20%">      
                    <asp:Label ID="lblNote" CssClass="label" AssociatedControlID="txtNote" runat="server" Text="Note"></asp:Label>
                </div>
                <div class="colOggetti" style="width:55%">   
                    <asp:TextBox ID="txtNote" CssClass="textbox" runat="server" TextMode="MultiLine" Rows="5"></asp:TextBox>        
                </div>
                <div class="colOggetti" style="width:20%">
                    <asp:checkbox id="ChKAccodamess" runat="server" AutoPostBack="true" Text="Aggiungi al messaggio"></asp:checkbox>
                 </div>  
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdInvia" CssClass="Pulsante" runat="server" Text="Conferma" />
                <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
            </div>
        </div>
    </fieldset>
    <asp:HiddenField ID="txtrisultatoricerca" runat="server" />
</asp:Content>
