Imports OpenTK

Public Class Light
    Public Position As Vector3
    Public Color As New Vector3()
    Public DiffuseIntensity As Single = 1.0F
    Public AmbientIntensity As Single = 0.1F

    Public Sub New(position As Vector3, color As Vector3, Optional diffuseintensity As Single = 1.0F, Optional ambientintensity As Single = 1.0F)
        Me.Position = position
        Me.Color = color
        Me.DiffuseIntensity = diffuseintensity
        Me.AmbientIntensity = ambientintensity
    End Sub

End Class
