Imports OpenTK

Module Geometry

    Public Class Cube
        Inherits Volume

        Public Overrides ReadOnly Property VerticeCount() As Integer
            Get
                Return 8
            End Get
        End Property

        Public Overrides ReadOnly Property IndiceCount() As Integer
            Get
                Return 36
            End Get
        End Property

        Public Overrides ReadOnly Property ColorDataCount() As Integer
            Get
                Return 8
            End Get
        End Property

        Public Sub New()

        End Sub

        Public Overrides Function GetVertices() As Vector3()
            Return New Vector3() {New Vector3(-0.5F, -0.5F, -0.5F),
                                New Vector3(0.5F, -0.5F, -0.5F),
                                New Vector3(0.5F, 0.5F, -0.5F),
                                New Vector3(-0.5F, 0.5F, -0.5F),
                                New Vector3(-0.5F, -0.5F, 0.5F),
                                New Vector3(0.5F, -0.5F, 0.5F),
                                New Vector3(0.5F, 0.5F, 0.5F),
                                New Vector3(-0.5F, 0.5F, 0.5F)}
        End Function

        Public Overrides Function GetIndices(Optional offset As Integer = 0) As Integer()
            Dim inds As Integer() = New Integer() {
            0, 2, 1, 'left
            0, 3, 2, '
            1, 2, 6, 'back
            6, 5, 1, '
            4, 5, 6, 'right
            6, 7, 4, '
            2, 3, 6, 'top
            6, 3, 7, '
            0, 7, 3, 'front
            0, 4, 7, '
            0, 1, 5, 'bottom
            0, 5, 4} '

            ' This offset is use because some tools start from 0 instead 1 and viceversa
            If offset <> 0 Then
                For i As Integer = 0 To inds.Length - 1
                    inds(i) += offset
                Next
            End If
            Return inds
        End Function

        Public Overrides Function GetColorData() As Vector3()
            Return New Vector3() {New Vector3(1.0F, 0F, 0F),
                                New Vector3(0F, 0F, 1.0F),
                                New Vector3(0F, 1.0F, 0F),
                                New Vector3(1.0F, 0F, 0F),
                                New Vector3(0F, 0F, 1.0F),
                                New Vector3(0F, 1.0F, 0F),
                                New Vector3(1.0F, 0F, 0F),
                                New Vector3(0F, 0F, 1.0F)}
        End Function

        Public Overrides Sub CalculateModelMatrix()
            ModelMatrix = Matrix4.CreateScale(Scale) * Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationY(Rotation.Y) * Matrix4.CreateRotationZ(Rotation.Z) * Matrix4.CreateTranslation(Position)
        End Sub

        Public Overrides Function GetTextureCoords() As Vector2()
            Return New Vector2() {}
        End Function
    End Class

    Class TexturedCube
        Inherits Cube

        Public Overrides ReadOnly Property VerticeCount() As Integer
            Get
                Return 24
            End Get
        End Property

        Public Overrides ReadOnly Property IndiceCount() As Integer
            Get
                Return 36
            End Get
        End Property

        Public Overrides ReadOnly Property ColorDataCount() As Integer
            Get
                Return 24
            End Get
        End Property

        Public Sub New()
            MyBase.New()

        End Sub

        Public Overrides Function GetVertices() As Vector3()
            Return New Vector3() {New Vector3(-0.5F, -0.5F, -0.5F), New Vector3(0.5F, 0.5F, -0.5F), New Vector3(0.5F, -0.5F, -0.5F), New Vector3(-0.5F, 0.5F, -0.5F), New Vector3(0.5F, -0.5F, -0.5F), New Vector3(0.5F, 0.5F, -0.5F),
            New Vector3(0.5F, 0.5F, 0.5F), New Vector3(0.5F, -0.5F, 0.5F), New Vector3(-0.5F, -0.5F, 0.5F), New Vector3(0.5F, -0.5F, 0.5F), New Vector3(0.5F, 0.5F, 0.5F), New Vector3(-0.5F, 0.5F, 0.5F),
            New Vector3(0.5F, 0.5F, -0.5F), New Vector3(-0.5F, 0.5F, -0.5F), New Vector3(0.5F, 0.5F, 0.5F), New Vector3(-0.5F, 0.5F, 0.5F), New Vector3(-0.5F, -0.5F, -0.5F), New Vector3(-0.5F, 0.5F, 0.5F),
            New Vector3(-0.5F, 0.5F, -0.5F), New Vector3(-0.5F, -0.5F, 0.5F), New Vector3(-0.5F, -0.5F, -0.5F), New Vector3(0.5F, -0.5F, -0.5F), New Vector3(0.5F, -0.5F, 0.5F), New Vector3(-0.5F, -0.5F, 0.5F)}
        End Function

        Public Overrides Function GetIndices(Optional offset As Integer = 0) As Integer()
            Dim inds As Integer() = New Integer() {0, 1, 2, 0, 3, 1,
            4, 5, 6, 4, 6, 7,
            8, 9, 10, 8, 10, 11,
            13, 14, 12, 13, 15, 14,
            16, 17, 18, 16, 19, 17,
            20, 21, 22, 20, 22, 23}

            If offset <> 0 Then
                For i As Integer = 0 To inds.Length - 1
                    inds(i) += offset
                Next
            End If
            Return inds
        End Function

        Public Overrides Function GetTextureCoords() As Vector2()
            Return New Vector2() {New Vector2(0F, 0F), New Vector2(-1.0F, 1.0F), New Vector2(-1.0F, 0F), New Vector2(0F, 1.0F), New Vector2(0F, 0F), New Vector2(0F, 1.0F),
            New Vector2(-1.0F, 1.0F), New Vector2(-1.0F, 0F), New Vector2(-1.0F, 0F), New Vector2(0F, 0F), New Vector2(0F, 1.0F), New Vector2(-1.0F, 1.0F),
            New Vector2(0F, 0F), New Vector2(0F, 1.0F), New Vector2(-1.0F, 0F), New Vector2(-1.0F, 1.0F), New Vector2(0F, 0F), New Vector2(1.0F, 1.0F),
            New Vector2(0F, 1.0F), New Vector2(1.0F, 0F), New Vector2(0F, 0F), New Vector2(0F, 1.0F), New Vector2(-1.0F, 1.0F), New Vector2(-1.0F, 0F)}
        End Function

        Public Overrides Function GetColorData() As Vector3()
            Return MyBase.GetColorData().Concat(MyBase.GetColorData().Concat(MyBase.GetColorData())).ToArray()
        End Function
    End Class

End Module