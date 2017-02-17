<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Viewer2DControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ImgViewer = New System.Windows.Forms.PictureBox()
        CType(Me.ImgViewer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImgViewer
        '
        Me.ImgViewer.BackColor = System.Drawing.SystemColors.ActiveBorder
        Me.ImgViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ImgViewer.Location = New System.Drawing.Point(0, 0)
        Me.ImgViewer.Name = "ImgViewer"
        Me.ImgViewer.Size = New System.Drawing.Size(251, 293)
        Me.ImgViewer.TabIndex = 2
        Me.ImgViewer.TabStop = False
        '
        'Viewer2DPanel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ImgViewer)
        Me.Name = "Viewer2DPanel"
        Me.Size = New System.Drawing.Size(251, 293)
        CType(Me.ImgViewer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ImgViewer As PictureBox
End Class
