<%@ Page Title="Elenco Servizi Ente" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmGestioneServizi.aspx.vb" Inherits="Futuro.WfrmGestioneServizi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <script  type="text/javascript">
        function ControllaScelta(idChk, IdSistema) {
            if (document.forms[0].elements[idChk].value == '0') {
                document.forms[0].elements['txtCodiceEnteScelto' + IdSistema].value = '';
                document.forms[0].elements['txtCodiceEnteScelto' + IdSistema].disabled = true;
            }
            if (document.forms[0].elements[idChk].value == '1') {
                document.forms[0].elements['txtCodiceEnteScelto' + IdSistema].value = '';
                document.forms[0].elements['txtCodiceEnteScelto' + IdSistema].disabled = false;
                document.forms[0].elements['txtCodiceEnteScelto' + IdSistema].focus()
            }
            if (document.forms[0].elements[idChk].value == '2') {
                document.forms[0].elements['txtCodiceEnteScelto' + IdSistema].value = '';
                document.forms[0].elements['txtCodiceEnteScelto' + IdSistema].disabled = true;
            }
        }

        function SelezionaDeselezioneCampi(idChk, IdSistema, chkSistemaFormativo) {
            //document.all.item('ddlScelta' + IdSistema).style.visibility='visible';
            //document.forms["Form1"].elements['ddlScelta' + IdSistema].style.visibility='visible';
            if (document.forms[0].elements[idChk].checked == true) {
                if (chkSistemaFormativo == true) {
                    //document.all.ddlScelta2.style.visibility='visible';
                    //document.forms["Form1"].elements['ddlScelta' + IdSistema].style.visibility='visible';
                    //document.forms["Form1"].elements['ddlScelta' + IdSistema].value='0';
                    document.forms[0].elements['ddlScelta' + IdSistema].disabled = false;
                    document.forms[0].elements['ddlScelta' + IdSistema].value = '0';
                    //document.all.item('ddlScelta' + IdSistema).style.visibility='visible';
                    //document.all.ddlScelta+IdSistema+.style.visibility='visible';
                    //document.forms["Form1"].elements['txtCodiceEnteScelto' + IdSistema].style.visibility='visible';
                    document.forms[0].elements['txtCodiceEnteScelto' + IdSistema].disabled = true;
                    //document.forms["Form1"].elements['txtCodiceEnteScelto' + IdSistema].focus();
                    //document.all.item('txtCodiceEnteScelto' + IdSistema).style.visibility='visible';
                    //document.all.txtCodiceEnteScelto+IdSistema+.style.visibility='visible';
                }
                else {
                    //alert('txtCodiceEnteScelto' + IdSistema)
                    //document.all.txtCodiceEnteScelto6.style.visibility='visible';
                    //document.forms["Form1"].elements['txtCodiceEnteScelto' + IdSistema].style.visibility='visible';
                    document.forms[0].elements['txtCodiceEnteScelto' + IdSistema].disabled = false;
                    document.forms[0].elements['txtCodiceEnteScelto' + IdSistema].focus();
                    //document.all.item('ddlScelta' + IdSistema).style.visibility='visible';
                    //document.all.ddlScelta+IdSistema+.style.visibility='visible';
                }
            }
            else {
                if (chkSistemaFormativo == true) {
                    document.forms[0].elements['ddlScelta' + IdSistema].disabled = true;
                    //document.forms["Form1"].elements['ddlScelta' + IdSistema].style.visibility='hidden';
                    //document.all.item('ddlScelta' + IdSistema).style.visibility='hidden';
                    //document.all.ddlScelta+IdSistema+.style.visibility='visible';
                    document.forms[0].elements['txtCodiceEnteScelto' + IdSistema].disabled = true;
                    //document.forms["Form1"].elements['txtCodiceEnteScelto' + IdSistema].style.visibility='hidden';
                    //document.all.item('txtCodiceEnteScelto' + IdSistema).style.visibility='hidden';
                    //document.all.txtCodiceEnteScelto+IdSistema+.style.visibility='visible';
                }
                else {
                    document.forms[0].elements['txtCodiceEnteScelto' + IdSistema].disabled = true;
                    //document.forms["Form1"].elements['txtCodiceEnteScelto' + IdSistema].style.visibility='hidden';
                    //document.all.item('ddlScelta' + IdSistema).style.visibility='hidden';
                    //document.all.ddlScelta+IdSistema+.style.visibility='visible';
                }
            }
        }


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Elenco Servizi Ente</legend>
        <asp:label id="txtMessaggio"  runat="server" CssClass="msgErrore"></asp:label>
        <br />
        <br />
        <h3>
            <asp:label id="lblElencoServiziEnte" runat="server" Text="Elenco Servizi Ente" AssociatedControlID="dgRisultatoRicerca"></asp:label>
        </h3>
        <asp:datagrid id="dgRisultatoRicerca" runat="server" CssClass="table" Width="100%" CellPadding="2" AllowSorting="True" ToolTip="Servizi Ente" AutoGenerateColumns="False" UseAccessibleHeader="True">
			<SelectedItemStyle BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
				<asp:BoundColumn Visible="False" DataField="idsistema" HeaderText="idsistema"></asp:BoundColumn>
				<asp:BoundColumn DataField="Sistema" HeaderText="Servizi">
					<HeaderStyle HorizontalAlign="Left" Width="0px"></HeaderStyle>
					<ItemStyle HorizontalAlign="Left"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="identeSistema" HeaderText="identeSistema">
					<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
					<ItemStyle HorizontalAlign="Left"></ItemStyle>
				</asp:BoundColumn>
				<asp:TemplateColumn>
					<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<ItemTemplate>
						<asp:CheckBox id="check1" runat="server" AutoPostBack="False" ToolTip="Seleziona Servizio Ente"></asp:CheckBox>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="acquisito" HeaderText="acquisito"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="chkScelta" HeaderText="chkScelta"></asp:BoundColumn>
				<asp:BoundColumn DataField="ddlScelta" HeaderText="Scelta">
					<HeaderStyle Width="0px" HorizontalAlign="Left"></HeaderStyle>
					<ItemStyle HorizontalAlign="Left"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="TxtCodEnte" HeaderText="Cod. Ente">
                    <HeaderStyle Width="0px" HorizontalAlign="Left"></HeaderStyle>
					<ItemStyle HorizontalAlign="Left"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="IdSistemaCheck" HeaderText="IdSistemaCheck"></asp:BoundColumn>
			</Columns>
		</asp:datagrid>
        <div class="wrapper" style="width:100%; border-style:none">
            <div class="RigaVuota">&nbsp;</div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdConferma" CssClass="Pulsante" runat="server" Text="Conferma"  />&nbsp;
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
            </div>
        </div>
    </fieldset>
</asp:Content>
