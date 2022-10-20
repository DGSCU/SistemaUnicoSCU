<%@ Page Title="Autocertificazione e consensi ruoli Antimafia" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Autocertificazioni.aspx.vb" Inherits="Futuro.Autocertificazioni" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
          <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css"/>
        <script src="Scripts/JHelper.js" type="text/javascript"></script>
        <script src="https://polyfill.io/v3/polyfill.min.js?features=default" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
<fieldset class="ContornoPagina">
        <legend>Autocertificazione e consensi ruoli Antimafia</legend>
        
        <div class="wrapper" style="width:100%">

            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Autocertificazioni e Consensi"></asp:Label>
               <asp:Label ID="lblEnte" runat="server"  ></asp:Label>
               </h2>
            </div>
            <div class="row" style="height:auto">
                <asp:Label ID="msgConferma" CssClass="msgConferma" runat="server"></asp:Label>
            </div>
            <div class="row" style="height:auto">
                <asp:Label ID="msgErrore" CssClass="msgErrore" runat="server"></asp:Label>                
            </div>
            <div class="row" style="height:auto">
                <asp:Label ID="msgInfo" CssClass="msgInfo" runat="server"></asp:Label>
            </div>
           <div class="RigaVuota">
                &nbsp;
            </div>
            <p>Dichiaro che tra le risorse dell'Ente non sono previsti o non sono stati nominati i seguenti ruoli:</p>
            <div class="row" style="height:auto">  
               &nbsp;&nbsp;<asp:Label ID="lblAvviso" CssClass="label"  runat="server" Text="(è obbligatorio selezionare almeno un ruolo)" AssociatedControlID="hdTxtTipoOperazione"/>
            </div>
             <div class="RigaVuota">  
               &nbsp;
                 <asp:TextBox ID="hdTxtTipoOperazione" runat="server" Visible="false"></asp:TextBox>
                             <asp:TextBox ID="hdTxtIDEntiAutocertificazioni" runat="server" Visible="false"></asp:TextBox>
                             <asp:TextBox ID="txtPopup" runat="server" Visible="False"></asp:TextBox>
             </div> 
            <div class="row">
                <div class="collable" style="width:10%">          
                    &nbsp;
                </div>
                <div class="collable" style="width:5%">          
                    <asp:Label ID="lblEntefiglio" CssClass="label" runat="server" Text="Ente" AssociatedControlID="ddlEntiFigli" />
                </div>
                <div class="colOggetti" style="width:80%">
                    <asp:DropDownList ID="ddlEntiFigli" runat="server" CssClass="ddlClass" AutoPostBack="true"></asp:DropDownList>
               </div>
                <div class="colOggetti" style="width:5%">
                    <asp:ImageButton ID="imgOpenFigli" runat="server" ImageUrl="Images/lenteIngrandimento_small.png" AlternateText="Visualizza Enti Figli" ToolTip="Visualizza Enti Figli" Visible="false" />
               </div>  
            </div>
            <div class="row">
                <div class="collable" style="width:10%">
                &nbsp;  
                </div>
                <div class="colOggetti" style="width:5%">  
                    <asp:CheckBox ID="ChkDirettoreTecnico"  Text=" " runat="server" />
                </div>
                <div class="collable" style="width:55%">  
                    <asp:Label ID="lblDirettoreTecnico" CssClass="label"  runat="server" Text="direttore tecnico;" AssociatedControlID="ChkDirettoreTecnico"/>
                </div>
            </div>   

            <div class="row">
                <div class="collable" style="width:10%">
                &nbsp;  
                </div>
                 <div class="colOggetti" style="width:5%">  
                    <asp:CheckBox ID="ChKCondirettoreTecnico"  Text=" " runat="server" />
                </div>
                <div class="collable" style="width:55%">  
                    <asp:Label ID="lblCondirettoreTecnico" CssClass="label"  runat="server" Text="condirettore tecnico;"  AssociatedControlID="chkCondirettoreTecnico"/>
                </div>
             </div>  
            
            <div class="row">
                <div class="collable" style="width:10%">
                &nbsp;  
                </div>
                <div class="colOggetti" style="width:5%">  
                    <asp:CheckBox ID="chkResponsabileTecnico"  Text=" " runat="server" />
                </div>
                <div class="collable" style="width:55%">   
                    <asp:Label ID="lblResponsabileTecnico" CssClass="label"  runat="server" Text="responsabile tecnico;"  AssociatedControlID="chkResponsabileTecnico"/>
                </div>
            </div>   

            <div class="row">
                <div class="collable" style="width:10%">
                &nbsp;  
                </div>
                <div class="colOggetti" style="width:5%">  
                    <asp:CheckBox ID="chkSocioResponsabileTecnico"  Text=" " runat="server" />
                </div>
                <div class="collable" style="width:55%">   
                    <asp:Label ID="lblSocioResponsabileTecnico" CssClass="label"  runat="server" Text="socio responsabile tecnico;"  AssociatedControlID="chkSocioResponsabileTecnico"/>
                </div>
           </div>

            <div class="row">
                <div class="collable" style="width:10%">
                &nbsp;  
                </div>
                <div class="colOggetti" style="width:5%">  
                    <asp:CheckBox ID="chkResponsabilePreposto"  Text=" " runat="server" />
                </div>
                <div class="collable" style="width:55%">   
                    <asp:Label ID="lblResponsabilePreposto" CssClass="label"  runat="server" Text="responsabile preposto;"   AssociatedControlID="chkResponsabilePreposto"/>
                </div>
            </div>  

            <div class="row">
                <div class="collable" style="width:10%">
                &nbsp;  
                </div>
                 <div class="colOggetti" style="width:5%">  
                    <asp:CheckBox ID="chkPrepostoGestioneTecnica"  Text=" " runat="server" />
                </div>
                <div class="collable" style="width:55%">   
                    <asp:Label ID="lblPrepostoGestioneTecnica" CssClass="label"  runat="server" Text="preposto gestione tecnica;"   AssociatedControlID="chkPrepostoGestioneTecnica"/>
                </div>          
            </div>

            <div class="row">
                <div class="collable" style="width:10%">
                &nbsp;  
                </div>
                <div class="colOggetti" style="width:5%">  
                    <asp:CheckBox ID="chkPresidenteCollegio"  Text=" " runat="server" />
                </div>
                <div class="collable" style="width:55%">   
                    <asp:Label ID="lblPresidenteCollegio" CssClass="label"  runat="server" Text="presidente del collegio sindacale;"   AssociatedControlID="chkPresidenteCollegio"/>
                </div>
             </div>  
            <div class="row">
                <div class="collable" style="width:10%">
                &nbsp;  
                </div> 
                <div class="colOggetti" style="width:5%">  
                    <asp:CheckBox ID="chkSindaco"  Text=" " runat="server" />
                </div>
                <div class="collable" style="width:55%">   
                    <asp:Label ID="lblSindaco" CssClass="label"  runat="server" Text="membri del collegio sindacale o sindaco;" AssociatedControlID="chkSindaco"/>
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:10%">
                &nbsp;  
                </div>
                <div class="colOggetti" style="width:5%">  
                    <asp:CheckBox ID="chkSindacoProTempore"  Text=" " runat="server" />
                </div>
                <div class="collable" style="width:55%">   
                    <asp:Label ID="lblSindacoProTempore" CssClass="label"  runat="server" Text="sindaco pro-tempore;"   AssociatedControlID="chkSindacoProTempore"/>
                </div>
            </div>   
            <div class="row">
                <div class="collable" style="width:10%">
                &nbsp;  
                </div> 
                <div class="colOggetti" style="width:5%">  
                    <asp:CheckBox ID="chkSindacoSupplente"  Text=" " runat="server" />
                </div>
                <div class="collable" style="width:55%">   
                    <asp:Label ID="lblSindacoSupplente" CssClass="label"  runat="server" Text="sindaco supplente;"  AssociatedControlID="chkSindacoSupplente"/>
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:10%">
                &nbsp;  
                </div>
                <div class="colOggetti" style="width:5%">  
                    <asp:CheckBox ID="chkSoggettiVigilanza"  Text=" " runat="server" />
                </div>
                <div class="collable" style="width:55%">   
                    <asp:Label ID="lblSoggettiVigilanza" CssClass="label"  runat="server" Text="soggetti con compiti di vigilanza;"  AssociatedControlID="chkSoggettiVigilanza"/>
                </div>
            </div>  
            
            <div class="row">
                <div class="collable" style="width:10%">
                &nbsp;  
                </div>
                 <div class="colOggetti" style="width:5%">  
                    <asp:CheckBox ID="chkConsigliereSorveglianza"  Text=" " runat="server" />
                </div>
                <div class="collable" style="width:55%">   
                    <asp:Label ID="lblConsigliereSorveglianza" CssClass="label"  runat="server" Text="consigliere di sorveglianza;"  AssociatedControlID="chkConsigliereSorveglianza"/>
                </div>          
           </div>

            <div class="row">
                <div class="collable" style="width:10%">
                &nbsp;  
                </div>
                <div class="colOggetti" style="width:5%">  
                    <asp:CheckBox ID="chkConsigliereSorveglianzaSupplente"  Text=" " runat="server" />
                </div>
                <div class="collable" style="width:55%">   
                    <asp:Label ID="lblConsigliereSorveglianzaSupplente" CssClass="label"  runat="server" Text="consigliere di sorveglianza supplente;"/>
                </div>
            </div>  
            
            <div class="row">
                <div class="collable" style="width:10%">
                &nbsp;  
                </div>
                <div class="colOggetti" style="width:5%">  
                    <asp:CheckBox ID="chkPresidenteConsiglioSorveglianza"  Text=" " runat="server" />
                </div>
                <div class="collable" style="width:55%">   
                    <asp:Label ID="lblPresidenteConsiglioSorveglianza" CssClass="label"  runat="server" Text="presidente del consiglio di sorveglianza."   AssociatedControlID="chkPresidenteConsiglioSorveglianza"/>
                </div>
            </div>

             <div class="RigaVuota">
                &nbsp;
            </div>

            <div class="RigaPulsanti" style="height:auto;text-align:center">
                <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" ToolTip="Conferma Autocertificazione e consensi ruoli Antimafia" Text="Salva" />
                &nbsp;
                <asp:Button ID="cmdCancella" CssClass="Pulsante" runat="server" ToolTip="Cancella Autocertificazione e consensi ruoli Antimafia" Text="Cancella" />
                &nbsp;
                <asp:Button ID="cmdAnnulla" CssClass="Pulsante" runat="server" ToolTip="Torna alla pagina di gestione Ente" Text="Chiudi" />
            </div>
           <div class="RigaVuota">
                &nbsp;
            </div>
        </div>
        
    </fieldset>
    
</asp:Content>
