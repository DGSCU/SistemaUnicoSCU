<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaRegistrazioniPreseAtto.aspx.vb" Inherits="Futuro.WfrmRicercaRegistrazioniPreseAtto" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
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
            var DataInizio = CostruisciId("txtDataInizio");
            var readonlyInizio = '<%=txtDataInizio.ReadOnly%>';
            if (readonlyInizio == "False") {
                var sharpDataInizio = "#" + DataInizio
                $("" + sharpDataInizio + "").datepicker();
            }

            var DataFine = CostruisciId("txtDataFine");
            var readonlyFine = '<%=txtDataFine.ReadOnly%>';
            if (readonlyFine == "False") {
                var sharpDataFine = "#" + DataFine
                $("" + sharpDataFine + "").datepicker();
            }
        });

    </script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Ricerca lista registrazioni - presa d’atto</legend>
        <div id="divForbidden" runat="server" visible="false">
            <asp:label id="lblForbidden"  runat="server" CssClass="msgErrore">Attenzione: l'utente collegato non ha accesso a questa funzionalità.</asp:label>
        </div>
    <div id="divRicerca" runat="server">
        <div class="wrapper" style="width:100%">
            <div class="headers" >
                <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca lista registrazioni - presa d’atto"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <asp:label id="lblmess"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDataInizio" CssClass="label" AssociatedControlID="txtDataInizio" runat="server" Text="Data di inizio periodo"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtDataInizio" CssClass="textbox" runat="server" Width="75px" MaxLength="10"></asp:TextBox>         
                </div> 
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDataFine" CssClass="label" AssociatedControlID="txtDataFine" runat="server" Text="Data di fine periodo"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtDataFine" CssClass="textbox" runat="server" Width="75px" MaxLength="10"></asp:TextBox>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodiceEnte" CssClass="label" AssociatedControlID="txtCodiceEnte" runat="server" Text="Codice Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodiceEnte" CssClass="textbox" runat="server" MaxLength="10"></asp:TextBox>         
                </div>
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="LblDenominazione" CssClass="label" AssociatedControlID="TxtDenEnte" runat="server" Text="Denominazione Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="TxtDenEnte" autofocus="true" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">          
                    <asp:Label ID="lblcognome" CssClass="label"  runat="server" Text="Cognome rappresentante legale" AssociatedControlID="txtcognome" />
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:TextBox ID="txtcognome" CssClass="textbox"  runat="server" MaxLength="100"></asp:TextBox>
                </div>
                <div class="collable" style="width:15%">          
                    <asp:Label ID="lblCF" CssClass="label"  runat="server" Text="Codice fiscale rappresentante legale" AssociatedControlID="txtCF" />
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:TextBox ID="txtCF" CssClass="textbox"  runat="server" MaxLength="17"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblStatoAntimafia" CssClass="label" AssociatedControlID="ddlStatoAntimafia" runat="server" Text="Stato dichiarazione antimafia"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlStatoAntimafia" runat="server" CssClass="ddlClass">
                    <asp:ListItem Value="0">TUTTI</asp:ListItem>
                    <asp:ListItem Value="1">Dich. Antimafia non dovuta</asp:ListItem>
                    <asp:ListItem Value="2">Dich. antimafia inserita</asp:ListItem>
				    <asp:ListItem Value="3">Dich. antimafia non inserita</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCambioRL" CssClass="label" AssociatedControlID="ddlCambioRL" runat="server" Text="Cambio rappresentante legale"></asp:Label>
                </div>
                <div class="colOggetti" style="width:25%">       
                    <asp:DropDownList ID="ddlCambioRL" runat="server" Width="30%">
                    <asp:ListItem Value="0">TUTTI</asp:ListItem>
				    <asp:ListItem Value="1">Si</asp:ListItem>
				    <asp:ListItem Value="2">No</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblPresaAtto" CssClass="label" AssociatedControlID="ddlPresaAtto" runat="server" Text="Presa d’atto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlPresaAtto" runat="server" Width="30%">
                    <asp:ListItem Value="0">TUTTI</asp:ListItem>
				    <asp:ListItem Value="1">Con presa d’atto</asp:ListItem>
				    <asp:ListItem Value="2">Senza presa d’atto</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
                <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
		        <br />
	            <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
              &nbsp;
            </div>
        </div>
        <br />
        <h3>
            <asp:label id="lblmessaggio" runat="server" AssociatedControlID="dgRegistrazioni"></asp:label>
        </h3>
        <br />
        <div id="divAzioni" style="float:right" runat="server" visible="false">
            <asp:Button ID="cmdPresaAtto" runat="server" CssClass="Pulsante" Text="Conferma Presa d’atto" visible="false"/>
            <asp:Button ID="cmdSelezionaTutti" runat="server" CssClass="Pulsante" Text="Seleziona Tutti" />
            <asp:Button ID="cmdDeselezionaTutti" runat="server" CssClass="Pulsante" Text="Deseleziona Tutti" visible="false"/>
        </div>
        <asp:datagrid id="dgRegistrazioni" runat="server" Width="100%" 
                    ToolTip="Elenco Registrazioni - presa d’atto" CssClass="table"  AllowPaging="True" PageSize="50"
                    AllowSorting="True" AutoGenerateColumns="False"  Visible="True" 
                    UseAccessibleHeader="True">
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		        <HeaderStyle></HeaderStyle>
		        <Columns>
                    <asp:BoundColumn DataField="Data/Ora" HeaderText="Data/Ora"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Codice Ente" HeaderText="Codice Ente"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Denominazione Ente" HeaderText="Denominazione Ente"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Cognome/Nome" HeaderText="Cognome/Nome"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Codice Fiscale" HeaderText="Codice Fiscale"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Stato antimafia" HeaderText="Stato antimafia"></asp:BoundColumn>
                    <asp:BoundColumn DataField="CambioRL" HeaderText="Cambio RL"></asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="Presa d’atto"> 
                        <ItemTemplate>
					        <asp:CheckBox ID="chkPresaAtto" runat="server" AutoPostBack="true" OnCheckedChanged="chkPresaAtto_OnCheckedChanged"/>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="IDEntePersonaleRuolo"></asp:BoundColumn>
        		</Columns>
		    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
	    </asp:datagrid>
        <p>&nbsp;</p>
    </div>
    </fieldset>
</asp:Content>
