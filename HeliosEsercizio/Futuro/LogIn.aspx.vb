Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Newtonsoft.Json
Imports Logger.Data

Public Class Login
	Inherits SmartPage

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

		Session.Clear()
		Dim token As String = Request.QueryString("token")
		LoginToken(token)

	End Sub

	Private Class TokenInfo
		Public Property CodiceFiscaleEnte As String
		Public Property CodiceFiscale As String
		Public Property Username As String
		Public Property Albo As String
		Public Property Message As String
		Public Property Success As Boolean
	End Class

	Private Sub LoginToken(token As String)
		Dim json As String
		Dim tokenInfo As TokenInfo
		Try
			json = New Net.WebClient().DownloadString(AppSettings("UrlSistemaAccessoToken") & "/Accesso/CheckToken?token=" + token)
		Catch ex As Exception
			Log.Error(LogEvent.HELIOS_ERRORE_ACCESSO, "Verifica Token", ex)
			txtMessaggio.Text = "Riprovare ad effettuare l'accesso"
			lnkRitorna.HRef = AppSettings("UrlSistemaAccesso") & "/Accesso"
			Return
		End Try
		Try
			tokenInfo = JsonConvert.DeserializeObject(Of TokenInfo)(json)
		Catch ex As Exception
			Log.Error(LogEvent.HELIOS_ERRORE_ACCESSO, "Informazioni token non valide", ex)
			txtMessaggio.Text = "Riprovare ad effettuare l'accesso"
			lnkRitorna.HRef = AppSettings("UrlSistemaAccesso") & "/Accesso"
			Return
		End Try

		If (tokenInfo.Success) Then
			Session("conn") = New SqlConnection
			Session("conn").ConnectionString = "user id=" & AppSettings("DB_USERNAME") & ";password=" & AppSettings("DB_PASSWORD") & ";data source=" & AppSettings("DB_DATA_SOURCE") & ";persist security info=False;connect timeout=300;Max Pool Size=3000;initial catalog=" & AppSettings("DB_NAME") & ""

			Session("conn").open()
			Session("Read") = 0

			Dim connection As SqlConnection = Session("conn")
			Dim reader As SqlDataReader
			Try
				Dim command As New SqlCommand("SELECT Username, Nome FROM VW_REGISTRAZIONE_UTENZE WHERE CF_Persona=@CFPersona AND CF_Ente=@CFEnte AND Albo=@Albo AND Username=@Username", connection)
				command.Parameters.AddWithValue("@CFPersona", tokenInfo.CodiceFiscale)
				command.Parameters.AddWithValue("@CFEnte", tokenInfo.CodiceFiscaleEnte)
				command.Parameters.AddWithValue("@Albo", tokenInfo.Albo)
				command.Parameters.AddWithValue("@Username", tokenInfo.Username)
				reader = command.ExecuteReader()
			Catch ex As Exception
				Log.Error(LogEvent.HELIOS_ERRORE_ACCESSO, "Accesso al DB", ex)
				txtMessaggio.Text = "Riprovare ad effettuare l'accesso"
				lnkRitorna.HRef = AppSettings("UrlSistemaAccesso") & "/Accesso"
				Return
			End Try
			If reader.HasRows Then
				reader.Read()
				Dim codiceUtente = reader(0)
				Dim nomeUtente = reader("Nome")
				reader.Close()
				'ACCESSO
				codiceUtente = tokenInfo.Username
				Session("Utente") = codiceUtente
				Session("CodiceFiscaleUtente") = tokenInfo.CodiceFiscale
				Session("CodiceFiscaleEnte") = tokenInfo.CodiceFiscaleEnte
				Session("NomeUtente") = nomeUtente
				Session("TipoUtente") = "E"
				Session("LogIn") = True
				Session("Padre30") = True
				Session("Padre41") = True
				Session("Padre69") = True
				Session("Padre173") = True
				Session("Padre84") = True
				Session("Padre38") = True
				Session("Padre208") = True

				Session("Account") = codiceUtente

				ClsServer.RegistrazioneLogAccessi(codiceUtente, "LOGIN", IIf(Session("LogIn") = True, 1, 0), Session("conn"))
				Log.SetUsername(tokenInfo.CodiceFiscale)
				Log.SetEnte(tokenInfo.CodiceFiscaleEnte)
				Log.Information(LogEvent.HELIOS_ACCESSO)
				Response.Redirect("~/WfrmSistema.aspx")
			Else
				reader.Close()
				Log.SetUsername(tokenInfo.CodiceFiscale)
				Log.SetEnte(tokenInfo.CodiceFiscaleEnte)
				Log.Warning(LogEvent.HELIOS_ERRORE_ACCESSO, "UtenteInesistente", token)
				txtMessaggio.Text = "Utente Inesistente"
				lnkRitorna.HRef = AppSettings("UrlSistemaAccesso") & "/Accesso"
			End If




		Else
			Log.Warning(LogEvent.HELIOS_ERRORE_ACCESSO, "token Non valido", token)
			txtMessaggio.Text = "Riprovare ad effettuare l'accesso"
			lnkRitorna.HRef = AppSettings("UrlSistemaAccesso") & "/Accesso"
		End If
	End Sub

End Class