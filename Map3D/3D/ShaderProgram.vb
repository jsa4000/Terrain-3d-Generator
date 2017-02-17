Imports System.IO
Imports System.Text
Imports OpenTK.Graphics.OpenGL

Public Class ShaderProgram
    Public ProgramID As Integer = -1
    Public VShaderID As Integer = -1
    Public FShaderID As Integer = -1
    Public AttributeCount As Integer = 0
    Public UniformCount As Integer = 0

    Public Attributes As Dictionary(Of String, AttributeInfo) = New Dictionary(Of String, AttributeInfo)()
    Public Uniforms As Dictionary(Of String, UniformInfo) = New Dictionary(Of String, UniformInfo)()
    Public Buffers As Dictionary(Of String, UInteger) = New Dictionary(Of String, UInteger)()

    Public Sub New()
        ProgramID = GL.CreateProgram()
    End Sub

    Public Sub New(vshader As String, fshader As String, Optional fromFile As Boolean = False)
        ProgramID = GL.CreateProgram()

        If fromFile Then
            LoadShaderFromFile(vshader, ShaderType.VertexShader)
            LoadShaderFromFile(fshader, ShaderType.FragmentShader)
        Else
            LoadShaderFromString(vshader, ShaderType.VertexShader)
            LoadShaderFromString(fshader, ShaderType.FragmentShader)
        End If

        Link()
        GenBuffers()
    End Sub

    Private Sub loadShader(code As String, type As ShaderType, ByRef address As Integer)
        address = GL.CreateShader(type)
        GL.ShaderSource(address, code)
        GL.CompileShader(address)
        GL.AttachShader(ProgramID, address)
        Console.WriteLine(GL.GetShaderInfoLog(address))
    End Sub

    Public Sub LoadShaderFromString(code As String, type As ShaderType)
        If type = ShaderType.VertexShader Then
            loadShader(code, type, VShaderID)
        ElseIf type = ShaderType.FragmentShader Then
            loadShader(code, type, FShaderID)
        End If
    End Sub

    Public Sub LoadShaderFromFile(filename As String, type As ShaderType)
        Using sr As New StreamReader(filename)
            If type = ShaderType.VertexShader Then
                loadShader(sr.ReadToEnd(), type, VShaderID)
            ElseIf type = ShaderType.FragmentShader Then
                loadShader(sr.ReadToEnd(), type, FShaderID)
            End If
        End Using
    End Sub

    Public Sub Link()
        GL.LinkProgram(ProgramID)

        Console.WriteLine(GL.GetProgramInfoLog(ProgramID))

        GL.GetProgram(ProgramID, GetProgramParameterName.ActiveAttributes, AttributeCount)
        GL.GetProgram(ProgramID, GetProgramParameterName.ActiveUniforms, UniformCount)

        For i As Integer = 0 To AttributeCount - 1
            Dim info As New AttributeInfo()
            Dim length As Integer = 0

            Dim name As New StringBuilder()
            GL.GetActiveAttrib(ProgramID, i, 256, length, info.size, info.type, name)

            info.name = name.ToString()
            info.address = GL.GetAttribLocation(ProgramID, info.name)
            Attributes.Add(name.ToString(), info)
        Next

        For i As Integer = 0 To UniformCount - 1
            Dim info As New UniformInfo()
            Dim length As Integer = 0
            Dim name As New StringBuilder()

            GL.GetActiveUniform(ProgramID, i, 256, length, info.size, info.type, name)
            info.name = name.ToString()

            Uniforms.Add(name.ToString(), info)
            info.address = GL.GetUniformLocation(ProgramID, info.name)
        Next
    End Sub

    Public Sub GenBuffers()
        For i As Integer = 0 To Attributes.Count - 1
            Dim buffer As UInteger = 0
            GL.GenBuffers(1, buffer)
            Buffers.Add(Attributes.Values.ElementAt(i).name, buffer)
        Next

        For i As Integer = 0 To Uniforms.Count - 1
            Dim buffer As UInteger = 0
            GL.GenBuffers(1, buffer)
            Buffers.Add(Uniforms.Values.ElementAt(i).name, buffer)
        Next
    End Sub

    Public Sub EnableVertexAttribArrays()
        For i As Integer = 0 To Attributes.Count - 1
            GL.EnableVertexAttribArray(Attributes.Values.ElementAt(i).address)
        Next
    End Sub

    Public Sub DisableVertexAttribArrays()
        For i As Integer = 0 To Attributes.Count - 1
            GL.DisableVertexAttribArray(Attributes.Values.ElementAt(i).address)
        Next
    End Sub

    Public Function GetAttribute(name As String) As Integer
        If Attributes.ContainsKey(name) Then
            Return Attributes(name).address
        Else
            Return -1
        End If
    End Function

    Public Function GetUniform(name As String) As Integer
        If Uniforms.ContainsKey(name) Then
            Return Uniforms(name).address
        Else
            Return -1
        End If
    End Function

    Public Function GetBuffer(name As String) As UInteger
        If Buffers.ContainsKey(name) Then
            Return Buffers(name)
        Else
            Return 0
        End If
    End Function

    Public Class UniformInfo
        Public name As String = String.Empty
        Public address As Integer = -1
        Public size As Integer = 0
        Public type As ActiveUniformType
    End Class

    Public Class AttributeInfo
        Public name As String = String.Empty
        Public address As Integer = -1
        Public size As Integer = 0
        Public type As ActiveAttribType
    End Class
End Class