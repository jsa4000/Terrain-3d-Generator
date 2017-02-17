
Imports System.IO
Imports OpenTK
Imports OpenTK.Graphics.OpenGL

Public Class TestScene
    Inherits Scene

    Public Overrides Sub Load()
        GL.GenBuffers(1, ibo_elements)

        ' Load shaders from file
        Shaders.Add("default", New ShaderProgram("vs.glsl", "fs.glsl", True))
        Shaders.Add("textured", New ShaderProgram("vs_tex.glsl", "fs_tex.glsl", True))
        Shaders.Add("normal", New ShaderProgram("vs_norm.glsl", "fs_norm.glsl", True))
        Shaders.Add("lit", New ShaderProgram("vs_lit.glsl", "fs_lit.glsl", True))

        ActiveShader = "lit"

        'Load materials for the current scene.
        loadMaterials("opentk.mtl")

        ' Create our objects
        Dim tc As New TexturedCube()
        tc.TextureID = Textures(Materials("opentk1").DiffuseMap)
        tc.CalculateNormals()
        tc.Material = Materials("opentk1")
        Objects.Add(tc)

        Dim tc2 As New TexturedCube()
        tc2.Position += New Vector3(1.0F, 1.0F, 1.0F)
        tc2.TextureID = Textures(Materials("opentk2").DiffuseMap)
        tc2.CalculateNormals()
        tc2.Material = Materials("opentk2")
        Objects.Add(tc2)

        ' Move camera away from origin
        Camera.Position += New Vector3(0.0F, 0.0F, 3.0F)

        Textures.Add("earth.png", Images.LoadImage("earth.png"))
        Dim earth As ObjVolume = ObjVolume.LoadFromFile("earth.obj")
        earth.TextureID = Textures("earth.png")
        earth.Position += New Vector3(1.0F, 1.0F, -2.0F)
        earth.Material = New Material(New Vector3(0.15F), New Vector3(1), New Vector3(0.2F), 5)
        Objects.Add(earth)

    End Sub

End Class
