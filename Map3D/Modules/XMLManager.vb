Imports System.Xml.Serialization
Imports System.IO

Public Class XMLFileManager(Of t)
    Public Sub Serialize(ByVal OutputXMLFile As String, ByVal data As t)
        Dim objStreamWriter As StreamWriter = Nothing
        Try
            'Serialize object to a text file.
            objStreamWriter = New StreamWriter(OutputXMLFile)
            Dim xml_serializer As New XmlSerializer(GetType(t))
            xml_serializer.Serialize(objStreamWriter, data)
        Catch ex As Exception

        Finally
            If objStreamWriter IsNot Nothing Then
                objStreamWriter.Close()
            End If
        End Try
    End Sub

    Public Function Deserialize(ByVal InputXMLFile As String) As t
        Dim objStreamReader As StreamReader = Nothing
        Dim data As t = Nothing
        Try
            'Deserialize object to a text file.
            objStreamReader = New StreamReader(InputXMLFile)
            Dim xml_serializer As New XmlSerializer(GetType(t))
            data = DirectCast(xml_serializer.Deserialize(objStreamReader), t)
        Catch ex As Exception

        Finally
            If objStreamReader IsNot Nothing Then
                objStreamReader.Close()
            End If
        End Try
        Return data
    End Function

End Class
