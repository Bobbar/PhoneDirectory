<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PhoneDirectory
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PhoneDirectory))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtExtension = New System.Windows.Forms.TextBox()
        Me.txtExtName = New System.Windows.Forms.TextBox()
        Me.ExtensionGrid = New System.Windows.Forms.DataGridView()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.cmdClear = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.ExtensionGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.cmdClear)
        Me.GroupBox1.Controls.Add(Me.cmdSearch)
        Me.GroupBox1.Controls.Add(Me.txtExtName)
        Me.GroupBox1.Controls.Add(Me.txtExtension)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(13, 13)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox1.Size = New System.Drawing.Size(544, 206)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Search"
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.ExtensionGrid)
        Me.GroupBox2.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(13, 227)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox2.Size = New System.Drawing.Size(544, 335)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Results"
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(76, 40)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 18)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Extension"
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(247, 40)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(160, 18)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Extension Name/User"
        '
        'txtExtension
        '
        Me.txtExtension.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtExtension.Location = New System.Drawing.Point(80, 62)
        Me.txtExtension.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtExtension.Name = "txtExtension"
        Me.txtExtension.Size = New System.Drawing.Size(137, 25)
        Me.txtExtension.TabIndex = 2
        '
        'txtExtName
        '
        Me.txtExtName.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtExtName.Location = New System.Drawing.Point(251, 62)
        Me.txtExtName.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtExtName.Name = "txtExtName"
        Me.txtExtName.Size = New System.Drawing.Size(212, 25)
        Me.txtExtName.TabIndex = 3
        '
        'ExtensionGrid
        '
        Me.ExtensionGrid.AllowUserToAddRows = False
        Me.ExtensionGrid.AllowUserToDeleteRows = False
        Me.ExtensionGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExtensionGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.ExtensionGrid.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ExtensionGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(135, Byte), Integer), CType(CType(188, Byte), Integer), CType(CType(252, Byte), Integer))
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.ExtensionGrid.DefaultCellStyle = DataGridViewCellStyle1
        Me.ExtensionGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.ExtensionGrid.Location = New System.Drawing.Point(8, 24)
        Me.ExtensionGrid.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ExtensionGrid.Name = "ExtensionGrid"
        Me.ExtensionGrid.ReadOnly = True
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ExtensionGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.ExtensionGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.ExtensionGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.ExtensionGrid.Size = New System.Drawing.Size(528, 303)
        Me.ExtensionGrid.TabIndex = 0
        '
        'cmdSearch
        '
        Me.cmdSearch.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.cmdSearch.Location = New System.Drawing.Point(175, 124)
        Me.cmdSearch.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(168, 51)
        Me.cmdSearch.TabIndex = 4
        Me.cmdSearch.Text = "Search"
        Me.cmdSearch.UseVisualStyleBackColor = True
        '
        'cmdClear
        '
        Me.cmdClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdClear.Location = New System.Drawing.Point(436, 158)
        Me.cmdClear.Name = "cmdClear"
        Me.cmdClear.Size = New System.Drawing.Size(99, 37)
        Me.cmdClear.TabIndex = 5
        Me.cmdClear.Text = "Clear"
        Me.cmdClear.UseVisualStyleBackColor = True
        '
        'PhoneDirectory
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 18.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(570, 574)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MinimumSize = New System.Drawing.Size(559, 613)
        Me.Name = "PhoneDirectory"
        Me.Text = "Phone Directory"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.ExtensionGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents txtExtName As TextBox
    Friend WithEvents txtExtension As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents cmdSearch As Button
    Friend WithEvents ExtensionGrid As DataGridView
    Friend WithEvents cmdClear As Button
End Class
