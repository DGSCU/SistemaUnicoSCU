Public Class WfrmInfoPresentazioneIstanza
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ControlloGenerazioneCopertine(Request.QueryString("IdBandoAttivita"))
    End Sub
     Private Function ControlloGenerazioneCopertine(ByVal IDBA As Integer) As Integer
        Dim strSql As String
        Dim dtrCount As SqlClient.SqlDataReader
        Dim intNumProgPres As Integer = 0
        Dim intNumCop As Integer = 0
        strSql = "select count(*)as ProgettiPresentati from attività where idbandoattività= " & IDBA
        dtrCount = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrCount.HasRows Then
            dtrCount.Read()
            intNumProgPres = dtrCount("ProgettiPresentati")

            If Not dtrCount Is Nothing Then
                dtrCount.Close()
                dtrCount = Nothing
            End If





            strSql = " select  count(distinct a.idattività) as Tot" & _
                     " from attivitàdocumenti ad " & _
                     " inner join attività  a on  ad.idattività=a.idattività " & _
                     " where  ad.datainserimento>a.datapresentazioneProgetto and left(ad.FileName,5) In ('BOX17','BOX20','BOXIT') and idbandoattività= " & IDBA
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

        LblTotPre.Text = intNumProgPres
        LblTotElab.Text = CInt(intNumProgPres - intNumCop)
        If CInt(LblTotElab.Text) = 0 Then
            strSql = " select a.idbando,convert(varchar,a.datainiziovalidità,103) as datainizio," & _
                     " convert(varchar,a.datafinevalidità,103) as datafine" & _
                     " from bando a inner join bandiattività ba on a.idbando = ba.idbando " & _
                     " where  ba.idbandoattività= " & IDBA
            dtrCount = ClsServer.CreaDatareader(strSql, Session("conn"))
            If dtrCount.HasRows Then
                dtrCount.Read()
                Dim DataFine As String = "" & dtrCount("datafine")
                Dim DataInizio As String = "" & dtrCount("datainizio")
                Dim id As Integer = "" & dtrCount("idbando")
                If Not dtrCount Is Nothing Then
                    dtrCount.Close()
                    dtrCount = Nothing
                End If
                If Request.QueryString("IdBP") Is Nothing Then
                    'reinderizzo alla maschera della presentazione istanza perla stampa delle copertine
                    Response.Redirect("WfrmIstanzaPresentazione.aspx?Stampa=SI&DataFine=" & DataFine & "&DataInizio=" & DataInizio & "&Verso=Mod&id=" & id & "&Stato=Presentata&Arrivo=WfrmRicIstanzadiPresentazione.aspx&idBA=" & IDBA & "")
                Else
                    Response.Redirect("WfrmProgrammiIstanza.aspx?Stampa=SI&Presenta=OK&idBP=" & Request.QueryString("idBP"))
                End If
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