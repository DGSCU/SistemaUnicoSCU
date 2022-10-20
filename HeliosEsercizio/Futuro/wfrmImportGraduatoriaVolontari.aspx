<%@ Page Title="Importazione File Graduatorie Volontari" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="wfrmImportGraduatoriaVolontari.aspx.vb" Inherits="Futuro.wfrmImportGraduatoriaVolontari" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script language="javascript" type="text/javascript">
//<![CDATA[
   
        
    function Nascondi() {
        document.all.MainContent_lblMessaggioErrore.style.visibility = 'visible';
        document.all.MainContent_lblMessaggioErrore.style.fontSize = '1.6em'
        document.all.MainContent_lblMessaggioErrore.style.fontWeight = 'bold'
        document.all.MainContent_lblMessaggioErrore.style.color = '#3a4f63'
        document.all.MainContent_lblMessaggioErrore.innerText = 'ATTENDERE........';
        document.all.MainContent_CmdElabora.style.visibility = 'hidden';
     }
 
//]]>
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
<legend >Importazione Graduatoria Volontari</legend>

<div id="TabellaImportGraduatoriaVolontari" class="wrapper" style="width:100%">

     <div class="headers">
      <h2>
      <asp:Label ID="Label5" runat="server"  Text="Import Graduatoria Volontari"></asp:Label>
       </h2>
    </div>
   
     <div class="RigaVuota">
    
      &nbsp;</div>

    

    

     <div class="row" >
     
    
  
      <%If (Session("Sistema") = "Helios") Then%>
            <asp:Label ID="Label4" runat="server" Width="330px" Text="1) Per il download del file csv da compilare"></asp:Label>
            <asp:HyperLink ID="HplFileGraduatorie" NavigateUrl="~/WfrmConsultaDomandeOnline.aspx?Tipo=NO" runat="server">Clicca qui..</asp:HyperLink>
       <%Else%> 
             <asp:Label ID="Label6" runat="server" Width="330px" Text="1) Per il download del file csv da compilare"></asp:Label>
            <asp:HyperLink ID="HplFileGraduatorieGG" NavigateUrl="~/WfrmConsultaDomandeOnline.aspx?Tipo=NO" runat="server">Clicca qui..</asp:HyperLink>
      <% end if %>
      
    </div>

     <div class="row" >
     
            <asp:Label ID="Label3" runat="server" Width="330px" Text="2) Eseguire l'Upload del file completo"></asp:Label> 
                
                <br />
                <br />
                
     <p>
     <asp:Label ID="lblUpLoad" runat="server" AssociatedControlID="txtSelFile" Text="Seleziona File: "></asp:Label><asp:FileUpload ID="txtSelFile"  runat="server" /> &nbsp;<asp:Button ID="CmdElabora" OnClientClick="Nascondi()" CssClass="Pulsante" runat="server" Text="Elabora" /><asp:Label ID="lblMessaggioErrore" CssClass="msgErrore" runat="server" Text=""></asp:Label>
     </p>
    

    </div>
  
     <p>&nbsp;</p>
     <fieldset>
     <h3><strong>Note:</strong></h3>
  
     <br/>
    <h3>
		Il campo EsitoSelezione deve contenere:
    </h3>
     <%Call CaricaEsitoSelezione()%>
    <br />
    <h3>
	    Il campo TipoPosto deve contenere:
    </h3>
    <fieldset>
    <ul>
	    <li><strong>1</strong> per <strong>Vitto e Alloggio</strong></li>
	    <li><strong>2</strong> per <strong>Solo Vitto</strong></li>
	    <li><strong>3</strong> per <strong>Senza Vitto e Alloggio</strong></li>
    </ul>
    
   
   </fieldset>
    <%-- <h3>
	    Il campo StatoCivile deve contenere:
    </h3> 
     <%Call CaricaTipiStatoCivile()%>                           
  --%>
                           
      </fieldset>
     <div class="RigaPulsanti">
        <asp:Button 
            ID="CmdChiudi" CssClass="Pulsante" runat="server"
              Text="Chiudi"  />&nbsp;

     </div>
</div>

</fieldset>
<p>&nbsp;</p>
</asp:Content>
