<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="wfrmAnagraficaEnteAccordoNoAdeguamento.aspx.vb" Inherits="Futuro.wfrmAnagraficaEnteAccordoNoAdeguamento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
        <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <script src="Scripts/JHelper.js" type="text/javascript"></script>
        <style type="text/css">
            .ui-datepicker
            {
                font-size: 11px;
            }
        </style>
        <script type="text/javascript">
            var prefissoIdClient = "MainContent_";

            function SetContextKey() {
                $find('<%=AutoCompleteExtenderIndirizzo.ClientID%>').set_contextKey($get("<%=ddlComune.ClientID %>").value);
            }
	    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
     <asp:HiddenField ID="HFIdSede" runat="server" />
     <fieldset class="ContornoPagina">
        <legend>Modifica Rapida Anagrafica Ente di Accoglienza</legend>
        <asp:Label ID="lblMessaggio" runat="server" CssClass="msgErrore"></asp:Label>
        <div class="wrapper" style="width: 100%">
            <div class="headers">
                <h2>
                    <asp:Label ID="lblTitolo" runat="server" Text="Modifica Anagrafica Enti di Accoglienza"></asp:Label></h2>
            </div>
            <div class="RigaVuota">
                &nbsp;</div>
        </div>
        <div class="wrapper" style="width: 100%">
            <div class="RigaVuota">
                &nbsp;</div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblEnte" CssClass="label" AssociatedControlID="txtdenominazione" runat="server"
                        Text="(*)Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 60%">
                    <asp:TextBox ID="txtdenominazione" CssClass="textbox" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblEmailOrdinaria" CssClass="label" AssociatedControlID="txtemail"
                        runat="server" Text="(*)E-mail Ordinaria"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:TextBox ID="txtemail" CssClass="textbox" runat="server" Width="70%" ></asp:TextBox>
                </div>
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblPEC" CssClass="label" AssociatedControlID="txtEmailpec" runat="server"
                        Text="PEC"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:TextBox ID="txtEmailpec" CssClass="textbox" runat="server" Width="70%"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblTelefono" CssClass="label" AssociatedControlID="txtTelefono" runat="server"
                        Text="(*)Telefono"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:TextBox ID="txtprefisso" runat="server" CssClass="textbox" Width="50px"></asp:TextBox>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="textbox" Width="120px"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width: 15%">
                    <asp:Label ID="lblHttp" CssClass="label" AssociatedControlID="txthttp" runat="server"
                        Text="(*)Http"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 35%">
                    <asp:TextBox ID="txthttp" CssClass="textbox" runat="server" Width="70%"></asp:TextBox>
                </div>
            </div>
        </div>
        
         <p style="text-align: center">
                <asp:Label ID="lblSedeLegale" runat="server" CssClass="bold" Text="Sede Legale Ente"></asp:Label>
            </p>
            <div class="wrapper" style="width: 100%">
                <div class="RigaVuota">
                    &nbsp;</div>
                 <div class="row">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblProvincia" CssClass="label" AssociatedControlID="ddlProvincia"
                            runat="server" Text="(*)Provincia/Nazione"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 25%">
                        <asp:DropDownList ID="ddlProvincia" CssClass="ddlClass" AutoPostBack="true" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="colOggetti" style="width: 10%">
                        <asp:CheckBox ID="ChkEstero" AutoPostBack="true" Text="Estero" ToolTip="Flag Estero"
                            runat="server" />
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblComune" CssClass="label" AssociatedControlID="ddlComune" runat="server"
                            Text="(*)Comune"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 35%">
                        <asp:DropDownList ID="ddlComune" CssClass="ddlClass" runat="server">
                            <asp:ListItem Value="0" Text=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblIndirizzo" CssClass="label" runat="server" Text="(*)Indirizzo" AssociatedControlID="txtIndirizzo"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 35%">
                        <asp:TextBox ID="txtIndirizzo" runat="server" CssClass="textbox" onkeyup="javascript: SetContextKey();"/>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtenderIndirizzo" TargetControlID="txtIndirizzo"
                            ContextKey="ddlComune" CompletionListCssClass="ddl_Autocomplete" UseContextKey="true"
                            CompletionInterval="100" EnableCaching="false" runat="server" MinimumPrefixLength="5"
                            ServiceMethod="GetCompletionList">
                        </asp:AutoCompleteExtender>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblNumeroCivico" runat="server" Text="(*)Numero civico" AssociatedControlID="txtCivico"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 10%">
                        <asp:TextBox ID="txtCivico" runat="server" CssClass="textbox"></asp:TextBox>    
                    </div>
                  
                    <div class="collable" style="width: 10%">
                        <asp:Label ID="lblCAP" CssClass="label" runat="server" Text="(*)C.A.P." AssociatedControlID="txtCAP"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 5%">
                        <asp:TextBox ID="txtCAP" CssClass="textbox" runat="server"></asp:TextBox>
                    </div>
                     <div class="collable" style="width: 5%">
                        <asp:ImageButton ID="imgCap" runat="server" title="Seleziona il Cap" ImageUrl="Images/valida_small.png"
                            AlternateText="Seleziona il Cap" Style="width: 20px; height: 20px" />
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblDettaglioRecapito" CssClass="label" runat="server" Text="Dettaglio recapito"
                            AssociatedControlID="TxtDettaglioRecapito"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 80%">
                        <asp:TextBox ID="TxtDettaglioRecapito" CssClass="textbox" runat="server" ToolTip="Dettagli aggiuntivi del recapito"
                            TextMode="MultiLine" Rows="2" Width="99%"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="wrapper" style="width: 100%; border-style: none">
                <div class="RigaVuota">
                    &nbsp;</div>
                <div class="RigaPulsanti">
                    <asp:Button ID="CmdModifica" runat="server" CssClass="Pulsante" Text="Modifica" />
                    <asp:Button ID="CmdSalva" runat="server" CssClass="Pulsante" Text="Salva" />
                    <asp:Button ID="CmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
                    <asp:Button ID="CmdAnnulla" runat="server" CssClass="Pulsante" Text="Annulla" />
                </div>
            </div>
        </fieldset>
</asp:Content>
