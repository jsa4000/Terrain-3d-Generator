<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PropertiesControl
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
        Me.gridProperties = New System.Windows.Forms.DataGridView()
        Me.colField = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colValue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.gridProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gridProperties
        '
        Me.gridProperties.AllowUserToAddRows = False
        Me.gridProperties.AllowUserToDeleteRows = False
        Me.gridProperties.BackgroundColor = System.Drawing.SystemColors.Window
        Me.gridProperties.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridProperties.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colField, Me.colValue})
        Me.gridProperties.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridProperties.Location = New System.Drawing.Point(0, 0)
        Me.gridProperties.Name = "gridProperties"
        Me.gridProperties.RowHeadersVisible = False
        Me.gridProperties.RowTemplate.Height = 30
        Me.gridProperties.ShowEditingIcon = False
        Me.gridProperties.Size = New System.Drawing.Size(211, 258)
        Me.gridProperties.TabIndex = 1
        '
        'colField
        '
        Me.colField.HeaderText = "Field"
        Me.colField.Name = "colField"
        '
        'colValue
        '
        Me.colValue.HeaderText = "Value"
        Me.colValue.Name = "colValue"
        '
        'PropertiesPanel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.gridProperties)
        Me.Name = "PropertiesPanel"
        Me.Size = New System.Drawing.Size(211, 258)
        CType(Me.gridProperties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents gridProperties As DataGridView
    Friend WithEvents colField As DataGridViewTextBoxColumn
    Friend WithEvents colValue As DataGridViewTextBoxColumn
End Class
