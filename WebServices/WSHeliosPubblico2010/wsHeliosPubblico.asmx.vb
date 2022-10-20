Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class wsHeliosPubblico
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function GetRisultatoAutenticazione_NEW(ByVal strTipoAutenticazione As String, ByVal strToken As String, ByVal strUserid As String, ByVal strPassword As String) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiAutenticazioneVolontario_NEW(strTipoAutenticazione, strToken, strUserid, strPassword))

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetRisultatoAutenticazione_NEW = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetRisultatoAutenticazione_NEW

    End Function

    <WebMethod()> _
    Public Function GetModificaPasswordDOL(ByVal strUsernameCrypt As String, ByVal strPasswordCrypt As String, ByVal strVecchiaPassword As String, ByVal strNuovaPassword As String) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiModificaPasswordDOL(strUsernameCrypt, strPasswordCrypt, strVecchiaPassword, strNuovaPassword))

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetModificaPasswordDOL = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetModificaPasswordDOL

    End Function

    <WebMethod()> _
    Public Function GetResetPasswordDOL(ByVal strCodiceFiscale As String, ByVal strReturnUrl As String) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiResetPasswordDOL(strCodiceFiscale, strReturnUrl))

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetResetPasswordDOL = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetResetPasswordDOL

    End Function


    <WebMethod()> _
    Public Function GetDatiRicercaEnti(ByVal strRegione As String, ByVal strProvincia As String, ByVal strComune As String, ByVal strTipoEnte As String, ByVal strCompetenza As String, ByVal strClasse As String, ByVal strDenominazioneEnte As String, ByVal strCodiceEnte As String) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        Try
            xmlDOC.LoadXml(wsProva.EseguiRicercaEnti(strRegione, strProvincia, strComune, strTipoEnte, strCompetenza, strClasse, strDenominazioneEnte, strCodiceEnte))
        Catch ex As Exception
            Throw ex
        End Try


        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetDatiRicercaEnti = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetDatiRicercaEnti

    End Function

    <WebMethod()> _
    Public Function GetDatiRicercaEntiSCU(ByVal strRegione As String, ByVal strProvincia As String, ByVal strComune As String, ByVal strTipoEnte As String, ByVal strCompetenza As String, ByVal strClasse As String, ByVal strDenominazioneEnte As String, ByVal strCodiceEnte As String) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        Try
            xmlDOC.LoadXml(wsProva.EseguiRicercaEntiSCU(strRegione, strProvincia, strComune, strTipoEnte, strCompetenza, strClasse, strDenominazioneEnte, strCodiceEnte))
        Catch ex As Exception
            Throw ex
        End Try


        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetDatiRicercaEntiSCU = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetDatiRicercaEntiSCU

    End Function

    <WebMethod()> _
    Public Function GetDatiRicercaEntiSCU_NEW(ByVal strLocalizzazione As String, ByVal strNazione As String, ByVal strRegione As String, ByVal strProvincia As String, ByVal strComune As String, ByVal strSezione As String, ByVal strDenominazioneEnte As String, ByVal strCodiceEnte As String, ByVal strTitolareAccoglienza As String) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        Try
            xmlDOC.LoadXml(wsProva.EseguiRicercaEntiSCU_NEW(strLocalizzazione, strNazione, strRegione, strProvincia, strComune, strSezione, strDenominazioneEnte, strCodiceEnte, strTitolareAccoglienza))
        Catch ex As Exception
            Throw ex
        End Try


        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetDatiRicercaEntiSCU_NEW = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetDatiRicercaEntiSCU_NEW

    End Function

    <WebMethod()> _
    Public Function GetElencoSediEnteSCU_NEW(ByVal strLocalizzazione As String, ByVal strNazione As String, ByVal strRegione As String, ByVal strProvincia As String, ByVal strComune As String, ByVal strCodiceEnte As String) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        Try
            xmlDOC.LoadXml(wsProva.EseguiElencoSediEnteSCU_NEW(strLocalizzazione, strNazione, strRegione, strProvincia, strComune, strCodiceEnte))
        Catch ex As Exception
            Throw ex
        End Try


        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetElencoSediEnteSCU_NEW = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetElencoSediEnteSCU_NEW

    End Function

    <WebMethod()> _
    Public Function GetElencoEntiAccoglienzaEnteSCU_NEW(ByVal strLocalizzazione As String, ByVal strNazione As String, ByVal strRegione As String, ByVal strProvincia As String, ByVal strComune As String, ByVal strCodiceEnteTitolare As String) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        Try
            xmlDOC.LoadXml(wsProva.EseguiElencoEntiAccoglienzaEnteSCU_NEW(strLocalizzazione, strNazione, strRegione, strProvincia, strComune, strCodiceEnteTitolare))
        Catch ex As Exception
            Throw ex
        End Try


        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetElencoEntiAccoglienzaEnteSCU_NEW = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetElencoEntiAccoglienzaEnteSCU_NEW

    End Function

    <WebMethod()> _
    Public Function GetDatiProgetti(ByVal strCodiceEnte As String, ByVal strEnte As String, ByVal strTitolo As String, ByVal strRegione As String, ByVal strProvincia As String, ByVal strComune As String, ByVal strSettore As String, ByVal strArea As String, ByVal strGazzetta As String, ByVal strCompetenzaEnte As String, ByVal strCompetenzaProgetto As String, ByVal strTipoProgetto As String, ByVal strMisure As String, ByVal strDurata As String, ByVal strGiovaniMinoriOpportunita As String, ByVal strEsteroUE As String, ByVal strTutoraggio As String, ByVal strFAMI As String, ByVal strCodiceProgetto As String, ByVal strDigitale As String, ByVal strAmbientale As String) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiRicercaProgetto(strCodiceEnte, strEnte, strTitolo, strRegione, strProvincia, strComune, strSettore, strArea, strGazzetta, strCompetenzaEnte, strCompetenzaProgetto, strTipoProgetto, strMisure, strDurata, strGiovaniMinoriOpportunita, strEsteroUE, strTutoraggio, strFAMI, strCodiceProgetto, strDigitale, strAmbientale))

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetDatiProgetti = xmlDOC

        Return GetDatiProgetti

    End Function

    <WebMethod()> _
