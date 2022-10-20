Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.IO


' Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Service1
    Inherits System.Web.Services.WebService
    '*************************************************************
    ' CREATO DA SIMONA CORDELLA IL 08/11/2010
    ' INTEGRAZIONE BARRA CAD
    '*************************************************************
    <WebMethod()> _
    Public Function GetDatiProtocollo(ByVal strUserName As String, ByVal intIdChiave As Integer, ByVal intIdModello As Integer) As String
        '', ByVal strProtocollo As String, ByVal strDataProtocollo As String

        Dim sqlConn As New SqlClient.SqlConnection
        Dim myCommand As New SqlClient.SqlCommand
        'Dim strSql As String
        'Dim dtGetProtocollo As SqlClient.SqlDataReader
        Dim ESITO As String = ""

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")
            sqlConn.Open()

            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            Dim CustOrderHist As SqlClient.SqlCommand
            CustOrderHist = New SqlClient.SqlCommand
            CustOrderHist.CommandType = CommandType.StoredProcedure
            CustOrderHist.CommandText = "SP_HELIOSCAD_VERIFICADATIPROTOCOLLO"
            CustOrderHist.Connection = sqlConn

            Dim sparam As SqlClient.SqlParameter
            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@USERNAME"
            sparam.SqlDbType = SqlDbType.VarChar
            CustOrderHist.Parameters.Add(sparam)

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@CHIAVEID"
            sparam1.SqlDbType = SqlDbType.Int
            CustOrderHist.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@IDMODELLO"
            sparam2.SqlDbType = SqlDbType.Int
            CustOrderHist.Parameters.Add(sparam2)

            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@ESITO"
            sparam3.SqlDbType = SqlDbType.VarChar
            sparam3.Size = 1000
            sparam3.Direction = ParameterDirection.Output
            CustOrderHist.Parameters.Add(sparam3)

            CustOrderHist.Parameters("@USERNAME").Value = strUserName
            CustOrderHist.Parameters("@CHIAVEID").Value = intIdChiave
            CustOrderHist.Parameters("@IDMODELLO").Value = intIdModello
            CustOrderHist.ExecuteScalar()

            ESITO = CustOrderHist.Parameters("@ESITO").Value
            Return ESITO
        Catch ex As Exception
            'Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaEnti")
            'myLog.GeneraFileLog()
        End Try

        'If intValore = 0 Then ' l'ente nn ha completato le operazioni necessarie
        '    LeggiStoreVerificaVerificaCompletamentoAccreditamento = CustOrderHist.Parameters("@Motivazione").Value
        'Else
        '    LeggiStoreVerificaVerificaCompletamentoAccreditamento = ""
        'End If
        '    If Not Reader Is Nothing Then
        '        Reader.Close()
        '        Rea()

        'Catch ex As Exception
        '    strElettorato = "0"

        '    Dim myLog As New GeneraLog(ex.Message, "ReadStatoelettorato")
        '    myLog.GeneraFileLog()
        'Finally
        '    'controllo e chiudo se aperto il datareader
        '    If Not dtrLocal Is Nothing Then
        '        dtrLocal.Close()
        '        dtrLocal = Nothing
        '    End If

        '    If sqlConn.State = ConnectionState.Open Then
        '        sqlConn.Close()
        '    End If
        'End Try

        'ReadStatoElettorato = strElettorato

        'Try

        '    sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")
        '    sqlConn.Open()

        '    'richiamo store per controllo protocolli

        '    Select Case intIdModello
        '        Case 78 'trasmissioneprogrammazione.rtf
        '        Case 79 'letterainterlocutoria.rtf
        '        Case 109 'letterainterlocutoriamultipla.rtf
        '        Case 80 'credenziali.rtf
        '            '** STATO VERIFICA = 5
        '            '** PROTOCOLLI DataProtInvioFax , NProtInvioFax
        '            strSql = "Select idstatoVerifica, DataProtInvioFax, NProtInvioFax from TVerifiche where idVerifica=" & intIdChiave
        '            'eseguo la query
        '            myCommand.Connection = sqlConn
        '            myCommand.CommandText = strSql
        '            dtGetProtocollo = myCommand.ExecuteReader
        '            If dtGetProtocollo.HasRows = True Then
        '                dtGetProtocollo.Read()
        '                'controllo se la verifica è APERTA (stato =5)
        '                If dtGetProtocollo("idStatoVerifica") = 5 Then
        '                    'controllo DataProtInvioFax , NProtInvioFax se non sono stati già indicati
        '                    'If IsDBNull(dtGetProtocollo("DataProtInvioFax")) = True And IsDBNull(dtGetProtocollo("NProtInvioFax")) = True Then
        '                    '    ESITO = "POSITIVO"
        '                    'Else
        '                    '    ESITO = "NEGATIVO"
        '                    'End If
        '                    ESITO = "POSITIVO"
        '                Else
        '                    ESITO = "NEGATIVO"
        '                End If
        '            End If
        '        Case 81 'letteradiincarico.rtf
        '        Case 102 'credenzialiIGF.rtf
        '        Case 104 'letteradiincaricoIGF.rtf
        '        Case 83 'conclusioneVerificaPositiva.rtf
        '        Case 84 'VerificasenzaIrregolaritàConRichiamo.rtf
        '        Case 82 'trasmisisonerelazionealDG.rtf
        '        Case 93 'trasmissionerelazioneconrichiamo.rtf
        '        Case 85 'LetteraContestazioneAddebiti.rtf
        '            'NO CODICE SEDE
        '        Case 87 'letteratrasmdgcontestazione.rtf
        '        Case 101 'ContestazioneChiusura.rtf
        '            'NO CODICE SEDE
        '        Case 111 'Trasmissione SanzioneDG.rtf
        '        Case 92 'ContestazioneChiusura.rtf
        '            'NO CODICE SEDE
        '        Case 94 'letteatrasmisprovaiservizi.rtf
        '    End Select

        '    sqlConn.Close()
        '    Return ESITO
        'Catch ex As Exception
        '    'Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaEnti")
        '    'myLog.GeneraFileLog()
        'End Try
    End Function

    <WebMethod()> _
    Public Function SetAggiornaProtocollo(ByVal strUserName As String, ByVal intIdChiave As Integer, ByVal intIdModello As Integer, ByVal strProtocollo As String, ByVal strDataProtocollo As String) As String

        Dim sqlConn As New SqlClient.SqlConnection
        Dim myCommand As New SqlClient.SqlCommand
        Dim ESITO As String = ""

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")
            sqlConn.Open()

            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            Dim CustOrderHist As SqlClient.SqlCommand
            CustOrderHist = New SqlClient.SqlCommand
            CustOrderHist.CommandType = CommandType.StoredProcedure
            CustOrderHist.CommandText = "SP_HELIOSCAD_AGGIORNADATIPROTOCOLLO"
            CustOrderHist.Connection = sqlConn

            Dim sparam As SqlClient.SqlParameter
            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@USERNAME"
            sparam.SqlDbType = SqlDbType.VarChar
            CustOrderHist.Parameters.Add(sparam)

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@CHIAVEID"
            sparam1.SqlDbType = SqlDbType.Int
            CustOrderHist.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@IDMODELLO"
            sparam2.SqlDbType = SqlDbType.Int
            CustOrderHist.Parameters.Add(sparam2)

            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@NUMPROTOCOLLO"
            sparam3.SqlDbType = SqlDbType.VarChar
            CustOrderHist.Parameters.Add(sparam3)

            Dim sparam4 As SqlClient.SqlParameter
            sparam4 = New SqlClient.SqlParameter
            sparam4.ParameterName = "@DATAPROTOCOLLO"
            sparam4.SqlDbType = SqlDbType.VarChar
            CustOrderHist.Parameters.Add(sparam4)

            Dim sparam5 As SqlClient.SqlParameter
            sparam5 = New SqlClient.SqlParameter
            sparam5.ParameterName = "@ESITO"
            sparam5.SqlDbType = SqlDbType.VarChar
            sparam5.Size = 1000
            sparam5.Direction = ParameterDirection.Output
            CustOrderHist.Parameters.Add(sparam5)

            CustOrderHist.Parameters("@USERNAME").Value = strUserName
            CustOrderHist.Parameters("@CHIAVEID").Value = intIdChiave
            CustOrderHist.Parameters("@IDMODELLO").Value = intIdModello
            CustOrderHist.Parameters("@NUMPROTOCOLLO").Value = strProtocollo
            CustOrderHist.Parameters("@DATAPROTOCOLLO").Value = strDataProtocollo
            CustOrderHist.ExecuteScalar()

            ESITO = CustOrderHist.Parameters("@ESITO").Value
            Return ESITO
        Catch ex As Exception
            'Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaEnti")
            'myLog.GeneraFileLog()
        End Try
    End Function
    <WebMethod()> _
    Public Function SetAggiornaProtocolloIncarico_Elenco(ByVal strUserName As String, ByVal strElencoId As String, ByVal strProtocollo As String, ByVal strDataProtocollo As String) As String

        Dim sqlConn As New SqlClient.SqlConnection
        Dim myCommand As New SqlClient.SqlCommand
        Dim ESITO As String = ""

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")
            sqlConn.Open()

            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            Dim CustOrderHist As SqlClient.SqlCommand
            CustOrderHist = New SqlClient.SqlCommand
            CustOrderHist.CommandType = CommandType.StoredProcedure
            CustOrderHist.CommandText = "SP_HELIOSCAD_AGGIORNAPROTOCOLLOINCARICO_ELENCO"
            CustOrderHist.Connection = sqlConn

            Dim sparam As SqlClient.SqlParameter
            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@USERNAME"
            sparam.SqlDbType = SqlDbType.VarChar
            CustOrderHist.Parameters.Add(sparam)

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@ELENCOID"
            sparam1.SqlDbType = SqlDbType.VarChar
            CustOrderHist.Parameters.Add(sparam1)

            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@NUMPROTOCOLLO"
            sparam3.SqlDbType = SqlDbType.VarChar
            CustOrderHist.Parameters.Add(sparam3)

            Dim sparam4 As SqlClient.SqlParameter
            sparam4 = New SqlClient.SqlParameter
            sparam4.ParameterName = "@DATAPROTOCOLLO"
            sparam4.SqlDbType = SqlDbType.VarChar
            CustOrderHist.Parameters.Add(sparam4)

            Dim sparam5 As SqlClient.SqlParameter
            sparam5 = New SqlClient.SqlParameter
            sparam5.ParameterName = "@ESITO"
            sparam5.SqlDbType = SqlDbType.VarChar
            sparam5.Size = 1000
            sparam5.Direction = ParameterDirection.Output
            CustOrderHist.Parameters.Add(sparam5)

            CustOrderHist.Parameters("@USERNAME").Value = strUserName
            CustOrderHist.Parameters("@ELENCOID").Value = strElencoId
            CustOrderHist.Parameters("@NUMPROTOCOLLO").Value = strProtocollo
            CustOrderHist.Parameters("@DATAPROTOCOLLO").Value = strDataProtocollo
            CustOrderHist.ExecuteNonQuery()

            ESITO = CustOrderHist.Parameters("@ESITO").Value
            Return ESITO
        Catch ex As Exception
            'Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaEnti")
            'myLog.GeneraFileLog()
        End Try
    End Function

    <WebMethod()> _
    Public Function GetDatiIndirizzo(ByVal strUserName As String, ByVal strSoggetto As String) As System.Xml.XmlDocument
        'intSogetto : 0 = SED , <>0 =IDENTE
        Dim sqlConn As New SqlClient.SqlConnection
        Dim myCommand As New SqlClient.SqlCommand
        Dim strSql As String

        Dim dtsLocal As New DataSet

        Dim dtT As New DataTable
        Dim dtR As DataRow
        Dim xmlLocal As String
        Dim xmlDOC As New System.Xml.XmlDocument
        Dim sTipoSoggetto() As String
        Dim i As Integer
        Try
            If strSoggetto = "0" Then
                ''TORNA SEDE
                'xmlLocal = "SEDE"
                'alla riga assegno la riga delle intestazioni appena creata
                dtT.Columns.Add(New DataColumn("Denominazione", GetType(String)))
                'alla riga assegno la riga delle intestazioni appena creata
                dtR = dtT.NewRow()

                'carico la prima riga della datatable
                dtR(0) = "SEDE"

                'aggiungo la riga appena caricata alla datatable
                dtT.Rows.Add(dtR)
                dtsLocal.Tables.Add(dtT)
                dtsLocal.DataSetName = "DatiIndirizzo"

                dtsLocal.Tables(0).TableName = "IndirizzoEnte"
                xmlLocal = dtsLocal.GetXml
                xmlDOC = DocumentXML(xmlLocal)
                'xmlDOC = xmlLocal
            Else

                sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")
                sqlConn.Open()

                myCommand = New SqlClient.SqlCommand
                myCommand.Connection = sqlConn

                sTipoSoggetto = Split(strSoggetto, "#")
                Dim tipo As String
                Dim intSogetto As Integer

                '1)parametri report*****************************
                For i = 0 To UBound(sTipoSoggetto)
                    tipo = Mid(sTipoSoggetto(i), 1, 2)
                    intSogetto = Mid(sTipoSoggetto(i), 3)
                    Select Case tipo

                        Case "EN" 'idente
                            'modificato il 05/04/2012 da s.c. inserito il campo email ed emailPec al posto delal regionecompetenza
                            strSql = "SELECT E.CodiceRegione AS CodiceEnte, E.Denominazione AS DenominazioneEnte, " & _
                                     "       E.Indirizzo AS IndirizzoEnte, " & _
                                     "       E.Civico as CivicoEnte,E.CAP as CapEnte, " & _
                                     "       E.Comune AS ComuneEnte, E.ProvinciaBreve AS ProvinciaEnte,e.Email,e.Pec " & _
                                     "FROM VW_HELIOSCAD_ENTI AS E  " & _
                                     "WHERE E.IDEnte = " & intSogetto & ""
                            ' e.Descrizione AS CompetenzaEnte
                            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

                            dtsLocal.DataSetName = "DatiIndirizzo" 'nome al dataset
                            CMD.Fill(dtsLocal)
                            dtsLocal.Tables(i).TableName = "IndirizzoEnte" 'nome alla tabella del dataset
                            xmlLocal = dtsLocal.GetXml 'trasmormo il dataset in xml
                            sqlConn.Close()
                            xmlDOC = DocumentXML(xmlLocal)
                        Case "SA" 'identesedeattuazione
                            strSql = "SELECT Denominazione , " & _
                                     "       Indirizzo, " & _
                                     "       Civico ,CAP as CapEnte, " & _
                                     "       Comune , Provincia  " & _
                                     "FROM VW_HELIOSCAD_SEDI   " & _
                                     "WHERE IDEnteSedeAttuazione = " & intSogetto & ""

                            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

                            'dtsLocal.DataSetName = "DatiSede" 'nome al dataset
                            CMD.Fill(dtsLocal)
                            dtsLocal.Tables(i).TableName = "IndirizzoSede" 'nome alla tabella del dataset
                            xmlLocal = dtsLocal.GetXml 'trasmormo il dataset in xml
                            sqlConn.Close()
                            xmlDOC = DocumentXML(xmlLocal)
                        Case "VO" 'identità
                            strSql = "SELECT  Nominativo,  " & _
                                     "       IndirizzoRecapito, " & _
                                     "       CivicoRecapito ,CapRecapito, " & _
                                     "       ComuneRecapito , ProvinciaRecapitoBreve  " & _
                                     "FROM VW_HELIOSCAD_Volontari   " & _
                                     "WHERE identità = " & intSogetto & ""

                            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

                            'dtsLocal.DataSetName = "DatiSede" 'nome al dataset
                            CMD.Fill(dtsLocal)
                            dtsLocal.Tables(i).TableName = "IndirizzoVolontario" 'nome alla tabella del dataset
                            xmlLocal = dtsLocal.GetXml 'trasmormo il dataset in xml
                            sqlConn.Close()
                            xmlDOC = DocumentXML(xmlLocal)
                    End Select
                Next i
            End If
            GetDatiIndirizzo = xmlDOC
            Return GetDatiIndirizzo
        Catch ex As Exception
            'Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaEnti")
            'myLog.GeneraFileLog()
        End Try
    End Function

    Public Function DocumentXML(ByVal localXML As String) As System.Xml.XmlDocument
        Dim xmlDOC As New System.Xml.XmlDocument
        Try
            xmlDOC.LoadXml(localXML) 'trasformo da xlm ad un xml document
        Catch ex As Exception
            Throw ex
        End Try

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement 'punto inizio file xml
        xmlDOC.InsertBefore(xmldecl, root)
        Return xmlDOC
    End Function
End Class