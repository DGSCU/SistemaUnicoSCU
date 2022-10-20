<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfrmStoricoNote.aspx.vb" Inherits="Futuro.WfrmStoricoNote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<HTML>
	<HEAD>
		<title>Storico Note Graduatoria</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script event="onclick" for="cmdChiudi">
			window.close();	
		</script>
		<%--<script event="onclick" for="cmdSalva">
			window.opener.document.all.txtNote.value = document.all.txtNuovaNota.value;
		</script>--%>
		<script language="javascript" type="text/javascript">
        function Lunghezza() {
            if (document.all.Form2.txtNuovaNota.value.length > 199)
			{
				alert("Attenzione, è stato raggiunto il numero massimo di caratteri!");
				document.all.Form2.txtNuovaNota.focus();
				return false;
			}
            }
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<center>
			<form id="Form2" title="" method="post" runat="server">
				<TABLE id="Table1" cellSpacing="0" cellPadding="0" align="center" border="0">
					<tr>
						<td>
							<TABLE id="Table2" style="BORDER-COLLAPSE: collapse" cellSpacing="0" cellPadding="0" align="center"
								border="0">
								<TR>
									<TD style="BORDER-TOP: #6489f7 thin ridge; BORDER-LEFT: #6489f7 thin ridge; WIDTH: 75px; HEIGHT: 33px"
										align="center" bgColor="#6699ff"><asp:image id="Image1" runat="server" DESIGNTIMEDRAGDROP="109" ImageUrl="images/documento_small.png"
											Height="30px"></asp:image></TD>
									<TD style="BORDER-RIGHT: #6489f7 thin ridge; BORDER-TOP: #6489f7 thin ridge; BORDER-LEFT-COLOR: #6699ff; FONT: caption; COLOR: white; HEIGHT: 33px"
										align="center" bgColor="#6699ff"><asp:label id="lblTitolo" style="FONT: caption" runat="server" ForeColor="White" Font-Size="X-Small"
											Font-Bold="True" Width="128px">Storico Note</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;</TD>
								</TR>
								<TR>
									<TD style="BORDER-LEFT: #6699ff thin ridge; WIDTH: 75px; HEIGHT: 22px" align="right"
										bgColor="#99ccff"><font face="verdana" color="navy" size="1"><b>Nuova Nota:</b></font></TD>
									<TD style="BORDER-RIGHT: #6699ff thin ridge; FONT: caption; COLOR: navy; HEIGHT: 22px"
										align="left" bgColor="#99ccff">&nbsp;
										<asp:textbox id="txtNuovaNota" runat="server" Width="671px" onkeypress="javascript:Lunghezza()" TextMode="MultiLine" MaxLength="200"></asp:textbox><font face="verdana" color="red" size="1"><b></b></font><A href="javascript:apriCalendario()"></A></TD>
								</TR>
								<TR>
									<TD style="BORDER-RIGHT: #6699ff thin ridge; BORDER-LEFT: #6699ff thin ridge; HEIGHT: 22px"
										align="right" bgColor="#99ccff" colSpan="2"><font face="verdana" color="navy" size="1"><b></b></font></TD>
								</TR>
								<TR>
									<TD style="BORDER-RIGHT: #6699ff thin ridge; BORDER-LEFT: #6699ff thin ridge; HEIGHT: 22px"
										align="right" bgColor="#99ccff" colSpan="2"><font face="verdana" color="navy" size="1"><b><asp:imagebutton id="cmdSalva" style="CURSOR: hand" runat="server" ImageUrl="images/salva.jpg" CausesValidation="False"
													BorderWidth="1px" BorderColor="#6699FF" ToolTip="Salva" BorderStyle="Outset"></asp:imagebutton>&nbsp;<asp:imagebutton id="cmdChiudi" style="CURSOR: hand" runat="server" ImageUrl="images/chiudi.jpg"
													CausesValidation="False" BorderWidth="1px" BorderColor="#6699FF" ToolTip="Chiudi" BorderStyle="Outset"></asp:imagebutton></b></font></TD>
								</TR>
								<TR>
									<TD style="BORDER-RIGHT: #6699ff thin ridge; BORDER-LEFT: #6699ff thin ridge; HEIGHT: 22px"
										align="right" bgColor="#99ccff" colSpan="2"><font face="verdana" color="navy" size="1"><b></b></font></TD>
								</TR>
								<TR>
									<TD style="BORDER-RIGHT: #6699ff thin ridge; BORDER-LEFT: #6699ff thin ridge; BORDER-BOTTOM: #6699ff thin ridge; HEIGHT: 22px"
										align="right" bgColor="#99ccff" colSpan="2"><font face="verdana" color="navy" size="1"><b><asp:datagrid id="dtgStoricoNote" runat="server" Width="753px" BorderWidth="2px" BorderColor="#B4D5FF"
													ToolTip="Elenco Ricerca Sedi" BorderStyle="Ridge" AllowPaging="True" AutoGenerateColumns="False" AllowSorting="True" HorizontalAlign="Center" CellPadding="0" BackColor="White"
													PageSize="3">
													<FooterStyle Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
													<SelectedItemStyle Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True" ForeColor="Navy" BackColor="White"></SelectedItemStyle>
													<EditItemStyle Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Navy" BackColor="#C9DFFF"></EditItemStyle>
													<AlternatingItemStyle Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Navy" BackColor="#C9DFFF"></AlternatingItemStyle>
													<ItemStyle Font-Size="XX-Small" Font-Names="Verdana" ForeColor="Navy" BackColor="#C9DFFF"></ItemStyle>
													<HeaderStyle Font-Size="XX-Small" Font-Names="Verdana" Font-Bold="True" HorizontalAlign="Center"
														Height="30px" BorderWidth="2px" ForeColor="White" BorderStyle="Solid" BorderColor="#6489F7"
														BackColor="#6699FF"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="IdCronologiaNoteGraduatoria" HeaderText="IdNotaGraduatoria"></asp:BoundColumn>
														<asp:BoundColumn DataField="UserNameNota" HeaderText="UserName">
															<HeaderStyle Width="80px"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="DataNota" HeaderText="Data Nota">
															<HeaderStyle Width="150px"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
														</asp:BoundColumn>
														<asp:BoundColumn DataField="NoteGraduatoria" HeaderText="Nota Graduatoria"></asp:BoundColumn>
														<asp:ButtonColumn Text="&lt;img src=images/canc_small.png title='Rimuovi Sede' border=0&gt;"
															HeaderText="Rimuovi" CommandName="Rimuovi">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
														</asp:ButtonColumn>
													</Columns>
													<PagerStyle NextPageText="&gt;&gt;" Height="30px" BorderWidth="2px" Font-Size="XX-Small" Font-Names="Verdana"
														Font-Bold="True" PrevPageText="&lt;&lt;" BorderStyle="Solid" HorizontalAlign="Center" ForeColor="Navy"
														BackColor="#99CCFF" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></b></font></TD>
								</TR>
							</TABLE>
						</td>
					</tr>
				</TABLE>
			</form>
		</center>
	</body>
</HTML>

