Namespace Terrain

    Public Class Terrain3D
        Public Name As String = String.Empty
        Public Vertices As New List(Of Vector3D)
        Public VertexColor As New List(Of Vector3D)
        Public TextureCoordinates As New List(Of Vector2D)
        Public Normals As New List(Of Vector3D)
        Public faces As New List(Of Polygon3D)
        Public VertexDictionary As New Dictionary(Of Integer, List(Of VertexData))

        Public Shared Function Create(heightMatrix As Matrix2D, Optional heightfactor As Double = 1, Optional scaleFactor As Double = 3, Optional invert As Boolean = False) As Terrain3D
            Dim result As New Terrain3D()
            result.Load(heightMatrix, heightfactor, scaleFactor, invert)
            Return result
        End Function

        Public Sub Load(heightMatrix As Matrix2D, Optional heightfactor As Double = 1, Optional scaleFactor As Double = 3, Optional invert As Boolean = False)
            'DEFAULT NAME WILL BE THE NAME OF THE FILE WITHOUT THE FILE NAME EXTENSION
            Me.Name = My.Computer.FileSystem.GetName(heightMatrix.FilePath).Split(".")(0)
            ' Get the height and the Width
            Dim height As Double = heightMatrix.Height
            Dim width As Double = heightMatrix.Width
            Dim maxvalue As Double = heightMatrix.Maxvalue

            ''''''''''''''''''''''''''''''''
            ' COMPUTE VERTICES (NORMALIZED)           
            ''''''''''''''''''''''''''''''''
            Dim offsetX As Double = scaleFactor / height
            Dim offsetZ As Double = scaleFactor / width

            For i As Integer = 0 To height - 1
                For j As Integer = 0 To width - 1
                    Dim vertex As New Vector3D()
                    vertex.X = (i * offsetX) - (scaleFactor / 2) 'Center into the world axis
                    vertex.Y = 0
                    If (maxvalue <> 0) Then
                        vertex.Y = (heightMatrix.Matrix(i, j).Value / maxvalue) * heightfactor 'Normalized result with scaled factor
                    End If
                    If (invert) Then vertex.Y = 1 - vertex.Y
                    vertex.Z = (j * offsetZ) - (scaleFactor / 2) 'Center into the world axis
                    'Add the normalized vertex
                    Me.Vertices.Add(vertex)
                    'Add Color Vertex information
                    Dim vertexColor As New Vector3D()
                    vertexColor.X = heightMatrix.Matrix(i, j).Coords.X
                    vertexColor.Y = heightMatrix.Matrix(i, j).Coords.Y
                    vertexColor.Z = heightMatrix.Matrix(i, j).RealValue
                    'Add the normalized vertex
                    Me.VertexColor.Add(vertexColor)
                Next
            Next

            ''''''''''''''''''''''''''''''''
            ' COMPUTE TEXTURE INDEXES
            ''''''''''''''''''''''''''''''''
            '(UNTIL PREVIOUS VERTICES OF THE MATRIX = -2)

            ' In this case each texture id will be a fragment per vertice for the entire texture (from 0:0 to 1:1) uv
            ' 3x3 image (where each facce it's a portion of the image))
            ' 0 0 0  ..  
            ' 0 0 0  
            ' 0 0 0
            ' ..
            ' 512       512 x 512 (1x1)
            ' 1 / vertices -> number of tiles. The texture must be square
            ' 4x4 vertices 1/4 0.25 increment per vertice
            ' (0,0)  0, 0.
            ' (0,1)  0, 0.25
            ' (0,1)  0.25, 0.5
            ' (0,2) ..
            ' (1,0) 0.25, 0
            ' (1,1) 0.25, 0.25

            Dim hIncrement As Double = 1 / (width - 1)
            Dim vIncrement As Double = 1 / (height - 1)

            ' TO-DO Seems to be rotated
            'For i As Integer = Height - 1 To 0 Step -1
            'For i As Integer = 0 To Height - 1
            'For j As Integer = 0 To Width - 1
            For i As Integer = 0 To height - 1
                For j As Integer = width - 1 To 0 Step -1
                    Dim textCoords As New Vector2D
                    textCoords.X = i * vIncrement
                    textCoords.Y = j * hIncrement
                    Me.TextureCoordinates.Add(textCoords)
                Next
            Next

            ''''''''''''''''''''''''''''''''
            ' COMPUTE POLYGONS 
            ''''''''''''''''''''''''''''''''
            '(UNTIL PREVIOUS VERTICES OF THE MATRIX = -2)

            ' In this case each face will be composed from the neighbours vertices in the Matrix (Clock-wise)
            ' 0 1 2  3 
            ' 4 5 6  7
            ' 8 9 10 11
            ' Faces -> [0,1,5,4], [1,2,6,5], .., [6, 7, 11, 10]
            For i As Integer = 0 To height - 2
                For j As Integer = 0 To width - 2
                    Dim face As New Polygon3D()
                    For index As Integer = 0 To 3
                        Dim vertexData As New VertexData()
                        Dim vertexPosition As Double = 0
                        Select Case index
                            Case 0 ' (0,0)
                                vertexPosition = (i * width) + j
                            Case 1 ' (0,1)
                                vertexPosition = (i * width) + (j + 1)
                            Case 2 ' (1,1)
                                vertexPosition = ((i + 1) * width) + (j + 1)
                            Case 3 ' (1,0)
                                vertexPosition = ((i + 1) * width) + j
                        End Select
                        ' BLENDER start from Id = 1 instead 0
                        vertexData.Vertex = New Tuple(Of Integer, Vector3D)(vertexPosition + 1, Me.Vertices(vertexPosition))
                        ' SET TEXTURE CORRDIANTE
                        vertexData.TextureCoordinate = New Tuple(Of Integer, Vector2D)(vertexPosition + 1, Me.TextureCoordinates(vertexPosition))
                        'Add vertex data to the vertices of the poly
                        face.Vertices.Add(vertexData)
                        'Add this vertez daa to the global dictionary for this mesh
                        If (Not Me.VertexDictionary.ContainsKey(vertexData.Vertex.Item1)) Then
                            Me.VertexDictionary.Add(vertexData.Vertex.Item1, New List(Of VertexData))
                        End If
                        ' Finally add this into the dictionary
                        Me.VertexDictionary(vertexData.Vertex.Item1).Add(vertexData)
                    Next
                    'Add the normalized vertex
                    Me.faces.Add(face)
                Next
            Next

            ''''''''''''''''''''''''''''''''
            ' COMPUTE NORMALS
            ''''''''''''''''''''''''''''''''
            ' Each Face will have its own smoothing group or vertex normal by default to the average of the vectors. (Local Normal of the face)
            'WRITE ALL THE FACES OF THE MESH
            For Each face As Polygon3D In Me.faces
                Dim normal As Vector3D = face.ComputeNormals()
                'Add the normals for this face into the global normals
                Me.Normals.Add(normal)
                ' Set the same id normal for all the vertices of the face
                For Each vertex As VertexData In face.Vertices
                    ' This takes into accont BLENDERS issue to starts from 1 instead 0 index
                    vertex.Normal = New Tuple(Of Integer, Vector3D)(Me.Normals.Count, normal)
                Next
            Next
        End Sub

        Public Sub Smooth()
            Me.Normals.Clear()
            ' This operation will searh for vertices in the same direction  (angle )
            For Each vertexItem In Me.VertexDictionary
                'Compute the average for all the vertex position
                Dim smoothNormal As New Vector3D()
                Me.Normals.Add(smoothNormal)
                For Each vertex As VertexData In vertexItem.Value
                    smoothNormal.X += vertex.Normal.Item2.X
                    smoothNormal.Y += vertex.Normal.Item2.Y
                    smoothNormal.Z += vertex.Normal.Item2.Z
                Next
                smoothNormal /= vertexItem.Value.Count ' Average or mean
                smoothNormal /= 1 'Normalize the result
                'SE this new vertez normal for all the vertex data
                For Each vertex As VertexData In vertexItem.Value
                    vertex.Normal = New Tuple(Of Integer, Vector3D)(Me.Normals.Count, smoothNormal)
                Next
            Next
        End Sub
    End Class

End Namespace
