<%@ Page Title="Ricerca Progetti per Volontari" Language="vb" AutoEventWireup="false"
    MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaIbanVolontari.aspx.vb"
    Inherits="Futuro.WfrmRicercaIbanVolontari" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
  /* <![CDATA[ */
  
       function SoloNumeri() 
       {
           var evtobj = window.event;
           var keyascii = evtobj.keyCode;
           if (keyascii > 57 || keyascii <= 47) 
           {
               window.event.keyCode = 0;
           }
           else 
           {
               window.event.keyCode = keyascii;
           }
       }
   
	/* ]]> */
    
   	
    </script>
  <%-- SPOSTATO LATO SERVER	<script for="chkSelDesel" event="onclick">			
				for (var i=0;i<document.elements.length;i++)
				{
					var e = document.elements[i];
					if (e.type == 'checkbox'){					
						if (e.disabled==false)
						{
							e.checked = document.Form1.chkSelDesel.checked;
						}
					}				
				}			
		</script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Ricerca Progetti </legend>
        <div class="wrapper" style="width: 100%;">
            <div class="headers">
                <h2>
                    <asp:Label ID="lblRicercaProgettiVolontari" runat="server" AssociatedControlID=""
                        Text="RICERCA PROGETTI"></asp:Label>
                </h2>
            </div>
            <div class="RigaVuota">
                &nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblTitoloProgetto" AssociatedControlID="txtTitoloProgetto" runat="server" CssClass="label"   
                        Text="Titolo Progetto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:TextBox ID="txtTitoloProgetto" autofocus="true"  runat="server"  CssClass="textbox"></asp:TextBox>
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblTipiProgetto" AssociatedControlID="DdlTipiProgetto" ToolTip="codice dell'ente" CssClass="label"
                        runat="server" Text="Tipo Progetto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="DdlTipiProgetto" runat="server" CssClass="ddlClass">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%"> 
                    <asp:Label ID="lblCodicePtogetto" AssociatedControlID="txtCodProg" runat="server" CssClass="label"
                        Text="Codice Progetto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:TextBox ID="txtCodProg"   runat="server"  CssClass="textbox"></asp:TextBox>
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblCircolare" AssociatedControlID="DdlBando" runat="server" Text="Circolare" CssClass="label"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="DdlBando" runat="server" CssClass="ddlClass">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblSettore" AssociatedControlID="ddlMaccCodAmAtt" ToolTip="codice dell'ente" CssClass="label"
                        runat="server" Text="Settore"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="ddlMaccCodAmAtt" runat="server"  AutoPostBack="True"  CssClass="ddlClass">
                    </asp:DropDownList>
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblAreaIntervento" AssociatedControlID="ddlCodAmAtt" runat="server" CssClass="label"
                        Text="Area Intervento"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="ddlCodAmAtt" CssClass="ddlClass" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
            <%If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then%>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblCodReg" AssociatedControlID="txtCodReg" runat="server" Text="Cod Ente" CssClass="label"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:TextBox ID="txtCodReg"   runat="server"  CssClass="textbox"></asp:TextBox>
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDenEnte" AssociatedControlID="txtDenominazioneEnte" runat="server" CssClass="label"
                        Text="Ente Presentante"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:TextBox ID="txtDenominazioneEnte"  runat="server"  CssClass="textbox"></asp:TextBox>
                </div>
            </div>
            <% End If%>
            <div class="RigaPulsanti">
                <asp:Button ID="CmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;<asp:Button
                    ID="CmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
            </div>
            <div class="footers">
                <asp:Label ID="Label6" runat="server" Text="">&nbsp;</asp:Label>
            </div>
            <div class="RigaVuotaPrint">
                &nbsp;
            </div>
        </div>
        <br />
         <h3>
        <asp:Label ID="lblmessaggio" runat="server" AssociatedControlID="dgRisultatoRicerca"></asp:Label>
    </h3>
    <br />
    <div class="RigaPulsanti" style="text-align: right;">
        <asp:Button ID="imgEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV"
            Visible="False" ToolTip="Esporta nel formato CSV" />
            <asp:hyperlink id="hlVolontari" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" 
        Text="DOWNLOAD CSV" runat="server" 
        ForeColor="#003399" Visible="False"></asp:HyperLink>

    </div>
    <p>
        &nbsp;
    </p>
    <div class="row">
        <div class="collable">
            <asp:CheckBox ID="chkSelDesel"   runat="server" AutoPostBack="True"
                Visible="False" Text="Seleziona Tutto"></asp:CheckBox>
        </div>
    </div>
   
    <asp:DataGrid ID="dgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco Progetti"
        CssClass="table" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True"
        PageSize="1000" ShowFooter="True">
        <FooterStyle></FooterStyle>
        <SelectedItemStyle BackColor="White"></SelectedItemStyle>
        <EditItemStyle></EditItemStyle>
        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
        <ItemStyle CssClass="tr"></ItemStyle>
        <HeaderStyle></HeaderStyle>
        <Columns>
            <asp:TemplateColumn HeaderText="Sel.">
                <HeaderStyle Width="7%"></HeaderStyle>
                <ItemTemplate>
                     <asp:CheckBox ID="chkSelProg" ToolTip="Seleziona progetto" AutoPostBack="false" runat="server" Text="Seleziona"     ></asp:CheckBox>                 
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="denominazione" HeaderText="Denominazione">
                <HeaderStyle ></HeaderStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="titolo" HeaderText="Titolo">
                <HeaderStyle ></HeaderStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="bando" HeaderText="Bando">
                <HeaderStyle ></HeaderStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="ambito" HeaderText="Settore / Area Intervento">
                <HeaderStyle HorizontalAlign="Center"  VerticalAlign="Middle"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn Visible="False" DataField="selezionato" HeaderText="selezionato">
                <HeaderStyle ></HeaderStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="statoattivit&#224;" HeaderText="Stato Progetto">
                <HeaderStyle HorizontalAlign="Center"  VerticalAlign="Middle"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn Visible="False" DataField="VolRic" HeaderText="N&#176; Vol. Con.">
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Volontari" HeaderText="N&#176; Vol.">
                <HeaderStyle HorizontalAlign="Center" ></HeaderStyle>
            </asp:BoundColumn>
            <asp:BoundColumn Visible="False" DataField="IDAttivit&#224;" HeaderText="IDAttivit&#224;">
                <HeaderStyle ></HeaderStyle>
            </asp:BoundColumn>
        </Columns>
    </asp:DataGrid>
    <p>
    </p>
    </fieldset>
    <br />
   
</asp:Content>
