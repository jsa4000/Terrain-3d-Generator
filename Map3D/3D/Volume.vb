Imports OpenTK

Public MustInherit Class Volume

    Public Position As Vector3 = Vector3.Zero
    Public Rotation As Vector3 = Vector3.Zero
    Public Scale As Vector3 = Vector3.One

    Public ModelMatrix As Matrix4 = Matrix4.Identity
    Public ViewProjectionMatrix As Matrix4 = Matrix4.Identity
    Public ModelViewProjectionMatrix As Matrix4 = Matrix4.Identity

    Public Overridable ReadOnly Property VerticeCount() As Integer
    Public Overridable ReadOnly Property IndiceCount() As Integer
    Public Overridable ReadOnly Property ColorDataCount() As Integer
    Public Overridable ReadOnly Property NormalCount() As Integer
        Get
            Return Normals.Length
        End Get
    End Property
    Public Overridable ReadOnly Property TextureCoordsCount() As Integer

    Private Normals As Vector3() = New Vector3(-1) {}

    Public Material As New Material()
    Public IsTextured As Boolean = False
    Public TextureID As Integer

    Public MustOverride Function GetVertices() As Vector3()
    Public MustOverride Function GetIndices(Optional offset As Integer = 0) As Integer()
    Public MustOverride Function GetColorData() As Vector3()
    Public MustOverride Function GetTextureCoords() As Vector2()

    Public MustOverride Sub CalculateModelMatrix()

    Public Overridable Function GetNormals() As Vector3()
        Return Normals
    End Function

    Public Sub CalculateNormals()
        Dim normals As Vector3() = New Vector3(VerticeCount - 1) {}
        Dim verts As Vector3() = GetVertices()
        Dim inds As Integer() = GetIndices()
        ' Compute normals for each face
        For i As Integer = 0 To IndiceCount - 1 Step 3
            Dim v1 As Vector3 = verts(inds(i))
            Dim v2 As Vector3 = verts(inds(i + 1))
            Dim v3 As Vector3 = verts(inds(i + 2))
            ' The normal is the cross-product of two sides of the triangle
            normals(inds(i)) += Vector3.Cross(v2 - v1, v3 - v1)
            normals(inds(i + 1)) += Vector3.Cross(v2 - v1, v3 - v1)
            normals(inds(i + 2)) += Vector3.Cross(v2 - v1, v3 - v1)
        Next
        'Normalize tthe Normal
        For i As Integer = 0 To NormalCount - 1
            normals(i) = normals(i).Normalized()
        Next
        normals = normals
    End Sub

End Class

