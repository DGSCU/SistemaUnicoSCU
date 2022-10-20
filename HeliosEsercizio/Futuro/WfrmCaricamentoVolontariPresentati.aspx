<%@ Page Title="Ricerca Progetti per Volontari" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmCaricamentoVolontariPresentati.aspx.vb" Inherits="Futuro.WfrmCaricamentoVolontariPresentati" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript" >
 //<![CDATA[
  
    function Nascondi() {
        document.all.MainContent_lblMessaggioErrore.style.visibility = 'visible';
        document.all.MainContent_lblMessaggioErrore.style.fontSize = '1.6em'
        document.all.MainContent_lblMessaggioErrore.style.fontWeight = 'bold'
        document.all.MainContent_lblMessaggioErrore.style.color = '#3a4f63'
        document.all.MainContent_lblMessaggioErrore.innerText = 'ATTENDERE........';
        document.all.MainContent_CmdElabora.style.visibility = 'hidden';
     }

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
   
//]]>
     </script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<fieldset class="ContornoPagina">
    <legend >Inserimento Domande</legend>
    <div class ="wrapper" style="width:100%;">
        <div  class="headers">
            <h2>
            <asp:Label ID="Label1" runat="server" AssociatedControlID=""  Text="UPLOAD"></asp:Label>
            </h2>
             </div>
            <div class="RigaVuota">&nbsp;</div>
            <div class="row" style="text-align:left">
            <p>
                <asp:Label ID="lblUpLoad" runat="server" AssociatedControlID="txtSelFile" Text="Seleziona File: "></asp:Label><asp:FileUpload ID="txtSelFile"  runat="server"  /> &nbsp;<asp:Button ID="CmdElabora" OnClientClick="Nascondi()" CssClass="Pulsante" runat="server" Text="Elabora" /> &nbsp;<asp:Label ID="lblMessaggioErrore" CssClass="msgErrore" runat="server" Text=""></asp:Label>
             </p>
            </div>
       
    </div>
    <br/>
    <br/>
    <div class="wrapper" style="width:100%;">
        <div class="headers" >
            <h2>
            <asp:Label ID="lblRicercaProgettiVolontari" runat="server" AssociatedControlID=""  Text="RICERCA PROGETTI"></asp:Label>
            </h2>
        </div>

        <div class="RigaVuota">&nbsp;</div>

        <div class="row" >
            <div class="collable" style="width:15%">
                <asp:Label ID="lblCodicePtogetto" AssociatedControlID="txtCodiceProgetto" runat="server" Text="Codice Progetto:"/>
            </div>
            <div class="colOggetti" style="width:35%">       
                <asp:TextBox ID="txtCodiceProgetto" autofocus="true" runat="server"/>
            </div>
            <div class="collable" style="width:15%">
                <asp:Label ID="lblTitoloProgetto"  AssociatedControlID="txtTitoloProgetto" runat="server" Text="Titolo Progetto:"/>
            </div>
            <div class="colOggetti" style="width:35%" >
                <asp:TextBox ID="txtTitoloProgetto" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row" >
            <div class="collable" style="width:15%">
               <asp:Label ID="lblSettore"  AssociatedControlID="ddlMaccCodAmAtt" ToolTip="codice dell'ente" runat="server" Text="Settore:"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:DropDownList ID="ddlMaccCodAmAtt"  runat="server" AutoPostBack="True"></asp:DropDownList>        
            </div>
            <div class="collable" style="width:15%">
                <asp:Label ID="lblAreaIntervento"  AssociatedControlID="ddlCodAmAtt" runat="server" Text="Area Intervento:"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%" >
                <asp:DropDownList ID="ddlCodAmAtt" runat="server" > </asp:DropDownList>
            </div> 
        </div>
        <div class="row" >
            <div class="collable" style="width:15%">
                <asp:Label ID="lblTipoRicerca" AssociatedControlID="ddlTipo"  runat="server" Text="Tipo Ricerca:"></asp:Label>        
            </div>
            <div class="colOggetti" style="width:35%" >
                <asp:DropDownList ID="ddlTipo"  runat="server">
                    <asp:ListItem Value="0" Selected="True">TUTTI</asp:ListItem>
				    <asp:ListItem Value="1">DA LAVORARE</asp:ListItem>
				    <asp:ListItem Value="2">LAVORATI</asp:ListItem>
                </asp:DropDownList>        
            </div>
         </div> 

        <div class="RigaPulsanti">
            <asp:Button ID="cmdEsporta" CssClass="Pulsante" Text="Esporta CSV" runat="server" Visible ="False"/>&nbsp;
            <asp:Button ID="CmdRicerca" CssClass="Pulsante" Text="Ricerca" runat="server"/>&nbsp;
            <asp:Button ID="CmdChiudi" CssClass="Pulsante" Text="Chiudi" runat="server"/>
            <br />
	        <asp:HyperLink ID="hlVolontari" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
        </div>
        <div class="footers">
            <asp:Label ID="Label6" runat="server" Text="">&nbsp;</asp:Label>
        </div>
        <div class="RigaVuotaPrint"> &nbsp; </div>
    </div>
    <br />
    <br />
    <h3>
        <asp:label id="lblmessaggio" runat="server" AssociatedControlID="dgRisultatoRicerca"></asp:label>
    </h3>
    <br />
    <div>
        <asp:CheckBox id="chkSelectAll" Runat="server" Text="Seleziona tutti" Visible = "False" AutoPostBack="true" ></asp:CheckBox>   
    </div>
          
    <asp:datagrid id="dgRisultatoRicerca" runat="server"   Width="100%"
        ToolTip="Elenco Progetti"   CssClass="table"
        AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True" 
        PageSize="1000" ShowFooter="True" Caption="Risultato Ricerca Progetti">
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr"></ItemStyle>
	    <HeaderStyle></HeaderStyle>
		    <Columns>
			    <asp:TemplateColumn HeaderText="Sel." >
				    <HeaderStyle Width="3%"></HeaderStyle>
					<ItemTemplate>
						<asp:CheckBox id="chkSelProg" toolTip="Seleziona" AutoPostBack="False" runat="server" Text="&nbsp;"></asp:CheckBox>
					</ItemTemplate>
			    </asp:TemplateColumn>
			    <asp:BoundColumn DataField="idattivit&#224;" HeaderText="idattivit&#224;" Visible="false"></asp:BoundColumn>
			    <asp:BoundColumn DataField="CodiceProgetto" HeaderText="Codice Progetto"></asp:BoundColumn>
			    <asp:BoundColumn DataField="titolo" HeaderText="Titolo"></asp:BoundColumn>
			    <asp:BoundColumn DataField="bando" HeaderText="Bando"></asp:BoundColumn>
			    <asp:BoundColumn DataField="SettAmb" HeaderText="Settore / Area Intervento" Visible="false"></asp:BoundColumn>
			    <asp:BoundColumn DataField="VolontariConcessi" HeaderText="N&#176; Volontari Concessi" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"></asp:BoundColumn>
			    <asp:BoundColumn DataField="Competenza" HeaderText="Competenza"></asp:BoundColumn>
		        <asp:BoundColumn DataField="VolontariPresentati" HeaderText="Domande Ricevute" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"></asp:BoundColumn>
		    </Columns>
    </asp:datagrid>
    <p></p>
   
</fieldset>

</asp:Content>
