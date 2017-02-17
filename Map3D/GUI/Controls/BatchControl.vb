Imports Map3D.Managers

Public Class BatchControl

    Private Sub btnBrowseFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBrowserOutputPath.Click
        With FolderBrowserDialog1
            .Reset()
            .Description = "Select Folder:"
            .SelectedPath = txtOutPath.Text
            .ShowNewFolderButton = False
            Dim FolderDialogResult As DialogResult = .ShowDialog()
            If FolderDialogResult = Windows.Forms.DialogResult.OK Then
                txtOutPath.Text = .SelectedPath
            End If
            .Dispose()
        End With
    End Sub

    Private Sub cmdBrowseFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
                cmdBrowseAvFile.Click, cmdBrowseContFile.Click
        Dim currentTxt As TextBox = Nothing
        If (sender Is cmdBrowseAvFile) Then
            currentTxt = txtAvFile
        ElseIf (sender Is cmdBrowseContFile) Then
            currentTxt = txtContFile
        End If
        With OpenFileDialog1
            .Reset()
            .Title = "Select Input File"
            If currentTxt.Text = "" Then .InitialDirectory = Application.StartupPath
            If currentTxt.Text <> "" Then .InitialDirectory = My.Computer.FileSystem.GetParentPath(currentTxt.Text)
            Dim FileDialogResult As DialogResult = .ShowDialog
            If FileDialogResult = DialogResult.OK Then
                currentTxt.Text = .FileName
            End If
            .Dispose()
        End With
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click

        Dim terrainManager As New BatchTerrainManager()
        'terrainManager.ConvertInputFiles(True)

        terrainManager.Process()

        MsgBox("Done!")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim terrainManager As New BatchTerrainManager()


        MsgBox("Done!")
    End Sub
End Class
