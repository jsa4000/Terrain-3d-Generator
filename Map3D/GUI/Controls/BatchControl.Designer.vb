<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class BatchControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.chkSmooth = New System.Windows.Forms.CheckBox()
        Me.chkFaceted = New System.Windows.Forms.CheckBox()
        Me.chkRangeValues = New System.Windows.Forms.CheckBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.cmdBrowseAvFile = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtAvFile = New System.Windows.Forms.TextBox()
        Me.cmdBrowseContFile = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtContFile = New System.Windows.Forms.TextBox()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtOutPath = New System.Windows.Forms.TextBox()
        Me.cmdBrowserOutputPath = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtProjectName = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkGenerateContent = New System.Windows.Forms.CheckBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnGenerate
        '
        Me.btnGenerate.Location = New System.Drawing.Point(486, 174)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(75, 23)
        Me.btnGenerate.TabIndex = 0
        Me.btnGenerate.Text = "Generate"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'chkSmooth
        '
        Me.chkSmooth.AutoSize = True
        Me.chkSmooth.Location = New System.Drawing.Point(460, 91)
        Me.chkSmooth.Name = "chkSmooth"
        Me.chkSmooth.Size = New System.Drawing.Size(98, 17)
        Me.chkSmooth.TabIndex = 1
        Me.chkSmooth.Text = "Smooth Terrain"
        Me.chkSmooth.UseVisualStyleBackColor = True
        '
        'chkFaceted
        '
        Me.chkFaceted.AutoSize = True
        Me.chkFaceted.Location = New System.Drawing.Point(460, 68)
        Me.chkFaceted.Name = "chkFaceted"
        Me.chkFaceted.Size = New System.Drawing.Size(101, 17)
        Me.chkFaceted.TabIndex = 2
        Me.chkFaceted.Text = "Faceted Terrain"
        Me.chkFaceted.UseVisualStyleBackColor = True
        '
        'chkRangeValues
        '
        Me.chkRangeValues.AutoSize = True
        Me.chkRangeValues.Location = New System.Drawing.Point(460, 115)
        Me.chkRangeValues.Name = "chkRangeValues"
        Me.chkRangeValues.Size = New System.Drawing.Size(115, 17)
        Me.chkRangeValues.TabIndex = 3
        Me.chkRangeValues.Text = "Use Range Values"
        Me.chkRangeValues.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.cmdBrowseAvFile)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.txtAvFile)
        Me.GroupBox2.Controls.Add(Me.cmdBrowseContFile)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.txtContFile)
        Me.GroupBox2.Location = New System.Drawing.Point(39, 53)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(390, 85)
        Me.GroupBox2.TabIndex = 69
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Inputs"
        '
        'cmdBrowseAvFile
        '
        Me.cmdBrowseAvFile.Location = New System.Drawing.Point(345, 16)
        Me.cmdBrowseAvFile.Name = "cmdBrowseAvFile"
        Me.cmdBrowseAvFile.Size = New System.Drawing.Size(26, 20)
        Me.cmdBrowseAvFile.TabIndex = 15
        Me.cmdBrowseAvFile.Text = "..."
        Me.cmdBrowseAvFile.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(11, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(78, 13)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Availability File:"
        '
        'txtAvFile
        '
        Me.txtAvFile.Location = New System.Drawing.Point(94, 16)
        Me.txtAvFile.Name = "txtAvFile"
        Me.txtAvFile.Size = New System.Drawing.Size(245, 20)
        Me.txtAvFile.TabIndex = 13
        '
        'cmdBrowseContFile
        '
        Me.cmdBrowseContFile.Location = New System.Drawing.Point(345, 49)
        Me.cmdBrowseContFile.Name = "cmdBrowseContFile"
        Me.cmdBrowseContFile.Size = New System.Drawing.Size(26, 20)
        Me.cmdBrowseContFile.TabIndex = 4
        Me.cmdBrowseContFile.Text = "..."
        Me.cmdBrowseContFile.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 52)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Continuity File:"
        '
        'txtContFile
        '
        Me.txtContFile.Location = New System.Drawing.Point(94, 49)
        Me.txtContFile.Name = "txtContFile"
        Me.txtContFile.Size = New System.Drawing.Size(245, 20)
        Me.txtContFile.TabIndex = 0
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtOutPath)
        Me.GroupBox1.Controls.Add(Me.cmdBrowserOutputPath)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Location = New System.Drawing.Point(39, 155)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(390, 57)
        Me.GroupBox1.TabIndex = 70
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Outputs"
        '
        'txtOutPath
        '
        Me.txtOutPath.Location = New System.Drawing.Point(94, 19)
        Me.txtOutPath.Name = "txtOutPath"
        Me.txtOutPath.Size = New System.Drawing.Size(245, 20)
        Me.txtOutPath.TabIndex = 12
        '
        'cmdBrowserOutputPath
        '
        Me.cmdBrowserOutputPath.Location = New System.Drawing.Point(345, 19)
        Me.cmdBrowserOutputPath.Name = "cmdBrowserOutputPath"
        Me.cmdBrowserOutputPath.Size = New System.Drawing.Size(26, 20)
        Me.cmdBrowserOutputPath.TabIndex = 10
        Me.cmdBrowserOutputPath.Text = "..."
        Me.cmdBrowserOutputPath.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(19, 22)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(71, 13)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "OutputFolder:"
        '
        'txtProjectName
        '
        Me.txtProjectName.Enabled = False
        Me.txtProjectName.Location = New System.Drawing.Point(170, 16)
        Me.txtProjectName.Name = "txtProjectName"
        Me.txtProjectName.Size = New System.Drawing.Size(365, 20)
        Me.txtProjectName.TabIndex = 16
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(58, 17)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(106, 16)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "Project Name:"
        '
        'chkGenerateContent
        '
        Me.chkGenerateContent.AutoSize = True
        Me.chkGenerateContent.Location = New System.Drawing.Point(460, 138)
        Me.chkGenerateContent.Name = "chkGenerateContent"
        Me.chkGenerateContent.Size = New System.Drawing.Size(110, 17)
        Me.chkGenerateContent.TabIndex = 71
        Me.chkGenerateContent.Text = "Generate Content"
        Me.chkGenerateContent.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(486, 203)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 72
        Me.Button1.Text = "Test"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'BatchControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.chkGenerateContent)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtProjectName)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.chkRangeValues)
        Me.Controls.Add(Me.chkFaceted)
        Me.Controls.Add(Me.chkSmooth)
        Me.Controls.Add(Me.btnGenerate)
        Me.Name = "BatchControl"
        Me.Size = New System.Drawing.Size(605, 244)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnGenerate As Button
    Friend WithEvents chkSmooth As CheckBox
    Friend WithEvents chkFaceted As CheckBox
    Friend WithEvents chkRangeValues As CheckBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents cmdBrowseAvFile As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents txtAvFile As TextBox
    Friend WithEvents cmdBrowseContFile As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents txtContFile As TextBox
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents txtOutPath As TextBox
    Friend WithEvents cmdBrowserOutputPath As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents txtProjectName As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents chkGenerateContent As CheckBox
    Friend WithEvents Button1 As Button
End Class
