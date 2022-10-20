<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="modificapwd.aspx.vb" Inherits="Futuro.modificapwd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" lang="it" xml:lang="it">
<head id="Head1" runat="server">
    <title title="Modifica Password" >Modifica Password</title>
    <link href="Styles/Site.css" rel="stylesheet"  type="text/css" media="screen" />
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
	<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
	<meta name="vs_defaultClientScript" content="JavaScript">
	<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
  
		<script language="vbscript" type="text/vbscript" >
			sub CheckAlphaNumeric()
				dim intX
				dim blnCheck
				dim risposta
				
				blnCheck = "False"
				for intX = 1 to len(document.all.txtNuovaPwd.value)
					if IsNumeric(mid(document.all.txtNuovaPwd.value,1,intX)) = divue then
						blnCheck = "divue"
					end if
				next
				if blnCheck = "False" then
					alert("Attenzione. La password immessa deve rispettare i criteri di complessità richiesti.")
					exit sub
				else
					risposta=confirm("Procedere con l'operazione?")
					if risposta = false then
						exit sub
					end if
				end if
			end sub
		</script>
</head>

		

	<body  >
   
     <form id="IdFormModificaPwd" runat="server" style="width:50%;position:relative">

<div class="wrapper" style="width:100%" >
    <div class="headers" >
       <h2>
      <asp:Label ID="lblIdModificaPwd" runat="server"  Text="MODIFICA PASSWORD"></asp:Label>
      </h2>
 </div>
 
  <div class="row">
  <div class="colOggetti"  style="width:100%;">
    <asp:Image  id="imgAlert" AlternateText="Immagine alert errore." name="CmdInfo"  runat="server" Visible="false" ImageUrl="~/images/error.png" width="27" height="27"/>
     <asp:Label id="lblmessaggiosopra"  CssClass="msgErrore" runat="server"  Text="" Visible="false">
    </asp:Label>
    </div>
   
  </div>
  

    <div class="row" >

        <div class="colOggetti"  >
            <asp:Label ID="lblIdVecchiaPwd" AssociatedControlID="txtVecchiaPwd" runat="server" Text="Vecchia password"></asp:Label>
        </div>
        <div class="colOggetti" >
            <asp:TextBox ID="txtVecchiaPwd" autofocus="true" runat="server" TextMode="Password" ></asp:TextBox>
        </div>
        <div class="colOggetti" >
           
           &nbsp;
         
        </div>
        <div class="colOggetti" >
           
              &nbsp;
        </div>
  
    </div>

    <div class="row" >
    
       <div class="colOggetti" >     
           <asp:Label ID="lblIdNuovaPwd" AssociatedControlID="txtNuovaPwd"  runat="server" Text="Nuova password"></asp:Label>
       </div>
       <div class="colOggetti" >
           <asp:TextBox ID="txtNuovaPwd" runat="server" TextMode="Password" ></asp:TextBox>
       </div>
       <div class="colOggetti" >
          
            &nbsp;
         
       </div>
       <div class="colOggetti" >
           
              &nbsp;
         
       </div>
      
    </div>

    <div class="row" >
   
        <div class="colOggetti" >
            <asp:Label ID="Label8" AssociatedControlID="txtConfermaNuovaPwd" runat="server" Text="Conferma nuova password"></asp:Label>
       </div>
        <div class="colOggetti" >    
            <asp:TextBox ID="txtConfermaNuovaPwd" runat="server" TextMode="Password" ></asp:TextBox>
         
       </div>
    </div>

            <h3><strong>
				<asp:Label id="Label11" runat="server">Criteri Complessità Password</asp:Label>
			</strong></h3>
				 <h3>
					<asp:Label id="Label5"  style ="font-variant:normal;" runat="server">Regole di complessità</asp:Label>
				</h3>
                <ul>
					<li>Lunghezza: La lunghezza minima della password deve essere 8 caratteri.</li>
					<li>Complessità: Dei 4 set di caratteri (Lettere Minuscole, Lettere Maiuscole, Numeri, Interpunzioni) almeno 3 devono essere presenti nella nuova password.</li>
					<li>La nuova password non deve essere stata usata nelle ultime 10 modifiche password dell’utente.</li>
                    <li>Se la nuova password é già stata usata in passato devono essere passati almeno 6 mesi dall’ultimo uso.</li>
                </ul>
                   
                 <h3>
					<asp:Label id="Label1" style ="font-variant:normal;" runat="server">Set di caratteri  validi</asp:Label>
				</h3>
                <ul>
                    <li>Lettere Minuscole: abcdefghijklmnopqrstuvwxyz</li>
                    <li>Lettere Maiuscole : ABCDEFGHIJKLMNOPQRSTUVWXYZ</li>
                    <li>Numeri: 1234567890</li>
                    <li>Interpunzioni: : , . !</li>
                </ul>
  
    <div class="RigaPulsanti">
        <asp:Button
          ID="CmdSalva" runat="server" CssClass="Pulsante" Text="Salva" />&nbsp;<asp:Button 
            ID="imgChiudi" CssClass="Pulsante" runat="server"
              Text="Chiudi"  />

   </div>
  
   </div>

  </form>
     

</body>
</html>