Public Function GetDatiSettoreArea() As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiRicercaSettoreDiIntervento)

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetDatiSettoreArea = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetDatiSettoreArea

    End Function

    <WebMethod()> _
Public Function GetDatiRicercaClassi() As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiRicercaClassi)

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetDatiRicercaClassi = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetDatiRicercaClassi

    End Function

    <WebMethod()> _
    Public Function GetDatiRicercaSezioni_NEW() As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiRicercaSezioni_NEW)

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetDatiRicercaSezioni_NEW = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetDatiRicercaSezioni_NEW

    End Function

    <WebMethod()> _
    Public Function GetDatiRicercaClassiSCU() As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiRicercaClassiSCU)

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetDatiRicercaClassiSCU = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetDatiRicercaClassiSCU

    End Function

    <WebMethod()> _
    Public Function GetDatiNazioniAccreditamentoSCU_NEW() As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiRicercaNazioniAccreditamentoSCU_NEW)

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetDatiNazioniAccreditamentoSCU_NEW = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetDatiNazioniAccreditamentoSCU_NEW

    End Function

    <WebMethod()> _
Public Function GetDatiGeografico() As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiRicercaGeografico)

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetDatiGeografico = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetDatiGeografico

    End Function
    <WebMethod()> _
    Public Function GetDatiGeograficoDomicilio() As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiRicercaGeograficoDomicilio)

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetDatiGeograficoDomicilio = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetDatiGeograficoDomicilio

    End Function
    <WebMethod()> _
