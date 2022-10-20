<%@ Page Title="Ricerca Enti Disponibili" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicEnteinAccordo.aspx.vb" Inherits="Futuro.WfrmRicEnteinAccordo" %>
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


        function ValidazioneClientRicerca() {

            var idDataInserimentoDal = CostruisciId("TxtDataInserimentoDal");
            var txtDataInserimentoDal = document.getElementById(idDataInserimentoDal);

            var idDataInserimentoAl = CostruisciId("TxtDataInserimentoAl");
            var txtDataInserimentoAl = document.getElementById(idDataInserimentoAl);

            if (txtDataInserimentoDal.value =='' && txtDataInserimentoAl.value!='') {
                alert("Inserire la data inizio del range per la ricerca");
                return false;
            }

            if (txtDataInserimentoDal.value != '') {

                if (isCorrectFormatDate(txtDataInserimentoDal, "Data Inserimento Accordo Dal") == false)
                {
                    return false;
                }
                
            }

            if (txtDataInserimentoAl.value != '') {

                if (isCorrectFormatDate(txtDataInserimentoAl, "Data Inserimento Accordo Al") == false) {
                    return false;
                }

            }

            if (txtDataInserimentoDal.value != '' && txtDataInserimentoAl.value != '') {

                if (CompareDate(txtDataInserimentoDal, txtDataInserimentoAl) == false) {
                    return false;
                }

            }
            

            return true;
        }

        $(function () {
            var DataInserimentoDal = CostruisciId("TxtDataInserimentoDal");
            var readonly = '<%=TxtDataInserimentoDal.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataInserimentoDal = "#" + DataInserimentoDal
                $("" + sharpDataInserimentoDal + "").datepicker();
            }
        });

        $(function () {
            var DataInserimentoAl = CostruisciId("TxtDataInserimentoAl");
            var readonly = '<%=TxtDataInserimentoAl.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataInserimentoAl = "#" + DataInserimentoAl
                $("" + sharpDataInserimentoAl + "").datepicker();
            }
        });

        function isCorrectFormatDate(txtDataRiferimento, nomeDataRiferimento) {
           
            //Checks for dd/mm/yyyy format.
            var currVal = txtDataRiferimento.value;
            var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
            var dtArray = currVal.match(rxDatePattern); // is format OK?
            if (dtArray == null) {
                alert("La '" + nomeDataRiferimento + "' non è valida. Inserire la data nel formato GG/MM/AAAA.")
                txtDataRiferimento.focus();
                return false;
            }

            dtDay = dtArray[1];
            dtMonth = dtArray[3];
            dtYear = dtArray[5];

            if (dtMonth < 1 || dtMonth > 12) {
                alert("La '" + nomeDataRiferimento + "' non è valida. Inserire la data nel formato GG/MM/AAAA.")
                txtDataRiferimento.focus();
                return false;
            }
            else if (dtDay < 1 || dtDay > 31) {
                alert("La '" + nomeDataRiferimento + "' non è valida. Inserire la data nel formato GG/MM/AAAA.")
                txtDataRiferimento.focus();
                return false;
            }
            else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31) {
                alert("La '" + nomeDataRiferimento + "' non è valida. Inserire la data nel formato GG/MM/AAAA.")
                txtDataRiferimento.focus();
                return false;
            }
            else if (dtMonth == 2) {
                var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
                if (dtDay > 29 || (dtDay == 29 && !isleap)) {
                    alert("La '" + nomeDataRiferimento + "' non è valida. Inserire la data nel formato GG/MM/AAAA.")
                    txtDataRiferimento.focus();
                    return false;
                }
            }

            return true;
        }

        function CompareDate(dateFrom, dateTo) {

            var arrDateFrom = dateFrom.value.split("/");
            var arrDateTo = dateTo.value.split("/");

            var dtFrom = new Date(arrDateFrom[2], arrDateFrom[1], arrDateFrom[0]); //Year, Month, Date
            var dtTo = new Date(arrDateTo[2], arrDateTo[1], arrDateTo[0]); //Year, Month, Date

            if (dtFrom > dtTo) {

                alert("La data di inizio e\' maggiore della data finale");
                return false;
            }

            return true;
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Ricerca Enti Disponibili</legend>
        <asp:label id="lblControlloClasse"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Enti Disponibili"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
                <div class="collable" style="width:10%">      
                    <asp:Label ID="lblDenominazione" CssClass="label" AssociatedControlID="txtdenominazione" runat="server" Text="Denominazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:23%">       
                    <asp:TextBox ID="txtdenominazione" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:10%">      
                    <asp:Label ID="lblCodEnte" CssClass="label" AssociatedControlID="txtCodRegione" runat="server" Text="Cod. Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:23%">       
                    <asp:TextBox ID="txtCodRegione" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:10%">      
                    <asp:Label ID="Label1" CssClass="label" AssociatedControlID="TxtCodiceFiscale" runat="server" Text="Cod. Fis."></asp:Label>
                </div>
                <div class="colOggetti" style="width:23%">       
                    <asp:TextBox ID="TxtCodiceFiscale" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:10%">      
                    <asp:Label ID="lblTipoRelazione" CssClass="label" AssociatedControlID="ddlCAccreditamento" runat="server" Text="Tipo Relazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:23%">       
                     <asp:DropDownList ID="ddlCAccreditamento" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                </div>
                <div class="collable" style="width:10%">      
                    <asp:Label ID="lblStatoEnte" CssClass="label" AssociatedControlID="CboStatoEnte" runat="server" Text="Stato Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:23%">       
                     <asp:DropDownList ID="CboStatoEnte" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                </div>
                <asp:PlaceHolder id="phStatoAccreditamento" runat="server">
                    <div class="collable" style="width:10%">      
                        <asp:Label ID="Label4" CssClass="label" AssociatedControlID="ddlstato" runat="server" Text="Stato Acc."></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:23%">       
                         <asp:DropDownList ID="ddlstato" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                    </div>  
                </asp:PlaceHolder> 
            </div>
            <div class="row">
                <div class="collable" style="width:10%">      
                    <asp:Label ID="Label2" CssClass="label" AssociatedControlID="ddltipologia" runat="server" Text="Tipologia"></asp:Label>
                </div>
                <div class="colOggetti" style="width:23%">       
                     <asp:DropDownList ID="ddltipologia" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                </div>
                <div class="collable" style="width:10%">      
                    <asp:Label ID="Label3" CssClass="label" AssociatedControlID="TxtDataInserimentoDal" runat="server" Text="Data Ins. Accordo Dal"></asp:Label>
                </div>
                <div class="colOggetti" style="width:23%">       
                     <asp:TextBox ID="TxtDataInserimentoDal" CssClass="textbox" runat="server" Width="35%" MaxLength="10"></asp:TextBox>         
                </div>
                <div class="collable" style="width:10%">      
                    <asp:Label ID="Label5" CssClass="label" AssociatedControlID="TxtDataInserimentoAl" runat="server" Text="Data Ins. Accordo Al"></asp:Label>
                </div>
                <div class="colOggetti" style="width:23%">       
                    <asp:TextBox ID="TxtDataInserimentoAl" CssClass="textbox" runat="server" Width="35%" MaxLength="10"></asp:TextBox>         
                </div>  
            </div>
            <div class="row">
                <div class="collable" style="width:10%">      
                    <asp:Label ID="Label6" CssClass="label" AssociatedControlID="ddlRichistaVariazione" runat="server" Text="Richiesta Variazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:23%">       
                     <asp:DropDownList ID="ddlRichistaVariazione" runat="server" CssClass="ddlClass">
                        <asp:ListItem Text="Tutti" Value=""></asp:ListItem>
						<asp:ListItem Text="Variazione richieste" Value="1"></asp:ListItem>
						<asp:ListItem Text="Senza variazione" Value="0"></asp:ListItem>
                     </asp:DropDownList>         
                </div>
                <div class="collable" style="width:10%">      
                    <asp:Label ID="LblRiserva" CssClass="label" AssociatedControlID="ddlRiserva" runat="server" Text="Riserva"></asp:Label>
                </div>
                <div class="colOggetti" style="width:23%">       
                    <asp:DropDownList ID="ddlRiserva" runat="server" CssClass="ddlClass">
                        <asp:ListItem Selected="True">Tutti</asp:ListItem>
                        <asp:ListItem Value="0">No</asp:ListItem>
                        <asp:ListItem Value="1">Si</asp:ListItem>
                    </asp:DropDownList>         
                </div>
                <div class="collable" style="width:10%">      
                    <asp:Label ID="lblFiltroFase" CssClass="label" 
                        AssociatedControlID="txtFiltroFase" runat="server" Text="Rif. Fase" 
                        Visible="False"></asp:Label>
                </div>
                <div class="colOggetti" style="width:23%">       
                    <asp:TextBox ID="txtFiltroFase" CssClass="textbox" runat="server" Visible="false"></asp:TextBox>
                    <asp:Label ID="lblErroreFiltroFase" CssClass="msgErrore" Text="Inserire un valore numerico." Visible="false" runat="server" />
                </div>    
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" OnClientClick="return ValidazioneClientRicerca()"/>
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
                <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
		        <br />
	            <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>&nbsp;
            </div>
        </div>
        <h3>
            <asp:label id="lblMessaggi" runat="server" AssociatedControlID="dgRicercaEnte"></asp:label>
        </h3>
        <br />
        <asp:datagrid id="dgRicercaEnte" runat="server" Width="100%" ToolTip="Elenco Enti" CssClass="table" CellPadding="2"  AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"  Visible="False" UseAccessibleHeader="True">	
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
           
                <asp:TemplateColumn HeaderText="Sel.">
                    <ItemTemplate>
					    <asp:ImageButton ID="ImbBtnEnte" CommandName="Select" ToolTip="Seleziona Ente" AlternateText="Seleziona Ente" runat="server" ImageURL="images/ente-mini.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn DataField="CodiceRegione" HeaderText="Cod.Ente">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Denominazione" HeaderText="Denominazione"></asp:BoundColumn>
				<asp:BoundColumn DataField="tiporelazione" HeaderText="Tipo di Relazione"></asp:BoundColumn>
				<asp:BoundColumn DataField="Tipologia" HeaderText="Tipologia"></asp:BoundColumn>
				<asp:BoundColumn DataField="ClasseAccreditamento" HeaderText="Dettaglio Relazione"></asp:BoundColumn>
				<asp:BoundColumn DataField="Http" Visible="False" HeaderText="HTTP"></asp:BoundColumn>
				<asp:BoundColumn DataField="Email" Visible="False" HeaderText="E-mail"></asp:BoundColumn>
				<asp:BoundColumn DataField="NumeroTotaleSedi" HeaderText="Totale Sedi"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="idente" HeaderText="idente"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="Stato" HeaderText="Stato Accordo">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="identerelazione" HeaderText="identeRelazione"></asp:BoundColumn>
				<asp:BoundColumn DataField="StatoEnte" HeaderText="Stato Ente">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="idstatoente" HeaderText="idstatoente"></asp:BoundColumn>
				<asp:BoundColumn Visible="True" DataField="DataInizioValidita" HeaderText="Data Inserimento" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:d}"></asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
    </fieldset>
</asp:Content>
