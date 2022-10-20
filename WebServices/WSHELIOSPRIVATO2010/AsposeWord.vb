Imports System.Text
Imports System.IO
Imports Aspose.Words
Imports Aspose.Words.Drawing

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
Public Class AsposeWord
    'Private db As SqlADO
    Private currentFile As String
    Public doc As Aspose.Words.Document
    Private fields() As String
    Private fieldValues() As Object
    Private imgfields() As String
    Private imgpaths() As String
    Private htmlfields() As String
    Private htmltexts() As String

    Public Class HandleMergeImageFieldFromBlob
        Implements Aspose.Words.MailMerging.IFieldMergingCallback
        Private imgfields() As String
        Private imgpaths() As String
        Private htmlfields() As String
        Private htmltexts() As String

        Sub FieldMerging(ByVal args As Aspose.Words.MailMerging.FieldMergingArgs) Implements Aspose.Words.MailMerging.IFieldMergingCallback.FieldMerging
            'i field da sostituire con html devono avere il nome iniziante per "html"
            If (args.DocumentFieldName.StartsWith("html")) Then
                For i As Integer = 0 To htmlfields.Length - 1
                    If htmlfields(i) = args.DocumentFieldName Then
                        'Insert the text for this merge field as HTML data, using DocumentBuilder.
                        Dim builder As New DocumentBuilder(args.Document)
                        builder.MoveToMergeField(args.DocumentFieldName)
                        builder.InsertHtml(htmltexts(i))
                        'The HTML text itself should not be inserted: we have already inserted it as an HTML.
                        args.Text = ""
                    End If
                Next
            End If

            'Do nothing.
        End Sub

        ''' <summary>
        ''' This is called when mail merge engine encounters Image:XXX merge field in the document.
        ''' You have a chance to return an Image object, file name or a stream that contains the image.
        ''' </summary>
        ''' <remarks></remarks>
        Sub ImageFieldMerging(ByVal e As Aspose.Words.MailMerging.ImageFieldMergingArgs) Implements Aspose.Words.MailMerging.IFieldMergingCallback.ImageFieldMerging
            'The field value is a byte array, just cast it and create a stream on it.
            ' MemoryStream imageStream = new MemoryStream((byte[])e.FieldValue);
            'Now the mail merge engine will retrieve the image from the stream.
            ' e.ImageStream = imageStream;
            'cerca il fieldname
            If IsNothing(imgfields) OrElse imgfields.Length < 1 Then Return
            For i As Integer = 0 To UBound(imgfields)
                If imgfields(i) = e.DocumentFieldName Then
                    e.ImageFileName = imgpaths(i)

                    'Dim b() As Byte = Utils.leggiFileBinario(imgpaths(i))
                    'Dim imageStream As New MemoryStream(b)
                    'e.ImageStream = imageStream
                End If
            Next
        End Sub
        Sub setImgData(ByVal imgFieldsIn() As String, ByVal imgPathsIn() As String, ByVal htmlFieldsIn() As String, ByVal htmlTextsIn() As String)
            imgfields = imgFieldsIn
            imgpaths = imgPathsIn
            htmlfields = htmlFieldsIn
            htmltexts = htmlTextsIn
        End Sub
    End Class
    Public Sub New()
        license()
    End Sub

    Public Sub open(ByVal path As String)
        currentFile = path
        doc = New Aspose.Words.Document(path)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="field"></param>
    ''' <param name="fieldvalue"></param>
    ''' <remarks></remarks>
    Public Sub addFieldValue(ByVal field As String, ByVal fieldvalue As Object)
        If IsNothing(fields) Then
            ReDim fields(0)
            ReDim fieldValues(0)
            fields(0) = field
            fieldValues(0) = fieldvalue

        Else
            Dim n As Integer = fields.Length
            ReDim Preserve fields(n)
            ReDim Preserve fieldValues(n)
            fields(n) = field
            fieldValues(n) = fieldvalue
        End If
    End Sub
    Public Sub addFieldImg(ByVal field As String, ByVal filepath As String)
        If IsNothing(imgfields) Then
            ReDim imgfields(0)
            ReDim imgpaths(0)
            imgfields(0) = field
            imgpaths(0) = filepath

        Else
            Dim n As Integer = imgfields.Length
            ReDim Preserve imgfields(n)
            ReDim Preserve imgpaths(n)
            imgfields(n) = field
            imgpaths(n) = filepath
        End If
    End Sub
    Public Sub addFieldHtml(ByVal field As String, ByVal htmlText As String)
        If IsNothing(htmlfields) Then
            ReDim htmlfields(0)
            ReDim htmltexts(0)
            htmlfields(0) = field
            htmltexts(0) = htmlText

        Else
            Dim n As Integer = htmlfields.Length
            ReDim Preserve htmlfields(n)
            ReDim Preserve htmltexts(n)
            htmlfields(n) = field
            htmltexts(n) = htmlText
        End If
    End Sub
    Public Sub save(ByVal savepath As String)
        doc.Save(savepath)
    End Sub
    Public Sub mergeTextFields(ByVal fieldsIn() As String, ByVal valuesIn() As Object)
        Dim hi As New HandleMergeImageFieldFromBlob
        doc.MailMerge.FieldMergingCallback = hi

        doc.MailMerge.Execute(fieldsIn, valuesIn)
    End Sub
    Public Sub merge()
        Dim hi As New HandleMergeImageFieldFromBlob
        hi.setImgData(imgfields, imgpaths, htmlfields, htmltexts)
        doc.MailMerge.FieldMergingCallback = hi
        doc.MailMerge.Execute(fields, fieldValues)
    End Sub
    Public Sub savePdf(ByVal savepath As String)
        Dim i As Integer = InStrRev(savepath, ".")
        If i > 0 Then
            savepath = Left(savepath, i) & "pdf"
        Else
            savepath &= ".pdf"
        End If
        Dim options As New Aspose.Words.Saving.PdfSaveOptions()
        options.EmbedFullFonts = True
        options.Compliance = Saving.PdfCompliance.PdfA1b
        options.PrettyFormat = True
        options.TextCompression = Saving.PdfTextCompression.None
        doc.Save(savepath, options)
    End Sub
    'Public Sub replaceText(ByVal templateText As String, ByVal replaceText As String)
    '    doc.Range.Replace(templateText, replaceText, False, False)
    'End Sub
    Public Function docxBytes() As Byte()
        Dim docStream As New MemoryStream()
        doc.Save(docStream, SaveFormat.Docx)
        Return docStream.ToArray()
    End Function
    Public Function pdfBytes() As Byte()
        Dim docStream As New MemoryStream()
        doc.Save(docStream, SaveFormat.Pdf)
        Return docStream.ToArray()
    End Function
    Public Sub license()
        Dim Key As String = "xxxxxx"
        Dim LStream As Stream = New MemoryStream(Convert.FromBase64String(Key))
        Dim license As New Aspose.Words.License()
        license.SetLicense(LStream)
    End Sub



End Class