Public Function GetRisultatoAutenticazione(ByVal strUserName As String, ByVal strPWD As String) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiAutenticazioneVolontario(strUserName, strPWD))

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetRisultatoAutenticazione = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetRisultatoAutenticazione

    End Function

    <WebMethod()> _
        Public Function GetCandidatiPerEvento(ByVal annoEvento As Integer) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiRicercaCandidatiPerEvento(annoEvento))

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetCandidatiPerEvento = xmlDOC

        Return GetCandidatiPerEvento
    End Function

    <WebMethod()> _
    Public Function GetVotiPerEvento(ByVal annoEvento As Integer) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiRicercaVotiPerEvento(annoEvento))

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetVotiPerEvento = xmlDOC

        Return GetVotiPerEvento
    End Function

    <WebMethod()> _
    Public Function GetRisultatoContatori() As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiRicercaContatori)

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetRisultatoContatori = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetRisultatoContatori

    End Function

    <WebMethod()> _
Public Function GetRisultatoDettaglioEnte(ByVal strCodiceEnte As String, ByVal strRegione As String, ByVal strProvincia As String, ByVal strComune As String) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiRicercaDettaglioEnte(strCodiceEnte, strRegione, strProvincia, strComune))

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetRisultatoDettaglioEnte = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetRisultatoDettaglioEnte

    End Function

    <WebMethod()> _
Private Function GetRisultatoModificaPWD(ByVal strUserName As String, ByVal strVecchiaPWD As String, ByVal strNuovaPWD As String) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiModificaPwdVolontario(strUserName, strVecchiaPWD, strNuovaPWD))

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetRisultatoModificaPWD = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetRisultatoModificaPWD

    End Function

    <WebMethod()> _
        Public Function GetDatiVolontario(ByVal strUserName As String, ByVal strPWD As String) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiRichiestaDatiVolontario(strUserName, strPWD))

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetDatiVolontario = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetDatiVolontario

    End Function

    <WebMethod()> _
        Public Function GetRecuperoPassword(ByVal strUserName As String, ByVal strCodVol As String, ByVal strEmail As String) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiRecuperoPassword(strUserName, strCodVol, strEmail))

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetRecuperoPassword = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetRecuperoPassword

    End Function

    <WebMethod()> _
    Public Function GetModificaDatiVolontario(ByVal strUserName As String, ByVal strPWD As String, ByVal strNuovaEmail As String, ByVal strNuovoTelefono As String, ByVal strNuovoIBAN As String, ByVal strNuovoSWIFT As String, ByVal strNuovaProvinciaDomicilio As String, ByVal strNuovoComuneDomicilio As String, ByVal strNuovoIndirizzoDomicilio As String, ByVal strNuovoCivicoDomicilio As String, ByVal strNuovoCAPDomicilio As String) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiRichiestaModificaDatiVolontario(strUserName, strPWD, strNuovaEmail, strNuovoTelefono, strNuovoIBAN, strNuovoSWIFT, strNuovaProvinciaDomicilio, strNuovoComuneDomicilio, strNuovoIndirizzoDomicilio, strNuovoCivicoDomicilio, strNuovoCAPDomicilio))

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetModificaDatiVolontario = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetModificaDatiVolontario

    End Function
    <WebMethod()> _
    Public Function GetContrattoVolontario(ByVal strUserName As String, ByVal strPWD As String) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiRichiestaContratto(strUserName, strPWD))

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetContrattoVolontario = xmlDOC

        Return GetContrattoVolontario
    End Function
    <WebMethod()> _
    Public Function UploadContrattoVolontario(ByVal strUserName As String, ByVal strPWD As String, ByVal File As String, ByVal NomeFile As String) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiUploadContrattoVolontario(strUserName, strPWD, File, NomeFile))

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        UploadContrattoVolontario = xmlDOC

        Return UploadContrattoVolontario
    End Function
    <WebMethod()> _
    Public Function GetAttestatoVolontario(ByVal strUserName As String, ByVal strPWD As String) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiRichiestaAttestato(strUserName, strPWD))

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetAttestatoVolontario = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetAttestatoVolontario

    End Function

    <WebMethod()> _
