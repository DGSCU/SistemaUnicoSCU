Public Class clsSelezionaComune
    '*** CLASSE CHE POPOLA I CAMPI RELATIVI ALLA PROVINCIA/NAZIONE E COMUNE
    Public Function CaricaProvinciaNazione(ByVal ObjCombo As DropDownList, ByVal Estero As Boolean, ByVal conn As SqlClient.SqlConnection) As DropDownList
        '** ESTERO: 0 --> ProvinciaNazionale (ITALIA)  --> 1
        '** ESTERO: 1 --> ProvinciaNazionale (ESTERO)  --> 0
        Dim strsql As String
        Dim ProvinceNazionali As String
        Dim myDateset As New DataSet

        ObjCombo.DataSource = Nothing
        If Estero = False Then
            ProvinceNazionali = "1" 'ITALIA
        Else
            ProvinceNazionali = "0" 'ESTERO
        End If
        strsql = "select '' as idprovincia, '' as provincia from provincie UNION "
        strsql &= "select idprovincia ,provincia from provincie "
        strsql &= " where provincenazionali= '" & ProvinceNazionali & "'"
        strsql &= " order by provincia "
        myDateset = ClsServer.DataSetGenerico(strsql, conn)
        ObjCombo.DataSource = myDateset
        ObjCombo.DataTextField = "provincia"
        ObjCombo.DataValueField = "idprovincia"
        ObjCombo.DataBind()

        Return ObjCombo
    End Function

    Public Function CaricaComuni(ByVal ObjCompoComune As DropDownList, ByVal ID As String, ByVal conn As SqlClient.SqlConnection) As DropDownList
        Dim strsql As String
        Dim myDateset As New DataSet

        ObjCompoComune.DataSource = Nothing
        strsql = " select '' as idcomune, '' as denominazione from comuni UNION "
        strsql &= " select idcomune,denominazione from comuni "
        strsql &= " where idprovincia = '" & ID & "'  and isnull(CodiceISTAT,'') <>''"
        strsql &= " order by denominazione "
        myDateset = ClsServer.DataSetGenerico(strsql, conn)

        ObjCompoComune.DataSource = myDateset
        ObjCompoComune.DataTextField = "denominazione"
        ObjCompoComune.DataValueField = "idcomune"
        ObjCompoComune.DataBind()

        Return ObjCompoComune
    End Function

	Public Function CaricaComuniNascita(ByVal ObjCompoComune As DropDownList, ByVal ID As String, ByVal conn As SqlClient.SqlConnection) As DropDownList
        'CREATA DA SIMONA CORDELLA IL 10/03/2017
        'FUNZIONE CHE RENDE VISIBILI I COMUNI DISMESSI SOLAMENTE PER IL CARICAMENTO DEI COMUNI DI NASCITA
        '*******
        Dim strsql As String
        Dim myDateset As New DataSet

        ObjCompoComune.DataSource = Nothing
        strsql = " select '' as idcomune, '' as denominazione from comuni "
        strsql &= " UNION "
        strsql &= " select idcomune,denominazione from comuni "
        strsql &= " where idprovincia = '" & ID & "'  and isnull(CodiceISTAT,'') <>''"
        strsql &= " UNION "
        strsql &= " select idcomune,denominazione from comuni "
        strsql &= " where idprovincia = '" & ID & "'  and isnull(CodiceIstatDismesso,'') <>''"
        strsql &= " order by denominazione "
        myDateset = ClsServer.DataSetGenerico(strsql, conn)

        ObjCompoComune.DataSource = myDateset
        ObjCompoComune.DataTextField = "denominazione"
        ObjCompoComune.DataValueField = "idcomune"
        ObjCompoComune.DataBind()

        Return ObjCompoComune
    End Function
    Public Function RitornaCap(ByVal strIdComune As String, ByVal strIndirizzo As String, ByVal strCivico As String, ByVal conn As SqlClient.SqlConnection) As String
        Dim strSql As String
        'datareader locale
        Dim dtrComuni As SqlClient.SqlDataReader
        'variabile contatore
        Dim intX As Integer
        ''variabile che conterrà il comune selezionato
        Dim strCAP As String = ""

        'setto la variabile contatore a 0
        intX = 0
        'controllo e chiudo il datareader se aperto
        If Not dtrComuni Is Nothing Then
            dtrComuni.Close()
            dtrComuni = Nothing
        End If

        'set di controlli sul tipo di ricerca
        strSql = "select DBO.CAP_RITORNACAP (" & strIdComune & ", '" & Replace(strIndirizzo, "'", "''") & "','" & Replace(strCivico, "'", "''") & "') as MIOCAP "
        'eseguo la query
        dtrComuni = ClsServer.CreaDatareader(strSql, conn)
        'controllo se ci sono dei record
        If dtrComuni.HasRows = True Then
            'carico le stringhe con i dati del datareader
            Do While dtrComuni.Read
                strCAP = dtrComuni.Item("MIOCAP")
                intX = intX + 1
            Loop

            'controllo e chiudo il datareader se aperto
            If Not dtrComuni Is Nothing Then
                dtrComuni.Close()
                dtrComuni = Nothing
            End If
        End If

        If Not dtrComuni Is Nothing Then
            dtrComuni.Close()
            dtrComuni = Nothing
        End If
        Return strCAP
    End Function

    'Public Sub CaricaRegioneNazione(ByRef dropDownList As DropDownList, ByVal estero As Boolean, ByVal conn As SqlClient.SqlConnection)
    '    Dim IdNazione As String
    '    Dim myDateset As New DataSet
    '    dropDownList.DataSource = Nothing
    '    Dim query As StringBuilder = New StringBuilder()
    '    query.Append("select '' as IdRegione, '' as Regione from Regioni ")
    '    query.Append("UNION")
    '    query.Append("select IdRegione, Regione from Regioni ")
    '    query.Append("where ")
    '    If Not (estero) Then
    '        query.Append("IdNazione = 1 ")
    '    Else
    '        query.Append("IdNazione <> 1 ")
    '    End If
    '    myDateset = ClsServer.DataSetGenerico(query.ToString(), conn)
    '    dropDownList.DataSource = myDateset
    '    dropDownList.DataTextField = "IdRegione"
    '    dropDownList.DataValueField = "IdRegione"
    '    dropDownList.DataBind()

    'End Sub

    Public Sub CaricaProvincie(ByRef ddlProvincie As DropDownList, ByVal Estero As Boolean, ByVal conn As SqlClient.SqlConnection)
        Dim strsql As String
        Dim provincieNazionali As String
        Dim myDateset As New DataSet

        'ddlProvincie.DataSource = Nothing
        If Estero = False Then
            provincieNazionali = "1" 'ITALIA
        Else
            provincieNazionali = "0" 'ESTERO
        End If
        strsql = "select 0 as idprovincia, '' as provincia from provincie UNION "
        strsql &= "select idprovincia ,provincia from provincie "
        strsql &= " where provincenazionali= " & provincieNazionali & ""
        strsql &= " order by provincia "
        myDateset = ClsServer.DataSetGenerico(strsql, conn)
        ddlProvincie.DataSource = myDateset
        ddlProvincie.DataTextField = "provincia"
        ddlProvincie.DataValueField = "idprovincia"
        ddlProvincie.DataBind()
    End Sub
    Public Sub CaricaComuniDaProvincia(ByRef ddlComuni As DropDownList, ByVal idProvincia As String, ByVal conn As SqlClient.SqlConnection)
        Dim strsql As String
        Dim myDateset As New DataSet

        ddlComuni.DataSource = Nothing
        strsql = " select '' as idcomune, '' as denominazione from comuni UNION "
        strsql &= " select idcomune,denominazione from comuni "
        strsql &= " where idprovincia = '" & idProvincia & "' and  isnull(CodiceISTAT,'') <>''"
        strsql &= " order by denominazione "
        myDateset = ClsServer.DataSetGenerico(strsql, conn)

        ddlComuni.DataSource = myDateset
        ddlComuni.DataTextField = "denominazione"
        ddlComuni.DataValueField = "idcomune"
        ddlComuni.DataBind()

    End Sub
End Class
