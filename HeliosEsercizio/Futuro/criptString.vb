Imports System.Security.Cryptography
Public Class criptString
    Private TripleDes As New TripleDESCryptoServiceProvider
    Private Function TruncateHash(ByVal key As String, ByVal length As Integer) As Byte()
        Dim sha1 As New SHA1CryptoServiceProvider

        ' Hash the key.
        Dim keyBytes() As Byte = System.Text.Encoding.Unicode.GetBytes(key)
        Dim hash() As Byte = sha1.ComputeHash(keyBytes)

        ' Truncate or pad the hash.
        ReDim Preserve hash(length - 1)
        Return hash
    End Function

    Sub New()
        ' Initialize the crypto provider.
        'TripleDes.Key = TruncateHash(key, TripleDes.KeySize \ 8)
        'TripleDes.IV = TruncateHash("", TripleDes.BlockSize \ 8)

        Dim b() As Byte = {16, 245, 123, 16, 64, 22, 1, 200}
        Dim c() As Byte = {23, 118, 87, 73, 6, 102, 76, 255, 41, 84, 2, 24, 201, 105, 54, 19, 54, 17, 3, 88, 13, 224, 173, 135}

        TripleDes.Key = c
        TripleDes.IV = b

    End Sub

    Public Function EncryptData(ByVal plaintext As String) As String

        '' Convert the plaintext string to a byte array.
        Dim plaintextBytes() As Byte = _
            System.Text.Encoding.UTF8.GetBytes(plaintext)

        ' Create the stream.
        Dim ms As New System.IO.MemoryStream
        ' Create the encoder to write to the stream.
        Dim encStream As New CryptoStream(ms, TripleDes.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write)

        ' Use the crypto stream to write the byte array to the stream.
        encStream.Write(plaintextBytes, 0, plaintextBytes.Length)
        encStream.FlushFinalBlock()

        ' Convert the encrypted stream to a printable string.
        Return Convert.ToBase64String(ms.ToArray)
        'Return plaintext
    End Function

    Public Function DecryptData(ByVal encryptedtext As String) As String


        Dim encryptedBytes() As Byte = Convert.FromBase64String(encryptedtext)

        ' Create the stream.
        Dim ms As New System.IO.MemoryStream
        ' Create the decoder to write to the stream.
        Dim decStream As New CryptoStream(ms, TripleDes.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write)

        ' Use the crypto stream to write the byte array to the stream.
        decStream.Write(encryptedBytes, 0, encryptedBytes.Length)
        decStream.FlushFinalBlock()

        ' Convert the plaintext stream to a string.
        Return System.Text.Encoding.UTF8.GetString(ms.ToArray)
        'Return encryptedtext
    End Function
    
End Class
