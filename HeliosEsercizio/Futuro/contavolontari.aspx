<%@ Page Title="Contatore Volontari Selezionati" Language="vb" AutoEventWireup="false" CodeBehind="contavolontari.aspx.vb" Inherits="Futuro.contavolontari" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" lang="it-it" xml:lang="it-it">
<head runat="server">
    <title>Conta Volontari</title>
    		<script type="text/javascript">
			<!--
    		    var time = null
    		    function move() {
    		        window.location = 'contavolontari.aspx?blocco=1'
    		    }			
			//-->
		</script>
</head>
<body onclick="window.close()">
    <form id="form1" runat="server" >
    <fieldset class="ContornoPaginaScroll" >
<legend >Numero Volontari Selezionati</legend>
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2 style="text-align:center">
     <asp:Label ID="NumeroVolontariSelezionati" CssClass="labeldati" Font-Size="50" ForeColor="#3a4f63" Font-Bold="true" style="text-align:center"   runat="server"></asp:Label>
      </h2>
      </div>

      
      </div>
      </fieldset>
    </form>
</body>
</html>
