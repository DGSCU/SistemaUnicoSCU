<%@ Page Title="Elenco Protocollazione" Language="vb" AutoEventWireup="false"  CodeBehind="WfrmSIGEDProtocollazione.aspx.vb" Inherits="Futuro.WfrmSIGEDProtocollazione" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
	<head>
		<title>Elenco Protocollazione</title>
        	 <link href="Styles/Site.css" rel="stylesheet"  type="text/css" media="screen" />
        <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
	    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
	    <meta name="vs_defaultClientScript" content="JavaScript"/>
	    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
        <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script  type="text/javascript">
     /* <![CDATA[ */
        var formId;

        function ImpostaFormId() {
            formId = document.forms[0].id;
            return formId;
        }

    function Conferma(){
            var result = true;
			if (document.getElementById("txtSelFile").value == "")
			{
				alert("Selezionare il file da inviare.")
				result = false;			
			}
		
			else if ( '<%= txtCodFasc.value %>'  && document.getElementById("txtDescFascicolo").value == '')
			{
				alert("Indicare la descrizione del fascicolo.")
				result= false;			
			}
			
			else if (document.getElementById("txtOggetto").value == "")
			{
				alert("Indicare la descrizione dell'oggetto.")
				result= false;			
			}
			if(result== true){
			document.getElementById("imgConferma").Enabled = false;
            }
            return result
		};
          /* ]]> */
</script>

    </head>
	<body>

    <form id="form1" method="post" runat="server">
    <fieldset class="ContornoPagina" >
       <legend>Invio file SIGED</legend>
    <div class="wrapper"  style="width:100%;border:0px">
    <div class= "row" style="height:auto" >
        <asp:label id="lblmessaggio"  runat="server" CssClass="msgErrore" ></asp:label>
        <asp:HiddenField id="txtCodFasc" runat="server" />
     </div>
     <div class= "RigaVuota">&nbsp;
     </div  >
     <div class= "row"  >
        <div class="collable" style="width:15%" >
            <asp:label ID="LblSelFile" CssClass="label" AssociatedControlID="txtSelFile" runat="server" Text="Selezionare il file" ></asp:label>
        </div>
        <div class="colOggetti" style="width:85%" >
            <asp:FileUpload ID="txtSelFile" style="width:90%"  runat="server"></asp:FileUpload>
        </div>
    </div>
     <div class= "row"  >
        <div class="collable" style="width:15%" >
            <asp:Label ID="lblDescFasc" CssClass="label" AssociatedControlID="txtDescFascicolo" runat="server" Text="Descr. Fascicolo"></asp:Label>
        </div>
        <div class="colOggetti" style="width:85%" >
            <asp:TextBox ID="txtDescFascicolo" CssClass="textbox" TextMode="MultiLine" runat="server" ></asp:TextBox>
        </div>
       </div>
      <div class= "RigaVuota"  > &nbsp;
      </div>
     <div class= "row">
        <div class="collable" style="width:15%" >
            <asp:Label ID="LabelChkProtocollo" CssClass="label" AssociatedControlID="chkProtocollo" runat="server" Text="Protocollo"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%" >
            <asp:CheckBox ID="chkProtocollo"  Text="SI" runat="server" ></asp:CheckBox>
        </div>
          <div class="collable" style="width:15%" >
            <asp:Label ID="lblOggetto" CssClass="label" AssociatedControlID="txtOggetto" runat="server" Text="Oggetto" ></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%" >
            <asp:TextBox ID="txtOggetto" CssClass="textbox"  runat="server" ></asp:TextBox>
        </div>
       </div>
    <div class= "row">
        <div class="collable" style="width:15%" >
        <asp:Label ID="LblTipoDoc" CssClass="label" AssociatedControlID="cboTipoDocumento" runat="server" Text="Tipo Documento"></asp:Label>
        </div>
    <div class="colOggetti" style="width:35%" >
        <asp:DropDownList ID="cboTipoDocumento"  CssClass="ddlClass" runat="server" ></asp:DropDownList>
    </div>
    <div class="collable" style="width:15%" >
    <asp:Label ID="LblTitolario" CssClass="label" AssociatedControlID="cboTitolario" runat="server" Text="Titolario" ></asp:Label>
    </div>
    <div class="colOggetti" style="width:35%" >
        <asp:DropDownList ID="cboTitolario"  CssClass="ddlClass" runat="server" ></asp:DropDownList>
    </div>
</div>

<div   class="RigaPulsanti">
    <asp:Button 
    ID="imgConferma" CssClass="Pulsante" runat="server" OnClientClick="return Conferma();" Text="Conferma"  />

</div>
</div>
</fieldset>

    </form>
</body>
</html>

