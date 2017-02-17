Imports System.IO
Imports System.Collections.Generic
Imports WeifenLuo.WinFormsUI.Docking


Public Class Form1
    Public Windows As New Dictionary(Of String, DockWindow)
    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        Dim frmAbout As New AboutForm()
        frmAbout.Show()
    End Sub

    Private Sub Viewer2DToolStripMenuItem_CheckedChanged(sender As Object, e As EventArgs) Handles Viewer2DToolStripMenuItem.CheckedChanged
        Dim key As String = "Viewer2DControl"
        Dim title As String = "Viewer 2D"
        If (Not Windows.ContainsKey(key)) Then
            Windows.Add(key, DockWindow.Create(New Viewer2DControl(), title))
            Windows(key).Show(DockPanel, DockState.DockLeft)
        Else
            Windows(key).IsHidden = Not Windows(key).IsHidden
        End If
    End Sub
    Private Sub Viewer3DToolStripMenuItem_CheckedChanged(sender As Object, e As EventArgs) Handles Viewer3DToolStripMenuItem.CheckedChanged
        Dim key As String = "Viewer3DControl"
        Dim title As String = "Viewer 3D"
        If (Not Windows.ContainsKey(key)) Then
            Windows.Add(key, DockWindow.Create(New Viewer3DControl(), title))
            Windows(key).Show(DockPanel)
        Else
            Windows(key).IsHidden = Not Windows(key).IsHidden
        End If
    End Sub
    Private Sub PropertiesToolStripMenuItem_CheckedChanged(sender As Object, e As EventArgs) Handles PropertiesToolStripMenuItem.CheckedChanged
        Dim key As String = "PropertiesControl"
        Dim title As String = "Properties"
        If (Not Windows.ContainsKey(key)) Then
            Windows.Add(key, DockWindow.Create(New PropertiesControl(), title))
            ' Windows(key).Show.Show(DockPanel, DockState.DockLeft)
            Windows(key).Show(Windows("Viewer2DControl").Pane, DockAlignment.Bottom, 0.4)
        Else
            Windows(key).IsHidden = Not Windows(key).IsHidden
        End If
    End Sub

    Private Sub BatchToolStripMenuItem_CheckedChanged(sender As Object, e As EventArgs) Handles BatchToolStripMenuItem.CheckedChanged
        Dim key As String = "BatchControl"
        Dim title As String = "Batch Process"
        If (Not Windows.ContainsKey(key)) Then
            Windows.Add(key, DockWindow.Create(New BatchControl(), title))
            Windows(key).Show(DockPanel)
        Else
            Windows(key).IsHidden = Not Windows(key).IsHidden
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        BatchToolStripMenuItem.Checked = True

        'Viewer3DToolStripMenuItem.Checked = True
        'Viewer2DToolStripMenuItem.Checked = True
        'PropertiesToolStripMenuItem.Checked = True
    End Sub
End Class
