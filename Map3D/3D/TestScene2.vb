
Imports System.IO
Imports OpenTK
Imports OpenTK.Graphics.OpenGL

Public Class TestScene2
    Inherits Scene

    Public Overrides Sub Load()
        GL.GenBuffers(1, ibo_elements)

        ' Load shaders from file
        Shaders.Add("default", New ShaderProgram("vs.glsl", "fs.glsl", True))
        Shaders.Add("textured", New ShaderProgram("vs_tex.glsl", "fs_tex.glsl", True))
        Shaders.Add("normal", New ShaderProgram("vs_norm.glsl", "fs_norm.glsl", True))
        Shaders.Add("lit", New ShaderProgram("vs_lit.glsl", "fs_lit.glsl", True))
        ActiveShader = "lit"

        Dim path As String = "C:\Development\VB2015\Map3D\Debug\Content\assets"

        'Load materials for the current scene.
        loadMaterials(path & "\export.mtl", path)
        ' Create our objects
        Dim tc As ObjVolume = ObjVolume.LoadFromFile(path & "\export.obj")
        tc.TextureID = Textures(Materials("Matrix").DiffuseMap)
        tc.CalculateNormals()
        tc.Material = Materials("Matrix")
        Objects.Add(tc)


        ' Move camera away from origin
        Camera.Position += New Vector3(0F, 0F, 3.0F)
    End Sub

End Class
