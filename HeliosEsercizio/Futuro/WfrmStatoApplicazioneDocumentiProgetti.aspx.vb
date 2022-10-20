Public Class WfrmStatoApplicazioneDocumentiProgetti
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        StatoDocumenti(Request.QueryString("idattivita"))
    End Sub
    Private Function StatoDocumenti(ByVal idattivita As Integer) As Integer
        Dim strSql As String
        Dim dtrCount As SqlClient.SqlDataReader

        strSql = " SELECT   NElaborazioniMancanti FROM LockDocumentiEnte l " & _
                 " INNER JOIN Attività a on a.identepresentante = l.idente WHERE a.idattività = " & idattivita
        dtrCount = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrCount.HasRows Then
            dtrCount.Read()
            LblTotElab.Text = dtrCount("NElaborazioniMancanti")
            If Not dtrCount Is Nothing Then
                dtrCount.Close()
                dtrCount = Nothing
            End If
        Else

            If Not dtrCount Is Nothing Then
                dtrCount.Close()
                dtrCount = Nothing
            End If
            If Trim(Request.QueryString("VengoDa")) = "Istanza" Then
                '    'DA MANDARE A CHI MI HA CHIAMATO(forse qui)

                'dove cazzo deve andare??????? INS e MOD ??????
                If Request.QueryString("Verso") = "Ins" Then
                    Response.Redirect("WfrmIstanzaPresentazione.aspx?Verso=Ins&VediEnte=1")
                Else
                    Response.Redirect("wfrmIstanzaPresentazione.aspx?id=" & Request.QueryString("id") & "&DataFine=" & Request.QueryString("DataFine") & "&DataInizio=" & Request.QueryString("DataInizio") & "&Verso=" & Request.QueryString("Verso") & "&Stato=" & Request.QueryString("Stato") & "&Arrivo=" & Request.QueryString("Arrivo") & "&idBA=" & Request.QueryString("idBA"))
                End If

            Else

                Response.Redirect("wfrmDocumentiProgetto.aspx?Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&StatoApplica=SI&IdAttivita=" & idattivita & "")
            End If
        End If
        'If Trim(Request.QueryString("VengoDa")) = "Istanza" Then
        '    Response.Write("<script>")
        '    Response.Write("opener.location.href = opener.location;" & vbCrLf)
        '    'Response.Write("window.opener.location.href = ""WfrmIstanzaPresentazione.aspx""")
        '    Response.Write("</script>")
        'End If
    End Function

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        'DA MANDARE A CHI MI HA CHIAMATO
        If Request.QueryString("Verso") = "Mod" Then
            Response.Redirect("wfrmIstanzaPresentazione.aspx?id=" & Request.QueryString("id") & "&DataFine=" & Request.QueryString("DataFine") & "&DataInizio=" & Request.QueryString("DataInizio") & "&Verso=" & Request.QueryString("Verso") & "&Stato=" & Request.QueryString("Stato") & "&Arrivo=" & Request.QueryString("Arrivo") & "&idBA=" & Request.QueryString("idBA"))
        Else
            Response.Redirect("WfrmIstanzaPresentazione.aspx?Verso=Ins&VediEnte=1")
        End If

    End Sub
End Class