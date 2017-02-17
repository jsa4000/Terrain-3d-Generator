Imports OpenTK

Public Class Viewer3DControl

    Private WithEvents glControl As New OpenTK.GLControl()
    Private WithEvents glTimer As New Timer()

    Private Currentscene As Scene = Nothing
    Private Render As New Render()

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        glControl.Dock = DockStyle.Fill
        Me.Controls.Add(glControl)

        'Configure the timer if animations or update in real-time
        glTimer.Interval = 1000 / 100 ' fps for the timer
        glTimer.Start()
    End Sub

    Private Sub Viewer3DControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = Render.GetVersion()
        ' Init Render engine
        Render.Init(glControl.ClientSize.Width, glControl.ClientSize.Height)
        ' Set the default scene
        Currentscene = New TestScene()
        Render.SetScene(Currentscene)
    End Sub

    Private Sub glTimer_Tick(sender As Object, e As EventArgs) Handles glTimer.Tick
        'Force the render to update the gui
        'While glControl.IsIdle
        glControl.Invalidate()
        'End While
    End Sub

    Private Sub glControl_KeyUp(sender As Object, e As KeyEventArgs) Handles glControl.KeyUp
        If e.KeyCode = Keys.F12 Then
            Images.GrabScreenshot(Me.Width, Me.Height, Me.ClientRectangle).Save("screenshot.png")
        ElseIf e.KeyCode = Keys.F11 Then
            Currentscene = New TestScene2()
            Render.SetScene(Currentscene)
        ElseIf e.KeyCode = Keys.F10 Then
            Currentscene.ActiveShader = "default"
        ElseIf e.KeyCode = Keys.F9 Then
            Currentscene.Materials("Matrix").Opacity -= 0.1F
            Currentscene.Materials("Matrix").DiffuseColor = New Vector3(0, 0, 0)
        ElseIf e.KeyCode = Keys.F8 Then
            Currentscene.Materials("Matrix").DiffuseColor = New Vector3(1, 1, 1)
            Currentscene.Materials("Matrix").Opacity += 0.1F
        End If
    End Sub

    Private Sub glControl_Resize(sender As Object, e As EventArgs) Handles glControl.Resize
        If glControl.ClientSize.Height = 0 Then
            glControl.ClientSize = New System.Drawing.Size(glControl.ClientSize.Width, 1)
        End If
        ' Tell the render to resize the Render output
        Render.ResizeWindow(glControl.ClientSize.Width, glControl.ClientSize.Height)
    End Sub

    Private Sub glControl_KeyDown(sender As Object, e As KeyEventArgs) Handles glControl.KeyDown
        Render.KeyDown(e)
    End Sub

    Public Sub glControl_MouseMove(sender As Object, e As MouseEventArgs) Handles glControl.MouseMove
        Render.MouseMove(e)
    End Sub

    Public Sub glControl_MouseUp(sender As Object, e As MouseEventArgs) Handles glControl.MouseUp
        Render.MouseUp(e)
    End Sub

    Public Sub glControl_MouseWheel(sender As Object, e As MouseEventArgs) Handles glControl.MouseWheel
        Render.MouseWheel(e)
    End Sub

    Private Sub glControl_Paint(sender As Object, e As PaintEventArgs) Handles glControl.Paint
        Render.UpdateFrame()
        Render.RenderFrame()
        glControl.SwapBuffers()
    End Sub

End Class
