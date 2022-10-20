Imports System.Net
Imports System.Data.SqlClient
Imports System.Web.UI
Imports Logger
Imports Logger.Data

Public Class SmartPage
	Inherits Page
	Public Log As Logger.Logger

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

		Dim ipAddress As String = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
		If String.IsNullOrEmpty(ipAddress) Then
			ipAddress = Request.ServerVariables("REMOTE_ADDR")
		End If

		Dim controllerName As String = Request.FilePath
		Dim actionName As String = Nothing
		Dim methodName As String = Request.HttpMethod
		Dim sessionId As String = Session.SessionID
		Dim username As String = If(Session("CodiceFiscaleUtente"), Session("Account"))
		Dim ente As String = Session("CodiceFiscaleEnte")
		If ente Is Nothing And Session("conn") IsNot Nothing And Session("idEnte") IsNot Nothing Then
			ente = ClsServer.GetCodiceFiscaleEnte(Session("idEnte").ToString, Session("conn"))
		End If

		Dim hostname As String = Dns.GetHostName()
		Dim hostIp As String = Dns.GetHostAddresses(hostname).FirstOrDefault(Function(a) a.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork).ToString()
		Log = New Logger.Logger(controllerName, actionName, methodName, username:=username, ente:=ente, ipAddress:=ipAddress, applicationIpAddress:=hostIp, applicationHost:=hostname, sessionId:=sessionId)
	End Sub


	Public Sub SetLog(ByVal methodName As String, ByVal controllerName As String, ByVal actionName As String, ByVal sessionId As String)
		Dim ipAddress As String = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
		If String.IsNullOrEmpty(ipAddress) Then
			ipAddress = Request.ServerVariables("REMOTE_ADDR")
		End If
		'Dim claim = User.Claims.FirstOrDefault(Function(c) c.Type = "User")?.Value
		'Dim userinfo As userInfo = Nothing

		'If claim IsNot Nothing Then
		'	userinfo = JsonConvert.DeserializeObject(Of userInfo)(claim)
		'End If

		Dim hostname As String = Dns.GetHostName()
		Dim hostIp As String = Dns.GetHostAddresses(hostname).FirstOrDefault(Function(a) a.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork).ToString()
		Log = New Logger.Logger(controllerName, actionName, methodName, username:="", name:="", ente:="", ipAddress:=ipAddress, applicationIpAddress:=hostIp, applicationHost:=hostname, sessionId:=sessionId)
	End Sub

	Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
		If Log IsNot Nothing Then
			Log.Trace("Pagina Terminata", 21)
		End If

    End Sub

    Protected Sub checkSpid(Optional ByVal Progetti As Boolean = False)
        'in test e' possibile entrare anche senza essere passati da SPID
        If ConfigurationSettings.AppSettings("IsTest") = "1" Or Session("TipoUtente") <> "E" Then Return

        If Progetti = False Then
            If Session("CodiceFiscaleUtente") & "" = "" Then
                Response.Redirect("WfrmMain.aspx?nospid=1")
            End If
        Else
            Dim Alboente As String = "SCU"
            If Session("conn") IsNot Nothing And Session("idEnte") IsNot Nothing Then
                Alboente = ClsServer.GetAlboEnte(Session("idEnte").ToString, Session("conn"))
            End If

            If Session("CodiceFiscaleUtente") & "" = "" And Alboente = "SCU" Then
                Response.Redirect("WfrmMain.aspx?nospid=1")
            End If
        End If

    End Sub

	Protected Sub CaricaAllegatoFromDatarow(ByVal dr As DataRow, ByVal SessionName As String, ByVal RowNoFile As HtmlGenericControl, RowWithFile As HtmlGenericControl,
		ByVal txtFileName As Literal, ByVal txtHash As Literal, ByVal txtData As Literal)

		Dim _allegato As New Allegato
		_allegato.Filename = CStr(dr("FileName"))
		_allegato.Blob = DirectCast(dr("BinData"), Byte())
		_allegato.DataInserimento = dr("DataInserimento")
		_allegato.Filesize = CInt(dr("FileLength").ToString())
		_allegato.Hash = dr("HashValue").ToString()
		_allegato.IdTipoAllegato = CInt(dr("IdTipoAllegato").ToString())
		_allegato.Id = CInt(dr("IdEnteDocumento"))
		Session(SessionName) = _allegato
		Dim linkValidazione As String = ""
		If Session("TipoUtente") = "U" Then
			Dim sStato = "Da validare"
			If CInt(dr("Stato").ToString()) = 1 Then
				sStato = "Valido"
			ElseIf CInt(dr("Stato").ToString()) = 2 Then
				sStato = "Non Valido"
            End If
            If dr("IdEnteFase").ToString <> "" Then
                linkValidazione = String.Format(" <a href=""wfrmConsultaDocumenti.aspx?Vengoda=ConsultaDocumenti&tipologia=Enti&idDocumento={0}&IdEnteFase={2}"">[{1}]</a>",
                 _allegato.Filename, sStato, dr("IdEnteFase").ToString)
            End If
        End If
        If _allegato IsNot Nothing Then
            'Se l'allegato è caricato in Sessione (Inserimento)
            RowNoFile.Visible = False
            RowWithFile.Visible = True
            txtFileName.Text = _allegato.Filename & linkValidazione
            txtHash.Text = _allegato.Hash
            txtData.Text = _allegato.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        Else
            'Se l'allegato non è ancora caricato
            RowNoFile.Visible = True
            RowWithFile.Visible = False
        End If


	End Sub
	Protected Sub CaricaAllegatoFromDB(ByVal IdAllegato As Integer, ByVal SessionName As String, ByVal RowNoFile As HtmlGenericControl, RowWithFile As HtmlGenericControl,
		ByVal txtFileName As Literal, ByVal txtHash As Literal, ByVal txtData As Literal)
		Dim sqlGetAllegato = "SELECT Top 1 IdTipoAllegato,FileName,HashValue, len(BinData) FileLength,BinData,DataInserimento, IdEnteFase,Stato From entidocumenti WHERE IdEnteDocumento = @idAllegato"
		Dim getAllegatoCommand As New SqlCommand(sqlGetAllegato, Session("conn"))
		getAllegatoCommand.Parameters.AddWithValue("@idAllegato", IdAllegato)
		Dim dr As SqlDataReader = getAllegatoCommand.ExecuteReader()
		If dr.Read Then
			Dim _allegato As New Allegato
			_allegato.Filename = CStr(dr("FileName"))
			_allegato.Blob = DirectCast(dr("BinData"), Byte())
			_allegato.DataInserimento = dr("DataInserimento")
			_allegato.Filesize = CInt(dr("FileLength").ToString())
			_allegato.Hash = dr("HashValue").ToString()
			_allegato.IdTipoAllegato = CInt(dr("IdTipoAllegato").ToString())
			_allegato.Id = IdAllegato
			Session(SessionName) = _allegato
			Dim linkValidazione As String = ""

			If Session("TipoUtente") = "U" Then
				Dim sStato = "Da validare"
				If CInt(dr("Stato").ToString()) = 1 Then
					sStato = "Valido"
				ElseIf CInt(dr("Stato").ToString()) = 2 Then
					sStato = "Non Valido"
				End If
				linkValidazione = String.Format(" <a href=""wfrmConsultaDocumenti.aspx?Vengoda=ConsultaDocumenti&tipologia=Enti&idDocumento={0}&IdEnteFase={2}"">[{1}]</a>",
				_allegato.Filename, sStato, dr("IdEnteFase").ToString)
			End If

			If _allegato IsNot Nothing Then
				'Se l'atto di  è caricato in Sessione (Inserimento)
				RowNoFile.Visible = False
				RowWithFile.Visible = True
				txtFileName.Text = _allegato.Filename & linkValidazione
				txtHash.Text = _allegato.Hash
				txtData.Text = _allegato.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
			Else
				Session(SessionName) = Nothing
				RowNoFile.Visible = True
				RowWithFile.Visible = False
			End If
		End If
		dr.Close()

	End Sub
	Function SalvaAllegato(A As Allegato, IdTipoAllegato As Integer, IdEnteFase As Integer, MyCommand As SqlClient.SqlCommand) As Integer
		'L'allegato va sempre in insert per tenere lo storico
		Dim strsql As String
		Dim _ret As Integer
		MyCommand.Parameters.Clear()
		MyCommand.Parameters.AddWithValue("@IdTipoAllegato", IdTipoAllegato)
		MyCommand.Parameters.AddWithValue("@BinData", A.Blob)
		MyCommand.Parameters.AddWithValue("@FileName", A.Filename)
		MyCommand.Parameters.AddWithValue("@HashValue", A.Hash)
		MyCommand.Parameters.AddWithValue("@UsernameInserimento", If(Session("CodiceFiscaleUtente"), Session("Account")))
		MyCommand.Parameters.AddWithValue("@IdEnteFase", IdEnteFase)
		strsql = "DELETE FROM entidocumenti where IdEnteFase=@IdenteFase and FileName=@FileName; " _
			& "INSERT INTO entidocumenti ( IdTipoAllegato, BinData, FileName, HashValue, DataInserimento, UsernameInserimento, IdEnteFase) " _
			& " VALUES (@IdTipoAllegato,@BinData,@FileName,@HashValue,GETDATE(),@UsernameInserimento,@IdEnteFase); " _
			& "SELECT SCOPE_IDENTITY()"
		MyCommand.CommandText = strsql
		_ret = CInt(MyCommand.ExecuteScalar)
		A.Id = _ret
		Return _ret
	End Function
    Function CercaAllegato(A As Allegato, IdEnteFase As Integer) As Boolean
        Dim _ret As Boolean = False

        Dim MyCommand As New SqlCommand("", Session("conn"))

        MyCommand.Parameters.Clear()
        MyCommand.Parameters.AddWithValue("@IdAllegato", A.Id)
        MyCommand.Parameters.AddWithValue("@IdEnteFase", IdEnteFase)
        Dim searchSQL As String = "select identedocumento from entidocumenti WHERE IdEnteDocumento=@IdAllegato and IdEnteFase=@IdEnteFase"
        MyCommand.CommandText = searchSQL
        Dim res = MyCommand.ExecuteScalar()
        If res IsNot Nothing AndAlso Integer.TryParse(res.ToString, Nothing) Then
            _ret = True
        End If

        MyCommand.Dispose()
        Return _ret
    End Function

    Function SalvaAllegato(A As Allegato, IdTipoAllegato As Integer, IdEnteFase As Integer) As Integer
        Dim MyCommand As New SqlCommand("", Session("conn"))
        Dim _ret As Integer = SalvaAllegato(A, IdTipoAllegato, IdEnteFase, MyCommand)
        MyCommand.Dispose()
        Return _ret
    End Function

    Function SalvaAllegato(A As Allegato, IdTipoAllegato As Integer, IdEnteFase As Integer, tran As SqlTransaction) As Integer
        Dim MyCommand As New SqlCommand("", Session("conn"))
        MyCommand.Transaction = tran
        Dim _ret As Integer = SalvaAllegato(A, IdTipoAllegato, IdEnteFase, MyCommand)
        MyCommand.Dispose()
        Return _ret
    End Function

    Sub AggiornaAllegato(A As Allegato, Optional ByVal IdEnteFase As Integer = 0)
        'L'allegato va sempre in insert per tenere lo storico
        Dim MyCommand As New SqlCommand("", Session("conn"))
        AggiornaAllegato(A, MyCommand, IdEnteFase)
        MyCommand.Dispose()
    End Sub

    Sub AggiornaAllegato(A As Allegato, ByVal MyCommand As SqlCommand, Optional ByVal IdEnteFase As Integer = 0)
        'L'allegato va sempre in insert per tenere lo storico
        MyCommand.Parameters.Clear()
        MyCommand.Parameters.AddWithValue("@BinData", A.Blob)
        MyCommand.Parameters.AddWithValue("@FileName", A.Filename)
        MyCommand.Parameters.AddWithValue("@HashValue", A.Hash)
        MyCommand.Parameters.AddWithValue("@IdAllegato", A.Id)
        MyCommand.Parameters.AddWithValue("@UsernameInserimento", If(Session("CodiceFiscaleUtente"), Session("Account")))
        MyCommand.Parameters.AddWithValue("@IdEnteFase", IdEnteFase)
        Dim updateSql As String = "UPDATE entidocumenti SET BinData=@BinData, FileName=@FileName, HashValue=@HashValue, "
        updateSql &= "DataInserimento=GETDATE(), UsernameInserimento=@UsernameInserimento "
        updateSql &= "WHERE IdEnteDocumento=@IdAllegato and IdEnteFase=@IdEnteFase"
        MyCommand.CommandText = updateSql
        MyCommand.ExecuteNonQuery()
    End Sub
    Function AllegatoModificato(ByVal A As Allegato, ByVal OldId As Integer) As Boolean
        If IsNothing(A) Then Return False 'nessun allegato
        If IsNothing(A.Id) Then Return True 'Id non assegnato ancora, e' certamente nuovo
        Return (A.Id <> OldId)

    End Function


End Class
