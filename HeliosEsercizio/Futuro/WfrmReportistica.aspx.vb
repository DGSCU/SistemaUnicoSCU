Imports System.IO
Imports System.Data.SqlClient
Public Class WfrmReportistica
    Inherits System.Web.UI.Page


    Dim dts As DataSet
    Dim strsql As String
    Public strEsito As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        Dim sDati As String
        Dim intTipoStampa As Integer
        Dim sEsito As String
        intTipoStampa = Request.Params("sTipoStampa")

        If intTipoStampa = 1 Then           'Stampa progetti chiusi
            sDati = "idBando," & Request.Params("idBando") & ":"
            strEsito = ClsServer.CreatePdf("crtProgChiusi.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If

        ElseIf intTipoStampa = 2 Then       'Stampa progetti approvati non finaziati
            sDati = "idBando," & Request.Params("idBando") & ":finanziato," & Request.Params("Finanziato") & ":"
            strEsito = ClsServer.CreatePdf("crpProgettiGraduatoriaF.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 3 Then       'Stampa progetti approvati finaziati

        ElseIf intTipoStampa = 10 Then 'stampa riepilogo generale
            sDati = "idEnte," & Request.Params("idente") & ":Ispezione," & Request.Params("ispezione") & ":"
            sEsito = ClsServer.CreatePdf("crpIspezioneGenerale.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(sEsito)
            Else
                sEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 11 Then 'dettaglio ente
            sDati = "Ispezione," & Request.Params("ispezione") & ":"
            sEsito = ClsServer.CreatePdf("crpIspezioneEnte.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(sEsito)
            Else
                sEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 12 Then 'dettaglio sedi
            sDati = "Ispezione," & Request.Params("ispezione") & ":"
            sEsito = ClsServer.CreatePdf("crpIspezioneSede.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(sEsito)
            Else
                sEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 13 Then 'dettaglio risorse
            sDati = "Ispezione," & Request.Params("ispezione") & ":"
            sEsito = ClsServer.CreatePdf("crpIspezioneRuolo.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(sEsito)
            Else
                sEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 14 Then 'dettaglio risorse
            sDati = "Ispezione," & Request.Params("ispezione") & ":"
            sEsito = ClsServer.CreatePdf("crpIspezioneAttività.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(sEsito)
            Else
                sEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 15 Then 'dettaglio risorse         
            sDati = "Ispezione," & Request.Params("ispezione") & ":"
            sEsito = ClsServer.CreatePdf("crpIspezioneAttivitàEntità.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(sEsito)
            Else
                sEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 16 Then       'Stampa Copertina Presentazione
            sDati = "IdBandoAttivita," & Request.Params("IdBandoAttivita") & ":"
            If Session("Sistema") = "Helios" Then
                strEsito = ClsServer.CreatePdf("crpCopertinaPresentazioneNazSCN.rpt", sDati, Me.Session)
            Else
                strEsito = ClsServer.CreatePdf("crpCopertinaPresentazioneNazGG.rpt", sDati, Me.Session)
            End If
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 17 Then       'Stampa Scheda di Valutazione del Progetto
            sDati = "IdAttivita," & Request.Params("IdAttivita") & ":IdStorico," & Request.Params("IdStorico") & ":"
            strEsito = ClsServer.CreatePdf("crpValutQual.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 18 Then       'Stampa Scheda di Valutazione dei Progetti dell'Ente
            sDati = "IdEnte," & Request.Params("IdEnte") & ":IdBando," & Request.Params("IdBando") & ":"
            If Session("Sistema") = "Helios" Then
                strEsito = ClsServer.CreatePdf("crpValutQualTotSCN.rpt", sDati, Me.Session)
            Else
                strEsito = ClsServer.CreatePdf("crpValutQualTotGG.rpt", sDati, Me.Session)
            End If
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 19 Then       'Stampa Scheda Progetto
            sDati = "IdAttività," & Request.Params("IdAttivita") & ":TipoReport,1:"
            If Session("Sistema") = "Helios" Then
                strEsito = ClsServer.CreatePdf("crpProgettoSCN.rpt", sDati, Me.Session)
            Else
                strEsito = ClsServer.CreatePdf("crpProgettoGG.rpt", sDati, Me.Session)
            End If

            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If

        ElseIf intTipoStampa = 21 Then       'Stampa Formazione Volontari
            sDati = "IdCronologia," & Request.Params("IdCronologia") & ":"
            If Session("Sistema") = "Helios" Then
                strEsito = ClsServer.CreatePdf("crpFormazioneOreVolontari.rpt", sDati, Me.Session, "crpSottoFormazioneOreVolontari")
            Else
                'ADC 24/08/2021
                strEsito = ClsServer.CreatePdf("crpFormazioneOreVolontariGG.rpt", sDati, Me.Session, "crpSottoFormazioneOreVolontari")
            End If

            'modificato da simona cordella il 16/04/2012 
            ' modifico NULL le informazioni relative alle coordinate bancarie
            ModificaDatiBanca(Request.Params("IdCronologia"))
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If

        ElseIf intTipoStampa = 22 Then       'Stampa Scheda di Valutazione  Formazione
            sDati = "IdBando," & Request.Params("IdBando") & ":IdStorico," & Request.Params("IdStorico") & ":"
            strEsito = ClsServer.CreatePdf("crpValutFormazione.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 23 Then       'Stampa Scheda di Valutazione del Progetto Nuovi >07
            sDati = "IdAttivita," & Request.Params("IdAttivita") & ":IdStorico," & Request.Params("IdStorico") & ":"
            If Session("TipoUtente") = "U" Then
                strEsito = ClsServer.CreatePdf("crpValutQualNuoviUNSC.rpt", sDati, Me.Session)
            Else
                strEsito = ClsServer.CreatePdf("crpValutQualNuovi.rpt", sDati, Me.Session)
            End If
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 24 Then       'Stampa Scheda di Valutazione del Progetto Nuovi >07
            sDati = "IdAttivita," & Request.Params("IdAttivita") & ":IdStorico," & Request.Params("IdStorico") & ":"
            If Session("TipoUtente") = "U" Then
                strEsito = ClsServer.CreatePdf("crpValutQualSenzaPuntUNSC.rpt", sDati, Me.Session)
            Else
                strEsito = ClsServer.CreatePdf("crpValutQualSenzaPunt.rpt", sDati, Me.Session)
            End If

            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
            ElseIf intTipoStampa = 25 Then       'Stampa attestato
                sDati = "IdVolontario," & Request.Params("IdVolontario") & ":"
                strEsito = ClsServer.CreatePdf("crpVolontariAttestato.rpt", sDati, Me.Session)
                If ClsServer.GetPdfError = "" Then
                    Response.Redirect(strEsito)
                Else
                    strEsito = ClsServer.GetPdfError
                End If
        ElseIf intTipoStampa = 26 Then       'Stampa lettera attestato
            sDati = "IdVolontario," & Request.Params("IdVolontario") & ":"
            strEsito = ClsServer.CreatePdf("crpVolontariLettera.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 27 Then       'Stampa attestato duplicato
            sDati = "IdVolontario," & Request.Params("IdVolontario") & ":"
            strEsito = ClsServer.CreatePdf("crpVolontariAttestatoDuplicato.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 28 Then       'Stampa lettera attestato duplicato
            sDati = "IdVolontario," & Request.Params("IdVolontario") & ":"
            strEsito = ClsServer.CreatePdf("crpVolontariLetteraDuplicato.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
            ElseIf intTipoStampa = 29 Then ' nuovo questionario
                sDati = "IdBando," & Request.Params("IdBando") & ":IdStorico," & Request.Params("IdStorico") & ":"
                strEsito = ClsServer.CreatePdf("crpBloccoQuestionario.rpt", sDati, Me.Session)
                If ClsServer.GetPdfError = "" Then
                    Response.Redirect(strEsito)
                Else
                    strEsito = ClsServer.GetPdfError
                End If
        ElseIf intTipoStampa = 30 Then       'Stampa  Progetto Verifiche
            sDati = "IdVerifica," & Request.Params("IdVerifica") & ":TipoReport,1:"
            'strEsito = ClsServer.CreatePdf("crpProgettoVerifiche.rpt", sDati, Me.Session)
            If Session("Sistema") = "Helios" Then
                strEsito = ClsServer.CreatePdf("crpProgettoVerifiche.rpt", sDati, Me.Session)
            Else
                strEsito = ClsServer.CreatePdf("crpProgettoVerificheGG.rpt", sDati, Me.Session)
            End If
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 31 Then       'Stampa  Elenco SAP ITALIA
            sDati = "IdAttivita," & Request.Params("IdAttivita") & ":"
            ':TipoReport,1:"
            strEsito = ClsServer.CreatePdf("crpElencoSAPItalia.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 32 Then       'Stampa  Elenco SAP ITALIA per IDBANDOATTIVITà
            Try
                Dim strNomeFile As String = Day(Session("dataserver")) & "_" & Month(Session("dataserver")) & "_" & Year(Session("dataserver")) & "_" & Now.Hour.ToString & "_" & Now.Minute.ToString & "_" & Now.Second.ToString & "_" & Session("Utente") & ".pdf"
                Dim strPercorsoFile As String = Server.MapPath("BOX/" & strNomeFile)
                Dim localWS As New WS_Editor.WSMetodiDocumentazione

                localWS.Url = ConfigurationSettings.AppSettings("URL_WS_Documentazione")

                localWS.Timeout = 1000000

                DecodeFile(localWS.getBox(Request.Params("IdBandoAttivita"), Session("utente"), "crpElencoSAPItaliaBando.rpt"), strPercorsoFile)
                Response.Redirect("BOX/" & strNomeFile)
            Catch ex As Exception
                Response.Write(ex.Message)
            End Try
        ElseIf intTipoStampa = 33 Then       'Stampa Scheda di Valutazione del Progetto 2010
            sDati = "IdAttivita," & Request.Params("IdAttivita") & ":IdStorico," & Request.Params("IdStorico") & ":"
            If Session("Sistema") = "Helios" Then
                If Session("TipoUtente") = "U" Then
                    strEsito = ClsServer.CreatePdf("crpValutQualNuoviUNSC2010SCN.rpt", sDati, Me.Session)
                Else
                    'strEsito = ClsServer.CreatePdf("crpValutQualNuovi2010SCN.rpt", sDati, Me.Session) 'mod 21/02/2019 da Danilo per visualizzazione deflettori e punteggio COE
                    strEsito = ClsServer.CreatePdf("crpValutQualNuoviUNSC2010SCN.rpt", sDati, Me.Session)
                End If
            Else
                If Session("TipoUtente") = "U" Then
                    strEsito = ClsServer.CreatePdf("crpValutQualNuoviUNSC2010GG.rpt", sDati, Me.Session)
                Else
                    strEsito = ClsServer.CreatePdf("crpValutQualNuovi2010GG.rpt", sDati, Me.Session)
                End If
            End If

            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 34 Then       'Stampa Scheda di Valutazione del Progetto 2010
            sDati = "IdAttivita," & Request.Params("IdAttivita") & ":IdStorico," & Request.Params("IdStorico") & ":"
            If Session("Sistema") = "Helios" Then
                If Session("TipoUtente") = "U" Then
                    strEsito = ClsServer.CreatePdf("crpValutQualSenzaPuntUNSC2010SCN.rpt", sDati, Me.Session)
                Else
                    strEsito = ClsServer.CreatePdf("crpValutQualSenzaPunt2010SCN.rpt", sDati, Me.Session)
                End If
            Else
                If Session("TipoUtente") = "U" Then
                    strEsito = ClsServer.CreatePdf("crpValutQualSenzaPuntUNSC2010GG.rpt", sDati, Me.Session)
                Else
                    strEsito = ClsServer.CreatePdf("crpValutQualSenzaPunt2010GG.rpt", sDati, Me.Session)
                End If
            End If

            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 35 Then       'Stampa  Elenco SAP ESTERO per IDBANDOATTIVITà
            Try
                Dim strNomeFile As String = Day(Session("dataserver")) & "_" & Month(Session("dataserver")) & "_" & Year(Session("dataserver")) & "_" & Now.Hour.ToString & "_" & Now.Minute.ToString & "_" & Now.Second.ToString & "_" & Session("Utente") & ".pdf"
                Dim strPercorsoFile As String = Server.MapPath("BOX/" & strNomeFile)
                Dim localWS As New WS_Editor.WSMetodiDocumentazione

                localWS.Url = ConfigurationSettings.AppSettings("URL_WS_Documentazione")

                localWS.Timeout = 1000000

                DecodeFile(localWS.getBox(Request.Params("IdBandoAttivita"), Session("utente"), "crpElencoSAPEsteroBando.rpt"), strPercorsoFile)
                Response.Redirect("BOX/" & strNomeFile)
            Catch ex As Exception
                Response.Write(ex.Message)
            End Try
        ElseIf intTipoStampa = 36 Then       'Stampa Copertina Presentazione in Bozza
            sDati = "IdBandoAttivita," & Request.Params("IdBandoAttivita") & ":"
            If Session("Sistema") = "Helios" Then
                strEsito = ClsServer.CreatePdf("crpCopertinaPresentazioneBozzaSCN.rpt", sDati, Me.Session)
            Else
                strEsito = ClsServer.CreatePdf("crpCopertinaPresentazioneBozzaGG.rpt", sDati, Me.Session)
            End If
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 37 Then       'Stampa Copertina Presentazione Progetti Regionale
            sDati = "IdBandoAttivita," & Request.Params("IdBandoAttivita") & ":"
            strEsito = ClsServer.CreatePdf("crpCopertinaPresentazione1.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 38 Then       'Stampa Copertina Presentazione Progetti Regionale
            sDati = "IdAttivita," & Request.Params("IdAttivita") & ":"
            strEsito = ClsServer.CreatePdf("crpElencoDocumentiProgetto.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 39 Then       'Stampa Copertina Presentazione Accreditamento\Adeguamento
            sDati = "IDEnteFase," & Request.Params("IDEnteFase") & ":"
            'crptcrpCopertinaAccreditamentoAdeguamento.rpt()
            strEsito = ClsServer.CreatePdf("crpCopertinaAccreditamentoAdeguamento.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 40 Then       'Stampa Copertina Presentazione Accreditamento\Adeguamento senza documenti
            sDati = "IDEnteFase," & Request.Params("IDEnteFase") & ":"
            'crptcrpCopertinaAccreditamentoAdeguamento.rpt()
            strEsito = ClsServer.CreatePdf("crpCopertinaAccreditamentoAdeguamentoNODoc.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 41 Then       'Stampa Formazione Volontari a TRANCHE
            sDati = "IdCronologia," & Request.Params("IdCronologia") & ":"
            If Session("Sistema") = "Helios" Then
                strEsito = ClsServer.CreatePdf("crpFormazioneOreVolontariTranche.rpt", sDati, Me.Session, "crpSottoFormazioneOreVolontari")
            Else
                'ADC 24/08/2021
                strEsito = ClsServer.CreatePdf("crpFormazioneOreVolontariTrancheGG.rpt", sDati, Me.Session, "crpSottoFormazioneOreVolontari")
            End If

            ModificaDatiBanca(Request.Params("IdCronologia"))
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If

        ElseIf intTipoStampa = 42 Then       'Stampa CHECK LISTA Paga Mensile Volontario
            'aggiunto il 22/04/2015
            sDati = "@IdCheckList," & Request.Params("IdCheckList") & ":"

            strEsito = ClsServer.CreatePdf("crpCKLMeseVolontario.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 43 Then       '' nuovo questionario Rev2
            sDati = "IdBando," & Request.Params("IdBando") & ":IdStorico," & Request.Params("IdStorico") & ":"
            strEsito = ClsServer.CreatePdf("crpQuestionarioR2.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 44 Then       'Stampa CHECK LISTA Paga Mensile collettivo
            'aggiunto il 05/06/2015
            sDati = "@IdCheckList," & Request.Params("IdCheckList") & ":"

            strEsito = ClsServer.CreatePdf("crpCKLMeseVolontario_Collettivo.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 45 Then       'Stampa CHECK LISTA Paga Mensile Volontario rimborso
            'aggiunto il 05/06/2015
            sDati = "@IdCheckList," & Request.Params("IdCheckList") & ":"

            strEsito = ClsServer.CreatePdf("crpCKLMeseVolontario_Rimborsi.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 46 Then       'Stampa CHECK LISTA Volontario FORMAZIONE
            'aggiunto il 05/06/2015
            sDati = "@IdCheckList," & Request.Params("IdCheckList") & ":"

            strEsito = ClsServer.CreatePdf("crpCKLVolontario_Formazione.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 47 Then       'Stampa attestato GG
            sDati = "IdVolontario," & Request.Params("IdVolontario") & ":"
            strEsito = ClsServer.CreatePdf("crpVolontariAttestatoGG.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If '
        ElseIf intTipoStampa = 48 Then       'Stampa attestato GG duplicato
            sDati = "IdVolontario," & Request.Params("IdVolontario") & ":"
            strEsito = ClsServer.CreatePdf("crpVolontariAttestatoDuplicatoGG.rpt", sDati, Me.Session)
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If


        ElseIf intTipoStampa = 70 Then 'multipla

            'Antonello
            sDati = "IdCorso," & Request.Params("IdCorso") & ":"
            strEsito = ClsServer.CreatePdf("crpOLPAttestato.rpt", sDati, Me.Session)

            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If

        ElseIf intTipoStampa = 71 Then 'singola

            'Antonello
            sDati = "IdCorsoDett," & Request.Params("IdCorso") & ":"
            strEsito = ClsServer.CreatePdf("crpOLPAttestatoDett.rpt", sDati, Me.Session)

            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If


        ElseIf intTipoStampa = 72 Then 'Programmi copertina presentazione
            sDati = "@IdBandoProgramma," & Request.Params("IdBP") & ":"
            strEsito = ClsServer.CreatePdf("crpCopertinaIstanzaProgrammi.rpt", sDati, Me.Session)

            SalvaCopertina(strEsito, Request.Params("IdBP"))

            'If Session("Sistema") = "Helios" Then
            '    strEsito = ClsServer.CreatePdf("crpCopertinaIstanzaProgrammi.rpt", sDati, Me.Session)
            'Else
            '    strEsito = ClsServer.CreatePdf("crpCopertinaIstanzaProgrammiGG.rpt", sDati, Me.Session)
            'End If
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        ElseIf intTipoStampa = 73 Then 'Programmi copertina presentazione BOZZA
            sDati = "@IdBandoProgramma," & Request.Params("IdBP") & ":"
            strEsito = ClsServer.CreatePdf("crpCopertinaIstanzaProgrammiBozza.rpt", sDati, Me.Session)

            'If Session("Sistema") = "Helios" Then
            '    strEsito = ClsServer.CreatePdf("crpCopertinaIstanzaProgrammiBozza.rpt", sDati, Me.Session)
            'Else
            '    strEsito = ClsServer.CreatePdf("crpCopertinaIstanzaProgrammiBozzaGG.rpt", sDati, Me.Session)
            'End If
            If ClsServer.GetPdfError = "" Then
                Response.Redirect(strEsito)
            Else
                strEsito = ClsServer.GetPdfError
            End If
        End If
    End Sub

    Function DecodeToByte(ByVal enc As String) As Byte()
        Dim bt() As Byte
        bt = System.Convert.FromBase64String(enc)
        Return bt
    End Function

    Sub SalvaCopertina(file As String, idBP As Integer)
        Try
            Dim idente As Integer
            Dim idbando As Integer

            Dim dtrstrsql As System.Data.SqlClient.SqlDataReader
            Dim strsql As String
            strsql = "select idente, idbando from bandiprogrammi where idbandoprogramma=" & idBP
            dtrstrsql = ClsServer.CreaDatareader(strsql, Session("conn"))
            dtrstrsql.Read()
            If dtrstrsql.HasRows = True Then
                idente = dtrstrsql("idente")
                idbando = dtrstrsql("idbando")
            End If
            dtrstrsql.Close()
            dtrstrsql = Nothing


            Dim fs As New FileStream _
                     (Server.MapPath("") & "\" & file.Replace("/", "\"), FileMode.Open, FileAccess.Read)
            Dim iLen As Integer = CInt(fs.Length - 1)
            'Dim iLen As Integer = CInt(fs.Length)
            Dim bBLOBStorage(iLen) As Byte


            Dim numBytesToRead As Integer = CType(fs.Length, Integer)
            Dim numBytesRead As Integer = 0

            While (numBytesToRead > 0)
                ' Read may return anything from 0 to numBytesToRead.
                Dim n As Integer = fs.Read(bBLOBStorage, numBytesRead, _
                    numBytesToRead)
                ' Break when the end of the file is reached.
                If (n = 0) Then
                    Exit While
                End If
                numBytesRead = (numBytesRead + n)
                numBytesToRead = (numBytesToRead - n)

            End While
            numBytesToRead = bBLOBStorage.Length

            fs.Close()


            'sub che consente l'inserimento dei documenti da associare al progetto
            Dim cmd As SqlCommand = New SqlCommand _
            ("INSERT INTO BandiProgrammiCopertine (IdBandoProgramma,IdEnte,IdBando,DataCopertina,UsernameCopertina, BinData) " _
                & "VALUES(@IdBandoProgramma,@IdEnte, @IdBando, getdate(),@UsernameCopertina, @blob_data  )", Session("conn"))
            cmd.CommandType = CommandType.Text

            'dichiaro parametri
            cmd.Parameters.Add("@IdBandoProgramma", SqlDbType.Int)
            cmd.Parameters("@IdBandoProgramma").Direction = ParameterDirection.Input

            cmd.Parameters.Add("@IdEnte", SqlDbType.Int)
            cmd.Parameters("@IdEnte").Direction = ParameterDirection.Input

            cmd.Parameters.Add("@IdBando", SqlDbType.Int)
            cmd.Parameters("@IdBando").Direction = ParameterDirection.Input

            cmd.Parameters.Add("@UsernameCopertina", SqlDbType.VarChar)
            cmd.Parameters("@UsernameCopertina").Size = 255
            cmd.Parameters("@UsernameCopertina").Direction = ParameterDirection.Input

            cmd.Parameters.Add("@blob_data", SqlDbType.Image) 'varbinary???
            cmd.Parameters("@blob_data").Direction = ParameterDirection.Input

            'assegno valori ai parametri
            cmd.Parameters("@IdBandoProgramma").Value = idBP
            cmd.Parameters("@IdEnte").Value = idente
            cmd.Parameters("@IdBando").Value = idbando
            cmd.Parameters("@UsernameCopertina").Value = Session("Utente")
            cmd.Parameters("@blob_data").Value = bBLOBStorage
            cmd.ExecuteNonQuery()



        Catch ex As Exception

        End Try
    End Sub
    Sub DecodeFile(ByVal srcFile As String, ByVal destFile As String)
        'Dim src As String
        'Dim sr As New IO.StreamReader(srcFile)
        'src = sr.ReadToEnd
        'sr.Close()
        Dim bt64 As Byte() = DecodeToByte(srcFile)
        If IO.File.Exists(destFile) Then
            IO.File.Delete(destFile)
        End If

        Dim sw As New IO.FileStream(destFile, IO.FileMode.CreateNew)
        sw.Write(bt64, 0, bt64.Length)
        sw.Close()
    End Sub
    Sub ModificaDatiBanca(ByVal IdCrono As String)
        Dim strnull As String = "NULL"
        strsql = " Update QuestionarioCronologiaStampe " & _
                 " SET Iban = " & strnull & " ," & _
                 " BicSwift = " & strnull & " ," & _
                 " IdProvincia = " & strnull & " ," & _
                 " NumeroConto = " & strnull & " " & _
                 " WHERE IdCronologiaStampa = '" & IdCrono & "'"
        Dim myCommand As New SqlClient.SqlCommand
        myCommand = New SqlClient.SqlCommand(strsql, Session("conn"))
        myCommand.ExecuteNonQuery()
    End Sub


End Class