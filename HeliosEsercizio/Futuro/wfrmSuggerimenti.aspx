<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrmSuggerimenti.aspx.vb" Inherits="Futuro.wfrmSuggerimenti" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" lang="it" xml:lang="it">
<head runat="server">

<link href="Styles/Site.css" rel="stylesheet" type="text/css" />
 
    <title>Suggerimenti Indirizzi</title>

</head>
<body >
    <form id="form1" runat="server">
            
  <fieldset class="ContornoPagina">
<legend >Suggerimento Indirizzo</legend>
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:Label ID="Label5" runat="server"  Text="Suggerimento per"></asp:Label><asp:Label id="lblIndirizzo" runat="server"></asp:Label>
      </h2>
     </div>
   
    <div class="RigaVuota">
       <asp:Label ID="lblidcomune"  runat="server" ></asp:Label>
      &nbsp;</div>

    <div class="row" >
  
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:Label ID="lblRicercaIndirizzo" runat="server" Width="150px" AssociatedControlID="txtIndirizzo" Text="Ricerca Indirizzo"></asp:Label>
        <asp:TextBox ID="txtIndirizzo" runat="server"></asp:TextBox>

        <asp:AutoCompleteExtender 
            ID="AutoCompleteExtender1" 
            TargetControlID="txtIndirizzo" 
            runat="server" UseContextKey="True" MinimumPrefixLength="5"    ServiceMethod="GetCompletionList" >
        </asp:AutoCompleteExtender>
        <asp:Button ID="CmdRicerca" runat="server" CssClass="Pulsante"  Text="Cerca" />
     
    </div>
  
    <div class="RigaVuota">

      &nbsp;</div>
    

     
   
    <div class="row" style="height:100%" >
  
        <asp:Label id="lblSuggerimenti"  runat="server" ></asp:Label>
   
    </div>
   
</div></fieldset>
<fieldset class="ContornoPagina">
<legend>Risultato Indirizzi Ricercati</legend>
<asp:datalist ID="listaIndirizzi" CssClass="bold" runat="server" Visible="true">
 <ItemTemplate>
 <%# Eval("Indirizzo")%>
        
        </ItemTemplate>
</asp:datalist>

</fieldset>
    </form>
</body>

</html>
