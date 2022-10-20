Imports System.Data.SqlClient
Imports System.Drawing

Public Class WfrmGestFlagEnti
    Inherits System.Web.UI.Page
    Dim myQuery As String
    Dim myDataReader As Data.SqlClient.SqlDataReader

#Region "Utility"
    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub

    Private Sub VerificaSessione()
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
    End Sub
    Private Sub CancellaMessaggiInfo()
        msgErrore.Text = String.Empty
        msgConferma.Text = String.Empty
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        Dim stridvin As String
        If Page.IsPostBack = False Then
            If Request.QueryString("tipologia") = "Enti" Then 'se gestisco enti
                '***********gestione sola lettura per stato di non presentata
                If Not Request.QueryString("SolaLettura") Is Nothing Then
                    rdbno.Enabled = False
                    rdbsi.Enabled = False
                    TxtProt.ReadOnly = True
                    txtData.ReadOnly = True
                    TxtProt.ReadOnly = True
                    TxtProt.BackColor = Color.Gainsboro
                    txtData.BackColor = Color.Gainsboro
                    TxtNotaStorico.BackColor = Color.Gainsboro
                    imgConferma.Visible = False
                    imgCancella.Visible = False
                End If
                myQuery = "select idvincolo, idtipovincolo, Vincolo from vincoli where idvincolo='" & ClsServer.NoApice(Request.QueryString("Vincolo")) & "'"
                myDataReader = ClsServer.CreaDatareader(myQuery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) 'verifico se il vincolo può essere gestito
                If myDataReader.HasRows = True Then
                    myDataReader.Read()
                    If myDataReader.GetValue(1) = "1" Then 'se il vincolo selezionato dalla Wform dell'albero è di tipo 1 lo gestisco
                        lblvincolo.Text = myDataReader.GetValue(2) 'Request.QueryString("Vincolo")
                        stridvin = myDataReader.GetValue(0)
                        ChiudiDataReader(myDataReader)
                        myQuery = "select idflagenti, valore, NotaStorico, NumeroProtocollo, DataProtocollazione from flagenti where idente='" & ClsServer.NoApice(Session("IdEnte")) & "' and idvincolo=" & ClsServer.NoApice(stridvin) & ""
                        myDataReader = ClsServer.CreaDatareader(myQuery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) 'verifico se il vincolo è stato già gestito
                        If myDataReader.HasRows = True Then
                            Do While myDataReader.Read() 'se è già gestito valorizzo RadioBotton
                                If myDataReader.GetValue(1) = "1" Then
                                    rdbsi.Checked = True
                                Else
                                    rdbno.Checked = True
                                End If
                                TxtNotaStorico.Text = IIf(IsDBNull(myDataReader.GetValue(2)) = True, "", myDataReader.GetValue(2))
                                TxtProt.Text = IIf(IsDBNull(myDataReader.GetValue(3)) = True, "", myDataReader.GetValue(3))
                                If IsDBNull(myDataReader.GetValue(4)) = False Then
                                    txtData.Text = Format(myDataReader.GetValue(4), "d")
                                End If
                            Loop
                           ChiudiDataReader(myDataReader)
                        Else 'se non è stato ancora gestico rendo invisibile il tasto elimina
                            imgCancella.Visible = False
                            ChiudiDataReader(myDataReader)
                        End If
                    Else 'se tipo vincolo ediverso da "1" chiudo popup e ritorno alla Wformalbero
                       ChiudiDataReader(myDataReader)

                    End If
                Else 'se il vincolo selezionato non esiste più chiudo la popup
                   ChiudiDataReader(myDataReader)
                End If
            ElseIf Request.QueryString("tipologia") = "Ruoli" Then 'verifico per ruoli
                myQuery = "select idvincolo, idtipovincolo, Vincolo from vincoli where idvincolo='" & ClsServer.NoApice(Request.QueryString("Vincolo")) & "'"
                myDataReader = ClsServer.CreaDatareader(myQuery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) 'verifico se il vincolo può essere gestito
                If myDataReader.HasRows = True Then
                    myDataReader.Read()
                    If myDataReader.GetValue(1) = "1" Then 'se il vincolo selezionato dalla Wform dell'albero è di tipo 1 lo gestisco
                        lblvincolo.Text = myDataReader.GetValue(2) 'Request.QueryString("Vincolo")
                        stridvin = myDataReader.GetValue(0)
                        ChiudiDataReader(myDataReader)
                        myQuery = "select idFlagEntePersonale, valore, NotaStorico, NumeroProtocollo, DataProtocollazione from FlagEntePersonale where idEntePersonale=" & ClsServer.NoApice(Request.QueryString("IDEntePersonale")) & " and idvincolo=" & ClsServer.NoApice(stridvin) & ""
                        myDataReader = ClsServer.CreaDatareader(myQuery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) 'verifico se il vincolo è stato già gestito
                        If myDataReader.HasRows = True Then
                            Do While myDataReader.Read() 'se è già gestito valorizzo RadioBotton
                                If myDataReader.GetValue(1) = "1" Then
                                    rdbsi.Checked = True
                                Else
                                    rdbno.Checked = True
                                End If
                                TxtNotaStorico.Text = IIf(IsDBNull(myDataReader.GetValue(2)) = True, "", myDataReader.GetValue(2))
                                TxtProt.Text = IIf(IsDBNull(myDataReader.GetValue(3)) = True, "", myDataReader.GetValue(3))
                                If IsDBNull(myDataReader.GetValue(4)) = False Then
                                    txtData.Text = Format(myDataReader.GetValue(4), "d")
                                End If
                            Loop
                            ChiudiDataReader(myDataReader)
                        Else 'se non è stato ancora gestico rendo invisibile il tasto elimina
                            imgCancella.Visible = False
                            ChiudiDataReader(myDataReader)
                        End If
                    Else 'se tipo vincolo ediverso da "1" chiudo popup e ritorno alla Wformalbero
                       ChiudiDataReader(myDataReader)
                    End If
                Else 'se il vincolo selezionato non esiste più chiudo la popup
                    ChiudiDataReader(myDataReader)
                End If
            ElseIf Request.QueryString("tipologia") = "Progetti" Then 'verifico per progetti

                myQuery = "select idvincolo, idtipovincolo, Vincolo from vincoli where idvincolo='" & ClsServer.NoApice(Request.QueryString("Vincolo")) & "'"
                myDataReader = ClsServer.CreaDatareader(myQuery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) 'verifico se il vincolo può essere gestito
                If myDataReader.HasRows = True Then
                    myDataReader.Read()
                    If myDataReader.GetValue(1) = "1" Then 'se il vincolo selezionato dalla Wform dell'albero è di tipo 1 lo gestisco
                        lblvincolo.Text = myDataReader.GetValue(2) 'Request.QueryString("Vincolo")
                        stridvin = myDataReader.GetValue(0)
                       ChiudiDataReader(myDataReader)
                        myQuery = "select idattività, valore, NotaStorico, NumeroProtocollo, DataProtocollazione from FlagAttività where idattività=" & ClsServer.NoApice(Request.QueryString("IDattivita")) & " and idvincolo=" & ClsServer.NoApice(stridvin) & ""
                        myDataReader = ClsServer.CreaDatareader(myQuery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) 'verifico se il vincolo è stato già gestito
                        If myDataReader.HasRows = True Then
                            Do While myDataReader.Read() 'se è già gestito valorizzo RadioBotton
                                If myDataReader.GetValue(1) = "1" Then
                                    rdbsi.Checked = True
                                Else
                                    rdbno.Checked = True
                                End If
                                TxtNotaStorico.Text = IIf(IsDBNull(myDataReader.GetValue(2)) = True, "", myDataReader.GetValue(2))
                                TxtProt.Text = IIf(IsDBNull(myDataReader.GetValue(3)) = True, "", myDataReader.GetValue(3))
                                If IsDBNull(myDataReader.GetValue(4)) = False Then
                                    txtData.Text = Format(myDataReader.GetValue(4), "d")
                                End If
                            Loop
                            ChiudiDataReader(myDataReader)
                        Else 'se non è stato ancora gestico rendo invisibile il tasto elimina
                            imgCancella.Visible = False
                           ChiudiDataReader(myDataReader)
                        End If
                    Else 'se tipo vincolo ediverso da "1" chiudo popup e ritorno alla Wformalbero
                       ChiudiDataReader(myDataReader)
                    End If
                Else 'se il vincolo selezionato non esiste più chiudo la popup
                  ChiudiDataReader(myDataReader)
                End If
            End If
        End If
    End Sub

    Private Sub imgChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgChiudi.Click
        Dim tipologia As String = Request.QueryString("tipologia")
        Response.Redirect("WfrmAlbero.aspx?tipologia=" & tipologia & "&IDEntePersonale=" & Request.QueryString("IDEntePersonale") & "&IDRuolo=" & Request.QueryString("IDRuolo"))
    End Sub

    Private Sub imgConferma_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgConferma.Click
        CancellaMessaggiInfo()
        '***Generata da Gianluigi Paesani in data:14/05/04
        '***Questa Routine Gestisce il flag per vincolo selezionato
        Dim cmdinsert As Data.SqlClient.SqlCommand
        Dim btygenerico As Byte 'byte generico
        Dim strid As String 'variabile appoggio idflagenti
        Dim stridvin As String 'variabile appoggio idvincolo
        ChiudiDataReader(myDataReader)


        If Request.QueryString("tipologia") = "Enti" Then 'verifico se ente
            If rdbno.Checked = True Or rdbsi.Checked = True Then 'verifico se è stato selezionato un flag

                If rdbno.Checked = True Then 'valorizzo variabile
                    btygenerico = 0
                Else
                    btygenerico = 1
                End If
                stridvin = Request.QueryString("Vincolo") 'passo idvincolo
                myQuery = "select idflagenti from flagenti where idente='" & Session("IdEnte") & "' and idvincolo=" & stridvin & ""
                myDataReader = ClsServer.CreaDatareader(myQuery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                If myDataReader.HasRows = True Then 'verifico se è già stato gestito
                    Do While myDataReader.Read
                        strid = myDataReader.GetValue(0) 'valorizzo con id se già gestito
                    Loop
                    myDataReader.Close()
                    myDataReader = Nothing
                    'eseguo modiFICA
                    Dim data As String = IIf(txtData.Text = "", "DataProtocollazione=null", "DataProtocollazione=convert(datetime,'" & txtData.Text & "',103)")
                    myQuery = "update flagenti set valore=" & btygenerico & "," & _
                    " datamodifica=getdate(), NotaStorico='" & ClsServer.NoApice(TxtNotaStorico.Text.Trim) & "'," & _
                    " NumeroProtocollo='" & ClsServer.NoApice(TxtProt.Text.Trim) & "', " & data & " where idflagenti=" & strid & ""
                    cmdinsert = New SqlClient.SqlCommand(myQuery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    cmdinsert.ExecuteNonQuery()
                    cmdinsert.Dispose()
                    imgConferma.Visible = True
                    imgCancella.Visible = True
                    msgConferma.Text = "La modifica è stata effettuata con successo"
                    'chiudo POPUP dopo modiFICA e aggiorno la pag sotto
                    Response.Write("<script>" & vbCrLf)
                    Response.Write("opener.location.reload()" & vbCrLf)
                    Response.Write("window.close();" & vbCrLf)
                    Response.Write("</script>")
                Else 'se non è stato gestito
                    myDataReader.Close()
                    myDataReader = Nothing
                    'procedo con l'inserimento
                    Dim dataIns As String = IIf(txtData.Text = "", "null", "convert(datetime,'" & txtData.Text & "',103)")
                    myQuery = "insert into flagenti (idente,idvincolo,valore,datamodifica," & _
                    " NotaStorico,NumeroProtocollo,DataProtocollazione) " & _
                    " values(" & Session("idEnte") & "," & stridvin & "" & _
                    " ," & btygenerico & ",getdate()," & _
                    " '" & ClsServer.NoApice(TxtNotaStorico.Text.Trim) & "'," & _
                    " '" & ClsServer.NoApice(TxtProt.Text.Trim) & "'," & dataIns & ")"
                    cmdinsert = New SqlClient.SqlCommand(myQuery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    cmdinsert.ExecuteNonQuery()
                    cmdinsert.Dispose()
                    imgCancella.Visible = True
                    imgConferma.Visible = True
                    msgConferma.Text = "La modifica è stata effettuata con successo"
                    'chiudo POPUP 
                    Response.Write("<script>" & vbCrLf)
                    Response.Write("opener.location.reload()" & vbCrLf)
                    Response.Write("window.close();" & vbCrLf)
                    Response.Write("</script>")
                End If
            Else 'se non seleziono
                msgErrore.Text = "Selezionare una opzione prima di confermare"
            End If
        ElseIf Request.QueryString("tipologia") = "Ruoli" Then 'verifico se ruoli*******************************************************
            If rdbno.Checked = True Or rdbsi.Checked = True Then 'verifico se è stato selezionato un flag
                If rdbno.Checked = True Then 'valorizzo variabile
                    btygenerico = 0
                Else
                    btygenerico = 1
                End If
                stridvin = Request.QueryString("Vincolo") 'passo idvincolo
                myQuery = "select idflagentepersonale from FlagEntePersonale where identepersonale='" & Request.QueryString("IDEntePersonale") & "' and idvincolo=" & stridvin & ""
                myDataReader = ClsServer.CreaDatareader(myQuery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                If myDataReader.HasRows = True Then 'verifico se è già stato gestito
                    Do While myDataReader.Read
                        strid = myDataReader.GetValue(0) 'valorizzo con id se già gestito
                    Loop
                    myDataReader.Close()
                    myDataReader = Nothing

                    'eseguo modiFICA
                    Dim dataR As String = IIf(txtData.Text = "", "DataProtocollazione=null", "DataProtocollazione=convert(datetime,'" & txtData.Text & "',103)")
                    myQuery = "update FlagEntePersonale set valore=" & btygenerico & "," & _
                    " datamodifica=getdate(), NotaStorico='" & ClsServer.NoApice(TxtNotaStorico.Text.Trim) & "'," & _
                    " NumeroProtocollo='" & ClsServer.NoApice(TxtProt.Text.Trim) & "', " & dataR & "" & _
                    " where IdFlagEntePersonale=" & strid & ""
                    cmdinsert = New SqlClient.SqlCommand(myQuery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    cmdinsert.ExecuteNonQuery()
                    cmdinsert.Dispose()
                    imgConferma.Visible = True
                    imgCancella.Visible = True
                    msgConferma.Text = "La modifica è stata effettuata con successo"
                    'chiudo POPUP dopo modiFICA
                    Response.Write("<script>" & vbCrLf)
                    Response.Write("opener.location.reload()" & vbCrLf)
                    Response.Write("window.close();" & vbCrLf)
                    Response.Write("</script>")
                Else 'se non è stato gestito
                    myDataReader.Close()
                    myDataReader = Nothing
                    'procedo con l'inserimento
                    Dim dataInsR As String = IIf(txtData.Text = "", "null", "convert(datetime,'" & txtData.Text & "',103)")
                    myQuery = "insert into FlagEntePersonale (idEntePersonale,idvincolo," & _
                    " valore,datamodifica, NotaStorico,NumeroProtocollo,DataProtocollazione) values(" & Request.QueryString("IDEntePersonale") & "," & _
                    " " & stridvin & "," & btygenerico & ",getdate(), " & _
                    " '" & ClsServer.NoApice(TxtNotaStorico.Text.Trim) & "'," & _
                    " '" & ClsServer.NoApice(TxtProt.Text.Trim) & "'," & dataInsR & ")"
                    cmdinsert = New SqlClient.SqlCommand(myQuery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    cmdinsert.ExecuteNonQuery()
                    cmdinsert.Dispose()
                    imgCancella.Visible = True
                    imgConferma.Visible = True
                    msgConferma.Text = "La modifica è stata effettuata con successo"
                    'chiudo POPUP 
                    Response.Write("<script>" & vbCrLf)
                    Response.Write("opener.location.reload()" & vbCrLf)
                    Response.Write("window.close();" & vbCrLf)
                    Response.Write("</script>")
                End If
            Else 'se non seleziono
                msgErrore.Text = "Selezionare una opzione prima di confermare"
            End If
        ElseIf Request.QueryString("tipologia") = "Progetti" Then 'verifico se progetti
            If rdbno.Checked = True Or rdbsi.Checked = True Then 'verifico se è stato selezionato un flag
                If rdbno.Checked = True Then 'valorizzo variabile
                    btygenerico = 0
                Else
                    btygenerico = 1
                End If
                stridvin = Request.QueryString("Vincolo") 'passo idvincolo
                myQuery = "select idflagattività from FlagAttività where idattività='" & Request.QueryString("IDattivita") & "' and idvincolo=" & stridvin & ""
                myDataReader = ClsServer.CreaDatareader(myQuery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                If myDataReader.HasRows = True Then 'verifico se è già stato gestito
                    Do While myDataReader.Read
                        strid = myDataReader.GetValue(0) 'valorizzo con id se già gestito
                    Loop
                    myDataReader.Close()
                    myDataReader = Nothing
                    'eseguo modiFICA
                    Dim dataA As String = IIf(txtData.Text = "", "DataProtocollazione=null", "DataProtocollazione=convert(datetime,'" & txtData.Text & "',103)")
                    myQuery = "update FlagAttività set valore=" & btygenerico & "," & _
                    " datamodifica=getdate(),NotaStorico='" & ClsServer.NoApice(TxtNotaStorico.Text.Trim) & "'," & _
                    " NumeroProtocollo='" & ClsServer.NoApice(TxtProt.Text.Trim) & "', " & dataA & "" & _
                    " where IdFlagAttività=" & strid & ""

                    cmdinsert = New SqlClient.SqlCommand(myQuery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    cmdinsert.ExecuteNonQuery()
                    cmdinsert.Dispose()
                    imgConferma.Visible = True
                    imgCancella.Visible = True
                    msgConferma.Text = "La modifica è stata effettuata con successo"
                    'chiudo POPUP dopo modiFICA
                    Response.Write("<script>" & vbCrLf)
                    Response.Write("opener.location.reload()" & vbCrLf)
                    Response.Write("window.close();" & vbCrLf)
                    Response.Write("</script>")
                Else 'se non è stato gestito
                    myDataReader.Close()
                    myDataReader = Nothing
                    'procedo con l'inserimento
                    Dim dataInsA As String = IIf(txtData.Text = "", "null", "convert(datetime,'" & txtData.Text & "',103)")
                    myQuery = "insert into FlagAttività (idattività,idvincolo,valore,datamodifica, NotaStorico,NumeroProtocollo,DataProtocollazione)" & _
                    " values(" & Request.QueryString("IDattivita") & "," & _
                    " " & stridvin & "," & btygenerico & ",getdate()," & _
                    " '" & ClsServer.NoApice(TxtNotaStorico.Text.Trim) & "'," & _
                    " '" & ClsServer.NoApice(TxtProt.Text.Trim) & "'," & dataInsA & ")"
                    cmdinsert = New SqlClient.SqlCommand(myQuery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    cmdinsert.ExecuteNonQuery()
                    cmdinsert.Dispose()
                    imgCancella.Visible = True
                    imgConferma.Visible = True
                    msgConferma.Text = "La modifica è stata effettuata con successo"
                    'chiudo POPUP 
                    Response.Write("<script>" & vbCrLf)
                    Response.Write("opener.location.reload()" & vbCrLf)
                    Response.Write("window.close();" & vbCrLf)
                    Response.Write("</script>")
                End If
            Else 'se non seleziono
                msgErrore.Text = "Selezionare una opzione prima di confermare"
            End If
        End If
    End Sub

    Private Sub imgCancella_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgCancella.Click
        CancellaMessaggiInfo()
        Dim cmdinsert As Data.SqlClient.SqlCommand
        Dim stridvin As String 'variabile appoggio idvincolo
        ChiudiDataReader(myDataReader)

        If Request.QueryString("tipologia") = "Enti" Then 'elimino flagenti
            stridvin = Request.QueryString("Vincolo") 'valorizzo con ID
            myQuery = "select idflagenti from flagenti where idente='" & Session("IdEnte") & "' and idvincolo=" & stridvin & ""
            myDataReader = ClsServer.CreaDatareader(myQuery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If myDataReader.HasRows = True Then 'identifico id record da eliminare 
                Do While myDataReader.Read
                    stridvin = myDataReader.GetValue(0) 'passo valore id
                Loop
                ChiudiDataReader(myDataReader)
                cmdinsert = New SqlClient.SqlCommand("delete from flagenti where idflagenti=" & stridvin & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                cmdinsert.ExecuteNonQuery()
                cmdinsert.Dispose()
                imgCancella.Visible = False
                rdbno.Checked = False
                rdbsi.Checked = False
                msgConferma.Text = "La modifica è stata effettuata con successo"
            Else
               ChiudiDataReader(myDataReader)
                msgErrore.Text = "Non risultano esserci vincoli da cancellare"
            End If
        ElseIf Request.QueryString("tipologia") = "Ruoli" Then 'elimino flagruoli ***************************************************
            stridvin = Request.QueryString("Vincolo") 'valorizzo con ID
            myQuery = "select idflagentepersonale from FlagEntePersonale where identepersonale='" & Request.QueryString("IDEntePersonale") & "' and idvincolo=" & stridvin & ""
            myDataReader = ClsServer.CreaDatareader(myQuery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If myDataReader.HasRows = True Then 'identifico id record da eliminare 
                Do While myDataReader.Read
                    stridvin = myDataReader.GetValue(0) 'passo valore id
                Loop
            ChiudiDataReader(myDataReader)
                'esegui comando delete
                cmdinsert = New SqlClient.SqlCommand("delete from FlagEntePersonale where idFlagEntePersonale=" & stridvin & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                cmdinsert.ExecuteNonQuery()
                cmdinsert.Dispose()
                imgCancella.Visible = False
                rdbno.Checked = False
                rdbsi.Checked = False
                msgConferma.Text = "La modifica è stata effettuata con successo"

            Else
                ChiudiDataReader(myDataReader)
                msgErrore.Text = "Non risultano esserci vincoli da cancellare"
            End If
        ElseIf Request.QueryString("tipologia") = "Progetti" Then 'elimino flagattività ***************************************************

            stridvin = Request.QueryString("Vincolo") 'valorizzo con ID
            myQuery = "select idflagattività from FlagAttività where idAttività='" & Request.QueryString("IDattivita") & "' and idvincolo=" & stridvin & ""
            myDataReader = ClsServer.CreaDatareader(myQuery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If myDataReader.HasRows = True Then 'identifico id record da eliminare 
                Do While myDataReader.Read
                    stridvin = myDataReader.GetValue(0) 'passo valore id
                Loop
                ChiudiDataReader(myDataReader)
                cmdinsert = New SqlClient.SqlCommand("delete from FlagAttività where idFlagAttività=" & stridvin & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                cmdinsert.ExecuteNonQuery()
                cmdinsert.Dispose()
                imgCancella.Visible = False
                rdbno.Checked = False
                rdbsi.Checked = False
                msgConferma.Text = "La modifica è stata effettuata con successo"
            Else
                ChiudiDataReader(myDataReader)
                msgErrore.Text = "Non risultano esserci vincoli da cancellare"
            End If

        End If
    End Sub

End Class