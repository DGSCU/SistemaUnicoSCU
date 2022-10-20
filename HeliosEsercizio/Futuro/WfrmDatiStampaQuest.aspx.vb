Imports System.Drawing
Imports System.Data.SqlClient

Public Class WfrmDatiStampaQuest
    Inherits System.Web.UI.Page
    Dim strSql As String
    Dim dtsAnagraficaEnti As System.Data.DataSet
    Dim dtaAnagraficaEnti As SqlClient.SqlDataAdapter
    Dim strProvinciaEstero As String
    Dim selComune As New clsSelezionaComune
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then
            Dim selComune As New clsSelezionaComune
            strProvinciaEstero = ""
            Call CaricaDati()
            Call CaricaComboProvincia()
            selComune.CaricaProvincie(ddlProvinciaNascita, ChkEstero.Checked, Session("Conn"))
            ColorDatiAccreditoEnte(Color.LightGray, False, True)
            ColorDatiAccreditoAltriEnte(Color.LightGray, False, True)
            Call TipologiaEnte()
        End If


    End Sub

    Private Sub CaricaDati()
        Dim dtrCrono As SqlClient.SqlDataReader

        strSql = "SELECT QuestionarioCronologiaStampe.IdEnte, QuestionarioCronologiaStampe.CognomeSottoscritto, QuestionarioCronologiaStampe.NomeSottoscritto, " _
               & " QuestionarioCronologiaStampe.DataDiNascita, QuestionarioCronologiaStampe.IDComuneNascita, QuestionarioCronologiaStampe.CapComune, " _
               & " QuestionarioCronologiaStampe.NominativoDaContattare, QuestionarioCronologiaStampe.RuoloNominativo, " _
               & " QuestionarioCronologiaStampe.ValoreContributoCifra, QuestionarioCronologiaStampe.ValoreContributoLettere, comuni.Denominazione AS comune, provincie.Provincia, provincie.IDProvincia, provincie.provincenazionali " _
               & " FROM provincie INNER JOIN " _
               & " comuni ON provincie.IDProvincia = comuni.IDProvincia INNER JOIN " _
               & " QuestionarioCronologiaStampe ON comuni.IDComune = QuestionarioCronologiaStampe.IDComuneNascita " _
               & " WHERE (QuestionarioCronologiaStampe.IdCronologiaStampa =(SELECT MAX(IdCronologiaStampa) FROM  QuestionarioCronologiaStampe Where IdEnte=" & Session("IdEnte") & "))"

        dtrCrono = ClsServer.CreaDatareader(strSql, Session("conn"))

        dtrCrono.Read()

        Dim clsConNum As New clsConvertiNumeriInLettere
        Dim IDComuneNascita As String
        Dim IDProvinciaNascita As String

        If dtrCrono.HasRows = True Then
            ChkEstero.Checked = Not CBool(dtrCrono("ProvinceNazionali"))
            txtCognome.Text = dtrCrono("CognomeSottoscritto")
            txtNome.Text = dtrCrono("NomeSottoscritto")
            txtDataNascita.Text = dtrCrono("DataDiNascita")



            IDProvinciaNascita = dtrCrono("IDProvincia")
            If dtrCrono("ProvinceNazionali") <> 1 Then
                strProvinciaEstero = "UNION select idprovincia, provincia from provincie where idprovincia = " & dtrCrono("IDProvincia")
            End If

            ddlComuneNascita.SelectedValue = dtrCrono("IDComuneNascita")

            IDComuneNascita = dtrCrono("IDComuneNascita")

            txtCAPNascita.Text = dtrCrono("CapComune")
            txtNominativo.Text = dtrCrono("NominativoDaContattare")
            txtPosizione.Text = dtrCrono("RuoloNominativo")
            txtIDComuneNascita.Value = dtrCrono("IDComuneNascita")

            ChiudiDataReader(dtrCrono)
            selComune.CaricaProvincie(ddlProvinciaNascita, ChkEstero.Checked, Session("Conn"))
            ChiudiDataReader(dtrCrono)
            If Not IDProvinciaNascita Is Nothing Then
                ddlProvinciaNascita.SelectedValue = IDProvinciaNascita
            End If
            If Not IDComuneNascita Is Nothing Then
                'selComune.CaricaComuniDaProvincia(ddlComuneNascita, IDProvinciaNascita, Session("Conn"))
                selComune.CaricaComuniNascita(ddlComuneNascita, IDProvinciaNascita, Session("Conn"))
                ddlComuneNascita.SelectedValue = IDComuneNascita
                ChiudiDataReader(dtrCrono)
            End If
        End If
         ChiudiDataReader(dtrCrono)

        'mod il 25/03/2008 da simona cordella
        ' riporto l'importo che viene sempre calcolato 
        txtImportoEuro.Text = Request.QueryString("Importo")
        txtImportoLettere.Text = clsConNum.NumToCars(txtImportoEuro.Text)

    End Sub

    Sub CaricaComboProvincia()
        'creato da simona cordella il 17/04/2012
        Dim dsProv As DataSet

        strSql = " SELECT '0' as idprovincia ,'' as provincia from provincie " & _
                 " WHERE  provincenazionali = 1 and idregionecompetenza <> 22 " & _
                 " UNION  " & _
                 " SELECT idprovincia,provincia from provincie  " & _
                 " WHERE provincenazionali = 1 and idregionecompetenza <> 22 " & _
                 " Order by provincia "
        dsProv = ClsServer.DataSetGenerico(strSql, Session("conn"))
        ddlProvincia.DataSource = dsProv
        ddlProvincia.DataValueField = "idprovincia"
        ddlProvincia.DataTextField = "provincia"
        ddlProvincia.DataBind()
    End Sub

    Sub ColorDatiAccreditoEnte(ByVal colore As Color, ByVal blnEnabled As Boolean, ByVal blnReadOnly As Boolean)
        ddlProvincia.BackColor = colore
        txtNConto.BackColor = colore
        ddlProvincia.Enabled = blnEnabled
        txtNConto.ReadOnly = blnReadOnly
        txtIbanTesoreria.BackColor = colore
        txtIbanTesoreria.ReadOnly = blnReadOnly
    End Sub

    Sub ColorDatiAccreditoAltriEnte(ByVal colore As Color, ByVal blnEnabled As Boolean, ByVal blnReadOnly As Boolean)

        txtIban.BackColor = colore
        txtBicSwift.BackColor = colore
        txtIban.Enabled = blnEnabled
        txtBicSwift.ReadOnly = blnReadOnly
    End Sub

    Sub TipologiaEnte()
        Dim dtrTipologia As SqlClient.SqlDataReader
        strSql = "Select t.privato from enti e inner join TipologieEnti t on e.tipologia =t.descrizione  where idente =" & Session("IdEnte")
        dtrTipologia = ClsServer.CreaDatareader(strSql, Session("Conn"))
        If dtrTipologia.HasRows = True Then
            dtrTipologia.Read()
            'If dtrTipologia("Tipologia") = "Privato" Then
            If dtrTipologia("privato") = 1 Then
                ColorDatiAccreditoEnte(Color.LightGray, False, True)
                ColorDatiAccreditoAltriEnte(Color.White, True, False)
                optEntePubblico.Enabled = False
                optAltriEnti.Checked = True
            End If
        End If
        dtrTipologia.Close()
        dtrTipologia = Nothing
    End Sub

    Private Sub cmdSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSalva.Click
        '*****************************************************************************************+
        'AUTORE: Guido Testa 
        'DATA: 28/06/2006
        'DESCRIZONE: preparo le informazioni necessarie per poter eseguire la stmpa del modulo


        '******************************************************************

        '*********CONTROLLI SERVER ****************************************
        If controlliSalvataggioServer() = False Then
            Exit Sub
        End If
        '******************************************************************

        '*****************************************************************************************+
        Dim IdCrono As Integer


        '******************** CONTROLLI VALIDITA'  ********************
        'PROVINCIA E CONTO
        If ControlliFormali() = False Then
            Exit Sub
        End If


        'IBAN
        If optRimborsoNo.Checked = False Then
            If ControlloValiditàIban() = False Then
                Exit Sub
            End If
        End If

        Dim SelTipoModuloF As Integer
        If Request.QueryString("TipoFormazioneGenerale") = 1 Then
            SelTipoModuloF = 21
        Else
            SelTipoModuloF = 41
        End If
        'inserisco la cronologia di stampa
        IdCrono = InsertCronologia()
        'lancio il report pdf
        Response.Write("<script>")
        Response.Write("myWin = window.open ('WfrmReportistica.aspx?sTipoStampa=" & SelTipoModuloF & "&IdCronologia=" & IdCrono & "','Report','height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes')")
        Response.Write("</script>")
        'chiudo la form di immissione dati stampa
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.close();" & vbCrLf)
        Response.Write("</script>")
    End Sub

    Private Function ControlliFormali() As Boolean
        'Creata da simona cordella il 20/04/2012
        ControlliFormali = True
        lblErr.Visible = True
        If optEntePubblico.Checked = True Then

            If ControlloValiditàIbanTesoreria() = False Then
                ControlliFormali = False
                Exit Function
            End If


            'If ddlProvincia.SelectedValue <> 0 Then 'provincia valorizzata
            '    If txtNConto.Text = "" Then
            '        lblErr.Text = " E' necessario indicare il numero del conto."
            '        ControlliFormali = False
            '    Else
            '        If IsNumeric(txtNConto.Text) = False Then
            '            lblErr.Text = "Il numero del conto è un campo numerico."
            '            ControlliFormali = False
            '        End If
            '    End If
            'Else
            '    'If IsNumeric(txtNConto.Text) = False Then
            '    '    lblmessaggiosopra.Text = "Il numero del conto è un campo numerico."
            '    '    ControlliFormali = False
            '    'End If
            '    'If txtNConto.Text <> "" Then
            '    lblErr.Text = "E' necessario indicare la provincia."
            '    ControlliFormali = False
            '    'End If
            'End If
        Else

            If optRimborsoNo.Checked = False Then
                If txtIban.Text = "" Then
                    lblErr.Text = "E' necessario indicare il codice IBAN."
                    ControlliFormali = False
                Else
                    If txtBicSwift.Text = "" Then

                        lblErr.Text = "E' necessario indicare il codice Bic/Swift."
                        ControlliFormali = False

                    End If
                End If
            End If


        End If


    End Function

    Private Function ControlloValiditàIban() As Boolean
        Dim clsIban As New CheckBancari
        lblErr.Visible = False
        ControlloValiditàIban = True
        '******************** CONTROLLO VALIDITA' IBAN ********************
        'mod. il 18/04/2012 Controllo l'iban e bic/swift solo se il conto è in italia
        If txtIban.Text <> "" Then

            If UCase(Left(txtIban.Text, 2)) = "IT" Then
                lblErr.Visible = True
                If Len(txtIban.Text) < 27 Then
                    lblErr.Text = "La lunghezza del codice IBAN è errata."
                    ControlloValiditàIban = False
                    Exit Function
                Else
                    If clsIban.VerificaLetteraCin(txtIban.Text) = "1" Then
                        lblErr.Text = "Codice Iban errato."
                        ControlloValiditàIban = False
                        Exit Function
                    End If

                    'Funzione che controlla l'autenticità del codice iban indicato
                    Dim ChkCalcolaIban As String = clsIban.CalcolaIBAN(Left(txtIban.Text, 2), Mid(txtIban.Text, 5))
                    If UCase(ChkCalcolaIban) <> UCase(txtIban.Text) Then
                        lblErr.Text = "Codice Iban errato."
                        ControlloValiditàIban = False
                        Exit Function
                    Else
                        'controllo abi e cab non devono corrispondere a queste caratteristiche 
                        'ABI   = 07601 
                        'CAB = 3384
                        If ClsUtility.AbiCab(UCase(txtIban.Text)) Then
                            lblErr.Text = "Il codice iban indicato non fa riferimento ad un conto corrente bancario."
                            ControlloValiditàIban = False
                            Exit Function
                        End If
                    End If
                End If
                ''CODICE BIC/SWIFT
                If Trim(txtBicSwift.Text) <> "" Then
                    'lunghezza bicswift 8 - 11
                    If (Len(txtBicSwift.Text) <> 8) Then
                        If (Len(txtBicSwift.Text) <> 11) Then
                            lblErr.Text = "La lunghezza del codice  BIC/SWIFT è errata."
                            ControlloValiditàIban = False
                            Exit Function
                        End If
                    End If
                End If
                If Trim(txtBicSwift.Text) = "" Then 'Trim(txtIban.Text) <> "" And 
                    lblErr.Text = "E' necessario indicare il BIC/SWIFT."
                    ControlloValiditàIban = False
                    Exit Function
                End If
            Else
                lblErr.Visible = True
                lblErr.Text = "Codice Iban errato."
                ControlloValiditàIban = False
                Exit Function
            End If
        End If
    End Function
    Private Function ControlloValiditàIbanTesoreria() As Boolean
        Dim clsIban As New CheckBancari
        lblErr.Visible = False
        ControlloValiditàIbanTesoreria = True
        '******************** CONTROLLO VALIDITA' IBAN ********************
        'mod. il 18/04/2012 Controllo l'iban e bic/swift solo se il conto è in italia
        If txtIbanTesoreria.Text <> "" Then

            If UCase(Left(txtIbanTesoreria.Text, 2)) = "IT" Then
                lblErr.Visible = True
                If Len(txtIbanTesoreria.Text) < 27 Then
                    lblErr.Text = "La lunghezza del codice IBAN è errata."
                    ControlloValiditàIbanTesoreria = False
                    Exit Function
                Else

                    'If clsIban.Tesoreria <> Mid(txtIbanTesoreria.Text, 6, 5) Then
                    '    lblErr.Text = "Codice Tesoreria errato."
                    '    ControlloValiditàIbanTesoreria = False
                    '    Exit Function
                    'End If


                    If clsIban.VerificaLetteraCin(txtIbanTesoreria.Text) = "1" Then
                        lblErr.Text = "Codice Iban errato."
                        ControlloValiditàIbanTesoreria = False
                        Exit Function
                    End If

                    'Funzione che controlla l'autenticità del codice iban indicato
                    Dim ChkCalcolaIban As String = clsIban.CalcolaIBAN(Left(txtIbanTesoreria.Text, 2), Mid(txtIbanTesoreria.Text, 5))
                    If UCase(ChkCalcolaIban) <> UCase(txtIbanTesoreria.Text) Then
                        lblErr.Text = "Codice Iban errato."
                        ControlloValiditàIbanTesoreria = False
                        Exit Function
                    Else
                        'controllo abi e cab non devono corrispondere a queste caratteristiche 
                        'ABI   = 07601 
                        'CAB = 3384
                        If ClsUtility.AbiCab(UCase(txtIbanTesoreria.Text)) Then
                            lblErr.Text = "Il codice iban indicato non fa riferimento ad un conto corrente bancario."
                            ControlloValiditàIbanTesoreria = False
                            Exit Function
                        End If
                    End If
                End If

            Else
                lblErr.Visible = True
                lblErr.Text = "Codice Iban errato."
                ControlloValiditàIbanTesoreria = False
                Exit Function
            End If
        End If
    End Function

    Private Function InsertCronologia() As Integer
        'Modificato il 29/03/2007 da Simona Cordella
        'Aggiunto update in AttivitàFormazioneGenerale per indicare il Rimborso (solo se è SI)
        Dim dtrPrendiId As SqlClient.SqlDataReader
        Dim IdCrono As Integer
        Dim dtPrg As New DataTable
        Dim i As Integer
        Dim bytRimb As Byte
        Dim strNull As String = "NULL"
        '**inserisco la cronologia

        strSql = "Insert into QuestionarioCronologiaStampe (idente,CognomeSottoscritto,NomeSottoscritto,DataDiNascita,IDComuneNascita,"
        strSql = strSql & "  CapComune,NominativoDaContattare,RuoloNominativo,ValoreContributoCifra,ValoreContributoLettere,DataStampa,Username, Iban, BicSwift, IdProvincia, NumeroConto,IbanTesoreria) VALUES "
        strSql = strSql & " ('" & Session("idente") & "','" & txtCognome.Text.Replace("'", "''") & "','" & txtNome.Text.Replace("'", "''") & "', "
        strSql = strSql & " '" & txtDataNascita.Text & "'," & ddlComuneNascita.SelectedValue & ",'" & txtCAPNascita.Text & "','" & txtNominativo.Text.Replace("'", "''") & "', "
        strSql = strSql & " '" & txtPosizione.Text.Replace("'", "''") & "','" & txtImportoEuro.Text.Replace(",", ".") & "','" & txtImportoLettere.Text & "',GetDate(),'" & Session("Utente") & "', "

        If optEntePubblico.Checked = True Then
            'iban e bic a null
            strSql = strSql & "" & strNull & ", "
            strSql = strSql & "" & strNull & ", "
            'provincia e numero conto
            If txtNConto.Text = "" Then
                strSql = strSql & "" & strNull & ", "
                strSql = strSql & "" & strNull & ", "
            Else
                strSql = strSql & "" & ddlProvincia.SelectedValue & ", "
                strSql = strSql & "" & txtNConto.Text & ", "
            End If
            'IBAN TESORERIA
            If txtIbanTesoreria.Text = "" Then
                strSql = strSql & "" & strNull & " "
            Else
                strSql = strSql & "'" & txtIbanTesoreria.Text & "' "
            End If
        End If
        If optAltriEnti.Checked = True Then
            'iban e bic 
            If txtIban.Text = "" Then
                strSql = strSql & "" & strNull & ", "
                strSql = strSql & "" & strNull & ", "
            Else
                strSql = strSql & "'" & txtIban.Text & "', "
                strSql = strSql & "'" & txtBicSwift.Text & "', "
            End If


            'provincia e numero conto Iban Tesoreria a null
            strSql = strSql & "" & strNull & ", "
            strSql = strSql & "" & strNull & ", "
            strSql = strSql & "" & strNull & " "
        End If
        If optAltriEnti.Checked = False And optEntePubblico.Checked = False Then
            strSql = strSql & "" & strNull & ", "
            strSql = strSql & "" & strNull & ", "
            strSql = strSql & "" & strNull & ", "
            strSql = strSql & "" & strNull & ", "
            strSql = strSql & "" & strNull & " "
        End If


        strSql = strSql & ")"


        Dim myCommand As New SqlClient.SqlCommand
        myCommand = New SqlClient.SqlCommand(strSql, Session("conn"))
        myCommand.ExecuteNonQuery()

        'trovo l'id appena inserito
        strSql = "select @@identity as IDMAx"
        'eseguo la query
        myCommand.CommandText = strSql
        dtrPrendiId = myCommand.ExecuteReader
        dtrPrendiId.Read()
        IdCrono = dtrPrendiId("IDMAx")
        dtrPrendiId.Close()
        dtrPrendiId = Nothing
        If optRimborsoSi.Checked = True Then
            bytRimb = 1
        Else
            bytRimb = 0
        End If

        '**inserisco i progetti della cronologia di stampa
        dtPrg = Session("DtbRicVol")
        i = 0
        For i = 0 To dtPrg.Rows.Count - 1
            If dtPrg.Rows(i).Item(7) = 1 Then
                strSql = "Insert into QuestionarioStampaProgetti (IdCronologiastampa,IdAttività) Values (" & IdCrono & "," & dtPrg.Rows(i).Item(5) & ")"
                myCommand = New SqlClient.SqlCommand(strSql, Session("conn"))
                myCommand.ExecuteNonQuery()
                strSql = "UPDATE AttivitàFormazioneGenerale SET ChiedeRimborso = " & bytRimb & "  WHERE IdAttività = " & dtPrg.Rows(i).Item(5) & ""
                myCommand = New SqlClient.SqlCommand(strSql, Session("conn"))
                myCommand.ExecuteNonQuery()
                'Agg. il 23/05/2007 da Simona Cordella  
                'inserisco nella tabella i volontari presenti nel modulo di stampa
                strSql = " INSERT INTO QuestionarioStampaProgettiVolontari (IdStamapProgetti, IdEntità) " & _
                        " (SELECT QuestionarioStampaProgetti.IdStamapProgetti, attivitàentità.IDEntità " & _
                        " FROM  attivitàentità " & _
                        " INNER JOIN attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione " & _
                        " INNER JOIN QuestionarioStampaProgetti ON attivitàentisediattuazione.IDAttività = QuestionarioStampaProgetti.IDAttività " & _
                        " INNER JOIN entità ON attivitàentità.IDEntità = entità.IDEntità " & _
                        " INNER JOIN StatiEntità ON entità.IDStatoEntità = StatiEntità.IDStatoEntità " & _
                        " WHERE (StatiEntità.InServizio=1 OR StatiEntità.Sospeso=1 OR StatiEntità.Chiuso = 1) " & _
                        " AND attivitàentità.EscludiFormazione = 0 " & _
                        " AND QuestionarioStampaProgetti.IdAttività = " & dtPrg.Rows(i).Item(5) & "  " & _
                        " AND QuestionarioStampaProgetti.IdCronologiaStampa=" & IdCrono & " ) "

                myCommand = New SqlClient.SqlCommand(strSql, Session("conn"))
                myCommand.ExecuteNonQuery()
            End If
        Next

        myCommand.Dispose()

        Return IdCrono

    End Function

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.close();" & vbCrLf)
        Response.Write("</script>")
    End Sub

    Private Sub optEntePubblico_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optEntePubblico.CheckedChanged
        If optEntePubblico.Checked = True Then
            ColorDatiAccreditoEnte(Color.White, True, False)
            ColorDatiAccreditoAltriEnte(Color.LightGray, False, True)
        End If
    End Sub

    Private Sub optAltriEnti_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optAltriEnti.CheckedChanged
        If optAltriEnti.Checked = True Then
            ColorDatiAccreditoAltriEnte(Color.White, True, False)
            ColorDatiAccreditoEnte(Color.LightGray, False, True)
        End If
    End Sub

    Private Function controlliSalvataggioServer() As Boolean

        If txtCognome.Text.Trim = String.Empty Then
            lblErr.Visible = True
            lblErr.Text = "Inserire il Cognome."
            txtCognome.Focus()
            Return False
        End If

        If txtNome.Text.Trim = String.Empty Then
            lblErr.Visible = True
            lblErr.Text = "Inserire il Nome."
            txtNome.Focus()
            Return False
        End If

        If ddlComuneNascita.SelectedIndex = 0 Then
            lblErr.Visible = True
            lblErr.Text = "Selezionare il Comune di Nascita."
            txtIDComuneNascita.Value = ""
            ddlComuneNascita.Focus()
            Return False
        End If

        If Session("ModIns") = "2" Then
            If txtIDComuneNascita.Value = String.Empty Then
                lblErr.Visible = True
                lblErr.Text = "Selezionare il Comune di Nascita."
                ddlComuneNascita.Focus()
                Return False
            End If
        End If

        If txtDataNascita.Text.Trim = String.Empty Then
            lblErr.Visible = True
            lblErr.Text = "Inserire la Data di Nascita."
            txtDataNascita.Focus()
            Return False
        End If

        Dim dataNascita As Date
        If (Date.TryParse(txtDataNascita.Text, dataNascita) = False) Then
            lblErr.Visible = True
            lblErr.Text = "Il formato della data è incorretto: il formato deve essere GG/MM/AAAA."
            txtDataNascita.Focus()
            Return False
        End If

        If txtCAPNascita.Text.Trim = String.Empty Then
            lblErr.Visible = True
            lblErr.Text = "Selezionare il CAP del Comune di Nascita."
            txtCAPNascita.Focus()
            txtCAPNascita.Text = String.Empty
            Return False
        End If

        If txtNominativo.Text.Trim = String.Empty Then
            lblErr.Visible = True
            lblErr.Text = "Selezionare il nominativo da contattare."
            txtNominativo.Focus()
            txtNominativo.Text = String.Empty
            Return False
        End If

        If txtPosizione.Text.Trim = String.Empty Then
            lblErr.Visible = True
            lblErr.Text = "Selezionare il ruolo della persona da contattare."
            txtPosizione.Focus()
            txtPosizione.Text = String.Empty
            Return False
        End If

        If txtImportoEuro.Text.Trim = String.Empty Then
            lblErr.Visible = True
            lblErr.Text = "Selezionare l'Importo in cifra da corrispondere."
            txtImportoEuro.Focus()
            txtImportoEuro.Text = String.Empty
            Return False
        End If

        Dim importoEuro As Decimal
        Dim importoEuroDecimal As Boolean
        importoEuroDecimal = Decimal.TryParse(txtImportoEuro.Text.Trim.Replace(".", ","), importoEuro)

        If importoEuroDecimal = False Then
            lblErr.Visible = True
            lblErr.Text = "L'importo Complessivo deve essere un numero."
            txtImportoEuro.Focus()
            Return False
        End If

        If txtImportoLettere.Text.Trim = String.Empty Then
            lblErr.Visible = True
            lblErr.Text = "Selezionare l'Importo in lettere da corrispondere."
            txtImportoLettere.Focus()
            txtImportoLettere.Text = String.Empty
            Return False
        End If

        If optRimborsoSi.Checked = False And optRimborsoNo.Checked = False Then
            lblErr.Visible = True
            lblErr.Text = "E' necessario indicare il rimborso."
            optRimborsoSi.Focus()
            Return False
        End If

        Return True

    End Function

    
    Private Sub ddlProvinciaNascita_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProvinciaNascita.SelectedIndexChanged
        Dim selComune As New clsSelezionaComune
        ddlComuneNascita = selComune.CaricaComuni(ddlComuneNascita, ddlProvinciaNascita.SelectedValue, Session("Conn"))
        ddlComuneNascita.Enabled = True
    End Sub

    Protected Sub ChkEstero_CheckedChanged(sender As Object, e As EventArgs) Handles ChkEstero.CheckedChanged
        Dim selComune As New clsSelezionaComune
        selComune.CaricaProvincie(ddlProvinciaNascita, ChkEstero.Checked, Session("Conn"))
        ddlComuneNascita.Items.Clear()

    End Sub
End Class