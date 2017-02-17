Imports WeifenLuo.WinFormsUI.Docking
Public Class DockWindow
    Inherits DockContent

    Public UserControl As UserControl = Nothing

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Shared Function Create(UserControl As System.Windows.Forms.UserControl, Name As String)
        Dim result As DockWindow = New DockWindow()
        result.Text = Name
        result.UserControl = UserControl
        result.UserControl.Dock = DockStyle.Fill
        result.Content.Controls.Add(result.UserControl)
        Return result
    End Function

End Class