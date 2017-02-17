Imports System.IO
Imports OpenTK

Public MustInherit Class Scene


    Public Objects As New List(Of Volume)()
    Public Camera As New Camera()
    Public ActiveLight As New Light(New Vector3(), New Vector3(0.9F, 0.8F, 0.8F))

    Public Textures As New Dictionary(Of String, Integer)()
    Public Shaders As New Dictionary(Of String, ShaderProgram)()
    Public ActiveShader As String = "normal"
    Public Materials As New Dictionary(Of String, Material)()

    ' Number of Buffers created
    Public ibo_elements As Integer 'Buffer elements

    Public MustOverride Sub Load()

    Protected Sub loadMaterials(filename As String, Optional ImagesPath As String = "")
        For Each item In Material.LoadFromFile(filename)
            If Not Materials.ContainsKey(item.Key) Then
                Materials.Add(item.Key, item.Value)
            End If
        Next

        If (Not String.IsNullOrEmpty(ImagesPath) AndAlso Not ImagesPath.EndsWith("\")) Then
            ImagesPath &= "\"
        End If

        ' Load textures
        For Each mat As Material In Materials.Values
            If File.Exists(ImagesPath & mat.AmbientMap) AndAlso Not Textures.ContainsKey(mat.AmbientMap) Then
                Textures.Add(mat.AmbientMap, Images.LoadImage(ImagesPath & mat.AmbientMap))
            End If

            If File.Exists(ImagesPath & mat.DiffuseMap) AndAlso Not Textures.ContainsKey(mat.DiffuseMap) Then
                Textures.Add(mat.DiffuseMap, Images.LoadImage(ImagesPath & mat.DiffuseMap))
            End If

            If File.Exists(ImagesPath & mat.SpecularMap) AndAlso Not Textures.ContainsKey(mat.SpecularMap) Then
                Textures.Add(mat.SpecularMap, Images.LoadImage(ImagesPath & mat.SpecularMap))
            End If

            If File.Exists(ImagesPath & mat.NormalMap) AndAlso Not Textures.ContainsKey(mat.NormalMap) Then
                Textures.Add(mat.NormalMap, Images.LoadImage(ImagesPath & mat.NormalMap))
            End If

            If File.Exists(ImagesPath & mat.OpacityMap) AndAlso Not Textures.ContainsKey(mat.OpacityMap) Then
                Textures.Add(mat.OpacityMap, Images.LoadImage(ImagesPath & mat.OpacityMap))
            End If
        Next
    End Sub

End Class
