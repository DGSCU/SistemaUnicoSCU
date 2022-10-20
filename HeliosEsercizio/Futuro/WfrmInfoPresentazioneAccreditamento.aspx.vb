Public Class WfrmInfoPresentazioneAccreditamento
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ControlloGenerazioneAllegat06_ElencoSedi(Request.QueryString("IdEnteFase"))
    End Sub
    Private Function ControlloGenerazioneAllegat06_ElencoSedi(ByVal IdEnteFase As Integer) As Integer
        Dim strSql As String
        Dim dtrCount As SqlClient.SqlDataReader
        Dim intNumEntiPres As Integer = 0
        Dim intNumCop As Integer = 0
        Dim tipofase As Integer
        ''strSql = "select count(*)as ProgettiPresentati from attività where idbandoattività= " & IDBA
        strSql = "Select tipofase from entifasi Where IdEnteFase = " & Request.QueryString("IDEnteFase")
        dtrCount = ClsServer.CreaDatareader(strSql, Session("conn"))
        dtrCount.Read()
        tipofase = dtrCount("tipofase")

        If Not dtrCount Is Nothing Then
            dtrCount.Close()
            dtrCount = Nothing
        End If
        If tipofase = 3 Then
            strSql = " SELECT count(DISTINCT E.IDEnte) as EntiPresentati from EntiFasi_Sedi  EFS"
            strSql &= " INNER JOIN entifasi EF ON EFS.IdEnteFASE= EF.IdEnteFASE"
            strSql &= " INNER JOIN entifasi EFART2 ON EF.IdEnteFASE= EFART2.IdEnteFASERiferimento"
            strSql &= " INNER JOIN entisediattuazioni ESA ON EFS.IdEnteSedeAttuazione= ESA.IDEnteSedeAttuazione"
            strSql &= " INNER JOIN entisedi ES ON ESA.IDEnteSede=ES.IDEnteSede"
            strSql &= " INNER JOIN enti E ON ES.IDEnte=E.IDEnte"
            strSql &= " WHERE EFART2.identefase = " & IdEnteFase & "  AND efs.Azione<>'Richiesta Cancellazione' and isnull(VariazioneArt2,0) = 1"

        Else
            strSql = " SELECT COUNT(DISTINCT Enti.IDEnte)as EntiPresentati "
            strSql &= " FROM EntiFasi "
            strSql &= " INNER JOIN EntiFasi_Sedi on EntiFasi.IdEnteFase =EntiFasi_Sedi.IdEnteFase "
            strSql &= " INNER JOIN Entisediattuazioni ON EntiFasi_Sedi.IdEnteSedeAttuazione=Entisediattuazioni.IDEnteSedeAttuazione "
            strSql &= " INNER JOIN Entisedi ON Entisediattuazioni.IDEnteSede=Entisedi.IDEnteSede "
            strSql &= " INNER JOIN Enti ON Entisedi.IDEnte=Enti.IDEnte "
            strSql &= " WHERE EntiFasi.IdEnteFase = " & IdEnteFase & " AND EntiFasi_Sedi.Azione<>'Richiesta Cancellazione' "
        End If
        

        dtrCount = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrCount.HasRows Then
            dtrCount.Read()
            intNumEntiPres = dtrCount("EntiPresentati")

            If Not dtrCount Is Nothing Then
                dtrCount.Close()
                dtrCount = Nothing
            End If

            strSql = " SELECT  COUNT(distinct identedocumento) as Tot" & _
                     " from EntiDocumenti " & _
                     " where left(FileName,7) ='BoxSedi' and IdEnteFase= " & IdEnteFase
            dtrCount = ClsServer.CreaDatareader(strSql, Session("conn"))
            If dtrCount.HasRows Then
                dtrCount.Read()

                intNumCop = dtrCount("Tot")
                If Not dtrCount Is Nothing Then
                    dtrCount.Close()
                    dtrCount = Nothing
                End If

            End If
        End If
        If Not dtrCount Is Nothing Then
            dtrCount.Close()
            dtrCount = Nothing
        End If

        LblTotPre.Text = intNumEntiPres
        LblTotElab.Text = CInt(intNumEntiPres - intNumCop)
        If CInt(LblTotElab.Text) = 0 Then
            strSql = " select e.codiceregione " & _
                     " from enti e inner join entifasi ef on e.idente = ef.idente" & _
                     " where ef.IdEnteFase= " & IdEnteFase
            dtrCount = ClsServer.CreaDatareader(strSql, Session("conn"))
            If dtrCount.HasRows Then
                dtrCount.Read()
                Dim CodcieRegione As String = "" & dtrCount("codiceregione")
                If Not dtrCount Is Nothing Then
                    dtrCount.Close()
                    dtrCount = Nothing
                End If
                'reinderizzo alla maschera dell'albero per la stampa delle copertine
                Response.Redirect("WfrmAlbero.aspx?IdEnteFase=" & IdEnteFase & "&tipologia=Enti&Presenta=1&CodiceRegione='" & CodcieRegione & "'")
            End If
        End If
        If Not dtrCount Is Nothing Then
            dtrCount.Close()
            dtrCount = Nothing
        End If
    End Function


    Protected Sub imgChiudi_Click(sender As Object, e As EventArgs) Handles imgChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

End Class