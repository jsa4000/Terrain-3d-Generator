Namespace Terrain

    ' Some links:
    '   DockPanel Suite:
    '       http://www.nuget.org/packages/DockPanelSuite (PM> Install-Package DockPanelSuite -Version 2.10.0)
    '       http://docs.dockpanelsuite.com/en/latest/getting-started/installing-in-visualstudio.html
    '       http://www.independent-software.com/weifenluo-dockpanelsuite-tutorial-cookbook/
    '    http://www.sophie3d.com/website/index_en.php?page=24ci2p7e
    '    http://javisart.com/php/WebLoader/index.html

    Public Class Vector2D
        Public X As Double = 0 ' x position
        Public Y As Double = 0 ' y position
        Public Sub New(X As Double, Y As Double)
            Me.X = X
            Me.Y = Y
        End Sub
        Public Sub New()
        End Sub
    End Class

    Public Class Vector3D
        Public X As Double = 0 ' x position
        Public Y As Double = 0 ' y position
        Public Z As Double = 0 ' z position
        Public Sub New(X As Double, Y As Double, Z As Double)
            Me.X = X
            Me.Y = Y
            Me.Z = Z
        End Sub
        Public Sub New()
        End Sub
        Public Function Max() As Double
            If (Me.X >= Me.Y AndAlso Me.X >= Me.Z) Then
                Return Me.X
            ElseIf (Me.Y >= Me.X AndAlso Me.Y >= Me.Z) Then
                Return Me.Y
            Else
                Return Me.Z
            End If
        End Function
        Public Shared Operator -(p1 As Vector3D, p2 As Vector3D) As Vector3D
            Return New Vector3D(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z)
        End Operator
        Public Shared Operator +(p1 As Vector3D, p2 As Vector3D) As Vector3D
            Return New Vector3D(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z)
        End Operator
        Public Shared Operator /(p1 As Vector3D, value As Double) As Vector3D
            Return New Vector3D(p1.X / value, p1.Y / value, p1.Z / value)
        End Operator
        Public Shared Function Dot(p1 As Vector3D, p2 As Vector3D) As Double
            Return p1.X * p2.X + p1.Y * p2.Y + p1.Z * p2.Z
        End Function
        Public Shared Function Cross(v1 As Vector3D, v2 As Vector3D) As Vector3D
            Return New Vector3D(v1.Y * v2.Z - v1.Z * v2.Y, -v1.X * v2.Z + v1.Z * v2.X, v1.X * v2.Y - v1.Y * v2.X)
        End Function
        Public Shared Function Dist(p1 As Vector3D, p2 As Vector3D) As Double
            Return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) +
                             (p1.Y - p2.Y) * (p1.Y - p2.Y) +
                             (p1.Z - p2.Z) * (p1.Z - p2.Z))
        End Function
        Private Shared Function Cost(p1 As Vector3D, p2 As Vector3D, p3 As Vector3D) As Double
            Return Vector3D.Dist(p1, p2) + Vector3D.Dist(p2, p3) + Vector3D.Dist(p3, p1)
        End Function

    End Class

    Public Class VertexData
        'geometric vertex id And direct reference to the vector3D
        Public Vertex As Tuple(Of Integer, Vector3D)
        'texture coordinates vertices id and direct reference to the vector2D
        Public TextureCoordinate As Tuple(Of Integer, Vector2D)
        'vertex normals  id and direct reference to the vector3D
        Public Normal As Tuple(Of Integer, Vector3D)
    End Class

    Public Class Polygon3D
        ' All vertices in the polygon
        Public Vertices As New List(Of VertexData)
        'All triangules that conform this polygon using the vertex data information
        Public Triangles As New List(Of Polygon3D)

        Public Function ComputeNormals() As Vector3D
            Dim result As New Vector3D()

            'In order to compute the normal of a number of vertices:
            ' 1. Find the plane created by those vertices
            ' 2. Finde a perpendicluar vector to this plane.

            ' In order the triangualte the polygon first the triangulation is needed
            Me.Triangulate()
            ' With all the triangles already computed we need to compute the average plane for all of them.
            For Each triangle In Me.Triangles
                ' Given A, B, C point of a triangle.
                ' 1 First vector between two points by subtracting the coordinates Of the points. The vector from point (x1, y1) to point (x2, y2) Is   V = <x2 - x1, y2 - y1>
                ' 2 Computhe the dot product
                'Dir = (B - A) x (C - A)
                Dim pointA As Vector3D = triangle.Vertices(0).Vertex.Item2
                Dim pointB As Vector3D = triangle.Vertices(1).Vertex.Item2
                Dim pointC As Vector3D = triangle.Vertices(2).Vertex.Item2
                Dim normal As Vector3D = Vector3D.Cross(pointB - pointA, pointC - pointA)
                ' Normalize the result
                'Norm = Dir() / Len(Dir)    
                result += (normal / normal.Max)
            Next
            Return result / Me.Triangles.Count
        End Function

        Private Sub Triangulate()
            ' EFFICENT ALGORITHM TO COMPUTE THE TRIANGULATION
            ' This will triagulate the mesh using Minimum Cost Polygon Triangulation
            '  (Points of covnvex polygon are given in order (either clockwise or anticlockwise)
            ' The problem is to find the cost of triangulation with the minimum cost. The cost of a triangulation is sum 
            ' of the weights of its component triangles. Weight of each triangle Is its perimeter (sum of lengths of all sides)

            ' For this particular case it will be used an initial pivot point (first vertice). And from this one The riangulation will be done.
            Dim pivot As Integer = 0
            Dim index As Double = 1
            While index + 1 < Vertices.Count
                'Create a new triangulation polygon
                Dim triangle As New Polygon3D()
                ' Get from the vertice 0 to the index + index + 1
                triangle.Vertices.Add(Vertices(pivot))
                triangle.Vertices.Add(Vertices(index))
                triangle.Vertices.Add(Vertices(index + 1))
                'Add the triangle to the list of triangles of the polygon surface
                Me.Triangles.Add(triangle)
                'Move to the following vertices
                index += 1
            End While
        End Sub
    End Class

    Public Class MaterialData
        Public name As String = String.Empty

        Public AmbientColor As New Vector3D()
        Public DiffuseColor As New Vector3D()
        Public SpecularColor As New Vector3D()
        Public SpecularExponent As Single = 1
        Public Opacity As Single = 1.0F

        Public AmbientMap As String = String.Empty
        Public DiffuseMap As String = String.Empty
        Public SpecularMap As String = String.Empty
        Public OpacityMap As String = String.Empty
        Public NormalMap As String = String.Empty

        Public Shared Function CreateDefault(name As String, Optional textureFile As String = Nothing, Optional type As Integer = 0) As MaterialData
            Dim result As New MaterialData()

            result.name = name
            If (type = 1) Then
                'Ns 10.0
                'Ni 1.5
                'd 1.0
                'Tr 0.0000
                'Tf 1.0 1.0000 1.0000 
                'illum 2
                'Ka 0.188 0.1880 0.1880
                'Kd 1.0 1.0000 1.0000
                'Ks 0.1 0.1000 0.1000
                'map_Ka opentksquare2.png
                'map_Kd opentksquare2.png

                result.AmbientColor = New Vector3D(0.188, 0.188, 0.188)
                result.DiffuseColor = New Vector3D(1, 1, 1)
                result.SpecularColor = New Vector3D(0.1, 0.1, 0.1)
                If (textureFile IsNot Nothing) Then
                    result.AmbientMap = My.Computer.FileSystem.GetName(textureFile)
                    result.DiffuseMap = My.Computer.FileSystem.GetName(textureFile)
                End If
                result.Opacity = 1
                result.SpecularExponent = 10
            Else
                'Ns 96.078431
                'Ka 1.0 1.000000 1.000000
                'Kd 0.64 0.640000 0.640000
                'Ks 0.000000 0.000000 0.000000
                'Ni 1.0
                'd 1.0
                'illum 2

                result.AmbientColor = New Vector3D(1, 1, 1)
                result.DiffuseColor = New Vector3D(0.64, 0.64, 0.64)
                result.SpecularColor = New Vector3D(0.0, 0.0, 0.0)
                If (textureFile IsNot Nothing) Then
                    result.DiffuseMap = My.Computer.FileSystem.GetName(textureFile)
                End If
                result.Opacity = 1
                result.SpecularExponent = 96.078431
            End If

            Return result
        End Function

    End Class

    Public Class TerrainHelpers

        Public Shared Function GetAvailabilityPalette(Optional isPercentage As Boolean = True) As List(Of ColorRange)
            Dim palette As New List(Of ColorRange)
            ' Check if the values must be in decimal instead
            Dim denominator As Double = 1
            If (Not isPercentage) Then
                denominator = 100
            End If
            'B&W Availability
            palette.Add(New ColorRange(0 / denominator, 0.0000001 / denominator, &HFF000000, &HFF000000))
            palette.Add(New ColorRange(0.0000001 / denominator, 69.9999999 / denominator, &HFF1A1A1A, &HFF1A1A1A))
            palette.Add(New ColorRange(70 / denominator, 79.9999999 / denominator, &HFF666666, &HFF666666))
            palette.Add(New ColorRange(80 / denominator, 89.9999999 / denominator, &HFF808080, &HFF808080))
            palette.Add(New ColorRange(90 / denominator, 94.9999999 / denominator, &HFF999999, &HFF999999))
            palette.Add(New ColorRange(95 / denominator, 97.9999999 / denominator, &HFFB3B3B3, &HFFB3B3B3))
            palette.Add(New ColorRange(98 / denominator, 98.9999999 / denominator, &HFFCCCCCC, &HFFCCCCCC))
            palette.Add(New ColorRange(99 / denominator, 99.8999999 / denominator, &HFFE6E6E6, &HFFE6E6E6))
            palette.Add(New ColorRange(99.9 / denominator, 100 / denominator, &HFFFFFFFF, &HFFFFFFFF))
            Return palette
        End Function

        Public Shared Function GetAvailabilityPalette2(Optional isPercentage As Boolean = True) As List(Of ColorRange)
            Dim palette As New List(Of ColorRange)
            ' Check if the values must be in decimal instead
            Dim denominator As Double = 1
            If (Not isPercentage) Then
                denominator = 100
            End If
            'B&W Availability
            'palette.Add(New ColorRange(0 / denominator, 69.9999999 / denominator, &HFF000000, &HFF1A1A1A))
            'palette.Add(New ColorRange(70 / denominator, 79.9999999 / denominator, &HFF1A1A1A, &HFF666666))
            palette.Add(New ColorRange(0 / denominator, 69.9999999 / denominator, &HFF000000, &HFF4D4D4D))
            palette.Add(New ColorRange(70 / denominator, 79.9999999 / denominator, &HFF4D4D4D, &HFF666666))
            palette.Add(New ColorRange(80 / denominator, 89.9999999 / denominator, &HFF666666, &HFF808080))
            palette.Add(New ColorRange(90 / denominator, 94.9999999 / denominator, &HFF808080, &HFF999999))
            palette.Add(New ColorRange(95 / denominator, 97.9999999 / denominator, &HFF999999, &HFFB3B3B3))
            palette.Add(New ColorRange(98 / denominator, 98.9999999 / denominator, &HFFB3B3B3, &HFFCCCCCC))
            palette.Add(New ColorRange(99 / denominator, 99.8999999 / denominator, &HFFCCCCCC, &HFFE6E6E6))
            palette.Add(New ColorRange(99.9 / denominator, 100 / denominator, &HFFE6E6E6, &HFFFFFFFF))

            'palette.Add(New ColorRange(0 / denominator, 94.9999999 / denominator, &HFF000000, &HFF999999))
            'palette.Add(New ColorRange(95 / denominator, 97.9999999 / denominator, &HFF999999, &HFFB3B3B3))
            'palette.Add(New ColorRange(98 / denominator, 98.9999999 / denominator, &HFFB3B3B3, &HFFCCCCCC))
            'palette.Add(New ColorRange(99 / denominator, 99.8999999 / denominator, &HFFCCCCCC, &HFFE6E6E6))
            'palette.Add(New ColorRange(99.9 / denominator, 100 / denominator, &HFFE6E6E6, &HFFFFFFFF))

            Return palette
        End Function

        Public Shared Sub CreateHeightMapFromFile(inputFile As String, outputFile As String, Optional palette As List(Of ColorRange) = Nothing, Optional delimiter As Char = " "c)
            'Gray scale Image from the min value (baclk) to the max value (white)
            If (palette Is Nothing) Then
                Dim result As Double() = ImageUtils.GetBoundaries(inputFile)
                palette = New List(Of ColorRange)
                'palette.Add(New ColorRange(result(0), result(1), &HFFFFFFFF, &HFF000000, True))
                palette.Add(New ColorRange(result(0), result(1), &HFF000000, &HFFFFFFFF, True))
            End If

            Dim myImage As Image = ImageUtils.LoadImage(inputFile, palette, delimiter)
            If (myImage IsNot Nothing) Then
                myImage.Save(outputFile, System.Drawing.Imaging.ImageFormat.Png)
            End If

        End Sub

        Public Shared Sub CreateFillHeightMap(value As Double, size As Size, outputFile As String, Optional palette As List(Of ColorRange) = Nothing, Optional delimiter As Char = " "c)
            Dim tempFilePath As String = outputFile & ".txt"
            Dim oFile As System.IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(outputFile & ".txt", False, System.Text.Encoding.ASCII)
            For i As Integer = 0 To size.Height - 1
                For j As Integer = 0 To size.Width - 1
                    oFile.Write(value & " ")
                Next
                oFile.WriteLine()
            Next
            oFile.Close()
            'Create empty file
            CreateHeightMapFromFile(tempFilePath, outputFile, palette)
            My.Computer.FileSystem.DeleteFile(tempFilePath)
        End Sub


        Public Shared Sub SaveObj(outputPath As String, terrain As Terrain3D, material As MaterialData, Optional material2 As MaterialData = Nothing)

            Dim objFilePath As String = outputPath & "\" & terrain.Name & ".obj"
            Dim oFile As System.IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(objFilePath, False, System.Text.Encoding.ASCII)

            ' WRITE THE NAME OF THE MATERIAL TO IMPORT
            oFile.WriteLine("mtllib " & terrain.Name & ".mtl")

            ' WRITE THE NAME OF THE CURRENT MESH
            oFile.WriteLine("o " & terrain.Name)

            ' WRITE ALL TEH VERTICES OF THE MESH
            For index As Integer = 0 To terrain.Vertices.Count - 1
                Dim vertice As Vector3D = terrain.Vertices(index)
                Dim color As Vector3D = terrain.VertexColor(index)
                oFile.WriteLine("v " & vertice.X.ToString("0.00000") & " " & vertice.Y.ToString("0.00000") & " " & vertice.Z.ToString("0.00000") & " " &
                                      color.X.ToString("0.00000") & " " & color.Y.ToString("0.00000") & " " & color.Z.ToString("0.00000"))
            Next

            ' WRITE ALL TEH VERTICES OF THE MESH
            For Each vertice As Vector3D In terrain.Normals
                oFile.WriteLine("vn " & vertice.X.ToString("0.00000") & " " & vertice.Y.ToString("0.00000") & " " & vertice.Z.ToString("0.00000"))
            Next

            ' WRITE THE TEXTURES COORDINATES OF THE MESH
            For Each textCoords As Vector2D In terrain.TextureCoordinates
                oFile.WriteLine("vt " & textCoords.X.ToString("0.00000") & " " & textCoords.Y.ToString("0.00000"))
            Next

            ' SET OBJ TO USE A MATERIAL called rth3e same way as the object
            oFile.WriteLine("usemtl " & material.name)

            'WRITE ALL THE FACES OF THE MESH
            For Each face As Polygon3D In terrain.faces
                Dim line As String = "f"
                For Each vertex As VertexData In face.Vertices
                    line &= " " & vertex.Vertex.Item1 & "/" & vertex.TextureCoordinate.Item1 & "/" & vertex.Normal.Item1
                Next
                oFile.WriteLine(line)
            Next

            oFile.Close()

            'Write MATERIAL FILE
            Dim mtlFilePath As String = outputPath & "\" & terrain.Name & ".mtl"
            Dim mFile As System.IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(mtlFilePath, False, System.Text.Encoding.ASCII)

            ' WRITE THE NAME OF THE MATERIAL TO IMPORT
            mFile.WriteLine("newmtl " & material.name)

            ' Write custom properties
            mFile.WriteLine("Ni 1.0")
            mFile.WriteLine("Tr 0.0000")
            mFile.WriteLine("Tf 1.0 1.0000 1.0000")

            ' Difuse Color
            mFile.WriteLine("Kd " & material.DiffuseColor.X.ToString("0.00000") & " " & material.DiffuseColor.Y.ToString("0.00000") & " " & material.DiffuseColor.Z.ToString("0.00000"))

            ' Ambien Color
            mFile.WriteLine("Ka " & material.AmbientColor.X.ToString("0.00000") & " " & material.AmbientColor.Y.ToString("0.00000") & " " & material.AmbientColor.Z.ToString("0.00000"))

            ' Opacity and illumination
            mFile.WriteLine("d " & material.Opacity)
            mFile.WriteLine("illum 2")

            'Specular Color
            mFile.WriteLine("Ks " & material.SpecularColor.X.ToString("0.00000") & " " & material.SpecularColor.Y.ToString("0.00000") & " " & material.SpecularColor.Z.ToString("0.00000"))
            mFile.WriteLine("Ns " & material.SpecularExponent)

            ' Ambient Map
            If (Not String.IsNullOrEmpty(material.AmbientMap)) Then
                mFile.WriteLine("map_Ka " & material.AmbientMap)
            End If

            'Diffuse Map
            If (Not String.IsNullOrEmpty(material.DiffuseMap)) Then
                mFile.WriteLine("map_Kd " & material.DiffuseMap)
            End If

            ' Check for the second material
            If (material2 IsNot Nothing) Then
                ' WRITE THE NAME OF THE MATERIAL TO IMPORT
                mFile.WriteLine("newmtl " & material2.name)

                ' Write custom properties
                mFile.WriteLine("Ni 1.0")
                mFile.WriteLine("Tr 0.0000")
                mFile.WriteLine("Tf 1.0 1.0000 1.0000")

                ' Difuse Color
                mFile.WriteLine("Kd " & material2.DiffuseColor.X.ToString("0.00000") & " " & material2.DiffuseColor.Y.ToString("0.00000") & " " & material2.DiffuseColor.Z.ToString("0.00000"))

                ' Ambien Color
                mFile.WriteLine("Ka " & material2.AmbientColor.X.ToString("0.00000") & " " & material2.AmbientColor.Y.ToString("0.00000") & " " & material2.AmbientColor.Z.ToString("0.00000"))

                ' Opacity and illumination
                mFile.WriteLine("d " & material2.Opacity)
                mFile.WriteLine("illum 2")

                'Specular Color
                mFile.WriteLine("Ks " & material2.SpecularColor.X.ToString("0.00000") & " " & material2.SpecularColor.Y.ToString("0.00000") & " " & material2.SpecularColor.Z.ToString("0.00000"))
                mFile.WriteLine("Ns " & material2.SpecularExponent)

                ' Ambient Map
                If (Not String.IsNullOrEmpty(material2.AmbientMap)) Then
                    mFile.WriteLine("map_Ka " & material2.AmbientMap)
                End If

                'Diffuse Map
                If (Not String.IsNullOrEmpty(material2.DiffuseMap)) Then
                    mFile.WriteLine("map_Kd " & material2.DiffuseMap)
                End If
            End If

            mFile.Close()
        End Sub

    End Class

End Namespace

