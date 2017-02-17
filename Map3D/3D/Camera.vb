Imports OpenTK

Public Class Camera
    Public Position As Vector3 = Vector3.Zero
    Public Orientation As New Vector3(CSng(Math.PI), 0.0F, 0F)
    Public MoveSpeed As Single = 0.02F
    Public MouseSensitivity As Single = 0.001F

    Private target As New Vector3(0F, 0F, 0F)

    Private right As Vector3 = Vector3.Zero
    Private Direction As Vector3 = New Vector3(0.0F, 0.0F, 1.0F)
    Private Up As Vector3 = New Vector3(0.0F, 1.0F, 0.0F)

    Private PitchAngle As Single = 0F
    Private YawAngle As Single = 0F
    Private Scale As Vector3 = New Vector3(2.0F)

    Public Sub New()
    End Sub

    Public Sub New(Position As Vector3, Orientation As Vector3)
        Me.Position = Position
        Me.Orientation = Orientation
    End Sub

    Public Function GetViewMatrix() As Matrix4
        Dim lookat As New Vector3()

        lookat.X = CSng(Math.Sin(CSng(Orientation.X)) * Math.Cos(CSng(Orientation.Y)))
        lookat.Y = CSng(Math.Sin(CSng(Orientation.Y)))
        lookat.Z = CSng(Math.Cos(CSng(Orientation.X)) * Math.Cos(CSng(Orientation.Y)))

        Return Matrix4.LookAt(Position, Position + lookat, Vector3.UnitY)
    End Function

    ' transform matricies

    Public Sub Orient(orientation As Quaternion)
        Dim newOrientation As Matrix4 = Matrix4.CreateFromQuaternion(orientation)
        Direction = New Vector3(newOrientation.M31, newOrientation.M32, newOrientation.M33)
        Up = New Vector3(newOrientation.M21, newOrientation.M22, newOrientation.M23)
        right = Vector3.Cross(Up, Direction).Normalized()
    End Sub


    Protected Function DegreeToRadian(angleInDegrees As Single) As Single
        Return CSng(Math.PI) * angleInDegrees / 180.0F
    End Function

    Public Sub mouseDeltaOrient(XDelta As Single, YDelta As Single)
        XDelta = XDelta * (MouseSensitivity * 6)
        YDelta = YDelta * (MouseSensitivity * 6)

        If PitchAngle > CSng(Math.PI) / 2.0F AndAlso PitchAngle < 1.5F * CSng(Math.PI) Then
            ' upside down
            XDelta *= -1.0F
        End If

        PitchAngle += DegreeToRadian(YDelta)
        YawAngle += DegreeToRadian(XDelta)

        Const twoPi As Single = CSng(2.0 * Math.PI)
        If PitchAngle < 0F Then
            PitchAngle += twoPi
        ElseIf PitchAngle > twoPi Then
            PitchAngle -= twoPi
        End If

        'var pitchOri = Quaternion.FromAxisAngle(this.Up, pitchAngle);
        Dim pitchOri = Quaternion.FromAxisAngle(Vector3.UnitY, -YawAngle)
        Dim yawOri = Quaternion.FromAxisAngle(Vector3.UnitX, -PitchAngle)
        Me.Orient(pitchOri * yawOri)

    End Sub

    Public Function rotationOnlyViewMatrixInverted() As Matrix4
        Dim mat As Matrix4 = Matrix4.Identity

        ' rotation..
        mat.M11 = right.X
        mat.M12 = right.Y
        mat.M13 = right.Z

        mat.M21 = Up.X
        mat.M22 = Up.Y
        mat.M23 = Up.Z

        mat.M31 = Direction.X
        mat.M32 = Direction.Y
        mat.M33 = Direction.Z

        mat.M41 = Position.X
        mat.M42 = Position.Y
        mat.M43 = Position.Z

        Return mat
    End Function

    Public Sub Move(x As Single, y As Single, z As Single)
        Dim offset As New Vector3()

        Dim forward As New Vector3(CSng(Math.Sin(CSng(Orientation.X))), 0, CSng(Math.Cos(CSng(Orientation.X))))
        Dim right As New Vector3(-forward.Z, 0, forward.X)

        offset += x * right
        offset += y * forward
        offset.Y += z

        offset.NormalizeFast()
        offset = Vector3.Multiply(offset, MoveSpeed)
        'Update position
        Position += offset
    End Sub

    Public Sub AddRotation(x As Single, y As Single)
        x = -x * MouseSensitivity
        y = -y * MouseSensitivity

        Orientation.X = (Orientation.X + x) Mod (CSng(Math.PI) * 2.0F)
        Orientation.Y = Math.Max(Math.Min(Orientation.Y + y, CSng(Math.PI) / 2.0F - 0.1F), CSng(-Math.PI) / 2.0F + 0.1F)
    End Sub

End Class
