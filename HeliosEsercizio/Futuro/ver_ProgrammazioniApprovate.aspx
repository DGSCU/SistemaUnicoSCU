<%@ Page Title="Elenco Verifiche" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="ver_ProgrammazioniApprovate.aspx.vb" Inherits="Futuro.ver_ProgrammazioniApprovate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
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
            $(function () {
                var clientId = "#" + CostruisciId('txtDataProtocolloPresenta');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('txtDataProtocolloApprova');
                    $("" + clientId + "").datepicker();
            });
             
                 /* ]]> */
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
        function SelProtocollo0() {

            var IdFascicolo = $('#<%= hfTxtCodiceFasc.ClientID %>').val();
            var idTxtCodiceFascicolo = $('#<%= TxtCodiceFascicolo.ClientID %>').val();
            alert(IdFascicolo);
            if (IdFascicolo == '') {
                alert("Specificare il numero fascicolo!");

            }
            else {
                var codFasc;
                codFasc = Replace('<%= hfTxtCodiceFasc.value %>');

                myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?NumeroFascicolo=" + codFasc, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");
            }
            return false;

        }
    </script>
    <script language="JavaScript" type="text/JavaScript">

        function ElencoFascicoli() {



            var IdTxtCodiceFascicolo = CostruisciId("TxtCodiceFascicolo");
            var IdTxtCodiceFasc = CostruisciId("hfTxtCodiceFasc");
            var IdtxtDescFasc = CostruisciId("txtDescFasc");
            var codiceFascicolo = document.getElementById(IdTxtCodiceFascicolo).value;
            var NumFascicolo = '<%= TxtCodiceFascicolo.Text %>';

            formId = document.forms[0];


            myWin = window.open("WfrmSIGEDElencoFascicoli.aspx?Processo=MONITORAGGIO&objForm=" + formId + "&objNumero=" + IdTxtCodiceFascicolo + "&objCodice=" + IdTxtCodiceFasc + "&objDescFasc=" + IdtxtDescFasc + "&IdFascicolo=" + codiceFascicolo, "Visualizza", "width=800,height=350,dependent=no,scrollbars=yes,status=no");
            return false;

        }
    </script>
    <%--
    <script language="javascript" event="onclick" for="imgStampa">
			StampaPagina()
			return false;
    </script>
    --%>
    <%--
    <script event="onclick" for="cmdFascCanc">
			PulisciCampiFascicolo();
			return false;
    </script>--%>
    <script language="JavaScript" type="text/JavaScript">

			function ApriAllegati(NumProt,DataProt)
			{
				    var IdFascicolo =  $('#<%= hfTxtCodiceFasc.ClientID %>').val();              
                     var idTxtCodiceFascicolo = $('#<%= TxtCodiceFascicolo.ClientID %>').val();   
               
					if (  idTxtCodiceFascicolo  == ''){
							alert ("Specificare il numero fascicolo!");		
					}else{	
						if (document.getElementById(CostruisciId(NumProt)).value==''){
							alert("Indicare il numero protocollo!");
						}else if (document.getElementById(CostruisciId(  DataProt)).value==''){
							alert("Indicare la data protocollo!");
						}else{
							myWin = window.open("WfrmSIGEDElencoAllegati.aspx?NumeroProtocollo=" + document.getElementById(CostruisciId(NumProt)).value + "&DataProtocollo=" + document.getElementById(CostruisciId(  DataProt)).value + "&CodiceFascicolo=" + IdFascicolo ,"Visualizza","width=800,height=350,dependent=no,scrollbars=yes,status=no");		
						}
					}
			}
			

           function SelProtocollo(NumProt,DataProt)
			{
					if ($('#<%= TxtCodiceFascicolo.ClientID %>').val()  =='')
					{
						alert ("Specificare il numero fascicolo!");				
					}
					
					else
					{
						 
						var codFasc;
						codFasc = Replace('<%= hfTxtCodiceFasc.value %>');
                        formId = document.forms[0];
						myWin = window.open("WfrmSIGEDElencoDocumenti.aspx?objForm="+formId+"&TxtProt="+CostruisciId(NumProt)+"&TxtData="+CostruisciId(DataProt)+"&NumeroFascicolo=" + codFasc, "Visualizza", "width=800,height=400,dependent=no,scrollbars=yes,status=no");			
					}
                      return false;

			}


			
			function Protocollazione(NumProt,DataProt)
			{
            
				var codFasc;		
				codFasc =  Replace('<%= hfTxtCodiceFasc.value %>');
				if (document.getElementById(CostruisciId(NumProt)).style.backgroundColor!='lightgrey')
					{
                        formId = document.forms[0];
						myWin = window.open("WfrmSIGEDProtocollazione.aspx?CorrCodUnivoco=<%= Session("pCodEnte")%>&objForm="+formId+"&TxtFasc="+ codFasc +"&TxtCodFasc="+CostruisciId("TxtCodiceFascicolo")+"&TxtDescFascicolo="+CostruisciId("txtDescFasc")+"&TxtProt="+CostruisciId(NumProt)+"&TxtData="+CostruisciId( DataProt)+"&NumeroFascicolo=" + codFasc +"&DescFascicolo=" + Form1.txtDescFasc.value  + "&CodEnte=<%= Session("pCodEnte")%>", "Visualizza","width=800,height=400,dependent=no,scrollbars=yes,status=no");			
						
					}	
			}
			
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="firefox">
        <fieldset class="ContornoPaginaScroll">
            <legend>Programmazione</legend>
            <div class="wrapper" style="width: 100%">
                <div class="headers">
                    <h2>
                        <asp:Label ID="lblTitolo" runat="server" Text="Programmazione"></asp:Label></h2>
                </div>
                <div class="RigaVuota">
                    &nbsp;</div>
                <div class="row" id="div1" runat="server">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblDesc" CssClass="label" AssociatedControlID="LblDescrProgrammazione"
                            runat="server" Text="Programmazione "></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 85%">
                        <asp:Label ID="LblDescrProgrammazione" CssClass="labelDati" AssociatedControlID="btFascicola"
                            runat="server" Text=" "></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 15%">
                        <asp:Button ID="btFascicola" runat="server" Text="Fascicola" CssClass="Pulsante"
                            Visible="False"></asp:Button>
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblCodiceFascicolo" CssClass="label" runat="server" Text="Numero Fascicolo"
                            AssociatedControlID="TxtCodiceFascicolo"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 45%">
                        <asp:TextBox ID="TxtCodiceFascicolo" CssClass="textbox" runat="server"/>
                    </div>
                    <div class="collable" style="width: 20%">
                        <asp:ImageButton ID="cmdSelFascicolo" runat="server" Visible="true" OnClientClick="javascript: return ElencoFascicoli();"
                            ToolTip="Seleziona Codice Fascicolo" AlternateText="Seleziona Codice Fascicolo"
                            ImageUrl="Images/cartella_small.png"></asp:ImageButton>
                        <asp:ImageButton ID="cmdSelProtocollo0" runat="server" Visible="true" OnClientClick="javascript: return SelProtocollo0();"
                            ImageUrl="Images/busta_small.png" ToolTip="Elenco Documenti" AlternateText="Elenco Documenti">
                        </asp:ImageButton>
                        <asp:ImageButton ID="cmdFascCanc" runat="server" Visible="true" ImageUrl="Images/canc_small.png"
                            ToolTip="Rimuovi Fascicolo/Protocolli" AlternateText="Rimuovi Fascicolo/Protocolli">
                        </asp:ImageButton>
                    </div>
                    <div class="colOggetti" style="width: 15%">
                     
          
                        <asp:HiddenField ID="hfTxtCodiceFasc" runat="server" />
                        <%--   <asp:TextBox ID="TxtCodiceFasc" runat="server" Width="0px"></asp:TextBox>--%>
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="LblDescrFascicolo" CssClass="label" AssociatedControlID="txtDescFasc"
                            runat="server" Text="Descr. Fascicolo:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 45%">
                        <asp:TextBox ID="txtDescFasc" runat="server" CssClass="textboxData" >
                        </asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblDataProtPresenta" CssClass="label" AssociatedControlID="txtDataProtocolloPresenta"
                            runat="server" Text="Data Prot. Presentazione:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 15%">
                        <asp:TextBox ID="txtDataProtocolloPresenta" runat="server" CssClass="textboxData">
                        </asp:TextBox>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblNumProtPresenta" CssClass="label" runat="server" Text="N° Prot. Presentazione:"
                            AssociatedControlID="TxtNumProtPresenta" />
                    </div>
                    <div class="colOggetti" style="width: 15%">
                        <asp:TextBox ID="TxtNumProtPresenta" CssClass="textbox" runat="server"/>
                    </div>
                    <div class="colOggetti" style="width: 20%">
                        <asp:ImageButton ID="cmdSc1SelProtocollo1" runat="server" ImageUrl="Images/busta_small.png"
                            ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo"  OnClientClick="javascript: return SelProtocollo('TxtNumProtPresenta','txtDataProtocolloPresenta')"
                            Style="cursor: hand" Width="30px"></asp:ImageButton>
                        <asp:ImageButton ID="cmdSc1Allegati1" runat="server" 
                            ImageUrl="images/documento_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati"  OnClientClick="javascript: return ApriAllegati('TxtNumProtPresenta','txtDataProtocolloPresenta')"
                            Style="cursor: hand" Width="30px"></asp:ImageButton>
                        <%--                        <asp:ImageButton ID="cmdNuovoFascioclo" runat="server" ImageUrl="images/giu.jpg"
                            ToolTip="Richiesta Protocollazione" Height="20px" OnClientClick="javascript: return Protocollazione('TxtNumProtPresenta','txtDataProtocolloPresenta')"
                            Style="cursor: hand"></asp:ImageButton>--%>
                    </div>
                </div>
                <div class="row">
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblDataProtApprova" CssClass="label" AssociatedControlID="txtDataProtocolloApprova"
                            runat="server" Text="Data Prot. Approvazione:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width: 15%">
                        <asp:TextBox ID="txtDataProtocolloApprova" runat="server" CssClass="textboxData">
                        </asp:TextBox>
                    </div>
                    <div class="collable" style="width: 15%">
                        <asp:Label ID="lblNumProtApprova" CssClass="label" runat="server" Text="N° Prot. Approvazione:"
                            AssociatedControlID="TxtNumProtApprova" />
                    </div>
                    <div class="colOggetti" style="width: 15%">
                        <asp:TextBox ID="TxtNumProtApprova" CssClass="textbox" runat="server" />
                    </div>
                    <div class="colOggetti" style="width: 20%">
                        <asp:ImageButton ID="cmdSc1SelProtocollo2" runat="server" ImageUrl="Images/busta_small.png"
                            ToolTip="Seleziona Protocollo" AlternateText="Seleziona Protocollo" OnClientClick="javascript: return SelProtocollo('TxtNumProtApprova','txtDataProtocolloApprova')"
                            Style="cursor: hand" Width="30px"></asp:ImageButton>
                        <asp:ImageButton ID="cmdSc1Allegati2" runat="server" 
                            ImageUrl="images/documento_small.png" ToolTip="Visualizza Allegati" AlternateText="Visualizza Allegati" OnClientClick="javascript: return ApriAllegati('TxtNumProtApprova','txtDataProtocolloApprova')"
                            Style="cursor: hand" Width="30px"></asp:ImageButton>
                        <%--        <asp:ImageButton ID="cmdNuovoFascioclo1" runat="server" ImageUrl="images/giu.jpg"
                            ToolTip="Richiesta Protocollazione" Height="20px" OnClientClick="javascript: return Protocollazione('TxtNumProtApprova','txtDataProtocolloApprova')"
                            Style="cursor: hand"></asp:ImageButton>--%>
                    </div>
                </div>
                <div class="RigaVuota">
                    &nbsp;</div>
                <div class="row" id="div2" runat="server">
                    <div class="collable" style="width: 85%">
                        <asp:Label ID="LblCompetenza" CssClass="label" runat="server" Text=" "></asp:Label>
                    </div>
                </div>
                <div class="RigaPulsanti">
                              <asp:Button ID="CmdStampaProgrammazione" runat="server" Text="Stampa Programmazione"
                            CssClass="Pulsante"></asp:Button>
                    <asp:Button ID="cmdCancella" runat="server" Text="Elimina Programmazione" CssClass="Pulsante"></asp:Button>
                    <asp:Button ID="imgStampa" runat="server" CssClass="Pulsante" 
                        Text="Esporta CSV" Visible="False" />
                    <asp:Button ID="CmdSalva" runat="server" Text="Salva" CssClass="Pulsante"></asp:Button>
                    <asp:Button ID="cmdChiudi" runat="server" Text="Chiudi" CssClass="Pulsante"></asp:Button>
                    
                </div>
            </div>
            <div class="wrapper" id="Div21" style="width: 100%; border: 0px">
                <div class="RigaPulsanti">
                    <asp:HyperLink ID="ApriCSV1" runat="server" AccessKey="S" CssClass="linkStampa" ForeColor="#003399"
                        Text="DOWNLOAD CSV" ToolTip="Link per la stampa del risultato della ricerca"
                        Visible="False"></asp:HyperLink>
                </div>
            </div>
            <div class="RigaVuota">
                &nbsp;</div>
                 <div class="row" style="height:auto">
                <asp:Label ID="msgErrore" CssClass="msgErrore" runat="server"></asp:Label>                
            </div>
            <asp:Label ID="lblMessaggio" runat="server" CssClass="msgConferma" ></asp:Label>
            <div class="RigaVuota">
                &nbsp;</div>
            <asp:DataGrid ID="dgRisultatoRicerca" runat="server" ToolTip="Elenco Progetti" Width="100%"
                AllowPaging="true" CssClass="table" AllowSorting="True" AutoGenerateColumns="False"
                UseAccessibleHeader="True" Visible="true">
                <FooterStyle></FooterStyle>
                <SelectedItemStyle BackColor="White" Font-Bold="true"></SelectedItemStyle>
                <EditItemStyle></EditItemStyle>
                <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                <HeaderStyle></HeaderStyle>
                <Columns>
                    <asp:BoundColumn Visible="False" DataField="idverifica" HeaderText="idverifica">
                    </asp:BoundColumn>
                    <asp:TemplateColumn Visible="false"> 
                        <ItemTemplate>
                            <asp:ImageButton ID="imgProg"   CommandName="Select" ToolTip="Programmazione" AlternateText="Programmazione" runat="server" ImageURL="images/vincoli_small.png" CausesValidation="false"></asp:ImageButton>                         
                        </ItemTemplate>
                    </asp:TemplateColumn>

                  
                    <asp:BoundColumn DataField="Ente" HeaderText="Ente"></asp:BoundColumn>
                    <asp:BoundColumn Visible="False" DataField="idattivit&#224;entesedeattuazione" HeaderText="idattivit&#224;entesedeattuazione">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Progetto" HeaderText="Progetto"></asp:BoundColumn>
                    <asp:BoundColumn DataField="ambito" HeaderText="Settore / Ambito"></asp:BoundColumn>
                    <asp:BoundColumn DataField="datafineattivit&#224;" HeaderText="Data Fine Progetto"
                        DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Sedeattuazione" HeaderText="Sede Attuazione"></asp:BoundColumn>
                    <asp:BoundColumn DataField="NumeroVolontari" HeaderText="Num. Vol."></asp:BoundColumn>
                    <asp:BoundColumn DataField="comune" HeaderText="Comune"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Regione" HeaderText="Regione"></asp:BoundColumn>
                    <asp:BoundColumn DataField="verificatore" HeaderText="Assegnata a:"></asp:BoundColumn>
                    <asp:BoundColumn Visible="False" DataField="StatoVerifiche" HeaderText="StatoVerifiche">
                    </asp:BoundColumn>
                </Columns>
                <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center"
                    CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
            </asp:DataGrid>
        </fieldset>
    </div>
</asp:Content>
