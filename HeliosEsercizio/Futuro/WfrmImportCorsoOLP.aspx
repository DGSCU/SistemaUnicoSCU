<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmImportCorsoOLP.aspx.vb" Inherits="Futuro.WfrmImportCorsoOLP" %>
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
<legend >Importazione Corso OLP</legend>
 <asp:Label ID="lblNonAbilitato" CssClass="msgErrore" AssociatedControlID="nonabilitato" runat="server" Visible="false" Text="ENTE NON ABILITATO AL CARICAMENTO DEI CORSI OLP"></asp:Label>
<div id="nonabilitato" runat="server">

<div id="TabellaImportCorsiOLP" class="wrapper" style="width:100%">
<div class="RigaVuota">
    <asp:Label ID="lblMessaggioErrore" CssClass="msgErrore" runat="server" Text=""></asp:Label>
  </div>

     <div class="headers">
      <h2>
      <asp:Label ID="Label5" runat="server"  Text="Importazione Corso OLP"></asp:Label>
       </h2>
    </div>
   
     <div class="RigaVuota">
    
      &nbsp;</div>


     

     <div class="row" >
     
    
        
          
        
      <%If (Session("Sistema") = "Helios") Then%>
            <asp:Label ID="Label4" runat="server" Width="330px" Text="1) Scaricare il file CorsoOLP.csv"></asp:Label>
            <asp:HyperLink ID="HplFileGraduatorie" NavigateUrl="~/download/Master/CorsoOLP.csv" runat="server">File CorsoOLP.csv</asp:HyperLink>
       
      <% end if %>
      
    </div>

     <div class="row" >
     
       
        
            <asp:Label ID="Label3" runat="server" Width="330px" Text="2) Eseguire l'Upload del file completo"></asp:Label> 
                
                <br />
                <br />
                
     <p>
     <asp:Label ID="lblUpLoad" runat="server" AssociatedControlID="txtSelFile" Text="Seleziona File: "></asp:Label><asp:FileUpload ID="txtSelFile"  runat="server" /> &nbsp;<asp:Button ID="CmdElabora" OnClientClick="Nascondi()" CssClass="Pulsante" runat="server" Text="Elabora" />
     </p>
    

    </div>
  
     <p>&nbsp;</p>
       <fieldset>
           
                <strong>Note:</strong> 
           <h3>
               <br />
                    Tutti i valori da inserire nel File sono Obbligatori
            </h3>  
               <br />
               <h3>
                    Il campo "NumeroOre" deve essere numerico
              
               </h3>  
                <br />
               <h3>
               Per la compilazione della colonna "Periodo Svolgimento Corso" attenersi alle seguenti indicazioni:
               </h3>  
              
               <ul>
               <li>Esempio singola data: "il 24/07/2016"</li>
               <li>Esempio pi&ugrave date: "il 25/07/2016 e 27/07/2016"</li>
               <li>Esempio periodo: "dal 24/07/2016 al 28/07/2016"</li>
               </ul>
               <br />
            <h3>
               Ogni file deve fare riferimento ad un singolo corso
          </h3>      
       </fieldset>
      <div class="RigaPulsanti">
        <asp:Button 
            ID="CmdChiudi" CssClass="Pulsante" runat="server"
              Text="Chiudi"  />&nbsp;

     </div>
     </div>
</div>
 </fieldset>
</asp:Content>
