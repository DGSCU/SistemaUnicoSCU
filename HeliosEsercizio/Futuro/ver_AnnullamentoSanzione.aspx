<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"    CodeBehind="ver_AnnullamentoSanzione.aspx.vb" Inherits="Futuro.ver_AnnullamentoSanzione" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>

  <script type="text/javascript">
           /* <![CDATA[ */
            $(function () {
                var clientId = "#" + CostruisciId('txtDataProtAnnullamento');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('txtDataProtRipristino');
                    $("" + clientId + "").datepicker();
            });
              


                 /* ]]> */
    </script>
      <script type="text/javascript">
          function Replace(cod) {
              var str = cod; //"2011#FASC#1018/2011"; 
              var appo;
              //var appo2 ;
              for (var i = 0; i < str.length; i++) {
                  appo = str.substring(i, 1); //<BR> mid(i,str[i];
                  if (appo = !'#')

                  { }
                  else {
                      str = str.replace('#', '%23')
                  }
              }
              return str //str.replace("#", "%23"); 
          }
				
    </script>
       <script type="text/javascript">
    /* <![CDATA[ */
        var prefissoIdClient = "MainContent_";

        function CostruisciId(IdServer) {
            var IdClient = prefissoIdClient + IdServer;
            return IdClient;
        };

   
          /* ]]> */

    </script>
     <script language="JavaScript" type="text/JavaScript">
    function ApriAllegati(NumProt,DataProt)
			{
                   var IdFascicolo =  $('#<%= hfTxtCodiceFasc.ClientID %>').val();              
                  

                   if (IdFascicolo == '') {
							alert ("Specificare il numero fascicolo!");						 
					}else{
			    if (document.getElementById(CostruisciId(NumProt)).value == '') {
					    alert("Indicare il numero protocollo!");
					} else if (document.getElementById(CostruisciId(DataProt)).value == '') {
					    alert("Indicare la data protocollo!");
					} else {
					    myWin = window.open("WfrmSIGEDElencoAllegati.aspx?NumeroProtocollo=" + document.getElementById(CostruisciId(NumProt)).value + "&DataProtocollo=" + document.getElementById(CostruisciId(DataProt)).value + "&CodiceFascicolo=" + IdFascicolo, "Visualizza", "width=800,height=350,dependent=no,scrollbars=yes,status=no");
					}
			        }
		        	return false;
			}
			
			function SelProtocollo(NumProt,DataProt)
			{
			     var IdFascicolo =  $('#<%= hfTxtCodiceFasc.ClientID %>').val();
			     formId = document.forms[0];

                   if (IdFascicolo == '')  {
						alert ("Specificare il numero fascicolo!");				
					}
					else
					{
						 
						var codFasc;
						codFasc = Replace('<%= hfTxtCodiceFasc.value %>');

						myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?objForm=" + formId + "&TxtProt=" + CostruisciId(NumProt) + "&TxtData=" + CostruisciId(DataProt) + "&NumeroFascicolo=" + codFasc, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");			
					}
                      return false;

			}
             </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="firefox">
        <fieldset class="ContornoPaginaScroll">
            <legend>Annullamento/Ripristino Sanzione</legend>
            <div class="RigaVuota">
                &nbsp;</div>
            <div class="wrapper" style="width: 100%">
                <div class="headers">
                    <h2>
                        <asp:Label ID="lblTitolo" runat="server" Text="Annullamento/Ripristino Sanzione"></asp:Label>
                    </h2>
                </div>
                  <div class="rowGroup" style="height: auto">
                    <asp:Label ID="lblmessaggio" runat="server" CssClass="msgConferma"></asp:Label>
                    <asp:Label ID="lblerrore" runat="server" CssClass="msgErrore"></asp:Label>
                    <div class="RigaVuota">
                        &nbsp;</div>
                </div>
                <fieldset id="flsInformazioniGenerali" class="ContornoPagina">
                    <legend id="CaratteristicheOrganizzative">Informazioni Sanzione </legend>
                    <div class="wrapper" id="InformazioniGeneraliContent" style="width: 100%; border: 0px">
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label13" CssClass="label" AssociatedControlID="TxtTipoSanzione" runat="server"
                                    Text="Tipo Sanzione:"></asp:Label>
                            </div>
                            <div class="collable" style="width: 35%">
                                <asp:TextBox ID="TxtTipoSanzione" CssClass="textbox" runat="server" ReadOnly="True" />
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label14" CssClass="label" AssociatedControlID="TxtStatoSanzione" runat="server"
                                    Text="Stato Sanzione:"></asp:Label>
                            </div>
                            <div class="collable" style="width: 35%">
                                <asp:TextBox ID="TxtStatoSanzione" CssClass="textbox" runat="server" ReadOnly="True" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label15" CssClass="label" AssociatedControlID="TxtSanzione" runat="server"
                                    Text="Sanzione:"></asp:Label>
                            </div>
                            <div class="collable" style="width: 85%">
                                <asp:TextBox ID="TxtSanzione" CssClass="textbox" runat="server" ReadOnly="True" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label1" CssClass="label" AssociatedControlID="TxtSoggettoSanzione"
                                    runat="server" Text="Soggetto Sanzione:"></asp:Label>
                            </div>
                            <div class="collable" style="width: 85%">
                                <asp:TextBox ID="TxtSoggettoSanzione" CssClass="textbox" runat="server" ReadOnly="True" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label18" CssClass="label" AssociatedControlID="TxtNumFascicolo" runat="server"
                                    Text="Numero Fascicolo:"></asp:Label>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:TextBox ID="TxtNumFascicolo" CssClass="textbox" runat="server" ReadOnly="True" />
                                     <asp:HiddenField ID="hfTxtCodiceFasc" runat="server" />
                              <%--  <asp:TextBox
                                    ID="TxtCodiceFasc" runat="server" Width="0px"></asp:TextBox>--%>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label20" CssClass="label" AssociatedControlID="TxtDescrFascicolo" runat="server"
                                    Text="Descrizione Fascicolo:"></asp:Label>
                            </div>
                            <div class="collable" style="width: 55%">
                                <asp:TextBox ID="TxtDescrFascicolo" CssClass="textbox" runat="server" ReadOnly="True"
                                    TextMode="MultiLine" Width="99%" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblTxtDataProtInvioSanzione" CssClass="label" runat="server" Text="Applicata il:"
                                    AssociatedControlID="TxtDataProtInvioSanzione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="TxtDataProtInvioSanzione" runat="server" CssClass="textboxData"
                                    ReadOnly="True"></asp:TextBox>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblTxtNumProtInvioSanzione" CssClass="label" runat="server" Text="Numero Protocollo:"
                                    AssociatedControlID="TxtNumProtInvioSanzione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="TxtNumProtInvioSanzione" runat="server" CssClass="textbox" ReadOnly="True" />
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label2" CssClass="label" runat="server" Text="Eseguita da:" AssociatedControlID="TxtUtenteEsecutoreSanzione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 20%">
                                <asp:TextBox ID="TxtUtenteEsecutoreSanzione" runat="server" CssClass="textbox" ReadOnly="True" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label3" CssClass="label" runat="server" Text="Annullata il:" AssociatedControlID="TxtDataAnnullamento"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="TxtDataAnnullamento" runat="server" CssClass="textboxData" ReadOnly="True"></asp:TextBox>
                            </div>
                            <div class="collable" style="width: 15%">
                            </div>
                            <div class="colOggetti" style="width: 15%">
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label5" CssClass="label" runat="server" Text="Eseguita da:" AssociatedControlID="TxtUserAnnullamento"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 20%">
                                <asp:TextBox ID="TxtUserAnnullamento" runat="server" CssClass="textbox" ReadOnly="True" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label4" CssClass="label" runat="server" Text="Ripristinata il:" AssociatedControlID="TxtDataRipristino"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="TxtDataRipristino" runat="server" CssClass="textboxData" ReadOnly="True"></asp:TextBox>
                            </div>
                            <div class="collable" style="width: 15%">
                            </div>
                            <div class="colOggetti" style="width: 15%">
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label6" CssClass="label" runat="server" Text="Eseguita da:" AssociatedControlID="TxtUserRipristino"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 20%">
                                <asp:TextBox ID="TxtUserRipristino" runat="server" CssClass="textbox" ReadOnly="True" />
                            </div>
                        </div>
                    </div>
                </fieldset>
                <fieldset id="Fieldset1" class="ContornoPagina">
                    <legend id="Legend1">Annullamento Sanzione </legend>
                    <div class="wrapper" id="Div1" style="width: 100%; border: 0px">
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblTipoAnnullamento" CssClass="label" AssociatedControlID="ddlTipoAnnullamento"
                                    runat="server" Text="Tipo Annullamento"></asp:Label>
                            </div>
                            <div class="collable" style="width: 35%">
                                <asp:DropDownList ID="ddlTipoAnnullamento" runat="server" CssClass="ddlClass" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblAutoritaAnnullamento" CssClass="label" AssociatedControlID="ddlAutoritaAnnullamento"
                                    runat="server" Text="Autorità Annullamento"></asp:Label>
                            </div>
                            <div class="collable" style="width: 35%">
                                <asp:DropDownList ID="ddlAutoritaAnnullamento" runat="server" CssClass="ddlClass"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblDataProtAnn" CssClass="label" runat="server" Text="Data Prot. Annullamento"
                                    AssociatedControlID="txtDataProtAnnullamento"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="txtDataProtAnnullamento" runat="server" CssClass="textboxData" ></asp:TextBox>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblNumProtAnnullamento" CssClass="label" runat="server" Text="N° Prot. Annullamento"
                                    AssociatedControlID="txtNumProtAnnullamento"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 20%">
                                <asp:TextBox ID="txtNumProtAnnullamento" runat="server" CssClass="textbox"/>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:ImageButton ID="ImgProtocolloAnnullamento" OnClientClick="javascript: return SelProtocollo('txtNumProtAnnullamento','txtDataProtAnnullamento')"
                                    runat="server" ImageUrl="Images/busta_small.png" ToolTip="Seleziona Protocollo">
                                </asp:ImageButton>
                                <asp:ImageButton ID="ImgApriAllegatiAnnullamento" OnClientClick="javascript: return ApriAllegati('txtNumProtAnnullamento','txtDataProtAnnullamento')"
                                    runat="server" ImageUrl="images/documento_small.png" 
                                    ToolTip="Visualizza Allegati" Width="30px">
                                </asp:ImageButton>
                            </div>
                        </div>
               
                    </div>
                </fieldset>
                <fieldset id="Fieldset2" class="ContornoPagina">
                    <legend id="Legend2">Ripristino Sanzione </legend>
                    <div class="wrapper" id="Div2" style="width: 100%; border: 0px">
                        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblTipoRipristino" CssClass="label" AssociatedControlID="ddlTipoRipristino"
                                    runat="server" Text="Tipo Ripristino"></asp:Label>
                            </div>
                            <div class="collable" style="width: 35%">
                                <asp:DropDownList ID="ddlTipoRipristino" runat="server" CssClass="ddlClass" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblAutoritaRipristino" CssClass="label" AssociatedControlID="ddlAutoritaRipristino"
                                    runat="server" Text="Autorità Ripristino"></asp:Label>
                            </div>
                            <div class="collable" style="width: 35%">
                                <asp:DropDownList ID="ddlAutoritaRipristino" runat="server" CssClass="ddlClass" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </div>
              <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblDataProtRipristino" CssClass="label" runat="server" Text="Data Prot. Ripristino"
                                    AssociatedControlID="txtDataProtRipristino"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:TextBox ID="txtDataProtRipristino" runat="server" CssClass="textboxData"></asp:TextBox>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblNumProtRipristino" CssClass="label" runat="server" Text="N° Prot. Ripristino"
                                    AssociatedControlID="txtNumProtRipristino"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 20%">
                                <asp:TextBox ID="txtNumProtRipristino" runat="server" CssClass="textbox" />
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:ImageButton ID="ImgProtolloRipristino" OnClientClick="javascript: return SelProtocollo('txtNumProtRipristino','txtDataProtRipristino')"
                                    runat="server" ImageUrl="Images/busta_small.png" ToolTip="Seleziona Protocollo">
                                </asp:ImageButton>
                                <asp:ImageButton ID="ImgApriAllegatiRipristino" OnClientClick="javascript: return ApriAllegati('txtNumProtRipristino','txtDataProtRipristino')"
                                    runat="server" ImageUrl="images/documento_small.png" 
                                    ToolTip="Visualizza Allegati" Width="30px">
                                </asp:ImageButton>
                            </div>
                        </div>

        <div class="row">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="LblNoteRipristino" CssClass="label" AssociatedControlID="txtNote" runat="server"
                                    Text="Note"></asp:Label>
                            </div>
                            <div class="collable" style="width: 85%">
                                <asp:TextBox ID="txtNote" CssClass="textbox" runat="server"   TextMode="MultiLine" Width ="99%"/>
                            </div>
                        </div>



                    </div>
                </fieldset>

                              <div class="RigaPulsanti">
                    <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" Text="Salva"></asp:Button>
                    <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"></asp:Button>
                  
                </div>


            </div>
        </fieldset>
    </div>
    
</asp:Content>
