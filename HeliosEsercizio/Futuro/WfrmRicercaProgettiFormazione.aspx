<%@ Page Title="Ricerca Progetti Per Formazione Corsi " Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaProgettiFormazione.aspx.vb" Inherits="Futuro.WfrmRicercaProgettiFormazione" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Ricerca Progetti Per Formazione Corsi</legend>
        <asp:label id="lblmessaggio"  runat="server" CssClass="msgErrore" Visible="False" AssociatedControlID="dgRisultatoRicerca"></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
                <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Progetti Per Formazione Corsi"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblTitoloProgetto" CssClass="label" AssociatedControlID="txtTitoloProgetto" runat="server" Text="Titolo Progetto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtTitoloProgetto" autofocus="true" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblTipoProgetto" CssClass="label" AssociatedControlID="DdlTipiProgetto" runat="server" Text="Tipo Progetto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="DdlTipiProgetto" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodiceProgetto" CssClass="label" AssociatedControlID="txtCodProg" runat="server" Text="Codice Progetto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodProg" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCircolare" CssClass="label" AssociatedControlID="DdlBando" runat="server" Text="Circolare"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="DdlBando" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblStatiProgetto" CssClass="label" AssociatedControlID="ddlStatoAttivita" runat="server" Text="Stati Progetto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlStatoAttivita" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                </div>  
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblSettore" CssClass="label" AssociatedControlID="ddlMaccCodAmAtt" runat="server" Text="Settore"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlMaccCodAmAtt" runat="server" CssClass="ddlClass" AutoPostBack="true"></asp:DropDownList>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblAreaIntervento" CssClass="label" AssociatedControlID="ddlCodAmAtt" runat="server" Text="Area Intervento"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlCodAmAtt" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                </div>  
            </div>
            <div class="row" id="divCodiceEnte" runat="server">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodReg" CssClass="label" AssociatedControlID="txtCodReg" runat="server" Text="Cod Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodReg" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDenEnte" CssClass="label" AssociatedControlID="txtDenominazioneEnte" runat="server" Text="Ente Presentante"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtDenominazioneEnte" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblEnteSecondario" CssClass="label" AssociatedControlID="txtDenominazioneEnteSecondario" runat="server" Text="Ente Secondario"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtDenominazioneEnteSecondario" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblRegione" CssClass="label" AssociatedControlID="txtRegione" runat="server" Text="Regione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtRegione" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblProvincia" CssClass="label" AssociatedControlID="txtProvincia" runat="server" Text="Provincia"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtProvincia" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblComune" CssClass="label" AssociatedControlID="txtcomune" runat="server" Text="Comune"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtcomune" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDatiPrevisionali" CssClass="label" runat="server" Text="Dati Previsionali"></asp:Label>
                </div>
                <div class="colOggetti" style="width:50%">
                    <asp:RadioButton ID="optOreTutte" Text="Tutto" GroupName="gOre" runat="server" Checked="True"/>&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="optOreSi" Text="Si" GroupName="gOre" runat="server"/>&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="optOreNo" Text="No" GroupName="gOre" runat="server"/>
                </div>
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
                <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
		        <br />
	            <asp:HyperLink ID="hlVolontari" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
            </div>
            <div class="row" id="divPianificazioniConfermate" runat="server">
                <div class="colOggetti" style="width:50%;">      
                    <asp:Image ImageUrl="images/Lotus.jpg" id="imgLotus" runat="server" AlternateText="Progetti Pianificazioni Confermate"/>&nbsp;
                    <asp:Label ID="lblPianificazioniConfermate" CssClass="label" runat="server" Text="Progetti Pianificazioni Confermate" AssociatedControlID="imgLotus"></asp:Label>
                </div>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="colOggetti">
                    <asp:checkbox id="chkSelDesel" runat="server" Text="Seleziona tutto" Visible="False" AutoPostBack="true"></asp:checkbox>
                </div>
            </div>
            <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco Progetti" CssClass="table" CellPadding="3"  AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
				<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		        <HeaderStyle></HeaderStyle>
				<Columns>
					<asp:TemplateColumn HeaderText="Sel.">
						<HeaderStyle Width="1%"></HeaderStyle>
						<ItemTemplate>
							<asp:CheckBox id="chkSelProg" toolTip="Seleziona" AutoPostBack="False" runat="server" Text="&nbsp;"></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="NomeEnte" HeaderText="Denominazione">
						<HeaderStyle Width="12%"></HeaderStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="titolo" HeaderText="Titolo">
						<HeaderStyle Width="6%"></HeaderStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="bando" HeaderText="Bando">
						<HeaderStyle Width="12%"></HeaderStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="ambito" HeaderText="Settore / Area Intervento">
						<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="selezionato" HeaderText="selezionato">
						<HeaderStyle Width="1%"></HeaderStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="statoattivit&#224;" HeaderText="Stato Progetto">
						<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Volontari" HeaderText="N&#176; Vol.">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					</asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="IdAttivit&#224;" HeaderText="IdAttivit&#224;"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="idbando"></asp:BoundColumn>
					<asp:BoundColumn DataField="DatiPianificazione" HeaderText="Dati Pianificazione (Si/No)"></asp:BoundColumn>
				</Columns>
			</asp:datagrid>
        </div>
        <asp:HiddenField id="txtTitoloProgetto1" runat="server" />
        <asp:HiddenField id="txtDenominazioneEnte1" runat="server" />
        <asp:HiddenField id="ddlMaccCodAmAtt1" runat="server" />
        <asp:HiddenField id="ddlCodAmAtt1" runat="server" />
        <asp:HiddenField ID="ddlStatoAttivita1" runat="server" />
    </fieldset>
</asp:Content>
