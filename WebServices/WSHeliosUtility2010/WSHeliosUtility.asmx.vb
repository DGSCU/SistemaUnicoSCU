Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class WSHeliosUtility
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function DataMatrixToRtfString(ByVal TestoDaConvertire As String) As String
        'generazione datamatrix con libreria Datamatrixlibrary.dll 
        Dim DM As New DatamatrixLibrary.DataMatrix(TestoDaConvertire, DatamatrixLibrary.EncodingType.Ascii)
        Dim dmFinale As System.Drawing.Bitmap
        dmFinale = DatamatrixLibrary.DMImgUtility.SimpleResizeBmp(DM.Image, 3, 3)

        'Predisposizione stringa da includere in file rtf
        Dim PictureToStingRTF = "{\pict\wmetafile8"
        'PictureToStingRTF = PictureToStingRTF & "\picw" & dmFinale.Width
        'PictureToStingRTF = PictureToStingRTF & "\pich" & dmFinale.Height
        PictureToStingRTF = PictureToStingRTF & "\picwgoal" & dmFinale.Width * 15
        PictureToStingRTF = PictureToStingRTF & "\pichgoal" & dmFinale.Height * 15 & vbCrLf

        'conversione in stringa dell'immagine del datamatrix
        Dim x As New ESCommon.Rtf.RtfImage(dmFinale, ESCommon.Rtf.RtfImageFormat.Wmf)
        PictureToStingRTF = PictureToStingRTF & x.HexData
        PictureToStingRTF = PictureToStingRTF & "}"

        'ritorno stringa completa
        DataMatrixToRtfString = PictureToStingRTF

    End Function

End Class