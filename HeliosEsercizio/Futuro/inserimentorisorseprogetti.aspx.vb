Imports System.Data.SqlClient

Public Class inserimentorisorseprogetti
    Inherits System.Web.UI.Page
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
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        If Page.IsPostBack = False Then
            CaricaComboProvinciaNazione(chkEsteroNascita.Checked)
        End If
    End Sub

    Function CheckEsistenzaCodFis(ByVal strCodFis As String) As Boolean
        Dim dtrCheckCodFis As SqlClient.SqlDataReader
        Dim strsql As String
        CheckEsistenzaCodFis = True
        'carico la stringa della select di controllo esistenza codicefiscale per la risorsa che si vuole inserire 
        strsql = "select CodiceFiscale from entepersonale "
        strsql = strsql & "where idente=" & Session("IdEnte") & " and CodiceFiscale='" & Trim(strCodFis) & "'"
        ChiudiDataReader(dtrCheckCodFis)
        dtrCheckCodFis = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrCheckCodFis.HasRows = True Then
            CheckEsistenzaCodFis = False
        End If
        ChiudiDataReader(dtrCheckCodFis)

    End Function

    Private Function CongruenzaCodiceFiscale(ByVal pCodiceFiscale As String, ByVal pCognome As String, ByVal pNome As String, ByVal pDataNascita As String) As Boolean
        Dim strCodCatasto As String
        Dim strNewCF As String
        Dim blnCheckSesso As Boolean
        If Len(txtCodiceFiscale.Text) <> 16 Then
            CongruenzaCodiceFiscale = False
            Exit Function
        End If
        'ricavo il codice catastale del comune
        strCodCatasto = ClsUtility.GetCodiceCatasto(Session("conn"), ddlComuneNascita.SelectedValue)
        'genero il CF con i dati anagrafici del volontario
        strNewCF = ClsUtility.CreaCF(Trim(Replace(pCognome, "'", "''")), Trim(Replace(pNome, "'", "''")), Trim(pDataNascita), strCodCatasto, "M")
        'lo confronto con il CF inserito nel CSV
        If UCase(Trim(pCodiceFiscale)) <> UCase(strNewCF) Then
            'genero il CF con i dati anagrafici del volontario
            strNewCF = ClsUtility.CreaCF(Trim(Replace(pCognome, "'", "''")), Trim(Replace(pNome, "'", "''")), Trim(pDataNascita), strCodCatasto, "F")
            If UCase(Trim(txtCodiceFiscale.Text)) <> UCase(strNewCF) Then
                'verifo eventuale OMOCODIA
                If ClsUtility.VerificaOmocodia(UCase(strNewCF), UCase(Trim(pCodiceFiscale))) = False Then
                    blnCheckSesso = False
                Else
                    blnCheckSesso = True
                End If
            Else
                blnCheckSesso = True
            End If
        Else
            blnCheckSesso = True
        End If
        If blnCheckSesso = False Then
            CongruenzaCodiceFiscale = False
            Exit Function
        Else
            CongruenzaCodiceFiscale = True
        End If

    End Function

    Private Function DecodificaOmocodici(ByVal pValore) As String
        Dim TuttiGliOmocodici As String = "LMNPQRSTUV"

        If InStr(TuttiGliOmocodici, pValore) > 0 Then

            Select Case pValore
                Case Is = "L"
                    DecodificaOmocodici = "0"

                Case "M"
                    DecodificaOmocodici = "1"

                Case "N"
                    DecodificaOmocodici = "2"

                Case "P"
                    DecodificaOmocodici = "3"

                Case "Q"
                    DecodificaOmocodici = "4"

                Case "R"
                    DecodificaOmocodici = "5"

                Case "S"
                    DecodificaOmocodici = "6"

                Case "T"
                    DecodificaOmocodici = "7"

                Case "U"
                    DecodificaOmocodici = "8"

                Case "V"
                    DecodificaOmocodici = "9"

            End Select
        Else
            DecodificaOmocodici = pValore
        End If

    End Function

    Private Sub cmdConferma_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdConferma.Click
        Dim dtrIdMax As SqlClient.SqlDataReader
        Dim strsql As String
        Dim strIdMax As String
        Dim strIdRuolo As String
        Dim strIdEntePersonaleRuolo As String
        lblErrore.Text = String.Empty
        txtCognome.Text = txtCognome.Text.ToUpper
        txtNome.Text = txtNome.Text.ToUpper
        txtCodiceFiscale.Text = txtCodiceFiscale.Text.ToUpper
        If VerificaCampiObbligatori() = True Then
            If chkEsteroNascita.Checked = False Then
                'buono
                If CongruenzaCodiceFiscale(txtCodiceFiscale.Text, txtCognome.Text, txtNome.Text, txtDataNascita.Text) = True Then
                    'vado a controllare se esiste già un codice fiscale
                    'false esiste
                    If CheckEsistenzaCodFis(txtCodiceFiscale.Text) = False Then
                        'vado a controllare se esiste il ruolo che si sta cercando inserire 
                        'per questa risorsa 
                        'se non esiste vado a fare la insert della risorsa
                        If CheckRuoloRisorsa(txtCodiceFiscale.Text, Request.QueryString("Ruolo")) = True Then
                            'caricamento della stringa contenente l'istruzione per la insert nella tabella entepersonale

                            'trovo l'id del ruolo che devo dare
                            'al tizio appena aggiunto
                            strsql = "select idruolo from ruoli where DescrAbb='" & Request.QueryString("Ruolo") & "'"
                            ChiudiDataReader(dtrIdMax)
                            dtrIdMax = ClsServer.CreaDatareader(strsql, Session("conn"))
                            'se ci sono dei record
                            If dtrIdMax.HasRows = True Then
                                dtrIdMax.Read()
                                strIdRuolo = dtrIdMax("idruolo")
                            End If
                            ChiudiDataReader(dtrIdMax)

                            strsql = "select IdEntePersonale from entepersonale where idente='" & Session("IdEnte") & "' and codicefiscale='" & txtCodiceFiscale.Text & "'"
                            dtrIdMax = ClsServer.CreaDatareader(strsql, Session("conn"))
                            'se ci sono dei record
                            If dtrIdMax.HasRows = True Then
                                dtrIdMax.Read()
                                strIdMax = dtrIdMax("IdEntePersonale")
                            End If
                            ChiudiDataReader(dtrIdMax)

                            strsql = "insert into entepersonaleruoli (IDEntePersonale,"
                            strsql = strsql & "IDRuolo,DataInizioValidità,Principale,Visibilità,DataInseritore,UsernameInseritore) values "
                            strsql = strsql & "('" & strIdMax & "',"
                            strsql = strsql & "'" & strIdRuolo & "',getdate(),0,0,GetDate(),'" & "N" & Mid(Session("Utente"), 2) & "')"

                            Dim myCommand As New SqlClient.SqlCommand
                            'sql command che mi esegue la insert
                            myCommand = New SqlClient.SqlCommand(strsql, Session("conn"))
                            myCommand.ExecuteNonQuery()
                            myCommand.Dispose()

                            If Request.QueryString("VengoDa") = "Sostituzione" Then
                                Response.Write("<script>" & vbCrLf)
                                Response.Write("window.opener.document.location.href='WfrmRisorseSostituibili.aspx?Cognome=" & txtCognome.Text & "&Nome=" & txtNome.Text & "&Tiporuolo=" & Request.QueryString("Tiporuolo") & "&IdEntePersonale=" & Request.QueryString("IdEntePersonale") & "&IdEntePersonaleRuolo=" & Request.QueryString("IdEntePersonaleRuolo") & "&IdRuolo=" & Request.QueryString("IdRuolo") & "'" & vbCrLf)
                                Response.Write("window.close()" & vbCrLf)
                                Response.Write("</script>" & vbCrLf)
                            Else
                                Response.Redirect("WebElencoOlp.aspx?Modifica=" & Request.QueryString("Modifica") & "&Tiporuolo=" & Request.QueryString("Tiporuolo") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&IdSedeAttuazione=" & Request.QueryString("IdSedeAttuazione") & "&idattES=" & Request.QueryString("idattES") & "&Nazionale=" & Request.QueryString("Nazionale"))
                            End If
                        Else 'codice fiscale presente controllo lo stato dell'accreditamento

                            'vado a controllare se lo stato dell'accreditamento del ruolo è = -1
                            'in questo caso vado a ripristinare lo stato di presentato per quel ruolo
                            'ma prima vado ad inserire nella cronologia lo stato precedente
                            'se è -1 (non accerditato)
                            strIdEntePersonaleRuolo = ControllaStatoAccerditamentoRuolo(Trim(txtCodiceFiscale.Text))
                            If strIdEntePersonaleRuolo <> "" Then

                                'preparo la insert nella cronologiaenterpersonaleruoli
                                strsql = "insert into cronologiaentepersonaleruoli (IdEntePersonaleRuolo, Accreditato, DataCronologia, IdTipoCronologia, UserNameAccreditatore, Forzatura, UserNameInseritore, datainseritore) "
                                strsql = strsql & "select identepersonaleruolo, accreditato, getdate(), '1', usernameaccreditatore, '0', usernameinseritore, datainseritore from entepersonaleruoli where identepersonaleruolo='" & strIdEntePersonaleRuolo & "'"

                                Dim myCommand As New SqlClient.SqlCommand
                                'sql command che mi esegue la insert
                                myCommand = New SqlClient.SqlCommand(strsql, Session("conn"))
                                myCommand.ExecuteNonQuery()
                                myCommand.Dispose()

                                'preparo l'update per la tabella dei ruoli della risorsa riportando allo stato di presentato il ruolo
                                strsql = "update entepersonaleruoli set accreditato=0, UserNameInseritore='" & "N" & Mid(Session("Utente"), 2) & "', DataInseritore=GetDate() where identepersonaleruolo='" & strIdEntePersonaleRuolo & "'"

                                'sql command che mi esegue la insert
                                myCommand = New SqlClient.SqlCommand(strsql, Session("conn"))
                                myCommand.ExecuteNonQuery()
                                myCommand.Dispose()

                                'modifica aggiunta da JOOJOJJJOOJJOOJOJOJOJJOO
                                'il 27/03/2008
                                'aggiunta la possibilità di raggiungere questa form dalla sostituzione delle risorse a progetto
                                If Request.QueryString("VengoDa") = "Sostituzione" Then
                                    Response.Write("<script>" & vbCrLf)
                                    Response.Write("window.opener.document.location.href='WfrmRisorseSostituibili.aspx?Cognome=" & txtCognome.Text & "&Nome=" & txtNome.Text & "&Tiporuolo=" & Request.QueryString("Tiporuolo") & "&IdEntePersonale=" & Request.QueryString("IdEntePersonale") & "&IdEntePersonaleRuolo=" & Request.QueryString("IdEntePersonaleRuolo") & "&IdRuolo=" & Request.QueryString("IdRuolo") & "'" & vbCrLf)
                                    Response.Write("window.close()" & vbCrLf)
                                    Response.Write("</script>" & vbCrLf)
                                Else
                                    Response.Redirect("WebElencoOlp.aspx?Modifica=" & Request.QueryString("Modifica") & "&Tiporuolo=" & Request.QueryString("Tiporuolo") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&IdSedeAttuazione=" & Request.QueryString("IdSedeAttuazione") & "&idattES=" & Request.QueryString("idattES") & "&Nazionale=" & Request.QueryString("Nazionale"))
                                End If
                            Else

                                lblErrore.Text = " Si sta tentando di inserire una risorsa gia' presente."
                                AppoID.Value = ddlComuneNascita.SelectedValue
                                Exit Sub
                            End If
                        End If
                    Else 'true non  esiste codicefiscale e vado ad inserire
                        'caricamento della stringa contenente l'istruzione per la insert nella tabella entepersonale
                        strsql = "insert into entepersonale (IDEnte,Cognome,Nome,IDComuneNascita,DataNascita,CodiceFiscale,DataCreazioneRecord,UsernameInseritore,CorsoOLP) "
                        strsql = strsql & "values (" & CInt(Session("idEnte")) & ",'" & Replace(Trim(txtCognome.Text), "'", "''") & "','" & Replace(Trim(txtNome.Text), "'", "''") & "',"
                        strsql = strsql & "'" & ddlComuneNascita.SelectedValue & "','" & txtDataNascita.Text & "','" & Replace(Trim(txtCodiceFiscale.Text), "'", "''") & "',GetDate(),'" & "N" & Mid(Session("Utente"), 2) & "'," & IIf(chkCorsoOlp.Checked = True, 1, 0) & ")"


                        Dim myCommand As New SqlClient.SqlCommand
                        myCommand = New SqlClient.SqlCommand(strsql, Session("conn"))
                        myCommand.ExecuteNonQuery()
                        myCommand.Dispose()

                        'trovo l'id max dell'utente appena inserito per associarlo
                        'al tizio appena aggiunto
                        strsql = "select top 1 IDEntePersonale from entepersonale where idente='" & Session("IdEnte") & "' and CodiceFiscale ='" & Trim(txtCodiceFiscale.Text) & "' order by identepersonale desc"
                        ChiudiDataReader(dtrIdMax)
                        dtrIdMax = ClsServer.CreaDatareader(strsql, Session("conn"))
                        'se ci sono dei record
                        If dtrIdMax.HasRows = True Then
                            dtrIdMax.Read()
                            strIdMax = dtrIdMax("IDEntePersonale")
                        End If
                        ChiudiDataReader(dtrIdMax)
                        strsql = "select idruolo from ruoli where DescrAbb='" & Request.QueryString("Ruolo") & "'"
                        dtrIdMax = ClsServer.CreaDatareader(strsql, Session("conn"))
                        'se ci sono dei record
                        If dtrIdMax.HasRows = True Then
                            dtrIdMax.Read()
                            strIdRuolo = dtrIdMax("idruolo")
                        End If
                        ChiudiDataReader(dtrIdMax)

                        strsql = "insert into entepersonaleruoli (IDEntePersonale,"
                        strsql = strsql & "IDRuolo,DataInizioValidità,Principale,Visibilità,DataInseritore,UserNameInseritore) values "
                        strsql = strsql & "('" & strIdMax & "',"
                        strsql = strsql & "'" & strIdRuolo & "',getdate(),1,0,GetDate(),'" & "N" & Mid(Session("Utente"), 2) & "')"
                        'sql command momentaneo che mi esegue la insert
                        myCommand = New SqlClient.SqlCommand(strsql, Session("conn"))
                        myCommand.ExecuteNonQuery()
                        myCommand.Dispose()

                        'modifica aggiunta da JOOJOJJJOOJJOOJOJOJOJJOO
                        'il 27/03/2008
                        'aggiunta la possibilità di raggiungere questa form dalla sostituzione delle risorse a progetto
                        If Request.QueryString("VengoDa") = "Sostituzione" Then
                            Response.Write("<script>" & vbCrLf)
                            Response.Write("window.opener.document.location.href='WfrmRisorseSostituibili.aspx?Cognome=" & txtCognome.Text & "&Nome=" & txtNome.Text & "&Tiporuolo=" & Request.QueryString("Tiporuolo") & "&IdEntePersonale=" & Request.QueryString("IdEntePersonale") & "&IdEntePersonaleRuolo=" & Request.QueryString("IdEntePersonaleRuolo") & "&IdRuolo=" & Request.QueryString("IdRuolo") & "'" & vbCrLf)
                            Response.Write("window.close()" & vbCrLf)
                            Response.Write("</script>" & vbCrLf)
                        Else
                            Response.Redirect("WebElencoOlp.aspx?Modifica=" & Request.QueryString("Modifica") & "&Tiporuolo=" & Request.QueryString("Tiporuolo") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&IdSedeAttuazione=" & Request.QueryString("IdSedeAttuazione") & "&idattES=" & Request.QueryString("idattES") & "&Nazionale=" & Request.QueryString("Nazionale"))
                        End If
                    End If
                Else
                    lblErrore.Text = "Il codice Fiscale risulta essere incorretto."
                    AppoID.Value = ddlComuneNascita.SelectedValue
                    Exit Sub
                End If
            Else 'nazionalità straniera salta tutti i controlli sul codice fiscale
                'vado a controllare se esiste già un codice fiscale
                'false esiste
                If CheckEsistenzaCodFis(txtCodiceFiscale.Text) = False Then
                    'vado a controllare se esiste il ruolo che si sta cercando inserire 
                    'per questa risorsa 
                    'se non esiste vado a fare la insert della risorsa
                    If CheckRuoloRisorsa(txtCodiceFiscale.Text, Request.QueryString("Ruolo")) = True Then
                        'caricamento della stringa contenente l'istruzione per la insert nella tabella entepersonale

                        'trovo l'id del ruolo che devo dare
                        'al tizio appena aggiunto
                        strsql = "select idruolo from ruoli where DescrAbb='" & Request.QueryString("Ruolo") & "'"
                        ChiudiDataReader(dtrIdMax)

                        dtrIdMax = ClsServer.CreaDatareader(strsql, Session("conn"))
                        'se ci sono dei record
                        If dtrIdMax.HasRows = True Then
                            dtrIdMax.Read()
                            strIdRuolo = dtrIdMax("idruolo")
                        End If
                        ChiudiDataReader(dtrIdMax)


                        'trovo l'id del ruolo che devo dare
                        'al tizio appena aggiunto
                        strsql = "select IdEntePersonale from entepersonale where idente='" & Session("IdEnte") & "' and codicefiscale='" & txtCodiceFiscale.Text & "'"
                        'controllo e chiudo se aperto il datareader
                        ChiudiDataReader(dtrIdMax)
                        dtrIdMax = ClsServer.CreaDatareader(strsql, Session("conn"))
                        'se ci sono dei record
                        If dtrIdMax.HasRows = True Then
                            dtrIdMax.Read()
                            strIdMax = dtrIdMax("IdEntePersonale")
                        End If
                        ChiudiDataReader(dtrIdMax)
                        strsql = "insert into entepersonaleruoli (IDEntePersonale,"
                        strsql = strsql & "IDRuolo,DataInizioValidità,Principale,Visibilità,DataInseritore,UserNameInseritore) values "
                        strsql = strsql & "('" & strIdMax & "',"
                        strsql = strsql & "'" & strIdRuolo & "',getdate(),0,0,GetDate(),'" & "N" & Mid(Session("Utente"), 2) & "')"

                        Dim myCommand As New SqlClient.SqlCommand
                        'sql command che mi esegue la insert
                        myCommand = New SqlClient.SqlCommand(strsql, Session("conn"))
                        myCommand.ExecuteNonQuery()
                        myCommand.Dispose()

                        If Request.QueryString("VengoDa") = "Sostituzione" Then
                            Response.Write("<script>" & vbCrLf)
                            Response.Write("window.opener.document.location.href='WfrmRisorseSostituibili.aspx?Cognome=" & txtCognome.Text & "&Nome=" & txtNome.Text & "&Tiporuolo=" & Request.QueryString("Tiporuolo") & "&IdEntePersonale=" & Request.QueryString("IdEntePersonale") & "&IdEntePersonaleRuolo=" & Request.QueryString("IdEntePersonaleRuolo") & "&IdRuolo=" & Request.QueryString("IdRuolo") & "'" & vbCrLf)
                            Response.Write("window.close()" & vbCrLf)
                            Response.Write("</script>" & vbCrLf)
                        Else
                            Response.Redirect("WebElencoOlp.aspx?Modifica=" & Request.QueryString("Modifica") & "&Tiporuolo=" & Request.QueryString("Tiporuolo") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&IdSedeAttuazione=" & Request.QueryString("IdSedeAttuazione") & "&idattES=" & Request.QueryString("idattES") & "&Nazionale=" & Request.QueryString("Nazionale"))
                        End If
                    Else 'codice fiscale presente controllo lo stato dell'accreditamento

                        'vado a controllare se lo stato dell'accreditamento del ruolo è = -1
                        'in questo caso vado a ripristinare lo stato di presentato per quel ruolo
                        'ma prima vado ad inserire nella cronologia lo stato precedente
                        'se è -1 (non accerditato)
                        strIdEntePersonaleRuolo = ControllaStatoAccerditamentoRuolo(Trim(txtCodiceFiscale.Text))
                        If strIdEntePersonaleRuolo <> "" Then

                            'preparo la insert nella cronologiaenterpersonaleruoli
                            strsql = "insert into cronologiaentepersonaleruoli (IdEntePersonaleRuolo, Accreditato, DataCronologia, IdTipoCronologia, UserNameAccreditatore, Forzatura, UserNameInseritore, datainseritore) "
                            strsql = strsql & "select identepersonaleruolo, accreditato, getdate(), '1', usernameaccreditatore, '0', usernameinseritore, datainseritore from entepersonaleruoli where identepersonaleruolo='" & strIdEntePersonaleRuolo & "'"

                            Dim myCommand As New SqlClient.SqlCommand
                            'sql command che mi esegue la insert
                            myCommand = New SqlClient.SqlCommand(strsql, Session("conn"))
                            myCommand.ExecuteNonQuery()
                            myCommand.Dispose()

                            'preparo l'update per la tabella dei ruoli della risorsa riportando allo stato di presentato il ruolo
                            strsql = "update entepersonaleruoli set accreditato=0, UserNameInseritore='" & "N" & Mid(Session("Utente"), 2) & "', DataInseritore=GetDate() where identepersonaleruolo='" & strIdEntePersonaleRuolo & "'"

                            'sql command che mi esegue la insert
                            myCommand = New SqlClient.SqlCommand(strsql, Session("conn"))
                            myCommand.ExecuteNonQuery()
                            myCommand.Dispose()

                            'aggiunta la possibilità di raggiungere questa form dalla sostituzione delle risorse a progetto
                            If Request.QueryString("VengoDa") = "Sostituzione" Then
                                Response.Write("<script>" & vbCrLf)
                                Response.Write("window.opener.document.location.href='WfrmRisorseSostituibili.aspx?Cognome=" & txtCognome.Text & "&Nome=" & txtNome.Text & "&Tiporuolo=" & Request.QueryString("Tiporuolo") & "&IdEntePersonale=" & Request.QueryString("IdEntePersonale") & "&IdEntePersonaleRuolo=" & Request.QueryString("IdEntePersonaleRuolo") & "&IdRuolo=" & Request.QueryString("IdRuolo") & "'" & vbCrLf)
                                Response.Write("window.close()" & vbCrLf)
                                Response.Write("</script>" & vbCrLf)
                            Else
                                Response.Redirect("WebElencoOlp.aspx?Modifica=" & Request.QueryString("Modifica") & "&Tiporuolo=" & Request.QueryString("Tiporuolo") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&IdSedeAttuazione=" & Request.QueryString("IdSedeAttuazione") & "&idattES=" & Request.QueryString("idattES") & "&Nazionale=" & Request.QueryString("Nazionale"))
                            End If
                        Else
                            lblErrore.Text = "Si sta tentando di inserire una risorsa gi&#224; presente."
                            AppoID.Value = ddlComuneNascita.SelectedValue
                            Exit Sub
                        End If
                    End If
                Else 'true non esiste e vado ad inserire
                    'caricamento della stringa contenente l'istruzione per la insert nella tabella entepersonale
                    strsql = "insert into entepersonale (IDEnte,Cognome,Nome,IDComuneNascita,DataNascita,CodiceFiscale,DataCreazioneRecord,UsernameInseritore,CorsoOLP) "
                    strsql = strsql & "values (" & CInt(Session("idEnte")) & ",'" & Replace(Trim(txtCognome.Text), "'", "''") & "','" & Replace(Trim(txtNome.Text), "'", "''") & "',"
                    strsql = strsql & "'" & ddlComuneNascita.SelectedValue & "','" & txtDataNascita.Text & "','" & Replace(Trim(txtCodiceFiscale.Text), "'", "''") & "',GetDate(),'" & "N" & Mid(Session("Utente"), 2) & "'," & IIf(chkCorsoOlp.Checked = True, 1, 0) & ")"

                    Dim myCommand As New SqlClient.SqlCommand
                    'sql command che mi esegue la insert
                    myCommand = New SqlClient.SqlCommand(strsql, Session("conn"))
                    myCommand.ExecuteNonQuery()
                    myCommand.Dispose()

                    strsql = "select top 1 IDEntePersonale from entepersonale where idente='" & Session("IdEnte") & "' and CodiceFiscale ='" & Trim(txtCodiceFiscale.Text) & "' order by identepersonale desc"
                    ChiudiDataReader(dtrIdMax)
                    dtrIdMax = ClsServer.CreaDatareader(strsql, Session("conn"))
                    'se ci sono dei record
                    If dtrIdMax.HasRows = True Then
                        dtrIdMax.Read()
                        strIdMax = dtrIdMax("IDEntePersonale")
                    End If
                    ChiudiDataReader(dtrIdMax)
                    'trovo l'id del ruolo che devo dare
                    'al tizio appena aggiunto
                    strsql = "select idruolo from ruoli where DescrAbb='" & Request.QueryString("Ruolo") & "'"
                    ChiudiDataReader(dtrIdMax)
                    dtrIdMax = ClsServer.CreaDatareader(strsql, Session("conn"))
                    'se ci sono dei record
                    If dtrIdMax.HasRows = True Then
                        dtrIdMax.Read()
                        strIdRuolo = dtrIdMax("idruolo")
                    End If
                    ChiudiDataReader(dtrIdMax)

                    strsql = "insert into entepersonaleruoli (IDEntePersonale,"
                    strsql = strsql & "IDRuolo,DataInizioValidità,Principale,Visibilità,DataInseritore,UserNameInseritore) values "
                    strsql = strsql & "('" & strIdMax & "',"
                    strsql = strsql & "'" & strIdRuolo & "',getdate(),1,0,GetDate(),'" & "N" & Mid(Session("Utente"), 2) & "')"
                    'sql command momentaneo che mi esegue la insert
                    myCommand = New SqlClient.SqlCommand(strsql, Session("conn"))
                    myCommand.ExecuteNonQuery()
                    myCommand.Dispose()

                    If Request.QueryString("VengoDa") = "Sostituzione" Then
                        Response.Write("<script>" & vbCrLf)
                        Response.Write("window.opener.document.location.href='WfrmRisorseSostituibili.aspx?Cognome=" & txtCognome.Text & "&Nome=" & txtNome.Text & "&Tiporuolo=" & Request.QueryString("Tiporuolo") & "&IdEntePersonale=" & Request.QueryString("IdEntePersonale") & "&IdEntePersonaleRuolo=" & Request.QueryString("IdEntePersonaleRuolo") & "&IdRuolo=" & Request.QueryString("IdRuolo") & "'" & vbCrLf)
                        Response.Write("window.close()" & vbCrLf)
                        Response.Write("</script>" & vbCrLf)
                    Else
                        Response.Redirect("WebElencoOlp.aspx?Modifica=" & Request.QueryString("Modifica") & "&Tiporuolo=" & Request.QueryString("Tiporuolo") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&IdSedeAttuazione=" & Request.QueryString("IdSedeAttuazione") & "&idattES=" & Request.QueryString("idattES") & "&Nazionale=" & Request.QueryString("Nazionale"))
                    End If
                End If
            End If
        End If

    End Sub

    Function ControllaStatoAccerditamentoRuolo(ByVal strCodiceFiscale As String) As String
        Dim strSql As String
        Dim dtrCheckCodFis As SqlClient.SqlDataReader

        strSql = "select b.identepersonaleruolo, isnull(b.accreditato,'') as accreditato from entepersonale as a "
        strSql = strSql & "inner join entepersonaleruoli as b on a.identepersonale=b.identepersonale "
        strSql = strSql & "inner join ruoli as c on b.idruolo=c.idruolo "
        Select Case Request.QueryString("Ruolo")
            Case "OLP"
                strSql = strSql & "where a.codicefiscale='" & ClsServer.NoApice(strCodiceFiscale) & "' and  c.ruolo='Operatore Locale di Progetto' "
            Case "RLEA"
                strSql = strSql & "where a.codicefiscale='" & ClsServer.NoApice(strCodiceFiscale) & "' and  c.ruolo='Responsabile Locale Ente di Accreditamento' "
            Case "TUTOR"
                strSql = strSql & "where a.codicefiscale='" & ClsServer.NoApice(strCodiceFiscale) & "' and  c.ruolo='Tutor' "
        End Select

        'controllo e chiudo se aperto il datareader
        If Not dtrCheckCodFis Is Nothing Then
            dtrCheckCodFis.Close()
            dtrCheckCodFis = Nothing
        End If
        'eseguo la query
        dtrCheckCodFis = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrCheckCodFis.HasRows = True Then
            dtrCheckCodFis.Read()
            If dtrCheckCodFis("accreditato") = "-1" Then
                ControllaStatoAccerditamentoRuolo = dtrCheckCodFis("identepersonaleruolo")
            Else
                ControllaStatoAccerditamentoRuolo = ""
            End If
        Else
            ControllaStatoAccerditamentoRuolo = ""
        End If

        'il ruolo per lq risorsa risulta essere già in uso
        'controllo e chiudo se aperto il datareader
        If Not dtrCheckCodFis Is Nothing Then
            dtrCheckCodFis.Close()
            dtrCheckCodFis = Nothing
        End If


    End Function

    Function CheckRuoloRisorsa(ByVal strCodFis As String, ByVal strRuolo As String) As Boolean
        'Operatore Locale di Progetto
        'Tutor
        'Responsabile Locale Ente di Accreditamento	
        'dtr per controllo esistenza codice fiscale
        Dim dtrCheckCodFis As SqlClient.SqlDataReader
        Dim strsql As String

        CheckRuoloRisorsa = True

        strsql = "select "
        strsql = strsql & "a.Nome, "
        strsql = strsql & "a.Cognome, "
        strsql = strsql & "a.CodiceFiscale,"
        strsql = strsql & "b.idruolo, "
        strsql = strsql & "c.idruolo, "
        strsql = strsql & "c.ruolo "
        strsql = strsql & "from entepersonale as a "
        strsql = strsql & "inner join entepersonaleruoli as b on a.identepersonale=b.identepersonale "
        strsql = strsql & "inner join ruoli as c on b.idruolo=c.idruolo "
        strsql = strsql & "where a.codicefiscale='" & strCodFis & "' and a.idente='" & Session("IdEnte") & "' and c.DescrAbb = '" & strRuolo & "'"

        'controllo e chiudo se aperto il datareader
        If Not dtrCheckCodFis Is Nothing Then
            dtrCheckCodFis.Close()
            dtrCheckCodFis = Nothing
        End If
        'eseguo la query
        dtrCheckCodFis = ClsServer.CreaDatareader(strsql, Session("conn"))
        'se ci sono dei record
        If dtrCheckCodFis.HasRows = True Then
            'il ruolo per lq risorsa risulta essere già in uso
            CheckRuoloRisorsa = False
        End If
        'controllo e chiudo se aperto il datareader
        If Not dtrCheckCodFis Is Nothing Then
            dtrCheckCodFis.Close()
            dtrCheckCodFis = Nothing
        End If

    End Function

    'true italiano 
    'false straniero
    Function NazionalitaItaliana(ByVal pComune As String) As Boolean
        Dim dtrNazioneBase As Data.SqlClient.SqlDataReader
        Dim strsql As String

        strsql = "SELECT Nazioni.NazioneBase FROM Nazioni " & _
                 "INNER JOIN Regioni ON Regioni.IdNazione = Nazioni.IdNazione " & _
                 "INNER JOIN Provincie ON Provincie.IdRegione = Regioni.IdRegione " & _
                 "INNER JOIN Comuni ON Comuni.IdProvincia = Provincie.IdProvincia " & _
                 "WHERE Comuni.Denominazione = '" & ClsServer.NoApice(pComune) & "'"

        dtrNazioneBase = ClsServer.CreaDatareader(strsql, Session("conn"))

        dtrNazioneBase.Read()
        If dtrNazioneBase.HasRows = False Then
            NazionalitaItaliana = False
        Else
            NazionalitaItaliana = dtrNazioneBase("NazioneBase")
        End If

        dtrNazioneBase.Close()
        dtrNazioneBase = Nothing
    End Function

    Private Sub imgChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgChiudi.Click
        If Request.QueryString("VengoDa") = "Sostituzione" Then
            Response.Write("<script>" & vbCrLf)
            Response.Write("window.opener.document.location.href='WfrmRisorseSostituibili.aspx?Cognome=" & txtCognome.Text & "&Nome=" & txtNome.Text & "&Tiporuolo=" & Request.QueryString("Tiporuolo") & "&IdEntePersonale=" & Request.QueryString("IdEntePersonale") & "&IdEntePersonaleRuolo=" & Request.QueryString("IdEntePersonaleRuolo") & "&IdRuolo=" & Request.QueryString("IdRuolo") & "'" & vbCrLf)
            Response.Write("window.close()" & vbCrLf)
            Response.Write("</script>" & vbCrLf)
        Else
            Response.Redirect("WebElencoOlp.aspx?Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&Tiporuolo=" & Request.QueryString("Tiporuolo") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&IdSedeAttuazione=" & Request.QueryString("IdSedeAttuazione") & "&idattES=" & Request.QueryString("idattES") & "&VengoDa=" & Request.QueryString("VengoDa"))
        End If
    End Sub

    Private Sub ChkEsteroNascita_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkEsteroNascita.CheckedChanged
        CaricaComboProvinciaNazione(chkEsteroNascita.Checked)
        ddlComuneNascita.DataSource = Nothing
        ddlComuneNascita.Items.Add("")
        ddlComuneNascita.SelectedIndex = 0
    End Sub
    Private Sub CaricaComboProvinciaNazione(ByVal blnEstero As Boolean)
        Dim SelComune As New clsSelezionaComune
        ddlProvinciaNascita = SelComune.CaricaProvinciaNazione(ddlProvinciaNascita, blnEstero, Session("Conn"))

    End Sub
    Private Sub ddlProvinciaNascita_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProvinciaNascita.SelectedIndexChanged
        Dim SelComune As New clsSelezionaComune
        ddlComuneNascita.Enabled = True
        ddlComuneNascita = SelComune.CaricaComuniNascita(ddlComuneNascita, ddlProvinciaNascita.SelectedValue, Session("Conn"))
    End Sub

    Private Function VerificaCampiObbligatori() As Boolean
        Dim utility As ClsUtility = New ClsUtility()
        Dim idTipoProgetto As String = utility.TipologiaProgettoDaIdAttivita(Request.QueryString("IdAttivita"), Session("conn"))
        Dim campiValidi As Boolean
        Dim campoObbligatorio As String = "Il campo {0} è obbligatorio.<br/>"
        If (txtCognome.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Cognome")
            campiValidi = False
        End If
        If (txtNome.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Nome")
            campiValidi = False
        End If

        If (txtCodiceFiscale.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Codice Fiscale")
            campiValidi = False
        End If
        If (txtDataNascita.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Data di nascita")
            campiValidi = False
        Else
            campiValidi = ValidaData(txtDataNascita.Text, "Data di nascita")
        End If
        If (ddlProvinciaNascita.SelectedItem.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Provincia/Nazione di nascita")
            campiValidi = False
        End If
        If (ddlComuneNascita.SelectedItem.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Comune di nascita")
            campiValidi = False
        End If

        Return campiValidi
    End Function
    Private Function ValidaData(ByVal data As String, ByVal nomeCampo As String) As Boolean
        Dim dataTmp As Date
        Dim dataValida As Boolean = True
        Dim messaggioDataValida As String = "Il valore di '{0}' non è valido. Inserire la data nel formato gg/mm/aaaa.<br/>"

        If (Date.TryParse(data, dataTmp) = False) Then
            lblErrore.Text = lblErrore.Text + String.Format(messaggioDataValida, nomeCampo)
            dataValida = False
        End If
        Return dataValida

    End Function

End Class