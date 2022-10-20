<%@ Page Title="Ricerca Bandi" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmlistaBandidaGraduare.aspx.vb" Inherits="Futuro.WfrmlistaBandidaGraduare" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <script src="Scripts/JHelper.js" type="text/javascript"></script>

    <style type="text/css"> .ui-datepicker {font-size: 11px;}</style>

    <script  type="text/javascript">
    //<![CDATA[

        var prefissoIdClient = "MainContent_";

        function CostruisciId(IdServer) {
            var IdClient = prefissoIdClient + IdServer;
            return IdClient;
        };

        $(function () {
            var DataInizio = CostruisciId("txtInizio");
            var readonly = '<%=txtInizio.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataInizio = "#" + DataInizio
                $("" + sharpDataInizio + "").datepicker();
            }
        });

        $(function () {
            var DataFine = CostruisciId("txtfine");
            var readonly = '<%=txtfine.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataFine = "#" + DataFine
                $("" + sharpDataFine + "").datepicker();
            }
        });

        function ValidazioneClientRicerca() {

            var idInizio = CostruisciId("txtInizio");
            var dtInizio = document.getElementById(idInizio);
            if (dtInizio.value != '') {
                if (isCorrectFormatDate(dtInizio, "Data Inizio") == false) {
                    return false;
                }
            }

            var idFine = CostruisciId("txtfine");
            var dtFine = document.getElementById(idFine);
            if (dtFine.value != '') {
                if (isCorrectFormatDate(dtFine, "Data Fine") == false) {
                    return false;
                }
            }

            return true
        }

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

     //]]>    
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Ricerca Bandi</legend>
        <asp:label id="lblgraduatoria"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTit" runat="server"  Text="Ricerca Bandi"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblBando" CssClass="label" AssociatedControlID="ddlBando" runat="server" Text="Bando"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlBando" runat="server" CssClass="ddlClass"></asp:DropDownList>            
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="Label1" CssClass="label" AssociatedControlID="ddlCompetenze" runat="server" Text="Competenze"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlCompetenze" runat="server" CssClass="ddlClass"></asp:DropDownList>             
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblDataInizio" CssClass="label" AssociatedControlID="txtInizio" runat="server" Text="Data Inizio"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtInizio" CssClass="textbox" runat="server" Width="35%" MaxLength="10"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblDataFine" CssClass="label" AssociatedControlID="txtfine" runat="server" Text="Data Fine"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtfine" CssClass="textbox" runat="server" Width="35%" MaxLength="10"></asp:TextBox>         
                </div>
            </div>
             <div class="row" id="dvStato" runat="server">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblStato" CssClass="label" AssociatedControlID="ddlStato" runat="server" Text="Stato"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlStato" runat="server" CssClass="ddlClass"></asp:DropDownList>            
                </div>
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" OnClientClick="return ValidazioneClientRicerca()"/>
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
            </div>
        </div>
        <br />
        <p style="text-align:center">
            <asp:label id="lbltitolo" runat="server" CssClass="bold" AssociatedControlID="dgRisultatoRicerca"></asp:label>
        </p>
        <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco Bandi" CssClass="table" CellPadding="2"  AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
                <asp:TemplateColumn HeaderText="Sel"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgBando" CommandName="Select" ToolTip="Seleziona Bando" AlternateText="Seleziona Bando" runat="server" ImageURL="images/lenteIngrandimento_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn Visible="False"  DataField="idbando" HeaderText="idbando"></asp:BoundColumn>
				<asp:BoundColumn DataField="Bando" HeaderText="Bando"></asp:BoundColumn>
				<asp:BoundColumn DataField="Stato" HeaderText="Stato"></asp:BoundColumn>
				<asp:BoundColumn DataField="datainizio" HeaderText="Data Inizio Bando">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="datafine" HeaderText="Data Fine Bando">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="progetti" HeaderText="N. Progetti">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Importostanziato" HeaderText="Cifra Stanziata">
					<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
					<ItemStyle HorizontalAlign="Right"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="color" HeaderText="color"></asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
    </fieldset>
</asp:Content>
