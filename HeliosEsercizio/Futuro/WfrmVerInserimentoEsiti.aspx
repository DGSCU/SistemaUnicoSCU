<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmVerInserimentoEsiti.aspx.vb" Inherits="Futuro.WfrmVerInserimentoEsiti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

						


     <fieldset id="Esiti" class="ContornoPagina" runat="server">
        <legend>Inserimeneto Esito</legend>
        <asp:label id="lblmsg" AssociatedControlID="Esiti" Visible="false" CssClass="msgErrore" Text=" " runat="server"></asp:label>
        <div class="wrapper" style="width: 100%">
            <div class="headers">
                <h2>
                    <asp:Label ID="lblTitoloPagina" runat="server" AssociatedControlID="Esiti" ForeColor="White" Text="Inserimeneto Esito"></asp:Label>
                </h2>
            </div>
            <div class="RigaVuota">
                &nbsp;
            </div>
            <div class="row" >
                <div class="colOggetti" style="width:10%">
								        <asp:label id="LblDescrizione" AssociatedControlID="txtDescrizione" runat="server">Esito</asp:label>
            
                </div>
                <div class="colOggetti" style="width:85%" >
                    <asp:textbox id="txtDescrizione" Runat="server" Width="100%" MaxLength="50"></asp:textbox>
                </div>
            </div>
             <div class="row" >
                        <div class="colOggetti" style="width:10%">
                        <asp:label id="LblNote" AssociatedControlID="txtNote" runat="server">Note</asp:label>
                        </div>
                        <div class="colOggetti" style="width:85%">
                        <asp:textbox id="txtNote" Width="100%" Runat="server" MaxLength="500" TextMode="MultiLine"></asp:textbox>
                        </div>
              </div>
               <div class="RigaPulsanti"> 
             <asp:Button ID="cmdSalva" CssClass="Pulsante" Text="Salva" runat="server" />
             <asp:Button ID="cmdChiudi" CssClass="Pulsante" Text="Chiudi" runat="server"  />
        </div>
         </div>

        
           
   </fieldset>
			
</asp:Content>
