Imports System.Data.SqlClient

Public Class informazionientesede
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

    Private Sub NascondiMenuLaterale()
        Session("TP") = True
    End Sub
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        PopolaMaschera(CInt(Request.QueryString("IdEnteSede")))

    End Sub

    Sub PopolaMaschera(ByVal IdEnteSede As Integer)
        Dim dtrGenerico As SqlClient.SqlDataReader
        Dim strsql As String
        Dim DsSedi As DataSet = New DataSet
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        'popolamento della maschera dati DB


        strsql = " Select comuni.idComune,comuni.denominazione as Comune,provincie.provincia,StatiEntiSedi.statoentesede," & _
                 " entisediattuazioni.idEnteSedeAttuazione,tipisedi.tiposede," & _
                 " entisedi.Denominazione, entisedi.Indirizzo,entisedi.DettaglioRecapito, entisedi.civico, entisedi.cap, " & _
                 " isnull(entisedi.prefissoTelefono,'') + ''+ isnull(entisedi.Telefono,'') as telefono," & _
                 " entisedi.Palazzina,entisedi.Piano,entisedi.scala,entisedi.interno, " & _
                 " isnull(entisedi.prefissofax,'') + ''+ isnull(entisedi.fax,'') as fax ,entisediattuazioni.NMaxVolontari," & _
                 " entisedi.http, entisedi.email, TitoliGiuridici.TitoloGiuridico, entisedi.AltroTitoloGiuridico  " & _
                 " from entisedi " & _
                 " inner join entisediattuazioni on entisedi.identesede = entisediattuazioni.identesede " & _
                 " inner join StatiEntiSedi on (entisedi.IdStatoEnteSede=dbo.StatiEntiSedi.IdStatoEnteSede) " & _
                 " inner join comuni on (comuni.idcomune=entisedi.idcomune) " & _
                 " inner join provincie on (provincie.idprovincia=comuni.idprovincia)" & _
                 " left join entisediTipi on (entisediTipi.identesede=entisedi.idEntesede)" & _
                 " inner join tipiSedi on entisediTipi.idtiposede = tipiSedi.idtiposede" & _
                 " inner join TitoliGiuridici on TitoliGiuridici.IdTitoloGiuridico = entisedi.IdTitoloGiuridico" & _
                 " where entisedi.identeSede = " & IdEnteSede & ""

        dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrGenerico.Read()
        If dtrGenerico.HasRows = True Then
            'aggiunto il 05/03/2008 da s.c.
            lblStato.Text = "" & dtrGenerico("statoentesede")
            If Not IsDBNull(dtrGenerico("denominazione")) Then
                lblSedeEnte.Text = dtrGenerico("denominazione")
                lblCodSedeAttuazione.Text = dtrGenerico("idEnteSedeAttuazione")
            End If
            If Not IsDBNull(dtrGenerico("indirizzo")) Then
                lblIndirizzo.Text = dtrGenerico("indirizzo")
            End If
            If Not IsDBNull(dtrGenerico("DettaglioRecapito")) Then
                lblDettRecapito.Text = dtrGenerico("DettaglioRecapito")
            End If

            '**** agg. da simona cordella il 19/05/2009 per nuovo accreditamento luglio 2009
            'palazzina
            If Not IsDBNull(dtrGenerico("Palazzina")) Then
                lblPalazzina.Text = dtrGenerico("Palazzina")
            End If
            'Scala
            If Not IsDBNull(dtrGenerico("Scala")) Then
                lblScala.Text = dtrGenerico("Scala")
            End If
            'Piano
            If Not IsDBNull(dtrGenerico("Piano")) Then
                lblPiano.Text = dtrGenerico("Piano")
            End If
            ' Interno
            If Not IsDBNull(dtrGenerico("Interno")) Then
                lblInterno.Text = dtrGenerico("Interno")
            End If
            'IdTitoloGiuridico
            If Not IsDBNull(dtrGenerico("TipoSede")) Then
                lbltipologia.Text = dtrGenerico("TipoSede")
            End If
            'IdTitoloGiuridico
            If Not IsDBNull(dtrGenerico("TitoloGiuridico")) Then
                lblTitoloGiuridico.Text = dtrGenerico("TitoloGiuridico")
            End If
            'AltroTitoloGiuridico -- agg. il 03/07/2009 da s.c.
            If Not IsDBNull(dtrGenerico("AltroTitoloGiuridico")) Then
                lblSpecAltro.Text = dtrGenerico("AltroTitoloGiuridico")
            End If

            '*******************
            If Not IsDBNull(dtrGenerico("NMaxVolontari")) Then
                lblNumVo.Text = dtrGenerico("NMaxVolontari")
            End If

            If Not IsDBNull(dtrGenerico("Civico")) Then
                lblNumero.Text = dtrGenerico("Civico")
            End If
            If Not IsDBNull(dtrGenerico("Cap")) Then
                lblCAP.Text = dtrGenerico("Cap")
            End If
            If Not IsDBNull(dtrGenerico("Comune")) Then
                lblComunue.Text = dtrGenerico("Comune")
            End If

            If Not IsDBNull(dtrGenerico("tiposede")) Then

                lbltipologia.Text = dtrGenerico("tiposede")
            End If
            If Not IsDBNull(dtrGenerico("Telefono")) Then
                lblTelefono.Text = dtrGenerico("Telefono")
            End If

            If Not IsDBNull(dtrGenerico("Fax")) Then
                lblFax.Text = dtrGenerico("Fax")
            End If
            If Not IsDBNull(dtrGenerico("http")) Then
                lblHTTP.Text = dtrGenerico("http")
            End If
            If Not IsDBNull(dtrGenerico("email")) Then
                lblHTTP.Text = dtrGenerico("email")
            End If
           
        End If

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub


End Class