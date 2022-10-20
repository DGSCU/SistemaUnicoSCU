<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmVisualizzaAccordoEnte.aspx.vb" Inherits="Futuro.WfrmVisualizzaAccordoEnte" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head runat="server">
    <title>Visualizzazione dell'Accordo Esistente</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmVisualizzaAccordoEnte" runat="server" method="post">
        <div class="fieldsetrasparente">
            <fieldset class="ContornoPagina">
                <legend> Visualizzazione dell'Accordo Esistente</legend>
                <div class="wrapper" style="width:100%">
                    <div class="row">
                        <div class="collable" style="width:15%">
                            <asp:Label ID="lblEntePadreLabel" CssClass="label" AssociatedControlID="lblEntePadre" runat="server" Text="Ente Padre"></asp:Label>   
                        </div> 
                        <div class="colOggetti" style="width:35%">       
                            <asp:label id="lblEntePadre" CssClass="bold" runat="server"></asp:label>         
                        </div>
                        <div class="collable" style="width:15%">
                            <asp:Label ID="lblCodEntePadreLabel" CssClass="label" AssociatedControlID="lblCodEntePadre" runat="server" Text="Cod. Ente"></asp:Label>   
                        </div> 
                        <div class="colOggetti" style="width:34%">       
                            <asp:label id="lblCodEntePadre" CssClass="bold" runat="server"></asp:label>         
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width:15%">
                            <asp:Label ID="lblEnteFiglioLabel" CssClass="label" AssociatedControlID="lblEnteFiglio" runat="server" Text="Ente Figlio"></asp:Label>   
                        </div> 
                        <div class="colOggetti" style="width:35%">       
                            <asp:label id="lblEnteFiglio" CssClass="bold" runat="server"></asp:label>         
                        </div>
                        <div class="collable" style="width:15%">
                            <asp:Label ID="lblCodEnteFiglioLabel" CssClass="label" AssociatedControlID="lblCodEnteFiglio" runat="server" Text="Cod. Ente"></asp:Label>   
                        </div> 
                        <div class="colOggetti" style="width:34%">       
                            <asp:label id="lblCodEnteFiglio" CssClass="bold" runat="server"></asp:label>         
                        </div>
                    </div>
                     <div class="row">
                        <div class="collable" style="width:15%">
                            <asp:Label ID="lblTipoAccordoLabel" CssClass="label" AssociatedControlID="lblTipoAccordo" runat="server" Text="Tipo Accordo"></asp:Label>   
                        </div> 
                        <div class="colOggetti" style="width:84%">       
                            <asp:label id="lblTipoAccordo" CssClass="bold" runat="server"></asp:label>         
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width:15%">
                            <asp:Label ID="lblDataRegistrazioneLabel" CssClass="label" AssociatedControlID="lbldataInizio" runat="server" Text="Data Registrazione"></asp:Label>   
                        </div> 
                        <div class="colOggetti" style="width:35%">       
                            <asp:label id="lbldataInizio" CssClass="bold" runat="server"></asp:label>         
                        </div>
                        <div class="collable" style="width:15%">
                            <asp:Label ID="lblDataCancellazioneLabel" CssClass="label" AssociatedControlID="lblfineAc" runat="server" Text="Data Cancellazione"></asp:Label>   
                        </div> 
                        <div class="colOggetti" style="width:34%">       
                            <asp:label id="lblfineAc" CssClass="bold" runat="server"></asp:label>         
                        </div>
                    </div>
                    <div class="row">
                        <div class="collable" style="width:15%">
                            <asp:Label ID="lblDataStipulaLabel" CssClass="label" AssociatedControlID="lblStipula" runat="server" Text="Data Stipula"></asp:Label>   
                        </div> 
                        <div class="colOggetti" style="width:35%">       
                            <asp:label id="lblStipula" CssClass="bold" runat="server"></asp:label>         
                        </div>
                        <div class="collable" style="width:15%">
                            <asp:Label ID="lblDataScadenzaLabel" CssClass="label" AssociatedControlID="lblScadenza" runat="server" Text="Data Scadenza"></asp:Label>   
                        </div> 
                        <div class="colOggetti" style="width:34%">       
                            <asp:label id="lblScadenza" CssClass="bold" runat="server"></asp:label>         
                        </div>
                    </div>
                    <div class="RigaPulsanti">
                        <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
                    </div>
                </div>
            </fieldset>
        </div>
    </form>
</body>
</html>
