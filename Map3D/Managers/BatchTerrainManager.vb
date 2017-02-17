Imports System.Drawing
Imports Map3D.Terrain

Namespace Managers

    Public Class BatchTerrainManager

        'Content Folder


        Public Function Process() As Boolean



            Return True
        End Function

        Public Sub CopyDirectory(ByVal sourcePath As String, ByVal destinationPath As String, Optional recursive As Boolean = True, Optional move As Boolean = False)
            Dim sourceDirectoryInfo As New System.IO.DirectoryInfo(sourcePath)

            ' If the destination folder don't exist then create it
            If Not System.IO.Directory.Exists(destinationPath) Then
                System.IO.Directory.CreateDirectory(destinationPath)
            End If

            Dim fileSystemInfo As System.IO.FileSystemInfo
            For Each fileSystemInfo In sourceDirectoryInfo.GetFileSystemInfos
                Dim destinationFileName As String =
                    System.IO.Path.Combine(destinationPath, fileSystemInfo.Name)

                ' Now check whether its a file or a folder and take action accordingly
                If TypeOf fileSystemInfo Is System.IO.FileInfo Then
                    If (Not move) Then
                        System.IO.File.Copy(fileSystemInfo.FullName, destinationFileName, True)
                    Else
                        System.IO.File.Move(fileSystemInfo.FullName, destinationFileName)
                    End If
                Else
                    If (recursive) Then
                        ' Recursively call the mothod to copy all the neste folders
                        CopyDirectory(fileSystemInfo.FullName, destinationFileName)
                    End If
                End If
            Next
        End Sub

    End Class

End Namespace