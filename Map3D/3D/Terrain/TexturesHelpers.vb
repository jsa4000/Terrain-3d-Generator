Imports System.IO
Imports System.Drawing
Namespace Terrain

    Public Class TexturesHelpers

        Public Shared ProgramsFolder As String = My.Application.Info.DirectoryPath & "\Programs"

        Public Shared Function GetMatrixSize(inputfile As String, Optional delimiter As Char = " "c) As Size
            Dim result As Size = Nothing
            If (File.Exists(inputfile)) Then
                result = New Size()
                'Read all the rows (lines) from the file
                Dim rows As String() = File.ReadAllLines(inputfile)
                'The height of the image will correspondo with the number of lines
                result.Height = rows.Length
                'The Width of the image will correspondo with the number of characters
                result.Width = rows(0).Split(New Char() {delimiter}, StringSplitOptions.RemoveEmptyEntries).Length
            End If
            Return result
        End Function

        Public Shared Sub GenerateAvailabilityTexture(matrixFile As String, matrixSize As Size, outputFile As String, Optional drawMap As Boolean = True, Optional isPercentage As Boolean = True, Optional HD As Boolean = False)
            Dim values As String() = {"0", "0.0000001", "69.9999999", "70", "79.9999999", "80", "89.9999999", "90", "94.9999999", "95", "97.9999999", "98", "98.9999999", "99", "99.8999999", "99.9", "100"}
            Dim colorRange As Integer = 100
            If (Not isPercentage) Then
                values = {"0", "0.00000001", "0.699999999", "0.70", "0.799999999", "0.80", "0.899999999", "0.90", "0.949999999", "0.95", "0.979999999", "0.98", "0.989999999", "0.99", "0.998999999", "0.999", "1"}
                colorRange = 1
            End If
            'Create the palette
            Dim palette As String = "" & values(0) & " 'grey', " & values(1) & " 'grey', " & values(1) & " 'dark-blue', " & values(2) & " 'dark-blue', " &
                "" & values(3) & " '#0040ff', " & values(4) & " '#0040ff', " & values(5) & " 'cyan', " & values(6) & " 'cyan', " & values(7) & " 'green', " & values(8) & " 'green', " & values(9) & " 'yellow', " &
               "" & values(10) & " 'yellow', " & values(11) & " 'orange', " & values(12) & " 'orange', " & values(13) & " 'red' , " & values(14) & " 'red' , " & values(15) & " 'dark-red', " & values(16) & " 'dark-red'"
            GenerateTexture(matrixFile, matrixSize, outputFile, colorRange, palette, drawMap, HD)
        End Sub

        Public Shared Sub GenerateContinuityTexture(matrixFile As String, matrixSize As Size, outputFile As String, Optional drawMap As Boolean = True, Optional HD As Boolean = False)
            Dim palette As String = "0 'dark-red', 0.0001 'dark-red', 0.0001 'red', 0.0005 'red', 0.0005 'orange', 0.001 'orange', 0.001 'yellow', 0.005 'yellow', 0.005 'green', 0.01 'green', 0.01 'cyan', 0.02 'cyan', 0.02 '#0040ff' , 0.03 '#0040ff' , 0.03 'dark-blue', 1 'dark-blue', 1.0000001 'grey', 2 'grey'"
            Dim colorRange As Integer = 2
            GenerateTexture(matrixFile, matrixSize, outputFile, colorRange, palette, drawMap, HD)
        End Sub

        Public Shared Sub GenerateTexture(matrixFile As String, matrixSize As Size, outputFile As String,
                                      colorRange As Integer, palette As String, drawMap As Boolean, Optional HD As Boolean = False)
            ' MT Transmitted Graphic
            Dim Gplot_Script As String = My.Computer.FileSystem.GetParentPath(outputFile) & "\script.gp"
            Dim res As Integer = 1
            If (HD) Then res = 2

            Dim f_Gplot_Script As System.IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(Gplot_Script, False, System.Text.Encoding.ASCII)
            f_Gplot_Script.WriteLine("set term png truecolor enhanced font 'Verdana,10' size " & (960 * res) & ", " & (720 * res) & " fontscale 1 linewidth 2")
            f_Gplot_Script.WriteLine("set encoding utf8")
            f_Gplot_Script.WriteLine("set output '" & outputFile & "'")
            If (drawMap) Then f_Gplot_Script.WriteLine("set multiplot")
            f_Gplot_Script.WriteLine("set lmargin at screen 0")
            f_Gplot_Script.WriteLine("set rmargin at screen 1")
            f_Gplot_Script.WriteLine("set bmargin at screen 0")
            f_Gplot_Script.WriteLine("set tmargin at screen 1")
            f_Gplot_Script.WriteLine("file1 = '" & matrixFile & "'")
            f_Gplot_Script.WriteLine("set palette defined (" & palette & ")")
            f_Gplot_Script.WriteLine("set cbrange [0:" & colorRange & "]")
            f_Gplot_Script.WriteLine("unset colorbox ")
            f_Gplot_Script.WriteLine("set xrange [0:" & matrixSize.Width & "]")
            f_Gplot_Script.WriteLine("set yrange [0:" & matrixSize.Height & "]")
            f_Gplot_Script.WriteLine("unset tics")
            f_Gplot_Script.WriteLine("unset key")
            f_Gplot_Script.WriteLine("set view map")
            f_Gplot_Script.WriteLine("splot file1 matrix with image")
            If (drawMap) Then
                f_Gplot_Script.WriteLine("file2 = '" & ProgramsFolder & "\world_borders.dat" & "'")
                f_Gplot_Script.WriteLine("set xrange [-30:40]")
                f_Gplot_Script.WriteLine("set yrange [26:70]")
                f_Gplot_Script.WriteLine("plot file2 w l lt rgb 'black'")
            End If
            f_Gplot_Script.Close()
            f_Gplot_Script.Dispose()
            SuperShell(ProgramsFolder & "\gnuplot\gnuplot.exe", """" & Gplot_Script & """", True, , ProcessWindowStyle.Hidden)
            My.Computer.FileSystem.DeleteFile(Gplot_Script)
        End Sub

    End Class
End Namespace