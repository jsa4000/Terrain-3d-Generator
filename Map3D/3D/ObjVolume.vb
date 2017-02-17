Imports System.IO
Imports OpenTK

Public Class FaceVertex
    Public Position As Vector3
    Public Normal As Vector3
    Public TextureCoord As Vector2

    Public Sub New(positon As Vector3, normal As Vector3, texcoord As Vector2)
        Me.Position = positon
        Me.Normal = normal
        Me.TextureCoord = texcoord
    End Sub
End Class

Public Class ObjVolume
    Inherits Volume

    Private faces As New List(Of Tuple(Of FaceVertex, FaceVertex, FaceVertex))()

    Public Overrides ReadOnly Property VerticeCount() As Integer
        Get
            Return faces.Count * 3
        End Get
    End Property

    Public Overrides ReadOnly Property IndiceCount() As Integer
        Get
            Return faces.Count * 3
        End Get
    End Property

    Public Overrides ReadOnly Property ColorDataCount() As Integer
        Get
            Return faces.Count * 3
        End Get
    End Property

    Public Overrides ReadOnly Property TextureCoordsCount() As Integer
        Get
            Return faces.Count * 3
        End Get
    End Property

    Public Overrides Function GetNormals() As Vector3()
        If MyBase.GetNormals().Length > 0 Then
            Return MyBase.GetNormals()
        End If

        Dim normals As New List(Of Vector3)()

        For Each face In faces
            normals.Add(face.Item1.Normal)
            normals.Add(face.Item2.Normal)
            normals.Add(face.Item3.Normal)
        Next

        Return normals.ToArray()
    End Function

    Public Overrides ReadOnly Property NormalCount() As Integer
        Get
            Return faces.Count * 3
        End Get
    End Property

    ''' <summary>
    ''' Get vertice data for this object
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Function GetVertices() As Vector3()
        Dim verts As New List(Of Vector3)()

        For Each face In faces
            verts.Add(face.Item1.Position)
            verts.Add(face.Item2.Position)
            verts.Add(face.Item3.Position)
        Next

        Return verts.ToArray()
    End Function

    ''' <summary>
    ''' Get indices
    ''' </summary>
    ''' <param name="offset"></param>
    ''' <returns></returns>
    Public Overrides Function GetIndices(Optional offset As Integer = 0) As Integer()
        Return Enumerable.Range(offset, IndiceCount).ToArray()
    End Function

    ''' <summary>
    ''' Get color data.
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Function GetColorData() As Vector3()
        Return New Vector3(ColorDataCount - 1) {}
    End Function

    ''' <summary>
    ''' Get texture coordinates.
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Function GetTextureCoords() As Vector2()
        Dim coords As New List(Of Vector2)()

        For Each face In faces
            coords.Add(face.Item1.TextureCoord)
            coords.Add(face.Item2.TextureCoord)
            coords.Add(face.Item3.TextureCoord)
        Next

        Return coords.ToArray()
    End Function

    ''' <summary>
    ''' Calculates the matrix model.
    ''' </summary>
    Public Overrides Sub CalculateModelMatrix()
        ModelMatrix = Matrix4.CreateScale(Scale) * Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationY(Rotation.Y) * Matrix4.CreateRotationZ(Rotation.Z) * Matrix4.CreateTranslation(Position)
    End Sub

    ''' <summary>
    ''' Loads a model from a file.
    ''' </summary>
    ''' <param name="filename">File to load model from</param>
    ''' <returns>ObjVolume of loaded model</returns>
    Public Shared Function LoadFromFile(filename As String) As ObjVolume
        Dim obj As New ObjVolume()
        Try
            Using reader As New StreamReader(New FileStream(filename, FileMode.Open, FileAccess.Read))
                obj = LoadFromString(reader.ReadToEnd())
            End Using
        Catch e As FileNotFoundException
            Console.WriteLine("File not found: {0}", filename)
        Catch e As Exception
            Console.WriteLine("Error loading file: {0}." & vbLf & "{1}", filename, e)
        End Try

        Return obj
    End Function

    Public Shared Function LoadFromString(obj As String) As ObjVolume
        ' Seperate lines from the file
        Dim lines As List(Of [String]) = New List(Of String)(obj.Split(ControlChars.Lf))

        ' Lists to hold model data
        Dim verts As New List(Of Vector3)()
        Dim normals As New List(Of Vector3)()
        Dim texs As New List(Of Vector2)()
        Dim faces As New List(Of Tuple(Of TempVertex, TempVertex, TempVertex))()

        ' Base values
        verts.Add(New Vector3())
        texs.Add(New Vector2())
        normals.Add(New Vector3())

        Dim currentindice As Integer = 0

        ' Read file line by line
        For Each line As [String] In lines
            If line.StartsWith("v ") Then
                ' Vertex definition
                ' Cut off beginning of line
                Dim temp As [String] = line.Substring(2)

                Dim vec As New Vector3()

                If temp.Trim().Count(Function(c As Char) c = " "c) = 2 Then
                    ' Check if there's enough elements for a vertex
                    Dim vertparts As [String]() = temp.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)

                    ' Attempt to parse each part of the vertice
                    Dim success As Boolean = Single.TryParse(vertparts(0), vec.X)
                    success = success Or Single.TryParse(vertparts(1), vec.Y)
                    success = success Or Single.TryParse(vertparts(2), vec.Z)

                    ' If any of the parses failed, report the error
                    If Not success Then
                        Console.WriteLine("Error parsing vertex: {0}", line)
                    End If
                Else
                    Console.WriteLine("Error parsing vertex: {0}", line)
                End If

                verts.Add(vec)
            ElseIf line.StartsWith("vt ") Then
                ' Texture coordinate
                ' Cut off beginning of line
                Dim temp As [String] = line.Substring(2)

                Dim vec As New Vector2()

                If temp.Trim().Count(Function(c As Char) c = " "c) > 0 Then
                    ' Check if there's enough elements for a vertex
                    Dim texcoordparts As [String]() = temp.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)

                    ' Attempt to parse each part of the vertice
                    Dim success As Boolean = Single.TryParse(texcoordparts(0), vec.X)
                    success = success Or Single.TryParse(texcoordparts(1), vec.Y)

                    ' If any of the parses failed, report the error
                    If Not success Then
                        Console.WriteLine("Error parsing texture coordinate: {0}", line)
                    End If
                Else
                    Console.WriteLine("Error parsing texture coordinate: {0}", line)
                End If

                texs.Add(vec)
            ElseIf line.StartsWith("vn ") Then
                ' Normal vector
                ' Cut off beginning of line
                Dim temp As [String] = line.Substring(2)

                Dim vec As New Vector3()

                If temp.Trim().Count(Function(c As Char) c = " "c) = 2 Then
                    ' Check if there's enough elements for a normal
                    Dim vertparts As [String]() = temp.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)

                    ' Attempt to parse each part of the vertice
                    Dim success As Boolean = Single.TryParse(vertparts(0), vec.X)
                    success = success Or Single.TryParse(vertparts(1), vec.Y)
                    success = success Or Single.TryParse(vertparts(2), vec.Z)

                    ' If any of the parses failed, report the error
                    If Not success Then
                        Console.WriteLine("Error parsing normal: {0}", line)
                    End If
                Else
                    Console.WriteLine("Error parsing normal: {0}", line)
                End If

                normals.Add(vec)
            ElseIf line.StartsWith("f ") Then
                ' Face definition
                ' Cut off beginning of line
                Dim temp As [String] = line.Substring(2)

                Dim face As New Tuple(Of TempVertex, TempVertex, TempVertex)(New TempVertex(), New TempVertex(), New TempVertex())

                If temp.Trim().Count(Function(c As Char) c = " "c) >= 2 Then
                    ' Check if there's enough elements for a face
                    Dim faceparts As [String]() = temp.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
                    Dim triangulate As Boolean = False

                    Dim v1 As Integer, v2 As Integer, v3 As Integer, v4 As Integer
                    Dim t1 As Integer, t2 As Integer, t3 As Integer, t4 As Integer
                    Dim n1 As Integer, n2 As Integer, n3 As Integer, n4 As Integer

                    ' Attempt to parse each part of the face
                    Dim success As Boolean = Integer.TryParse(faceparts(0).Split("/"c)(0), v1)
                    success = success Or Integer.TryParse(faceparts(1).Split("/"c)(0), v2)
                    success = success Or Integer.TryParse(faceparts(2).Split("/"c)(0), v3)
                    If (temp.Trim().Count(Function(c As Char) c = " "c) > 2) Then
                        success = success Or Integer.TryParse(faceparts(3).Split("/"c)(0), v4)
                        triangulate = True
                    End If

                    If faceparts(0).Count(Function(c As Char) c = "/"c) >= 2 Then
                        success = success Or Integer.TryParse(faceparts(0).Split("/"c)(1), t1)
                        success = success Or Integer.TryParse(faceparts(1).Split("/"c)(1), t2)
                        success = success Or Integer.TryParse(faceparts(2).Split("/"c)(1), t3)
                        success = success Or Integer.TryParse(faceparts(0).Split("/"c)(2), n1)
                        success = success Or Integer.TryParse(faceparts(1).Split("/"c)(2), n2)
                        success = success Or Integer.TryParse(faceparts(2).Split("/"c)(2), n3)

                        If (triangulate) Then
                            success = success Or Integer.TryParse(faceparts(3).Split("/"c)(1), t4)
                            success = success Or Integer.TryParse(faceparts(3).Split("/"c)(2), n4)
                        End If
                    Else
                        If texs.Count > v1 AndAlso texs.Count > v2 AndAlso texs.Count > v3 Then
                            t1 = v1
                            t2 = v2
                            t3 = v3
                        Else
                            t1 = 0
                            t2 = 0
                            t3 = 0
                        End If

                        If normals.Count > v1 AndAlso normals.Count > v2 AndAlso normals.Count > v3 Then
                            n1 = v1
                            n2 = v2
                            n3 = v3
                        Else
                            n1 = 0
                            n2 = 0
                            n3 = 0
                        End If

                        If (triangulate) Then
                            If texs.Count > v4 Then
                                t4 = v4
                            Else
                                t4 = 0
                            End If
                            If normals.Count > v4 Then
                                n4 = v4
                            Else
                                n4 = 0
                            End If
                        End If

                    End If

                    ' If any of the parses failed, report the error
                    If Not success Then
                        Console.WriteLine("Error parsing face: {0}", line)
                    Else
                        Dim tv1 As New TempVertex(v1, n1, t1)
                        Dim tv2 As New TempVertex(v2, n2, t2)
                        Dim tv3 As New TempVertex(v3, n3, t3)
                        Dim tv4 As TempVertex = Nothing
                        face = New Tuple(Of TempVertex, TempVertex, TempVertex)(tv1, tv2, tv3)
                        faces.Add(face)
                        If (triangulate) Then
                            tv4 = New TempVertex(v4, n4, t4)
                            face = New Tuple(Of TempVertex, TempVertex, TempVertex)(tv1, tv3, tv4)
                            faces.Add(face)
                        End If
                    End If
                Else
                    Console.WriteLine("Error parsing face: {0}", line)
                End If
            End If
        Next

        ' Create the ObjVolume
        Dim vol As New ObjVolume()

        For Each face In faces
            Dim v1 As New FaceVertex(verts(face.Item1.Vertex), normals(face.Item1.Normal), texs(face.Item1.Texcoord))
            Dim v2 As New FaceVertex(verts(face.Item2.Vertex), normals(face.Item2.Normal), texs(face.Item2.Texcoord))
            Dim v3 As New FaceVertex(verts(face.Item3.Vertex), normals(face.Item3.Normal), texs(face.Item3.Texcoord))

            vol.faces.Add(New Tuple(Of FaceVertex, FaceVertex, FaceVertex)(v1, v2, v3))
        Next

        Return vol
    End Function

    Private Class TempVertex
        Public Vertex As Integer
        Public Normal As Integer
        Public Texcoord As Integer

        Public Sub New(Optional vert As Integer = 0, Optional norm As Integer = 0, Optional tex As Integer = 0)
            Vertex = vert
            Normal = norm
            Texcoord = tex
        End Sub
    End Class
End Class
