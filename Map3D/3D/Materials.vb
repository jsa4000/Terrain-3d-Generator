Imports System.IO
Imports OpenTK

Public Class Material
    Public AmbientColor As New Vector3()
    Public DiffuseColor As New Vector3()
    Public SpecularColor As New Vector3()
    Public SpecularExponent As Single = 1
    Public Opacity As Single = 1.0F

    Public AmbientMap As String = String.Empty
    Public DiffuseMap As String = String.Empty
    Public SpecularMap As String = String.Empty
    Public OpacityMap As String = String.Empty
    Public NormalMap As String = String.Empty

    Public Sub New()
    End Sub

    Public Sub New(ambient As Vector3, diffuse As Vector3, specular As Vector3, Optional specexponent As Single = 1.0F, Optional opacity As Single = 1.0F)
        AmbientColor = ambient
        DiffuseColor = diffuse
        SpecularColor = specular
        SpecularExponent = specexponent
        Me.Opacity = opacity
    End Sub

    Public Shared Function LoadFromFile(filename As String) As Dictionary(Of String, Material)
        Dim result As New Dictionary(Of String, Material)()
        Try
            Dim currentmat As String = String.Empty
            Using reader As New StreamReader(New FileStream(filename, FileMode.Open, FileAccess.Read))
                Dim currentLine As String
                While Not reader.EndOfStream
                    currentLine = reader.ReadLine()
                    If Not currentLine.StartsWith("newmtl") Then
                        If currentmat.StartsWith("newmtl") Then
                            currentmat += currentLine + vbLf
                        End If
                    Else
                        If currentmat.Length > 0 Then
                            Dim newMat As New Material()
                            Dim newMatName As String = String.Empty
                            newMat = LoadFromString(currentmat, newMatName)
                            result.Add(newMatName, newMat)
                        End If
                        currentmat = currentLine + vbLf
                    End If
                End While
            End Using
            ' Add final material
            If currentmat.Count(Function(c As Char) c = ControlChars.Lf) > 0 Then
                Dim newMat As New Material()
                Dim newMatName As String = String.Empty
                newMat = LoadFromString(currentmat, newMatName)
                result.Add(newMatName, newMat)
            End If
        Catch generatedExceptionName As FileNotFoundException
            Console.WriteLine("File not found: {0}", filename)
        Catch generatedExceptionName As Exception
            Console.WriteLine("Error loading file: {0}", filename)
        End Try

        Return result
    End Function

    Private Shared Function LoadFromString(mat As String, ByRef name As String) As Material
        Dim output As New Material()
        name = String.Empty
        Dim lines As List(Of String) = mat.Split(ControlChars.Lf).ToList()

        ' Skip until the material definition starts
        lines = lines.SkipWhile(Function(s) Not s.StartsWith("newmtl ")).ToList()

        ' Make sure an actual material was included
        If lines.Count <> 0 Then
            ' Get name from first line
            name = lines(0).Substring("newmtl ".Length)
        End If

        ' Remove leading whitespace
        lines = lines.[Select](Function(s As String) s.Trim()).ToList()

        ' Read material properties
        For Each line As String In lines
            ' Skip comments and blank lines
            If line.Length < 3 OrElse line.StartsWith("//") OrElse line.StartsWith("#") Then
                Continue For
            End If

            ' Parse ambient color
            If line.StartsWith("Ka") Then
                Dim colorparts As String() = line.Substring(3).Split(" "c)

                ' Check that all vector fields are present
                If colorparts.Length < 3 Then
                    Throw New ArgumentException("Invalid color data")
                End If

                Dim vec As New Vector3()

                ' Attempt to parse each part of the color
                Dim success As Boolean = Single.TryParse(colorparts(0), vec.X)
                success = success Or Single.TryParse(colorparts(1), vec.Y)
                success = success Or Single.TryParse(colorparts(2), vec.Z)

                output.AmbientColor = New Vector3(Single.Parse(colorparts(0)), Single.Parse(colorparts(1)), Single.Parse(colorparts(2)))

                ' If any of the parses failed, report the error
                If Not success Then
                    Console.WriteLine("Error parsing color: {0}", line)
                End If
            End If

            ' Parse diffuse color
            If line.StartsWith("Kd") Then
                Dim colorparts As String() = line.Substring(3).Split(" "c)

                ' Check that all vector fields are present
                If colorparts.Length < 3 Then
                    Throw New ArgumentException("Invalid color data")
                End If

                Dim vec As New Vector3()

                ' Attempt to parse each part of the color
                Dim success As Boolean = Single.TryParse(colorparts(0), vec.X)
                success = success Or Single.TryParse(colorparts(1), vec.Y)
                success = success Or Single.TryParse(colorparts(2), vec.Z)

                output.DiffuseColor = New Vector3(Single.Parse(colorparts(0)), Single.Parse(colorparts(1)), Single.Parse(colorparts(2)))

                ' If any of the parses failed, report the error
                If Not success Then
                    Console.WriteLine("Error parsing color: {0}", line)
                End If
            End If

            ' Parse specular color
            If line.StartsWith("Ks") Then
                Dim colorparts As String() = line.Substring(3).Split(" "c)

                ' Check that all vector fields are present
                If colorparts.Length < 3 Then
                    Throw New ArgumentException("Invalid color data")
                End If

                Dim vec As New Vector3()

                ' Attempt to parse each part of the color
                Dim success As Boolean = Single.TryParse(colorparts(0), vec.X)
                success = success Or Single.TryParse(colorparts(1), vec.Y)
                success = success Or Single.TryParse(colorparts(2), vec.Z)

                output.SpecularColor = New Vector3(Single.Parse(colorparts(0)), Single.Parse(colorparts(1)), Single.Parse(colorparts(2)))

                ' If any of the parses failed, report the error
                If Not success Then
                    Console.WriteLine("Error parsing color: {0}", line)
                End If
            End If

            ' Parse specular exponent
            If line.StartsWith("Ns") Then
                ' Attempt to parse each part of the color
                Dim exponent As Single = 0F
                Dim success As Boolean = Single.TryParse(line.Substring(3), exponent)

                output.SpecularExponent = exponent

                ' If any of the parses failed, report the error
                If Not success Then
                    Console.WriteLine("Error parsing specular exponent: {0}", line)
                End If
            End If

            ' Parse ambient map
            If line.StartsWith("map_Ka") Then
                ' Check that file name is present
                If line.Length > "map_Ka".Length + 6 Then
                    output.AmbientMap = line.Substring("map_Ka".Length + 1)
                End If
            End If

            ' Parse diffuse map
            If line.StartsWith("map_Kd") Then
                ' Check that file name is present
                If line.Length > "map_Kd".Length + 6 Then
                    output.DiffuseMap = line.Substring("map_Kd".Length + 1)
                End If
            End If

            ' Parse specular map
            If line.StartsWith("map_Ks") Then
                ' Check that file name is present
                If line.Length > "map_Ks".Length + 6 Then
                    output.SpecularMap = line.Substring("map_Ks".Length + 1)
                End If
            End If

            ' Parse normal map
            If line.StartsWith("map_normal") Then
                ' Check that file name is present
                If line.Length > "map_normal".Length + 6 Then
                    output.NormalMap = line.Substring("map_normal".Length + 1)
                End If
            End If

            ' Parse opacity map
            If line.StartsWith("map_opacity") Then
                ' Check that file name is present
                If line.Length > "map_opacity".Length + 6 Then
                    output.OpacityMap = line.Substring("map_opacity".Length + 1)
                End If

            End If
        Next
        Return output
    End Function
End Class