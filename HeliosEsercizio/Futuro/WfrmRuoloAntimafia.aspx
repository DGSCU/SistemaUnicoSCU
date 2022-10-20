<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRuoloAntimafia.aspx.vb" Inherits="Futuro.WfrmRuoloAntimafia" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <script src="Scripts/JHelper.js" type="text/javascript"></script>

    <style type="text/css"> .ui-datepicker {font-size: 11px;}</style>

    <script  type="text/javascript">

        var prefissoIdClient = "MainContent_";

        function CostruisciId(IdServer) {
            var IdClient = prefissoIdClient + IdServer;
            return IdClient;
        };

        $(function () {
            var DataNascita = CostruisciId("txtDataNascita");
            var readonly = '<%=txtDataNascita.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataNascita = "#" + DataNascita
                $("" + sharpDataNascita + "").datepicker();
            }
        });

        function SetContextKey() {
            $find('<%=AutoCompleteExtenderResidenza.ClientID%>').set_contextKey($get("<%=ddlComuneResidenza.ClientID %>").value);
        };

        function CodiceFiscaleUpperCase() {
            var idCodiceFiscale = CostruisciId("txtCodiceFiscale");
            var txtCodiceFiscale = document.getElementById(idCodiceFiscale);
            txtCodiceFiscale.value = txtCodiceFiscale.value.toUpperCase();
        };

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <fieldset class="ContornoPagina">
        <legend id="lgContornoPagina" runat="server">Ruolo Antimafia</legend>
        <asp:Label ID="lblMessaggio" runat="server" CssClass="msgErrore"></asp:Label>
        <br />
        <div runat="server" id="divPrincipale">
            <div class="wrapper" style="width: 100%">
                <div class="headers">
                    <h2>
                        <asp:Label ID="lblTitolo" runat="server" Text="Ruolo Antimafia"></asp:Label></h2>
                </div>
                <div class="RigaVuota">
                    &nbsp;
                </div>
                <div class="row">
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblEnte" CssClass="label" AssociatedControlID="ddlEnti" runat="server" Text="<strong>(*)</strong> Ente"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:dropdownlist id="ddlEnti" CssClass="ddlClass" runat="server" AutoPostBack="true"></asp:dropdownlist>         
                    </div>    
                    <div class="collable" style="width:15%">          
                        <asp:Label ID="lblcodicefiscale" CssClass="label"  runat="server" Text="<strong>(*)</strong> Codice Fiscale" AssociatedControlID="txtcodicefiscale" />
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:TextBox ID="txtcodicefiscale" CssClass="textbox" runat="server" Width="160px" MaxLength="16"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width:15%">          
                        <asp:Label ID="lblcognome" CssClass="label"  runat="server" Text="<strong>(*)</strong> Cognome" AssociatedControlID="txtcognome" />
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:TextBox ID="txtcognome" CssClass="textbox"  runat="server" MaxLength="100"></asp:TextBox>
                    </div>
                    <div class="collable" style="width:15%">          
                        <asp:Label ID="lblnome" CssClass="label"  runat="server" Text="<strong>(*)</strong> Nome" AssociatedControlID="txtnome" />
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:TextBox ID="txtnome" CssClass="textbox"  runat="server" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblRuoliAntimafia" CssClass="label" AssociatedControlID="ddlRuoliAntiMafia" runat="server" Text="<strong>(*)</strong> Ruolo"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:dropdownlist id="ddlRuoliAntiMafia" CssClass="ddlClass" runat="server"></asp:dropdownlist>         
                    </div>    
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblDataNascita" CssClass="label" AssociatedControlID="txtDataNascita" runat="server" Text="<strong>(*)</strong> Data di nascita"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:7%">       
                        <asp:TextBox ID="txtDataNascita" CssClass="textbox" runat="server" Width="75px" MaxLength="10"></asp:TextBox>         
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblProvinciaNascita" CssClass="label" AssociatedControlID="ddlProvinciaNascita" runat="server" Text="<strong>(*)</strong> Provincia di Nascita"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:25%">       
                        <asp:dropdownlist id="ddlProvinciaNascita" CssClass="ddlClass" AutoPostBack="true" runat="server"  ></asp:dropdownlist>           
                    </div>
                    <div class="colOggetti" style="width:10%">
                        <asp:CheckBox ID="ChkEsteroNascita" AutoPostBack="true" Text="Estero" ToolTip="Flag Estero Nazione di Nascita" runat="server" />
                    </div>  
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblComuneNascita" CssClass="label" AssociatedControlID="ddlComuneNascita" runat="server" Text="<strong>(*)</strong> Comune di nascita"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:dropdownlist id="ddlComuneNascita" CssClass="ddlClass" runat="server"></asp:dropdownlist>         
                    </div>    
                </div>
                <div class="row" >
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblProvinciaResidenza" CssClass="label" AssociatedControlID="ddlProvinciaResidenza" runat="server" Text="<strong>(*)</strong> Provincia di Residenza"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:25%">       
                        <asp:dropdownlist id="ddlProvinciaResidenza" CssClass="ddlClass" AutoPostBack="true" runat="server"  ></asp:dropdownlist>           
                    </div>
                    <div class="colOggetti" style="width:10%">
                        <asp:CheckBox ID="ChkEsteroResidenza" AutoPostBack="true" Text="Estero" ToolTip="Flag Estero Nazione di Nascita" runat="server" />
                    </div>                 
                    <div class="collable" style="width:15%">      
                        <asp:Label ID="lblComuneResidenza" CssClass="label" AssociatedControlID="ddlComuneResidenza" runat="server" Text="<strong>(*)</strong> Comune di residenza"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">       
                        <asp:dropdownlist id="ddlComuneResidenza" CssClass="ddlClass" runat="server">
                            <asp:ListItem Value="0" Text=""></asp:ListItem>
                        </asp:dropdownlist>         
                    </div>    
                </div>
                <div class="row">
                    <div class="collable" style="width:15%">
                        <asp:Label ID="lblIndirizzoResidenza" CssClass="label" runat="server" Text="<strong>(*)</strong> Indirizzo di residenza" AssociatedControlID="txtIndirizzo"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:TextBox ID="txtIndirizzo" runat="server" CssClass="textbox" onkeyup="javascript: SetContextKey();" MaxLength="200"></asp:TextBox>
                        <asp:AutoCompleteExtender 
                            ID="AutoCompleteExtenderResidenza" 
                            TargetControlID="txtIndirizzo" 
                            ContextKey ="ddlComuneResidenza"
                            CompletionListCssClass="ddl_Autocomplete"
                            UseContextKey="true"
                            CompletionInterval="100" EnableCaching="false" 
                            runat="server" MinimumPrefixLength="5" ServiceMethod="GetCompletionList" >
                        </asp:AutoCompleteExtender>
                    </div>
                    <div class="collable" style="width:15%">
                        <asp:Label ID="lblNumeroCivico"  runat="server" Text="<strong>(*)</strong> Numero civico" AssociatedControlID="txtCivico"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:10%">
                        <asp:TextBox ID="txtCivico" runat="server" CssClass="textbox" MaxLength="10"></asp:TextBox>
                    </div>
                    <div class="collable" style="width:5%">
                         <asp:ImageButton ID="infocivicoRec" runat="server" AlternateText="Informazioni Civico di Residenza" 
                            ImageUrl="Images/info_small.png" style="width:20px;height:20px"  
                            ToolTip="NEL CAMPO CIVICO E' POSSIBILE INSERIRE SOLO I SEGUENTI FORMATI:
                                    - 21
                                    - 21/A 
                                    - 21/A5 
                                    - 21 BIS 
                                    - KM 21,500 
                                    OPPURE IL VALORE SNC" disabled="disabled"/>
                    </div>
 
                    <div class="collable" style="width:10%">
                        <asp:Label ID="lblCAP" CssClass="label" runat="server" Text="<strong>(*)</strong> C.A.P." AssociatedControlID="txtCAP"  ></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:5%">
                        <asp:TextBox ID="txtCAP" CssClass="textbox" runat="server" MaxLength="8"></asp:TextBox>

                    </div>
                    <div class="collable" style="width:5%">
                        <asp:ImageButton ID="imgCap" runat="server" title="Seleziona il Cap di Residenza" ImageUrl="Images/valida_small.png" AlternateText="Seleziona il Cap di Residenza"  style="width:20px;height:20px" />
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width:15%">
                        <asp:Label ID="lblTelefono" CssClass="label" runat="server" Text="Telefono" AssociatedControlID="txtTelefono"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:TextBox ID="txtTelefono" runat="server" CssClass="textbox" MaxLength="20"></asp:TextBox>
                    </div>
                    <div class="collable" style="width:15%">
                        <asp:Label ID="lblPEC" CssClass="label" runat="server" Text="P.E.C." AssociatedControlID="txtPEC"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:TextBox ID="txtPEC" runat="server" CssClass="textbox" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width:15%">
                        <asp:Label ID="lblEmail" CssClass="label" runat="server" Text="Email" AssociatedControlID="txtEmail"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div class="RigaPulsanti" >
                    <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" ToolTip="Salva Ruolo" Text="Salva"  />
                    <asp:Button ID="cmdElimina" CssClass="Pulsante" runat="server" ToolTip="Elimina Ruolo" Text="Elimina"  />
                    <asp:Button ID="cmdAnnulla" CssClass="Pulsante" runat="server" ToolTip="Chiudi" Text="Chiudi"  />
                </div>
           </div>
       </div>
       <asp:HiddenField ID="hIdRuoloAntiMafia" runat="server" />
       <asp:HiddenField ID="hIdEnteFaseAntimafia" runat="server" />
    </fieldset>
</asp:Content>
