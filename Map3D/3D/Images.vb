Imports System.IO
Imports OpenTK.Graphics.OpenGL

Public Class Images

    Public Shared Function GrabScreenshot(Width As Integer, Height As Integer, ClientRectangle As Rectangle) As Bitmap
        Dim bmp As New Bitmap(Width, Height)
        Dim data As System.Drawing.Imaging.BitmapData = bmp.LockBits(ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb)
        GL.ReadPixels(0, 0, Width, Height, PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0)
        bmp.UnlockBits(data)
        bmp.RotateFlip(RotateFlipType.RotateNoneFlipY)
        Return bmp
    End Function

    Public Shared Function LoadImage(image As Bitmap) As Integer
        Dim texID As Integer = GL.GenTexture()
        GL.BindTexture(TextureTarget.Texture2D, texID)
        Dim data As System.Drawing.Imaging.BitmapData = image.LockBits(New System.Drawing.Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.ImageLockMode.[ReadOnly], System.Drawing.Imaging.PixelFormat.Format32bppArgb)
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0)
        image.UnlockBits(data)
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D)
        Return texID
    End Function

    Public Shared Function LoadImage(filename As String) As Integer
        Try
            Dim file As New Bitmap(filename)
            Return LoadImage(file)
        Catch e As FileNotFoundException
            Return -1
        End Try
    End Function

End Class
