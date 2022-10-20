<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmDatiStampaQuest.aspx.vb" Inherits="Futuro.WfrmDatiStampaQuest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml"  xml:lang="it" lang="it">
<head runat="server">
    <title>Stampa Modulo "F"</title>

    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    
    <style type="text/css"> .ui-datepicker {font-size: 11px;}</style>

    <script  type="text/javascript">
        
        $(function () {
            var DataNascita ="txtDataNascita";
            var readonly = '<%=txtDataNascita.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataNascita = "#" + DataNascita
                $("" + sharpDataNascita + "").datepicker();
            }
        });

    </script>

</head>
<body>
    <form id="frmMain" runat="server" method="post">
        <div class="fieldsetrasparente">
            <fieldset class="ContornoPagina">
                <legend>Stampa Modulo "F"</legend>
                <h3>Allegare al "Modulo F" copia documento del dichiarante - NON inviare per posta il questionario, i fogli firma e quant'altro non richiesto.</h3>
                <asp:label id="lblErr"  runat="server" CssClass="msgErrore"></asp:label>
                <div class="wrapper" style="width:100%">
                    <div class="headers" >
                        <h2><asp:Label ID="lblTitolo" runat="server" Text='Stampa Modulo "F"'></asp:Label></h2>
                    </div>
                    <div class="RigaVuota" >&nbsp;</div>
                    <div class="row" >
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblCognome" CssClass="label" AssociatedControlID="txtCognome" runat="server" Text="Cognome"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtCognome" CssClass="textbox" runat="server"></asp:TextBox>         
                        </div>
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblNome" CssClass="label" AssociatedControlID="txtNome" runat="server" Text="Nome"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtNome" CssClass="textbox" runat="server"></asp:TextBox>         
                        </div>        
                    </div>
                    <div class="row" >
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblProvinciaNascita" CssClass="label" AssociatedControlID="ddlProvinciaNascita" runat="server" Text="Provincia/Nazione di nascita"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:25%">       
                            <asp:dropdownlist id="ddlProvinciaNascita" CssClass="ddlClass" AutoPostBack="true" runat="server"  ></asp:dropdownlist>           
                        </div>  
                           <div class="collable" style="width:10%">
            <asp:CheckBox ID="ChkEstero"  AutoPostBack="true" Text="Estero" runat="server" />
            </div>
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblComuneNascita" CssClass="label" AssociatedControlID="ddlComuneNascita" runat="server" Text="Comune/Nazione di nascita"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:dropdownlist id="ddlComuneNascita" CssClass="ddlClass" runat="server">
                            </asp:dropdownlist> 
                             
                        </div>    
                    </div>
                    <div class="row" >
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblDataNascita" CssClass="label" AssociatedControlID="txtDataNascita" runat="server" Text="Data di nascita"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtDataNascita" CssClass="textbox" runat="server" Width="75px" MaxLength="10"></asp:TextBox>         
                        </div>
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblCAPNascita" CssClass="label" AssociatedControlID="txtCAPNascita" runat="server" Text="Cap"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtCAPNascita" CssClass="textbox" runat="server" Width="64px"></asp:TextBox>         
                        </div>        
                    </div>
                    <div class="row" >
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblNominativo" CssClass="label" AssociatedControlID="txtNominativo" runat="server" Text="Nominativo da contattare"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtNominativo" CssClass="textbox" runat="server"></asp:TextBox>         
                        </div>
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblPosizione" CssClass="label" AssociatedControlID="txtPosizione" runat="server" Text="Ruolo nell'ente"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtPosizione" CssClass="textbox" runat="server"></asp:TextBox>         
                        </div>        
                    </div>
                    <div class="row" >
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblImportoEuro" CssClass="label" AssociatedControlID="txtImportoEuro" runat="server" Text="Importo Complessivo" ></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtImportoEuro" CssClass="textbox" runat="server" ReadOnly="true" Width="136px"></asp:TextBox>         
                        </div>
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblImportoLettere" CssClass="label" AssociatedControlID="txtImportoLettere" runat="server" Text="Importo complessivo (in lettere)"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtImportoLettere" CssClass="textbox" runat="server" ReadOnly="true"></asp:TextBox>         
                        </div>        
                    </div>
                </div>
                <br />
                <br />
                <p style="text-align:center">
                    <asp:label id="LblTipo" runat="server" Text="TIPO ACCREDITO" CssClass="bold"></asp:label>
                </p>
                <div class="wrapper" style="width:100%">
                    <div class="RigaVuota" >&nbsp;</div>
                    <div class="row" >
                        <div class="colOggetti" style="width:100%">
                            <asp:radiobutton id="optEntePubblico" runat="server" AutoPostBack="True" GroupName="optAccredito" Text="1) Enti Pubblici cui si applica l' art. 35, comma 8, 9 e 10 del D.L. 24 Gennaio 2012, n° 1" CssClass="bold"></asp:radiobutton>
                        </div>
                                  
                    </div>
                    <div class="row" >
                        <div class="collable" style="width:15%">  
                            <asp:Label ID="lblIbanTesoreria" CssClass="label" 
                                AssociatedControlID="txtIbanTesoreria" runat="server" 
                                Text="IBAN TESORERIA" Visible="True"></asp:Label>
                            <asp:Label ID="lblTesoreriaProvinciale" CssClass="label" 
                                AssociatedControlID="ddlProvincia" runat="server" 
                                Text="Tesoreria Provinciale dello Stato - (provincia)" Visible="False"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%"> 
                         <asp:TextBox ID="txtIbanTesoreria" CssClass="textbox" runat="server" MaxLength="27"></asp:TextBox>        
                            <asp:DropDownList ID="ddlProvincia" runat="server" CssClass="ddlClass" 
                                Visible="False"></asp:DropDownList>         
                        </div>
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="Label2" CssClass="label" AssociatedControlID="txtNConto" Visible="False" runat="server" Text="N. Conto"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtNConto" CssClass="textbox" runat="server" MaxLength="6" 
                                width="200px" Visible="False"></asp:TextBox>         
                        </div>        
                    </div>
                </div>
                <br />
                <div class="wrapper" style="width:100%">
                    <div class="RigaVuota" >&nbsp;</div>
                    <div class="row" >
                        <div class="colOggetti" style="width:100%">
                            <asp:radiobutton id="optAltriEnti" runat="server" AutoPostBack="True" GroupName="optAccredito" Text="2) Altri Enti:" CssClass="bold"></asp:radiobutton>
                        </div>
                    </div>
                    <div class="row" >
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblIBAN" CssClass="label" AssociatedControlID="txtIban" runat="server" Text="IBAN"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtIban" CssClass="textbox" runat="server" MaxLength="27"></asp:TextBox>         
                        </div>
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblBIC" CssClass="label" AssociatedControlID="txtBicSwift" runat="server" Text="BIC/SWIFT"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtBicSwift" CssClass="textbox" runat="server" MaxLength="11"></asp:TextBox>         
                        </div>        
                    </div>
                </div> 
                <div class="wrapper" style="width:100%; border-style:none">
                    <div class="RigaVuota">&nbsp;</div>
                    <div class="row">
                        <div class="collable" style="width:15%">
                            <asp:Label ID="lblRimborso" CssClass="label" runat="server" Text="Chiede Rimborso"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">
                            <asp:RadioButton ID="optRimborsoSi" Text="Si" GroupName="gRimborso" runat="server" />&nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="optRimborsoNo" Text="No" GroupName="gRimborso" runat="server"/>
                        </div>
                    </div>
                    <h3>Si ricorda che insieme al Modulo F deve essere inviato l&#39;Allegato 1 per i progetti Italia e l&#39;Allegato 2 per i progetti  estero.
                        I due allegati sono reperibili al seguente indirizzo:  <br />https://www.serviziocivile.gov.it/main/area-enti-hp/formazione/moduli-per-la-formazione/ .<br />
                        I documenti (Modulo F, 
                        Allegato 1, Allegato 2) dovranno essere firmati digitalmente oppure dovra&#39; essere allegata copia del Documento di riconoscimento del Rappresentante legale o del Responsabile del servizio civile.<br />
                        La notizia relativa a questo “memorandum” è stata pubblicata sul sito del Dipartimento il 9/5/2017. </h3>
                    <div class="RigaPulsanti" style="width:100%">
                        <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" Text="Salva" ToolTip="Salva e Stampa"/>&nbsp;
                        <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" ToolTip="Chiudi"/>
                    </div>
                </div>
                <asp:HiddenField id="chkKeyPressNascita" runat="server" />
                <asp:HiddenField id="chkKeyLenComune" runat="server" />
                <asp:HiddenField id="txtIDComuneNascita" runat="server"/>        
            </fieldset>
        </div>
    </form>
</body>
</html>