Public Function GetStatiAttestatoVolontario(ByVal strUserName As String, ByVal strPWD As String) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiRichiestaStatiAttestato(strUserName, strPWD))

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetStatiAttestatoVolontario = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetStatiAttestatoVolontario

    End Function

    <WebMethod()> _
Public Function GetModificaPWDVolontario(ByVal strUserName As String, ByVal strVecchiaPWD As String, ByVal strNuovaPWD As String) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiModificaPwdVolontario(strUserName, strVecchiaPWD, strNuovaPWD))

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetModificaPWDVolontario = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetModificaPWDVolontario

    End Function



    'Public Function GetDatiPagamenti(ByVal strUserName As String, ByVal strPWD As String) As System.Xml.XmlDocument
    '    Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
    '    Dim testXML As System.Xml.XmlNode
    '    Dim xmlDOC As New System.Xml.XmlDocument

    '    xmlDOC.LoadXml(wsProva.EseguiRicercaPagamenti(strUserName, strPWD))

    '    'Create an XML declaration. 
    '    Dim xmldecl As System.Xml.XmlDeclaration
    '    xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

    '    'Add the new node to the document.
    '    Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
    '    xmlDOC.InsertBefore(xmldecl, root)

    '    GetDatiPagamenti = xmlDOC

    '    Return GetDatiPagamenti

    'End Function

    <WebMethod()> _
    Public Function GetDatiPagamenti(ByVal strUserName As String, ByVal strPWD As String) As DataSet
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato

        Return wsProva.EseguiRicercaPagamenti(strUserName, strPWD)

    End Function
    <WebMethod()> _
    Public Function GetVolontarioCUD(ByVal strUserName As String, ByVal strPWD As String) As String
        'Dim wsProva As New GetVolontarioCUD.webPrivatoCUD

        'Return wsProva.EseguiRichiestaCUD(strUserName, strPWD, 0, 2)


        Dim Client As New GetCUVolontari.CUServiceClient
        Dim vol As New GetCUVolontari.Volontario
        Dim risultato As String

        vol.StrUserName = strUserName
        vol.StrPWD = strPWD
        vol.IdCud = False
        vol.IdVol = False

        risultato = Client.GetCUVol(vol)
        Client.Close()
        Return risultato

    End Function

    <WebMethod()> _
    Public Function GetModificaStatoQuestionario(ByVal strUserName As String, ByVal strPWD As String, ByVal strCodiceVolontario As String, ByVal intStatoQuestionario As Integer) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiModificaStatoQuestionario(strUserName, strPWD, strCodiceVolontario, intStatoQuestionario))

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetModificaStatoQuestionario = xmlDOC

        Return GetModificaStatoQuestionario

    End Function

    <WebMethod()> _
    Public Function GetObiettiviProgrammi() As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiRicercaObiettiviProgrammi)

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetObiettiviProgrammi = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetObiettiviProgrammi

    End Function

    <WebMethod()> _
    Public Function GetAmbitiProgrammi() As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiRicercaAmbitiProgrammi)

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetAmbitiProgrammi = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetAmbitiProgrammi

    End Function

    <WebMethod()> _
    Public Function GetCertificatoVolontario(ByVal strUserName As String, ByVal strPWD As String, ByVal strSoloVerifica As String) As System.Xml.XmlDocument
        Dim wsProva As New GetDatiHeliosPrivato.wsHeliosPrivato
        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.LoadXml(wsProva.EseguiRichiestaCertificatoServizioSvolto(strUserName, strPWD, strSoloVerifica))

        'Create an XML declaration. 
        Dim xmldecl As System.Xml.XmlDeclaration
        xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

        'Add the new node to the document.
        Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
        xmlDOC.InsertBefore(xmldecl, root)

        GetCertificatoVolontario = xmlDOC

        'Dim xmlLocal As String

        'Try
        '    xmlLocal = wsProva.EseguiRicercaEnti("", "", "", "", "", "", "", "")
        '    GetDatiRicercaEnti = xmlLocal
        'Catch ex As Exception

        'End Try

        Return GetCertificatoVolontario

    End Function
End Class