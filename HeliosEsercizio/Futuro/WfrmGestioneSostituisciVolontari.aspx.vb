Imports System.IO
Imports System.Drawing

Public Class WfrmGestioneSostituisciVolontari
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim dtsGenerico As DataSet
    Public dtrLeggiDati As SqlClient.SqlDataReader
    Dim MyTransaction As System.Data.SqlClient.SqlTransaction
    Dim dtrgenerico As SqlClient.SqlDataReader
    
    Dim myCommand As System.Data.SqlClient.SqlCommand
#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

#Region "stampe"

    'Private Sub StampaAssegnazioneVolo(ByVal ControlloNomeFile As String)
    '    If Not dtrLeggiDati Is Nothing Then
    '        dtrLeggiDati.Close()
    '        dtrLeggiDati = Nothing
    '    End If
    '    If Not dtrgenerico Is Nothing Then
    '        dtrgenerico.Close()
    '        dtrgenerico = Nothing
    '    End If

    '    If ControlloNomeFile = "SostituzioneVolontariNazionali" Or ControlloNomeFile = "SostituzioneVolontariEstero" Then   'AssegnazioneVolontariNazionali SI TRATTA DELLA LETTERA SUBENTRO
    '        strsql = " Select IndirizzoDomicilio FROM entità WHERE (ISNULL(IndirizzoDomicilio, '') <> '') AND IDEntità = " & txtidentitaSbentrante.Value
    '        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

    '        If dtrLeggiDati.HasRows = False Then
    '            strsql = "select isnull(replace(replace(replace(replace(replace(replace(replace(e.cognome,'°',''),'ì','i'''),'é','e'''), " & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') + ' ' " & _
    '            " + isnull(replace(replace(replace(replace(replace(replace(replace(e.Nome,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Nominativo,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.indirizzo,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as indirizzoRes,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.numerocivico,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as civicoRes,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.cap,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as capRes,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.CodiceFiscale,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as CodFis,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(cRes.denominazione,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as comuneRes,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(pRes.provincia,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as provinciaRes,"
    '            strsql = strsql & "isnull(case len(day(getdate())) when 1 then '0' + " & _
    '            " convert(varchar(20),day(getdate())) else convert(varchar(20),day(getdate())) " & _
    '            " end + '/' + (case len(month(getdate())) when 1 then '0' + convert(varchar(20),month(getdate())) " & _
    '            " else convert(varchar(20),month(getdate()))  end + '/' + Convert(varchar(20), Year(getdate()))),'') as DataOdierna,"
    '            strsql = strsql & "isnull(case len(day(asa.dataUltimoStato)) when 1 then '0' +  convert(varchar(20),day(asa.dataUltimoStato)) " & _
    '            " else convert(varchar(20),day(asa.dataUltimoStato))  end + '/' + (case len(month(asa.dataUltimoStato)) " & _
    '            " when 1 then '0' + convert(varchar(20),month(asa.dataUltimoStato))  " & _
    '            " else convert(varchar(20),month(asa.dataUltimoStato))  end + '/' + Convert(varchar(20), " & _
    '            " Year(asa.dataUltimoStato))),'') as DataApprovazioneGraduatoria,"
    '            strsql = strsql & " isnull(case len(day(e.dataInizioservizio)) when 1 then '0' +  convert(varchar(20),day(e.dataInizioservizio)) " & _
    '            " else convert(varchar(20),day(e.dataInizioservizio))  end + '/' + (case len(month(e.dataInizioservizio)) " & _
    '            " when 1 then '0' + convert(varchar(20),month(e.dataInizioservizio))  " & _
    '            " else convert(varchar(20),month(e.dataInizioservizio))  end + '/' + Convert(varchar(20), " & _
    '            " Year(e.dataInizioservizio))),'') as DataInizioServizio,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.Codicevolontario,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Codicevolontario,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(a.titolo,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Titolo,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(en.denominazione,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Ente,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(en.CodiceRegione,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as NZ,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(es.indirizzo,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as indirizzoSede,"
    '            strsql = strsql & " isnull(replace(replace(replace(replace(replace(replace(replace(es.civico,'°',''),'ì','i''')" & _
    '            ",'é','e'''), 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as civicoSede, "
    '            strsql = strsql & " isnull(replace(replace(replace(replace(replace(replace(replace(es.cap,'°',''),'ì', " & _
    '            " 'i'''),'é','e'''), 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as capSede,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(a.codiceEnte,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as CodiceProgetto,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(cSa.denominazione,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as comuneSa,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(pRes.DescrAbb,'°',''),'ì','i'''),'é','e'''), " & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as DescAbbre, "
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(pSa.provincia,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as provinciaSa, "
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(pSa.DescrAbb,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as DescAbbreSa, "
    '            strsql = strsql & "  isnull(case len(day(a.datafineattività)) when 1 then '0' + " & _
    '            " Convert(varchar(20), Day(a.datafineattività)) " & _
    '            " else convert(varchar(20),day(a.datafineattività))  " & _
    '            " end + '/' + (case len(month(a.datafineattività))  when 1 then '0' " & _
    '            " + convert(varchar(20),month(a.datafineattività))   else convert(varchar(20)," & _
    '            " month(a.datafineattività))  end + '/' + Convert(varchar(20),  Year(a.datafineattività))),'') " & _
    '            " as DatafineProgetto, "
    '            strsql = strsql & "  isnull(case len(day(e.datanascita)) when 1 then '0' + " & _
    '            " Convert(varchar(20), Day(e.datanascita)) " & _
    '            " else convert(varchar(20),day(e.datanascita))  " & _
    '            " end + '/' + (case len(month(e.datanascita))  when 1 then '0' " & _
    '            " + convert(varchar(20),month(e.datanascita))   else convert(varchar(20)," & _
    '            " month(e.datanascita))  end + '/' + Convert(varchar(20),  Year(e.datanascita))),'') " & _
    '            " as DataNascita, "
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(cNas.denominazione,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as comuneNas, "
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(entisedi_1.Indirizzo,'°',''),'ì','i'''),'é','e'''), " & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Indi, "
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(comuni_1.Denominazione,'°',''),'ì','i'''),'é','e'''), " & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Deno, "
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(Provincie.DescrAbb,'°',''),'ì','i'''),'é','e'''), " & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Abbre, "
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(entisedi_1.Civico,'°',''),'ì','i'''),'é','e'''), " & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Civ, "
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(entisedi_1.CAP,'°',''),'ì','i'''),'é','e'''), " & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as CAP, "
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(enti_1.Denominazione,'°',''),'ì','i'''),'é','e'''), " & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Denom, "
    '            strsql = strsql & "isnull(enti_1.CodiceRegione,'')as NZpa,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(Bando.GazzettaUfficiale,'°',''),'ì','i'''),'é','e'''), " & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Gazzetta, "
    '            strsql = strsql & "isnull(bando.Volontari,'')as NVolo,"
    '            strsql = strsql & "isnull(e.Username,'')as Username, "
    '            strsql = strsql & "isnull(e.Password,'')as Password, "
    '            strsql = strsql & "isnull(e.IdSedePrimaAssegnazione,'')as PrimaAss"
    '            strsql = strsql & " from entità e"
    '            strsql = strsql & " inner join comuni cRes on cRes.idcomune=e.idcomuneResidenza"
    '            strsql = strsql & " inner join comuni cNas on cNas.idcomune=e.idcomunenascita"
    '            strsql = strsql & " inner join provincie pRes on pres.idprovincia=cRes.idprovincia"
    '            strsql = strsql & " inner join graduatorieEntità ge on ge.identità=e.idEntità"
    '            strsql = strsql & " inner join AttivitàSediAssegnazione asa on asa.idattivitàSedeassegnazione=ge.idattivitàSedeassegnazione"
    '            strsql = strsql & " inner join attività a on a.idattività=asa.idattività"
    '            strsql = strsql & " inner join Bandiattività on a.idbandoattività=bandiattività.idbandoattività"
    '            strsql = strsql & " inner join bando on bandiattività.idbando=bando.idbando"
    '            strsql = strsql & " inner join entisedi es on es.identesede=asa.identesede"
    '            strsql = strsql & " inner join comuni cSa on cSa.idcomune=es.idcomune"
    '            strsql = strsql & " inner join provincie pSa on pSa.idprovincia=cSa.idprovincia"
    '            strsql = strsql & " inner join Enti en on  en.idente=es.idente"
    '            strsql = strsql & " left Join entisediattuazioni ON e.IdSedePrimaAssegnazione = entisediattuazioni.IDEnteSedeAttuazione"
    '            strsql = strsql & " left JOIN entisedi entisedi_1 ON entisediattuazioni.IDEnteSede = entisedi_1.IDEnteSede"
    '            strsql = strsql & " left join enti enti_1 ON entisedi_1.idente = enti_1.idente"
    '            strsql = strsql & " left JOIN comuni comuni_1 ON entisedi_1.IDComune = comuni_1.IDComune"
    '            strsql = strsql & " left JOIN provincie ON comuni_1.IDProvincia = provincie.IDProvincia"
    '            strsql = strsql & " where e.identità=" & txtidentitaSbentrante.Value & ""
    '        Else
    '            strsql = "select isnull(replace(replace(replace(replace(replace(replace(replace(e.cognome,'°',''),'ì','i'''),'é','e'''), " & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') + ' ' " & _
    '            " + isnull(replace(replace(replace(replace(replace(replace(replace(e.Nome,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Nominativo,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.indirizzodomicilio,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as indirizzoRes,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.numerocivicodomicilio,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as civicoRes,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.capdomicilio,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as capRes,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.CodiceFiscale,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as CodFis,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(cRes.denominazione,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as comuneRes,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(pRes.provincia,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as provinciaRes,"
    '            strsql = strsql & "isnull(case len(day(getdate())) when 1 then '0' + " & _
    '            " convert(varchar(20),day(getdate())) else convert(varchar(20),day(getdate())) " & _
    '            " end + '/' + (case len(month(getdate())) when 1 then '0' + convert(varchar(20),month(getdate())) " & _
    '            " else convert(varchar(20),month(getdate()))  end + '/' + Convert(varchar(20), Year(getdate()))),'') as DataOdierna,"
    '            strsql = strsql & "isnull(case len(day(asa.dataUltimoStato)) when 1 then '0' +  convert(varchar(20),day(asa.dataUltimoStato)) " & _
    '            " else convert(varchar(20),day(asa.dataUltimoStato))  end + '/' + (case len(month(asa.dataUltimoStato)) " & _
    '            " when 1 then '0' + convert(varchar(20),month(asa.dataUltimoStato))  " & _
    '            " else convert(varchar(20),month(asa.dataUltimoStato))  end + '/' + Convert(varchar(20), " & _
    '            " Year(asa.dataUltimoStato))),'') as DataApprovazioneGraduatoria,"
    '            strsql = strsql & " isnull(case len(day(e.dataInizioservizio)) when 1 then '0' +  convert(varchar(20),day(e.dataInizioservizio)) " & _
    '            " else convert(varchar(20),day(e.dataInizioservizio))  end + '/' + (case len(month(e.dataInizioservizio)) " & _
    '            " when 1 then '0' + convert(varchar(20),month(e.dataInizioservizio))  " & _
    '            " else convert(varchar(20),month(e.dataInizioservizio))  end + '/' + Convert(varchar(20), " & _
    '            " Year(e.dataInizioservizio))),'') as DataInizioServizio,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.Codicevolontario,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Codicevolontario,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(a.titolo,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Titolo,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(en.denominazione,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Ente,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(en.CodiceRegione,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as NZ,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(es.indirizzo,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as indirizzoSede,"
    '            strsql = strsql & " isnull(replace(replace(replace(replace(replace(replace(replace(es.civico,'°',''),'ì','i''')" & _
    '            ",'é','e'''), 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as civicoSede, "
    '            strsql = strsql & " isnull(replace(replace(replace(replace(replace(replace(replace(es.cap,'°',''),'ì', " & _
    '            " 'i'''),'é','e'''), 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as capSede,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(a.codiceEnte,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as CodiceProgetto,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(cSa.denominazione,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as comuneSa,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(pRes.DescrAbb,'°',''),'ì','i'''),'é','e'''), " & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as DescAbbre, "
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(pSa.provincia,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as provinciaSa, "
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(pSa.DescrAbb,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as DescAbbreSa, "
    '            strsql = strsql & "  isnull(case len(day(a.datafineattività)) when 1 then '0' + " & _
    '            " Convert(varchar(20), Day(a.datafineattività)) " & _
    '            " else convert(varchar(20),day(a.datafineattività))  " & _
    '            " end + '/' + (case len(month(a.datafineattività))  when 1 then '0' " & _
    '            " + convert(varchar(20),month(a.datafineattività))   else convert(varchar(20)," & _
    '            " month(a.datafineattività))  end + '/' + Convert(varchar(20),  Year(a.datafineattività))),'') " & _
    '            " as DatafineProgetto, "
    '            strsql = strsql & "  isnull(case len(day(e.datanascita)) when 1 then '0' + " & _
    '            " Convert(varchar(20), Day(e.datanascita)) " & _
    '            " else convert(varchar(20),day(e.datanascita))  " & _
    '            " end + '/' + (case len(month(e.datanascita))  when 1 then '0' " & _
    '            " + convert(varchar(20),month(e.datanascita))   else convert(varchar(20)," & _
    '            " month(e.datanascita))  end + '/' + Convert(varchar(20),  Year(e.datanascita))),'') " & _
    '            " as DataNascita, "
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(cNas.denominazione,'°',''),'ì','i'''),'é','e''')," & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as comuneNas, "
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(entisedi_1.Indirizzo,'°',''),'ì','i'''),'é','e'''), " & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Indi, "
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(comuni_1.Denominazione,'°',''),'ì','i'''),'é','e'''), " & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Deno, "
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(Provincie.DescrAbb,'°',''),'ì','i'''),'é','e'''), " & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Abbre, "
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(entisedi_1.Civico,'°',''),'ì','i'''),'é','e'''), " & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Civ, "
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(entisedi_1.CAP,'°',''),'ì','i'''),'é','e'''), " & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as CAP, "
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(enti_1.Denominazione,'°',''),'ì','i'''),'é','e'''), " & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Denom, "
    '            strsql = strsql & "isnull(enti_1.CodiceRegione,'')as NZpa,"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(Bando.GazzettaUfficiale,'°',''),'ì','i'''),'é','e'''), " & _
    '            " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Gazzetta, "
    '            strsql = strsql & "isnull(bando.Volontari,'')as NVolo,"
    '            strsql = strsql & "isnull(e.Username,'')as Username, "
    '            strsql = strsql & "isnull(e.Password,'')as Password, "
    '            strsql = strsql & "isnull(e.IdSedePrimaAssegnazione,'')as PrimaAss"
    '            strsql = strsql & " from entità e"
    '            strsql = strsql & " inner join comuni cRes on cRes.idcomune=e.idcomunedomicilio"
    '            strsql = strsql & " inner join comuni cNas on cNas.idcomune=e.idcomunenascita"
    '            strsql = strsql & " inner join provincie pRes on pres.idprovincia=cRes.idprovincia"
    '            strsql = strsql & " inner join graduatorieEntità ge on ge.identità=e.idEntità"
    '            strsql = strsql & " inner join AttivitàSediAssegnazione asa on asa.idattivitàSedeassegnazione=ge.idattivitàSedeassegnazione"
    '            strsql = strsql & " inner join attività a on a.idattività=asa.idattività"
    '            strsql = strsql & " inner join Bandiattività on a.idbandoattività=bandiattività.idbandoattività"
    '            strsql = strsql & " inner join bando on bandiattività.idbando=bando.idbando"
    '            strsql = strsql & " inner join entisedi es on es.identesede=asa.identesede"
    '            strsql = strsql & " inner join comuni cSa on cSa.idcomune=es.idcomune"
    '            strsql = strsql & " inner join provincie pSa on pSa.idprovincia=cSa.idprovincia"
    '            strsql = strsql & " inner join Enti en on  en.idente=es.idente"
    '            strsql = strsql & " left Join entisediattuazioni ON e.IdSedePrimaAssegnazione = entisediattuazioni.IDEnteSedeAttuazione"
    '            strsql = strsql & " left JOIN entisedi entisedi_1 ON entisediattuazioni.IDEnteSede = entisedi_1.IDEnteSede"
    '            strsql = strsql & " left join enti enti_1 ON entisedi_1.idente = enti_1.idente"
    '            strsql = strsql & " left JOIN comuni comuni_1 ON entisedi_1.IDComune = comuni_1.IDComune"
    '            strsql = strsql & " left JOIN provincie ON comuni_1.IDProvincia = provincie.IDProvincia"
    '            strsql = strsql & " where e.identità=" & txtidentitaSbentrante.Value & ""
    '        End If
    '    Else
    '        strsql = "select isnull(replace(replace(replace(replace(replace(replace(replace(e.cognome,'°',''),'ì','i'''),'é','e'''), " & _
    '                        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') + ' ' " & _
    '                        " + isnull(replace(replace(replace(replace(replace(replace(replace(e.Nome,'°',''),'ì','i'''),'é','e''')," & _
    '                        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Nominativo,"
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.indirizzo,'°',''),'ì','i'''),'é','e''')," & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as indirizzoRes,"
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.numerocivico,'°',''),'ì','i'''),'é','e''')," & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as civicoRes,"
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.cap,'°',''),'ì','i'''),'é','e''')," & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as capRes,"
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.CodiceFiscale,'°',''),'ì','i'''),'é','e''')," & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as CodFis,"
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(cRes.denominazione,'°',''),'ì','i'''),'é','e''')," & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as comuneRes,"
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(pRes.provincia,'°',''),'ì','i'''),'é','e''')," & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as provinciaRes,"
    '        strsql = strsql & "isnull(case len(day(getdate())) when 1 then '0' + " & _
    '        " convert(varchar(20),day(getdate())) else convert(varchar(20),day(getdate())) " & _
    '        " end + '/' + (case len(month(getdate())) when 1 then '0' + convert(varchar(20),month(getdate())) " & _
    '        " else convert(varchar(20),month(getdate()))  end + '/' + Convert(varchar(20), Year(getdate()))),'') as DataOdierna,"
    '        strsql = strsql & "isnull(case len(day(asa.dataUltimoStato)) when 1 then '0' +  convert(varchar(20),day(asa.dataUltimoStato)) " & _
    '        " else convert(varchar(20),day(asa.dataUltimoStato))  end + '/' + (case len(month(asa.dataUltimoStato)) " & _
    '        " when 1 then '0' + convert(varchar(20),month(asa.dataUltimoStato))  " & _
    '        " else convert(varchar(20),month(asa.dataUltimoStato))  end + '/' + Convert(varchar(20), " & _
    '        " Year(asa.dataUltimoStato))),'') as DataApprovazioneGraduatoria,"
    '        strsql = strsql & " isnull(case len(day(e.dataInizioservizio)) when 1 then '0' +  convert(varchar(20),day(e.dataInizioservizio)) " & _
    '        " else convert(varchar(20),day(e.dataInizioservizio))  end + '/' + (case len(month(e.dataInizioservizio)) " & _
    '        " when 1 then '0' + convert(varchar(20),month(e.dataInizioservizio))  " & _
    '        " else convert(varchar(20),month(e.dataInizioservizio))  end + '/' + Convert(varchar(20), " & _
    '        " Year(e.dataInizioservizio))),'') as DataInizioServizio,"
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.Codicevolontario,'°',''),'ì','i'''),'é','e''')," & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Codicevolontario,"
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(a.titolo,'°',''),'ì','i'''),'é','e''')," & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Titolo,"
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(en.denominazione,'°',''),'ì','i'''),'é','e''')," & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Ente,"
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(en.CodiceRegione,'°',''),'ì','i'''),'é','e''')," & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as NZ,"
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(es.indirizzo,'°',''),'ì','i'''),'é','e''')," & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as indirizzoSede,"
    '        strsql = strsql & " isnull(replace(replace(replace(replace(replace(replace(replace(es.civico,'°',''),'ì','i''')" & _
    '        ",'é','e'''), 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as civicoSede, "
    '        strsql = strsql & " isnull(replace(replace(replace(replace(replace(replace(replace(es.cap,'°',''),'ì', " & _
    '        " 'i'''),'é','e'''), 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as capSede,"
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(a.codiceEnte,'°',''),'ì','i'''),'é','e''')," & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as CodiceProgetto,"
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(cSa.denominazione,'°',''),'ì','i'''),'é','e''')," & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as comuneSa,"
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(pRes.DescrAbb,'°',''),'ì','i'''),'é','e'''), " & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as DescAbbre, "
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(pSa.provincia,'°',''),'ì','i'''),'é','e''')," & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as provinciaSa, "
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(pSa.DescrAbb,'°',''),'ì','i'''),'é','e''')," & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as DescAbbreSa, "
    '        strsql = strsql & "  isnull(case len(day(a.datafineattività)) when 1 then '0' + " & _
    '        " Convert(varchar(20), Day(a.datafineattività)) " & _
    '        " else convert(varchar(20),day(a.datafineattività))  " & _
    '        " end + '/' + (case len(month(a.datafineattività))  when 1 then '0' " & _
    '        " + convert(varchar(20),month(a.datafineattività))   else convert(varchar(20)," & _
    '        " month(a.datafineattività))  end + '/' + Convert(varchar(20),  Year(a.datafineattività))),'') " & _
    '        " as DatafineProgetto, "
    '        strsql = strsql & "  isnull(case len(day(e.datanascita)) when 1 then '0' + " & _
    '        " Convert(varchar(20), Day(e.datanascita)) " & _
    '        " else convert(varchar(20),day(e.datanascita))  " & _
    '        " end + '/' + (case len(month(e.datanascita))  when 1 then '0' " & _
    '        " + convert(varchar(20),month(e.datanascita))   else convert(varchar(20)," & _
    '        " month(e.datanascita))  end + '/' + Convert(varchar(20),  Year(e.datanascita))),'') " & _
    '        " as DataNascita, "
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(cNas.denominazione,'°',''),'ì','i'''),'é','e''')," & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as comuneNas, "
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(entisedi_1.Indirizzo,'°',''),'ì','i'''),'é','e'''), " & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Indi, "
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(comuni_1.Denominazione,'°',''),'ì','i'''),'é','e'''), " & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Deno, "
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(Provincie.DescrAbb,'°',''),'ì','i'''),'é','e'''), " & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Abbre, "
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(entisedi_1.Civico,'°',''),'ì','i'''),'é','e'''), " & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Civ, "
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(entisedi_1.CAP,'°',''),'ì','i'''),'é','e'''), " & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as CAP, "
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(enti_1.Denominazione,'°',''),'ì','i'''),'é','e'''), " & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Denom, "
    '        strsql = strsql & "isnull(enti_1.CodiceRegione,'')as NZpa,"
    '        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(Bando.GazzettaUfficiale,'°',''),'ì','i'''),'é','e'''), " & _
    '        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Gazzetta, "
    '        strsql = strsql & "isnull(bando.Volontari,'')as NVolo,"
    '        strsql = strsql & "isnull(e.Username,'')as Username, "
    '        strsql = strsql & "isnull(e.Password,'')as Password, "
    '        strsql = strsql & "isnull(e.IdSedePrimaAssegnazione,'')as PrimaAss"
    '        strsql = strsql & " from entità e"
    '        strsql = strsql & " inner join comuni cRes on cRes.idcomune=e.idcomuneResidenza"
    '        strsql = strsql & " inner join comuni cNas on cNas.idcomune=e.idcomunenascita"
    '        strsql = strsql & " inner join provincie pRes on pres.idprovincia=cRes.idprovincia"
    '        strsql = strsql & " inner join graduatorieEntità ge on ge.identità=e.idEntità"
    '        strsql = strsql & " inner join AttivitàSediAssegnazione asa on asa.idattivitàSedeassegnazione=ge.idattivitàSedeassegnazione"
    '        strsql = strsql & " inner join attività a on a.idattività=asa.idattività"
    '        strsql = strsql & " inner join Bandiattività on a.idbandoattività=bandiattività.idbandoattività"
    '        strsql = strsql & " inner join bando on bandiattività.idbando=bando.idbando"
    '        strsql = strsql & " inner join entisedi es on es.identesede=asa.identesede"
    '        strsql = strsql & " inner join comuni cSa on cSa.idcomune=es.idcomune"
    '        strsql = strsql & " inner join provincie pSa on pSa.idprovincia=cSa.idprovincia"
    '        strsql = strsql & " inner join Enti en on  en.idente=es.idente"
    '        strsql = strsql & " left Join entisediattuazioni ON e.IdSedePrimaAssegnazione = entisediattuazioni.IDEnteSedeAttuazione"
    '        strsql = strsql & " left JOIN entisedi entisedi_1 ON entisediattuazioni.IDEnteSede = entisedi_1.IDEnteSede"
    '        strsql = strsql & " left join enti enti_1 ON entisedi_1.idente = enti_1.idente"
    '        strsql = strsql & " left JOIN comuni comuni_1 ON entisedi_1.IDComune = comuni_1.IDComune"
    '        strsql = strsql & " left JOIN provincie ON comuni_1.IDProvincia = provincie.IDProvincia"
    '        strsql = strsql & " where e.identità=" & txtidentitaSbentrante.Value & ""

    '    End If

    '    If Not dtrLeggiDati Is Nothing Then
    '        dtrLeggiDati.Close()
    '        dtrLeggiDati = Nothing
    '    End If
    '    If Not dtrgenerico Is Nothing Then
    '        dtrgenerico.Close()
    '        dtrgenerico = Nothing
    '    End If
    '    dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
    'End Sub
    'Function LetteraAssegnazioneVOlontario(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
    '    'Implementazione stampa generata da Alessandra Taballione il 16/06/2005
    '    'Determina Adeguamento Positivo 
    '    Dim xStr As String
    '    Dim xLinea As String
    '    Dim Writer As StreamWriter
    '    Dim Reader As StreamReader
    '    Dim strPercorsoFile As String
    '    Dim strsql As String
    '    Dim strDataOdierna As String
    '    Dim strNomeFile As String
    '    Dim passworddecript As String
    '    Try
    '        'chiudo il datareader
    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If



    '        'parla da sola
    '        StampaAssegnazioneVolo(NomeFile)
    '        If dtrLeggiDati.HasRows = True Then
    '            dtrLeggiDati.Read()

    '            passworddecript = dtrLeggiDati("Password")
    '            If passworddecript <> "" Then passworddecript = ClsUtility.ReadPsw(passworddecript)
    '            'creo il nome del file
    '            strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
    '            'creo il percorso del file da salvare
    '            strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

    '            'apro il file che fa da template
    '            Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"))
    '            Writer = New StreamWriter(strPercorsoFile)

    '            'Writer.WriteLine("{\rtf1")

    '            'Write the page header and footer
    '            'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
    '            '               "AS04\par}}")
    '            'apro il template
    '            xLinea = Reader.ReadLine()
    '            While xLinea <> ""
    '                xLinea = Replace(xLinea, "<Nominativo>", dtrLeggiDati("Nominativo"))

    '                'DATI RESIDENZA O DOMICILIO A SACONDA DELLE CONDIZIONI GESTITE NELLA COSTRUZIONE DELLA QUERY
    '                xLinea = Replace(xLinea, "<IndirizzoRes>", dtrLeggiDati("indirizzoRes"))
    '                xLinea = Replace(xLinea, "<CivicoRes>", dtrLeggiDati("civicoRes"))
    '                xLinea = Replace(xLinea, "<ComuneRes>", dtrLeggiDati("comuneRes"))
    '                xLinea = Replace(xLinea, "<CapRes>", dtrLeggiDati("capRes"))
    '                xLinea = Replace(xLinea, "<ProvinciaRes>", "(" & dtrLeggiDati("provinciaRes") & ")")
    '                'FINE DATI RESIDENZA O DOMICILIO 
    '                xLinea = Replace(xLinea, "<ComuneNas>", dtrLeggiDati("comuneNas"))
    '                xLinea = Replace(xLinea, "<DataOdierna>", dtrLeggiDati("DataOdierna"))
    '                xLinea = Replace(xLinea, "<CodFis>", dtrLeggiDati("CodFis"))
    '                xLinea = Replace(xLinea, "<CodiceVolontario>", dtrLeggiDati("Codicevolontario"))
    '                xLinea = Replace(xLinea, "<Titolo>", dtrLeggiDati("Titolo"))
    '                xLinea = Replace(xLinea, "<Ente>", dtrLeggiDati("Ente"))
    '                xLinea = Replace(xLinea, "<dataFineProgetto>", dtrLeggiDati("dataFineProgetto"))
    '                xLinea = Replace(xLinea, "<dataNascita>", dtrLeggiDati("dataNascita"))
    '                xLinea = Replace(xLinea, "<IndirizzoSede>", dtrLeggiDati("indirizzoSede"))
    '                xLinea = Replace(xLinea, "<CivicoSede>", dtrLeggiDati("CivicoSede"))
    '                xLinea = Replace(xLinea, "<CapSede>", dtrLeggiDati("CapSede"))
    '                xLinea = Replace(xLinea, "<ProvinciaSede>", "(" & dtrLeggiDati("provinciaSa") & ")")
    '                'xLinea = Replace(xLinea, "<DescAbbre>", dtrLeggiDati("DescAbbre"))
    '                xLinea = Replace(xLinea, "<DescAbbreSa>", dtrLeggiDati("DescAbbreSa"))
    '                xLinea = Replace(xLinea, "<ComuneSede>", dtrLeggiDati("comuneSa"))
    '                xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
    '                xLinea = Replace(xLinea, "<Abbre>", dtrLeggiDati("Abbre"))
    '                xLinea = Replace(xLinea, "<Indi>", dtrLeggiDati("Indi"))
    '                xLinea = Replace(xLinea, "<Civ>", dtrLeggiDati("Civ"))
    '                xLinea = Replace(xLinea, "<Deno>", dtrLeggiDati("Deno"))
    '                xLinea = Replace(xLinea, "<Denom>", dtrLeggiDati("Denom"))
    '                xLinea = Replace(xLinea, "<NZ>", dtrLeggiDati("NZ"))
    '                xLinea = Replace(xLinea, "<NZpa>", dtrLeggiDati("NZpa"))
    '                xLinea = Replace(xLinea, "<Nvolo>", dtrLeggiDati("Nvolo"))
    '                xLinea = Replace(xLinea, "<Gazzetta>", dtrLeggiDati("Gazzetta"))
    '                xLinea = Replace(xLinea, "<CodiceProgetto>", dtrLeggiDati("CodiceProgetto"))
    '                xLinea = Replace(xLinea, "<DataApprovazioneGraduatoria>", dtrLeggiDati("DataApprovazioneGraduatoria"))
    '                xLinea = Replace(xLinea, "<DataInizioServizio>", dtrLeggiDati("DataInizioServizio"))
    '                xLinea = Replace(xLinea, "<GiorniP>", Session("intGGP"))
    '                xLinea = Replace(xLinea, "<GiorniR>", Session("intGGR"))
    '                xLinea = Replace(xLinea, "_", " ") 'DataApprovazioneGraduatoria,DataInizioServizio
    '                xLinea = Replace(xLinea, "<Username>", dtrLeggiDati("UserName"))
    '                xLinea = Replace(xLinea, "<Password>", passworddecript)


    '                Writer.WriteLine(xLinea)

    '                xLinea = Reader.ReadLine()
    '            End While
    '            'close the RTF string and file
    '            'Writer.WriteLine("}")
    '            Writer.Close()
    '            Writer = Nothing

    '            ''chiudo lo streaming in scrittura
    '            Reader.Close()
    '            Reader = Nothing
    '        End If
    '        'chiudo il datareader
    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If
    '        LetteraAssegnazioneVOlontario = "documentazione/" & strNomeFile
    '    Catch ex As Exception
    '        Response.Write(ex.Message)
    '    End Try
    'End Function

    'Function LetteraChiusuraIniziale(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
    '    'Implementazione stampa generata da Amilcare Paolella il 12/10/2005

    '    Dim xStr As String
    '    Dim xLinea As String
    '    Dim Writer As StreamWriter
    '    Dim Reader As StreamReader
    '    Dim strPercorsoFile As String
    '    Dim strsql As String
    '    Dim strDataOdierna As String
    '    Dim strNomeFile As String

    '    Try
    '        'chiudo il datareader
    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If

    '        'parla da sola
    '        CaricaDati()
    '        dtrLeggiDati.Read()
    '        Session("EnteSP") = dtrLeggiDati("Denominazione")
    '        Session("indirizzoSedeSP") = dtrLeggiDati("Indirizzo")
    '        Session("CivicoSedeSP") = dtrLeggiDati("Civico")
    '        Session("CapSedeSP") = dtrLeggiDati("CAP")
    '        Session("provinciaSaSP") = dtrLeggiDati("Provincia")
    '        Session("comuneSaSP") = dtrLeggiDati("Comune")


    '        RecuperoInfoVecchioVolontario()
    '        If dtrLeggiDati.HasRows = True Then
    '            dtrLeggiDati.Read()
    '            'creo il nome del file
    '            strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
    '            'creo il percorso del file da salvare
    '            strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

    '            'apro il file che fa da template
    '            Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"))
    '            Writer = New StreamWriter(strPercorsoFile)

    '            'Writer.WriteLine("{\rtf1")

    '            'Write the page header and footer
    '            'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
    '            '               "AS04\par}}")
    '            'apro il template
    '            xLinea = Reader.ReadLine()
    '            While xLinea <> ""
    '                xLinea = Replace(xLinea, "<Nominativo>", lblCognome.Text & " " & lblNome.Text)
    '                xLinea = Replace(xLinea, "<IndirizzoVol>", Session("IndirizzoVol"))
    '                xLinea = Replace(xLinea, "<NumeroCivicoVol>", Session("NumeroCivicoVol"))
    '                xLinea = Replace(xLinea, "<ComuneVol>", lblComuneNascita.Text)
    '                xLinea = Replace(xLinea, "<CAPVol>", Session("CapVol"))
    '                xLinea = Replace(xLinea, "<ProvinciaVol>", Session("ProvinciaVol"))
    '                xLinea = Replace(xLinea, "<CodiceVolontario>", dtrLeggiDati("Codicevolontario"))
    '                xLinea = Replace(xLinea, "<DenominazioneEnte>", Session("EnteSP"))
    '                xLinea = Replace(xLinea, "<Indirizzo>", Session("indirizzoSedeSP"))
    '                xLinea = Replace(xLinea, "<NumeroCivico>", Session("CivicoSedeSP"))
    '                xLinea = Replace(xLinea, "<CAP>", Session("CapSedeSP"))
    '                xLinea = Replace(xLinea, "<Provincia>", Session("provinciaSaSP"))
    '                xLinea = Replace(xLinea, "<Comune>", Session("comuneSaSP"))
    '                xLinea = Replace(xLinea, "<CodiceEnte>", dtrLeggiDati("CodiceProgetto"))
    '                xLinea = Replace(xLinea, "<DataInizioProgetto>", dtrLeggiDati("dataInizioProgetto"))
    '                'Aggiunto da Alesandra TAballione il 01/11/2005
    '                xLinea = Replace(xLinea, "<ComuneRes>", dtrLeggiDati("ComuneRes"))
    '                xLinea = Replace(xLinea, "<ProvinciaRes>", dtrLeggiDati("ProvinciaRes"))
    '                xLinea = Replace(xLinea, "<IndirizzoRes>", dtrLeggiDati("IndirizzoRes"))
    '                xLinea = Replace(xLinea, "<Civicores>", dtrLeggiDati("Civicores"))
    '                xLinea = Replace(xLinea, "<CapRes>", dtrLeggiDati("CapRes"))
    '                '***************
    '                xLinea = Replace(xLinea, "_", " ")

    '                Writer.WriteLine(xLinea)

    '                xLinea = Reader.ReadLine()
    '            End While
    '            'close the RTF string and file
    '            'Writer.WriteLine("}")
    '            Writer.Close()
    '            Writer = Nothing

    '            ''chiudo lo streaming in scrittura
    '            Reader.Close()
    '            Reader = Nothing
    '        End If
    '        'chiudo il datareader
    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If
    '        LetteraChiusuraIniziale = "documentazione/" & strNomeFile
    '    Catch ex As Exception
    '        Response.Write(ex.Message)
    '    End Try
    'End Function

    'Function LetteraChiusuraInServizio(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
    '    'Implementazione stampa generata da Amilcare Paolella il 12/10/2005

    '    Dim xStr As String
    '    Dim xLinea As String
    '    Dim Writer As StreamWriter
    '    Dim Reader As StreamReader
    '    Dim strPercorsoFile As String
    '    Dim strsql As String
    '    Dim strDataOdierna As String
    '    Dim strNomeFile As String

    '    Try
    '        'chiudo il datareader
    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If

    '        CaricaDati()
    '        dtrLeggiDati.Read()
    '        Session("EnteSP") = dtrLeggiDati("Denominazione")
    '        Session("indirizzoSedeSP") = dtrLeggiDati("Indirizzo")
    '        Session("CivicoSedeSP") = dtrLeggiDati("Civico")
    '        Session("CapSedeSP") = dtrLeggiDati("CAP")
    '        Session("provinciaSaSP") = dtrLeggiDati("Provincia")
    '        Session("comuneSaSP") = dtrLeggiDati("Comune")

    '        RecuperoInfoVecchioVolontario()
    '        If dtrLeggiDati.HasRows = True Then
    '            dtrLeggiDati.Read()
    '            'creo il nome del file
    '            strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
    '            'creo il percorso del file da salvare
    '            strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

    '            'apro il file che fa da template
    '            Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"))
    '            Writer = New StreamWriter(strPercorsoFile)

    '            'Writer.WriteLine("{\rtf1")

    '            'Write the page header and footer
    '            'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
    '            '               "AS04\par}}")
    '            'apro il template
    '            xLinea = Reader.ReadLine()
    '            While xLinea <> ""

    '                xLinea = Replace(xLinea, "<Nominativo>", lblCognome.Text & " " & lblNome.Text)
    '                xLinea = Replace(xLinea, "<IndirizzoVol>", Session("IndirizzoVol"))
    '                xLinea = Replace(xLinea, "<NumeroCivicoVol>", Session("NumeroCivicoVol"))
    '                xLinea = Replace(xLinea, "<ComuneVol>", lblComuneNascita.Text)
    '                xLinea = Replace(xLinea, "<CAPVol>", Session("CapVol"))
    '                xLinea = Replace(xLinea, "<ProvinciaVol>", Session("ProvinciaVol"))
    '                xLinea = Replace(xLinea, "<CodiceVolontario>", dtrLeggiDati("Codicevolontario"))
    '                xLinea = Replace(xLinea, "<DenominazioneEnte>", Session("EnteSP"))
    '                xLinea = Replace(xLinea, "<Indirizzo>", Session("indirizzoSedeSP"))
    '                xLinea = Replace(xLinea, "<NumeroCivico>", Session("CivicoSedeSP"))
    '                xLinea = Replace(xLinea, "<CAP>", Session("CapSedeSP"))
    '                xLinea = Replace(xLinea, "<Provincia>", Session("provinciaSaSP"))
    '                xLinea = Replace(xLinea, "<Comune>", Session("comuneSaSP"))
    '                xLinea = Replace(xLinea, "<DataInizioServizioVol>", dtrLeggiDati("DataInizioServizio"))
    '                xLinea = Replace(xLinea, "<DataFineServizioVol>", Session("DataFineServizioVol"))
    '                'Aggiunto da Alesandra TAballione il 01/11/2005
    '                xLinea = Replace(xLinea, "<ComuneRes>", dtrLeggiDati("ComuneRes"))
    '                xLinea = Replace(xLinea, "<ProvinciaRes>", dtrLeggiDati("ProvinciaRes"))
    '                xLinea = Replace(xLinea, "<IndirizzoRes>", dtrLeggiDati("IndirizzoRes"))
    '                xLinea = Replace(xLinea, "<Civicores>", dtrLeggiDati("Civicores"))
    '                xLinea = Replace(xLinea, "<CapRes>", dtrLeggiDati("CapRes"))
    '                '***************
    '                xLinea = Replace(xLinea, "_", " ")

    '                Writer.WriteLine(xLinea)

    '                xLinea = Reader.ReadLine()
    '            End While
    '            'close the RTF string and file
    '            'Writer.WriteLine("}")
    '            Writer.Close()
    '            Writer = Nothing

    '            ''chiudo lo streaming in scrittura
    '            Reader.Close()
    '            Reader = Nothing
    '        End If
    '        'chiudo il datareader
    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If
    '        LetteraChiusuraInServizio = "documentazione/" & strNomeFile
    '    Catch ex As Exception
    '        Response.Write(ex.Message)
    '    End Try
    'End Function
    'Function Rinunciaserviziovolontario(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String

    '    'Implementazione stampa generata da Alessandra Taballione il 16/06/2005
    '    'Lettera Adeguamento Positivo Negativo
    '    Dim xStr As String
    '    Dim xLinea As String
    '    Dim Writer As StreamWriter
    '    Dim Reader As StreamReader
    '    Dim strPercorsoFile As String
    '    Dim strsql As String
    '    Dim strDataOdierna As String
    '    Dim strNomeFile As String

    '    Try
    '        'chiudo il datareader
    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If

    '        'prendo la data dal server
    '        dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
    '        dtrLeggiDati.Read()
    '        'passo la data odierna ad una variabile locale
    '        strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))



    '        'parla da sola
    '        'chiudo il datareader
    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If

    '        Dim x As New clsDocumentiRegioni
    '        x.RecuperaDati(txtIdentitaVecchia.Value, Session("conn"))



    '        CaricaDati()

    '        If dtrLeggiDati.HasRows = True Then
    '            dtrLeggiDati.Read()
    '            'creo il nome del file
    '            strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
    '            'creo il percorso del file da salvare
    '            strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

    '            'apro il file che fa da template
    '            Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"))
    '            Writer = New StreamWriter(strPercorsoFile)

    '            'Writer.WriteLine("{\rtf1")

    '            'Write the page header and footer
    '            'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
    '            '"AS02\par}}")
    '            'apro il template
    '            xLinea = Reader.ReadLine()

    '            While xLinea <> ""

    '                xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
    '                xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
    '                xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta") & "^")
    '                xLinea = Replace(xLinea, "<DataCostituzione>", dtrLeggiDati("DataCostituzione"))
    '                xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
    '                xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
    '                xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
    '                xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
    '                '
    '                xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
    '                xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
    '                xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta"))
    '                xLinea = Replace(xLinea, "<DataRinuncia>", txtdataAvvio.Text)
    '                xLinea = Replace(xLinea, "<NomeVolontario>", lblCognome.Text & " " & lblNome.Text)
    '                xLinea = Replace(xLinea, "<NomeVolontarioSostituito>", txtVolSubentarata.Value)
    '                xLinea = Replace(xLinea, "<CodiceVolontarioVecchio>", "(" & txtCodiceVolontarioVecchio.Value & ")")
    '                xLinea = Replace(xLinea, "<CodiceVolontarioSubentrante>", "(" & txtCodiceVolontarioSubentrante.Value & ")")

    '                xLinea = Replace(xLinea, "<Gazzetta>", x.Gazzetta)
    '                xLinea = Replace(xLinea, "<NVol>", x.NVolontari)
    '                xLinea = Replace(xLinea, "<IntestazioneRegione>", x.Intestazione)
    '                xLinea = Replace(xLinea, "<SettoreRegione>", x.Settore)
    '                xLinea = Replace(xLinea, "<IndirizzoRegione>", x.Indirizzo)
    '                xLinea = Replace(xLinea, "<CapRegione>", x.Cap)
    '                xLinea = Replace(xLinea, "<LocalitaRegione>", x.Località)
    '                ''Write a sentence in the first paragraph of the document
    '                'Writer.WriteLine("\par\fs24\cf2 This is a sample \b RTF \b0 " & _
    '                '                 "document created with ASP.\cf0")



    '                Writer.WriteLine(xLinea)

    '                xLinea = Reader.ReadLine()
    '            End While


    '            'Writer.WriteLine("}") 'End Table

    '            'Add a page break and then a new paragraph
    '            'Writer.WriteLine("\par \page")
    '            'Writer.WriteLine("\pard\fs18\cf2\qc " & _
    '            '                 "This sample provided by Microsoft Developer Support.")

    '            'close the RTF string and file
    '            'Writer.WriteLine("}")
    '            Writer.Close()
    '            Writer = Nothing

    '            ''chiudo lo streaming in scrittura
    '            Reader.Close()
    '            Reader = Nothing

    '        End If

    '        'chiudo il datareader
    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If

    '        Rinunciaserviziovolontario = "documentazione/" & strNomeFile

    '        'vado a fare la insert
    '        Dim cmdinsert As Data.SqlClient.SqlCommand
    '        strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
    '        strsql = strsql & "values "
    '        strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',2)"
    '        cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
    '        cmdinsert.ExecuteNonQuery()

    '        cmdinsert.Dispose()

    '    Catch ex As Exception
    '        Response.Write(ex.Message)
    '    End Try

    'End Function
    'Function Elencovolontariammessi(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String

    '    Dim xStr As String
    '    Dim xLinea As String
    '    Dim Writer As StreamWriter
    '    Dim Reader As StreamReader
    '    Dim strPercorsoFile As String
    '    Dim strsql As String
    '    Dim strDataOdierna As String
    '    Dim strNomeFile As String
    '    Dim dtrLeggiDati As SqlClient.SqlDataReader
    '    Dim arrayIdVol As New ArrayList

    '    Try
    '        'chiudo il datareader
    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If

    '        'prendo la data dal server
    '        dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
    '        dtrLeggiDati.Read()
    '        'passo la data odierna ad una variabile locale
    '        strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

    '        'creo il nome del file
    '        strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
    '        'creo il percorso del file da salvare
    '        strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

    '        'apro il file che fa da template
    '        Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"))

    '        Writer = New StreamWriter(strPercorsoFile)

    '        'chiudo il datareader
    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If


    '        strsql = "SELECT DISTINCT "
    '        strsql = strsql & " isnull(upper(replace(replace(replace(replace(replace(replace(replace(attività.Titolo,'°',''),'ì','i'''),'é','e'''),"
    '        strsql = strsql & "'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '')as Titolo,"
    '        strsql = strsql & " isnull(attività.CodiceEnte, 'Nessun Codice') as codiceprogetto, "
    '        strsql = strsql & "isnull(case len(day(attività.datainizioattività)) when 1 then '0' + convert(varchar(20),day(attività.datainizioattività)) "
    '        strsql = strsql & "else convert(varchar(20),day(attività.datainizioattività))  end + '/' + "
    '        strsql = strsql & "(case len(month(attività.datainizioattività)) when 1 then '0' + convert(varchar(20),month(attività.datainizioattività)) "
    '        strsql = strsql & "else convert(varchar(20),month(attività.datainizioattività))  end + '/' + "
    '        strsql = strsql & "Convert(varchar(20), Year(attività.datainizioattività))),'XX/XX/XXXX') as DataInizio, "
    '        strsql = strsql & "comuni.Denominazione as Comune "
    '        strsql = strsql & "FROM attività INNER JOIN "
    '        strsql = strsql & "AttivitàSediAssegnazione ON attività.IDAttività = AttivitàSediAssegnazione.IDAttività INNER JOIN "
    '        strsql = strsql & "entisedi ON AttivitàSediAssegnazione.IDEnteSede = entisedi.IDEnteSede INNER JOIN "
    '        strsql = strsql & "comuni ON entisedi.IDComune = comuni.IDComune INNER JOIN "
    '        strsql = strsql & "GraduatorieEntità ON AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione = GraduatorieEntità.IdAttivitàSedeAssegnazione INNER JOIN "
    '        strsql = strsql & "entità ON GraduatorieEntità.IdEntità = entità.IDEntità "
    '        strsql = strsql & "WHERE AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione='" & txtidattivitasedeassegnazione.Value & "'"

    '        'strsql = "select titolo as titolo, "
    '        'strsql = strsql & "isnull(codiceente,'Nessun Codice') as codiceprogetto, "
    '        'strsql = strsql & "isnull(case len(day(datainizioattività)) when 1 then '0' + convert(varchar(20),day(datainizioattività)) "
    '        'strsql = strsql & "else convert(varchar(20),day(datainizioattività))  end + '/' + "
    '        'strsql = strsql & "(case len(month(datainizioattività)) when 1 then '0' + convert(varchar(20),month(datainizioattività)) "
    '        'strsql = strsql & "else convert(varchar(20),month(datainizioattività))  end + '/' + "
    '        'strsql = strsql & "Convert(varchar(20), Year(datainizioattività))),'XX/XX/XXXX') as DataInizio "
    '        'strsql = strsql & "from attività "
    '        'strsql = strsql & "where idattività='" & lblidattivita.Text & "'"

    '        'eseguo la query e passo il risultato al datareader
    '        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

    '        If dtrLeggiDati.HasRows = True Then
    '            dtrLeggiDati.Read()

    '            'Writer.WriteLine("{\rtf1")

    '            'apro il template
    '            xLinea = Reader.ReadLine()

    '            Dim strTitolo As String = dtrLeggiDati("titolo")
    '            Dim strCodiceProgetto As String = dtrLeggiDati("codiceprogetto")
    '            Dim strDataAvvio As String = dtrLeggiDati("DataInizio")
    '            Dim strSede As String = dtrLeggiDati("Comune")

    '            'chiudo il datareader
    '            If Not dtrLeggiDati Is Nothing Then
    '                dtrLeggiDati.Close()
    '                dtrLeggiDati = Nothing
    '            End If

    '            strsql = "Select entità.identità,isnull(entità.CodiceVolontario, 'Nessun Codice') as CodiceVolontario, "
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(entità.cognome,'°',''),'ì','i'''),'é','e'''),"
    '            strsql = strsql & " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') + ' ' +"
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(entità.nome,'°',''),'ì','i'''),'é','e'''),"
    '            strsql = strsql & "'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '')as nominativo, "
    '            strsql = strsql & "entità.Codicefiscale, "
    '            strsql = strsql & "isnull(case len(day(entità.datanascita)) when 1 then '0' + convert(varchar(20),day(entità.datanascita)) "
    '            strsql = strsql & "else convert(varchar(20),day(entità.datanascita))  end + '/' + "
    '            strsql = strsql & "(case len(month(entità.datanascita)) when 1 then '0' + convert(varchar(20),month(entità.datanascita)) "
    '            strsql = strsql & "else convert(varchar(20),month(entità.datanascita))  end + '/' + "
    '            strsql = strsql & "Convert(varchar(20), Year(entità.datanascita))),'') as datanascita, "
    '            strsql = strsql & "isnull(case len(day(entità.DataInizioServizio)) when 1 then '0' + convert(varchar(20),day(entità.DataInizioServizio)) "
    '            strsql = strsql & "else convert(varchar(20),day(entità.DataInizioServizio))  end + '/' + "
    '            strsql = strsql & "(case len(month(entità.DataInizioServizio)) when 1 then '0' + convert(varchar(20),month(entità.DataInizioServizio)) "
    '            strsql = strsql & "else convert(varchar(20),month(entità.DataInizioServizio))  end + '/' + "
    '            strsql = strsql & "Convert(varchar(20), Year(entità.DataInizioServizio))),'') as DataInizioServizio, "
    '            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(comuni.denominazione,'°',''),'ì','i'''),'é','e'''),"
    '            strsql = strsql & "'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') + '(' + isnull(replace(replace(replace(replace(replace(replace(replace(provincie.provincia ,'°',''),'ì','i'''),'é','e'''),"
    '            strsql = strsql & "'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') + ')'as comune, "
    '            strsql = strsql & "isnull(convert(varchar,entisediattuazioni.IDEnteSedeAttuazione),'Nessun Codice') as CodiceSede "
    '            strsql = strsql & "from entità "
    '            strsql = strsql & "inner join statientità on statientità.idstatoentità=entità.idstatoentità "
    '            strsql = strsql & "left join impVolontariLotus on entità.codicefiscale=impVolontariLotus.cf "
    '            strsql = strsql & "inner join graduatorieEntità on graduatorieEntità.identità=Entità.identità "
    '            strsql = strsql & "left join TipologiePosto on graduatorieEntità.idtipologiaposto=TipologiePosto.idtipologiaposto "
    '            strsql = strsql & "inner join comuni on comuni.idcomune=entità.idcomunenascita "
    '            strsql = strsql & "inner join provincie on (provincie.idprovincia=comuni.idprovincia) "
    '            strsql = strsql & "INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità "
    '            strsql = strsql & "INNER JOIN attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione "
    '            strsql = strsql & "INNER JOIN entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione "
    '            strsql = strsql & "where(graduatorieEntità.idattivitàsedeassegnazione='" & txtidattivitasedeassegnazione.Value & "') and graduatorieEntità.ammesso = 1 and statientità.inservizio=1 "
    '            strsql = strsql & "order by nominativo"

    '            'eseguo la query e passo il risultato al datareader
    '            dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

    '            Dim strVolontari As String = ""

    '            While xLinea <> ""

    '                xLinea = Replace(xLinea, "<TitoloProgetto>", strTitolo)
    '                xLinea = Replace(xLinea, "<CodiceProgetto>", strCodiceProgetto)
    '                xLinea = Replace(xLinea, "<DataAvvio>", strDataAvvio)
    '                xLinea = Replace(xLinea, "<Sede>", strSede)

    '                If InStr(xLinea, "<BreakPoint>") > 0 Then
    '                    If dtrLeggiDati.HasRows = True Then
    '                        While dtrLeggiDati.Read
    '                            strVolontari = strVolontari & (dtrLeggiDati("CodiceVolontario") & " " & dtrLeggiDati("Nominativo") & " nato/a il " & dtrLeggiDati("datanascita") & " a " & dtrLeggiDati("comune") & " avvio il " & dtrLeggiDati("DataInizioServizio") & " Codice Sede " & dtrLeggiDati("CodiceSede") & "\par") & vbCrLf

    '                            'Array degli id dei volontari per la gestione della cronologia dei documenti
    '                            arrayIdVol.Add(dtrLeggiDati("identità"))
    '                        End While
    '                        xLinea = Replace(xLinea, "<BreakPoint>", strVolontari)
    '                    End If
    '                End If

    '                Writer.WriteLine(xLinea)

    '                xLinea = Reader.ReadLine()
    '            End While

    '            'close the RTF string and file
    '            'Writer.WriteLine("}")
    '            Writer.Close()
    '            Writer = Nothing

    '            ''chiudo lo streaming in scrittura
    '            Reader.Close()
    '            Reader = Nothing

    '        End If

    '        'chiudo il datareader
    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If

    '        Elencovolontariammessi = "documentazione/" & strNomeFile

    '        'vado a fare la insert
    '        Dim cmdinsert As Data.SqlClient.SqlCommand
    '        strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
    '        strsql = strsql & "values "
    '        strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',2)"
    '        cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
    '        cmdinsert.ExecuteNonQuery()

    '        cmdinsert.Dispose()

    '        'Cronologia documenti volontari in graduatoria
    '        Dim intX As Integer
    '        For intX = 0 To arrayIdVol.Count - 1
    '            ClsUtility.CronologiaDocEntità(arrayIdVol(intX), Session("Utente"), "Graduatoria Volontari", Session("conn"))
    '        Next


    '    Catch ex As Exception
    '        Response.Write(ex.Message)
    '    End Try
    'End Function
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If IsPostBack = False Then

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

            'Modifica del 17/01/2006 di Amilcare Paolella ***************************
            'Ricavo le informazioni dell'utente per valorizzare la path dei documenti
            strsql = "SELECT RegioniCompetenze.CodiceRegioneCompetenza AS Path FROM UtentiUNSC INNER JOIN " & _
                     "RegioniCompetenze ON UtentiUNSC.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza " & _
                     "WHERE UtentiUNSC.UserName ='" & Session("Utente") & "'"
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            If dtrgenerico.Read = True Then
                Session("Path") = dtrgenerico("Path")
                Session("Path") &= "/"
                dtrgenerico.Close()
                dtrgenerico = Nothing
            Else
                'Non c'è corrispondenza è successo qualcosa di inusuale;esco e torno alla logon
                dtrgenerico.Close()
                dtrgenerico = Nothing
                Response.Redirect("LogOn.aspx")
            End If


            txtIdentitaVecchia.Value = Request.QueryString("identita")
            txtidattivita.Value = Request.QueryString("idattivita")
            txtCodiceFiscale.Value = Request.QueryString("CodiceFiscale")
            'Verifico se entita e gia stata sostituita
            strsql = "select idcronologiaSostituzioni,identitàsubentrante,attivitàentità.datainizioattivitàEntità " & _
            " from cronologiasostituzioni " & _
            " inner join attivitàentità on attivitàentità.identità=cronologiasostituzioni.identitàsostituita " & _
            " where identitàSostituita=" & txtIdentitaVecchia.Value & ""
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                txtidentitaSbentrante.Value = dtrgenerico("identitàsubentrante")
                'E gia stato Sostituito
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                'aggiunto da Alessandra Taballione il 05/07/2005
                strsql = "select cognome + ' ' + nome as nominativo,datainizioservizio,isnull(codiceVolontario,'')as codicevolontario from entità where identità=" & txtidentitaSbentrante.Value & " "
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                dtrgenerico.Read()
                txtVolSubentarata.Value = dtrgenerico("nominativo")
                txtCodiceVolontarioSubentrante.Value = dtrgenerico("CodiceVolontario")
                txtdataAvvio.Text = "" & dtrgenerico("datainizioservizio")
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                PopolaMaschera()
                CaricaSediAttuazione()
                'lblTitotloElencoVolontari.Value = "Volontario Subentrante"
                CaricaVolontarioSubentrante()
                Bloccamaschera()
                Session("Sostituito") = "Sostituito"
                'LblAnnullaChiusura.Visible = False
                ImgAnnullaChiusura.Visible = False
            Else
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                PopolaMaschera()
                CaricaSediAttuazione()
                'controllo se sto tornando da una selezione di un volontario di un altro progetto
                If Request.QueryString("VengoDa") = "AltriProgetti" Then
                    dgRisultatoRicercaVolontari.Visible = False
                    rigaLegenda.Visible = False
                    dtgElencoVolontariAltroProgetto.Visible = True
                    caricavolontarialtriprogetti()
                    lblTesto.Visible = False
                    ImageButton1.Visible = False
                Else
                    CaricaVolontari()
                    If dgRisultatoRicercaVolontari.Items.Count = 0 Then
                        lblTesto.Visible = True
                        ImageButton1.Visible = True
                        rigaLegenda.Visible = False

                    Else
                        rigaLegenda.Visible = True
                    End If
                End If
                Session("Sostituito") = ""
                AbilitaDisabilitaAnnullaChiusura()
            End If


        End If
        AbilitaDisabilitaModifChiusura()
        AbilitaPulsanteCancella()
    End Sub


    Sub caricavolontarialtriprogetti()
        strsql = "select entità.identità, entità.cognome + ' ' + entità.nome as nominativo,entità.codicefiscale," & _
        " cn.denominazione + '(' + pn.provincia + ')' as comuneNascita," & _
        " entità.datanascita,entità.idcomunenascita, graduatorieEntità.punteggio," & _
        " entisedi.denominazione + ' ' + entisedi.Indirizzo + ' ' + " & _
        " entisedi.Civico + ' ' + cass.Denominazione + '(' + pAss.provincia + ')'  as sedeAssegnazione " & _
        " from graduatorieEntità " & _
        " inner join entità on graduatorieEntità.identità=entità.identità " & _
        " inner join attivitàsediassegnazione " & _
        " on attivitàsediassegnazione.idattivitàsedeassegnazione=graduatorieEntità.idattivitàsedeassegnazione " & _
        " inner join entisedi on entisedi.identesede=attivitàsediassegnazione.identesede " & _
        " inner join comuni cn on cn.idcomune=entità.idcomunenascita " & _
        " inner join provincie pn on pn.idprovincia=cn.idprovincia " & _
        " inner join comuni cAss on cAss.idcomune=entisedi.idcomune " & _
        " inner join provincie pAss on pAss.idprovincia=cAss.idprovincia " & _
        " where entità.identità='" & Request.QueryString("IdNuovaEntita") & "'"
        dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
        CaricaDataGrid(dtgElencoVolontariAltroProgetto)

    End Sub

    Private Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid) 'valorizzo la datagrid passata
        dgRisultatoRicercaSedi.SelectedIndex = -1
        dgRisultatoRicercaSedi.EditItemIndex = -1
        GridDaCaricare.DataSource = dtsGenerico
        GridDaCaricare.DataBind()
        Session("DatiVolontario") = dtsGenerico
        ControlloVolontariNew()
    End Sub
    Private Sub PopolaMaschera()
        'Generata da Alessandra Taballione il 22.11.2004
        'Effettua il Popolamento della maschera relativo 
        'alle informazioni del Volontario e del progetto
        'Volontario
        'Modificato da simona cordella il 15/01/2007
        'aggiunto il codice volontario nella label cognome e nome e cognome nella label nome
        strsql = "select isnull(replace(replace(replace(replace(replace(replace(replace(entità.cognome,'°',''),'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Cognome, isnull(replace(replace(replace(replace(replace(replace(replace(entità.Nome,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Nome,entità.datachiusura,entità.indirizzo,entità.cap,entità.numerocivico,statientità.Statoentità, entità.datanascita,entità.idcomunenascita," & _
        " cn.denominazione as comuneNascita,cr.denominazione as comuneresidenza," & _
        " entità.idcomuneresidenza,entità.datainizioservizio,entità.datafineservizio,entità.codicefiscale,case entità.sesso when 1 then 'F' else 'M' end as sesso,isnull(entità.codicevolontario,'') as codicevolontario,isnull(p.descrabb,p.provincia) as ProvinciaVol, " & _
        " isnull(entità.AmmessoRecupero,0) as AmmessoRecupero " & _
        " from entità " & _
        " inner join statientità on statientità.idstatoEntità=entità.idstatoentità " & _
        " inner join comuni cn on cn.idcomune=entità.idcomunenascita " & _
        " inner join comuni cr on cr.idcomune=entità.idcomuneresidenza " & _
        " inner join provincie p on p.idprovincia=cr.idprovincia " & _
        " where entità.identità=" & txtIdentitaVecchia.Value & ""
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            lblCognome.Text = IIf(Not IsDBNull(dtrgenerico("Cognome")), dtrgenerico("Cognome"), "")
            lblNome.Text = IIf(Not IsDBNull(dtrgenerico("nome")), dtrgenerico("nome"), "")
            lblComuneNascita.Text = IIf(Not IsDBNull(dtrgenerico("comuneNascita")), dtrgenerico("comuneNascita"), "")
            lblComuneResidenza.Text = IIf(Not IsDBNull(dtrgenerico("comuneresidenza")), dtrgenerico("comuneresidenza"), "")
            lblCodFis.Text = IIf(Not IsDBNull(dtrgenerico("codicefiscale")), dtrgenerico("codicefiscale"), "")
            'lblsesso.Text = dtrgenerico("sesso")
            lbldataNascita.Text = dtrgenerico("DataNascita")
            lblStato.Text = dtrgenerico("Statoentità")
            txtdatachiusuraEV.Text = IIf(Not IsDBNull(dtrgenerico("datachiusura")), dtrgenerico("datachiusura"), "")
            txtCodiceVolontarioVecchio.Value = dtrgenerico("CodiceVolontario")
            Session("IndirizzoVol") = dtrgenerico("Indirizzo")
            Session("CapVol") = dtrgenerico("Cap")
            Session("NumeroCivicoVol") = dtrgenerico("NumeroCivico")
            Session("ProvinciaVol") = dtrgenerico("ProvinciaVol")
            Session("DataFineServizioVol") = dtrgenerico("DataFineServizio")
            If dtrgenerico("AmmessoRecupero") = 1 Then
                LblAmmessoRecupero.Visible = True
                LblAmmessoRecupero.Text = "Recupero posti"
            Else
                LblAmmessoRecupero.Visible = False
            End If
            LblDataInizioServizio.Text = dtrgenerico("DataInizioServizio")
            LblDataFineServizio.Text = dtrgenerico("DataFineServizio")
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        strsql = "Select titolo, datainizioattività,datafineattività,dateadd(day,90,datainizioattività) as dataLimite from attività where idattività=" & txtidattivita.Value & ""
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            lblPreogetto.Text = IIf(Not IsDBNull(dtrgenerico("titolo")), dtrgenerico("titolo"), "")
            lblDataInizio.Text = IIf(Not IsDBNull(dtrgenerico("datainizioattività")), dtrgenerico("datainizioattività"), "")
            lbldataFine.Text = IIf(Not IsDBNull(dtrgenerico("datafineattività")), dtrgenerico("datafineattività"), "")
            txtdatalimite.Value = IIf(Not IsDBNull(dtrgenerico("datalimite")), dtrgenerico("datalimite"), "")
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'aggiunto da Alessandra Taballione il 04/07/2005
        strsql = "select attivitàsediassegnazione.idattivitàsedeassegnazione,attivitàsediassegnazione.datainiziodifferita,attivitàsediassegnazione.datafinedifferita " & _
        " from graduatorieEntità " & _
        " inner join entità on graduatorieEntità.identità=entità.identità  " & _
        " inner join attivitàsediassegnazione  " & _
        " on attivitàsediassegnazione.idattivitàsedeassegnazione=graduatorieEntità.idattivitàsedeassegnazione  " & _
        " where entità.identità=" & txtIdentitaVecchia.Value & ""
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            txtidattivitasedeassegnazione.Value = dtrgenerico("idattivitàsedeassegnazione")
            If Not IsDBNull(dtrgenerico("datainiziodifferita")) Then
                'aggiorno inizio e fine prendendolo dalla graduatoria
                lblDataInizio.Text = dtrgenerico("datainiziodifferita")
                lbldataFine.Text = dtrgenerico("datafinedifferita")
            Else
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                'modifica del 03/08/2020 per eventuale presenza periodo di interruzione sulla sede di destinazione
                strsql = "select isnull(DATEDIFF(DAY,'16/04/2020',DataRipresaServizio),0) giorniinterruzione, b.DataFineAttività +  case when b.DataInizioAttività < '16/04/2020' then isnull(DATEDIFF(DAY,'16/04/2020',a.DataRipresaServizio),0) else 0 end as DataFineEffettiva  " & _
                    " from attivitàentisediattuazione a " & _
                    " inner join attività b on a.IDAttività = b.IDAttività  " & _
                    " inner join attivitàentità c on a.IDAttivitàEnteSedeAttuazione = c.IDAttivitàEnteSedeAttuazione and c.IdStatoAttivitàEntità = 1 " & _
                    " where c.identità=" & txtIdentitaVecchia.Value
                dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                If dtrgenerico.HasRows = True Then
                    dtrgenerico.Read()
                    If dtrgenerico("giorniinterruzione") > 0 Then
                        lbldataFine.Text = dtrgenerico("DataFineEffettiva")
                    End If
                End If
            End If
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If



    End Sub

    'Private Sub ControlloVolontari()
    '    'Generata da Alessandra Taballione il 25/05/2005
    '    'Routine che verifica Anomalie volontario
    '    Dim item As DataGridItem
    '    Dim Eta As Integer
    '    Dim i As Integer
    '    'Dim color1 As New System.Drawing.Color
    '    'Dim color As New System.Drawing.Color
    '    For Each item In dgRisultatoRicercaVolontari.Items


    '        'Aggiunto da Alessandra Taballione il 09/06/2005
    '        'Verifica su tabella con codici Fiscali lotus 
    '        strsql = "select * from impVolontariLotus where CF='" & Replace(dgRisultatoRicercaVolontari.Items(item.ItemIndex).Cells(2).Text, "'", "''") & "' "
    '        If Not dtrgenerico Is Nothing Then
    '            dtrgenerico.Close()
    '            dtrgenerico = Nothing
    '        End If
    '        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
    '        If dtrgenerico.HasRows = True Then
    '            For i = 0 To 8
    '                dgRisultatoRicercaVolontari.Items(item.ItemIndex).Cells(i).BackColor = Color.FromArgb(155, 255, 155)
    '            Next
    '        Else
    '            'dgRisultatoRicerca.Items(item.ItemIndex).Cells(23).Text = ""
    '        End If


    '        'Verifico se l'età del volontario risulta maggiore dei 28 anni
    '        strsql = "select e.datanascita," & _
    '        " (select case when dateadd (yy,bando.maxeta,e.datanascita)> CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.DataFineVolontari ELSE BANDORICORSI.DataFineVolontari END then 'Ammesso' else 'NonAmmesso' end " & _
    '        " from bando " & _
    '        " inner join bandiattività on bandiattività.idbando=bando.idbando  " & _
    '        " inner join attività on attività.idbandoAttività=bandiattività.idbandoAttività  " & _
    '        " inner join attivitàsediassegnazione on attivitàsediassegnazione.idattività=attività.idattività  " & _
    '        " where attivitàsediassegnazione.idattivitàsedeassegnazione=asa.idattivitàsedeassegnazione )as Ammesso " & _
    '        " from entità e " & _
    '        " inner join graduatorieentità ge on e.identità=ge.identità " & _
    '        " inner join attivitàsediassegnazione asa on asa.idattivitàsedeassegnazione=ge.idattivitàsedeassegnazione " & _
    '        " inner join attività a on a.idattività=asa.idattività " & _
    '        " LEFT JOIN BANDORICORSI ON a.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
    '        " where e.identità=" & dgRisultatoRicercaVolontari.Items(item.ItemIndex).Cells(8).Text & " "
    '        If Not dtrgenerico Is Nothing Then
    '            dtrgenerico.Close()
    '            dtrgenerico = Nothing
    '        End If
    '        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
    '        dtrgenerico.Read()
    '        'Eta = dgRisultatoRicerca.Items(item.ItemIndex).Cells(21).Text
    '        If dtrgenerico("Ammesso") = "NonAmmesso" Then
    '            For i = 0 To 8
    '                dgRisultatoRicercaVolontari.Items(item.ItemIndex).Cells(i).BackColor = Color.Khaki
    '            Next
    '        End If
    '        If Not dtrgenerico Is Nothing Then
    '            dtrgenerico.Close()
    '            dtrgenerico = Nothing
    '        End If
    '        'Verifico se l'età del volontario risulta minore dei 18 anni
    '        'Eta = dgRisultatoRicerca.Items(item.ItemIndex).Cells(21).Text
    '        strsql = "select e.datanascita," & _
    '        " (select case when DATEADD ( YY , bando.mineta, e.datanascita ) <= CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.DataFineVolontari ELSE BANDORICORSI.DataFineVolontari END then 'Ammesso' else 'NonAmmesso' end" & _
    '        " from bando " & _
    '        " inner join bandiattività on bandiattività.idbando=bando.idbando  " & _
    '        " inner join attività on attività.idbandoAttività=bandiattività.idbandoAttività  " & _
    '        " inner join attivitàsediassegnazione on attivitàsediassegnazione.idattività=attività.idattività  " & _
    '        " where attivitàsediassegnazione.idattivitàsedeassegnazione=asa.idattivitàsedeassegnazione )as Ammesso " & _
    '        " from entità e " & _
    '        " inner join graduatorieentità ge on e.identità=ge.identità " & _
    '        " inner join attivitàsediassegnazione asa on asa.idattivitàsedeassegnazione=ge.idattivitàsedeassegnazione " & _
    '        " inner join attività a on a.idattività=asa.idattività " & _
    '        " LEFT JOIN BANDORICORSI ON a.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
    '        " where e.identità=" & dgRisultatoRicercaVolontari.Items(item.ItemIndex).Cells(8).Text & " "
    '        If Not dtrgenerico Is Nothing Then
    '            dtrgenerico.Close()
    '            dtrgenerico = Nothing
    '        End If
    '        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
    '        dtrgenerico.Read()
    '        If dtrgenerico("Ammesso") = "NonAmmesso" Then
    '            For i = 0 To 8
    '                dgRisultatoRicercaVolontari.Items(item.ItemIndex).Cells(i).BackColor = Color.Khaki
    '            Next
    '        End If
    '        If Not dtrgenerico Is Nothing Then
    '            dtrgenerico.Close()
    '            dtrgenerico = Nothing
    '        End If
    '        'Esiste Record in AttivitàEntità con stato <> rinuncia?
    '        'strsql = "Select distinct Entità.identità,Entità.cognome,Entità.nome," & _
    '        '" statiEntità.Rinuncia from Entità " & _
    '        '" inner join AttivitàEntità on (Entità.identità=AttivitàEntità.identità) " & _
    '        ' " inner join AttivitàEntisediAttuazione on (AttivitàEntisediAttuazione.idattivitàEntesedeattuazione=AttivitàEntità.idattivitàEntesedeattuazione) " & _
    '        '" inner join statiEntità on (statiEntità.idstatoEntità=Entità.idstatoentità) " & _
    '        '" where entità.codicefiscale='" & dgRisultatoRicercaVolontari.Items(item.ItemIndex).Cells(2).Text & "' and entità.identità<>" & dgRisultatoRicercaVolontari.Items(item.ItemIndex).Cells(8).Text & " and statiEntità.rinuncia <> 1 " 'and AttivitàEntisediAttuazione.idattività <> " & txtidattivita.Text & ""

    '        strsql = "Select count(*) as n " & _
    '                 " from Entità " & _
    '                 " inner join AttivitàEntità on (Entità.identità=AttivitàEntità.identità) " & _
    '                 " inner join AttivitàEntisediAttuazione on (AttivitàEntisediAttuazione.idattivitàEntesedeattuazione=AttivitàEntità.idattivitàEntesedeattuazione) " & _
    '                 " inner join statiEntità on (statiEntità.idstatoEntità=Entità.idstatoentità) " & _
    '                 " left join causali on entità.idcausalechiusura = causali.idcausale " & _
    '                 " where ENTITà.IDENTITà<>'" & dgRisultatoRicercaVolontari.Items(item.ItemIndex).Cells(8).Text & " ' AND entità.codicefiscale='" & dgRisultatoRicercaVolontari.Items(item.ItemIndex).Cells(2).Text & "' and statiEntità.rinuncia <> 1 and isnull(causali.bloccanuovoservizio,1) <> 0" ' and AttivitàEntisediAttuazione.idattività <> " & lblidattivita.Text & ""


    '        If Not dtrgenerico Is Nothing Then
    '            dtrgenerico.Close()
    '            dtrgenerico = Nothing
    '        End If

    '        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
    '        'If dtrgenerico.HasRows = True Then
    '        '    For i = 0 To 8
    '        '        dgRisultatoRicercaVolontari.Items(item.ItemIndex).Cells(i).BackColor = Color.LightSalmon
    '        '    Next
    '        'End If

    '        If dtrgenerico.HasRows = True Then
    '            dtrgenerico.Read()
    '            If dtrgenerico("n") > 0 Then
    '                For i = 0 To dgRisultatoRicercaVolontari.Columns.Count - 1
    '                    dgRisultatoRicercaVolontari.Items(item.ItemIndex).Cells(i).BackColor = Color.Plum
    '                Next
    '            End If
    '        End If

    '        'Esiste Record in GraduatorieEntità per stesso Volontario e Stesso Bando?
    '        strsql = "SELECT count(graduatorieentità.identità) as idEntità FROM graduatorieentità " & _
    '        " inner join entità on entità.identità = graduatorieentità.identità " & _
    '        " inner join attivitàsediassegnazione on " & _
    '        " (attivitàsediassegnazione.idattivitàsedeassegnazione=graduatorieentità.idattivitàsedeassegnazione)" & _
    '        " inner join attività on (attività.idattività=attivitàsediassegnazione.idattività)" & _
    '        " inner join bandiAttività on (Attività.idbandoattività=bandiAttività.idbandoattività)" & _
    '        " inner join bando on (bando.idbando=bandiAttività.idbando)" & _
    '        " where entità.codicefiscale='" & dgRisultatoRicercaVolontari.Items(item.ItemIndex).Cells(2).Text & "' and bando.idbando =(Select bando.idBando from Attività " & _
    '        " inner join bandiAttività on (Attività.idbandoattività=bandiAttività.idbandoattività)" & _
    '        " inner join bando on (bando.idbando=bandiAttività.idbando)" & _
    '        " where Attività.idattività=" & txtidattivita.Value & ")"
    '        '" where graduatorieentità.identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(19).Text & " and bando.idbando =(Select bando.idBando from Attività " & _
    '        If Not dtrgenerico Is Nothing Then
    '            dtrgenerico.Close()
    '            dtrgenerico = Nothing
    '        End If
    '        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
    '        If dtrgenerico.HasRows = True Then
    '            dtrgenerico.Read()
    '            If dtrgenerico("identità") > 1 Then
    '                For i = 0 To 8
    '                    dgRisultatoRicercaVolontari.Items(item.ItemIndex).Cells(i).BackColor = Color.LightSalmon
    '                Next
    '            End If
    '        End If
    '        'End If 'qui

    '    Next
    '    If Not dtrgenerico Is Nothing Then
    '        dtrgenerico.Close()
    '        dtrgenerico = Nothing
    '    End If
    'End Sub

    Private Sub CaricaVolontari()

        strsql = "select entità.identità, entità.cognome + ' ' + entità.nome as nominativo,entità.codicefiscale," & _
        " cn.denominazione + '(' + pn.provincia + ')' as comuneNascita," & _
        " entità.datanascita,entità.idcomunenascita, graduatorieEntità.punteggio," & _
        " entisedi.denominazione + ' ' + entisedi.Indirizzo + ' ' + " & _
        " entisedi.Civico + ' ' + cass.Denominazione + '(' + pAss.provincia + ')'  as sedeAssegnazione " & _
        " from graduatorieEntità " & _
        " inner join entità on graduatorieEntità.identità=entità.identità " & _
        " inner join attivitàsediassegnazione " & _
        " on attivitàsediassegnazione.idattivitàsedeassegnazione=graduatorieEntità.idattivitàsedeassegnazione " & _
        " inner join entisedi on entisedi.identesede=attivitàsediassegnazione.identesede " & _
        " inner join comuni cn on cn.idcomune=entità.idcomunenascita " & _
        " inner join provincie pn on pn.idprovincia=cn.idprovincia " & _
        " inner join comuni cAss on cAss.idcomune=entisedi.idcomune " & _
        " inner join provincie pAss on pAss.idprovincia=cAss.idprovincia " & _
        " where attivitàsediassegnazione.idattività=" & txtidattivita.Value & " and " & _
        " graduatorieentità.stato = 1 And graduatorieentità.ammesso = 0 " & _
        " and entità.disponibilestessoProg=1 and isnull(entità.ammessorecupero,0) = " & IIf(LblAmmessoRecupero.Visible, 1, 0) & " "
        If chkStessaSede.Visible = True Then
            If chkStessaSede.Checked = True Then
                strsql = strsql & " and attivitàsediassegnazione.identesede=" & dgRisultatoRicercaSedi.Items(0).Cells(14).Text & " "
            End If
        End If
        strsql = strsql & " order by graduatorieEntità.punteggio desc, entità.datanascita asc"
        dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
        CaricaDataGrid(dgRisultatoRicercaVolontari)
        If dgRisultatoRicercaVolontari.Items.Count > 0 Then
            chkStessaSede.Visible = True
        Else
            chkStessaSede.Visible = False
        End If
    End Sub
    Private Sub CaricaVolontarioSubentrante()
        strsql = "select entità.identità, entità.cognome + ' ' + entità.nome as nominativo,entità.codicefiscale," & _
        " cn.denominazione + '(' + pn.provincia + ')' as comuneNascita," & _
        " entità.datanascita,entità.idcomunenascita, graduatorieEntità.punteggio," & _
        " entisedi.denominazione + ' ' + entisedi.Indirizzo + ' ' + " & _
        " entisedi.Civico + ' ' + cass.Denominazione + '(' + pAss.provincia + ')'  as sedeAssegnazione " & _
        " from graduatorieEntità " & _
        " inner join entità on graduatorieEntità.identità=entità.identità " & _
        " inner join attivitàsediassegnazione " & _
        " on attivitàsediassegnazione.idattivitàsedeassegnazione=graduatorieEntità.idattivitàsedeassegnazione " & _
        " inner join entisedi on entisedi.identesede=attivitàsediassegnazione.identesede " & _
        " inner join comuni cn on cn.idcomune=entità.idcomunenascita " & _
        " inner join provincie pn on pn.idprovincia=cn.idprovincia " & _
        " inner join comuni cAss on cAss.idcomune=entisedi.idcomune " & _
        " inner join provincie pAss on pAss.idprovincia=cAss.idprovincia " & _
        " where  entità.identità=" & txtidentitaSbentrante.Value & " order by graduatorieEntità.punteggio desc, entità.datanascita desc"
        dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
        CaricaDataGrid(dgRisultatoRicercaVolontari)
    End Sub
    Private Sub CaricaSediAttuazione()
        strsql = "select  entisedi.denominazione as sedefisica, entisediattuazioni.denominazione as sedeAttuazione," & _
       " entisedi.indirizzo,Comuni.denominazione + '(' + provincie.provincia + ')' as Comune,attivitàentità.identità, " & _
       "attivitàentità.identità,attivitàentità.idattivitàentesedeattuazione,attivitàentità.datafineattivitàentità," & _
       " (select idstatoattivitàentità from statiattivitàentità where defaultstato=1) as statodefault," & _
       " attivitàentità.note,attivitàentità.percentualeutilizzo,attivitàentità.idtipologiaposto,entisediattuazioni.identesede " & _
       " from attivitàentisediattuazione" & _
       " inner join attivitàentità on " & _
       " attivitàentità.idattivitàentesedeattuazione = attivitàentisediattuazione.idattivitàentesedeattuazione " & _
       " inner join entisediattuazioni on " & _
       " attivitàentisediattuazione.IdEnteSedeAttuazione = entisediattuazioni.IdEnteSedeAttuazione " & _
       " inner join entisedi on entisedi.identesede=entisediattuazioni.identesede " & _
       " inner join comuni on comuni.idcomune=entisedi.idcomune " & _
       " inner join provincie on provincie.idprovincia=comuni.idprovincia" & _
       " inner join attività on attivitàentisediattuazione.IdAttività=attività.IdAttività " & _
       " where attivitàentità.IdStatoAttivitàEntità = 1 and attivitàentità.identità=" & txtIdentitaVecchia.Value & " and attivitàentisediattuazione.idattività=" & txtidattivita.Value & ""
        dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
        CaricaDataGrid(dgRisultatoRicercaSedi)
    End Sub

    Private Sub dgRisultatoRicercaVolontari_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgRisultatoRicercaVolontari.SelectedIndexChanged
        txtidentitaSbentrante.Value = dgRisultatoRicercaVolontari.SelectedItem.Cells(8).Text
        'aggiunto da Alessandra Taballione il 05/07/2005
        strsql = "select cognome + ' ' + nome as nominativo,isnull(codiceVolontario,'')as codicevolontario  from entità where identità=" & txtidentitaSbentrante.Value & " "
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrgenerico.Read()
        txtVolSubentarata.Value = dtrgenerico("nominativo")
        txtCodiceVolontarioSubentrante.Value = dtrgenerico("codicevolontario")
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub
    Private Sub AddiornaDati()
        Dim null As String
        Dim item As DataGridItem
        Dim bytPostoOccupato As Byte
        Dim intidtipoprogetto As Integer

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        strsql = "Select idtipoprogetto from attività where idattività=" & txtidattivita.Value & ""
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            intidtipoprogetto = dtrgenerico("idtipoprogetto")
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        Dim CodVolontario As String
        Dim MyDtrControllo4 As System.Data.SqlClient.SqlDataReader
        MyDtrControllo4 = ClsServer.CreaDatareader("Select isnull(codicevolontario,'') as codicevolontario From Entità Where  IdEntità = " & txtidentitaSbentrante.Value, Session("Conn"))
        If MyDtrControllo4.HasRows = True Then
            MyDtrControllo4.Read()
            CodVolontario = MyDtrControllo4("codicevolontario")
        End If
        If Not MyDtrControllo4 Is Nothing Then
            MyDtrControllo4.Close()
            MyDtrControllo4 = Nothing
        End If

        Dim NMesi As Integer
        NMesi = NumeroMesiProgetto(txtidattivitasedeassegnazione.Value)

        null = "null"
        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = Session("conn")
        Try
            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            myCommand.Transaction = MyTransaction
            'inizializzo la connessione al command
            'Inserisco cronologia Sostituzioni
            strsql = "Insert into CronologiaSostituzioni (IdentitàSostituita,IdentitàSubentrante,dataCronologia,Username) " & _
            " values (" & txtIdentitaVecchia.Value & "," & txtidentitaSbentrante.Value & ",getdate(),'" & Session("Utente") & "')"
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            'Inserisco Cronologia Entità del nuovo prima di effettuare le modifiche su entità
            strsql = "Insert into cronologiaentità (identità,idstatoEntità,UsernameStato,datachiusura,datacronologia,notestato,idcausalechiusura)" & _
            " select identità,idstatoEntità,UsernameStato,datachiusura,dataUltimoStato,notestato,idcausalechiusura " & _
            " from entità where identità=" & txtidentitaSbentrante.Value & ""
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()

            'Aggiorno Entità subentrante (compreso il codice volontario)
            ''Agg.il 17/04/2008 da simona cordella campo POSTOOCCUPATO = 0 se subentro a volontario chiusto in servizio
            'POSTOOCCUPATO = 1 se subentra a voontari RIMUNCIATARI
            If lblStato.Text = "Rinunciatario" Then
                bytPostoOccupato = 1
            Else
                bytPostoOccupato = 0
            End If
            
            strsql = " Update entità set " & _
                     " idstatoentità = (select idstatoEntità from statientità where inservizio=1), " & _
                     " Usernamestato = '" & Replace(Session("Utente"), "'", "''") & "', " & _
                     " datachiusura = " & null & ", " & _
                     " datainizioservizio = '" & txtdataAvvio.Text & "', "
            If LblAmmessoRecupero.Visible Then 'Or intidtipoprogetto = 4 tolta il 02/09/2021 per nuova gestione date fine gg
                strsql = strsql & "datafineservizio = dateadd(d,-1,dateadd(m," & NMesi & ",'" & txtdataAvvio.Text & "')),"
                '       strsql = strsql & "datafineservizio = dateadd(yy,1,dateadd(d,-1,'" & txtdataAvvio.Text & "')),"
            Else
                strsql = strsql & "datafineservizio = '" & lbldataFine.Text & "',"
            End If
            '                                 "username=(SELECT 'V' + left(codicefiscale,3) + replicate('0',6-len(convert(varchar,identità))) + convert(varchar, identità) from entità where identità=" & txtidentitaSbentrante.Value & "), " & _'
            strsql = strsql & " DataUltimostato = getdate(), " & _
                                 "notestato = " & null & ", " & _
                                 "idcausaleChiusura = " & null & ", " & _
                                 "POSTOOCCUPATO = " & bytPostoOccupato & ", " & _
                                 "UserName= dbo.FN_CalcoloUsernameVolontario(" & txtidentitaSbentrante.Value & ")," & _
                                 "password='" & ClsUtility.CriptaNuovaPass & "' "
            If CodVolontario = "" Then
                strsql = strsql & " , codiceVolontario = " & _
                                     "(SELECT 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE())) + " & _
                                     "CONVERT(VARCHAR(6),REPLICATE('0', 6-LEN(CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) ) )) + " & _
                                     "CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) " & _
                                     "From Entità WHERE SUBSTRING(CodiceVolontario,1,5) = 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE()))) "
            End If
            strsql = strsql & "where identità=" & txtidentitaSbentrante.Value & ""

            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()

            'inserisco Attività entità del nuovo
            For Each item In dgRisultatoRicercaSedi.Items
                strsql = "insert into attivitàentità (idattivitàentesedeattuazione,identità,datainizioAttivitàEntità,datafineAttivitàEntità,idstatoattivitàentità,note,PercentualeUtilizzo,idtipologiaPosto) values " & _
                 "( " & dgRisultatoRicercaSedi.Items(item.ItemIndex).Cells(8).Text & "," & txtidentitaSbentrante.Value & "," & _
                 " '" & txtdataAvvio.Text & "','" & lbldataFine.Text & "'," & dgRisultatoRicercaSedi.Items(item.ItemIndex).Cells(10).Text & "," & _
                 "'" & Replace(dgRisultatoRicercaSedi.Items(item.ItemIndex).Cells(11).Text, "'", "''") & "'," & dgRisultatoRicercaSedi.Items(item.ItemIndex).Cells(12).Text & "," & dgRisultatoRicercaSedi.Items(item.ItemIndex).Cells(13).Text & ")"
                myCommand.CommandText = strsql
                myCommand.ExecuteNonQuery()
            Next
            'Aggiorno graduatorieEntità del subentrante ammesso=1 subentrato=1
            strsql = "update graduatorieEntità set ammesso=1,subentrato=1, idattivitàsedeassegnazione=" & txtidattivitasedeassegnazione.Value & " where identità=" & txtidentitaSbentrante.Value & ""
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            'aggiorno graduatoriaEntità del vecchio sostituito=1
            strsql = "update graduatorieEntità set sostituito=1 where identità=" & txtIdentitaVecchia.Value & ""
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            MyTransaction.Commit()
        Catch e As Exception
            Response.Write(strsql)
            Response.Write("<br>")
            Response.Write(e.Message.ToString)
            MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
        End Try
        MyTransaction.Dispose()
    End Sub
    Private Sub cmdSalva_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSalva.Click
        'aggiungo il controllo sul titolario
        'se c'è vado avanti con la cvonferma della graduatoria
        'altrimenti informo l'utente che non è possibile procedere finchè 
        'non aggiunge il titolario per il bando
        Dim DataAvvioProgetto As String

        lblMessaggi.Text = ""
        LblInfo.Text = ""
        LblConfermaOperazione.Text = ""
        If (ValidaCampi() = True) Then
            If ClsUtility.TrovaTitolario(Request.QueryString("IdAttivita"), Session("conn")) = True Then

                If ControlloGraduatoria(dgRisultatoRicercaSedi.Items(0).Cells(8).Text, DataAvvioProgetto) = "NEGATIVO" Then
                    Response.Write("<script>" & vbCrLf)
                    Response.Write("window.open(""WfrmControlliGraduatoria.aspx?IdAttivita=" & Request.QueryString("IdAttivita") & "&DataAvvio=" & DataAvvioProgetto & "&IdAttivitaEnteSedeAttuazione=" & dgRisultatoRicercaSedi.Items(0).Cells(8).Text & """, """", ""width=950,height=600,dependent=no,scrollbars=yes,status=no,resizable=yes"")" & vbCrLf)
                    Response.Write("</script>")
                    Exit Sub
                End If

                AddiornaDati()
                Bloccamaschera()
                'imgmess.Visible = True
                LblConfermaOperazione.Visible = True
                LblConfermaOperazione.Text = "La sostituzione è avvenuta con successo."
                '*** 09/12/2010 verifico esistenza del codice fascicolo
                strsql = "Select isnull(CodiceFascicolo,'') as CodiceFascicolo,CodiceVolontario from Entità where identità = " & txtidentitaSbentrante.Value
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                dtrgenerico.Read()
                Dim codicevolontario As String
                codicevolontario = dtrgenerico("CodiceVolontario")
                If dtrgenerico("CodiceFascicolo") = "" Then

                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    'loggo su logfascicolivolontari
                    strsql = "INSERT INTO LogFascicoliVolontari([Username],[Metodo],[IdEntità],[DataOraRichiesta],[DataOraEsecuzione],[Eseguito])"
                    strsql = strsql & " VALUES('" & Session("Utente") & "','GeneraFascicolo','" & txtidentitaSbentrante.Value & "',getdate(),NULL,0)"
                    myCommand.CommandText = strsql
                    myCommand.ExecuteNonQuery()

                    '---recupero l'id appena inserito
                    Dim strID As String
                    strsql = "select @@identity as Id"
                    dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                    dtrgenerico.Read()
                    strID = dtrgenerico("Id")
                    dtrgenerico.Close()
                    dtrgenerico = Nothing


                    If ClsUtility.GeneraFascicolo(Session("Utente"), txtidentitaSbentrante.Value, Session("conn"), strID) <> "0" Then       'errore

                        Dim strErr As String
                        strErr = "<table align=""center"" width=""750"" style=""width: 750px"" cellSpacing=""1"" cellPadding=""1""><tr><td align=""750"">"
                        strErr = strErr & "<table align=""center"" cellSpacing=""1"" cellPadding=""1""><tr><td><img src=""http://helios.serviziocivile.it/images/logoIII.jpg"" border=""0""></td></tr></table>"
                        strErr = strErr & "<table align=""center"" bgcolor=""orange"" width=""750""><tr><td><b>Errore generazione fascicolo: Maschera WfrmGestioneSostituisciVolontari.aspx</b></td></tr>"
                        strErr = strErr & "<tr><td><b>Utente:</b> " & Session("Utente") & "</td></tr>"
                        strErr = strErr & "<tr><td><b>IP:</b> " & HttpContext.Current.Request.UserHostAddress & "</td></tr>"
                        strErr = strErr & "<tr><td><b>Codice Volontario: </b>" & codicevolontario & "</td></tr>"
                        strErr = strErr & "</table>"
                        strErr = strErr & "</td></tr></table>"


                        'imgmess.Visible = True
                        lblMessaggi.Visible = True
                        lblMessaggi.Text = "La sostituzione è avvenuta con successo. Per il volontario subentrante non e' stato possibile creare il fascicolo SIGED."
                        ClsUtility.invioEmail("heliosweb@serviziocivile.it", "heliosweb@serviziocivile.it", "d.spagnulo@logicainformatica.it;a.dicroce@logicainformatica.it;c.ottaviani@logicainformatica.it", "ERRORE GENERAZIONE FASCICOLO", strErr)

                    Else
                        strsql = "update LogFascicoliVolontari set DataOraEsecuzione = getdate(), Eseguito=1 where IdLogFascicoliVolontari = " & strID
                        myCommand.CommandText = strsql
                        myCommand.ExecuteNonQuery()


                    End If
                End If
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                '***

            Else
                'imgmess.Visible = True
                lblMessaggi.Visible = True
                lblMessaggi.Text = "Attenzione per il bando di riferimento del progetto non è associato un titolario. Provvedere all'associazione e ripetere l'operazione."
            End If
        End If
    End Sub

    Function ValidaCampi() As Boolean
        Dim campiValidi As Boolean = True
        Dim errore As StringBuilder = New StringBuilder()
        Dim dataFine As Date = Date.Parse(LblDataFineServizio.Text)
        Dim dataInizioProgetto As Date = Date.Parse(lblDataInizio.Text)
        Dim dataFineProgetto As Date = Date.Parse(lbldataFine.Text)
        Dim datalimite As Date = DateAdd("d", 90, dataInizioProgetto)
        Dim dataInizioServizio As Date
        If txtidentitaSbentrante.Value = "" Then
            errore.Append("Selezionare il volontario subentrante.<br/>")
            campiValidi = False
        End If
        If (txtdataAvvio.Text = "") Then
            errore.Append("Selezionare una data di avvio al servizio.<br/>")
            campiValidi = False
        Else

            If (Date.TryParse(txtdataAvvio.Text, dataInizioServizio) = False) Then
                Dim err As String = "Il campo 'Data di avvio al servizio' contiene una data non valida. Immettere la data nel formato gg/mm/aaaa. <br/>"
                errore.Append(String.Format(err))
                campiValidi = False
            Else
                If dataInizioServizio >= dataFineProgetto Then
                    errore.Append("La data di avvio al servizio non può essere uguale o successiva alla data di fine progetto. <br/>")
                    campiValidi = False
                ElseIf dataInizioServizio > datalimite Then
                    LblInfo.Text = "La data di avvio al servizio è superiore di 90 giorni alla data di inizio Progetto.<br/>"
                End If
                If dataInizioServizio < dataFine Then
                    errore.Append("La data di avvio al servizio non può essere antecedente  alla data di fine servizio del volontario Sostituito.<br/>")
                    campiValidi = False
                End If

                If txtidentitaSbentrante.Value <> "" Then
                    'controllo per sospensioni servizio
                    Dim strmessaggio As String = String.Empty
                    Dim blnRiattivazione As Boolean

                    blnRiattivazione = ControllaRiattivazioni(txtidentitaSbentrante.Value, dataInizioServizio, dgRisultatoRicercaSedi.Items(0).Cells(8).Text, strmessaggio)

                    If blnRiattivazione = False Then
                        errore.Append(strmessaggio)
                        campiValidi = False
                    End If
                    'fine implementazione

                    'CONTROLLO NEET
                    strmessaggio = ControlloDataNeet(txtidattivitasedeassegnazione.Value, txtidentitaSbentrante.Value, dataInizioServizio)
                    If strmessaggio <> "" Then
                        errore.Append(strmessaggio)
                        campiValidi = False
                    End If

                    'FINE CONTROLLO NEET
                End If
               
            End If
        End If

        If (campiValidi = False) Then
            lblMessaggi.Text = lblMessaggi.Text + errore.ToString()
            lblMessaggi.Visible = True
        End If
        Return campiValidi
    End Function
    Private Sub Bloccamaschera()
        txtdataAvvio.ReadOnly = True
        cmdSalva.Visible = False
        dgRisultatoRicercaVolontari.Columns(0).Visible = False
        lblTesto.Visible = False
        ImageButton1.Visible = False
    End Sub
    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        If Request.QueryString("provieneda") = "Volontari" Then
            Response.Redirect("WfrmVolontari.aspx?IdVol=" & CInt(Request.QueryString("IdEntita")) & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&op=" & Request.QueryString("op") & "")
        Else
            Response.Redirect("WfrmRicercaVolontariSospesi.aspx?op=" & Request.QueryString("op") & "")
        End If
    End Sub
    Private Sub dgRisultatoRicercaSedi_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicercaSedi.PageIndexChanged
        dgRisultatoRicercaSedi.SelectedIndex = -1
        dgRisultatoRicercaSedi.EditItemIndex = -1
        dgRisultatoRicercaSedi.CurrentPageIndex = e.NewPageIndex
        CaricaSediAttuazione()
    End Sub
    Private Sub dgRisultatoRicercaVolontari_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicercaVolontari.PageIndexChanged
        'passo il nuovo indice selezionato all'indice della pagina da visualizzare
        dgRisultatoRicercaVolontari.SelectedIndex = -1
        dgRisultatoRicercaVolontari.CurrentPageIndex = e.NewPageIndex
        'riassegno il dataset dichiarato volutamente pubblico a tutta la pagina
        dgRisultatoRicercaVolontari.DataSource = Session("DatiVolontario")
        dgRisultatoRicercaVolontari.DataBind()
        ControlloVolontariNew()
    End Sub

    'Private Sub imgGeneraDoc_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgGeneraDoc.Click, imgGeneraDocMultipla.Click
    '    If sender.id = "imgGeneraDoc" Then
    '        lblGenera.Text = "Scarica File  "
    '        imgGeneraDoc.Visible = False
    '        hplDownloadCopia.Visible = True
    '        hplDownload.Visible = True
    '        lblsingola.Visible = False

    '        Dim Documento As New GeneratoreModelli
    '        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
    '        hplDownload.NavigateUrl = Documento.VOL_rinunciaserviziovolontario(Session("IdEnte"), txtIdentitaVecchia.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
    '        hplDownloadCopia.NavigateUrl = Documento.VOL_rinunciaserviziovolontarioCopiaReg(Session("IdEnte"), txtIdentitaVecchia.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
    '        Documento.Dispose()

    '        'hplDownload.NavigateUrl = Rinunciaserviziovolontario(Session("IdEnte"), "rinunciaserviziovolontario")
    '        'hplDownloadCopia.NavigateUrl = Rinunciaserviziovolontario(Session("IdEnte"), "rinunciaserviziovolontarioCopiaReg")
    '        'Cronologia creazione documento.
    '        ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Rinuncia Servizio Volontario", Session("conn"))
    '    Else
    '        lblGenera.Text = "Scarica File  "
    '        imgGeneraDocMultipla.Visible = False
    '        hplDownload.Visible = True
    '        hplDownloadCopia.Visible = True
    '        lblmultipla.Visible = False

    '        Dim Documento As New GeneratoreModelli 'Request.QueryString("idattivita")
    '        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
    '        hplDownload.NavigateUrl = Documento.VOL_rinunciaserviziovolontariomultipla(Session("IdEnte"), txtidentitaSbentrante.Text, ClsUtility.TrovaIDBando(Request.QueryString("IdAttivita"), Session("conn")), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
    '        hplDownloadCopia.NavigateUrl = Documento.VOL_rinunciaserviziovolontariomultiplaCopiaReg(Session("IdEnte"), txtidentitaSbentrante.Text, ClsUtility.TrovaIDBando(Request.QueryString("IdAttivita"), Session("conn")), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
    '        Documento.Dispose()

    '        'hplDownload.NavigateUrl = Rinunciaserviziovolontario(Session("IdEnte"), "rinunciaserviziovolontariomultipla")
    '        'hplDownloadCopia.NavigateUrl = Rinunciaserviziovolontario(Session("IdEnte"), "rinunciaserviziovolontariomultiplaCopiaReg")

    '    End If
    '    hplDownload.Target = "_blank"
    'End Sub


    Sub CaricaDati()
        Dim strsql As String

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "select isnull(replace(replace(replace(replace(replace(replace(replace(a.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Denominazione, "
        strsql = strsql & "isnull(a.CodiceRegione,'') as CodiceRegione, "
        strsql = strsql & "isnull(a.IdClasseAccreditamentoRichiesta,'') as ClasseRichiesta, "
        strsql = strsql & "isnull(case len(day(a.DataCostituzione)) when 1 then '0' + convert(varchar(20),day(a.DataCostituzione)) "
        strsql = strsql & "else convert(varchar(20),day(a.DataCostituzione))  end + '/' + "
        strsql = strsql & "(case len(month(a.DataCostituzione)) when 1 then '0' + convert(varchar(20),month(a.DataCostituzione)) "
        strsql = strsql & "else convert(varchar(20),month(a.DataCostituzione))  end + '/' + "
        strsql = strsql & "Convert(varchar(20), Year(a.DataCostituzione))),'') as DataCostituzione,"
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(b.indirizzo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Indirizzo, "
        strsql = strsql & "isnull(b.Civico,'') as Civico, "
        strsql = strsql & "isnull(b.CAP,'') as CAP, "
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(comuni.denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Comune, "
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(provincie.provincia,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Provincia, "
        strsql = strsql & "(SELECT isnull((SELECT COUNT(*) "
        strsql = strsql & "FROM EntiSediAttuazioni "
        strsql = strsql & "INNER JOIN StatiEntiSedi  "
        strsql = strsql & "ON EntiSediAttuazioni.IdStatoEnteSede = StatiEntiSedi.IdStatoEnteSede "
        strsql = strsql & "INNER JOIN EntiSedi ON EntiSedi.IdEnteSede = EntiSediAttuazioni.IdEnteSede "
        strsql = strsql & "WHERE StatiEntiSedi.Attiva = 1 AND IdEnte = " & Session("IdEnte") & ") "
        strsql = strsql & "+ "
        strsql = strsql & "(SELECT COUNT(*) FROM EntiSediAttuazioni "
        strsql = strsql & "INNER JOIN EntiSedi ON EntiSediAttuazioni.IdEnteSede = EntiSedi.IdEnteSede "
        strsql = strsql & "INNER JOIN AssociaEntiRelazioniSediAttuazioni "
        strsql = strsql & "ON AssociaEntiRelazioniSediAttuazioni.IdEnteSedeAttuazione = EntiSediAttuazioni.IdEnteSedeAttuazione "
        strsql = strsql & "INNER JOIN EntiRelazioni ON EntiRelazioni.IdEnteRelazione = AssociaEntiRelazioniSediAttuazioni.IdEnteRelazione "
        strsql = strsql & "INNER JOIN StatiEntiSedi ON EntiSediAttuazioni.IdStatoEnteSede = StatiEntiSedi.IdStatoEnteSede "
        strsql = strsql & "WHERE StatiEntiSedi.Attiva = 1 AND EntiRelazioni.IdEntePadre = " & Session("IdEnte") & " ),0)) as sediacc  "
        strsql = strsql & "from enti as a "
        strsql = strsql & "left join entisedi as b on a.idente=b.idente "
        strsql = strsql & "and b.identesede = any (SELECT pippo.identesede FROM entisedi pippo "
        strsql = strsql & "INNER JOIN entiseditipi pluto ON pippo.identesede = pluto.identesede "
        strsql = strsql & "WHERE pluto.idtiposede = 1) "
        strsql = strsql & "left join comuni on b.idcomune=comuni.idcomune "
        strsql = strsql & "left join provincie on comuni.idprovincia=provincie.idprovincia "
        strsql = strsql & "left join entiseditipi as c on b.identesede=c.identesede "
        strsql = strsql & "left join tipisedi as d on c.idtiposede=d.idtiposede "
        strsql = strsql & "left join statientisedi as e on b.idstatoentesede=e.idstatoentesede "
        strsql = strsql & "where a.IdEnte=" & Session("IdEnte") & " and ((d.tiposede='Principale') or (b.identesede is null))"

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

    End Sub

    'Private Sub imgGeneraDoc2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgGeneraDoc2.Click
    '    lblGenera2.Text = "Scarica File  "
    '    imgGeneraDoc2.Visible = False
    '    hplDownload2.Visible = True

    '    Dim Documento As New GeneratoreModelli
    '    'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
    '    hplDownload2.NavigateUrl = Documento.VOL_elencovolontariammessi(txtidattivitasedeassegnazione.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
    '    Documento.Dispose()


    '    CronologiaVolontariGraduatoria(txtidattivitasedeassegnazione.Value)
    '    NuovaCronologia("elencovolontariammessi")
    '    'hplDownload2.NavigateUrl = Elencovolontariammessi(Session("IdEnte"), "elencovolontariammessi")
    '    hplDownload2.Target = "_blank"
    'End Sub

    Sub NuovaCronologia(ByVal strDocumento As String)
        'vado a fare la insert
        Dim cmdinsert As Data.SqlClient.SqlCommand
        strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
        strsql = strsql & "values "
        strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & strDocumento & "',2)"
        cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
        cmdinsert.ExecuteNonQuery()

        cmdinsert.Dispose()
    End Sub

   

    Sub CronologiaVolontariGraduatoria(ByVal IdSedeAssegnazione As Integer)

        strsql = "Select DISTINCT entità.identità"
        strsql = strsql & " from entità "
        strsql = strsql & " INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità "
        strsql = strsql & " INNER JOIN attivitàentisediattuazione"
        strsql = strsql & " ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione "
        strsql = strsql & " inner join statientità on statientità.idstatoentità=entità.idstatoentità "
        strsql = strsql & "left join impVolontariLotus on entità.codicefiscale=impVolontariLotus.cf "
        strsql = strsql & "inner join graduatorieEntità on graduatorieEntità.identità=Entità.identità "
        strsql = strsql & "left join TipologiePosto on graduatorieEntità.idtipologiaposto=TipologiePosto.idtipologiaposto "
        strsql = strsql & "inner join comuni on comuni.idcomune=entità.idcomunenascita "
        strsql = strsql & "inner join provincie on (provincie.idprovincia=comuni.idprovincia) "
        strsql = strsql & "where(graduatorieEntità.idattivitàsedeassegnazione='" & IdSedeAssegnazione & "') and graduatorieEntità.ammesso = 1 and statientità.inservizio=1 and attivitàentità.idstatoattivitàentità=1 "

        dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))

        'Cronologia documenti volontari in graduatoria
        Dim intX As Integer

        For intX = 0 To dtsGenerico.Tables(0).Rows.Count - 1
            ClsUtility.CronologiaDocEntità(dtsGenerico.Tables(0).Rows(intX).Item("identità"), Session("Utente"), "Graduatoria Volontari", Session("conn"))
        Next

    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        If Request.QueryString("provieneda") = "Volontari" Then
            Response.Redirect("ricercavolontarialtroprogetto.aspx?AmmessoRecupero=" & IIf(LblAmmessoRecupero.Visible, 1, 0) & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&IdEntita=" & Request.QueryString("IdEntita") & "&VecchioIdAttivitaEntita=" & Request.QueryString("VecchioIdAttivitaEntita") & "&Op=" & Request.QueryString("Op") & "&provieneda=" & Request.QueryString("provieneda"))
        Else
            Response.Redirect("ricercavolontarialtroprogetto.aspx?AmmessoRecupero=" & IIf(LblAmmessoRecupero.Visible, 1, 0) & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&IdEntita=" & Request.QueryString("IdEntita") & "&VecchioIdAttivitaEntita=" & Request.QueryString("VecchioIdAttivitaEntita") & "&Op=" & Request.QueryString("Op"))
        End If

    End Sub

    Private Sub dtgElencoVolontariAltroProgetto_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtgElencoVolontariAltroProgetto.SelectedIndexChanged
        txtidentitaSbentrante.Value = dtgElencoVolontariAltroProgetto.SelectedItem.Cells(8).Text
        'aggiunto da Alessandra Taballione il 05/07/2005
        strsql = "select cognome + ' ' + nome as nominativo,isnull(codiceVolontario,'')as codicevolontario  from entità where identità=" & txtidentitaSbentrante.Value & " "
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrgenerico.Read()
        txtVolSubentarata.Value = dtrgenerico("nominativo")
        txtCodiceVolontarioSubentrante.Value = dtrgenerico("codicevolontario")
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub

    Private Sub chkStessaSede_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkStessaSede.CheckedChanged
        CaricaVolontari()
    End Sub

    'Private Sub imgGeneraDoc3_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgGeneraDoc3.Click
    '    'lblGenera3.Text = "Scarica File  "
    '    lblGenera3.Text = "<Font size=""1"" face=""Verdana"">Scarica</Font>"
    '    imgGeneraDoc3.Visible = False
    '    hplDownload3.Visible = True
    '    hplDownloadB.Visible = True
    '    'hplDownload3.NavigateUrl = Elencovolontariammessi(Session("IdEnte"), "elencovolontariammessi")
    '    hplDownload3.Target = "_blank"
    '    hplDownloadB.Target = "_blank"
    '    strsql = "select a.idtipoprogetto as naz, " & _
    '    " isnull(identitàSubentrante,0) as  subentro, " & _
    '    " e.datainizioservizio,a.datainizioattività " & _
    '    " from Attività a  " & _
    '    " inner join attivitàsediassegnazione asa on asa.idattività=a.idattività  " & _
    '    " inner join graduatorieentità ge on ge.idattivitàsedeassegnazione=asa.idattivitàsedeassegnazione  " & _
    '    " inner join entità e on e.idEntità=ge.idEntità  " & _
    '    " left join cronologiasostituzioni cs on cs.identitàsubentrante=ge.idEntità " & _
    '    " where ge.idEntità=" & txtidentitaSbentrante.Text & ""
    '    If Not dtrgenerico Is Nothing Then
    '        dtrgenerico.Close()
    '        dtrgenerico = Nothing
    '    End If
    '    dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
    '    'Verifica se Volontario Estero o nazionale
    '    If dtrgenerico.HasRows = True Then
    '        dtrgenerico.Read()
    '        Dim dataInizServ As DateTime = dtrgenerico("datainizioservizio") '15
    '        Dim dataInizAtt As DateTime = dtrgenerico("datainizioattività") '20
    '        Dim intx As Integer = 0

    '        If dataInizServ <> dataInizAtt Then
    '            Do While dataInizServ > dataInizAtt
    '                intx += 1
    '                dataInizAtt = dataInizAtt.AddDays(1)
    '            Loop
    '            Select Case intx
    '                Case 1 To 15
    '                    Session("intGGP") = "20"
    '                    Session("intGGR") = "15"
    '                Case 16 To 45
    '                    Session("intGGP") = "18"
    '                    Session("intGGR") = "14"
    '                Case 46 To 75
    '                    Session("intGGP") = "16"
    '                    Session("intGGR") = "13"
    '                Case Else
    '                    Session("intGGP") = "14"
    '                    Session("intGGR") = "11"
    '            End Select
    '        End If
    '        'If dtrgenerico("naz") = "N" Then
    '        '    hplDownload3.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "SostituzioneVolontariNazionali")
    '        'Else
    '        '    hplDownload3.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "SostituzioneVolontariEsteri")
    '        'End If
    '        If dtrgenerico("naz") = 1 Or dtrgenerico("naz") = 3 Then
    '            If dtrgenerico("datainizioservizio") = dtrgenerico("datainizioattività") Then
    '                If Not dtrgenerico Is Nothing Then
    '                    dtrgenerico.Close()
    '                    dtrgenerico = Nothing
    '                End If

    '                Dim Documento As New GeneratoreModelli
    '                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
    '                hplDownload3.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionali(txtidentitaSbentrante.Text, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
    '                hplDownloadB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliB(txtidentitaSbentrante.Text, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
    '                Documento.Dispose()

    '                'hplDownload3.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "AssegnazioneVolontariNazionali")
    '                'hplDownloadB.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "AssegnazioneVolontariNazionaliB")

    '                'Cronologia creazione documento.
    '                ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Assegnazione Volontario - Nazionale", Session("conn"))
    '            Else
    '                If Not dtrgenerico Is Nothing Then
    '                    dtrgenerico.Close()
    '                    dtrgenerico = Nothing
    '                End If

    '                Dim Documento As New GeneratoreModelli
    '                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
    '                hplDownload3.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionali(txtidentitaSbentrante.Text, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
    '                hplDownloadB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliB(txtidentitaSbentrante.Text, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
    '                Documento.Dispose()

    '                'hplDownload3.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "SostituzioneVolontariNazionali")
    '                'hplDownloadB.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "SostituzioneVolontariNazionaliB")

    '                'Cronologia creazione documento.
    '                ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Sostituzione Volontario - Nazionale", Session("conn"))
    '            End If
    '        Else
    '            If dtrgenerico("datainizioservizio") = dtrgenerico("datainizioattività") Then
    '                If Not dtrgenerico Is Nothing Then
    '                    dtrgenerico.Close()
    '                    dtrgenerico = Nothing
    '                End If

    '                Dim Documento As New GeneratoreModelli
    '                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
    '                hplDownload3.NavigateUrl = Documento.VOL_AssegnazioneVolontariEstero(txtidentitaSbentrante.Text, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
    '                hplDownloadB.NavigateUrl = Documento.VOL_AssegnazioneVolontariEsteroB(txtidentitaSbentrante.Text, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
    '                Documento.Dispose()

    '                'hplDownload3.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "AssegnazioneVolontariEstero")
    '                'hplDownloadB.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "AssegnazioneVolontariEsteroB")

    '                'Cronologia creazione documento.
    '                ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Assegnazione Volontario - Estero", Session("conn"))
    '            Else
    '                If Not dtrgenerico Is Nothing Then
    '                    dtrgenerico.Close()
    '                    dtrgenerico = Nothing
    '                End If

    '                Dim Documento As New GeneratoreModelli
    '                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
    '                hplDownload3.NavigateUrl = Documento.VOL_SostituzioneVolontariEsteri(txtidentitaSbentrante.Text, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
    '                hplDownloadB.NavigateUrl = Documento.VOL_SostituzioneVolontariEsteriB(txtidentitaSbentrante.Text, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
    '                Documento.Dispose()

    '                'hplDownload3.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "SostituzioneVolontariEsteri")
    '                'hplDownloadB.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "SostituzioneVolontariEsteriB")

    '                'Cronologia creazione documento.
    '                ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Sostituzione Volontario - Estero", Session("conn"))
    '            End If
    '        End If
    '    End If
    'End Sub

    'Sub per la gestione della stampa della lettera di Chiusura (inziale o in servizio)
    'Private Sub imgChiusura_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgChiusura.Click
    '    lblChiusura.Text = "Scarica File  "
    '    imgChiusura.Visible = False
    '    hplChiusura.Visible = True
    '    hplChiusura.Target = "_blank"
    '    'Controllo se è una Chiusura Iniziale o In Servizio
    '    If imgChiusura.AlternateText = "CI" Then
    '        'Chiusura Iniziale

    '        Dim Documento As New GeneratoreModelli
    '        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
    '        hplChiusura.NavigateUrl = Documento.VOL_ChiusuraIniziale(txtIdentitaVecchia.Value, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
    '        Documento.Dispose()

    '        'hplChiusura.NavigateUrl = LetteraChiusuraIniziale(Session("IdEnte"), "ChiusuraIniziale")
    '        'Cronologia creazione documento.
    '        ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Chiusura Iniziale", Session("conn"))
    '    Else
    '        Dim Documento As New GeneratoreModelli
    '        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
    '        hplChiusura.NavigateUrl = Documento.VOL_ChiusuraInServizio(txtIdentitaVecchia.Value, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
    '        Documento.Dispose()

    '        'Chiusura In Servizio
    '        'hplChiusura.NavigateUrl = LetteraChiusuraInServizio(Session("IdEnte"), "ChiusuraInServizio")
    '        'Cronologia creazione documento.
    '        ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Chiusura In Servizio", Session("conn"))
    '    End If

    'End Sub

    Private Sub RecuperoInfoVecchioVolontario()
        strsql = "select isnull(replace(replace(replace(replace(replace(replace(replace(e.cognome,'°',''),'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') + ' ' " & _
        " + isnull(replace(replace(replace(replace(replace(replace(replace(e.Nome,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Nominativo,"
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.indirizzo,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as indirizzoRes,"
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.numerocivico,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as civicoRes,"
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.cap,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as capRes,"
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.CodiceFiscale,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as CodFis,"
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(cRes.denominazione,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as comuneRes,"
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(pRes.provincia,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as provinciaRes,"
        strsql = strsql & "isnull(case len(day(getdate())) when 1 then '0' + " & _
        " convert(varchar(20),day(getdate())) else convert(varchar(20),day(getdate())) " & _
        " end + '/' + (case len(month(getdate())) when 1 then '0' + convert(varchar(20),month(getdate())) " & _
        " else convert(varchar(20),month(getdate()))  end + '/' + Convert(varchar(20), Year(getdate()))),'') as DataOdierna,"
        strsql = strsql & "isnull(case len(day(asa.dataUltimoStato)) when 1 then '0' +  convert(varchar(20),day(asa.dataUltimoStato)) " & _
        " else convert(varchar(20),day(asa.dataUltimoStato))  end + '/' + (case len(month(asa.dataUltimoStato)) " & _
        " when 1 then '0' + convert(varchar(20),month(asa.dataUltimoStato))  " & _
        " else convert(varchar(20),month(asa.dataUltimoStato))  end + '/' + Convert(varchar(20), " & _
        " Year(asa.dataUltimoStato))),'') as DataApprovazioneGraduatoria,"
        strsql = strsql & " isnull(case len(day(e.dataInizioservizio)) when 1 then '0' +  convert(varchar(20),day(e.dataInizioservizio)) " & _
        " else convert(varchar(20),day(e.dataInizioservizio))  end + '/' + (case len(month(e.dataInizioservizio)) " & _
        " when 1 then '0' + convert(varchar(20),month(e.dataInizioservizio))  " & _
        " else convert(varchar(20),month(e.dataInizioservizio))  end + '/' + Convert(varchar(20), " & _
        " Year(e.dataInizioservizio))),'') as DataInizioServizio,"
        strsql = strsql & " isnull(case len(day(e.dataFineservizio)) when 1 then '0' +  convert(varchar(20),day(e.dataFineservizio)) " & _
        " else convert(varchar(20),day(e.dataFineservizio))  end + '/' + (case len(month(e.dataFineservizio)) " & _
        " when 1 then '0' + convert(varchar(20),month(e.dataFineservizio))  " & _
        " else convert(varchar(20),month(e.dataFineservizio))  end + '/' + Convert(varchar(20), " & _
        " Year(e.dataFineservizio))),'') as DataFineServizio,"
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.Codicevolontario,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Codicevolontario,"
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(a.titolo,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Titolo,"
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(en.denominazione,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Ente,"
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(es.indirizzo,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as indirizzoSede,"
        strsql = strsql & " isnull(replace(replace(replace(replace(replace(replace(replace(es.civico,'°',''),'ì','i''')" & _
        ",'é','e'''), 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as civicoSede, "
        strsql = strsql & " isnull(replace(replace(replace(replace(replace(replace(replace(es.cap,'°',''),'ì', " & _
        " 'i'''),'é','e'''), 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as capSede,"
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(a.codiceEnte,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as CodiceProgetto,"
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(cSa.denominazione,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as comuneSa,"
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(pSa.provincia,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as provinciaSa, "
        strsql = strsql & "  isnull(case len(day(a.datafineattività)) when 1 then '0' + " & _
        " Convert(varchar(20), Day(a.datafineattività)) " & _
        " else convert(varchar(20),day(a.datafineattività))  " & _
        " end + '/' + (case len(month(a.datafineattività))  when 1 then '0' " & _
        " + convert(varchar(20),month(a.datafineattività))   else convert(varchar(20)," & _
        " month(a.datafineattività))  end + '/' + Convert(varchar(20),  Year(a.datafineattività))),'') " & _
        " as DatafineProgetto,"
        strsql = strsql & "  isnull(case len(day(a.dataInizioattività)) when 1 then '0' + " & _
        " Convert(varchar(20), Day(a.dataInizioattività)) " & _
        " else convert(varchar(20),day(a.dataInizioattività))  " & _
        " end + '/' + (case len(month(a.dataInizioattività))  when 1 then '0' " & _
        " + convert(varchar(20),month(a.dataInizioattività))   else convert(varchar(20)," & _
        " month(a.dataInizioattività))  end + '/' + Convert(varchar(20),  Year(a.dataInizioattività))),'') " & _
        " as DataInizioProgetto "
        strsql = strsql & " from entità e"
        strsql = strsql & " inner join comuni cRes on cRes.idcomune=e.idcomuneResidenza"
        strsql = strsql & " inner join provincie pRes on pres.idprovincia=cRes.idprovincia"
        strsql = strsql & " inner join graduatorieEntità ge on ge.identità=e.idEntità"
        strsql = strsql & " inner join AttivitàSediAssegnazione asa on asa.idattivitàSedeassegnazione=ge.idattivitàSedeassegnazione"
        strsql = strsql & " inner join attività a on a.idattività=asa.idattività"
        strsql = strsql & " inner join entisedi es on es.identesede=asa.identesede"
        strsql = strsql & " inner join comuni cSa on cSa.idcomune=es.idcomune"
        strsql = strsql & " inner join provincie pSa on pSa.idprovincia=cSa.idprovincia"
        strsql = strsql & " inner join Enti en on  en.idente=es.idente"
        strsql = strsql & " where e.identità=" & txtIdentitaVecchia.Value & ""

        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
    End Sub

    Private Sub CmdModChiVol_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles CmdModChiVol.Click
        'Sub per la sostituzione del tipo di chiusura
        If Session("Sostituito") = "Sostituito" Then
            lblMessaggi.Text = "Non è possibile cambiare il tipo di chiusura perchè il volontario è stato già sostituito!"
        Else
            If Request.QueryString("provieneda") = "Volontari" Then
                Response.Redirect("WfrmModificaChiusuraVolontario.aspx?provieneda=" & Request.QueryString("provieneda") & "&IdProgetto=" & Request.QueryString("IdAttivita") & "&IdVolontario=" & Request.QueryString("IdEntita") & "&VecchioIdAttivitaEntita=" & Request.QueryString("VecchioIdAttivitaEntita") & "&Op=" & Request.QueryString("Op"))
            Else
                Response.Redirect("WfrmModificaChiusuraVolontario.aspx?IdProgetto=" & Request.QueryString("IdAttivita") & "&IdVolontario=" & Request.QueryString("IdEntita") & "&VecchioIdAttivitaEntita=" & Request.QueryString("VecchioIdAttivitaEntita") & "&Op=" & Request.QueryString("Op"))
            End If

        End If

    End Sub

    Private Sub AbilitaDisabilitaModifChiusura()

        'Verifica se l'uetnte puo modificare il libretto postale
        strsql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu" & _
                 " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
        '============================================================================================================================
        '====================================================30/09/2008==============================================================
        '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
        '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
        '============================================================================================================================
        If Session("Read") <> "1" Then
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        Else
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        End If
        strsql = strsql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Forza Tipo Chiusura'" & _
                 " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            CmdModChiVol.Enabled = True
            CmdModChiVol.Visible = True
        Else
            CmdModChiVol.Enabled = False
            CmdModChiVol.Visible = False
        End If
        dtrgenerico.Close()
        dtrgenerico = Nothing

    End Sub

    Private Sub AbilitaPulsanteCancella()

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        strsql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                " VociMenu.IdVoceMenuPadre" & _
                " FROM VociMenu" & _
                " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
        '============================================================================================================================
        '====================================================30/09/2008==============================================================
        '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
        '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
        '============================================================================================================================
        If Session("Read") <> "1" Then
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        Else
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        End If

        strsql = strsql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                " WHERE (VociMenu.descrizione = 'Forza Chiusura')" & _
                " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))

        If dtrgenerico.HasRows = True Then
            imgVolontari.Visible = True
            'lblModChiusura.Visible = True
        Else
            imgVolontari.Visible = False
            'lblModChiusura.Visible = False
        End If

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub

    Protected Sub imgVolontari_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgVolontari.Click


        Response.Redirect("DettagliVolontario.aspx?VengoDa=Modifica&IdProgetto=" & Request.QueryString("IdAttivita") & "&IdVolontario=" & Request.QueryString("IdEntita") & "&VecchioIdAttivitaEntita=" & Request.QueryString("VecchioIdAttivitaEntita") & "&Op=" & Request.QueryString("Op"))


    End Sub

    Private Sub AbilitaDisabilitaAnnullaChiusura()
        'aggiunto il 11/12/2006 da simona cordella
        'Verifico se l'utente è abilitato al Menu' "Annulla Chiusura Volontario" 
        strsql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu" & _
                 " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
        '============================================================================================================================
        '====================================================30/09/2008==============================================================
        '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
        '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
        '============================================================================================================================
        If Session("Read") <> "1" Then
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        Else
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        End If
        strsql = strsql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Forza Ripristina Volontario'" & _
                 " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            'LblAnnullaChiusura.Visible = True
            ImgAnnullaChiusura.Visible = True
        Else
            'LblAnnullaChiusura.Visible = False
            ImgAnnullaChiusura.Visible = False
        End If
        dtrgenerico.Close()
        dtrgenerico = Nothing
    End Sub

    Protected Sub ImgAnnullaChiusura_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ImgAnnullaChiusura.Click
        Dim strDataFineAttivita As String
        Dim intIdEntita As Integer
        Dim intIdStatoEntita As Integer
        Dim intIdCausale As Integer
        Dim strDataChiusura As String
        Dim strNote As String
        Dim strDataUltimoStato As String
        LblConfermaOperazione.Text = ""
        lblMessaggi.Text = ""
        LblInfo.Text = ""
        'aggiunto il 04/12/2006 da Simona Cordella
        ' Trovo la data fine attività del progetto per riportare in Servizio il Volontario
        strsql = "SELECT  entità.IDEntità, attività.DataFineAttività,dateadd(yy,1,dateadd(d,-1,entità.DataInizioServizio)) as DataFineAttivitàGG , entità.IDStatoEntità, " & _
                " entità.IDCausaleChiusura, entità.DataChiusura, entità.DataUltimoStato, entità.NoteStato " & _
                " FROM entità INNER JOIN " & _
                " attivitàentità ON entità.IDEntità = attivitàentità.IDEntità INNER JOIN " & _
                " attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione " & _
                " INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
                " WHERE entità.CodiceVolontario ='" & txtCodiceVolontarioVecchio.Value & "'"
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrgenerico.Read()

        If Session("Sistema") = "Futuro" Then
            strDataFineAttivita = "" + (dtrgenerico!DataFineAttivitàGG)
        Else
            strDataFineAttivita = "" + (dtrgenerico!DataFineAttività)
        End If

        ' strDataFineAttivita = "" + (dtrgenerico!DataFineAttività)

        intIdEntita = (dtrgenerico!IDEntità)
        intIdStatoEntita = (dtrgenerico!IDStatoEntità)
        intIdCausale = (dtrgenerico!IDCausaleChiusura)
        strDataChiusura = (dtrgenerico!DataChiusura)
        strDataUltimoStato = (Day(dtrgenerico!DataUltimoStato) & "/" & Month(dtrgenerico!DataUltimoStato) & "/" & Year(dtrgenerico!DataUltimoStato))
        strNote = "Ripristinato:" + (dtrgenerico!NoteStato)
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        If strDataFineAttivita <> "" Then
            strsql = "INSERT INTO CronologiaEntità " & _
                     "(IDEntità, IDStatoEntità, UserNameStato, " & _
                       " DataChiusura, DataCronologia, NoteStato, IDCausaleChiusura) " & _
                     " VALUES (" & intIdEntita & ", " & intIdStatoEntita & ", '" & Session("Utente") & "', " & _
                     " '" & strDataChiusura & "','" & strDataUltimoStato & "','" & Replace(strNote, "'", "''") & "','" & intIdCausale & "')"
            myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))

            strsql = " UPDATE entità " & _
                     " SET DataFineServizio = '" & strDataFineAttivita & " '," & _
                     " IDStatoEntità =3, DataChiusura =null, NoteStato =null, IDCausaleChiusura = null ," & _
                     " UsernameStato ='" & Session("Utente") & "', DataUltimoStato=getdate(), UserNameUltimaModifica='" & Session("Utente") & "', DataUltimaModifica=getdate() " & _
                     " where IDEntità =" & intIdEntita & ""
            ',AltreInformazioni =null
            myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))

            strsql = " UPDATE Attivitàentità " & _
                    " SET DataFineAttivitàEntità = '" & strDataFineAttivita & " '" & _
                    " where IDEntità =" & intIdEntita & " AND IDSTATOATTIVITàENTITà =1 "
            myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
            'imgmess.Visible = True
            LblConfermaOperazione.Visible = True
            LblConfermaOperazione.Text = "La modifica è avvenuta con successo."
            Bloccamaschera()
            'LblAnnullaChiusura.Visible = False
            ImgAnnullaChiusura.Visible = False
            'lblModChiusura.Visible = False
            imgVolontari.Visible = False
            CmdModChiVol.Visible = False
        End If

    End Sub
    Private Sub ControlloVolontariNew()
        Dim item As DataGridItem
        Dim strRisultato As String
        Dim i As Integer
        Dim strVisualizzaEsito As String = ""
        Dim numeroCelle As Integer
        If (dgRisultatoRicercaVolontari.Items.Count > 0) Then
            numeroCelle = dgRisultatoRicercaVolontari.Items(0).Cells.Count
        End If

        For Each item In dgRisultatoRicercaVolontari.Items
            strRisultato = UCase(LeggiStoreVolontariControlli(dgRisultatoRicercaVolontari.Items(item.ItemIndex).Cells(8).Text, strVisualizzaEsito))
            If strRisultato = "DA VERIFICARE" Then
                For i = 0 To numeroCelle - 1
                    dgRisultatoRicercaVolontari.Items(item.ItemIndex).Cells(i).BackColor = Color.Khaki
                Next
            End If
            If strRisultato = "ANOMALIA" Then
                For i = 0 To numeroCelle - 1
                    dgRisultatoRicercaVolontari.Items(item.ItemIndex).Cells(i).BackColor = Color.LightSalmon

                Next
            End If
            ' colonna Info Anomalie (deve essere lasciata come ultima colonna del datagrid)
            dgRisultatoRicercaVolontari.Items(item.ItemIndex).Cells(numeroCelle - 1).Text = strVisualizzaEsito

        Next
    End Sub

    Private Function LeggiStoreVolontariControlli(ByVal IDEntità As Integer, ByRef VisualizzaEsito As String) As String
        'Agg. da Simona Cordella il 16/06/2009
        'richiamo store che verifca se l'ente ha completato tutti gli inserimeni e gli aggiornamenti necessari per effettuare la presentazione della domanda di accrditamento /adeguamento
        Dim intValore As Integer

        Dim myCommand As SqlClient.SqlCommand
        myCommand = New SqlClient.SqlCommand
        myCommand.CommandType = CommandType.StoredProcedure
        myCommand.CommandText = "SP_VOLONTARI_CONTROLLI"
        myCommand.Connection = Session("Conn")

        Dim sparam As SqlClient.SqlParameter
        sparam = New SqlClient.SqlParameter
        sparam.ParameterName = "@IdEntita"
        sparam.SqlDbType = SqlDbType.Int
        myCommand.Parameters.Add(sparam)


        Dim sparam1 As SqlClient.SqlParameter
        sparam1 = New SqlClient.SqlParameter
        sparam1.ParameterName = "@Esito"
        sparam1.SqlDbType = SqlDbType.VarChar
        sparam1.Size = 50
        sparam1.Direction = ParameterDirection.Output
        myCommand.Parameters.Add(sparam1)

        Dim sparam2 As SqlClient.SqlParameter
        sparam2 = New SqlClient.SqlParameter
        sparam2.ParameterName = "@VisualizzaEsito"
        sparam2.SqlDbType = SqlDbType.VarChar
        sparam2.Size = 100
        sparam2.Direction = ParameterDirection.Output
        myCommand.Parameters.Add(sparam2)


        myCommand.Parameters("@IdEntita").Value = IDEntità
        myCommand.Parameters("@VisualizzaEsito").Value = VisualizzaEsito
        myCommand.ExecuteScalar()

        LeggiStoreVolontariControlli = myCommand.Parameters("@Esito").Value
        VisualizzaEsito = myCommand.Parameters("@VisualizzaEsito").Value
    End Function
    Protected Sub ImgDocumentazione_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ImgDocumentazione.Click
        Dim url As StringBuilder = New StringBuilder()
        url.Append("WfrmElencoDocumentazioneSostituzioneVolontario.aspx")
        url.Append("?")
        url.Append("IdAttivita=")
        url.Append(Request.QueryString("idattivita"))
        url.Append("&")
        url.Append("identita=")
        url.Append(Request.QueryString("identita"))
        url.Append("&")
        url.Append("IdAttivitaSedeAssegnazione=")
        url.Append(txtidattivitasedeassegnazione.Value)
        url.Append("&")
        url.Append("IdEntitaSubentrante=")
        url.Append(txtidentitaSbentrante.Value)
        url.Append("&")
        url.Append("IdVolVecchio=")
        url.Append(txtIdentitaVecchia.Value)
        url.Append("&")
        url.Append("CodiceFiscale=")
        url.Append(txtCodiceFiscale.Value)
        url.Append("&")
        url.Append("NominativoSostituito=")
        url.Append(lblCognome.Text)
        url.Append(" ")
        url.Append(lblNome.Text)
        url.Append("&")
        url.Append("Progetto=")
        url.Append(lblPreogetto.Text)
        url.Append("&")
        url.Append("NominativoSubentrante=")
        url.Append(txtVolSubentarata.Value)
        Response.Redirect(url.ToString())
    End Sub

    Private Function NumeroMesiProgetto(ByVal IdAttivitàSedeAssegnazione As Integer) As Integer
        Dim dtrGenerico As SqlClient.SqlDataReader
        Dim strSql As String
        Dim NMesi As Integer
        strSql = " SELECT a.nmesi from attività a "
        strSql &= " inner join  AttivitàSediAssegnazione b on a.IDAttività=b.IDAttività"
        strSql &= " where  b.idattivitàsedeassegnazione = " & IdAttivitàSedeAssegnazione
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        dtrGenerico.Read()

        NMesi = dtrGenerico("nmesi")
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        Return NMesi
    End Function
    Private Function ControllaRiattivazioni(ByVal IdVolontario As Integer, ByVal DataAvvio As Date, ByVal IdAttivitàEnteSedeAttuazione As Integer, ByRef messaggio As String) As Boolean

        Dim esito As Boolean
        Dim MySqlCommand As SqlClient.SqlCommand

        MySqlCommand = New SqlClient.SqlCommand
        MySqlCommand.CommandType = CommandType.StoredProcedure
        MySqlCommand.CommandText = "[SP_VERIFICA_SOSTITUISCI_VOLONTARIO]"
        MySqlCommand.Connection = Session("conn")

        Try

            MySqlCommand.Parameters.Add("@IdVolontario", SqlDbType.Int).Value = IdVolontario
            MySqlCommand.Parameters("@IdVolontario").Direction = ParameterDirection.Input

            MySqlCommand.Parameters.Add("@DataAvvio", SqlDbType.DateTime).Value = DataAvvio
            MySqlCommand.Parameters("@DataAvvio").Direction = ParameterDirection.Input

            MySqlCommand.Parameters.Add("@IdAttivitàEnteSedeAttuazione", SqlDbType.Int).Value = IdAttivitàEnteSedeAttuazione
            MySqlCommand.Parameters("@IdAttivitàEnteSedeAttuazione").Direction = ParameterDirection.Input

            MySqlCommand.Parameters.Add("@Esito", SqlDbType.Bit)
            MySqlCommand.Parameters("@Esito").Direction = ParameterDirection.Output

            MySqlCommand.Parameters.Add("@MESSAGGIO", SqlDbType.VarChar)
            MySqlCommand.Parameters("@MESSAGGIO").Direction = ParameterDirection.Output
            MySqlCommand.Parameters("@MESSAGGIO").Size = 8000

            MySqlCommand.ExecuteNonQuery()

            esito = MySqlCommand.Parameters("@Esito").Value
            messaggio = MySqlCommand.Parameters("@MESSAGGIO").Value

        Catch ex As Exception
            'Response.Write(ex.Message.ToString())
        End Try

        Return esito

    End Function
    Private Function ControlloDataNeet(ByVal IdAttivitaSedeAssegnazione As Integer, ByVal IdEntità As Integer, ByVal DataAvvio As String) As String

        Dim sqlDAP As SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strEsito As String
        Dim strNomeStore As String = "[SP_GG_VERIFICA_DATE_CONTROLLI_NEET_SINGOLO]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@IdAttivitàSedeAssegnazione", SqlDbType.Int).Value = IdAttivitaSedeAssegnazione
            sqlDAP.SelectCommand.Parameters.Add("@IdEntità", SqlDbType.Int).Value = IdEntità
            sqlDAP.SelectCommand.Parameters.Add("@DataAvvio", SqlDbType.Date).Value = CDate(DataAvvio)

            Dim sparam0 As SqlClient.SqlParameter
            sparam0 = New SqlClient.SqlParameter
            sparam0.ParameterName = "@Esito"
            sparam0.Size = 100
            sparam0.SqlDbType = SqlDbType.Int
            sparam0.Direction = ParameterDirection.Output
            sqlDAP.SelectCommand.Parameters.Add(sparam0)

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Motivazione"
            sparam1.Size = 1000
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlDAP.SelectCommand.Parameters.Add(sparam1)

            sqlDAP.Fill(dataSet)
            strEsito = sqlDAP.SelectCommand.Parameters("@Motivazione").Value
            Return strEsito
        Catch ex As Exception
            'lblMessaggioAlert.Visible = True
            'lblMessaggioAlert.Text = "Si è verificato un errore non gestito. Contattare l'assistenza."
            'Exit Function
        End Try


    End Function
    Private Function ControlloGraduatoria(ByVal IdAttivitaEnteSedeAttuazione As Integer, ByRef DataAvvioProgetto As String) As String

        Dim sqlDAP As SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strEsito As String
        Dim strNomeStore As String = "[SP_GRADUATORIA_CONTROLLI_V3]"
        Dim IdAttivita As Integer

        Dim DtrAppo As System.Data.SqlClient.SqlDataReader
        Dim StrSql As String

        StrSql = " SELECT DISTINCT a.idattività, dbo.formatodata(a.datainizioattività) as DataAvvio" & _
                 " FROM attività a inner join attivitàentisediattuazione aesa on a.idattività = aesa.idattività" & _
                 " where aesa.idattivitàentesedeattuazione = " & IdAttivitaEnteSedeAttuazione
        DtrAppo = ClsServer.CreaDatareader(StrSql, Session("Conn"))
        DtrAppo.Read()
        IdAttivita = DtrAppo.Item("idattività")
        DataAvvioProgetto = DtrAppo.Item("DataAvvio")

        DtrAppo.Close()
        DtrAppo = Nothing


        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@IdAttivita", SqlDbType.Int).Value = IdAttivita
            sqlDAP.SelectCommand.Parameters.Add("@DataAvvio", SqlDbType.Date).Value = CDate(DataAvvioProgetto)
            sqlDAP.SelectCommand.Parameters.Add("@IDAttivitàEnteSedeAttuazione", SqlDbType.Int).Value = IdAttivitaEnteSedeAttuazione

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlDAP.SelectCommand.Parameters.Add(sparam1)

            sqlDAP.Fill(dataSet)
            strEsito = sqlDAP.SelectCommand.Parameters("@Esito").Value
            Return strEsito
        Catch ex As Exception
            lblMessaggi.Visible = True
            lblMessaggi.Text = "Si è verificato un errore non gestito. Contattare l'assistenza."
            Exit Function
        End Try


    End Function
End Class
