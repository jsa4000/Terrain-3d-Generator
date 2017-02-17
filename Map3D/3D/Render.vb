Imports OpenTK
Imports OpenTK.Graphics.OpenGL
Public Class Render

    ' Install Open TK dependences from Nugets command line
    '    - Install-Package OpenTK
    '    - Install-Package OpenTK.GLControl
    ' Samples
    ' https://gist.github.com/GeirGrusom/347f30e981f33972c934
    'http://neokabuto.blogspot.com.es/2014/01/opentk-tutorial-5-basic-camera.html

    ' Vertex info to render
    Private vertdata As Vector3()
    Private coldata As Vector3()
    Private texcoorddata As Vector2()
    Private normdata As Vector3()
    Private indicedata As Integer()

    Private WindowHeight As Integer = 800
    Private WindowWidth As Integer = 600

    'Current scene that is rendered
    Private CurrentScene As Scene
    Private View As Matrix4 = Matrix4.Identity
    Private lastMousePos As Vector2 = Nothing

    Public Sub New()
    End Sub

    Public Sub SetScene(Scene As Scene)
        ' Set and load current scene
        CurrentScene = Scene
        CurrentScene.Load()
    End Sub

    Public Function GetVersion() As String
        Return GL.GetString(StringName.Vendor) & " " & GL.GetString(StringName.Renderer) & " " & GL.GetString(StringName.Version)
    End Function

    Public Sub Init(Width As Integer, Height As Integer)
        ' Enable the OpenGL Control
        GL.ClearColor(Color.Gray)
        GL.Enable(EnableCap.DepthTest)
        GL.PointSize(5.0F)
        ' Forze to update the first time the render view
        ResizeWindow(Width, Height)
    End Sub

    Public Sub ResizeWindow(Width As Integer, Height As Integer)
        Me.WindowWidth = Width
        Me.WindowHeight = Height
        GL.Viewport(0, 0, Width, Height)
        Dim aspect_ratio As Single = Width / CSng(Height)
        Dim perpective As Matrix4 = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 64)
        GL.MatrixMode(MatrixMode.Projection)
        GL.LoadMatrix(perpective)
    End Sub

    Public Sub KeyDown(Key As KeyEventArgs)
        Select Case Key.KeyData
            Case Keys.W
                CurrentScene.Camera.Move(0F, 0.1F, 0F)
            Case Keys.A
                CurrentScene.Camera.Move(-0.1F, 0F, 0F)
            Case Keys.S
                CurrentScene.Camera.Move(0F, -0.1F, 0F)
            Case Keys.D
                CurrentScene.Camera.Move(0.1F, 0F, 0F)
            Case Keys.Q
                CurrentScene.Camera.Move(0F, 0F, 0.1F)
            Case Keys.E
                CurrentScene.Camera.Move(0F, 0F, -0.1F)
        End Select
    End Sub

    Public Sub MouseMove(mouse As MouseEventArgs)
        If (mouse.Button = MouseButtons.Left) Then
            Dim currentMousePos As Vector2 = New Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y)
            If (lastMousePos = Nothing) Then
                lastMousePos = New Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y)
            End If
            Dim delta As Vector2 = lastMousePos - currentMousePos
            CurrentScene.Camera.AddRotation(delta.X, delta.Y)
            'CurrentScene.Camera.mouseDeltaOrient(delta.X, delta.Y)
            lastMousePos = currentMousePos
        ElseIf (mouse.Button = MouseButtons.Middle) Then
                Dim currentMousePos As Vector2 = New Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y)
            Dim delta As Vector2 = lastMousePos - currentMousePos
            CurrentScene.Camera.Move(delta.X, 0F, -delta.Y)
            lastMousePos = currentMousePos
        End If
    End Sub

    Public Sub MouseDown(mouse As MouseEventArgs)
        lastMousePos = New Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y)
    End Sub

    Public Sub MouseUp(mouse As MouseEventArgs)
        lastMousePos = Nothing ' Initialize the vector
    End Sub

    Public Sub MouseWheel(mouse As MouseEventArgs)
        If (mouse.Delta > 0) Then
            CurrentScene.Camera.Move(0F, 0.4F, 0F)
        Else
            CurrentScene.Camera.Move(0F, -0.4F, 0F)
        End If
    End Sub

    Public Sub UpdateFrame()
        Dim verts As New List(Of Vector3)()
        Dim inds As New List(Of Integer)()
        Dim colors As New List(Of Vector3)()
        Dim texcoords As New List(Of Vector2)()
        Dim normals As New List(Of Vector3)()

        ' Assemble vertex and indice data for all volumes
        Dim vertcount As Integer = 0
        For Each v As Volume In CurrentScene.Objects
            verts.AddRange(v.GetVertices().ToList())
            inds.AddRange(v.GetIndices(vertcount).ToList())
            colors.AddRange(v.GetColorData().ToList())
            texcoords.AddRange(v.GetTextureCoords())
            normals.AddRange(v.GetNormals().ToList())
            vertcount += v.VerticeCount
        Next

        vertdata = verts.ToArray()
        indicedata = inds.ToArray()
        coldata = colors.ToArray()
        texcoorddata = texcoords.ToArray()
        normdata = normals.ToArray()

        ''Get the current Active shader loaded
        Dim ActiveShader As ShaderProgram = CurrentScene.Shaders(CurrentScene.ActiveShader)
        GL.BindBuffer(BufferTarget.ArrayBuffer, ActiveShader.GetBuffer("vPosition"))

        GL.BufferData(Of Vector3)(BufferTarget.ArrayBuffer, New System.IntPtr(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw)
        GL.VertexAttribPointer(ActiveShader.GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, False, 0, 0)

        ' Buffer vertex color if shader supports it
        If ActiveShader.GetAttribute("vColor") <> -1 Then
            GL.BindBuffer(BufferTarget.ArrayBuffer, ActiveShader.GetBuffer("vColor"))
            GL.BufferData(Of Vector3)(BufferTarget.ArrayBuffer, New System.IntPtr(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw)
            GL.VertexAttribPointer(ActiveShader.GetAttribute("vColor"), 3, VertexAttribPointerType.Float, True, 0, 0)
        End If

        ' Buffer texture coordinates if shader supports it
        If ActiveShader.GetAttribute("texcoord") <> -1 Then
            GL.BindBuffer(BufferTarget.ArrayBuffer, ActiveShader.GetBuffer("texcoord"))
            GL.BufferData(Of Vector2)(BufferTarget.ArrayBuffer, New System.IntPtr(texcoorddata.Length * Vector2.SizeInBytes), texcoorddata, BufferUsageHint.StaticDraw)
            GL.VertexAttribPointer(ActiveShader.GetAttribute("texcoord"), 2, VertexAttribPointerType.Float, True, 0, 0)
        End If

        If ActiveShader.GetAttribute("vNormal") <> -1 Then
            GL.BindBuffer(BufferTarget.ArrayBuffer, ActiveShader.GetBuffer("vNormal"))
            GL.BufferData(Of Vector3)(BufferTarget.ArrayBuffer, New System.IntPtr(normdata.Length * Vector3.SizeInBytes), normdata, BufferUsageHint.StaticDraw)
            GL.VertexAttribPointer(ActiveShader.GetAttribute("vNormal"), 3, VertexAttribPointerType.Float, True, 0, 0)
        End If

        ' Update model view matrices
        For Each v As Volume In CurrentScene.Objects
            v.CalculateModelMatrix()
            'v.ViewProjectionMatrix = CurrentScene.Camera.rotationOnlyViewMatrixInverted().Inverted() * Matrix4.CreatePerspectiveFieldOfView(0.7F, WindowWidth / CSng(WindowHeight), 0.1F, 70.0F)
            v.ViewProjectionMatrix = CurrentScene.Camera.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(0.7F, WindowWidth / CSng(WindowHeight), 0.1F, 70.0F)
            v.ModelViewProjectionMatrix = v.ModelMatrix * v.ViewProjectionMatrix
        Next

        GL.UseProgram(ActiveShader.ProgramID)
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0)

        ' Buffer index data
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, CurrentScene.ibo_elements)
        GL.BufferData(BufferTarget.ElementArrayBuffer, New System.IntPtr(indicedata.Length * 4), indicedata, BufferUsageHint.StaticDraw)

        View = CurrentScene.Camera.GetViewMatrix()
        'View = CurrentScene.Camera.rotationOnlyViewMatrixInverted().Inverted()
    End Sub

    Public Sub RenderFrame()
        GL.Viewport(0, 0, WindowWidth, WindowHeight)
        GL.Clear(ClearBufferMask.ColorBufferBit Or ClearBufferMask.DepthBufferBit)
        GL.Enable(EnableCap.DepthTest)

        'Get the current Active shader loaded
        Dim ActiveShader As ShaderProgram = CurrentScene.Shaders(CurrentScene.ActiveShader)

        GL.UseProgram(ActiveShader.ProgramID)
        ActiveShader.EnableVertexAttribArrays()

        Dim indiceat As Integer = 0

        ' Draw all objects
        For Each v As Volume In CurrentScene.Objects
            GL.BindTexture(TextureTarget.Texture2D, v.TextureID)
            GL.UniformMatrix4(ActiveShader.GetUniform("modelview"), False, v.ModelViewProjectionMatrix)

            If ActiveShader.GetAttribute("maintexture") <> -1 Then
                GL.Uniform1(ActiveShader.GetAttribute("maintexture"), v.TextureID)
            End If

            If ActiveShader.GetUniform("view") <> -1 Then
                GL.UniformMatrix4(CurrentScene.Shaders(CurrentScene.ActiveShader).GetUniform("view"), False, View)
            End If

            If ActiveShader.GetUniform("model") <> -1 Then
                GL.UniformMatrix4(ActiveShader.GetUniform("model"), False, v.ModelMatrix)
            End If

            If ActiveShader.GetUniform("material_ambient") <> -1 Then
                'GL.Uniform3(v.Material.AmbientColor, ActiveShader.GetUniform("material_ambient"))
            End If

            If ActiveShader.GetUniform("material_diffuse") <> -1 Then
                'GL.Uniform3(v.Material.DiffuseColor, ActiveShader.GetUniform("material_diffuse"))
            End If

            If ActiveShader.GetUniform("material_specular") <> -1 Then
                'GL.Uniform3(v.Material.SpecularColor, ActiveShader.GetUniform("material_specular"))
            End If

            If ActiveShader.GetUniform("material_specExponent") <> -1 Then
                GL.Uniform1(ActiveShader.GetUniform("material_specExponent"), v.Material.SpecularExponent)
            End If

            If ActiveShader.GetUniform("light_position") <> -1 Then
                'GL.Uniform3(CurrentScene.ActiveLight.Position, ActiveShader.GetUniform("light_position"))
            End If

            If ActiveShader.GetUniform("light_color") <> -1 Then
                'GL.Uniform3(CurrentScene.ActiveLight.Color, ActiveShader.GetUniform("light_color"))
            End If

            If ActiveShader.GetUniform("light_diffuseIntensity") <> -1 Then
                GL.Uniform1(ActiveShader.GetUniform("light_diffuseIntensity"), CurrentScene.ActiveLight.DiffuseIntensity)
            End If

            If ActiveShader.GetUniform("light_ambientIntensity") <> -1 Then
                GL.Uniform1(ActiveShader.GetUniform("light_ambientIntensity"), CurrentScene.ActiveLight.AmbientIntensity)
            End If

            GL.DrawElements(BeginMode.Triangles, v.IndiceCount, DrawElementsType.UnsignedInt, indiceat * 4)
            indiceat += v.IndiceCount
        Next
        ActiveShader.DisableVertexAttribArrays()

        GL.Flush()
    End Sub



End Class
