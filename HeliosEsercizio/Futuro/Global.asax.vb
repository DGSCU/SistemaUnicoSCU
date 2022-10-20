Imports System.Net.Mail
Imports System.Web
Imports System.Web.SessionState.HttpSessionState
Imports Logger.Data
Imports Logger.Logger
Imports Logger.Output


Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        'Configurazione Logger
        SetApplicationName("Helios")
        Dim logDB = New Logger.LogDB(ConfigurationManager.AppSettings("LogConnectionString"))

        Dim logFileDaysToExpire As Integer = 0
        Integer.TryParse(ConfigurationManager.AppSettings("LogConnectionString"), logFileDaysToExpire)

        Dim logFile = New LogFile(
            ConfigurationManager.AppSettings("LogFilePath"),
            ConfigurationManager.AppSettings("LogFileNameTemplate"),
            ConfigurationManager.AppSettings("LogFileTextTemplate"),
            logFileDaysToExpire)
        AddOutput(logDB)
        AddOutput(logFile)
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        'Response.ExpiresAbsolute = #1/1/1980#
        'Response.AddHeader("Pragma", "no-cache") ' Generato in caso di errore
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Generato all'inizio di ogni richiesta
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)

        ' Generato al tentativo di autenticare l'utilizzo

    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        'Dim Exception As Exception = Server.GetLastError().GetBaseException

        'in sviluppo non invia mail di errore 
        If HttpContext.Current.Request.ServerVariables("server_name") = "localhost" Then Return

        Dim strErr As String

        strErr = "<table align=""center"" width=""750"" style=""width: 750px"" cellSpacing=""1"" cellPadding=""1""><tr><td align=""750"">"
        If HttpContext.Current.Session("Sistema") = "Futuro" Then
            strErr = strErr & "<table align=""center"" cellSpacing=""1"" cellPadding=""1""><tr><td><img src=""http://sistemaunicoscn.serviziocivile.it/images/FUTURO-BLU.jpg"" border=""0""></td></tr></table>"
        Else
            strErr = strErr & "<table align=""center"" cellSpacing=""1"" cellPadding=""1""><tr><td><img src=""http://sistemaunicoscn.serviziocivile.it/images/HELIOS_BANNER.jpg"" border=""0""></td></tr></table>"
        End If
        strErr = strErr & "<table align=""center"" bgcolor=""#C9DFFF"" width=""750""><tr><td><b>Eccezione non gestita</b></td></tr>"
        strErr = strErr & "<tr><td><b>Sitema:</b> " & HttpContext.Current.Session("Sistema") & "</td></tr>"
        strErr = strErr & "<tr><td><b>Tipo Utente:</b> " & HttpContext.Current.Session("TipoUtente") & "</td></tr>"
        strErr = strErr & "<tr><td><b>Utente:</b> " & HttpContext.Current.Session("Utente") & "</td></tr>"
        strErr = strErr & "<tr><td><b>IP:</b> " & HttpContext.Current.Request.UserHostAddress & "</td></tr>"
        strErr = strErr & "<tr><td><b>Ente:</b> " & HttpContext.Current.Session("Denominazione") & "</td></tr>"
        strErr = strErr & "<tr><td><b>Codice Ente:</b> " & HttpContext.Current.Session("CodiceRegioneEnte") & "</td></tr>"
        strErr = strErr & "<tr><td><b>Errore in: </b><br>" + Request.Url.ToString() & "</td></tr>"
        strErr = strErr & "<tr><td width=""750""><b>Messaggio d'errore: </b><br>" & Server.GetLastError.InnerException.ToString & "</td></tr>"
        'strErr = strErr & "<tr><td><b>Stack Trace:</b><br>" & Exception.StackTrace.ToString() & "</td></tr>"
        strErr = strErr & "</table>"
        strErr = strErr & "</td></tr></table>"
        '''''strErr = "ERRORE IMPREVISTO"
        Try
            '' Istanzio l'eccezione Http e le assegno il valore dell'ultimo 
            '' errore verificatosi sul server grazie al metodo Server.GetLastError 
            Dim ex As HttpException
            ' quindi compongo il Body della mail che conterrà, grazie a 
            ' HttpException.GetHtmlErrorMessage, tutti i dati che mi interessano 
            ' sull'errore e la sua provenienza
            Dim ObjSMTP As System.Web.Mail.SmtpMail
            Dim myMail As New System.Web.Mail.MailMessage
            Dim strMessErr As String
            myMail.From = "sistemaunicoscn_error@serviziocivile.it"
            myMail.To = "d.spagnulo@logicainformatica.it"
            myMail.Cc = "a.dicroce@logicainformatica.it;c.ottaviani@logicainformatica.it;heliosweb@serviziocivile.it"
            myMail.Subject = "SISTEMA UNICO di SCN - " & Server.GetLastError.Message.ToString
            myMail.BodyFormat = System.Web.Mail.MailFormat.Html
            'strMessErr = Server.GetLastError.Message.ToString & vbCrLf & Server.GetLastError.Source.ToString & vbCrLf & Server.GetLastError.StackTrace.ToString
            'myMail.Body = strMessErr
            'strMessErr = Server.GetLastError.Message.ToString & vbCrLf & Server.GetLastError.Source.ToString & vbCrLf & Server.GetLastError.StackTrace.ToString
            myMail.Body = strErr
            Session("LastError") = Server.GetLastError


            Mail.SmtpMail.SmtpServer = "smtp.serviziocivile.it"
            'ObjSMTP.SmtpServer = "pop.serviziocivile.it"
            Mail.SmtpMail.Send(myMail)

            'Response.Write(strErr.ToString())
            Server.ClearError()

            Response.Redirect("page_error.aspx")
            'response.Redirect("page_error_readonly.aspx")
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try


    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Generato al termine della sessione
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Generato al termine dell'applicazione
    End Sub

End Class