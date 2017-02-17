Imports System.IO
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices

Namespace Terrain
    Public Class MatrixData
        Public Value As Double = 0
        Public Coords As New Vector2D
        Public RealValue As Double = 0

        Public Sub New()
        End Sub

        Public Sub New(pValue As Double)
            Value = pValue
        End Sub
    End Class

    Public Class Matrix2D
        Public FilePath As String = String.Empty
        Public Minvalue As Double = Double.MaxValue
        Public Maxvalue As Double = Double.MinValue
        Public Height As Integer = 0
        Public Width As Integer = 0
        Public Matrix(,) As MatrixData = Nothing

        Public Shared Function Create(filePath As String, Optional OriginalMatrixFile As String = Nothing, Optional coords As Vector2D(,) = Nothing, Optional delimiter As Char = " "c) As Matrix2D
            Dim result As New Matrix2D()
            result.Load(filePath, OriginalMatrixFile, coords, delimiter)
            Return result
        End Function

        Public Function Load(filePath As String, Optional OriginalMatrixFile As String = Nothing, Optional coords As Vector2D(,) = Nothing, Optional delimiter As Char = " "c) As Boolean
            'Check if the original matrix has been specified
            Dim OriginalMatrix(,) As Double = Nothing
            If (Not String.IsNullOrEmpty(OriginalMatrixFile)) Then
                'Read all the rows (lines) from the file
                Dim oData As String() = File.ReadAllLines(OriginalMatrixFile)
                'The height of the image will correspondo with the number of lines
                Dim oHeight As Integer = oData.Length
                'The Width of the image will correspondo with the number of characters
                Dim oWidth As Integer = oData(0).Split(New Char() {delimiter}, StringSplitOptions.RemoveEmptyEntries).Length
                Dim oMatrix(oHeight - 1, oWidth - 1) As Double
                For i As Integer = 0 To oHeight - 1
                    Dim oColumns As String() = oData(i).Split(New Char() {delimiter}, StringSplitOptions.RemoveEmptyEntries)
                    For j As Integer = 0 To oWidth - 1
                        Dim value As Double = Val(oColumns(j))
                        'Set the current values
                        oMatrix(i, j) = value
                    Next
                Next
                ' Set the new Original Matrix
                OriginalMatrix = oMatrix
            End If

            ' Check if the file exists
            If (File.Exists(filePath)) Then
                Me.FilePath = filePath
                ' Check whether the image is grayscale or a txt file
                If (filePath.EndsWith(".png") Or filePath.EndsWith(".jpg")) Then
                    ''''''''''''''''''''''''''''''''
                    'READ IMAGE FILE WITH THE MATRIX
                    ''''''''''''''''''''''''''''''''
                    Dim Image As New Bitmap(filePath)

                    ' Get the height and width
                    Me.Height = Image.Height
                    Me.Width = Image.Width
                    Dim BoundsRect As Rectangle = New Rectangle(0, 0, Me.Width, Me.Height)

                    ' Inititialize Combined image
                    Dim ImageData As BitmapData = Image.LockBits(BoundsRect, ImageLockMode.ReadOnly, Image.PixelFormat)
                    Dim bytes As Double = (ImageData.Stride * Me.Height) 'Strides contains the 4 bytes added for tyhe Pixelforma used
                    Dim rgbaImage(bytes - 1) As Byte
                    Dim bpp As Integer = ImageData.Stride / Me.Width

                    ' Copy the RGB values into the array.
                    Marshal.Copy(ImageData.Scan0, rgbaImage, 0, bytes)

                    Dim matrix(Me.Height - 1, Me.Width - 1) As MatrixData
                    For i As Integer = 0 To Me.Height - 1
                        For j As Integer = 0 To Me.Width - 1
                            '(0.2126*R + 0.7152*G + 0.0722*B)
                            Dim index As Integer = ((i * Me.Width) + j) * bpp
                            Dim value As Integer = 0
                            If (bpp = 3) Then 'rgb
                                value = Int(0.2126 * (rgbaImage(index)) + 0.7152 * (rgbaImage(index + 1)) + 0.0722 * (rgbaImage(index + 2)))
                            ElseIf (bpp = 4) Then 'argb
                                'value = Int(0.2126 * (rgbaImage(index + 1)) + 0.7152 * (rgbaImage(index + 2)) + 0.0722 * (rgbaImage(index + 3)))
                                value = Int(0.2126 * (rgbaImage(index)) + 0.7152 * (rgbaImage(index + 1)) + 0.0722 * (rgbaImage(index + 2)))
                            Else 'value
                                value = rgbaImage(index)
                            End If
                            If (value > Me.Maxvalue) Then Me.Maxvalue = value
                            If (value < Me.Minvalue) Then Me.Minvalue = value

                            'Set the values
                            matrix(i, j) = New MatrixData(value)
                            If (OriginalMatrix IsNot Nothing) Then
                                matrix(i, j).RealValue = OriginalMatrix((Me.Height - 1) - i, j)
                            End If
                            If (coords IsNot Nothing) Then
                                matrix(i, j).Coords.X = coords(i, j).X
                                matrix(i, j).Coords.Y = coords(i, j).Y
                            End If
                        Next
                    Next

                    'Dispose and unlock the iamges
                    Image.UnlockBits(ImageData)
                    Image.Dispose()
                    ' Set the new Matrix
                    Me.Matrix = matrix
                Else
                    ''''''''''''''''''''''''''''''''
                    'READ TEXT FILE WITH THE MATRIX
                    ''''''''''''''''''''''''''''''''
                    'Read all the rows (lines) from the file
                    Dim data As String() = File.ReadAllLines(filePath)
                    'The height of the image will correspondo with the number of lines
                    Me.Height = data.Length
                    'The Width of the image will correspondo with the number of characters
                    Me.Width = data(0).Split(New Char() {delimiter}, StringSplitOptions.RemoveEmptyEntries).Length
                    Dim mymatrix(Me.Height - 1, Me.Width - 1) As MatrixData
                    For i As Integer = 0 To Me.Height - 1
                        Dim columns As String() = data(i).Split(New Char() {delimiter}, StringSplitOptions.RemoveEmptyEntries)
                        For j As Integer = 0 To Me.Width - 1
                            Dim value As Double = Val(columns(j))
                            'Set the current values
                            mymatrix(i, j) = New MatrixData(value)
                            If (OriginalMatrix IsNot Nothing) Then
                                Matrix(i, j).RealValue = OriginalMatrix(i, j)
                            End If
                            If (coords IsNot Nothing) Then
                                Matrix(i, j).Coords.X = coords(i, j).X
                                Matrix(i, j).Coords.Y = coords(i, j).Y
                            End If
                            'Get min max value
                            If (value > Me.Maxvalue) Then Me.Maxvalue = value
                            If (value < Me.Minvalue) Then Me.Minvalue = value
                        Next
                    Next
                    ' Set the new Matrix
                    Me.Matrix = mymatrix
                End If
                Return True
            End If
            Return False
        End Function

        Public Sub Rotate()
            Dim newmatrix(Me.Width - 1, Me.Height - 1) As MatrixData
            For i As Integer = 0 To Me.Height - 1
                For j As Integer = 0 To Me.Width - 1
                    newmatrix(j, i) = Me.Matrix(i, j)
                Next
            Next
            'Update current Fields
            Dim temp As Double = Me.Height
            Me.Height = Me.Width
            Me.Width = temp
            Me.Matrix = newmatrix
        End Sub

        Public Sub Faced()
            'Read all the rows (lines) from the file
            Dim newmatrix((Me.Height * 2) - 1, (Me.Width * 2) - 1) As MatrixData
            For i As Integer = 0 To Me.Height - 1
                For j As Integer = 0 To Me.Width - 1
                    newmatrix(i * 2, j * 2) = Me.Matrix(i, j)
                    newmatrix(i * 2, (j * 2) + 1) = Me.Matrix(i, j)
                    newmatrix((i * 2) + 1, j * 2) = Me.Matrix(i, j)
                    newmatrix((i * 2) + 1, (j * 2) + 1) = Me.Matrix(i, j)
                Next
            Next
            ' Update Current Fields
            Me.Height *= 2
            Me.Width *= 2
            Me.Matrix = newmatrix
        End Sub

        Public Sub Pad()
            'Read all the rows (lines) from the file
            Dim newmatrix((Me.Height + 2) - 1, (Me.Width + 2) - 1) As MatrixData

            'Initialize the matrix with Zeros (padded) first
            For i As Integer = 0 To Me.Height + 1
                newmatrix(i, 0) = New MatrixData()
                newmatrix(i, Me.Width + 1) = New MatrixData()
            Next
            For j As Integer = 0 To Me.Width + 1
                newmatrix(0, j) = New MatrixData()
                newmatrix(Me.Height + 1, j) = New MatrixData()
            Next

            'Se the old values
            For i As Integer = 0 To Me.Height - 1
                For j As Integer = 0 To Me.Width - 1
                    newmatrix(i + 1, j + 1) = Me.Matrix(i, j)
                Next
            Next
            ' Update Current Fields
            Me.Height += 2
            Me.Width += 2
            Me.Matrix = newmatrix
        End Sub

    End Class

End Namespace
